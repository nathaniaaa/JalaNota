using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
// Tambahkan using ini
using System.Threading.Tasks;

namespace JalaNota
{
    public partial class LihatSetoranNelayan : Window
    {
        private Nelayan _nelayanLogin;
        public ObservableCollection<SetoranNelayanView> DaftarSetoranView { get; set; }

        public LihatSetoranNelayan(Nelayan nelayanDariLogin)
        {
            InitializeComponent();
            _nelayanLogin = nelayanDariLogin;

            textBlockWelcome.Text = $"Selamat Datang Kembali, {_nelayanLogin.NamaNelayan}!";
            DaftarSetoranView = new ObservableCollection<SetoranNelayanView>();
            dataGridSetoranNelayan.ItemsSource = DaftarSetoranView;
            this.Title = $"Riwayat Setoran - {_nelayanLogin.NamaNelayan}";
            this.Loaded += Window_Loaded_Async;
        }

        private async void Window_Loaded_Async(object sender, RoutedEventArgs e)
        {
            await MuatDaftarSetoran();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginNelayan loginWindow = new LoginNelayan();
            loginWindow.Show();
            this.Close();
        }

        private async Task MuatDaftarSetoran()
        {
            DaftarSetoranView.Clear();

            try
            {
                var client = SupabaseClient.Instance;
                int idNelayanSaatIni = this._nelayanLogin.IDNelayan;

                // ambil data ikan
                var ikanResponse = await client.From<JenisIkan>().Select("IDIkan, NamaIkan").Get();

                // ubah jadi kamus (dictionary) agar mudah dicari
                var kamusIkan = ikanResponse.Models.ToDictionary(
                    ikan => ikan.IDIkan,         // Key: IDIkan
                    ikan => ikan.NamaIkan        // Value: NamaIkan
                );

                // ambil data setoran hanya untuk nelayan ini
                var setoranResponse = await client.From<Setoran>()
                    .Where(s => s.IDNelayan == idNelayanSaatIni)
                    .Order("WaktuSetor", Postgrest.Constants.Ordering.Descending) // urutkan terbaru
                    .Get();

                if (!setoranResponse.Models.Any())
                {
                    MessageBox.Show($"Anda belum memiliki riwayat setoran.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // gabungkan data
                foreach (var s in setoranResponse.Models)
                {
                    // Cari nama ikan dari kamus
                    kamusIkan.TryGetValue(s.IDIkan, out string namaIkan);

                    DaftarSetoranView.Add(new SetoranNelayanView
                    {
                        IDSetoran = s.IDSetoran,
                        Tanggal = s.WaktuSetor.ToShortDateString(),
                        NamaIkan = namaIkan ?? "Ikan Dihapus", // jika ikan tidak ada di kamus
                        Berat = s.BeratKg,
                        Total = s.HargaTotal
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat riwayat setoran: {ex.Message}", "Error Koneksi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Kelas Model (View Model) untuk DataGrid di sisi Nelayan
    public class SetoranNelayanView
    {
        public int IDSetoran { get; set; }
        public string Tanggal { get; set; }
        public string NamaIkan { get; set; }
        public double Berat { get; set; }
        public double Total { get; set; }
    }
}