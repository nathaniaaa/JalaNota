using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks; 

namespace JalaNota
{
    public partial class ManageSetoran : Window
    {
        public ObservableCollection<SetoranView> DaftarSetoranView { get; set; }
        private Admin _adminLogin;
        private Dictionary<int, string> _kamusNelayan = new Dictionary<int, string>();
        private Dictionary<int, JenisIkan> _kamusIkan = new Dictionary<int, JenisIkan>();

        public ManageSetoran(Admin adminDariLogin)
        {
            InitializeComponent();
            _adminLogin = adminDariLogin;
            DaftarSetoranView = new ObservableCollection<SetoranView>();
            dataGridSetoran.ItemsSource = DaftarSetoranView;
            this.Loaded += Window_Loaded_Async;
        }

        private async void Window_Loaded_Async(object sender, RoutedEventArgs e)
        {
            await InisialisasiDataHalaman();
            dpTanggalSetoran.SelectedDate = DateTime.Now.Date;
            // Set waktu sekarang
            txtJam.Text = DateTime.Now.Hour.ToString("00");
            txtMenit.Text = DateTime.Now.Minute.ToString("00");
        }
        private async Task InisialisasiDataHalaman()
        {
            try
            {
                // ambil data Ikan, simpan di kamus, isi ComboBox
                var ikanResponse = await SupabaseClient.Instance.From<JenisIkan>().Get();
                _kamusIkan = ikanResponse.Models.ToDictionary(i => i.IDIkan);
                cmbNamaIkan.ItemsSource = ikanResponse.Models;

                // ambil data Nelayan, simpan di kamus, isi ComboBox
                var nelayanResponse = await SupabaseClient.Instance.From<Nelayan>().Get();
                _kamusNelayan = nelayanResponse.Models.ToDictionary(n => n.IDNelayan, n => n.NamaNelayan);
                cmbNamaNelayan.ItemsSource = nelayanResponse.Models;

                // muat data setoran
                await MuatDaftarSetoran();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat data awal: {ex.Message}", "Error Koneksi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task MuatDaftarSetoran()
        {
            DaftarSetoranView.Clear();

            try
            {
                var setoranResponse = await SupabaseClient.Instance.From<Setoran>()
                                    .Order("IDSetoran", Postgrest.Constants.Ordering.Descending)
                                    .Get();

                foreach (var s in setoranResponse.Models)
                {
                    _kamusIkan.TryGetValue(s.IDIkan, out JenisIkan ikan);
                    _kamusNelayan.TryGetValue(s.IDNelayan, out string namaNelayan);

                    DaftarSetoranView.Add(new SetoranView
                    {
                        IDSetoran = s.IDSetoran,
                        TanggalSaja = s.WaktuSetor.ToString("dd/MM/yyyy"),
                        WaktuSaja = s.WaktuSetor.ToString("HH:mm"),
                        WaktuSetor = s.WaktuSetor,
                        IDNelayan = s.IDNelayan,
                        IDIkan = s.IDIkan,
                        NamaNelayan = namaNelayan ?? $"ID {s.IDNelayan}",
                        NamaIkan = ikan?.NamaIkan ?? "N/A",
                        Berat = s.BeratKg,
                        Total = s.HargaTotal
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat daftar setoran: {ex.Message}", "Error Koneksi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // CRUD
        private async void btnInput_Click(object sender, RoutedEventArgs e)
        {
            // Validasi - ID tidak perlu dicek karena auto-generate di database
            if (cmbNamaNelayan.SelectedValue == null || cmbNamaIkan.SelectedValue == null ||
                !dpTanggalSetoran.SelectedDate.HasValue || string.IsNullOrWhiteSpace(txtBerat.Text) ||
                string.IsNullOrWhiteSpace(txtJam.Text) || string.IsNullOrWhiteSpace(txtMenit.Text))
            {
                MessageBox.Show("Semua kolom harus diisi dengan benar.", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validasi jam dan menit
            if (!int.TryParse(txtJam.Text, out int jam) || jam < 0 || jam > 23 ||
                !int.TryParse(txtMenit.Text, out int menit) || menit < 0 || menit > 59)
            {
                MessageBox.Show("Format waktu tidak valid. Jam (0-23) dan Menit (0-59).", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(txtBerat.Text, out double berat) || berat <= 0)
            {
                MessageBox.Show("Berat (kg) harus berupa angka positif yang valid.", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int idNelayan = (int)cmbNamaNelayan.SelectedValue;
                int idIkan = (int)cmbNamaIkan.SelectedValue;
                DateTime tanggalSetoran = dpTanggalSetoran.SelectedDate.Value.Date;

                // catat setoran
                _kamusIkan.TryGetValue(idIkan, out JenisIkan ikan);
                if (ikan == null)
                {
                    MessageBox.Show("Error: Data ikan tidak ditemukan.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                double totalHarga = berat * ikan.HargaPerKg; // hitung total

                // Gabungkan tanggal dengan jam dan menit
                var waktuSetor = tanggalSetoran
                    .AddHours(jam)
                    .AddMinutes(menit);

                // Validasi admin login
                if (_adminLogin == null || _adminLogin.IDAdmin <= 0)
                {
                    MessageBox.Show("Error: Data admin tidak valid. Silakan login ulang.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var setoranBaru = new Setoran
                {
                    IDNelayan = idNelayan,
                    IDIkan = idIkan,
                    IDAdmin = _adminLogin.IDAdmin,
                    WaktuSetor = waktuSetor,
                    BeratKg = berat,
                    HargaTotal = totalHarga
                    // IDSetoran akan di-generate otomatis oleh database
                };

                await SupabaseClient.Instance.From<Setoran>().Insert(setoranBaru);

                // perbarui Tampilan
                await MuatDaftarSetoran();
                MessageBox.Show("Setoran baru berhasil dicatat!", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal mencatat setoran: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // edit 
        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtIDSetoran.Text, out int idSetoran) || idSetoran <= 0)
            {
                MessageBox.Show("Pilih data setoran dari tabel untuk mengedit.", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cmbNamaNelayan.SelectedValue == null || cmbNamaIkan.SelectedValue == null || 
                !dpTanggalSetoran.SelectedDate.HasValue || string.IsNullOrWhiteSpace(txtBerat.Text) ||
                string.IsNullOrWhiteSpace(txtJam.Text) || string.IsNullOrWhiteSpace(txtMenit.Text))
            {
                MessageBox.Show("Semua kolom harus diisi dengan benar.", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validasi jam dan menit
            if (!int.TryParse(txtJam.Text, out int jam) || jam < 0 || jam > 23 ||
                !int.TryParse(txtMenit.Text, out int menit) || menit < 0 || menit > 59)
            {
                MessageBox.Show("Format waktu tidak valid. Jam (0-23) dan Menit (0-59).", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int idNelayan = (int)cmbNamaNelayan.SelectedValue;
                int idIkan = (int)cmbNamaIkan.SelectedValue;
                DateTime tanggalSetoran = dpTanggalSetoran.SelectedDate.Value.Date;
                double.TryParse(txtBerat.Text, out double berat);

                // edit setoran
                _kamusIkan.TryGetValue(idIkan, out JenisIkan ikan);
                if (ikan == null)
                {
                    MessageBox.Show("Error: Data ikan tidak ditemukan.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                double totalHarga = berat * ikan.HargaPerKg; // Hitung ulang total

                await SupabaseClient.Instance.From<Setoran>()
                    .Where(s => s.IDSetoran == idSetoran)
                    .Set(s => s.IDNelayan, idNelayan)
                    .Set(s => s.IDIkan, idIkan)
                    .Set(s => s.WaktuSetor, tanggalSetoran)
                    .Set(s => s.BeratKg, berat)
                    .Set(s => s.HargaTotal, totalHarga)
                    .Set(s => s.IDAdmin, _adminLogin.IDAdmin) // Catat siapa yg edit
                    .Update();

                // perbarui Tampilan
                await MuatDaftarSetoran();
                MessageBox.Show($"Setoran ID {idSetoran} berhasil diperbarui!", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memperbarui setoran: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // delete
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridSetoran.SelectedItem is SetoranView selected)
            {
                MessageBoxResult result = MessageBox.Show($"Yakin ingin menghapus Setoran ID {selected.IDSetoran}?",
                        "Konfirmasi Hapus", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await SupabaseClient.Instance.From<Setoran>()
                            .Where(s => s.IDSetoran == selected.IDSetoran)
                            .Delete();

                        await MuatDaftarSetoran();
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Gagal menghapus setoran: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
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
                txtIDSetoran.Text = selected.IDSetoran.ToString();
                txtBerat.Text = selected.Berat.ToString();
                dpTanggalSetoran.SelectedDate = selected.WaktuSetor.Date;
                txtJam.Text = selected.WaktuSetor.Hour.ToString("00");
                txtMenit.Text = selected.WaktuSetor.Minute.ToString("00");
                cmbNamaNelayan.SelectedValue = selected.IDNelayan;
                cmbNamaIkan.SelectedValue = selected.IDIkan;
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
            // Set waktu sekarang saat membersihkan form
            txtJam.Text = DateTime.Now.Hour.ToString("00");
            txtMenit.Text = DateTime.Now.Minute.ToString("00");
            dpTanggalSetoran.SelectedDate = DateTime.Now.Date;
            cmbNamaNelayan.SelectedIndex = -1;
            cmbNamaIkan.SelectedIndex = -1;
            dataGridSetoran.UnselectAll();
        }

        // navbar
        private void ManageSetoran_Click(object sender, RoutedEventArgs e) { }
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

    // Kelas Model (View Model)
    public class SetoranView
    {
        public int IDSetoran { get; set; }
        public string TanggalSaja { get; set; }    // Untuk menampilkan tanggal
        public string WaktuSaja { get; set; }      // Untuk menampilkan waktu
        public DateTime WaktuSetor { get; set; }   // Tetap menyimpan DateTime lengkap
        public int IDNelayan { get; set; }
        public int IDIkan { get; set; }
        public string NamaNelayan { get; set; }
        public string NamaIkan { get; set; }
        public double Berat { get; set; }
        public double Total { get; set; }
        public string HargaFormattedTotal 
        { 
            get 
            {
                return $"Rp {this.Total:N0}";
            }
        }
    }
}