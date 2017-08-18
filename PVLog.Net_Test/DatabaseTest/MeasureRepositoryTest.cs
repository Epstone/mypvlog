using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PVLog;
using NUnit.Framework;
using System.Data;
using PVLog.Models;
using PVLog.DataLayer;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.IO;
using PVLog.Utility;

namespace solar_tests.DatabaseTest
{
  [TestFixture]
  public class MeasureRepositoryTest
  {

    TestDbSetup _testDb;
    I_MeasureRepository _measureRepository;
    I_PlantRepository _plantRepository;

    [SetUp]
    public void Setup()
    {
      _testDb = new TestDbSetup();

      _testDb.TruncateAllTables();
      _measureRepository = new MeasureRepository();
      _plantRepository = new PlantRepository();

    }

    [Test]
    public void StoreMeasureTest()
    {
      var plantId = DatabaseHelpers.CreatePlantGetId();
      var inverterId = DatabaseHelpers.CreateInverter(plantId);

      Measure expected_1 = TestdataGenerator.GetTestMeasure(plantId);
      expected_1.PrivateInverterId = inverterId;

      //insert measure
      _measureRepository.InsertMeasure(expected_1);
    }

    [Test]
    public void StoreMeasureWithNullValues()
    {
      var plant = DatabaseHelpers.CreatePlantWithOneInverter();

      Measure measure = new Measure()
      {
        PlantId = plant.PlantId,
        PrivateInverterId = plant.InverterId,
        OutputWattage = 1337,
        GeneratorWattage = 1234,
        GridAmperage = 1234
      };

      _measureRepository.InsertMeasure(measure);

      var actual = _measureRepository.GetMinuteWiseMeasures(plant.InverterId).First();

      Assert.AreEqual(measure, actual);

    }

    /// <summary>
    /// Tests the minute wise wattage generation 
    /// </summary>
    [Test]
    public void GenerateMinutewiseMeasuresTest()
    {
      var plant = DatabaseHelpers.CreatePlantWithOneInverter();

      var referenceMeasure = TestdataGenerator.GetTestMeasure(plant.PlantId);

      //generate measures for 5 minutes. 10 each minute.
      var now = Utils.GetWith0Second(DateTime.Now);
      ((MeasureRepository)_measureRepository).StartTransaction();
      for (int i = 1; i < 6; i++)
      {
        for (int j = 0; j < 10; j++)
        {
          referenceMeasure.DateTime = now.AddMinutes(i).AddSeconds(j);
          referenceMeasure.OutputWattage = 1000 + j;
          referenceMeasure.PrivateInverterId = plant.InverterId;

          _measureRepository.InsertTemporary(referenceMeasure);
        }
      }
      ((MeasureRepository)_measureRepository).CommitTransaction();

      //move the temporary power measures into minute_wise power table
      _measureRepository.UpdateTemporaryToMinuteWise(plant.InverterId);

      //check that the average is 1004.5 W foreach minute and we have 4 minute wise result values
      // 4 minute_wise values because the last minute may not be over yet.
      IEnumerable<Measure> result = _measureRepository.GetMinuteWiseMeasures(plant.InverterId);

      Assert.AreEqual(4, result.Count());
      foreach (var measure in result)
      {
        Assert.AreEqual(1004.5, measure.Value);
        Assert.Greater(measure.DateTime, DateTime.Now.AddMinutes(-10));
        Assert.Less(measure.DateTime, DateTime.Now.AddMinutes(10));
        Assert.AreEqual(referenceMeasure.GeneratorAmperage, measure.GeneratorAmperage);
        Assert.AreEqual(referenceMeasure.GeneratorVoltage, measure.GeneratorVoltage);
        Assert.AreEqual(referenceMeasure.GeneratorWattage, measure.GeneratorWattage);
        Assert.AreEqual(referenceMeasure.GridAmperage, measure.GridAmperage);
        Assert.AreEqual(referenceMeasure.GridVoltage, measure.GridVoltage);
        Assert.AreNotEqual(0, measure.MeasureId);
        Assert.AreEqual(referenceMeasure.PlantId, measure.PlantId);
        Assert.AreEqual(referenceMeasure.PrivateInverterId, measure.PrivateInverterId);
        Assert.AreNotEqual(0, measure.PublicInverterId);
        Assert.AreEqual(null, measure.SystemStatus);
        Assert.AreEqual(referenceMeasure.Temperature, measure.Temperature);
      }

    }




    [Test]
    public void FlotChartDataTest()
    {

      var plant = DatabaseHelpers.CreatePlantWithOneInverter();

      var measures = TestdataGenerator.GetMeasureListWattageOnly(DateTime.Now, DateTime.Now.AddMinutes(11), 1000, plant.InverterId);

      //fill measure table with some measures
      foreach (var measure in measures)
      {
        _measureRepository.InsertMeasure(measure);
      }

      // flot data cumulated 
      var actual = _measureRepository.GetCumulatedMinuteWiseWattageChartData(plant.PlantId, DateTime.Now);
      Assert.Greater(actual.data.Count, 10);

      // flot data inverter wise
      var actual2 = _measureRepository.GetInverterWiseMinuteWiseWattageChartData(plant.PlantId, DateTime.Now);
      Assert.AreEqual(1, actual2.Count); // one inverter
      Assert.Greater(actual2.First().data.Count, 10); // 11 measures

    }


    [Test]
    public void EmptyGaugeDataTest()
    {
      // don't throw an exception at the client!!!
      var plant = DatabaseHelpers.CreatePlantWithOneInverter();

      var result = _measureRepository.GetLatestMeasuresByPlant(plant.PlantId);

      Assert.IsTrue(result.Count == 0);
    }

    [Test]
    public void LoggerTest()
    {
      PVLog.Utility.Logger.LogError(new ApplicationException());
    }



  }
}
