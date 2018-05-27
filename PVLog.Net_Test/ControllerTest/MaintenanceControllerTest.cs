namespace solar_tests.ControllerTest
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using Moq;
    using NUnit.Framework;
    using PVLog.Controllers;
    using PVLog.DataLayer;
    using PVLog.Enums;
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
            _maintenanceController.MembershipService = MembershipServiceMock.Object;
            var requestMock = new Mock<HttpRequestBase>();
            requestMock.SetupGet(x => x.IsLocal).Returns(true);

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(requestMock.Object);

            _maintenanceController.ControllerContext = new ControllerContext(context.Object, new RouteData(), _maintenanceController);
            _maintenanceController.AuthorizationOverride = true;
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
        private Mock<IMembershipService> MembershipServiceMock = new Mock<IMembershipService>();

        private IEnumerable<Inverter> DummyInverterList
        {
            get
            {
                var dummyList = new List<Inverter>();
                dummyList.Add(new Inverter
                {
                    InverterId = 4
                });
                return dummyList;
            }
        }


        private void When_The_update_request_is_processed()
        {
            _maintenanceController.UpdateStatistics("1234");
        }

        [Test]
        public void Given_the_update_request_comes_in_Then_the_user_notifications_should_be_asked_for_jobs()
        {
            _plantRepositoryMock.Setup(x => x.GetAllInverters()).Returns(() => DummyInverterList);
            When_The_update_request_is_processed();
            _userNotificationsMock.Verify(x => x.GetPlantNotifications(), Times.Once());
        }

        [Test]
        public void Given_therer_are_user_notifications_to_be_sent_Then_the_email_sender_is_called_as_expected()
        {
            var userMock = new Mock<IUser>();
            userMock.SetupGet(x => x.Email).Returns("test@test.com");
            _plantRepositoryMock.Setup(x => x.GetAllInverters()).Returns(() => DummyInverterList);
            _plantRepositoryMock.Setup(x => x.GetUsersOfSolarPlant(It.IsAny<int>(), It.IsAny<E_PlantRole>())).Returns(() => new List<int>() { 1 });
            MembershipServiceMock.Setup(x => x.GetUser(It.IsAny<int>())).Returns(() => userMock.Object);

            _userNotificationsMock.Setup(x => x.GetPlantNotifications()).Returns(() => new List<PlantNotification>()
            {
                new PlantNotification()
                {
                    Done = false,
                    NotificationType = NotificationType.Inactivity3Days,
                    plant = TestdataGenerator.GetPlant()
                },
                new PlantNotification()
                {
                    Done = false,
                    NotificationType = NotificationType.Inactivity10days,
                    plant = TestdataGenerator.GetPlant()
                },
                new PlantNotification()
                {
                    Done = true,
                    NotificationType = NotificationType.Inactivity3Days,
                    plant = TestdataGenerator.GetPlant()
                }
            });
            When_The_update_request_is_processed();
            _emailSenderMock.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }
    }
}