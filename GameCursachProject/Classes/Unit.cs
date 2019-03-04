using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLua;

namespace GameCursachProject
{
    enum Side { PLAYER, OPPONENT }

	enum Directions { DOWN, DOWN_LEFT, UP_LEFT, UP, UP_RIGHT, DOWN_RIGHT, LEFT, RIGHT}
	
	struct MoveInfo
	{
		public Vector2 FirstPoint;
		public Vector2 LastPoint;
		public Directions Direct;
		
		public MoveInfo(Vector2 FirstPoint, Vector2 LastPoint, Directions Direct)
		{
			this.FirstPoint = FirstPoint;
			this.LastPoint = LastPoint;
			this.Direct = Direct;
		}
	}
	
    class Unit : AnimatedSprite, IDrawable
    {
        private bool IsSpawning;
        private bool IsMoving;
        private Vector2 LastScale;
        private Vector2 LastPosition;
        private int iteration = 0;
        private Queue<MoveInfo> MoveList;
        private Vector2 AddVector;
        //Attack script
        private bool IsAttacking;
        private LuaFunction AttScr_StartAttack;
        private LuaFunction AttScr_Update;
        private LuaFunction AttScr_Draw;
        private LuaTable ScriptRuntimeInfo;
        private Vector2 EnemyDamageFLPos;
        private SpriteFont FLFont;
        private Unit AttackedUnit;
        private bool IsDestroying;
        private bool IsDestroyAnimPlayed;

        public UnitInfo UI_UnitInfo;

        public Point ParentTile { get; set; }

        public int Speed { get; set; }
        private int _MovePointsLeft;
        private int _Armor;
        private int _Damage;
        private int _HP;
        public int AttackDistance { get; set; }
        public bool CanAttack { get; set; }

        private int AttackDamage;

        public int MovePointsLeft
        {
            get
            {
                return _MovePointsLeft;
            }
            set
            {
                _MovePointsLeft = value;
                UI_UnitInfo.MovePointsText = value.ToString();
            }
        }
        public int Armor
        {
            get
            {
                return _Armor;
            }
            set
            {
                _Armor = value;
                UI_UnitInfo.DefenseText = value.ToString();
            }
        }
        public int Damage
        {
            get
            {
                return _Damage;
            }
            set
            {
                _Damage = value;
                UI_UnitInfo.DamageText = value.ToString();
            }
        }
        public int HP
        {
            get
            {
                return _HP;
            }
            set
            {
                _HP = value;
                UI_UnitInfo.HPText = value.ToString();
            }
        }

        public Side side { get; set; }

