#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace ProjectStella
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        #region Fields

        static Game instance;

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;

        bool renderVsync = true;


        // By preloading any assets used by UI rendering, we avoid framerate glitches
        // when they suddenly need to be loaded in the middle of a menu transition.
        static readonly string[] preloadAssets =
        {
            "Images/gradient",
            "Images/Controller/controller",
            "Images/Buttons/A",
            "Images/Buttons/B",
            "Images/Buttons/X",
            "Images/Buttons/Y",
        };

        
        #endregion

        #region Initialization


        /// <summary>
        /// The main game constructor.
        /// </summary>
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            audioEngine = new AudioEngine("content/Sounds/sounds.xgs");
            waveBank = new WaveBank(audioEngine, "content/Sounds/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "content/Sounds/Sound Bank.xsb");

            graphics.PreferredBackBufferWidth = GameOptions.ScreenWidth;
            graphics.PreferredBackBufferHeight = GameOptions.ScreenHeight;
            graphics.PreferMultiSampling = true;
            //graphics.IsFullScreen = true;

            IsFixedTimeStep = renderVsync;
            graphics.SynchronizeWithVerticalRetrace = renderVsync;

            // Create the screen manager component.
            screenManager = new ScreenManager(this, graphics);

            Components.Add(screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
        }


        /// <summary>
        /// Loads graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            foreach (string asset in preloadAssets)
            {
                Content.Load<object>(asset);
            }
        }


        #endregion

        #region Draw


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }

        public void ToggleFullScreen()
        {
            graphics.ToggleFullScreen();
        }


        #endregion

        #region Entry Point
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            using (Game game = new Game())
            {
                instance = game;
                game.Run();
            }
        }
        #endregion

        static public Game GetInstance()
        {
            return instance;
        }
    }


    /* Old entry point. Might use at some point in the future.
    #region Entry Point
        #if WINDOWS || XBOX
            static class Program
            {
                /// <summary>
                /// The main entry point for the application.
                /// </summary>
                static void Main(string[] args)
                {
                    using (Game game = new Game())
                    {
                        game.Run();
                    }
                }
            }
        #endif
    #endregion
     * */
}
