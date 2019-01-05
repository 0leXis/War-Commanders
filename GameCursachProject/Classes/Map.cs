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

    class Map
    {
        private Tile[][] Tiles;
        private List<Point> ChangedAnimTiles;
        private List<Arrow> PathFindingArrows;
        private Texture2D ArrowSegment;
        private Texture2D ArrowEndSegment;

        private TileInfo Info;
        private bool _UI_VisibleState;

        private int _ChoosedTileI = -1;
        private int _ChoosedTileJ = -1;

        public bool IsPathFinding { get; set; }
        public Point PFStart { get; set; }
        
        public int ChoosedTileI { get { return _ChoosedTileI; } }
        public int ChoosedTileJ { get { return _ChoosedTileJ; } }
        public int DefaultNotSelectedFrame { get; set; }
        public int DefaultClickedFrame { get; set; }
        public Animation DefaultSelectedAnim { get; set; }
        public Point SelectedTile { get; set; }
       
        public bool UI_VisibleState { get { return _UI_VisibleState;} }

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

        public Map(Tile[][] Tiles, int DefaultNotSelectedFrame, int DefaultClickedFrame, Animation DefaultSelectedAnim, Texture2D ArrowSegment, Texture2D ArrowEndSegment, Texture2D TileInfoTexture, SpriteFont InfoFont, Color InfoColor)
        {
            ChangedAnimTiles = new List<Point>();
            PathFindingArrows = new List<Arrow>();
            _UI_VisibleState = true;
            SelectedTile = new Point(-1, -1);
            _ChoosedTileI = -1;
            _ChoosedTileJ = -1;
            Info = new TileInfo(new Vector2(0, 0), TileInfoTexture, InfoFont, InfoColor, "", "", Tiles[0][0].Layer - 0.0005f)
            {
                Visible = false
            };
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
        }

        public void ChangeTilesAnims(int NotSelectedFrame, int ClickedFrame, Animation Selected, params Point[] Coords)
        {
            for (var i = 0; i < Coords.Length; i++)
                if (Tiles[Coords[i].X][Coords[i].Y] != null)
                {
                    Tiles[Coords[i].X][Coords[i].Y].NotSelectedFrame = NotSelectedFrame;
                    Tiles[Coords[i].X][Coords[i].Y].ClickedFrame = ClickedFrame;
                    Tiles[Coords[i].X][Coords[i].Y].ChangeAnimation("Selected", Selected);
                }
        }

        public void SetDefaultAnims()
        {
            foreach(var Coord in ChangedAnimTiles)
                if (Tiles[Coord.X][Coord.Y] != null)
                {
                    Tiles[Coord.X][Coord.Y].NotSelectedFrame = DefaultNotSelectedFrame;
                    Tiles[Coord.X][Coord.Y].ClickedFrame = DefaultClickedFrame;
                    Tiles[Coord.X][Coord.Y].ChangeAnimation("Selected", DefaultSelectedAnim);
                }
            ChangedAnimTiles.Clear();
        }

        public Point[] GetTilesByAllowedZones(MapZones AllowedZones, CardCanUseOnTiles AllowedTiles)
        {
            var TileList = new List<Point>();
            if (AllowedZones == MapZones.ALL)
            {
                switch (AllowedTiles)
                {
                    case CardCanUseOnTiles.ONLY_NONE:
                        for (var i = 0; i < Tiles.Length; i++)
                            for (var j = 0; j < Tiles[0].Length; j++)
                                if (Tiles[i][j].TileContains == MapTiles.NONE)
                                    TileList.Add(new Point(i, j));
                        break;
                    case CardCanUseOnTiles.ONLY_WITH_BUILDING:
                        for (var i = 0; i < Tiles.Length; i++)
                            for (var j = 0; j < Tiles[0].Length; j++)
                                if (Tiles[i][j].TileContains == MapTiles.WITH_BUILDING)
                                    TileList.Add(new Point(i, j));
                        break;
                    case CardCanUseOnTiles.ONLY_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            for (var j = 0; j < Tiles[0].Length; j++)
                                if (Tiles[i][j].TileContains == MapTiles.WITH_UNIT)
                                    TileList.Add(new Point(i, j));
                        break;
                    case CardCanUseOnTiles.WITH_BUILDING_AND_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            for (var j = 0; j < Tiles[0].Length; j++)
                                if (Tiles[i][j].TileContains == MapTiles.WITH_UNIT_AND_BUILDING)
                                    TileList.Add(new Point(i, j));
                        break;
                    case CardCanUseOnTiles.NONE_OR_WITH_BUILDING:
                        for (var i = 0; i < Tiles.Length; i++)
                            for (var j = 0; j < Tiles[0].Length; j++)
                                if (Tiles[i][j].TileContains == MapTiles.NONE || Tiles[i][j].TileContains == MapTiles.WITH_BUILDING)
                                    TileList.Add(new Point(i, j));
                        break;
                    case CardCanUseOnTiles.NONE_OR_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            for (var j = 0; j < Tiles[0].Length; j++)
                                if (Tiles[i][j].TileContains == MapTiles.NONE || Tiles[i][j].TileContains == MapTiles.WITH_UNIT)
                                    TileList.Add(new Point(i, j));
                        break;
                    case CardCanUseOnTiles.WITH_BUILDING_OR_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            for (var j = 0; j < Tiles[0].Length; j++)
                                if (Tiles[i][j].TileContains == MapTiles.WITH_BUILDING || Tiles[i][j].TileContains == MapTiles.WITH_UNIT)
                                    TileList.Add(new Point(i, j));
                        break;
                }
            }
            else
                if (AllowedZones == MapZones.LEFT)
            {
                switch (AllowedTiles)
                {
                    case CardCanUseOnTiles.ONLY_NONE:
                        for (var i = 0; i < Tiles.Length; i++)
                            if (Tiles[i][0].TileContains == MapTiles.NONE)
                                    TileList.Add(new Point(i, 0));
                        break;
                    case CardCanUseOnTiles.ONLY_WITH_BUILDING:
                        for (var i = 0; i < Tiles.Length; i++)
                            if (Tiles[i][0].TileContains == MapTiles.WITH_BUILDING)
                                    TileList.Add(new Point(i, 0));
                        break;
                    case CardCanUseOnTiles.ONLY_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            if (Tiles[i][0].TileContains == MapTiles.WITH_UNIT)
                                    TileList.Add(new Point(i, 0));
                        break;
                    case CardCanUseOnTiles.WITH_BUILDING_AND_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            if (Tiles[i][0].TileContains == MapTiles.WITH_UNIT_AND_BUILDING)
                                    TileList.Add(new Point(i, 0));
                        break;
                    case CardCanUseOnTiles.NONE_OR_WITH_BUILDING:
                        for (var i = 0; i < Tiles.Length; i++)
                            if (Tiles[i][0].TileContains == MapTiles.NONE || Tiles[i][0].TileContains == MapTiles.WITH_BUILDING)
                                    TileList.Add(new Point(i, 0));
                        break;
                    case CardCanUseOnTiles.NONE_OR_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            if (Tiles[i][0].TileContains == MapTiles.NONE || Tiles[i][0].TileContains == MapTiles.WITH_UNIT)
                                    TileList.Add(new Point(i, 0));
                        break;
                    case CardCanUseOnTiles.WITH_BUILDING_OR_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            if (Tiles[i][0].TileContains == MapTiles.WITH_BUILDING || Tiles[i][0].TileContains == MapTiles.WITH_UNIT)
                                    TileList.Add(new Point(i, 0));
                        break;
                }
            }
            else
            {
                switch (AllowedTiles)
                {
                    case CardCanUseOnTiles.ONLY_NONE:
                        for (var i = 0; i < Tiles.Length; i++)
                            if (Tiles[i][Tiles[0].Length - 1].TileContains == MapTiles.NONE)
                                TileList.Add(new Point(i, Tiles[0].Length - 1));
                        break;
                    case CardCanUseOnTiles.ONLY_WITH_BUILDING:
                        for (var i = 0; i < Tiles.Length; i++)
                            if (Tiles[i][Tiles[0].Length - 1].TileContains == MapTiles.WITH_BUILDING)
                                TileList.Add(new Point(i, Tiles[0].Length - 1));
                        break;
                    case CardCanUseOnTiles.ONLY_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            if (Tiles[i][Tiles[0].Length - 1].TileContains == MapTiles.WITH_UNIT)
                                TileList.Add(new Point(i, Tiles[0].Length - 1));
                        break;
                    case CardCanUseOnTiles.WITH_BUILDING_AND_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            if (Tiles[i][Tiles[0].Length - 1].TileContains == MapTiles.WITH_UNIT_AND_BUILDING)
                                TileList.Add(new Point(i, Tiles[0].Length - 1));
                        break;
                    case CardCanUseOnTiles.NONE_OR_WITH_BUILDING:
                        for (var i = 0; i < Tiles.Length; i++)
                            if (Tiles[i][Tiles[0].Length - 1].TileContains == MapTiles.NONE || Tiles[Tiles[0].Length - 1][i].TileContains == MapTiles.WITH_BUILDING)
                                TileList.Add(new Point(i, Tiles[0].Length - 1));
                        break;
                    case CardCanUseOnTiles.NONE_OR_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            if (Tiles[i][Tiles[0].Length - 1].TileContains == MapTiles.NONE || Tiles[Tiles[0].Length - 1][i].TileContains == MapTiles.WITH_UNIT)
                                TileList.Add(new Point(i, Tiles[0].Length - 1));
                        break;
                    case CardCanUseOnTiles.WITH_BUILDING_OR_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            if (Tiles[i][Tiles[0].Length - 1].TileContains == MapTiles.WITH_BUILDING || Tiles[i][Tiles[0].Length - 1].TileContains == MapTiles.WITH_UNIT)
                                TileList.Add(new Point(i, Tiles[0].Length - 1));
                        break;
                }
            }
            if (ChangedAnimTiles.Count > 0)
                SetDefaultAnims();
            ChangedAnimTiles = new List<Point>(TileList);
            return TileList.ToArray();
        }

        public bool CheckTileAllowed(Point TileCoords, MapZones AllowedZones, CardCanUseOnTiles AllowedTiles)
        {
            if (AllowedZones == MapZones.RIGHT && TileCoords.Y != Tiles[0].Length - 1)
                return false;
            if (AllowedZones == MapZones.LEFT && TileCoords.Y != 0)
                return false;
            if (AllowedTiles == CardCanUseOnTiles.ONLY_NONE && Tiles[TileCoords.X][TileCoords.Y].TileContains != MapTiles.NONE)
                return false;
            if (AllowedTiles == CardCanUseOnTiles.ONLY_WITH_BUILDING && Tiles[TileCoords.X][TileCoords.Y].TileContains != MapTiles.WITH_BUILDING)
                return false;
            if (AllowedTiles == CardCanUseOnTiles.ONLY_WITH_UNIT && Tiles[TileCoords.X][TileCoords.Y].TileContains != MapTiles.WITH_UNIT)
                return false;
            if (AllowedTiles == CardCanUseOnTiles.WITH_BUILDING_AND_WITH_UNIT && Tiles[TileCoords.X][TileCoords.Y].TileContains != MapTiles.WITH_UNIT_AND_BUILDING)
                return false;
            if (AllowedTiles == CardCanUseOnTiles.NONE_OR_WITH_UNIT && !(Tiles[TileCoords.X][TileCoords.Y].TileContains == MapTiles.NONE || Tiles[TileCoords.X][TileCoords.Y].TileContains == MapTiles.WITH_UNIT))
                return false;
            if (AllowedTiles == CardCanUseOnTiles.NONE_OR_WITH_BUILDING && !(Tiles[TileCoords.X][TileCoords.Y].TileContains == MapTiles.NONE || Tiles[TileCoords.X][TileCoords.Y].TileContains == MapTiles.WITH_BUILDING))
                return false;
            if (AllowedTiles == CardCanUseOnTiles.WITH_BUILDING_OR_WITH_UNIT && !(Tiles[TileCoords.X][TileCoords.Y].TileContains == MapTiles.WITH_UNIT || Tiles[TileCoords.X][TileCoords.Y].TileContains == MapTiles.WITH_BUILDING))
                return false;
            return true;
        }

        public int[] GetTileIJByCoords(Vector2 Coords)//OPTIMIZATION NEED
        {
            for (var i = 0; i < Tiles.Length; i++)
                for (var j = 0; j < Tiles[0].Length; j++)
                    if (Tiles[i][j] != null)
                        if (Tiles[i][j].Intersector.IntersectionCheck(Coords))
                            return new int[2] { i, j };
            return null;
        }

        public Tile GetTile(int i, int j)
        {
            return Tiles[i][j];
        }

        public Vector2 GetTileCenterByIJ(int i, int j)
        {
            return new Vector2(Tiles[i][j].Position.X + Tiles[i][j].FrameSize.X / 2 * Tiles[i][j].Scale.X,
                               Tiles[i][j].Position.Y + Tiles[i][j].FrameSize.Y / 2 * Tiles[i][j].Scale.Y);
        }

        private List<Point> GetSmez(Point vers, int stopi, int stopj)
        {
            var list = new List<Point>();
            if (vers.X - 1 >= 0)
            {
            	if (Tiles[vers.X - 1][vers.Y] != null && (!(Tiles[vers.X - 1][vers.Y].TileContains == MapTiles.WITH_UNIT || Tiles[vers.X - 1][vers.Y].TileContains == MapTiles.WITH_UNIT_AND_BUILDING) || (vers.X - 1 == stopi && vers.Y == stopj)))
                    list.Add(new Point(vers.X - 1, vers.Y));
            }
            if (vers.X + 1 < Tiles.Length)
            {
                if (Tiles[vers.X + 1][vers.Y] != null && (!(Tiles[vers.X + 1][vers.Y].TileContains == MapTiles.WITH_UNIT || Tiles[vers.X + 1][vers.Y].TileContains == MapTiles.WITH_UNIT_AND_BUILDING) || (vers.X + 1 == stopi && vers.Y == stopj)))
                    list.Add(new Point(vers.X + 1, vers.Y));
            }
            if(vers.Y % 2 == 0)
            {
                if(vers.Y + 1 < Tiles[0].Length)
                {
                    if (Tiles[vers.X][vers.Y + 1] != null && (!(Tiles[vers.X][vers.Y + 1].TileContains == MapTiles.WITH_UNIT || Tiles[vers.X][vers.Y + 1].TileContains == MapTiles.WITH_UNIT_AND_BUILDING) || (vers.X == stopi && vers.Y + 1== stopj)))
                        list.Add(new Point(vers.X, vers.Y + 1));
                    if(vers.X - 1 >= 0)
                        if (Tiles[vers.X - 1][vers.Y + 1] != null && (!(Tiles[vers.X - 1][vers.Y + 1].TileContains == MapTiles.WITH_UNIT || Tiles[vers.X - 1][vers.Y + 1].TileContains == MapTiles.WITH_UNIT_AND_BUILDING) || (vers.X - 1 == stopi && vers.Y + 1 == stopj)))
                            list.Add(new Point(vers.X - 1, vers.Y + 1));
                }
                if (vers.Y - 1 >= 0)
                {
                    if (Tiles[vers.X][vers.Y - 1] != null && (!(Tiles[vers.X][vers.Y - 1].TileContains == MapTiles.WITH_UNIT || Tiles[vers.X][vers.Y - 1].TileContains == MapTiles.WITH_UNIT_AND_BUILDING) || (vers.X == stopi && vers.Y - 1 == stopj)))
                        list.Add(new Point(vers.X, vers.Y - 1));
                    if (vers.X - 1 >= 0)
                        if (Tiles[vers.X - 1][vers.Y - 1] != null && (!(Tiles[vers.X - 1][vers.Y - 1].TileContains == MapTiles.WITH_UNIT || Tiles[vers.X - 1][vers.Y - 1].TileContains == MapTiles.WITH_UNIT_AND_BUILDING) || (vers.X - 1 == stopi && vers.Y - 1 == stopj)))
                            list.Add(new Point(vers.X - 1, vers.Y - 1));
                }
            }
            else
            {
                if (vers.Y + 1 < Tiles[0].Length)
                {
                    if (Tiles[vers.X][vers.Y + 1] != null && (!(Tiles[vers.X][vers.Y + 1].TileContains == MapTiles.WITH_UNIT || Tiles[vers.X][vers.Y + 1].TileContains == MapTiles.WITH_UNIT_AND_BUILDING) || (vers.X == stopi && vers.Y + 1 == stopj)))
                        list.Add(new Point(vers.X, vers.Y + 1));
                    if (vers.X + 1 < Tiles.Length)
                        if (Tiles[vers.X + 1][vers.Y + 1] != null && (!(Tiles[vers.X + 1][vers.Y + 1].TileContains == MapTiles.WITH_UNIT || Tiles[vers.X + 1][vers.Y + 1].TileContains == MapTiles.WITH_UNIT_AND_BUILDING) || (vers.X + 1 == stopi && vers.Y + 1 == stopj)))
                            list.Add(new Point(vers.X + 1, vers.Y + 1));
                }
                if (vers.Y - 1 >= 0)
                {
                    if (Tiles[vers.X][vers.Y - 1] != null && (!(Tiles[vers.X][vers.Y - 1].TileContains == MapTiles.WITH_UNIT || Tiles[vers.X][vers.Y - 1].TileContains == MapTiles.WITH_UNIT_AND_BUILDING) || (vers.X == stopi && vers.Y - 1 == stopj)))
                        list.Add(new Point(vers.X, vers.Y - 1));
                    if (vers.X + 1 < Tiles.Length)
                        if (Tiles[vers.X + 1][vers.Y - 1] != null && (!(Tiles[vers.X + 1][vers.Y - 1].TileContains == MapTiles.WITH_UNIT || Tiles[vers.X + 1][vers.Y - 1].TileContains == MapTiles.WITH_UNIT_AND_BUILDING) || (vers.X + 1 == stopi && vers.Y - 1 == stopj)))
                            list.Add(new Point(vers.X + 1, vers.Y - 1));
                }
            }
            return list;
        }

        public List<Point> PathFinding(int starti, int startj, int stopi, int stopj, int MovePoints, out int PathLength, out List<Point> Marked)
        {
            var dlini = new double[Tiles.Length, Tiles[0].Length];
            var puti = new List<Point>[Tiles.Length, Tiles[0].Length];
            var marked = new List<Point>();
            for (var i = 0; i < Tiles.Length; i++)
                for (var j = 0; j < Tiles[0].Length; j++)
                    dlini[i, j] = double.PositiveInfinity;
            var used = new HashSet<Point>();
            var needuse = new HashSet<Point>();
            dlini[starti, startj] = 0;
            puti[starti, startj] = new List<Point>
            {
                new Point(starti, startj)
            };
            needuse.Add(new Point(starti, startj));

            while (needuse.Count > 0)
            {
                var min = double.PositiveInfinity;
                Point v = new Point(-1,-1);
                for(var i = 0; i < Tiles.Length; i++)
                    for (var j = 0; j < Tiles[0].Length; j++)
                    {
                        if(!used.Contains(new Point(i,j)) && dlini[i,j] < min)
                        {
                            min = dlini[i, j];
                            v = new Point(i, j);
                        }
                    }
                if (v.X == -1)
                    break;
                used.Add(v);
                needuse.Remove(v);
                foreach (var rebro in GetSmez(v, stopi, stopj))
                {
                    if (!used.Contains(rebro))
                        if (dlini[v.X, v.Y] + Tiles[rebro.X][rebro.Y].MovingPointsNeeded < dlini[rebro.X, rebro.Y] && dlini[v.X, v.Y] + Tiles[rebro.X][rebro.Y].MovingPointsNeeded <= MovePoints)
                        {
                    		marked.Add(new Point(rebro.X, rebro.Y));
                            dlini[rebro.X, rebro.Y] = dlini[v.X, v.Y] + Tiles[rebro.X][rebro.Y].MovingPointsNeeded;
                            puti[rebro.X, rebro.Y] = new List<Point>(puti[v.X, v.Y])
                            {
                                new Point(rebro.X, rebro.Y)
                            };
                            needuse.Add(rebro);
                        }
                }
            }
            PathLength = (int)dlini[stopi, stopj];
            Marked = marked;
            return puti[stopi, stopj];
        }

        public void CreatePathArrows(List<Point> Path, Camera cam)
        {
        	PathFindingArrows = new List<Arrow>();
        	if(Path != null)
        	{
            	PathFindingArrows = new List<Arrow>();
            	for (var i = 0; i < Path.Count - 1; i++)
            	{
            		var tmpVectStart = CoordsConvert.WorldToWindowCoords(GetTileCenterByIJ(Path[i].X, Path[i].Y), cam);
            		var tmpVectStop = CoordsConvert.WorldToWindowCoords(GetTileCenterByIJ(Path[i+1].X, Path[i+1].Y), cam);
                	PathFindingArrows.Add(new Arrow(tmpVectStart, tmpVectStop, ArrowSegment, ArrowEndSegment, 0.4f + i*0.001f));
                	PathFindingArrows.Last().Scale = 0.5f;
            	}
        	}
        }

        private Directions GetDirection(Point startvers, Point endvers)
        {
        	var raznost = endvers - startvers;
            if (raznost.X - 1 == 0 && raznost.Y == 0)
            {
            	return Directions.DOWN;
            }
            if (raznost.X + 1 == 0 && raznost.Y == 0)
            {
            	return Directions.UP;
            }
            if(startvers.Y % 2 == 0)
            {
            	if (raznost.X == 0 && raznost.Y + 1 == 0)
                   return Directions.DOWN_LEFT;
                if (raznost.X + 1 == 0 && raznost.Y + 1 == 0)
                   return Directions.UP_LEFT;
                if (raznost.X == 0 && raznost.Y - 1 == 0)
                   return Directions.DOWN_RIGHT;
                if (raznost.X + 1 == 0 && raznost.Y - 1 == 0)
                   return Directions.UP_RIGHT;
            }
            else
            { 
                if (raznost.X == 0 && raznost.Y + 1 == 0)
                   return Directions.UP_LEFT;
                if (raznost.X - 1 == 0 && raznost.Y + 1 == 0)
                   return Directions.DOWN_LEFT;
                if (raznost.X == 0 && raznost.Y - 1 == 0)
                   return Directions.UP_RIGHT;
                if (raznost.X - 1 == 0 && raznost.Y - 1 == 0)
                   return Directions.DOWN_RIGHT;
            }
            return Directions.DOWN;
        }
        
        public void UnitMove(List<Point> Path)
        {
        	if(Path != null && (Path[0].X != Path[Path.Count - 1].X || Path[0].Y != Path[Path.Count - 1].Y))
            {
        		if(Tiles[Path[0].X][Path[0].Y].TileContains == MapTiles.WITH_UNIT_AND_BUILDING)
        			Tiles[Path[0].X][Path[0].Y].TileContains = MapTiles.WITH_BUILDING;
        		else
        			Tiles[Path[0].X][Path[0].Y].TileContains = MapTiles.NONE;
        		
        		if(Tiles[Path[Path.Count - 1].X][Path[Path.Count - 1].Y].TileContains == MapTiles.WITH_BUILDING)
        			Tiles[Path[Path.Count - 1].X][Path[Path.Count - 1].Y].TileContains = MapTiles.WITH_UNIT_AND_BUILDING;
        		else
        			Tiles[Path[Path.Count - 1].X][Path[Path.Count - 1].Y].TileContains = MapTiles.WITH_UNIT;
        		
        		Tiles[Path[Path.Count - 1].X][Path[Path.Count - 1].Y].UnitOnTile = new Unit(Tiles[Path[0].X][Path[0].Y].UnitOnTile);
                Tiles[Path[0].X][Path[0].Y].UnitOnTile = null;

                for (var i = 0; i < Path.Count - 1; i++)
        		{
                    Tiles[Path[Path.Count - 1].X][Path[Path.Count - 1].Y].UnitOnTile.MoveTo(GetTileCenterByIJ(Path[i].X, Path[i].Y), GetTileCenterByIJ(Path[i + 1].X, Path[i + 1].Y), GetDirection(Path[i], Path[i+1]));
        		}
        	}
        }
        
        public void SelectTile(Point Til)
        {
        	SelectedTile = Til;
        	Tiles[Til.X][Til.Y].HighLite();
        }
        
        public void DeSelectTile(Point Til)
        {
        	if(SelectedTile.X != -1)
        	{
        		Tiles[Til.X][Til.Y].DeHighLite();
        		Tiles[SelectedTile.X][SelectedTile.Y].Update();
        		SelectedTile = new Point(-1, -1);
        	}
        }
                
        public void UpdateAllTiles()
        {
            for (var i = 0; i < Tiles.Length; i++)
                for (var j = 0; j < Tiles[0].Length; j++)
                    if (!(i == _ChoosedTileI && j == _ChoosedTileJ) && Tiles[i][j] != null)
                        Tiles[i][j].Update();
        }

        public void ShowUnitStats()
        {
            _UI_VisibleState = true;
            for (var i = 0; i < Tiles.Length; i++)
                for (var j = 0; j < Tiles[0].Length; j++)
                    if (Tiles[i][j] != null && Tiles[i][j].UnitOnTile != null)
                        Tiles[i][j].UnitOnTile.UI_Visible = true;
        }

        public void HideUnitStats()
        {
            _UI_VisibleState = false;
            for (var i = 0; i < Tiles.Length; i++)
                for (var j = 0; j < Tiles[0].Length; j++)
                    if (Tiles[i][j] != null && Tiles[i][j].UnitOnTile != null)
                        Tiles[i][j].UnitOnTile.UI_Visible = false;
        }

        public void HighLiteTilesWithPF()
        {
            int PL;
            List<Point> Marked;
            PathFinding(PFStart.X, PFStart.Y, 0, 0, Tiles[PFStart.X][PFStart.Y].UnitOnTile.MovePointsLeft, out PL, out Marked);
            foreach (var Til in Marked)
                if (!ChangedAnimTiles.Contains(Til))
                    ChangedAnimTiles.Add(Til);
            foreach (var TilCoords in Marked)
                ChangeTilesAnims(2, 1, new Animation(1, 1, true), TilCoords);
        }

        public void Update(ref bool IsMouseHandled, Hand hand, Camera cam)
        {
        	if(MouseControl.IsRightBtnClicked)
        	{
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
                            Tiles[i][j].UpdateUnit();
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
                        		UnitMove(PathFinding(PFStart.X, PFStart.Y, _ChoosedTileI, _ChoosedTileJ, Tiles[PFStart.X][PFStart.Y].UnitOnTile.MovePointsLeft, out PL, out Marked));
                        		SetDefaultAnims();
                                UpdateAllTiles();
                        		CreatePathArrows(null, cam);
                        		DeSelectTile(SelectedTile);
                        		SelectTile(new Point(_ChoosedTileI, _ChoosedTileJ));
                        		//Переместить
                        	}
                        	else
                        	{
                        		if(SelectedTile.X != -1)
                        		{
                        			DeSelectTile(SelectedTile);
                        		}
                        		SelectTile(new Point(_ChoosedTileI, _ChoosedTileJ));
                        	}
                        }
                        
                        if(IsPathFinding)
                        {
                        	int PL;
                        	List<Point> Marked;
                        	CreatePathArrows(PathFinding(PFStart.X, PFStart.Y, _ChoosedTileI, _ChoosedTileJ, Tiles[PFStart.X][PFStart.Y].UnitOnTile.MovePointsLeft, out PL, out Marked), cam);
                        	foreach(var Til in Marked)
                        		if(!ChangedAnimTiles.Contains(Til))
                        			ChangedAnimTiles.Add(Til);
                        	foreach(var TilCoords in Marked)
                        		ChangeTilesAnims(2, 1, new Animation(1, 1, true), TilCoords);
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
                                Tiles[i][j].UpdateUnit();
                            }
                }
            Info.Update();
        }

        
        
        public void Draw(SpriteBatch Target, Camera cam)//TODO: Dobavit param dla begin
        {

            foreach (var Tile in Tiles)
                foreach (var Til in Tile)
                    if (Til != null)
                        Til.Draw(Target);

            Target.End();
            Target.Begin(SpriteSortMode.BackToFront);
            	Info.Draw(Target);
            	foreach (var Tile in Tiles)
                	foreach (var Til in Tile)
                    	if (Til != null && Til.UnitOnTile != null)
                        	Til.UnitOnTile.DrawUI(Target, cam);
                foreach (var arrow in PathFindingArrows)
            	{
                	arrow.Draw(Target);
            	}
            Target.End();
            Target.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, cam.GetTransform(Target.GraphicsDevice));
        }
    }
}
