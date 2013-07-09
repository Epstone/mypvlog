using System;
using System.Web;
using NUnit.Framework;
using PVLog;
using PVLog.InputProcessing;

namespace solar_tests
{
  [TestFixture]
  public class MeasureReaderTest
  {
    [Test]
    public void GetKaco1MeasureTest()
    {

      string input = "26.12.2009;23:53:00;5;158.0;3.20;134;229.6;1.34;150;7";


      Measure actual = MeasureReader.ReadKaco1Data(input, 1, 1);

      Assert.AreEqual(DateTime.Now.Second, actual.DateTime.Second);
      Assert.AreEqual(5, actual.SystemStatus);
      Assert.AreEqual(158.0, actual.GeneratorVoltage);
      Assert.AreEqual(3.2, actual.GeneratorAmperage);
      Assert.AreEqual(134, actual.GeneratorWattage);
      Assert.AreEqual(229.6, actual.GridVoltage);
      Assert.AreEqual(1.34, actual.GridAmperage);
      Assert.AreEqual(150, actual.OutputWattage);
      Assert.AreEqual(7, actual.Temperature);
      Assert.AreEqual(1, actual.PrivateInverterId);
      Assert.AreEqual(1, actual.PlantId);
    }

    [Test]
    public void GetKaco2MeasureTest() //measure string from kaco 485
    {

      string input = "*020;4;378.2;3.96;1498;228.9;6.55;1438;29;5000;";

      Measure actual = MeasureReader.ReadKaco2Data(input, 1);
      Assert.AreEqual(DateTime.Now.Second, actual.DateTime.Second);
      Assert.AreEqual(4, actual.SystemStatus);
      Assert.AreEqual(378.2, actual.GeneratorVoltage);
      Assert.AreEqual(3.96, actual.GeneratorAmperage);
      Assert.AreEqual(1498, actual.GeneratorWattage);
      Assert.AreEqual(228.9, actual.GridVoltage);
      Assert.AreEqual(6.55, actual.GridAmperage);
      Assert.AreEqual(1438, actual.OutputWattage);
      Assert.AreEqual(29, actual.Temperature);
      Assert.AreEqual(2, actual.PublicInverterId);
      Assert.AreEqual(1, actual.PlantId);
    }
  }
}
