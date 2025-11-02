using System;
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

namespace JalaNota
{
    public partial class LoginNelayan : Window
    {
        public LoginNelayan()
        {
            InitializeComponent();
        }

        private void PilihLoginBtnNelayan_Click(object sender, RoutedEventArgs e)
        {
        }
        private void PilihLoginBtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            LoginAdmin loginAdminWindow = new LoginAdmin();
            loginAdminWindow.Show();
            this.Close();
        }

        private async void LoginNlyn_Click(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string password = tbPassword.Password;

            try
            {
                // ambil data dari Supabase
                var response = await SupabaseClient.Instance.From<Nelayan>()
                    .Where(n => n.UsnNelayan == username && n.PasswordNelayan == password)
                    .Get();

                // ambil data nelayan pertama (jika ada)
                var nelayan = response.Models.FirstOrDefault();

                // cek apakah nelayan ditemukan
                if (nelayan != null) // jika 'nelayan' tidak null, login berhasil
                {
                    MessageBox.Show($"Selamat datang, {nelayan.NamaNelayan}!", "Login Berhasil", MessageBoxButton.OK, MessageBoxImage.Information);

                    // kirim objek 'nelayan' (yang sudah berisi data) ke halaman berikutnya
                    LihatSetoranNelayan riwayatWindow = new LihatSetoranNelayan(nelayan);
                    riwayatWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username atau password salah. Silakan coba lagi.", "Login Gagal", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi error koneksi: {ex.Message}", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void tbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Cek jika PasswordBox kosong
            if (string.IsNullOrEmpty(tbPassword.Password))
            {
                // Jika kosong, tampilkan placeholder
                PasswordPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                // Jika ada isinya, sembunyikan placeholder
                PasswordPlaceholder.Visibility = Visibility.Collapsed;
            }
        }

        private void tbUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Cek jika TextBox username kosong
            if (string.IsNullOrEmpty(tbUsername.Text))
            {
                // Jika kosong, tampilkan placeholder
                UsernamePlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                // Jika ada isinya, sembunyikan placeholder
                UsernamePlaceholder.Visibility = Visibility.Collapsed;
            }
        }
    }
}