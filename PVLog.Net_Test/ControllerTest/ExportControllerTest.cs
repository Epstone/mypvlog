using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using PVLog.DataLayer;
using PVLog.Controllers;
using NUnit.Framework;
using PVLog.Models;

namespace solar_tests.ControllerTest
{
  public class ExportControllerTest
  {
    ExportController _exportController;
    Mock<I_PlantRepository> _plantRepositoryMock;
    Mock<IMeasureRepository> _measureRepositoryMock;

    [SetUp]
    public void Setup()
    {
      _plantRepositoryMock = new Mock<I_PlantRepository>();
      _measureRepositoryMock = new Mock<IMeasureRepository>();

      _exportController = new ExportController(_measureRepositoryMock.Object,
                                          _plantRepositoryMock.Object);
    }


    [Test]
    public void min_day_Export_Test()
    {
      // setup mock to return 2 inverters 
      _plantRepositoryMock.Setup(x => x.GetAllInvertersByPlant(It.IsAny<int>())).Returns(new Inverter[]{ 
        new Inverter(){
          PublicInverterId = 1,
        },
        new Inverter(){
          PublicInverterId = 2}});

      //setup mock to return some test measures
      _measureRepositoryMock.Setup(x => x.GetMinuteWiseMeasures(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
        .Returns(
        (DateTime startDate, DateTime endDate, int inverterId)
          => { return TestdataGenerator.GetMeasureList(startDate, endDate, 1000, inverterId); });


      Console.WriteLine(_exportController.min_day(1).Script);

    }
  }
}
