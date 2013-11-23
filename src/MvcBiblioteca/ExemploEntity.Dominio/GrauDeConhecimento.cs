using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominioEscolar
{
    public class GrauDeConhecimento
    {
        public GrauDeConhecimento()
        {
            ListaDeEstudantes = new List<Estudante>();
        }
        public int GrauDeConhecimentoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public virtual ICollection<Estudante> ListaDeEstudantes { get; set; }
    }

}

