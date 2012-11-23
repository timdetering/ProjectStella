#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace ProjectStella
{
    class ButtonLayoutScreen : MenuScreen
    {
        #region Fields

        MenuEntry backMenuEntry;

        #endregion

        public ButtonLayoutScreen() : base("", "Normal")
        {
            backMenuEntry = new MenuEntry("Back");

            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(backMenuEntry);
        }

        #region Handle Input

        void backMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            
        }

        #endregion
    }
}
