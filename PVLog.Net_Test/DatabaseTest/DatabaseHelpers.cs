using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PVLog.DataLayer;

namespace solar_tests.DatabaseTest
{
  public static class DatabaseHelpers
  {
    public static int CreatePlantGetId()
    {
      var plantDb = new PlantRepository();
      var plant = TestdataGenerator.GetPlant();
      return plantDb.CreatePlant(plant);
    }


    internal static int CreateInverter(int plantId)
    {
      var plantDb = new PlantRepository();
      return plantDb.CreateInverter(plantId, null, 0.4F, "Test-Generator");
    }

    internal static TestSolarPlant CreatePlantWithOneInverter()
    {
      var plantId = CreatePlantGetId();
      var inverterId = CreateInverter(plantId);

      return new TestSolarPlant()
      {
        InverterId = inverterId,
        PlantId = plantId
      };
    }
  }
}
