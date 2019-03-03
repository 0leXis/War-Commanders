using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace GameCursachProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int ScreenHeight, ScreenWidth;

        GameState gameState;
        MainMenu mainMenu;
        GameMenu Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.AllowUserResizing = false;//DONE: Онли в меню настройки
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            ContentLoader.Init(Content);
        }
        
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Log.EnableConsoleLog = true;
            //Log.EnableFileLog = true;

            ScreenWidth = Window.ClientBounds.Width;
            ScreenHeight = Window.ClientBounds.Height;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GameContent.LoadGameContent();

            //mainMenu = new MainMenu(new Vector2(ScreenWidth, ScreenHeight), GameContent.UI_MainMenu_LogIn_BackGround, GameContent.UI_MainMenu_LogIn_Button, GameContent.UI_MainMenu_LogIn_EditBox, GameContent.UI_MainMenu_LogIn_ConnIcon, GameContent.UI_MainMenu_MenuBar, GameContent.UI_MainMenu_Button, GameContent.UI_MainMenu_HomeButton, GameContent.UI_InfoFont, Color.Black, GraphicsDevice, 0.1f);
            gameState = new GameState(ScreenWidth, ScreenHeight, GraphicsDevice);

            KeyBindings.RegisterKeyBind("KEY_MENU", Keys.Escape);
            Menu = new GameMenu(new Vector2(ScreenWidth, ScreenHeight), this, GameContent.UI_GameMenu_MainBack, GameContent.UI_GameMenu_OptionsBack, GameContent.UI_GameMenu_Button, GameContent.UI_GameMenu_ListBoxBtn, GameContent.UI_GameMenu_ListBoxChoosed, GameContent.UI_GameMenu_ListBoxOpenBtn, GameContent.UI_InfoFont, Color.Black, 0.1f);
            Menu.Hide(null, gameState);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            if (gameState != null)
                gameState.Close();

            Log.EnableConsoleLog = false;
            //Log.EnableFileLog = false;
        }

        public void ApplyChanges()
        {
            if (graphics.IsFullScreen)
            {
                graphics.IsFullScreen = false;
                graphics.ApplyChanges();
            }
            graphics.PreferredBackBufferWidth = Config.Resolutions[Config.CurrResolution].X;
            graphics.PreferredBackBufferHeight = Config.Resolutions[Config.CurrResolution].Y;
            graphics.ApplyChanges();
            graphics.IsFullScreen = Config.FullScreen;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Window.ClientBounds.Width != ScreenWidth || Window.ClientBounds.Height != ScreenHeight)
            {
                ScreenWidth = Window.ClientBounds.Width; // TODO: ON RESIZE
                ScreenHeight = Window.ClientBounds.Height;
                if (gameState != null)
                    gameState.Resize(ScreenWidth, ScreenHeight);
            }

            MouseControl.Update();
            KeyBindings.Update();
            if (mainMenu != null)
                mainMenu.Update();
            if (gameState != null)
            {
                Menu.Update(gameState.UI, gameState);
                gameState.Update(Menu);
            }
            else
            {
                Menu.Update(null, gameState);
            }

            //Window.Title = gameState.watch.Elapsed.ToString() + "|--|" + Convert.ToString(Mouse.GetState().X) + "||" + Convert.ToString(Mouse.GetState().Y) + "|ZOOM|" + gameState.cam.Zoom.ToString() + "|POS|" + gameState.cam.Position.ToString();
            Window.Title = "|--|" + Convert.ToString(Mouse.GetState().X) + "||" + Convert.ToString(Mouse.GetState().Y);

            base.Update(gameTime);
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if(mainMenu != null)
                mainMenu.Draw(spriteBatch);
            if (gameState != null)
                gameState.Draw(spriteBatch);
            spriteBatch.Begin(SpriteSortMode.BackToFront);
                Menu.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
