using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    public enum MenuState { HOME, PLAY, COLLECTION, PACK_OPEN }

    class MainMenu : IDrawable
    {
        MenuState State;

        Collection collection;
        Play play;

        BasicSprite BackGround;
        public LogInForm LogIn { get; set; }

        BasicSprite MenuBar;
        Button Home;
        Button Play;
        Button Collection;
        Button OpenCardPack;
        Button Options;

        BasicText PlayerName;
        BasicSprite NameBack;
        BasicSprite MoneyBack;
        BasicText PlayerMoney;

        RollBack RollingBack;

        bool IsLoginState;

        NetworkInterface MasterNI;

        public MainMenu
            (Vector2 ScreenRes, 
            Texture2D LogInBackGroundTexture, Texture2D LogInButtonTexture, 
            Texture2D LogInEditTexture, Texture2D ConnectingIconTexture,
            Texture2D MenuBarTexture, Texture2D ButtonTexture, Texture2D HomeButtonTexture, Texture2D MoneyBackTexture,
            Texture2D RollingBackTexture, Texture2D NameBackTexture,
            SpriteFont Font, Color TextColor, GraphicsDevice Graphicsdevice, float Layer = BasicSprite.DefaultLayer
            )
        {
            LogIn = new LogInForm((ScreenRes - new Vector2(LogInBackGroundTexture.Width, LogInBackGroundTexture.Height)) / 2, LogInBackGroundTexture, LogInButtonTexture, LogInEditTexture, ConnectingIconTexture, Font, TextColor, Graphicsdevice, Layer - 0.001f);

            MenuBar = new BasicSprite(new Vector2(0, 50), MenuBarTexture, Layer + 0.0005f);
            Home = new Button(MenuBar.Position, HomeButtonTexture, HomeButtonTexture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer);
            Play = new Button(new Vector2(Home.Position.X + Home.FrameSize.X, MenuBar.Position.Y), ButtonTexture, "Играть", Font, TextColor, HomeButtonTexture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer);
            Collection = new Button(new Vector2(Play.Position.X + Play.FrameSize.X, MenuBar.Position.Y), ButtonTexture, "Коллекция", Font, TextColor, HomeButtonTexture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer);
            OpenCardPack = new Button(new Vector2(Collection.Position.X + Collection.FrameSize.X, MenuBar.Position.Y), ButtonTexture, "Наборы карт", Font, TextColor, HomeButtonTexture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer);
            Options = new Button(new Vector2(OpenCardPack.Position.X + OpenCardPack.FrameSize.X, MenuBar.Position.Y), ButtonTexture, "Настройки/Выход", Font, TextColor, HomeButtonTexture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer);

            NameBack = new BasicSprite(new Vector2(0), NameBackTexture, Layer);
            PlayerName = new BasicText(new Vector2(NameBack.Position.X + 10, NameBack.Position.Y + 5), "", Font, TextColor, Layer - 0.0005f);
            MoneyBack = new BasicSprite(new Vector2(ScreenRes.X - MoneyBackTexture.Width, 0), MoneyBackTexture, Layer);
            PlayerMoney = new BasicText(new Vector2(MoneyBack.Position.X + 50, MoneyBack.Position.Y + 5), "", Font, TextColor, Layer - 0.0005f);

            RollingBack = new RollBack(new Vector2(0, 100), new Vector2(0, ScreenRes.Y), 20, RollingBackTexture, Layer + 0.0005f);

            IsLoginState = true;

            MasterNI = new NetworkInterface();
            CommandParser.InitMasterServer(MasterNI);

            State = MenuState.HOME;

            collection = new Collection(ScreenRes, Color.Black, new bool[] { }, new bool[] { }, 200, Layer - 0.0005f);
            play = new Play();
        }

        public void LockClicking()
        {
            Home.Enabled = false;
            Play.Enabled = false;
            Collection.Enabled = false;
            OpenCardPack.Enabled = false;
            Options.Enabled = false;
            LogIn.Hide(false);
        }

        public void UnlockClicking()
        {
            if(IsLoginState)
                LogIn.Show(false);

            Home.Enabled = true;
            Play.Enabled = true;
            Collection.Enabled = true;
            OpenCardPack.Enabled = true;
            Options.Enabled = true;
        }

        public void Update(GameMenu menu)
        {
            if (IsLoginState)
            {
                if (LogIn.Update(MasterNI) == 1)
                {
                    IsLoginState = false;
                    PlayerName.Text = LogIn.PlayerName;
                }
            }
            else
            {
                if (Home.Update() == ButtonStates.CLICKED)
                {
                     if(State != MenuState.HOME)
                     {
                        if (RollingBack.Position != RollingBack.DownPosition)
                        {
                            RollingBack.SetDown();
                        }
                        if (State == MenuState.COLLECTION)
                            collection.Hide();
                        else
                        if (State == MenuState.PLAY)
                            play.Hide();
                        State = MenuState.HOME;
                     }
                }
                if (Play.Update() == ButtonStates.CLICKED)
                {
                    if (State != MenuState.PLAY)
                    {
                        RollingBack.SetUp();
                        if (State == MenuState.COLLECTION)
                            collection.Hide();
                        play.Show();
                        State = MenuState.PLAY;
                    }
                }
                if (Collection.Update() == ButtonStates.CLICKED)
                {
                    if(State != MenuState.COLLECTION)
                    {
                        if (State == MenuState.PLAY)
                            play.Hide();
                        RollingBack.SetUp();
                        State = MenuState.COLLECTION;
                    }
                }
                if (OpenCardPack.Update() == ButtonStates.CLICKED)
                {
                //    RollingBack.SetUp();
                //    if (State == MenuState.COLLECTION)
                //        collection.Hide();
                }
                if (Options.Update() == ButtonStates.CLICKED)
                {
                    menu.Show(null, null, this);
                    if (State == MenuState.COLLECTION)
                        collection.Hide();
                    else
                        if (State == MenuState.PLAY)
                            play.Hide();
                }

                if(State == MenuState.COLLECTION)
                {
                    if (!RollingBack.IsMoving && !collection.IsShown)
                        collection.Show(1, true);
                }
            }
            RollingBack.Update();
            collection.Update();
            play.Update();
        }

        public void Draw(SpriteBatch Target)
        {
            Target.Begin(SpriteSortMode.BackToFront);
                LogIn.Draw(Target);

                MenuBar.Draw(Target);
                Home.Draw(Target);
                Play.Draw(Target);
                Collection.Draw(Target);
                OpenCardPack.Draw(Target);
                Options.Draw(Target);

                PlayerName.Draw(Target);
                NameBack.Draw(Target);
                MoneyBack.Draw(Target);
                PlayerMoney.Draw(Target);

                RollingBack.Draw(Target);

                collection.Draw(Target);
            Target.End();
        }
    }
}
