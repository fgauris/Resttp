using Resttp.IoC;
using System.Linq;
using System.Reflection;

namespace Resttp.Extensions
{
    public static class IoCContainerBuilderExtensions
    {

        public static void AddResttpControllers(this IoCContainerBuilder builder, Assembly assembly)
        {
            var controllerTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(RestController)));
            foreach (var controllerType in controllerTypes)
            {
                builder.AddType(controllerType).ForSelf().SetPerDependency();
            }
        }
    }
}
