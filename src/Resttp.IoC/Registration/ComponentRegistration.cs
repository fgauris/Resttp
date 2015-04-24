using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp.IoC.Registration
{
    public class ComponentRegistration
    {
        public Type LookupType { get; set; }

        public Type CreateType { get; set; }

        public string Mode { get; set; }//singleton, onrequest, new

        public IList<Parameter> Parameters { get; set; }

        public ComponentRegistration(Type createType)
        {
            CreateType = createType;
            Parameters = new List<Parameter>();
        }

        public ComponentRegistration(Type lookUpType, Type createType)
        {
            LookupType = lookUpType;
            CreateType = createType;
            Parameters = new List<Parameter>();
        }


    }
}
