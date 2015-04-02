using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Combres;
namespace TNV.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.AddCombresRoute("Combres");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
           routes.MapRoute(
              "Default", // Route name
              "{controller}/{action}/{memvar1}/{memvar2}/{memvar3}/{memvar4}/{memvar5}", // URL with parameters
              new { controller = "Home", action = "Index", memvar1 = UrlParameter.Optional, memvar2 = UrlParameter.Optional, memvar3 = UrlParameter.Optional, memvar4 = UrlParameter.Optional, memvar5 = UrlParameter.Optional } // Parameter defaults
          );
       
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
        }
    }
}