using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVLog.DataLayer;
using PVLog.Models;

namespace PVLog.Controllers
{
  [HandleError]
  [Authorize]
  public class InverterController : MyController
  {
   

   

    [HttpGet]
    public ActionResult Edit(int id)
    {
      if (!IsInverterOwner(id))
        ThrowNotAuthorizedException();

      var model = _plantRepository.GetInverter(id);



      return View("EditInverter", model);
    }



    [HttpPost]
    public ActionResult Edit(Inverter postedModel)
    {
      if (!IsInverterOwner(postedModel.InverterId))
        ThrowNotAuthorizedException();

      if (ModelState.IsValid)
      {
        var inverterToUpdate = _plantRepository.GetInverter(postedModel.InverterId);

        // update inverter settings
        inverterToUpdate.EuroPerKwh = postedModel.EuroPerKwh;
        inverterToUpdate.Name = postedModel.Name;
        inverterToUpdate.ACPowerMax = postedModel.ACPowerMax;

        //store edited inverter
        _plantRepository.StoreInverter(inverterToUpdate);

        //notify user about success
        ViewData["Message"] = "Ihre Änderungen wurden übernommen.";
      }

      //reload inverter from repository
      var getModel = _plantRepository.GetInverter(postedModel.InverterId);

      return View("EditInverter", getModel);
    }

    private bool IsInverterOwner(int id)
    {
      return _plantRepository.IsOwnerOfInverter(id, CurrentUserId) || IsAdmin;
    }

    private void ThrowNotAuthorizedException()
    {
      throw new HttpException(401, "This inverter does not belong to one of your plants");
    }

    [HttpGet]
    public ActionResult DeleteRequest(int id)
    {
      if (!IsInverterOwner(id))
        ThrowNotAuthorizedException();

      var inverter = _plantRepository.GetInverter(id);

      return View("ConfirmInverterDeletion", inverter);
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
      if (!IsInverterOwner(id))
        ThrowNotAuthorizedException();

      var inverterToDelete = _plantRepository.GetInverter(id);
      _plantRepository.DeleteInverterById(id);

      return RedirectToAction("Edit", "Plant", new { id = inverterToDelete.PlantId });
    }

  }
}
