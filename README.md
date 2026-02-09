# Product Management API – Task 1

Bu proje, modern yazılım geliştirme prensipleri ve katmanlı mimari yaklaşımı kullanılarak geliştirilmiş bir Ürün Yönetim API’sidir.  
Proje kapsamında veritabanı yönetimi, asenkron programlama ve API dokümantasyonu standartları uygulanmıştır.

---

## 🛠️ Kullanılan Teknolojiler

- **Framework:** ASP.NET Core Web API (.NET)
- **Veritabanı:** PostgreSQL (Docker konteyner üzerinde çalışmaktadır)
- **ORM:** Entity Framework Core
- **Dokümantasyon:** Swagger / OpenAPI
- **Mimari:** Katmanlı Mimari (Models, DTOs, Controllers, Data)

---

## 📌 Teknik Özellikler

- **Asenkron Yapı:**  
  Tüm veritabanı işlemleri `async/await` yapısı kullanılarak asenkron olarak gerçekleştirilmiştir.

- **DTO Kullanımı:**  
  Veri transferi sırasında `ProductDto` kullanılarak model izolasyonu ve veri güvenliği sağlanmıştır.

- **Migration Yönetimi:**  
  Veritabanı şeması Entity Framework Core Migrations aracılığıyla yönetilmektedir.

- **Bağımlılık Enjeksiyonu (Dependency Injection):**  
  `DbContext` ve diğer bağımlılıklar, .NET’in yerleşik Dependency Injection yapısı ile yapılandırılmıştır.

---

## 🚀 Kurulum ve Çalıştırma

### 1. Veritabanı Hazırlığı

Docker üzerinde PostgreSQL servisinin çalıştığından emin olun.  
`appsettings.json` dosyasında bulunan `ConnectionStrings` alanını kendi yerel ayarlarınıza göre düzenleyin.

---

### 2. Uygulamayı Başlatma

Proje klasöründe terminal açarak aşağıdaki komutları çalıştırın:

```bash
# Bağımlılıkları yükle
dotnet restore

# Uygulamayı çalıştır
dotnet run
3. API Testi (Swagger)
Uygulama çalıştıktan sonra, API endpoint’lerini test etmek için tarayıcınızdan aşağıdaki adrese gidin:

http://localhost:5062/swagger
Not: Port numarası ortamınıza göre farklılık gösterebilir. Terminal çıktısını kontrol ediniz.
```

## 📂 Proje Yapısı
- **Controllers:** API endpoint’lerini içerir

- **Models:** Veritabanı entity tanımları

- **DTOs:** Veri transfer nesneleri

- **Data:** DbContext ve veritabanı yapılandırmaları

- **Migrations:** EF Core migration dosyaları
