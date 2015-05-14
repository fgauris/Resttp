using Resttp.Common;
using Resttp.Routing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Resttp
{
    public class HttpRouteList : IEnumerable<IHttpRoute>
    {
        private Assembly EntryAssembly { get { return Assembly.GetEntryAssembly(); } }

        private readonly IList<IHttpRoute> _routes;

        //For testing only

        private readonly IEnumerable<string> _httpMethods;

        public HttpRouteList()
        {
            _routes = new List<IHttpRoute>();
            _httpMethods = new[]
            {
                "Get", "Post", "Put", "Delete", "Patch", "Head", "Options"
            };
        }

        public IHttpRoute GetRoute(string controller, string action)
        {
            return _routes.FirstOrDefault(r => r.ControllerName == controller && r.ActionName == action);
        }

        public void AddRoutes(string name, string template, dynamic defaults = null)
        {
            if (name == null)
                throw new ArgumentNullException("id");
            if (template == null)
                throw new ArgumentNullException("template");
            if (defaults == null)
            {
                defaults = new object();
            }
            var defaultsDictionary = ConvertToDictionary(defaults) as IDictionary<string, object>;
            var controller = defaultsDictionary.ContainsKey("controller") ? defaultsDictionary["controller"].ToString() : null;
            var action = defaultsDictionary.ContainsKey("action") ? defaultsDictionary["action"].ToString() : null;
            var httpMethod = defaultsDictionary.ContainsKey("httpMethod") ? defaultsDictionary["httpMethod"].ToString() : null;
            var resolvedTemplate = ResolveTemplate(template, defaultsDictionary);
            AddRoute(name, resolvedTemplate, controller, action, httpMethod);
        }

        private void AddRouteWithoutController(string name, string template, string action, string httpMethod)
        {
            foreach (var controller in ControllerHelper.GetControllerNames(EntryAssembly))
            {
                AddRoute(name, template.Replace("{controller}", controller), controller, action, httpMethod);
            }
        }

        private void AddRouteWithoutAction(string name, string template, string controller, string httpMethod)
        {
            var actions = ControllerHelper.GetActions(EntryAssembly, controller);
            if (!template.Contains("{action}"))
            {
                actions = actions
                    .Where(a => a.Name == "Get" || a.Name == "Post" || a.Name == "Put" || a.Name == "Delete" || a.Name == "Patch" || a.Name == "Head" || a.Name == "Options");
            }
            foreach (var action in actions)
            {
                AddRoute(name, template.Replace("{action}", action.Name), controller, action.Name, httpMethod);
            }
        }

        private void AddRoute(string name, string template, string controllerName, string actionName, string httpMethod)
        {
            if (controllerName == null)
            {
                AddRouteWithoutController(name, template, actionName, httpMethod);
            }
            else if (actionName == null)
            {
                AddRouteWithoutAction(name, template, controllerName, httpMethod);
            }
            else
            {
                AddRoute(new HttpRoute(name, template, controllerName, actionName, ResolveHttpMethod(controllerName, actionName, httpMethod)));
            }

        }

        private void AddRoute(IHttpRoute route)
        {
            var oldRoute = _routes.FirstOrDefault(r => r.ActionName == route.ActionName && r.ControllerName == route.ControllerName);
            if (oldRoute == null)
                _routes.Add(route);
        }

        private IDictionary<string, object> ConvertToDictionary(dynamic dyn)
        {
            if (dyn != null)
                return (dyn.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public) as PropertyInfo[])
                    .ToDictionary<PropertyInfo, string, object>(prop => prop.Name, prop => prop.GetValue(dyn));
            else 
                return  new Dictionary<string, object>();
        }

        private string ResolveTemplate(string template, IDictionary<string, object> parameters)
        {
            var resolved = new StringBuilder(template);
            resolved = parameters.Aggregate(resolved, (current, parameter) => current.Replace("{" + parameter.Key + "}", parameter.Value.ToString()));
            return resolved.ToString();
        }

        /// <summary>
        /// Resolves http method for route
        /// </summary>
        private string ResolveHttpMethod(string controllerName, string actionName, string httpMethod)
        {
            var methodAttribute = ControllerHelper.GetActions(Assembly.GetEntryAssembly(), controllerName)
                .First(m => m.Name == actionName)
                .GetCustomAttribute<HttpAttribute>();
            if (methodAttribute != null)
                return methodAttribute.Method;

            if (httpMethod != null)
                return httpMethod;

            string defaultMethod;
            if (actionName.StartsWith("Get", StringComparison.OrdinalIgnoreCase))
                defaultMethod = "Get";
            else if (actionName.StartsWith("Post", StringComparison.OrdinalIgnoreCase))
                defaultMethod = "Post";
            else if (actionName.StartsWith("Put", StringComparison.OrdinalIgnoreCase))
                defaultMethod = "Put";
            else if (actionName.StartsWith("Delete", StringComparison.OrdinalIgnoreCase))
                defaultMethod = "Delete";
            else if (actionName.StartsWith("Head", StringComparison.OrdinalIgnoreCase))
                defaultMethod = "Head";
            else if (actionName.StartsWith("Patch", StringComparison.OrdinalIgnoreCase))
                defaultMethod = "Patch";
            else if (actionName.StartsWith("Options", StringComparison.OrdinalIgnoreCase))
                defaultMethod = "Options";
            else defaultMethod = "Post";
            return defaultMethod;
        }

        #region IEnumerable
        public IEnumerator<IHttpRoute> GetEnumerator()
        {
            return _routes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _routes.GetEnumerator();
        }
        #endregion
    }
}
