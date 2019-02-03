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
            if (startvers.Y % 2 == 0)
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
            if (Path != null && (Path[0].X != Path[Path.Count - 1].X || Path[0].Y != Path[Path.Count - 1].Y))
            {

                if (Tiles[Path[Path.Count - 1].X][Path[Path.Count - 1].Y].TileContains == MapTiles.WITH_BUILDING)
                    Tiles[Path[Path.Count - 1].X][Path[Path.Count - 1].Y].TileContains = MapTiles.WITH_UNIT_AND_BUILDING;
                else
                    Tiles[Path[Path.Count - 1].X][Path[Path.Count - 1].Y].TileContains = MapTiles.WITH_UNIT;
                Tiles[Path[Path.Count - 1].X][Path[Path.Count - 1].Y].UnitOnTile = new Unit(Tiles[Path[0].X][Path[0].Y].UnitOnTile, new Point(Path[Path.Count - 1].X, Path[Path.Count - 1].Y));
                RemoveUnit(Tiles[Path[0].X][Path[0].Y].UnitOnTile);
                for (var i = 0; i < Path.Count - 1; i++)
                {
                    Tiles[Path[Path.Count - 1].X][Path[Path.Count - 1].Y].UnitOnTile.MoveTo(GetTileCenterByIJ(Path[i].X, Path[i].Y), GetTileCenterByIJ(Path[i + 1].X, Path[i + 1].Y), GetDirection(Path[i], Path[i + 1]));
                }
            }
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
    }
}