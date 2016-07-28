using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace TunnelDecent
{
    public class PlayerSprite : AnimatedSprite
    {
        public int lastHighScoreIndex = 11;
        private const float AccelerometerScale = 1.5f;
        protected Direction lastDirection = Direction.Right;

        SoundEffect explodeEffect;
        SoundEffectInstance explodeInstance;

        // input
        protected bool previousMoveRightPressed = false;
        protected bool previousMoveLeftPressed = false;
        protected bool previousJumpPressed = false;
        protected bool previousSpecialPressed = false;
        protected bool previousShootPressed = false;
        protected bool moveRightPressed = false;
        protected bool moveLeftPressed = false;
        protected bool moveRightTriggered = false;
        protected bool moveLeftTriggered = false;


        // characteristics
        protected float movementSpeed = 15f;
        protected int deadDuration = 1000;

        // state
        protected bool isDead = false;
        protected int deadElapsed = 0;

        // stats
        protected int points = 0;

        Vector2 particleOffset;


        public PlayerSprite()
            : base("Ship", new Point(64,64), new Point(32,32), 1, Vector2.Zero, new Vector2(480 / 2 - 32, 800 - 80 - 120))
        {
            AddAnimation(new Animation("Idle", 1, 1, 100, false, SpriteEffects.FlipHorizontally));
            PlayAnimation("Idle");
            particleOffset = new Vector2(frameDimensions.X / 2, frameDimensions.Y - 10);
            Activate();

            explodeEffect = TunnelGame.sigletonGame.Content.Load<SoundEffect>("Audio/explosion");
            explodeInstance = explodeEffect.CreateInstance();
            explodeInstance.Volume = .5f;
        }



        

        public Direction LastDirection
        {
            get { return lastDirection; }
        }

        
        public override void CollisionAction(GameSprite otherSprite)
        {
            Die();
        }



        public double CaptureInput()
        {
            AccelerometerState accelState = Accelerometer.GetState();

            double movement = 0f;
            if (accelState.IsActive)
            {
                // set our movement speed
                movement = MathHelper.Clamp(accelState.Acceleration.X * AccelerometerScale, -1f, 1f);

                // set values of move left or move right
                if (movement > 0.15f)
                    moveRightPressed = true;
                else
                    moveRightPressed = false;
                if (movement < -0.15f)
                    moveLeftPressed = true;
                else
                    moveLeftPressed = false;
            }

            // debugging input
            if (InputManager.IsKeyPressed(Keys.Left))
            {
                moveLeftPressed = true;
                movement = -.75f;
            }
            else
                moveLeftPressed = false;
            if (InputManager.IsKeyPressed(Keys.Right))
            {
                moveRightPressed = true;
                movement = .75f;
            }
            else
                moveRightPressed = false;

            
            if (!previousMoveLeftPressed && moveLeftPressed)
            {
                moveLeftTriggered = true;
            }
            else
            {
                moveLeftTriggered = false;
            }

            if (!previousMoveRightPressed && moveRightPressed)
            {
                moveRightTriggered = true;
            }
            else
            {
                moveRightTriggered = false;
            }


            previousMoveRightPressed = moveRightPressed;
            previousMoveLeftPressed = moveLeftPressed;


            return movement;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            double movement = CaptureInput();

            if (isDead)
            {
                deadElapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (deadElapsed > deadDuration)
                    Deactivate();
                return;
            }

            // handle movement input
            position.X += movementSpeed * (float)movement;

            if (position.X + frameDimensions.X > 480)
                position.X = 480 - frameDimensions.X;
            if (position.X < 0)
                position.X = 0;

            if (!GameplayScreen.isBoosting)
                ParticleSystem.AddParticles(position + particleOffset, ParticleType.Explosion, sizeScale: .3f, lifetimeScale: .5f);
            else
                ParticleSystem.AddParticles(position + particleOffset, ParticleType.Explosion, sizeScale: .6f, lifetimeScale: .5f);
        }




        public void RewardPoints(int points)
        {
            this.points += points;
        }



      

        public virtual void Die()
        {
            deadElapsed = 0;
            isDead = true;
            PlayAnimation("Dead");
            ParticleSystem.AddParticles(position + new Vector2(frameDimensions.X / 2, frameDimensions.Y / 2),
                ParticleType.Explosion, sizeScale: 2f, lifetimeScale: 1f);
            explodeInstance.Play();
        }

        
        // Accessors
        public bool IsDead
        {
            get { return isDead; }
        }

       
        public int Points
        {
            get { return points; }
        }

        public String PointsString
        {
            get { return points.ToString(); }
        }


    }
}