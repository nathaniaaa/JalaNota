using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace JalaNota
{
    public partial class LihatHargaIkan_non_admin_ : Window
    {
        public LihatHargaIkan_non_admin_()
        {
            InitializeComponent();   
            LoadDummyData();         
        }

        private void LoadDummyData()
        {
            var daftarIkan = new List<HargaIkan>
            {
                new HargaIkan { NamaIkan = "Ikan Lele", Harga = "25.000" },
                new HargaIkan { NamaIkan = "Ikan Nila", Harga = "28.000" },
                new HargaIkan { NamaIkan = "Ikan Gurame", Harga = "45.000" },
                new HargaIkan { NamaIkan = "Ikan Mujair", Harga = "30.000" },
                new HargaIkan { NamaIkan = "Ikan Patin", Harga = "27.000" },
                new HargaIkan { NamaIkan = "Ikan Tuna", Harga = "75.000" },
                new HargaIkan { NamaIkan = "Ikan Bandeng", Harga = "33.000" },
                new HargaIkan { NamaIkan = "Ikan Tongkol", Harga = "38.000" },
                new HargaIkan { NamaIkan = "Ikan Kakap Merah", Harga = "60.000" },
                new HargaIkan { NamaIkan = "Ikan Kembung", Harga = "32.000" }
            };

            dataGridIkan.ItemsSource = daftarIkan;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginNelayan loginNelayanWindow = new LoginNelayan();
            loginNelayanWindow.Show();
            this.Close();
        }

        private void dataGridIkan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public class HargaIkan
    {
        public string NamaIkan { get; set; }
        public string Harga { get; set; }
    }
}
