using System;
using System.Collections.Generic;
using System.Text;

namespace PathLenghtCrawl.Log
{
    public static class Log
    {
        //public static void Write2ErrorLog(string path, string value)
        //{
        //    using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(path + System.IO.Path.DirectorySeparatorChar + "log.log", true, Encoding.UTF8))
        //    {
        //        logFile.WriteLine(value);
        //    }
        //}
        public static void Write2ErrorLog(string path, DateTime dateTime, string errorMessage,string pathCausingError)
        {
            using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(path + System.IO.Path.DirectorySeparatorChar + "LogErrors.log", true, Encoding.UTF8))
            {
                logFile.WriteLine(dateTime.ToString());
                logFile.WriteLine(errorMessage);
                logFile.WriteLine(pathCausingError);
                logFile.WriteLine(Environment.NewLine);
            }
        }
        public static void Write2WarningLog(string path, DateTime dateTime, string errorMessage, string pathCausingError)
        {
            using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(path + System.IO.Path.DirectorySeparatorChar + "LogWarnings.log", true, Encoding.UTF8))
            {
                logFile.WriteLine(dateTime.ToString());
                logFile.WriteLine(errorMessage);
                logFile.WriteLine(pathCausingError);
                logFile.WriteLine(Environment.NewLine);
            }
        }
    }
}
