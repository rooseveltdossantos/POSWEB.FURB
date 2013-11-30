using Biblioteca.Dominio;
using MvcBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcBiblioteca.Negocio;

namespace MvcBiblioteca.Controllers
{
    public class DevolucaoController : Controller
    {
        private Devolucao devolucao;

        public DevolucaoController()
        {
            devolucao = new Devolucao();
        }

        public ActionResult Index()
        {
            return View(ObterLivrosDevolvidos());
        }

        public ActionResult Novo()
        {
            return View(new DevolucaoViewModel());
        }

        public ActionResult Devolver(int idEmprestimo)
        {
            devolucao.Devolver(idEmprestimo);
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Devolver(DevolucaoViewModel u)
        {
            devolucao.Devolver(u.idEmprestimo);
            return View("Index", ObterLivrosDevolvidos());
        }

        public IEnumerable<Emprestimo> ObterLivrosDevolvidos()
        {
            return devolucao.ObterLivrosDevolvidos();
        }

        public ActionResult ListarLivrosDoUsuario(int idUsuario)
        {
            return Json(devolucao.ListarLivrosDoUsuario(idUsuario), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CarregarEmprestimo(int idUsuario, int idLivro)
        {
            return Json(new { idEmprestimo = devolucao.CarregarEmprestimo(idUsuario, idLivro) }, JsonRequestBehavior.AllowGet);

        }

    }
}
