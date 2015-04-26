using Resttp.Dependencies;
using Resttp.IoC.Registration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Resttp.IoC
{
    public class IoCContainerBuilder
    {
        private IList<Component> Components { get; set; }

        public IoCContainerBuilder()
        {
            Components = new List<Component>();
        }

        #region Adding
        public Component AddType<T>()
        {
            return AddType(typeof(T));
        }

        public Component AddType(Type createType)
        {
            var component = new Component(createType);
            Components.Add(component);
            return component;
        }

        public Component AddInstance<T>(Func<T> creator)
        {
            var component = new Component(null, typeof(T));
            Func<object> convertedCreator = () => creator();
            component.ComponentRegistration.ResultFunc = convertedCreator;
            Components.Add(component);
            return component;
        }

        #endregion

        public IScopedDependencyResolver Build()
        {
            var container = new IoCContainer(null, Components.Select(c => c.ComponentRegistration).ToList());
            return container;
        }
    }
}
