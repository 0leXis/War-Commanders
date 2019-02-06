using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    enum MapZones { ALL, LEFT, RIGHT}

    enum MapTiles{ NONE, WITH_UNIT, WITH_BUILDING, WITH_UNIT_AND_BUILDING }

    partial class Map
    {
        private Tile[][] Tiles;
        private List<Point> ChangedAnimTiles;
        private List<Arrow> PathFindingArrows;
        private Texture2D ArrowSegment;
        private Texture2D ArrowEndSegment;

        private Texture2D CPNeutral;
        private Texture2D CPAllied;
        private Texture2D CPEnemy;

        private BasicSprite Attack_Radius;

        private TileInfo Info;
        private bool _UI_VisibleState;

        private int _ChoosedTileI = -1;
        private int _ChoosedTileJ = -1;

        public bool IsAttack { get; set; }
        public Point AttackStart { get; set; }

        public bool IsPathFinding { get; set; }
        public Point ActionStartPoint { get; set; }
        
        public int ChoosedTileI { get { return _ChoosedTileI; } }
        public int ChoosedTileJ { get { return _ChoosedTileJ; } }
        public int DefaultNotSelectedFrame { get; set; }
        public int DefaultClickedFrame { get; set; }
        public Animation DefaultSelectedAnim { get; set; }
        public Point SelectedTile { get; set; }
       
        public bool UI_VisibleState { get { return _UI_VisibleState;} }

        private BasicSprite[] ControlPoints;

        public bool IsAnimsChanged
        {
            get
            {
                if (ChangedAnimTiles.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public Map(Tile[][] Tiles, int DefaultNotSelectedFrame, int DefaultClickedFrame, Animation DefaultSelectedAnim, Texture2D ArrowSegment, Texture2D ArrowEndSegment, Texture2D TileInfoTexture, Texture2D CPNeutral, Texture2D CPAllied, Texture2D CPEnemy, Texture2D Attack_Radius, SpriteFont InfoFont, Color InfoColor, Point[] CPTiles)
        {
            ChangedAnimTiles = new List<Point>();
            PathFindingArrows = new List<Arrow>();
            _UI_VisibleState = true;
            SelectedTile = new Point(-1, -1);
            _ChoosedTileI = -1;
            _ChoosedTileJ = -1;
            Info = new TileInfo(new Vector2(0, 0), TileInfoTexture, InfoFont, InfoColor, "", "", Tiles[0][0].Layer - 0.2f)
            {
                Visible = false
            };

            this.Attack_Radius = new BasicSprite(Vector2.Zero, Attack_Radius, Tiles[0][0].Layer + 0.002f);
            this.Attack_Radius.Visible = false;

            this.Tiles = new Tile[Tiles.Length][];
            this.ArrowSegment = ArrowSegment;
            this.ArrowEndSegment = ArrowEndSegment;
            for (var i = 0; i < Tiles.Length; i++)
            {
                this.Tiles[i] = new Tile[Tiles[0].Length];
                Tiles[i].CopyTo(this.Tiles[i], 0);
            }
            this.DefaultNotSelectedFrame = DefaultNotSelectedFrame;
            this.DefaultClickedFrame = DefaultClickedFrame;
            this.DefaultSelectedAnim = DefaultSelectedAnim;

            this.CPNeutral = CPNeutral;
            this.CPAllied = CPAllied;
            this.CPEnemy = CPEnemy;
            ControlPoints = new BasicSprite[CPTiles.Length];
            for(var i = 0; i < CPTiles.Length; i++)
            {
                ControlPoints[i] = new BasicSprite(Tiles[CPTiles[i].X][CPTiles[i].Y].Position, CPNeutral, Tiles[0][0].Layer + 0.001f);
            }
        }
                
        public void UpdateAllTiles(Camera cam)
        {
            for (var i = 0; i < Tiles.Length; i++)
                for (var j = 0; j < Tiles[0].Length; j++)
                    if (!(i == _ChoosedTileI && j == _ChoosedTileJ) && Tiles[i][j] != null)
                        Tiles[i][j].Update(cam: cam);
        }

        public void Update(ref bool IsMouseHandled, Hand hand, Camera cam, bool IsPlayerTurn)
        {
        	if(MouseControl.IsRightBtnClicked)
        	{
                if (IsPathFinding)
                {
                    IsPathFinding = false;
                    SetDefault();
                    UpdateAllTiles(cam);
                    CreatePathArrows(null, cam);
                }
                else
                if (IsAttack)
                {
                    IsAttack = false;
                    SetDefault();
                    UpdateAllTiles(cam);
                    CreatePathArrows(null, cam);
                }
                else
                    DeSelectTile(SelectedTile);
        	}
            if (_ChoosedTileI == -1 || _ChoosedTileJ == -1)
            {
                for (var i = 0; i < Tiles.Length; i++)
                    for (var j = 0; j < Tiles[0].Length; j++)
                        if (Tiles[i][j] != null)
                        {
                            Tiles[i][j].UpdateAnims();
                            if (!IsMouseHandled && Tiles[i][j].Update(cam: cam) != ButtonStates.NONE)
                            {
                                IsMouseHandled = true;
                                _ChoosedTileI = i;
                                _ChoosedTileJ = j;
                            }
                            else
                                Tiles[i][j].StopAnimation(true, Tiles[i][j].NotSelectedFrame);
                            Tiles[i][j].UpdateUnit(this, cam);
                        }
            }
            else
                if (Tiles[_ChoosedTileI][_ChoosedTileJ] != null)
                {
                    Tiles[_ChoosedTileI][_ChoosedTileJ].UpdateAnims();
                    var UpdateResult = Tiles[_ChoosedTileI][_ChoosedTileJ].Update(cam: cam);
                    if (!IsMouseHandled && UpdateResult != ButtonStates.NONE)
                    {
                        IsMouseHandled = true;
                        if(UpdateResult == ButtonStates.CLICKED)
                        {
                        	if(IsPathFinding)
                        	{
                        		IsPathFinding = false;
                        		int PL;
                        		List<Point> Marked;

                        		UnitMove(PathFinding(ActionStartPoint.X, ActionStartPoint.Y, _ChoosedTileI, _ChoosedTileJ, Tiles[ActionStartPoint.X][ActionStartPoint.Y].UnitOnTile.MovePointsLeft, out PL, out Marked));

                        		SetDefault();
                                UpdateAllTiles(cam);
                        		CreatePathArrows(null, cam);
                        		DeSelectTile(SelectedTile);
                        		SelectTile(new Point(_ChoosedTileI, _ChoosedTileJ));
                        		//Переместить
                        	}
                            else
                            if (IsAttack)
                            {
                                IsAttack = false;

                                if(EnemyFinding(ActionStartPoint.X, ActionStartPoint.Y, Tiles[ActionStartPoint.X][ActionStartPoint.Y].UnitOnTile.AttackDistance).Contains(new Point(_ChoosedTileI, _ChoosedTileJ)))
                                    Tiles[ActionStartPoint.X][ActionStartPoint.Y].UnitOnTile.Attack(Tiles[_ChoosedTileI][_ChoosedTileJ].UnitOnTile);

                                SetDefault();
                                UpdateAllTiles(cam);
                                CreatePathArrows(null, cam);
                            }
                            else
                        	{
                        		if(SelectedTile.X != -1)
                        		{
                        			DeSelectTile(SelectedTile);
                        		}
                                if(IsPlayerTurn)
                        		    SelectTile(new Point(_ChoosedTileI, _ChoosedTileJ));
                        	}
                        }
                        
                        if(IsPathFinding)
                        {
                        	int PL;
                        	List<Point> Marked;
                        	CreatePathArrows(PathFinding(ActionStartPoint.X, ActionStartPoint.Y, _ChoosedTileI, _ChoosedTileJ, Tiles[ActionStartPoint.X][ActionStartPoint.Y].UnitOnTile.MovePointsLeft, out PL, out Marked), cam);
                        	foreach(var Til in Marked)
                        		if(!ChangedAnimTiles.Contains(Til))
                        			ChangedAnimTiles.Add(Til);
                        	foreach(var TilCoords in Marked)
                        		ChangeTilesAnims(2, 1, new Animation(1, 1, true), TilCoords);
                        }
                        else
                        if (IsAttack)
                        {
                            Attack_Radius.Visible = true;
                            var EnFind = EnemyFinding(ActionStartPoint.X, ActionStartPoint.Y, Tiles[ActionStartPoint.X][ActionStartPoint.Y].UnitOnTile.AttackDistance);
                            foreach (var Til in EnFind)
                                if (!ChangedAnimTiles.Contains(Til))
                                    ChangedAnimTiles.Add(Til);
                            foreach (var TilCoords in EnFind)
                                ChangeTilesAnims(2, 1, new Animation(1, 1, true), TilCoords);
                            if(EnFind.Contains(new Point(_ChoosedTileI, _ChoosedTileJ)))
                            {
                                var TmpList = new List<Point>();
                                TmpList.Add(new Point(ActionStartPoint.X, ActionStartPoint.Y));
                                TmpList.Add(new Point(_ChoosedTileI, _ChoosedTileJ));
                                CreatePathArrows(TmpList, cam);
                            }
                            else
                            {
                                CreatePathArrows(null, cam);
                            }
                        }

                        if(hand.ChoosedCard == -1)
                        {
                            Info.Appear();
                            Info.Position = new Vector2(MouseControl.X - Info.Texture.Width / 2, MouseControl.Y + 20);
                            Info.NeedMovePointsText = Tiles[_ChoosedTileI][_ChoosedTileJ].MovingPointsNeeded.ToString();
                        }
                    }
                    else
                    {
                        Info.Disappear();
                        Tiles[_ChoosedTileI][_ChoosedTileJ].StopAnimation(true, Tiles[_ChoosedTileI][_ChoosedTileJ].NotSelectedFrame);
                        _ChoosedTileI = -1;
                        _ChoosedTileJ = -1;
                    }
                    for (var i = 0; i < Tiles.Length; i++)
                        for (var j = 0; j < Tiles[0].Length; j++)
                            if (Tiles[i][j] != null)
                            {
                                Tiles[i][j].UpdateUnit(this, cam);
                            }
                }
            Info.Update();
            FlyingTextProcessor.Update();
        }
        
        public void Draw(SpriteBatch Target, Camera cam)//TODO: Dobavit param dla begin
        {

            foreach (var Tile in Tiles)
                foreach (var Til in Tile)
                    if (Til != null)
                    {
                        Til.DrawDecoration(Target);
                        Til.Draw(Target);
                    }

            foreach (var CP in ControlPoints)
                CP.Draw(Target);

            Attack_Radius.Draw(Target);

            Target.End();

            Target.Begin(SpriteSortMode.BackToFront);
            foreach (var arrow in PathFindingArrows)
            {
                arrow.Draw(Target);
            }
            Target.End();

            Target.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, cam.GetTransform(Target.GraphicsDevice));
                foreach (var Tile in Tiles)
                    foreach (var Til in Tile)
                        if (Til != null)
                            Til.DrawUnit(Target);

            FlyingTextProcessor.Draw(Target);
            Target.End();

            Target.Begin(SpriteSortMode.BackToFront);
                Info.Draw(Target);
            	foreach (var Tile in Tiles)
                	foreach (var Til in Tile)
                    	if (Til != null && Til.UnitOnTile != null)
                        	Til.UnitOnTile.DrawUI(Target, cam);
            Target.End();

            Target.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, cam.GetTransform(Target.GraphicsDevice));
        }
    }
}
