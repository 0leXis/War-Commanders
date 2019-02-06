﻿using System;
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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.AllowUserResizing = true;//TODO: Онли в меню настройки
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

            gameState = new GameState(ScreenWidth, ScreenHeight, GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            gameState.Close();

            Log.EnableConsoleLog = false;
            //Log.EnableFileLog = false;
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
                gameState.Resize(ScreenWidth, ScreenHeight);
            }
            gameState.Update();
            Window.Title = gameState.watch.Elapsed.ToString() + "|--|" + Convert.ToString(Mouse.GetState().X) + "||" + Convert.ToString(Mouse.GetState().Y) + "|ZOOM|" + gameState.cam.Zoom.ToString() + "|POS|" + gameState.cam.Position.ToString();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            gameState.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
