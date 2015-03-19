using Resttp;

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
