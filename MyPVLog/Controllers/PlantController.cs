using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVLog.Models;
using PVLog.DataLayer;
using PVLog.Models.PlantView;
using PVLog.Management;
using PVLog.Extensions;
using PVLog.OutputProcessing;
using PVLog.Enums;
using PVLog.Utility;
using System.Web.Security;
using PoliteCaptcha;

namespace PVLog.Controllers
{
  [HandleError]
  public class PlantController : MyController
  {

    public PlantController()
    {

    }

    public PlantController(I_PlantRepository plantRepository)
    {
      _plantRepository = plantRepository;
    }

    public ActionResult Index()
    {
      return View();
    }

    [HttpGet, Authorize]
    public ActionResult Add()
    {
      return View();
    }


    [Authorize, HttpPost, ValidateSpamPrevention]
    public ActionResult Add(SolarPlant model)
    {

      if (ModelState.IsValid)
      {
        model.Password = Utils.GenerateRandomString( 8 );
        var plantId = _plantRepository.CreatePlant( model );
        _plantRepository.StoreUserPlantRelation( CurrentUserId, plantId, E_PlantRole.Owner );

        return RedirectToAction( "View", new { id = plantId, name = model.Name } );
      }
      else
      {
        return View( model );
      }
    }

    [HttpGet]
    public ActionResult View(int id)
    {
      var plant = _plantRepository.GetPlantById( id );
      var kwhRepository = new KwhRepository();

      // build view model
      var plantModel = PlantHomeModel.Create( plant );
      //var today =Utils.GetTodaysDate();
      var today = Utils.GetTodaysDate();
      plantModel.SummaryTable = new PlantSummaryTableModel();

      plantModel.SummaryTable.Today = _dataProvider.GetkwhAndMoneyPerTimeFrame( today, today.AddDays( 1 ), id, E_TimeMode.day );
      plantModel.SummaryTable.ThisMonth = _dataProvider.GetkwhAndMoneyPerTimeFrame( Utils.FirstDayOfMonth(), Utils.FirstDayNextMonth(), id, E_TimeMode.month );
      plantModel.SummaryTable.ThisYear = _dataProvider.GetkwhAndMoneyPerTimeFrame( Utils.FirstDayOfYear(), Utils.FirstDayNextYear(), id, E_TimeMode.year );


      //is user allowed to edit the plant?
      plantModel.HeaderModel = GetHeaderModel( plant );

      return View( plantModel );
    }


    [HttpGet]
    [Authorize]
    public ActionResult Edit(int id)
    {
      if (!IsAuthorizedForPlant( id ))
        ThrowNotAuthorizedForPlantException();

      var model = _plantRepository.GetPlantById( id );
      return View( "EditPlant", model );

    }

    [HttpPost]
    [Authorize]
    public ActionResult Edit(SolarPlant model)
    {
      var plantToUpdate = _plantRepository.GetPlantById( model.PlantId );

      if (!IsAuthorizedForPlant( model.PlantId ))
        ThrowNotAuthorizedForPlantException();

      if (ModelState.IsValid)
      {
        // store all updated fields
        plantToUpdate.Name = model.Name;
        plantToUpdate.AutoCreateInverter = model.AutoCreateInverter;
        plantToUpdate.PeakWattage = model.PeakWattage;
        plantToUpdate.PostalCode = model.PostalCode;

        _plantRepository.UpdatePlant( plantToUpdate );


        ViewData["Message"] = "Ihre Änderungen wurden übernommen.";
      }
      return View( "EditPlant", plantToUpdate );


    }

    [HttpGet]
    public ActionResult List()
    {
      var model = new PlantListModel()
      {
        Plants = _plantRepository.GetAllPlants()
      };

      return View( model );
    }

    [HttpGet]
    public ActionResult Month(int id)
    {
      //get kwh data
      string googleTableContent = _dataProvider.GoogleDataTableContent( Utils.FirstDayOfMonth(),
                                                            Utils.FirstDayNextMonth(), id,
                                                            E_EurKwh.kwh, E_TimeMode.day );
      var plant = _plantRepository.GetPlantById( id );
      var model = new PlantDayModel()
      {
        Plant = plant,
        GoogleData = googleTableContent,
        HeaderModel = GetHeaderModel( plant )
      };

      return View( model );
    }

    private PlantHeader GetHeaderModel(SolarPlant plant)
    {
      return new PlantHeader()
      {
        IsEditingAllowed = IsAuthorizedForPlant( plant.PlantId ),
        Plant = plant
      };
    }

    [HttpGet]
    public ActionResult Year(int id)
    {
      //get kwh data
      string googleTableContent = _dataProvider.GoogleDataTableContent( Utils.FirstDayOfYear(),
                                                            Utils.FirstDayNextYear(), id,
                                                            E_EurKwh.kwh, E_TimeMode.month );
      var plant = _plantRepository.GetPlantById( id );
      var model = new PlantDayModel()
      {
        Plant = plant,
        GoogleData = googleTableContent,
        HeaderModel = GetHeaderModel( plant )
      };

      return View( model );
    }

    [HttpGet]
    public ActionResult Decade(int id)
    {
      //get kwh data
      string googleTableContent = _dataProvider.GoogleDataTableContent( new DateTime( 2010, 1, 1 ),
                                                             new DateTime( 2020, 1, 1 ), id,
                                                            E_EurKwh.kwh, E_TimeMode.year );
      var plant = _plantRepository.GetPlantById( id );
      var model = new PlantDayModel()
      {
        Plant = plant,
        GoogleData = googleTableContent,
        HeaderModel = GetHeaderModel( plant )
      };

      return View( model );
    }

    [Authorize]
    [HttpGet]
    public ActionResult AddInverter(int id)
    {
      if (!IsAuthorizedForPlant( id ))
        ThrowNotAuthorizedForPlantException();

      //add new inverter to plant
      _plantRepository.CreateInverter( id, null, 0.1F, "Neuer Generator" );

      return RedirectToAction( "Edit", new { id } );
    }

    private void ThrowNotAuthorizedForPlantException()
    {
      throw new HttpException( 401, "You are not allowed to edit this plant" );
    }

    private bool IsAuthorizedForPlant(int id)
    {
      return (Request.IsAuthenticated &&
             (_plantRepository.IsOwnerOfPlant( CurrentUserId, id ) || IsAdmin));
    }
  }
}
