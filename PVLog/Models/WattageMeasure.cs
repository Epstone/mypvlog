using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Models
{
    public class WattageMeasure : IMeasure
    {

        public double Wattage
        {
            set { this.Value = value; }
            get { return this.Value; }
        }
        public double Value { get; set; }
        public DateTime DateTime { get; set; }
        public int PrivateInverterId { get; set; }






        public int PublicInverterId{ get; set; }
       
    }
}