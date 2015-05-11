using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class ActionInvokerComponent
    {
        private AppFunc _next;

        public ActionInvokerComponent(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var controller = environment["resttp.Controller"] as RestController;
            if (controller == null)
                throw new Exception("Controller not found in OWIN environment dictionary");

            object result = null;
            try
            {
                result = InvokeAction(controller);
                
            }
            catch(Exception e)
            {
                result = e;
            }

            await ReturnActionResultAsync(environment, result);
        }

        private object InvokeAction(RestController controller)
        {


            throw new NotImplementedException();
        }

        private async Task ReturnActionResultAsync(IDictionary<string, object> environment, object result)
        {

            throw new NotImplementedException();
        }
    }
}
