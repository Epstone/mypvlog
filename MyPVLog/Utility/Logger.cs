using System;

namespace PVLog.Utility
{
    using Microsoft.ApplicationInsights;

    public static class Logger
    {
        static TelemetryClient telemetry = new TelemetryClient();
        public static void Log(Exception ex, SeverityLevel lvl, string customMessage)
        {
            telemetry.TrackTrace(customMessage, MapLevel(lvl));
            telemetry.TrackException(ex);

        }

        private static Microsoft.ApplicationInsights.DataContracts.SeverityLevel MapLevel(SeverityLevel lvl)
        {
            switch (lvl)
            {
                case SeverityLevel.Error:
                    return Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Error;
                case SeverityLevel.Info:
                case SeverityLevel.Startup:
                    return Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information;
                case SeverityLevel.Warning:
                    return Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Warning;
            }

            return Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Error;
        }

        public static void LogError(Exception ex)
        {
            telemetry.TrackException(ex);
        }

        public static void LogInfo(string msg)
        {
            telemetry.TrackTrace(msg, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information);
        }

        public static void LogWarning(string msg)
        {
            telemetry.TrackTrace(msg, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Warning);
        }
    }

}
