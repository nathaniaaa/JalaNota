using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JalaNota
{
    public class JenisIkan
    {
        // Properti
        public int IDIkan { get; set; }
        public string NamaIkan { get; set; }
        public double HargaPerKg { get; set; }

        // TODO: ganti dummy dengan data dari database
        private static List<JenisIkan> daftarIkan = new List<JenisIkan>
        {
            new JenisIkan { IDIkan = 1, NamaIkan = "Tuna", HargaPerKg = 30000 },
            new JenisIkan { IDIkan = 2, NamaIkan = "Tongkol", HargaPerKg = 25000 },
            new JenisIkan { IDIkan = 3, NamaIkan = "Cakalang", HargaPerKg = 28000 }
        };

        // Metode
        public static List<JenisIkan> LihatSemuaJenisIkan()
        {
            return daftarIkan;
        }

        public static JenisIkan GetJenisIkanById(int idIkan)
        {
            // Cari ikan di dalam daftar berdasarkan ID
            return daftarIkan.FirstOrDefault(i => i.IDIkan == idIkan);
        }

        public static void TambahIkanBaru(string nama, double harga)
        {
            // Membuat ID baru secara otomatis
            int newId = daftarIkan.Count > 0 ? daftarIkan.Max(i => i.IDIkan) + 1 : 1;
            daftarIkan.Add(new JenisIkan { IDIkan = newId, NamaIkan = nama, HargaPerKg = harga });
        }

        public static bool UbahHargaIkan(int idIkan, double hargaBaru)
        {
            // Cari ikan di dalam daftar berdasarkan ID
            JenisIkan ikan = GetJenisIkanById(idIkan);
            if (ikan != null)
            {
                // Jika ketemu, ubah harganya
                ikan.HargaPerKg = hargaBaru;
                return true;
            }
            // Jika tidak ketemu, kembalikan false
            return false;
        }
    }
}