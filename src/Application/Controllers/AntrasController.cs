using Resttp;

namespace Application.Controllers
{
    [ControllerRoute("Antras")]
    public class AntrasController : RestController
    {
        [ActionRoute("labas")]
        public int Labas()
        {
            return 1;
        }

    }
}
