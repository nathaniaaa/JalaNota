using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JalaNota
{
    [Table("Nelayan")]
    public class Nelayan : BaseModel
    {
        [PrimaryKey("IDNelayan", false)]
        public int IDNelayan { get; set; }

        [Column("NamaNelayan")]
        public string NamaNelayan { get; set; }

        [Column("UsnNelayan")]
        public string UsnNelayan { get; set; }

        [Column("PasswordNelayan")]
        public string PasswordNelayan { get; set; }

        [JsonIgnore]
        public string DisplayName
        {
            get
            {
                return $"{NamaNelayan} ({UsnNelayan})";
            }
        }

        [JsonIgnore]
        public string MaskedPassword
        {
            get
            {
                if (string.IsNullOrEmpty(PasswordNelayan))
                    return "";
                return new string('*', PasswordNelayan.Length);
            }
        }

        #region ENCAPSULATION: Static Methods for Database Operations

        // Encapsulation Method: Login dan Validasi
        public static async Task<Nelayan> Login(string username, string password)
        {
            try
            {
                // Validasi input
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    return null;

                // Hapus semua spasi dari username
                username = username.Replace(" ", "");

                var client = SupabaseClient.Instance;

                // Query ke database dengan filter
                var response = await client.From<Nelayan>()
                    .Where(n => n.UsnNelayan == username && n.PasswordNelayan == password)
                    .Get();

                // Return nelayan pertama jika ditemukan
                return response.Models?.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Encapsulation Method: Ambil data semua nelayan
        public static async Task<List<Nelayan>> LihatSemuaNelayan()
        {
            try
            {
                var client = SupabaseClient.Instance;

                var response = await client.From<Nelayan>()
                    .Select("*")
                    .Order("IDNelayan", Postgrest.Constants.Ordering.Ascending)
                    .Get();

                return response.Models ?? new List<Nelayan>();
            }
            catch (Exception)
            {
                return new List<Nelayan>();
            }
        }

        // Encapsulation Method: Cari nelayan berdasarkan ID
        public static async Task<Nelayan> GetNelayanById(int idNelayan)
        {
            try
            {
                var client = SupabaseClient.Instance;

                var response = await client.From<Nelayan>()
                    .Where(n => n.IDNelayan == idNelayan)
                    .Single();

                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error GetNelayanById: {ex.Message}");
                return null;
            }
        }

        // Encapsulation Method: Tambah Nelayan Baru
        public static async Task<bool> TambahNelayanBaru(string nama, string username, string password)
        {
            try
            {
                // Memastikan data valid sebelum masuk database
                if (string.IsNullOrWhiteSpace(nama))
                    throw new ArgumentException("Nama nelayan tidak boleh kosong.");

                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("Username tidak boleh kosong.");

                if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                    throw new ArgumentException("Password harus minimal 6 karakter.");

                // Hapus semua spasi dari username
                username = username.Replace(" ", "");

                // Cek username duplikat
                if (!await IsUsernameAvailable(username))
                    throw new InvalidOperationException("Username sudah digunakan.");

                var client = SupabaseClient.Instance;

                // Buat objek nelayan baru
                var nelayanBaru = new Nelayan
                {
                    NamaNelayan = nama,
                    UsnNelayan = username,
                    PasswordNelayan = password
                };

                // Insert ke database
                await client.From<Nelayan>().Insert(nelayanBaru);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error TambahNelayanBaru: {ex.Message}");
                return false;
            }
        }

        // Encapsulation Method: Ubah data nelayan - FIXED VERSION
        public static async Task<bool> UbahNelayan(int id, string nama, string username, string password)
        {
            try
            {
                // Memastikan data valid sebelum masuk database
                if (string.IsNullOrWhiteSpace(nama))
                    throw new ArgumentException("Nama nelayan tidak boleh kosong.");

                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("Username tidak boleh kosong.");

                if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                    throw new ArgumentException("Password harus minimal 6 karakter.");

                // Hapus semua spasi dari username
                username = username.Replace(" ", "");

                // Cek username duplikat (kecuali untuk nelayan ini sendiri)
                if (!await IsUsernameAvailable(username, id))
                {
                    System.Diagnostics.Debug.WriteLine($"Username {username} already used by another nelayan");
                    throw new InvalidOperationException("Username sudah digunakan oleh nelayan lain.");
                }

                var client = SupabaseClient.Instance;

                // PERBAIKAN: Update langsung dengan query, tanpa Get terlebih dahulu
                var updateModel = new Nelayan
                {
                    IDNelayan = id,
                    NamaNelayan = nama,
                    UsnNelayan = username,
                    PasswordNelayan = password
                };

                // Update ke database menggunakan metode yang benar
                await client.From<Nelayan>()
                    .Where(n => n.IDNelayan == id)
                    .Update(updateModel);

                System.Diagnostics.Debug.WriteLine($"Successfully updated Nelayan ID {id}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error UbahNelayan: {ex.Message}");
                return false;
            }
        }

        // Encapsulation Method: Hapus nelayan
        public static async Task<bool> HapusNelayan(int id)
        {
            try
            {
                var client = SupabaseClient.Instance;

                // Hapus langsung berdasarkan ID
                await client.From<Nelayan>()
                    .Where(n => n.IDNelayan == id)
                    .Delete();

                System.Diagnostics.Debug.WriteLine($"Successfully deleted Nelayan ID {id}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error HapusNelayan: {ex.Message}");
                return false;
            }
        }

        // Encapsulation Method: Cek ketersediaan username
        public static async Task<bool> IsUsernameAvailable(string username, int? excludeId = null)
        {
            try
            {
                var client = SupabaseClient.Instance;

                var response = await client.From<Nelayan>()
                    .Where(n => n.UsnNelayan == username)
                    .Get();

                var existingNelayan = response.Models;

                // Jika tidak ada nelayan dengan username ini, username available
                if (existingNelayan == null || !existingNelayan.Any())
                {
                    System.Diagnostics.Debug.WriteLine($"Username {username} is available (not found)");
                    return true;
                }

                // Jika ada excludeId, cek apakah semua nelayan dengan username ini adalah nelayan yang dikecualikan
                if (excludeId.HasValue)
                {
                    // Username available jika semua nelayan dengan username ini memiliki ID yang dikecualikan
                    bool isAvailable = existingNelayan.All(n => n.IDNelayan == excludeId.Value);
                    System.Diagnostics.Debug.WriteLine($"Username {username} availability (excluding ID {excludeId.Value}): {isAvailable}");
                    return isAvailable;
                }

                // Jika tidak ada excludeId dan username ditemukan, berarti tidak available
                System.Diagnostics.Debug.WriteLine($"Username {username} is NOT available (already exists)");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error IsUsernameAvailable: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}