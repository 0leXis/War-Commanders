using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCursachProject
{
    static class CommandParser
    {
    	//public const string ServerIP = "192.168.0.2:9080";
        public const string ServerIP = "127.0.0.1:9080";

        static public NetworkInterface NInterface;
    	static string[] _LastCommand;
    	
        static public string[] LastCommand
        {
            get
            {
                return _LastCommand;
            }
        }

    	static public void Init(NetworkInterface NI)
    	{
    		NInterface = NI;
            _LastCommand = null;
    	}
    	
    	static public void SendCommand(string[] command)
    	{
            NInterface.SendMsg(Serialization.Serialize(command));
    	    _LastCommand = command;
    	}
    	
    	static public void Update(out string[] Command)
    	{
            var msg = NInterface.GetNextMsg();
            if(msg != null)
            {
                Command = new string[0];

                var tmp = (Command as object);
                Serialization.DeSerialize(msg, ref tmp, typeof(string[]));
                Command = tmp as string[];

                Log.SendMessage(string.Format("Server (from {0}): {1}", NInterface.IP_Port, msg));
                return;
            }
            Command = null;
    	}
    	
    }
}
