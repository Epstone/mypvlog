using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Utility
{
  public class AppLog
  {
    public DateTime Date { get; set; }
    public string LogLevel { get; set; }
    public string ExceptionMessage { get; set; }
    public string ExceptionStacktrace { get; set; }
    public string CustomMessage { get; set; }
  }
}