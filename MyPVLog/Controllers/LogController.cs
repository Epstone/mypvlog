using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVLog.DataLayer;
using System.Globalization;
using PVLog.InputProcessing;
using System.Text;
using PVLog.Utility;

namespace PVLog.Controllers
{
  public class LogController : MyController
  {


    public LogController()
    {

    }




    public LogController(I_MeasureRepository measureRepository, I_PlantRepository plantRepository)
    {
      this._measureRepository = measureRepository;
      this._plantRepository = plantRepository;
    }


    public ActionResult Kaco1(string data, int plant, string pw, int inverter)
    {
      int publicInverterId = inverter;

      //validate plant id and password
      if (IsValidPlant(plant, pw))
      {
        // check inverterId or create one if autocreate is activated
        try
        {
          //get private inverterId for the public inverterId
          int privateInverterId = ValidateGetPrivateInverterId(plant, publicInverterId);

          //parse the measure
          var measure = MeasureReader.ReadKaco1Data(data, plant, privateInverterId);

          //store measure in repository and return the success view
          _measureRepository.InsertTemporary(measure);

          return View("MeasureSuccess");

        }
        catch (ArgumentException ex)
        {
          Logger.LogError(ex);
        }
      }


      return View("MeasureFailed");
    }

    public ActionResult Kaco2(string data, int plant, string pw)
    {
      if (IsValidPlant(plant, pw))
      {
        try
        {
          //parse the measure
          var measure = MeasureReader.ReadKaco2Data(data, plant);

          // get the private inverter Id
          measure.PrivateInverterId = ValidateGetPrivateInverterId(plant, measure.PublicInverterId);

          //store measure in repository and return the success view
          _measureRepository.InsertTemporary(measure);

          return View("MeasureSuccess");

        }
        catch (ArgumentException ex)
        {
          Logger.LogError(ex);
        }

      }

      return View("MeasureFailed");
    }

    private bool IsValidPlant(int plant, string pw)
    {
      bool isValid = false;

      try
      {
        isValid = _plantRepository.IsValidPlant(plant, pw);
      }
      catch (ArgumentException ex)
      {
        Logger.LogError(ex);
      }

      if (!isValid)
        ViewData["ErrorMessage"] = @"Sorry, the combination of your plant ID and password is not valid. Please recheck them or generate a new password on the website.";

      return isValid;

    }

    public ActionResult Generic(int plant, string pw, int inverter, double feedinpower,
                                double? gridvoltage, double? gridcurrent,
                        double? generatorvoltage, double? generatorcurrent, double? generatorpower, int? systemstatus, int? temperature, long? timestamp)
    {
      int publicInverterId = inverter;

      if (IsValidPlant(plant, pw))
      {
        // check inverterId or create one if autocreate is activated
        try
        {
          //get private inverterId for the public inverterId
          int privateInverterId = ValidateGetPrivateInverterId(plant, publicInverterId);

          //parse the measure
          var measure = new Measure()
          {
            DateTime = (timestamp == null) ? DateTimeUtils.GetGermanNow()
                                           : DateTimeUtils.UnixTimeStampToDateTime(timestamp.Value),
            GeneratorAmperage = generatorcurrent,
            GeneratorVoltage = generatorvoltage,
            GeneratorWattage = generatorpower,
            GridAmperage = gridcurrent,
            GridVoltage = gridvoltage,
            OutputWattage = feedinpower,
            PlantId = plant,
            PrivateInverterId = privateInverterId,
            PublicInverterId = publicInverterId,
            SystemStatus = systemstatus,
            Temperature = temperature

          };

          //store measure in repository and return the success view
          _measureRepository.InsertTemporary(measure);

          return View("MeasureSuccess");

        }
        catch (ArgumentException ex)
        {
          Logger.LogError(ex);
        }

      }

      return View("MeasureFailed");
    }



    /// <summary>
    /// Returns the private inverterId by a given public inverterId and plantId. Throws an 
    /// Argument Exception if the publich inverter Id is unknown and auto creation is disabled.
    /// </summary>
    /// <param name="plant">The solar plant Id</param>
    /// <param name="publicInverterId">The public InverterId for this inverter</param>
    /// <returns>The private inverterId if available</returns>
    private int ValidateGetPrivateInverterId(int plant, int publicInverterId)
    {
      int privateInverterId = -1;
      if (_plantRepository.IsValidInverter(plant, publicInverterId))
      {
        //exchange public and private inverterId if the public inverterId is valid for this plant
        privateInverterId = _plantRepository.GetPrivateInverterId(plant, publicInverterId);

      }
      else if (_plantRepository.IsAutoCreateInverterActive(plant))
      {
        //automatically add a new inverter for this plant
        privateInverterId = _plantRepository.CreateInverter(plant, publicInverterId, 0.1F, "Generator (neu)");

      }
      else
      {
        //the inverter is not yet existing and auto create inverters is turned off.
        throw new ArgumentException("Inverter is not valid for this plant");
      }
      return privateInverterId;
    }

  }
}
