using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resttp.IoC.Registration
{
    public class Component : IDisposable
    {
        public ComponentRegistration ComponentRegistration { get; private set; }

        public Component(Type createType, Expression<Func<object>> @delegate)
        {
            ComponentRegistration = new ComponentRegistration(createType) 
            {
                ResultFunc = @delegate 
            };
        }

        public Component(Type createType)
        {
            ComponentRegistration = new ComponentRegistration(createType);
        }

        public Component(Type createType, Type lookUpType)
        {
            ComponentRegistration = new ComponentRegistration(createType, lookUpType);
        }

        public Component ForSelf()
        {
            ComponentRegistration.LookupTypes.Add(ComponentRegistration.CreateType);
            return this;
        }

        public Component For<T>()
        {
            ComponentRegistration.LookupTypes.Add(typeof(T));
            return this;
        }

        public Component ForImplementedInterfaces()
        {
            foreach (var @interface in ComponentRegistration.CreateType.GetInterfaces())
            {
                ComponentRegistration.LookupTypes.Add(@interface);
            }
            return this;
        }

        public Component WithParameters(params Parameter[] parameters)
        {
            foreach (var p in parameters)
            {
                ComponentRegistration.Parameters.Add(p);
            }
            return this;
        }

        public Component SetSingleton()
        {
            ComponentRegistration.Mode = "Singleton";
            return this;
        }

        public Component SetPerRequest()
        {
            ComponentRegistration.Mode = "Request";
            return this;
        }

        public Component SetPerDependency()
        {
            ComponentRegistration.Mode = "Dependency";
            return this;
        }

        public void Dispose()
        {
            ComponentRegistration.Dispose();
        }
    }
}
