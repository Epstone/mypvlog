using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using FluentAssertions;
using NUnit.Framework;
using PVLog;
using PVLog.Controllers;

namespace solar_tests
{
    [TestFixture]
    public class InverterTrackerTest
    {
        private InverterTracker aggregator;
        private int inverterId;

        [SetUp]
        public void Setup()
        {
            inverterId = 1;
            aggregator = new InverterTracker(inverterId);
        }

        // add 2 items per minute and calculate avg measure per minute
        [Test]
        public void SimpleAverageForOneInverter()
        {
            var samples = new List<Measure>()
            {
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 2, 1), 100, inverterId),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 2, 2), 150, inverterId),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 3, 5), 100, inverterId),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 3, 59), 200, inverterId),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 4, 59), 200, inverterId)
            };

            aggregator.TrackMeasurements(samples);

            IEnumerable<Measure> samplesAggregated = aggregator.GetAveragesForMinutes();

            samplesAggregated.First().Value.Should().Be(125);
            samplesAggregated.Skip(1).Take(1).First().Value.Should().Be(150);

            aggregator.GetSampleCount().Should().Be(1);
        }


        [Test]
        public void When_not_enough_measures_Then_it_returns_an_empty_list()
        {
            var samples = new List<Measure>()
            {
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 2, 1), 100, inverterId),
            };

            aggregator.TrackMeasurements(samples);

            IEnumerable<Measure> samplesAggregated = aggregator.GetAveragesForMinutes();
            samplesAggregated.Should().BeEmpty();
            samplesAggregated.Should().NotBeNull();
        }

        [Test]
        public void When_there_is_one_minute_complete_Then_it_returns_one_measure()
        {
            var samples = new List<Measure>()
            {
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 2, 1), 100, inverterId),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 3, 2), 100, inverterId),
            };

            aggregator.TrackMeasurements(samples);

            IEnumerable<Measure> samplesAggregated = aggregator.GetAveragesForMinutes();
            samplesAggregated.Count().Should().Be(1);
        }
    }
}