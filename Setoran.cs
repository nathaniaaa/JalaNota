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

        private static List<Setoran> daftarSetoran = new List<Setoran>();

        // Metode
        public double HitungTotalHarga(double berat, double hargaPerKg)
        {
            return berat * hargaPerKg;
        }

        public static List<Setoran> LihatSemuaSetoran()
        {
            return daftarSetoran;
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
