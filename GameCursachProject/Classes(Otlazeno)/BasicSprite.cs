using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    /// <summary>
    /// Спрайт без анимации
    /// </summary>
    class BasicSprite : IDrawable
    {
        //Слой по умолчанию
        public const float DefaultLayer = 0.5f;
        //Координаты, угол поворота, точка поворота, увеличение
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 RotationPoint { get; set; }
        public Vector2 Scale { get; set; }
        //Текстура
        public Texture2D Texture { get; set; }
        //Слой
        public float Layer { get; set; }
        //Если true - рисовать спрайт
        public bool Visible { get; set; }

        /// <summary>
        /// Создает спрайт
        /// </summary>
        /// <param name="Position">Координаты спрайта</param>
        /// <param name="Texture">Текстура спрайта</param>
        /// <param name="Layer">Слой, на котором расположен спрайт</param>
        public BasicSprite(Vector2 Position, Texture2D Texture, float Layer = DefaultLayer)
        {
            this.Position = Position;
            this.Texture = Texture;

            this.Layer = Layer;

            Rotation = 0;
            RotationPoint = Vector2.Zero;
            Scale = Vector2.One;

            Visible = true;
        }
        /// <summary>
        /// Создает спрайт
        /// </summary>
        /// <param name="Position">Координаты спрайта</param>
        /// <param name="Texture">Текстура спрайта</param>
        /// <param name="RotationPoint">Точка поворота спрайта</param>
        /// <param name="Rotation">Угол поворота спрайта в градусах</param>
        /// <param name="Layer">Слой, на котором расположен спрайт</param>
        public BasicSprite(Vector2 Position, Texture2D Texture, Vector2 RotationPoint, float Rotation, float Layer = DefaultLayer)
        {
            this.Position = Position;
            this.Texture = Texture;

            this.Layer = Layer;

            this.Rotation = Rotation;
            this.RotationPoint = RotationPoint;
            Scale = Vector2.One;

            Visible = true;
        }

        /// <summary>
        /// Увеличивает угол поворота спрайта на указанный угол
        /// </summary>
        /// <param name="Angle">Добавляемый угол</param>
        public void RotateOn(Single Angle)
        {
            Rotation += Angle;
            if (Rotation >= 360)
                Rotation -= 360;
            else
                if (Rotation <= 0)
                    Rotation += 360;
        }

        /// <summary>
        /// Выполняет отрисовку спрайта
        /// </summary>
        /// <param name="Target">Объект, на котором рисуется спрайт</param>
        public virtual void Draw(SpriteBatch Target)
        {
            if (Visible)
                Target.Draw(Texture, Position, null, null, RotationPoint, Rotation, Scale, Color.White, SpriteEffects.None, Layer);
        }
    }
}
