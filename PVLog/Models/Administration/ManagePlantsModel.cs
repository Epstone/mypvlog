using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Models.Administration
{
    public class ManagePlantsModel
    {
        public SolarPlant DemoPlant { get; set; }

        public IEnumerable<SolarPlant> RealPlants { get; set; }
    }
}