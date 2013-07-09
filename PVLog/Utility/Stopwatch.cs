using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Utility
{
  public class Stopwatch
  {
    DateTime _startTime = DateTime.Now;

    public TimeSpan LifeTime
    {
      get
      {
        return new TimeSpan(DateTime.Now.Ticks - _startTime.Ticks);
      }
    }
  }
}