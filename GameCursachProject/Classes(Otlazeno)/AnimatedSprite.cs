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
    /// Анимация
    /// </summary>
    class Animation
    {
        //Номера первого и последнего кадра
        public int FirstFrame { get; set; }
        public int LastFrame { get; set; }
        //Текущий кадр
        public int CurrentFrame { get; set; }
        //Если true - зацикленная
        public bool Looped { get; set; }
        /// <summary>
        /// Создает анимацию
        /// </summary>
        /// <param name="FirstFrame">Номер первого кадра</param>
        /// <param name="LastFrame">Номер последнего кадра</param>
        /// <param name="Looped">True - зацикленная</param>
        public Animation(int FirstFrame, int LastFrame, bool Looped)
        {
            this.FirstFrame = FirstFrame;
            this.LastFrame = LastFrame;
            this.Looped = Looped;
            CurrentFrame = FirstFrame;
        }
        public Animation(Animation anim)
        {
            FirstFrame = anim.FirstFrame;
            LastFrame = anim.LastFrame;
            Looped = anim.Looped;
            CurrentFrame = anim.FirstFrame;
        }
        /// <summary>
        /// Устанавливает первый кадр
        /// </summary>
        public void ResetAnim()
        {
            CurrentFrame = FirstFrame;
        }
        /// <summary>
        /// Устанавливает следующий кадр
        /// </summary>
        /// <param name="CurrentFrame">Текущий кадр</param>
        /// <returns>Возвращает false, если конец анимации (для looped всегда true)</returns>
        public bool NextFrame(ref int CurrentFrame, bool Reverse = false)
        {
            if (Reverse)
            {
                this.CurrentFrame--;
                if (this.CurrentFrame < FirstFrame)
                {
                    this.CurrentFrame = LastFrame;
                    if (Looped)
                    {
                        CurrentFrame = this.CurrentFrame;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                this.CurrentFrame++;
                if (this.CurrentFrame > LastFrame)
                {
                    this.CurrentFrame = FirstFrame;
                    if (Looped)
                    {
                        CurrentFrame = this.CurrentFrame;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            CurrentFrame = this.CurrentFrame;
            return true;
        }
    }

    ///<summary>
    ///Анимированный спрайт
    ///</summary>
    class AnimatedSprite : BasicSprite, IDrawable
    {
        //Размер кадра
        protected Vector2 _FrameSize;
        //Кол-во кадров
        private int Frames;
        //Определяет, на какие значения FpsCounter отрисовывать кадр (60/FPS)
        private int DrawEvryNFrame;
        //Счётчик кадров
        private int FpsCounter;
        //Если true - проигрывать анимацию в обратном порядке
        private bool Reverse;
        //Текущий кадр
        public int CurrentFrame;
        //Имя текущей анимации
        public string CurrAnimName { get; set; }
        //Текущая анимация
        protected Animation CurrAnim;
        //Словарь с анимациями
        private Dictionary<string, Animation> Anims;
        //FPS
        public int FPS { get; set; }

        public Vector2 FrameSize
        {
            get
            {
                return _FrameSize;
            }
        }

        /// <summary>
        /// Создает анимированный спрайт
        /// </summary>
        /// <param name="Position">Координаты спрайта</param>
        /// <param name="Texture">Текстура спрайта</param>
        /// <param name="FrameSizeX">Размер кадра по X</param>
        /// <param name="FPS">Кол-во кадров в секунду в анимациях</param>
        /// <param name="Layer">Слой, на котором расположен спрайт</param>
        public AnimatedSprite(Vector2 Position, Texture2D Texture, int FrameSizeX, int FPS, float Layer = DefaultLayer) : base(Position, Texture, Layer)
        {
            _FrameSize = new Vector2(FrameSizeX, Texture.Height);
            Frames = Texture.Width / FrameSizeX;
            Anims = new Dictionary<string, Animation>();
            CurrAnim = null;
            CurrentFrame = 0;
            if (FPS > 60)
                throw new Exception("FPS не может превышать 60");
            DrawEvryNFrame = 60 / FPS;
            this.FPS = FPS;
            FpsCounter = 1;
        }
        /// <summary>
        /// Создает анимированный спрайт
        /// </summary>
        /// <param name="Position">Координаты спрайта</param>
        /// <param name="Texture">Текстура спрайта</param>
        /// <param name="FrameSizeX">Размер кадра по X</param>
        /// <param name="RotationPoint">Точка поворота спрайта</param>
        /// <param name="Rotation">Угол поворота спрайта в градусах</param>
        /// <param name="FPS">Кол-во кадров в секунду в анимациях</param>
        /// <param name="Layer">Слой, на котором расположен спрайт</param>
        public AnimatedSprite(Vector2 Position, Texture2D Texture, int FrameSizeX, Vector2 RotationPoint, float Rotation, int FPS, float Layer = DefaultLayer) : base(Position, Texture, RotationPoint, Rotation, Layer)
        {
            _FrameSize = new Vector2(FrameSizeX, Texture.Height);
            Frames = Texture.Width / FrameSizeX;
            Anims = new Dictionary<string, Animation>();
            CurrAnim = null;
            CurrentFrame = 0;
            if (FPS > 60)
                throw new Exception("FPS не может превышать 60");
            DrawEvryNFrame = 60 / FPS;
            this.FPS = FPS;
            FpsCounter = 1;
        }
        /// <summary>
        /// Добавляет анимацию
        /// </summary>
        /// <param name="Name">Название анимации</param>
        /// <param name="FirstFrame">Первый кадр</param>
        /// <param name="LastFrame">Последний кадр</param>
        /// <param name="Looped">True - зацикленная</param>
        public void AddAnimation(string Name, int FirstFrame, int LastFrame, bool Looped)
        {
            if (Anims.ContainsKey(Name))
                throw new Exception("Название анимации дублируется");
            else
            {
                Anims.Add(Name, new Animation(FirstFrame, LastFrame, Looped));
            }
        }
        /// <summary>
        /// Добавляет анимацию
        /// </summary>
        /// <param name="Name">Название анимации</param>
        /// <param name="Anim">Анимация</param>
        public void AddAnimation(string Name, Animation Anim)
        {
            if (Anims.ContainsKey(Name))
                throw new Exception("Название анимации дублируется");
            else
            {
                Anims.Add(Name, Anim);
            }
        }
        /// <summary>
        /// Заменяет анимацию с названием Name
        /// на новую анимацию
        /// </summary>
        /// <param name="Name">Название анимации</param>
        /// <param name="FirstFrame">Первый кадр</param>
        /// <param name="LastFrame">Последний кадр</param>
        /// <param name="Looped">True - зацикленная</param>
        public void ChangeAnimation(string Name, int FirstFrame, int LastFrame, bool Looped)
        {
            Anims.Remove(Name);
            Anims.Add(Name, new Animation(FirstFrame, LastFrame, Looped));
        }
        /// <summary>
        /// Заменяет анимацию с названием Name
        /// на новую анимацию
        /// </summary>
        /// <param name="Name">Название анимации</param>
        /// <param name="Anim">Анимация</param>
        public void ChangeAnimation(string Name, Animation Anim)
        {
            Anims.Remove(Name);
            Anims.Add(Name, Anim);
        }
        /// <summary>
        /// Удаляет анимацию
        /// </summary>
        /// <param name="Name">Название анимации</param>
        public void RemoveAnimation(string Name)
        {
            StopAnimation();
            Anims.Remove(Name);
        }
        /// <summary>
        /// Позволяет получить копию анимации
        /// </summary>
        /// <param name="Name">Название анимации</param>
        /// <returns> Копия анимации Name</returns>
        public Animation GetAnimation(string Name)
        {
            Animation Anim;
            Anims.TryGetValue(Name, out Anim);
            return new Animation(Anim);
        }
        /// <summary>
        /// Проигрывает анимацию
        /// </summary>
        /// <param name="Name">Название анимации</param>
        public void PlayAnimation(string Name, bool IsReverse = false)
        {
            if (!Anims.ContainsKey(Name))
                throw new Exception("Анимация не найдена");
            else
            {
                FpsCounter = 1;
                StopAnimation();
                CurrAnimName = Name;
                Anims.TryGetValue(Name,out CurrAnim);
                CurrAnim.ResetAnim();
                Reverse = IsReverse;
                if(Reverse)
                    CurrentFrame = CurrAnim.LastFrame;
                else
                    CurrentFrame = CurrAnim.FirstFrame;
            }
        }
        /// <summary>
        /// Останавливает текущую анимацию
        /// </summary>
        /// <param name="ToFirstFrame">Вернуть на первый кадр</param>
        public void StopAnimation(bool ToFirstFrame = false, int FirstFrame = 0)
        {
            CurrAnimName = "NONE";
            CurrAnim = null;
            if (ToFirstFrame)
                CurrentFrame = FirstFrame;
        }
        /// <summary>
        /// Обновление анимаций
        /// </summary>
        public virtual void UpdateAnims()
        {
            if (Visible && CurrAnim != null)
            {
                if (ProcessFPS())
                    if (!CurrAnim.Looped & !CurrAnim.NextFrame(ref CurrentFrame, Reverse))
                        StopAnimation();
            }
        }
        /// <summary>
        /// Выполняет отрисовку объекта
        /// </summary>
        /// <param name="Target">Объект, на котором рисуется спрайт</param>
        public override void Draw(SpriteBatch Target)
        {
            if (Visible)
                Target.Draw(Texture, Position, null, new Rectangle(Convert.ToInt32(CurrentFrame*_FrameSize.X), 0, Convert.ToInt32(_FrameSize.X), Convert.ToInt32(_FrameSize.Y)), RotationPoint, Rotation, Scale, Color.White, SpriteEffects.None, Layer);
        }
        /// <summary>
        /// Определяет, нужно ли изменять кадр
        /// </summary>
        /// <returns></returns>
        private bool ProcessFPS()
        {
            FpsCounter++;
            if (FpsCounter > 60)
                FpsCounter = 1;
            if (FpsCounter % DrawEvryNFrame == 0)
                return true;
            return false;
        }

    }
}
