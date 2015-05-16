using System;
using Resttp;

namespace Application.Controllers
{
    public class HomeController: RestController
    {
        public DUmmy Index(int sk)
        {
            return new DUmmy("Labas!!! Testas is kontrolerio", 111);
        }

        public int Pagrindinis()
        {
            throw new NotImplementedException();
        }

        public int Get()
        {
            throw new NotImplementedException();
        }

        
        public int Post()
        {
            throw new NotImplementedException();
        }
        
        [Http("Put")]
        public int PutPut()
        {
            throw new NotImplementedException();
        }
    }

    public class DUmmy
    {
        public string Labas { get; set; }
        public int Sk { get; set; }

        public DUmmy(string labas, int? sk)
        {
            Labas = labas ?? "Laba!!";
            Sk = sk ?? 5;
        }
    }
}
