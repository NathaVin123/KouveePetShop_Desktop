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
        public Pegawai()
        {
            InitializeComponent();

            conn = new MySqlConnection();
            conn.ConnectionString = "SERVER=localhost;DATABASE=petshop;UID=root;PASSWORD=;Convert Zero Datetime=true";
            BindGrid();
            BindGridPegawai();
        }

        private void BindGrid()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT nip AS 'NIP', nama_pegawai AS 'Nama Pegawai', alamat_Pegawai AS 'Alamat Pegawai', tglLahir_pegawai AS 'Tanggal Lahir', noTelp_pegawai AS 'Nomor Telepon', stat AS 'Stat', password AS 'Password', gambar as 'Gambar', updateLog_by AS 'Update Log By' FROM pegawais";
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

        private void BindGridPegawai()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT NIP AS 'NIP', nama_pegawai AS 'Nama Pegawai' FROM pegawais";
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            adapter.Fill(dt);
            nipDT.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                LabelCount.Visibility = System.Windows.Visibility.Hidden;
                nipDT.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                LabelCount.Visibility = System.Windows.Visibility.Visible;
                nipDT.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void ClearAll()
        {
            nipTxt.Text = "";
            namapegawaiTxt.Text = "";
            alamatpegawaiTxt.Text = "";
            tanggallahirTxt.Text = "";
            notelpTxt.Text = "";
            statTxt.Text = "";
            passwordTxt.Text = "";
            updatelogbyTxt.Text = "";
            tambahBtn.Content = "Tambah";
            nipTxt.IsEnabled = true;
        }


        private void MenuUtama_Click(object sender, RoutedEventArgs e)
        {
            var Menu = new Menu.Menu_KouveePetShop();
            Menu.Show();
            this.Close();
        }

        private void Tambah_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;

            byte[] gambarBT = null;
            FileStream fs = new FileStream(this.GambarPath.Text, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            gambarBT = br.ReadBytes((int)fs.Length);

            string nip = nipTxt.Text;
            string nama_pegawai = namapegawaiTxt.Text;
            string alamat_pegawai = alamatpegawaiTxt.Text;
            string tglLahir_pegawai = tanggallahirTxt.Text;
            string noTelp_pegawai = notelpTxt.Text;
            string stat = statTxt.Text;
            string password = passwordTxt.Text;
            string updateLog_by = updatelogbyTxt.Text;

            if (nipTxt.Text != "" && namapegawaiTxt.Text != "" && alamatpegawaiTxt.Text != "" && tanggallahirTxt.Text != "" && notelpTxt.Text != "" && statTxt.Text != "" && passwordTxt.Text != "" && updatelogbyTxt.Text != "")
            {
                if (nipTxt.IsEnabled == true)
                {
                    cmd.CommandText = "INSERT INTO pegawais(nip,nama_pegawai,alamat_pegawai,tglLahir_pegawai,noTelp_pegawai,stat,password,gambar,updateLog_by) VALUES (@nip,@nama_pegawai,@alamat_pegawai,@tglLahir_pegawai,@noTelp_pegawai,@stat,@password,@gambar,@updateLog_by)";
                    cmd.Parameters.AddWithValue("@nip", nip);
                    cmd.Parameters.AddWithValue("@nama_pegawai", nama_pegawai);
                    cmd.Parameters.AddWithValue("@alamat_pegawai", alamat_pegawai);
                    cmd.Parameters.AddWithValue("@tglLahir_pegawai", tglLahir_pegawai);
                    cmd.Parameters.AddWithValue("@noTelp_pegawai", noTelp_pegawai);
                    cmd.Parameters.AddWithValue("@stat", stat);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@gambar", gambarBT);
                    cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Data Pegawai berhasil ditambahkan");
                    ClearAll();
                }
                else
                {
                    cmd.CommandText = "UPDATE pegawais set nip = @nip, nama_pegawai = @nama_pegawai, tglLahir_pegawai = @tglLahir_pegawai, noTelp_pegawai = @noTelp_pegawai, stat = @stat, password = @password, gambar = @gambar, updateLog_By = @updateLog_by WHERE nip = @nip";
                    cmd.Parameters.AddWithValue("@nip", nip);
                    cmd.Parameters.AddWithValue("@nama_pegawai", nama_pegawai);
                    cmd.Parameters.AddWithValue("@alamat_pegawai", alamat_pegawai);
                    cmd.Parameters.AddWithValue("@tglLahir_pegawai", tglLahir_pegawai);
                    cmd.Parameters.AddWithValue("@noTelp_pegawai", noTelp_pegawai);
                    cmd.Parameters.AddWithValue("@stat", stat);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@gambar", gambarBT);
                    cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Data Pegawai berhasil diubah");
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("ID Produk harap diisi");
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
                tanggallahirTxt.Text = row["Tanggal Lahir"].ToString();
                notelpTxt.Text = row["Nomor Telepon"].ToString();
                statTxt.Text = row["Stat"].ToString();
                passwordTxt.Text = row["Password"].ToString();
                updatelogbyTxt.Text = row["Update Log By"].ToString();
                nipTxt.IsEnabled = false;
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
            if (pegawaiDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)pegawaiDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM pegawais where nip =" + row["NIP"].ToString();
                cmd.ExecuteNonQuery();
                BindGrid();
                MessageBox.Show("Data pegawai berhasil di hapus");
                ClearAll();

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

            cmd.Parameters.AddWithValue("@nama_pegawai", nama_pegawai);
            cmd.CommandText = "SELECT nip AS 'NIP', nama_pegawai AS 'Nama Pegawai', alamat_Pegawai AS 'Alamat Pegawai', tglLahir_pegawai AS 'Tanggal Lahir', noTelp_pegawai AS 'Nomor Telepon', stat AS 'Stat', password AS 'Password', gambar as 'Gambar', updateLog_by AS 'Update Log By' FROM pegawais WHERE nip";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            adapter.Fill(dt);
            pegawaiDT.ItemsSource = dt.AsDataView();
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
    }
}