using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using MvcBiblioteca.Models;

namespace MvcBiblioteca.Controllers
{
    [Authorize(Roles = PapeisDaBiblioteca.PodeEmprestar)]
    public class EmprestimosController : Controller
    {
        //
        // GET: /Emprestimos/
        [Authorize]
        public ActionResult Index()
        {
            return View(new EmprestimoViewModel());

        }

        public ActionResult Novo()
        {
            return View(new EmprestimoViewModel());
        }


        private Emprestimo ObterEmprestimoDoLivro(int livroId)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from e in bd.Emprestimos
                             where e.LivroEmprestimo.LivroId == livroId && !e.DevolvidoEm.HasValue
                             select e).FirstOrDefault();
                return query;
            }
        }

        private IEnumerable<Emprestimo> ObterEmprestimosDoUsuario(int usuarioId)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from e in bd.Emprestimos
                             where e.UsuarioEmprestimo.UsuarioId == usuarioId && !e.DevolvidoEm.HasValue
                             select e).Distinct().ToList();
                return query.ToList();
            }
        }

        private bool PodeEmprestar(Usuario usuario, int quantidadeEmprestada)
        {
            TipoUsuario tipo = usuario.TipoUsuario;
            bool realizaEmprestimo = false;
            switch (tipo)
            {
                case TipoUsuario.Professor:
                    // 10 livros
                    realizaEmprestimo = quantidadeEmprestada <= 10 ? true : false;
                    break;
                case TipoUsuario.Aluno:
                    // 5 livros
                    realizaEmprestimo = quantidadeEmprestada <= 5 ? true : false;
                    break;
                case TipoUsuario.Funcionario:
                    // 3 livros
                    realizaEmprestimo = quantidadeEmprestada <= 3 ? true : false;
                    break;
                case TipoUsuario.ExAluno:
                    // 1 livro
                    realizaEmprestimo = quantidadeEmprestada <= 1 ? true : false;
                    break;
                default:
                    realizaEmprestimo = false;
                    break;
            }
            return realizaEmprestimo;
        }

        private DateTime GetPrazo(TipoUsuario tipo)
        {
            DateTime hoje = DateTime.Now;
            DateTime prazo = DateTime.Now;

            switch (tipo)
            {
                case TipoUsuario.Professor:                 
                    prazo = hoje.AddDays(10);
                    break;
                case TipoUsuario.Aluno:
                    prazo = hoje.AddDays(7);
                    break;
                case TipoUsuario.Funcionario:
                    prazo = hoje.AddDays(7);
                    break;
                case TipoUsuario.ExAluno:
                    prazo = hoje.AddDays(7);
                    break;
                default:
                    prazo = DateTime.Now;
                    break;
            }
            return prazo;
        }

        public ActionResult Emprestar(int livroId, int usuarioId)
        {
            DateTime hoje = DateTime.Now;
            string msg = ("");

            try
            {
                // Se ocorrer qualquer erro, exibe a mensagem ao usuário, na tela de empréstimos.
                using (var bd = new BibliotecaDatabase())
                {
                    var livro = bd.Livros.Find(livroId);
                    if (livro == null)
                    {
                        msg = ("Não foi possível encontrar o livro: " + livroId);
                        throw new Exception(msg);
                    }

                    // Verifica se o livro está emprestado                    
                    if (ObterEmprestimoDoLivro(livroId) != null)
                    {
                        msg = ("O livro " + livro.Titulo + ", com código " + livroId +  ", já está emprestado");
                        throw new Exception(msg);
                    }

                    // Verifica se o livro está reservado
                    //Márcio Koch: Ajustada a verificação da reserva do livro.
                    ReservaLivro reserva = ReservaController.getReserva(livroId);
                    if (reserva != null && reserva.ReservaLivroId > 0 && !reserva.UsuarioDeb.UsuarioId.Equals(usuarioId))
                    {
                        msg = ("O livro " + livro.Titulo + " está reservado para outro usuário.");
                        throw new Exception(msg);
                    }                    

                    // Verifica se o usuário existe
                    var usuario = bd.Usuarios.Find(usuarioId);
                    if (usuario == null)
                    {
                        msg = ("Não foi possível encontrar o usuário: " + usuarioId);
                        throw new Exception(msg);
                    }

                    // Verifica a quantidade de livros emprestados para o Usuário e o tipo do usuário.
                    IEnumerable<Emprestimo> emprestimosAtivos = ObterEmprestimosDoUsuario(usuarioId);
                    int quantidadeEmprestada = emprestimosAtivos.Count();

                    if (PodeEmprestar(usuario, quantidadeEmprestada))
                    {
                        DateTime prazo = GetPrazo(usuario.TipoUsuario);
                        Emprestimo emprestimo = new Emprestimo { LivroEmprestimo = livro, UsuarioEmprestimo = usuario, RetiradoEm = hoje, DevolverAte = prazo };
                        bd.Emprestimos.Add(emprestimo);
                        bd.SaveChanges();
                        msg = ("Livro emprestado com sucesso, com prazo até " + prazo + " " + msg);
                    }
                    else
                    {
                        // Exibe mensagem de erro ao usuário.
                        msg = ("O emprestimo não foi efetuado, foi excedido o número de livros emprestados ao usuário. " + msg);
                        throw new Exception(msg);
                    }
                }
            }
            catch
            {
                ViewBag.Mensagem = msg;
                return View("Index", new EmprestimoViewModel());
            }
            ViewBag.Mensagem = msg;
            return View("Index", new EmprestimoViewModel() );
        }
    }
}
