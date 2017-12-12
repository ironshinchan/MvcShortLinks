using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcShortLinks
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Link",
                url: "{id}",
                defaults: new { controller = "Home", action = "URL" }
            );

            routes.MapRoute(
                name: "ShortURLControllerCreate",
                url: "ShortURL/CreateShortURL/{url}",
                defaults: new { controller = "ShortURL", action = "CreateShortURL", url = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}