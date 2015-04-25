using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;
using Resttp.Routing;

namespace Resttp
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class ResttpComponent
    {
        private AppFunc _next;

        public ResttpConfiguration Config { get; set; }

        public ResttpComponent(AppFunc next, ResttpConfiguration config)
        {
            _next = next;
            Config = config;

            //TestRoutes(new HttpRouteResolver(Config.HttpRoutes));
        }

        public async Task Invoke(IDictionary<string, object> enviroment)
        {
            var route = enviroment["resttp.Route"] as HttpRoute;
            route.ToString();
            //Do something here....
            await _next(enviroment);
            //Do something here too....

            var response = enviroment["owin.ResponseBody"] as Stream;
            enviroment["owin.ResponseStatusCode"] = 200;
            using (var writer = new StreamWriter(response))
            {
                await writer.WriteAsync("Hello!");
                return;
            }
        }

        private void TestRoutes(IHttpRouteResolver resolver)
        {
            var s1 = resolver.Resolve(new PathString("/lt/Index"), "Post"); s1.ToString();
            var s3 = resolver.Resolve(new PathString("/lt"), "Get"); s3.ToString();
            var s4 = resolver.Resolve(new PathString("/lt"), "Post"); s4.ToString();
            var s5 = resolver.Resolve(new PathString("/lt"), "Put"); s5.ToString();
            //var s3 = resolver.Resolve(new PathString("/Antras/labas")); s3.ToString();



        }


    }
}
