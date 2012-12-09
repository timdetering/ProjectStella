#region Using Statements
using Microsoft.Xna.Framework;
using System.Globalization;
using System.Threading;
using System;
#endregion

namespace ProjectStella
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        // Determines what screen the player is coming from.
        string comingFrom;

        // Creates all of the menu entries that are on the options screen
        MenuEntry difficultyMenuEntry;
        MenuEntry languageMenuEntry;
        MenuEntry buttonLayoutMenuEntry;
        MenuEntry thumbstickLayoutMenuEntry;
        MenuEntry backMenuEntry;


        MenuEntry frobnicateMenuEntry;
        MenuEntry elfMenuEntry;

        enum Difficulty
        {
            DebugEntry,
            Easy,
            Normal,
            Hard,
        }

        static Difficulty currentDifficulty = Difficulty.Normal;

        static string[] languages = { "English", "French" };
        static int currentLanguage = 0;

        static bool frobnicate = true;

        static int elf = 23;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen(string comingFrom)
            : base("Options","Normal")
        {
            TransitionOnTime = TimeSpan.Zero;
            TransitionOffTime = TimeSpan.FromSeconds(0f);

            this.comingFrom = comingFrom;

            // Create our menu entries.
            difficultyMenuEntry = new MenuEntry(string.Empty);
            languageMenuEntry = new MenuEntry(string.Empty);
            buttonLayoutMenuEntry = new MenuEntry(string.Empty);
            thumbstickLayoutMenuEntry = new MenuEntry(string.Empty);
            backMenuEntry = new MenuEntry(string.Empty);

            frobnicateMenuEntry = new MenuEntry(string.Empty);
            elfMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            // Hook up menu event handlers.
            difficultyMenuEntry.Selected += DifficultyMenuEntrySelected;
            languageMenuEntry.Selected += LanguageMenuEntrySelected;
            buttonLayoutMenuEntry.Selected += ButtonLayoutMenuEntrySelected;
            thumbstickLayoutMenuEntry.Selected += ThumbStickLayoutMenuEntrySelected;

            //frobnicateMenuEntry.Selected += FrobnicateMenuEntrySelected;
            //elfMenuEntry.Selected += ElfMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(difficultyMenuEntry);
            MenuEntries.Add(buttonLayoutMenuEntry);
            MenuEntries.Add(thumbstickLayoutMenuEntry);
            //MenuEntries.Add(frobnicateMenuEntry);
            //MenuEntries.Add(elfMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            GameOptions.SetDifficulty((int)currentDifficulty);

            if (GameOptions.Language == "French")
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
            else if (GameOptions.Language == "English")
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");


            Strings.Culture = CultureInfo.CurrentUICulture;

            languageMenuEntry.Text = "Language: " + languages[currentLanguage];
            difficultyMenuEntry.Text = "Difficulty: " + currentDifficulty;
            buttonLayoutMenuEntry.Text = "Button Layout";
            thumbstickLayoutMenuEntry.Text = "Thumbstick Layout";
            backMenuEntry.Text = Strings.Back;                                 
        }

        /// <summary>
        /// Updates the options menu to the current language.
        /// </summary>
        void UpdateLanguage()
        {
            GameOptions.SetLanguage(languages[currentLanguage]);

            if (GameOptions.Language == "French")
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
            else if (GameOptions.Language == "English")
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Difficulty menu entry is selected.
        /// </summary>
        void DifficultyMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentDifficulty++;

            if (currentDifficulty > Difficulty.Hard)
                currentDifficulty = 0;

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;

            UpdateLanguage();

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the InvertY menu entry is selected.
        /// </summary>
        void ButtonLayoutMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ButtonLayoutScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Thumbstick Layout menu entry is selected.
        /// </summary>
        void ThumbStickLayoutMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ThumbstickLayoutScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void FrobnicateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            frobnicate = !frobnicate;

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void ElfMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            elf++;

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Back menu entry is selected.
        /// </summary>
        void BackMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (comingFrom == "MainMenu")
            {
                BlankTransitionScreen.Load(ScreenManager, true, e.PlayerIndex, new BackgroundScreen(), new MainMenuScreen());
            }
            else if (comingFrom == "PauseMenu")
                OnCancel(e.PlayerIndex);
        }

        /// <summary>
        /// Overrides the OnCancel method so that we don't get stuck
        /// in the BlankTransitionScreen.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            if (comingFrom == "MainMenu")
            {
                BlankTransitionScreen.Load(ScreenManager, false, playerIndex, new BackgroundScreen(), new MainMenuScreen());
            }
            else if (comingFrom == "PauseMenu")
            {
                PauseMenuScreen.SetMenuText();
                base.OnCancel(playerIndex);

            }
        }

        #endregion
    }
}
