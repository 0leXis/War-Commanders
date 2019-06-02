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
    public enum GlobalGameState { MainMenu, Game }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GlobalGameState _GlobalState;
        public GlobalGameState GlobalState
        {
            get
            {
                return _GlobalState;
            }
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int ScreenHeight, ScreenWidth;

        GameState gameState;
        MainMenu mainMenu;
        GameMenu Menu;

        ScreenBr LoadingBr;
        int LoadingIterations;
        bool IsLoadToGame;
        bool IsLoadToMenu;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.AllowUserResizing = false;//DONE: Онли в меню настройки
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.SynchronizeWithVerticalRetrace = true;
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
            Log.SendMessage("CLR: " + Environment.Version);
            Log.SendMessage("Machine Name: " + Environment.MachineName);
            Log.SendMessage("RAM for process (bytes): " + Environment.WorkingSet);
            Log.SendMessage("Number of processors: " + Environment.ProcessorCount);
            Log.SendMessage("OS x64: " + Environment.Is64BitOperatingSystem);
            Log.SendMessage("Process x64: " + Environment.Is64BitProcess);
            Log.SendMessage("OS: " + Environment.OSVersion.VersionString);
            //Log.EnableFileLog = true;

            Config.LoadConfigFile();

            ScreenWidth = Window.ClientBounds.Width;
            ScreenHeight = Window.ClientBounds.Height;

            LoadingBr = new ScreenBr(new Vector2(ScreenWidth, ScreenHeight), 60, 255, GraphicsDevice, 0.001f);
            LoadingBr.CurrentFrame = 59;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GameContent.LoadGameContent();
            _GlobalState = GlobalGameState.MainMenu;
            mainMenu = new MainMenu(new Vector2(ScreenWidth, ScreenHeight), GameContent.UI_MainMenu_LogIn_BackGround, GameContent.UI_MainMenu_LogIn_Button, GameContent.UI_MainMenu_LogIn_EditBox, GameContent.UI_MainMenu_LogIn_ConnIcon, GameContent.UI_MainMenu_MenuBar, GameContent.UI_MainMenu_Button, GameContent.UI_MainMenu_HomeButton, GameContent.UI_MainMenu_MoneyBack, GameContent.UI_MainMenu_RollBack, GameContent.UI_MainMenu_NameBack, GameContent.UI_InfoFont, Color.Black, GraphicsDevice, this, 0.1f);
            //gameState = new GameState(ScreenWidth, ScreenHeight, GraphicsDevice);

            KeyBindings.RegisterKeyBind("KEY_MENU", Keys.Escape);
            Menu = new GameMenu(new Vector2(ScreenWidth, ScreenHeight), this, GameContent.UI_GameMenu_MainBack, GameContent.UI_GameMenu_OptionsBack, GameContent.UI_GameMenu_Button, GameContent.UI_GameMenu_ListBoxBtn, GameContent.UI_GameMenu_ListBoxChoosed, GameContent.UI_GameMenu_ListBoxOpenBtn, GameContent.UI_InfoFont, Color.Black, 0.1f);
            Menu.Hide(null, gameState, null);
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
            if (mainMenu != null && _GlobalState == GlobalGameState.MainMenu)
            {
                Menu.Update(null, null, mainMenu);
                mainMenu.Update(Menu);
            }
            if (gameState != null && _GlobalState == GlobalGameState.Game)
            {
                Menu.Update(gameState.UI, gameState, mainMenu);
                gameState.Update(Menu);
            }
            else
            {
                Menu.Update(null, gameState, mainMenu);
            }

            //Window.Title = gameState.watch.Elapsed.ToString() + "|--|" + Convert.ToString(Mouse.GetState().X) + "||" + Convert.ToString(Mouse.GetState().Y) + "|ZOOM|" + gameState.cam.Zoom.ToString() + "|POS|" + gameState.cam.Position.ToString();
            Window.Title = "|--|" + Convert.ToString(Mouse.GetState().X) + "||" + Convert.ToString(Mouse.GetState().Y);

            LoadingBr.UpdateAnims();
            if (IsLoadToGame)
            {
                LoadingIterations++;
                if(LoadingIterations == 60)
                {
                    _GlobalState = GlobalGameState.Game;
                    LoadingBr.ScreenBrUp();
                }
                if (LoadingIterations == 120)
                    IsLoadToGame = false;
            }
            if (IsLoadToMenu)
            {
                LoadingIterations++;
                if (LoadingIterations == 60)
                {
                    _GlobalState = GlobalGameState.MainMenu;
                    mainMenu.ReturnFromGame();
                    gameState = null;
                    LoadingBr.ScreenBrUp();
                }
                if (LoadingIterations == 120)
                    IsLoadToMenu = false;
            }
            base.Update(gameTime);
        }
        
        public void CreateGame(NetworkInterface NI)
        {
            gameState = new GameState(NI, this, ScreenWidth, ScreenHeight, GraphicsDevice);
            LoadingIterations = 0;
            IsLoadToGame = true;
            LoadingBr.ScreenBrDown();
        }

        public void EndGame()
        {
            LoadingIterations = 0;
            IsLoadToMenu = true;
            LoadingBr.ScreenBrDown();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if(mainMenu != null && _GlobalState == GlobalGameState.MainMenu)
                mainMenu.Draw(spriteBatch);
            if (gameState != null && _GlobalState == GlobalGameState.Game)
                gameState.Draw(spriteBatch);
            spriteBatch.Begin(SpriteSortMode.BackToFront);
                Menu.Draw(spriteBatch);
                LoadingBr.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
