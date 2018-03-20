using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing.Constraints;
using System.Web.Routing;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

[assembly: OwinStartup(typeof(Gallery.Api.Startup))]

namespace Gallery.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new MyAuthorizationServerProvider()
        };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            GlobalConfiguration.Configure(WebApiConfig.Register);       
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            InjectionWebApi.ApiApplicationStart();

            var config = new HttpConfiguration();

            //config.Routes.MapHttpRoute(
            // name: "DefaultConnection",
            // routeTemplate: "{controller}"
            // );
            //config.EnableCors(new EnableCorsAttribute(Properties.Settings.Default.Cors, " ", " ")

            app.UseWebApi(config);

        }

    }
}
