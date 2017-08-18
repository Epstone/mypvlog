using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PVLog.Models;
using PVLog;
using PVLog.Utility;

namespace solar_tests
{
  public class TestdataGenerator
  {
    public static List<Measure> GetAverageMeasures(int count, DateTime date)
    {
      return GetAverageMeasures(count, date, 50);
    }

    public static List<Measure> GetAverageMeasures(int count, DateTime date, double averageWattage)
    {
      List<Measure> measures = new List<Measure>();
      for (int i = 0; i < count; i++)
      {
        var measure = new Measure();
        measure.OutputWattage = averageWattage;
        measure.DateTime = date.AddMinutes(i);
        measure.PrivateInverterId = 1;
        measure.PlantId = 1;
        measures.Add(measure);
      }
      return measures;
    }

    internal static IList<DateTime> GetDates(DateTime startAverageDateTime, int minutes)
    {
      List<DateTime> dates = new List<DateTime>();
      for (int i = 0; i < minutes; i++)
      {
        dates.Add(startAverageDateTime.AddMinutes(i));
      }

      return dates;
    }

    internal static SortedList<DateTime, Measure> GetAverageMeasuresSorted(int count, DateTime date, int wattage)
    {
      var measures = GetAverageMeasures(count, date, wattage);
      var result = new SortedList<DateTime, Measure>();

      foreach (var measure in measures)
      {
        result.Add(measure.DateTime, measure);
      }

      return result;
    }

    /// <summary>
    /// 2010, 12, 24, 12, 0, 0
    /// </summary>
    /// <returns></returns>
    internal static DateTime GetTestDate()
    {
      return new DateTime(2010, 12, 24, 12, 0, 0);
    }

    internal static List<IMeasure> CombineMeasureLists(List<List<Measure>> measurelists)
    {
      var completeList = new List<IMeasure>();
      foreach (var measureList in measurelists)
      {
        foreach (var measure in measureList)
        {
          completeList.Add(measure);
        }
      }
      return completeList;
    }

    internal static Measure GetTestMeasure(DateTime time, double outputWattage)
    {
      return GetTestMeasure(time, outputWattage, 1);
    }

    internal static Measure GetTestMeasure()
    {
      return GetTestMeasure(DateTime.Now, 594.5);
    }

    internal static Measure GetTestMeasure(DateTime time, double outputWattage, int inverterID)
    {
      Measure measure = new Measure();
      measure.DateTime = time;
      measure.GeneratorAmperage = 5.3;
      measure.GeneratorVoltage = 229.3;
      measure.GeneratorWattage = 324;
      measure.GridAmperage = 3.4;
      measure.GridVoltage = 230.9;
      measure.PrivateInverterId = inverterID;
      measure.OutputWattage = outputWattage;
      measure.PlantId = 1;
      measure.SystemStatus = 5;
      measure.Temperature = 19;

      return measure;
    }

    internal static List<Measure> GetMeasureListWattageOnly(DateTime startDate, DateTime endDate, double wattage, int inverterId)
    {
      var result = new List<Measure>();
      var countDate = startDate;
      while (countDate < endDate)
      {
        //set correct plantId for that measure
        result.Add(new Measure()
        {
          DateTime = countDate,
          OutputWattage = wattage,
          PrivateInverterId = inverterId
        });

        //increase countDate
        countDate = countDate.AddMinutes(1);
      }

      return result;
    }

    internal static List<Measure> GetMeasureList(DateTime startDate, DateTime endDate, double wattage, int inverterId)
    {
      var result = new List<Measure>();
      var countDate = startDate;
      while (countDate < endDate)
      {
        //set correct plantId for that measure
        result.Add(GetTestMeasure(countDate, wattage, inverterId));

        //increase countDate
        countDate = countDate.AddMinutes(1);
      }

      return result;
    }

    internal static MeasureKwH GetKwhDay(double kwhValue)
    {
      MeasureKwH kwh = new MeasureKwH();
      kwh.DateTime = Utils.GetTodaysDate();
      kwh.PrivateInverterId = 2;
      kwh.TimeMode = PVLog.Enums.E_TimeMode.day;
      kwh.Value = kwhValue;

      return kwh;
    }

    internal static Measure GetTestMeasure(int plantId)
    {
      var measure = GetTestMeasure();
      measure.PlantId = plantId;

      return measure;
    }

    internal static SolarPlant GetPlant()
    {
      return new SolarPlant()
      {
        IsDemoPlant = false,
        Name = "Test Anlage",
        Password = "123456",
        PeakWattage = 8000,
        PostalCode = "12345"
      };
    }
  }
}
