using System.Collections.Generic;
using Resttp.Routing;

namespace Resttp
{
    public abstract class RestController
    {
        public IDictionary<string, object> OwinEnvironment { get; set; }
        public RequestContext Request { get; set; }
        public ResponseContext Response { get; set; }
        public IHttpRoute Route { get; set; }
    }
}
