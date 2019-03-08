using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class RollBack : BasicSprite
    {
        public Vector2 UpPosition { get; set; }
        public Vector2 DownPosition { get; set; }
        public Vector2 PlusVector { get; set; }
        private int iteration;
        private int Speed;
        private bool IsUpping;
        private bool IsDowning;

        public bool IsMoving
        {
            get
            {
                return IsUpping || IsDowning;
            }
        }

        public RollBack( Vector2 UpPosition, Vector2 DownPosition, int Speed, Texture2D Texture, float Layer = BasicSprite.DefaultLayer) : base( DownPosition, Texture, Layer)
        {
            this.UpPosition = UpPosition;
            this.DownPosition = DownPosition;
            PlusVector = (DownPosition - UpPosition) / Speed;
            iteration = Speed + 1;
            this.Speed = Speed;
        }

        public void SetUp()
        {
            if (!IsUpping)
            {
                iteration = 0;
                Position = DownPosition;
                IsUpping = true;
                IsDowning = false;
            }
        }

        public void SetDown()
        {
            if (!IsDowning)
            {
                iteration = 0;
                Position = UpPosition;
                IsUpping = false;
                IsDowning = true;
            }
        }

        public void Update()
        {
            if(iteration <= Speed)
            {
                if (IsUpping)
                {
                    Position -= PlusVector;
                }
                if (IsDowning)
                {
                    Position += PlusVector;
                }
                if(iteration == Speed)
                {
                    if (IsUpping)
                    {
                        Position = UpPosition;
                        IsUpping = false;
                    }
                    if (IsDowning)
                    {
                        Position = DownPosition;
                        IsDowning = false;
                    }
                }
                iteration++;
            }
        }
    }
}
