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

        public static IEnumerable<MethodInfo> GetActions(Assembly assembly, string controllerName)
        {
            return GetController(assembly, controllerName)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

    }
}
