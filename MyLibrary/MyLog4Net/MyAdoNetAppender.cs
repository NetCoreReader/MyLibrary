using log4net.Appender;
using log4net.Layout;
using System;

namespace MyLibrary.MyLog4Net
{
    internal class MyAdoNetAppender
    {
        private static Lazy<MyAdoNetAppender> lazy = new Lazy<MyAdoNetAppender>(() => new MyAdoNetAppender());

        internal static MyAdoNetAppender Instance { get { return lazy.Value; } }

        private MyAdoNetAppender()
        {

        }

        internal AdoNetAppender _adoNetAppender { get; set; }

        internal AdoNetAppender GetAdoNetAppender()
        {
            if (String.IsNullOrEmpty(Helper.Instance._cnnString))
                throw new Exception("ConnectionString Boş Geçilemez.");
            if (MyConnect.Instance.TabloVarMi(Helper.Instance._cnnString, "MyLog", out MyConnect.Instance.exp) == false)
            {
                MyConnect.Instance.ExecuteNonQuery(Helper.Instance._cnnString, "CREATE TABLE [dbo].[MyLog]([Id][int] IDENTITY(1, 1) NOT NULL, [Date][datetime] NOT NULL, [Thread][varchar](255) NOT NULL, [Level][varchar](50) NOT NULL, [Logger][varchar](255) NOT NULL, [Message][varchar](4000) NOT NULL, [Exception][varchar](2000) NULL, [Class][varchar](4000) NOT NULL, [Method][varchar](4000) NOT NULL, CONSTRAINT[PK_Log4NetLog] PRIMARY KEY CLUSTERED ([Id] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY] ", System.Data.CommandType.Text, 60, out MyConnect.Instance.exp);
            }


            _adoNetAppender = new AdoNetAppender()
            {
                CommandType = System.Data.CommandType.Text,
                ConnectionString = Helper.Instance._cnnString,
                CommandText = "INSERT INTO MyLog (Date, Thread, Level, Logger, Class, Method, Message, Exception) VALUES (@log_date, @thread, @log_level, @logger, @class, @method, @message, @exception)",
                Layout = Helper.Instance.GetPatternLayout(),
                Threshold = Helper.Instance._logLevel,
                ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                BufferSize = 1,
                Name = "adoNetAppender"
            };
            _adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@log_date", DbType = System.Data.DbType.DateTime, Layout = new RawTimeStampLayout() });
            _adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@thread", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%thread")) });
            _adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@log_level", DbType = System.Data.DbType.String, Size = 50, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
            _adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@logger", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger")) });
            _adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@class", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%class")) });
            _adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@method", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%method")) });
            _adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@message", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%message")) });
            _adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@exception", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(new ExceptionLayout()) });
            _adoNetAppender.ActivateOptions();
            Helper.Instance.SetConfigure(_adoNetAppender);
            return _adoNetAppender;
        }
    }
}
