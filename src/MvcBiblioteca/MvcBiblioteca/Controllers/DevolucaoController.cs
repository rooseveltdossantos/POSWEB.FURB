using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using MvcBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MvcBiblioteca.Controllers
{
    public class DevolucaoController : Controller
    {

        public ActionResult Popular()
        {
            var meuUser = new Usuario { Login = "lah", Nome = "Luiz Angelo Heinzen", Senha = "segredo", StatusAtivacao = StatusAtivacao.Ativo, TipoUsuario = TipoUsuario.Funcionario };
            var proto = new Usuario { Login = "proto", Nome = "Proto(u)suario", Senha = "segredo", StatusAtivacao = StatusAtivacao.Ativo, TipoUsuario = TipoUsuario.Aluno };
            var tioBill = new Usuario { Login = "bgates", Nome = "Bill Gates", Senha = "segredo", StatusAtivacao = StatusAtivacao.Ativo, TipoUsuario = TipoUsuario.Professor};
            var guerraDosTronos = new Livro { Ano = 2000, Autor = "George R.R. Martin", Preco = 40m, Titulo = "A Guerra dos Tronos" };
            var carrie = new Livro { Ano = 1979, Autor = "Stepen King", Preco = 15m, Titulo = "Carrie, a estranha" };
            var arteGuerra = new Livro { Ano = 1600, Autor = "Sun-Tzu", Preco = 5m, Titulo = "A arte da Guerra" };
            var jogosVorazes = new Livro { Ano = 2003, Autor = "Stephanie Meyer", Preco = 25m, Titulo = "Jogos Vorazes" };
            var demian = new Livro { Ano = 1985, Autor = "Herman Hesse", Preco = 8m, Titulo = "Demian" };

            var hoje = DateTime.Now.Date;
            var ontem = hoje.AddDays(-1);
            var anteOntem = hoje.AddDays(-2);
            var semanaPassada = hoje.AddDays(-7);
            var amanha = hoje.AddDays(1);
            var depoisDeAmanha = hoje.AddDays(2);
            var proximaSemana = hoje.AddDays(7);

            using(var bd = new BibliotecaDatabase())
	        {
                bd.Usuarios.Add(meuUser);
                bd.Usuarios.Add(proto);
                bd.Usuarios.Add(tioBill);
                bd.Livros.Add(guerraDosTronos);
                bd.Livros.Add(carrie);
                bd.Livros.Add(arteGuerra);
                bd.Livros.Add(jogosVorazes);
                bd.Livros.Add(demian);

                bd.Emprestimos.Add(new Emprestimo { LivroEmprestimo = jogosVorazes, UsuarioEmprestimo = meuUser, RetiradoEm = semanaPassada, DevolverAte = hoje });
                bd.Emprestimos.Add(new Emprestimo { LivroEmprestimo = carrie, UsuarioEmprestimo = meuUser, RetiradoEm = semanaPassada, DevolverAte = ontem });
                bd.Emprestimos.Add(new Emprestimo { LivroEmprestimo = guerraDosTronos, UsuarioEmprestimo = meuUser, RetiradoEm = ontem, DevolverAte = proximaSemana });
                bd.Emprestimos.Add(new Emprestimo { LivroEmprestimo = arteGuerra, UsuarioEmprestimo = tioBill, RetiradoEm = semanaPassada, DevolverAte = proximaSemana });
                bd.SaveChanges();
	        }

            //Depois os Emprestimos para testar


            return RedirectToAction("Index", new DevolucaoViewModel());
        }
        
        public ActionResult Index()
        {
            return View(new DevolucaoViewModel());
        }

        public ActionResult Devolver(int idEmprestimo)
        {
            // TODO: Verificar se há necessidade de fazer método que poderá ser usado na listagem de emprestimo, passado usuário e id pode-se devolver em apenas 
            // clique, evitando assim a interface, tentar manter os dois modelos.
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Devolver(DevolucaoViewModel u)
        {

            // TODO: Tratar também o evento do change dos select, trazer o primeiro emprestimo para que não seja necessário fazer o change.

            // Procurar pelo emprestimo e se a data de entrega do livro for maior que a devolução gerar débito
            // e calcular a diferença de dias para fazer a gravação correta do débito.
            using (var bd = new BibliotecaDatabase())
            {
                Emprestimo emprestimo = bd.Emprestimos.Find(u.idEmprestimo);
                emprestimo.DevolvidoEm = DateTime.Now.Date;
                //Calculando os dias de atraso
                TimeSpan ts = emprestimo.DevolvidoEm.Value - emprestimo.DevolverAte;
                int diasAtraso = ts.Days;
                if (diasAtraso > 0)
                {
                    Debito debito = new Debito();
                    debito.DebitoAtivo = true;
                    debito.Emprestimo = emprestimo;
                    debito.UsuarioDeb = emprestimo.UsuarioEmprestimo;
                    debito.DiasAtraso = diasAtraso;
                    bd.Debitos.Add(debito);
                    bd.SaveChanges();
                }
            }
            
            return RedirectToAction("Index", new DevolucaoViewModel());
        }

        public ActionResult ListarLivrosDoUsuario(int id)
        {

            using (var bd = new BibliotecaDatabase())
            {

                var query = (from e in bd.Emprestimos.Include(j => j.LivroEmprestimo)
                             where e.UsuarioEmprestimo.UsuarioId == id && !e.DevolvidoEm.HasValue
                             select e.LivroEmprestimo).ToList();

                return Json(query.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CarregarEmprestimo(int idUsuario, int idLivro)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var query = (from e in bd.Emprestimos.Include(j => j.LivroEmprestimo)
                             where e.LivroEmprestimo.LivroId == idLivro && e.UsuarioEmprestimo.UsuarioId == idUsuario
                             select e.EmprestimoId).FirstOrDefault();

                return Json(new { idEmprestimo = query }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
