using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Xml.Serialization;
using PVLog.Enums;

namespace PVLog.Models
{
    [XmlRoot("PlantSettings")]
    public class PlantSettings
    {
        [XmlElement("PlantVisibility")]
        public E_PlantVisibility PlantVisibility { get; set; }
        [XmlElement("InverterEuroMapping")]
        public SerializableDictionary<int, double> InverterEuroMapping { get; set; }


        internal static PlantSettings Create()
        {
            var result = new PlantSettings();
            result.InverterEuroMapping = new SerializableDictionary<int, double>();

            return result;
        }
    }
}