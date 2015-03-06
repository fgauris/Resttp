
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
           controller: "Home", 
           action: "Index"
           );

        config.HttpRoutes.AddRoute
           (
           "Home",
           "/{lang}/Home/Index/",
           "Home", "Index"
           );
        config.HttpRoutes.AddRoute
           (
           "Home",
           "/{lang}/Home/Index/{id}",
           "Home", "Index", new { lang = "lt" }
           );


        config.HttpRoutes.AddRoute
            (
            "Default",
            "/{controller}/{id}"
            );
        
        app.UseResttp(config);
    }
}