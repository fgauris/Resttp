using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp.Routing
{
    public interface IHttpRoute
    {
        string Id { get; set; }
        string Template { get; set; }
        string ControllerName { get; set; }
        string ActionName { get; set; }
        string HttpMethodName { get; set; }
    
    }
}
