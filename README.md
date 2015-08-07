# Log-MVC
Log designed to be used in various projects. This project is a case study.

# Example using 

  Qualquer classe ou object da para passar como parametros e é serializado em Json
  
  object[] arguments =   {"Teste Log", "Teste Log 2"};

* Log.LogarInformacao("Informacao", args: arguments);
* Log.LogarInformacao("Informacao");
* Log.LogarAlerta("Alerta");
* Log.LogarErro("Erro");
* Log.LogarErro("Erro", args: arguments);
* Log.LogarErro("Erro", new ArgumentNullException("Nada"), args: arguments);
* Log.LogarFatal("Fatal");
* Log.LogarFatal("Fatal", new ArgumentNullException("Nada"));
* Log.LogarFatal("Fatal", new ArgumentNullException("Nada"),args: arguments);

