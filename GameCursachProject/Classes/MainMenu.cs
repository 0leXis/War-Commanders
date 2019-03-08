using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class MainMenu : IDrawable
    {
        BasicSprite BackGround;
        public LogInForm LogIn { get; set; }

        BasicSprite MenuBar;
        Button Home;
        Button Play;
        Button Collection;
        Button OpenCardPack;
        Button Options;

        bool IsLoginState;

        NetworkInterface MasterNI;

        public MainMenu
            (Vector2 ScreenRes, 
            Texture2D LogInBackGroundTexture, Texture2D LogInButtonTexture, 
            Texture2D LogInEditTexture, Texture2D ConnectingIconTexture,
            Texture2D MenuBarTexture, Texture2D ButtonTexture, Texture2D HomeButtonTexture,
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

            IsLoginState = true;

            MasterNI = new NetworkInterface();
            CommandParser.InitMasterServer(MasterNI);
        }

        public void Update()
        {
            if (IsLoginState)
            {
                if (LogIn.Update(MasterNI) == 1)
                {
                    IsLoginState = false;

                }
            }
            else
            {
                Home.Update();
                Play.Update();
                Collection.Update();
                OpenCardPack.Update();
                Options.Update();
            }
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
            Target.End();
        }
    }
}
