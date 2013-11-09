using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteca.DataAccess
{
    public class PorAutor
    {
        public string Autor { get; set; }
        public int Total { get; set; }
    }

    public class Categorizador
    {

        private readonly BibliotecaDatabase bd;

        public Categorizador(BibliotecaDatabase bd)
        {
            this.bd = bd;
        }

        public IEnumerable<PorAutor> LivrosPorAutor()
        {
            var livros = from livro in bd.Livros    
                         group livro by livro.Autor
                         into agrupador
                         orderby agrupador.Count() descending
                         select new PorAutor
                             {
                             Autor = agrupador.Key,
                             Total = agrupador.Count()
                         };
            return livros.ToList();
        }
    }
}
