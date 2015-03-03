
using Microsoft.Owin;
using Owin;
using Resttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Startup))]
public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        var config = new ResttpConfiguration();

        config.HttpRoutes.AddRoute
           (
           "Home",
           "/Home/Index/",
           controller: "home", 
           action: "index"
           );

        config.HttpRoutes.AddRoute
           (
           "Home",
           "{lang}/Home/Index/",
           "home", "index"
           );
        config.HttpRoutes.AddRoute
           (
           "Home",
           "{lang}/Home/Index/{id}",
           "home", "index", new { lang = "lt" }
           );


        config.HttpRoutes.AddRoute
            (
            "Default",
            "/{controller}/{id}",
            null, null
            );
        app.UseResttp(config);
    }
}