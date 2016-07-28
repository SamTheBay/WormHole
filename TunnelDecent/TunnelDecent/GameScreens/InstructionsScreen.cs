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
    class InstructionsScreen : MenuScreen
    {

        public InstructionsScreen()
        {
            IsPopup = true;
        }



        Vector2 instruction1 = new Vector2(480 / 2, 800 / 2 - 150);
        Vector2 instruction2 = new Vector2(480 / 2, 800 / 2 - 50);
        Vector2 boost1 = new Vector2(480 / 2, 800 / 2 + 80);
        Vector2 boost2 = new Vector2(480 / 2, 800 / 2 + 110);
        Vector2 scoreLocation = new Vector2(10, 10);
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();

            Fonts.DrawCenteredText(spriteBatch, Fonts.HeaderFont, "Tilt to Move", instruction1, Color.White);
            Fonts.DrawCenteredText(spriteBatch, Fonts.HeaderFont, "Avoid the Worm Hole", instruction2, Color.White);
            Fonts.DrawCenteredText(spriteBatch, Fonts.HeaderFont, "Touch Screen to Boost", boost1, Color.White);
            Fonts.DrawCenteredText(spriteBatch, Fonts.HeaderFont, "for Extra Points", boost2, Color.White);


            spriteBatch.End();
        }

    }
}
