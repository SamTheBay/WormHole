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
    class TitleScreen : MenuScreen
    {

        public TitleScreen()
        {
            IsPopup = true;
            RestartOnVisible = true;

            MenuEntry entry = new MenuEntry("");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = GameSprite.game.Content.Load<Texture2D>("Textures/Buttons/Start");
            entry.PressTexture = GameSprite.game.Content.Load<Texture2D>("Textures/Buttons/StartPress");
            entry.SetStartAnimation(new Vector2(-entry.Texture.Width - 10, 200), new Vector2(240 - entry.Texture.Width / 2, 200), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            MenuEntries.Add(entry);

            entry = new MenuEntry("");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = GameSprite.game.Content.Load<Texture2D>("Textures/Buttons/Inst");
            entry.PressTexture = GameSprite.game.Content.Load<Texture2D>("Textures/Buttons/InstPress");
            entry.SetStartAnimation(new Vector2(490, 280), new Vector2(240 - entry.Texture.Width / 2, 280), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            MenuEntries.Add(entry);

            entry = new MenuEntry("");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = GameSprite.game.Content.Load<Texture2D>("Textures/Buttons/Leader");
            entry.PressTexture = GameSprite.game.Content.Load<Texture2D>("Textures/Buttons/LeaderPress");
            entry.SetStartAnimation(new Vector2(-entry.Texture.Width - 10, 360), new Vector2(240 - entry.Texture.Width / 2, 360), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            MenuEntries.Add(entry);

            entry = new MenuEntry("");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = GameSprite.game.Content.Load<Texture2D>("Textures/Buttons/Games");
            entry.PressTexture = GameSprite.game.Content.Load<Texture2D>("Textures/Buttons/GamesPress");
            entry.SetStartAnimation(new Vector2(490, 440), new Vector2(240 - entry.Texture.Width / 2, 440), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            MenuEntries.Add(entry);

            entry = new MenuEntry("");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = GameSprite.game.Content.Load<Texture2D>("Textures/Buttons/About");
            entry.PressTexture = GameSprite.game.Content.Load<Texture2D>("Textures/Buttons/AboutPress");
            entry.SetStartAnimation(new Vector2(-entry.Texture.Width - 10, 520), new Vector2(240 - entry.Texture.Width / 2, 520), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            MenuEntries.Add(entry);
        }



        void entry_Selected(object sender, EventArgs e)
        {
            if (selectorIndex == 0)
            {
                GameplayScreen screen = GameplayScreen.singleton;
                screen.ResetGame();
                AddNextScreen(screen);
            }
            else if (selectorIndex == 1)
            {
                AddNextScreen(new InstructionsScreen());
            }
            else if (selectorIndex == 2)
            {
                AddNextScreen(new LeaderboardScreen());
            }
            else if (selectorIndex == 3)
            {
                AddNextScreen(new OtherGamesScreen());
            }
            else if (selectorIndex == 4)
            {
                AddNextScreen(new AboutScreen());
            }
        }




        public override void ExitScreen()
        {
            TunnelGame.sigletonGame.Exit();
        }

    }
}
