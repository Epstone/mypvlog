using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVLog.Management;
using PVLog.Utility;
using PVLog.DataLayer;
using System.Configuration;

namespace PVLog.Controllers
{
  public class MaintenanceController : MyController
  {
    MeasureManagement _measureManagement = new MeasureManagement();

    public MaintenanceController()
    {

    }

    public MaintenanceController(I_MeasureRepository measureRepository, I_PlantRepository plantRepo)
    {
      base._measureRepository = measureRepository;
      base._plantRepository = plantRepo;
    }

    //
    // GET: /Maintenance/
    public ActionResult UpdateStatistics(string pw)
    {
      if (!isAuthorized(pw))
      {
        return View("NotLocal");
      }

      //only update between 4am and 23pm
      DateTime startTime = Utils.GetTodaysDate().AddHours(4);
      DateTime endTime = Utils.GetTodaysDate().AddHours(23);
      var totalStopWatch = new Stopwatch();

      try
      {
        if (IsInTimeRange(startTime, endTime))
        {
          //calculate the minutewise wattage measures for today
          var minuteWiseStopwatch = new Stopwatch();


          foreach (var inverter in _plantRepository.GetAllInverters())
          {
            _measureRepository.UpdateTemporaryToMinuteWise(inverter.InverterId);

          }

          Logger.LogInfo("Recalculate Minutewise sec: " + minuteWiseStopwatch.LifeTime.TotalSeconds);

          //Recalculate the kwh for today
          var kwhStopwatch = new Stopwatch();
          UpdateTodaysKwhValues();
          Logger.LogInfo("Update todays kwh sec: " + kwhStopwatch.LifeTime.TotalSeconds);

          // update plants online status
          var onlineStatusWatch = new Stopwatch();
          _plantRepository.UpdatePlantOnlineStatus();
          Logger.LogInfo("Update plant online status sec: " + onlineStatusWatch.LifeTime.TotalSeconds);

          // log total time
          Logger.LogInfo("Total: " + totalStopWatch.LifeTime.TotalSeconds + " sec");
        }
      }
      catch (Exception ex)
      {
        Logger.LogError(ex);
        throw;
      }
      finally
      {
        _measureManagement.CleanUp();
      }

      return new EmptyResult();
    }

    private static bool isAuthorized(string pw)
    {
      var maintenancePassword = ConfigurationManager.AppSettings["maintenance-password"];
      
      return !string.IsNullOrEmpty(pw) && pw.Equals(maintenancePassword);
    }

    [HttpGet]
    public ActionResult RemoveOldMeasures(string pw)
    {
      if (!isAuthorized(pw))
      {
        return View("NotLocal");
      }

      var dayCount = MySettings.KeepMeasureDayCount;
      _measureRepository.RemoveMeasuresOlderThan(dayCount);

      return new EmptyResult();
    }

    private bool IsInTimeRange(DateTime startTime, DateTime endTime)
    {
      return (Utils.GetGermanNow() > startTime) && (Utils.GetGermanNow() < endTime);
    }

    private void UpdateTodaysKwhValues()
    {
      var plantRepo = new PlantRepository();
      var today = Utils.GetTodaysDate();

      foreach (var plant in plantRepo.GetAllPlants())
      {
        _measureManagement.ReCalculateKwh_by_dayToDB(today, today.AddDays(1), plant.PlantId);
      }
    }


  }
}
