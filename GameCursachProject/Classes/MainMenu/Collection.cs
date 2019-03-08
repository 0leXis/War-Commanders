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

        public Vector2 ScreenRes
        {
            set
            {
                var TmpVect = new Vector2((value.X - 150 - CardTextureWidth * 3) / 4, value.Y - CardTextureHeight * 1.5f);
                for (var i = 0; i < Cards.Length; i++)
                {
                    Cards[i].Position = new Vector2(TmpVect.X * (i + 1) + CardTextureWidth * i, TmpVect.Y);
                }
            }
        }

        public Collection(Vector2 ScreenRes, bool[] IsSovietCardInCollection, bool[] IsGermanCardInCollection, int CardTextureWidth, float Layer = BasicSprite.DefaultLayer)
        {
            Cards = new Card[4];
            this.CardTextureWidth = CardTextureWidth;
            CardTextureHeight = GameContent.CardTexture.Height;
            this.Layer = Layer;

            Pages = (GameContent.UnitCards.Count - 1) / Cards.Length + 1;
            GermanyPages = (GameContent.GermanyUnitCards.Count - 1) / Cards.Length + 1;
            SovietPages = (GameContent.SovietUnitCards.Count - 1) / Cards.Length + 1;
            Page = 1;

            var TmpVect = new Vector2((ScreenRes.X - 150 - CardTextureWidth * Cards.Length) / (Cards.Length + 1), ScreenRes.Y - CardTextureHeight * 1.5f);
            for (var i = 0; i < Cards.Length; i++)
            {
                Cards[i] = new Card(new Vector2(TmpVect.X * (i + 1) + CardTextureWidth * i, TmpVect.Y), GameContent.CardTexture, GameContent.UnitCards[0].Card_Decoration, new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, GameContent.UnitCards[0].Name, GameContent.UnitCards[0].Damage.ToString(), GameContent.UnitCards[0].Armor.ToString(), GameContent.UnitCards[0].AttackRadius.ToString(), GameContent.UnitCards[0].Speed.ToString(), GameContent.UnitCards[0].HP.ToString(), GameContent.UnitCards[0].Cost.ToString(), 141, 315, 4, 4, 37, true, MapZones.ALL, Layer: Layer);
            }

            Hide();

            State = CollectionState.ALL_CARDS;
        }

        public void Hide()
        {
            IsShown = false;
            for (var i = 0; i < Cards.Length; i++)
            {
                Cards[i].Visible = false;
            }
        }

        public void Show()
        {
            IsShown = true;
            Page = 1;
            if (State == CollectionState.ALL_CARDS)
            {
                for (var i = 0; i < Cards.Length; i++)
                {
                    if (GameContent.UnitCards.Count >= (i + 1) * Page)
                        Cards[i] = new Card(Cards[i].Position, GameContent.CardTexture, GameContent.UnitCards[(i + 1) * Page - 1].Card_Decoration, new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, GameContent.UnitCards[(i + 1) * Page - 1].Name, GameContent.UnitCards[(i + 1) * Page - 1].Damage.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].Armor.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].AttackRadius.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].Speed.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].HP.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].Cost.ToString(), 141, 315, 4, 4, 37, true, MapZones.ALL, Layer: Layer);
                    else
                        Cards[i].Visible = false;
                }
            }
            else
            if(State == CollectionState.GERMANY_CARDS)
            {
                for (var i = 0; i < Cards.Length; i++)
                {
                    if (GameContent.GermanyUnitCards.Count >= (i + 1) * Page)
                        Cards[i] = new Card(Cards[i].Position, GameContent.CardTexture, GameContent.UnitCards[(i + 1) * Page - 1].Card_Decoration, new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, GameContent.UnitCards[(i + 1) * Page - 1].Name, GameContent.UnitCards[(i + 1) * Page - 1].Damage.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].Armor.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].AttackRadius.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].Speed.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].HP.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].Cost.ToString(), 141, 315, 4, 4, 37, true, MapZones.ALL, Layer: Layer);
                    else
                        Cards[i].Visible = false;
                }
            }
            else
            if (State == CollectionState.SOVIET_CARDS)
            {
                for (var i = 0; i < Cards.Length; i++)
                {
                    if (GameContent.SovietUnitCards.Count >= (i + 1) * Page)
                        Cards[i] = new Card(Cards[i].Position, GameContent.CardTexture, GameContent.UnitCards[(i + 1) * Page - 1].Card_Decoration, new Vector2(16, 9), 200, 10, 0, 13, new Animation(14, 16, true), new Animation(2, 6, false), new Animation(7, 12, false), new Animation(1, 1, true), 0, GameContent.UI_InfoFont, Color.White, GameContent.UnitCards[(i + 1) * Page - 1].Name, GameContent.UnitCards[(i + 1) * Page - 1].Damage.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].Armor.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].AttackRadius.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].Speed.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].HP.ToString(), GameContent.UnitCards[(i + 1) * Page - 1].Cost.ToString(), 141, 315, 4, 4, 37, true, MapZones.ALL, Layer: Layer);
                    else
                        Cards[i].Visible = false;
                }
            }
        }

        public void Draw(SpriteBatch Target)
        {
            for (var i = 0; i < Cards.Length; i++)
            {
                Cards[i].Draw(Target);
            }
        }
    }
}
