using Microsoft.Owin;
using Resttp.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp
{
    public class HttpRouteResolver : IHttpRouteResolver
    {
        private readonly HttpRouteList _routes;

        public HttpRouteList RouteList { get { return _routes; } }

        public HttpRouteResolver(HttpRouteList routes)
        {
            _routes = routes;
        }

        public IHttpRoute Resolve(PathString path, string httpMethod)
        {
            if (!path.HasValue)
            {
                return null;
            }
            foreach (var route in RouteList)
            {
                var templatePath = route.Template.Split('?')[0];
                if (path.Value.Equals(templatePath, StringComparison.OrdinalIgnoreCase) && route.HttpMethodName.Equals(httpMethod, StringComparison.OrdinalIgnoreCase))
                {
                    return route;
                }
            }
            return null;
        }
    }
}
