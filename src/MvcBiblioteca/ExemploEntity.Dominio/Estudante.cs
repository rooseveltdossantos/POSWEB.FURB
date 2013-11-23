using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DominioEscolar
{
    public class Estudante
    {
        public Estudante()
        {

        }

        public int EstudanteId { get; set; }
        
        public string Nome { get; set; }

        public int GrauDeConhecimentoId { get; set; }

        public virtual GrauDeConhecimento GrauDeConhecimento { get; set; }

        
        public virtual EnderecoDoEstudante EnderecoDoEstudante { get; set; }

        public virtual ICollection<Estudante> Estudantes { get; set; }

    }

    public class EnderecoDoEstudante
    {        
        public int EstudanteId { get; set; }

        public string Endereco01 { get; set; }
        public string Endereco02 { get; set; }
        public string Cidade { get; set; }
        public int CEP { get; set; }
        public string UF { get; set; }
        public string Pais { get; set; }

        public virtual Estudante Estudante { get; set; }
    }
}

