using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp.ActionInvoker
{
    public class ActionDescriptor
    {
        public ActionDescriptor(string actionName)
        {
            ActionName = actionName;
            ActionArguments = new List<ActionArgumentDescriptor>();
        }


        public string ActionName { get; set; }

        public IList<ActionArgumentDescriptor> ActionArguments { get; set; }
    }
}
