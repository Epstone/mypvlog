using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVLog.DataLayer;
using PVLog.Models.Administration;
using PVLog.Models;
using PVLog.Utility;

namespace PVLog.Controllers
{
  [Authorize(Roles = "Admin")]
  public class AdministrationController : MyController
  {
   

    //
    // GET: /Administration/

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult ManagePlants()
    {
      ManagePlantsModel model = new ManagePlantsModel();

      //demo plant status
      if (_plantRepository.IsDemoPlantExsting())
        model.DemoPlant = _plantRepository.GetDemoPlant();

      //real plants
      model.RealPlants = _plantRepository.GetAllPlants();

      return View(model);
    }

    public ActionResult ManageUsers()
    {
      return View();
    }


    public ActionResult CreateTestPlant()
    {
      if (!_plantRepository.IsDemoPlantExsting())
      {
        var plant = new SolarPlant()
        {
          Name = "Demo Anlage",
          Password = Utils.GenerateRandomString(6),
          PeakWattage = 50000,
          PostalCode = "123456",
          IsDemoPlant = true
        };


        var id = _plantRepository.CreatePlant(plant);

        _plantRepository.Cleanup();

        ViewData["Message"] = "Die Testanlage wurde erfolgreich angelegt.";
        return View("GenericAdminSuccess");
      }
      else
      {
        ViewData["Message"] = "Es ist bereits eine Testanlage vorhanden.";
        return View("GenericAdminFail");
      }

    }

    public ActionResult ShowLogs( string loglevel)
    {
      var db = new UtilDatabase();
      var model = new ShowLogsModel()
      {
        Logs = db.GetAllLogs(loglevel,100)
      };

      db.Cleanup();
      return View(model);
    }
  }
}
