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

        // ContentManager for loading.
        ContentManager content;

        // Controller Images
        Texture2D controllerImage;

        // Button Images
        Texture2D aButtonTexture;
        Texture2D bButtonTexture;
        Texture2D xButtonTexture;
        Texture2D yButtonTexture;

        string message =  GameOptions.CurrentThumbstickLayout;
        int currentThumbstickLayoutNumber;
        string[] thumbStickLayoutNames = { "Default", "Southpaw", "Placeholder" };

        // Are the axes inverted?
        static bool xInverted = false;
        static bool yInverted = false;

        // Input States
        KeyboardState oldKeyboardState;
        GamePadState oldGamePadState;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor.
        /// </summary>
        public ThumbstickLayoutScreen()
        {
            DeterminePosition();

            SetText();

            // Transitions on in half a second.
            TransitionOnTime = TimeSpan.FromSeconds(.25f);

            // Transitions off in a quarter of a second.
            TransitionOffTime = TimeSpan.FromSeconds(.25f);
        }

        /// <summary>
        /// Determines what position in the array that 
        /// </summary>
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

        /// <summary>
        /// Sets the text that is displayed.
        /// </summary>
        void SetText()
        {
            GameOptions.SetThumbstickLayout(thumbStickLayoutNames[currentThumbstickLayoutNumber]);

            message = GameOptions.CurrentThumbstickLayout;
        }

        /// <summary>
        /// Loads all the content needed by this screen.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            // Controller Images
            controllerImage = content.Load<Texture2D>("Images/Controller/controller");

            // Button Images
            aButtonTexture = content.Load<Texture2D>("Images/Buttons/A");
            bButtonTexture = content.Load<Texture2D>("Images/Buttons/B");
            xButtonTexture = content.Load<Texture2D>("Images/Buttons/X");
            yButtonTexture = content.Load<Texture2D>("Images/Buttons/Y");
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

            // Goes forward through the layout list
            if (oldKeyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyDown(Keys.Right))
            {
                currentThumbstickLayoutNumber = (currentThumbstickLayoutNumber + 1) % thumbStickLayoutNames.Length;

                SetText();
            }

            // Goes backward through the layout list
            if (oldKeyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyDown(Keys.Left))
            {
                currentThumbstickLayoutNumber = (currentThumbstickLayoutNumber - 1) % thumbStickLayoutNames.Length;

                SetText();
            }

            // Deals with the inversion of the y axis.
            if (oldKeyboardState.IsKeyUp(Keys.Y) && keyboardState.IsKeyDown(Keys.Y))
                InvertY();

            // Deals with the inversion of the x axis.
            if (oldKeyboardState.IsKeyUp(Keys.X) && keyboardState.IsKeyDown(Keys.X))
                InvertX();

            // Exits the screen
            if (oldKeyboardState.IsKeyUp(Keys.Escape) && keyboardState.IsKeyDown(Keys.Escape))
                ExitScreen();
          
            oldKeyboardState = keyboardState;
            oldGamePadState = gamePadState;
        }

        /// <summary>
        /// Switches the inversion of the Y axis.
        /// </summary>
        void InvertY()
        {
            if (yInverted)
                yInverted = false;
            else
                yInverted = true;

            GameOptions.SetYAxis(yInverted);
        }

        /// <summary>
        /// Switches the inversion of the X axis.
        /// </summary>
        void InvertX()
        {
            if (xInverted)
                xInverted = false;
            else
                xInverted = true;

            GameOptions.SetXAxis(xInverted);
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the picture of the Xbox Controller on the screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Sets the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Origin for the controller image
            Vector2 origin = new Vector2(controllerImage.Width, controllerImage.Height) / 2;

            // Origin for each button image
            Vector2 aOrigin = new Vector2(aButtonTexture.Width, aButtonTexture.Height) / 2;
            Vector2 bOrigin = new Vector2(bButtonTexture.Width, bButtonTexture.Height) / 2;
            Vector2 xOrigin = new Vector2(xButtonTexture.Width, xButtonTexture.Height) / 2;
            Vector2 yOrigin = new Vector2(yButtonTexture.Width, yButtonTexture.Height) / 2;

            // Sets the font to the ScreenManager's font.
            SpriteFont font = ScreenManager.Font;

            Vector2 stringLength;

            stringLength = font.MeasureString(message);

            spriteBatch.Begin();

            // Draw the name of the current thumbstick layout.
            spriteBatch.DrawString(font, message, new Vector2(640f - (stringLength.X / 2), 100), Color.White * TransitionAlpha);

            spriteBatch.DrawString(font, GameOptions.YInverted.ToString(), Vector2.Zero, Color.White * TransitionAlpha);
            spriteBatch.DrawString(font, GameOptions.XInverted.ToString(), Vector2.Zero + new Vector2(0, font.LineSpacing), Color.White * TransitionAlpha);

            // Draw the image of the thumbstick layout.
            spriteBatch.Draw(controllerImage, new Vector2(640f, 360f), null, Color.White * TransitionAlpha, 0f, origin, 0.4f, SpriteEffects.None, 0f);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        #endregion
    }
}
