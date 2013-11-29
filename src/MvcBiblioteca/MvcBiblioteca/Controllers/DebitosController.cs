using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using MvcBiblioteca.Models;
using System.Diagnostics;
using System.IO;
using System.Data.Entity;


namespace MvcBiblioteca.Controllers
{
    public class DebitosController : Controller
    {
        /// <summary>
        /// Método que lista todos os débitos ativos
        /// </summary>
        public ActionResult Index()
        {
            return View("Index", ObterDebitos(string.Empty));
        }

        /// <summary>
        /// Método que lista todos os débitos ativos de determinado usuário
        /// </summary>
        public ActionResult LocalizarDebitosUsuario(string usuario)
        {
            var debitos = ObterDebitos(usuario);
            return View("Index", debitos);
        }

        /// <summary>
        /// Método responsável por acionar a tela de efetuar o pagamento do débito
        /// </summary>
        public ActionResult EfetuarPgto(long debitoId)
        {
            var debito = RetornaInfoDebito(debitoId);
            return View(debito);
        }

        /// <summary>
        /// Método para efetuar o pagamento do débito do usuário
        /// </summary>
        [HttpPost]
        public ActionResult EfetuarPgto(Debito PgtoDebito)
        {
            if (ModelState.IsValid)
            {
                using (var bd = new BibliotecaDatabase())
                {
                    if (PgtoDebito.DebitoId <= 0)
                    {
                        throw new Exception("Não é possível efetuar o pagamento deste empréstimo. Id desconhecido.");
                    }
                    else
                    {
                        var pgtoAux = RetornaInfoDebito(PgtoDebito.DebitoId);
                        PgtoDebito = pgtoAux;
                        PgtoDebito.DebitoAtivo = false;
                        bd.Entry(PgtoDebito).State = EntityState.Modified;
                    }

                    bd.SaveChanges();

                    return RedirectToAction("index");
                }
            }
            return RedirectToAction("index");
        }

        /// <summary>
        /// Método que retorna a lista de acordo 
        /// </summary>
        private static IEnumerable<Debito> ObterDebitos(string NomeUsuario)
        {
            NomeUsuario = NomeUsuario.ToLower();

            using (var bd = new BibliotecaDatabase())
            {
                IQueryable<Debito> debitos = from debito in bd.Debitos.Include("Emprestimo").Include("Emprestimo.LivroEmprestimo").Include("UsuarioDeb")
                                             where debito.DebitoAtivo
                                             orderby debito.Emprestimo.DevolvidoEm descending
                                             select debito;

                if (!string.IsNullOrEmpty(NomeUsuario))
                    debitos = debitos.Where(deb => deb.UsuarioDeb.Nome.ToLower().IndexOf(NomeUsuario) >= 0);

                return debitos.ToList();
            }
        }

        /// <summary>
        /// Método usado para preencher dinamicamente o textbox de pesquisa
        /// </summary>
        public ActionResult Procurar(string term)
        {
            var debitos = from debito in ObterDebitos(term)
                         select new
                         {
                             id = debito.DebitoId,
                             label = debito.UsuarioDeb.Nome,
                             value = debito.UsuarioDeb.Nome
                         };
            return Json(debitos, JsonRequestBehavior.AllowGet);
        }

        public Debito RetornaInfoDebito(long IdDebito){

            using (var bd = new BibliotecaDatabase())
            {
                var deb = from debito in bd.Debitos.Include("Emprestimo").Include("Emprestimo.LivroEmprestimo").Include("UsuarioDeb")
                          where debito.DebitoId == IdDebito
                          select debito;
                return deb.FirstOrDefault();
            }
        }

    }
}