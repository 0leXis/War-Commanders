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
        private List<Point> GetAttack(Point vers)
        {
            var list = new List<Point>();
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

        public List<Point> EnemyFinding(int starti, int startj, int UnitAttackRadius)
        {
            var NeedUse = new Queue<Point>();
            var NeedUseTmp = new Queue<Point>();
            var Radius = 0;

            var Used = new HashSet<Point>();
            var EnemyList = new List<Point>();

            NeedUse.Enqueue(new Point(starti, startj));

            while (Radius < UnitAttackRadius)
            {
                if (NeedUse.Count == 0)
                {
                    if (NeedUseTmp.Count == 0)
                        break;
                    NeedUse = new Queue<Point>(NeedUseTmp);
                    NeedUseTmp.Clear();
                    Radius++;
                }

                Point v = NeedUse.Dequeue();
                Used.Add(v);

                foreach (var rebro in GetAttack(v))
                {
                    if (!Used.Contains(rebro))
                    {
                        if (NeedUse.Contains(rebro) || NeedUseTmp.Contains(rebro) || Used.Contains(rebro))
                            continue;
                        else
                        {
                            NeedUseTmp.Enqueue(rebro);
                            if (Tiles[rebro.X][rebro.Y].UnitOnTile != null && Tiles[rebro.X][rebro.Y].UnitOnTile.side == Side.OPPONENT)
                                EnemyList.Add(rebro);
                        }
                    }
                }
            }
            return EnemyList;
        }

        public void HighLiteTilesWithEnemy()
        {
            Attack_Radius.Visible = true;

            var Radius = (Tiles[ActionStartPoint.X][ActionStartPoint.Y].FrameSize.X / 2 + Tiles[ActionStartPoint.X][ActionStartPoint.Y].FrameSize.X * Tiles[ActionStartPoint.X][ActionStartPoint.Y].UnitOnTile.AttackDistance - 1) * 2;
            Attack_Radius.Scale = new Vector2(Radius / Attack_Radius.Texture.Width);

            Attack_Radius.Position = Tiles[ActionStartPoint.X][ActionStartPoint.Y].Position - Tiles[ActionStartPoint.X][ActionStartPoint.Y].FrameSize * (Attack_Radius.Scale.X - 1) / 2;
            var EnFind = EnemyFinding(ActionStartPoint.X, ActionStartPoint.Y, Tiles[ActionStartPoint.X][ActionStartPoint.Y].UnitOnTile.AttackDistance);
            foreach (var Til in EnFind)
                if (!ChangedAnimTiles.Contains(Til))
                    ChangedAnimTiles.Add(Til);
            foreach (var TilCoords in EnFind)
                ChangeTilesAnims(2, 1, new Animation(1, 1, true), TilCoords);
        }
    }
}