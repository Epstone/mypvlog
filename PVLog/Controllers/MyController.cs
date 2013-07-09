using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using PVLog.Models;
using PVLog.DataLayer;
using PVLog.OutputProcessing;

namespace PVLog.Controllers
{
  public class MyController : Controller
  {
    public I_MeasureRepository _measureRepository { get; set; }
    public I_PlantRepository _plantRepository { get; set; }
    public IMembershipService MembershipService { get; set; }
    public KwhRepository _kwhRepository { get; set; }
    public UiDataProvider _dataProvider { get; set; }

    protected override void Initialize(System.Web.Routing.RequestContext requestContext)
    {
      base.Initialize(requestContext);

      if (this.MembershipService == null) this.MembershipService = new AccountMembershipService();

      if (this._measureRepository == null) _measureRepository = new MeasureRepository();
      if (this._plantRepository == null) _plantRepository = new PlantRepository();
      if (this._kwhRepository == null) this._kwhRepository = new KwhRepository();
      if (this._dataProvider == null) this._dataProvider = new UiDataProvider();
    }

    protected override void OnActionExecuted(ActionExecutedContext filterContext)
    {
      _measureRepository.Cleanup();
      _plantRepository.Cleanup();
      _dataProvider.CleanUp();
      _kwhRepository.Cleanup();

      base.OnActionExecuted(filterContext);
    }

    public int CurrentUserId
    {
      get
      {
        return (int)MembershipService.CurrentUserId;
      }
    }

    public bool IsAdmin
    {
      get
      {
        return User.IsInRole("Admin");
      }
    }



  }
}
