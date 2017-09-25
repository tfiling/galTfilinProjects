using System;
using System.Collections.Generic;
using System.IO;

namespace ForumBuilder.Systems
{
    class Logger : ILogger
    {
        private static Logger _singleton;
        private StreamWriter _sw1;
        private StreamWriter _sw2;
        private StreamWriter _sw3;

        private Logger()
        {
            _sw1 = File.CreateText(Path.GetFullPath("log.txt"));//File.AppendText("log.txt");// new StreamWriter(fullPath);
            _sw2 = File.CreateText(Path.GetFullPath("logActions.txt"));
            _sw3 = File.CreateText(Path.GetFullPath("logErrors.txt"));
        }

        public static Logger getInstance
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new Logger(); 
                }
                return _singleton;
            }
        }

        public void logPrint(String contentToPrint,int swrite)
        {
            switch (swrite)
            {
                case 0:
                    _sw1.WriteLine("Log Entry : \t\t\t\t" + DateTime.Now);
                    _sw1.WriteLine(contentToPrint);
                    _sw1.WriteLine("-------------------------------------------------------------------------------------");
                    _sw1.Flush();
                    break;
                case 1:
                    _sw2.WriteLine("Log Entry : \t\t\t\t" + DateTime.Now);
                    _sw2.WriteLine(contentToPrint);
                    _sw2.WriteLine("-------------------------------------------------------------------------------------");
                    _sw2.Flush();
                    break;
                case 2:
                    _sw3.WriteLine("Log Entry : \t\t\t\t" + DateTime.Now);
                    _sw3.WriteLine(contentToPrint);
                    _sw3.WriteLine("-------------------------------------------------------------------------------------");
                    _sw3.Flush();
                    break;
                default:
                    _sw1.WriteLine("Log Entry : \t\t\t\t" + DateTime.Now);
                    _sw1.WriteLine(contentToPrint);
                    _sw1.WriteLine("-------------------------------------------------------------------------------------");
                    _sw1.Flush();
                    break;
            }
        }
    }
}
