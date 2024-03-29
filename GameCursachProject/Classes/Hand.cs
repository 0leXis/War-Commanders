﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
	class Hand : IDrawable
    {
        private List<Card> Cards;
        private List<Card> KilledNonTargetCards;
        private int _ChoosedCard = -1;
        private Vector2 _StartPoint;
        private Vector2 _CurrentScreenRes;
        private Arrow ChooseArrow;
        private float CardLayer;

        public bool IsClick;

        public int CardsCount
        {
            get
            {
                return Cards.Count;
            }
        }

        public Card this[int i]
        {
            get
            {
                return Cards[i];
            }
        }

        public Vector2 CurrentScreenRes 
        {
        	get
        	{
        		return _CurrentScreenRes;
        	}
        	set
        	{
        		_CurrentScreenRes = value;
        		CalculateCardPosition(10, false);
        	}
        }

        public int ChoosedCard { get { return _ChoosedCard; } }

        public Hand(Vector2 CenterPoint, Card[] Cards, Vector2 CurrentScreenRes, Texture2D ArrowSegment, Texture2D ArrowEndSegment, float CardLayer)
        {
            this.Cards = new List<Card>();
            KilledNonTargetCards = new List<Card>();
            this.CardLayer = CardLayer;
            if(Cards != null)
                foreach (var Card in Cards)
                    this.Cards.Add(Card);
            this.CurrentScreenRes = CurrentScreenRes;
            ChooseArrow = new Arrow(new Vector2((300 + CurrentScreenRes.X) / 2, CurrentScreenRes.Y + 20), new Vector2(400, 400), ArrowSegment, ArrowEndSegment, 0.30f);
            ChooseArrow.Visible = false;
            ChooseArrow.Disappear();
            IsClick = false;
        }

        public void CalculateCardPosition(int MoveSpeed, bool WithAnim, params int[] WithoutCards)
        {
            if (Cards.Count != 0)
            {
            	_StartPoint = new Vector2((CurrentScreenRes.X - (Cards.Count - WithoutCards.Length - 1) * Cards[0].FrameSize.X * 2 / 5 + Cards[0].FrameSize.X) / 2, CurrentScreenRes.Y - Cards[0].FrameSize.Y * 0.5f);
                var tmp = 0;
                for (var i = 0; i < Cards.Count; i++)
                {
                    Cards[i].Layer = CardLayer + i * 0.001f;
                    if (WithoutCards.Contains(i))
                        tmp += 1;
                    else
                    if (WithAnim)
                        Cards[i].StartMove(new Vector2(_StartPoint.X + (i - tmp) * Cards[0].FrameSize.X * 2 / 5, _StartPoint.Y), MoveSpeed);
                    else
                        Cards[i].Position = new Vector2(_StartPoint.X + (i - tmp) * Cards[0].FrameSize.X * 2 / 5, _StartPoint.Y);
                }
            }
        }

        public void AddCards(int MoveSpeed, params Card[] Cards)
        {
            foreach(var card in Cards)
            {
                this.Cards.Add(card);
            }
            CalculateCardPosition(MoveSpeed, true);
        }

        public void Update(ref bool IsMouseHandled, Map TiledMap, Camera cam, float CardAppearPos, bool IsPlayerTurn, MapZones PlayerZone, GameState state, int PlayerMoney)
        {
            ChooseArrow.Update();
            for (var i = 0; i < KilledNonTargetCards.Count; i++)
                if (KilledNonTargetCards[i] != null)
                    if (KilledNonTargetCards[i].CurrAnimName == "NONE")
                    {
                        KilledNonTargetCards.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        KilledNonTargetCards[i].UpdateAnims();
                    }
            if (_ChoosedCard == -1)
            {
                for (var i = 0; i < Cards.Count; i++)
                    if (Cards[i] != null)
                    {
                        Cards[i].MoveUpdate();
                        Cards[i].UpdateAnims();
                        if (!IsMouseHandled && Cards[i].Update() != ButtonStates.NONE)
                        {
                            IsMouseHandled = true;
                            _ChoosedCard = i;
                        }
                        else
                        {
                            Cards[i].StopAnimation(true);
                            //Cards[i].IterationReset();
                            Cards[i].Down();
                        }
                    }
            }
            else
                if (Cards[_ChoosedCard] != null)
                {
                    IsMouseHandled = true;
                    for (var i = 0; i < Cards.Count; i++)
                    {
                        Cards[i].MoveUpdate();
                        Cards[i].UpdateAnims();
                    }
                    if (MouseControl.IsLeftBtnClicked)
                        if (IsClick)
                        {
                            IsClick = false;
                            Cards[_ChoosedCard].IsPressed = false;
                        }
                        else
                        {
                            IsClick = true;
                            Cards[_ChoosedCard].IsPressed = true;
                        }
                    var Upd = Cards[_ChoosedCard].Update();
                    if (IsPlayerTurn && (IsClick || Upd == ButtonStates.PRESSED))
                    {
                        CalculateCardPosition(10, true, _ChoosedCard);
                        Cards[_ChoosedCard].Down();
                        Cards[_ChoosedCard].SetUpLayer();
                        Cards[_ChoosedCard].Position = new Vector2(MouseControl.X - (Cards[_ChoosedCard].FrameSize.X * 0.25f), MouseControl.Y - (Cards[_ChoosedCard].FrameSize.Y * 0.25f));
                        if (MouseControl.Y < CardAppearPos)
                        {
                            IsMouseHandled = false;
                            if (Cards[_ChoosedCard].IsTargeted)
                            {
                                if (!TiledMap.IsAnimsChanged)
                                    TiledMap.ChangeTilesAnims(2, 1, new Animation(1, 1, true), TiledMap.GetTilesByAllowedZones(Cards[_ChoosedCard].AllowedZones, Cards[_ChoosedCard].AllowedTiles));
                                Cards[_ChoosedCard].Disappear();
                                ChooseArrow.Appear();
                                ChooseArrow.BeginPoint = new Vector2((300 + CurrentScreenRes.X) / 2, CurrentScreenRes.Y + 20);
                                ChooseArrow.EndPoint = new Vector2(MouseControl.X, MouseControl.Y);
                            }
                            else
                                Cards[_ChoosedCard].PlayAnimation("Choosed");
                        }
                        else
                        {
                            if (Cards[_ChoosedCard].IsTargeted)
                            {
                                TiledMap.SetDefault();
                                ChooseArrow.Disappear();
                                Cards[_ChoosedCard].Appear();
                                Cards[_ChoosedCard].IsDisappearing = false;
                            }
                            else
                                Cards[_ChoosedCard].CurrentFrame = 0;
                        }
                    }
                    else
                        if (Upd != ButtonStates.NONE)
                        {
                            if (Upd == ButtonStates.CLICKED)
                            {
                                Cards[_ChoosedCard].Down();
                                if (Cards[_ChoosedCard].IsTargeted)
                                {
                                    ChooseArrow.Disappear();
                                    Cards[_ChoosedCard].Appear();
                                    Cards[_ChoosedCard].IsDisappearing = false;
                                }
                                else
                                {
                                    Cards[_ChoosedCard].StopAnimation(true);
                                }
                                if (MouseControl.Y < CardAppearPos)
                                {
                                	var Tmp = TiledMap.GetTileIJByCoords(MouseControl.MouseToWorldCoords(cam));
                                    Tile TmpTile;
                                    if (Tmp != null)
                                        TmpTile = TiledMap.GetTile(Tmp[0], Tmp[1]);
                                    else
                                        TmpTile = null;
                                    if (Cards[_ChoosedCard].IsTargeted)
                                    {
                                        if (TmpTile != null && PlayerMoney >= Convert.ToInt32(Cards[_ChoosedCard].MoneyInfo.Text) && TiledMap.CheckTileAllowed(new Point(Tmp[0], Tmp[1]), PlayerZone, Cards[_ChoosedCard].AllowedTiles))//DONE: Убрать костыль TmpTile.NotSelectedFrame != 0, ввести тип Player для поиска правильной стороны
                                        {
                                            //TEST: Юниты
                                            //TmpTile.SpawnUnit(new Unit(Vector2.Zero, GameContent.UnitTextures[0], GameContent.UI_Info_Allied, GameContent.UI_InfoFont, Color.White, 392, 20, 5, 3, 6, 1, 2, Side.PLAYER, AttScript, ScrForUnit, new Point(Tmp[0], Tmp[1]), new Animation(8, 17, false), 0.4f), MapZones.RIGHT, TiledMap.UI_VisibleState);
                                            CommandParser.SendCommandToGameServer(new string[]{ "SPAWN", _ChoosedCard.ToString(), Tmp[0].ToString(),Tmp[1].ToString()});
                                            state.SetEnemyTurn();
                                            Cards.RemoveAt(_ChoosedCard);
                                        }
                                        else
                                            CalculateCardPosition(10, true);
                                    }
                                    else
                                    {
                                        Cards[_ChoosedCard].StopAnimation(true);
                                        KilledNonTargetCards.Add(new Card(Cards[_ChoosedCard]));
                                        KilledNonTargetCards.Last().Disappear();
                                        Cards.RemoveAt(_ChoosedCard);
                                    }
                                    TiledMap.SetDefault();
                                }
                                else
                                    CalculateCardPosition(10, true);
                                _ChoosedCard = -1;
                            }
                        }
                        else
                        {
                            Cards[_ChoosedCard].Down();
                            //Cards[_ChoosedCard].IterationReset();
                            _ChoosedCard = -1;
                        }
                }
        }

        public void Draw(SpriteBatch Target)
        {
            ChooseArrow.Draw(Target);
            foreach (var Card in Cards)
                if (Card != null)
                    Card.Draw(Target);
            foreach (var Card in KilledNonTargetCards)
                if (Card != null)
                    Card.Draw(Target);
        }
    }
}
