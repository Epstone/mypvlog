using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Reactive.Testing;
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
        public void SimpleAverage()
        {
            var samples = new List<Measure>()
            {
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 2, 1), 100),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 2, 2), 150),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 3, 5), 100),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 3, 59), 200),
                TestdataGenerator.GetTestMeasure(new DateTime(2018, 07, 06, 3, 4, 59), 200)
            };

            MinuteWiseAggregator aggregator = new MinuteWiseAggregator();
            aggregator.TrackMeasurements(samples);

            IEnumerable<Measure> samplesAggregated = aggregator.GetAveragesForMinutes();

            samplesAggregated.First().Value.Should().Be(125);
            samplesAggregated.Skip(1).Take(1).First().Value.Should().Be(150);

            aggregator.GetSampleCount().Should().Be(1);

        }

        [Test,Ignore]
        public async Task FactMethodName()
        {
            var pace = Observable.Interval(TimeSpan.FromSeconds(1));


            var observable = Observable.Range(0, 20).Select(i =>
                new Measure()
                {
                    DateTime = DateTime.UtcNow,
                    GeneratorWattage = 1,
                    GeneratorVoltage = 1,
                    GeneratorAmperage = 1,
                    GridAmperage = 1,
                    GridVoltage = 1,
                    OutputWattage = 1,
                    PlantId = 1,
                    PrivateInverterId = 1,
                    PublicInverterId = 1,
                    SystemStatus = 1,
                    Temperature = 20,
                    Value = i,
                }).Zip(pace, (m, p) => m);

            var result = observable.Buffer(TimeSpan.FromSeconds(3))
                .Select(window => CalculateAverageSample(window));

            result.Subscribe(x => Console.WriteLine($"Durchschnitt: {x.Value}"));

            await result.ToTask();

            Console.WriteLine("doneu");
        }

        // |-----X--------X-------X--------X----|  _____ |-------X----------X-------x|
        // window 1                                             window2
        //private void HandleNextWindow(IObservable<Measure> observable1)
        //{
        //    observable1.Aggregate((acc, src) =>
        //    {
        //        var avgValue = sr

        [Test]
        public void divideByTimestamp()
        {
            var scheduler = new TestScheduler();
            var interval = Observable.Interval(TimeSpan.FromSeconds(10), scheduler)
                .Take(3);
            var actualValues = new List<long>();
            interval.Subscribe(actualValues.Add);

            var expectedValues = new List<long>() { 0, 1, 2 };
            scheduler.Start();

            CollectionAssert.AreEqual(expectedValues, actualValues);
        }

        [Test]
        public async Task GroupMeasuresByDate()
        {
            var list = new List<Measure>()
            {
                new Measure() {DateTime = new DateTime(2018, 10, 1, 3, 2, 1)},
                new Measure() {DateTime = new DateTime(2018, 10, 1, 3, 2, 13)},
                new Measure() {DateTime = new DateTime(2018, 10, 1, 3, 3, 15)},
                new Measure() {DateTime = new DateTime(2018, 10, 1, 3, 3, 18)}
            };

            TimeSpan interval = new TimeSpan(0, 1, 0);     // 1 minutes.

            var groupedObservables = list.ToObservable().GroupBy(x => x.DateTime.Ticks / interval.Ticks, measure => measure);

            var observable = groupedObservables.Select(x => x);
            var actualValues = new List<Measure>();

            observable.Subscribe(groupy =>
            {
                Console.WriteLine($"group key {new DateTime(groupy.Key * interval.Ticks)}");
                groupy.Subscribe((measure) => Console.WriteLine($"measure: {measure.DateTime}"),() => Console.WriteLine("sq done"));

            });

            await observable.ToTask();

            Console.WriteLine("done");
        }




        private static Measure CalculateAverageSample(IList<Measure> inputWindow)
        {
            var avgMeasure = new Measure();

            avgMeasure.Value = inputWindow.Average(x => x.Value);

            return avgMeasure;

        }
    }
}