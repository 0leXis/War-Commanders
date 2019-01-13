using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
	class MainUI : IDrawable
    {
        private Vector2 _CurrentScreenRes;
        private bool _ShowInf;

        public BasicSprite UI_Bottom { get; set; }
        public BasicSprite UI_BottomLeft { get; set; }
        public BasicSprite UI_Up { get; set; }
        public BasicSprite UI_UpLeft { get; set; }
        public BasicSprite UI_UpRight { get; set; }
        public BasicSprite PlayerIcon { get; set; }
        public BasicSprite OpponentIcon { get; set; }

        public BasicText PlayerName { get; set; }
        public BasicText OpponentName { get; set; }
        public UI_Resource_Info PlayerPoints { get; set; }
        public UI_Resource_Info OpponentPoints { get; set; }
        public UI_Resource_Info PlayerMoney { get; set; }
        public UI_Resource_Info RoundTime { get; set; }

        public Button Btn_Move { get; set; }
        public Button Btn_Attack { get; set; }
        public Button Btn_GameMenu { get; set; }
        public Button Btn_Chat { get; set; }
		public Button Btn_EndTurn { get; set; }
		public Button Btn_Stats { get; set; }
		
        public InfoBox Inf { get; set; }

        public Vector2 CurrentScreenRes
        {
            get
            {
                return _CurrentScreenRes;
            }
            set
            {
                _CurrentScreenRes = value;
                UI_Bottom.Position = new Vector2(UI_BottomLeft.Texture.Width, _CurrentScreenRes.Y - UI_Bottom.Texture.Height);
                UI_BottomLeft.Position = new Vector2(0, _CurrentScreenRes.Y - UI_BottomLeft.Texture.Height);
                UI_Bottom.Scale = new Vector2((_CurrentScreenRes.X + 20 * _CurrentScreenRes.X) / 1280, UI_Bottom.Scale.Y);
                UI_Up.Scale = new Vector2((_CurrentScreenRes.X + 20 * _CurrentScreenRes.X) / 1280, UI_Bottom.Scale.Y);
                UI_UpRight.Position = new Vector2(_CurrentScreenRes.X - UI_UpRight.Texture.Width, 0);

                OpponentIcon.Position = new Vector2(_CurrentScreenRes.X - OpponentIcon.Texture.Width, 0);
                OpponentName.Position = OpponentIcon.Position - new Vector2(OpponentName.Font.MeasureString(OpponentName.Text).X + 10, 0);

                OpponentPoints.Position = new Vector2(OpponentName.Position.X + OpponentName.Font.MeasureString(OpponentName.Text).X - this.OpponentPoints.WidthHeight.X, this.OpponentName.Position.Y + OpponentName.Font.MeasureString(OpponentName.Text).Y);
                RoundTime.Position = new Vector2(OpponentPoints.Position.X, this.OpponentPoints.Position.Y + this.OpponentPoints.WidthHeight.Y + 5);

                Btn_Move.Position = new Vector2(this.UI_BottomLeft.Position.X, this.UI_BottomLeft.Position.Y + 30);
                Btn_Attack.Position = new Vector2(Btn_Move.Position.X + Btn_Move.FrameSize.X + 1, this.UI_BottomLeft.Position.Y + 30);

                Btn_EndTurn.Position = new Vector2(this.UI_BottomLeft.Position.X, Btn_Move.Position.Y + Btn_Move.Texture.Height);

                Btn_Stats.Position = new Vector2(this.UI_BottomLeft.Position.X, Btn_EndTurn.Position.Y + Btn_EndTurn.Texture.Height);
                Btn_Chat.Position = new Vector2(Btn_Stats.Position.X + Btn_Stats.FrameSize.X + 1, Btn_Stats.Position.Y);
                Btn_GameMenu.Position = new Vector2(Btn_Chat.Position.X + Btn_Chat.FrameSize.X + 1, Btn_Stats.Position.Y);
            }
        }

        public MainUI
            (
            Vector2 CurrentScreenRes, 
            Texture2D UI_Info, Texture2D UI_Bottom, Texture2D UI_BottomLeft, Texture2D UI_Up, 
            Texture2D UI_UpLeft, Texture2D UI_UpRight, Texture2D ButtonEndTurn_Texture, 
            Texture2D ButtonMove_Texture, Texture2D ButtonAttack_Texture, Texture2D ButtonGameMenu_Texture, 
            Texture2D ButtonChat_Texture, Texture2D ButtonStats_Texture, Texture2D PlayerIcon, 
            Texture2D OpponentIcon, Texture2D PlayerPointsIcon, Texture2D OpponentPointsIcon,
            Texture2D PlayerMoneyIcon, Texture2D RoundTimeIcon,
            SpriteFont Font,
            SpriteFont ResFont,
            GraphicsDevice Gr, 
            string PlayerName, string OpponentName, string PlayerPoints, string PlayerPoints_Inc, 
            string OpponentPoints, string OpponentPoints_Inc, string Points_Needed,
            string PlayerMoney, string PlayerMoney_Inc,
            string RoundTime,
            float Layer = BasicSprite.DefaultLayer
            )
        {
            Inf = new InfoBox(Vector2.One, Color.Black, Color.LightBlue, Font, Color.Black, " ", Gr, 0.01f);
            Inf.Visible = false;
            _CurrentScreenRes = CurrentScreenRes;
            this.UI_Bottom = new BasicSprite(new Vector2(UI_BottomLeft.Width, _CurrentScreenRes.Y - UI_Bottom.Height), UI_Bottom, Layer);
            this.UI_BottomLeft = new BasicSprite(new Vector2(0, _CurrentScreenRes.Y - UI_BottomLeft.Height), UI_BottomLeft, Layer - 0.0001f);
            this.UI_Up = new BasicSprite(new Vector2(0, 0), UI_Up, Layer);
            this.UI_UpLeft = new BasicSprite(new Vector2(0, 0), UI_UpLeft, Layer - 0.0001f);
            this.UI_UpRight = new BasicSprite(new Vector2(_CurrentScreenRes.X - UI_UpRight.Width, 0), UI_UpRight, Layer - 0.0001f);

            //Верхний UI
            this.PlayerIcon = new BasicSprite(new Vector2(0, 0), PlayerIcon, Layer - 0.0001f);
            this.OpponentIcon = new BasicSprite(new Vector2(_CurrentScreenRes.X - OpponentIcon.Width, 0), OpponentIcon, Layer - 0.0001f);

            this.PlayerName = new BasicText(this.PlayerIcon.Position + new Vector2(PlayerIcon.Width + 10, 0), PlayerName, Font, Color.White, Layer - 0.0005f);
            this.OpponentName = new BasicText(this.OpponentIcon.Position - new Vector2(Font.MeasureString(OpponentName).X + 10, 0), OpponentName, Font, Color.White, Layer - 0.0005f);

            this.PlayerPoints = new UI_Resource_Info(new Vector2(this.PlayerName.Position.X, this.PlayerName.Position.Y + Font.MeasureString(PlayerName).Y), Color.Black, Color.FromNonPremultiplied(0, 0, 0, 130), ResFont, PlayerPointsIcon, Color.White, Color.LightGreen, PlayerPoints + @"\" + Points_Needed, " (+" + PlayerPoints_Inc + ")", Gr, Layer - 0.0005f);
            this.OpponentPoints = new UI_Resource_Info(this.PlayerName.Position, Color.Black, Color.FromNonPremultiplied(0, 0, 0, 130), ResFont, OpponentPointsIcon, Color.White, Color.LightGreen, OpponentPoints + @"\" + Points_Needed, " (+" + OpponentPoints_Inc + ")", Gr, Layer - 0.0005f);
            this.OpponentPoints.Position = new Vector2(this.OpponentName.Position.X + Font.MeasureString(OpponentName).X - this.OpponentPoints.WidthHeight.X, this.OpponentName.Position.Y + Font.MeasureString(OpponentName).Y);

            this.PlayerMoney = new UI_Resource_Info(new Vector2(this.PlayerName.Position.X, this.PlayerPoints.Position.Y + this.PlayerPoints.WidthHeight.Y + 5), Color.Black, Color.FromNonPremultiplied(0, 0, 0, 130), ResFont, PlayerMoneyIcon, Color.White, Color.LightGreen, PlayerMoney, " (+" + PlayerMoney_Inc + ")", Gr, Layer - 0.0005f);
            this.RoundTime = new UI_Resource_Info(this.PlayerName.Position, Color.Black, Color.FromNonPremultiplied(0, 0, 0, 130), ResFont, RoundTimeIcon, Color.White, Color.Red, RoundTime, "", Gr, Layer - 0.0005f);
            this.RoundTime.Position = new Vector2(this.OpponentPoints.Position.X, this.OpponentPoints.Position.Y + this.OpponentPoints.WidthHeight.Y + 5);
            //Нижний UI
            Btn_Move = new Button(new Vector2(this.UI_BottomLeft.Position.X, this.UI_BottomLeft.Position.Y + 30), ButtonMove_Texture, ButtonMove_Texture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);
            Btn_Attack = new Button(new Vector2(Btn_Move.Position.X + Btn_Move.FrameSize.X + 1, this.UI_BottomLeft.Position.Y + 30), ButtonAttack_Texture, ButtonAttack_Texture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);
            
            Btn_EndTurn = new Button(new Vector2(this.UI_BottomLeft.Position.X, Btn_Move.Position.Y + Btn_Move.Texture.Height), ButtonEndTurn_Texture, "Закончить ход", Font, Color.Black, ButtonEndTurn_Texture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);
            
            Btn_Stats = new Button(new Vector2(this.UI_BottomLeft.Position.X, Btn_EndTurn.Position.Y + Btn_EndTurn.Texture.Height), ButtonStats_Texture, ButtonStats_Texture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);
            Btn_Chat = new Button(new Vector2(Btn_Stats.Position.X + Btn_Stats.FrameSize.X + 1, Btn_Stats.Position.Y), ButtonChat_Texture, ButtonChat_Texture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);
            Btn_GameMenu = new Button(new Vector2(Btn_Chat.Position.X + Btn_Chat.FrameSize.X + 1, Btn_Stats.Position.Y), ButtonGameMenu_Texture, ButtonGameMenu_Texture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);
        }

        public void Update(ref bool IsMouseHandled, Map map, Hand hand, Camera cam)
        {
            if (!IsMouseHandled)
            {
                if (MouseControl.X < UI_BottomLeft.Texture.Width)
                {
                    if (MouseControl.Y > _CurrentScreenRes.Y - UI_BottomLeft.Texture.Height)
                    {
                        IsMouseHandled = true;
                    }
                }
                else
                if (MouseControl.Y > _CurrentScreenRes.Y - UI_Bottom.Texture.Height)
                    IsMouseHandled = true;
                else
                if(MouseControl.Y < UI_UpLeft.Texture.Height)
                    IsMouseHandled = true;
            }

            var MoveUpd = Btn_Move.Update();
            var Attack = Btn_Attack.Update();
            var EndTurn = Btn_EndTurn.Update();
            var Stats = Btn_Stats.Update();
            var Chat = Btn_Chat.Update();
            var GameMenu = Btn_GameMenu.Update();
            _ShowInf = false;

            if (!map.IsPathFinding)
            {
                Btn_EndTurn.Enabled = true;
                for(var i = 0; i < hand.CardsCount; i++)
                {
                    hand[i].Enabled = true;
                }
            }
            else
            {
                for (var i = 0; i < hand.CardsCount; i++)
                {
                    hand[i].Enabled = false;
                }
            }

            if ((MoveUpd == ButtonStates.CLICKED || KeyBindings.CheckKeyReleased("KEY_MOVEUNIT")) && map.SelectedTile.X != -1)
            {
                if (map.IsPathFinding)
                {
                    map.IsPathFinding = false;
                    map.SetDefaultAnims();
                    map.UpdateAllTiles(cam);
                    map.CreatePathArrows(null, cam);
                }
                else
            	if(map.GetTile(map.SelectedTile.X, map.SelectedTile.Y).TileContains == MapTiles.WITH_UNIT || map.GetTile(map.SelectedTile.X, map.SelectedTile.Y).TileContains == MapTiles.WITH_UNIT_AND_BUILDING)
            	{
                    Btn_EndTurn.Enabled = false;
                    map.IsPathFinding = true;
                	map.PFStart = new Point(map.SelectedTile.X, map.SelectedTile.Y);
                    map.HighLiteTilesWithPF();
                    map.UpdateAllTiles(cam);
                }
            }
            else
            if(MoveUpd == ButtonStates.ENTERED)
            {
                Inf.Appear();
                Inf.Text = "Перемещение/ближний бой";
                _ShowInf = true;
            }

            if (Attack == ButtonStates.ENTERED)
            {
                Inf.Appear();
                Inf.Text = "Дальняя атака";
                _ShowInf = true;
            }
            if (Stats == ButtonStates.CLICKED || KeyBindings.CheckKeyReleased("KEY_STATS"))
            {
                if (map.UI_VisibleState)
                {
                    map.HideUnitStats();
                }
                else
                {
                    map.ShowUnitStats();
                }
            }
            else
            if (Stats == ButtonStates.ENTERED)
            {
                Inf.Appear();
                Inf.Text = "Скрыть/показать\n характ. юнитов";
                _ShowInf = true;
            }
            if (Chat == ButtonStates.ENTERED)
            {
                Inf.Appear();
                Inf.Text = "Чат";
                _ShowInf = true;
            }
            if (GameMenu == ButtonStates.ENTERED)
            {
                Inf.Appear();
                Inf.Text = "Игровое меню";
                _ShowInf = true;
            }
            if (PlayerPoints.Update() == ButtonStates.ENTERED)
            {
                Inf.Appear();
                Inf.Text = "Очки/Необходимо\nдля победы";
                _ShowInf = true;
            }
            if (OpponentPoints.Update() == ButtonStates.ENTERED)
            {
                Inf.Appear();
                Inf.Text = "Очки/Необходимо\nдля победы";
                _ShowInf = true;
            }
            if (PlayerMoney.Update() == ButtonStates.ENTERED)
            {
                Inf.Appear();
                Inf.Text = "Ресурсы";
                _ShowInf = true;
            }
            if (RoundTime.Update() == ButtonStates.ENTERED)
            {
                Inf.Appear();
                Inf.Text = "Время хода";
                _ShowInf = true;
            }

            if (!_ShowInf)
                Inf.Disappear();
            else
            {
                Inf.Position = new Vector2(MouseControl.X + 10, MouseControl.Y);
            }
            Inf.Update();
        }

        public void Draw(SpriteBatch Target)
        {
            UI_Bottom.Draw(Target);
            UI_BottomLeft.Draw(Target);
            UI_Up.Draw(Target);
            UI_UpRight.Draw(Target);
            UI_UpLeft.Draw(Target);

            PlayerIcon.Draw(Target);
            OpponentIcon.Draw(Target);
            PlayerName.Draw(Target);
            OpponentName.Draw(Target);
            PlayerPoints.Draw(Target);
            OpponentPoints.Draw(Target);
            PlayerMoney.Draw(Target);
            RoundTime.Draw(Target);

            Btn_Move.Draw(Target);
            Btn_Attack.Draw(Target);
            Btn_EndTurn.Draw(Target);
            Btn_Stats.Draw(Target);
            Btn_GameMenu.Draw(Target);
            Btn_Chat.Draw(Target);
            Inf.Draw(Target);
        }
    }
}
