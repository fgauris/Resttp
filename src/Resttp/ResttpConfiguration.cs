using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Resttp
{
    public class ResttpConfiguration
    {
        public HttpRouteList HttpRoutes { get; private set; }

        public ResttpConfiguration()
        {
            HttpRoutes = new HttpRouteList();
        }
        #region Routing
        public void MapHttpRoutesFromAttributes()
        {
            var actions = GetActionsWithRouteAttribute(Assembly.GetEntryAssembly());
            ValidateRouteAttributeExistance(actions);

            foreach (var action in actions)
            {
                var actionAttr = action.GetCustomAttribute<ActionRouteAttribute>();
                var ctrlAttr = action.DeclaringType.GetCustomAttribute<ControllerRouteAttribute>();
                
                if (HttpRoutes.GetRoute(action.DeclaringType.Name, action.Name) == null)
                {
                    HttpRoutes.AddRoute(
                        id: actionAttr.Id ?? action.DeclaringType.Name + action.Name,
                        template: ctrlAttr.Template + "/" + actionAttr.Template,
                        controller: action.DeclaringType.Name,
                        action: action.Name
                    );
                }
            }
        }

        private void ValidateRouteAttributeExistance(IEnumerable<MethodInfo> actions)
        {
            foreach (var action in actions)
            {
                if (action.DeclaringType.GetCustomAttribute<ControllerRouteAttribute>() == null)
                    throw new RoutingException(action);
            }
        }

        private IEnumerable<MethodInfo> GetActionsWithRouteAttribute(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(t => t.BaseType == typeof(RestController))
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                .Where(m => m.GetCustomAttribute<ActionRouteAttribute>() != null);
        }

        #endregion
    }

    public class HttpRouteList
    {
        protected readonly IList<HttpRoute> _routes;

        //For testing only
        private readonly IEnumerable<string> controllers;

        private readonly IEnumerable<string> httpMethods;

        public HttpRouteList()
        {
            _routes = new List<HttpRoute>();
            controllers = GetControllers(Assembly.GetEntryAssembly()).Select(c => c.Name);
            httpMethods = new[]
            {
                "Get", "Post", "Put", "Delete", "Patch", "Head", "Options"
            };
        }

        public void AddRoute(string id, string template, string controller = null, string action = null, dynamic defaults = null)
        {
            if(defaults == null)
            {
                defaults = new object();
            }
            if (controller == null)
            {
                AddRouteWithoutController(id, template, action, defaults);
            }
            else if (action == null)
            {
                AddRouteWithoutAction(id, template, controller, defaults);
            }
            else
            {
                AddRoute(id, template, controller, action, ConvertToDictionary(defaults));
            }
        }

        private void AddRouteWithoutController(string id, string template, string action, dynamic defaults)
        {
            foreach (var controller in controllers)
            {
                AddRoute(id, template, controller, action, defaults);
            }
        }

        private void AddRouteWithoutAction(string id, string template, string controller, dynamic defaults)
        {
            foreach (var action in httpMethods)
            {
                AddRoute(id, template, controller, action, defaults);
            }
        }

        private void AddRoute(string id, string template, string controllerName, string actionName, Dictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }
            AddRoute(new HttpRoute(id, template, controllerName, actionName, parameters));
        }

        private void AddRoute(HttpRoute route)
        {
            var oldRoute = _routes.FirstOrDefault(r => r.ActionName == route.ActionName && r.ControllerName == route.ControllerName);
            if (oldRoute == null)
            {
                _routes.Add(route);
            }
        }

        public HttpRoute GetRoute(string path, IReadableStringCollection query, string httpMethod)
        {
            throw new NotImplementedException();
        }

        public HttpRoute GetRoute(string id)
        {
            return _routes.FirstOrDefault(r => r.Id == id);
        }

        public HttpRoute GetRoute(string controller, string action)
        {
            return _routes.FirstOrDefault(r => r.ControllerName == controller && r.ActionName == action);
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

    }
    
    public class HttpRoute
    {
        public string Id { get; set; }

        public string Template { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public IDictionary<string, object> DefaultParams { get; set; }

        public HttpRoute()
        {
            DefaultParams = new Dictionary<string, object>();
        }

        public HttpRoute(string id, string template, string controllerName, string actionName, IDictionary<string, object> defaults)
        {
            if (id == null)
                throw new ArgumentNullException("id");
            if (template == null)
                throw new ArgumentNullException("template");
            if (controllerName == null)
                throw new ArgumentNullException("controllerName");
            if (actionName == null)
                throw new ArgumentNullException("actionName");
            if (defaults == null)
                throw new ArgumentNullException("parameters");

            Id = id;
            Template = template;
            ControllerName = controllerName;
            ActionName = actionName;
            DefaultParams = defaults;
        }

        
    }
}
