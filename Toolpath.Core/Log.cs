using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Luthier.Core
{
    /*Use Singleton Pattern for Log*/
    public class Log
    {
        private static Log instance = new Log();

        private string path = Path.Combine(@"C:\Users\Richard\Documents\Development\ToolPath\Log.txt");

        protected Log() { }

        public static Log Instance() => instance;


        public void Append(string message)
        {
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                // This text is always added, making the file longer over time
                // if it is not deleted.
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(message);
                }
            }
        }
    }
}
