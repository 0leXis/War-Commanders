using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    //Отображает информацию о клетке
    class TileInfo : BasicSprite, IDrawable
    {
        //Текст, отображающий кол-во очков передвижения для прохождения клетки юнитом
        private BasicText _NeedMovePointsInfo;
        //Текст, отображающий кол-во очков защиты, которые получает юнит, находящийся на данной клетке
        private BasicText _DefenseInfo;
        //Для работы анимации
        private int iteration;
        private Vector2 LastScale;
        private bool IsAppearing = false;
        //Ширина ячейки, в которой отображается текст
        private int CellWidth;

        //Позиция
        new public Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                var VectTmp = _NeedMovePointsInfo.Font.MeasureString(_NeedMovePointsInfo.Text);
                _NeedMovePointsInfo.Position = new Vector2(value.X + CellWidth + CellWidth / 2 - VectTmp.X / 2 + 3, value.Y + Texture.Height / 2 - VectTmp.Y / 2 + 1);
                VectTmp = _NeedMovePointsInfo.Font.MeasureString(_DefenseInfo.Text);
                _DefenseInfo.Position = new Vector2(value.X + CellWidth * 3 + CellWidth / 2 - VectTmp.X / 2 + 6, value.Y + Texture.Height / 2 - VectTmp.Y / 2 + 1);
            }
        }
        //Увеличение
        new public Vector2 Scale
        {
            get
            {
                return base.Scale;
            }
            set
            {
                base.Scale = value;
                _NeedMovePointsInfo.Scale = value;
                _DefenseInfo.Scale = value;
            }
        }
        //Кол-во очков перемещения
        public string NeedMovePointsText
        {
            get
            {
                return _NeedMovePointsInfo.Text;
            }
            set
            {
                _NeedMovePointsInfo.Text = value;
            }
        }
        //Кол-во очков защиты
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
        //Если true - отрисовывать элемент
        new public bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                _NeedMovePointsInfo.Visible = value;
                _DefenseInfo.Visible = value;
                base.Visible = value;
            }
        }
        //Конструктор
        public TileInfo(Vector2 Position, Texture2D Texture, SpriteFont Font, Color TextColor, string NeedMovePoints, string Defense, float Layer = DefaultLayer) : base(Position, Texture, Layer)
        {
            CellWidth = Texture.Width / 4 + 1;
            var VectTmp = Font.MeasureString(NeedMovePoints);
            _NeedMovePointsInfo = new BasicText(new Vector2(Position.X + CellWidth + CellWidth / 2 - VectTmp.X / 2 + 3, Position.Y + Texture.Height / 2 - VectTmp.Y / 2 + 1), NeedMovePoints, Font, TextColor, Layer - 0.0001f);
            VectTmp = Font.MeasureString(Defense);
            _DefenseInfo = new BasicText(new Vector2(Position.X + CellWidth * 3 + CellWidth / 2 - VectTmp.X / 2 + 6, Position.Y + Texture.Height / 2 - VectTmp.Y / 2 + 1), Defense, Font, TextColor, Layer - 0.0001f);
            iteration = 0;
        }
        //Плавное появление элемента
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
        //Плавное исчезание элемента
        public void Disappear()
        {
            if (IsAppearing)
            {
                IsAppearing = false;
                Scale = LastScale;
                Visible = false;
            }
        }
        //Обработчик событий элемента
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
        //Отрисовка элемента
        public override void Draw(SpriteBatch Target)
        {
            base.Draw(Target);
            _NeedMovePointsInfo.Draw(Target);
            _DefenseInfo.Draw(Target);
        }
    }
}
