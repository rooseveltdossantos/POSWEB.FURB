using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biblioteca.DataAccess;
using Biblioteca.Dominio;
using MvcBiblioteca.Models;
using System.Data.Entity;

namespace MvcBiblioteca.Controllers
{
    public class UsuariosController : Controller
    {
        //
        // GET: /Usuario/

        public ActionResult Index()
        {
            using (var bd = new BibliotecaDatabase())
            {
                var usuarios = bd.Usuarios.ToList();
                var usu = usuarios.Select(usuario => new UsuarioViewModel(usuario));
                return View(usu);
            }            
        }

        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Novo(UsuarioViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                using (var bd = new BibliotecaDatabase())
                {
                    if (usuario.UsuarioId <= 0)
                        bd.Usuarios.Add(usuario.ParaEntidade());
                    else
                        bd.Entry(usuario.ParaEntidade()).State = EntityState.Modified;
                    bd.SaveChanges();
                    return RedirectToAction("index");
                } 
            }
            return View(usuario);
        }

        public ActionResult Alterar(UsuarioViewModel usuarioId)
        {
            return View("Novo", usuarioId);
        }

        public ActionResult Excluir(long usuarioId)
        {
            using (var bd = new BibliotecaDatabase())
            {
                var usuario = bd.Usuarios.Find(usuarioId);
                bd.Entry(usuario).State = EntityState.Deleted;
                bd.SaveChanges();
                return RedirectToAction("index");
            }
        }

        public JsonResult CPFValido(long cpf)
        {
            var cpfCalculado = CpfCalculado(cpf);

            return Json(cpfCalculado == cpf, JsonRequestBehavior.AllowGet);
        }


        private long CpfCalculado(long cpf)
        {
            var @base = cpf / 100L;

            var primeiroDigito = CalculaPrimeiroDigito(@base);

            var segundoDigito = CalculaSegundoDigito(primeiroDigito, @base);

            var dv = (10 * primeiroDigito) + segundoDigito;

            var cpfCalculado = @base * 100 + dv;
            return cpfCalculado;
        }

        private long CalculaSegundoDigito(long primeiroDigito, long @base)
        {
            @base = (@base * 10) + primeiroDigito;

            var fator = 1000000000L;
            var soma = 0L;

            for (var i = 0; i <= 9; i++)
            {
                var digito = @base / fator;
                @base = @base - (digito * fator);
                fator /= 10;
                soma += digito * i;
            }

            var sd = soma % 11;
            return sd;
        }

        private long CalculaPrimeiroDigito(long @base)
        {
            var fator = 100000000L;
            var soma = 0L;

            for (var i = 1; i <= 9; i++)
            {
                var digito = @base / fator;
                @base = @base - (digito * fator);
                fator /= 10;
                soma += digito * i;
            }

            var pd = soma % 11;
            return pd;
        }
    }
}
