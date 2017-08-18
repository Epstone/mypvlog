using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PVLog.Controllers;
using PVLog.DataLayer;
using Moq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PVLog.Models;

namespace solar_tests.ControllerTest
{
  [TestFixture]
  public class MaintenanceControllerTest
  {

    MaintenanceController _maintenanceController;
    Mock<I_MeasureRepository> _measureRepositoryMock;
    Mock<I_PlantRepository> _plantRepositoryMock;


    [SetUp]
    public void Setup()
    {
      _measureRepositoryMock = new Mock<I_MeasureRepository>();
      _plantRepositoryMock = new Mock<I_PlantRepository>();
      _maintenanceController = new MaintenanceController(_measureRepositoryMock.Object, _plantRepositoryMock.Object);

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
      _measureRepositoryMock.Verify(x => x.UpdateTemporaryToMinuteWise(It.IsAny<int>()), Times.Once());

    }

    private IEnumerable<PVLog.Models.Inverter> GetDummyInverterList()
    {
      var dummyList = new List<Inverter>();
      dummyList.Add(new Inverter()
      {
        InverterId = 4
      });
      return dummyList;
    }

  }
}
