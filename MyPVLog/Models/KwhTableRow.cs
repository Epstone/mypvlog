using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PVLog.Models
{
    public class KwhTableRow
    {
        public DateTime Index { get; set; }
        public List<IMeasure> kwhValues { get; set; }

        public KwhTableRow(DateTime dateTime)
        {
            kwhValues = new List<IMeasure>();
            this.Index = dateTime;
        }

        internal IMeasure GetKwh(int publicInverterId)
        {
            return kwhValues.Single(x => x.PublicInverterId == publicInverterId);
        }
    }
}
