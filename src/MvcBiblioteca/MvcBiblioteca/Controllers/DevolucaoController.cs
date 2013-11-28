using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using MvcBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MvcBiblioteca.Controllers
{
    public class DevolucaoController : Controller
    {

        public ActionResult Index()
        {
            return View(new DevolucaoViewModel());
        }

        public ActionResult Devolver(int idEmprestimo)
        {
            // TODO: Verificar se há necessidade de fazer método que poderá ser usado na listagem de emprestimo, passado usuário e id pode-se devolver em apenas 
            // clique, evitando assim a interface, tentar manter os dois modelos.
            throw new NotImplementedException();
        }

        public ActionResult Devolver(DevolucaoViewModel u)
        {

            // TODO: Procurar pelo emprestimo e se a data de entrega do livro for maior que a devolução gerar débito
            // e calcular a diferença de dias para fazer a gravação correta do débito.

            // TODO: Tratar também o evento do change dos select, trazer o primeiro emprestimo para que não seja necessário fazer o change.
            return View("Index", new UsuarioViewModel());
        }


        [ActionName("ListarLivrosDoUsuario")]
        public ActionResult ListarLivrosDoUsuario(int id)
        {

            using (var bd = new BibliotecaDatabase())
            {

                var query = (from e in bd.Emprestimos.Include(j => j.LivroEmprestimo)
                             where e.UsuarioEmprestimo.UsuarioId == id
                             select e.LivroEmprestimo).ToList();

                return Json(query.ToList(), JsonRequestBehavior.AllowGet);
            }
        }


        [ActionName("CarregarEmprestimo")]
        public ActionResult CarregarEmprestimo(int idUsuario, int idLivro)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from e in bd.Emprestimos.Include(j => j.LivroEmprestimo)
                             where e.LivroEmprestimo.LivroId == idLivro && e.UsuarioEmprestimo.UsuarioId == idUsuario
                             select e.EmprestimoId).FirstOrDefault();

                return Json(new { idEmprestimo = query }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
