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
    public class MinuteTrackerTest
    {

        // add 2 items per minute and calculate avg measure per minute
        [Test]
        public void SimpleAverageForOneInverter()
        {
            var inverterId = 1;
            var samples = new List<Measure>()
            {
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 2, 1), 100, inverterId),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 2, 2), 150, inverterId),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 3, 5), 100, inverterId),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 3, 59), 200, inverterId),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 4, 59), 200, inverterId)
            };

            MinuteWiseAggregator aggregator = new MinuteWiseAggregator();
            aggregator.TrackMeasurements(samples);

            IEnumerable<Measure> samplesAggregated = aggregator.GetAveragesForMinutes();

            samplesAggregated.First().Value.Should().Be(125);
            samplesAggregated.Skip(1).Take(1).First().Value.Should().Be(150);

            aggregator.GetSampleCount().Should().Be(1);
        }


        [Test]
        public void SimpleAverageForMultipleInverters()
        {
            var inverterId_1 = 1;
            var inverterId_2 = 2;
            var samples = new List<Measure>()
            {
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 2, 1), 100, inverterId_1),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 3, 1), 123, inverterId_1),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 4, 59), 234, inverterId_2),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 5, 59), 345, inverterId_2)
            };

            MinuteWiseAggregator aggregator = new MinuteWiseAggregator();
            aggregator.TrackMeasurements(samples);

            var samplesAggregated = aggregator.GetAveragesForMinutes();

            samplesAggregated[0].Value.Should().Be(100);
            samplesAggregated[0].PrivateInverterId.Should().Be(inverterId_1);
            samplesAggregated[1].Value.Should().Be(234);
            samplesAggregated[1].PrivateInverterId.Should().Be(inverterId_2);

            aggregator.GetSampleCount().Should().Be(2);
            aggregator.MeasuresToRemove.Count.Should().Be(0);
        }


        // |-----X--------X-------X--------X----|  _____ |-------X----------X-------x|
        // window 1                                             window2
        //private void HandleNextWindow(IObservable<Measure> observable1)
        //{
        //    observable1.Aggregate((acc, src) =>
        //    {
        //        var avgValue = sr


        private static Measure CalculateAverageSample(IList<Measure> inputWindow)
        {
            var avgMeasure = new Measure();

            avgMeasure.Value = inputWindow.Average(x => x.Value);

            return avgMeasure;

        }
    }
}