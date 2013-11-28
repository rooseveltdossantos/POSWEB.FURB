using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace MvcBiblioteca.Controllers
{
    [Authorize]
    public class ReservaController : Controller
    {
        //
        // GET: /Reserva/

        private ReservaLivro getReserva(int livroId) 
        {
            ReservaLivro result = null;

            using (var bd = new BibliotecaDatabase())
            {


                result = (from r in bd.Reservas.Include("LivroRelacionado").Include("UsuarioDeb")                                
                                where r.LivroRelacionado.LivroId.Equals(livroId) && r.Situacao.Equals(true)
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

                //Pega o id do usuário logado
                int userId = WebSecurity.GetUserId(User.Identity.Name);

                var usuario = bd.Usuarios.Find(userId);
                if(usuario == null)
                {
                    throw new Exception("Não foi possível encontrar o usuário:" + userId + "-" + User.Identity.Name);
                }

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
