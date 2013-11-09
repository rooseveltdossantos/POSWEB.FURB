using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteca.Dominio
{
    public class VisualizacaoAttribute : Attribute
    {
        public string ApresentacaoParaFormulario { get; set; }
        public string ApresentacaoParaTabela { get; set; }
    }
}
