using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameCursachProject
{
    static class KeyBindings
    {
        static private KeyboardState State;
        static private KeyboardState LastState;

        static public Dictionary<string, Keys> BindedKeys = new Dictionary<string, Keys>();

        static public void Init()
        {
            Update();
            Update();
        }

        static public void RegisterKeyBind(string KeyFunction, Keys Key)
        {
            if(!BindedKeys.ContainsKey(KeyFunction))
                BindedKeys.Add(KeyFunction, Key);
        }

        static public bool CheckKeyPressed(string KeyFunction)
        {
            Keys Key;
            var Res = BindedKeys.TryGetValue(KeyFunction, out Key);
            if (Res)
            {
                if (State.IsKeyDown(Key))
                    return true;
            }
            return false;
        }

        static public bool CheckKeyPressed(Keys Key)
        {
            if (State.IsKeyDown(Key))
               return true;
            return false;
        }

        static public bool CheckKeyReleased(string KeyFunction)
        {
            Keys Key;
            var Res = BindedKeys.TryGetValue(KeyFunction, out Key);
            if (Res)
            {
                if (State.IsKeyUp(Key) && LastState.IsKeyDown(Key))
                    return true;
            }
            return false;
        }

        static public bool CheckKeyReleased(Keys Key)
        {
            if (State.IsKeyUp(Key) && LastState.IsKeyDown(Key))
               return true;
            return false;
        }

        static public void Update()
        {
            LastState = State;
            State = Keyboard.GetState();
        }
    }
}
