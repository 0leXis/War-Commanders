using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameCursachProject
{
    class EditBox : Button
    {
        public int MaxLength { get; set; }
        public bool IsMasked { get; set; }

        private bool Focused;
        private string RealText;

        new public string Text
        {
            get
            {
                return RealText;
            }
            set
            {
                RealText = value;
                base.Text = value;
            }
        }

        public EditBox(Vector2 Position, Texture2D Texture, string Text, SpriteFont Font, Color TextColor, int MaxLength, bool IsMasked, float Layer) : base(Position, Texture, Text, Font, TextColor, Texture.Width, 60, 0, new Animation(0,0,false), 0, 0, Layer)
        {
            this.MaxLength = MaxLength;
            this.IsMasked = IsMasked;
            RealText = Text;
        }

        public void Update()
        {
            var Upd = base.Update();
            if (Visible)
            {
                if (MouseControl.IsLeftBtnClicked)
                {
                    if (Upd == ButtonStates.CLICKED)
                    {
                        if (!Focused)
                        {
                            Focused = true;
                            base.Text += "|";
                        }
                    }
                    else
                    if (Focused)
                    {
                        Focused = false;
                        base.Text = base.Text.Remove(base.Text.Length - 1);
                    }
                }

                if (Focused)
                {
                    var IsKeyFinded = false;
                    string Symbol = "";

                    if (RealText.Length < MaxLength)
                    {
                        for (var i = 48; i < 58; i++)
                        {
                            if (KeyBindings.CheckKeyReleased((Keys)i) == true)
                            {
                                IsKeyFinded = true;
                                Symbol = char.ConvertFromUtf32(i);
                                break;
                            }
                        }
                        if (!IsKeyFinded)
                            for (var i = 65; i < 91; i++)
                            {
                                if (KeyBindings.CheckKeyReleased((Keys)i) == true)
                                {
                                    IsKeyFinded = true;
                                    Symbol = char.ConvertFromUtf32(i);
                                    break;
                                }
                            }
                        if (!IsKeyFinded)
                            for (var i = 96; i < 106; i++)
                            {
                                if (KeyBindings.CheckKeyReleased((Keys)i) == true)
                                {
                                    IsKeyFinded = true;
                                    Symbol = char.ConvertFromUtf32(i - 48);
                                    break;
                                }
                            }
                        if (IsKeyFinded)
                        {
                            base.Text = base.Text.Remove(base.Text.Length - 1);
                            if (!(KeyBindings.CheckKeyPressed(Keys.LeftShift) || KeyBindings.CheckKeyPressed(Keys.RightShift)))
                            {
                                Symbol = Symbol.ToLower();
                            }
                            RealText += Symbol;
                            if (IsMasked)
                            {
                                base.Text += "*";
                            }
                            else
                            {
                                base.Text += Symbol;
                            }
                            base.Text += "|";
                        }
                    }

                    if (KeyBindings.CheckKeyReleased(Keys.Back))
                    {
                        base.Text = base.Text.Remove(base.Text.Length - 1);
                        if (base.Text.Length > 0)
                        {
                            base.Text = base.Text.Remove(base.Text.Length - 1);
                        }
                        if (RealText.Length > 0)
                        {
                            RealText = RealText.Remove(base.Text.Length - 1);
                        }
                        base.Text += "|";
                    }
                }
            }
        }
    }
}
