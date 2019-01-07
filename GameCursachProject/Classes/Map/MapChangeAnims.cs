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
            foreach (var Coord in ChangedAnimTiles)
                if (Tiles[Coord.X][Coord.Y] != null)
                {
                    Tiles[Coord.X][Coord.Y].NotSelectedFrame = DefaultNotSelectedFrame;
                    Tiles[Coord.X][Coord.Y].ClickedFrame = DefaultClickedFrame;
                    Tiles[Coord.X][Coord.Y].ChangeAnimation("Selected", DefaultSelectedAnim);
                }
            ChangedAnimTiles.Clear();
        }

        public void SelectTile(Point Til)
        {
            SelectedTile = Til;
            Tiles[Til.X][Til.Y].HighLite();
        }

        public void DeSelectTile(Point Til)
        {
            if (SelectedTile.X != -1)
            {
                Tiles[Til.X][Til.Y].DeHighLite();
                Tiles[SelectedTile.X][SelectedTile.Y].Update();
                SelectedTile = new Point(-1, -1);
            }
        }
    }
}