using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBiblioteca.Controllers
{
    public class EmprestimosController : Controller
    {
        //
        // GET: /Emprestimos/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = PapeisDaBiblioteca.PodeEmprestar)]
        public ActionResult Novo()
        {
            return View();
        }

    }
}
