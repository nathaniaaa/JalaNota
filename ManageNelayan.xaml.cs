using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Threading.Tasks; 
using System; 

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
            this.Loaded += Window_Loaded_Async;
        }

        private async void Window_Loaded_Async(object sender, RoutedEventArgs e)
        {
            await MuatDaftarNelayan();
        }

        private async Task MuatDaftarNelayan()
        {
            DaftarNelayanView.Clear();
            try
            {
                // ambil data dari Supabase
                var response = await SupabaseClient.Instance.From<Nelayan>()
                                    .Order("NamaNelayan", Postgrest.Constants.Ordering.Ascending)
                                    .Get();

                if (response.Models != null)
                {
                    foreach (var nelayan in response.Models)
                    {
                        DaftarNelayanView.Add(nelayan);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat daftar nelayan: {ex.Message}", "Error Koneksi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // CRUD
        private async void btnInput_Click(object sender, RoutedEventArgs e)
        {
            // validasi
            if (string.IsNullOrWhiteSpace(txtNamaNelayan.Text) || string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Nama, Username, dan Password harus diisi!", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // tambah nelayan
            try
            {
                var nelayanBaru = new Nelayan
                {
                    NamaNelayan = txtNamaNelayan.Text,
                    UsnNelayan = txtUsername.Text,
                    PasswordNelayan = txtPassword.Text
                };

                await SupabaseClient.Instance.From<Nelayan>().Insert(nelayanBaru);

                await MuatDaftarNelayan(); // muat ulang data
                ClearForm();
                MessageBox.Show("Nelayan baru berhasil ditambahkan.", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal menambahkan nelayan: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // edit nelayan
        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridNelayan.SelectedItem is Nelayan selected)
            {
                if (string.IsNullOrWhiteSpace(txtNamaNelayan.Text) || string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Nama, Username, dan Password tidak boleh kosong!", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    await SupabaseClient.Instance.From<Nelayan>()
                        .Where(n => n.IDNelayan == selected.IDNelayan)
                        .Set(n => n.NamaNelayan, txtNamaNelayan.Text)
                        .Set(n => n.UsnNelayan, txtUsername.Text)
                        .Set(n => n.PasswordNelayan, txtPassword.Text)
                        .Update();

                    await MuatDaftarNelayan(); // muat ulang data
                    ClearForm();
                    MessageBox.Show("Data nelayan berhasil diubah.", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Gagal mengubah data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Pilih data nelayan yang ingin diedit dari tabel.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // delete nelayan
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridNelayan.SelectedItem is Nelayan selected)
            {
                MessageBoxResult result = MessageBox.Show($"Yakin ingin menghapus {selected.NamaNelayan}?", "Konfirmasi Hapus", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await SupabaseClient.Instance.From<Nelayan>()
                            .Where(n => n.IDNelayan == selected.IDNelayan)
                            .Delete();

                        await MuatDaftarNelayan(); // muat ulang data
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Gagal menghapus nelayan: {ex.Message}. (Pastikan nelayan tidak memiliki data setoran terkait)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Pilih data nelayan yang ingin dihapus dari tabel.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

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

        // navbar
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