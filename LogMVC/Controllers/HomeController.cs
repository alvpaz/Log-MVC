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

            Log.LogarInformacao("Mensagem de Informacao", args: arguments);
            Log.LogarInformacao("Mensagem de Informacao");
            Log.LogarAlerta("Mensagem de Alerta");
            Log.LogarErro("Mensagem de Erro");
            Log.LogarErro("Mensagem de Erro", args: arguments);
            Log.LogarErro("Mensagem de Erro", new ArgumentNullException("Nada"), args: arguments);
            Log.LogarFatal("Mensagem de Fatal");
            Log.LogarFatal("Mensagem de Fatal", new ArgumentNullException("Nada"));
            
            return View();
        }
    }
}