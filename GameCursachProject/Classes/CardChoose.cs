using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class CardChoose : IDrawable
    {
        public Vector2 ScreenRes
        {
            get
            {
                return _ScreenRes;
            }
            set
            {
                _ScreenRes = value;
                Move(false);
            }
        }

        private Vector2 _ScreenRes;
        private List<Card> Cards;
        private List<BasicSprite> Replaces;
        private bool[] Replaced;
        private bool WithReplace;
        private Texture2D ReplaceTexture;

        public CardChoose(Vector2 ScreenRes, Texture2D ReplaceTexture)
        {
            this.ReplaceTexture = ReplaceTexture;
            this.ScreenRes = ScreenRes;
            Cards = new List<Card>();
            Replaces = new List<BasicSprite>();
        }

        public void ShowCards(bool WithReplace, params Card[] Cards)
        {
            this.WithReplace = WithReplace;
            this.Cards.Clear();
            if (WithReplace)
            {
                Replaces.Clear();
                Replaced = new bool[Cards.Length];
            }

            foreach(var card in Cards)
            {
                card.Position = ScreenRes;
                this.Cards.Add(card);
                if (WithReplace)
                {
                    var TmpSprite = new BasicSprite(Vector2.Zero, ReplaceTexture, 0.001f);
                    TmpSprite.Visible = false;
                    Replaces.Add(TmpSprite);
                }
            }
            Move();
        }

        private void Move(bool WithAnim = true)
        {
            if(Cards != null)
            if (Cards.Count != 0)
            {
                var Offset = new Vector2((ScreenRes.X - Cards.Count * Cards[0].FrameSize.X) / (Cards.Count + 1), (ScreenRes.Y - Cards[0].FrameSize.Y) / 2);
                for (var i = 0; i < Cards.Count; i++)
                {
                    var TmpVect = new Vector2(Offset.X * (i + 1) + Cards[0].FrameSize.X * i, Offset.Y);
                    if (WithAnim)
                        Cards[i].StartMove(new Vector2(Offset.X * (i + 1) + Cards[0].FrameSize.X * i, Offset.Y), 25);
                    else
                        Cards[i].Position = new Vector2(Offset.X * (i + 1) + Cards[0].FrameSize.X * i, Offset.Y);
                    if (WithReplace)
                        Replaces[i].Position = TmpVect;
                }
            }
        }

        public bool[] GetReplacedCards()
        {
            return Replaced;
        }

        public List<Card> GetCards(bool Copy = false)
        {
            if(Copy)
                return new List<Card>(Cards);
            else
                return Cards;
        }

        public void ClearCardList()
        {
            Cards.Clear();
            Replaces.Clear();
        }

        public void ReplaceCards(params Card[] Cards)
        {
            foreach(var card in Cards)
            {
                for(var i = 0; i < Replaced.Length; i++)
                {
                    if (Replaced[i])
                    {
                        card.Position = ScreenRes;
                        card.StartMove(this.Cards[i].Position, 25);
                        this.Cards[i] = card;
                        Replaced[i] = false;
                        Replaces[i].Visible = false;
                        break;
                    }
                }
            }
        }

        public int Update()
        {
            var result = -1;
            for(var i = 0; i < Cards.Count; i++)
            {
                Cards[i].MoveUpdate();
                Cards[i].UpdateAnims();
                if (Cards[i].Update() == ButtonStates.CLICKED)
                {
                    if (WithReplace)
                    {
                        if (Replaced[i])
                        {
                            Replaced[i] = false;
                            Replaces[i].Visible = false;
                        }
                        else
                        {
                            Replaced[i] = true;
                            Replaces[i].Visible = true;
                        }
                    }
                    else
                    {
                        result = i;
                    }
                }
                Cards[i].SetUp();
                if(Cards[i].CurrAnimName == null)
                {
                    Cards[i].PlayAnimation("Selected");
                }
            }
            return result;
        }

        public void Draw(SpriteBatch Target)
        {
            foreach (var card in Cards)
                card.Draw(Target);
            foreach (var repl in Replaces)
                repl.Draw(Target);
        }
    }
}
