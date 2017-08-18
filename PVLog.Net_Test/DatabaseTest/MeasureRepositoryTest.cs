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
    using FluentAssertions;

    [TestFixture]
    public class MeasureRepositoryTest
    {
        TestDbSetup _testDb;
        MeasureRepository _measureRepository;
        PlantRepository _plantRepository;

        [SetUp]
        public void Setup()
        {
            _testDb = new TestDbSetup();

            _testDb.TruncateAllTables();
            _measureRepository = new MeasureRepository();
            _plantRepository = new PlantRepository();
        }

        [TearDown]
        public void TearDown()
        {
            _measureRepository.Dispose();
            _plantRepository.Dispose();
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


        [Test]
        public void
            Given_measures_for_seconds_of_a_minute_When_I_aggregate_Then_they_are_aggregated_to_a_minute_measurement()
        {
            var plant = DatabaseHelpers.CreatePlantWithOneInverter();

        }

        /// <summary>
        /// Tests the minute wise wattage generation 
        /// </summary>
        [Test]
        public void GenerateMinutewiseMeasuresTest()
        {
            var plant = DatabaseHelpers.CreatePlantWithOneInverter();

            var expectedMeasure = TestdataGenerator.GetTestMeasure(plant.PlantId);

            //generate measures for 5 minutes. 10 each minute.
            var now = Utils.GetWith0Second(DateTime.Now);
            _measureRepository.StartTransaction();
            for (int i = 1; i < 6; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    expectedMeasure.DateTime = now.AddMinutes(i).AddSeconds(j);
                    expectedMeasure.OutputWattage = 1000 + j;
                    expectedMeasure.PrivateInverterId = plant.InverterId;

                    _measureRepository.InsertTemporary(expectedMeasure);
                }
            }
            _measureRepository.CommitTransaction();

            //move the temporary power measures into minute_wise power table
            _measureRepository.UpdateTemporaryToMinuteWise(plant.InverterId);

            // 4 minute_wise values because the last minute may not be over yet.
            IEnumerable<Measure> actualMeasures = _measureRepository.GetMinuteWiseMeasures(plant.InverterId);

            actualMeasures.Count().Should().Be(4);
            foreach (var measure in actualMeasures)
            {
                measure.Value.Should().BeInRange(1004, 1005);
                measure.DateTime.Should().BeWithin(TimeSpan.FromMinutes(5));
                measure.MeasureId.Should().NotBe(0);
                measure.PlantId.Should().Be(expectedMeasure.PlantId);
                measure.PrivateInverterId.Should().Be(expectedMeasure.PrivateInverterId);
                measure.PublicInverterId.Should().NotBe(0);
                measure.SystemStatus.Should().BeNull();
                measure.Temperature.Should().Be(expectedMeasure.Temperature);
            }
        }

        [Test]
        public void Given_10_minute_wise_measures_When_I_request_measures_for_flot_line_chart_Then_they_are_formatted_correctly()
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
        public void Given_a_new_plant_When_I_request_the_latest_measures_Then_there_should_be_not_measures()
        {
            var plant = DatabaseHelpers.CreatePlantWithOneInverter();
            var result = _measureRepository.GetLatestMeasuresByPlant(plant.PlantId);
            Assert.IsTrue(result.Count == 0);
        }

    }
}
