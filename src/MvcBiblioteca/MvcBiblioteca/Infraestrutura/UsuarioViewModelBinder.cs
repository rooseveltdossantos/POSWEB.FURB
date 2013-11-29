using System;
using System.Linq;
using System.Web.Mvc;
using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using MvcBiblioteca.Models;

namespace MvcBiblioteca.Infraestrutura
{
    public class UsuarioViewModelBinder : DefaultModelBinder
    {
        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
        {
            if (propertyDescriptor.PropertyType == typeof(TipoUsuario))
            {
                var propertyName = propertyDescriptor.Name;
                var propertyValue = bindingContext.ModelState[propertyName].Value.AttemptedValue;

                ConverterParaTipoUsuario(bindingContext, propertyName, propertyValue);
            }
            else
                base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }

        private static void ConverterParaTipoUsuario(ModelBindingContext bindingContext, string propertyName, string propertyValue)
        {
            var tipoUsuario = ((TipoUsuario[]) Enum.GetValues(typeof (TipoUsuario))).FirstOrDefault(i =>
                {
                    var visualizacaoAttribute = (VisualizacaoAttribute)Attribute.GetCustomAttribute(i.GetType().GetMember(i.ToString())[0], typeof (VisualizacaoAttribute));

                    if (visualizacaoAttribute == null)
                        return propertyValue == i.ToString();

                    return (visualizacaoAttribute.ApresentacaoParaFormulario == propertyValue || propertyValue == i.ToString());
                });

            (bindingContext.Model as UsuarioViewModel).TipoUsuario = tipoUsuario;
            bindingContext.ModelState[propertyName].Errors.Clear();
        }

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
