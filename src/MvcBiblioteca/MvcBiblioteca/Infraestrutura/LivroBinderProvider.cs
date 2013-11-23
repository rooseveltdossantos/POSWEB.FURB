using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBiblioteca.Models;
using Biblioteca.Dominio;

namespace MvcBiblioteca.Infraestrutura
{
    public class LivroBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (modelType.IsAssignableFrom(typeof(Livro)))
                return new LivroBinder();
            return null;
        }
    }
}