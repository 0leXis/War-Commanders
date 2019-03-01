﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
    class LogInForm : IDrawable
    {
        BasicSprite BackGround;
        ScreenBr Br;

        BasicSprite Connecting_Icon;
        BasicText Connecting_Text;

        BasicText Name;
        BasicText Password;
        EditBox NameEdit;
        EditBox PassEdit;

        Button Register;
        Button Login;

        bool IsConnectingState;

        public Vector2 Position
        {
            get
            {
                return BackGround.Position;
            }
            set
            {
                BackGround.Position = value;
                Name.Position = Position + new Vector2(10);
                NameEdit.Position = Name.Position + new Vector2(0, 30);
                Password.Position = NameEdit.Position + new Vector2(0, NameEdit.Texture.Height);
                PassEdit.Position = Password.Position + new Vector2(0, 30);

                Login.Position = PassEdit.Position + new Vector2(0, PassEdit.Texture.Height);
                Register.Position = Login.Position + new Vector2(0, PassEdit.Texture.Height);

                Connecting_Icon.Position = value + new Vector2(BackGround.Texture.Width, BackGround.Texture.Height) / 2;
                Connecting_Text.Position = Connecting_Icon.Position - new Vector2(Connecting_Text.Font.MeasureString("Подключение к серверу...").X, 64);
            }
        }

        public LogInForm(Vector2 Position, Texture2D BackGroundTexture, Texture2D ButtonTexture, Texture2D EditTexture, Texture2D ConnectingIconTexture, SpriteFont Font, Color TextColor, GraphicsDevice gr, float Layer = BasicSprite.DefaultLayer)
        {
            BackGround = new BasicSprite(Position, BackGroundTexture, Layer);
            Name = new BasicText(Position + new Vector2(10), "Логин:", Font, TextColor, Layer - 0.0005f);
            NameEdit = new EditBox(Name.Position + new Vector2(0, 30), EditTexture, "", Font, TextColor, 20, false, Layer - 0.0005f);
            Password = new BasicText(NameEdit.Position + new Vector2(0, NameEdit.Texture.Height), "Пароль:", Font, TextColor, Layer - 0.0005f);
            PassEdit = new EditBox(Password.Position + new Vector2(0, 30), EditTexture, "", Font, TextColor, 20, true, Layer - 0.0005f);

            Login = new Button(PassEdit.Position + new Vector2(0, PassEdit.Texture.Height + 5), ButtonTexture, "Вход", Font, TextColor, ButtonTexture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.0005f);
            Register = new Button(Login.Position + new Vector2(0, PassEdit.Texture.Height + 50), ButtonTexture, "Регистрация", Font, TextColor, ButtonTexture.Width / 4, 60, 0, new Animation(1, 1, true), 2, 3, Layer - 0.0005f);

            Connecting_Icon = new BasicSprite(Position + new Vector2(BackGround.Texture.Width, BackGround.Texture.Height) / 2, ConnectingIconTexture, new Vector2(ConnectingIconTexture.Width, ConnectingIconTexture.Height) / 2, 0f, Layer - 0.0005f);
            Connecting_Icon.Visible = false;
            Connecting_Text = new BasicText(Connecting_Icon.Position - new Vector2(Font.MeasureString("Подключение к серверу...").X / 2, 64), "Подключение к серверу...", Font, TextColor, Layer - 0.0005f);
            Connecting_Text.Visible = false;

            Br = new ScreenBr(Config.Resolutions[Config.CurrResolution].ToVector2(), 3, 180, gr, Layer + 0.0005f);
        }

        public void ShowConnectingState()
        {
            IsConnectingState = true;

            Connecting_Icon.Visible = true;
            Connecting_Text.Visible = true;

            Name.Visible = false;
            NameEdit.Visible = false;
            Password.Visible = false;
            PassEdit.Visible = false;

            Login.Visible = false;
            Register.Visible = false;
        }

        public void ShowNormalState()
        {
            IsConnectingState = false;

            Connecting_Icon.Visible = false;
            Connecting_Text.Visible = false;

            Name.Visible = true;
            NameEdit.Visible = true;
            Password.Visible = true;
            PassEdit.Visible = true;

            Login.Visible = true;
            Register.Visible = true;
        }

        public void Update()
        {
            NameEdit.Update();
            PassEdit.Update();
            Br.UpdateAnims();
            if (Login.Update() == ButtonStates.CLICKED)
            {
                //TODO: Запрос на сервер
                ShowConnectingState();
            }
            else
            if (Register.Update() == ButtonStates.CLICKED)
            {
                //TODO: Запрос на сервер
                ShowConnectingState();
            }

            if (IsConnectingState)
            {
                Connecting_Icon.RotateOn(0.01f);
            }
        }

        public void Draw(SpriteBatch Target)
        {
            BackGround.Draw(Target);
            Br.Draw(Target);

            Name.Draw(Target);
            Password.Draw(Target);
            NameEdit.Draw(Target);
            PassEdit.Draw(Target);

            Register.Draw(Target);
            Login.Draw(Target);

            Connecting_Icon.Draw(Target);
            Connecting_Text.Draw(Target);
        }
    }
}
