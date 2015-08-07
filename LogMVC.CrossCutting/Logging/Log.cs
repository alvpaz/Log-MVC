using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LogMVC.CrossCutting.Logging
{
    public class Log
    {
        private static string GetCallingType()
        {
            return (new StackFrame(2, false).GetMethod()).DeclaringType.ToString();
        }

        public static void LogarErro(string mensagem, Exception exception,
                                                     [CallerMemberName]string sourceMember = "",
                                                     [CallerFilePath]string sourceFilePath = "",
                                                     [CallerLineNumber] int sourceLineNumber = 0,
                                                      params object[] args)
        {

            var mensagemErro = String.Format(CultureInfo.InvariantCulture,
                                            "ERRO-{0}\n Metodo: {1}\n TipoChamada: {2}\n CaminhoArquivo:{3} - Linha:{4}\n Mensagem de Exceção:{5}\n"
                                            , mensagem
                                            , sourceMember
                                            , GetCallingType()
                                            , sourceFilePath
                                            , sourceLineNumber
                                            , exception);

            LoggerFactory.CreateLog().Error(mensagemErro, exception, args);
        }

        public static void LogarErro(string mensagem, [CallerMemberName]string sourceMember = "",
                                                      [CallerFilePath]string sourceFilePath = "",
                                                      [CallerLineNumber] int sourceLineNumber = 0,
                                                      params object[] args)
        {

            var mensagemErro = String.Format(CultureInfo.InvariantCulture,
                                            "ERRO-{0}\n Metodo: {1}\n TipoChamada: {2}\n CaminhoArquivo:{3} - Linha:{4}\n"
                                            , mensagem
                                            , sourceMember
                                            , GetCallingType()
                                            , sourceFilePath
                                            , sourceLineNumber);

            LoggerFactory.CreateLog().Error(mensagemErro, args);

        }

        public static void LogarFatal(string mensagem, [CallerMemberName]string memberName = "", params object[] args)
        {
            LoggerFactory.CreateLog().Fatal(String.Format(CultureInfo.InvariantCulture, "FATAL-{0} Metodo: {1} ", mensagem, memberName), args);
        }

        public static void LogarFatal(string mensagem, Exception exception, [CallerMemberName]string memberName = "", params object[] args)
        {
            LoggerFactory.CreateLog().Fatal(String.Format(CultureInfo.InvariantCulture,
                                            "FATAL-{0} Metodo: {1} - Mensagem de Exceção: {2}", 
                                            mensagem, memberName, exception.Message), exception, args);
        }

        public static void LogarInformacao(string mensagem, [CallerMemberName]string memberName = "", params object[] args)
        {
            //Performance 
            //Task.Run(() =>
            //{
                LoggerFactory.CreateLog().Info(String.Format(CultureInfo.InvariantCulture,
                                               "INFO-{0} Metodo:{1} ", mensagem, memberName), args);
          //  });
        }

        public static void LogarAlerta(string mensagem, [CallerMemberName]string memberName = "", params object[] args)
        {
            LoggerFactory.CreateLog().Warning(String.Format(CultureInfo.InvariantCulture, 
                                              "ALERTA-{0} Metodo: {1} ", mensagem, memberName), args);
        }
    }
}
