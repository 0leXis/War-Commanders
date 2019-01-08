using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

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
        public const string ServerIP = "25.47.239.150:9080";
        
        [STAThread]
        static void Main()
        {
        	Log.EnableConsoleLog = true;
            //Log.EnableFileLog = true;
            
			NI = new NetworkInterface();
			Log.SendMessage("Подключение к: "+ServerIP);
            NI.ConnectTo(ServerIP);
            while(true)
            {
            	var gg = NI.GetMsgs();
            	if(gg.Length > 0)
            	{
            		var str = "";
            		foreach(var g in gg)
            			str += g;
            		int[][] mass;
            		var jsonFormatter = new DataContractJsonSerializer(typeof(int[][]));
            		mass = (int[][])jsonFormatter.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(str)));
            		for(var i = 0; i < mass.Length; i++)
            			for(var j = 0; j < mass.Length; j++)
            				Log.SendMessage(mass[i][j].ToString());
            		break;
            	}
            }
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
