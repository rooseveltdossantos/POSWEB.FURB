using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBiblioteca.Controllers
{
    public class HistoricoController : Controller
    {
        //
        // GET: /Historico/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListarHistoricoUsuario(int id)
        {
            using (var bd = new BibliotecaDatabase())
            {

                var query = (from e in bd.Emprestimos.Include(j => j.LivroEmprestimo)
                             where e.UsuarioEmprestimo.UsuarioId == id
                             select e.LivroEmprestimo).ToList();

                return Json(query.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListarHistoricoLivro()
        {
            return null;
        }

    }
}
