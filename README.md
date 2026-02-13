# Product & Auth Management API (Task 2)

Bu proje, modern yazılım mimarileri ve tasarım desenleri kullanılarak geliştirilmiş, JWT tabanlı kimlik doğrulama ve Redis önbellekleme mekanizmalarına sahip bir Ürün Yönetim API'sidir.

## 🏗️ Mimari Yapı: Onion Architecture
Projede bağımlılıkların merkeze (Core) doğru olduğu, katmanlı bir mimari uygulanmıştır:
- **Core:** Entity'ler ve temel arayüzler.
- **Application:** CQRS Handler'ları, DTO'lar ve iş mantığı (Business Logic).
- **Infrastructure:** Veritabanı (EF Core, PostgreSQL) ve Önbellek (Redis) yapılandırmaları.
- **API:** Controller'lar ve Middleware yönetimi.

## 🚀 Kullanılan Teknolojiler & Desenler
- **.NET 8 (Web API):** Ana framework.
- **PostgreSQL:** İlişkisel veritabanı.
- **Redis:** Dağıtık önbellekleme (Distributed Caching).
- **MediatR (CQRS):** Komut ve sorguların (Command/Query) birbirinden ayrıştırılması.
- **JWT Authentication:** Güvenli kimlik doğrulama.
- **Entity Framework Core:** ORM aracı.

## ⚡ Caching Stratejisi
- **Cache-Aside Pattern:** Ürün listeleme ve tekil ürün sorgularında önce Redis kontrol edilir.
- **Cache Invalidation:** Yeni ürün eklendiğinde veya bir ürün silindiğinde, veritabanı ile önbelleğin tutarlılığını korumak adına ilgili cache anahtarları temizlenir.



## 🛠️ Kurulum ve Çalıştırma

### Gereksinimler
- .NET SDK (8.0+)
- Docker (PostgreSQL ve Redis için önerilir)

### Adımlar
1. **Veritabanı ve Redis'i Ayağa Kaldırın:**
   (Eğer Docker kullanıyorsanız)
   ```bash
   docker run --name postgres-db -e POSTGRES_PASSWORD=your_password -p 5432:5432 -d postgres
   docker run --name redis-cache -p 6379:6379 -d redis
Bağlantı Ayarlarını Güncelleyin:
appsettings.json dosyasındaki ConnectionStrings ve Redis ayarlarının doğruluğundan emin olun.

Veritabanını Oluşturun:
Uygulama ilk çalıştığında EnsureCreated() ile tabloları otomatik oluşturacaktır. Manuel yapmak isterseniz:

Bash
dotnet ef database update
Projeyi Çalıştırın:

Bash
dotnet run --project ProductApi
🔐 Kimlik Doğrulama (Auth)

`/api/Auth/register` ile kullanıcı oluşturun.

`/api/Auth/login` ile token alın.


📝 Versiyonlama
Bu proje test/v1.0.0 branch'i üzerinde geliştirilmiştir.
