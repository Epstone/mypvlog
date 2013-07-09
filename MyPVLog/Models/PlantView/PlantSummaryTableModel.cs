using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Models.PlantView
{
    public class PlantSummaryTableModel
    {
        public KwhEurResult Today { get; set; }
        public KwhEurResult ThisMonth { get; set; }
        public KwhEurResult ThisYear { get; set; }

        
    }
}