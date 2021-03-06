﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using Microsoft.Win32;

namespace KouveePetShop_Desktop.Pegawai
{
    /// <summary>
    /// Interaction logic for Pegawai.xaml
    /// </summary>
    public partial class Pegawai : Window
    {
        MySqlConnection conn;
        DataTable dt;

        DateTime tanggal = DateTime.Now;
        public Pegawai()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = "SERVER=localhost;DATABASE=petshopd;UID=root;PASSWORD=;Allow Zero Datetime=True";
                BindGrid();
                BindGridLog();
                FillComboBoxNIP();
                FillComboBoxRole();
            }
            catch
            {
                MessageBox.Show("Tidak ada database...");
            }
            
        }

        private void BindGrid()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            try
            {
                cmd.CommandText = "SELECT NIP AS 'NIP', nama_pegawai AS 'Nama Pegawai', alamat_Pegawai AS 'Alamat Pegawai', tglLahir_pegawai AS 'Tanggal Lahir', noTelp_pegawai AS 'Nomor Telepon', stat AS 'Role', gambar AS 'Gambar', updateLog_by AS 'Diubah Oleh' FROM pegawais WHERE aktif LIKE '1'";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                pegawaiDT.ItemsSource = dt.AsDataView();

                if (dt.Rows.Count > 0)
                {
                    LabelCount.Visibility = System.Windows.Visibility.Hidden;
                    pegawaiDT.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    LabelCount.Visibility = System.Windows.Visibility.Visible;
                    pegawaiDT.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data pegawai...");
            }
            
        }

        private void BindGridLog()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT nama_pegawai AS 'Nama Pegawai', createLog_at AS 'Di Buat', updateLog_at AS 'Di Ubah', deleteLog_at AS 'Di Hapus' FROM pegawais";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                logDT.ItemsSource = dt.AsDataView();

                if (dt.Rows.Count > 0)
                {
                    LabelCount.Visibility = System.Windows.Visibility.Hidden;
                    logDT.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    LabelCount.Visibility = System.Windows.Visibility.Visible;
                    logDT.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data pegawai");
            }
        }

        private void ClearAll()
        {
            nipTxt.Text = "";
            namapegawaiTxt.Text = "";
            alamatpegawaiTxt.Text = "";
            tanggallahirDp.Text = "";
            notelpTxt.Text = "";
            roleCb.Text = "";
            passwordTxt.Clear();
            updatelogbyCb.Text = "";
            tambahBtn.Content = "Tambah";
            nipTxt.IsEnabled = true;
            passwordTxt.IsEnabled = true;
        }


        private void MenuUtama_Click(object sender, RoutedEventArgs e)
        {
            var Menu = new Menu.Menu_KouveePetShop();
            Menu.Show();
            this.Close();
        }

        public void FillComboBoxNIP()
        {
            string query = "SELECT NIP FROM petshopd.pegawais;";

            MySqlCommand mySqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader mySqlDataReader;

            try
            {
                mySqlDataReader = mySqlCommand.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    string NIP = mySqlDataReader.GetString("NIP");
                    updatelogbyCb.Items.Add(NIP);
                }
                mySqlDataReader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        public void FillComboBoxRole()
        {
            string query = "SELECT stat FROM petshopd.pegawais;";

            MySqlCommand mySqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader mySqlDataReader;

            try
            {
                mySqlDataReader = mySqlCommand.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    string stat = mySqlDataReader.GetString("stat");
                    roleCb.Items.Add(stat);
                }
                mySqlDataReader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        public int id_Pegawai_ai = 10;
        private void Tambah_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            try
            {
                byte[] gambarBT = null;
                try
                {
                    FileStream fs = new FileStream(this.GambarPath.Text, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    gambarBT = br.ReadBytes((int)fs.Length);
                }
                catch
                {
                    MessageBox.Show("Harap masukan gambar terlebih dahulu");
                }

                try
                {
                    id_Pegawai_ai++;
                    string nip_ai_2 = id_Pegawai_ai.ToString("PEG-0");
                    string nama_pegawai = namapegawaiTxt.Text;
                    string alamat_pegawai = alamatpegawaiTxt.Text;
                    string tglLahir_pegawai = tanggallahirDp.SelectedDate.Value.ToString("yyyy-MM-dd");
                    string noTelp_pegawai = notelpTxt.Text;
                    string stat = roleCb.Text;
                    //string password = passwordTxt.Text;
                    string updateLog_by = updatelogbyCb.Text;
                    string nip = nipTxt.Text;
                    string updateLog_at = tanggal.ToString("yyyy-MM-dd H:mm:ss");

                    if (namapegawaiTxt.Text != "" && alamatpegawaiTxt.Text != "" && tanggallahirDp.Text != "" && notelpTxt.Text != "" || string.IsNullOrEmpty(roleCb.Text) || string.IsNullOrEmpty(updatelogbyCb.Text))
                    {
                        if (nipTxt.IsEnabled == true)
                        {
                            try
                            {
                                cmd.CommandText = "INSERT INTO pegawais(nip,nama_pegawai,alamat_pegawai,tglLahir_pegawai,noTelp_pegawai,stat,password,gambar,updateLog_by) VALUES (@nip,@nama_pegawai,@alamat_pegawai,@tglLahir_pegawai,@noTelp_pegawai,@stat,@password,@gambar,@updateLog_by)";
                                cmd.Parameters.AddWithValue("@nip", nip_ai_2);
                                cmd.Parameters.AddWithValue("@nama_pegawai", nama_pegawai);
                                cmd.Parameters.AddWithValue("@alamat_pegawai", alamat_pegawai);
                                cmd.Parameters.AddWithValue("@tglLahir_pegawai", tglLahir_pegawai);
                                cmd.Parameters.AddWithValue("@noTelp_pegawai", noTelp_pegawai);
                                cmd.Parameters.AddWithValue("@stat", stat);
                                cmd.Parameters.AddWithValue("@password", passwordTxt);
                                cmd.Parameters.AddWithValue("@gambar", gambarBT);
                                cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                                cmd.ExecuteNonQuery();
                                BindGrid();
                                MessageBox.Show("Data Pegawai berhasil ditambahkan");
                            }
                            catch
                            {
                                MessageBox.Show("Terjadi kesalahan dalam menambahkan data pegawai");
                            }
                            ClearAll();
                        }
                        else
                        {
                            try
                            {
                                cmd.CommandText = "UPDATE pegawais set nip = @nip, nama_pegawai = @nama_pegawai, tglLahir_pegawai = @tglLahir_pegawai, noTelp_pegawai = @noTelp_pegawai, stat = @stat, gambar = @gambar, updateLog_By = @updateLog_by WHERE nip = @nip";
                                cmd.Parameters.AddWithValue("@nip", nip);
                                cmd.Parameters.AddWithValue("@nama_pegawai", nama_pegawai);
                                cmd.Parameters.AddWithValue("@alamat_pegawai", alamat_pegawai);
                                cmd.Parameters.AddWithValue("@tglLahir_pegawai", tglLahir_pegawai);
                                cmd.Parameters.AddWithValue("@noTelp_pegawai", noTelp_pegawai);
                                cmd.Parameters.AddWithValue("@stat", stat);
                                //cmd.Parameters.AddWithValue("@password", password);
                                cmd.Parameters.AddWithValue("@gambar", gambarBT);
                                cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                                cmd.Parameters.AddWithValue("@updateLog_at", updateLog_at);
                                cmd.ExecuteNonQuery();
                                BindGrid();
                                MessageBox.Show("Data Pegawai berhasil diubah");
                            }
                            catch
                            {
                                MessageBox.Show("Terjadi kesalahan dalam mengubah data pegawai");
                            }
                            ClearAll();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Data Pegawai mohon dilengkapi");
                    }
                }
                catch
                {
                    MessageBox.Show("Data Pegawai mohon diisi");
                }
            }
            catch
            {
                MessageBox.Show("Mohon data Pegawai harap diisi");
            }
        }

        private void Ubah_Click(object sender, RoutedEventArgs e)
        {
            if (pegawaiDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)pegawaiDT.SelectedItems[0];
                nipTxt.Text = row["NIP"].ToString();
                namapegawaiTxt.Text = row["Nama Pegawai"].ToString();
                alamatpegawaiTxt.Text = row["Alamat Pegawai"].ToString();
                tanggallahirDp.Text = row["Tanggal Lahir"].ToString();
                notelpTxt.Text = row["Nomor Telepon"].ToString();
                roleCb.Text = row["Role"].ToString();
                //passwordTxt.Text = row["Password"].ToString();
                updatelogbyCb.Text = row["Diubah Oleh"].ToString();
                nipTxt.IsEnabled = false;
                passwordTxt.IsEnabled = false;
                tambahBtn.Content = "Update";
            }
            else
            {
                MessageBox.Show("Tolong pilih data pegawai yang mau diubah");
            }
        }

        private void Batal_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void Hapus_Click(object sender, RoutedEventArgs e)
        {
            int aktif = '0';
            string message = "Apakah anda ingin menghapus data ini ?";
            string caption = "Warning";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;

            if (pegawaiDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)pegawaiDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE pegawais set aktif = @aktif where NIP =" + row["NIP"].ToString();
                        cmd.Parameters.AddWithValue("@aktif", aktif);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        BindGridLog();
                        MessageBox.Show("Data pegawai berhasil di hapus");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam menghapus data pegawai");
                    }
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("Tolong pilih data pegawai yang ingin dihapus");
            }
        }

        private void Cari_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;

            string nama_pegawai = cariTxt.Text;
            try
            {
                cmd.Parameters.AddWithValue("@nama_pegawai", nama_pegawai);
                cmd.CommandText = "SELECT nip AS 'NIP', nama_pegawai AS 'Nama Pegawai', alamat_Pegawai AS 'Alamat Pegawai', tglLahir_pegawai AS 'Tanggal Lahir', noTelp_pegawai AS 'Nomor Telepon', stat AS 'Stat', password AS 'Password', gambar as 'Gambar', updateLog_by AS 'Diubah Oleh' FROM pegawais WHERE nama_pegawai = @nama_pegawai";

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                pegawaiDT.ItemsSource = dt.AsDataView();
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam mencari data pegawai");
            }
            
        }

        private void GambarBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Multiselect = false;
            opf.Title = "Silahkan pilih gambar";
            opf.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (opf.ShowDialog() == true)
            {
                UploadImage.Source = new BitmapImage(new Uri(opf.FileName));
                string gambarpath = opf.FileName.ToString();
                GambarPath.Text = gambarpath;
            }
        }

        private void CariTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindGrid();
        }
    }
}
