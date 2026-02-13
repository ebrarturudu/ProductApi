# Microservices Architecture & Gateway Implementation

Bu proje, modern yazılım mimarisi prensipleriyle (Onion Architecture) geliştirilmiş; kimlik doğrulama, ürün yönetimi ve merkezi bir API Gateway yapısını içeren kapsamlı bir .NET ekosistemidir.

## 🏗️ Mimari Yapı ve Katmanlar
Proje, **Onion Architecture** üzerine inşa edilmiştir:
- **Core:** Entity'ler ve temel arayüzler.
- **Application:** CQRS (MediatR), DTO'lar ve Validation kuralları.
- **Infrastructure:** Veritabanı erişimi (EF Core), Redis Cache ve Middleware'ler.
- **WebAPI:** Endpoint'ler ve servis kayıtları.

---

## 🚀 Öne Çıkan Özellikler

### 1. API Gateway (YARP)
Tüm servis trafiği merkezi bir Gateway üzerinden yönetilir. 
- **Port:** `localhost:5000` üzerinden tüm API'lere erişim sağlanır.
- **Reverse Proxy:** İstekleri ilgili mikroservislere yönlendirir.

### 2. Güvenlik ve Hız Sınırı (Rate Limiting)
- Gateway üzerinde **Fixed Window** politikası uygulanmıştır.
- IP tabanlı olarak dakikada maksimum 10 isteğe izin verilir. Sınır aşıldığında `503 Service Unavailable` döner.

### 3. Otomatik Doğrulama (Validation Pipeline)
- **FluentValidation** kullanılarak giriş verileri denetlenir.
- **MediatR Pipeline Behavior** sayesinde, veriler henüz Handler'a ulaşmadan otomatik olarak doğrulanır.

### 4. Merkezi Hata Yönetimi (Global Exception Handling)
- Özel bir **Middleware** ile tüm sistem hataları yakalanır.
- Kullanıcıya karmaşık hata ekranları yerine, standart ve anlaşılır JSON formatında (ErrorResponse) yanıt dönülür.

### 5. Performans (Caching)
- **Redis (Distributed Cache)** entegrasyonu ile sık erişilen veriler önbelleğe alınır.

---

## 💻 Kurulum ve Çalıştırma

1. **Docker Servislerini Başlatın:**
 ```bash
 docker-compose up -d
  ```

2. **Veritabanı Migration:**
```bash
dotnet ef database update --project ProductApi
```

3. **Uygulamayı Başlatın:**
Önce `ProductApi` ve `AuthApi` servislerini, ardından `ApiGateway` projesini çalıştırın:
```bash
dotnet run --project ApiGateway
```
🧪 Test Adımları
1. **Doğrulama Testi:** 
`POST /api/Products` endpoint'ine geçersiz veri (örn: fiyat -5) göndererek `400 Bad Request` sonucunu doğrulayın.

2. **Sınır Testi:**
Gateway üzerinden art arda 10'dan fazla istek atarak Rate Limit mekanizmasını test edin.
