using Resttp.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Resttp
{
    using Resttp.Routing;
    using System.Reflection;
    using AppFunc = Func<IDictionary<string, object>, Task>;

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

        public async Task Invoke(IDictionary<string, object> environment)
        {
            //getting route 
            var route = environment["resttp.Route"] as IHttpRoute;
            if (route == null)
                throw new Exception("Route not found in OWIN environment dictionary");

            //creating request scope
            var scope = DependencyResolver.StartScope();

            //creating request controller
            var controller = CreateController(environment, route, scope);
            if (controller == null)
                throw new Exception("Failed to create a controller " + route.ControllerName);
            environment.Add("resttp.Controller", controller);

            await _next(environment);

            //Destroying request scope
            scope.Dispose();
        }

        private RestController CreateController(IDictionary<string, object> enviroment, IHttpRoute route, IDependencyResolver resolver)
        {
            var controller = resolver.Resolve(GetControllerType(route.ControllerName + "Controller")) as RestController;
            controller.OwinEnvironment = enviroment;
            controller.Request = new RequestContext(enviroment);
            controller.Response = new ResponseContext(enviroment);
            controller.Route = route;
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
