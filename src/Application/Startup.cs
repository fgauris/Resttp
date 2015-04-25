
using System;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.Owin;
using Owin;
using Resttp;
using Resttp.Dependencies;
using Resttp.IoC;
using Resttp.IoC.Registration;

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
        var container = new IoCContainerBuilder();

        //Add<T>(Func<T> @delegate)
        container.AddInstance(() => 3).For<int>();

        //container.AddType<TypeToCreate>().For<TypeToLookup>().Set...
        container.AddType<Object>().ForSelf().WithParameters(new Parameter("Labas", "labas")).SetSingleton();
        container.AddType<Object>().For<IList>().SetPerDependency();
        container.AddType<Object>().ForImplementedInterfaces().SetPerRequest();

        container.AddInstance(() => new Object()).ForSelf().SetSingleton();
        container.AddInstance(() => new object()).For<Object>().SetPerRequest();
        container.AddInstance(() => new object()).ForImplementedInterfaces().SetPerDependency();
        container.AddInstance(() => new object()).For<Object>();
        return container.Build();
    }






}