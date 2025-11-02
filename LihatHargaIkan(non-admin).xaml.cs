using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Threading.Tasks;

namespace JalaNota
{
    public partial class LihatHargaIkan_non_admin_ : Window
    {
        public LihatHargaIkan_non_admin_()
        {
            InitializeComponent();
            LoadDataFromSupabase();
        }

        private async void LoadDataFromSupabase()
        {
            try
            {
                var client = SupabaseClient.Instance;

                // query untuk ambil data dari tabel 'JenisIkan'
                var response = await client.From<JenisIkan>()
                                            .Select("*")
                                            .Order("NamaIkan", Postgrest.Constants.Ordering.Ascending) // Urutkan A-Z
                                            .Get();

                // memasukkan hasilnya ke DataGrid
                if (response.Models != null)
                {
                    dataGridIkan.ItemsSource = response.Models;
                }
                else
                {
                    MessageBox.Show("Gagal mengambil data ikan dari database.");
                }
            }
            catch (Exception ex)
            {
                // pesan jika koneksi gagal atau ada error
                MessageBox.Show($"Terjadi error saat memuat data: {ex.Message}", "Error Koneksi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginNelayan loginNelayanWindow = new LoginNelayan();
            loginNelayanWindow.Show();
            this.Close();
        }

        private void dataGridIkan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}