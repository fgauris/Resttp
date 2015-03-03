using Microsoft.Owin.Hosting;
using Owin;
using Resttp;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Resttp")]
namespace Application
{
    //Vietoj sito galima sukonvertuoti i dll'a ir naudoti IIS.
    //Tam nebereikia sitos Program klases.
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
}
