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
        private DateTime minute1;
        private TestSolarPlant plant;
        private Measure testMeasure;

        public MeasureRepositoryTest()
        {
            minute1 = new DateTime(2017, 8, 4, 8, 20, 0);
        }

        [SetUp]
        public void Setup()
        {
            _testDb = new TestDbSetup();

            _testDb.TruncateAllTables();
            _measureRepository = new MeasureRepository();
            _plantRepository = new PlantRepository();

            plant = DatabaseHelpers.CreatePlantWithOneInverter();

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

            Measure expected_1 = TestdataGenerator.GetTestMeasure(plantId, inverterId);

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
            Given_measures_for_seconds_When_I_aggregate_Then_they_are_aggregated_to_a_minute_measurement()
        {
            Given_measures_for_five_seconds_of_a_minute(plant, minute1);

            When_I_aggregate_the_measures_to_minute_wise();
            Then_expected_measures_count_should_be(0);

            When_I_create_the_measures_in_the_following_minute();
            When_I_aggregate_the_measures_to_minute_wise();

            Then_expected_measures_count_should_be(1);
        }

        private void When_I_create_the_measures_in_the_following_minute()
        {
            Measure measureNextMinute = TestdataGenerator.GetTestMeasure(plant.PlantId, plant.InverterId);
            measureNextMinute.DateTime = minute1.AddMinutes(1.1);
            _measureRepository.InsertTemporary(measureNextMinute);
        }

        private void Then_expected_measures_count_should_be(int expectedMeasuresCount)
        {
            var actualMeasures = _measureRepository.GetMinuteWiseMeasures(plant.InverterId);
            actualMeasures.Count().Should().Be(expectedMeasuresCount); // minute could not be complete yet
        }

        private void When_I_aggregate_the_measures_to_minute_wise()
        {
            _measureRepository.AggregateTemporaryToMinuteWiseMeasures(plant.InverterId);
        }

        private void Given_measures_for_five_seconds_of_a_minute(TestSolarPlant plant, DateTime minute_1)
        {
            var secondMeasures = Enumerable.Range(1, 5).Select(second =>
            {
                var measure = TestdataGenerator.GetTestMeasure(plant.PlantId, plant.InverterId);
                measure.DateTime = minute_1.AddSeconds(second);
                measure.OutputWattage = 1000;
                return measure;
            }).ToList();

            secondMeasures.ForEach(measure => _measureRepository.InsertTemporary(measure));
        }

        // todo, write test case for transaction

        /// <summary>
        /// Tests the minute wise wattage generation 
        /// </summary>
        [Test]
        public void Given_measures_for_3_minutes_When_I_aggregate_Then_there_are_2_minute_wise_measures_created()
        {
            Given_measures_for_first_3_minutes();

            When_I_aggregate_the_measures_to_minute_wise();

            Then_expected_measures_count_should_be(2);

            //foreach (var measure in actualMeasures)
            //{
            //    
            //}
        }

        [Test]
        public void Given_a_temporary_measure_is_aggregated_Then_the_values_should_be_consistent()
        {
            Given_a_completed_measure_minute_in_temporary_table();
            When_I_aggregate_the_measures_to_minute_wise();
            Then_the_values_should_be_equivalent();
        }

        private void Then_the_values_should_be_equivalent()
        {
            IEnumerable<Measure> measures = _measureRepository.GetMinuteWiseMeasures(plant.InverterId);
            var measure = measures.First();
                
            measure.Value.Should().Be(1000);
            measure.MeasureId.Should().NotBe(0);
            measure.PlantId.Should().Be(plant.PlantId);
            measure.PrivateInverterId.Should().Be(plant.InverterId);
            measure.PublicInverterId.Should().NotBe(0);
            measure.SystemStatus.Should().BeNull();
            measure.Temperature.Should().Be(testMeasure.Temperature);
        }

        private void Given_a_completed_measure_minute_in_temporary_table()
        {
            testMeasure = TestdataGenerator.GetTestMeasure(minute1, 1000, plant.InverterId);
            _measureRepository.InsertTemporary(testMeasure);
            _measureRepository.InsertTemporary(TestdataGenerator.GetTestMeasure(minute1.AddMinutes(1.1), 1000, plant.InverterId));
            
        }

        private void Given_measures_for_first_3_minutes()
        {
            Enumerable.Range(0, 3).ToList().ForEach(x =>
            {
                var measure = TestdataGenerator.GetTestMeasure(minute1.AddMinutes(x), 1000, plant.InverterId);
                _measureRepository.InsertTemporary(measure);
            });
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
