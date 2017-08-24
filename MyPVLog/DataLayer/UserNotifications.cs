namespace PVLog.DataLayer
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using Models;

    public class UserNotifications : IDisposable
    {
        private readonly I_PlantRepository plantRepository;

        public UserNotifications(I_PlantRepository plantRepository)
        {
            this.plantRepository = plantRepository;
        }

        static ConcurrentDictionary<int, PlantNotification> recentNotifications = new ConcurrentDictionary<int, PlantNotification>();
        public IEnumerable<PlantNotification> GetPlantNotifications()
        {
            var plants = plantRepository.GetAllPlants().ToList();
            var result = plants.Where(PlantWithActivityBetween3And10Days)
                .Select(plant => CreatePlantNotification(plant, NotificationType.Inactivity3Days)).ToList();

            var solarPlants10DaysInactive = plants.Where(PlantWithActivityOlderThan10Days)
                .Select(plant => CreatePlantNotification(plant, NotificationType.Inactivity10days));

            result.AddRange(solarPlants10DaysInactive);

            result.ForEach(MarkJobsDone);

            result.ForEach(notification => recentNotifications.AddOrUpdate(notification.plant.PlantId, (id) => notification, (id, update) => notification));
            return result;
        }

        private void MarkJobsDone(PlantNotification job)
        {
            if (!recentNotifications.ContainsKey(job.plant.PlantId))
            {
                return;
            }

            var recentNotification = recentNotifications[job.plant.PlantId];
            if (recentNotification.NotificationType == job.NotificationType)
            {
                job.Done = true;
            }
        }

        private static bool PlantWithActivityBetween3And10Days(SolarPlant x)
        {
            DateTime before3Days = DateTime.UtcNow.Subtract(TimeSpan.FromDays(3));
            DateTime before10Days = DateTime.UtcNow.Subtract(TimeSpan.FromDays(10));
            return x.LastMeasureDate < before3Days && x.LastMeasureDate > before10Days;
        }

        private static bool PlantWithActivityOlderThan10Days(SolarPlant x)
        {
            DateTime before10Days = DateTime.UtcNow.Subtract(TimeSpan.FromDays(10));
            return x.LastMeasureDate < before10Days;
        }

        private static PlantNotification CreatePlantNotification(SolarPlant solarPlant, NotificationType inactivity3Days)
        {
            return new PlantNotification()
            {
                NotificationType = inactivity3Days,
                plant = solarPlant
            };
        }

        public void Dispose()
        {
            plantRepository?.Dispose();
        }

        public static void ResetCache()
        {
            recentNotifications.Clear();
        }
    }


    public class PlantNotification
    {
        public SolarPlant plant { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool Done { get; set; }
    }

    public enum NotificationType
    {
        Inactivity3Days, Inactivity10days, PlantOnlineAgain
    }
}