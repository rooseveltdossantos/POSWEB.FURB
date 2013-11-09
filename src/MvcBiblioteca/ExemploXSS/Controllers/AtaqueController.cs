using System.Web.Mvc;

namespace ExemploXSS.Controllers
{
    public class AtaqueController : Controller
    {

        public ActionResult Registrar(string entrada)
        {
            return View();
        }

    }
}
