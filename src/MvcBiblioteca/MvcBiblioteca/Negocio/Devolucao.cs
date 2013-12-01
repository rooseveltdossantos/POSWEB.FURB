using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using MvcBiblioteca.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace MvcBiblioteca.Negocio
{
    public class Devolucao
    {
        #region -------- Código para Popular --------
        //public ActionResult Popular()
        //{
        //    var meuUser = new Usuario { Login = "lah", Nome = "Luiz Angelo Heinzen", Senha = "segredo", StatusAtivacao = StatusAtivacao.Ativo, TipoUsuario = TipoUsuario.Funcionario };
        //    var proto = new Usuario { Login = "proto", Nome = "Proto(u)suario", Senha = "segredo", StatusAtivacao = StatusAtivacao.Ativo, TipoUsuario = TipoUsuario.Aluno };
        //    var tioBill = new Usuario { Login = "bgates", Nome = "Bill Gates", Senha = "segredo", StatusAtivacao = StatusAtivacao.Ativo, TipoUsuario = TipoUsuario.Professor};
        //    var guerraDosTronos = new Livro { Ano = 2000, Autor = "George R.R. Martin", Preco = 40m, Titulo = "A Guerra dos Tronos" };
        //    var carrie = new Livro { Ano = 1979, Autor = "Stepen King", Preco = 15m, Titulo = "Carrie, a estranha" };
        //    var arteGuerra = new Livro { Ano = 1600, Autor = "Sun-Tzu", Preco = 5m, Titulo = "A arte da Guerra" };
        //    var jogosVorazes = new Livro { Ano = 2003, Autor = "Stephanie Meyer", Preco = 25m, Titulo = "Jogos Vorazes" };
        //    var demian = new Livro { Ano = 1985, Autor = "Herman Hesse", Preco = 8m, Titulo = "Demian" };

        //    var hoje = DateTime.Now.Date;
        //    var ontem = hoje.AddDays(-1);
        //    var anteOntem = hoje.AddDays(-2);
        //    var semanaPassada = hoje.AddDays(-7);
        //    var amanha = hoje.AddDays(1);
        //    var depoisDeAmanha = hoje.AddDays(2);
        //    var proximaSemana = hoje.AddDays(7);

        //    using(var bd = new BibliotecaDatabase())
        //    {
        //        bd.Usuarios.Add(meuUser);
        //        bd.Usuarios.Add(proto);
        //        bd.Usuarios.Add(tioBill);
        //        bd.Livros.Add(guerraDosTronos);
        //        bd.Livros.Add(carrie);
        //        bd.Livros.Add(arteGuerra);
        //        bd.Livros.Add(jogosVorazes);
        //        bd.Livros.Add(demian);

        //        bd.Emprestimos.Add(new Emprestimo { LivroEmprestimo = jogosVorazes, UsuarioEmprestimo = meuUser, RetiradoEm = semanaPassada, DevolverAte = hoje });
        //        bd.Emprestimos.Add(new Emprestimo { LivroEmprestimo = carrie, UsuarioEmprestimo = meuUser, RetiradoEm = semanaPassada, DevolverAte = ontem });
        //        bd.Emprestimos.Add(new Emprestimo { LivroEmprestimo = guerraDosTronos, UsuarioEmprestimo = meuUser, RetiradoEm = ontem, DevolverAte = proximaSemana });
        //        bd.Emprestimos.Add(new Emprestimo { LivroEmprestimo = arteGuerra, UsuarioEmprestimo = tioBill, RetiradoEm = semanaPassada, DevolverAte = proximaSemana });
        //        bd.SaveChanges();
        //    }

        //    //Depois os Emprestimos para testar


        //    return RedirectToAction("Index", new DevolucaoViewModel());
        //}

        #endregion

        public IEnumerable<Usuario> ObterUsuariosComEmprestimo()
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from p in bd.Emprestimos
                             where p.DevolvidoEm == null
                             select p.UsuarioEmprestimo).Distinct().ToList();
                return query.ToList();
            }
        }

        public void Devolver(int idEmprestimo)
        {
            using (var bd = new BibliotecaDatabase())
            {
                using (var transacao = bd.Database.BeginTransaction())
                {
                    try
                    {
                        var emprestimo = bd.Emprestimos.Find(idEmprestimo);
                        emprestimo.DevolvidoEm = DateTime.Now;
                        bd.Entry(emprestimo).State = EntityState.Modified;

                        var ts = emprestimo.DevolvidoEm.Value - emprestimo.DevolverAte;
                        var diasAtraso = ts.Days;

                        if (diasAtraso > 0)
                        {
                            var debito = new Debito
                                                {
                                                    DebitoAtivo = true,
                                                    Emprestimo = emprestimo,
                                                    UsuarioDeb = emprestimo.UsuarioEmprestimo,
                                                    DiasAtraso = diasAtraso
                                                };

                            bd.Debitos.Add(debito);

                            //Márcio Koch - Libera a reserva do livro, caso ele esteja reservado.
                            //TODO: Isso precisa ser testado ainda, devido a devolução ainda não estar pronta.
                            ReservaController.removerReserva(bd, emprestimo.LivroEmprestimo.LivroId);
                        }

                        transacao.Commit();
                        bd.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        transacao.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public IEnumerable<Emprestimo> ObterLivrosDevolvidos()
        {
            using (var bd = new BibliotecaDatabase())
            {
                var livrosDevolvidos = (from e in bd.Emprestimos.Include(j => j.LivroEmprestimo)
                                        where e.DevolvidoEm != null
                                        select e).ToList();

                return livrosDevolvidos;
            }
        }

        public IEnumerable<Livro> ListarLivrosDoUsuario(int idEmprestimo)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var emprestimo = (from e in bd.Emprestimos.Include(j => j.LivroEmprestimo)
                                  where e.UsuarioEmprestimo.UsuarioId == idEmprestimo && !e.DevolvidoEm.HasValue
                                  select e.LivroEmprestimo).ToList();

                return emprestimo;
            }
        }

        public int CarregarEmprestimo(int idUsuario, int idLivro)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from e in bd.Emprestimos.Include(j => j.LivroEmprestimo)
                             where e.LivroEmprestimo.LivroId == idLivro && e.UsuarioEmprestimo.UsuarioId == idUsuario
                             select e.EmprestimoId).FirstOrDefault();

                return (int)query;
            }
        }

    }
}