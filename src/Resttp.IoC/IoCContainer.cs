using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resttp.Dependencies;
using Resttp.IoC.Registration;

namespace Resttp.IoC
{
    public class IoCContainer : IDependencyResolver
    {
        

        public IoCContainer()
        {
            
        }

        public object Resolve(Type type)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
