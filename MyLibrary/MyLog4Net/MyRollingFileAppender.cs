using log4net.Appender;
using System;

namespace MyLibrary.MyLog4Net
{
    internal class MyRollingFileAppender
    {
        private static Lazy<MyRollingFileAppender> lazy = new Lazy<MyRollingFileAppender>(() => new MyRollingFileAppender());

        internal static MyRollingFileAppender Instance { get { return lazy.Value; } }

        private MyRollingFileAppender()
        {

        }
        internal RollingFileAppender _rollingFileAppender { get; set; }
        internal RollingFileAppender GetRollingFileAppender()
        {
            _rollingFileAppender = new RollingFileAppender()
            {
                Layout =Helper.Instance. GetPatternLayout(),
                Name = "RollingAppender",
                Threshold = Helper.Instance._logLevel,
                AppendToFile = true,
                File = Helper.Instance.filePath,
                MaximumFileSize = "10KB",//Log Dosyalarının max boyutunu belirleniyor. Log dosyası max boyuta ulaştığında yeni bir log dosyasına başlanıyor. Eski logları dosyanın sonuna sıra numarası koyarak saklamaya devam ediyor.
                MaxSizeRollBackups = 9,//Yeni log dosyalarının oluşabileceği max adet belirtiliyor. Bu sınıra gelindiğinde sistem log doyası tutmuyor.
            };
            _rollingFileAppender.ActivateOptions();
            Helper.Instance.SetConfigure(_rollingFileAppender);
            return _rollingFileAppender;
        }
    }
}
