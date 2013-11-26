using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBiblioteca.Controllers
{
    public class DevolucaoController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        private static IEnumerable<Usuario> ObterUsuariosComEmprestimo()
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from p in bd.Emprestimos
                             where p.DevolvidoEm == null
                             select p.UsuarioEmprestimo).Distinct().ToList();

                return query.ToList();
            }
        }


        public ActionResult ListarLivros(int id)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = from p in bd.Emprestimos
                            where p.UsuarioEmprestimo.UsuarioId == id
                            select new
                            {
                                id = p.LivroEmprestimo.LivroId,
                                value = p.LivroEmprestimo.Titulo
                            };

                return Json(query, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
