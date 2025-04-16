using System; // Temel sistem fonksiyonları için gerekli kütüphane.
using System.Collections.Generic; // Koleksiyon sınıfları (List, Dictionary vb.) için kütüphane.
using System.ComponentModel; // Bileşen tabanlı programlama özellikleri için kütüphane.
using System.Data; // Veri erişimi ve yönetimi için sınıfları içerir (DataTable vb.).
using System.Data.SqlClient; // SQL Server veritabanı işlemleri için kütüphane.
using System.Drawing; // Grafiksel işlemler (renk, font vb.) için kütüphane.
using System.Linq; // LINQ sorguları (veri sorgulama) için kütüphane.
using System.Text; // Metin işleme (StringBuilder vb.) için kütüphane.
using System.Threading.Tasks; // Asenkron programlama modelleri için kütüphane.
using System.Windows.Forms; // Windows Forms uygulamaları (formlar, butonlar vb.) için kütüphane.
using WebTokenProjesi.JWT; // Proje içindeki JWT (Token işlemleri) klasörünü kullanır.

namespace WebTokenProjesi // Projenin ana isim alanı (namespace).
{
    public partial class FrmLogin : Form // FrmLogin adında bir form sınıfı, Form sınıfından türetilmiş. 'partial' anahtar kelimesi sınıfın başka dosyalarda da tanımlanabileceğini belirtir (genellikle tasarımcı kodu için).
    {
        public FrmLogin() // FrmLogin sınıfının kurucu metodu (form ilk oluşturulduğunda çalışır).
        {
            InitializeComponent(); // Form üzerindeki kontrolleri (buton, textbox vb.) başlatan metot (tasarımcı tarafından otomatik oluşturulur).
        }
        // SQL Server veritabanı bağlantı nesnesi oluşturuluyor ve bağlantı bilgileri (connection string) tanımlanıyor.
        SqlConnection sqlConnection = new SqlConnection("Server=UMUT\\SQLEXPRESS;initial catalog=WebTokenProjesiDB;integrated security=true");

        // btnLogin isimli butona tıklandığında çalışacak olay (event) metodu.
        private void btnLogin_Click(object sender, EventArgs e)
        {
            TokenGenerator tokenGenerator = new TokenGenerator(); // Token oluşturmak için TokenGenerator sınıfından bir nesne (instance) yaratılıyor.

            sqlConnection.Open(); // Veritabanı bağlantısını açar. Bağlantı açma/kapama işlemleri genellikle using bloğu ile yapılırsa daha güvenlidir.
            // SQL komutu nesnesi oluşturuluyor: Kullanıcı adı ve şifreye göre TblUser tablosunda eşleşen kaydı seçer. Parametreli sorgu SQL Injection'a karşı koruma sağlar.
            SqlCommand command = new SqlCommand("Select * From TblUser Where Username=@username and Password=@password", sqlConnection);
            command.Parameters.AddWithValue("@username", txtUserName.Text); // SQL sorgusundaki @username parametresine txtUserName textbox'ının içeriğini (Text özelliğini) atar.
            command.Parameters.AddWithValue("@password", txtPassword.Text); // SQL sorgusundaki @password parametresine txtPassword textbox'ının içeriğini atar.
            SqlDataReader sqlDataReader = command.ExecuteReader(); // SQL komutunu çalıştırır ve dönen sonuçları satır satır okumak için bir SqlDataReader nesnesi döndürür.

            if (sqlDataReader.Read()) // Eğer SqlDataReader en az bir satır okuyabildiyse (yani girilen kullanıcı adı ve şifreyle eşleşen bir kayıt bulunduysa).
            {
                // Kullanıcı adı kullanılarak bir JWT (JSON Web Token) oluşturulur.
                string token = tokenGenerator.GenerateJwtToken2(txtUserName.Text);
                //MessageBox.Show(token); // Oluşturulan token'ı bir mesaj kutusunda gösterir (bu satır şu anda yorum satırı olarak devre dışı).
                FrmEmployee frm = new FrmEmployee(); // FrmEmployee isimli diğer formdan yeni bir nesne oluşturur.
                frm.tokenGet = token; // Oluşturulan token değerini, FrmEmployee formundaki public tokenGet değişkenine aktarır.
                frm.Show(); // FrmEmployee formunu kullanıcıya gösterir.
                // this.Hide(); // İsteğe bağlı: Giriş formunu gizlemek için bu satırın yorumunu kaldırabilirsiniz.
            }
            else // Eğer SqlDataReader herhangi bir satır okuyamadıysa (kullanıcı adı veya şifre yanlışsa).
            {
                MessageBox.Show("Hatalı Kullanıcı Adı ya da Şifre"); // Kullanıcıya bir hata mesajı gösterir.
                txtPassword.Clear(); // Şifre giriş alanının (textbox) içeriğini temizler.
                txtUserName.Clear(); // Kullanıcı adı giriş alanının (textbox) içeriğini temizler.
                txtUserName.Focus(); // Kullanıcının tekrar giriş yapmasını kolaylaştırmak için imleci (kürsörü) kullanıcı adı giriş alanına odaklar.
            }
            sqlConnection.Close(); // Veritabanı bağlantısını kapatır. Hata oluşsa bile kapanması için try-finally bloğu veya using ifadesi kullanmak daha iyidir.
        }

        // FrmLogin formu yüklendiğinde çalışacak olay metodu.
        private void FrmLogin_Load(object sender, EventArgs e)
        {
            // Bu metodun içi şu anda boş, yani form yüklendiğinde özel bir işlem yapılmıyor.
            // Buraya form ilk açıldığında yapılması gereken kodlar eklenebilir (örn: varsayılan değer atama).
        }
    }
}