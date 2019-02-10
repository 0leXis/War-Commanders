using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class ScalingText : BasicText, IDrawable
    {
        public int Ticks { get; set; }

        private bool Appearing;
        private bool Disappearing;
        private int iteration;
        private Vector2 Scaling;

        public ScalingText(Vector2 Position, string Text, SpriteFont Font, Color color, int Ticks, float Layer = BasicSprite.DefaultLayer) : base(Position, Text, Font, color, Layer)
        {
            this.Ticks = Ticks;
        }

        public void Appear()
        {
            if (!Appearing)
            {
                Visible = true;
                Appearing = true;
                Disappearing = false;
                iteration = 0;
                Scaling = new Vector2((float)1 / Ticks);
                Scale = Vector2.Zero;
            }
        }

        public void Disappear()
        {
            if (!Disappearing)
            {
                Appearing = false;
                Disappearing = true;
                iteration = 0;
                Scaling = new Vector2((float)1 / Ticks);
                Scale = Vector2.One;
            }
        }

        public void Update()
        {
            if(iteration < Ticks)
            {
                if (Appearing)
                {
                    Scale += Scaling;
                }
                else
                if (Disappearing)
                {
                    Scale -= Scaling;
                }
                iteration++;
                if(iteration == Ticks && Disappearing)
                {
                    Visible = false;
                }
            }
        }

    }
}
