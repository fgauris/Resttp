using System;
using Resttp;

namespace Application.Controllers
{
    public class HomeController: RestController
    {

        // GET /Main?sk=111&txt=labaslabas
        /// <summary>
        /// Returns parameters values in dummy class
        /// </summary>
        /// <param name="sk"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        public Dummy Index(int sk, string txt)
        {
            return new Dummy(txt, sk);
        }

        //POST Home/Pagrindinis
        public int Pagrindinis()
        {
            return 1;
        }

        // GET /Home/Get?nr=11111
        public string Get(long nr)
        {
            return "GET success with nr " + nr + ".";
        }

        // POST /Home/Post
        public string Post()
        {
            return "POST success";
        }
        
        // PUT /Home/Put
        [Http("Put")]
        public string PutPut()
        {
            return "PUT success";
        }
    }

    public class Dummy
    {
        public string Labas { get; set; }
        public int Sk { get; set; }

        public Dummy(string labas, int? sk)
        {
            Labas = labas ?? "Laba!!";
            Sk = sk ?? 5;
        }
    }
}
