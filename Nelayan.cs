using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JalaNota
{
    public class Nelayan
    {
        // Properti
        public int IDNelayan { get; set; }
        public string NamaNelayan { get; set; }
        public string UsnNelayan { get; set; }
        public string PasswordNelayan { get; set; }

        // Daftar nelayan statis
        private static List<Nelayan> daftarNelayan = new List<Nelayan>
        {
            new Nelayan { IDNelayan = 1, NamaNelayan = "Nelayan Satu", UsnNelayan = "nelayanSatu", PasswordNelayan = "satu123" },
            new Nelayan { IDNelayan = 2, NamaNelayan = "Nelayan Dua", UsnNelayan = "nelayanDua", PasswordNelayan = "dua123" }
        };

        // Metode
        public Boolean Login(string username, string password)
        {
            // Cari nelayan berdasarkan username dan password
            Nelayan nelayanLogin = daftarNelayan.FirstOrDefault(n => n.UsnNelayan == username && n.PasswordNelayan == password);

            if (nelayanLogin != null)
            {
                // Jika ditemukan, isi properti objek Nelayan saat ini
                this.IDNelayan = nelayanLogin.IDNelayan;
                this.NamaNelayan = nelayanLogin.NamaNelayan;
                this.UsnNelayan = nelayanLogin.UsnNelayan;
                return true;
            }
            else
            {
                return false;
            }
        }

        // Metode statis untuk melihat semua nelayan (digunakan oleh Admin)
        public static List<Nelayan> LihatSemuaNelayan()
        {
            return daftarNelayan;
        }

        // Metode statis untuk mendapatkan Nelayan berdasarkan ID
        public static Nelayan GetNelayanById(int idNelayan)
        {
            return daftarNelayan.FirstOrDefault(n => n.IDNelayan == idNelayan);
        }

        public void LihatHargaIkan()
        {
            Console.WriteLine("\n--- Daftar Harga Ikan Saat Ini ---");
            List<JenisIkan> daftarIkan = JenisIkan.LihatSemuaJenisIkan();

            // Menampilkan setiap ikan dalam daftar
            foreach (var ikan in daftarIkan)
            {
                Console.WriteLine($"ID: {ikan.IDIkan}, Nama: {ikan.NamaIkan}, Harga/Kg: Rp{ikan.HargaPerKg}");
            }
            Console.WriteLine("---------------------------------\n");
        }

        public void LihatRiwayatSetoran()
        {
            Console.WriteLine($"\n--- Riwayat Setoran untuk {this.NamaNelayan} ---");

            // Mengambil semua data setoran
            List<Setoran> semuaSetoran = Setoran.LihatSemuaSetoran();

            // Memfilter data setoran hanya untuk nelayan yang sedang login
            var riwayatNelayan = semuaSetoran.Where(s => s.IDNelayan == this.IDNelayan).ToList();

            if (riwayatNelayan.Any())
            {
                foreach (var setoran in riwayatNelayan)
                {
                    // Mengambil detail nama ikan berdasarkan IDIkan
                    JenisIkan ikan = JenisIkan.GetJenisIkanById(setoran.IDIkan);
                    string namaIkan = ikan != null ? ikan.NamaIkan : "Tidak Ditemukan";

                    Console.WriteLine($"ID Setoran: {setoran.IDSetoran}");
                    Console.WriteLine($"Tanggal   : {setoran.WaktuSetor}");
                    Console.WriteLine($"Ikan      : {namaIkan}");
                    Console.WriteLine($"Berat     : {setoran.BeratKg} Kg");
                    Console.WriteLine($"Total     : Rp{setoran.HargaTotal}");
                    Console.WriteLine("---------------------------------");
                }
            }
            else
            {
                Console.WriteLine("Anda belum memiliki riwayat setoran.");
                Console.WriteLine("---------------------------------\n");
            }
        }
    }
}