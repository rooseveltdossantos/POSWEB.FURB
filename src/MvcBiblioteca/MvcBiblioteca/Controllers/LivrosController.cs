using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBiblioteca.Models;
using System.IO;
using Biblioteca.Dominio;
using System.Diagnostics;
using Biblioteca.DataAccess;


namespace MvcBiblioteca.Controllers
{
    public class LivrosController : Controller
    {
        
        //
        // GET: /Livros/

        public ActionResult Index()
        {
           return View("Index", ObterLivros(string.Empty));
        }

        
        [Authorize(Roles=PapeisDaBiblioteca.PodeAdicionarLivro)]
        public ActionResult Novo()
        {            
            return View();
        }
        
        [HttpPost]
        public ActionResult Novo(Livro livro)
        {
            AdicionaLivro(livro);
            return RedirectToAction("index");
        }

        public ActionResult CategorizadosPorAutor()
        {
            using (var bd = new BibliotecaDatabase())
            {
                var cat = new Categorizador(bd);
                return View(cat.LivrosPorAutor());
            }            
        }

        public ActionResult DescricaoLivro(long livroId)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var livro = bd.Livros.Find(livroId);
                if (Request.IsAjaxRequest())
                    return PartialView(livro);
                else
                    return View(livro);
            }
        }

        private void AdicionaLivro(Livro livro)
        {
            using (var ld = new BibliotecaDatabase())
            {
                Debug.WriteLine(ld.Database.Connection.ConnectionString);
                ld.Livros.Add(livro);
                ld.SaveChanges();
            }
        }

        public ActionResult Localizar(string q)
        {            
            var livros = ObterLivros(q);
            return View("Index", livros);
        }

        private static IEnumerable<Livro> ObterLivros(string q)
        {
            q = q.ToLower();
            
            using (var bd = new BibliotecaDatabase())
            {
                var livros = from livro in bd.Livros
                             select livro;
                
                if (!string.IsNullOrEmpty(q))
                    livros = livros = livros.Where(l => l.Titulo.ToLower().IndexOf(q) >= 0);

                return livros.ToList();
            }
        }

        public ActionResult Procurar(string term)
        {
            var livros = from livro in ObterLivros(term)
                         select new
                         {
                             id = livro.LivroId,
                             label = livro.Titulo,
                             value = livro.Titulo
                         };
            return Json(livros, JsonRequestBehavior.AllowGet);
        }
    }
}


/*
 * 
 * <form method="post" action="">
        <fieldset>
            Título: <input type="text" name="Nome" /><br />
            Autor: <input type="text" name="Autor" /><br />
            Ano: <input type="text" name="Ano" /><br />
            Preço: <input type="text" name="Preco" /><br />
            <input type="submit" value="Inserir" />
        </fieldset>
    </form>
 * 
 * */
