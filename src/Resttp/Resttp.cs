using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resttp
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class ResttpComponent
    {
        private AppFunc _next;

        public ResttpComponent(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> enviroment)
        {
            //Do something here....
            await _next(enviroment);
            //Do something here too....

            //var response = enviroment["owin.ResponseBody"] as Stream;
            //using (var writer = new StreamWriter(response))
            //{
            //    return writer.WriteAsync("Hello!");
            //}
        }
    }
}
