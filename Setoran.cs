using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JalaNota
{
    public class Setoran
    {
        // Properti
        public int IDSetoran { get; set; }
        public int IDNelayan { get; set; }
        public int IDAdmin { get; set; }
        public int IDIkan { get; set; }
        public DateTime WaktuSetor { get; set; }
        public double BeratKg { get; set; }
        public double HargaTotal { get; set; }

        // Mengisi daftarSetoran dengan data dummy
        private static List<Setoran> daftarSetoran = new List<Setoran>
        {
            // Setoran Dummy 1
            new Setoran
            {
                IDSetoran = 1,
                IDNelayan = 1, // Nelayan Satu
                IDAdmin = 1,   // Admin Kay
                IDIkan = 1,    // Tuna (Harga 30000 di JenisIkan.cs)
                WaktuSetor = new DateTime(2025, 10, 20),
                BeratKg = 10.5,
                HargaTotal = 10.5 * 30000 // Total: 315,000
            },
            
            // Setoran Dummy 2
            new Setoran
            {
                IDSetoran = 2,
                IDNelayan = 2, // Nelayan Dua
                IDAdmin = 2,   // Admin Ega
                IDIkan = 2,    // Tongkol (Harga 25000 di JenisIkan.cs)
                WaktuSetor = new DateTime(2025, 10, 23),
                BeratKg = 5.0,
                HargaTotal = 5.0 * 25000 // Total: 125,000
            },
            
            // Setoran Dummy 3
            new Setoran
            {
                IDSetoran = 3,
                IDNelayan = 1, // Nelayan Satu
                IDAdmin = 3,   // Admin Nia
                IDIkan = 3,    // Cakalang (Harga 28000 di JenisIkan.cs)
                WaktuSetor = new DateTime(2025, 10, 26),
                BeratKg = 15.0,
                HargaTotal = 15.0 * 28000 // Total: 420,000
            }
        };

        // Metode
        public double HitungTotalHarga(double berat, double hargaPerKg)
        {
            return berat * hargaPerKg;
        }

        public static List<Setoran> LihatSemuaSetoran()
        {
            return daftarSetoran;
        }

        public static bool UbahSetoran(int idSetoran, int idNelayan, int idIkan, double beratKg, DateTime waktuSetor, int idAdmin, double hargaTotal)
        {
            // Cari setoran di dalam daftar berdasarkan IDSetoran
            Setoran setoran = daftarSetoran.FirstOrDefault(s => s.IDSetoran == idSetoran);

            if (setoran != null)
            {
                // Jika ditemukan, perbarui propertinya
                setoran.IDNelayan = idNelayan;
                setoran.IDIkan = idIkan;
                setoran.BeratKg = beratKg;
                setoran.WaktuSetor = waktuSetor;
                setoran.IDAdmin = idAdmin;
                setoran.HargaTotal = hargaTotal; // HargaTotal harus sudah dihitung di Admin.cs
                return true;
            }
            // Jika tidak ketemu, kembalikan false
            return false;
        }

        public static void TambahSetoranBaru(Setoran setoranBaru)
        {
            // Membuat ID baru secara otomatis
            int newId = daftarSetoran.Count > 0 ? daftarSetoran.Max(s => s.IDSetoran) + 1 : 1;
            setoranBaru.IDSetoran = newId;
            daftarSetoran.Add(setoranBaru);
        }
    }
}