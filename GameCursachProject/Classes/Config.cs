using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    static class Config
    {
        static public readonly string[] ResolutionTypes = new string[] { "1280x720", "1920x1080" };
        static public readonly Point[] Resolutions = new Point[] { new Point(1280, 720), new Point(1920, 1080) };

        static public byte CurrResolution = 0;
        static public bool FullScreen = false;

        static public void LoadConfigFile()
        {

        }

        static public void SaveConfigFile()
        {

        }
    }
}
