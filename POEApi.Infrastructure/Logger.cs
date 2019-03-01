using System;
using System.IO;

namespace POEApi.Infrastructure
{
    public static class Logger
    {
        private const string Output = "DebugInfo.log";
        public static void Log(Exception e)
        {
            Log(e.ToString());
        }
        public static void Log(string message)
        {
            File.AppendAllText(Output, string.Format("{0}[{1}] {2}", Environment.NewLine, DateTime.Now.ToString("dd-MM-yyyy H:mm"), message));
        }
    }
}
