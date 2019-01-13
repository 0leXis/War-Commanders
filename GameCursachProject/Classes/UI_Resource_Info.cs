using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class UI_Resource_Info : IDrawable
    {
        private SimpleRectangle _Rect;
        private BasicText _MainText;
        private BasicText _AdditionalText;
        private BasicSprite _Icon;

        private Intersector intersect;
        private Vector2 _Scale;
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

                _Icon.Position = new Vector2(Position.X + 5, Position.Y + 2);
                _MainText.Position = new Vector2(Position.X + 5 + _Icon.Texture.Width * Scale.X, Position.Y + 2);
                var textwidth = _MainText.Font.MeasureString(MainText) * _MainText.Scale + new Vector2(_Icon.Texture.Width * Scale.X, 0);
                _AdditionalText.Position = new Vector2(Position.X + 5 + textwidth.X, Position.Y + 2);
                intersect = new Intersector(new Rectangle(_Rect.Position.ToPoint(), _Rect.WidthHeight.ToPoint()));
            }
        }

        public Vector2 WidthHeight
        {
            get
            {
                return _Rect.WidthHeight;
            }
        }

        public Vector2 Scale
        {
            get
            {
                return _Scale;
            }
            set
            {
                _Scale = value;
                _MainText.Scale = value;
                _AdditionalText.Scale = value;
                _Icon.Scale = value;

                _MainText.Position = new Vector2(Position.X + 5 + _Icon.Texture.Width * Scale.X, Position.Y + 2);
                var textwidth = _MainText.Font.MeasureString(MainText) * _MainText.Scale + new Vector2(_Icon.Texture.Width * Scale.X, 0);
                _AdditionalText.Position = new Vector2(Position.X + 5 + textwidth.X, Position.Y + 2);
                textwidth = new Vector2(textwidth.X + _AdditionalText.Font.MeasureString(AdditionalText).X * _AdditionalText.Scale.X, textwidth.Y);
                _Rect.WidthHeight = textwidth + new Vector2(9, 3);
                intersect = new Intersector(new Rectangle(_Rect.Position.ToPoint(), _Rect.WidthHeight.ToPoint()));
            }
        }

        public string MainText
        {
            get
            {
                return _MainText.Text;
            }
            set
            {
                _MainText.Text = value;

                _MainText.Position = new Vector2(Position.X + 5 + _Icon.Texture.Width * Scale.X, Position.Y + 2);
                var textwidth = _MainText.Font.MeasureString(MainText) * _MainText.Scale + new Vector2(_Icon.Texture.Width * Scale.X, 0);
                _AdditionalText.Position = new Vector2(Position.X + 5 + textwidth.X, Position.Y + 2);
                textwidth = new Vector2(textwidth.X + _AdditionalText.Font.MeasureString(AdditionalText).X * _AdditionalText.Scale.X, textwidth.Y);
                _Rect.WidthHeight = textwidth + new Vector2(9, 3);
                intersect = new Intersector(new Rectangle(_Rect.Position.ToPoint(), _Rect.WidthHeight.ToPoint()));
            }
        }

        public string AdditionalText
        {
            get
            {
                return _AdditionalText.Text;
            }
            set
            {
                _AdditionalText.Text = value;

                _MainText.Position = new Vector2(Position.X + 5 + _Icon.Texture.Width * Scale.X, Position.Y + 2);
                var textwidth = _MainText.Font.MeasureString(MainText) * _MainText.Scale + new Vector2(_Icon.Texture.Width * Scale.X, 0);
                _AdditionalText.Position = new Vector2(Position.X + 5 + textwidth.X, Position.Y + 2);
                textwidth = new Vector2(textwidth.X + _AdditionalText.Font.MeasureString(AdditionalText).X * _AdditionalText.Scale.X, textwidth.Y);
                _Rect.WidthHeight = textwidth + new Vector2(9, 3);
                intersect = new Intersector(new Rectangle(_Rect.Position.ToPoint(), _Rect.WidthHeight.ToPoint()));
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
                _MainText.Visible = value;
                _AdditionalText.Visible = value;
                _Icon.Visible = value;
                _Rect.Visible = value;
                _Visible = value;
            }
        }

        public UI_Resource_Info(Vector2 Position, Color LineColor, Color RectColor, SpriteFont Font, Texture2D Icon, Color TextColor, Color AddTextColor, string MainText, string AddText, GraphicsDevice GrDevice, float Layer = BasicSprite.DefaultLayer)
        {
            _Icon = new BasicSprite(new Vector2(Position.X + 5, Position.Y + 2), Icon, Layer - 0.0001f);
            _MainText = new BasicText(new Vector2(Position.X + 5 + Icon.Width, Position.Y + 2), MainText, Font, TextColor, Layer - 0.0001f);
            var textwidth = _MainText.Font.MeasureString(MainText) * _MainText.Scale + new Vector2(Icon.Width, 0);
            _AdditionalText = new BasicText(new Vector2(Position.X + 5 + textwidth.X, Position.Y + 2), AddText, Font, AddTextColor, Layer - 0.0001f);
            textwidth = new Vector2(textwidth.X + _AdditionalText.Font.MeasureString(AddText).X * _AdditionalText.Scale.X, textwidth.Y);
            _Rect = new SimpleRectangle(Position, textwidth + new Vector2(9, 3), LineColor, RectColor, GrDevice, Layer);
            intersect = new Intersector(new Rectangle(_Rect.Position.ToPoint(), _Rect.WidthHeight.ToPoint()));
            this.Position = Position;
            Scale = new Vector2(1);
            this.Layer = Layer;
            Visible = true;
        }

        public ButtonStates Update()
        {
            if(Visible && intersect.IntersectionCheck(new Vector2(MouseControl.X, MouseControl.Y)))
            {
                return ButtonStates.ENTERED;
            }
            return ButtonStates.NONE;
        }

        public void Draw(SpriteBatch Target)
        {
            if (Visible)
            {
                _Rect.Draw(Target);
                _Icon.Draw(Target);
                _MainText.Draw(Target);
                _AdditionalText.Draw(Target);
            }
        }
    }
}
