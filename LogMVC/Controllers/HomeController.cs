using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogMVC.CrossCutting.Logging;

namespace LogMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            object[] arguments =   {"Teste Log", "Teste Log 2"};


            Log.LogarInformacao("Informacao", args: arguments);
            Log.LogarInformacao("Informacao");
            Log.LogarAlerta("Alerta");
            Log.LogarErro("Erro");
            Log.LogarErro("Erro", args: arguments);
            Log.LogarErro("Erro", new ArgumentNullException("Nada"), args: arguments);
            Log.LogarFatal("Fatal");
            Log.LogarFatal("Fatal", new ArgumentNullException("Nada"));
            
            return View();
        }
    }
}