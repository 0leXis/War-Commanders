using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;
using System.Diagnostics; //ДЫБУГ


namespace GameCursachProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public const float LAYER_MAP = 0.5f;
        public const float LAYER_UI_FAR = 0.4f;
        public const float LAYER_CARDS = 0.3f;
        public const float LAYER_UI = 0.2f;
		
        int laststate;
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Map Map;
        Hand Hand;
        MainUI UI;

        Camera cam;

        Stopwatch watch; //Дыбуг
        static public Texture2D TankTexture;
        static public Texture2D UInfoTexture;
        static public SpriteFont UInfoFont;

        int ScreenHeight, ScreenWidth;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.AllowUserResizing = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
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
            this.IsMouseVisible = true;
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
            watch = new Stopwatch();//ДЫБГ
            var MapArr = new Tile[8][];
            var Rnd = new Random();
            for(var i = 0; i< 8; i++)
            {
                MapArr[i] = new Tile[15];
                for (var j = 0; j < 15; j++)
                {
                    if (j % 2 == 0)
                        MapArr[i][j] = new Tile(new Vector2(j * 294, i * 339), Content.Load<Texture2D>(@"Textures\PoleTile"), Content.Load<Texture2D>(@"Textures\PoleTileHighlited"), 392, 20, 0, new Animation(1, 1, true), 1, Rnd.Next(1, 5), LAYER_MAP);
                    else
                        if (i != MapArr.GetLength(0) - 1)
                            MapArr[i][j] = new Tile(new Vector2(j * 294, i * 339 + 169.5f), Content.Load<Texture2D>(@"Textures\PoleTile"), Content.Load<Texture2D>(@"Textures\PoleTileHighlited"), 392, 20, 0, new Animation(1, 1, true), 1, Rnd.Next(1, 5), LAYER_MAP);
                        else
                            MapArr[i][j] = null;
                }
            }
            Map = new Map(MapArr, 0, 1, new Animation(1, 1, true), Content.Load<Texture2D>(@"Textures\ArrowSegment"), Content.Load<Texture2D>(@"Textures\ArrowEnd"), Content.Load<Texture2D>(@"Textures\TileHaracts"), Content.Load<SpriteFont>(@"Fonts\TileInfoFont"), Color.White);
            //foreach (var Til in Map)
            //    if (Til != null)
            //    {
            //        Til.Scale = new Vector2(2, 2);
            //        Til.Position = new Vector2(Til.Position.X * Til.Scale.X, Til.Position.Y * Til.Scale.Y);
            //    }
            char a = Convert.ToChar(12);
            var Arr = new Card[10];
            Arr[0] = new Card(new Vector2(100, 300), Content.Load<Texture2D>(@"Textures\Card"), Content.Load<Texture2D>(@"Textures\Tiger"), new Vector2(16, 9), 200, 10, 0, new Animation(0, 0, true), new Animation(2, 6, false), new Animation(7, 9, false), new Animation(1, 1, true), 0, Content.Load<SpriteFont>(@"Fonts\TileInfoFont"), Color.White, "Pz. VI H \"Tiger\"", "3", "1", "5", "5", "6", 141, 315, 4, 4, 37, false, Layer: LAYER_CARDS)
            {
                Scale = new Vector2(0.5f, 0.5f)
            };
            for (var i = 1; i < Arr.Length; i++)
            {
                Arr[i] = new Card(new Vector2(100 + i * 60, 300), Content.Load<Texture2D>(@"Textures\Card"), Content.Load<Texture2D>(@"Textures\Tiger"), new Vector2(16, 9), 200, 10, 0, new Animation(0, 0, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, Content.Load<SpriteFont>(@"Fonts\TileInfoFont"), Color.White, "Pz. VI H \"Tiger\"", "3", "1", "5", "5", "6", 141, 315, 4, 4, 37, true, Layer: LAYER_CARDS + 0.001f * i)
                {
                    Scale = new Vector2(0.5f, 0.5f),
                    AllowedZones = MapZones.RIGHT
                };
            }
            Hand = new Hand(new Vector2(ScreenWidth / 2, ScreenHeight - 175), Arr, new Vector2(ScreenWidth, ScreenHeight), Content.Load<Texture2D>(@"Textures\ArrowSegment"), Content.Load<Texture2D>(@"Textures\ArrowEnd"));

            TankTexture = Content.Load<Texture2D>(@"Textures\TankPrototype");
            UInfoTexture = Content.Load<Texture2D>(@"Textures\UnitHaracts");
            UInfoFont = Content.Load<SpriteFont>(@"Fonts\TileInfoFont");

            UI = new MainUI(new Vector2(ScreenWidth, ScreenHeight), Content.Load<Texture2D>(@"Textures\UI_BtnInfo"), Content.Load<Texture2D>(@"Textures\UI_Main_Bottom"), Content.Load<Texture2D>(@"Textures\UI_Main_Bottom_Left"), Content.Load<Texture2D>(@"Textures\BtnNewTurn"), Content.Load<Texture2D>(@"Textures\BtnMove"), Content.Load<Texture2D>(@"Textures\BtnAttack"), Content.Load<Texture2D>(@"Textures\BtnMenu"),Content.Load<Texture2D>(@"Textures\BtnChat"), Content.Load<Texture2D>(@"Textures\BtnStats"), Content.Load<SpriteFont>(@"Fonts\ButtonFont"), LAYER_UI_FAR);
            // TODO: use this.Content to load your game content here
            cam = new Camera(new Vector2(ScreenWidth, ScreenHeight));
            cam.Zoom = 0.25f;
            laststate = 0;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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
                Hand.CurrentScreenRes = new Vector2(ScreenWidth, ScreenHeight);
                UI.CurrentScreenRes = Hand.CurrentScreenRes;
                cam.ScreenRes = new Vector2(ScreenWidth, ScreenHeight);
            }
            MouseControl.Update();

            UpdateGameObjects();

            //Window.Title = Convert.ToString(Hand.Cards[0].Layer) + " " + Convert.ToString(Hand.Cards[1].Layer) + " " + Convert.ToString(Hand.Cards[2].Layer);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //Window.Title = Convert.ToString(Mouse.GetState().X) + "||" + Convert.ToString(Mouse.GetState().Y);
            //Window.Title = Convert.ToString(ScreenWidth) + "||" + Convert.ToString(ScreenHeight);
            //Window.Title = Convert.ToString(Mouse.GetState().X) + "||" + Convert.ToString(Mouse.GetState().Y) + "|---|" + Convert.ToString(arrow.EndPoint.X) + "||" + Convert.ToString(arrow.EndPoint.Y) + "|---|" + Convert.ToString(arrow._Rotation);
            base.Update(gameTime);
            watch.Stop();
            Window.Title = watch.Elapsed.ToString() + "|--|" + Convert.ToString(Mouse.GetState().X) + "||" + Convert.ToString(Mouse.GetState().Y) + "|ZOOM|" + cam.Zoom.ToString() + "|POS|" + cam.Position.ToString();
            //if (MouseControl.IsLeftBtnClicked)
            //    Window.Title = "Left";
            //if (MouseControl.IsRightBtnClicked)
            //    Window.Title = "Right";
            //Window.Title = Hand.IsClick.ToString();
            //Window.Title = Map.Tiles[0][0].NotSelectedFrame.ToString() + " || " + Map.Tiles[1][0].NotSelectedFrame.ToString();

            /*PathFindingTest*/
