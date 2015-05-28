using System;
using System.Collections;
using Microsoft.Owin;
using Owin;
using Resttp;
using Resttp.Extensions;
using Resttp.Dependencies;
using Resttp.IoC;
using Resttp.IoC.Registration;
using System.Reflection;
using Application.Controllers;

[assembly: OwinStartup(typeof(Startup))]
public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        var config = new ResttpConfiguration(typeof(HomeController).Assembly);
        config.MapHttpRoutesFromAttributes();
        config.HttpRoutes.AddRoutes
            (
            "Home",
            "/Main",
            new
            {
                controller = "Home",
                action = "Index",
                httpMethod = "Get"
            });
        config.HttpRoutes.AddRoutes
            (
            "Home",
            "/{controller}/{action}",
            new
            {
                controller = "Home"
            });

        config.DependencyResolver = GetResolver();
        app.UseResttp(config);
    }

    public IScopedDependencyResolver GetResolver()
    {
        var builder = new IoCContainerBuilder();

        builder.AddResttpControllers(Assembly.GetExecutingAssembly());

        builder.AddInstance(() => 3).For<int>();

        builder.AddType<Object>().ForSelf().WithParameters(new Parameter("Labas", "labas")).SetSingleton();
        builder.AddType<Object>().For<IList>().SetPerDependency();
        builder.AddType<Object>().ForImplementedInterfaces().SetPerRequest();

        builder.AddInstance(() => new Object()).For<Object>().SetSingleton();
        builder.AddInstance(() => new object()).ForSelf().SetPerRequest();
        builder.AddInstance(() => new object()).ForImplementedInterfaces().SetPerDependency();
        return builder.Build();
    }






}