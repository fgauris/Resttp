
using Microsoft.Owin;
using Owin;
using Resttp;

[assembly: OwinStartup(typeof(Startup))]
public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        var config = new ResttpConfiguration();
        config.MapHttpRoutesFromAttributes();
        

     
        config.HttpRoutes.AddRoutes
            (
            "Home",
            "/lt/",
            new
            {
                controller = "Home"
            });
        config.HttpRoutes.AddRoutes
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