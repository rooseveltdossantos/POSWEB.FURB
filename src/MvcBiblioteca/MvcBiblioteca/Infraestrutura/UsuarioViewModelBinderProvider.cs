using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBiblioteca.Models;

namespace MvcBiblioteca.Infraestrutura
{
    public class UsuarioViewModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (modelType.IsAssignableFrom(typeof(UsuarioViewModel)))
                return new UsuarioViewModelBinder();
            return null;
        }
    }
}