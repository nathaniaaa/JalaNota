using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls; 

namespace JalaNota
{
    public partial class ManageDaftarIkan : Window
    {
        public ObservableCollection<Ikan> DaftarIkan { get; set; }

        public ManageDaftarIkan()
        {
            InitializeComponent();

            // Data dummy 
            DaftarIkan = new ObservableCollection<Ikan>()
            {
                new Ikan(){ IDIkan="IKN001", NamaIkan="Gurame", Harga=40000 },
                new Ikan(){ IDIkan="IKN002", NamaIkan="Lele", Harga=25000 },
                new Ikan(){ IDIkan="IKN003", NamaIkan="Nila", Harga=30000 }
            };
            dataGridIkan.ItemsSource = DaftarIkan;
        }

        // --- CRUD Buttons ---

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIDIkan.Text) ||
                string.IsNullOrWhiteSpace(txtNamaIkan.Text) ||
                string.IsNullOrWhiteSpace(txtHarga.Text))
            {
                MessageBox.Show("Semua field harus diisi!", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtHarga.Text, out int harga))
            {
                MessageBox.Show("Harga harus berupa angka!", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DaftarIkan.Add(new Ikan()
            {
                IDIkan = txtIDIkan.Text,
                NamaIkan = txtNamaIkan.Text,
                Harga = harga
            });

            ClearForm();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridIkan.SelectedItem is Ikan selected)
            {
                if (!int.TryParse(txtHarga.Text, out int harga))
                {
                    MessageBox.Show("Harga harus berupa angka!", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                selected.IDIkan = txtIDIkan.Text;
                selected.NamaIkan = txtNamaIkan.Text;
                selected.Harga = harga;
                dataGridIkan.Items.Refresh();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Pilih data ikan yang ingin diedit dari tabel.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridIkan.SelectedItem is Ikan selected)
            {
                MessageBoxResult result = MessageBox.Show($"Yakin ingin menghapus {selected.NamaIkan}?",
                    "Konfirmasi Hapus", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DaftarIkan.Remove(selected);
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
            if (dataGridIkan.SelectedItem is Ikan selected)
            {
                txtIDIkan.Text = selected.IDIkan;
                txtNamaIkan.Text = selected.NamaIkan;
                txtHarga.Text = selected.Harga.ToString();
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
            MessageBox.Show("Navigasi ke halaman 'Manage Setoran'");
        }

        private void ManageNelayan_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Navigasi ke halaman 'Manage Nelayan'");
            
        }

        private void ManageIkan_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logout berhasil!");
        }
    }

    public class Ikan
    {
        public string IDIkan { get; set; }
        public string NamaIkan { get; set; }
        public int Harga { get; set; }
    }
}