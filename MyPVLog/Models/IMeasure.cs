using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Models
{
    public interface IMeasure
    {
        double Value { get; set; }
        DateTime DateTime { get; set; }
        int PrivateInverterId { get; set; }
        int PublicInverterId { get; set; }
        //int PlantId { get; set; }
    }
}