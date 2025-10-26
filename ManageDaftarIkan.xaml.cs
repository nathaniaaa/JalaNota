using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace JalaNota
{
    // Menggunakan JenisIkan langsung sebagai Model View karena strukturnya cocok
    public partial class ManageDaftarIkan : Window
    {
        // ObservableCollection untuk menampilkan dan merefresh DataGrid
        public ObservableCollection<JenisIkan> DaftarIkanView { get; set; }

        public ManageDaftarIkan()
        {
            InitializeComponent();

            DaftarIkanView = new ObservableCollection<JenisIkan>();
            MuatDaftarIkan();
            dataGridIkan.ItemsSource = DaftarIkanView;
        }

        private void MuatDaftarIkan()
        {
            DaftarIkanView.Clear();
            List<JenisIkan> dataDariModel = JenisIkan.LihatSemuaJenisIkan();

            foreach (var ikan in dataDariModel)
            {
                DaftarIkanView.Add(ikan);
            }
        }

        // --- CRUD Buttons ---

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            // Validasi: ID Ikan tidak perlu diisi karena dibuat otomatis di JenisIkan.TambahIkanBaru
            if (string.IsNullOrWhiteSpace(txtNamaIkan.Text) || string.IsNullOrWhiteSpace(txtHarga.Text))
            {
                MessageBox.Show("Nama Ikan dan Harga harus diisi!", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(txtHarga.Text, out double harga))
            {
                MessageBox.Show("Harga harus berupa angka!", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Memanggil metode statis dari JenisIkan.cs
            JenisIkan.TambahIkanBaru(txtNamaIkan.Text, harga);

            // Muat ulang data untuk merefresh DataGrid
            MuatDaftarIkan();
            ClearForm();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridIkan.SelectedItem is JenisIkan selected)
            {
                if (!double.TryParse(txtHarga.Text, out double hargaBaru))
                {
                    MessageBox.Show("Harga harus berupa angka!", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Cek apakah ada perubahan
                if (selected.NamaIkan == txtNamaIkan.Text && selected.HargaPerKg == hargaBaru)
                {
                    MessageBox.Show("Tidak ada perubahan data yang dilakukan.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Hanya HargaPerKg yang bisa diubah via method statis yang ada di JenisIkan.cs
                bool success = JenisIkan.UbahHargaIkan(selected.IDIkan, hargaBaru);

                if (success)
                {
                    if (selected.NamaIkan != txtNamaIkan.Text)
                    {
                        MessageBox.Show("Nama Ikan tidak dapat diubah karena model JenisIkan.cs tidak menyediakan method untuk itu.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    // Muat ulang data untuk memastikan perubahan tercermin
                    MuatDaftarIkan();
                    MessageBox.Show("Harga ikan berhasil diperbarui.", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Gagal menemukan ID Ikan untuk diubah.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Pilih data ikan yang ingin diedit dari tabel.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridIkan.SelectedItem is JenisIkan selected)
            {
                MessageBoxResult result = MessageBox.Show($"Yakin ingin menghapus {selected.NamaIkan}? (Aksi ini tidak didukung oleh model JenisIkan.cs)",
                    "Konfirmasi Hapus", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // TODO: Perlu menambahkan metode statis HapusIkan(int IDIkan) di JenisIkan.cs
                    MessageBox.Show("Penghapusan gagal karena model JenisIkan.cs tidak menyediakan method untuk menghapus data statis.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ClearForm();
                }
            }
            else
            {
                MessageBox.Show("Pilih data ikan yang ingin dihapus dari tabel.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void dataGridIkan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridIkan.SelectedItem is JenisIkan selected)
            {
                // IDIkan dan HargaPerKg adalah properti di JenisIkan.cs
                txtIDIkan.Text = selected.IDIkan.ToString();
                txtNamaIkan.Text = selected.NamaIkan;
                txtHarga.Text = selected.HargaPerKg.ToString();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtIDIkan.Clear();
            txtNamaIkan.Clear();
            txtHarga.Clear();
            dataGridIkan.UnselectAll();
        }

        // --- Navbar Clicks ---

        private void ManageSetoran_Click(object sender, RoutedEventArgs e)
        {
            ManageSetoran manageSetoranWindow = new ManageSetoran();
            manageSetoranWindow.Show();
            this.Close();
        }

        private void ManageNelayan_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Navigasi ke halaman 'Manage Nelayan'");
        }

        private void ManageIkan_Click(object sender, RoutedEventArgs e)
        {
            // Tetap di halaman ini
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logout berhasil!");
        }
    }
}