using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;


namespace ExemploEntity.Dominio
{
    public class Context : DbContext
    {
        public Context()
            : base()
        {

        }

        public DbSet<Estudante> Estudantes { get; set; }
        public DbSet<GrauDeConhecimento> GrausDeConhecimento { get; set; }

    }

}
