using Resttp.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Resttp
{
    public class HttpRoute : IHttpRoute
    {
        public string Id { get; set; }

        public string Template { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string HttpMethodName { get; set; }

        public HttpRoute(string id, string template, string controllerName, string actionName, string httpMethodName)
        {
            if (id == null)
                throw new ArgumentNullException("id");
            if (template == null)
                throw new ArgumentNullException("template");
            if (controllerName == null)
                throw new ArgumentNullException("controllerName");
            if (actionName == null)
                throw new ArgumentNullException("actionName");
            if(httpMethodName == null)
                throw new ArgumentNullException("httpMethodName");

            if (!template.StartsWith("/"))
            {
                throw new ArgumentException("The template must start with a '/' followed by one or more characters.");
            }

            Id = id;
            Template = template;
            ControllerName = controllerName;
            ActionName = actionName;
            HttpMethodName = httpMethodName;
        }
    }
}
