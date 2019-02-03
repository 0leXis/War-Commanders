using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    static class FlyingTextProcessor
    {
        static private List<FlyingText> flyingTexts = new List<FlyingText>();

        static public void CreateText(Vector2 StartPosition, Vector2 StopPosition, int TickCount, string Text, SpriteFont Font, Color color, Vector2 BaseScale, float Layer = BasicSprite.DefaultLayer)
        {
            flyingTexts.Add(new FlyingText(StartPosition, Text, Font, color, BaseScale, Layer));
            flyingTexts[flyingTexts.Count - 1].Show(StartPosition, StopPosition, TickCount);
        }

        static public void Update()
        {
            for(var i = 0; i < flyingTexts.Count; i++)
            {
                if (!flyingTexts[i].IsShown)
                {
                    flyingTexts.RemoveAt(i);
                    i--;
                }
                else
                    flyingTexts[i].Update();
            }
        }

        static public void Draw(SpriteBatch Target)
        {
            foreach (var flt in flyingTexts)
                flt.Draw(Target);
        }
    }
}
