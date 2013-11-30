using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Biblioteca.DataAccess;
using MvcBiblioteca.Models;
using Biblioteca.Dominio;

namespace MvcBiblioteca.Infraestrutura
{
    public class LivroBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valor = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valor == null || string.IsNullOrEmpty(valor.AttemptedValue))
                return base.BindModel(controllerContext, bindingContext);

            int livroId;

            if (!int.TryParse(valor.AttemptedValue, out livroId))
                return base.BindModel(controllerContext, bindingContext);

            using (var bd = new BibliotecaDatabase())
            {
                var livro = bd.Livros.Find(livroId);
                return livro;
            }
        }
    }
}
