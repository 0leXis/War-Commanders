﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.IO;

namespace GameCursachProject
{
    //Информация о карте и юните
    public struct UnitCardInfo
    {
        //Имя
        public string Name;
        //Стоимость
        public int Cost;
        //Броня
        public int Armor;
        //Радиус атаки
        public int AttackRadius;
        //Урон
        public int Damage;
        //Очки жизни
        public int HP;
        //Скорость
        public int Speed;
        //Текстура юнита
        public Texture2D UnitTexture;
        //Скрипт анимации атаки юнита
        public string UnitAttackScript;
        //Текстура для карты
        public Texture2D Card_Decoration;

        //Конструктор
        public UnitCardInfo(string Name, int Cost, int Armor, int AttackRadius, int Damage, int HP, int Speed, Texture2D UnitTexture,
        string UnitAttackScript, Texture2D Card_Decoration)
        {
            this.Name = Name;
            this.Cost = Cost;
            this.Armor = Armor;
            this.AttackRadius = AttackRadius;
            this.Damage = Damage;
            this.HP = HP;
            this.Speed = Speed;
            this.UnitTexture = UnitTexture;
            this.UnitAttackScript = UnitAttackScript;
            this.Card_Decoration = Card_Decoration;
        }
        //Конструктор из файла
        public UnitCardInfo(string InfoPath)
        {
            Name = "";
            Cost = 0;
            Armor = 0;
            AttackRadius = 0;
            Damage = 0;
            HP = 0;
            Speed = 0;

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"Content\" + InfoPath + @"\Info.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Name == "Name")
                {
                    Name = xnode.InnerText;
                }
                if (xnode.Name == "Cost")
                {
                    Cost = Convert.ToInt32(xnode.InnerText);
                }
                if (xnode.Name == "Armor")
                {
                    Armor = Convert.ToInt32(xnode.InnerText);
                }
                if (xnode.Name == "AttackRadius")
                {
                    AttackRadius = Convert.ToInt32(xnode.InnerText);
                }
                if (xnode.Name == "Damage")
                {
                    Damage = Convert.ToInt32(xnode.InnerText);
                }
                if (xnode.Name == "HP")
                {
                    HP = Convert.ToInt32(xnode.InnerText);
                }
                if (xnode.Name == "Speed")
                {
                    Speed = Convert.ToInt32(xnode.InnerText);
                }
            }

