using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLibrary.MyLog4Net
{
    internal class MyFileAppender
    {
        private static Lazy<MyFileAppender> lazy = new Lazy<MyFileAppender>(() => new MyFileAppender());

        internal static MyFileAppender Instance { get { return lazy.Value; } }

        private MyFileAppender()
        {

        }

        internal FileAppender _fileAppender { get; set; }

        internal FileAppender GetFileAppender()
        {
            _fileAppender = new FileAppender()
            {
                Layout = Helper.Instance.GetPatternLayout(),
                Name = "LogAppender",
                Threshold = Helper.Instance._logLevel,
                AppendToFile = true,
                File = Helper.Instance.filePath,

            };
            _fileAppender.ActivateOptions();
            Helper.Instance.SetConfigure(_fileAppender);
            return _fileAppender;
        }
    }
}
