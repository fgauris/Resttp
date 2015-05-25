using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp.ActionInvoker
{
    public class ActionParameterBinder : IActionParameterBinder
    {
        public  void BindParameters(ActionDescriptor actionDescriptor, IDictionary<string, object> environment)
        {
            throw new NotImplementedException();
        }
    }
}
