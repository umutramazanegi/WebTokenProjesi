using Microsoft.IdentityModel.Tokens; // Güvenlik tokenları ve doğrulama sınıfları için kütüphane.
using System; // Temel sistem sınıfları için kütüphane.
using System.Collections.Generic; // Koleksiyon sınıfları için kütüphane.
using System.IdentityModel.Tokens.Jwt; // JWT (JSON Web Token) işleme ve doğrulaması için kütüphane.
using System.Linq; // LINQ sorguları için kütüphane.
using System.Security.Claims; // Talep (claim) tabanlı kimlik doğrulama sınıfları için kütüphane.
using System.Text; // Metin kodlama (UTF8 gibi) sınıfları için kütüphane.
using System.Threading.Tasks; // Asenkron programlama için kütüphane.

namespace WebTokenProjesi.JWT // Projenin JWT ile ilgili sınıflarının bulunduğu isim alanı (namespace).
{
    public class TokenValidator // Token doğrulama işlemlerini yapan sınıf.
    {
        // Verilen JWT token'ını doğrulayan metot. Başarılı olursa ClaimsPrincipal, olmazsa null döner.
        public ClaimsPrincipal ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler(); // JWT token'larını okumak ve doğrulamak için bir nesne oluşturur.
            // Token'ı imzalamak için kullanılan gizli anahtarı byte dizisine çevirir (UTF8 encoding ile). Token üreten yerdeki anahtarla aynı olmalı.
            var key = Encoding.UTF8.GetBytes("20Derste20ProjeToken+-*/1234tokenJWT");
            try // Doğrulama sırasında oluşabilecek hataları yakalamak için try-catch bloğu.
            {
                // Token'ı belirtilen parametrelerle doğrulamaya çalışır.
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // İmzalama anahtarının doğrulanmasını zorunlu kılar.
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Doğrulama için kullanılacak simetrik güvenlik anahtarını ayarlar.
                    ValidateIssuer = true, // Token'ı yayınlayanın (issuer) doğrulanmasını zorunlu kılar.
                    ValidIssuer = "localhost", // Beklenen yayınlayıcıyı belirtir.
                    ValidateAudience = true, // Token'ın hedef kitlesinin (audience) doğrulanmasını zorunlu kılar.
                    ValidAudience = "localhost", // Beklenen hedef kitleyi belirtir.
                    ValidateLifetime = true, // Token'ın geçerlilik süresinin (ömrünün) doğrulanmasını zorunlu kılar.
                    ClockSkew = TimeSpan.Zero, // Zaman aşımı kontrollerinde izin verilen zaman farkını (sapmayı) sıfıra ayarlar.
                }, out SecurityToken validatedToken); // Başarılı olursa, doğrulanmış güvenlik token'ını dışarı verir.

                return principal; // Doğrulama başarılı olursa, doğrulanmış kimliği temsil eden ClaimsPrincipal'ı döndürür.
            }
            catch (Exception) // Token doğrulama sırasında herhangi bir istisna (hata) oluşursa yakalar (örn: geçersiz imza, süresi dolmuş token).
            {
                return null; // Doğrulama herhangi bir nedenle başarısız olursa null döndürür.
            }
        }
    }
}