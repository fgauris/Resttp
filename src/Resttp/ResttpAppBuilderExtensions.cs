using Owin;
using System;

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

            app.Use<ResttpComponent>(config);
            
        }
    }
}
