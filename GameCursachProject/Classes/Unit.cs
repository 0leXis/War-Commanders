using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
	enum Directions { DOWN, DOWN_LEFT, UP_LEFT, UP, UP_RIGHT, DOWN_RIGHT}
	
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
        public UnitInfo UI_UnitInfo;

        public int Speed { get; set; }
        public int MovePointsLeft { get; set; }
        public int Armor { get; set; }
        public int Damage { get; set; }
        public int HP { get; set; }

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

        public Unit(Vector2 Position, Texture2D Texture, Texture2D UInfoTexture, SpriteFont UInfoFont, Color UInfoColor, int FrameSizeX, int FPS, int Speed, int Damage, int HP, int Armor, float Layer = DefaultLayer) : base(Position, Texture, FrameSizeX, FPS, Layer)
        {
        	MoveList = new Queue<MoveInfo>();
            this.Speed = Speed;
            this.MovePointsLeft = Speed;
            this.Damage = Damage;
            this.HP = HP;
            this.Armor = Armor;
            UI_UnitInfo = new UnitInfo(new Vector2(Position.X + (FrameSizeX - UInfoTexture.Width) / 2, Position.Y + FrameSize.Y), UInfoTexture, UInfoFont, UInfoColor, Speed.ToString(), Armor.ToString(), Damage.ToString(), HP.ToString(), Layer - 0.001f);
        }

        public Unit(Unit OldUnit) : base(OldUnit.Position, OldUnit.Texture, (int)OldUnit.FrameSize.X, OldUnit.FPS, OldUnit.Layer)
        {
            MoveList = new Queue<MoveInfo>(OldUnit.MoveList);
            IsSpawning = OldUnit.IsSpawning;
            IsMoving = OldUnit.IsMoving;
            LastScale = OldUnit.LastScale;
            LastPosition = OldUnit.LastPosition;
            iteration = OldUnit.iteration;
            AddVector = OldUnit.AddVector;

            Speed = OldUnit.Speed;
            MovePointsLeft = OldUnit.MovePointsLeft;
            Damage = OldUnit.Damage;
            HP = OldUnit.HP;
            Armor = OldUnit.Armor;

            UI_UnitInfo = new UnitInfo(new Vector2(Position.X + (FrameSize.X - OldUnit.UI_UnitInfo.Texture.Width) / 2, Position.Y + FrameSize.Y), OldUnit.UI_UnitInfo.Texture, OldUnit.UI_UnitInfo._DamageInfo.Font, OldUnit.UI_UnitInfo._DamageInfo.color, OldUnit.Speed.ToString(), OldUnit.Armor.ToString(), OldUnit.Damage.ToString(), OldUnit.HP.ToString(), OldUnit.Layer - 0.001f);
            UI_UnitInfo.Visible = OldUnit.UI_UnitInfo.Visible;
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

        public void MoveTo(Vector2 FirstPoint, Vector2 LastPoint, Directions Direct)
        {
        	MoveList.Enqueue(new MoveInfo(FirstPoint, LastPoint, Direct));
        }
        
        public void SetFrame(Directions CurrDirect)
        {
        	switch (CurrDirect)
        	{
        		case Directions.DOWN:
        			CurrentFrame = 0;
        			break;
       			case Directions.DOWN_LEFT:
        			CurrentFrame = 1;
        			break;
        		case Directions.UP_LEFT:
        			CurrentFrame = 2;
        			break;
        		case Directions.UP:
        			CurrentFrame = 3;
        			break;
        		case Directions.UP_RIGHT:
        			CurrentFrame = 4;
        			break;
        		case Directions.DOWN_RIGHT:
        			CurrentFrame = 5;
        			break;
        	}
        }
        
        public void Update()
        {
        	if(MoveList.Count > 0 && !(IsSpawning || IsMoving))
        	{
        		var NextMove = MoveList.Dequeue();
        		AddVector = new Vector2((NextMove.FirstPoint.X - NextMove.LastPoint.X) / 20, (NextMove.FirstPoint.Y - NextMove.LastPoint.Y) / 20);
        		SetFrame(NextMove.Direct);
        		IsMoving = true;
        		iteration = 0;
        	}
        	if(iteration < 20)
        	{
        		if(IsMoving)
        		{
        			Position -= AddVector;
                    if(iteration == 19)
                    	IsMoving = false;
        		}
        		else
            		if(IsSpawning && iteration < 10)
            		{
                    	Scale = new Vector2(Scale.X - 0.05f, Scale.Y - 0.05f);
                    	Position = new Vector2(Position.X, Position.Y + 5);
                    	if(iteration == 9)
                    		IsSpawning = false;
            		}
            	iteration++;
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
                base.Draw(Target);
            }
        }
    }
}
