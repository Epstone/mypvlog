using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PVLog.Models
{
    public class SolarPlant
    {
        public int PlantId { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie einen Namen für die Anlage an.")]
        [DisplayName("Name der Anlage")]
        [StringLength(30, ErrorMessage = "Maximal 30 Zeichen")]
        public string Name { get; set; }

        //[DisplayName("Anlagen-Kennwort")]
        //[Required(ErrorMessage = "Bitte geben Sie ein Kennwort für die Anlage an.")]
        public string Password { get; set; }

        [DisplayName("Peak Leistung in Watt")]
        [Required(ErrorMessage = "Bitte geben Sie die Maximalleistung in Watt an.")]
        public int PeakWattage { get; set; }

        [DisplayName("Postleitzahl")]
        [Required(ErrorMessage = "Bitte geben Sie die Postleitzahl der Anlage angeben.")]
        public string PostalCode { get; set; }

        [DisplayName("Neue Generatoren/Wechselrichter automatisch hinzufügen")]
        public bool AutoCreateInverter { get; set; }

        public IEnumerable<Inverter> Inverters { get; set; }
        public bool IsDemoPlant { get; set; }

        public bool IsOnline { get; set; }
        public DateTime LastMeasureDate { get; set; }

        [DisplayName("E-Mail Benachrichtigungen einschalten.")]

        public bool EmailNotificationsEnabled { get; set; }
    }
}