using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp.ActionInvoker
{
    public interface IActionParameterBinder
    {
        void BindParameters(ActionDescriptor actionDescriptor, IDictionary<string, object> environment);
    }
}
