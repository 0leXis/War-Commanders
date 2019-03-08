using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{

    class Tile : Button, IDrawable
    {
        private bool LockClicking;
		private ButtonStates LastState;
        private BasicSprite Decoration;
		private Texture2D MainTexture;
		private Texture2D HighLitedTexture;
		
    	public MapTiles TileContains { get; set; }
        public int MovingPointsNeeded { get; set; }
        public int GiveArmor { get; set; }
        public Unit UnitOnTile { get; set; }
        
        new public Vector2 Position
        {
            get
            {
                return _Position;
            }
            set
            {
                _Position = new Vector2(value.X, value.Y);
                if (Text != null)
                    _Text.Position = new Vector2(_Position.X + _FrameSize.X * _Scale.X / 2 - _Text.Font.MeasureString(_Text.Text).X * _Scale.X / 2, _Position.Y + _FrameSize.Y * _Scale.Y / 2 - _Text.Font.MeasureString(_Text.Text).Y * _Scale.Y / 2);
                Intersector.GetPointsFromOffsets
                (
                    new Vector2(_Position.X, _Position.Y + _FrameSize.Y * _Scale.Y / 2), 
                    new Vector2(_FrameSize.X * _Scale.X / 4, -_FrameSize.Y * _Scale.Y / 2), 
                    new Vector2(3 * _FrameSize.X * _Scale.X / 4, -_FrameSize.Y * _Scale.Y / 2), 
                    new Vector2(_FrameSize.X * _Scale.X, 0),
                    new Vector2(3 * _FrameSize.X * _Scale.X / 4, _FrameSize.Y * _Scale.Y / 2),
                    new Vector2(_FrameSize.X * _Scale.X / 4, _FrameSize.Y * _Scale.Y / 2)
                );
                Decoration.Position = value;
            }
        }

        new public Vector2 Scale
        {
            get
            {
                return _Scale;
            }
            set
            {
                
                _Scale = value;
                if (Text != null)
                {
                    _Text.Position = new Vector2(_Position.X + _FrameSize.X * _Scale.X / 2 - _Text.Font.MeasureString(_Text.Text).X * _Scale.X / 2, _Position.Y + _FrameSize.Y * _Scale.Y / 2 - _Text.Font.MeasureString(_Text.Text).Y * _Scale.Y / 2);
                    _Text.Scale = value;
                }
                Intersector.GetPointsFromOffsets
                (
                    new Vector2(_Position.X, _Position.Y + _FrameSize.Y * _Scale.Y / 2),
                    new Vector2(_FrameSize.X * _Scale.X / 4, -_FrameSize.Y * _Scale.Y / 2),
                    new Vector2(3 * _FrameSize.X * _Scale.X / 4, -_FrameSize.Y * _Scale.Y / 2),
                    new Vector2(_FrameSize.X * _Scale.X, 0),
                    new Vector2(3 * _FrameSize.X * _Scale.X / 4, _FrameSize.Y * _Scale.Y / 2),
                    new Vector2(_FrameSize.X * _Scale.X / 4, _FrameSize.Y * _Scale.Y / 2)
                );
                Decoration.Scale = value;
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
                Decoration.Layer = value + 0.003f;
            }
        }

        public string TileName { get; set; }
        
        public Tile(Vector2 Position, Texture2D Texture, Texture2D HighLitedTexture, Texture2D DecorationTexture, int FrameSizeX, int FPS, int NotSelectedFrame, Animation Selected, int ClickedFrame, int MovingPointsNeeded, int GiveArmor, string TileName, float Layer = DefaultLayer) : base(Position, Texture, FrameSizeX, FPS, NotSelectedFrame, Selected, ClickedFrame, NotSelectedFrame, Layer)
        {
        	this.TileName = TileName;
        	TileContains = MapTiles.NONE;
            this.MovingPointsNeeded = MovingPointsNeeded;
            this.GiveArmor = GiveArmor;
            this.HighLitedTexture = HighLitedTexture;
            MainTexture = Texture;
            Decoration = new BasicSprite(Position, DecorationTexture, Layer + 0.003f);

            Intersector.GetPointsFromOffsets
            (
                new Vector2(_Position.X, _Position.Y + _FrameSize.Y * _Scale.Y / 2),
                new Vector2(_FrameSize.X * _Scale.X / 4, -_FrameSize.Y * _Scale.Y / 2),
                new Vector2(3 * _FrameSize.X * _Scale.X / 4, -_FrameSize.Y * _Scale.Y / 2),
                new Vector2(_FrameSize.X * _Scale.X, 0),
                new Vector2(3 * _FrameSize.X * _Scale.X / 4, _FrameSize.Y * _Scale.Y / 2),
                new Vector2(_FrameSize.X * _Scale.X / 4, _FrameSize.Y * _Scale.Y / 2)
            );
        }

        public void SpawnUnit(Unit UnitToSpawn, MapZones Side, bool UI_Show)
        {
            UnitToSpawn.Position = new Vector2(Position.X, Position.Y);
            if (Side == MapZones.RIGHT)
                UnitToSpawn.CurrentFrame = 1;
            else
                UnitToSpawn.CurrentFrame = 5;
            UnitOnTile = UnitToSpawn;
            UnitToSpawn.Spawn();
            if (TileContains == MapTiles.NONE)
                TileContains = MapTiles.WITH_UNIT;
            else
            	TileContains = MapTiles.WITH_UNIT_AND_BUILDING;
            if (!UI_Show)
                UnitToSpawn.UI_Visible = false;
        }

        public void UpdateUnit(Map map, Camera cam)
        {
            if (UnitOnTile != null)
                UnitOnTile.Update(map, cam);
        }

        public void HighLite()
        {
        	Texture = HighLitedTexture;
        }
        
        public void DeHighLite()
        {
        	Texture = MainTexture;
        }
		
        public override ButtonStates Update(bool DontUpdBtnAnims = false, Camera cam = null)
        {
            if(LockClicking)
                return base.Update(true, cam);
            return base.Update(DontUpdBtnAnims, cam);
        }

        public void DrawUnit(SpriteBatch Target)
        {
            if (UnitOnTile != null)
                UnitOnTile.Draw(Target);
        }

        public void DrawDecoration(SpriteBatch Target)
        {
            Decoration.Draw(Target);
        }

        public override void Draw(SpriteBatch Target)
        {
            base.Draw(Target);
        }
    }
}
