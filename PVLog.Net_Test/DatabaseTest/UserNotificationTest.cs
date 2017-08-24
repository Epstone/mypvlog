namespace solar_tests.DatabaseTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using PVLog.DataLayer;
    using PVLog.Models;

    [TestFixture]
    public class UserNotificationTest
    {
        private UserNotifications userNotifications;
        private Mock<I_PlantRepository> plantRepositoryMock;
        private int inactivePlantId4Days;
        private SolarPlant solarPlantInactive4Days;
        private SolarPlant solarPlantInactive11Days;

        [SetUpAttribute]
        public void Setup()
        {
            plantRepositoryMock = new  Mock<I_PlantRepository>();
            inactivePlantId4Days = 123;
            UserNotifications.ResetCache();
        }

        [Test]
        public void Given_a_plant_with_no_recent_activity_in_last_3_days_Then_a_notification_is_returned()
        {
            Given_a_plant_which_is_inactive_for_4_days();

            this.userNotifications = new UserNotifications(plantRepositoryMock.Object);
            var plantNotifications = userNotifications.GetPlantNotifications();

            PlantNotification actualNotification = plantNotifications.First();
            actualNotification.plant.ShouldBeEquivalentTo(solarPlantInactive4Days);
            actualNotification.NotificationType.Should().Be(NotificationType.Inactivity3Days);
        }

        [Test]
        public void Given_a_plant_with_no_recent_activity_in_last_11_days_Then_a_notification_for_10_days_is_returned()
        {
            Given_a_plant_which_is_inactive_for_11_days();

            var plantNotifications = userNotifications.GetPlantNotifications();

            PlantNotification actualNotification = plantNotifications.First();
            actualNotification.plant.ShouldBeEquivalentTo(solarPlantInactive11Days);
            actualNotification.NotificationType.Should().Be(NotificationType.Inactivity10days);
        }

        [Test]
        public void Given_the_get_notification_jobs_call_is_done_twice_Then_an_old_job_is_marked_as_done()
        {
            Given_a_plant_which_is_inactive_for_11_days();
            var plantNotifications = userNotifications.GetPlantNotifications();
            plantNotifications.First().Done.Should().BeFalse();
            plantNotifications = userNotifications.GetPlantNotifications();
            plantNotifications.First().Done.Should().BeTrue();
        }

        private void Given_a_plant_which_is_inactive_for_11_days()
        {
            this.plantRepositoryMock.Setup(x => x.GetAllPlants()).Returns(() =>
            {
                solarPlantInactive11Days = new SolarPlant()
                {
                    PlantId = 1234,
                    LastMeasureDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(11)),
                };
                return new List<SolarPlant>()
                {
                    solarPlantInactive11Days,
                };
            });
            this.userNotifications = new UserNotifications(plantRepositoryMock.Object);

        }

        private void Given_a_plant_which_is_inactive_for_4_days()
        {
            this.plantRepositoryMock.Setup(x => x.GetAllPlants()).Returns(() =>
            {
                solarPlantInactive4Days = new SolarPlant()
                {
                    PlantId = inactivePlantId4Days,
                    LastMeasureDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(4)),
                };
                return new List<SolarPlant>()
                {
                    solarPlantInactive4Days,
                };
            });
            this.userNotifications = new UserNotifications(plantRepositoryMock.Object);
        }
    }
}