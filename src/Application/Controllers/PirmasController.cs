using Resttp;

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
