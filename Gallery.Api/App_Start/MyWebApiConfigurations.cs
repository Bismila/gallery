//using Newtonsoft.Json.Serialization;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Http;

//namespace Gallery.Api.App_Start
//{
//    public class MyWebApiConfigurations : HttpConfiguration
//    {
//        public MyWebApiConfigurations()
//        {
//            ConfigRoutes();
//            ConfigJSON();
//        }

//        void ConfigRoutes()
//        {
//            Routes.MapHttpRoute(
//               name: "DefaultApi",
//                routeTemplate: "api/{controller}/{id}",
//                defaults: new { id = RouteParameter.Optional }
//                );
//        }

//        void ConfigJSON()
//        {
//            var jsonProps = Formatters.JsonFormatter.SerializerSettings;
//            jsonProps.Formatting = Newtonsoft.Json.Formatting.Indented;
//            jsonProps.ContractResolver = new CamelCasePropertyNamesContractResolver();

//            var xml = Formatters.XmlFormatter;
//            Formatters.Remove(xml);
//        }
//    }
//}