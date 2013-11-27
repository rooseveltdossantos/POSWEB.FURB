using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
               // result = (from r in bd.Reservas /*Como fazer join com Livro e Usuário? está retornando null*/
               //                where r.LivroRelacionado.LivroId == livroId                              
              //                 where r.Situacao == true
              //                 join l in bd.Livros on l.
             //                  /*orderby r.DtReserva tem que ordenar decrescente isso*/
             //                   select r).FirstOrDefault();
                /*result = reservas.ToList().FirstOrDefault();*/

                var livros = from l in bd.Livros select l;
                Debug.WriteLine("livros.ToList().Count:" + livros.ToList().Count);

                var usuarios = from u in bd.Usuarios select u;
                Debug.WriteLine("usuarios.ToList().Count:" + usuarios.ToList().Count);

                result = (from r in bd.Reservas
                                where r.LivroRelacionado.LivroId == livroId && r.Situacao == true
                          join l in bd.Livros on r.LivroRelacionado.LivroId equals l.LivroId
                          join u in bd.Usuarios on r.UsuarioDeb.UsuarioId equals u.UsuarioId
                                
                                select r).FirstOrDefault();

                if (result == null)
                {
                    result = new ReservaLivro();
                    var livro = bd.Livros.Find(livroId); /*Já engatinha na View o livro sendo reservado*/
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
                var usuario = bd.Usuarios.Find(1); /*Precisa pegar o usuário logado*/
                ReservaLivro reserva = new ReservaLivro();
                reserva.LivroRelacionado = livro;
                reserva.UsuarioDeb = usuario;
                reserva.Situacao = true;
                reserva.DtReserva = DateTime.Today;

                bd.Reservas.Add(reserva);
                bd.SaveChanges();
            }

            return RedirectToAction("Index", "Livros");
        }

        public ActionResult Procurar(string term)
        {
            using (var bd = new BibliotecaDatabase())
            {

                var livros = from livro in bd.Livros
                             select new
                             {
                                 id = livro.LivroId,
                                 label = livro.Titulo,
                                 value = livro.Titulo
                             };
                return Json(livros, JsonRequestBehavior.AllowGet);
            }
        }

        public void CancelaReserva(ReservaLivro livroReservado)
        {
            using (var bd = new BibliotecaDatabase())
            {
                bd.Reservas.Remove(livroReservado);
                bd.SaveChanges();
            }
        }

    }
}
