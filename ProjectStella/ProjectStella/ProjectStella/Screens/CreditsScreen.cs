#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace ProjectStella
{
    class CreditsScreen : MenuScreen
    {
        #region Fields

        MenuEntry backMenuEntry;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public CreditsScreen()
            : base("Credits", "Normal")
        {
            backMenuEntry = new MenuEntry(string.Empty);

            SetMenuText();

            backMenuEntry.Selected += BackMenuEntrySelected;

            MenuEntries.Add(backMenuEntry);
        }

        void SetMenuText()
        {
            backMenuEntry.Text = "Back";
        }

        #endregion

        #region HandleInput

        void BackMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            BlankTransitionScreen.Load(ScreenManager, false, e.PlayerIndex, new BackgroundScreen(), new MainMenuScreen());
        }

        #endregion
    }
}
