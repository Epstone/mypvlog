using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Extensions
{
    public static class DisplayExtensions
    {
        public static string ToKwhString(this double? kwh)
        {
            if (kwh.HasValue)
                return (Math.Round(kwh.Value, 1) + " kwh");
            else
                return " - ";
        }

        public static string ToEuroString(this double? eur)
        {
            if (eur.HasValue)
                return (Math.Round(eur.Value, 2) + " €");
            else
                return " - ";
        }
        
    }
}