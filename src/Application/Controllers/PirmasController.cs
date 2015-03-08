using Resttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ControllerRoute("Pirmas")]
    public class PirmasController : RestController
    {

        public void Index()
        {

        }

        [Http("Get")]
        [ActionRoute("labas")]
        public void Labas()
        {

        }


    }
}
