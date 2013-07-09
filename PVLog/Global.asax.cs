using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using SimpleMvcUserManagement;
using System.Web.Security;
namespace PVLog
{
  // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
  // visit http://go.microsoft.com/?LinkId=9394801

  public class MvcApplication : System.Web.HttpApplication
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      UserManagementController.RegisterMe();

      // min_day.js
      routes.MapRoute(
          "minDayRoute", // Route name
          "Export/{plantId}/min_day.js", // URL with parameters
          new { controller = "Export", action = "min_day" } // Parameter defaults
      );

      // minYYMMDD.js
      routes.MapRoute(
          "minDayYYMMDD", // Route name
          "Export/{plantId}/min{date}.js", // URL with parameters
          new { controller = "Export", action = "MinuteWiseDefinedDate" } // Parameter defaults
      );

      routes.MapRoute(
          "SolarlogExport", // Route name
          "Export/{plantId}/{action}.js", // URL with parameters
          new { controller = "Export" } // Parameter defaults
      );


      routes.MapRoute(
          "IdName", // Route name
          "Plant/{action}/{id}/{name}", // URL with parameters
          new { controller = "Plant", action = "View", id = UrlParameter.Optional, name = UrlParameter.Optional } // Parameter defaults
      );

      routes.MapRoute(
          "Default", // Route name
          "{controller}/{action}/{id}", // URL with parameters
          new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
      );



    }

    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();

      RegisterRoutes(RouteTable.Routes);
    }

    protected void Application_AuthenticateRequest()
    {
      UserManagementController.IsRequestAuthorized = Roles.IsUserInRole("Admin");
    }
  }
}