using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    enum CollectionState { ALL_CARDS, SOVIET_CARDS, GERMANY_CARDS }

    class Collection : IDrawable
    {
        private Card[] Cards;

        private Button Prev;
        private Button Next;
        private Button[] DeckChange;
        private BasicText PageText;

        private BasicSprite BackGround_Bottom;
        private BasicSprite BackGround_Right;

        private CollectionState State;

        public bool[] IsCardInCollection;
        public int CardTextureWidth { get; set; }
        public int CardTextureHeight { get; set; }

        private int Pages;
        private int SovietPages;
        private int GermanyPages;
        private int Page;

        public bool IsShown;

        public float Layer { get; set; }

        public Vector2 _ScreenRes;

        public Vector2 ScreenRes
        {
            get
            {
                return _ScreenRes;
            }
            set
            {
                _ScreenRes = value;
                var TmpVect = new Vector2((value.X - 300 - CardTextureWidth * 3) / 4, value.Y - CardTextureHeight * 1.5f);
                for (var i = 0; i < Cards.Length; i++)
                {
                    Cards[i].Position = new Vector2(TmpVect.X * (i + 1) + CardTextureWidth * i, TmpVect.Y);
                }

                PageText.Position = new Vector2((ScreenRes.X - 300 - GameContent.UI_MainMenu_Collection_PageTextFont.MeasureString(PageText.Text).X) / 2, ScreenRes.Y - GameContent.UI_MainMenu_Collection_Prev.Height - 20);
                Prev.Position = new Vector2(PageText.Position.X - GameContent.UI_MainMenu_Collection_Prev.Width / 4 - 10, PageText.Position.Y - (GameContent.UI_MainMenu_Collection_Prev.Height - GameContent.UI_MainMenu_Collection_PageTextFont.MeasureString(PageText.Text).Y) / 2);
                Next.Position = new Vector2(PageText.Position.X + 10 + GameContent.UI_MainMenu_Collection_PageTextFont.MeasureString(PageText.Text).X + 3, PageText.Position.Y - (GameContent.UI_MainMenu_Collection_Prev.Height - GameContent.UI_MainMenu_Collection_PageTextFont.MeasureString(PageText.Text).Y) / 2);
            }
        }

        public Collection(Vector2 ScreenRes, Color FontColor, bool[] IsSovietCardInCollection, bool[] IsGermanCardInCollection, int CardTextureWidth, float Layer = BasicSprite.DefaultLayer)
        {
            Cards = new Card[3];
            DeckChange = new Button[8];

            this.CardTextureWidth = CardTextureWidth;
            CardTextureHeight = GameContent.CardTexture.Height;
            this.Layer = Layer;

            Pages = (GameContent.UnitCards.Count - 1) / Cards.Length + 1;
            GermanyPages = (GameContent.GermanyUnitCards.Count - 1) / Cards.Length + 1;
            SovietPages = (GameContent.SovietUnitCards.Count - 1) / Cards.Length + 1;
            Page = 1;

            var TmpVect = new Vector2((ScreenRes.X - 300 - CardTextureWidth * Cards.Length) / (Cards.Length + 1), ScreenRes.Y - CardTextureHeight * 1.5f);
            for (var i = 0; i < Cards.Length; i++)
            {
                Cards[i] = new Card(new Vector2(TmpVect.X * (i + 1) + CardTextureWidth * i, TmpVect.Y), GameContent.CardTexture, GameContent.UnitCards[0].Card_Decoration, new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, GameContent.UnitCards[0].Name, GameContent.UnitCards[0].Damage.ToString(), GameContent.UnitCards[0].Armor.ToString(), GameContent.UnitCards[0].AttackRadius.ToString(), GameContent.UnitCards[0].Speed.ToString(), GameContent.UnitCards[0].HP.ToString(), GameContent.UnitCards[0].Cost.ToString(), 141, 315, 4, 4, 37, true, MapZones.ALL, Layer: Layer);
            }

            PageText = new BasicText(new Vector2((ScreenRes.X - 300 - GameContent.UI_MainMenu_Collection_PageTextFont.MeasureString("10").X) / 2, ScreenRes.Y - GameContent.UI_MainMenu_Collection_Prev.Height - 20), "10", GameContent.UI_MainMenu_Collection_PageTextFont, FontColor, Layer - 0.001f);
            Prev = new Button(new Vector2(PageText.Position.X - GameContent.UI_MainMenu_Collection_Prev.Width / 4 - 10, PageText.Position.Y - (GameContent.UI_MainMenu_Collection_Prev.Height - GameContent.UI_MainMenu_Collection_PageTextFont.MeasureString("10").Y) / 2), GameContent.UI_MainMenu_Collection_Prev, GameContent.UI_MainMenu_Collection_Prev.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);
            Next = new Button(new Vector2(PageText.Position.X + 10 + GameContent.UI_MainMenu_Collection_PageTextFont.MeasureString("10").X + 3, PageText.Position.Y - (GameContent.UI_MainMenu_Collection_Prev.Height - GameContent.UI_MainMenu_Collection_PageTextFont.MeasureString("10").Y) / 2), GameContent.UI_MainMenu_Collection_Next, GameContent.UI_MainMenu_Collection_Next.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);

            BackGround_Bottom = new BasicSprite(new Vector2(0, ScreenRes.Y - GameContent.UI_MainMenu_Collection_Bottom.Height), GameContent.UI_MainMenu_Collection_Bottom, Layer - 0.0002f);
            BackGround_Right = new BasicSprite(new Vector2( ScreenRes.X - GameContent.UI_MainMenu_Collection_Right.Width, 102), GameContent.UI_MainMenu_Collection_Right, Layer - 0.0001f);
            for (var i = 0; i < DeckChange.Length / 2; i++)
            {
                DeckChange[i] = new Button(new Vector2(BackGround_Right.Position.X, BackGround_Right.Position.Y + i * GameContent.UI_MainMenu_SovietDeck.Height), GameContent.UI_MainMenu_SovietDeck, "Колода " + i, GameContent.UI_ButtonFont, Color.White, GameContent.UI_MainMenu_SovietDeck.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.002f);
            }
            for (var i = DeckChange.Length / 2; i < DeckChange.Length; i++)
            {
                DeckChange[i] = new Button(new Vector2(BackGround_Right.Position.X, BackGround_Right.Position.Y + i * GameContent.UI_MainMenu_GermanDeck.Height), GameContent.UI_MainMenu_GermanDeck, "Колода " + i, GameContent.UI_ButtonFont, Color.White, GameContent.UI_MainMenu_GermanDeck.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.002f);
            }

            Hide();

            State = CollectionState.ALL_CARDS;

            this.ScreenRes = ScreenRes;
        }

        public void Hide()
        {
            IsShown = false;
            for (var i = 0; i < Cards.Length; i++)
            {
                Cards[i].Visible = false;
            }
            foreach (var button in DeckChange)
                button.Visible = false;
            PageText.Visible = false;
            Prev.Visible = false;
            Next.Visible = false;
            BackGround_Bottom.Visible = false;
            BackGround_Right.Visible = false;
        }

        public void Show(int Page, bool ShowDeckButtons)
        {
            if (Page > Pages || Page < 1)
                return;
            IsShown = true;
            this.Page = Page;
            PageText.Text = Page.ToString();

            PageText.Position = new Vector2((ScreenRes.X - 300 - GameContent.UI_MainMenu_Collection_PageTextFont.MeasureString(PageText.Text).X) / 2, ScreenRes.Y - GameContent.UI_MainMenu_Collection_Prev.Height - 20);
            Prev.Position = new Vector2(PageText.Position.X - GameContent.UI_MainMenu_Collection_Prev.Width / 4 - 10, PageText.Position.Y - (GameContent.UI_MainMenu_Collection_Prev.Height - GameContent.UI_MainMenu_Collection_PageTextFont.MeasureString(PageText.Text).Y) / 2);
            Next.Position = new Vector2(PageText.Position.X + 10 + GameContent.UI_MainMenu_Collection_PageTextFont.MeasureString(PageText.Text).X + 3, PageText.Position.Y - (GameContent.UI_MainMenu_Collection_Prev.Height - GameContent.UI_MainMenu_Collection_PageTextFont.MeasureString(PageText.Text).Y) / 2);

            if (State == CollectionState.ALL_CARDS)
            {
                for (var i = 0; i < Cards.Length; i++)
                {
                    if (GameContent.UnitCards.Count > (Page - 1) * 3 + i)
                        Cards[i] = new Card(Cards[i].Position, GameContent.CardTexture, GameContent.UnitCards[(Page - 1) * 3 + i].Card_Decoration, new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, GameContent.UnitCards[(Page - 1) * 3 + i].Name, GameContent.UnitCards[(Page - 1) * 3 + i].Damage.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].Armor.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].AttackRadius.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].Speed.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].HP.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].Cost.ToString(), 141, 315, 4, 4, 37, true, MapZones.ALL, Layer: Layer);
                    else
                        Cards[i].Visible = false;
                }
            }
            else
            if(State == CollectionState.GERMANY_CARDS)
            {
                for (var i = 0; i < Cards.Length; i++)
                {
                    if (GameContent.GermanyUnitCards.Count > (Page - 1) * 3 + i)
                        Cards[i] = new Card(Cards[i].Position, GameContent.CardTexture, GameContent.UnitCards[(Page - 1) * 3 + i].Card_Decoration, new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, GameContent.UnitCards[(Page - 1) * 3 + i].Name, GameContent.UnitCards[(Page - 1) * 3 + i].Damage.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].Armor.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].AttackRadius.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].Speed.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].HP.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].Cost.ToString(), 141, 315, 4, 4, 37, true, MapZones.ALL, Layer: Layer);
                    else
                        Cards[i].Visible = false;
                }
            }
            else
            if (State == CollectionState.SOVIET_CARDS)
            {
                for (var i = 0; i < Cards.Length; i++)
                {
                    if (GameContent.SovietUnitCards.Count > (Page - 1) * 3 + i)
                        Cards[i] = new Card(Cards[i].Position, GameContent.CardTexture, GameContent.UnitCards[(Page - 1) * 3 + i].Card_Decoration, new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, GameContent.UnitCards[(Page - 1) * 3 + i].Name, GameContent.UnitCards[(Page - 1) * 3 + i].Damage.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].Armor.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].AttackRadius.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].Speed.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].HP.ToString(), GameContent.UnitCards[(Page - 1) * 3 + i].Cost.ToString(), 141, 315, 4, 4, 37, true, MapZones.ALL, Layer: Layer);
                    else
                        Cards[i].Visible = false;
                }
            }

            if(ShowDeckButtons)
                foreach (var button in DeckChange)
                    button.Visible = true;

            PageText.Visible = true;
            Prev.Visible = true;
            Next.Visible = true;
            BackGround_Bottom.Visible = true;
            BackGround_Right.Visible = true;
        }
        
        public void Update()
        {
            if (Prev.Update() == ButtonStates.CLICKED)
                Show(Page - 1, false);
            if (Next.Update() == ButtonStates.CLICKED)
                Show(Page + 1, false);
            foreach (var button in DeckChange)
                button.Update();
        }

        public void Draw(SpriteBatch Target)
        {
            for (var i = 0; i < Cards.Length; i++)
            {
                Cards[i].Draw(Target);
            }
            foreach (var button in DeckChange)
                button.Draw(Target);
            PageText.Draw(Target);
            Prev.Draw(Target);
            Next.Draw(Target);
            BackGround_Bottom.Draw(Target);
            BackGround_Right.Draw(Target);
        }
    }
}
