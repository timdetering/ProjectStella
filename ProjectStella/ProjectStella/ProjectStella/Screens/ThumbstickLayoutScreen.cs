#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
#endregion

namespace ProjectStella
{
    class ThumbstickLayoutScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        Texture2D controllerImage;

        //MenuEntry backMenuEntry;

        string message =  GameOptions.CurrentThumbstickLayout;
        int currentThumbstickLayoutNumber;
        string[] thumbStickLayoutNames = { "Default", "Southpaw", "Placeholder" };

        KeyboardState oldKeyboardState;
        GamePadState oldGamePadState;

        #endregion

        #region Initialize

        public ThumbstickLayoutScreen()
        {
            DeterminePosition();

            SetText();

            TransitionOnTime = TimeSpan.FromSeconds(.5f);
            TransitionOffTime = TimeSpan.FromSeconds(.25f);

            //MenuEntries.Add(backMenuEntry);

            //TransitionOnTime = TimeSpan.FromSeconds(.5);
            //TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        void DeterminePosition()
        {
            switch (GameOptions.CurrentThumbstickLayout)
            {
                case "Default" :
                    currentThumbstickLayoutNumber = 0;
                    break;
                case "Southpaw" :
                    currentThumbstickLayoutNumber = 1;
                    break;
                case "Placeholder" :
                    currentThumbstickLayoutNumber = 2;
                    break;
            }
        }

        void SetText()
        {
            GameOptions.SetThumbstickLayout(thumbStickLayoutNames[currentThumbstickLayoutNumber]);

            message = GameOptions.CurrentThumbstickLayout;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            controllerImage = content.Load<Texture2D>("controller");
        }

        /// <summary>
        /// Sets the desired message for each individual menu entry.
        /// </summary>
        void SetMenuEntryText()
        {
            //backMenuEntry.Text = Strings.Back;
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Back menu entry is selected.
        /// </summary>
        void BackMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //OnCancel(e.PlayerIndex);
        }


        /// <summary>
        /// Handles all input that is accepted on this screen.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            // If the input is null, throw an exception
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            if (oldKeyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyDown(Keys.Right))
            {
                currentThumbstickLayoutNumber = (currentThumbstickLayoutNumber + 1) % thumbStickLayoutNames.Length;

                SetText();
            }

            if (oldKeyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyDown(Keys.Left))
            {
                currentThumbstickLayoutNumber = (currentThumbstickLayoutNumber + 1) % thumbStickLayoutNames.Length;

                SetText();
            }

            if (oldKeyboardState.IsKeyUp(Keys.Escape) && keyboardState.IsKeyDown(Keys.Escape))
                ExitScreen();
          
            oldKeyboardState = keyboardState;
            oldGamePadState = gamePadState;
        }

        #endregion


        /// <summary>
        /// Draws the picture of the Xbox Controller on the screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Vector2 origin = new Vector2(controllerImage.Width, controllerImage.Height) / 2;
            SpriteFont font = ScreenManager.Font;

            Vector2 stringLength;

            stringLength = font.MeasureString(message);

            spriteBatch.Begin();

            // Draw the name of the current thumbstick layout.
            spriteBatch.DrawString(font, message, new Vector2(640f - (stringLength.X / 2), 100), Color.White * TransitionAlpha);

            // Draw the image of the thumbstick layout.
            spriteBatch.Draw(controllerImage, new Vector2(640f, 360f), null, Color.White * TransitionAlpha, 0f, origin, 0.4f, SpriteEffects.None, 0f);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
