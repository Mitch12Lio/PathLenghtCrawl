using System;
using System.Collections.Generic;
using System.Text;

namespace PathLenghtCrawl.Log
{
    public class Log
    {

        public static void WriteLog(string path, string value)
        {
            using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(path + System.IO.Path.DirectorySeparatorChar + "log.log", true, Encoding.UTF8))
            {
                logFile.WriteLine(value);
            }
        }
    }
}
