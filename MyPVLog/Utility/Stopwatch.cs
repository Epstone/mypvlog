using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Utility
{
  public class Stopwatch
  {
    DateTime _startTime = Utils.GetGermanNow();

    public TimeSpan LifeTime
    {
      get
      {
        return new TimeSpan(Utils.GetGermanNow().Ticks - _startTime.Ticks);
      }
    }
  }
}