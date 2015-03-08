
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
        //config.MapHttpRoutesFromAttributes();
        

     
        config.HttpRoutes.AddRoute
            (
            "Home",
            "/lt/",
            new
            {
                controller = "Home"
            });
        config.HttpRoutes.AddRoute
            (
            "Home",
            "/lt/{action}",
            new
            {
                controller = "Home"
            });


        app.UseResttp(config);
    }
}