using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Extensions
{
  public static class DateTimeExtensions
  {
    public static DateTime ToLastDayOfMonth(this DateTime date)
    {
      return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
    }
  }
}