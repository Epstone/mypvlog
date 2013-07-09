using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PVLog.Utility;
using PVLog.Models;
using PVLog.Enums;

namespace solar_tests
{
  [TestFixture]
  public class UtilityTest
  {

    [Test]
    public void XMLSerializerTest()
    {
      MyXmlSerializer<PlantSettings> serializer = new MyXmlSerializer<PlantSettings>();
      PlantSettings settings = new PlantSettings();
      settings.PlantVisibility = E_PlantVisibility.OwnersOnly;
      settings.InverterEuroMapping = new SerializableDictionary<int, double>();
      settings.InverterEuroMapping.Add(1, 2.5);
      settings.InverterEuroMapping.Add(2, 3.5);

      string actual = serializer.SerializeObject(settings);
      Console.Write(actual);

      PlantSettings result = serializer.DeserializeObject(actual);
      Assert.AreEqual(E_PlantVisibility.OwnersOnly, result.PlantVisibility);
    }

    [Test]
    public void SerializerIncompleteObjTest()
    {
      MyXmlSerializer<PlantSettings> serializer = new MyXmlSerializer<PlantSettings>();
      StringBuilder builder = new StringBuilder();
      builder.Append(@"<?xml version=""1.0"" encoding=""utf-16""?>");
      builder.Append(@"<PlantSettings xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">");
      builder.Append(@"<PlantVisibility>OwnersOnly</PlantVisibility>");
      builder.Append(@"</PlantSettings>");

      PlantSettings actual = serializer.DeserializeObject(builder.ToString());
      Assert.IsNull(actual.InverterEuroMapping);
      Assert.AreEqual(E_PlantVisibility.OwnersOnly, actual.PlantVisibility);

    }

    /// <summary>
    /// Verifys that the Unix Timestamp to DateTime Conversion is working correctly
    /// </summary>
    [Test]
    public void UnixTimeStampToDateTimeTest()
    {

      long input = 1328455638;

      var expected = new DateTime(2012, 2, 5, 15, 27, 0);
      var actual = Utils.UnixTimeStampToDateTime(input);

      //crop second and milliseconds
      actual = Utils.GetWith0Second(actual);

      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void DateTimeToJavaScriptTimestamp()
    {

      var input = new DateTime(2012,2,5,15,37,0);
      var input2 = new DateTime(2012, 2, 5, 14, 37, 0, 0);
      var input3 = new DateTime(2012, 2, 5, 16, 37, 0, 0);

      var expected = 1328452620000;
      var actual = Utils.DateTimeToJavascriptTimestamp(input);

      

      Console.WriteLine(Utils.DateTimeToJavascriptTimestamp(input2));
      Console.WriteLine(Utils.DateTimeToJavascriptTimestamp(input3));

      //crop second and milliseconds
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void RandomStringTest()
    {

      for (int i = 0; i < 100; i++)
      {
        Console.WriteLine(Utils.GenerateRandomString(6));
      }
    }
  }
}
