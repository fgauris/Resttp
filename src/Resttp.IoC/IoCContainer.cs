using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resttp.Dependencies;
using Resttp.IoC.Registration;
using System.Reflection;

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

        public IoCContainer CreateChildContainer(IDictionary<Type, Object> objects)
        {
            return new IoCContainer(this, ComponentRegistrations)
            {
                Objects = objects.ToDictionary(e => e.Key, e => e.Value)
            };
        }

        public IDependencyResolver StartScope()
        {
            return CreateChildContainer(Objects);
        }

        public void Dispose()
        {
            Objects.Clear();
            ComponentRegistrations.Clear();
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
                throw new Exception("Type not found in IoC container. Type: " + type.FullName);//TODO Change to return null;

            if (reg.Level < _level)//Get from parent
            {
                if (ParentContainer == null)
                    throw new Exception(string.Format("Bad level. Current: {0}. Required: {1}.", _level, reg.Level));//TODO Change to return null;
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
                    {
                        result = reg.ResultFunc();
                    }
                    else
                    {
                        var constructors = type.GetConstructors()
                            .Where(c => c.GetParameters().Count() > 0)
                            .OrderByDescending(c => c.GetParameters().Count());

                        if (!constructors.Any())
                        {
                            result = Activator.CreateInstance(reg.CreateType);
                        }
                        foreach (var constructor in constructors)
                        {
                            var parameters = constructor.GetParameters();
                            result = constructor.Invoke(ResolveParameters(reg, parameters).ToArray());
                        }
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
                {
                    var newParam = Resolve(parameter.ParameterType);
                    if (newParam == null)
                        throw new Exception(string.Format("Parameter not found. Name: {0}, Type: {1}", parameter.Name, parameter.ParameterType.FullName));
                    else
                        yield return newParam;
                }
            }
        }


    }
}
