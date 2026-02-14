# Product & Auth Management API 

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

## 🔐 Güvenlik ve Kimlik Doğrulama (JWT)

Bu proje, katmanlar arası güvenliği sağlamak için **JWT (JSON Web Token)** tabanlı bir kimlik doğrulama mekanizması kullanır.

### Nasıl Test Edilir?
1. **Token Alın:** Öncelikle `AuthApi` üzerinden giriş yaparak bir JWT Token edinin.
2. **Swagger Yetkilendirme:** `ProductApi` Swagger arayüzünde sağ üstte bulunan **"Authorize"** butonuna tıklayın.
3. **Bearer Token:** Açılan pencereye kopyaladığınız token değerini yapıştırın.
4. **Erişim:** Artık `[Authorize]` ile korunan ürün ekleme, silme ve listeleme işlemlerini gerçekleştirebilirsiniz. Yetkisiz istekler sistem tarafından **401 Unauthorized** kodu ile reddedilecektir.

> ## 📌 Note: 
.NET 10 Preview sürümü kullanıldığı için OpenAPI yapılandırması `Microsoft.OpenApi.Models` üzerinden özelleştirilmiştir.

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
**Bağlantı Ayarlarını Güncelleyin:**
`appsettings.json` dosyasındaki `ConnectionStrings` ve `Redis` ayarlarının doğruluğundan emin olun.

**Veritabanını Oluşturun:**
Uygulama ilk çalıştığında `EnsureCreated()` ile tabloları otomatik oluşturacaktır. Manuel yapmak isterseniz:

```Bash
dotnet ef database update
```
Projeyi Çalıştırın:

```Bash
dotnet run --project ProductApi
```
### 🔐 Kimlik Doğrulama (Auth)

`/api/Auth/register` ile kullanıcı oluşturun.

`/api/Auth/login` ile token alın.


### 📝 Versiyonlama

Bu proje `test/v1.0.0` branch'i üzerinde geliştirilmiştir.
