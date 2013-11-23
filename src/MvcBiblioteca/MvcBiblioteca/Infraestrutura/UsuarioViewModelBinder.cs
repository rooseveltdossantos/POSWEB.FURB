using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Biblioteca.DataAccess;
using MvcBiblioteca.Models;

namespace MvcBiblioteca.Infraestrutura
{
    public class UsuarioViewModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valor = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valor == null || string.IsNullOrEmpty(valor.AttemptedValue))
                return base.BindModel(controllerContext, bindingContext);

            int usuarioId;

            if (!int.TryParse(valor.AttemptedValue, out usuarioId))
                return base.BindModel(controllerContext, bindingContext);

            using (var bd = new BibliotecaDatabase())
            {
                var usuario = bd.Usuarios.Find(usuarioId);
                return new UsuarioViewModel(usuario);
            }
        }
    }
}
