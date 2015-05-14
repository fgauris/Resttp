using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resttp.Dependencies;
using Resttp.IoC.Registration;
using System.Reflection;
using Resttp.IoC.Exceptions;

namespace Resttp.IoC
{
    public class IoCContainer : IScopedDependencyResolver
    {
        private int _level;
        public IoCContainer ParentContainer { get; set; }
        public IList<ComponentRegistration> ComponentRegistrations { get; set; }
        public IDictionary<Type, object> Objects { get; set; }


        public IoCContainer(IoCContainer parent, IList<ComponentRegistration> registrations)
        {
            ParentContainer = parent;
            _level = parent != null ? parent._level + 1 : 1;
            ComponentRegistrations = registrations;
            Objects = new Dictionary<Type, object>();
        }

        public IoCContainer CreateChildContainer()//IDictionary<Type, Object> objects)
        {
            return new IoCContainer(this, ComponentRegistrations)
                //{
                //    Objects = objects.ToDictionary(e => e.Key, e => e.Value)
                //}
            ;
        }

        public IDependencyResolver StartScope()
        {
            return CreateChildContainer();//Objects);
        }

        public void Dispose()
        {
            Objects.Clear();
        }


        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            object result = null;
            var reg = ComponentRegistrations.FirstOrDefault(r => r.LookupTypes.Contains(type));
            if (reg == null)
                throw new IoCException("Type not found in IoC container. Type: " + type.FullName);

            if (reg.Level < _level)//Get from parent
            {
                if (ParentContainer == null)
                    throw new IoCException(string.Format("Bad level. Current: {0}. Required: {1}.", _level, reg.Level));
                result = ParentContainer.Resolve(type);
            }
            else//From this
            {
                if (Objects.ContainsKey(type))//from cache
                {
                    return Objects[type];
                }
                else//create new
                {
                    if (reg.ResultFunc != null)
                        result = reg.ResultFunc();
                    else
                    {
                        var constructor = type.GetConstructors()
                                .Where(c => c.GetParameters().Count() > 0)
                                .OrderByDescending(c => c.GetParameters().Count())
                                .SingleOrDefault();

                        if (constructor == null)
                            result = Activator.CreateInstance(reg.CreateType);
                        else
                            result = constructor.Invoke(ResolveParameters(reg, constructor.GetParameters()).ToArray());
                    }

                    //Add to cache
                    if (result != null && _level == reg.Level)
                        Objects.Add(type, result);
                }
            }
            return result;
        }


        private IEnumerable<object> ResolveParameters(ComponentRegistration registration, IEnumerable<ParameterInfo> parameters)
        {
            foreach (var parameter in parameters)
            {
                var defaultParam = registration.Parameters.FirstOrDefault(dp => dp.Name == parameter.Name);
                if (defaultParam != null)
                    yield return defaultParam.Value;
                else
                    yield return Resolve(parameter.ParameterType);
            }
        }


    }
}
