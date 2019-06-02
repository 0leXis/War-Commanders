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
    /// Текст
    /// </summary>
    class BasicText : IDrawable
    {   
        //Слой по умолчанию
        public const float DefaultLayer = 0.5f;
        //Координаты, угол поворота, точка поворота, увеличение
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 RotationPoint { get; set; }
        public Vector2 Scale { get; set; }
        //Текст, шрифт и цвет
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Color color { get; set; }
        //Слой
        public float Layer { get; set; }
        //Если true - рисовать спрайт
        public bool Visible { get; set; }

        /// <summary>
        /// Создает спрайт
        /// </summary>
        /// <param name="Position">Координаты спрайта</param>
        /// <param name="Text">Текст</param>
        /// <param name="Font">Шрифт</param>
        /// <param name="Color">Цвет текста</param>
        /// <param name="Layer">Слой, на котором расположен спрайт</param>
        public BasicText(Vector2 Position, string Text, SpriteFont Font, Color color, float Layer = DefaultLayer)
        {
            this.Position = Position;
            this.Text = Text;
            this.Font = Font;
            this.color = color;

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
        /// <param name="Text">Текст</param>
        /// <param name="Font">Шрифт</param>
        /// <param name="Color">Цвет текста</param>
        /// <param name="RotationPoint">Точка поворота спрайта</param>
        /// <param name="Rotation">Угол поворота спрайта в градусах</param>
        /// <param name="Layer">Слой, на котором расположен спрайт</param>
        public BasicText(Vector2 Position, string Text, SpriteFont Font, Color color, Vector2 RotationPoint, float Rotation, float Layer = DefaultLayer)
        {
            this.Position = Position;
            this.Text = Text;
            this.Font = Font;
            this.color = color;

            this.Layer = Layer;

            this.Rotation = Rotation;
            this.RotationPoint = RotationPoint;
            Scale = Vector2.One;

            Visible = true;
        }
        /// <summary>
        /// Выполняет отрисовку текста
        /// </summary>
        /// <param name="Target">Объект, на котором рисуется спрайт</param>
        public virtual void Draw(SpriteBatch Target)
        {
            if (Visible)
                Target.DrawString(Font, Text, Position, color, Rotation, RotationPoint, Scale, SpriteEffects.None, Layer);
        }
    }
}
