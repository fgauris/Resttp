using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp
{
    public class HttpRouteResolver
    {
        private readonly HttpRouteList _routes;

        public HttpRouteList RouteList { get { return _routes; } }

        public HttpRouteResolver(HttpRouteList routes)
        {
            _routes = routes;
        }

        public HttpRoute Resolve(OwinRequest request)
        {
            return Resolve(request.Path, request.Method);
        }

        public HttpRoute Resolve(PathString path, string httpMethod)
        {
            if (!path.HasValue)
            {
                return null;
            }
            foreach (var route in RouteList)
            {
                var templatePath = route.ResolvedTemplate.Split('?')[0];
                if (path.Value.Equals(templatePath, StringComparison.OrdinalIgnoreCase) && route.HttpMethodName == httpMethod)
                {
                    return route;
                }
            }
            return null;
        }
    }
}
