
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
        config.MapHttpRoutesFromAttributes();
        config.HttpRoutes.AddRoute
           (
           "Home",
           "/Home/Index/",
           controller: "HomeController", 
           action: "Index"
           );

        config.HttpRoutes.AddRoute
           (
           "Home",
           "{lang}/Home/Index/",
           "HomeController", "Index"
           );
        config.HttpRoutes.AddRoute
           (
           "Home",
           "{lang}/Home/Index/{id}",
           "HomeController", "Index", new { lang = "lt" }
           );


        config.HttpRoutes.AddRoute
            (
            "Default",
            "/{controller}/{id}"
            );
        app.UseResttp(config);
    }
}