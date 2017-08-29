using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using PVLog.DataLayer;
using PVLog.OutputProcessing;
using PVLog.Extensions;
using System.Globalization;
using PVLog.Utility;

namespace PVLog.Controllers
{
  public class ExportController : MyController
  {
    public ExportController()
    {

    }

    public ExportController(I_MeasureRepository i_MeasureRepository, I_PlantRepository i_PlantRepository)
    {
      base._measureRepository = i_MeasureRepository;
      base._plantRepository = i_PlantRepository;
    }

    [HttpGet]
    public JavaScriptResult base_vars(int plantId)
    {
      //WRInfo[WR Index]=new Array("WR Kurzname","???",Angeschlossene AC Leistung,0,"WR Langname",Anzahl Strings,null,null,0,null,1,0,0,1000,null)

      //WRInfo[0]=new Array("SMA SB 2100","???",4711,0,"SMA Sunny Boy 2100TL",1,null,null,0,null,1,0,0,1000,null) 
      //WRInfo[1]=new Array("SMA SB 2100","???",4711,0,"SMA Sunny Boy 2100TL",1,null,null,0,null,1,0,0,1000,null)

      StringBuilder result = new StringBuilder();

      // Inverter Count
      var plant = _plantRepository.GetPlantById(plantId);
      result.AppendFormat("var AnzahlWR = {0}", plant.Inverters.Count());
      result.AppendLine();

      // Anlagen KWP
      result.AppendFormat("var AnlagenKWP={0}", plant.PeakWattage);
      result.AppendLine();

      // Append array initialization
      result.AppendLine("var WRInfo = new Array(AnzahlWR)");

      //get inverters for plant and build WRInfo
      var inverters = plant.Inverters;

      int i = 0;
      foreach (var inverter in inverters)
      {
        result.AppendFormat(@"WRInfo[{0}]=new Array(""{1}"",""???"",{2},0,""{1}"",1,null,null,0,null,1,0,0,1000,null)", i, inverter.Name, inverter.ACPowerMax);
        result.AppendLine();
        i++;
      }

      return JavaScript(result.ToString());
    }

    [HttpGet]
    public JavaScriptResult months(int plantId)
    {
      // mo[mx++]="Datum|Ertrag WR1|Ertrag WR2"
      string dateFormat = "dd.MM.yy";
      var monthResult = _dataProvider.GetMonthTable(plantId);

      StringBuilder builder = new StringBuilder();
      foreach (var row in monthResult.Rows.OrderByDescending(x => x.Key)) //need to be descending, This month should be first
      {
        //add a new entry
        builder.AppendFormat("mo[mx++]=\"{0}|", row.Key.ToLastDayOfMonth().ToString(dateFormat));

        //order by public inverter id first
        var sortedByPublicInverterId = row.Value.kwhValues.OrderBy(y => y.PublicInverterId).ToArray();

        // iterate through inverter kwh values
        for (int i = 0; i < sortedByPublicInverterId.Length; i++)
        {
          builder.Append(GetRoundedWattageHour(sortedByPublicInverterId[i].Value)); //kWh to Wh

          if (i != sortedByPublicInverterId.Length - 1)
            builder.Append("|");
          else
            builder.Append("\"");
        }
        builder.AppendLine();

      }

      return JavaScript(builder.ToString());
    }

    private double GetRoundedWattageHour(double value)
    {
      return Math.Round(value * 1000);
    }

    [HttpGet]
    public JavaScriptResult days_hist(int plantId)
    {
      // Two Inverters:
      /*  da[dx++]="14.02.09|123456;1234|123456;1234"
          da[dx++]="13.02.09|123456;1234|123456;1234"
          da[dx++]="12.02.09|123456;1234|123456;1234"
          da[dx++]="11.02.09|123456;1234|123456;1234" */
      string dateFormat = "dd.MM.yy";
      var dayResult = _kwhRepository.GetDayKwhByDateRange(DateTimeUtils.GetGermanNow().AddYears(-10), DateTimeUtils.GetGermanNow(), plantId);


      StringBuilder builder = new StringBuilder();
      foreach (var row in dayResult.Rows.OrderByDescending(x => x.Key)) //need to be descending, This month should be first
      {
        //add a new entry
        builder.AppendFormat("da[dx++]=\"{0}|", row.Key.ToString(dateFormat));

        //order by public inverter id first
        var sortedByPublicInverterId = row.Value.kwhValues.OrderBy(y => y.PublicInverterId).ToArray();

        // iterate through inverter kwh values
        for (int i = 0; i < sortedByPublicInverterId.Length; i++)
        {
          builder.Append(GetRoundedWattageHour(sortedByPublicInverterId[i].Value)); //kWh to Wh
          builder.Append(";0");                                                     //add max wattage for that day (TODO)

          if (i != sortedByPublicInverterId.Length - 1)
            builder.Append("|");
          else
            builder.Append("\"");
        }
        builder.AppendLine();

      }


      return JavaScript(builder.ToString());
    }

    [HttpGet]
    public JavaScriptResult min_day(int plantId)
    {
      /*Beispiel für 2 Wechselrichter
       * m[mi++]="Datum Uhrzeit|AC Leistung;DC Leistung;AC Tagesertrag;DC Spannung;WR Temperatur|AC Leistung;DC Leistung;AC Leistung;DC Spannung;WR Temperatur"
       * m[mi++]="15.02.09 08:30:00|0;11;0;346;6|0;10;0;335;7"
       * m[mi++]="15.02.09 08:25:00|0;12;0;324;5|0;11;0;306;6"
       * m[mi++]="15.02.09 08:20:00|0;13;0;282;5|0;10;0;267;6" 
       */

      string result = GetMinDayJsFileContent(DateTimeUtils.GetGermanNow(), plantId);

      return JavaScript(result);
    }

