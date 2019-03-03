using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics; //ДЫБУГ

namespace GameCursachProject
{
    class GameState
    {
        public NetworkInterface NI;

        public const float LAYER_MAP = 0.5f;
        public const float LAYER_UI_FAR = 0.4f;
        public const float LAYER_CARDS = 0.3f;
        public const float LAYER_UI = 0.2f;

        int laststate;

        Map Map;
        Hand Hand;
        public MainUI UI;
        public Camera cam;

        public Stopwatch watch; //Дыбуг

        public Script UnitAttEngine;

        public bool IsPlayerTurn;
        public bool IsNotLockClick;

        public MapZones PlayerSide = MapZones.RIGHT;
        public MapZones OpponentSide = MapZones.LEFT;

        private GraphicsDevice gr_Dev;

        public GameState(int ScreenWidth, int ScreenHeight, GraphicsDevice gr_Device)
        {
            gr_Dev = gr_Device;

            NI = new NetworkInterface();
            while (!NI.IsConnected)
            {
                NI.ConnectTo(CommandParser.ServerIP);
            }
            Log.SendMessage("Подключено");
            CommandParser.Init(NI);
            int[][] mass = null;
            while (true)
            {
                var msg = NI.GetNextMsg();
                if (msg != null)
                {
                    var masss = (mass as object);
                    Serialization.DeSerialize(msg, ref masss, typeof(int[][]));
                    mass = masss as int[][];
                    break;
                }
            }

            Log.SendMessage("Ожидание команды начала игры");

            var player1_name = "";
            var player2_name = "";
            var player_money = "";
            string[] tmpmsg = null;
            int[] Cards = null;
            while (true)
            {
                var msg = NI.GetNextMsg();
                if (msg != null)
                {

                    var masss = (tmpmsg as object);
                    Serialization.DeSerialize(msg, ref masss, typeof(string[]));
                    tmpmsg = masss as string[];

                    player1_name = tmpmsg[0];
                    player2_name = tmpmsg[1];
                    player_money = tmpmsg[2];
                    PlayerSide = (tmpmsg[3] == "RIGHT") ? MapZones.RIGHT : MapZones.LEFT;
                    OpponentSide = (PlayerSide == MapZones.LEFT) ? MapZones.RIGHT : MapZones.LEFT;
                    Cards = new int[tmpmsg.Length - 4];
                    for (var i = 4; i < tmpmsg.Length; i++)
                    {
                        Cards[i - 4] = Convert.ToInt32(tmpmsg[i]);
                    }
                    break;
                }
            }

            watch = new Stopwatch();//ДЫБГ
            var MapArr = new Tile[8][];
            var Rnd = new Random();
            for (var i = 0; i < 8; i++)
            {
                MapArr[i] = new Tile[15];
                for (var j = 0; j < 15; j++)
                {
                    if (j % 2 == 0)
                        MapArr[i][j] = new Tile(new Vector2(j * 294, i * 339), GameContent.TileBorder, GameContent.TileBorder_HL, GameContent.TileTypes[mass[i][j]].Tile_Decoration, 392, 20, 0, new Animation(1, 1, true), 1, GameContent.TileTypes[mass[i][j]].SpeedNeeded, GameContent.TileTypes[mass[i][j]].Name, LAYER_MAP);
                    else
                        if (i != MapArr.GetLength(0) - 1)
                        MapArr[i][j] = new Tile(new Vector2(j * 294, i * 339 + 169.5f), GameContent.TileBorder, GameContent.TileBorder_HL, GameContent.TileTypes[mass[i][j]].Tile_Decoration, 392, 20, 0, new Animation(1, 1, true), 1, GameContent.TileTypes[mass[i][j]].SpeedNeeded, GameContent.TileTypes[mass[i][j]].Name, LAYER_MAP);
                    else
                        MapArr[i][j] = null;
                }
            }


            Map = new Map(MapArr, 0, 1, new Animation(1, 1, true), GameContent.ArrowSegment, GameContent.ArrowEnd, GameContent.UI_TileInfo, GameContent.Tile_ControlPoint_Neutral, GameContent.Tile_ControlPoint_Allied, GameContent.Tile_ControlPoint_Enemy, GameContent.Unit_AttackRadius, GameContent.UI_InfoFont, Color.White, new Point[] { new Point(0, 7), new Point(3, 7), new Point(6, 7) });
            //foreach (var Til in Map)
            //    if (Til != null)
            //    {
            //        Til.Scale = new Vector2(2, 2);
            //        Til.Position = new Vector2(Til.Position.X * Til.Scale.X, Til.Position.Y * Til.Scale.Y);
            //    }
            char a = Convert.ToChar(12);
            //var Arr = new Card[10];
            //Arr[0] = new Card(new Vector2(100, 300), Content.Load<Texture2D>(@"Textures\Card"), Content.Load<Texture2D>(@"Textures\Tiger"), new Vector2(16, 9), 200, 10, 0, 13, new Animation(0, 0, true), new Animation(2, 6, false), new Animation(7, 9, false), new Animation(1, 1, true), 0, Content.Load<SpriteFont>(@"Fonts\TileInfoFont"), Color.White, "Pz. VI H \"Tiger\"", "3", "1", "5", "5", "6", 141, 315, 4, 4, 37, false, Layer: LAYER_CARDS)
            //{
            //    Scale = new Vector2(0.5f, 0.5f)
            //};
            //for (var i = 1; i < Arr.Length; i++)
            //{
            //    Arr[i] = new Card(new Vector2(100 + i * 60, 300), Content.Load<Texture2D>(@"Textures\Card"), Content.Load<Texture2D>(@"Textures\Tiger"), new Vector2(16, 9), 200, 10, 0, 13, new Animation(0, 0, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, Content.Load<SpriteFont>(@"Fonts\TileInfoFont"), Color.White, "Pz. VI H \"Tiger\"", "3", "1", "5", "5", "6", 141, 315, 4, 4, 37, true, Layer: LAYER_CARDS + 0.001f * i)
            //    {
            //        Scale = new Vector2(0.5f, 0.5f),
            //        AllowedZones = MapZones.RIGHT
            //    };
            //}
            Hand = new Hand(new Vector2(ScreenWidth / 2, ScreenHeight - 175), null, new Vector2(ScreenWidth, ScreenHeight), GameContent.ArrowSegment, GameContent.ArrowEnd, LAYER_CARDS);

            UI = new MainUI
                (
                new Vector2(ScreenWidth, ScreenHeight), GameContent.UI_Main_Bottom,
                GameContent.UI_Main_Bottom_Left, GameContent.UI_Main_Up,
                GameContent.UI_Main_Up_Left, GameContent.UI_Main_Up_Right,
                GameContent.UI_Btn_NewTurn, GameContent.UI_Btn_Move,
                GameContent.UI_Btn_Attack, GameContent.UI_Btn_Menu,
                GameContent.UI_Btn_Chat, GameContent.UI_Btn_Stats,
                GameContent.UI_Player_Icons[0], GameContent.UI_Player_Icons[0],
                GameContent.UI_Flag_Player, GameContent.UI_Flag_Enemy,
                GameContent.UI_Money, GameContent.UI_HourGlass,
                GameContent.UI_Vs, GameContent.UI_AlliedPoint,
                GameContent.UI_EnemyPoint, GameContent.UI_NeutralPoint, GameContent.UI_EnemyTurn,
                GameContent.UI_ButtonFont,
                GameContent.UI_MiniFont,
                GameContent.UI_NewTurnFont,
                gr_Device, this,
                player1_name, player2_name,
                "0", "1", "0", "1", "100",
                player_money, "1",
                "1:30", new string[] { "A", "B", "C" },
                Cards, (Cards.Length == 3) ? true : false,
                LAYER_UI_FAR);
            // TODO: use this.Content to load your game content here
            cam = new Camera(new Vector2(ScreenWidth, ScreenHeight));
            cam.Zoom = 0.25f;
            laststate = 0;

            KeyBindings.Init();
            KeyBindings.RegisterKeyBind("KEY_MOVEUNIT", Keys.M);
            KeyBindings.RegisterKeyBind("KEY_ATTACKUNIT", Keys.F);
            KeyBindings.RegisterKeyBind("KEY_STATS", Keys.T);
            KeyBindings.RegisterKeyBind("KEY_CAMMOVE_UP", Keys.W);
            KeyBindings.RegisterKeyBind("KEY_CAMMOVE_DOWN", Keys.S);
            KeyBindings.RegisterKeyBind("KEY_CAMMOVE_LEFT", Keys.A);
            KeyBindings.RegisterKeyBind("KEY_CAMMOVE_RIGHT", Keys.D);

            UnitAttEngine = new Script("", "GameCursachProject", false);

            //TEST
            //Map.GetTile(4, 6).SpawnUnit(new Unit(Vector2.Zero, GameContent.UnitTextures[0], GameContent.UI_Info_Enemy, GameContent.UI_InfoFont, Color.White, 392, 20, 5, 3, 6, 1, 2, Side.OPPONENT, GameContent.UnitAttackScripts[0], UnitAttEngine, new Point(4, 6), new Animation(8, 17, false), 0.4f), MapZones.RIGHT, Map.UI_VisibleState);
            //TEST
            SetEnemyTurn();
        }

