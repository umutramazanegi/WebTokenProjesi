using System; // Temel sistem fonksiyonları için gerekli kütüphane.
using System.Collections.Generic; // Koleksiyon sınıfları (List, Dictionary vb.) için kütüphane.
using System.ComponentModel; // Bileşen tabanlı programlama özellikleri için kütüphane.
using System.Data; // Veri erişimi ve yönetimi için sınıfları içerir (DataTable vb.).
using System.Data.SqlClient; // SQL Server veritabanı işlemleri için kütüphane.
using System.Drawing; // Grafiksel işlemler (renk, font vb.) için kütüphane.
using System.IdentityModel.Tokens.Jwt; // JWT (JSON Web Token) işleme ve doğrulaması için kütüphane.
using System.Linq; // LINQ sorguları (veri sorgulama) için kütüphane.
using System.Text; // Metin işleme (StringBuilder vb.) için kütüphane.
using System.Threading.Tasks; // Asenkron programlama modelleri için kütüphane.
using System.Windows.Forms; // Windows Forms uygulamaları (formlar, butonlar vb.) için kütüphane.
using WebTokenProjesi.JWT; // Proje içindeki JWT (Token işlemleri) klasörünü kullanır.

namespace WebTokenProjesi // Projenin ana isim alanı (namespace).
{
    public partial class FrmEmployee : Form // FrmEmployee adında bir form sınıfı, Form sınıfından türetilmiş.
    {
        public FrmEmployee() // FrmEmployee sınıfının kurucu metodu (form ilk oluşturulduğunda çalışır).
        {
            InitializeComponent(); // Form üzerindeki kontrolleri (buton, textbox vb.) başlatan metot (tasarımcı tarafından oluşturulur).
        }
        public string tokenGet; // Giriş formundan gelen JWT token'ını saklamak için public değişken.
        // SQL Server veritabanı bağlantı nesnesi oluşturuluyor ve bağlantı bilgileri (connection string) tanımlanıyor.
        SqlConnection sqlConnection = new SqlConnection("Server=UMUT\\SQLEXPRESS;initial catalog=WebTokenProjesiDB;integrated security=true");

        private void FrmEmployee_Load(object sender, EventArgs e) // Form yüklendiğinde çalışacak olay metodu.
        {
            TokenValidator tokenValidator = new TokenValidator(); // Token doğrulaması yapmak için TokenValidator sınıfından bir nesne oluşturuluyor.

            richTextBox1.Text = tokenGet; // Gelen token'ı ekrandaki richTextBox1 kontrolünde gösterir (genellikle test amaçlı).

            // Gelen token'ı doğrular ve doğrulanmış kimlik bilgilerini (claims principal) alır.
            var principal = tokenValidator.ValidateJwtToken(tokenGet);

            if (principal != null) // Eğer token başarılı bir şekilde doğrulandıysa (principal null değilse).
            {
                // Token içerisinden 'sub' (subject - genellikle kullanıcı adı) claim'ini alır. Null ise Value'ya erişmez (?. operatörü).
                string username = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                MessageBox.Show("Hoş geldiniz: " + username); // Kullanıcıya hoş geldin mesajı gösterir.

                sqlConnection.Open(); // Veritabanı bağlantısını açar.
                // SQL komutu nesnesi oluşturuluyor: TblEmployee tablosundaki tüm verileri seçer.
                SqlCommand command = new SqlCommand("Select * From TblEmployee", sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(command); // Verileri veritabanından çekip DataTable'a doldurmak için adaptör oluşturur.
                DataTable dataTable = new DataTable(); // Verileri tutmak için boş bir DataTable nesnesi oluşturur.
                adapter.Fill(dataTable); // Adaptör aracılığıyla veritabanından gelen verileri DataTable'a doldurur.
                dataGridView1.DataSource = dataTable; // DataTable'daki verileri dataGridView1 kontrolünde gösterir.
                sqlConnection.Close(); // Veritabanı bağlantısını kapatır.
            }
            else // Eğer token doğrulanamadıysa (principal null ise).
            {
                MessageBox.Show("Geçersiz Token!"); // Kullanıcıya geçersiz token mesajı gösterir.
                // Opsiyonel: Formu kapatabilir veya kullanıcıyı giriş ekranına yönlendirebilirsiniz.
                // this.Close();
            }
        }
    }
}