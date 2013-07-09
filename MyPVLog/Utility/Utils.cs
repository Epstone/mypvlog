using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Bortosky.Google.Visualization;
using PVLog.Models;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;

namespace PVLog.Utility
{
  public class Utils
  {

    /// <summary>
    /// Converts a unix timestamp in seconds since January, 1st 1970 into a DateTime Object.
    /// </summary>
    /// <param name="Timestamp">Unix TimeStamp in seconds</param>
    /// <returns>Returns converted c# DateTime object</returns>
    public static DateTime UnixTimeStampToDateTime(long Timestamp)
    {
      System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

      // add timestamp to 1970-01-01
      return dateTime.AddSeconds(Timestamp);

    }

    /// <summary>
    /// Converts a c# DateTime Object in a JavaScript timestamp. 
    /// </summary>
    /// <param name="input">The DateTime to convert.</param>
    /// <returns>JavaScript TimeStamp</returns>
    public static long DateTimeToJavascriptTimestamp(System.DateTime input)
    {
      DateTime d1 = new DateTime(1970, 1, 1);
      DateTime d2 = input.ToUniversalTime();
      TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);

      return (long)ts.TotalMilliseconds;
    }

    public static DateTime JavascriptUtcTimestampToLocalTime(double timestamp)
    {
      return new DateTime(1970, 01, 01).AddMilliseconds(timestamp).ToLocalTime();
    }

    public static SortedList<DateTime, IMeasure> ConvertToSortedList(List<IMeasure> measures)
    {
      var result = new SortedList<DateTime, IMeasure>();
      foreach (var averageMeasure in measures)
      {
        try
        {
          result.Add(averageMeasure.DateTime, averageMeasure);
        }
        catch (Exception ex)
        {
          Logger.Log(ex, SeverityLevel.Error, "measure mit ist mehrfach vorhanden: " + averageMeasure.DateTime.ToString());
        }

      }

      return result;
    }

    internal static List<int> SplitToIntList(string rawIDs)
    {
      var options = StringSplitOptions.RemoveEmptyEntries;
      return rawIDs.Split(new char[] { ',' }, options).ToList().ConvertAll<int>(s => Convert.ToInt32(s));
    }

    /// <summary>
    /// Gets the SH a1 hash.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    public static string GetSHA1Hash(string text)
    {
      if (string.IsNullOrWhiteSpace(text))
        throw new ArgumentException("Can't create hash from empty string");

      var SHA1 = new SHA1CryptoServiceProvider();

      byte[] arrayData;
      byte[] arrayResult;
      string result = null;
      string temp = null;

      arrayData = Encoding.ASCII.GetBytes(text);
      arrayResult = SHA1.ComputeHash(arrayData);
      for (int i = 0; i < arrayResult.Length; i++)
      {
        temp = Convert.ToString(arrayResult[i], 16);
        if (temp.Length == 1)
          temp = "0" + temp;
        result += temp;
      }
      return result;
    }

    /// <summary>
    /// Returns the current minute as Datetime with 0 as second.
    /// </summary>
    /// <returns></returns>
    public static DateTime GetCurrentMinute()
    {
      var now = GetGermanNow();
      return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
    }
    /// <summary>
    /// Returns the current minute as Datetime with 0 as second.
    /// </summary>
    /// <returns></returns>
    public static DateTime GetLastMinute()
    {
      var now = GetGermanNow();
      return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0).AddMinutes(-1);
    }

    public static DateTime GetWith0Second(DateTime time)
    {
      return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);
    }

    internal static TimeSpan GetTimeSpanToNextMinute(int seconds)
    {
      var now = GetGermanNow();
      var startMinute = Utils.GetCurrentMinute().AddMinutes(1).AddSeconds(seconds);

      return startMinute - now;
    }

    /// <summary>
    /// returns new Date(todays.year, todays.month, todays.day, 0, 0, 0);
    /// </summary>
    /// <returns></returns>
    public static DateTime GetTodaysDate()
    {
      return new DateTime(GetGermanNow().Year, GetGermanNow().Month, GetGermanNow().Day);
    }

    public static bool EarlierThanThisMinute(DateTime itemDateTime)
    {
      DateTime currentMinute = Utils.GetCurrentMinute();
      DateTime measureMinute = Utils.GetWith0Second(itemDateTime);

      return (measureMinute < currentMinute);
    }

    public static DateTime CropHourMinuteSecond(DateTime date)
    {
      return new DateTime(date.Year, date.Month, date.Day);
    }

    public static string sqlCommandToString(MySqlCommand c, Boolean bVerbose)
    {
      string retval = c.CommandText + " ";
      MySqlParameter p;
      for (int i = 0; i < c.Parameters.Count; i++)
      {
        p = c.Parameters[i];
        if (bVerbose)
        {
          retval = retval + p.ParameterName + "(" + p.MySqlDbType.ToString() + ")=" + p.Value.ToString() + ", ";
        }
        else
        {
          retval = retval + "'" + p.Value.ToString() + "',";
        }
      }
      return retval;
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

    public static string GenerateRandomString(int p)
    {
      string rStr = Path.GetRandomFileName();
      rStr = rStr.Replace(".", "").Substring(0, p); // For Removing the .
      return rStr;
    }

    const string tzId = "W. Europe Standard Time";
    internal static DateTime GetGermanNow()
    {
      return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, tzId);
    }

    internal static TimeZoneInfo GetGermanTimeZone()
    {
      return TimeZoneInfo.FindSystemTimeZoneById(tzId);
    }
  }

}