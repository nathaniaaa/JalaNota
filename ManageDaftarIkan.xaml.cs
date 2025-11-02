using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Threading.Tasks; 
using System; 

namespace JalaNota
{
    public partial class ManageDaftarIkan : Window
    {
        public ObservableCollection<JenisIkan> DaftarIkanView { get; set; }
        private Admin _adminLogin;

        public ManageDaftarIkan(Admin adminDariLogin)
        {
            InitializeComponent();
            _adminLogin = adminDariLogin;

            DaftarIkanView = new ObservableCollection<JenisIkan>();
            dataGridIkan.ItemsSource = DaftarIkanView;
            this.Loaded += Window_Loaded_Async;
        }

        private async void Window_Loaded_Async(object sender, RoutedEventArgs e)
        {
            await MuatDaftarIkan();
        }

        private async Task MuatDaftarIkan()
        {
            DaftarIkanView.Clear();

            try
            {
                // ambil data dari Supabase
                var response = await SupabaseClient.Instance.From<JenisIkan>()
                                                .Order("NamaIkan", Postgrest.Constants.Ordering.Ascending)
                                                .Get();

                if (response.Models != null)
                {
                    foreach (var ikan in response.Models)
                    {
                        DaftarIkanView.Add(ikan);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat daftar ikan: {ex.Message}", "Error Koneksi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- CRUD Buttons ---
        private async void btnInput_Click(object sender, RoutedEventArgs e)
        {
            // validasi 
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

            // tambah ikan baru
            try
            {
                var ikanBaru = new JenisIkan
                {
                    NamaIkan = txtNamaIkan.Text,
                    HargaPerKg = harga
                };

                // kirim data baru ke Supabase
                await SupabaseClient.Instance.From<JenisIkan>().Insert(ikanBaru);

                // muat ulang data untuk merefresh DataGrid
                await MuatDaftarIkan();
                ClearForm();
                MessageBox.Show("Ikan baru berhasil ditambahkan.", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal menambahkan ikan: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridIkan.SelectedItem is JenisIkan selected)
            {
                if (!double.TryParse(txtHarga.Text, out double hargaBaru))
                {
                    MessageBox.Show("Harga harus berupa angka!", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // cek apakah ada perubahan
                if (selected.NamaIkan == txtNamaIkan.Text && selected.HargaPerKg == hargaBaru)
                {
                    MessageBox.Show("Tidak ada perubahan data yang dilakukan.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // ubah tabel ikan
                try
                {
                    await SupabaseClient.Instance.From<JenisIkan>()
                          .Where(i => i.IDIkan == selected.IDIkan)
                          .Set(i => i.NamaIkan, txtNamaIkan.Text)
                          .Set(i => i.HargaPerKg, hargaBaru)
                          .Update();

                    // muat ulang data
                    await MuatDaftarIkan();
                    ClearForm();
                    MessageBox.Show("Data ikan berhasil diperbarui.", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Gagal mengedit ikan: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Pilih data ikan yang ingin diedit dari tabel.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // delete
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridIkan.SelectedItem is JenisIkan selected)
            {
                MessageBoxResult result = MessageBox.Show($"Yakin ingin menghapus {selected.NamaIkan}?",
                    "Konfirmasi Hapus", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await SupabaseClient.Instance.From<JenisIkan>()
                                    .Where(i => i.IDIkan == selected.IDIkan)
                                    .Delete();

                        // muat ulang data
                        await MuatDaftarIkan();
                        ClearForm();
                        MessageBox.Show("Data ikan berhasil dihapus.", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Gagal menghapus ikan: {ex.Message}. (Pastikan ikan tidak dipakai di data setoran)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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

        // navbar

        private void ManageSetoran_Click(object sender, RoutedEventArgs e)
        {
            ManageSetoran manageSetoranWindow = new ManageSetoran(_adminLogin);
            manageSetoranWindow.Show();
            this.Close();
        }

        private void ManageNelayan_Click(object sender, RoutedEventArgs e)
        {
            ManageNelayan manageNelayanWindow = new ManageNelayan(_adminLogin);
            manageNelayanWindow.Show();
            this.Close();
        }

        private void ManageIkan_Click(object sender, RoutedEventArgs e)
        {
            // Tetap di halaman ini
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginNelayan loginWindow = new LoginNelayan();
            loginWindow.Show();
            this.Close();
        }
    }
}