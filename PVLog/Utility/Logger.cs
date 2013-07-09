using System;
using PVLog.DataLayer;

namespace PVLog.Utility
{
    public static class Logger
    {
        public static SeverityLevel ApplicationLogLevel { get; set; }

        public static void Log(Exception ex, SeverityLevel lvl, string customMessage)
        {
            var loggerDb = new UtilDatabase();
            loggerDb.AddLog(lvl, ex.Message, ex.StackTrace, customMessage);

        }

        public static void LogError(Exception ex)
        {
            AddLogToDB(ex.Message, ex.StackTrace, SeverityLevel.Error, string.Empty);
        }

        private static void AddLogToDB(string exceptionMessage, string stackTrace, SeverityLevel lvl, string customMessage)
        {
            if (lvl >= ApplicationLogLevel)
            {
                var loggerDb = new UtilDatabase();
                loggerDb.AddLog(lvl, exceptionMessage, stackTrace, customMessage);
            }

        }

        public static void LogInfo(string msg)
        {
            AddLogToDB(string.Empty, string.Empty, SeverityLevel.Info, msg);
        }

        public static SeverityLevel StringToSeverityLevel(string lvl)
        {
            SeverityLevel result = (SeverityLevel)Enum.Parse(typeof(SeverityLevel), lvl);
            return result;
        }

        internal static void SetApplicationLogLevel(string logLevel)
        {
            Logger.ApplicationLogLevel = StringToSeverityLevel(logLevel);
        }

        public static void LogWarning(string msg)
        {
            AddLogToDB(string.Empty, string.Empty, SeverityLevel.Warning, msg);
        }
    }

}
