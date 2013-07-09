using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PVLog.Models
{
  public class Inverter
  {
    [Required]
    public int InverterId { get; set; }

    [Required]
    [DisplayName("Wechselrichter Bezeichnung")]
    public string Name { get; set; }

    [Required]
    public int PublicInverterId { get; set; }

    [Required]
    public int PlantId { get; set; }

    [DisplayName("Euro Pro kwh")]
    [Required]

    public float EuroPerKwh { get; set; }


    [DisplayName("AC Leistung Maximal")]
    [Required]
    [Range(50, 50000,ErrorMessage="Bitte einen Wert zwischen 50 W und 50000 W angeben")]
    public int ACPowerMax { get; set; }
  }
}