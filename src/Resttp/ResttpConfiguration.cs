using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;
using Resttp.Dependencies;

namespace Resttp
{
    public class ResttpConfiguration
    {
        public Assembly ControllersAssembly { get; set; }

        public HttpRouteList HttpRoutes { get; private set; }

        public ResttpConfiguration(Assembly controllersAssembly)
        {
            HttpRoutes = new HttpRouteList();
            ControllersAssembly = controllersAssembly;
        }

        #region Routing
        public void MapHttpRoutesFromAttributes()
        {
            var actions = GetActionsWithRouteAttribute(Assembly.GetEntryAssembly()).ToArray();
            ValidateRouteAttributeExistance(actions);

            foreach (var action in actions)
            {
                var actionAttr = action.GetCustomAttribute<ActionRouteAttribute>();
                var ctrlAttr = action.DeclaringType.GetCustomAttribute<ControllerRouteAttribute>();

                if (HttpRoutes.GetRoute(action.DeclaringType.Name, action.Name) != null)
                    return;
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

        private void ValidateRouteAttributeExistance(IEnumerable<MethodInfo> actions)
        {
            var action =
                actions.FirstOrDefault(a => a.DeclaringType.GetCustomAttribute<ControllerRouteAttribute>() == null);
            if (action != null)
                throw new RoutingException(string.Format("Action '{0}' contains route attribute, but controller '{1}' does not.", action.Name, action.DeclaringType.Name));
        }

        private IEnumerable<MethodInfo> GetActionsWithRouteAttribute(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(t => t.BaseType == typeof(RestController))
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                .Where(m => m.GetCustomAttribute<ActionRouteAttribute>() != null);
        }

        #endregion

        #region Dependency injection

        public IScopedDependencyResolver DependencyResolver { get; set; }

        #endregion


    }
}
