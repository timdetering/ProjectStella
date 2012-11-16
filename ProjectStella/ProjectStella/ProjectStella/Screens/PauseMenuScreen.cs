#region Using Statements
using Microsoft.Xna.Framework;
using System.Globalization;
using System.Threading;
#endregion

namespace ProjectStella
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        #region Fields

        string comingFrom = "PauseMenu";

        static MenuEntry resumeGameMenuEntry;
        static MenuEntry optionsMenuEntry;
        static MenuEntry quitGameMenuEntry;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen()
            : base("Paused", "Normal")
        {
            // Create our menu entries.
            resumeGameMenuEntry = new MenuEntry(Strings.Resume);
            optionsMenuEntry = new MenuEntry(Strings.Options);
            quitGameMenuEntry = new MenuEntry(Strings.Quit);

            // Sets the menu 
            SetMenuText();
            
            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        public static void SetMenuText()
        {
            UpdateLanguage();

            resumeGameMenuEntry.Text = Strings.Resume;
            optionsMenuEntry.Text = Strings.Options;
            quitGameMenuEntry.Text = Strings.Quit;
        }

        /// <summary>
        /// Updates the Pause menu's menu entries to the proper language.
        /// </summary>
        public static void UpdateLanguage()
        {
            if (GameOptions.Language == "French")
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
            else if (GameOptions.Language == "English")
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(comingFrom), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }

        #endregion
    }
}