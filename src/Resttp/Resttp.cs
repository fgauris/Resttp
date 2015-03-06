using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resttp
{
    using Microsoft.Owin;
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class ResttpComponent
    {
        private AppFunc _next;

        public ResttpConfiguration Config { get; set; }

        public ResttpComponent(AppFunc next, ResttpConfiguration config)
        {
            _next = next;
            Config = config;

            var resolver = new HttpRouteResolver(Config.HttpRoutes);
            var s1 = resolver.Resolve(new PathString("/Pirmas/Labas")); s1.ToString();
            var s2 = resolver.Resolve(new PathString("/pirmas/labas")); s2.ToString();
            var s3 = resolver.Resolve(new PathString("/Antras/labas")); s3.ToString();



        }

        public async Task Invoke(IDictionary<string, object> enviroment)
        {
            //Do something here....
            await _next(enviroment);
            //Do something here too....
            
            //var response = enviroment["owin.ResponseBody"] as Stream;
            //using (var writer = new StreamWriter(response))
            //{
            //    return writer.WriteAsync("Hello!");
            //}
        }
    }
}