        new public Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                UI_UnitInfo.Position = new Vector2(value.X + (FrameSize.X - UI_UnitInfo.Texture.Width) / 2, value.Y + FrameSize.Y);
            }
        }

        public bool UI_Visible
        {
        	get
        	{
        		return UI_UnitInfo.Visible;
        	}
        	set 
        	{
        		UI_UnitInfo.Visible = value;
        	}
        }

        public Unit(Vector2 Position, Texture2D Texture, Texture2D UInfoTexture, SpriteFont UInfoFont, Color UInfoColor, int FrameSizeX, int FPS, int Speed, int Damage, int HP, int Armor, int AttackDistance, Side side, string AttackScript, Script ScrEngine, Point ParentTile, Animation DestroyAnim, float Layer = DefaultLayer) : base(Position, Texture, FrameSizeX, FPS, Layer)
        {
            this.ParentTile = ParentTile;
            this.side = side;
        	MoveList = new Queue<MoveInfo>();
            FLFont = UInfoFont;
            UI_UnitInfo = new UnitInfo(new Vector2(Position.X + (FrameSizeX - UInfoTexture.Width) / 2, Position.Y + FrameSize.Y), UInfoTexture, UInfoFont, UInfoColor, Speed.ToString(), Armor.ToString(), Damage.ToString(), HP.ToString(), Layer - 0.001f);

            this.Speed = Speed;
            MovePointsLeft = Speed;
            this.Damage = Damage;
            this.HP = HP;
            this.Armor = Armor;
            this.AttackDistance = AttackDistance;

            ScrEngine.TextScript = AttackScript;
            ScrEngine.DoScript();
            AttScr_StartAttack = ScrEngine.GetFunc("Init");
            AttScr_Update = ScrEngine.GetFunc("Update");
            AttScr_Draw = ScrEngine.GetFunc("Draw");
            CanAttack = false;

            AddAnimation("Destroy", DestroyAnim);
        }

        public Unit(Unit OldUnit, Point ParentTile) : base(OldUnit.Position, OldUnit.Texture, (int)OldUnit.FrameSize.X, OldUnit.FPS, OldUnit.Layer)
        {
        	ScriptRuntimeInfo = OldUnit.ScriptRuntimeInfo;
            MoveList = new Queue<MoveInfo>(OldUnit.MoveList);
            IsSpawning = OldUnit.IsSpawning;
            IsMoving = OldUnit.IsMoving;
            LastScale = OldUnit.LastScale;
            LastPosition = OldUnit.LastPosition;
            iteration = OldUnit.iteration;
            AddVector = OldUnit.AddVector;

            UI_UnitInfo = new UnitInfo(new Vector2(Position.X + (FrameSize.X - OldUnit.UI_UnitInfo.Texture.Width) / 2, Position.Y + FrameSize.Y), OldUnit.UI_UnitInfo.Texture, OldUnit.UI_UnitInfo._DamageInfo.Font, OldUnit.UI_UnitInfo._DamageInfo.color, OldUnit.Speed.ToString(), OldUnit.Armor.ToString(), OldUnit.Damage.ToString(), OldUnit.HP.ToString(), OldUnit.Layer - 0.001f);
            UI_UnitInfo.Visible = OldUnit.UI_UnitInfo.Visible;

            Speed = OldUnit.Speed;
            MovePointsLeft = OldUnit.MovePointsLeft;
            Damage = OldUnit.Damage;
            HP = OldUnit.HP;
            Armor = OldUnit.Armor;
            AttackDistance = OldUnit.AttackDistance;

            AttScr_StartAttack = OldUnit.AttScr_StartAttack;
            AttScr_Update = OldUnit.AttScr_Update;
            AttScr_Draw = OldUnit.AttScr_Draw;
            side = OldUnit.side;

            FLFont = OldUnit.FLFont;
            this.ParentTile = ParentTile;
            CanAttack = OldUnit.CanAttack;

            AddAnimation("Destroy", OldUnit.GetAnimation("Destroy"));
        }

        public void Spawn()
        {
            if (!IsSpawning)
            {
                IsSpawning = true;
                LastScale = Scale;
                LastPosition = Position;
                Position = new Vector2(Position.X, Position.Y - 50);
                Scale = new Vector2(1.5f, 1.5f);
                iteration = 0;
            }
        }

        public void Attack(Unit EnemyUnit, int Damage)
        {
            if (!IsAttacking)
            {
                MovePointsLeft = 0;
                AttackDamage = Damage;
                CanAttack = false;
                IsAttacking = true;
                EnemyDamageFLPos = new Vector2(EnemyUnit.Position.X + EnemyUnit.FrameSize.X / 2, EnemyUnit.Position.Y + EnemyUnit.FrameSize.Y / 2);
                AttackedUnit = EnemyUnit;

                var tmp = AttScr_StartAttack.Call(this, EnemyUnit);
                if (tmp == null)
                {
                    ScriptRuntimeInfo = null;
                }
                else
                {
                    ScriptRuntimeInfo = tmp[0] as LuaTable;
                }
            }
        }

        public void Destroy()
        {
            IsDestroying = true;
            IsDestroyAnimPlayed = false;
        }

        public void MoveTo(Vector2 FirstPoint, Vector2 LastPoint, Directions Direct)
        {
            MoveList.Enqueue(new MoveInfo(FirstPoint, LastPoint, Direct));
        }
        
        public void SetFrame(Directions CurrDirect)
        {
            CurrentFrame = (int)CurrDirect;
        }

        public void SetFrame(int CurrDirect)
        {
            CurrentFrame = CurrDirect;
        }

        public void Update(Map map, Camera cam)
        {
            UpdateAnims();
            if (!IsAttacking && IsDestroying)
            {
                if (CurrAnimName == null || CurrAnimName == "NONE")
                {
                    if (IsDestroyAnimPlayed)
                    {
                        Visible = false;
                        map.RemoveUnit(this);
                    }
                    else
                    {
                        PlayAnimation("Destroy");
                        IsDestroyAnimPlayed = true;
                    }
                }
            }
            else
            {
                if (MoveList.Count > 0 && !(IsSpawning || IsMoving))
                {
                    var NextMove = MoveList.Dequeue();
                    AddVector = new Vector2((NextMove.FirstPoint.X - NextMove.LastPoint.X) / 20, (NextMove.FirstPoint.Y - NextMove.LastPoint.Y) / 20);
                    SetFrame(NextMove.Direct);
                    IsMoving = true;
                    iteration = 0;
                }
                if (iteration < 20)
                {
                    if (IsMoving)
                    {
                        Position -= AddVector;
                        if (iteration == 19)
                            IsMoving = false;
                    }
                    else
                        if (IsSpawning && iteration < 10)
                    {
                        Scale = new Vector2(Scale.X - 0.05f, Scale.Y - 0.05f);
                        Position = new Vector2(Position.X, Position.Y + 5);
                        if (iteration == 9)
                            IsSpawning = false;
                    }
                    iteration++;
                }
                if (IsAttacking)
                {
                    var tmp = AttScr_Update.Call(this, ScriptRuntimeInfo, IsAttacking);
                    if (tmp == null)
                    {
                        ScriptRuntimeInfo = null;
                    }
                    else
                    {
                        ScriptRuntimeInfo = tmp[0] as LuaTable;
                        IsAttacking = (bool)tmp[1];
                        if (!IsAttacking)
                        {
                            FlyingTextProcessor.CreateText(EnemyDamageFLPos, EnemyDamageFLPos - new Vector2(0, 300), 160, "-" + AttackDamage.ToString(), FLFont, Color.Red, new Vector2(4), 0.1f);
                            AttackedUnit.HP -= AttackDamage;
                            if (AttackedUnit.HP <= 0)
                                AttackedUnit.Destroy();
                        }
                    }
                }
            }
        }

        public void DrawUI(SpriteBatch Target, Camera cam)
        {
        	if (Visible)
            {             
        		UI_UnitInfo.Position = new Vector2(CoordsConvert.WorldToWindowCoords(Position, cam).X + (FrameSize.X * cam.Zoom - UI_UnitInfo.Texture.Width) / 2, CoordsConvert.WorldToWindowCoords(Position + FrameSize, cam).Y);
        		UI_UnitInfo.Draw(Target);
        	}
        }
        
        public override void Draw(SpriteBatch Target)
        {
            if (Visible)
            {
                if (IsAttacking)
                {
                	var tmp = AttScr_Draw.Call(this, Target, ScriptRuntimeInfo);
                	if(tmp == null)
            		{
            			ScriptRuntimeInfo = null;
            		}
            		else
            		{
            			ScriptRuntimeInfo = tmp[0] as LuaTable;
            		}
                }
                base.Draw(Target);
            }
        }
    }
}
