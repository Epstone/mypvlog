using System;
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
    using Autofac;
    using Autofac.Integration.Mvc;
    using PVLog.Controllers;
    using PVLog.DataLayer;

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

            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<UserNotifications>().As<IUserNotifications>();
            builder.RegisterType<EmailSender>().As<IEmailSender>();
            builder.RegisterType<MeasureRepository>().As<IMeasureRepository>();
            builder.RegisterType<PlantRepository>().As<I_PlantRepository>();
            builder.RegisterType<InverterTrackerRegistry>().As<IInverterTrackerRegistry>().SingleInstance();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }



        protected void Application_AuthenticateRequest()
        {
            UserManagementController.IsRequestAuthorized = Roles.IsUserInRole("Admin");
        }
    }
}