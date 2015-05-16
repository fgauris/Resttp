using Resttp.Common;
using System.Linq;
using System.Reflection;

namespace Resttp.ActionInvoker
{
    public class ActionDescriptorGenerator
    {

        /// <summary>
        /// Generates action descriptor from controller
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public ActionDescriptor GenerateActionDescriptor(RestController controller)
        {
            var actionDescriptor = new ActionDescriptor(controller.Route.ActionName);
            var action = GetActionMethodInfo(controller, actionDescriptor.ActionName);
            if (action == null)
                return null;
            MapActionParameters(actionDescriptor, action);
            return actionDescriptor;
        }

        public MethodInfo GetActionMethodInfo(RestController controller, string actionName)
        {
            return ControllerHelper.GetActions(controller.GetType()).First(m => m.Name == actionName);
        }

        /// <summary>
        /// Maps action MethodInfo parameters to action descriptor
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="action"></param>
        public void MapActionParameters(ActionDescriptor descriptor, MethodInfo action)
        {
            foreach (var parameter in action.GetParameters())
                descriptor.ActionArguments.Add(new ActionArgumentDescriptor(parameter.Name, parameter.ParameterType, parameter.HasDefaultValue ? parameter.DefaultValue : null));
        }

    }
}
