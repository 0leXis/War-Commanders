using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace GameCursachProject
{
    static class Log
    {
        //Разрешает отправление отладочной
        //информации в log.txt
        static private bool _EnableFileLog = false;
        //Log.txt
        static private FileStream LogFile;
        /// <summary>
        /// Разрешает отправление отладочной
        /// информации в консоль
        /// </summary>
        static public bool EnableConsoleLog { get; set; }
        /// <summary>
        /// Разрешает отправление отладочной
        /// информации в log.txt
        /// </summary>
        static public bool EnableFileLog
        {
            get
            {
                return _EnableFileLog;
            }
            set
            {
                if (value)
                {
                    //var Str = DateTime.Now.ToString("G");
                    //Str = Str.Replace('.', '-');
                    //Str = Str.Replace(':', '-');
                    //Str = Str.Replace(" ", "_");
                    if (!Directory.Exists(@"log"))
                        Directory.CreateDirectory(@"log");
                    //LogFile = new FileStream(@"log/" + Str + "_log.txt", FileMode.Create);
                    LogFile = new FileStream(@"log/log.txt", FileMode.Create);
                    _EnableFileLog = value;
                }
                else
                {
                    if (LogFile != null)
                        LogFile.Close();
                    _EnableFileLog = value;
                }
            }
        }

        /// <summary>
        /// Отправляет отладочную информацию
        /// </summary>
        /// <param name="Message"></param>
        static public void SendMessage(string Message)
        {
            if (EnableConsoleLog)
                Console.WriteLine("[" + DateTime.Now.ToString() + "]: " + Message);
            if (EnableFileLog)
            {
                var logbuffer = Encoding.Default.GetBytes("[" + DateTime.Now.ToString() + "]: " + Message + Environment.NewLine);
                LogFile.WriteAsync(logbuffer, 0, logbuffer.Length);
            }
        }
        /// <summary>
        /// Отправляет отладочную информацию об ошибке
        /// </summary>
        /// <param name="Message"></param>
        static public void SendError(string Message)
        {
            if (EnableConsoleLog)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR[" + DateTime.Now.ToString() + "]: " + Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (EnableFileLog)
            {
                var logbuffer = Encoding.Default.GetBytes("ERROR[" + DateTime.Now.ToString() + "]: " + Message + Environment.NewLine);
                LogFile.WriteAsync(logbuffer, 0, logbuffer.Length);
            }
        }
        /// <summary>
        /// Отправляет отладочную информацию о предупреждении
        /// </summary>
        /// <param name="Message"></param>
        static public void SendWarning(string Message)
        {
            if (EnableConsoleLog)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("WARNING[" + DateTime.Now.ToString() + "]: " + Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (EnableFileLog)
            {
                var logbuffer = Encoding.Default.GetBytes("WARNING[" + DateTime.Now.ToString() + "]: " + Message + Environment.NewLine);
                LogFile.WriteAsync(logbuffer, 0, logbuffer.Length);
            }
        }
    }
}
