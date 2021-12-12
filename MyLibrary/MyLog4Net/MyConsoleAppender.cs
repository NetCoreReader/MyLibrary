using log4net.Appender;
using System;

namespace MyLibrary.MyLog4Net
{
    internal class MyConsoleAppender
    {
        private static Lazy<MyConsoleAppender> lazy = new Lazy<MyConsoleAppender>(() => new MyConsoleAppender());

        internal static MyConsoleAppender Instance { get { return lazy.Value; } }

        private MyConsoleAppender()
        {

        }

        internal ConsoleAppender _consoleAppender { get; set; }

        internal ConsoleAppender GetConsoleAppender()
        {
            _consoleAppender = new ConsoleAppender()
            {
                Layout = Helper.Instance.GetPatternLayout(),
                Name = "LogAppender",
                Threshold = Helper.Instance._logLevel
            };
            _consoleAppender.ActivateOptions();
            Helper.Instance.SetConfigure(_consoleAppender);
            return _consoleAppender;
        }
    }
}
