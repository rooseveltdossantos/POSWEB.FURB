using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using MvcBiblioteca.Models;

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

        public ActionResult LocalizarDebitosUsuario(string NomeUsuario)
        {
            var debitos = ObterDebitos(NomeUsuario);
            return View("Index", debitos);
        }

        private static IEnumerable<Debito> ObterDebitos(string NomeUsuario)
        {
            NomeUsuario = NomeUsuario.ToLower();

            using (var bd = new BibliotecaDatabase())
            {
                IQueryable<Debito> debitos = from debito in bd.Debitos
                                             where debito.DebitoAtivo
                                             orderby debito.Emprestimo.horarioTermino descending
                                             select debito;

                if (!string.IsNullOrEmpty(NomeUsuario))
                    debitos = debitos.Where(deb => deb.UsuarioDeb.Nome.ToLower().IndexOf(NomeUsuario) >= 0);

                return debitos.ToList();
            }
        }

    }
}
