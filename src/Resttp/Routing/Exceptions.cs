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
    }
}
