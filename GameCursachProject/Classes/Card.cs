using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    enum CardCanUseOnTiles
    {
        ONLY_NONE,
        ONLY_WITH_UNIT,
        ONLY_WITH_BUILDING,

        WITH_BUILDING_AND_WITH_UNIT,

        NONE_OR_WITH_UNIT,
        NONE_OR_WITH_BUILDING,
        WITH_BUILDING_OR_WITH_UNIT,
        ALL
    }

    class Card : Button, IDrawable
    {
        private const float UpLayer = 0.005f;
        private float LastLayer;
        private Vector2 _UpPosition;
        private Vector2 _DownPosition;
        private Vector2 _MoveVector;
        private bool IsDown, LockClicking, IsMoving;
        private byte iteration;
        private byte moveiteration;
        private int MoveSpeed;

        public bool IsTargeted { get; set; }
        public bool IsDisappearing { get; set; }
        public bool IsPressed { get; set; } 

        public MapZones AllowedZones { get; set; }
        public CardCanUseOnTiles AllowedTiles { get; set; }
        public BasicSprite Art { get; set; }
        public Vector2 ArtOffset { get; set; }
        public int CardName_OffsY { get; set; }
        public int Stats_OffsY { get; set; }
        public int FirstStat_OffsX { get; set; }
        public int MidStat_OffsX { get; set; }
        public int StatCellWidth { get; set; }
        public bool UpdBtnAnims { get; set; }

        public BasicText CardName { get; set; }
        public BasicText CardDescription { get; set; }
        public BasicText MovePointsInfo { get; set; }
        public BasicText DefenseInfo { get; set; }
        public BasicText HPInfo { get; set; }
        public BasicText DamageInfo { get; set; }
        public BasicText AttackDistanceInfo { get; set; }
        public BasicText MoneyInfo { get; set; }

        new public Vector2 FrameSize
        {
            get
            {
                return base.FrameSize;
            }
        }

        new public Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                _UpPosition = new Vector2(base.Position.X - FrameSize.X * 0.25f, base.Position.Y - FrameSize.Y * 0.5f);
                _DownPosition = new Vector2(base.Position.X, base.Position.Y);
                Art.Position = value + ArtOffset * Scale;
                CardName.Position = new Vector2(value.X + (FrameSize.X - CardName.Font.MeasureString(CardName.Text).X) / 2 * Scale.X, value.Y + CardName_OffsY * Scale.Y);
                DamageInfo.Position = new Vector2(value.X + (FirstStat_OffsX + (StatCellWidth - DamageInfo.Font.MeasureString(DamageInfo.Text).X) / 2) * Scale.X, value.Y + Stats_OffsY * Scale.Y);
                DefenseInfo.Position = new Vector2(value.X + (FirstStat_OffsX + StatCellWidth + (StatCellWidth - DefenseInfo.Font.MeasureString(DefenseInfo.Text).X) / 2) * Scale.X, value.Y + Stats_OffsY * Scale.Y);
                AttackDistanceInfo.Position = new Vector2(value.X + (FirstStat_OffsX + MidStat_OffsX + StatCellWidth * 2 + (StatCellWidth - AttackDistanceInfo.Font.MeasureString(AttackDistanceInfo.Text).X) / 2) * Scale.X, value.Y + Stats_OffsY * Scale.Y);
                MovePointsInfo.Position = new Vector2(value.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 3 + (StatCellWidth - MovePointsInfo.Font.MeasureString(MovePointsInfo.Text).X) / 2) * Scale.X, value.Y + Stats_OffsY * Scale.Y);
                HPInfo.Position = new Vector2(value.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 4 + (StatCellWidth - HPInfo.Font.MeasureString(HPInfo.Text).X) / 2) * Scale.X, value.Y + Stats_OffsY * Scale.Y);

                MoneyInfo.Position = new Vector2(value.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 4 + 1 + (StatCellWidth - MoneyInfo.Font.MeasureString(MoneyInfo.Text).X) / 2) * Scale.X, value.Y + 1 + (StatCellWidth * Scale.Y - MoneyInfo.Font.MeasureString(MoneyInfo.Text).Y * Scale.Y) / 2);
            }
        }

        new public Vector2 Scale
        {
            get
            {
                return base.Scale;
            }
            set
            {
                base.Scale = value;
                Art.Scale = value;
                CardName.Scale = value;
                DamageInfo.Scale = value;
                DefenseInfo.Scale = value;
                AttackDistanceInfo.Scale = value;
                MovePointsInfo.Scale = value;
                HPInfo.Scale = value;
                MoneyInfo.Scale = value;
                Art.Position = Position + ArtOffset*Scale;
                CardName.Position = new Vector2(Position.X + (FrameSize.X - CardName.Font.MeasureString(CardName.Text).X) / 2 * value.X, Position.Y + CardName_OffsY * value.Y);
                DamageInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + (StatCellWidth - DamageInfo.Font.MeasureString(DamageInfo.Text).X) / 2) * value.X, Position.Y + Stats_OffsY * value.Y);
                DefenseInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + StatCellWidth + (StatCellWidth - DefenseInfo.Font.MeasureString(DefenseInfo.Text).X) / 2) * value.X, Position.Y + Stats_OffsY * value.Y);
                AttackDistanceInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX + StatCellWidth * 2 + (StatCellWidth - AttackDistanceInfo.Font.MeasureString(AttackDistanceInfo.Text).X) / 2) * value.X, Position.Y + Stats_OffsY * value.Y);
                MovePointsInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 3 + (StatCellWidth - MovePointsInfo.Font.MeasureString(MovePointsInfo.Text).X) / 2) * value.X, Position.Y + Stats_OffsY * value.Y);
                HPInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 4 + (StatCellWidth - HPInfo.Font.MeasureString(HPInfo.Text).X) / 2) * value.X, Position.Y + Stats_OffsY * value.Y);

                MoneyInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 4 + 1 + (StatCellWidth - MoneyInfo.Font.MeasureString(MoneyInfo.Text).X) / 2) * value.X, Position.Y + 1 + (StatCellWidth * value.Y - MoneyInfo.Font.MeasureString(MoneyInfo.Text).Y * value.Y) / 2);
            }
        }

        new public float Layer
        {
            get
            {
                return base.Layer;
            }
            set
            {
                base.Layer = value;
                LastLayer = value;
                Art.Layer = base.Layer + 0.0001f;
                CardName.Layer = base.Layer - 0.0001f;
                DamageInfo.Layer = base.Layer - 0.0001f;
                DefenseInfo.Layer = base.Layer - 0.0001f;
                AttackDistanceInfo.Layer = base.Layer - 0.0001f;
                MovePointsInfo.Layer = base.Layer - 0.0001f;
                HPInfo.Layer = base.Layer - 0.0001f;
                MoneyInfo.Layer = base.Layer - 0.0001f;
            }
        }

        public Card(Vector2 Position, Texture2D Texture, Texture2D ArtTexture, Vector2 ArtOffset, int FrameSizeX, int FPS, int NotSelectedFrame, int DisabledFrame, Animation Selected, Animation Disappear, Animation Appear, Animation Choosed, int ClickedFrame, SpriteFont Font, Color TextColor, string CardName, string DamageInfo, string DefenseInfo, string AttackDistanceInfo, string MovePointsInfo, string HPInfo, string MoneyInfo, int CardName_OffsY, int Stats_OffsY, int FirstStat_OffsX, int MidStat_OffsX, int StatCellWidth, bool IsTargeted, MapZones AllowedZones = MapZones.ALL, CardCanUseOnTiles AllowedTiles = CardCanUseOnTiles.ONLY_NONE, float Layer = DefaultLayer) : base(Position, Texture, FrameSizeX, FPS, NotSelectedFrame, Selected, ClickedFrame, DisabledFrame, Layer)
        {
            _UpPosition = new Vector2(base.Position.X - FrameSize.X * 0.25f, base.Position.Y - FrameSize.Y * 0.5f);
            _DownPosition = new Vector2(base.Position.X, base.Position.Y);
            this.ArtOffset = ArtOffset;
            this.Stats_OffsY = Stats_OffsY;
            this.FirstStat_OffsX = FirstStat_OffsX;
            this.MidStat_OffsX = MidStat_OffsX;
            this.StatCellWidth = StatCellWidth;
            this.CardName = new BasicText(new Vector2(Position.X + (FrameSize.X - Font.MeasureString(CardName).X) / 2, Position.Y + CardName_OffsY), CardName, Font, TextColor, Layer - 0.0001f);
            this.CardName_OffsY = CardName_OffsY;
            this.DamageInfo = new BasicText(new Vector2(Position.X + FirstStat_OffsX + (StatCellWidth - Font.MeasureString(DamageInfo).X) / 2, Position.Y + Stats_OffsY), DamageInfo, Font, TextColor, Layer - 0.0001f);
            this.DefenseInfo = new BasicText(new Vector2(Position.X + FirstStat_OffsX + StatCellWidth + (StatCellWidth - Font.MeasureString(DefenseInfo).X) / 2, Position.Y + Stats_OffsY), DefenseInfo, Font, TextColor, Layer - 0.0001f);
            this.AttackDistanceInfo = new BasicText(new Vector2(Position.X + FirstStat_OffsX + MidStat_OffsX + StatCellWidth * 2 + (StatCellWidth - Font.MeasureString(AttackDistanceInfo).X) / 2, Position.Y + Stats_OffsY), AttackDistanceInfo, Font, TextColor, Layer - 0.0001f);
            this.MovePointsInfo = new BasicText(new Vector2(Position.X + FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 3 + (StatCellWidth - Font.MeasureString(MovePointsInfo).X) / 2, Position.Y + Stats_OffsY), MovePointsInfo, Font, TextColor, Layer - 0.0001f);
            this.HPInfo = new BasicText(new Vector2(Position.X + FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 4 + (StatCellWidth - Font.MeasureString(HPInfo).X) / 2, Position.Y + Stats_OffsY), HPInfo, Font, TextColor, Layer - 0.0001f);

            this.MoneyInfo = new BasicText(new Vector2(Position.X + FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 4 + 1 + (StatCellWidth - Font.MeasureString(MoneyInfo).X) / 2, Position.Y + 1 + (StatCellWidth - Font.MeasureString(MoneyInfo).Y) / 2), MoneyInfo, Font, Color.Khaki, Layer - 0.0001f);

            Art = new BasicSprite(Position + ArtOffset, ArtTexture, Layer + 0.0005f);
            AddAnimation("Disappear", Disappear);
            AddAnimation("Appear", Appear);
            AddAnimation("Choosed", Choosed);
            LastLayer = Layer;
            IsDown = true;
            IsPressed = false;
            LockClicking = false;
            IsMoving = false;
            this.IsTargeted = IsTargeted;
            MoveSpeed = 0;
            UpdBtnAnims = false;

            this.AllowedTiles = AllowedTiles;
            this.AllowedZones = AllowedZones;
        }

        public Card(Card card) : base(card.Position, card.Texture, (int)Math.Truncate(card.FrameSize.X), card.FPS, card.NotSelectedFrame, card.GetAnimation("Selected"), card.ClickedFrame, card.DisabledFrame, card.Layer)
        {
        	Art = new BasicSprite(card.Art.Position, card.Art.Texture, card.Art.Layer);
            CardName = new BasicText(card.CardName.Position, card.CardName.Text, card.CardName.Font, card.CardName.color, card.CardName.Layer);
                                     ;
            DamageInfo = new BasicText(card.DamageInfo.Position, card.DamageInfo.Text, card.DamageInfo.Font, card.DamageInfo.color, card.DamageInfo.Layer);
            DefenseInfo = new BasicText(card.DefenseInfo.Position, card.DefenseInfo.Text, card.DefenseInfo.Font, card.DefenseInfo.color, card.DefenseInfo.Layer);
            AttackDistanceInfo = new BasicText(card.AttackDistanceInfo.Position, card.AttackDistanceInfo.Text, card.AttackDistanceInfo.Font, card.AttackDistanceInfo.color, card.AttackDistanceInfo.Layer);
            MovePointsInfo = new BasicText(card.MovePointsInfo.Position, card.MovePointsInfo.Text, card.MovePointsInfo.Font, card.MovePointsInfo.color, card.MovePointsInfo.Layer);
            HPInfo = new BasicText(card.HPInfo.Position, card.HPInfo.Text, card.HPInfo.Font, card.HPInfo.color, card.HPInfo.Layer);

            MoneyInfo = new BasicText(card.MoneyInfo.Position, card.MoneyInfo.Text, card.MoneyInfo.Font, card.MoneyInfo.color, card.MoneyInfo.Layer);

            _UpPosition = card._UpPosition;
            _DownPosition = card._DownPosition;
            _MoveVector = card._MoveVector;
            AddAnimation("Disappear", card.GetAnimation("Disappear"));
            AddAnimation("Appear", card.GetAnimation("Appear"));
            AddAnimation("Choosed", card.GetAnimation("Choosed"));
            LastLayer = card.LastLayer;
            IsDown = card.IsDown;
            LockClicking = card.LockClicking;
            IsPressed = card.IsPressed;
            LockClicking = card.LockClicking;
            IsMoving = card.IsMoving;
            IsTargeted = card.IsTargeted;
            IsDisappearing = card.IsDisappearing;
            Position = card.Position;
            Scale = card.Scale;
            Stats_OffsY = card.Stats_OffsY;
            FirstStat_OffsX = card.FirstStat_OffsX;
            MidStat_OffsX = card.MidStat_OffsX;
            StatCellWidth = card.StatCellWidth;
            MoveSpeed = card.MoveSpeed;
            UpdBtnAnims = card.UpdBtnAnims;
            if(card.IsDisappearing)
            {
            	Art.Visible = false;
                CardName.Visible = false;
                DamageInfo.Visible = false;
                DefenseInfo.Visible = false;
                AttackDistanceInfo.Visible = false;
                MovePointsInfo.Visible = false;
                HPInfo.Visible = false;
                MoneyInfo.Visible = false;
            }

            AllowedTiles = card.AllowedTiles;
            AllowedZones = card.AllowedZones;
        }

        public void Appear()
        {
            if (IsDisappearing)
            {
                Art.Visible = true;
                CardName.Visible = true;
                DamageInfo.Visible = true;
                DefenseInfo.Visible = true;
                AttackDistanceInfo.Visible = true;
                MovePointsInfo.Visible = true;
                HPInfo.Visible = true;
                MoneyInfo.Visible = true;
                IsDisappearing = false;
                PlayAnimation("Appear");
            }
        }

        public void Disappear()
        {
            if (!IsDisappearing)
            {
                Art.Visible = false;
                CardName.Visible = false;
                DamageInfo.Visible = false;
                DefenseInfo.Visible = false;
                AttackDistanceInfo.Visible = false;
                MovePointsInfo.Visible = false;
                HPInfo.Visible = false;
                MoneyInfo.Visible = false;
                IsDisappearing = true;
                PlayAnimation("Disappear");
            }
        }

        private void Move()
        {
            if (IsMoving)
            {
                Position = new Vector2(_Position.X + _MoveVector.X, _Position.Y + _MoveVector.Y);
                if (moveiteration == MoveSpeed)
                {
                    LockClicking = false;
                    IsMoving = false;
                    IterationReset();
                }
            }
        }

        public void StartMove(Vector2 NewPoint, int MoveSpeed = 10)
        {
            this.MoveSpeed = MoveSpeed;
            LockClicking = true;
            IsMoving = true;
            _MoveVector = new Vector2((NewPoint.X - _Position.X) / MoveSpeed, (NewPoint.Y - _Position.Y) / MoveSpeed);
            moveiteration = 0;
        }

        public void Up()
        {
                Scale = new Vector2(1, 1);
                if (iteration != 60 && base.Position.Y > _UpPosition.Y)
                {
                    base.Position = new Vector2(_UpPosition.X, base.Position.Y - FrameSize.Y / (iteration * 8f));
                    Art.Position = Position + ArtOffset * Scale;
                    CardName.Position = new Vector2(Position.X + (FrameSize.X - CardName.Font.MeasureString(CardName.Text).X) / 2 * Scale.X, Position.Y + CardName_OffsY * Scale.Y);
                    DamageInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + (StatCellWidth - DamageInfo.Font.MeasureString(DamageInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
                    DefenseInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + StatCellWidth + (StatCellWidth - DefenseInfo.Font.MeasureString(DefenseInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
                    AttackDistanceInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX + StatCellWidth * 2 + (StatCellWidth - AttackDistanceInfo.Font.MeasureString(AttackDistanceInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
                    MovePointsInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 3 + (StatCellWidth - MovePointsInfo.Font.MeasureString(MovePointsInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
                    HPInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 4 + (StatCellWidth - HPInfo.Font.MeasureString(HPInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
                    MoneyInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 4 + 1 + (StatCellWidth - MoneyInfo.Font.MeasureString(MoneyInfo.Text).X) / 2) * Scale.X, Position.Y + 1 + (StatCellWidth * Scale.Y - MoneyInfo.Font.MeasureString(MoneyInfo.Text).Y * Scale.Y) / 2);
                }
                SetUpLayer();
                Art.Layer = base.Layer + 0.0001f;
                CardName.Layer = base.Layer - 0.0001f;
                IsDown = false;
        }

        public void SetUp()
        {
            Position = _DownPosition;
            Scale = new Vector2(1, 1);
            Art.Position = Position + ArtOffset * Scale;
            CardName.Position = new Vector2(Position.X + (FrameSize.X - CardName.Font.MeasureString(CardName.Text).X) / 2 * Scale.X, Position.Y + CardName_OffsY * Scale.Y);
            DamageInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + (StatCellWidth - DamageInfo.Font.MeasureString(DamageInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
            DefenseInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + StatCellWidth + (StatCellWidth - DefenseInfo.Font.MeasureString(DefenseInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
            AttackDistanceInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX + StatCellWidth * 2 + (StatCellWidth - AttackDistanceInfo.Font.MeasureString(AttackDistanceInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
            MovePointsInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 3 + (StatCellWidth - MovePointsInfo.Font.MeasureString(MovePointsInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
            HPInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 4 + (StatCellWidth - HPInfo.Font.MeasureString(HPInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
            MoneyInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 4 + 1 + (StatCellWidth - MoneyInfo.Font.MeasureString(MoneyInfo.Text).X) / 2) * Scale.X, Position.Y + 1 + (StatCellWidth * Scale.Y - MoneyInfo.Font.MeasureString(MoneyInfo.Text).Y * Scale.Y) / 2);
            SetUpLayer();
            Art.Layer = base.Layer + 0.0001f;
            CardName.Layer = base.Layer - 0.0001f;
            IsDown = false;
        }

        public void Down()
        {
            if (!IsDown)
            {
                Scale = new Vector2(0.5f, 0.5f);
                base.Position = _DownPosition;
                Art.Position = Position + ArtOffset * Scale;
                CardName.Position = new Vector2(Position.X + (FrameSize.X - CardName.Font.MeasureString(CardName.Text).X) / 2 * Scale.X, Position.Y + CardName_OffsY * Scale.Y);
                DamageInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + (StatCellWidth - DamageInfo.Font.MeasureString(DamageInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
                DefenseInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + StatCellWidth + (StatCellWidth - DefenseInfo.Font.MeasureString(DefenseInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
                AttackDistanceInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX + StatCellWidth * 2 + (StatCellWidth - AttackDistanceInfo.Font.MeasureString(AttackDistanceInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
                MovePointsInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 3 + (StatCellWidth - MovePointsInfo.Font.MeasureString(MovePointsInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
                HPInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 4 + (StatCellWidth - HPInfo.Font.MeasureString(HPInfo.Text).X) / 2) * Scale.X, Position.Y + Stats_OffsY * Scale.Y);
                MoneyInfo.Position = new Vector2(Position.X + (FirstStat_OffsX + MidStat_OffsX * 2 + StatCellWidth * 4 + 1 + (StatCellWidth - MoneyInfo.Font.MeasureString(MoneyInfo.Text).X) / 2) * Scale.X, Position.Y + 1 + (StatCellWidth * Scale.Y - MoneyInfo.Font.MeasureString(MoneyInfo.Text).Y * Scale.Y) / 2);
                base.Layer = LastLayer;
                Art.Layer = base.Layer + 0.0001f;
                CardName.Layer = base.Layer - 0.0001f;
                DamageInfo.Layer = base.Layer - 0.0001f;
                DefenseInfo.Layer = base.Layer - 0.0001f;
                AttackDistanceInfo.Layer = base.Layer - 0.0001f;
                MovePointsInfo.Layer = base.Layer - 0.0001f;
                HPInfo.Layer = base.Layer - 0.0001f;
                MoneyInfo.Layer = base.Layer - 0.0001f;
                IsDown = true;
            }
        }

        public void IterationReset()
        {
            iteration = 1;
        }

        public void MoveUpdate()
        {
            if (moveiteration < MoveSpeed)
                moveiteration++;
            Move();
        }

        public void SetUpLayer()
        {
            base.Layer = UpLayer;
            Art.Layer = base.Layer + 0.0001f;
            CardName.Layer = base.Layer - 0.0001f;
            DamageInfo.Layer = base.Layer - 0.0001f;
            DefenseInfo.Layer = base.Layer - 0.0001f;
            AttackDistanceInfo.Layer = base.Layer - 0.0001f;
            MovePointsInfo.Layer = base.Layer - 0.0001f;
            HPInfo.Layer = base.Layer - 0.0001f;
            MoneyInfo.Layer = base.Layer - 0.0001f;
        }

        public ButtonStates Update()
        {
            if (Visible)
            {
                if (iteration < 60)
                    iteration++;
                ButtonStates result;
                if (UpdBtnAnims)
                    result = base.Update(false);
                else
                    result = base.Update(true);
                if (IsPressed)
                {
                    base.Layer = UpLayer;
                    if (MouseControl.LeftBtn == MouseButtonStates.RELEASED)
                    {
                        IsPressed = false;
                        Layer = LastLayer;
                    }
                    if (result == ButtonStates.CLICKED)
                        return ButtonStates.CLICKED;
                    else
                        return ButtonStates.PRESSED;
                }
                else
                    if (!LockClicking)
                    {
                        if (IntersectionCheckResult != LastIntersectionCheckResult)
                            IterationReset();
                        if (result == ButtonStates.PRESSED)
                        {
                            //Down();
                            IsPressed = true;
                            SetUpLayer();
                        }
                        else
                            if (IntersectionCheckResult)
                                Up();
                            else
                                Down();
                        return result;
                    }
            }
                //if (CurrAnim != null)
                //    StopAnimation();
                return ButtonStates.NONE;
        }

        public override void Draw(SpriteBatch Target)
        {
            if (Visible)
            {
                var TmpCardNameColor = CardName.color;
                var TmpDamageInfoColor = DamageInfo.color;
                var TmpDefenseInfoColor = DefenseInfo.color;
                var TmpAttDistInfoColor = AttackDistanceInfo.color;
                var TmpMovePointsInfoColor = MovePointsInfo.color;
                var TmpHPInfoColor = HPInfo.color;
                var TmpMoneyInfoColor = MoneyInfo.color;
                if (!Enabled)
                {
                    CurrentFrame = DisabledFrame;
                    CardName.color = Color.Gray;
                    DamageInfo.color = Color.Gray;
                    DefenseInfo.color = Color.Gray;
                    AttackDistanceInfo.color = Color.Gray;
                    MovePointsInfo.color = Color.Gray;
                    HPInfo.color = Color.Gray;
                    MoneyInfo.color = Color.Gray;
                }

                Target.Draw(Texture, Position, null, new Rectangle(Convert.ToInt32(CurrentFrame * FrameSize.X), 0, Convert.ToInt32(FrameSize.X), Convert.ToInt32(FrameSize.Y)), RotationPoint, 0, Scale, Color.White, SpriteEffects.None, Layer);
                if (Art != null)
                    Art.Draw(Target);
                if (CardName != null)
                    CardName.Draw(Target);
                if (DamageInfo != null)
                    DamageInfo.Draw(Target);
                if (DefenseInfo != null)
                    DefenseInfo.Draw(Target);
                if (AttackDistanceInfo != null)
                    AttackDistanceInfo.Draw(Target);
                if (MovePointsInfo != null)
                    MovePointsInfo.Draw(Target);
                if (HPInfo != null)
                    HPInfo.Draw(Target);
                if (MoneyInfo != null)
                    MoneyInfo.Draw(Target);

                if (!Enabled)
                {
                    CardName.color = TmpCardNameColor;
                    DamageInfo.color = TmpDamageInfoColor;
                    DefenseInfo.color = TmpDefenseInfoColor;
                    AttackDistanceInfo.color = TmpAttDistInfoColor;
                    MovePointsInfo.color = TmpMovePointsInfoColor;
                    HPInfo.color = TmpHPInfoColor;
                    MoneyInfo.color = TmpMoneyInfoColor;
                }
            }
        }
    }
}
