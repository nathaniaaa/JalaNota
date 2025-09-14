using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JalaNota
{
    public class Admin
    {
        // Properti
        public int IDAdmin { get; set; }
        public string NamaAdmin { get; set; }
        public string UsernameAdmin { get; set; }
        public string PasswordAdmin { get; set; }

        // Metode
        public Boolean Login(string username, string password)
        {
            if (username == "adminKay" && password == "kay123")
            {
                // TODO: ganti dummy dengan data dari database
                IDAdmin = 1;
                NamaAdmin = "Admin Kay";
                UsernameAdmin = "adminKay";
                return true;
            }
            else if (username == "adminEga" && password == "ega123")
            {
                IDAdmin = 2;
                NamaAdmin = "Admin Ega";
                UsernameAdmin = "adminEga";
                return true;
            }
            else if (username == "adminNia" && password == "nia123")
            {
                IDAdmin = 3;
                NamaAdmin = "Admin Nia";
                UsernameAdmin = "adminNia";
                return true;
            }
            else
            {
                return false;
            }
        }

        public void TambahNelayan(string nama, string user, string password)
        {
            // TODO: isi kode untuk menyimpan data nelayan baru ke database
            Console.WriteLine($"Menambahkan nelayan: Nama={nama}, Username={user}");
        }
        public void EditNelayan(int IDNelayan, string nama, string user, string password)
        {
            // TODO: isi kode untuk memperbarui data nelayan di database berdasarkan IDNelayan
            Console.WriteLine($"Mengedit nelayan ID={IDNelayan}: Nama={nama}, Username={user}");
        }
        public void HapusNelayan(int IDNelayan)
        {
            // TODO: isi kode untuk menghapus data nelayan dari database berdasarkan IDNelayan
        }

        public void TambahJenisIkan(string namaIkan, double harga)
        {
            JenisIkan.TambahIkanBaru(namaIkan, harga);
            Console.WriteLine($"--- Admin {this.NamaAdmin} menambahkan ikan: {namaIkan} ---");
        }

        public void EditHargaIkan(int idIkan, double hargaBaru)
        {
            JenisIkan.UbahHargaIkan(idIkan, hargaBaru);
            Console.WriteLine($"--- Admin {this.NamaAdmin} mengubah harga ikan ID: {idIkan} ---");
        }
    }
}