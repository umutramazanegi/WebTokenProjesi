using System; // Temel sistem fonksiyonları için gerekli kütüphane.
using System.Collections.Generic; // Koleksiyon sınıfları için kütüphane.
using System.ComponentModel; // Bileşen tabanlı programlama özellikleri için kütüphane.
using System.Data; // Veri erişimi ve yönetimi için sınıfları içerir.
using System.Drawing; // Grafiksel işlemler (renk, font vb.) için kütüphane.
using System.Linq; // LINQ sorguları (veri sorgulama) için kütüphane.
using System.Text; // Metin işleme (StringBuilder vb.) için kütüphane.
using System.Threading.Tasks; // Asenkron programlama modelleri için kütüphane.
using System.Windows.Forms; // Windows Forms uygulamaları (formlar, butonlar vb.) için kütüphane.
using WebTokenProjesi.JWT; // Proje içindeki JWT (Token işlemleri) klasörünü kullanır.

namespace WebTokenProjesi // Projenin ana isim alanı (namespace).
{
    public partial class FrmTokenGenerator : Form // FrmTokenGenerator adında bir form sınıfı, Form sınıfından türetilmiş.
    {
        public FrmTokenGenerator() // FrmTokenGenerator sınıfının kurucu metodu (form ilk oluşturulduğunda çalışır).
        {
            InitializeComponent(); // Form üzerindeki kontrolleri (buton, textbox vb.) başlatan metot (tasarımcı tarafından oluşturulur).
        }

        // btnCreateToken isimli butona tıklandığında çalışacak olay (event) metodu.
        private void btnCreateToken_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text; // txtUserName textbox'ındaki metni 'username' değişkenine atar.
            string email = txtEmail.Text; // txtEmail textbox'ındaki metni 'email' değişkenine atar.
            string name = txtName.Text; // txtName textbox'ındaki metni 'name' değişkenine atar.
            string surname = txtSurname.Text; // txtSurname textbox'ındaki metni 'surname' değişkenine atar.
            TokenGenerator tokenGenerator = new TokenGenerator(); // Token oluşturmak için TokenGenerator sınıfından bir nesne yaratır.
            // TokenGenerator nesnesinin GenerateJwtToken metodunu çağırarak, textbox'lardan alınan bilgilerle bir JWT oluşturur.
            string token = tokenGenerator.GenerateJwtToken(username, email, name, surname);
            richTextBox1.Text = token; // Oluşturulan token'ı richTextBox1 kontrolünde gösterir.
        }

        // FrmTokenGenerator formu yüklendiğinde çalışacak olay metodu.
        private void FrmTokenGenerator_Load(object sender, EventArgs e)
        {
            // Bu metodun içi şu anda boş, yani form yüklendiğinde özel bir işlem yapılmıyor.
        }
    }
}