using System.Linq;
using System.Web.Mvc;
using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using System.Data.Entity;

namespace MvcBiblioteca.Controllers
{
    [Authorize(Roles = PapeisDaBiblioteca.PodeComentarEAdicionar)]
    public class ComentariosController : Controller
    {
        //
        // GET: /Comentarios/

        [AllowAnonymous]
        public ActionResult Index(long livroId)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var livro = (from l in bd.Livros.Include(c => c.Comentarios)
                            where l.LivroId == livroId
                            select l).SingleOrDefault();

                return View(livro);
            }
            
        }


        [HttpPost]
        public ActionResult AdicionarComentario(long livroId, string comentario)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var livro = (from l in bd.Livros.Include(c => c.Comentarios)
                             where l.LivroId == livroId
                             select l).SingleOrDefault();

                livro.Comentarios.Add(new ComentarioLivro { Comentario = comentario });
                bd.SaveChanges();
               
                if (Request.IsAjaxRequest())
                {
                    ViewBag.Comentario = comentario;
                    return PartialView();
                }
                return RedirectToAction("Index");
            }

        }
    }
}
