using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Advertising.Mobile.Xna;
using Microsoft.Phone.Shell;
using System.Net;
using System.Net.Browser;

namespace TunnelDecent
{

    public class TunnelGame : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        public static ScreenManager screenManager;
        public static Game sigletonGame;
        public static int masterController = 0;
        public static AdControlManager adControlManager;

        public static Rectangle ScreenSize = new Rectangle(0, 0, 480, 800);
        private static Vector2 gameTimeDrawLoc = new Vector2(100, 100);
        private static Vector2 maxGameTimeDrawLoc = new Vector2(100, 130);
        private static Vector2 memoryDrawLoc = new Vector2(100, 160);
        public static Random rand = new Random(DateTime.Now.Millisecond);
       

        public TunnelGame()
        {
            sigletonGame = this;
            graphics = new GraphicsDeviceManager(this);

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Pre-auto scale settings.
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;

            Content.RootDirectory = "Content";

            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;

            // add the screen manager
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            GameSprite.game = this;

            HttpWebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);

            adControlManager = new AdControlManager(Components, false);
            adControlManager.ShowAds = true;

            Activated += new EventHandler<EventArgs>(TunnelGameOnActivated);
            Deactivated += new EventHandler<EventArgs>(TunnelGameDeactivated);
        }


        protected override void Initialize()
        {
            InputManager.Initialize();

            base.Initialize();

            Fonts.LoadContent(Content);
            Accelerometer.Initialize();
            InternalContentManager.Load();
            AudioManager.Initialize(TunnelGame.sigletonGame);
            AudioManager.audioManager.LoadSFX(0, 0);
            new MusicManager();
            //MusicManager.SingletonMusicManager.LoadTune("intro", TunnelDecent.sigletonGame.Content);

            new GameplayScreen();
            screenManager.AddScreen(new MenuBackgroundScreen());
            screenManager.AddScreen(new TitleScreen());
            
        }


        void TunnelGameOnActivated(object sender, EventArgs args)
        {
            if (adControlManager != null)
            {
                adControlManager.Load();
            }

            // check if we have a game currently running
            if (GameplayScreen.singleton != null && GameplayScreen.singleton.IsPlaying && !(screenManager.GetScreens()[screenManager.GetScreens().Length - 1] is PauseScreen))
            {
                screenManager.AddScreen(new PauseScreen());
            }
        }

        void TunnelGameDeactivated(object sender, EventArgs e)
        {
            if (adControlManager != null)
            {
                adControlManager.UnLoad();
            }
        }



        protected override void LoadContent()
        {
            base.LoadContent();

            adControlManager.Load();
        }


        protected override void UnloadContent()
        {
            Fonts.UnloadContent();

            base.UnloadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            base.Update(gameTime);

            adControlManager.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);

        }

        

    }
}
