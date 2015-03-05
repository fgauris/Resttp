﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class ActionRouteAttribute : Attribute
    {
        private string _template;

        public ActionRouteAttribute(string template)
        {
            _template = template;
        }

        /// <summary>
        /// Id of a route
        /// </summary>
        public string Id { get; set; }
        public string Template { get { return _template; } }
    }
}
