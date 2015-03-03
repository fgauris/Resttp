using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic;

namespace Resttp
{
    public class ResttpConfiguration
    {
        public HttpRouteList HttpRoutes { get; private set; }

        public ResttpConfiguration()
        {
            HttpRoutes = new HttpRouteList();
        }

        public void MapHttpRoutesFromAttributes()
        {

        }
    }

    public class HttpRouteList
    {
        private readonly List<HttpRoute> _routes;

        //For testing only
        private readonly string[] controllers;

        private readonly string[] httpMethods;

        public HttpRouteList()
        {
            _routes = new List<HttpRoute>();
            controllers = new[]
            {
                "account", "home", "text", "time"
            };
            httpMethods = new[]
            {
                "get", "post", "put", "delete", "patch", "head"
            };
        }

        public void AddRoute(string id, string template, string controller, string action, dynamic defaults)
        {
            if(defaults == null)
            {
                defaults = new { };
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
                var paramsDictionary = new Dictionary<string, object>();
                foreach (var prop in defaults.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public) as PropertyInfo[])
                {
                    paramsDictionary.Add(prop.Name, prop.GetValue(defaults));
                }
                AddRoute(id, template, controller, action, paramsDictionary);
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
            else
            {
                _routes.Add(new HttpRoute(id, template, controllerName, actionName, parameters));
            }
        }

        public HttpRoute GetRoute(string path, string httpMethod)
        {
            throw new NotImplementedException();
        }
    }

    public class HttpRoute
    {
        public string Id { get; set; }

        public string Template { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public Dictionary<string, object> Params { get; set; }

        public HttpRoute(string id, string template, string controllerName, string actionName, Dictionary<string, object> parameters)
        {
            if (id == null)
                throw new ArgumentNullException("id");
            if (template == null)
                throw new ArgumentNullException("template");
            if (controllerName == null)
                throw new ArgumentNullException("controllerName");
            if (actionName == null)
                throw new ArgumentNullException("actionName");
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            Id = id.ToLower();
            Template = template.ToLower();
            ControllerName = controllerName.ToLower();
            ActionName = actionName.ToLower();
            Params = parameters;
        }
    }
}
