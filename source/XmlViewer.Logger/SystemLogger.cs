using System;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace XmlViewer.Logger
{
    public class SystemLogger
    {
        private static bool configured;

        private const string FILE_TARGET_NAME = "logfile";

        private const string LAYOUT = "${longdate}|${level:uppercase=true}| - ${message}. ${exception:format=tostring}";

        private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public static void Fatal(Exception e, string message)
        {
            if (configured)
            {
                logger.Fatal(e, message);
            }
        }

        public static void Error(Exception e, string message)
        {
            if (configured)
            {
                logger.Error(e, message);
            }
        }

        public static void Info(string message)
        {
            if (configured)
            {
                logger.Info(message);
            }
        }

        public static void Configure(string logFileName)
        {
            if (configured)
            {
                return;
            }

            var config = new LoggingConfiguration();
            var logfile = new FileTarget(FILE_TARGET_NAME)
            {
                FileName = logFileName,
                Layout = LAYOUT
            };

            config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);
            LogManager.Configuration = config;
            configured = true;
        }
    }
}
