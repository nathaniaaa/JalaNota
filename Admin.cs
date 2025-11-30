using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JalaNota
{
    [Table("Admin")]
    public class Admin : BaseModel
    {
        [PrimaryKey("IDAdmin")]
        public int IDAdmin { get; set; }

        [Column("NamaAdmin")]
        public string NamaAdmin { get; set; }

        [Column("UsernameAdmin")]
        public string UsernameAdmin { get; set; }

        [Column("PasswordAdmin")]
        public string PasswordAdmin { get; set; }

        public static async Task<Admin> Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    return null;

                // Hapus semua spasi dari username
                username = username.Replace(" ", "");

                var client = SupabaseClient.Instance;

                // Add debugging
                System.Diagnostics.Debug.WriteLine($"Attempting login for: {username}");

                var response = await client.From<Admin>()
                    .Where(a => a.UsernameAdmin == username && a.PasswordAdmin == password)
                    .Get();

                System.Diagnostics.Debug.WriteLine($"Response count: {response.Models?.Count ?? 0}");

                return response.Models?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }
    }
}