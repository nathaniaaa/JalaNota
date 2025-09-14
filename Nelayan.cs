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
    }
}