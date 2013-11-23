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
                var std = new Estudante() { Nome = "Novo Estudante" };
                ctx.Estudantes.Add(std);
                ctx.SaveChanges();
            }

            Console.WriteLine("DEU CERTO");
            Console.ReadLine();
        }
    }
}
