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
            var list = new List<SelectListItem>();

            foreach (var usuario in ObterUsuariosComEmprestimo())
                list.Add(new SelectListItem() { Text = usuario.Nome, Value = usuario.UsuarioId.ToString() });

            ViewBag.Usuarios = list;
            return View();
        }


        public IEnumerable<Usuario> ObterUsuariosComEmprestimo()
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from p in bd.Emprestimos
                             //where p.DevolvidoEm == null
                             select p.UsuarioEmprestimo).ToList();
                return query.ToList();
            }
        }


        public ActionResult Devolver()
        {
            return View();
        }

        [ActionName("ListarLivros")]
        public ActionResult ListarLivros()
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from p in ObterUsuariosComEmprestimo()
                             //where p.UsuarioEmprestimo.UsuarioId == id
                             select new { Id = p.UsuarioId, Value = p.Nome });


                return Json(query, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
