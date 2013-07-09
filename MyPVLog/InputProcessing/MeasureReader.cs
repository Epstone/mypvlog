using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PVLog.Models;
using System.Globalization;
using PVLog.DataLayer;
using PVLog.Utility;

namespace PVLog.InputProcessing
{
  public static class MeasureReader
  {

    public static Measure ReadKaco1Data(string data, int plantId, int privateInverterId)
    {
      string[] values = data.Split(';');

      Measure measure = new Measure();
      measure.SystemStatus = Convert.ToInt32(values[2]);
      measure.GeneratorVoltage = ParseDouble(values[3]);
      measure.GeneratorAmperage = ParseDouble(values[4]);
      measure.GeneratorWattage = Convert.ToInt32(values[5]);
      measure.GridVoltage = ParseDouble(values[6]);
      measure.GridAmperage = ParseDouble(values[7]);
      measure.OutputWattage = Convert.ToInt32(values[8]);
      measure.Temperature = Convert.ToInt32(values[9]);

      measure.PrivateInverterId = privateInverterId;
      measure.PlantId = plantId;

      // everything to german time
      measure.DateTime = Utils.GetGermanNow();

      return measure;
    }

   

    private static double ParseDouble(string p)
    {
      return Double.Parse(p, CultureInfo.InvariantCulture);
    }

    public static Measure ReadKaco2Data(string data, int plantId)
    {

      string[] values = data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

      Measure measure = new Measure();

      //get inverter ID from *0x0
      measure.PublicInverterId = int.Parse(values[0].Trim().Replace("*", "").Substring(0, 2));

      measure.SystemStatus = int.Parse(values[1]);
      measure.GeneratorVoltage = ParseDouble(values[2]);
      measure.GeneratorAmperage = ParseDouble(values[3]);
      measure.GeneratorWattage = int.Parse(values[4]);
      measure.GridVoltage = ParseDouble(values[5]);
      measure.GridAmperage = ParseDouble(values[6]);
      measure.OutputWattage = int.Parse(values[7]);
      measure.Temperature = int.Parse(values[8]);

      measure.PlantId = plantId;
      measure.DateTime = Utils.GetGermanNow();

      return measure;

    }
  }
}