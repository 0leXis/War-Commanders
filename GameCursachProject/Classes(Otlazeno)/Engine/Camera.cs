using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    //Класс камеры
    class Camera
    {
        //Максимальное и минимальное приближение камеры
        public float MAX_ZOOM = 0.25f;
        public float MIN_ZOOM = 2f;
        //Приближение камеры
        private float _zoom;
        //Центр приближения
        private Vector2 _zoomoffset;

        //Позиция камеры
        public Vector2 Position { get; set; }
        //Поворот камеры
        public float Rotation { get; set; }
        //Разрешение окна
        public Vector2 ScreenRes { get; set; }

        //Центр приближения
        public Vector2 ZoomOffset
        {
            get
            {
                return _zoomoffset;
            }
        }
        //Приближение камеры
        public float Zoom //TODO: Плавный зум и перемещение
        {
        	get 
        	{
        		return _zoom;
        	}
        	set 
        	{
                if(value < MAX_ZOOM)
                {
                    value = MAX_ZOOM;
                }
                else
                if (value > MIN_ZOOM)
                {
                    value = MIN_ZOOM;
                }
                var lastpos = ScreenRes / _zoom;
        		_zoom = value;
        		Position += (lastpos - ScreenRes / _zoom) / 2;
                _zoomoffset += (lastpos - ScreenRes / _zoom) / 2;
            }
        }
        //Конструктор
        public Camera(Vector2 ScreenRes)
        {
            _zoom = 1.0f;
            _zoomoffset = Vector2.Zero;
            Rotation = 0.0f;
            Position = Vector2.Zero;
            this.ScreenRes = ScreenRes;
        }
        //Получение матрицы камеры
        public Matrix GetTransform()
        {
            var _transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                             Matrix.CreateRotationZ(Rotation) *
                             Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
            return _transform;
        }
    }
}
