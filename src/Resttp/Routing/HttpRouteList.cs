using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Resttp
{
    public class HttpRouteList : IEnumerable<HttpRoute>
    {
        private Assembly EntryAssembly { get { return Assembly.GetEntryAssembly(); } }

        private readonly IList<HttpRoute> _routes;

        //For testing only

        private readonly IEnumerable<string> httpMethods;

        public HttpRouteList()
        {
            _routes = new List<HttpRoute>();
            httpMethods = new[]
            {
                "Get", "Post", "Put", "Delete", "Patch", "Head", "Options"
            };
        }

        public HttpRoute GetRoute(string id)
        {
            return _routes.FirstOrDefault(r => r.Id == id);
        }

        public HttpRoute GetRoute(string controller, string action)
        {
            return _routes.FirstOrDefault(r => r.ControllerName == controller && r.ActionName == action);
        }

        public void AddRoute(string id, string template, dynamic defaults = null)
        {
            if (id == null)
                throw new ArgumentNullException("id");
            if (template == null)
                throw new ArgumentNullException("template");
            if (defaults == null)
            {
                defaults = new object();
            }
            var defaultsDictionary = ConvertToDictionary(defaults) as IDictionary<string, object>;
            string controller = defaultsDictionary.ContainsKey("controller") ? defaultsDictionary["controller"].ToString() : null;
            string action = defaultsDictionary.ContainsKey("action") ? defaultsDictionary["action"].ToString() : null;
            string httpMethod = defaultsDictionary.ContainsKey("httpMethod") ? defaultsDictionary["httpMethod"].ToString() : null;
            var resolvedTemplate = ResolveTemplate(template, defaultsDictionary);
            AddRoute(id, resolvedTemplate, controller, action, httpMethod);
        }

        private void AddRouteWithoutController(string id, string template, string action, string httpMethod)
        {
            var controllerNames = GetControllers(EntryAssembly).Select(c => c.Name.Replace("Controller", string.Empty));
            foreach (var controller in controllerNames)
            {
                AddRoute(id, template.Replace("{controller}", controller), controller, action, httpMethod);
            }
        }

        private void AddRouteWithoutAction(string id, string template, string controller, string httpMethod)
        {
            IEnumerable<MethodInfo> actions = GetActions(EntryAssembly, controller);
            if (!template.Contains("{action}"))
            {
                actions = actions
                    .Where(a => a.Name == "Get" || a.Name == "Post" || a.Name == "Put" || a.Name == "Delete" || a.Name == "Patch" || a.Name == "Head" || a.Name == "Options");
            }
            foreach (var action in actions)
            {
                AddRoute(id, template.Replace("{action}", action.Name), controller, action.Name, httpMethod);
            }
        }

        private void AddRoute(string id, string template, string controllerName, string actionName, string httpMethod)
        {
            if (controllerName == null)
            {
                AddRouteWithoutController(id, template, actionName, httpMethod);
            }
            else if (actionName == null)
            {
                AddRouteWithoutAction(id, template, controllerName, httpMethod);
            }
            else
            {
                AddRoute(new HttpRoute(id, template, controllerName, actionName, ResolveHttpMethod(controllerName, actionName, httpMethod)));
            }

        }

        private void AddRoute(HttpRoute route)
        {
            var oldRoute = _routes.FirstOrDefault(r => r.ActionName == route.ActionName && r.ControllerName == route.ControllerName);
            if (oldRoute == null)
                _routes.Add(route);
        }

        private IDictionary<string, object> ConvertToDictionary(dynamic dyn)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var prop in dyn.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public) as PropertyInfo[])
            {
                dictionary.Add(prop.Name, prop.GetValue(dyn));
            }
            return dictionary;
        }

        private IEnumerable<Type> GetControllers(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(t => t.BaseType == typeof(RestController));
        }

        private Type GetController(Assembly assembly, string controllerName)
        {
            var name = controllerName.Contains("Controller") ? controllerName : controllerName + "Controller";
            return GetControllers(assembly).First(c => c.Name == name);
        }

        private IEnumerable<MethodInfo> GetActions(Assembly assembly, string controllerName)
        {
            return GetController(assembly, controllerName)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        private string ResolveTemplate(string template, IDictionary<string, object> parameters)
        {
            var resolved = new System.Text.StringBuilder(template);
            foreach (var parameter in parameters)
            {
                resolved = resolved.Replace("{" + parameter.Key + "}", parameter.Value.ToString());
            }
            return resolved.ToString();
        }

        private string ResolveHttpMethod(string controllerName, string actionName, string httpMethod)
        {
            var methodAttribute = GetActions(Assembly.GetEntryAssembly(), controllerName)
                .First(m => m.Name == actionName)
                .GetCustomAttribute<HttpAttribute>();
            if (methodAttribute != null)
                return methodAttribute.Method;

            if (httpMethod != null)
                return httpMethod;

            string defaultMethod = null;
            if (actionName.Equals("Get", StringComparison.OrdinalIgnoreCase))
                defaultMethod = "Get";
            else if (actionName.Equals("Post", StringComparison.OrdinalIgnoreCase))
                defaultMethod = "Post";
            else if (actionName.Equals("Put", StringComparison.OrdinalIgnoreCase))
                defaultMethod = "Put";
            else if (actionName.Equals("Delete", StringComparison.OrdinalIgnoreCase))
                defaultMethod = "Delete";
            else if (actionName.Equals("Head", StringComparison.OrdinalIgnoreCase))
                defaultMethod = "Head";
            else if (actionName.Equals("Patch", StringComparison.OrdinalIgnoreCase))
                defaultMethod = "Patch";
            else if (actionName.Equals("Options", StringComparison.OrdinalIgnoreCase))
                defaultMethod = "Options";
            else defaultMethod = "Post";
            return defaultMethod;
        }

        #region IEnumerable
        public IEnumerator<HttpRoute> GetEnumerator()
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
