using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PVLog.Controllers;
using Moq;
using PVLog.DataLayer;
using PVLog.Models;
using System.Web.Mvc;
using System.Web.Security;



namespace solar_tests.ControllerTest
{
    [TestFixture]
    public class PlantControllerTest
    {

        PlantController _plantController;
        Mock<I_PlantRepository> _plantRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _plantRepositoryMock = new Mock<I_PlantRepository>();
            _plantController = new PlantController((I_PlantRepository)_plantRepositoryMock.Object);
        }

        [Test]
        public void AddPlantTest()
        {
          var userId = 1337;

          //setup membership service
          var membershipMock = new Mock<IMembershipService>();
          membershipMock.SetupGet(x => x.CurrentUserId).Returns(userId);
          _plantController.MembershipService = membershipMock.Object;

          
            var plantModel = new SolarPlant()
            {
                Name = "test_plant",
                Password = "123456"
            };

            _plantController.Add(plantModel, true);
            _plantRepositoryMock.Verify(x => x.CreatePlant(plantModel), Times.Once());
            _plantRepositoryMock.Verify(x => x.StoreUserPlantRelation(userId, It.IsAny<int>(), PVLog.Enums.E_PlantRole.Owner), Times.Once());
        }
    }
}
