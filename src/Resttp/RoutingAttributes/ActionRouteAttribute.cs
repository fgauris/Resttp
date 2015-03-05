using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ActionRouteAttribute : Attribute
    {
        private string _template;

        public ActionRouteAttribute(string template)
        {
            _template = template;
        }

        public string Template { get { return _template; } }
    }
}
