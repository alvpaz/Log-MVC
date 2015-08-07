using LogMVC.CrossCutting.Logging;

namespace LogMVC
{
    public static class Configure 
    {
        internal static void ConfigureFactories()
        {
            LoggerFactory.SetCurrent(new TraceSourceLogFactory());
        }
    }
}