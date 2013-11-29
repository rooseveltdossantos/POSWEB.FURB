using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Biblioteca.Dominio
{
    public class Debito
    {
        public long DebitoId { get; set; }

        [DisplayName("Usuário")]
        public Usuario UsuarioDeb { get; set; }

        public Emprestimo Emprestimo { get; set; }

        [DisplayName("Dias Atraso")]
        public int DiasAtraso { get; set; }

        public bool DebitoAtivo { get; set; }

    }
}
