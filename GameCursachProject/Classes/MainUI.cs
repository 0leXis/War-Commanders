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
        private int iteration;
        private bool IsStartVs;
        private bool IsStopVs;
        private Vector2 _DestinationPointPlayer;
        private Vector2 _DestinationPointOpponent;
        private Vector2 _TmpPlDistance;
        private Vector2 _TmpOpDistance;
        private Vector2 _TmpPlNameDistance;
        private Vector2 _TmpOpNameDistance;
        private Vector2 _TmpIconScale;
        private Vector2 _TmpTextScale;

        private bool IsEnemyTurn;
        private bool IsPlayerTurnCardShown;
        public bool IsPlayerTurnSturted { get; set; }
        private bool IsEnTurnMove;
        private Vector2 offset;
        private int ETiteration;

        private GameState Parent;

        private bool _IsVs;
        public bool IsVs { get { return _IsVs; } }

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
		
        public ScreenBr Br { get; set; }
        public BasicSprite Vs { get; set; }
        
        public BasicText TileName { get; set; }
        public BasicText EnemyTurnText { get; set; }
        public BasicSprite EnemyTurnSprite { get; set; }
        public ScalingText NewTurnText { get; set; }

        public CardChoose Cardchoose { get; set; }
        public BasicText ChooseText { get; set; }
        public Button ChooseConfirm { get; set; }

        public CapturePointInfo[] CPInfos { get; set; }

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

                OpponentIcon.Position = new Vector2(_CurrentScreenRes.X - OpponentIcon.Texture.Width * OpponentIcon.Scale.X, 0);
                OpponentName.Position = OpponentIcon.Position - new Vector2(OpponentName.Font.MeasureString(OpponentName.Text).X + 10, 0);

                OpponentPoints.Position = new Vector2(OpponentName.Position.X + OpponentName.Font.MeasureString(OpponentName.Text).X - this.OpponentPoints.WidthHeight.X, this.OpponentName.Position.Y + OpponentName.Font.MeasureString(OpponentName.Text).Y);
                RoundTime.Position = new Vector2(OpponentPoints.Position.X, this.OpponentPoints.Position.Y + this.OpponentPoints.WidthHeight.Y + 5);

                Btn_Move.Position = new Vector2(this.UI_BottomLeft.Position.X, this.UI_BottomLeft.Position.Y + 50);
                Btn_Attack.Position = new Vector2(Btn_Move.Position.X + Btn_Move.FrameSize.X + 1, this.UI_BottomLeft.Position.Y + 50);

                Btn_EndTurn.Position = new Vector2(this.UI_BottomLeft.Position.X, Btn_Move.Position.Y + Btn_Move.Texture.Height);

                Btn_Stats.Position = new Vector2(this.UI_BottomLeft.Position.X, Btn_EndTurn.Position.Y + Btn_EndTurn.Texture.Height);
                Btn_Chat.Position = new Vector2(Btn_Stats.Position.X + Btn_Stats.FrameSize.X + 1, Btn_Stats.Position.Y);
                Btn_GameMenu.Position = new Vector2(Btn_Chat.Position.X + Btn_Chat.FrameSize.X + 1, Btn_Stats.Position.Y);

                Br.ScreenRes = value;
                Vs.Position = CurrentScreenRes / 2 - new Vector2(Vs.Texture.Width, Vs.Texture.Height) / 2;
                Cardchoose.ScreenRes = value;

                TileName.Position = new Vector2(UI_BottomLeft.Position.X + UI_BottomLeft.Texture.Width / 2 - TileName.Font.MeasureString(TileName.Text).X / 2, UI_BottomLeft.Position.Y + 5);
                ChooseText.Position = new Vector2((CurrentScreenRes.X - ChooseText.Font.MeasureString(ChooseText.Text).X) / 2, value.Y / 2 - 250);
                ChooseConfirm.Position = new Vector2((CurrentScreenRes.X - ChooseConfirm.FrameSize.X) / 2, value.Y / 2 + 250);

                NewTurnText.Position = (CurrentScreenRes - NewTurnText.Font.MeasureString(NewTurnText.Text)) / 2;

                if (CPInfos.Length != 0)
                {
                    var StartPos = new Vector2((CurrentScreenRes.X - CPInfos.Length * CPInfos[0].Neutral_Texture.Width) / 2, 0);
                    for (var i = 0; i < CPInfos.Length; i++)
                    {
                        CPInfos[i].Position = new Vector2(StartPos.X + i * CPInfos[i].Neutral_Texture.Width, 0);
                    }
                }

                EnemyTurnSprite.Position = UI_BottomLeft.Position;
                EnemyTurnText.Position = new Vector2(UI_BottomLeft.Position.X + (EnemyTurnSprite.Texture.Width - EnemyTurnText.Font.MeasureString(EnemyTurnText.Text).X) / 2, UI_BottomLeft.Position.Y + 5);
                if (IsEnemyTurn)
                {
                    SetEnemyTurn();
                }
            }
        }

        public MainUI
            (
            Vector2 CurrentScreenRes, 
            Texture2D UI_Bottom, Texture2D UI_BottomLeft, Texture2D UI_Up, 
            Texture2D UI_UpLeft, Texture2D UI_UpRight, Texture2D ButtonEndTurn_Texture, 
            Texture2D ButtonMove_Texture, Texture2D ButtonAttack_Texture, Texture2D ButtonGameMenu_Texture, 
            Texture2D ButtonChat_Texture, Texture2D ButtonStats_Texture, Texture2D PlayerIcon, 
            Texture2D OpponentIcon, Texture2D PlayerPointsIcon, Texture2D OpponentPointsIcon,
            Texture2D PlayerMoneyIcon, Texture2D RoundTimeIcon, Texture2D Vs, 
            Texture2D CPAllied, Texture2D CPEnemy, Texture2D CPNeutral, Texture2D EnemyTurn,
            SpriteFont Font,
            SpriteFont ResFont,
            SpriteFont NewTurn,
            GraphicsDevice Gr, 
            GameState Parent,
            string PlayerName, string OpponentName, string PlayerPoints, string PlayerPoints_Inc, 
            string OpponentPoints, string OpponentPoints_Inc, string Points_Needed,
            string PlayerMoney, string PlayerMoney_Inc,
            string RoundTime,
            string[] CPNames,
            float Layer = BasicSprite.DefaultLayer
            )
        {
            this.Parent = Parent;

            Inf = new InfoBox(Vector2.One, Color.Black, Color.LightBlue, Font, Color.Black, " ", Gr, 0.01f);
            Inf.Visible = false;
            _CurrentScreenRes = CurrentScreenRes;
            this.UI_Bottom = new BasicSprite(new Vector2(UI_BottomLeft.Width, _CurrentScreenRes.Y - UI_Bottom.Height), UI_Bottom, Layer);
            this.UI_BottomLeft = new BasicSprite(new Vector2(0, _CurrentScreenRes.Y - UI_BottomLeft.Height), UI_BottomLeft, Layer - 0.0001f);
            this.UI_Up = new BasicSprite(new Vector2(0, 0), UI_Up, Layer);
            this.UI_UpLeft = new BasicSprite(new Vector2(0, 0), UI_UpLeft, Layer - 0.0001f);
            this.UI_UpRight = new BasicSprite(new Vector2(_CurrentScreenRes.X - UI_UpRight.Width, 0), UI_UpRight, Layer - 0.0001f);

            //Верхний UI
            this.PlayerIcon = new BasicSprite(new Vector2(0, 0), PlayerIcon, 0.09f);
            this.PlayerIcon.Scale = new Vector2(0.4f);
            this.OpponentIcon = new BasicSprite(new Vector2(_CurrentScreenRes.X - OpponentIcon.Width * 0.4f, 0), OpponentIcon, 0.09f);
            this.OpponentIcon.Scale = new Vector2(0.4f);
            this.PlayerName = new BasicText(this.PlayerIcon.Position + new Vector2(PlayerIcon.Width * this.PlayerIcon.Scale.X + 10, 0), PlayerName, Font, Color.White, 0.09f);
            this.OpponentName = new BasicText(this.OpponentIcon.Position - new Vector2(Font.MeasureString(OpponentName).X + 10, 0), OpponentName, Font, Color.White, 0.09f);

            this.PlayerPoints = new UI_Resource_Info(new Vector2(this.PlayerName.Position.X, this.PlayerName.Position.Y + Font.MeasureString(PlayerName).Y), Color.Black, Color.FromNonPremultiplied(0, 0, 0, 130), ResFont, PlayerPointsIcon, Color.White, Color.LightGreen, PlayerPoints + @"\" + Points_Needed, " (+" + PlayerPoints_Inc + ")", Gr, Layer - 0.0005f);
            this.OpponentPoints = new UI_Resource_Info(this.PlayerName.Position, Color.Black, Color.FromNonPremultiplied(0, 0, 0, 130), ResFont, OpponentPointsIcon, Color.White, Color.LightGreen, OpponentPoints + @"\" + Points_Needed, " (+" + OpponentPoints_Inc + ")", Gr, Layer - 0.0005f);
            this.OpponentPoints.Position = new Vector2(this.OpponentName.Position.X + Font.MeasureString(OpponentName).X - this.OpponentPoints.WidthHeight.X, this.OpponentName.Position.Y + Font.MeasureString(OpponentName).Y);

            this.PlayerMoney = new UI_Resource_Info(new Vector2(this.PlayerName.Position.X, this.PlayerPoints.Position.Y + this.PlayerPoints.WidthHeight.Y + 5), Color.Black, Color.FromNonPremultiplied(0, 0, 0, 130), ResFont, PlayerMoneyIcon, Color.White, Color.LightGreen, PlayerMoney, " (+" + PlayerMoney_Inc + ")", Gr, Layer - 0.0005f);
            this.RoundTime = new UI_Resource_Info(this.PlayerName.Position, Color.Black, Color.FromNonPremultiplied(0, 0, 0, 130), ResFont, RoundTimeIcon, Color.White, Color.Red, RoundTime, "", Gr, Layer - 0.0005f);
            this.RoundTime.Position = new Vector2(this.OpponentPoints.Position.X, this.OpponentPoints.Position.Y + this.OpponentPoints.WidthHeight.Y + 5);
            //Нижний UI
            TileName = new BasicText(new Vector2(this.UI_BottomLeft.Position.X, this.UI_BottomLeft.Position.Y + 5), "", Font, Color.Black, Layer - 0.0005f);
            
            Btn_Move = new Button(new Vector2(this.UI_BottomLeft.Position.X, this.UI_BottomLeft.Position.Y + 50), ButtonMove_Texture, ButtonMove_Texture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);
            Btn_Attack = new Button(new Vector2(Btn_Move.Position.X + Btn_Move.FrameSize.X + 1, this.UI_BottomLeft.Position.Y + 50), ButtonAttack_Texture, ButtonAttack_Texture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);
            
            Btn_EndTurn = new Button(new Vector2(this.UI_BottomLeft.Position.X, Btn_Move.Position.Y + Btn_Move.Texture.Height), ButtonEndTurn_Texture, "Закончить ход", Font, Color.Black, ButtonEndTurn_Texture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);
            
            Btn_Stats = new Button(new Vector2(this.UI_BottomLeft.Position.X, Btn_EndTurn.Position.Y + Btn_EndTurn.Texture.Height), ButtonStats_Texture, ButtonStats_Texture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);
            Btn_Chat = new Button(new Vector2(Btn_Stats.Position.X + Btn_Stats.FrameSize.X + 1, Btn_Stats.Position.Y), ButtonChat_Texture, ButtonChat_Texture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);
            Btn_GameMenu = new Button(new Vector2(Btn_Chat.Position.X + Btn_Chat.FrameSize.X + 1, Btn_Stats.Position.Y), ButtonGameMenu_Texture, ButtonGameMenu_Texture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);

            EnemyTurnSprite = new BasicSprite(this.UI_BottomLeft.Position, EnemyTurn, this.UI_BottomLeft.Layer + 0.0005f);
            EnemyTurnText = new BasicText(new Vector2(this.UI_BottomLeft.Position.X + (EnemyTurn.Width - Font.MeasureString("Ход противника").X) / 2, this.UI_BottomLeft.Position.Y + 5), "Ход противника", Font, Color.Black, this.UI_BottomLeft.Layer + 0.0003f);
            NewTurnText = new ScalingText((CurrentScreenRes - NewTurn.MeasureString("")) / 2, "", NewTurn, Color.White, 20, 0f);
            NewTurnText.Visible = false;

            Br = new ScreenBr(CurrentScreenRes, 60, 220, Gr, 0.1f);
            this.Vs = new BasicSprite(CurrentScreenRes / 2 - new Vector2(Vs.Width, Vs.Height) / 2, Vs, 0.09f);
            Cardchoose = new CardChoose(CurrentScreenRes, GameContent.UI_CardReplace);
            ChooseText = new BasicText(Vector2.Zero, "Выберите карты, которые хотите заменить", Font, Color.White, 0.001f);
            ChooseText.Visible = false;
            ChooseText.Position = new Vector2((CurrentScreenRes.X - Font.MeasureString(ChooseText.Text).X) / 2, CurrentScreenRes.Y / 2 - 250);
            ChooseConfirm = new Button(new Vector2((CurrentScreenRes.X - ButtonEndTurn_Texture.Width / 4) / 2, CurrentScreenRes.Y / 2 + 250), ButtonEndTurn_Texture, "Заменить", Font, Color.Black, ButtonEndTurn_Texture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, 0.001f);
            ChooseConfirm.Visible = false;

            CPInfos = new CapturePointInfo[CPNames.Length];
            var StartPos = new Vector2((CurrentScreenRes.X - CPNames.Length * CPNeutral.Width) / 2, 0);
            for(var i = 0; i < CPNames.Length; i++)
            {
                CPInfos[i] = new CapturePointInfo(new Vector2(StartPos.X + i * CPNeutral.Width, 0), CPAllied, CPEnemy, CPNeutral, Font, CPNames[i], this.UI_Up.Layer - 0.001f);
            }

            StartVS();

            IsEnemyTurn = false;
        }

        public void DisableUI()
        {
            Btn_Move.Enabled = false;
            Btn_Attack.Enabled = false;
            Btn_GameMenu.Enabled = false;
            Btn_Chat.Enabled = false;
            Btn_EndTurn.Enabled = false;
            Btn_Stats.Enabled = false;

            ChooseConfirm.Enabled = false;
            Cardchoose.Enabled = false;
        }

        public void EnableUI()
        {
            Btn_Move.Enabled = true;
            Btn_Attack.Enabled = true;
            Btn_GameMenu.Enabled = true;
            Btn_Chat.Enabled = true;
            Btn_EndTurn.Enabled = true;
            Btn_Stats.Enabled = true;

            ChooseConfirm.Enabled = true;
            Cardchoose.Enabled = true;
        }

        public void Update(ref bool IsMouseHandled, Map map, Hand hand, Camera cam)
        {
            EnemyTurnMoveProcess();
        	Br.UpdateAnims();
            if (_IsVs)
            {
                Cardchoose.Update();
                if(ChooseConfirm.Update() == ButtonStates.CLICKED)
                {
                    ChooseConfirm.Visible = false;
                    var replacedcards = Cardchoose.GetReplacedCards();
                    var CardsToReplace = new List<Card>();
                    foreach(var rep in replacedcards)
                    {
                        if (rep)
                        {
                            CardsToReplace.Add(new Card(Vector2.One, GameContent.CardTexture, GameContent.Card_Decorations[0], new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, "Pz. VI H \"Tiger\"", "3", "1", "5", "5", "6", 141, 315, 4, 4, 37, true, MapZones.RIGHT, Layer: 0.001f));
                        }
                    }
                    Cardchoose.ReplaceCards(CardsToReplace.ToArray());
                    iteration = 0;
                }
                IsMouseHandled = true;
                if(iteration < 120)
                {
                    if (IsStartVs && iteration > 80)
                    {
                        PlayerIcon.Position += new Vector2((_DestinationPointPlayer.X - PlayerIcon.Position.X) / 8, 0);
                        OpponentIcon.Position -= new Vector2((OpponentIcon.Position.X - _DestinationPointOpponent.X) / 8, 0);
                        PlayerName.Position = new Vector2(PlayerIcon.Position.X + PlayerIcon.Texture.Width / 2 * PlayerIcon.Scale.X - PlayerName.Font.MeasureString(PlayerName.Text).X, PlayerIcon.Position.Y + PlayerIcon.Texture.Height * PlayerIcon.Scale.Y);
                        OpponentName.Position = new Vector2(OpponentIcon.Position.X + OpponentIcon.Texture.Width / 2 * OpponentIcon.Scale.X - OpponentName.Font.MeasureString(OpponentName.Text).X, OpponentIcon.Position.Y + OpponentIcon.Texture.Height * OpponentIcon.Scale.Y);
                    }
                    if (IsStopVs && iteration > 80)
                    {
                        PlayerIcon.Position -= _TmpPlDistance;
                        OpponentIcon.Position -= _TmpOpDistance;
                        PlayerIcon.Scale -= _TmpIconScale;
                        OpponentIcon.Scale -= _TmpIconScale;
                        PlayerName.Position -= _TmpPlNameDistance;
                        OpponentName.Position -= _TmpOpNameDistance;
                        PlayerName.Scale -= _TmpTextScale;
                        OpponentName.Scale -= _TmpTextScale;
                        Vs.Visible = false;
                    }
                    if(iteration == 119)
                    {
                        if (IsStartVs)
                        {
                            IsStartVs = false;
                            StopVS();
                        }
                        else
                        if (IsStopVs)
                        {
                            IsStopVs = false;
                            PlayerIcon.Position = Vector2.Zero;
                            OpponentIcon.Position = new Vector2(_CurrentScreenRes.X - OpponentIcon.Texture.Width * OpponentIcon.Scale.X, 0);
                            PlayerIcon.Scale = new Vector2(0.4f);
                            OpponentIcon.Scale = new Vector2(0.4f);
                            PlayerName.Scale = Vector2.One;
                            OpponentName.Scale = Vector2.One;
                            PlayerName.Position = PlayerIcon.Position + new Vector2(PlayerIcon.Texture.Width * PlayerIcon.Scale.X + 10, 0);
                            OpponentName.Position = OpponentIcon.Position - new Vector2(OpponentName.Font.MeasureString(OpponentName.Text).X + 10, 0);
                            ChooseText.Visible = true;
                            ChooseConfirm.Visible = true;
                            Cardchoose.ShowCards(true,
                                new Card(Vector2.One, GameContent.CardTexture, GameContent.Card_Decorations[0], new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, "Pz. VI H \"Tiger\"", "3", "1", "5", "5", "6", 141, 315, 4, 4, 37, true, MapZones.RIGHT, Layer: 0.001f),
                                new Card(Vector2.One, GameContent.CardTexture, GameContent.Card_Decorations[0], new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, "Pz. VI H \"Tiger\"", "3", "1", "5", "5", "6", 141, 315, 4, 4, 37, true, MapZones.RIGHT, Layer: 0.001f),
                                new Card(Vector2.One, GameContent.CardTexture, GameContent.Card_Decorations[0], new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, "Pz. VI H \"Tiger\"", "3", "1", "5", "5", "6", 141, 315, 4, 4, 37, true, MapZones.RIGHT, Layer: 0.001f), 
                                new Card(Vector2.One, GameContent.CardTexture, GameContent.Card_Decorations[0], new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, "Pz. VI H \"Tiger\"", "3", "1", "5", "5", "6", 141, 315, 4, 4, 37, true, MapZones.RIGHT, Layer: 0.001f));
                        }
                        else
                        {
                            _IsVs = false;
                            //Test
                            SetPlayerTurn(1);

                            ChooseText.Visible = false;
                            ChooseText.Text = "Выберите карту";
                            ChooseText.Position = new Vector2((CurrentScreenRes.X - ChooseText.Font.MeasureString(ChooseText.Text).X) / 2, CurrentScreenRes.Y / 2 - 250);
                            hand.AddCards(20, Cardchoose.GetCards(true).ToArray());
                            Cardchoose.ClearCardList();
                            Br.ScreenBrUp();
                        }
                    }
                    iteration++;
                }
            }
            else
            if (IsPlayerTurnSturted)
            {
                if(iteration < 100)
                {
                    NewTurnText.Update();
                    if(iteration == 79)
                    {
                        NewTurnText.Disappear();
                    }
                    iteration++;
                }
                else
                {
                    if (!IsPlayerTurnCardShown)
                    {
                        IsPlayerTurnCardShown = true;
                        ChooseText.Visible = true;
                        Cardchoose.ShowCards(false,
                            new Card(Vector2.One, GameContent.CardTexture, GameContent.Card_Decorations[0], new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, "Pz. VI H \"Tiger\"", "3", "1", "5", "5", "6", 141, 315, 4, 4, 37, true, MapZones.RIGHT, Layer: 0.001f),
                            new Card(Vector2.One, GameContent.CardTexture, GameContent.Card_Decorations[0], new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, "Pz. VI H \"Tiger\"", "3", "1", "5", "5", "6", 141, 315, 4, 4, 37, true, MapZones.RIGHT, Layer: 0.001f),
                            new Card(Vector2.One, GameContent.CardTexture, GameContent.Card_Decorations[0], new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, "Pz. VI H \"Tiger\"", "3", "1", "5", "5", "6", 141, 315, 4, 4, 37, true, MapZones.RIGHT, Layer: 0.001f));
                    }
                    var Upd = Cardchoose.Update();
                    if (Upd != -1)
                    {
                        //Отправить на серв данные
                        hand.AddCards(20, Cardchoose.GetCards(true).ToArray()[Upd]);
                        ChooseText.Visible = false;
                        Cardchoose.ClearCardList();
                        IsPlayerTurnSturted = false;
                    }
                }
            }
            else
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
                    if (MouseControl.Y < UI_UpLeft.Texture.Height)
                        IsMouseHandled = true;
                }

                var MoveUpd = Btn_Move.Update();
                var AttackUpd = Btn_Attack.Update();
                var EndTurnUpd = Btn_EndTurn.Update();
                var StatsUpd = Btn_Stats.Update();
                var ChatUpd = Btn_Chat.Update();
                var GameMenuUpd = Btn_GameMenu.Update();
                _ShowInf = false;

                if (!(map.IsPathFinding || map.IsAttack))
                {
                    Btn_EndTurn.Enabled = true;
                    if(!map.IsPathFinding)
                        Btn_Attack.Enabled = true;
                    if (!map.IsAttack)
                        Btn_Move.Enabled = true;
                    for (var i = 0; i < hand.CardsCount; i++)
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

                if ((MoveUpd == ButtonStates.CLICKED || (Btn_Move.Enabled && KeyBindings.CheckKeyReleased("KEY_MOVEUNIT"))) && map.SelectedTile.X != -1)
                {
                    if (map.IsPathFinding)
                    {
                        map.IsPathFinding = false;
                        map.SetDefault();
                        map.UpdateAllTiles(cam);
                        map.CreatePathArrows(null, cam);
                    }
                    else
                    if ((map.GetTile(map.SelectedTile.X, map.SelectedTile.Y).TileContains == MapTiles.WITH_UNIT || map.GetTile(map.SelectedTile.X, map.SelectedTile.Y).TileContains == MapTiles.WITH_UNIT_AND_BUILDING) && map.GetTile(map.SelectedTile.X, map.SelectedTile.Y).UnitOnTile.side == Side.PLAYER)
                    {
                        Btn_Attack.Enabled = false;
                        Btn_EndTurn.Enabled = false;
                        map.IsPathFinding = true;
                        map.ActionStartPoint = new Point(map.SelectedTile.X, map.SelectedTile.Y);
                        map.HighLiteTilesWithPF();
                        map.UpdateAllTiles(cam);
                    }
                }
                else
                if (MoveUpd == ButtonStates.ENTERED)
                {
                    Inf.Appear();
                    Inf.Text = "Перемещение/ближний бой";
                    _ShowInf = true;
                }

                if ((AttackUpd == ButtonStates.CLICKED || (Btn_Attack.Enabled && KeyBindings.CheckKeyReleased("KEY_ATTACKUNIT"))) && map.SelectedTile.X != -1)
                {
                    if (map.IsAttack)
                    {
                        map.IsAttack = false;
                        map.SetDefault();
                        map.UpdateAllTiles(cam);
                        map.CreatePathArrows(null, cam);
                    }
                    else
                    if ((map.GetTile(map.SelectedTile.X, map.SelectedTile.Y).TileContains == MapTiles.WITH_UNIT || map.GetTile(map.SelectedTile.X, map.SelectedTile.Y).TileContains == MapTiles.WITH_UNIT_AND_BUILDING) && map.GetTile(map.SelectedTile.X, map.SelectedTile.Y).UnitOnTile.side == Side.PLAYER)
                    {
                        Btn_EndTurn.Enabled = false;
                        Btn_Move.Enabled = false;
                        map.IsAttack = true;
                        map.ActionStartPoint = new Point(map.SelectedTile.X, map.SelectedTile.Y);
                        map.HighLiteTilesWithEnemy();
                        map.UpdateAllTiles(cam);
                    }
                }
                else
                if (AttackUpd == ButtonStates.ENTERED)
                {
                    Inf.Appear();
                    Inf.Text = "Дальняя атака";
                    _ShowInf = true;
                }


                if (StatsUpd == ButtonStates.CLICKED || KeyBindings.CheckKeyReleased("KEY_STATS"))
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
                if (StatsUpd == ButtonStates.ENTERED)
                {
                    Inf.Appear();
                    Inf.Text = "Скрыть/показать\n характ. юнитов";
                    _ShowInf = true;
                }
                if (ChatUpd == ButtonStates.ENTERED)
                {
                    Inf.Appear();
                    Inf.Text = "Чат";
                    _ShowInf = true;
                }
                if (GameMenuUpd == ButtonStates.ENTERED)
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

                //*TEST
                if(EndTurnUpd == ButtonStates.CLICKED)
                {
                    if(IsEnemyTurn)
                        SetPlayerTurn(1);
                    else
                        SetEnemyTurn();
                }
                //*TEST

                if (!_ShowInf)
                    Inf.Disappear();
                else
                {
                    Inf.Position = new Vector2(MouseControl.X + 10, MouseControl.Y);
                }
                Inf.Update();
                if(map.ChoosedTileI != -1)
                {
                	TileName.Text = map.GetTile(map.ChoosedTileI, map.ChoosedTileJ).TileName;
                	TileName.Position = new Vector2(UI_BottomLeft.Position.X + UI_BottomLeft.Texture.Width / 2 - TileName.Font.MeasureString(TileName.Text).X / 2, UI_BottomLeft.Position.Y + 5);
                }
            }
        }

        public void GameInit()
        {
        	Br.CurrentFrame = 0;
        }
        
        public void StartVS()
        {
            _IsVs = true;
        	IsStartVs = true;
        	IsStopVs = false;
            iteration = 0;
            //PlayerIcon.Scale = new Vector2(2.5f);
            PlayerIcon.Scale = Vector2.One;
            PlayerIcon.Position = new Vector2(-PlayerIcon.Scale.X * PlayerIcon.Texture.Width, CurrentScreenRes.Y / 2 - PlayerIcon.Texture.Height * PlayerIcon.Scale.Y / 2);
            //OpponentIcon.Scale = new Vector2(2.5f);
            OpponentIcon.Scale = Vector2.One;
            OpponentIcon.Position = new Vector2(OpponentIcon.Scale.X * PlayerIcon.Texture.Width + CurrentScreenRes.X, CurrentScreenRes.Y / 2 - OpponentIcon.Texture.Height * OpponentIcon.Scale.Y / 2);

            _DestinationPointPlayer = new Vector2(Vs.Position.X - PlayerIcon.Texture.Width * PlayerIcon.Scale.X - 50, CurrentScreenRes.Y / 2 - PlayerIcon.Texture.Height * PlayerIcon.Scale.Y / 2);
            _DestinationPointOpponent = new Vector2(Vs.Position.X + Vs.Texture.Width + 50, CurrentScreenRes.Y / 2 - OpponentIcon.Texture.Height * OpponentIcon.Scale.Y / 2);

            PlayerName.Position = new Vector2(PlayerIcon.Position.X + PlayerIcon.Texture.Width / 2 * PlayerIcon.Scale.X - PlayerName.Font.MeasureString(PlayerName.Text).X, PlayerIcon.Position.Y + PlayerIcon.Texture.Height * PlayerIcon.Scale.Y);
        	OpponentName.Position = new Vector2(OpponentIcon.Position.X + OpponentIcon.Texture.Width / 2 * OpponentIcon.Scale.X - OpponentName.Font.MeasureString(OpponentName.Text).X, OpponentIcon.Position.Y + OpponentIcon.Texture.Height * OpponentIcon.Scale.Y);
        	PlayerName.Scale = new Vector2(2f);
        	OpponentName.Scale = new Vector2(2f);
        	
        	//PlayerIcon.Position = new Vector2(Vs.Position.X - PlayerIcon.Texture.Width * PlayerIcon.Scale.X - 50, CurrentScreenRes.Y / 2 - PlayerIcon.Texture.Height * PlayerIcon.Scale.Y / 2);
        	//OpponentIcon.Position = new Vector2(Vs.Position.X + Vs.Texture.Width + 50, CurrentScreenRes.Y / 2 - OpponentIcon.Texture.Height * OpponentIcon.Scale.Y / 2);

            //PlayerName.Position = new Vector2(PlayerIcon.Position.X + PlayerIcon.Texture.Width / 2 * PlayerIcon.Scale.X - PlayerName.Font.MeasureString(PlayerName.Text).X, PlayerIcon.Position.Y + PlayerIcon.Texture.Height * PlayerIcon.Scale.Y);
            //OpponentName.Position = new Vector2(OpponentIcon.Position.X + OpponentIcon.Texture.Width / 2 * OpponentIcon.Scale.X - OpponentName.Font.MeasureString(OpponentName.Text).X, OpponentIcon.Position.Y + OpponentIcon.Texture.Height * OpponentIcon.Scale.Y);
        }
                
        public void StopVS()
        {
        	IsStopVs = true;
        	IsStartVs = false;
            _TmpPlDistance = PlayerIcon.Position / 40;
            _TmpOpDistance = (OpponentIcon.Position - new Vector2(_CurrentScreenRes.X - OpponentIcon.Texture.Width * 0.4f, 0)) / 40;
            _TmpIconScale = (PlayerIcon.Scale - new Vector2(0.40f)) / 40;

            _TmpPlNameDistance = (PlayerName.Position - new Vector2(PlayerIcon.Texture.Width * 0.4f + 10, 0)) / 40;
            _TmpOpNameDistance = (OpponentName.Position - new Vector2(_CurrentScreenRes.X - OpponentIcon.Texture.Width * 0.4f - OpponentName.Font.MeasureString(OpponentName.Text).X + 10, 0)) / 40;
            _TmpTextScale = (PlayerName.Scale - new Vector2(1f)) / 40;
            iteration = 0;
        }
                
        public void BrUp()
        {
        	Br.ScreenBrUp();
        }
        
        public void SetEnemyTurn()
        {
            Parent.SetEnemyTurn();
            IsEnemyTurn = true;
            IsEnTurnMove = true;
            ETiteration = 0;
            offset = new Vector2(0, EnemyTurnSprite.Texture.Height / 10);
        }

        public void SetPlayerTurn(int Turn_Number)
        {
            IsPlayerTurnSturted = true;
            IsPlayerTurnCardShown = false;
            iteration = 0;

            NewTurnText.Appear();
            NewTurnText.Text = "ХОД " + Turn_Number.ToString();
            NewTurnText.Position = (CurrentScreenRes - NewTurnText.Font.MeasureString(NewTurnText.Text)) / 2;

            Parent.SetPlayerTurn();
            IsEnemyTurn = false;
            IsEnTurnMove = false;
            EnemyTurnSprite.Position = UI_BottomLeft.Position;
            EnemyTurnText.Position = new Vector2(UI_BottomLeft.Position.X + (EnemyTurnSprite.Texture.Width - EnemyTurnText.Font.MeasureString(EnemyTurnText.Text).X) / 2, UI_BottomLeft.Position.Y + 5);
        }

        public void EnemyTurnMoveProcess()
        {
            if (IsEnTurnMove)
            {
                if(ETiteration < 10)
                {
                    EnemyTurnSprite.Position -= offset;
                    EnemyTurnText.Position -= offset;
                }
                else
                {
                    IsEnTurnMove = false;
                }
                ETiteration++;
            }
        }

        public void Draw(SpriteBatch Target)
        {
        	Br.Draw(Target);
        	Vs.Draw(Target);
            Cardchoose.Draw(Target);
            ChooseText.Draw(Target);
            ChooseConfirm.Draw(Target);

            UI_Bottom.Draw(Target);
            UI_BottomLeft.Draw(Target);
            UI_Up.Draw(Target);
            UI_UpRight.Draw(Target);
            UI_UpLeft.Draw(Target);

            EnemyTurnSprite.Draw(Target);
            EnemyTurnText.Draw(Target);
            NewTurnText.Draw(Target);

            PlayerIcon.Draw(Target);
            OpponentIcon.Draw(Target);
            PlayerName.Draw(Target);
            OpponentName.Draw(Target);
            PlayerPoints.Draw(Target);
            OpponentPoints.Draw(Target);
            PlayerMoney.Draw(Target);
            RoundTime.Draw(Target);

            foreach(var cp in CPInfos)
                cp.Draw(Target);

            TileName.Draw(Target);
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
