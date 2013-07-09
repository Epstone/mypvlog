using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.OutputProcessing
{
    public class FlotLineChartTable
    {
        List<double[]> _table = new List<double[]>();

        public string label { get; set; }

        public void AddValue(double timeStamp, double wattage)
        {

            var row = new double[2];
            row[0] = timeStamp;
            row[1] = wattage;

            _table.Add(row);
        }

        public List<double[]> data { get { return _table; } }


        internal void SetSeriesNameForInverter(int currentInverterId)
        {
            this.label = "Inverter " + currentInverterId;
        }
    }
}