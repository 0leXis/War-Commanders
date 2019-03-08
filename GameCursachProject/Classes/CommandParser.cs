using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCursachProject
{
    static class CommandParser
    {
    	//public const string ServerIP = "192.168.0.1:9080";
        public const string ServerIP = "127.0.0.1:9080";

        static public NetworkInterface GameServerInterface;
        static public NetworkInterface MasterServerInterface;
        static string[] _LastCommand;
    	
        static public string[] GameServerLastCommand
        {
            get
            {
                return _LastCommand;
            }
        }

    	static public void InitGameServer(NetworkInterface NI)
    	{
            GameServerInterface = NI;
            _LastCommand = null;
    	}

        static public void InitMasterServer(NetworkInterface NI)
        {
            MasterServerInterface = NI;
            _LastCommand = null;
        }

        static public void SendCommandToGameServer(string[] command)
    	{
            GameServerInterface.SendMsg(Serialization.Serialize(command));
    	    _LastCommand = command;
    	}

        static public void SendCommandToMasterServer(string[] command)
        {
            MasterServerInterface.SendMsg(Serialization.Serialize(command));
        }

        static public void UpdateMasterServer(out string[] Command)
        {
            var msg = MasterServerInterface.GetNextMsg();
            if (msg != null)
            {
                Command = new string[0];

                var tmp = (Command as object);
                Serialization.DeSerialize(msg, ref tmp, typeof(string[]));
                Command = tmp as string[];

                Log.SendMessage(string.Format("Server (from {0}): {1}", MasterServerInterface.IP_Port, msg));
                return;
            }
            Command = null;
        }

        static public void UpdateGameServer(out string[] Command)
    	{
            var msg = GameServerInterface.GetNextMsg();
            if(msg != null)
            {
                Command = new string[0];

                var tmp = (Command as object);
                Serialization.DeSerialize(msg, ref tmp, typeof(string[]));
                Command = tmp as string[];

                Log.SendMessage(string.Format("Server (from {0}): {1}", GameServerInterface.IP_Port, msg));
                return;
            }
            Command = null;
    	}
    	
    }
}
