﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBiblioteca.Controllers
{
    public class HistoricoController : Controller
    {
        //
        // GET: /Historico/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListarHistoricoUsuario()
        {
            return null;
        }

    }
}
