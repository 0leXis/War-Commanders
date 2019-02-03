using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class FlyingText : BasicText, IDrawable
    {
        private bool _IsShown;
        private Vector2 AddVect;
        private int TickCount;
        private int iteration;
        private Vector2 BaseScale;

        public bool IsShown
        {
            get
            {
                return _IsShown;
            }
        }

        public FlyingText(Vector2 Position, string Text, SpriteFont Font, Color color, Vector2 BaseScale, float Layer = BasicSprite.DefaultLayer) : base(Position, Text, Font, color, Layer)
        {
            this.BaseScale = BaseScale;
            Visible = false;
        }

        public void Show(Vector2 StartPosition, Vector2 StopPosition, int TickCount)
        {
            _IsShown = true;
            Visible = true;
            Scale = Vector2.Zero;
            Position = StartPosition;
            AddVect = new Vector2((StopPosition.X - StartPosition.X) / TickCount, (StopPosition.Y - StartPosition.Y) / TickCount);
            this.TickCount = TickCount;
            iteration = 0;
        }

        public void Update()
        {
            if (IsShown)
            {
                if (iteration < 10)
                    Scale += BaseScale / 10;
                else
                if (iteration < 10 + TickCount)
                    Position += AddVect;
                else
                if (iteration < 20 + TickCount)
                    Scale -= BaseScale / 10;
                else
                {
                    Visible = false;
                    _IsShown = false;
                }
                    iteration++;
            }
        }
    }
}
