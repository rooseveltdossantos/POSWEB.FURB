using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteca.Dominio
{
    public enum TipoUsuario
    {
        Professor = 0,
        [Visualizacao(ApresentacaoParaFormulario = "Funcionário", ApresentacaoParaTabela = "Funcionário")]
        Funcionario = 1,
        Aluno = 2,

        [Visualizacao(ApresentacaoParaFormulario="Ex Aluno", ApresentacaoParaTabela="Ex-Aluno")]
        ExAluno = 3
    }
}
