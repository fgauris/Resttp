using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resttp
{
    using Microsoft.Owin;
    using Resttp.Routing;
    using System.IO;
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class RoutingComponent
    {
        private AppFunc _next;

        public IHttpRouteResolver Resolver { get; set; }

        public RoutingComponent(AppFunc next, IHttpRouteResolver resolver)
        {
            _next = next;
            Resolver = resolver;
        }

        public async Task Invoke(IDictionary<string, object> enviroment)
        {
            var route = Resolver.Resolve(new PathString(enviroment["owin.RequestPath"] as string), enviroment["owin.RequestMethod"] as string);
            if(route == null)
            {
                await GenerateNotFoundResult(enviroment);
                return;
            }
            else
            {
                enviroment.Add("resttp.Route", route);
                await _next(enviroment);
            }
        }

        public Task GenerateNotFoundResult(IDictionary<string, object> enviroment)
        {
            var responseBody = enviroment["owin.ResponseBody"] as Stream;
            enviroment["owin.ResponseStatusCode"] = 404;
            using (var writer = new StreamWriter(responseBody))
            {
                return writer.WriteAsync("No resource found on provided route.");
            }
        }

    }
}