        public void Update(GameMenu Menu)
        {

            UpdateGameObjects(Menu);

            //Window.Title = Convert.ToString(Hand.Cards[0].Layer) + " " + Convert.ToString(Hand.Cards[1].Layer) + " " + Convert.ToString(Hand.Cards[2].Layer);

            // TODO: Add your update logic here
            //Window.Title = Convert.ToString(Mouse.GetState().X) + "||" + Convert.ToString(Mouse.GetState().Y);
            //Window.Title = Convert.ToString(ScreenWidth) + "||" + Convert.ToString(ScreenHeight);
            //Window.Title = Convert.ToString(Mouse.GetState().X) + "||" + Convert.ToString(Mouse.GetState().Y) + "|---|" + Convert.ToString(arrow.EndPoint.X) + "||" + Convert.ToString(arrow.EndPoint.Y) + "|---|" + Convert.ToString(arrow._Rotation);
            watch.Stop();
            //Window.Title = CoordsConvert.WindowToWorldCoords(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), cam).ToString();
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

            string[] CN;
            CommandParser.Update(out CN);
            if (CN != null)
            {
                if (CN[0] == "REPLACE")
                {
                    if(CN.Length == 1)
                    {
                        UI.ReplaceCards(null);
                    }
                    else
                    {
                        var CardsToReplace = new int[CN.Length - 1];
                        for (var i = 1; i < CN.Length; i++)
                        {
                            CardsToReplace[i - 1] = Convert.ToInt32(CN[i]);
                        }
                        UI.ReplaceCards(CardsToReplace);
                    }
                }
                if (CN[0] == "START")
                {
                    UI.IterationReset();
                }
                if (CN[0] == "TURN")
                {
                    if(CN[1] == "ENEMY")
                    {
                        UI.SetEnemyTurn();
                        IsPlayerTurn = false;
                    }
                    else
                    {
                        var CardsToChoose = new int[CN.Length - 2];
                        for (var i = 2; i < CN.Length; i++)
                        {
                            CardsToChoose[i - 2] = Convert.ToInt32(CN[i]);
                        }
                        UI.SetPlayerTurn(Convert.ToInt32(CN[1]), CardsToChoose);
                        IsPlayerTurn = true;
                    }
                }
                if (CN[0] == "CHOOSE")
                {
                    if(CN[1] == "OK")
                    {
                        UI.ChooseCards(Hand);
                    }
                }
                if (CN[0] == "SPAWN")
                {
                    if(CN[1] == "ALLIED")
                    {
                        Map.GetTile(Convert.ToInt32(CN[3]), Convert.ToInt32(CN[4])).SpawnUnit(new Unit(Vector2.Zero, GameContent.UnitCards[Convert.ToInt32(CN[2])].UnitTexture, GameContent.UI_Info_Allied, GameContent.UI_InfoFont, Color.White, 392, 20, GameContent.UnitCards[Convert.ToInt32(CN[2])].Speed, GameContent.UnitCards[Convert.ToInt32(CN[2])].Damage, GameContent.UnitCards[Convert.ToInt32(CN[2])].HP, GameContent.UnitCards[Convert.ToInt32(CN[2])].Armor, GameContent.UnitCards[Convert.ToInt32(CN[2])].AttackRadius, Side.PLAYER, GameContent.UnitCards[Convert.ToInt32(CN[2])].UnitAttackScript, UnitAttEngine, new Point(Convert.ToInt32(CN[3]), Convert.ToInt32(CN[4])), new Animation(8, 17, false), 0.4f), PlayerSide, Map.UI_VisibleState);
                    }
                    else
                    {
                        Map.GetTile(Convert.ToInt32(CN[3]), Convert.ToInt32(CN[4])).SpawnUnit(new Unit(Vector2.Zero, GameContent.UnitCards[Convert.ToInt32(CN[2])].UnitTexture, GameContent.UI_Info_Enemy, GameContent.UI_InfoFont, Color.White, 392, 20, GameContent.UnitCards[Convert.ToInt32(CN[2])].Speed, GameContent.UnitCards[Convert.ToInt32(CN[2])].Damage, GameContent.UnitCards[Convert.ToInt32(CN[2])].HP, GameContent.UnitCards[Convert.ToInt32(CN[2])].Armor, GameContent.UnitCards[Convert.ToInt32(CN[2])].AttackRadius, Side.OPPONENT, GameContent.UnitCards[Convert.ToInt32(CN[2])].UnitAttackScript, UnitAttEngine, new Point(Convert.ToInt32(CN[3]), Convert.ToInt32(CN[4])), new Animation(8, 17, false), 0.4f), (PlayerSide == MapZones.RIGHT) ? MapZones.LEFT : MapZones.RIGHT, Map.UI_VisibleState);
                    }
                }
                if (CN[0] == "MOVE")
                {
                    if (IsPlayerTurn)
                        SetPlayerTurn();

                }
                //if(CN[0] == "2")
                //{
                //	//Map.GetTile(Convert.ToInt32(CN[1]), Convert.ToInt32(CN[2])).SpawnUnit(new Unit(Vector2.Zero, GameContent.UnitTextures[0], GameContent.UI_Info_Allied, GameContent.UI_InfoFont, Color.White, 392, 20, 5, 3, 6, 1, 2, Side.PLAYER, GameContent.UnitAttackScripts[0], UnitAttEngine, new Point(Convert.ToInt32(CN[1]), Convert.ToInt32(CN[2])), new Animation(8, 17, false), 0.4f), MapZones.RIGHT, Map.UI_VisibleState);
                //}
            }
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

            if (KeyBindings.CheckKeyPressed("KEY_CAMMOVE_UP"))
                cam.Position -= new Vector2(0, 10f / cam.Zoom);
            else
            if (KeyBindings.CheckKeyPressed("KEY_CAMMOVE_DOWN"))
                cam.Position += new Vector2(0, 10f / cam.Zoom);

            if (KeyBindings.CheckKeyPressed("KEY_CAMMOVE_LEFT"))
                cam.Position -= new Vector2(10f / cam.Zoom, 0);
            else
            if (KeyBindings.CheckKeyPressed("KEY_CAMMOVE_RIGHT"))
                cam.Position += new Vector2(10f / cam.Zoom, 0);

            var maprectoffs = Map.GetMapRectangle(1);
            var maprect = Map.GetMapRectangle(0);
            var FirstVect = new Vector2(maprectoffs.X, maprectoffs.Y);
            var LastVect = new Vector2(maprectoffs.X + maprectoffs.Width, maprectoffs.Y + maprectoffs.Height);
            var LastMapVect = new Vector2(maprect.X + maprect.Width, maprect.Y + maprect.Height);
            if (cam.Position.X < FirstVect.X)
                cam.Position = new Vector2(FirstVect.X, cam.Position.Y);
            if (cam.Position.Y < FirstVect.Y)
                cam.Position = new Vector2(cam.Position.X, FirstVect.Y);

            if (cam.Position.X + cam.ScreenRes.X / cam.Zoom > LastVect.X)
                cam.Position = new Vector2(LastVect.X - cam.ScreenRes.X / cam.Zoom, cam.Position.Y);
            if (cam.Position.Y + cam.ScreenRes.Y / cam.Zoom > LastVect.Y)
                cam.Position = new Vector2(cam.Position.X, LastVect.Y - cam.ScreenRes.Y / cam.Zoom);
        }

