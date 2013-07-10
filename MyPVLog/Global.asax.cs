﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SimpleMvcUserManagement;

namespace MyPVLog
{
  // Hinweis: Anweisungen zum Aktivieren des klassischen Modus von IIS6 oder IIS7 
  // finden Sie unter "http://go.microsoft.com/?LinkId=9394801".
  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();

      WebApiConfig.Register(GlobalConfiguration.Configuration);
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
    }

   

    protected void Application_AuthenticateRequest()
    {
      UserManagementController.IsRequestAuthorized = Roles.IsUserInRole("Admin");
    }
  }
}