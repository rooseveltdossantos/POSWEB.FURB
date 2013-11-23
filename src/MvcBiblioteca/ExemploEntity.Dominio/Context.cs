using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using DominioEscolar;

namespace ExemploEntity.Dominio
{
    public class Context : DbContext
    {
        public Context()
            : base()
        {
            Database.SetInitializer<Context>(new CreateDatabaseIfNotExists<Context>());
            //Database.SetInitializer<Context>(new DropCreateDatabaseIfModelChanges<Context>());
            //Database.SetInitializer<Context>(new DropCreateDatabaseAlways<Context>());
            //Database.SetInitializer<Context>(new InicializadorBaseEscolar());

        }

        public DbSet<Estudante> Estudantes { get; set; }
        public DbSet<GrauDeConhecimento> GrausDeConhecimento { get; set; }

    }

    public class InicializadorBaseEscolar : DropCreateDatabaseAlways<Context>
    {
        protected override void Seed(Context context)
        {
            base.Seed(context);
        }
    }


}
