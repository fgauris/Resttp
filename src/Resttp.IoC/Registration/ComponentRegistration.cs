using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Resttp.IoC.Registration
{
    public class ComponentRegistration: IDisposable
        
    {
        public List<Type> LookupTypes { get; set; }

        public Type CreateType { get; set; }

        public string Mode { get; set; }//singleton, onrequest, new

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
    }
}
