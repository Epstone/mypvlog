using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PVLog.Utility;

namespace PVLog.Models.Administration
{
  public class ShowLogsModel
  {
    public IEnumerable<AppLog> Logs { get; set; }
  }
}