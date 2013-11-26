using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Biblioteca.Dominio
{
    public class ReservaLivro
    {
        public int ReservaLivroId { get; set; }

        [DisplayName("Livro")]
        public Livro LivroRelacionado { get; set; }

        [DisplayName("Usuário")]
        public Usuario UsuarioDeb { get; set; }

        [DisplayName("Situação")]
        public Boolean Situacao { get; set; }

        [DisplayName("Data da Reserva")]
        public  DateTime DtReserva { get; set; }

    }
}
