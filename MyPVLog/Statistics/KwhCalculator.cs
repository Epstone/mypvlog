using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PVLog.Models;
using PVLog.Utility;
using PVLog.Enums;

namespace PVLog
{
  public class KwhCalculator
  {

    internal static SortedList<DateTime, MeasureKwH> GetKwhHourly(List<IMeasure> measures, int startHour, int endHour)
    {
      var hourly = GetKwhHourlyForOneDay(measures);
      return FillHourGaps(hourly, startHour, endHour);
    }

    /// <summary>
    /// Calculates the kwh for one day only
    /// </summary>
    /// <param name="measures">The minutewise measures of one day and one inverter</param>
    /// <returns>It returns a SortedList where the key is a full hour DateTime and the value 
    /// the kwh result</returns>
    public static SortedList<DateTime, MeasureKwH> GetKwhHourlyForOneDay(List<IMeasure> measures)
    {
      //result variable
      SortedList<DateTime, MeasureKwH> result = new SortedList<DateTime, MeasureKwH>();

      //break if we don't have any data
      if (measures.Count <= 1)
        return result;

      //Convert to SortedList
      var sortedMeasures = Utils.ConvertToSortedList(measures);

      //Get private and public inverterID
      int privateInverterID = measures.First().PrivateInverterId;
      int publicInverterId = measures.First().PublicInverterId;

      var date = sortedMeasures.Values[0].DateTime;
      var currentDay = new DateTime(date.Year, date.Month, date.Day);

      // calculation vars
      double currentHourSum = 0;
      int measureCounter = 0;

      //Get first and last test hour
      DateTime firstMeasureCurrentDayTime = FirstMeasureTimeOfDay(currentDay, sortedMeasures.Keys);
      DateTime lastMeasureCurrentDayTime = LastMeasureTimeOfDay(currentDay, sortedMeasures.Keys);

      //temp variable
      DateTime lastMeasureOfHourTimePoint = new DateTime();
      var isHourCompleted = true;
      double incompletenesFactor = 1;

      foreach (var currentMeasure in sortedMeasures.Values)
      {
        //add to current hour total and increase measure counter
        currentHourSum += currentMeasure.Value;
        measureCounter++;

        //if hour was completed with the last iteration
        if (isHourCompleted)
        {
          //set latest measure dateTime current hour
          lastMeasureOfHourTimePoint = GetLastMeasureTimeOfHour(sortedMeasures.Keys, currentMeasure.DateTime);
          isHourCompleted = false;
        }

        // if this is the last hour of the current day
        if (currentMeasure.DateTime == lastMeasureOfHourTimePoint) //last measure of hour
        {
          //evaluate the incompletenes factor and then build the kwh result
          if (AreInSameHour(currentMeasure, firstMeasureCurrentDayTime))
          {
            // if this measure lies in the first of hour the current day
            int firstMinuteFirstMeasureHour = firstMeasureCurrentDayTime.Minute;
            incompletenesFactor = ((60 - firstMinuteFirstMeasureHour) / 60.0);
          }
          else if (AreInSameHour(currentMeasure, lastMeasureCurrentDayTime))
          {
            // if this measure lies in the last hour of the current day
            int lastMinute = currentMeasure.DateTime.Minute;
            incompletenesFactor = ((lastMinute + 1) / 60.0);
          }

          //calculate kwh and add to result
          MeasureKwH hourKwh = new MeasureKwH();
          hourKwh.DateTime = ExtractHourDateTime(currentMeasure);
          hourKwh.Value = (currentHourSum / measureCounter) * incompletenesFactor / 1000;// divided through 1000 because of "k"wh
          hourKwh.PrivateInverterId = privateInverterID;
          hourKwh.PublicInverterId = publicInverterId;
          hourKwh.TimeMode = Enums.E_TimeMode.hour;


          result.Add(hourKwh.DateTime, hourKwh);

          //reset calculation variables for the next hour to calculate
          currentHourSum = 0; measureCounter = 0; incompletenesFactor = 1;
          isHourCompleted = true;
        }
      }

      return result;
    }

