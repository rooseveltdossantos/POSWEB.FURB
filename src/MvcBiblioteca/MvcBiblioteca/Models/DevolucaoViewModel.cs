using Biblioteca.Dominio;
using System.Collections.Generic;
using MvcBiblioteca.Negocio;

namespace MvcBiblioteca.Models
{
    public class DevolucaoViewModel
    {
        private Devolucao devolucao;

        public DevolucaoViewModel()
        {
            devolucao = new Devolucao();
        }

        public int idEmprestimo { get; set; }


        /// <summary>
        /// Obetem usuários com emprestimos em aberto
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Usuario> ObterUsuariosComEmprestimo()
        {
            return devolucao.ObterUsuariosComEmprestimo();
            #region ---- Código antigo ----
            //using (var bd = new BibliotecaDatabase())
            //{
            //    var query = (from p in bd.Emprestimos
            //                 where p.DevolvidoEm == null
            //                 select p.UsuarioEmprestimo).Distinct().ToList();
            //    return query.ToList();
            //}
            #endregion
        }



    }
}