using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Biblioteca.Dominio;

namespace Biblioteca.DataAccess
{
    public class BibliotecaDatabase : DbContext
    {

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ReservaLivro> Reservas { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
    }
}