    /// <summary>
    /// Summarizes kwh values from hourly up to yearly. At first it initializes a result list, which is then filled with cumulated data from the input.
    /// </summary>
    /// <param name="measures">Kwh table for an undefined time range</param>
    /// <param name="startDate">The startDate Parameter can be used to crop
    /// unwanted kwh values at the beginning</param>
    /// <param name="endDate">The endDate Parameter crops all unwanted tailing kwh
    /// values</param>
    /// <param name="timeMode">The timeMode defines the target kwh stepping.
    /// Stepping includes from hourly up to yearly.</param>
    /// <param name="fillGaps"></param>
    /// <returns>The summarized kwh table</returns>
    public static SortedKwhTable SummarizeKwh(SortedKwhTable measures, DateTime startDate, DateTime endDate,
                                                                E_TimeMode timeMode, bool fillGaps)
    {
      //setup start and enddate
      startDate = GetTimePointByMode(timeMode, startDate);
      endDate = GetTimePointByMode(timeMode, endDate);

      //Initialize result list
      SortedKwhTable result;
      result = InitializeResultList(measures, startDate, endDate, timeMode, fillGaps);

      //Build the sum for the measures by each timepoint, depending on the timeMode
      foreach (var item in measures.ToList())
      {
        
        // get target time point where the current kwh should be added to
        DateTime currentTimePoint = GetTimePointByMode(timeMode, item.DateTime);

        //add the kwh of this hour for the inverter
        if (currentTimePoint >= startDate && currentTimePoint < endDate)
        {
          result.GetKwh(currentTimePoint, item.PublicInverterId).Value += item.Value;
        }
        else
        {
          Logger.Log(new ApplicationException(), SeverityLevel.Warning, "kwh value is out of the specified startDate and endate");
        }
      }

      return result;
    }

    /// <summary>
    /// Initializes the resultlist. It can be chosen wether we want to fill gaps or not.
    /// This is important for the "kwh by day" calculation because if we would fill gaps there, kwh_days would be
    /// deleted when there is no more measurement data for the timeframe
    /// </summary>
    /// <param name="measures"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="timeMode"></param>
    /// <param name="systemID"></param>
    /// <param name="fillGaps"></param>
    /// <returns></returns>
    private static SortedKwhTable InitializeResultList(SortedKwhTable measures
                                                                        , DateTime startDate, DateTime endDate
                                                                        , E_TimeMode timeMode
                                                                        , bool fillGaps)
    {
      //init day sum list for the hole time frame
      SortedKwhTable result = new SortedKwhTable();

      // Initialize the result with empty values first
      var countDate = startDate;
      while (countDate < endDate)
      {
        foreach (var inverterInfo in measures.KnownInverters)
        {
          if (fillGaps || ContainsMeasuresForThisTimePoint(measures, countDate, timeMode, inverterInfo.Value))
          {
            MeasureKwH dummy = new MeasureKwH();
            dummy.DateTime = countDate;
            dummy.PrivateInverterId = inverterInfo.Key;
            dummy.PublicInverterId = inverterInfo.Value;
            dummy.Value = 0;
            result.AddMeasure(dummy);
          }
        }

        //increase countdate
        countDate = IncreaseCountDate(timeMode, countDate);
      }
      return result;
    }

    /// <summary>
    /// Returns true if we have a measure which falls into the countdate timeframe
    /// </summary>
    /// <param name="measures"></param>
    /// <param name="countDate"></param>
    /// <param name="timeMode"></param>
    /// <param name="publicInverterID"></param>
    /// <returns></returns>
    private static bool ContainsMeasuresForThisTimePoint(SortedKwhTable measures, DateTime countDate, E_TimeMode timeMode, int publicInverterID)
    {
      foreach (var row in measures.Rows)
      {
        //datetime contains measure for this inverter ID
        if (measures.ContainsMeasureForPublicInverterId(row.Key, publicInverterID))
        {
          DateTime comparabledate = GetCompareableDate(row.Key, timeMode);
          if (countDate == comparabledate)
            return true;
        }

      }
      return false;
    }

    /// <summary>
    /// Crops the datetime that we can compare it. If the timemode is "Day" than all information of the hour will be
    /// set to 0. If the timemode is "Month" all information below day will be set to 1 or 0.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="timeMode"></param>
    /// <returns></returns>
    private static DateTime GetCompareableDate(DateTime dateTime, E_TimeMode timeMode)
    {
      switch (timeMode)
      {
        case E_TimeMode.hour:
          return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
        case E_TimeMode.day:
          return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        case E_TimeMode.month:
          return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0);
        case E_TimeMode.year:
          return new DateTime(dateTime.Year, 1, 0, 0, 0, 0);
        default:
          throw new ApplicationException();
      }
    }


    /// <summary>
    /// Increases the countdate depending on the requested stepping.
    /// </summary>
    /// <param name="timeMode"></param>
    /// <param name="countDate"></param>
    /// <returns></returns>
    private static DateTime IncreaseCountDate(E_TimeMode timeMode, DateTime countDate)
    {
      if (timeMode == E_TimeMode.day)
        countDate = countDate.AddDays(1);
      else if (timeMode == E_TimeMode.month)
        countDate = countDate.AddMonths(1);
      else if (timeMode == E_TimeMode.year)
        countDate = countDate.AddYears(1);
      return countDate;
    }

