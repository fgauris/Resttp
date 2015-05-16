using Resttp;

namespace Application.Controllers
{
    [ControllerRoute("Pirmas")]
    public class PirmasController : RestController
    {
        [Http("Get")]
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
