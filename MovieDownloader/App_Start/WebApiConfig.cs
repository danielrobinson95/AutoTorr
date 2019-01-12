using System;
using System.Configuration;
using System.Net.Http.Headers;
using System.Web.Http;
using Unity;
using Yify.API;

namespace MovieDownloader
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            InjectDependencies(config);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void InjectDependencies(HttpConfiguration config)
        {
            var url = ConfigurationManager.AppSettings["YifyUrl"] ?? throw new NullReferenceException("Missing key: YifyUrl");
            var torrentSavePath = ConfigurationManager
                                      .AppSettings["TorrentSaveLocation"] ?? throw new NullReferenceException("Missing key: TorrentSaveLocation");

            var container = new UnityContainer();
            container.RegisterType<IYifyService>(
                new InjectionFactory(c => new YifyService(url, torrentSavePath)));
            config.DependencyResolver = new UnityResolver(container);

        }
    }
}
