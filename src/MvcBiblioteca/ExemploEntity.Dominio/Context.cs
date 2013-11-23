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
        public Context() : base()
        {
            //Database.SetInitializer<Context>(new CreateDatabaseIfNotExists<Context>());
            //Database.SetInitializer<Context>(new DropCreateDatabaseIfModelChanges<Context>());
            //Database.SetInitializer<Context>(new DropCreateDatabaseAlways<Context>());
            Database.SetInitializer<Context>(new InicializadorBaseEscolar());

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configurar classes do domínio usando Fluent API 

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Estudante> Estudantes { get; set; }
        public DbSet<GrauDeConhecimento> GrausDeConhecimento { get; set; }

    }

    public class InicializadorBaseEscolar : DropCreateDatabaseAlways<Context>
    {
        protected override void Seed(Context context)
        {
            var grausPadrao = new List<GrauDeConhecimento>
            {
                new GrauDeConhecimento{ Nome = "Grau 1", Descricao = "Primeiro Grau"},
                new GrauDeConhecimento{ Nome = "Grau 2", Descricao = "Segundo Grau"},
                new GrauDeConhecimento{ Nome = "Grau 3", Descricao = "Terceiro Grau"}
            };

            foreach (var grau in grausPadrao)
            {
                context.GrausDeConhecimento.Add(grau);
            }
            base.Seed(context);
        }
    }


}
