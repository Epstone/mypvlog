namespace PVLog.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using DataLayer;
    using Enums;
    using Management;
    using Utility;

    public class MaintenanceController : MyController
    {
        private readonly IUserNotifications userNotifications;
        private readonly IEmailSender emailSender;
        private readonly MeasureManagement _measureManagement = new MeasureManagement();

        public MaintenanceController(IMeasureRepository measureRepository, I_PlantRepository plantRepo, IUserNotifications userNotifications, IEmailSender emailSender)
        {
            this.userNotifications = userNotifications;
            this.emailSender = emailSender;
            _measureRepository = measureRepository;
            _plantRepository = plantRepo;
        }

        //
        // GET: /Maintenance/
        public ActionResult UpdateStatistics(string pw)
        {
            if (!isAuthorized(pw))
            {
                Logger.LogWarning("not allowed update statistics request.");
                return View("NotLocal");
            }

            //only update between 4am and 23pm
            Logger.LogInfo("Received maintenance call.");

            UpdateStatistics();
            SendNotifications();

            Logger.LogInfo("Finished update statistics request.");
            return new EmptyResult();
        }

        public ActionResult UpdatePlantOnlineStatus(string pw)
        {
            if (!isAuthorized(pw))
            {
                Logger.LogWarning("not allowed update plant online status request.");
                return View("NotLocal");
            }

            Stopwatch watch = Stopwatch.StartNew();

            _plantRepository.UpdatePlantOnlineStatus();
            Logger.TrackMetric("Plant online status update", watch.Elapsed.TotalSeconds);

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

        private string GetSubject(PlantNotification plantNotification)
        {
            switch (plantNotification.NotificationType)
            {
                case NotificationType.Inactivity3Days:
                    return $"PV-Anlage '{plantNotification.plant.Name}' seit 3 Tagen inaktiv";
                case NotificationType.Inactivity10days:
                    return $"PV-Anlage '{plantNotification.plant.Name}' seit 10 Tagen inaktiv";
                case NotificationType.PlantOnlineAgain:
                    return $"PV-Anlage '{plantNotification.plant.Name}' wieder online";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SendNotifications()
        {

            var plantNotifications = userNotifications.GetPlantNotifications().Where(x => !x.Done);
            foreach (var plantNotification in plantNotifications.Where(x => !x.Done))
            {
                var recipientUserId = _plantRepository.GetUsersOfSolarPlant(plantNotification.plant.PlantId, E_PlantRole.Owner).First();
                string body = $"Ihre PV Anlage hat zuletzt am {plantNotification.plant.LastMeasureDate.ToLocalTime()} Daten gesendet";
                string subject = GetSubject(plantNotification);
                var user = MembershipService.GetUser(recipientUserId);

                emailSender.Send(body, subject, user.Email);
            }
        }

        private void UpdateStatistics()
        {
            DateTime startTime = DateTimeUtils.GetTodaysDate().AddHours(4);
            DateTime endTime = DateTimeUtils.GetTodaysDate().AddHours(23);
            var totalStopWatch = Stopwatch.StartNew();


            try
            {
                if (IsInTimeRange(startTime, endTime))
                {
                    //calculate the minutewise wattage measures for today
                    var minuteWiseStopwatch = Stopwatch.StartNew();

                    Logger.TrackMetric("Aggregate measures: minutewise", minuteWiseStopwatch.Elapsed.TotalSeconds);

                    //Recalculate the kwh for today
                    var kwhStopwatch = Stopwatch.StartNew();
                    UpdateTodaysKwhValues();

                    Logger.TrackMetric("Aggregate measures: Todays kWh", kwhStopwatch.Elapsed.TotalSeconds);

                    // log total time
                    Logger.TrackMetric("Total update time", totalStopWatch.Elapsed.TotalSeconds);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
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