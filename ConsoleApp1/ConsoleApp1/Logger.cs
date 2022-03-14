using System;
using System.IO;

namespace Utils
{
    public class Logger
    {
        private string logfile;
        public Logger(string fileName)
        {
            logfile = fileName;
        }
        public bool Write(string logMsg)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(logfile))
                {
                    sw.WriteLine(logMsg);
                    sw.Close();
                }
                return true;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
