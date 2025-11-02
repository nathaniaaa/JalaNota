using Supabase;
using System;

namespace JalaNota
{
    public static class SupabaseClient
    {
        public static Client Instance { get; private set; }

        public static void Initialize(string supabaseUrl, string supabaseApiKey)
        {
            if (Instance == null)
            {
                var options = new SupabaseOptions
                {
                    AutoConnectRealtime = true 
                };

                // inisialisasi klien
                Instance = new Client(supabaseUrl, supabaseApiKey, options);
            }
        }
    }
}