namespace Skype2.Server.Endpoints.Http
{
    using System;
    using System.Net.Http.Formatting;
    using System.Web.Http;

    using Owin;

    internal class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.MediaTypeMappings.Add(new RequestHeaderMapping("Accept", "text/html", StringComparison.InvariantCultureIgnoreCase, true, "application/json"));

            appBuilder.UseWebApi(config);
        }
    } 
}