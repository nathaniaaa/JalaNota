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

        // Metode
        public Boolean Login(string username, string password)
        {
            // TODO: ganti dummy dengan data dari database
            if (username == "nelayanSatu" && password == "satu123")
            {
                IDNelayan = 1;
                NamaNelayan = "Nelayan Satu";
                UsnNelayan = "nelayanSatu";
                return true;
            }
            else if (username == "nelayanDua" && password == "dua123")
            {
                IDNelayan = 2;
                NamaNelayan = "Nelayan Dua";
                UsnNelayan = "nelayanDua";
                return true;
            }
            else
            {
                return false;
            }
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