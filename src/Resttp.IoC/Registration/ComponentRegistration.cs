using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Resttp.IoC.Registration
{
    public class ComponentRegistration : IDisposable
    {
        public IList<Type> LookupTypes { get; set; }

        public Type CreateType { get; set; }

        public int Level { get; set; }//singleton = 1, onrequest = 2, new = 3

        public IList<Parameter> Parameters { get; set; }

        public Expression<Func<Object>> ResultFunc { get; set; }

        public ComponentRegistration()
        {
            LookupTypes = new List<Type>();
            Parameters = new List<Parameter>();
        }

        public ComponentRegistration(Type createType)
            : this()
        {
            CreateType = createType;
        }

        public ComponentRegistration(Type createType, Type lookUpType)
            : this()
        {
            CreateType = createType;
            LookupTypes.Add(lookUpType);
        }


        public void Dispose()
        {
            LookupTypes.Clear();
            Parameters.Clear();
        }

        //public object CreateObject(Type lookupType)
        //{
        //    if (LookupTypes.All(t => t.FullName != lookupType.FullName))//Check this 
        //        return null;

        //    if (ResultFunc != null)
        //    {
        //        return ResultFunc.Compile()();
        //    }
        //    else
        //    {

        //    }
        //}

        //protected object CreateFromMetaData(Type lookupType)
        //{
        //    ConstructorInfo[] contructors = CreateType.GetConstructors().OrderByDescending(c => c.GetParameters().Count()).ToArray();
        //    foreach (var constructor in contructors)
        //    {
        //        var parameters = constructor.GetParameters();
        //        if(!parameters.Any())
        //        {
                    
        //        }
        //    }
        //}

    }
}