            UnitTexture = ContentLoader.LoadTexture(InfoPath + @"\Unit");
            UnitAttackScript = ContentLoader.LoadScript(@"Content\" + InfoPath + @"\Attack.lua");
            Card_Decoration = ContentLoader.LoadTexture(InfoPath + @"\Card");
        }
    }

    //Информация о клетке поля боя
    public struct TileTypeInfo
    {
        //ID
        public int TileID;
        //Предоставляет броню юниту, стоящему на ней
        public int Armor;
        //Очки перемещения для прохождения через клетку
        public int SpeedNeeded;
        //Имя
        public string Name;
        //Текстура клетки
        public Texture2D Tile_Decoration;

        //Конструктор из файла
        public TileTypeInfo(string InfoPath)
        {
            TileID = 0;
            Armor = 0;
            SpeedNeeded = 0;
            Name = "";

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"Content\" + InfoPath + @"\Info.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Name == "Name")
                {
                    Name = xnode.InnerText;
                }
                if (xnode.Name == "Armor")
                {
                    Armor = Convert.ToInt32(xnode.InnerText);
                }
                if (xnode.Name == "SpeedNeeded")
                {
                    SpeedNeeded = Convert.ToInt32(xnode.InnerText);
                }
            }
            Tile_Decoration = ContentLoader.LoadTexture(InfoPath + @"\Tile");
        }
    }

    public static class GameContent
    {
        //Пути к основному контенту
        public const string DefaultSovietUnitCardsPath = @"Cards\Unit\Soviet";
        public const string DefaultGermanyUnitCardsPath = @"Cards\Unit\Germany";
        public const string DefaultTileTypesPath = @"Tiles";

        //Текстуры, скрипты, шрифты, используемые в игре
        static public Texture2D Unit_AttackRadius;
        static public List<UnitCardInfo> UnitCards = new List<UnitCardInfo>();
        static public List<UnitCardInfo> SovietUnitCards = new List<UnitCardInfo>();
        static public List<UnitCardInfo> GermanyUnitCards = new List<UnitCardInfo>();
        static public List<TileTypeInfo> TileTypes = new List<TileTypeInfo>();

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

        static public Texture2D UI_LoseImage;
        static public Texture2D UI_VictoryImage;

        static public Texture2D UI_GameMenu_MainBack;
        static public Texture2D UI_GameMenu_OptionsBack;
        static public Texture2D UI_GameMenu_Button;
        static public Texture2D UI_GameMenu_ListBoxBtn;
        static public Texture2D UI_GameMenu_ListBoxChoosed;
        static public Texture2D UI_GameMenu_ListBoxOpenBtn;

        static public Texture2D UI_MainMenu_LogIn_Button;
        static public Texture2D UI_MainMenu_LogIn_EditBox;
        static public Texture2D UI_MainMenu_LogIn_BackGround;
        static public Texture2D UI_MainMenu_LogIn_ConnIcon;
        static public Texture2D UI_MainMenu_BackGround;
        static public Texture2D UI_MainMenu_HomeButton;
        static public Texture2D UI_MainMenu_Button;
        static public Texture2D UI_MainMenu_MenuBar;
        static public Texture2D UI_MainMenu_MoneyBack;
        static public Texture2D UI_MainMenu_NameBack;
        static public Texture2D UI_MainMenu_RollBack;

        static public Texture2D UI_MainMenu_SovietDeck;
        static public Texture2D UI_MainMenu_GermanDeck;

        static public Texture2D UI_MainMenu_Collection_Prev;
        static public Texture2D UI_MainMenu_Collection_Next;
        static public Texture2D UI_MainMenu_Collection_Bottom;
        static public Texture2D UI_MainMenu_Collection_Right;

        static public SpriteFont UI_MainMenu_Collection_PageTextFont;

        static public Texture2D UI_MainMenu_Play_StartButton;

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

        static public Texture2D ArrowSegment;
        static public Texture2D ArrowEnd;

        static public Texture2D CardTexture;

        //Текстуры для скриптов атаки
        static public Texture2D Bullet;
        static public Texture2D Explosion;

        //Загрузка всех ресурсов
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

            UI_LoseImage = ContentLoader.LoadTexture(@"Textures\UI_Lose");
            UI_VictoryImage = ContentLoader.LoadTexture(@"Textures\UI_Victory");

            UI_GameMenu_MainBack = ContentLoader.LoadTexture(@"Textures\UI_GameMenu_Back");
            UI_GameMenu_OptionsBack = ContentLoader.LoadTexture(@"Textures\UI_GameMenu_OptionsBack");
            UI_GameMenu_Button = ContentLoader.LoadTexture(@"Textures\BtnNewTurn");
            UI_GameMenu_ListBoxChoosed = ContentLoader.LoadTexture(@"Textures\UI_Lst_Choosed");
            UI_GameMenu_ListBoxBtn = ContentLoader.LoadTexture(@"Textures\UI_Lst_Variant");
            UI_GameMenu_ListBoxOpenBtn = ContentLoader.LoadTexture(@"Textures\UI_BtnOpen");

            UI_MainMenu_LogIn_Button = ContentLoader.LoadTexture(@"Textures\BtnNewTurn");
            UI_MainMenu_LogIn_EditBox = ContentLoader.LoadTexture(@"Textures\UI_EditBox");
            UI_MainMenu_LogIn_BackGround = ContentLoader.LoadTexture(@"Textures\UI_Login_Back");
            UI_MainMenu_LogIn_ConnIcon = ContentLoader.LoadTexture(@"Textures\Connecting_Icon");
            //UI_MainMenu_BackGround = ContentLoader.LoadTexture(@"Textures\UI_BtnOpen");
            UI_MainMenu_HomeButton = ContentLoader.LoadTexture(@"Textures\UI_MainMenu_HomeButton");
            UI_MainMenu_Button = ContentLoader.LoadTexture(@"Textures\UI_MainMenu_Button");
            UI_MainMenu_MenuBar = ContentLoader.LoadTexture(@"Textures\UI_MainMenu_MenuBar");
            UI_MainMenu_MoneyBack = ContentLoader.LoadTexture(@"Textures\UI_MoneyBack");
            UI_MainMenu_NameBack = ContentLoader.LoadTexture(@"Textures\UI_NameBack");
            UI_MainMenu_RollBack = ContentLoader.LoadTexture(@"Textures\UI_RollBack");

            UI_MainMenu_SovietDeck = ContentLoader.LoadTexture(@"Textures\UI_SovietDeck");
            UI_MainMenu_GermanDeck = ContentLoader.LoadTexture(@"Textures\UI_GermanDeck");

            UI_MainMenu_Collection_Prev = ContentLoader.LoadTexture(@"Textures\Pred");
            UI_MainMenu_Collection_Next = ContentLoader.LoadTexture(@"Textures\Next");
            UI_MainMenu_Collection_PageTextFont = ContentLoader.LoadFont(@"Fonts\PageTextFont");
            UI_MainMenu_Collection_Bottom = ContentLoader.LoadTexture(@"Textures\UI_Collection_Bottom");
            UI_MainMenu_Collection_Right = ContentLoader.LoadTexture(@"Textures\UI_Collection_DeckList");

            UI_MainMenu_Play_StartButton = ContentLoader.LoadTexture(@"Textures\UI_StartButton");

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

            UI_Player_Icons.Add(ContentLoader.LoadTexture(@"Textures\Player_Icon_Soviet"));
            UI_Player_Icons.Add(ContentLoader.LoadTexture(@"Textures\Player_Icon_Germany"));

            var i = 0;
            while(Directory.Exists(@"Content\" + DefaultSovietUnitCardsPath + @"\" + i.ToString()))
            {
                SovietUnitCards.Add(new UnitCardInfo(DefaultSovietUnitCardsPath + @"\" + i.ToString()));
                UnitCards.Add(new UnitCardInfo(DefaultSovietUnitCardsPath + @"\" + i.ToString()));
                i++;
            }

            i = 0;
            while (Directory.Exists(@"Content\" + DefaultGermanyUnitCardsPath + @"\" + i.ToString()))
            {
                GermanyUnitCards.Add(new UnitCardInfo(DefaultGermanyUnitCardsPath + @"\" + i.ToString()));
                UnitCards.Add(new UnitCardInfo(DefaultGermanyUnitCardsPath + @"\" + i.ToString()));
                i++;
            }

            i = 0;
            while (Directory.Exists(@"Content\" + DefaultTileTypesPath + @"\" + i.ToString()))
            {
                TileTypes.Add(new TileTypeInfo(DefaultTileTypesPath + @"\" + i.ToString()));
                i++;
            }
        }
    }
}
