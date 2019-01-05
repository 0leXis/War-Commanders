using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class BtnInfo : BasicSprite, IDrawable
    {
        private BasicText _Text;

        private int iteration;
        private Vector2 LastScale;
        private bool IsAppearing = false;

        new public Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                var VectTmp = _Text.Font.MeasureString(_Text.Text);
                _Text.Position = new Vector2(value.X + Texture.Width / 2 * Scale.X - VectTmp.X / 2, value.Y + Texture.Height / 2 * Scale.Y - VectTmp.Y / 2);
            }
        }

        new public Vector2 Scale
        {
            get
            {
                return base.Scale;
            }
            set
            {
                _Text.Scale = value;
                base.Scale = _Text.Font.MeasureString(_Text.Text) * _Text.Scale / new Vector2(Texture.Width, Texture.Height) + new Vector2(0.01f, 0.01f);
            }
        }

        public string Text
        {
            get
            {
                return _Text.Text;
            }
            set
            {
                _Text.Text = value;
                base.Scale = _Text.Font.MeasureString(_Text.Text) * _Text.Scale / new Vector2(Texture.Width, Texture.Height) + new Vector2(0.01f, 0.01f);
            }
        }

        new public bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                _Text.Visible = value;
                base.Visible = value;
            }
        }

        public BtnInfo(Vector2 Position, Texture2D Texture, SpriteFont Font, Color TextColor, string InfoText, float Layer = DefaultLayer) : base(Position, Texture, Layer)
        {
            var VectTmp = Font.MeasureString(InfoText);
            _Text = new BasicText(new Vector2(Position.X + Texture.Width / 2 - VectTmp.X / 2, Position.Y + Texture.Height / 2 - VectTmp.Y / 2), InfoText, Font, TextColor, Layer - 0.0001f);
            base.Scale = _Text.Font.MeasureString(_Text.Text) * _Text.Scale / (Scale * new Vector2(Texture.Width, Texture.Height));
            _Text.Position = new Vector2(Position.X + Texture.Width / 2 * Scale.X - VectTmp.X / 2, Position.Y + Texture.Height / 2 * Scale.Y - VectTmp.Y / 2);
            iteration = 0;
        }

        public void Appear()
        {
            if (!IsAppearing)
            {
                IsAppearing = true;
                iteration = 0;
            }
        }

        public void Disappear()
        {
            if (IsAppearing)
            {
                IsAppearing = false;
                Visible = false;
                iteration = 50;
            }
        }

        public void Update()
        {
            if (iteration < 50)
            {
                if (IsAppearing && iteration == 49)
                {
                    Visible = true;
                }
                iteration++;
            }
        }

        public override void Draw(SpriteBatch Target)
        {
            base.Draw(Target);
            _Text.Draw(Target);
        }
    }
}