//            var Til = Map.GetTileIJByCoords(new Vector2(MouseControl.X, MouseControl.Y));
//            List<Point> LastList = null;
//            if (Til != null)
//            {
//                int PL;
//                var TmpList = Map.PathFinding(1, 0, Til[0], Til[1], out PL);
//                Map.CreatePathArrows(TmpList);
//                if (TmpList != LastList)
//                {
//                    LastList = new List<Point>(TmpList);
//                    Window.Title = PL.ToString();
//                }
//            }
            /*PathFindingTest End*/
            watch.Reset();
            UpdateCamera();
            //cam.Position += new Vector2(0.5f, 0.5f);
            //cam.Zoom = 0.25f;
            //cam.Rotation -= 0.01f;
        }

        private void UpdateCamera()
        {
            var state = Mouse.GetState();
            if (state.ScrollWheelValue - laststate < 0)
            {
                cam.Zoom += (state.ScrollWheelValue - laststate) * 0.001f * cam.Zoom;
            }
            else
            if (state.ScrollWheelValue - laststate > 0)
            {
                cam.Zoom += (state.ScrollWheelValue - laststate) * 0.001f * cam.Zoom;
            }
            laststate = state.ScrollWheelValue;

            var state2 = Keyboard.GetState();
            if (state2.IsKeyDown(Keys.W))
                cam.Position -= new Vector2(0, 10f / cam.Zoom);
            else
            if (state2.IsKeyDown(Keys.A))
                cam.Position -= new Vector2(10f / cam.Zoom, 0);
            else
            if (state2.IsKeyDown(Keys.S))
                cam.Position += new Vector2(0, 10f / cam.Zoom);
            else
            if (state2.IsKeyDown(Keys.D))
                cam.Position += new Vector2(10f / cam.Zoom, 0);

            var maprectoffs = Map.GetMapRectangle(1);
            var maprect = Map.GetMapRectangle(0);
            var FirstVect = new Vector2(maprectoffs.X, maprectoffs.Y);
            var LastVect = new Vector2(maprectoffs.X + maprectoffs.Width, maprectoffs.Y + maprectoffs.Height);
            var LastMapVect = new Vector2(maprect.X + maprect.Width, maprect.Y + maprect.Height);
            if (cam.Position.X < FirstVect.X)
                cam.Position = new Vector2(FirstVect.X, cam.Position.Y);
            if (cam.Position.X > LastVect.X)
                cam.Position = new Vector2(, cam.Position.Y);
            if (cam.Position.Y < FirstVect.Y)
                cam.Position = new Vector2(cam.Position.X, FirstVect.Y);
            if (cam.Position.Y> LastVect.Y)
                cam.Position = new Vector2(cam.Position.X, );
        }

        private void UpdateGameObjects()
        {
            var IsMouseHandled = false;
            Hand.Update(ref IsMouseHandled, Map, cam);
            UI.Update(ref IsMouseHandled, Map);
            Map.Update(ref IsMouseHandled, Hand, cam);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //UpdThread.Wait();
            watch.Start();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, cam.GetTransform(graphics.GraphicsDevice));
                Map.Draw(spriteBatch, cam);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.BackToFront);
                UI.Draw(spriteBatch);
                Hand.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
