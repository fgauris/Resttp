using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp.ActionInvoker
{
    public class ActionArgumentDescriptor
    {
        public string ParamName { get; set; }
        public Type ParamType { get; set; }

        public object ParamValue { get; set; }

        public ActionArgumentDescriptor(string paramName, Type paramType, object paramValue)
        {
            ParamName = paramName;
            ParamType = paramType;
            ParamValue = paramValue;
        }
    }
}
