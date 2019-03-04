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
                                if (Tiles[i][j] != null)
                                    if (Tiles[i][j].TileContains == MapTiles.NONE)
                                        TileList.Add(new Point(i, j));
                        break;
                    case CardCanUseOnTiles.ONLY_WITH_BUILDING:
                        for (var i = 0; i < Tiles.Length; i++)
                            for (var j = 0; j < Tiles[0].Length; j++)
                                if (Tiles[i][j] != null)
                                    if (Tiles[i][j].TileContains == MapTiles.WITH_BUILDING)
                                    TileList.Add(new Point(i, j));
                        break;
                    case CardCanUseOnTiles.ONLY_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            for (var j = 0; j < Tiles[0].Length; j++)
                                if (Tiles[i][j] != null)
                                    if (Tiles[i][j].TileContains == MapTiles.WITH_UNIT)
                                    TileList.Add(new Point(i, j));
                        break;
                    case CardCanUseOnTiles.WITH_BUILDING_AND_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            for (var j = 0; j < Tiles[0].Length; j++)
                                if (Tiles[i][j] != null)
                                    if (Tiles[i][j].TileContains == MapTiles.WITH_UNIT_AND_BUILDING)
                                    TileList.Add(new Point(i, j));
                        break;
                    case CardCanUseOnTiles.NONE_OR_WITH_BUILDING:
                        for (var i = 0; i < Tiles.Length; i++)
                            for (var j = 0; j < Tiles[0].Length; j++)
                                if (Tiles[i][j] != null)
                                    if (Tiles[i][j].TileContains == MapTiles.NONE || Tiles[i][j].TileContains == MapTiles.WITH_BUILDING)
                                    TileList.Add(new Point(i, j));
                        break;
                    case CardCanUseOnTiles.NONE_OR_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            for (var j = 0; j < Tiles[0].Length; j++)
                                if (Tiles[i][j] != null)
                                    if (Tiles[i][j].TileContains == MapTiles.NONE || Tiles[i][j].TileContains == MapTiles.WITH_UNIT)
                                    TileList.Add(new Point(i, j));
                        break;
                    case CardCanUseOnTiles.WITH_BUILDING_OR_WITH_UNIT:
                        for (var i = 0; i < Tiles.Length; i++)
                            for (var j = 0; j < Tiles[0].Length; j++)
                                if (Tiles[i][j] != null)
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
                SetDefault();
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

        public Rectangle GetMapRectangle(int Offset)
        {
            var MapWidthWithOffset = new Point((int)((Tiles[0].Length / 2 + 3 + Offset) * Tiles[0][0].FrameSize.X + (Tiles[0].Length / 2 + Offset) * Tiles[0][0].FrameSize.X / 3), (int)((Tiles.Length + 4 + Offset) * Tiles[0][0].FrameSize.Y));
            return new Rectangle(Tiles[0][0].Position.ToPoint() - new Point(Offset * (int)Tiles[0][0].FrameSize.X, (Offset + 1) * (int)Tiles[0][0].FrameSize.Y), MapWidthWithOffset);
        }

        public Tile[][] GetMap()
        {
            return Tiles;
        }
    }
}