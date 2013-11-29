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
    public class ErroReserva 
    {

        private String FMensagem = "Erro desconhecido.";
        public String mensagem{
            get
            {
                return FMensagem;
             }
            
            set{
                FMensagem = value;    
            }
        }
    }

    [Authorize]
    public class ReservaController : Controller
    {
        //
        // GET: /Reserva/

        

        public static ReservaLivro getReserva(int livroId) 
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
                    result.ReservaLivroId = 0;
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

        public static void removerReserva(int livroId)
        {
            ReservaLivro reserva = ReservaController.getReserva(livroId);
            if (reserva != null && reserva.ReservaLivroId > 0)
            {
                reserva.Situacao = false;//Libera a reserva
            }
            
        }
        
        
        public ActionResult Reservar(int livroId)
        {

            using (var bd = new BibliotecaDatabase())
            {
                var livro = bd.Livros.Find(livroId);

                //Busca o usuário logado na base
                var usuario = (from u in bd.Usuarios
                              where u.Login.Equals(User.Identity.Name) 
                              select u).FirstOrDefault();
                
                if(usuario == null)
                {
                    ErroReserva erro = new ErroReserva();
                    erro.mensagem = "Não foi possível encontrar o usuário:" + User.Identity.Name + ". É possível que a base de usuários e o controle de login estejam desincronizados.";
                    return View("Erro", erro);                    
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

        public ActionResult Erro(ErroReserva erro) 
        {
            return View(erro);
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
