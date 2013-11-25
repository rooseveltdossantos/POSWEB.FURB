using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Biblioteca.Dominio
{
    public class ReservaLivro
    {
        public int ReservaId { get; set; }

        [DisplayName("Livro")]
        public Livro LivroRelacionado { get; set; }

        [DisplayName("Usuário")]
        public Usuario UsuarioDeb { get; set; }

        [DisplayName("Situação")]
        public Usuario Situacao { get; set; }

        [DisplayName("Data da Reserva")]
        public Usuario DtReserva { get; set; }

    }
}
