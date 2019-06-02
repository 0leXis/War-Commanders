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
    /// Стрелка с динамически добавляемыми сегментами,
    /// в зависимости от ее длины
    /// </summary>
    class Arrow : IDrawable
    {
        //Список сегментов стрелки
        private List<BasicSprite> _Segments;
        //Кол-во сегментов
        private int _SprCount;
        //
        private Vector2 _Distance;
        //Поворот стрелки
        private float _Rotation;
        //Начальная точка стрелки
        private Vector2 _BeginPoint;
        //Конечная точка стрелки
        private Vector2 _EndPoint;
        //Слой сегментов(кроме первого)
        private float _Layer;
        //Размер
        private float _Scale;
        //Исходный размер(для функций плавного исчезновения и появления)
        private float LastScale;
        //Итерация метода Update(для функций плавного исчезновения и появления)
        private int iteration = 1;
        //Флаги функций плавного исчезновения и появления
        private bool IsDisappearing, IsAppearing;

        /// <summary>
        /// Слой сегментов
        /// (слой первого сегмента = Layer - 0.01)
        /// </summary>
        public float Layer
        {
            get
            {
                return _Layer;
            }

            set
            {
                _Layer = value;
                foreach (var Segm in _Segments)
                    if (Segm != null)
                        Segm.Layer = _Layer;
                _Segments.First().Layer = _Layer - 0.01f;
            }

        }

        /// <summary>
        /// Размер стрелки
        /// </summary>
        public float Scale
        {
            get
            {
                return _Scale;
            }
            set
            {
                _Scale = value;
                LastScale = value;
                foreach (var Segm in _Segments)
                    if (Segm != null)
                        Segm.Scale = new Vector2(Scale, Scale);
                UpdateSegments();
            }
        }

        /// <summary>
        /// Разрешает отрисовку стрелки
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Начальная точка стрелки
        /// </summary>
        public Vector2 BeginPoint
        {
            get
            {
                return _BeginPoint;
            }
            set
            {
                _BeginPoint = new Vector2(value.X - EndSegmentTexture.Width / 2, value.Y);
                UpdateSegments();
            }
        }

        /// <summary>
        /// Конечная точка стрелки
        /// </summary>
        public Vector2 EndPoint
        {
            get
            {
                return _EndPoint;
            }
            set
            {
                _EndPoint = new Vector2(value.X - EndSegmentTexture.Width / 2, value.Y);
                UpdateSegments();
            }
        }

        /// <summary>
        /// Текстура основных сегментов
        /// </summary>
        private Texture2D SegmentTexture;
        /// <summary>
        /// Текстура последнего сегмента
        /// </summary>
        private Texture2D EndSegmentTexture;

        /// <summary>
        /// Создает стрелку
        /// </summary>
        /// <param name="BeginPoint">Начальная точка</param>
        /// <param name="EndPoint">Конечная точка</param>
        /// <param name="SegmentTexture">Текстура основных сегментов</param>
        /// <param name="EndSegmentTexture">Текстура последнего сегмента</param>
        /// <param name="Layer">Слой сегментов
        /// (слой первого сегмента = Layer - 0.01)</param>
        public Arrow(Vector2 BeginPoint, Vector2 EndPoint, Texture2D SegmentTexture, Texture2D EndSegmentTexture, float Layer = 0)
        {
            _Distance = new Vector2(EndPoint.X - BeginPoint.X, EndPoint.Y - BeginPoint.Y);
            _Segments = new List<BasicSprite>();
            _Rotation = Convert.ToSingle(Math.Atan2(_Distance.X, _Distance.Y));
            _Layer = Layer;
            _Scale = 1;
            LastScale = 1;

            this.SegmentTexture = SegmentTexture;
            this.EndSegmentTexture = EndSegmentTexture;
            _Segments.Add(new BasicSprite(_EndPoint, EndSegmentTexture, new Vector2(EndSegmentTexture.Width / 2, EndSegmentTexture.Height / 2), _Rotation, _Layer));

            this.EndPoint = EndPoint;
            this.BeginPoint = BeginPoint;

            Visible = true;
            IsDisappearing = false;
        }

        /// <summary>
        /// Обновление сегментов
        /// </summary>
        public void UpdateSegments()
        {
            _Distance = new Vector2(_EndPoint.X - _BeginPoint.X, _EndPoint.Y - _BeginPoint.Y);
            _Rotation = Convert.ToSingle(Math.Atan2(_Distance.Y, _Distance.X) + 1.57079632679);
            if (_Rotation < 0)
                _Rotation += 6.28318530718f;
            _SprCount = (int)Math.Round(Math.Sqrt(Math.Pow(Math.Abs(_Distance.X / _Scale), 2) + Math.Pow(Math.Abs(_Distance.Y / _Scale), 2)) / (SegmentTexture.Height / Scale) - 0.7);
            if (_SprCount < 1)
                _SprCount = 1;

            var AddX = (_EndPoint.X - _BeginPoint.X) / _SprCount;
            var AddY = (_EndPoint.Y - _BeginPoint.Y) / _SprCount;

            for (var i = 0; i < _SprCount; i++)
            {
                if (_Segments.Count <= i)
                    _Segments.Add(new BasicSprite(new Vector2(_EndPoint.X - (i + 1) * AddX + EndSegmentTexture.Width / 2 - SegmentTexture.Width / 2, _EndPoint.Y - (i + 1) * AddY), SegmentTexture, new Vector2(SegmentTexture.Width / 2, SegmentTexture.Height / 2), _Rotation, _Layer));
                else
                {
                    if (i == 0)
                        _Segments[i].Position = new Vector2(_EndPoint.X + EndSegmentTexture.Width / 2 - EndSegmentTexture.Width / 4 * (float)Math.Sin(_Rotation) * _Scale, _EndPoint.Y + EndSegmentTexture.Height / 2 * (float)Math.Cos(_Rotation) * _Scale);
                    else
                        _Segments[i].Position = new Vector2(_EndPoint.X - (i + 1) * AddX + EndSegmentTexture.Width / 2, _EndPoint.Y - (i + 1) * AddY);
                    _Segments[i].Rotation = _Rotation;
                }
                _Segments[i].Scale = new Vector2(Scale, Scale);
            }
        }

        /// <summary>
        /// Плавное появление
        /// </summary>
        public void Appear()
        {
            if (!IsAppearing)
            {
                IsAppearing = true;
                IsDisappearing = false;
                _Scale = 0;
                iteration = 1;
            }
        }

        /// <summary>
        /// Плавное исчезновение
        /// </summary>
        public void Disappear()
        {
            if (!IsDisappearing)
            {
                IsAppearing = false;
                IsDisappearing = true;
                _Scale = LastScale;
                iteration = 1;
            }
        }

        /// <summary>
        /// Обновление объекта
        /// </summary>
        public void Update()
        {
            if (iteration != 11)
            {
                if (IsDisappearing)
                {
                    if (iteration == 9)
                        Visible = false;
                    else
                        _Scale = _Scale - LastScale / 10;
                }
                if (IsAppearing)
                {
                    if (iteration == 1)
                        Visible = true;
                    else
                        _Scale = _Scale + LastScale / 10;
                }
                foreach (var Segm in _Segments)
                    if (Segm != null)
                        Segm.Scale = new Vector2(Scale, Scale);
                iteration++;
            }
        }

        /// <summary>
        /// Выполняет отрисовку объекта
        /// </summary>
        /// <param name="Target">Объект, на котором рисуется спрайт</param>
        public void Draw(SpriteBatch Target)
        {
            if (Visible)
                for(var i = 0; i < _SprCount; i++)
                    _Segments[i].Draw(Target);
        }
        
    }
}
