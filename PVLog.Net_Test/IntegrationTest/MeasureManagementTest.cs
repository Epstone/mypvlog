using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Web;
using PVLog.Management;
using PVLog.DataLayer;
using PVLog;
using PVLog.Utility;
using solar_tests.DatabaseTest;
using PVLog.OutputProcessing;
using PVLog.Enums;
using PVLog.Models;
using solar_tests;

namespace zPVLogIntegrationTests.ManagementTest
{
  [TestFixture]
  public class zMeasureManagementTest
  {

    TestDbSetup _testDb;
    I_MeasureRepository _measureRepository;
    [SetUp]
    public void Init()
    {
      _testDb = new TestDbSetup();
      _testDb.TruncateAllTables();

      _measureRepository = new MeasureRepository();
    }

    /// <summary>
    /// Tests the kwh by day calculation workflow from minutewise measures to daywise kwh
    /// </summary>
    [Test]
    public void CalculateKwh_by_dayToDBTest()
    {
      var plantDb = new PlantRepository();
      int testPlantId = DatabaseHelpers.CreatePlantGetId();
      int privateInverterID = DatabaseHelpers.CreateInverter(testPlantId);
      int publicInverterId = plantDb.GetAllInvertersByPlant(testPlantId).First().PublicInverterId;


      var today = DateTimeUtils.GetTodaysDate();
      var tomorow = today.AddDays(1);
      var dayAfterTomorow = tomorow.AddDays(1);

      //time frame for kwh calculation
      var timeFrameStart = DateTimeUtils.CropHourMinuteSecond(today);
      var timeFrameEnd = timeFrameStart.AddDays(3);

      //time frame of test measures
      var todayStart = today.AddHours(8);
      var todayEnd = todayStart.AddHours(10);

      var tomorowStart = todayStart.AddDays(1);
      var tomorowEnd = tomorowStart.AddHours(8);

      var dayAfterTomorowStart = tomorowStart.AddDays(1);
      var dayAfterTomorowEnd = dayAfterTomorowStart.AddHours(8);

      //create test data and merge into one list
      var measureList = TestdataGenerator.GetMeasureList(todayStart, todayEnd, 1000, privateInverterID);
      var measureList_tomorow = TestdataGenerator.GetMeasureList(tomorowStart, tomorowEnd, 2000, privateInverterID);
      var measureList_dayAfterTomorow = TestdataGenerator.GetMeasureList(dayAfterTomorowStart, dayAfterTomorowEnd, 3000, privateInverterID);

      measureList.AddRange(measureList_tomorow);
      measureList.AddRange(measureList_dayAfterTomorow);

      var measureDb = new MeasureRepository();

      //Add test data to database

      measureDb.StartTransaction();
      measureList.ForEach(y=> _measureRepository.InsertMeasure(y));
      measureDb.CommitTransaction();

      //calculate kwh for each day and store in database
      var mgm = new MeasureManagement();
      mgm.ReCalculateKwh_by_dayToDB(timeFrameStart, timeFrameEnd, testPlantId);

      //read the calculated kwh days from the db
      var kwhDb = new KwhRepository();
      var actual = kwhDb.GetDayKwhByDateRange(timeFrameStart, timeFrameEnd, testPlantId);

      //Check the result from the db
      Assert.AreEqual(3, actual.Count);
      Assert.AreEqual(10.0, actual.GetKwh(today, publicInverterId).Value);
      Assert.AreEqual(16.0, actual.GetKwh(tomorow, publicInverterId).Value);
      Assert.AreEqual(24.0, actual.GetKwh(dayAfterTomorow, publicInverterId).Value);


      measureDb.StartTransaction();
      measureList_dayAfterTomorow.ForEach(y=>  measureDb.InsertMeasure(y));
      measureDb.CommitTransaction();

      //recalculate the kwh_days
      mgm.ReCalculateKwh_by_dayToDB(timeFrameStart, timeFrameEnd, testPlantId);
      var actual_2 = kwhDb.GetDayKwhByDateRange(timeFrameStart, timeFrameEnd, testPlantId);


      //results should be equal with those above
      Assert.AreEqual(3, actual.Count);
      Assert.AreEqual(10.0, actual.GetKwh(today, publicInverterId).Value);
      Assert.AreEqual(16.0, actual.GetKwh(tomorow, publicInverterId).Value);
      Assert.AreEqual(24.0, actual.GetKwh(dayAfterTomorow, publicInverterId).Value);
    }


  }
}
