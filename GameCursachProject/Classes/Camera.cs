using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class Camera
    {
        public float MAX_ZOOM = 0.2f;
        public float MIN_ZOOM = 2f;

        private float _zoom;

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 ScreenRes { get; set; }

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
        	}
        }

        public Camera(Vector2 ScreenRes)
        {
            _zoom = 1.0f;
            Rotation = 0.0f;
            Position = Vector2.Zero;
            this.ScreenRes = ScreenRes;
        }

        public Matrix GetTransform(GraphicsDevice device)
        {
            var _transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                             Matrix.CreateRotationZ(Rotation) *
                             Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
            return _transform;
        }
    }
}
