using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class UnitInfo : BasicSprite, IDrawable
    {
        public BasicText _MovePointsInfo { get; set; }
        public BasicText _DefenseInfo { get; set; }
        public BasicText _HPInfo { get; set; }
        public BasicText _DamageInfo { get; set; }
        private int iteration;
        private Vector2 LastScale;
        private bool IsAppearing = false;
        private int CellWidth;

        new public Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;

                var VectTmp = _DamageInfo.Font.MeasureString(_DamageInfo.Text);
                _DamageInfo.Position = new Vector2(value.X + CellWidth / 2 - VectTmp.X / 2, value.Y + Texture.Height / 2 - VectTmp.Y / 2 + 1);

                VectTmp = _DefenseInfo.Font.MeasureString(_DefenseInfo.Text);
                _DefenseInfo.Position = new Vector2(value.X + CellWidth + CellWidth / 2 - VectTmp.X / 2, value.Y + Texture.Height / 2 - VectTmp.Y / 2 + 1);

                VectTmp = _MovePointsInfo.Font.MeasureString(_MovePointsInfo.Text);
                _MovePointsInfo.Position = new Vector2(value.X + CellWidth * 2 + CellWidth / 2 - VectTmp.X / 2, value.Y + Texture.Height / 2 - VectTmp.Y / 2 + 1);

                VectTmp = _HPInfo.Font.MeasureString(_HPInfo.Text);
                _HPInfo.Position = new Vector2(value.X + CellWidth * 3 + CellWidth / 2 - VectTmp.X / 2, value.Y + Texture.Height / 2 - VectTmp.Y / 2 + 1);
            }
        }

        new public Vector2 Scale
        {
            get
            {
                return base.Scale;
            }
            set
            {
                base.Scale = value;
                _MovePointsInfo.Scale = value;
                _DefenseInfo.Scale = value;
            }
        }

        public string MovePointsText
        {
            get
            {
                return _MovePointsInfo.Text;
            }
            set
            {
                _MovePointsInfo.Text = value;
            }
        }

        public string DefenseText
        {
            get
            {
                return _DefenseInfo.Text;
            }
            set
            {
                _DefenseInfo.Text = value;
            }
        }

        public string DamageText
        {
            get
            {
                return _DamageInfo.Text;
            }
            set
            {
                _DamageInfo.Text = value;
            }
        }

        public string HPText
        {
            get
            {
                return _HPInfo.Text;
            }
            set
            {
                _HPInfo.Text = value;
            }
        }

        new public bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                _MovePointsInfo.Visible = value;
                _DefenseInfo.Visible = value;
                _HPInfo.Visible = value;
                _DamageInfo.Visible = value;
                base.Visible = value;
            }
        }

        public UnitInfo(Vector2 Position, Texture2D Texture, SpriteFont Font, Color TextColor, string MovePoints, string Defense, string Damage, string HP, float Layer = DefaultLayer) : base(Position, Texture, Layer)
        {
            CellWidth = Texture.Width / 4 + 1;

            var VectTmp = Font.MeasureString(Damage);
            _DamageInfo = new BasicText(new Vector2(Position.X + CellWidth / 2 - VectTmp.X / 2, Position.Y + Texture.Height / 2 - VectTmp.Y / 2 + 1), Damage, Font, TextColor, Layer - 0.0001f);

            VectTmp = Font.MeasureString(Defense);
            _DefenseInfo = new BasicText(new Vector2(Position.X + CellWidth + CellWidth / 2 - VectTmp.X / 2, Position.Y + Texture.Height / 2 - VectTmp.Y / 2 + 1), Defense, Font, TextColor, Layer - 0.0001f);

            VectTmp = Font.MeasureString(MovePoints);
            _MovePointsInfo = new BasicText(new Vector2(Position.X + CellWidth * 2 + CellWidth / 2 - VectTmp.X / 2, Position.Y + Texture.Height / 2 - VectTmp.Y / 2 + 1), MovePoints, Font, TextColor, Layer - 0.0001f);

            VectTmp = Font.MeasureString(HP);
            _HPInfo = new BasicText(new Vector2(Position.X + CellWidth * 3 + CellWidth / 2 - VectTmp.X / 2, Position.Y + Texture.Height / 2 - VectTmp.Y / 2 + 1), HP, Font, TextColor, Layer - 0.0001f);

            iteration = 0;
        }

        public void Appear()
        {
            if (!IsAppearing)
            {
                IsAppearing = true;
                Visible = true;
                LastScale = Scale;
                Scale = new Vector2(0, 0);
                iteration = 0;
            }
        }

        public void Disappear()
        {
            if (IsAppearing)
            {
                IsAppearing = false;
                Scale = LastScale;
                Visible = false;
            }
        }

        public void Update()
        {
            if(iteration < 50)
            {
                if (IsAppearing && iteration > 38)
                {
                    Scale = new Vector2(Scale.X + LastScale.X / 10, Scale.Y + LastScale.Y / 10);
                }
                iteration++;
            }
        }

        public override void Draw(SpriteBatch Target)
        {
            base.Draw(Target);
            _MovePointsInfo.Draw(Target);
            _DefenseInfo.Draw(Target);
            _HPInfo.Draw(Target);
            _DamageInfo.Draw(Target);
        }
    }
}
