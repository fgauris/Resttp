using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resttp
{
    using Microsoft.Owin;
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class ResttpComponent
    {
        private AppFunc _next;

        public ResttpConfiguration Config { get; set; }

        public ResttpComponent(AppFunc next, ResttpConfiguration config)
        {
            _next = next;
            Config = config;

            TestRoutes(new HttpRouteResolver(Config.HttpRoutes));



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

        private void TestRoutes(HttpRouteResolver resolver)
        {
            var s1 = resolver.Resolve(new PathString("/lt/Index"), "Post"); s1.ToString();
            var s3 = resolver.Resolve(new PathString("/lt"), "Get"); s3.ToString();
            var s4 = resolver.Resolve(new PathString("/lt"), "Post"); s4.ToString();
            var s5 = resolver.Resolve(new PathString("/lt"), "Put"); s5.ToString();
            //var s3 = resolver.Resolve(new PathString("/Antras/labas")); s3.ToString();



        }


    }
}
