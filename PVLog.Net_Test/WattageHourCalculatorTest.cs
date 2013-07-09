using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PVLog.OutputProcessing;

namespace solar_tests
{
  [TestFixture]
  public class WattageHourCalculatorTest
  {
    [Test]
    public void CalcTest()
    {
      WattageHourCalculator calc = new WattageHourCalculator();

      int inverter1 = 1;
      int inverter2 = 2;

      // 5 minute timespan
      var fiveMin = new TimeSpan(0, 5, 0);
      calc.AddPowerValue(inverter1, 500, fiveMin);
      calc.AddPowerValue(inverter2, 1000, fiveMin);

      var actualWh_1 = calc.GetWattageHour(inverter1);
      var actualWh_2 = calc.GetWattageHour(inverter2);
      
      int expected_1 = (int)Math.Round(41.66666667);
      int expected_2 = (int)Math.Round(83.33333333);

      Assert.AreEqual(expected_1, actualWh_1);
      Assert.AreEqual(expected_2, actualWh_2);
    }
  }
}
