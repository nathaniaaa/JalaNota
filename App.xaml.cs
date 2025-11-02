using System.Windows;
using System.Configuration; 

namespace JalaNota
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string supabaseUrl = ConfigurationManager.AppSettings["SupabaseUrl"];
            string supabaseKey = ConfigurationManager.AppSettings["SupabaseKey"];

            // Validasi sederhana
            if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
            {
                MessageBox.Show("Error: Supabase URL atau Key tidak ditemukan.");
                Current.Shutdown();
                return;
            }

            SupabaseClient.Initialize(supabaseUrl, supabaseKey);
        }
    }
}