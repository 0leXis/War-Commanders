using System;

namespace GameCursachProject
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        static public NetworkInterface NI;
        public const string ServerIP = "25.30.70.249:8888";
        
        [STAThread]
        static void Main()
        {
        	Log.EnableConsoleLog = true;
            //Log.EnableFileLog = true;
            
			NI = new NetworkInterface();
			Log.SendMessage("Подключение к: "+ServerIP);
            NI.ConnectTo(ServerIP);
            using (var game = new Game1())
            {
                game.Run();
            }
            NI.Disconnect();
            
            Log.EnableConsoleLog = false;
            //Log.EnableFileLog = false;
        }
    }
}
