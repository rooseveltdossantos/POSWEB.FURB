using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biblioteca.Dominio;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcBiblioteca.Models
{
    public class UsuarioViewModel
    {
        private Usuario usuario;

        public UsuarioViewModel()
        {
            this.usuario = new Usuario();
        }

        public UsuarioViewModel(Usuario usuario)
        {
            this.usuario = usuario;
        }
               
        public long UsuarioId
        {
            get { return this.usuario.UsuarioId; }
            set { this.usuario.UsuarioId = value; }
        }

        [DisplayName("CPF")]
        [Required(ErrorMessage = "O CPF deve ser informado")]
        [Remote("CPFValido", "Usuarios", ErrorMessage="Dígito verificador do CPF inválido")]
        public long Cpf
        {
            get { return this.usuario.Cpf; }
            set { this.usuario.Cpf = value; }
        }

        public string Nome
        {
            get { return this.usuario.Nome; }
            set { this.usuario.Nome = value; }
        }


        public string Login
        {
            get { return this.usuario.Login; }
            set { this.usuario.Login = value; }
        }

        public string Senha
        {
            get { return this.usuario.Senha; }
            set { this.usuario.Senha = value; }
        }

        //[Visualizacao(ApresentacaoParaFormulario = "Situação da Ativação", ApresentacaoParaTabela = "Sit. Ativação")]
        [DisplayName("Usuário Ativo?")]
        public bool Status
        {
            get
            {
                return this.usuario.StatusAtivacao == Biblioteca.Dominio.StatusAtivacao.Ativo;
            }
            set
            {
                this.usuario.StatusAtivacao = value
                    ? Biblioteca.Dominio.StatusAtivacao.Ativo
                    : Biblioteca.Dominio.StatusAtivacao.Inativo;
            }
        }

        [DisplayName("Tipo do Usuário")]
        public TipoUsuario TipoUsuario
        {
            get { return this.usuario.TipoUsuario; }
            set { this.usuario.TipoUsuario = value; }
        }

        [Display(Name = "Tipoo")]
        public TipoUsuario TipoUsuarioId { get; set; }
        public TipoUsuario TipoUsuarioTypeList { get; set; }
       

        internal Usuario ParaEntidade()
        {
            return this.usuario;
        }
    }
}