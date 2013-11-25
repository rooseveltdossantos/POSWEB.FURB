using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Biblioteca.Dominio
{
    public class Debito
    {
        public int DebitoId { get; set; }

        [DisplayName("Usuário")]
        public Usuario UsuarioDeb { get; set; }

        [DisplayName("Livro")]
        public Livro LivroRelacionado { get; set; }

        [DisplayName("Data Débito")]
        public DateTime DataDebito { get; set; }

        [DisplayName("Dias Atraso")]
        public int DiasAtraso { get; set; }

        public bool DebitoAtivo { get; set; }

    }
}
