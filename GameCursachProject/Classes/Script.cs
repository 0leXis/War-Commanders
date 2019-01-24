using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;

namespace GameCursachProject
{
    class Script
    {
        private Lua LuaScript;
        public string TextScript { get; set; }

        public Script(string TextScript_FileName, string NameSpace, bool IsFromFile = false)
        {
            LuaScript = new Lua();
            LuaScript.LoadCLRPackage();
            LuaScript.DoString("import '"+NameSpace+"'");
            LuaScript.DoString("import ('MonoGame.Framework', 'Microsoft.Xna.Framework')");
            if (IsFromFile)
                using (var Fil = new StreamReader(TextScript_FileName, Encoding.Default))
                    TextScript = Fil.ReadToEnd();
            else
                TextScript = TextScript_FileName;
        }

        public void LoadScript(string TextScript_FileName)
        {
            using (var Fil = new StreamReader(TextScript_FileName, Encoding.Default))
                TextScript = Fil.ReadToEnd();
        }

        public object[] DoScript()
        {
        	try
        	{
            	return LuaScript.DoString(TextScript);
        	}
            catch(Exception e)
            {
            	Log.SendError("[Script]" + e);
            }
            return null;
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
            return LuaScript.GetFunction(FuncName);
        }
    }
}
