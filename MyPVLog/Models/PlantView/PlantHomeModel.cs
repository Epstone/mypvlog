using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Models.PlantView
{
    public class PlantHomeModel
    {
        public SolarPlant Plant { get; set; }

        internal static PlantHomeModel Create(SolarPlant plant)
        {
            return new PlantHomeModel()
            {
                Plant = plant
            };

        }

        public PlantSummaryTableModel SummaryTable { get; set; }
        public PlantHeader HeaderModel { get; set; }
    }
}

