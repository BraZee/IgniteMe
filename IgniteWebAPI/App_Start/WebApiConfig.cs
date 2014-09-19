using System.Web.Http;
using System.Web.Http.Cors;

namespace IgniteWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "api/customers/GetCustomersByUsername/",
                defaults: new { controller = "customers",action = "getallcustomers",id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "api/customers/getauthenticate/{username}/{password}",
                defaults: new { controller = "customers", action = "getauthenticate", username = RouteParameter.Optional, password = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            
        }
    }
}
