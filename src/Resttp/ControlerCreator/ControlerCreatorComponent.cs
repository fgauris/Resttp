using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resttp.Dependencies;

namespace Resttp.ControlerCreator
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    public class ControlerCreatorComponent
    {
        private AppFunc _next;

        public IScopedDependencyResolver DependencyResolver { get; set; }

        public ControlerCreatorComponent(AppFunc next, IScopedDependencyResolver dependencyResolver)
        {
            _next = next;
            DependencyResolver = dependencyResolver;
        }

        public async Task Invoke(IDictionary<string, object> enviroment)
        {
            
            //creating request scope
            var scope = DependencyResolver.StartScope();

            await _next(enviroment);

            //Destroying request scope
            scope.Dispose();

            //TODO implement me
            throw new NotImplementedException();
        }

    }
}
