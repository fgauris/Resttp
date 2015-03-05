using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class ControllerRouteAttribute : Attribute
    {
        private string _template;

        public ControllerRouteAttribute(string template)
        {
            _template = template;
        }

        public string Template { get { return _template; } }
    }
}
