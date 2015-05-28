using Resttp;

namespace Application.Controllers
{
    [ControllerRoute("First")]
    public class PirmasController : RestController
    {
        //GET /First/Anon?nr=1111&txt=labas
        /// <summary>
        /// Returns a data in anonymous class. Works only with content-type: application/json.
        /// </summary>
        /// <param name="nr"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        [Http("Get")]
        [ActionRoute("Anon")]
        public object Index(int nr, string txt)
        {
            return new { nr = nr, txt = txt };
        }



    }
}
