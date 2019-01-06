using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    /// <summary>
    /// Состояния кнопки
    /// </summary>
    public enum ButtonStates
    { NONE, ENTERED, PRESSED, CLICKED }

    /// <summary>
    /// Кнопка
    /// </summary>
    class Button : AnimatedSprite, IDrawable
    {
        //Текст
        public BasicText Text { get; set; }
        //Определение пересечений с мышкой
        public Intersector Intersector { get; set; }
        //Кадр, когда кнопка не выбрана
        public int NotSelectedFrame { get; set; }
        //Кадр, когда кнопка нажата
        public int ClickedFrame { get; set; }
        //Кадр, когда кнопка отключена
        public int DisabledFrame { get; set; }

        //Предыдущий результат пересечения с курсором
        protected bool LastIntersectionCheckResult;
        //Результат пересечения с курсором
        protected bool IntersectionCheckResult;

        //Можно ли нажимать
        public bool Enabled { get; set; }
        //Координаты
        new public Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                //Изменить позицию кнопки, текста и координаты многоугольника пересечений
                base.Position = new Vector2(value.X, value.Y);
                if (Text != null)
                    Text.Position = new Vector2(Position.X + FrameSize.X * Scale.X / 2 - Text.Font.MeasureString(Text.Text).X * Scale.X / 2, Position.Y + FrameSize.Y * Scale.Y / 2 - Text.Font.MeasureString(Text.Text).Y * Scale.Y / 2);
                Intersector.GetPointsFromOffsets
                    (
                    Position, 
                    new Vector2(FrameSize.X * Scale.X, 0), 
                    new Vector2(FrameSize.X * Scale.X, FrameSize.Y * Scale.Y), 
                    new Vector2(0, FrameSize.Y * Scale.Y)
                    );
            }
        }
        //Координаты
        protected Vector2 _Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
            }
        }
        //Размер
        new public Vector2 Scale
        {
            get
            {
                return base.Scale;
            }
            set
            {
                //Изменить размер кнопки, текста и координаты многоугольника пересечений
                base.Scale = value;
                if (Text != null)
                {
                    Text.Position = new Vector2(Position.X + FrameSize.X * Scale.X / 2 - Text.Font.MeasureString(Text.Text).X * Scale.X / 2, Position.Y + FrameSize.Y * Scale.Y / 2 - Text.Font.MeasureString(Text.Text).Y * Scale.Y / 2);
                    Text.Scale = value;
                }
                Intersector.GetPointsFromOffsets
                    (
                    Position, 
                    new Vector2(FrameSize.X * Scale.X, 0), 
                    new Vector2(FrameSize.X * Scale.X, FrameSize.Y * Scale.Y), 
                    new Vector2(0, FrameSize.Y * Scale.Y)
                    );
            }
        }
        //Размер
        protected Vector2 _Scale
        {
            get
            {
                return base.Scale;
            }
            set
            {
                base.Scale = value;
            }
        }
        //Слой
        new public float Layer
        {
            get
            {
                return base.Layer;
            }
            set
            {
                if (Text == null)
                    base.Layer = value;
                else
                {
                    base.Layer = value + 0.0001f;
                    Text.Layer = value;
                }
            }
        }

        /// <summary>
        /// Создает кнопку
        /// </summary>
        /// <param name="Position">Координаты кнопки</param>
        /// <param name="Texture">Текстура кнопки</param>
        /// <param name="Text">Текст кнопки</param>
        /// <param name="Font">Шрифт</param>
        /// <param name="color">Цвет шрифта</param>
        /// <param name="FrameSizeX">Размер кадра по X</param>
        /// <param name="FPS">Кол-во кадров в секунду в анимациях</param>
        /// <param name="NotSelectedFrame">Кадр, когда кнопка не выбрана</param>
        /// <param name="Selected">Анимация, когда кнопка выбрана</param>
        /// <param name="ClickedFrame">Кадр, когда кнопка нажата</param>
        /// <param name="Layer">Слой, на котором расположена кнопка</param>
        public Button(Vector2 Position, Texture2D Texture, string Text, SpriteFont Font, Color color, int FrameSizeX, int FPS, int NotSelectedFrame, Animation Selected, int ClickedFrame, int DisabledFrame, float Layer = DefaultLayer) : base(Position, Texture, FrameSizeX, FPS, Layer+0.0001f)
        {
            AddAnimation("Selected", Selected);
            this.NotSelectedFrame = NotSelectedFrame;
            this.ClickedFrame = ClickedFrame;
            this.DisabledFrame = DisabledFrame;
            Intersector = new Intersector(Position, new Vector2(Position.X + FrameSize.X, Position.Y), new Vector2(Position.X + FrameSize.X, Position.Y + FrameSize.Y), new Vector2(Position.X, Position.Y + FrameSize.Y));
            this.Text = new BasicText(new Vector2(Position.X + FrameSize.X / 2 - Font.MeasureString(Text).X / 2, Position.Y + FrameSize.Y / 2 - Font.MeasureString(Text).Y / 2), Text, Font, color, Layer);
            Enabled = true;
        }
        /// <summary>
        /// Создает кнопку без текста
        /// </summary>
        /// <param name="Position">Координаты кнопки</param>
        /// <param name="Texture">Текстура кнопки</param>
        /// <param name="FrameSizeX">Размер кадра по X</param>
        /// <param name="FPS">Кол-во кадров в секунду в анимациях</param>
        /// <param name="NotSelectedFrame">Кадр, когда кнопка не выбрана</param>
        /// <param name="Selected">Анимация, когда кнопка выбрана</param>
        /// <param name="ClickedFrame">Кадр, когда кнопка нажата</param>
        /// <param name="Layer">Слой, на котором расположена кнопка</param>
        public Button(Vector2 Position, Texture2D Texture, int FrameSizeX, int FPS, int NotSelectedFrame, Animation Selected, int ClickedFrame, int DisabledFrame, float Layer = DefaultLayer) : base(Position, Texture, FrameSizeX, FPS, Layer + 0.0001f)
        {
            AddAnimation("Selected", Selected);
            this.NotSelectedFrame = NotSelectedFrame;
            this.ClickedFrame = ClickedFrame;
            this.DisabledFrame = DisabledFrame;
            Intersector = new Intersector(Position, new Vector2(Position.X + FrameSize.X, Position.Y), new Vector2(Position.X + FrameSize.X, Position.Y + FrameSize.Y), new Vector2(Position.X, Position.Y + FrameSize.Y));
            this.Text = null;
            Enabled = true;
        }

        /// <summary>
        /// Обновление кнопки
        /// </summary>
        /// <param name="DontUpdBtnAnims"> True - Не обновлять анимации</param>
        /// <returns></returns>
        public virtual ButtonStates Update( bool DontUpdBtnAnims = false, Camera cam = null)
        {
            if (Visible)
            {
                if (!Enabled)
                {
                    CurrentFrame = DisabledFrame;
                    return ButtonStates.NONE;
                }
                //Если отображение разрешено
                //Определить пересечение с курсором
                LastIntersectionCheckResult = IntersectionCheckResult;
                if(cam == null)
                	IntersectionCheckResult = Intersector.IntersectionCheck(new Vector2(MouseControl.X, MouseControl.Y));
                else
                	IntersectionCheckResult = Intersector.IntersectionCheck(MouseControl.MouseToWorldCoords(cam));
                if (IntersectionCheckResult)
                {
                    if (MouseControl.LeftBtn == MouseButtonStates.PRESSED)
                    {
                        if (!DontUpdBtnAnims)
                        {
                            StopAnimation(true, NotSelectedFrame);
                            CurrentFrame = ClickedFrame;
                        }
                        return ButtonStates.PRESSED;
                    }
                    else
                        if (MouseControl.LeftBtn == MouseButtonStates.RELEASED)
                        {
                            return ButtonStates.CLICKED;
                        }
                    else
                    {
                        if (!DontUpdBtnAnims)
                            if (CurrAnim == null)
                                PlayAnimation("Selected");
                        return ButtonStates.ENTERED;
                    }
                }
                if (!DontUpdBtnAnims)
                {
                    StopAnimation(true, NotSelectedFrame);
                    CurrentFrame = NotSelectedFrame;
                }
                return ButtonStates.NONE;
            }
            else
            {
                //Если отображение запрещено
                if (!DontUpdBtnAnims)
                  if (CurrAnim != null)
                    StopAnimation(true, NotSelectedFrame);
                return ButtonStates.NONE;
            }
        }

        /// <summary>
        /// Выполняет отрисовку объекта
        /// </summary>
        /// <param name="Target">Объект, на котором рисуется спрайт</param>
        public override void Draw(SpriteBatch Target)
        {
            if (Visible)
            {
                Target.Draw(Texture, Position, null, new Rectangle(Convert.ToInt32(CurrentFrame * FrameSize.X), 0, Convert.ToInt32(FrameSize.X), Convert.ToInt32(FrameSize.Y)), RotationPoint, 0, Scale, Color.White, SpriteEffects.None, Layer);
                if (Text != null)
                    Text.Draw(Target);
            }
        }
    }
}
