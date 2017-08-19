namespace PVLog.Utility
{
    using System;
    using System.Globalization;

    public static class DateTimeUtils
    {
        private const string tzId = "W. Europe Standard Time";

        /// <summary>
        ///     Converts a unix timestamp in seconds since January, 1st 1970 into a DateTime Object.
        /// </summary>
        /// <param name="Timestamp">Unix TimeStamp in seconds</param>
        /// <returns>Returns converted c# DateTime object</returns>
        public static DateTime UnixTimeStampToDateTime(long Timestamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            // add timestamp to 1970-01-01
            return dateTime.AddSeconds(Timestamp);
        }

        /// <summary>
        ///     Converts a c# DateTime Object in a JavaScript timestamp.
        /// </summary>
        /// <param name="input">The DateTime to convert.</param>
        /// <returns>JavaScript TimeStamp</returns>
        public static long DateTimeToJavascriptTimestamp(DateTime input)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = input.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);

            return (long) ts.TotalMilliseconds;
        }

        public static DateTime JavascriptUtcTimestampToLocalTime(double timestamp)
        {
            var dateTime = new DateTime(1970, 01, 01).AddMilliseconds(timestamp);
            return TimeZoneInfo.ConvertTime(dateTime, GetGermanTimeZone());
        }

        /// <summary>
        ///     Returns the current minute as Datetime with 0 as second.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentMinute()
        {
            var now = GetGermanNow();
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
        }

        /// <summary>
        ///     Returns the current minute as Datetime with 0 as second.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLastMinute()
        {
            var now = GetGermanNow();
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0).AddMinutes(-1);
        }

        public static DateTime CropBelowSecondsInclusive(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);
        }

        /// <summary>
        ///     returns new Date(todays.year, todays.month, todays.day, 0, 0, 0);
        /// </summary>
        /// <returns></returns>
        public static DateTime GetTodaysDate()
        {
            return new DateTime(GetGermanNow().Year, GetGermanNow().Month, GetGermanNow().Day);
        }

        public static bool EarlierThanThisMinute(DateTime itemDateTime)
        {
            DateTime currentMinute = GetCurrentMinute();
            DateTime measureMinute = CropBelowSecondsInclusive(itemDateTime);

            return measureMinute < currentMinute;
        }

        public static DateTime CropHourMinuteSecond(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static DateTime GetGermanNow()
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, tzId);
        }

        public static TimeZoneInfo GetGermanTimeZone()
        {
            return TimeZoneInfo.FindSystemTimeZoneById(tzId);
        }

        internal static TimeSpan GetTimeSpanToNextMinute(int seconds)
        {
            var now = GetGermanNow();
            var startMinute = GetCurrentMinute().AddMinutes(1).AddSeconds(seconds);

            return startMinute - now;
        }

        internal static DateTime FirstDayOfMonth()
        {
            var now = GetGermanNow();
            return new DateTime(now.Year, now.Month, 1);
        }

        internal static DateTime FirstDayOfMonth(int month, int year)
        {
            return new DateTime(year, month, 1);
        }

        internal static string GetMonthName(int month)
        {
            return new DateTimeFormatInfo().MonthNames[month - 1];
        }

        internal static DateTime FirstDayOfYear()
        {
            return new DateTime(GetGermanNow().Year, 1, 1);
        }

        internal static DateTime FirstDayOfYear(int year)
        {
            return new DateTime(year, 1, 1);
        }

        internal static DateTime FirstDayNextYear()
        {
            return FirstDayOfYear().AddYears(1);
        }

        internal static DateTime FirstDayNextMonth()
        {
            return FirstDayOfMonth().AddMonths(1);
        }
    }
}