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
        private SimpleRectangle _Rect;

        private int iteration;
        private bool IsAppearing = false;
        private Point _Position;
        private bool _Visible;
        private float Layer;

        public Vector2 Position
        {
            get
            {
                return _Position.ToVector2();
            }
            set
            {
                _Position = value.ToPoint();
                _Rect.Position = Position;
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
                var textwidth = _Text.Font.MeasureString(_Text.Text) * _Text.Scale;
                _Rect.WidthHeight = textwidth + new Vector2(9, 3);
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
                var textwidth = _Text.Font.MeasureString(_Text.Text) * _Text.Scale;
                _Rect.WidthHeight = textwidth + new Vector2(9, 3);
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
                _Rect.Visible = value;
                _Visible = value;
            }
        }

        public InfoBox(Vector2 Position, Color LineColor, Color RectColor, SpriteFont Font, Color TextColor, string InfoText, GraphicsDevice GrDevice, float Layer = BasicSprite.DefaultLayer)
        {
            _Text = new BasicText(new Vector2(Position.X + 5, Position.Y + 2), InfoText, Font, TextColor, Layer - 0.0001f);
            var textwidth = _Text.Font.MeasureString(_Text.Text) * _Text.Scale;
            _Rect = new SimpleRectangle(Position, textwidth + new Vector2(9, 3), LineColor, RectColor, GrDevice, Layer);
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
                _Rect.Draw(Target);
                _Text.Draw(Target);
            }
        }
    }
}