        private void UpdateGameObjects(GameMenu Menu)
        {
            var IsMouseHandled = false;
            if (UI.IsPlayerTurnSturted)
            {
                for (var i = 0; i < Hand.CardsCount; i++)
                {
                    Hand[i].Enabled = false;
                }
            }
            if (UI.IsVs)
            {
                UI.Update(ref IsMouseHandled, Map, Hand, cam, Menu, OpponentSide);
                Hand.Update(ref IsMouseHandled, Map, cam, UI.UI_Bottom.Position.Y, IsNotLockClick, PlayerSide);
            }
            else
            {
                Hand.Update(ref IsMouseHandled, Map, cam, UI.UI_Bottom.Position.Y, IsNotLockClick, PlayerSide);
                UI.Update(ref IsMouseHandled, Map, Hand, cam, Menu, OpponentSide);
            }
            Map.Update(ref IsMouseHandled, Hand, cam, IsNotLockClick, OpponentSide, this);
        }

        public void Resize(int ScreenWidth, int ScreenHeight)
        {
            Hand.CurrentScreenRes = new Vector2(ScreenWidth, ScreenHeight);
            UI.CurrentScreenRes = Hand.CurrentScreenRes;
            cam.ScreenRes = new Vector2(ScreenWidth, ScreenHeight);
        }

        public void SetPlayerTurn()
        {
            IsNotLockClick = true;
        }

        public void SetEnemyTurn()
        {
            IsNotLockClick = false;
            Hand.CalculateCardPosition(10, true);
            Map.DeSelectTile(Map.SelectedTile);
            if (Map.IsAttack)
            {
                Map.IsAttack = false;
                Map.SetDefault();
                Map.UpdateAllTiles(cam);
                Map.CreatePathArrows(null, cam);
            }
            if (Map.IsPathFinding)
            {
                Map.IsPathFinding = false;
                Map.SetDefault();
                Map.UpdateAllTiles(cam);
                Map.CreatePathArrows(null, cam);
            }
        }

        public void Close()
        {
        	//NI.SendMsg(Serialization.Serialize(new string[]{ "0" }));
        	
            NI.Disconnect();
        }

        public void Draw(SpriteBatch Target)
        {
            //UpdThread.Wait();
            watch.Start();
            // TODO: Add your drawing code here
            Target.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, cam.GetTransform(Target.GraphicsDevice));
                Map.Draw(Target, cam);
            Target.End();
            Target.Begin(SpriteSortMode.BackToFront);
                UI.Draw(Target);
                Hand.Draw(Target);
            Target.End();
        }
    }
}