    [HttpGet]
    public JavaScriptResult MinuteWiseDefinedDate(string date, int plantId)
    {
      var requestDate = DateTime.ParseExact(date, "yyMMdd", CultureInfo.InvariantCulture);

      var result = GetMinDayJsFileContent(requestDate, plantId);

      return JavaScript(result);
    }

    private string GetMinDayJsFileContent(DateTime dateTime, int plantId)
    {
      string dateTimeFormat = "dd.MM.yy HH:mm:ss";
      var startDate = DateTimeUtils.CropHourMinuteSecond(dateTime);
      var endDate = startDate.AddDays(1);

      List<Measure> allMeasures = new List<Measure>();
      var inverters = _plantRepository.GetAllInvertersByPlant(plantId);

      // grab all measures for the requested day
      foreach (var inverter in inverters)
      {
        var inverterMeasures = _measureRepository.GetMinuteWiseMeasures(startDate, endDate, inverter.InverterId);

        //group data by 5 minute intervals
        var inverterMeasures5Min = GroupMeasuresTo5MinIntervals(plantId, inverter, inverterMeasures);

        //add measures to all measures list
        allMeasures.AddRange(inverterMeasures5Min);
      }

      if (allMeasures.Count == 0)
        return string.Empty;

      // order measures by DateTime Descending and the public inverter Id Ascending
      var orderedMeasures = allMeasures.OrderBy(x => x.DateTime).ThenBy(x => x.PublicInverterId).ToList();

      //index: public inverterId, key: wattage hours
      WattageHourCalculator calc = new WattageHourCalculator();

      // count time for calculation and string building decisions      
      var countTime = orderedMeasures.First().DateTime;
      List<string> resultLines = new List<string>();
      StringBuilder builder = null;
      int measureCounter = 0;
      int inverterCount = inverters.Count();
      Measure latestMeasure = null;

      for (int i = 0; i < orderedMeasures.Count; i++)
      {

        var currentMeasure = orderedMeasures[i];

        // build a new line and append the old one
        if (latestMeasure == null || currentMeasure.DateTime != latestMeasure.DateTime)
        {
          // delete this line if not every inverter has a 5 min measure included
          if (measureCounter < inverterCount)
          {
            builder = null;
          }

          //reset measure counter
          measureCounter = 0;

          // finish last row and add it to result-lines list, if we have a stringbuilder allready
          if (builder != null)
          {
            builder.Append("\"");
            resultLines.Add(builder.ToString());
          }

          // reset string builder
          builder = new StringBuilder();

          //append dateTime and row start string
          builder.AppendFormat(@"m[mi++]=""{0}", currentMeasure.DateTime.ToString(dateTimeFormat));
        }

        //calculate wattage hour total for the current inverter where the measure is from
        calc.AddPowerValue(currentMeasure.PublicInverterId, currentMeasure.OutputWattage, new TimeSpan(0, 5, 0));
        measureCounter++;

        // append inverter Data
        builder.AppendFormat(@"|{0};{1};{2};{3};{4}", Math.Round(currentMeasure.OutputWattage),
                                                       currentMeasure.GeneratorWattage.RoundOrZero(),
                                                        calc.GetWattageHour(currentMeasure.PublicInverterId),
                                                        currentMeasure.GeneratorVoltage.RoundOrZero(),
                                                        (currentMeasure.Temperature.HasValue) ? currentMeasure.Temperature.Value : 0);

        // update count-time
        latestMeasure = currentMeasure;
      }

      // add latest row if valid
      if (measureCounter == inverterCount)
        resultLines.Add(builder.ToString());

      //reverse lines in list
      StringBuilder result = new StringBuilder();
      for (int i = resultLines.Count - 1; i >= 0; i--)
      {
        result.AppendLine(resultLines[i]);
      }

      return result.ToString();
    }

    private static List<Measure> GroupMeasuresTo5MinIntervals(int plantId, Models.Inverter inverter, IEnumerable<Measure> inverterMeasures)
    {
      // set all nullables to zero
      foreach (var m in inverterMeasures)
      {
        if (!m.GeneratorAmperage.HasValue) m.GeneratorAmperage = 0;
        if (!m.GeneratorVoltage.HasValue) m.GeneratorVoltage = 0;
        if (!m.GeneratorWattage.HasValue) m.GeneratorWattage = 0;
        if (!m.GridAmperage.HasValue) m.GridAmperage = 0;
        if (!m.GridVoltage.HasValue) m.GridVoltage = 0;
        if (!m.Temperature.HasValue) m.Temperature = 0;
      }

      var inverterMeasures5Min = inverterMeasures.GroupBy(x =>
      {
        var stamp = x.DateTime;
        stamp = stamp.AddMinutes(-(stamp.Minute % 5));
        stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
        return stamp;
      })
        .Select(g => new Measure
        {
          DateTime = g.Key,
          GeneratorAmperage = g.Average(s => s.GeneratorAmperage),
          GeneratorVoltage = g.Average(s => s.GeneratorVoltage),
          GeneratorWattage = g.Average(s => s.GeneratorWattage),
          GridAmperage = g.Average(s => s.GridAmperage),
          GridVoltage = g.Average(s => s.GridVoltage),
          OutputWattage = g.Average(s => s.OutputWattage),
          PlantId = plantId,
          PrivateInverterId = inverter.InverterId,
          PublicInverterId = inverter.PublicInverterId,
          Temperature = (int)g.Average(s => s.Temperature)
        })
        .ToList();
      return inverterMeasures5Min;
    }



  }
}
