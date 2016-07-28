
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;


namespace TunnelDecent
{

    public enum CenterDirection
    {
        Right,
        RightSlow,
        Left,
        LeftSlow,
        Straight
    }


        
    public class GameplayScreen : GameScreen
    {
        public static GameplayScreen singleton;
        public static PlayerSprite player;
        Rectangle screenRect = new Rectangle(0, 0, 480, 800);
        bool isPlaying = false;
        int tunnelWidth = 400;
        int shrinkTunnelDuration = 100;
        int shrinkTunnelElapsed = 0;
        int shrinkTunnelAmmount = 6;
        int minTunnelWidth = 120;
        int tunnelCenter = 480 / 2;
        int centerMoveSpeed = 6;
        CenterDirection  moveDirection = CenterDirection.Left;
        WallPair[] wallPairs = new WallPair[210];
        StarSprite[] stars = new StarSprite[80];
        WallPair topPair;
        int level;
        int newLevelTimerDuration = 2500;
        int newLevelTimerElapsed = 0;
        bool newLevelTimerActive = false;
        int newLevelStartDuration = 1500;
        int newLevelStartElapsed = 1500;
        Color levelColor;
        int highScore;
        public static string highScoreString;
        Texture2D title;
        Vector2 titleLoc;

        static public bool isBoosting = false;
        int boostDuration = 200;
        int boostElapsed = 0;
        int boostSpeed = 6;

        public GameplayScreen()
            : base()
        {
            singleton = this;
            title = GameSprite.game.Content.Load<Texture2D>("Textures/title");
            titleLoc = new Vector2(480 / 2 - title.Width/2, 50);
            EnabledGestures = GestureType.None;
            player = new PlayerSprite();

            ParticleSystem.Initialize(100);

            LoadHighScore();

            ResetGame();
            isPlaying = false;
        }


        public void LoadHighScore()
        {
            IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();

            IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("HighScore",
                FileMode.OpenOrCreate,
                FileAccess.Read,
                isoFile);

            BinaryReader reader = new BinaryReader(isoStream);

            // read out high score
            try
            {
                highScore = reader.ReadInt32();
            }
            catch (Exception)
            {
                highScore = 0;
            }
            highScoreString = "Best: " + highScore.ToString();

            reader.Close();
            isoStream.Close();
        }


        public void SaveHighScore()
        {
            IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();

            IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("HighScore",
                FileMode.Create,
                FileAccess.Write,
                isoFile);

            BinaryWriter writer = new BinaryWriter(isoStream);

            // write high score
            writer.Write((Int32)highScore);
            highScoreString = "Best: " + highScore.ToString();

            writer.Close();
            isoStream.Close();
        }


        public void ResetGame()
        {
            IsLoadingNext = false;

            tunnelWidth = 400;
            shrinkTunnelElapsed = 0;
            WallSprite.wallMoveSpeed = 10;
            tunnelCenter = 480 / 2;
            level = 1;
            levelColor = Color.Purple;
            newLevelStartElapsed = 0;
            minTunnelWidth = 120;
            centerMoveSpeed = 5;
            newLevelTimerActive = false;
            isBoosting = false;
            moveDirection = CenterDirection.Straight;

            // setup wall pairs initially
            wallPairs[0] = new WallPair();
            topPair = wallPairs[0];
            topPair.leftWall.position = new Vector2(tunnelCenter - tunnelWidth / 2 - topPair.leftWall.FrameDimensions.X, 805);
            topPair.rightWall.position = new Vector2(tunnelCenter + tunnelWidth / 2, 805);

            for (int i = 1; i < wallPairs.Length; i++)
            {
                wallPairs[i] = new WallPair();
                MovePairToTop(wallPairs[i]);
            }

            for (int i = 1; i < stars.Length; i++)
            {
                stars[i] = new StarSprite();
                stars[i].Reset();
                stars[i].position.Y = -1 * TunnelGame.rand.Next(15, 800);
            }

            player = new PlayerSprite();
            isPlaying = true;
        }


