using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// Interaction logic for LihatSetoranNelayan.xaml
    /// </summary>
    public partial class LihatSetoranNelayan : Window
    {
        // 1. Buat field privat untuk menyimpan data nelayan yang login
        private Nelayan _nelayanLogin;

        // Model untuk DataGrid
        public ObservableCollection<SetoranNelayanView> DaftarSetoranView { get; set; }

        // 2. Buat constructor baru yang menerima objek Nelayan
        public LihatSetoranNelayan(Nelayan nelayanDariLogin)
        {
            InitializeComponent();

            // 3. Simpan data nelayan yang dikirim ke field privat
            _nelayanLogin = nelayanDariLogin;

            // 4. Gunakan data dari _nelayanLogin
            textBlockWelcome.Text = $"Selamat Datang Kembali, {_nelayanLogin.NamaNelayan}!";

            DaftarSetoranView = new ObservableCollection<SetoranNelayanView>();

            // Pastikan XAML memiliki DataGrid bernama x:Name="dataGridSetoran"
            dataGridSetoranNelayan.ItemsSource = DaftarSetoranView;

            this.Loaded += Window_Loaded;

            // Opsional: Tampilkan nama nelayan di Title Bar
            this.Title = $"Riwayat Setoran - {_nelayanLogin .NamaNelayan}";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MuatDaftarSetoran();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginNelayan loginWindow = new LoginNelayan();
            loginWindow.Show();
            this.Close();
        }

        private void MuatDaftarSetoran()
        {
            // Ambil ID nelayan dari objek hardcoded
            int idNelayanSaatIni = this._nelayanLogin.IDNelayan;

            DaftarSetoranView.Clear();

            // 1. Ambil semua setoran
            var semuaSetoran = Setoran.LihatSemuaSetoran();

            // 2. Filter setoran hanya untuk nelayan yang sedang 'login'
            var setoranFiltered = semuaSetoran
                .Where(s => s.IDNelayan == idNelayanSaatIni)
                .OrderBy(s => s.IDSetoran);

            if (!setoranFiltered.Any())
            {
                MessageBox.Show($"Nelayan {_nelayanLogin.NamaNelayan} belum memiliki riwayat setoran.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // 3. Masukkan data ke View Model
            foreach (var s in setoranFiltered)
            {
                // Ambil data pendukung (Nama Ikan)
                var ikan = JenisIkan.GetJenisIkanById(s.IDIkan);

                DaftarSetoranView.Add(new SetoranNelayanView
                {
                    IDSetoran = s.IDSetoran,
                    Tanggal = s.WaktuSetor.ToShortDateString(),
                    NamaIkan = ikan?.NamaIkan ?? "N/A",
                    Berat = s.BeratKg,
                    Total = s.HargaTotal
                });
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