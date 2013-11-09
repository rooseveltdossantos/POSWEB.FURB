using System.Web.Mvc;

namespace ExemploXSS.Controllers
{
    //FUNCIONALIDADE DE CROSS-SITE SCRIPTING (XSRF)
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
