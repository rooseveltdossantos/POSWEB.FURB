using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBiblioteca.Models
{
    public class DevolucaoViewModel
    {

        private Emprestimo emprestimo;

        public DevolucaoViewModel()
        {
            this.emprestimo = new Emprestimo();
        }

        public int idEmprestimo { get; set; }  

        public IEnumerable<Usuario> ObterUsuariosComEmprestimo()
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from p in bd.Emprestimos
                             //where p.DevolvidoEm == null
                             select p.UsuarioEmprestimo).Distinct().ToList();
                return query.ToList();
            }
        }
    }
}