        public void MovePairToTop(WallPair wallPair)
        {
            if (moveDirection == CenterDirection.Right)
                tunnelCenter += 2;
            else if (moveDirection == CenterDirection.Left)
                tunnelCenter -= 2;
            else if (moveDirection == CenterDirection.LeftSlow)
                tunnelCenter -= 1;
            else if (moveDirection == CenterDirection.RightSlow)
                tunnelCenter += 1;


            shrinkTunnelElapsed++;
            if (shrinkTunnelElapsed > shrinkTunnelDuration)
            {
                tunnelWidth -= shrinkTunnelAmmount;
                shrinkTunnelElapsed = 0;
            }
            if (tunnelWidth < minTunnelWidth)
            {
                LevelUp();
            }


            if (!isBoosting)
            {
                player.RewardPoints(4 * level);
            }
            else
            {
                player.RewardPoints(4 * level * 2);
            }



            if (moveDirection == CenterDirection.Left)
            {
                if (tunnelCenter - tunnelWidth / 2 < 3)
                {
                    moveDirection = CenterDirection.Right;
                }
            }
            else if (moveDirection == CenterDirection.Right)
            {
                if (tunnelCenter + tunnelWidth / 2 > 477)
                {
                    moveDirection = CenterDirection.Left;
                }
            }
            else if (moveDirection == CenterDirection.LeftSlow)
            {
                if (tunnelCenter - tunnelWidth / 2 < 3)
                {
                    moveDirection = CenterDirection.RightSlow;
                }
            }
            else if (moveDirection == CenterDirection.RightSlow)
            {
                if (tunnelCenter + tunnelWidth / 2 > 477)
                {
                    moveDirection = CenterDirection.LeftSlow;
                }
            }

            wallPair.leftWall.wallColor = levelColor;
            wallPair.rightWall.wallColor = levelColor;
            wallPair.leftWall.position.Y = topPair.leftWall.position.Y - 4;
            wallPair.rightWall.position.Y = topPair.rightWall.position.Y - 4;
            wallPair.leftWall.position.X = tunnelCenter - tunnelWidth / 2 - wallPair.leftWall.FrameDimensions.X;
            wallPair.rightWall.position.X = tunnelCenter + tunnelWidth / 2;
            topPair = wallPair;
        }


