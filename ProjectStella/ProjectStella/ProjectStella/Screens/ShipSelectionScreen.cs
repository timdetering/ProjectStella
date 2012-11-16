#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace ProjectStella
{
    class ShipSelectionScreen : MenuScreen
    {
        #region Fields

        ContentManager content;
        ScreenManager screenManager;
        GameManager gameManager;

        #endregion

        #region Initialize

        public ShipSelectionScreen(ScreenManager screenManager)
            : base("Ship Selection","Normal")
        {
            this.screenManager = screenManager;
            gameManager = screenManager.gameManager;

            MenuEntry start = new MenuEntry("Start");
            MenuEntry back = new MenuEntry("Back");

            start.Selected += PlayGameMenuEntrySelected;
            back.Selected += BackMenuEntrySelected;

            MenuEntries.Add(start);
            MenuEntries.Add(back);
        }

        public override void LoadContent()
        {
            // If the content manager doesn't exist, then one is created
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            content.Dispose();

            base.UnloadContent();
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
        }

        /// <summary>
        /// Event handler for when the Back menu entry is selected.
        /// </summary>
        void BackMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            BlankTransitionScreen.Load(ScreenManager, true, e.PlayerIndex, new BackgroundScreen(), new MainMenuScreen());
        }

        /// <summary>
        /// Overrides the OnCancel method so that you don't go back to the
        /// blank transition screen, and get stuck in a dead screen.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            base.OnCancel(playerIndex);

            BlankTransitionScreen.Load(ScreenManager, false, playerIndex, new BackgroundScreen(), new MainMenuScreen());
        }

        #endregion
    }
}