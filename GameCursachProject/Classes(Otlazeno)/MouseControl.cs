using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameCursachProject
{
    /// <summary>
    /// Состояния кнопки
    /// </summary>
    public enum MouseButtonStates
    { NONE, PRESSED, RELEASED }
    //public enum MouseButtons
    //{ LEFT, RIGHT }

    /// <summary>
    /// Предоставляет сведения об устройстве управления
    /// </summary>
    static class MouseControl
    {
        //Состояние левой кнопки мыши
        static private MouseButtonStates _LeftBtn = MouseButtonStates.NONE;
        //Состояние правой кнопки мыши
        static private MouseButtonStates _RightBtn = MouseButtonStates.NONE;
        //True - кратковременное нажатие
        static private bool _IsLeftBtnClicked = false;
        static private bool _IsRightBtnClicked = false;
        //True - отслеживание кратковременного нажатия
        static private Stopwatch TimeLeftBtnClick = new Stopwatch();
        static private Stopwatch TimeRightBtnClick = new Stopwatch();
        //Координаты курсора
        static private int _X = 0;
        static private int _Y = 0;

        /// <summary>
        /// Состояние левой кнопки мыши
        /// </summary>
        static public MouseButtonStates LeftBtn { get{ return _LeftBtn; } }
        /// <summary>
        /// Состояние правой кнопки мыши
        /// </summary>
        static public MouseButtonStates RightBtn { get { return _RightBtn; } }

        /// <summary>
        /// True - произведено кратковременное нажатие левой кнопки
        /// </summary>
        static public bool IsLeftBtnClicked { get { return _IsLeftBtnClicked; } }
        /// <summary>
        /// True - произведено кратковременное нажатие правой кнопки
        /// </summary>
        static public bool IsRightBtnClicked { get { return _IsRightBtnClicked; } }
        /// <summary>
        /// Координата X курсора
        /// </summary>
        static public int X
        {
            get
            {
                return _X;
            }
        }
        /// <summary>
        /// Координата Y курсора
        /// </summary>
        static public int Y
        {
            get
            {
                return _Y;
            }
        }

        static public Vector2 MouseToWorldCoords(Camera camera)
        {
        	return CoordsConvert.WindowToWorldCoords(new Vector2(X, Y), camera);
        }

        /// <summary>
        /// Обновление данных
        /// </summary>
        static public void Update()
        {
            var State = Mouse.GetState();
            _X = State.X;
            _Y = State.Y;
            _IsRightBtnClicked = false;
            _IsLeftBtnClicked = false;

            if (State.RightButton == ButtonState.Pressed)
            {
                _RightBtn = MouseButtonStates.PRESSED;
                TimeRightBtnClick.Start();
            }
            else
                if (State.RightButton == ButtonState.Released && _RightBtn == MouseButtonStates.PRESSED)
                {
                    _RightBtn = MouseButtonStates.RELEASED;
                    TimeRightBtnClick.Stop();
                    if (TimeRightBtnClick.ElapsedMilliseconds <= 200)
                        _IsRightBtnClicked = true;
                    TimeRightBtnClick.Reset();
                }
                else
                    _RightBtn = MouseButtonStates.NONE;

            if (State.LeftButton == ButtonState.Pressed)
            {
                _LeftBtn = MouseButtonStates.PRESSED;
                TimeLeftBtnClick.Start();
            }
            else
                if (State.LeftButton == ButtonState.Released && _LeftBtn == MouseButtonStates.PRESSED)
                {
                    _LeftBtn = MouseButtonStates.RELEASED;
                    TimeLeftBtnClick.Stop();
                    if (TimeLeftBtnClick.ElapsedMilliseconds <= 200)
                        _IsLeftBtnClicked = true;
                    TimeLeftBtnClick.Reset();
                }
                else
                    _LeftBtn = MouseButtonStates.NONE;
        }
    }
}
