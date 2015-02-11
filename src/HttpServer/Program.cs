using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string uri = "http://localhost:61111";

            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine("Resttp started.");
                Console.ReadKey();
                Console.WriteLine("Resttp ended.");
            }
        }


    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Run(ctx => {
                return ctx.Response.WriteAsync("Labas");
            });



        }
    }
}
