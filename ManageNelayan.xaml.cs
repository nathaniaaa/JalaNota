using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

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
            MuatDaftarNelayan();
        }

        private void MuatDaftarNelayan()
        {
            DaftarNelayanView.Clear();
            List<Nelayan> dataDariModel = Nelayan.LihatSemuaNelayan();
            foreach (var nelayan in dataDariModel)
            {
                DaftarNelayanView.Add(nelayan);
            }
        }

        // --- Tombol CRUD ---
        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            // Validasi: ID Nelayan tidak perlu diisi karena dibuat otomatis di Nelayan.TambahNelayanBaru
            if (string.IsNullOrWhiteSpace(txtNamaNelayan.Text) || string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Nama, Username, dan Password harus diisi!", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Nelayan.TambahNelayanBaru(txtNamaNelayan.Text, txtUsername.Text, txtPassword.Text);
            MuatDaftarNelayan();
            ClearForm();
            MessageBox.Show("Nelayan baru berhasil ditambahkan.", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridNelayan.SelectedItem is Nelayan selected)
            {
                if (string.IsNullOrWhiteSpace(txtNamaNelayan.Text) || string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Nama, Username, dan Password tidak boleh kosong!", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                bool sukses = Nelayan.UbahNelayan(selected.IDNelayan, txtNamaNelayan.Text, txtUsername.Text, txtPassword.Text);
                if (sukses)
                {
                    MuatDaftarNelayan();
                    ClearForm();
                    MessageBox.Show("Data nelayan berhasil diubah.", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Gagal mengubah data. ID Nelayan tidak ditemukan.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Pilih data nelayan yang ingin diedit dari tabel.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridNelayan.SelectedItem is Nelayan selected)
            {
                MessageBoxResult result = MessageBox.Show($"Yakin ingin menghapus {selected.NamaNelayan}?", "Konfirmasi Hapus", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Nelayan.HapusNelayan(selected.IDNelayan);
                    MuatDaftarNelayan();
                    ClearForm();
                }
            }
            else
            {
                MessageBox.Show("Pilih data nelayan yang ingin dihapus dari tabel.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // --- Interaksi UI Lainnya ---
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

        // --- Tombol Navbar ---
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