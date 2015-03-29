using Microsoft.Owin;

namespace Resttp.Routing
{
    public interface IHttpRouteResolver
    {
        IHttpRoute Resolve(PathString path, string httpMethod);
    }
}
