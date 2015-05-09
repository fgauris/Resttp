using System.Collections.Generic;
namespace Resttp
{
    public abstract class RestController
    {
        public IDictionary<string, object> OwinEnvironment { get; set; }
        public RequestContext Request { get; set; }
        public ResponseContext Response { get; set; }
    }
}
