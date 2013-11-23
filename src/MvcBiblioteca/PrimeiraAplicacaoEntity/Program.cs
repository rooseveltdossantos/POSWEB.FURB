using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExemploEntity.Dominio;
using DominioEscolar;

namespace PrimeiraAplicacaoEntity
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new Context())
            {
                var grau01 = ctx.GrausDeConhecimento.Find(1);

                var std = new Estudante() { 
                        Nome = "Novo Estudante", 
                        EnderecoDoEstudante = new EnderecoDoEstudante()
                                {
                                    Cidade = "Blumenau"
                                }
                };

                grau01.ListaDeEstudantes.Add(std);

                var std02 = new Estudante()
                {
                    Nome = "Estudante de Indaial",
                    EnderecoDoEstudante = new EnderecoDoEstudante()
                    {
                        Cidade = "Indaial"
                    }
                };

                grau01.ListaDeEstudantes.Add(std02);

                ctx.Estudantes.Add(std);
                ctx.Estudantes.Add(std02);

                ctx.GrausDeConhecimento.Add(grau01);

                ctx.SaveChanges();
            }

            Console.WriteLine("DEU CERTO");
            Console.ReadLine();
        }
    }
}
