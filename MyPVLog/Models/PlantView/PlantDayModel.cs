using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Models.PlantView
{
    public class PlantDayModel
    {
       public SolarPlant Plant { get; set; }

       public string GoogleData { get; set; }
       public PlantHeader HeaderModel { get; set; }
    }
}