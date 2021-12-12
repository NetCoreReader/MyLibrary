using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using System;

namespace MyLibrary.MyLog4Net
{
    public class Helper
    {
        private static Lazy<Helper> lazy = new Lazy<Helper>(() => new Helper());

        public static Helper Instance { get { return lazy.Value; } }

        private Helper()
        {

        }
        #region DipNot
        //%date{dd MMM yyyy HH:mm:ss}   ----> Tarih bilgisi ve format ayarı
        //%message                      ----> Mesaj Bilgisi
        //%newline                      ----> Yeni Satır
        //%class                        ----> Hata Gönderilen Clasın Yeri
        //%method                       ----> Hata Gönderilen Methodun Adı
        //%level                        ----> Log Derecesi 
        ////ALL     : Tüm mesajların loglandığı seviyedir.
        ////DEBUG   : Developement aşamasına yönelik loglama seviyesidir.
        ////INFO    : Uygulamanın çalışması sırasında yararlı olabileceğini düşündüğümüz durum bilgilerini loglayabileceğimiz seviyedir.
        ////WARN    : Hata olmayan fakat önemli bir durumun oluştuğunu belirtebileceğimiz seviye.
        ////ERROR   : Hata durumunu belirten seviye. Sistem hala çalışır haldedir.
        ////FATAL   : Uygulamanın sonlanacağını, faaliyet gösteremeyeceğini belirten mesajlar için kullanılacak seviyedir.
        ////OFF     : Hiç bir mesajın loglanmadığı seviyedir.
        ///Logger Seiviyeleri :Fatal > Error > Warn > Info > Debug

        /*
         Appender Türleri
            {
                Layout          = Tanımlanan genel parametredir.
                Name            = Appnder adıdır
                Threshold       = Hangi logger seviyelerinin tutulması isteniyorsa o belirtiliyor. Eğer Warn diye belirtilirse Warn ve Warndan büyük olanlar (Error, Fatal) seviye logları tutacaktır.
                AppendToFile    = Appender'ı File ile özdeşleştirme
                File            = Loglamanın yapılacağı dosya yolu
            }
         ==> ConsoleAppender    => Hataları consol ekranında gösterecektir. Sistem kapatıldığında hata loglarıda silinir
         ==> FileAppender       => Hataları bir file dosyasına yazacaktır.
         Daha fazla Appender için => https://logging.apache.org/log4net/release/sdk/index.html
         */
        #endregion

        #region Field
        private ILog _logger { get; set; }
        private string layout = "%date{dd MMM yyyy HH:mm:ss} - [%thread] - [%logger] - [%class] - [%method] - [%level] %message%newline";
        internal string filePath = "LogFile.log";
        private MyLogType logType = MyLogType.FileAppender;
        internal Level _logLevel = Level.All;
        internal string _cnnString = String.Empty;
        #endregion

        #region Property
        /// <summary>
        /// Log Dosyasının Tutulma Biçimi
        /// Varsayılan: %date{dd MMM yyyy HH:mm:ss} - [%class] - [%method] - [%level] %message%newline
        /// </summary>
        public string Layout { set { layout = value; } }
        public string FileName { set { filePath = value; } }
        public string _CnnString { set { _cnnString = value; } }
        public MyLogType LogType { set { logType = value; } }
        public clsSmtpAppenderConfig SmtpConfig { set {
                MySmtpAppender.Instance.Config = value;
            } }
        #endregion

        #region Private
        private ILog GetLogger(Type type)
        {
            switch (logType)
            {
                case MyLogType.ConsoleAppender:
                    if (MyConsoleAppender.Instance._consoleAppender == null)
                        MyConsoleAppender.Instance._consoleAppender = MyConsoleAppender.Instance.GetConsoleAppender();
                    break;
                case MyLogType.FileAppender:
                    if (MyFileAppender.Instance. _fileAppender == null)
                        MyFileAppender.Instance._fileAppender = MyFileAppender.Instance.GetFileAppender();
                    break;
                case MyLogType.RollingFileAppender:
                    if (MyRollingFileAppender.Instance. _rollingFileAppender == null)
                        MyRollingFileAppender.Instance._rollingFileAppender = MyRollingFileAppender.Instance.GetRollingFileAppender();
                    break;
                case MyLogType.AdoNetAppender:
                    if (MyAdoNetAppender.Instance._adoNetAppender == null)
                        MyAdoNetAppender.Instance._adoNetAppender = MyAdoNetAppender.Instance.GetAdoNetAppender();
                    break;
                case MyLogType.SmtpAppender:
                    if (MySmtpAppender.Instance._smtpAppender == null)
                        MySmtpAppender.Instance._smtpAppender = MySmtpAppender.Instance.GetSmtpAppender();
                    break;
            }
            if (_logger != null)
                return _logger;
            return LogManager.GetLogger(type);
        }
        /// <summary>
        /// Loglama üst sınırı
        /// Fatal > Error > Warn > Info > Debug > All
        /// </summary>
        private MyLogLevelCategory LogLevel
        {
            set
            {
                switch (value)
                {
                    case MyLogLevelCategory.All:
                        _logLevel = Level.All;
                        break;
                    case MyLogLevelCategory.Fatal:
                        _logLevel = Level.Fatal;
                        break;
                    case MyLogLevelCategory.Error:
                        _logLevel = Level.Error;
                        break;
                    case MyLogLevelCategory.Warn:
                        _logLevel = Level.Warn;
                        break;
                    case MyLogLevelCategory.Info:
                        _logLevel = Level.Info;
                        break;
                    case MyLogLevelCategory.Debug:
                        _logLevel = Level.Debug;
                        break;
                }
            }
        }

        internal PatternLayout GetPatternLayout()
        {
            var patternLayout = new PatternLayout()
            {
                ConversionPattern = layout
            };
            patternLayout.ActivateOptions();
            return patternLayout;
        }

        internal void SetConfigure(IAppender appender)
        {
            BasicConfigurator.Configure(appender);
        }

        public void dde()
        {

            var deger = MySmtpAppender.Instance._smtpAppender;
        }
        #endregion

        #region Public
        public void CreateLog( MyLogLevelCategory _logLevel = MyLogLevelCategory.All)
        {
            this.LogLevel = _logLevel;
        }

        public void MyLog(Type clasBilgisi, MyLogLevel level, string Message)
        {
                _logger = GetLogger(clasBilgisi);

            switch (level)
            {
                case MyLogLevel.Fatal:
                    _logger.Fatal(Message);
                    break;
                case MyLogLevel.Error:
                    _logger.Error(Message);
                    break;
                case MyLogLevel.Warn:
                    _logger.Warn(Message);
                    break;
                case MyLogLevel.Info:
                    _logger.Info(Message);
                    break;
                case MyLogLevel.Debug:
                    _logger.Debug(Message);
                    break;
            }
        }
        #endregion

        #region Enum
        public enum MyLogType
        {
            ConsoleAppender,
            FileAppender,
            RollingFileAppender,
            AdoNetAppender,
            SmtpAppender
        }
        public enum MyLogLevel
        {
            Fatal,
            Error,
            Warn,
            Info,
            Debug
        }
        public enum MyLogLevelCategory
        {
            All,
            Fatal,
            Error,
            Warn,
            Info,
            Debug
        }
        #endregion
    }
}
