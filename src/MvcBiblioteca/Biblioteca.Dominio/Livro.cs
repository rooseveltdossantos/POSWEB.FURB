using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Biblioteca.Dominio
{
    public class Livro
    {
        public int LivroId { get; set; }

        [DisplayName("Título")]
        public string Titulo { get; set; }

        public string Autor {get; set; }        

        public int Ano { get; set; }

        [DisplayName("Preço")]
        public decimal Preco { get; set; }

        public ICollection<ComentarioLivro> Comentarios { get; set; }
    }
}