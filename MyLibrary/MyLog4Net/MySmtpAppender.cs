using log4net.Appender;
using System;

namespace MyLibrary.MyLog4Net
{
    public class clsSmtpAppenderConfig
    {
        /// <summary>
        /// Maili Gönderenin Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Maili Gönderen Kullanıcı Adı
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Maili Gönderen Kullanıcı Şifresi
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Maili Gönderen Smtp Hostu
        /// </summary>
        public string SmtpHost { get; set; }
        /// <summary>
        /// Bakılacak
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// Maili Alacak Kişi
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// Maili Bilgilendirilecek Kişi
        /// </summary>
        public string Cc { get; set; }
        /// <summary>
        /// Maili Gizli Alacak Kişi
        /// </summary>
        public string Bcc { get; set; }
        /// <summary>
        /// Mail ServisinSsl Bilgisi
        /// </summary>
        public bool Ssl { get; set; }
        /// <summary>
        /// Mail Portu
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Güvenlik Bilgisi
        /// </summary>
        public Auth _Auth { get; set; }
        internal SmtpAppender.SmtpAuthentication Authentication
        {
            get
            {
                switch (_Auth)
                {
                    case Auth.Basic:
                        return SmtpAppender.SmtpAuthentication.Basic;
                    case Auth.None:
                        return SmtpAppender.SmtpAuthentication.None;
                    case Auth.Ntlm:
                        return SmtpAppender.SmtpAuthentication.Ntlm;
                    default:
                        return SmtpAppender.SmtpAuthentication.Basic;
                }
            }
        }
        /// <summary>
        /// Mailin Konusu
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Default 512
        /// </summary>
        public int BufferSize { get; set; }
        /// <summary>
        /// Bilinmiyor Default true
        /// </summary>
        public bool Lossy { get; set; }
        /// <summary>
        /// Mail Önceliği
        /// </summary>
        public System.Net.Mail.MailPriority Priority { get; set; }
    }

    public enum Auth
    {
        Basic,
        None,
        Ntlm
    }
    internal class MySmtpAppender
    {
        private static Lazy<MySmtpAppender> lazy = new Lazy<MySmtpAppender>(() => new MySmtpAppender());

        internal static MySmtpAppender Instance { get { return lazy.Value; } }

        private MySmtpAppender()
        {

        }
        internal SmtpAppender _smtpAppender { get; set; }
        internal clsSmtpAppenderConfig Config { get; set; }
        internal SmtpAppender GetSmtpAppender()
        {
            _smtpAppender = new SmtpAppender()
            {
                Layout = Helper.Instance.GetPatternLayout(),
                Name = "SmtpAppender",
                Threshold = Helper.Instance._logLevel,
                Authentication = Config.Authentication,
                EnableSsl = Config.Ssl,
                From = Config.From,
                To = Config.To,
                Lossy = Config.Lossy,
                Username = Config.UserName,
                Password = Config.Password,
                Priority = Config.Priority,
                Bcc = Config.Bcc,
                Cc = Config.Cc,
                Port = Config.Port,
                Subject = Config.Subject,
                SmtpHost = Config.SmtpHost,
                BufferSize = Config.BufferSize, 
            };
            _smtpAppender.ActivateOptions();
            Helper.Instance.SetConfigure(_smtpAppender);
            return _smtpAppender;
        }
    }
}
