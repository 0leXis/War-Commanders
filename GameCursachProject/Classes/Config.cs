using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace GameCursachProject
{
    static class Config
    {
        static public readonly string[] ResolutionTypes = new string[] { "1280x720", "1920x1080" };
        static public readonly Point[] Resolutions = new Point[] { new Point(1280, 720), new Point(1920, 1080) };

        static public byte CurrResolution = 0;
        static public bool FullScreen = false;

        static public string ServerIP = "127.0.0.1:9080";
        public const string SettingsPath = "Config.xml";
        static public void LoadConfigFile()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(SettingsPath);
            XmlElement xRoot = xDoc.DocumentElement;
            var IP = "";
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Name == "IP")
                {
                    IP = xnode.InnerText;
                }
            }
            if (IP != "")
                ServerIP = IP;
        }

        static public void SaveConfigFile()
        {

        }
    }
}
