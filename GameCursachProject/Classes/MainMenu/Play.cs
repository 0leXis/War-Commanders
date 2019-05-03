using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class Play
    {
        Button[] DeckButtons;

        Button StartEnemySearch;
        Button CancelSearch;
        BasicSprite BackGround;
        BasicSprite Search_Icon;
        BasicText SearchDescription;

        int ChoosedDeck = 0;

        MainMenu Parent;
        public bool IsShown { get; set; }
        public bool IsSearchState { get; set; }
        public Play(Vector2 ScreenRes, float RollbackY, MainMenu Parent, float Layer = BasicSprite.DefaultLayer)
        {
            this.Parent = Parent;
            DeckButtons = new Button[8];
            var TmpX = (ScreenRes.X - GameContent.UI_MainMenu_SovietDeck.Width / 4 * DeckButtons.Length / 2) / (DeckButtons.Length / 2 + 1f);
            var TmpY = 150f;

            for (var i = 0; i < DeckButtons.Length / 2; i++)
            {
                DeckButtons[i] = new Button(new Vector2(TmpX * (i + 1) + GameContent.UI_MainMenu_SovietDeck.Width / 4 * i, RollbackY + TmpY), GameContent.UI_MainMenu_SovietDeck, "Колода " + i, GameContent.UI_ButtonFont, Color.White, GameContent.UI_MainMenu_SovietDeck.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.0005f);
            }
            for (var i = DeckButtons.Length / 2; i < DeckButtons.Length; i++)
            {
                DeckButtons[i] = new Button(new Vector2(TmpX * (i - DeckButtons.Length / 2 + 1) + GameContent.UI_MainMenu_SovietDeck.Width / 4 * (i - DeckButtons.Length / 2), RollbackY + TmpY * 2), GameContent.UI_MainMenu_GermanDeck, "Колода " + i, GameContent.UI_ButtonFont, Color.White, GameContent.UI_MainMenu_GermanDeck.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.0005f);
            }

            StartEnemySearch = new Button(new Vector2((ScreenRes.X - GameContent.UI_MainMenu_Play_StartButton.Width / 4) / 2, ScreenRes.Y - 20 - GameContent.UI_MainMenu_Play_StartButton.Height), GameContent.UI_MainMenu_Play_StartButton, GameContent.UI_MainMenu_Play_StartButton.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.002f);
            BackGround = new BasicSprite(new Vector2((ScreenRes.X - GameContent.UI_MainMenu_LogIn_BackGround.Width) / 2, (ScreenRes.Y - GameContent.UI_MainMenu_LogIn_BackGround.Height) / 2), GameContent.UI_MainMenu_LogIn_BackGround, Layer - 0.0007f);
            Search_Icon = new BasicSprite(BackGround.Position + new Vector2(BackGround.Texture.Width, BackGround.Texture.Height) / 2, GameContent.UI_MainMenu_LogIn_ConnIcon, new Vector2(GameContent.UI_MainMenu_LogIn_ConnIcon.Width, GameContent.UI_MainMenu_LogIn_ConnIcon.Height) / 2, 0f, Layer - 0.001f);
            CancelSearch = new Button(new Vector2((ScreenRes.X - GameContent.UI_MainMenu_Button.Width / 4) / 2, ScreenRes.Y / 2 + 50), GameContent.UI_MainMenu_Button, "Отмена", GameContent.UI_ButtonFont, Color.Black, GameContent.UI_MainMenu_Button.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.001f);
            SearchDescription = new BasicText(new Vector2((ScreenRes.X - GameContent.UI_ButtonFont.MeasureString("Поиск противника").X) / 2, ScreenRes.Y / 2 - 70), "Поиск противника", GameContent.UI_ButtonFont, Color.Black, Layer - 0.001f);
            Hide();
        }

        public void Hide()
        {
            IsShown = false;
            foreach (var button in DeckButtons)
                button.Visible = false;
            StartEnemySearch.Visible = false;
            BackGround.Visible = false;
            Search_Icon.Visible = false;
            CancelSearch.Visible = false;
            SearchDescription.Visible = false;
        }

        public void Show()
        {
            IsShown = true;
            foreach (var button in DeckButtons)
                button.Visible = true;
            StartEnemySearch.Visible = true;
        }

        public void HideCancelButton()
        {
            CancelSearch.Visible = false;
        }

        public void ShowCancelButton()
        {
            CancelSearch.Visible = true;
        }

        public void Update()
        {
            if (IsSearchState)
            {
                Search_Icon.RotateOn(0.01f);
                if(CancelSearch.Update() == ButtonStates.CLICKED)
                {
                    BackGround.Visible = false;
                    Search_Icon.Visible = false;
                    CancelSearch.Visible = false;
                    SearchDescription.Visible = false;
                    IsSearchState = false;
                    Parent.UnlockClicking();
                    CommandParser.SendCommandToMasterServer(new string[] { "STOPSEARCH" });
                }
            }
            else
            {
                for (var i = 0; i < DeckButtons.Length; i++)
                    if (DeckButtons[i].Update() == ButtonStates.CLICKED)
                        ChoosedDeck = i;
                if (StartEnemySearch.Update() == ButtonStates.CLICKED)
                {
                    IsSearchState = true;
                    Parent.LockClicking();
                    BackGround.Visible = true;
                    Search_Icon.Visible = true;
                    CancelSearch.Visible = true;
                    SearchDescription.Visible = true;
                    CommandParser.SendCommandToMasterServer(new string[] { "SEARCH" });
                }
                DeckButtons[ChoosedDeck].PlayAnimation("Selected");
            }
        }

        public void Draw(SpriteBatch Target)
        {
            foreach (var button in DeckButtons)
                button.Draw(Target);
            StartEnemySearch.Draw(Target);
            BackGround.Draw(Target);
            Search_Icon.Draw(Target);
            CancelSearch.Draw(Target);
            SearchDescription.Draw(Target);
        }
    }
}
