using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    public static class GameContent
    {
        static public Texture2D Unit_AttackRadius;
        static public List<Texture2D> UnitTextures = new List<Texture2D>();
        static public List<string> UnitAttackScripts = new List<string>();

        static public Texture2D UI_Info_Allied;
        static public Texture2D UI_Info_Enemy;
        static public Texture2D UI_TileInfo;

        static public Texture2D UI_Main_Bottom;
        static public Texture2D UI_Main_Bottom_Left;
        static public Texture2D UI_Main_Up;
        static public Texture2D UI_Main_Up_Left;
        static public Texture2D UI_Main_Up_Right;
        static public Texture2D UI_Btn_NewTurn;
        static public Texture2D UI_Btn_Move;
        static public Texture2D UI_Btn_Attack;
        static public Texture2D UI_Btn_Menu;
        static public Texture2D UI_Btn_Chat;
        static public Texture2D UI_Btn_Stats;
        static public Texture2D UI_Flag_Player;
        static public Texture2D UI_Flag_Enemy;
        static public Texture2D UI_Money;
        static public Texture2D UI_HourGlass;
        static public Texture2D UI_Vs;
        static public Texture2D UI_AlliedPoint;
        static public Texture2D UI_EnemyPoint;
        static public Texture2D UI_NeutralPoint;
        static public Texture2D UI_CardReplace;
        static public Texture2D UI_EnemyTurn;

        static public SpriteFont UI_ButtonFont;
        static public SpriteFont UI_MiniFont;
        static public SpriteFont UI_InfoFont;
        static public SpriteFont UI_NewTurnFont;
        static public List<Texture2D> UI_Player_Icons = new List<Texture2D>();

        static public Texture2D TileBorder;
        static public Texture2D TileBorder_HL;
        static public Texture2D Tile_ControlPoint_Neutral;
        static public Texture2D Tile_ControlPoint_Allied;
        static public Texture2D Tile_ControlPoint_Enemy;
        static public List<Texture2D> Tile_Decorations = new List<Texture2D>();

        static public Texture2D ArrowSegment;
        static public Texture2D ArrowEnd;

        static public Texture2D CardTexture;
        static public List<Texture2D> Card_Decorations = new List<Texture2D>();

        ///////////
        static public Texture2D Bullet;
        static public Texture2D Explosion;

        static public void LoadGameContent()
        {
            UI_Info_Allied = ContentLoader.LoadTexture(@"Textures\UnitHaracts_Allied");
            UI_Info_Enemy = ContentLoader.LoadTexture(@"Textures\UnitHaracts_Enemy");
            UI_TileInfo = ContentLoader.LoadTexture(@"Textures\TileHaracts");
            UI_Main_Bottom = ContentLoader.LoadTexture(@"Textures\UI_Main_Bottom");
            UI_Main_Bottom_Left = ContentLoader.LoadTexture(@"Textures\UI_Main_Bottom_Left");
            UI_Main_Up = ContentLoader.LoadTexture(@"Textures\UI_Main_Up");
            UI_Main_Up_Left = ContentLoader.LoadTexture(@"Textures\UI_Main_Up_Left");
            UI_Main_Up_Right = ContentLoader.LoadTexture(@"Textures\UI_Main_Up_Right");
            UI_Btn_NewTurn = ContentLoader.LoadTexture(@"Textures\BtnNewTurn");
            UI_Btn_Move = ContentLoader.LoadTexture(@"Textures\BtnMove");
            UI_Btn_Attack = ContentLoader.LoadTexture(@"Textures\BtnAttack");
            UI_Btn_Menu = ContentLoader.LoadTexture(@"Textures\BtnMenu");
            UI_Btn_Chat = ContentLoader.LoadTexture(@"Textures\BtnChat");
            UI_Btn_Stats = ContentLoader.LoadTexture(@"Textures\BtnStats");
            UI_Flag_Player = ContentLoader.LoadTexture(@"Textures\UI_Flag_Player");
            UI_Flag_Enemy = ContentLoader.LoadTexture(@"Textures\UI_Flag_Enemy");
            UI_Money = ContentLoader.LoadTexture(@"Textures\UI_Money");
            UI_HourGlass = ContentLoader.LoadTexture(@"Textures\UI_HourGlass");
            UI_Vs = ContentLoader.LoadTexture(@"Textures\Vs");
            UI_AlliedPoint = ContentLoader.LoadTexture(@"Textures\UI_Allied");
            UI_EnemyPoint = ContentLoader.LoadTexture(@"Textures\UI_Enemy");
            UI_NeutralPoint = ContentLoader.LoadTexture(@"Textures\UI_Neutral");
            UI_CardReplace = ContentLoader.LoadTexture(@"Textures\Card_Replace");
            UI_EnemyTurn = ContentLoader.LoadTexture(@"Textures\UI_EnemyTurn");

            UI_ButtonFont = ContentLoader.LoadFont(@"Fonts\ButtonFont");
            UI_MiniFont = ContentLoader.LoadFont(@"Fonts\UI_MiniFont");
            UI_InfoFont = ContentLoader.LoadFont(@"Fonts\TileInfoFont");
            UI_NewTurnFont = ContentLoader.LoadFont(@"Fonts\NewTurnFont");

            TileBorder = ContentLoader.LoadTexture(@"Textures\Tile");
            TileBorder_HL = ContentLoader.LoadTexture(@"Textures\TileHL");
            Tile_ControlPoint_Neutral = ContentLoader.LoadTexture(@"Textures\Neutral");
            Tile_ControlPoint_Allied = ContentLoader.LoadTexture(@"Textures\Allied");
            Tile_ControlPoint_Enemy = ContentLoader.LoadTexture(@"Textures\Enemy");

            ArrowSegment = ContentLoader.LoadTexture(@"Textures\ArrowSegment");
            ArrowEnd = ContentLoader.LoadTexture(@"Textures\ArrowEnd");

            Unit_AttackRadius = ContentLoader.LoadTexture(@"Textures\Attack_Radius");

            CardTexture = ContentLoader.LoadTexture(@"Textures\Card");

            Bullet = ContentLoader.LoadTexture(@"Textures\Bullet");
            Explosion = ContentLoader.LoadTexture(@"Textures\BABAH");

            //////////////////////////////////////////////////////////////////////////

            Tile_Decorations.Add(ContentLoader.LoadTexture(@"Textures\Tile_Forest"));
            Tile_Decorations.Add(ContentLoader.LoadTexture(@"Textures\Tile_Desert"));
            Tile_Decorations.Add(ContentLoader.LoadTexture(@"Textures\Tile_River"));

            UnitTextures.Add(ContentLoader.LoadTexture(@"Textures\TankPrototype"));

            UI_Player_Icons.Add(ContentLoader.LoadTexture(@"Textures\Player_Icon"));

            UnitAttackScripts.Add(ContentLoader.LoadScript(@"Content\Scripts\TestScript.lua"));

            Card_Decorations.Add(ContentLoader.LoadTexture(@"Textures\Tiger"));
        }
    }
}
