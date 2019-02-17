using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class ListBox : IDrawable
    {
        public delegate void ChangeHandler(ListBox Sender);

        public event ChangeHandler OnChange;

        private BasicText Text;
        private List<Button> Variants;
        private Button OpenButton;
        private BasicSprite ChoosedVariant;

        private bool IsOpen;
        private byte _Choosed;

        public byte Choosed
        {
            get
            {
                return _Choosed;
            }
            set
            {
                if(value != _Choosed && value < Variants.Count)
                {
                    _Choosed = value;
                    Text.Text = Variants[value].Text.Text;
                    OnChange(this);
                }
            }
        }

        public Vector2 Position
        {
            get
            {
                return OpenButton.Position;
            }
            set
            {
                OpenButton.Position = value;
                ChoosedVariant.Position = Position + new Vector2(OpenButton.FrameSize.X, 0);
                Text.Position = ChoosedVariant.Position + (new Vector2(ChoosedVariant.Texture.Width, ChoosedVariant.Texture.Height) - Text.Font.MeasureString(Text.Text)) / 2;
                for (var i = 0; i < Variants.Count; i++)
                {
                    Variants[i].Position = new Vector2(ChoosedVariant.Position.X, ChoosedVariant.Position.Y + ChoosedVariant.Texture.Height + Variants[i].Texture.Height * i);
                }
            }
        }

        public bool Visible
        {
            get
            {
                return OpenButton.Visible;
            }
            set
            {
                OpenButton.Visible = value;
                ChoosedVariant.Visible = value;
                Text.Visible = value;
                if (IsOpen)
                {
                    for (var i = 0; i < Variants.Count; i++)
                    {
                        Variants[i].Visible = value;
                    }

                }
            }
        }

        public ListBox(Vector2 Position, Texture2D VariantTexture, Texture2D ChoosedVariantTexture, Texture2D ButtonTexture, SpriteFont Font, Color TextColor, string[] Variants, float Layer = BasicSprite.DefaultLayer)
        {
            OpenButton = new Button(Position, ButtonTexture, ButtonTexture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer);
            ChoosedVariant = new BasicSprite(Position + new Vector2(OpenButton.FrameSize.X, 0), ChoosedVariantTexture, Layer);
            var TmpStr = "";
            if (Variants.Length > 0)
                TmpStr = Variants[0];
            Text = new BasicText(ChoosedVariant.Position + (new Vector2(ChoosedVariant.Texture.Width, ChoosedVariant.Texture.Height) - Font.MeasureString(TmpStr)) / 2, TmpStr, Font, TextColor, Layer - 0.0005f);

            this.Variants = new List<Button>();
            for(var i = 0; i < Variants.Length; i++)
            {
                this.Variants.Add(new Button(new Vector2(ChoosedVariant.Position.X, ChoosedVariant.Position.Y + ChoosedVariant.Texture.Height + VariantTexture.Height * i), VariantTexture, Variants[i], Font, TextColor, VariantTexture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer));
                this.Variants[i].Visible = false;
            }
        }

        //public void AddVariant()
        //{

        //}

        //public void RemoveVariant()
        //{

        //}

        public bool Update()
        {
            var Upd = OpenButton.Update();

            var ret = false;
            if (Upd != ButtonStates.NONE)
                ret = true;

            if (Upd == ButtonStates.CLICKED)
            {
                if (IsOpen)
                    Close();
                else
                    Open();
            }

            if (IsOpen)
            {
                for (var i = 0; i < Variants.Count; i++)
                {
                    Upd = Variants[i].Update();
                    if (Upd != ButtonStates.NONE)
                        ret = true;
                    if (Upd == ButtonStates.CLICKED)
                    {
                        Close();
                        Choosed = (byte)i;
                        break;
                    }
                }
            }

            if (MouseControl.IsRightBtnClicked)
            {
                Close();
            }
            return ret;
        }

        private void Open()
        {
            IsOpen = true;
            foreach(var variant in Variants)
            {
                variant.Visible = true;
            }
        }

        private void Close()
        {
            IsOpen = false;
            foreach (var variant in Variants)
            {
                variant.Visible = false;
            }
        }

        public void Draw(SpriteBatch Target)
        {
            OpenButton.Draw(Target);
            ChoosedVariant.Draw(Target);
            Text.Draw(Target);
            foreach (var variant in Variants)
            {
                variant.Draw(Target);
            }
        }
    }
}
