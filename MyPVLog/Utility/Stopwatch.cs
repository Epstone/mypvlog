using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Utility
{
  public class Stopwatch
  {
    DateTime _startTime = DateTimeUtils.GetGermanNow();

    public TimeSpan LifeTime
    {
      get
      {
        return new TimeSpan(DateTimeUtils.GetGermanNow().Ticks - _startTime.Ticks);
      }
    }
  }
}