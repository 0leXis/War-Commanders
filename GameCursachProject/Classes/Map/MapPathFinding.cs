using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    partial class Map
    {
        private List<Point> GetSmez(Point vers, int stopi, int stopj, bool isfirst)
        {
            var list = new List<Point>();
            if(!isfirst && Tiles[vers.X][vers.Y].UnitOnTile != null)
            {
                return list;
            }
            if (vers.X - 1 >= 0)
            {
                if (Tiles[vers.X - 1][vers.Y] != null)
                    list.Add(new Point(vers.X - 1, vers.Y));
            }
            if (vers.X + 1 < Tiles.Length)
            {
                if (Tiles[vers.X + 1][vers.Y] != null)
                    list.Add(new Point(vers.X + 1, vers.Y));
            }
            if (vers.Y % 2 == 0)
            {
                if (vers.Y + 1 < Tiles[0].Length)
                {
                    if (Tiles[vers.X][vers.Y + 1] != null)
                        list.Add(new Point(vers.X, vers.Y + 1));
                    if (vers.X - 1 >= 0)
                        if (Tiles[vers.X - 1][vers.Y + 1] != null)
                            list.Add(new Point(vers.X - 1, vers.Y + 1));
                }
                if (vers.Y - 1 >= 0)
                {
                    if (Tiles[vers.X][vers.Y - 1] != null)
                        list.Add(new Point(vers.X, vers.Y - 1));
                    if (vers.X - 1 >= 0)
                        if (Tiles[vers.X - 1][vers.Y - 1] != null)
                            list.Add(new Point(vers.X - 1, vers.Y - 1));
                }
            }
            else
            {
                if (vers.Y + 1 < Tiles[0].Length)
                {
                    if (Tiles[vers.X][vers.Y + 1] != null)
                        list.Add(new Point(vers.X, vers.Y + 1));
                    if (vers.X + 1 < Tiles.Length)
                        if (Tiles[vers.X + 1][vers.Y + 1] != null)
                            list.Add(new Point(vers.X + 1, vers.Y + 1));
                }
                if (vers.Y - 1 >= 0)
                {
                    if (Tiles[vers.X][vers.Y - 1] != null)
                        list.Add(new Point(vers.X, vers.Y - 1));
                    if (vers.X + 1 < Tiles.Length)
                        if (Tiles[vers.X + 1][vers.Y - 1] != null)
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
            var first = true;
            dlini[starti, startj] = 0;
            puti[starti, startj] = new List<Point>
            {
                new Point(starti, startj)
            };
            needuse.Add(new Point(starti, startj));

            while (needuse.Count > 0)
            {
                var min = double.PositiveInfinity;
                Point v = new Point(-1, -1);
                for (var i = 0; i < Tiles.Length; i++)
                    for (var j = 0; j < Tiles[0].Length; j++)
                    {
                        if (!used.Contains(new Point(i, j)) && dlini[i, j] < min)
                        {
                            min = dlini[i, j];
                            v = new Point(i, j);
                        }
                    }
                if (v.X == -1)
                    break;
                used.Add(v);
                needuse.Remove(v);
                foreach (var rebro in GetSmez(v, stopi, stopj, first))
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
                first = false;
            }
            PathLength = (int)dlini[stopi, stopj];
            Marked = marked;
            return puti[stopi, stopj];
        }

        public void CreatePathArrows(List<Point> Path, Camera cam)
        {
            PathFindingArrows = new List<Arrow>();
            if (Path != null)
            {
                PathFindingArrows = new List<Arrow>();
                for (var i = 0; i < Path.Count - 1; i++)
                {
                    var tmpVectStart = CoordsConvert.WorldToWindowCoords(GetTileCenterByIJ(Path[i].X, Path[i].Y), cam);
                    var tmpVectStop = CoordsConvert.WorldToWindowCoords(GetTileCenterByIJ(Path[i + 1].X, Path[i + 1].Y), cam);
                    PathFindingArrows.Add(new Arrow(tmpVectStart, tmpVectStop, ArrowSegment, ArrowEndSegment, 0.4f + i * 0.001f));
                    PathFindingArrows.Last().Scale = 0.5f;
                }
            }
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
    }
}