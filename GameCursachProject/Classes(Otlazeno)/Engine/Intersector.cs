using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameCursachProject
{
    /// <summary>
    /// Осуществляет проверку пересечений
    /// (многоугольник/точка; многоугольник/многоугольник)
    /// методом порядка точки относительно кривой
    /// </summary>
    class Intersector
    {
        /// <summary>
        /// Набор точек многоугольника
        /// </summary>
        public Vector2[] Points { get; set; }

        /// <summary>
        /// Создает новый объект проверки пересечения
        /// </summary>
        /// <param name="Points"> Набор точек многоугольника</param>
        public Intersector(params Vector2[] Points)
        {
            this.Points = new Vector2[Points.Length];
            Points.CopyTo(this.Points, 0);
        }

        /// <summary>
        /// Создает прямоугольный объект проверки пересечения
        /// </summary>
        /// <param name="rect">Прямоугольник</param>
        public Intersector(Rectangle rect)
        {
            Points = new Vector2[4];
            Points[0] = new Vector2(rect.X, rect.Y);
            Points[1] = new Vector2(rect.X + rect.Width, rect.Y);
            Points[2] = new Vector2(rect.X + rect.Width, rect.Y + rect.Height);
            Points[3] = new Vector2(rect.X, rect.Y + rect.Height);
        }

        /// <summary>
        /// Получает набор точек по смещению относительно первой точки
        /// </summary>
        /// <param name="FirstPoint"> Первая точка</param>
        /// <param name="Offsets"> Смещения относительно первой точки</param>
        public void GetPointsFromOffsets(Vector2 FirstPoint, params Vector2[] Offsets)
        {
            Points = new Vector2[Offsets.Length + 1];
            Points[0] = FirstPoint;
            for (var i = 1; i < Points.Length; i++)
                Points[i] = new Vector2(FirstPoint.X + Offsets[i - 1].X, FirstPoint.Y + Offsets[i - 1].Y);
        }

        private static float isLeft(Vector2 P0, Vector2 P1, Vector2 P2)
        {
            return (P1.X - P0.X) * (P2.Y - P0.Y)
                    - (P2.X - P0.X) * (P1.Y - P0.Y);
        }

        public bool IntersectionCheck(Vector2 Point)
        {
            int wn = 0;
            for (int i = 0; i < Points.Length; i++)
            {   // edge from V[i] to  V[i+1]
                if (i+1 == Points.Length)
                {
                    if (Points[i].Y <= Point.Y)
                    {          // start y <= P.y
                        if (Points[0].Y > Point.Y)      // an upward crossing
                            if (isLeft(Points[i], Points[0], Point) > 0)  // P left of  edge
                                ++wn;            // have  a valid up intersect
                    }
                    else
                    {                        // start y > P.y (no test needed)
                        if (Points[0].Y <= Point.Y)     // a downward crossing
                            if (isLeft(Points[i], Points[0], Point) < 0)  // P right of  edge
                                --wn;            // have  a valid down intersect
                    }
                }
                else
                {
                    if (Points[i].Y <= Point.Y)
                    {          // start y <= P.y
                        if (Points[i + 1].Y > Point.Y)      // an upward crossing
                            if (isLeft(Points[i], Points[i + 1], Point) > 0)  // P left of  edge
                                ++wn;            // have  a valid up intersect
                    }
                    else
                    {                        // start y > P.y (no test needed)
                        if (Points[i + 1].Y <= Point.Y)     // a downward crossing
                            if (isLeft(Points[i], Points[i + 1], Point) < 0)  // P right of  edge
                                --wn;            // have  a valid down intersect
                    }
                }

            }

            if (wn == 0)
                return false;
            else
                return true;
        }
    }
}
