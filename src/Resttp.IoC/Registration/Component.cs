using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp.IoC.Registration
{
    public class Component
    {
        public ComponentRegistration ComponentRegistration { get; private set; }

        public Component(Type createType)
        {
            ComponentRegistration = new ComponentRegistration(createType);
        }

        public Component(Type createType, Type lookUpType)
        {
            ComponentRegistration = new ComponentRegistration(lookUpType, createType);
        }

        public Component ForSelf()
        {
            ComponentRegistration.LookupType = ComponentRegistration.CreateType;
            return this;
        }

        public Component For<T>()
        {
            throw new NotImplementedException();
        }

        public Component ForImplementedInterfaces()
        {
            throw new NotImplementedException();
        }

        public Component WithParameters(object o)
        {
            throw new NotImplementedException();
        }

        public Component SetSingleton()
        {
            throw new NotImplementedException();
        }

        public Component SetPerRequest()
        {
            throw new NotImplementedException();
        }

        public Component SetPerDependency()
        {
            throw new NotImplementedException();
        }
    }
}
