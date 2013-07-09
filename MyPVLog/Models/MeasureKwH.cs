using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PVLog.Enums;

namespace PVLog.Models
{
    public class MeasureKwH : IEquatable<MeasureKwH>,IMeasure
    {
        public long ID { get; set; }
        //public int PlantId { get; set; }
        public E_TimeMode TimeMode { get; set; }
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
        public int PrivateInverterId { get; set; }
        public int PublicInverterId { get; set; }

        public bool Equals(MeasureKwH other)
        {
            if (this.DateTime.ToString() != other.DateTime.ToString()) return false;
            if (this.ID != other.ID) return false;
            if (this.PrivateInverterId != other.PrivateInverterId) return false;
            if (this.TimeMode != other.TimeMode) return false;
            if (this.Value != other.Value) return false;

            return true;
        }
        public override bool Equals(object obj)
        {
            MeasureKwH other = obj as MeasureKwH;
            if (other == null)
                return false;
            else
                return Equals(other);
        }
    }
}
