using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class SimpleRectangle
    {
        private Point _Position;
        private Point _WidthHeight;
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
            }
        }

        public Vector2 WidthHeight
        {
            get
            {
                return _WidthHeight.ToVector2();
            }
            set
            {
                _WidthHeight = value.ToPoint();
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
                _Visible = value;
            }
        }

        public SimpleRectangle(Vector2 Position, Vector2 WidthHeight, Color LineColor, Color RectColor, GraphicsDevice GrDevice, float Layer = BasicSprite.DefaultLayer)
        {
            RectTexture = new Texture2D(GrDevice, 1, 1);
            RectTexture.SetData(new Color[1] { RectColor });
            LineTexture = new Texture2D(GrDevice, 1, 1);
            LineTexture.SetData(new Color[1] { LineColor });
            this.Position = Position;
            this.WidthHeight = WidthHeight;
            this.Layer = Layer;
        }

        public void Draw(SpriteBatch Target)
        {
            if (Visible)
            {
                Target.Draw(RectTexture, new Rectangle(_Position, _WidthHeight), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, Layer);
                Target.Draw(LineTexture, new Rectangle(_Position, new Point(_WidthHeight.X, 1)), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, Layer - 0.0001f);
                Target.Draw(LineTexture, new Rectangle(new Point(_Position.X + _WidthHeight.X, _Position.Y), new Point(1, _WidthHeight.Y + 1)), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, Layer - 0.0001f);
                Target.Draw(LineTexture, new Rectangle(new Point(_Position.X, _Position.Y + _WidthHeight.Y), new Point(_WidthHeight.X, 1)), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, Layer - 0.0001f);
                Target.Draw(LineTexture, new Rectangle(_Position, new Point(1, _WidthHeight.Y)), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, Layer - 0.0001f);
            }
        }
    }
}
