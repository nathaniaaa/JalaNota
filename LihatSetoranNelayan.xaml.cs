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
        // Menyimpan data nelayan yang sedang 'login' secara hardcoded
        private Nelayan currentNelayan = new Nelayan
        {
            IDNelayan = 1,
            NamaNelayan = "Nelayan Satu" // Data ini diambil dari daftar statis di Nelayan.cs
        };

        // Model untuk DataGrid
        public ObservableCollection<SetoranNelayanView> DaftarSetoranView { get; set; }

        public LihatSetoranNelayan()
        {
            InitializeComponent();

            textBlockWelcome.Text = $"Selamat Datang Kembali, {currentNelayan.NamaNelayan}!";

            DaftarSetoranView = new ObservableCollection<SetoranNelayanView>();

            // Pastikan XAML memiliki DataGrid bernama x:Name="dataGridSetoran"
            dataGridSetoranNelayan.ItemsSource = DaftarSetoranView;

            this.Loaded += Window_Loaded;

            // Opsional: Tampilkan nama nelayan di Title Bar
            this.Title = $"Riwayat Setoran - {currentNelayan.NamaNelayan}";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MuatDaftarSetoran();
        }

        private void MuatDaftarSetoran()
        {
            // Ambil ID nelayan dari objek hardcoded
            int idNelayanSaatIni = this.currentNelayan.IDNelayan;

            DaftarSetoranView.Clear();

            // 1. Ambil semua setoran
            var semuaSetoran = Setoran.LihatSemuaSetoran();

            // 2. Filter setoran hanya untuk nelayan yang sedang 'login'
            var setoranFiltered = semuaSetoran
                .Where(s => s.IDNelayan == idNelayanSaatIni)
                .OrderBy(s => s.IDSetoran);

            if (!setoranFiltered.Any())
            {
                MessageBox.Show($"Nelayan {currentNelayan.NamaNelayan} belum memiliki riwayat setoran.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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