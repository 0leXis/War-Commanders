using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    enum CapturePointStates { NEUTRAL, ALLIED, ENEMY }

    class CapturePointInfo : IDrawable
    {
        private BasicSprite Sprite;
        private BasicText TextObj;

        public Texture2D Allied_Texture { get; set; }
        public Texture2D Enemy_Texture { get; set; }
        public Texture2D Neutral_Texture { get; set; }

        public Vector2 Position
        {
            get
            {
                return Sprite.Position;
            }
            set
            {
                Sprite.Position = value;
                TextObj.Position = Position + (new Vector2(Neutral_Texture.Width, Neutral_Texture.Height) - TextObj.Font.MeasureString(TextObj.Text)) / 2;
            }
        }

        public string Text
        {
            get
            {
                return TextObj.Text;
            }
            set
            {
                TextObj.Text = value;
                TextObj.Position = Position + (new Vector2(Neutral_Texture.Width, Neutral_Texture.Height) - TextObj.Font.MeasureString(TextObj.Text)) / 2;
            }
        }

        public bool Visible { get; set; }

        public CapturePointInfo(Vector2 Position, Texture2D Allied_Texture, Texture2D Enemy_Texture, Texture2D Neutral_Texture, SpriteFont Font, string Text, float Layer = BasicSprite.DefaultLayer)
        {
            this.Allied_Texture = Allied_Texture;
            this.Enemy_Texture = Enemy_Texture;
            this.Neutral_Texture = Neutral_Texture;

            Sprite = new BasicSprite(Position, Neutral_Texture, Layer);
            TextObj = new BasicText(Position + (new Vector2(Neutral_Texture.Width, Neutral_Texture.Height) - Font.MeasureString(Text)) / 2, Text, Font, Color.Black, Layer - 0.001f);
            Visible = true;
        }

        public void SetState(CapturePointStates State)
        {
            switch (State)
            {
                case CapturePointStates.ALLIED:
                    Sprite.Texture = Allied_Texture;
                    break;
                case CapturePointStates.ENEMY:
                    Sprite.Texture = Enemy_Texture;
                    break;
                default:
                    Sprite.Texture = Neutral_Texture;
                    break;
            }
        }

        public void Draw(SpriteBatch Target)
        {
            if (Visible)
            {
                Sprite.Draw(Target);
                TextObj.Draw(Target);
            }
        }
    }
}
