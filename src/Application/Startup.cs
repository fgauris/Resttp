
using System;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.Owin;
using Owin;
using Resttp;
using Resttp.Dependencies;
using Resttp.IoC;

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

        config.DependencyResolver = GetResolver();

        app.UseResttp(config);

        
    }

    public IDependencyResolver GetResolver()
    {
        var container = new IoCContainer();

        //Add<T>(Func<T> @delegate)
        container.Add<Object>(() => new object());

        //container.AddType<TypeToCreate>().For<TypeToLookup>().Set...
        container.AddType<Object>().ForSelf().SetSingleton();
        container.AddType<Object>().For<IList>().SetPerDependency();
        container.AddType<Object>().ForImplementedInterfaces().SetPerRequest();

        container.AddInstance(new Object()).ForSelf().SetSingleton();
        container.AddInstance(new object()).For<Object>().SetPerRequest();
        container.AddInstance(new object()).ForImplementedInterfaces().SetPerDependency();
        container.AddInstance(new object()).For<Object>().WithParameters(new { par = "parValue" });
        return container.Build();
    }






}