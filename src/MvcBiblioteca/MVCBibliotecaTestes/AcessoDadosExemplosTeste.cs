using System;
using System.Linq;
using Biblioteca.DataAccess;
using NUnit.Framework;

namespace MVCBibliotecaTestes
{
    [TestFixture]
    public class AcessoDadosExemplosTeste
    {
       [Test]
       public void TesteComJoin()
       {
           using (var bd = new BibliotecaDatabase())
           {
               var q = from l in bd.Livros
                       join r in bd.Reservas
                       on l equals r.LivroRelacionado
                       where r.UsuarioDeb.UsuarioId == 1
                       select l;

               var livro = q.ToList();
               livro.ForEach( l => Console.WriteLine(l.Titulo));
           }
       }
    }
}
