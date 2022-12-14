using NLog;

namespace FundooNotes
{
    public class NLog
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public static object GlobalDiagnosticsContext { get; internal set; }

        //Method to write success info to a Logfile
        public static void SuccessInfo(string message)
        {
            logger.Info(message);
        }

        //Method to write error info to a Logfile
        public static void ErrorInfo(string message)
        {
            logger.Error(message);
        }
    }
}
