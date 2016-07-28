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
    public class WallPair
    {
        public WallSprite leftWall = new WallSprite(true);
        public WallSprite rightWall = new WallSprite(false);

        public void Update(GameTime gameTime, GameplayScreen gameplayScreen)
        {
            leftWall.Update(gameTime);
            rightWall.Update(gameTime);
        }
    }


    public class WallSprite : AnimatedSprite
    {
        public static float wallMoveSpeed = 6;
        public Color wallColor = Color.Purple;

        public WallSprite(bool isLeft)
            : base("Gradient", new Point(128,4), new Point(20,20), 1, Vector2.Zero, new Vector2(480 / 2, 800 - 80 - 80))
        {
            if (isLeft)
            {
                AddAnimation(new Animation("Left", 1, 1, 100, false, SpriteEffects.None, wallColor));
                PlayAnimation("Left");
            }
            else
            {
                AddAnimation(new Animation("Right", 1, 1, 100, false, SpriteEffects.FlipHorizontally, wallColor));
                PlayAnimation("Right");
            }

            Activate();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            position.Y += wallMoveSpeed;

            animations[0].Tint = wallColor;
        }
    }
}
