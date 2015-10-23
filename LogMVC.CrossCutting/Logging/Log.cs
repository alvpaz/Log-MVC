using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LogMVC.CrossCutting.Logging
{
    public class Log
    {
        

        public static void LogarErro(string mensagem, 
                                     Exception exception,
                                     [CallerMemberName]string sourceMember = "",
                                     [CallerFilePath]string sourceFilePath = "",
                                     [CallerLineNumber] int sourceLineNumber = 0,
                                      params object[] args)
        {

            var mensagemErro = String.Format(CultureInfo.InvariantCulture,
                                            "ERRO-{0}\nMetodo: {1}\nCaminhoArquivo:{2} - Linha:{3}\n"
                                            , mensagem
                                            , sourceMember
                                            , sourceFilePath
                                            , sourceLineNumber);

            LoggerFactory.CreateLog().Error(mensagemErro, exception, args);

        }

        public static void LogarErro(string mensagem, [CallerMemberName]string sourceMember = "",
                                                      [CallerFilePath]string sourceFilePath = "",
                                                      [CallerLineNumber] int sourceLineNumber = 0,
                                                      params object[] args)
        {

            

            var mensagemErro = string.Format(CultureInfo.InvariantCulture,
                                            "ERRO-{0}\nMetodo: {1}\nCaminhoArquivo:{2} - Linha:{3}\n"
                                            , mensagem
                                            , sourceMember
                                            , sourceFilePath
                                            , sourceLineNumber);
            
            LoggerFactory.CreateLog().Error(mensagemErro, args);
        }

        public static void LogarFatal(string mensagem, [CallerMemberName]string sourceMember = "",
                                                       [CallerFilePath]string sourceFilePath = "",
                                                       [CallerLineNumber] int sourceLineNumber = 0,
                                                       params object[] args)
        {
            var mensagemErro = string.Format(CultureInfo.InvariantCulture,
                                            "FATAL-{0}\nMetodo: {1}\nCaminhoArquivo:{2} - Linha:{3}\n"
                                            , mensagem
                                            , sourceMember
                                            , sourceFilePath
                                            , sourceLineNumber);


            LoggerFactory.CreateLog().Fatal(mensagemErro, args);
        }

        public static void LogarFatal(string mensagem, 
                                     Exception exception,
                                     [CallerMemberName]string sourceMember = "",
                                     [CallerFilePath]string sourceFilePath = "",
                                     [CallerLineNumber] int sourceLineNumber = 0,
                                      params object[] args)
        {

            var mensagemErro = String.Format(CultureInfo.InvariantCulture,
                                            "FATAL-{0}\nMetodo: {1}\nCaminhoArquivo:{2} - Linha:{3}\n"
                                            , mensagem
                                            , sourceMember
                                            , sourceFilePath
                                            , sourceLineNumber);

            LoggerFactory.CreateLog().Fatal(mensagemErro, exception, args);
        }

        public static void LogarInformacao(string mensagem, [CallerMemberName]string memberName = "", params object[] args)
        {
            //Performance 
            Task.Run(() =>
            {
                LoggerFactory.CreateLog().Info(String.Format(CultureInfo.InvariantCulture,
                                               "INFO-{0} Metodo:{1} ", mensagem, memberName), args);
           });
        }

        public static void LogarAlerta(string mensagem, [CallerMemberName]string memberName = "", params object[] args)
        {
            LoggerFactory.CreateLog().Warning(String.Format(CultureInfo.InvariantCulture, 
                                              "ALERTA-{0} Metodo: {1} ", mensagem, memberName), args);
        }
    }
}
