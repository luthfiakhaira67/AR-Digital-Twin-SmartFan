# 🌬️ AR Digital Twin - Smart Fan

![Unity](https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white)
![Vuforia](https://img.shields.io/badge/Vuforia-000000?style=for-the-badge&logo=vuforia&logoColor=white)
![MQTT](https://img.shields.io/badge/MQTT-660066?style=for-the-badge&logo=mqtt&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)

Proyek **Augmented Reality Digital Twin** untuk memonitor dan menyimulasikan Kipas Cerdas (*Smart Fan*) secara *real-time* berbasis *Internet of Things* (IoT). Proyek ini dikembangkan sebagai bagian dari tugas Ujian Tengah Semester (UTS) mata kuliah Sistem Intelijen.

---

## 📝 Deskripsi Proyek
Sistem ini merupakan representasi virtual (*Digital Twin*) dari sebuah objek fisik berupa kipas angin. Dengan menggunakan teknologi *Augmented Reality* (AR), pengguna dapat mengarahkan kamera *smartphone* atau laptop ke sebuah *Image Target* (marker), dan model 3D kipas akan muncul di atasnya. 

Kipas virtual ini terhubung ke jaringan IoT menggunakan protokol komunikasi MQTT. Kecepatan putaran baling-baling kipas pada aplikasi AR akan berubah secara otomatis dan *real-time* berdasarkan data suhu lingkungan yang dikirimkan oleh *publisher* (seperti sensor fisik atau simulator Node-RED).

## ✨ Fitur Utama
* **AR Marker Tracking:** Melacak dan memunculkan objek 3D secara stabil menggunakan Vuforia Engine.
* **Integrasi IoT (MQTT):** Terhubung ke *broker* publik (`broker.emqx.io`) untuk menerima aliran data secara instan tanpa *delay*.
* **Logika Otomatisasi Responsif:** Kecepatan putaran kipas beradaptasi langsung dengan kondisi data yang diterima.
* **Indikator Visual:** Memudahkan pengguna memahami kondisi sistem secara sekilas melalui simulasi visual.

## ⚙️ Logika Sistem (Cara Kerja)
Aplikasi bertindak sebagai *Subscriber* yang terus memantau *topic* MQTT tertentu. Ketika ada pembaruan data suhu (*temperature*), sistem akan merespons dengan aturan berikut:

| Kondisi Suhu | Status Kipas 3D | Kecepatan Animasi |
| :--- | :--- | :--- |
| **Suhu $\ge$ 30°C** | 🟢 Menyala (Cepat) | Putaran Maksimal (Cepat) |
| **Suhu $\ge$ 25°C** | 🟡 Menyala (Normal)| Putaran Sedang (Normal) |
| **Suhu < 25°C** | 🔴 Mati | Baling-baling Berhenti |

## 🛠️ Teknologi & Spesifikasi
* **Game Engine:** Unity 3D
* **AR SDK:** Vuforia Engine v11.4.4
* **Protokol IoT:** MQTT 
* **Broker MQTT:** `broker.emqx.io` (Public Broker)
* **Bahasa Pemrograman:** C# (*Scripting* perilaku AR dan MQTT)
* **Testing Tool:** Node-RED / MQTT Explorer (Untuk simulasi *publisher* pengirim data suhu)

## 🚀 Cara Menjalankan Proyek (Instalasi)
Bagi Anda yang ingin mencoba atau mengembangkan ulang *project* ini:

1. **Clone Repositori:**
   ```bash
   git clone [https://github.com/luthfiakhaira67/AR-Digital-Twin-SmartFan.git](https://github.com/luthfiakhaira67/AR-Digital-Twin-SmartFan.git)
