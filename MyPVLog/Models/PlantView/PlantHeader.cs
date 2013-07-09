using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Models.PlantView
{
  public class PlantHeader
  {
    public SolarPlant Plant { get; set; }
    public bool IsEditingAllowed { get; set; }

  }
}