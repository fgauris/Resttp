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
            //Rasti actions su actionRoute
            //Patikrinti ar controlleriai turi ControllerRoute attributa
            //Jei neturi, tai exception
            //Jei turi zjbs

            var ctrls = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(t => t.BaseType == typeof(RestController));
            var actions = ctrls
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            ctrls.ToString();
            actions.ToString();
        }




        #endregion
    }

    public class HttpRouteList
    {
        private readonly IList<HttpRoute> _routes;

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
            else
            {
                _routes.Add(new HttpRoute(id, template, controllerName, actionName, parameters));
            }
        }

        public HttpRoute GetRoute(string path, IReadableStringCollection query, string httpMethod)
        {
            
            


            throw new NotImplementedException();
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

    }
    
    public class HttpRoute
    {
        public string Id { get; set; }

        public string Template { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public IDictionary<string, object> DefaultParams { get; set; }
        
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
