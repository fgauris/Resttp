using System;

namespace Resttp
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ControllerRouteAttribute : Attribute
    {
        private string _template;

        public string Name { get; set; }

        public ControllerRouteAttribute(string template)
        {
            _template = template;
        }

        public string Template { get { return _template; } }
    }
}
