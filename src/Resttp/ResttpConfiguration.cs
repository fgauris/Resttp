using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Collections;

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
                    var actionName = action.Name;
                    var controllerName = action.DeclaringType.Name.Replace("Controller", string.Empty);
                    HttpRoutes.AddRoutes(
                        name: actionAttr.Name ?? ctrlAttr.Name ?? controllerName + actionName,
                        template: '/' + ctrlAttr.Template.Trim('/') + "/" + actionAttr.Template.Trim('/'),
                        defaults: new
                        {
                            controller = controllerName,
                            action = actionName
                        }
                    );
                }
            }
        }

        private void ValidateRouteAttributeExistance(IEnumerable<MethodInfo> actions)
        {
            foreach (var action in actions)
            {
                if (action.DeclaringType.GetCustomAttribute<ControllerRouteAttribute>() == null)
                    throw new RoutingException(string.Format("Action '{0}' contains route attribute, but controller '{1}' does not.", action.Name, action.DeclaringType.Name));
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
}
