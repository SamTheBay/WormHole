using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TunnelDecent
{
    class PauseScreen : MenuScreen
    {
        public static bool paused = false;
        Texture2D grayOut;

        public PauseScreen()
        {
            paused = true;
            grayOut = InternalContentManager.GetTexture("Gray");
            IsPopup = true;

            MenuEntry entry = new MenuEntry("");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = GameSprite.game.Content.Load<Texture2D>("Textures/Buttons/Resume");
            entry.PressTexture = GameSprite.game.Content.Load<Texture2D>("Textures/Buttons/ResumePress");
            entry.Position = new Vector2(240 - entry.Texture.Width / 2, 250);
            MenuEntries.Add(entry);


            entry = new MenuEntry("");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = GameSprite.game.Content.Load<Texture2D>("Textures/Buttons/DropOut");
            entry.PressTexture = GameSprite.game.Content.Load<Texture2D>("Textures/Buttons/DropOutPress");
            entry.Position = new Vector2(240 - entry.Texture.Width / 2, 400);
            MenuEntries.Add(entry);
        }

        void entry_Selected(object sender, EventArgs e)
        {
            if (selectorIndex == 0)
            {
                ExitScreen();
            }
            else if (selectorIndex == 1)
            {
                GameplayScreen.singleton.CheckHighScore();

                GameplayScreen.player.Die();
                ScreenManager.RemoveScreen(GameplayScreen.singleton);
                GameplayScreen.singleton.IsPlaying = false;

                if (GameplayScreen.player.Points > 0)
                {
                    AddNextScreenAndExit(new LeaderboardScreen());
                }
                else
                {
                    ExitScreen();
                }
            }
        }


        public override void ExitScreen()
        {
            paused = false;
            base.ExitScreenImmediate();
        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();

            spriteBatch.Draw(grayOut, TunnelGame.ScreenSize, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
