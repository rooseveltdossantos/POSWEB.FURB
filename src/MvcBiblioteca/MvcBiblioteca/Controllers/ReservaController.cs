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
                

                var reservas = (from r in bd.Reservas.Include("LivroRelacionado").Include("UsuarioDeb")
                                //join l in bd.Livros.Include(a => a.) on r.LivroRelacionado.LivroId equals l.LivroId
                                //join u in bd.Usuarios on r.UsuarioDeb.UsuarioId equals u.UsuarioId
                                where r.LivroRelacionado.LivroId.Equals(livroId) && r.Situacao.Equals(true)
                                select r).FirstOrDefault();
                       

                result = reservas;


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

            return View("ReservaEfetuadaComSucesso");
        }

        public ActionResult ReservaEfetuadaComSucesso() 
        {
            return View();
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
