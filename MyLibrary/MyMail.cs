using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MyLibrary
{
    public sealed class MyMail
    {
        private static readonly Lazy<MyMail> lazy = new Lazy<MyMail>(() => new MyMail());

        public static MyMail Instance { get { return lazy.Value; } }

        private MyMail()
        {

        }

        #region MailParametreleri
        public sealed class MyMailParametreleri
        {
            public List<MyMailiAlacaklar> BccGonderiListesi { get; set; }
            public List<MyMailiAlacaklar> CcGonderiListesi { get; set; }
            public string GonderenIsim { get; set; }
            public string GonderenMail { get; set; }
            public List<string> GonderilecekDosyalar { get; set; }
            public bool GuvenlikIptal { get; set; }
            public string mailHostu { get; set; }
            public string MailiGonderecekMailinAdi { get; set; }
            public string MailiGonderecekMailinSifresi { get; set; }
            public string MailKonusu { get; set; }
            public string MailMesaji { get; set; }
            public MailPriority Onceligi { get; set; }
            public int Port { get; set; }
            public bool SSL_ID { get; set; }
            public bool IsBodyHTML { get; set; }
            public List<MyMailiAlacaklar> ToGonderiListesi { get; set; }
        }
        public sealed class MyMailiAlacaklar
        {
            public string KisiAdi { get; set; }
            public string MailAdresi { get; set; }
        }
        #endregion

        private bool mailKontrol(MyMailParametreleri parametre, out string _msg)
        {
            if (parametre.ToGonderiListesi == null)
            {
                _msg = "Mailin Alacak Hesabını (To) Doldurun.";
                return false;
            }
            else if (String.IsNullOrEmpty(parametre.mailHostu) || String.IsNullOrWhiteSpace(parametre.mailHostu))
            {
                _msg = "Host Bilgisini Doldurun.";
                return false;
            }
            else if (parametre.Port == 0)
            {
                _msg = "Port Bilgisini Doldurun.";
                return false;
            }
            else
            {
                _msg = string.Empty;
                return true;
            }
        }

        public bool MailSend(MyMailParametreleri param, ref string msg)
        {
            if (mailKontrol(param, out msg))
            {
                MailAddress address = new MailAddress(param.GonderenMail, param.GonderenIsim);
                MailMessage message = new MailMessage
                {
                    From = address,
                    IsBodyHtml = param.IsBodyHTML
                };
                if (param.BccGonderiListesi == null) param.BccGonderiListesi = new List<MyMailiAlacaklar>();
                if (param.CcGonderiListesi == null) param.CcGonderiListesi = new List<MyMailiAlacaklar>();


                foreach (MyMailiAlacaklar alacaklar in param.ToGonderiListesi)
                    message.To.Add(new MailAddress(alacaklar.MailAdresi, alacaklar.KisiAdi));
                foreach (MyMailiAlacaklar alacaklar2 in param.BccGonderiListesi)
                    message.Bcc.Add(new MailAddress(alacaklar2.MailAdresi, alacaklar2.KisiAdi));
                foreach (MyMailiAlacaklar alacaklar3 in param.CcGonderiListesi)
                    message.CC.Add(new MailAddress(alacaklar3.MailAdresi, alacaklar3.KisiAdi));
                message.Subject = param.MailKonusu._ToString();
                message.Body = param.MailMesaji._ToString();
                message.Priority = param.Onceligi;
                NetworkCredential credential = new NetworkCredential(param.MailiGonderecekMailinAdi, param.MailiGonderecekMailinSifresi);
                SmtpClient client = new SmtpClient
                {
                    Host = param.mailHostu,
                    Port = param.Port,
                    UseDefaultCredentials = false,
                    Credentials = credential,
                    EnableSsl = param.SSL_ID,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                foreach (string str in param.GonderilecekDosyalar)
                {
                    message.Attachments.Add(new Attachment(str));
                }
                try
                {
                    if (param.GuvenlikIptal)
                        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    client.Send(message);
                    msg = "Success";
                    try
                    {
                        message.Attachments.Clear();
                        message.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                    return true;
                }
                catch (Exception exception)
                {
                    msg = exception.Message;
                    return false;
                }
            }
            else
                return false;
        }
    }
}
