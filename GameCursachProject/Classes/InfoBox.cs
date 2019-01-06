using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class InfoBox : IDrawable
    {
        private BasicText _Text;

        private int iteration;
        private bool IsAppearing = false;
        private Point _Position;
        private Vector2 _Scale;
        private bool _Visible;
        private float Layer;
        private Texture2D RectTexture;
        private Texture2D LineTexture;

        public Vector2 Position
        {
            get
            {
                return _Position.ToVector2();
            }
            set
            {
                _Position = value.ToPoint();
                var VectTmp = _Text.Font.MeasureString(_Text.Text);
                _Text.Position = new Vector2(Position.X + 5, Position.Y + 2);
            }
        }

        public Vector2 Scale
        {
            get
            {
                return _Text.Scale;
            }
            set
            {
                _Text.Scale = value;
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
            }
        }

        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Text.Visible = value;
                _Visible = value;
            }
        }

        public InfoBox(Vector2 Position, Color LineColor, Color RectColor, SpriteFont Font, Color TextColor, string InfoText, GraphicsDevice GrDevice, float Layer = BasicSprite.DefaultLayer)
        {
            var VectTmp = Font.MeasureString(InfoText);

            RectTexture = new Texture2D(GrDevice, 1, 1);
            RectTexture.SetData(new Color[1] { RectColor });
            LineTexture = new Texture2D(GrDevice, 1, 1);
            LineTexture.SetData(new Color[1] { LineColor });

            _Text = new BasicText(new Vector2(Position.X + 5, Position.Y + 2), InfoText, Font, TextColor, Layer - 0.0001f);
            this.Position = Position;
            Scale = new Vector2(1);
            this.Layer = Layer;
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

        public void Draw(SpriteBatch Target)
        {
            if (Visible)
            {
                var textwidth = _Text.Font.MeasureString(_Text.Text).ToPoint();
                Target.Draw(RectTexture, new Rectangle(_Position, textwidth + new Point(9, 3)), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, Layer);
                Target.Draw(LineTexture, new Rectangle(_Position, new Point(textwidth.X + 9, 1)), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, _Text.Layer);
                Target.Draw(LineTexture, new Rectangle(new Point(_Position.X + textwidth.X + 9, _Position.Y), new Point(1, textwidth.Y + 4)), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, _Text.Layer);
                Target.Draw(LineTexture, new Rectangle(new Point(_Position.X, _Position.Y + textwidth.Y + 3), new Point(textwidth.X + 9, 1)), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, _Text.Layer);
                Target.Draw(LineTexture, new Rectangle(_Position, new Point(1, textwidth.Y + 3)), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, _Text.Layer);
                _Text.Draw(Target);
            }
        }
    }
}
