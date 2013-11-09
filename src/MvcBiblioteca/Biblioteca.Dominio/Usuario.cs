using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Biblioteca.Dominio
{
    public class Usuario
    {
        public long UsuarioId { get; set; }
        public long Cpf { get; set; }
        public string Nome { get; set; }       
        public StatusAtivacao StatusAtivacao { get; set; }
        public TipoUsuario TipoUsuario { get; set; }

    }
}
