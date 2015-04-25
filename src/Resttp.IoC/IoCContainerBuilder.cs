using Resttp.Dependencies;
using Resttp.IoC.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Resttp.IoC
{
    public class IoCContainerBuilder : IDisposable
    {
        private IList<Component> Components { get; set; }

        public IoCContainerBuilder()
        {
            Components = new List<Component>();
        }

        #region Adding

        public Component AddType<T>()
        {
            var component = new Component(typeof(T));
            Components.Add(component);
            return component;
        }

        public Component AddInstance<T>(Expression<Func<T>> creator)
        {
            var component = new Component(typeof(T));

            var convertedCreator = Expression.Lambda<Func<object>>(
                    Expression.Convert(creator.Body, typeof(object)), creator.Parameters
                );

            component.ComponentRegistration.ResultFunc = convertedCreator;
            Components.Add(component);
            return component;
        }

        #endregion

        public IDependencyResolver Build()
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            Components.Clear();
        }
    }
}
