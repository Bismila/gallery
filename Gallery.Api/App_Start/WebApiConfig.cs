using System.Web.Http;
using System.Web.Http.Cors;

namespace Gallery.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultConnection",  // DefaultConnection
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            //config.Filters.Add(new AuthorizeAttribute());
        }
    }
}
