using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resttp.Dependencies;

namespace Resttp.ControllerCreator
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using Resttp.Routing;
    using System.IO;
    using System.Reflection;
    public class ControlerCreatorComponent
    {
        private AppFunc _next;

        public IScopedDependencyResolver DependencyResolver { get; set; }

        public Assembly ControllersAssembly { get; set; }

        public ControlerCreatorComponent(AppFunc next, IScopedDependencyResolver dependencyResolver, Assembly controllersAssembly)
        {
            _next = next;
            DependencyResolver = dependencyResolver;
            ControllersAssembly = controllersAssembly;
        }

        public async Task Invoke(IDictionary<string, object> enviroment)
        {
            //getting route 
            var route = enviroment["resttp.Route"] as IHttpRoute;
            if (route == null)
                throw new Exception("Route not found in OWIN environment dictionary");

            //creating request scope
            var scope = DependencyResolver.StartScope();

            //creating request controller
            var controller = CreateController(enviroment, route, scope);
            if (controller == null)
                throw new Exception("Failed to create a controller " + route.ControllerName);
            enviroment.Add("resttp.Controller", controller);

            await _next(enviroment);

            //Destroying request scope
            scope.Dispose();
        }

        private RestController CreateController(IDictionary<string, object> enviroment, IHttpRoute route, IDependencyResolver resolver)
        {
            var controller = resolver.Resolve(GetControllerType(route.ControllerName + "Controller")) as RestController;
            controller.OwinEnvironment = enviroment;
            controller.Request = new RequestContext(enviroment, route.ActionName);
            controller.Response = new ResponseContext(enviroment);
            return controller;
        }

        private Type GetControllerType(string controllerName)
        {
            var controller = ControllersAssembly.GetTypes().FirstOrDefault(t => t.Name == controllerName);
            if (controller != null)
                return controller;
            else
                throw new Exception(controllerName + " type not found in assembly " + ControllersAssembly);
        }

    }
}
