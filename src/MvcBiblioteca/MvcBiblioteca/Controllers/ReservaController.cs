using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBiblioteca.Controllers
{
    public class ReservaController : Controller
    {
        //
        // GET: /Reserva/

        private ReservaLivro getReserva(int livroId) 
        {
            ReservaLivro result = null;

            using (var bd = new BibliotecaDatabase())
            {
                var reservas = (from r in bd.Reservas
                               where r.LivroRelacionado.LivroId == livroId
                               where r.UsuarioDeb.UsuarioId == 1
                               where r.Situacao == true
                               /*orderby r.DtReserva*/
                                select r).FirstOrDefault();
                //result = reservas.ToList().FirstOrDefault();

                if (result == null)
                {
                    result = new ReservaLivro();
                    var livro = bd.Livros.Find(livroId);
                    result.LivroRelacionado = livro;
                }
            }

            
           
            return result;
        }

        public ActionResult Index(int livroId)
        {
            return View(getReserva(livroId));
        }

        public ActionResult Reservar(int livroId)
        {

            using (var bd = new BibliotecaDatabase())
            {
                var livro = bd.Livros.Find(livroId);
                var usuario = bd.Usuarios.Find(1);
                ReservaLivro reserva = new ReservaLivro();
                reserva.LivroRelacionado = livro;
                reserva.UsuarioDeb = usuario;
                reserva.Situacao = true;
                reserva.DtReserva = DateTime.Today;

                bd.Reservas.Add(reserva);
                bd.SaveChanges();
            }
            
            return View("Livros/Index");
        }

    }
}
