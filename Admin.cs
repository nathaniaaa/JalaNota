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
        public void CatatSetoran(int idNelayan, int idIkan, double berat, DateTime waktuSetor)
        {
            JenisIkan ikan = JenisIkan.GetJenisIkanById(idIkan);
            if (ikan == null)
            {
                Console.WriteLine($"Error: Ikan dengan ID {idIkan} tidak ditemukan.");
                return;
            }

            Setoran setoranBaru = new Setoran();
            double totalHarga = setoranBaru.HitungTotalHarga(berat, ikan.HargaPerKg);

            setoranBaru.IDNelayan = idNelayan;
            setoranBaru.IDIkan = idIkan;
            setoranBaru.BeratKg = berat;
            setoranBaru.IDAdmin = this.IDAdmin;
            setoranBaru.WaktuSetor = waktuSetor;
            setoranBaru.HargaTotal = totalHarga;

            Setoran.TambahSetoranBaru(setoranBaru);

            Console.WriteLine("\n--- Setoran Berhasil Dicatat ---");
            Console.WriteLine($"Dicatat oleh Admin: {this.NamaAdmin}");
            Console.WriteLine($"Untuk Nelayan ID  : {idNelayan}");
            Console.WriteLine($"Ikan              : {ikan.NamaIkan}");
            Console.WriteLine($"Berat             : {berat} Kg");
            Console.WriteLine($"Total Harga       : Rp{totalHarga}");
            Console.WriteLine("--------------------------------\n");
        }
        public bool EditSetoran(int idSetoran, int idNelayan, int idIkan, double berat, DateTime waktuSetor)
        {
            JenisIkan ikan = JenisIkan.GetJenisIkanById(idIkan);
            if (ikan == null)
            {
                Console.WriteLine($"Error: Ikan dengan ID {idIkan} tidak ditemukan saat edit.");
                return false;
            }

            Setoran setoranModel = new Setoran(); // Objek sementara untuk memanggil HitungTotalHarga
            double totalHarga = setoranModel.HitungTotalHarga(berat, ikan.HargaPerKg);

            // Panggil metode statis UbahSetoran
            bool sukses = Setoran.UbahSetoran(idSetoran, idNelayan, idIkan, berat, waktuSetor, this.IDAdmin, totalHarga);

            if (sukses)
            {
                Console.WriteLine("\n--- Setoran Berhasil Diperbarui ---");
                Console.WriteLine($"Diperbarui oleh Admin: {this.NamaAdmin}");
                Console.WriteLine($"ID Setoran        : {idSetoran}");
                Console.WriteLine($"Ikan              : {ikan.NamaIkan}");
                Console.WriteLine($"Berat             : {berat} Kg");
                Console.WriteLine($"Total Harga       : Rp{totalHarga}");
                Console.WriteLine("--------------------------------\n");
            }
            else
            {
                Console.WriteLine($"Error: Setoran ID {idSetoran} tidak ditemukan untuk diubah.");
            }
            return sukses;
        }
    }
}