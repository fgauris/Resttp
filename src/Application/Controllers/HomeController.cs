using Resttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Controllers
{
    public class HomeController: RestController
    {
        public void Index()
        {

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
}
