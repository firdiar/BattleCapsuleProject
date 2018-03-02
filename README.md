Game ini Mengggunakan Firebase sebagain databasenya dan Photon Unity Network(PUN) sebagai servernya 
, game ini sudah bisa dimainkan ber2 atau lebih player , tapi sebagai tester saya membuat game ini masih bisa dimainkan oleh 1 orang 
, tapi jika dimainkan oleh 1 orang maka player tersebut akan langsung menang setelah beberapa detik Game berjalan

Gambar_Database
https://drive.google.com/file/d/1pQC5w2EEb1wDeUne4rGft0cxy-4wSEzn/view

sebenarnya saya sudah mempersiapkan untuk penggunaan gold gems dan lainnya 
, tetapi saya khawatir tidak cukup waktu sehingga saya megutamakan gameplay terlebih dahulu


Di Game Ini Player Harus Memutar karakter dengan cara memutar HP Milik pengguna 
, Saya Menggunakan Input.compass agar itu bisa terjadi dan Input.acceleration untuk merotasi kamera keatas dan ke bawah


Saat player Di Dalam Arena Player harus keluar Dari atas Melalui 2 Jalan yang ada merah dan biru.
keluar dari merah artinya memilih tim merah begitu pula sebaliknya , pembagian secara adil ,
jika terdapat 4 player maka maks 2 player di dalam sebuah tim , jika 2 orang sudah melompat dan memilih tim ,
maka collider akan aktiv sehingga player tidak dapat melompat melalui jalan tersebut.

Setelah Player Keluar Makan Player Harus Mencari Musuh di dalam Maze , untuk memudahkan ,
saya menambahkan map di sebelah kanan atas , tetapi hanya dapat menampilkan tim kita dan player kita sendiri saja.
