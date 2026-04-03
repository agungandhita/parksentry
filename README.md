# 🚗 ParkSentry - Parking Violation Management System

ParkSentry adalah aplikasi *Full-Stack* (Web & API) modern untuk mengelola sistem pelanggaran parkir. Sistem ini dirancang dengan arsitektur yang kuat menggunakan **.NET 8 (Clean Architecture)** di sisi *backend* dan **Vue.js 3 (Vite + Composition API)** di sisi *frontend*.

---

## 🚀 Fitur Utama
- **Manajemen Petugas (Officers):** Mengelola data petugas yang bertugas mencatat pelanggaran.
- **Pencatatan Kendaraan & Pelanggaran:** Sistem terpusat untuk merekam tipe kendaraan dan detail tiket tilang/pelanggaran.
- **Kategori Pelanggaran (Violation Types):** Konfigurasi tipe-tipe pelanggaran parkir secara dinamis.
- **Frontend Modern & Dark Mode:** UI responsif, cepat, dan dibekali dengan mode gelap untuk penggunaan optimal.
- **Arsitektur Skalabel:** Menggunakan prinsip *Clean Architecture* dan *Object-Oriented Programming* berskala Enterprise.

---

## 🛠️ Teknologi yang Digunakan

### Backend (API)
- **Framework:** .NET 8 Web API
- **Bahasa:** C#
- **Arsitektur:** Clean Architecture (API, Application, Domain, Infrastructure)
- **ORM:** Entity Framework Core
- **Database:** PostgreSQL

### Frontend (Web)
- **Framework:** Vue.js 3
- **Build Tool:** Vite
- **HTTP Client:** Axios
- **State Management:** Pinia (opsional/sesuai preferensi)
- **Styling:** CSS + Dark Mode Support

---

## 💻 Prasyarat Instalasi (Prerequisites)

Sebelum menjalankan aplikasi ini, pastikan Anda telah menginstal perangkat lunak berikut sesuai dengan sistem operasi laptop Anda (Windows / Mac):

1. **.NET 8.0 SDK**: [Download di sini](https://dotnet.microsoft.com/en-us/download)
2. **Node.js (LTS)**: [Download di sini](https://nodejs.org/en/) untuk menjalankan *frontend* Vue.
3. **Database Server (PostgreSQL)**:
   - **Windows:** Bisa diinstal secara natif dari [PostgreSQL.org](https://www.postgresql.org/download/windows/) atau menggunakan Docker.
   - **Mac:** Disarankan menggunakan [Docker Desktop](https://www.docker.com/products/docker-desktop/) (menjalankan *image* postgres) atau melalui aplikasi seperti *Postgres.app*.
4. **Alat Pengelola Database (Client/IDE)**:
   - **DBeaver** atau **pgAdmin** (Dapat digunakan di Windows dan Mac).
   - *Azure Data Studio* / *DataGrip* (opsional).
5. **Code Editor / IDE**: 
   - **Windows:** Visual Studio 2022 Community / Visual Studio Code
   - **Mac:** Visual Studio Code (dengan ekstensi *C# Dev Kit*) atau JetBrains Rider.

---

## ⚙️ Cara Menjalankan Aplikasi (Local Development)

### 1. Persiapan Database
1. Buka aplikasi pengelola database (seperti pgAdmin atau DBeaver).
2. Buat database baru dengan nama `ParkSentryDb` (atau sesuai konfigurasi di backend).
3. Buka *Visual Studio* atau *VS Code*. Arahkan ke file konfigurasi backend `ParkSentry.API/appsettings.json`, dan pastikan `ConnectionStrings:DefaultConnection` mengarah ke kredensial PostgreSQL lokal Anda.

Contoh `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=ParkSentryDb;Username=postgres;Password=password_anda"
}
```

### 2. Menjalankan Backend (.NET 8 API)
1. Buka **Terminal / Command Prompt**.
2. Arahkan direktori terminal ke dalam folder backend:
   ```bash
   cd ParkSentry.API
   ```
3. Lakukan migrasi skema database (Entity Framework):
   ```bash
   dotnet ef database update
   ```
4. Jalankan aplikasi web API:
   ```bash
   dotnet run
   ```
   > API akan berjalan di alamat *localhost* (contoh: `http://localhost:5xxx` atau `https://localhost:7xxx`). Anda bisa melihat dokumentasi _endpoint_ di Swagger `/swagger`.

### 3. Menjalankan Frontend (Vue.js 3 + Vite)
1. Buka jendela **Terminal baru**.
2. Arahkan spesifik ke dalam direktori frontend:
   ```bash
   cd parksentry-web
   ```
3. Instal dependencies/paket Node.js (.npm):
   ```bash
   npm install
   ```
4. Jalankan *development server*:
   ```bash
   npm run dev
   ```
   > UI akan terbuka, umumnya di `http://localhost:5173/`. Pastikan konfigurasi URL backend (Axios global/base URL) di *file* *frontend* sudah menunjuk ke port URL Backend (.NET) Anda.

---

## 🤝 Berkontribusi
Jika ingin berkontribusi, silakan buat _branch_ baru, rancang *commit message* yang mendeskripsikan secara jelas perubahan Anda, dan buka _Pull Request_!

## 📜 Lisensi
Aplikasi ini dikembangkan untuk kebutuhan manajemen sistem parkir dan tunduk pada kebijakan lisensi yang berlaku khusus untuk pengguna (bisa ditambahkan lisensi spesifik MIT/GPL jika sepenuhnya _open source_).