    /// <summary>
    /// Evaluates the time which is the last one of the current day
    /// </summary>
    /// <param name="timeList"></param>
    /// <param name="current"></param>
    /// <returns></returns>
    private static DateTime GetLastMeasureTimeOfHour(IList<DateTime> timeList, DateTime current)
    {
      DateTime result = new DateTime();

      for (int i = timeList.Count - 1; i >= 0; i--)
      {
        if (current.Year == timeList[i].Year
            && current.Month == timeList[i].Month
            && current.Day == timeList[i].Day
            && current.Hour == timeList[i].Hour)
        {
          result = timeList[i];
          break;
        }
      }
      return result;
    }

    private static DateTime ExtractHourDateTime(IMeasure lastMeasure)
    {
      DateTime last = lastMeasure.DateTime;
      return new DateTime(last.Year, last.Month, last.Day, last.Hour, 0, 0);
    }

    private static bool AreInSameHour(IMeasure measure, DateTime hourToCheck)
    {
      var current = hourToCheck;
      var last = measure.DateTime;
      var currentHour = new DateTime(current.Year, current.Month, current.Day, current.Hour, 0, 0);
      var lastHour = new DateTime(last.Year, last.Month, last.Day, last.Hour, 0, 0);
      return currentHour == lastHour;
    }

    private static DateTime LastMeasureTimeOfDay(DateTime currentDay, IList<DateTime> dateList)
    {
      DateTime result = new DateTime();

      for (int i = dateList.Count - 1; i >= 0; i--)
      {
        if (currentDay.Year == dateList[i].Year
            && currentDay.Month == dateList[i].Month
            && currentDay.Day == dateList[i].Day)
        {
          result = dateList[i];
          break;
        }
      }
      return result;
    }

    private static DateTime FirstMeasureTimeOfDay(DateTime currentDay, IList<DateTime> dateList)
    {
      DateTime result = new DateTime();
      foreach (var date in dateList)
      {
        if (currentDay.Year == date.Year
            && currentDay.Month == date.Month
            && currentDay.Day == date.Day)
        {
          result = date;
          break;
        }
      }
      return result;
    }

    internal static SortedList<DateTime, MeasureKwH> FillHourGaps(SortedList<DateTime, MeasureKwH> hourlyKwH
                                                                        , int startHour, int endHour)
    {
      //break if we have no data
      if (hourlyKwH.Count == 0)
      {
        return hourlyKwH;
      }
      int privateInverterID = hourlyKwH.Values.First().PrivateInverterId;
      int publicInverterId = hourlyKwH.Values.First().PublicInverterId;

      DateTime firstMeasureDate = hourlyKwH.First().Value.DateTime;
      DateTime lastMeasureDate = hourlyKwH.Last().Value.DateTime;

      //build startDate and endDate we need
      DateTime startDate = new DateTime(firstMeasureDate.Year
          , firstMeasureDate.Month
          , firstMeasureDate.Day
          , startHour, 0, 0);

      DateTime endDate = new DateTime(lastMeasureDate.Year
          , lastMeasureDate.Month
          , lastMeasureDate.Day
          , endHour, 0, 0);

      //counter
      var countDate = startDate;

      while (countDate <= endDate)
      {
        if (!hourlyKwH.ContainsKey(countDate))
        {
          MeasureKwH dummyMeasure = new MeasureKwH();
          dummyMeasure.DateTime = countDate;
          dummyMeasure.Value = 0;
          dummyMeasure.PrivateInverterId = privateInverterID;
          dummyMeasure.PublicInverterId = publicInverterId;
          hourlyKwH.Add(countDate, dummyMeasure);
        }

        //increase hour until endHour, then jump to the next day
        countDate = countDate.AddHours(1);
        if (countDate.Hour > endHour)
        {
          countDate = countDate.AddHours(24 - countDate.Hour + startHour);
        }
      }
      return hourlyKwH;
    }



    private static DateTime GetTimePointByMode(E_TimeMode timeMode, DateTime currentDateTime)
    {
      DateTime currentTimePoint;
      switch (timeMode)
      {
        case E_TimeMode.day:
          currentTimePoint = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day);
          break;
        case E_TimeMode.month:
          currentTimePoint = new DateTime(currentDateTime.Year, currentDateTime.Month, 1);
          break;
        case E_TimeMode.year:
          currentTimePoint = new DateTime(currentDateTime.Year, 1, 1);
          break;
        default:
          throw new ApplicationException();
      }
      return currentTimePoint;
    }




  }
}
