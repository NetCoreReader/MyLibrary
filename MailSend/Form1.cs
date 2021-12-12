using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace MailSend
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnExchangeKontrol_Click(object sender, EventArgs e)
        {
            try
            {
                Encoding encode = Encoding.GetEncoding("windows-1254");
                MailMessage Email = new MailMessage();
                MailAddress MailFrom = new MailAddress(txtE_GonderenMail.Text, txtE_GonderenAdi.Text, encode);
                MailAddress MailTO = new MailAddress(txtE_GonderilenMail.Text, txtE_GonderilenKisi.Text, encode);
                Email.To.Add(MailTO);
                Email.From = MailFrom;
                Email.Priority = MailPriority.High;
                Email.Subject = txtE_Konusu.Text;
                Email.Body = txtE_Icerigi.Text;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Email.Body, null, "text/html");
                Email.AlternateViews.Add(htmlView);
                Email.IsBodyHtml = true;
                SmtpClient SmtpMail = new SmtpClient(txtE_Server.Text, Convert.ToInt32(txtE_Port.Text));
                SmtpMail.Credentials = new System.Net.NetworkCredential(txtE_KullaniciAdi.Text, txtE_Sifresi.Text);
                SmtpMail.EnableSsl = false;
                SmtpMail.Send(Email);
            }
            catch (Exception ex)
            {
                txtE_MailCevabi.Text = "Mail Gönderim Hatası: " + ex.Message;
            }
        }

        private void btnPop3Kontrol_Click(object sender, EventArgs e)
        {
            MailAddress address = new MailAddress(txtP_GonderenMail.Text, txtP_GonderenAdi.Text);
            MailMessage message = new MailMessage
            {
                From = address,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(txtP_AlacakMail.Text, txtP_AlacakKisi.Text));
            message.Subject = txtP_Konusu.Text;
            message.Body = txtP_Icerigi.Text;
            message.Priority = MailPriority.High;
            NetworkCredential credential = new NetworkCredential(txtP_KullaniciAdi.Text, txtP_KullaniciSifresi.Text);
            SmtpClient client = new SmtpClient
            {
                Host = txtP_Host.Text,
                Port = Convert.ToInt32(txtP_Port.Text),
                UseDefaultCredentials = false,
                Credentials = credential,
                EnableSsl = chkP_SSL.Checked,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            try
            {
                if (chkGuvenlik.Checked)
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                client.Send(message);
                txtP_MailCevabi.Text = "Mail Gönderildi....";
                try
                {
                    message.Dispose();
                }
                catch (Exception ex)
                {
                    txtP_MailCevabi.Text = "Mail Dispose: " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                txtP_MailCevabi.Text = "Mail Gönderim Hatası: " + ex.Message;
            }
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
