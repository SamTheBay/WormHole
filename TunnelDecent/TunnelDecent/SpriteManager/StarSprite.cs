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
    class StarSprite : AnimatedSprite
    {
        int moveSpeed = 5;
        int minMoveSpeed = 15;
        int maxMoveSpeed = 30;


        public StarSprite()
            : base("vgradient", new Point(3,25), new Point(20,20), 1, Vector2.Zero, new Vector2(480 / 2, 800 - 80 - 80))
        {
            AddAnimation(new Animation("Idle", 1, 1, 100, false, SpriteEffects.FlipHorizontally, new Color(100,100,100)));
            PlayAnimation("Idle");
            Activate();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            position.Y += moveSpeed;

            if (Position.Y > 805)
            {
                Reset();
            }

        }


        public void Reset()
        {
            position.X = TunnelGame.rand.Next(5, 475);
            position.Y = -15;
            moveSpeed = TunnelGame.rand.Next(minMoveSpeed, maxMoveSpeed);
        }



    }
}
