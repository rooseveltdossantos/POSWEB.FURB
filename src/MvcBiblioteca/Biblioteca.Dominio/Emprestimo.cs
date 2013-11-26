using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Biblioteca.Dominio
{
    public class Emprestimo
    {
        public int EmprestimoId { get; set; }

        [DisplayName("Livro")]
        public Livro LivroEmprestimo { get; set; }

        [DisplayName("Usuário")]
        public Usuario UsuarioEmprestimo { get; set; }

        [DisplayName("Horário de Início")]
        public DateTime horarioInicio { get; set; }

        [DisplayName("Horário de Término")]
        public DateTime horarioTermino { get; set; }
    }
}
