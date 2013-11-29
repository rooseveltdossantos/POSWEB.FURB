﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using MvcBiblioteca.Models;

namespace MvcBiblioteca.Controllers
{
    public class EmprestimosController : Controller
    {
        //
        // GET: /Emprestimos/

        // TODO: Listar os empréstimos ativos
        public ActionResult Index()
        {
            return View();

        }

        [Authorize(Roles = PapeisDaBiblioteca.PodeEmprestar)]
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

        public ActionResult Emprestar(int livroId, int usuarioId)
        {
            DateTime hoje = DateTime.Now;
            DateTime prazoProfessor = hoje.AddDays(10);
            DateTime prazoAluno = hoje.AddDays(7);
            DateTime prazoFuncionario = hoje.AddDays(7);
            DateTime prazoExAluno = hoje.AddDays(7);

            string msg = ("");

            try
            {
                using (var bd = new BibliotecaDatabase()) {

                    var livro = bd.Livros.Find(livroId);
                    if (livro == null)
                    {
                        msg = ("Não foi possível encontrar o livro: " + livroId);
                        throw new Exception(msg);
                    }

                    // Verifica se o livro está emprestado ou reservado
                    if (ObterEmprestimoDoLivro(livroId) != null)
                    {
                        msg = ("O livro já está emprestado: " + livroId);
                        throw new Exception(msg);
                    }

                    var usuario = bd.Usuarios.Find(usuarioId);
                    if (usuario == null)
                    {
                        msg = ("Não foi possível encontrar o usuário:" + usuarioId);
                        throw new Exception(msg);
                    }

                    // Verifica a quantidade de livros emprestados para o Usuário e o tipo do usuário.
                    IEnumerable<Emprestimo> emprestimosAtivos = ObterEmprestimosDoUsuario(usuarioId);
                    int quantidadeEmprestada = emprestimosAtivos.Count();

                    TipoUsuario tipo = usuario.TipoUsuario;

                    bool realizaEmprestimo = false;
                    DateTime prazo = DateTime.Now;

                    switch (tipo)
                    {
                        case TipoUsuario.Professor:
                            // 10 livros
                            realizaEmprestimo = quantidadeEmprestada <= 10 ? true : false;
                            prazo = prazoProfessor;
                            break;
                        case TipoUsuario.Aluno:
                            // 5 livros
                            realizaEmprestimo = quantidadeEmprestada <= 5 ? true : false;
                            prazo = prazoAluno;
                            break;
                        case TipoUsuario.Funcionario:
                            // 3 livros
                            realizaEmprestimo = quantidadeEmprestada <= 3 ? true : false;
                            prazo = prazoFuncionario;
                            break;
                        case TipoUsuario.ExAluno:
                            // 1 livro
                            realizaEmprestimo = quantidadeEmprestada <= 1 ? true : false;
                            prazo = prazoExAluno;
                            break;
                        default:
                            realizaEmprestimo = false;
                            prazo = DateTime.Now;
                            Console.WriteLine("Nenhum tipo de usuário foi específicado");
                            break;
                    }

                    if (realizaEmprestimo)
                    {
                        Emprestimo emprestimo = new Emprestimo { LivroEmprestimo = livro, UsuarioEmprestimo = usuario, RetiradoEm = hoje, DevolverAte = prazo };
                        bd.Emprestimos.Add(emprestimo);
                        bd.SaveChanges();
                        msg = ("Livro emprestado com sucesso " + msg);
                        Console.WriteLine(msg);
                    }
                    else
                    {
                        msg = ("O emprestimo não foi efetuado. " + msg);
                        Console.WriteLine(msg);
                        //return View("Erro");
                        // Exibe mensagem de erro.
                    }
                }

            }
            catch {
                ViewBag.Mensagem = msg;
                return View("Index");
            }
            ViewBag.Mensagem = msg;
            return View("Index");
            
        }

    }
}
