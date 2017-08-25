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
        [SetUp]
        public void Setup()
        {
            plantRepositoryMock = new Mock<I_PlantRepository>();
            inactivePlantId4Days = 123;
            UserNotifications.ResetCache();
        }

        private UserNotifications userNotifications;
        private Mock<I_PlantRepository> plantRepositoryMock;
        private int inactivePlantId4Days;
        private SolarPlant solarPlantInactive4Days;
        private SolarPlant solarPlantInactive11Days;

        private void Given_a_plant_which_is_inactive_for_11_days()
        {
            plantRepositoryMock.Setup(x => x.GetAllPlants()).Returns(() =>
            {
                solarPlantInactive11Days = new SolarPlant
                {
                    PlantId = 1234,
                    LastMeasureDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(11)),
                    EmailNotificationsEnabled = true
                };
                return new List<SolarPlant>
                {
                    solarPlantInactive11Days
                };
            });
            userNotifications = new UserNotifications(plantRepositoryMock.Object);
        }

        private void Given_a_plant_which_is_inactive_for_4_days()
        {
            solarPlantInactive4Days = new SolarPlant
            {
                PlantId = inactivePlantId4Days,
                LastMeasureDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(4)),
                EmailNotificationsEnabled = true
            };

            plantRepositoryMock.Setup(x => x.GetAllPlants()).Returns(() => new List<SolarPlant>
            {
                solarPlantInactive4Days
            });
            userNotifications = new UserNotifications(plantRepositoryMock.Object);
        }

        [Test]
        public void Given_a_plant_which_is_first_3_days_off_And_then_11_days_off_The_correct_notificationJobsAreCreated()
        {
            Given_a_plant_which_is_inactive_for_4_days();
            var plantNotification = userNotifications.GetPlantNotifications().First();
            plantNotification.NotificationType.Should().Be(NotificationType.Inactivity3Days);
            solarPlantInactive4Days.LastMeasureDate = DateTime.UtcNow.Date.Subtract(TimeSpan.FromDays(11));
            plantNotification = userNotifications.GetPlantNotifications().First();
            plantNotification.NotificationType.Should().Be(NotificationType.Inactivity10days);
            plantNotification.Done.Should().BeFalse();
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
        public void Given_a_plant_with_no_recent_activity_in_last_3_days_Then_a_notification_is_returned()
        {
            Given_a_plant_which_is_inactive_for_4_days();

            userNotifications = new UserNotifications(plantRepositoryMock.Object);
            var plantNotifications = userNotifications.GetPlantNotifications();

            PlantNotification actualNotification = plantNotifications.First();
            actualNotification.plant.ShouldBeEquivalentTo(solarPlantInactive4Days);
            actualNotification.NotificationType.Should().Be(NotificationType.Inactivity3Days);
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

        [Test]
        public void Given_different_plants_When_there_are_notifications_they_should_only_be_sent_for_plants_where_notifications_are_enabled()
        {
            Given_two_plants_one_with_activated_and_one_with_deactivaed_notifications();
            var plantNotifications = userNotifications.GetPlantNotifications();
            plantNotifications.Count().Should().Be(1);
            plantNotifications.First().plant.EmailNotificationsEnabled.Should().BeTrue();
        }

        private void Given_two_plants_one_with_activated_and_one_with_deactivaed_notifications()
        {
            plantRepositoryMock.Setup(x => x.GetAllPlants()).Returns(() =>
            {
                var solarPlantInactive11DaysDisabled = new SolarPlant
                {
                    EmailNotificationsEnabled = true,
                    PlantId = 1234,
                    LastMeasureDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(11))
                };
                var solarPlantInactive11DaysEnabledNotifications = new SolarPlant
                {
                    EmailNotificationsEnabled = false,
                    PlantId = 1234,
                    LastMeasureDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(11))
                };
                return new List<SolarPlant>
                {
                    solarPlantInactive11DaysDisabled, solarPlantInactive11DaysEnabledNotifications
                };
            });
            userNotifications = new UserNotifications(plantRepositoryMock.Object);

        }
    }
}