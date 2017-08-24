﻿namespace solar_tests.ControllerTest
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Moq;
    using NUnit.Framework;
    using PVLog.Controllers;
    using PVLog.DataLayer;
    using PVLog.Models;

    [TestFixture]
    public class MaintenanceControllerTest
    {
        [SetUp]
        public void Setup()
        {
            _measureRepositoryMock = new Mock<I_MeasureRepository>();
            _plantRepositoryMock = new Mock<I_PlantRepository>();
            _userNotificationsMock = new Mock<IUserNotifications>();
            _emailSenderMock = new Mock<IEmailSender>();
            _maintenanceController = new MaintenanceController(_measureRepositoryMock.Object, _plantRepositoryMock.Object, _userNotificationsMock.Object, _emailSenderMock.Object);
        }


        [TearDown]
        public void TearDown()
        {
            _maintenanceController.Dispose();
        }

        private MaintenanceController _maintenanceController;
        private Mock<I_MeasureRepository> _measureRepositoryMock;
        private Mock<I_PlantRepository> _plantRepositoryMock;
        private Mock<IUserNotifications> _userNotificationsMock;
        private Mock<IEmailSender> _emailSenderMock;

        private IEnumerable<Inverter> GetDummyInverterList()
        {
            var dummyList = new List<Inverter>();
            dummyList.Add(new Inverter
            {
                InverterId = 4
            });
            return dummyList;
        }

        [Test]
        public void UpdateStatisticsTest()
        {
            // Setup Controller and Context
            var requestMock = new Mock<HttpRequestBase>();
            requestMock.SetupGet(x => x.IsLocal).Returns(true);

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(requestMock.Object);

            _maintenanceController.ControllerContext = new ControllerContext(context.Object, new RouteData(), _maintenanceController);
            _maintenanceController.AuthorizationOverride = true;

            // setup plant repository
            _plantRepositoryMock.Setup(x => x.GetAllInverters()).Returns(() => GetDummyInverterList());

            _maintenanceController.UpdateStatistics("1234");

            //verify that the minute wise calculation process is started
            _measureRepositoryMock.Verify(x => x.AggregateTemporaryToMinuteWiseMeasures(It.IsAny<int>()), Times.Once());

            _userNotificationsMock.Verify(x=>x.GetPlantNotifications(),Times.Once());

        }
    }
}