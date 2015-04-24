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
        private IList<Component> Components { get; set; }

        public IoCContainer()
        {
            Components = new List<Component>();
        }

        #region Adding
        public Component Add<T>(Func<T> func)
        {
            throw new NotImplementedException();
        }

        public Component AddType<T>()
        {
            var component = new Component(typeof (T));
            Components.Add(component);
            return component;
        }

        public Component AddInstance(object p0)
        {
            throw new NotImplementedException();
        }

        #endregion

        
        public IDependencyResolver Build()
        {
            throw new NotImplementedException();
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
