using Resttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ControllerRoute("Antras")]
    public class AntrasController : RestController
    {
        [ActionRoute("labas")]
        public void Labas()
        {

        }

        [ActionRoute("options/{id}")]
        public void Options()
        {

        }

    }
}
