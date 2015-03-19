using System;
using Microsoft.Owin;

namespace Resttp
{
    public interface IHttpRouteResolver
    {
        HttpRoute Resolve(PathString path, string httpMethod);
    }


    public class HttpRouteResolver : IHttpRouteResolver
    {
        private readonly HttpRouteList _routes;

        public HttpRouteList RouteList { get { return _routes; } }

        public HttpRouteResolver(HttpRouteList routes)
        {
            _routes = routes;
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
                if (path.Value.Equals(templatePath, StringComparison.OrdinalIgnoreCase) && route.HttpMethodName.Equals(httpMethod, StringComparison.OrdinalIgnoreCase))
                {
                    return route;
                }
            }
            return null;
        }
    }
}
