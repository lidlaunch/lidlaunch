using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace LidLaunchWebsite.Models
{
    public class Logger
    {
        public static void Log(String lines, string pathName)
        {
            // Write the string to a file.append mode is enabled so that the log
            // lines get appended to  test.txt than wiping content and writing the log

            try
            {
                using (StreamWriter sw = File.AppendText(pathName))
                {
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " --> " + lines);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

            }
        }
    }
}