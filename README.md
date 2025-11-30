# 🎣 JalaNota
JalaNota merupakan proyek **Junior Project** berupa pengembangan aplikasi desktop berbasis Windows Presentation Foundation (WPF) yang berfokus pada digitalisasi pencatatan tangkapan nelayan untuk penadah ikan, dirancang untuk membantu pengelolaan data nelayan, jenis ikan, dan setoran hasil tangkapan. Aplikasi ini menggunakan database cloud Supabase untuk penyimpanan data yang aman secara real-time.

---

## Kelompok SOJUN

Ketua Kelompok: KAYANA ANINDYA AZARIA - 23/521475/TK/57528

Anggota 1: EGA BASKARA NUGROHO - 23/521518/TK/57532

Anggota 2: NATHANIA RATNADEWI - 23/522605/TK/57712

## Class Diagram
![JUNPRO-class diagram (2)](https://github.com/user-attachments/assets/5d830220-d55e-492d-9d71-bd8c12e5c07e)

## ERD (Entity Relationship Diagram)
![JUNPRO-Page-9 (1)](https://github.com/user-attachments/assets/5b4fb8ec-222d-4bc4-af61-3e64e7daf5e5)

## Tujuan Aplikasi
- Memudahkan admin penadah ikan dalam mengelola data nelayan dan jenis ikan
- Mencatat setoran hasil tangkapan nelayan secara digital
- Memberikan transparansi harga ikan kepada publik
- Menyediakan riwayat setoran untuk setiap nelayan

## Pengguna Aplikasi
1. Pengunjung Umum: Dapat melihat daftar harga ikan tanpa login
2. Nelayan: Dapat melihat riwayat setoran pribadi setelah login
3. Admin: Memiliki akses penuh untuk mengelola data (operasi CRUD)

## Fitur Utama
### A. Halaman Publik (Tanpa Login)
- Lihat Harga Ikan: Menampilkan daftar harga ikan per kilogram secara real-time dari database
### B. Fitur Nelayan
- Login Nelayan: Autentikasi dengan username dan password
- Riwayat Setoran: Melihat history setoran pribadi dengan detail:
  - Tanggal dan waktu setoran
  - Jenis ikan yang disetor
  - Berat dalam kilogram
  - Total harga setoran
### C. Fitur Admin
- Login Admin: Autentikasi dengan username dan password khusus admin
- Manage Setoran:
  - Tambah setoran baru (pilih nelayan, ikan, input berat)
  - Edit setoran yang sudah ada
  - Hapus setoran
  - Lihat semua setoran dengan filter dan sorting
- Manage Nelayan:
  - Tambah nelayan baru 
  - Edit data nelayan (nama, username, password)
  - Hapus nelayan
  - Validasi password minimal 6 karakter
- Manage Daftar Ikan:
  - Tambah jenis ikan baru
  - Edit harga ikan
  - Hapus ikan
  - Format harga otomatis dengan separator ribuan

## Tech Stack
### Frontend
- Framework: .NET Framework
- UI Framework: Windows Presentation Foundation (WPF)
- Bahasa: C# 
- XAML: Untuk desain UI 
### Backend & Database
- Database: Supabase
- Authentication: Supabase Auth (API Key based)
### Tools
- IDE: Visual Studio 2022
- Version Control: Git + GitHub
- Package Manager: NuGet

## Arsitektur Aplikasi
![arsitektur aplikasi jalanota(1)](https://github.com/user-attachments/assets/aa4a52d4-1cf6-461b-a368-bc5bbe6d4d1a)
