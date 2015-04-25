using System;
using Owin;
using Resttp.ControlerCreator;

namespace Resttp
{
    public static class ResttpAppBuilderExtensions
    {
        public static void UseResttp(this IAppBuilder app, ResttpConfiguration config)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            app.Use<RoutingComponent>(new HttpRouteResolver(config.HttpRoutes));
            //app.Use<ControlerCreatorComponent>(config.DependencyResolver);
            app.Use<ResttpComponent>(config);
            
        }
    }
}
