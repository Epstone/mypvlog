namespace PVLog.Controllers
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Web.Mvc;
    using DataLayer;
    using Management;
    using Utility;

    public class MaintenanceController : MyController
    {
        private readonly MeasureManagement _measureManagement = new MeasureManagement();

        public MaintenanceController()
        {
        }

        public MaintenanceController(I_MeasureRepository measureRepository, I_PlantRepository plantRepo)
        {
            _measureRepository = measureRepository;
            _plantRepository = plantRepo;
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
            DateTime startTime = DateTimeUtils.GetTodaysDate().AddHours(4);
            DateTime endTime = DateTimeUtils.GetTodaysDate().AddHours(23);
            var totalStopWatch = new Stopwatch();

            try
            {
                if (IsInTimeRange(startTime, endTime))
                {
                    //calculate the minutewise wattage measures for today
                    var minuteWiseStopwatch = Stopwatch.StartNew();


                    foreach (var inverter in _plantRepository.GetAllInverters())
                    {
                        _measureRepository.AggregateTemporaryToMinuteWiseMeasures(inverter.InverterId);
                    }

                    Logger.TrackMetric("Aggregate measures: minutewise", minuteWiseStopwatch.Elapsed.TotalSeconds);

                    //Recalculate the kwh for today
                    var kwhStopwatch = Stopwatch.StartNew();
                    UpdateTodaysKwhValues();

                    Logger.TrackMetric("Aggregate measures: Todays kWh", kwhStopwatch.Elapsed.TotalSeconds);

                    // update plants online status
                    var onlineStatusWatch = Stopwatch.StartNew();
                    _plantRepository.UpdatePlantOnlineStatus();
                    Logger.TrackMetric("Update plant online status", onlineStatusWatch.Elapsed.TotalSeconds);

                    // log total time
                    Logger.TrackMetric("Total update time", totalStopWatch.Elapsed.TotalSeconds);
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

        public bool AuthorizationOverride { get; set; }

        private bool isAuthorized(string pw)
        {
            if (AuthorizationOverride)
            {
                return true;
            }

            var maintenancePassword = ConfigurationManager.AppSettings["maintenance-password"];

            return !string.IsNullOrEmpty(pw) && pw.Equals(maintenancePassword);
        }

        private bool IsInTimeRange(DateTime startTime, DateTime endTime)
        {
            return DateTimeUtils.GetGermanNow() > startTime && DateTimeUtils.GetGermanNow() < endTime;
        }

        private void UpdateTodaysKwhValues()
        {
            var plantRepo = new PlantRepository();
            var today = DateTimeUtils.GetTodaysDate();

            foreach (var plant in plantRepo.GetAllPlants())
            {
                _measureManagement.ReCalculateKwh_by_dayToDB(today, today.AddDays(1), plant.PlantId);
            }
        }
    }
}