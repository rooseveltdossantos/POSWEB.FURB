using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using MvcBiblioteca.Models;
using System.Web.Security;
using System.Linq;

namespace MvcBiblioteca.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<UsersContext>(null);

                try
                {
                    using (var context = new UsersContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                    
                    InicilizaComRolesPadraoEUsuarioAdmin();


                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }

            private void InicilizaComRolesPadraoEUsuarioAdmin()
            {
                var roles = Roles.GetAllRoles();

                if (!roles.Any(r => r == "PodeAdicionarLivro"))
                    Roles.CreateRole("PodeAdicionarLivro");

                if (!roles.Any(r => r == "PodeComentar"))
                    Roles.CreateRole("PodeComentar");

                if (!roles.Any(r => r == "PodeGerenciarUsuarios"))
                    Roles.CreateRole("PodeGerenciarUsuarios");

                if (!WebSecurity.UserExists("Admin"))
                {
                    WebSecurity.CreateUserAndAccount("Admin", "admin");
                    Roles.AddUserToRole("Admin", "PodeAdicionarLivro");
                }

                if (!Roles.IsUserInRole("Admin", "PodeAdicionarLivro")
                    Roles.AddUserToRole("Admin", "PodeAdicionarLivro");

                if (!Roles.IsUserInRole("Admin", "PodeGerenciarUsuarios"))
                    Roles.AddUserToRole("Admin", "PodeGerenciarUsuarios");



            }
        }
    }
}
