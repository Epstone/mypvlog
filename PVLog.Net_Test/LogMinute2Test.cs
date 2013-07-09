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
    public class LogMinute2Test
    {
        //[Test]
        //public void CalculateAverageTest()
        //{
        //    DateTime time = BigMama.GetTestDate();

        //    MeasureMinute calc = new MeasureMinute(time);

        //    var measure1A = BigMama.GetTestMeasure(time, 100);
        //    var measure1B = BigMama.GetTestMeasure(time.AddSeconds(10), 200);
        //    var measure2A = BigMama.GetTestMeasure(time, 300,2);
        //    var measure2B = BigMama.GetTestMeasure(time.AddSeconds(10), 500,2);

        //    calc.AddMeasure(measure1A);
        //    calc.AddMeasure(measure1B);
        //    calc.AddMeasure(measure2A);
        //    calc.AddMeasure(measure2B);

        //    var actual1 = calc.CalculateAverage()[0];
        //    var actual2 = calc.CalculateAverage()[1];
        //    Assert.AreEqual(150, actual1.OutputWattage);
        //    Assert.AreEqual(400, actual2.OutputWattage);
        //    Assert.AreEqual(time, actual1.DateTime);
        //    Assert.AreEqual(time, actual2.DateTime);
        //}

        //[Test]
        //public void CalcAvgCompleteTest()
        //{
        //    var currentMinute = Utils.GetCurrentMinute();
        //     MeasureMinute logMinute = new MeasureMinute(currentMinute);

            
        //    var measureA = BigMama.GetTestMeasure(currentMinute, 100);
        //    var measureB = BigMama.GetTestMeasure(currentMinute.AddSeconds(10), 200);

        //    //manipulate test measures
        //    measureA.GeneratorAmperage = 100;
        //    measureA.GeneratorVoltage = 100;
        //    measureA.GeneratorWattage = 100;
        //    measureA.GridAmperage = 100;
        //    measureA.GridVoltage = 100;

        //    measureB.GeneratorAmperage = 200;
        //    measureB.GeneratorVoltage = 200;
        //    measureB.GeneratorWattage = 200;
        //    measureB.GridAmperage = 200;
        //    measureB.GridVoltage = 200;

        //    logMinute.AddMeasure(measureA);
        //    logMinute.AddMeasure(measureB);

        //    var actual = logMinute.CalculateAverage()[0];

        //    Measure expected = BigMama.GetTestMeasure(currentMinute, 100);
        //    expected.DateTime = currentMinute;
        //    expected.GeneratorAmperage = 150;
        //    expected.GeneratorVoltage = 150;
        //    expected.GeneratorWattage = 150;
        //    expected.GridAmperage = 150;
        //    expected.GridVoltage = 150;
        //    expected.OutputWattage = 150;

        //    Assert.AreEqual(expected, actual);
        //}
    }
}
