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
    public class HistoricoController : Controller
    {
        //
        // GET: /Historico/

        public ActionResult Index()
        {
            return View();
        }
    
		
        public ActionResult ListarHistoricoUsuario(int idUsuario)
        {
            using (var bd = new BibliotecaDatabase())
            {

                var query = (from e in bd.Emprestimos.Include(j => j.LivroEmprestimo)
                             where e.UsuarioEmprestimo.UsuarioId == idUsuario
                             select e ).ToList();

                return Json(query.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
		
		//Novo metodos para retorna lista de Emprestimo por usuario
		 public IEnumerable<Emprestimo> ListEmprestimosDoUsuario(int usuarioId)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from e in bd.Emprestimos
                             where e.UsuarioEmprestimo.UsuarioId == usuarioId && !e.DevolvidoEm.HasValue
                             select e).Distinct().ToList();
                return query.ToList();
            }
        }
		
		//Novo metodos para retorna lista de Emprestimo por livro
		 public IEnumerable<Emprestimo> ListEmprestimosDoLivro(int livroId)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from e in bd.Emprestimos
                             where e.LivroEmprestimo.LivroId == livroId && !e.DevolvidoEm.HasValue
                             select e).Distinct().ToList();
                return query.ToList();
            }
        }
		
		public ActionResult ListarHistoricoLivro(int idLivro)
        {
              using (var bd = new BibliotecaDatabase())
            {
                var query = (from e in bd.Emprestimos.Include(j => j.LivroEmprestimo)
                             where e.LivroEmprestimo.LivroId == idLivro
                             select e ).ToList();

                return Json(query.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
		
		
		
		 public ActionResult ListarHistoricoLivroUsuario(int idUsuario, int idLivro)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from e in bd.Emprestimos.Include(j => j.LivroEmprestimo)
                             where e.LivroEmprestimo.LivroId == idLivro && e.UsuarioEmprestimo.UsuarioId == idUsuario
                             select e ).ToList();

                return Json(query.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
		

    }
}
