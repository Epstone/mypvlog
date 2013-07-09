using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PVLog.Statistics;
using PVLog.Utility;
using PVLog;

namespace solar_tests
{
    [TestFixture]
    public class AverageCalctulatorTest
    {
        [Test]
        public void CalculateAverageTest()
        {
            MinuteWiseAverageCalculator calc = new MinuteWiseAverageCalculator();
            DateTime time = BigMama.GetTestDate();
            var measureA = BigMama.GetTestMeasure(time, 100);
            var measureB = BigMama.GetTestMeasure(time.AddSeconds(10), 200);
            var expectedResultTime = Utils.GetWith0Second(time);

            calc.AddMeasure(measureA);
            calc.AddMeasure(measureB);
            double actual = calc.CalculateAverage()[0].OutputWattage;
            DateTime actualTime = calc.CalculateAverage()[0].DateTime;
            actual = calc.CalculateAverage()[0].OutputWattage;

            Assert.AreEqual(150, actual);
            Assert.AreEqual(expectedResultTime, actualTime);
        }

        [Test]
        public void CalculateAverageWithoutDataTest()
        {
            MinuteWiseAverageCalculator calc = new MinuteWiseAverageCalculator();
            List<Measure> actual = calc.CalculateAverage();

            Assert.AreEqual(0, actual.Count);
        }
    }
}
