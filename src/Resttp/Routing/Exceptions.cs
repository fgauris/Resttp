using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Resttp
{
    public class RoutingException : Exception
    {
        public RoutingException(string message): base(message){}

        public RoutingException(MethodInfo action)
            : base(string.Format("Action '{0}' contains route attribute, but controller '{1}' does not.", action.Name, action.DeclaringType.Name))
        {
        }

    }
}
