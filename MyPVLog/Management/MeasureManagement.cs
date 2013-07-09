using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PVLog.DataLayer;
using PVLog.Utility;
using PVLog.Models;
using System.Data;
using PVLog.InputProcessing;

namespace PVLog.Management
{
  public class MeasureManagement
  {
    I_MeasureRepository _measureRepository = new MeasureRepository();
    KwhRepository _kwhDb = new KwhRepository();
    PlantRepository _plantRepo = new PlantRepository();

    /// <summary>
    /// Builds a kwh table which contains hourly kwh values for each inverter id
    /// </summary>
    /// <param name="startDate">Defines the beginning of the time range for the result.</param>
    /// <param name="endDate">Defines the end of the time range for the result</param>
    /// <param name="inverterIDs">The inverter ID's which should be contained in the result kwh table</param>
    /// <returns>A hourly kwh table with the provided time range and inverter ID's</returns>
    public SortedKwhTable GetkwhHourlyByTimeFrame(DateTime startDate, DateTime endDate, List<int> inverterIDs)
    {
      SortedKwhTable result = new SortedKwhTable();


      foreach (var inverterID in inverterIDs)
      {
        var measures = _measureRepository.GetMinuteWiseMeasures(startDate, endDate, inverterID).Cast<IMeasure>().ToList();
        SortedList<DateTime, MeasureKwH> kwhByHours = KwhCalculator.GetKwhHourly(measures, MySettings.DiagDay_StartHour, MySettings.DiagDay_EndHour);

        foreach (var kwhByHour in kwhByHours.Values)
        {
          result.AddMeasure(kwhByHour);
        }
      }

      return result;
    }

    

    /// <summary>
    /// Recalculates the kwh values for the specified power plant in a given time range
    /// </summary>
    /// <param name="startDate">Defines the start of the time range.</param>
    /// <param name="endDate">Defines the end of the time range.</param>
    /// <param name="plantId">The plant which data should be recalculated</param>
    public void ReCalculateKwh_by_dayToDB(DateTime startDate, DateTime endDate, int plantId)
    {

      startDate = Utils.CropHourMinuteSecond(startDate);
      endDate = Utils.CropHourMinuteSecond(endDate);

      List<int> inverterIDs = _plantRepo.GetPrivateInverterIdsByPlant(plantId);
      var hourly = GetkwhHourlyByTimeFrame(startDate, endDate, inverterIDs);

      SortedKwhTable result = KwhCalculator.SummarizeKwh(hourly, startDate, endDate, Enums.E_TimeMode.day, false);

      foreach (var measureKwh in result.ToList())
      {
        var measureToAdd = measureKwh as MeasureKwH;
        _kwhDb.InsertDayKwh(measureToAdd);
      }

    }

    public void CleanUp()
    {
      _kwhDb.Cleanup();
      _measureRepository.Cleanup();
      _plantRepo.Cleanup();
    }

  }

}