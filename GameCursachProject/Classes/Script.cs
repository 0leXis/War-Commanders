using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;

namespace GameCursachProject.Classes
{
    class Script
    {
        private Lua LuaScript;

        public string TextScript { get; set; }

        public Script(string TextScript_FileName, bool IsFromFile = false)
        {
            LuaScript = new Lua();
            LuaScript.LoadCLRPackage();

            if (IsFromFile)
                TextScript = TextScript_FileName;
            else
                using (var Fil = new StreamReader(TextScript_FileName, Encoding.Default))
                    TextScript = Fil.ReadToEnd();
        }

        public object[] DoScript()
        {
            return LuaScript.DoString(TextScript);
        }

        public void RegisterVar(ref object Obj, string VarName)
        {
            LuaScript[VarName] = Obj;
        }

        public object GetVar(string VarName)
        {
            return LuaScript[VarName];
        }

        public LuaFunction GetFunc(string FuncName)
        {
            return LuaScript[FuncName] as LuaFunction;
        }
    }
}
