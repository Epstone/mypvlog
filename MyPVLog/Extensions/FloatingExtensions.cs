using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Extensions
{
  public static class FloatingExtensions
  {
    public static int RoundOrZero(this double? number)
    {
      return (number.HasValue) ? (int)Math.Round(number.Value) : 0;
    }

  }
}