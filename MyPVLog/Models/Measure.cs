//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:2.0.50727.4927
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using PVLog.Models;


namespace PVLog
{
    public class Measure : IEquatable<Measure>, IMeasure
    {

        public Measure()
        {
        }

        public long MeasureId { get; set; }
        public double OutputWattage { get; set; }
        public double? GeneratorWattage { get; set; }
        public int PlantId { get; set; }
        public int? Temperature { get; set; }
        public int? SystemStatus { get; set; }
        public int PrivateInverterId { get; set; }
        public int PublicInverterId { get; set; }
        public double? GridVoltage { get; set; }
        public double? GridAmperage { get; set; }
        public double? GeneratorVoltage { get; set; }
        public double? GeneratorAmperage { get; set; }
        public DateTime DateTime { get; set; }


        public bool Equals(Measure other)
        {
            if (this.DateTime.ToString() != other.DateTime.ToString()) return false;
            if (this.GeneratorAmperage != other.GeneratorAmperage) return false;
            if (this.GeneratorVoltage != other.GeneratorVoltage) return false;
            if (this.GeneratorWattage != other.GeneratorWattage) return false;
            if (this.GridAmperage != other.GridAmperage) return false;
            if (this.GridVoltage != other.GridVoltage) return false;
           
            if (this.PrivateInverterId != other.PrivateInverterId) return false;

            if (this.OutputWattage != other.OutputWattage) return false;
            if (this.PlantId != other.PlantId) return false;
            if (this.SystemStatus != other.SystemStatus) return false;
            if (this.Temperature != other.Temperature) return false;

            return true;
        }
        public override bool Equals(object otherObj)
        {
            Measure other = otherObj as Measure;
            if (other == null)
                return false;
            else
                return Equals(other);

        }

        public double Value
        {
            get
            {
                return this.OutputWattage;
            }
            set
            {
                this.OutputWattage = value;
            }
        }


    }
}