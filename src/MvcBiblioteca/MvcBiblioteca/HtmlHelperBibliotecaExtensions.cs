using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using Biblioteca.Dominio;
using System.Reflection;

namespace MvcBiblioteca
{
    public static class HtmlHelperBibliotecaExtensions
    {
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Expression<Func<VisualizacaoAttribute, string>> apresentar)
        {
            var items = Enum.GetValues(typeof(TProperty));

            var nomeDosItems = ((TProperty[])items).Select((p, s) => 
            {
                Type tipo = p.GetType();
                MemberInfo membro = tipo.GetMember(p.ToString())[0];
                
                var visualizacaoAttribute = (VisualizacaoAttribute)
                        Attribute.GetCustomAttribute(membro, typeof(VisualizacaoAttribute));

                if (visualizacaoAttribute != null)
                    return apresentar.Compile().Invoke(visualizacaoAttribute);
                else
                    return p.ToString();                
            });

            var selectList = new SelectList(nomeDosItems);
            return htmlHelper.DropDownListFor(expression, selectList);                       
        }



        public static MvcHtmlString LabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Expression<Func<VisualizacaoAttribute, string>> apresentar) {
            Type tipoModelo = typeof(TModel);
            
            MemberInfo membro = tipoModelo.GetMember(expression.ReturnType.Name)[0];

            var visualizacaoAttribute = (VisualizacaoAttribute)
                    Attribute.GetCustomAttribute(membro, typeof(VisualizacaoAttribute));

            if (visualizacaoAttribute != null)
                return htmlHelper.LabelFor(expression, apresentar.Compile().Invoke(visualizacaoAttribute));
            else
                return htmlHelper.LabelFor(expression);            
        }
    }
}