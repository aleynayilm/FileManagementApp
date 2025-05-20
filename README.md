# FileManagementApp
# CRM Dosya Yönetimi Sistemi

Bu proje, kullanıcıların giriş yaparak kişisel dosyalarını yükleyip yönetebilecekleri bir **CRM tabanlı dosya yönetim sistemidir**. Kullanıcılar yükledikleri dosyaları görüntüleyebilir, indirebilir veya silebilir. Sistem ayrıca kullanıcı bazlı loglama, güvenlik kontrolleri ve dosya türü sınırlamaları gibi profesyonel özellikler sunar.

---

## 🚀 Özellikler

- 👤 Kimlik doğrulama (Email ve parola)
- 📁 Dosya yükleme (PDF, PNG, JPG, JPEG – max 5MB)
- 📄 Dosya listeleme ve silme
- ⬇️ Dosya indirme
- 🔒 Dosya erişim güvenliği
- 🧾 Log kayıt sistemi
- 📅 Yüklenme tarihi kayıtları
- 🎯 Kullanıcı bazlı veritabanı ilişkileri

---

## 🛠️ Kullanılan Teknolojiler

- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQL Server**
- **Bootstrap 5**
- **Identity / Claims tabanlı yetkilendirme**

---

## 📁 Proje Yapısı

- Controllers --> İş mantığı ve yönlendirmeler
- Views --> Razor View sayfaları
- Models --> Entity sınıfları
- Services --> Dosya yönetimi ve iş mantığı
- wwwroot/uploads --> Yüklenen dosyalar

---

## 👤 Kullanıcı Özellikleri

- Sadece giriş yapmış kullanıcılar dosya yükleyebilir ve kendi dosyalarına erişebilir.
- Her dosya, kullanıcı kimliğiyle ilişkilidir.
- Veritabanındaki dosya silinse bile, fiziksel dosya da sistemden silinir.

---

## 🧪 Test Durumları

- Uygunsuz uzantı yükleme: Engellenir.
- 5 MB'dan büyük dosya yükleme: Reddedilir.
- Başka bir kullanıcıya ait dosyaya erişim: Engellenir.
- Giriş yapılmadan dosya yüklemeye çalışma: Reddedilir.

---

# Kurulum Kılavuzu

### 1. Gereksinimler

- Visual Studio 2022 veya üzeri
- .NET 6 / 7 SDK
- SQL Server (Express ya da LocalDB)
- Git

---

### 2. Projeyi Klonlama

```bash
git clone https://github.com/aleynayilm/FileManagementApp.git
cd FileManagementApp
```
---

### 3. Veritabanı Yapılandırması

appsettings.json dosyasındaki bağlantı cümlesini (ConnectionString) kendi SQL Server bilgilerinize göre düzenleyin.
```csharp
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=CrmDosyaDb;Trusted_Connection=True;"
}
```
Migraton ve database oluşturmak için Package Manager Console’da:
```bash
Update-Database
```
---

### 4. Projeyi Başlatma
Visual Studio’dan:

Ctrl + F5 ile projeyi başlatın.
Ana sayfada giriş ekranına yönlendirilirsiniz.

---

### 5. Giriş Bilgileri
İlk kayıt ekranı kullanıcı eklemenize olanak sağlar. Örnek giriş:

Email: test@example.com

Şifre: 12345

---

### 6. Dosya Yükleme
Giriş yaptıktan sonra "Dosya Yükle" sekmesine giderek uygun uzantılarda dosya seçin.

Yüklediğiniz dosyaları "Dosyalarım" sayfasından görebilir, indirebilir veya silebilirsiniz.
