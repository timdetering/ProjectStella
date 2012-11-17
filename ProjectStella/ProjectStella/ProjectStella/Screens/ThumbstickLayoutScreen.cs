#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace ProjectStella
{
    class ThumbstickLayoutScreen : MenuScreen
    {
        #region Fields

        MenuEntry backMenuEntry;

        #endregion

        #region Initialize

        public ThumbstickLayoutScreen()
            : base("Thumbstick Layout", "Normal")
        {
            backMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            backMenuEntry.Selected += BackMenuEntrySelected;

            MenuEntries.Add(backMenuEntry);
        }

        /// <summary>
        /// Sets the desired message for each individual menu entry.
        /// </summary>
        void SetMenuEntryText()
        {
            backMenuEntry.Text = Strings.Back;
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Back menu entry is selected.
        /// </summary>
        void BackMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }

        #endregion
    }
}
