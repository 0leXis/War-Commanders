using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class GameMenu : IDrawable
    {
        //BackGround
        private BasicSprite MainBack;
        private BasicSprite OptionsBack;

        //Main
        private BasicText MenuCaption;
        private Button Continue;
        private Button Surrender;
        private Button Options;
        private Button Quit;

        //Options
        private BasicText ScreenOptions;
        private ListBox Resolutions;
        private Button ScreenMode;

        private bool IsOptions;
        private bool _IsShown;
        private Vector2 _ScreenRes;

        private Game1 Parent;

        public bool IsShown { get { return _IsShown; } }

        public Vector2 ScreenRes
        {
            get
            {
                return _ScreenRes;
            }
            set
            {
                _ScreenRes = value;
                //TODO: Resize
            }
        }

        public GameMenu(Vector2 ScreenRes, Game1 Parent, Texture2D MainBackGround, Texture2D OptionsBackGround, Texture2D BtnTexture, Texture2D Variant, Texture2D ChoosedVariant, Texture2D OpenBtn, SpriteFont Font, Color TextColor, float Layer = BasicSprite.DefaultLayer)
        {
            this.Parent = Parent;

            MainBack = new BasicSprite((ScreenRes - new Vector2(MainBackGround.Width, MainBackGround.Height)) / 2, MainBackGround, Layer);

            var Offset = new Vector2((ScreenRes.X - BtnTexture.Width / 4) / 2, ((float)MainBack.Texture.Height - BtnTexture.Height * 4) / 5);
            var OffsetY = new Vector2(0, Offset.Y + BtnTexture.Height);

            MenuCaption = new BasicText(new Vector2(MainBack.Position.X + (MainBack.Texture.Width - Font.MeasureString("Меню").X) / 2, MainBack.Position.Y + 10), "Меню", Font, Color.Black, Layer - 0.0005f);
            Continue = new Button(new Vector2(Offset.X, MainBack.Position.Y + Offset.Y), BtnTexture, "Продолжить", Font, TextColor, BtnTexture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.0005f);
            Surrender = new Button(Continue.Position + OffsetY, BtnTexture, "Сдаться", Font, TextColor, BtnTexture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.0005f);
            Options = new Button(Surrender.Position + OffsetY, BtnTexture, "Настройки", Font, TextColor, BtnTexture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.0005f);
            Quit = new Button(Options.Position + OffsetY, BtnTexture, "Выход", Font, TextColor, BtnTexture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.0005f);

            OptionsBack = new BasicSprite((ScreenRes - new Vector2(MainBackGround.Width, MainBackGround.Height)) / 2, OptionsBackGround, Layer);
            ScreenOptions = new BasicText(OptionsBack.Position + new Vector2(20, 20), "Настройки экрана:", Font, TextColor, Layer - 0.0005f);
            Resolutions = new ListBox(ScreenOptions.Position + new Vector2(0, 30), Variant, ChoosedVariant, OpenBtn, Font, TextColor, Config.ResolutionTypes, Layer - 0.001f);
            ScreenMode = new Button(Resolutions.Position + new Vector2(0, 30), BtnTexture, "Оконный", Font, TextColor, BtnTexture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.0005f);
            OptionsBack.Visible = false;
            ScreenOptions.Visible = false;
            Resolutions.Visible = false;
            ScreenMode.Visible = false;

            if(Config.FullScreen)
                ScreenMode.Text = "Полноэкранный";
            else
                ScreenMode.Text = "Оконный";

            Resolutions.Choosed = Config.CurrResolution;
            Resolutions.OnChange += ResolutionChanged;
        }

        public void Show(MainUI UI, GameState State)
        {
            _IsShown = true;
            MenuCaption.Visible = true;
            MainBack.Visible = true;
            Continue.Visible = true;
            Surrender.Visible = true;
            Options.Visible = true;
            Quit.Visible = true;

            if(UI != null)
                UI.DisableUI();
            if (State != null)
                State.SetEnemyTurn();
        }

        public void Hide(MainUI UI, GameState State)
        {
            _IsShown = false;
            MenuCaption.Visible = false;
            MainBack.Visible = false;
            Continue.Visible = false;
            Surrender.Visible = false;
            Options.Visible = false;
            Quit.Visible = false;

            if (UI != null)
                UI.EnableUI();
            if (State != null)
                if(State.IsPlayerTurn)
                    State.SetPlayerTurn();
        }

        private void ChangeOptionsState()
        {
            if (IsOptions)
            {
                IsOptions = false;
                OptionsBack.Visible = false;
                ScreenOptions.Visible = false;
                Resolutions.Visible = false;
                ScreenMode.Visible = false;

                MenuCaption.Visible = true;
                MainBack.Visible = true;
                Continue.Visible = true;
                Surrender.Visible = true;
                Options.Visible = true;
                Quit.Visible = true;
            }
            else
            {
                IsOptions = true;
                OptionsBack.Visible = true;
                ScreenOptions.Visible = true;
                Resolutions.Visible = true;
                ScreenMode.Visible = true;

                MenuCaption.Visible = false;
                MainBack.Visible = false;
                Continue.Visible = false;
                Surrender.Visible = false;
                Options.Visible = false;
                Quit.Visible = false;
            }
        }

        public void ResolutionChanged(ListBox Sender)
        {
            Config.CurrResolution = Sender.Choosed;
            Parent.ApplyChanges();
        }

        public void Update(MainUI UI, GameState State)
        {
            if (KeyBindings.CheckKeyReleased("KEY_MENU"))
            {
                if (IsOptions)
                {
                    ChangeOptionsState();
                }
                else
                {
                    if (_IsShown)
                        Hide(UI, State);
                    else
                        Show(UI, State);
                }
            }

            if (_IsShown)
            {
                if (IsOptions)
                {
                    if(Resolutions.Update() == false)
                        if (ScreenMode.Update() == ButtonStates.CLICKED)
                        {
                            if (Config.FullScreen)
                            {
                                Config.FullScreen = false;
                                ScreenMode.Text = "Оконный";
                            }
                            else
                            {
                                Config.FullScreen = true;
                                ScreenMode.Text = "Полноэкранный";
                            }
                            Parent.ApplyChanges();
                        }
                }
                else
                {
                    if (Continue.Update() == ButtonStates.CLICKED)
                    {
                        Hide(UI, State);
                    }
                    else
                    if (Surrender.Update() == ButtonStates.CLICKED)
                    {
                        //TODO: Surrender
                    }
                    else
                    if (Options.Update() == ButtonStates.CLICKED)
                    {
                        ChangeOptionsState();
                    }
                    else
                    if (Quit.Update() == ButtonStates.CLICKED)
                    {
                        Parent.Exit();
                    }
                }
            }
        }

        public void Draw(SpriteBatch Target)
        {
            MainBack.Draw(Target);
            OptionsBack.Draw(Target);

            MenuCaption.Draw(Target);
            Continue.Draw(Target);
            Surrender.Draw(Target);
            Options.Draw(Target);
            Quit.Draw(Target);

            ScreenOptions.Draw(Target);
            Resolutions.Draw(Target);
            ScreenMode.Draw(Target);
        }

    }
}
