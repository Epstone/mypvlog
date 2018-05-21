using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Threading.Tasks;
using NUnit.Framework;
using PVLog;

namespace solar_tests
{
    [TestFixture]
    public class MinuteTrackerTest
    {
        [Test]
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


        private static Measure CalculateAverageSample(IList<Measure> inputWindow)
        {
            var avgMeasure = new Measure();

            avgMeasure.Value = inputWindow.Average(x => x.Value);

            return avgMeasure;

        }
    }
}