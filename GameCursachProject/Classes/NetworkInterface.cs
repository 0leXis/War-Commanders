using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace GameCursachProject
{
    public class NetworkInterface
    {
        private TcpClient Client;
        private Thread DGThread;
        private bool ExitCommand;

        public Queue<string> MsgReadList { get; set; }
        public Queue<string> MsgWriteList { get; set; }

        private object LockObject = new object();
        private string _IP_Port;

        public string IP_Port
        {
            get
            {
                return _IP_Port;
            }
        }

        public bool IsConnected
        {
            get
            {
                lock (LockObject)
                {
                    if (Client == null)
                        return false;
                    return Client.Connected;
                }
            }
        }

        public NetworkInterface()
        {
            MsgReadList = new Queue<string>();
            MsgWriteList = new Queue<string>();
        }

        public void ConnectTo(string IP_Port)
        {
            if (DGThread == null || !DGThread.IsAlive)
            {
                Log.SendMessage("[WCNetwork]: Подключение к " + IP_Port);
                _IP_Port = IP_Port;
                Client = new TcpClient();
                ExitCommand = false;
                DGThread = new Thread(new ParameterizedThreadStart(DataGetThread));
                DGThread.Start(IP_Port);
            }
        }

        public void Disconnect()
        {
            lock (LockObject)
            {
                ExitCommand = true;
            }
        }

        public string[] GetMsgs()
        {
            string[] data;
            lock (LockObject)
            {
                data = new string[MsgReadList.Count];
                var i = 0;
                while (MsgReadList.Count > 0)
                {
                    data[i] = MsgReadList.Dequeue();
                    i++;
                }
            }
            return data;
        }

        public void SendMsg(string Msg)
        {
            lock (LockObject)
            {
                MsgWriteList.Enqueue(Msg);
            }
        }

        private void DataGetThread(object Data)
        {
        	try
            {
            	var tmpmas = ((string)Data).Split(':');
            	Client.Connect(tmpmas[0], Convert.ToInt32(tmpmas[1]));
            	var TCPStream = Client.GetStream();

            	var readdata = new byte[256];
            	while (Client.Connected)
            	{
                	try
                	{
                    	var ReadStr = "";
                    	while (TCPStream.DataAvailable)
                    	{
                        	var bytes = TCPStream.Read(readdata, 0, readdata.Length);
                        	ReadStr += Encoding.UTF8.GetString(readdata, 0, bytes);
                    	}

                    	lock (LockObject)
                    	{
                        	if (ReadStr != "")
                            	MsgReadList.Enqueue(ReadStr);
                        	while (MsgWriteList.Count > 0)
                        	{
                            	var data = Encoding.UTF8.GetBytes(MsgWriteList.Dequeue());
                            	TCPStream.Write(data, 0, data.Length);
                        	}
                        	if (ExitCommand)
                            	break;
                    	}
                    	Thread.Sleep(1);
                	}
                	catch (Exception e)
                	{
                    	Log.SendError("[WCNetwork]: " + e.Message);
                	}
            	}
            	TCPStream.Close();
            	Client.Close();
        	}
            catch (Exception e)
            {
                    	Log.SendError("[WCNetwork]: " + e.Message);
            }
        }
    }
}