        public override void LoadContent()
        {
            ScreenManager.Game.ResetElapsedTime();
        }



        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive && !coveredByOtherScreen)
            {

                ParticleSystem.Update(gameTime);

                if (isPlaying)
                {
                    // run game

                    if (newLevelTimerActive)
                    {
                        newLevelTimerElapsed += gameTime.ElapsedGameTime.Milliseconds;
                        if (newLevelTimerElapsed >= newLevelTimerDuration)
                        {
                            newLevelTimerActive = false;
                            WallSprite.wallMoveSpeed += 3;
                            newLevelStartElapsed = 0;
                        }
                    }

                    if (isBoosting)
                    {
                        boostElapsed += gameTime.ElapsedGameTime.Milliseconds;
                        if (boostElapsed > boostDuration)
                        {
                            isBoosting = false;
                            WallSprite.wallMoveSpeed -= boostSpeed;
                        }
                    }

                    if (newLevelStartElapsed < newLevelStartDuration)
                        newLevelStartElapsed += gameTime.ElapsedGameTime.Milliseconds;

                    // update stars
                    for (int i = 1; i < stars.Length; i++)
                    {
                        stars[i].Update(gameTime);
                    }

                    // update player
                    player.Update(gameTime);


                    
                    // adjust center movement
                    if (TunnelGame.rand.Next(0, 20) == 0)
                    {
                        moveDirection = (CenterDirection)TunnelGame.rand.Next(0, 5);
                    }


                    // update walls
                    for (int i = 0; i < wallPairs.Length; i++)
                    {
                        wallPairs[i].Update(gameTime, this);
                    }

                    for (int i = 0; i < wallPairs.Length; i++)
                    {
                        if (wallPairs[i].leftWall.position.Y > 805)
                        {
                            MovePairToTop(wallPairs[i]);
                        }
                    }

                    // check for collision
                    for (int i = 0; i < wallPairs.Length; i++)
                    {
                        bool collision = player.CollisionDetect(wallPairs[i].leftWall);
                        if (collision)
                            player.CollisionAction(wallPairs[i].leftWall);
                        else
                        {
                            collision = player.CollisionDetect(wallPairs[i].rightWall);
                            if (collision)
                                player.CollisionAction(wallPairs[i].rightWall);
                        }
                    }


                    // check for end of game
                    if (player.IsActive == false)
                    {
                        isPlaying = false;
                        CheckHighScore();

                        AddNextScreenAndExit(new LeaderboardScreen());
                    }

                }
                else
                {
                    // give instructions
                }

            }  
        }


        public void CheckHighScore()
        {
            if (player.Points > highScore)
            {
                highScore = player.Points;
                SaveHighScore();
            }
        }


        public override void HandleInput()
        {
            if (InputManager.IsBackTriggered())
            {
                AddNextScreen(new PauseScreen());
            }
            if (InputManager.IsLocationPressed(TunnelGame.ScreenSize))
            {
                if (!isBoosting)
                {
                    isBoosting = true;
                    boostElapsed = 0;
                    WallSprite.wallMoveSpeed += boostSpeed;
                }
            }
        }


        public void LevelUp()
        {
            level++;
            tunnelWidth = 300;
            minTunnelWidth -= 5;
            newLevelTimerElapsed = 0;
            newLevelTimerActive = true;
            centerMoveSpeed += 2;

            if (level == 2)
                levelColor = Color.Red;
            else if (level == 3)
                levelColor = Color.Blue;
            else if (level == 4)
                levelColor = Color.Green;
            else if (level == 5)
                levelColor = Color.Yellow;
            else if (level == 6)
                levelColor = Color.Orange;

        }


        Vector2 scoreLocation = new Vector2(10, 10);
        Vector2 centerText = new Vector2(480 / 2, 800 / 2);
        Vector2 instruction1 = new Vector2(480 / 2, 800 / 2 - 180);
        Vector2 instruction2 = new Vector2(480 / 2, 800 / 2 - 80);
        Vector2 boost1 = new Vector2(480 / 2, 800 / 2 + 20);
        Vector2 boost2 = new Vector2(480 / 2, 800 / 2 + 50);
        Vector2 instruction3 = new Vector2(480 / 2, 800 / 2 + 150);
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();

            for (int i = 1; i < stars.Length; i++)
            {
                stars[i].Draw(spriteBatch, 0f);
            }

            player.Draw(spriteBatch, 0f);

            for (int i = 0; i < wallPairs.Length; i++)
            {
                wallPairs[i].leftWall.Draw(spriteBatch, 0f);
                wallPairs[i].rightWall.Draw(spriteBatch, 0f);
            }


            spriteBatch.DrawString(Fonts.DescriptionFont, player.PointsString, scoreLocation, Color.White);

            Vector2 stringSize = Fonts.DescriptionFont.MeasureString(highScoreString); 
            stringSize.X = 470 - stringSize.X;
            stringSize.Y = 10;
            spriteBatch.DrawString(Fonts.DescriptionFont, highScoreString, stringSize, Color.White);

            if (newLevelStartDuration > newLevelStartElapsed)
            {
                Fonts.DrawCenteredText(spriteBatch, Fonts.HeaderFont, "Level " + level.ToString(), centerText, levelColor);
            }

            spriteBatch.End();

            ParticleSystem.Draw(spriteBatch, gameTime);
        }


        public bool IsPlaying
        {
            get { return isPlaying; }
            set { isPlaying = value; }
        }
    }
}
