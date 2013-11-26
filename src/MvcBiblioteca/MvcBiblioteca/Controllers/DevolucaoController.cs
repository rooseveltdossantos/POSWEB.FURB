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
        //
        // GET: /Devolucao/

        public ActionResult Index()
        {
            return View();
        }

        private static IEnumerable<Emprestimo> ObterEmprestimosEmAberto()
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = from p in bd.Emprestimos
                            where p.DevolvidoEm == null
                            select p;

                return query.ToList();
            }
        }

        


    }
}
