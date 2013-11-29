using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biblioteca.Dominio;
using Biblioteca.DataAccess;

namespace MvcBiblioteca.Models
{
    public class EmprestimoViewModel
    {
        private Emprestimo emprestimo;
        public int EmprestimoId { get; set; }

        public EmprestimoViewModel() 
        {
            this.emprestimo = new Emprestimo();
        }

        public IEnumerable<Emprestimo> ObterEmprestimosAtivosDoUsuario(int usuarioId)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from e in bd.Emprestimos
                             where e.UsuarioEmprestimo.UsuarioId == usuarioId && !e.DevolvidoEm.HasValue
                             select e).Distinct().ToList();
                return query.ToList();
            }
        }

        public IEnumerable<Emprestimo> ObterEmprestimosAtivos()
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from e in bd.Emprestimos
                             where !e.DevolvidoEm.HasValue
                             select e).Distinct().ToList();
                return query.ToList();
            }
        }

        public IEnumerable<Usuario> ObterUsuarios()
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from u in bd.Usuarios
                             select u).Distinct().ToList();
                return query.ToList();
            }
        }

        public IEnumerable<Livro> ObterLivros()
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from l in bd.Livros
                             select l).Distinct().ToList();
                return query.ToList();
            }
        }

        public IEnumerable<Livro> ObterLivrosEmprestados()
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from e in bd.Emprestimos.Include("LivroEmprestimo")
                             where !e.DevolvidoEm.HasValue
                             select e.LivroEmprestimo).Distinct().ToList();
                return query.ToList();
            }
        }

        // Retorna apenas os livros não emprestados
        public IEnumerable<Livro> ObterLivrosNaoEmprestados()
        {
            return ObterLivros().Except(ObterLivrosEmprestados());
        }
    }
}