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
    /// <summary>
    /// Interaction logic for LoginAdmin.xaml
    /// </summary>
    public partial class LoginAdmin : Window
    {
        public LoginAdmin()
        {
            InitializeComponent();
        }

        private void PilihLoginBtnNelayan_Click(object sender, RoutedEventArgs e)
        {
            LoginNelayan loginNelayanWindow = new LoginNelayan();
            loginNelayanWindow.Show();
            this.Close();
        }
        private void PilihLoginBtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            // Tetap di halaman ini
        }

        private void tbPassword_PasswordChangedA(object sender, RoutedEventArgs e)
        {
            // Cek jika PasswordBox kosong
            if (string.IsNullOrEmpty(tbPasswordA.Password))
            {
                // Jika kosong, tampilkan placeholder
                PasswordPlaceholderA.Visibility = Visibility.Visible;
            }
            else
            {
                // Jika ada isinya, sembunyikan placeholder
                PasswordPlaceholderA.Visibility = Visibility.Collapsed;
            }
        }
        private void tbUsername_TextChangedA(object sender, TextChangedEventArgs e)
        {
            // Cek jika TextBox username kosong
            if (string.IsNullOrEmpty(tbUsernameA.Text))
            {
                // Jika kosong, tampilkan placeholder
                UsernamePlaceholderA.Visibility = Visibility.Visible;
            }
            else
            {
                // Jika ada isinya, sembunyikan placeholder
                UsernamePlaceholderA.Visibility = Visibility.Collapsed;
            }
        }

        private void LoginAdm_Click(object sender, RoutedEventArgs e)
        {
            string username = tbUsernameA.Text;
            string password = tbPasswordA.Password;
            Admin admin = new Admin();
            bool loginSuccess = admin.Login(username, password);
            if (loginSuccess)
            {
                MessageBox.Show($"Selamat datang, {admin.NamaAdmin}!", "Login Berhasil", MessageBoxButton.OK, MessageBoxImage.Information);
                ManageSetoran manageSetoranWindow = new ManageSetoran(admin);
                manageSetoranWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Username atau password salah. Silakan coba lagi.", "Login Gagal", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
