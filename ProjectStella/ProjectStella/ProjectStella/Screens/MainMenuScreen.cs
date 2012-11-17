#region Using Statements
using Microsoft.Xna.Framework;
using System.Globalization;
using System.Threading;
#endregion

namespace ProjectStella
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Fields

        string comingFrom = "MainMenu";

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("Inter Stellarum", "MainMenu")
        {
            Strings.Culture = CultureInfo.CurrentUICulture;

            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry(Strings.PlayGame);
            MenuEntry helpMenuEntry = new MenuEntry(Strings.Help);
            MenuEntry optionsMenuEntry = new MenuEntry(Strings.Options);
            MenuEntry creditsMenuEntry = new MenuEntry(Strings.Credits);
            MenuEntry exitMenuEntry = new MenuEntry(Strings.Exit);

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            helpMenuEntry.Selected += HelpMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            creditsMenuEntry.Selected += CreditsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(helpMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(creditsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            BlankTransitionScreen.Load(ScreenManager, true, e.PlayerIndex, new BackgroundScreen(), new ShipSelectionScreen(ScreenManager));
        }

        /// <summary>
        /// The Event handler for when the Help menu entry is selected.
        /// </summary>
        void HelpMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            BlankTransitionScreen.Load(ScreenManager, false, e.PlayerIndex, new BackgroundScreen(), new HelpMenuScreen());
        }


        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            BlankTransitionScreen.Load(ScreenManager, true, e.PlayerIndex, new BackgroundScreen(), new OptionsMenuScreen(comingFrom));
        }

        /// <summary>
        /// Event handler for when the Credits menu entry is selected.
        /// </summary>

        void CreditsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            BlankTransitionScreen.Load(ScreenManager, false, e.PlayerIndex, new BackgroundScreen(), new CreditsScreen());
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
