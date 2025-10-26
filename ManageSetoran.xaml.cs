using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace JalaNota
{
    public partial class ManageSetoran : Window
    {
        // Model untuk DataGrid
        public ObservableCollection<SetoranView> DaftarSetoranView { get; set; }

        // 1. Buat field privat untuk menyimpan data admin yang login
        private Admin _adminLogin;

        // 2. Buat constructor baru yang menerima objek Admin
        public ManageSetoran(Admin adminDariLogin)
        {
            InitializeComponent();

            // 3. Simpan data admin yang dikirim ke field privat
            _adminLogin = adminDariLogin;

            DaftarSetoranView = new ObservableCollection<SetoranView>();
            dataGridSetoran.ItemsSource = DaftarSetoranView;

            this.Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 1. Isi ComboBox Nelayan dan Ikan
            IsiComboBoxNelayan();
            IsiComboBoxIkan();

            // 2. Muat dan Tampilkan Data di DataGrid (Mengambil data dari Setoran.cs)
            MuatDaftarSetoran();

            // Set tanggal hari ini secara default
            dpTanggalSetoran.SelectedDate = DateTime.Now.Date;
        }

        private void IsiComboBoxNelayan()
        {
            // Data Nelayan diambil dari metode statis di Nelayan.cs
            var daftarNelayan = Nelayan.LihatSemuaNelayan();
            cmbNamaNelayan.ItemsSource = daftarNelayan;
            // XAML sudah diatur dengan DisplayMemberPath="NamaNelayan" dan SelectedValuePath="IDNelayan"
        }

        private void IsiComboBoxIkan()
        {
            // Data Ikan dari JenisIkan.cs
            var daftarIkan = JenisIkan.LihatSemuaJenisIkan();
            cmbNamaIkan.ItemsSource = daftarIkan;
        }

        private void MuatDaftarSetoran()
        {
            DaftarSetoranView.Clear();
            var semuaSetoran = Setoran.LihatSemuaSetoran().OrderBy(s => s.IDSetoran);

            foreach (var s in semuaSetoran)
            {
                // Ambil data pendukung
                var ikan = JenisIkan.GetJenisIkanById(s.IDIkan);

                // Cari Nama Nelayan menggunakan metode GetNelayanById()
                var nelayan = Nelayan.GetNelayanById(s.IDNelayan);
                string namaNelayan = nelayan?.NamaNelayan ?? $"ID Nelayan {s.IDNelayan}";

                DaftarSetoranView.Add(new SetoranView
                {
                    IDSetoran = s.IDSetoran,
                    // Mengambil hanya tanggal untuk tampilan DataGrid
                    Tanggal = s.WaktuSetor.ToShortDateString(),
                    IDNelayan = s.IDNelayan,
                    IDIkan = s.IDIkan,
                    NamaNelayan = namaNelayan,
                    NamaIkan = ikan?.NamaIkan ?? "N/A",
                    Berat = s.BeratKg,
                    Total = s.HargaTotal
                });
            }
        }

        // --- CRUD Buttons ---

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validasi Input
            if (cmbNamaNelayan.SelectedValue == null || cmbNamaIkan.SelectedValue == null ||
                !dpTanggalSetoran.SelectedDate.HasValue || string.IsNullOrWhiteSpace(txtBerat.Text))
            {
                MessageBox.Show("Semua kolom harus diisi dengan benar.", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(txtBerat.Text, out double berat) || berat <= 0)
            {
                MessageBox.Show("Berat (kg) harus berupa angka positif yang valid.", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Ambil Nilai
            int idNelayan = (int)cmbNamaNelayan.SelectedValue;
            int idIkan = (int)cmbNamaIkan.SelectedValue;
            DateTime tanggalSetoran = dpTanggalSetoran.SelectedDate.Value.Date;

            // 3. Catat Setoran
            _adminLogin.CatatSetoran(idNelayan, idIkan, berat, tanggalSetoran);

            // 4. Perbarui Tampilan
            MuatDaftarSetoran();
            MessageBox.Show("Setoran baru berhasil dicatat!", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);

            // 5. Reset Form
            ClearForm();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validasi ID Setoran
            if (!int.TryParse(txtIDSetoran.Text, out int idSetoran) || idSetoran <= 0)
            {
                MessageBox.Show("Pilih data setoran dari tabel untuk mengedit.", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Validasi Input Data
            if (cmbNamaNelayan.SelectedValue == null || cmbNamaIkan.SelectedValue == null ||
                !dpTanggalSetoran.SelectedDate.HasValue || string.IsNullOrWhiteSpace(txtBerat.Text))
            {
                MessageBox.Show("Semua kolom harus diisi dengan benar.", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(txtBerat.Text, out double berat) || berat <= 0)
            {
                MessageBox.Show("Berat (kg) harus berupa angka positif yang valid.", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 3. Ambil Nilai
            int idNelayan = (int)cmbNamaNelayan.SelectedValue;
            int idIkan = (int)cmbNamaIkan.SelectedValue;
            DateTime tanggalSetoran = dpTanggalSetoran.SelectedDate.Value.Date;

            // 4. Edit Setoran
            bool sukses = _adminLogin.EditSetoran(idSetoran, idNelayan, idIkan, berat, tanggalSetoran);

            // 5. Perbarui Tampilan
            if (sukses)
            {
                MuatDaftarSetoran();
                MessageBox.Show($"Setoran ID {idSetoran} berhasil diperbarui!", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
            }
            else
            {
                MessageBox.Show("Gagal memperbarui setoran. ID Setoran mungkin tidak valid atau ikan tidak ditemukan.", "Gagal", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Logika DELETE (masih placeholder karena model statis tidak mendukung)
            if (dataGridSetoran.SelectedItem is SetoranView selected)
            {
                MessageBoxResult result = MessageBox.Show($"Yakin ingin menghapus Setoran ID {selected.IDSetoran}? (Fitur ini belum diimplementasikan)",
                    "Konfirmasi Hapus", MessageBoxButton.YesNo, MessageBoxImage.Question);

            }
            else
            {
                MessageBox.Show("Pilih data setoran yang ingin dihapus dari tabel.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void dataGridSetoran_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridSetoran.SelectedItem is SetoranView selected)
            {
                // Mengisi form dari data yang dipilih
                txtIDSetoran.Text = selected.IDSetoran.ToString();
                txtBerat.Text = selected.Berat.ToString();

                // Mengisi DatePicker
                if (DateTime.TryParse(selected.Tanggal, out DateTime parsedDate))
                {
                    dpTanggalSetoran.SelectedDate = parsedDate;
                }

                // Memilih item di ComboBox berdasarkan ID
                cmbNamaNelayan.SelectedValue = selected.IDNelayan;
                cmbNamaIkan.SelectedValue = selected.IDIkan;
            }
            else
            {
                ClearForm();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtIDSetoran.Clear();
            txtBerat.Clear();
            dpTanggalSetoran.SelectedDate = DateTime.Now.Date;
            cmbNamaNelayan.SelectedIndex = -1;
            cmbNamaIkan.SelectedIndex = -1;
            dataGridSetoran.UnselectAll();
        }

        // --- Navbar Clicks ---
        private void ManageSetoran_Click(object sender, RoutedEventArgs e) 
        { 
            // Tetap di halaman ini 
        }
        private void ManageNelayan_Click(object sender, RoutedEventArgs e) 
        {
            ManageNelayan manageNelayanWindow = new ManageNelayan(_adminLogin);
            manageNelayanWindow.Show();
            this.Close();
        }
        private void ManageIkan_Click(object sender, RoutedEventArgs e)
        {
            ManageDaftarIkan manageDaftarIkanWindow = new ManageDaftarIkan(_adminLogin);
            manageDaftarIkanWindow.Show();
            this.Close();
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginNelayan loginWindow = new LoginNelayan();
            loginWindow.Show();
            this.Close();
        }
    }

    // Kelas Model yang digunakan untuk DataGrid (View Model)
    public class SetoranView
    {
        public int IDSetoran { get; set; }
        public string Tanggal { get; set; } // Untuk tampilan di DataGrid (String)
        public DateTime WaktuSetor { get; set; } 
        public int IDNelayan { get; set; }
        public int IDIkan { get; set; }
        public string NamaNelayan { get; set; }
        public string NamaIkan { get; set; }
        public double Berat { get; set; }
        public double Total { get; set; }
    }
}