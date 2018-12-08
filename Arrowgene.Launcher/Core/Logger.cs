namespace Arrowgene.Launcher.Core
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public class Logger
    {
        private string _logFile;

        public Logger(string logFile)
        {
            _logFile = logFile;
        }

        public void Log(string log, string origin)
        {
            log = "[" + DateTime.Now.ToString() + "] " + origin + ": " + log;
            Debug.WriteLine(log);
            using (StreamWriter file = new StreamWriter(_logFile, true))
            {
                file.WriteLine(log);
            }
        }

        public void Log(Exception ex, string origin)
        {
            Log(ex.ToString(), origin);
        }

    }
}
