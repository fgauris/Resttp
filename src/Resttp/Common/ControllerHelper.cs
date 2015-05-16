using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Resttp.Common
{
    public static class ControllerHelper
    {
        public static IEnumerable<Type> GetControllers(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(t => t.BaseType == typeof(RestController));
        }

        public static Type GetController(Assembly assembly, string controllerName)
        {
            var name = controllerName.Contains("Controller") ? controllerName : controllerName + "Controller";
            return GetControllers(assembly).First(c => c.Name == name);
        }

        public static IEnumerable<string> GetControllerNames(Assembly assembly)
        {
            return GetControllers(assembly).Select(c => c.Name.Replace("Controller", string.Empty));
        }

        /// <summary>
        /// Gets all controller actions.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetActions(Assembly assembly, string controllerName)
        {
            return GetActions(GetController(assembly, controllerName));
        }

        /// <summary>
        /// Gets all controller actions.
        /// </summary>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetActions(Type controllerType)
        {
            return controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

    }
}
