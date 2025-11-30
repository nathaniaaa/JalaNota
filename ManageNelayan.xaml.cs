using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Threading.Tasks;

namespace JalaNota
{
    public partial class ManageNelayan : Window
    {
        public ObservableCollection<Nelayan> DaftarNelayanView { get; set; }
        private Admin _adminLogin;

        public ManageNelayan(Admin adminDariLogin)
        {
            InitializeComponent();
            _adminLogin = adminDariLogin;
            DaftarNelayanView = new ObservableCollection<Nelayan>();
            dataGridNelayan.ItemsSource = DaftarNelayanView;

            // Load data saat window dibuka - using async pattern
            Loaded += async (s, e) => await LoadDataFromSupabase();
        }

        // LoadDataFromSupabase method - returns Task for awaitable calls
        private async Task LoadDataFromSupabase()
        {
            try
            {
                // Tampilkan loading indicator 
                this.Cursor = System.Windows.Input.Cursors.Wait;

                // Encapsulation Method: Nelayan.LihatSemuaNelayan()
                var daftarNelayan = await Nelayan.LihatSemuaNelayan();

                // Update UI
                DaftarNelayanView.Clear();
                foreach (var nelayan in daftarNelayan)
                {
                    DaftarNelayanView.Add(nelayan);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat data nelayan: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        // --- CRUD Operations menggunakan Encapsulation ---

        // Tambah nelayan baru
        private async void btnInput_Click(object sender, RoutedEventArgs e)
        {
            // Validasi UI - ID tidak perlu dicek karena auto-generate
            if (string.IsNullOrWhiteSpace(txtNamaNelayan.Text) || 
                string.IsNullOrWhiteSpace(txtUsername.Text) || 
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Nama, Username, dan Password harus diisi!", 
                    "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Password harus minimal 6 karakter!", 
                    "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                this.Cursor = System.Windows.Input.Cursors.Wait;

                // Trim dan simpan username (spasi dihapus otomatis di method Nelayan)
                string username = txtUsername.Text.Trim();

                // Encapsulation Method: Panggil method yang sudah ada validasinya
                bool sukses = await Nelayan.TambahNelayanBaru(
                    txtNamaNelayan.Text.Trim(), 
                    username, 
                    txtPassword.Text
                );

                if (sukses)
                {
                    // Tampilkan username tanpa spasi di pesan sukses
                    string cleanUsername = username.Replace(" ", "");
                    MessageBox.Show($"Nelayan baru berhasil ditambahkan.\nUsername: {cleanUsername}", 
                        "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Reload data dari database
                    await LoadDataFromSupabase();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Gagal menambahkan nelayan. Username mungkin sudah digunakan.", 
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        // Edit nelayan
        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Ambil ID dari TextBox bukan dari selected item
            if (string.IsNullOrWhiteSpace(txtIDNelayan.Text) || !int.TryParse(txtIDNelayan.Text, out int nelayanId))
            {
                MessageBox.Show("Pilih data nelayan yang ingin diedit dari tabel terlebih dahulu.",
                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Validasi UI
            if (string.IsNullOrWhiteSpace(txtNamaNelayan.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Nama, Username, dan Password tidak boleh kosong!",
                    "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Password harus minimal 6 karakter!",
                    "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                this.Cursor = System.Windows.Input.Cursors.Wait;

                // Trim username (spasi dihapus otomatis di method Nelayan)
                string username = txtUsername.Text.Trim();

                // Debug: Tampilkan ID yang akan diupdate
                System.Diagnostics.Debug.WriteLine($"Attempting to update Nelayan ID: {nelayanId}");
                System.Diagnostics.Debug.WriteLine($"New Username: {username}");

                // Encapsulation Method: Panggil method untuk update data dengan ID dari textbox
                bool sukses = await Nelayan.UbahNelayan(
                    nelayanId,  
                    txtNamaNelayan.Text.Trim(),
                    username,
                    txtPassword.Text
                );

                if (sukses)
                {
                    MessageBox.Show("Data nelayan berhasil diubah.",
                        "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Reload data dari database
                    await LoadDataFromSupabase();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Gagal mengubah data. Username mungkin sudah digunakan oleh nelayan lain.",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        // Hapus nelayan
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Ambil ID dari TextBox
            if (string.IsNullOrWhiteSpace(txtIDNelayan.Text) || !int.TryParse(txtIDNelayan.Text, out int nelayanId))
            {
                MessageBox.Show("Pilih data nelayan yang ingin dihapus dari tabel.",
                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Dapatkan nama untuk konfirmasi
            string namaNelayan = txtNamaNelayan.Text;

            MessageBoxResult result = MessageBox.Show(
                $"Yakin ingin menghapus {namaNelayan}?",
                "Konfirmasi Hapus",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    this.Cursor = System.Windows.Input.Cursors.Wait;

                    // Encapsulation Method: Panggil method untuk delete dengan ID dari textbox
                    bool sukses = await Nelayan.HapusNelayan(nelayanId);

                    if (sukses)
                    {
                        MessageBox.Show("Nelayan berhasil dihapus.",
                            "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Reload data dari database
                        await LoadDataFromSupabase();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Gagal menghapus nelayan.",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    this.Cursor = System.Windows.Input.Cursors.Arrow;
                }
            }
        }

        // --- UI Helper Methods ---

        private void dataGridNelayan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridNelayan.SelectedItem is Nelayan selected)
            {
                txtIDNelayan.Text = selected.IDNelayan.ToString();
                txtNamaNelayan.Text = selected.NamaNelayan;
                txtUsername.Text = selected.UsnNelayan;
                txtPassword.Text = selected.PasswordNelayan;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtIDNelayan.Clear();
            txtNamaNelayan.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            dataGridNelayan.UnselectAll();
        }

        // --- Navigation Methods ---
        private void ManageSetoran_Click(object sender, RoutedEventArgs e)
        {
            ManageSetoran manageSetoranWindow = new ManageSetoran(_adminLogin);
            manageSetoranWindow.Show();
            this.Close();
        }

        private void ManageNelayan_Click(object sender, RoutedEventArgs e) 
        { 
            // Tetap di halaman ini  
        }

        private void ManageIkan_Click(object sender, RoutedEventArgs e)
        {
            ManageDaftarIkan manageIkanWindow = new ManageDaftarIkan(_adminLogin);
            manageIkanWindow.Show();
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginNelayan loginWindow = new LoginNelayan();
            loginWindow.Show();
            this.Close();
        }
    }
}