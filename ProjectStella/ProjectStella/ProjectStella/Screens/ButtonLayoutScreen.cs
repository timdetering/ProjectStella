#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace ProjectStella
{
    class ButtonLayoutScreen : GameScreen
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

        // Arrow Textures
        Texture2D arrowTexture;

        // Background texture
        Texture2D backgroundTexture;

        string message = GameOptions.CurrentButtonLayout;
        int currentButtonLayoutNumber;
        string[] buttonLayoutNames = { "Default", "Placeholder1", "Placeholder2" };

        // Are the axes inverted?
        static bool xInverted = false;
        static bool yInverted = false;

        // Input States
        KeyboardState oldKeyboardState;
        GamePadState oldGamePadState;

        #endregion

        #region Initialize

        /// <summary>
        /// The constructor
        /// </summary>
        public ButtonLayoutScreen()
        {
            // Determines what position in the thumbstick layout array we are.
            DeterminePosition();

            // Calls the method to set the text on the screen.
            SetText();

            // Transitions on in half a second.
            TransitionOnTime = TimeSpan.FromSeconds(0f);

            // Transitions off in a quarter of a second.
            TransitionOffTime = TimeSpan.FromSeconds(0f);
        }

        /// <summary>
        /// Determines what position in the array that 
        /// </summary>
        void DeterminePosition()
        {
            switch (GameOptions.CurrentThumbstickLayout)
            {
                case "Default":
                    currentButtonLayoutNumber = 0;
                    break;
                case "Southpaw":
                    currentButtonLayoutNumber = 1;
                    break;
                case "Placeholder":
                    currentButtonLayoutNumber = 2;
                    break;
            }
        }

        /// <summary>
        /// Sets the text that is displayed.
        /// </summary>
        void SetText()
        {
            GameOptions.SetButtonLayout(buttonLayoutNames[currentButtonLayoutNumber]);

            message = GameOptions.CurrentButtonLayout;
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

            // Arrow images
            arrowTexture = content.Load<Texture2D>("arrow");

            // Background Image
            backgroundTexture = content.Load<Texture2D>("backgroundPlaceholder");
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
                currentButtonLayoutNumber = (currentButtonLayoutNumber + 1) % buttonLayoutNames.Length;

                SetText();
            }

            // Goes backward through the layout list
            if (oldKeyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyDown(Keys.Left))
            {
                currentButtonLayoutNumber = (currentButtonLayoutNumber - 1) % buttonLayoutNames.Length;

                if (currentButtonLayoutNumber == -1)
                    currentButtonLayoutNumber = 2;

                SetText();
            }

            // Exits the screen
            if (oldKeyboardState.IsKeyUp(Keys.Escape) && keyboardState.IsKeyDown(Keys.Escape))
                ExitScreen();

            oldKeyboardState = keyboardState;
            oldGamePadState = gamePadState;
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

            #region Origins

            // Origin for the controller image
            Vector2 origin = new Vector2(controllerImage.Width, controllerImage.Height) / 2;

            // Origin for each button image
            Vector2 aOrigin = new Vector2(aButtonTexture.Width, aButtonTexture.Height) / 2;
            Vector2 bOrigin = new Vector2(bButtonTexture.Width, bButtonTexture.Height) / 2;
            Vector2 xOrigin = new Vector2(xButtonTexture.Width, xButtonTexture.Height) / 2;
            Vector2 yOrigin = new Vector2(yButtonTexture.Width, yButtonTexture.Height) / 2;

            // Origin for arrow picture
            Vector2 arrowOrigin = new Vector2(arrowTexture.Width, arrowTexture.Height) / 2;

            // Origin for background image
            Vector2 backgroundOrigin = new Vector2(backgroundTexture.Width, backgroundTexture.Height) / 2;

            #endregion

            // Sets the font to the ScreenManager's font.
            SpriteFont font = ScreenManager.Font;

            Color yColor = yInverted ? Color.Yellow : Color.White;
            Color xColor = xInverted ? Color.Yellow : Color.White;

            // Messages the thumbstick layout names
            Vector2 stringLength = font.MeasureString(message);

            // Measures the Invert X and Y messages.
            Vector2 measureInvertY = font.MeasureString("Invert Y");
            Vector2 measureInvertX = font.MeasureString("Invert X");

            spriteBatch.Begin();

            //Draw Background
            spriteBatch.Draw(backgroundTexture, new Vector2(viewport.Width, viewport.Height) / 2, null, Color.White * TransitionAlpha, 0f, backgroundOrigin, 1f, SpriteEffects.None, 0f);

            // Draw the name of the current thumbstick layout.
            spriteBatch.DrawString(font, message, new Vector2(640f - (stringLength.X / 2), 100), Color.White * TransitionAlpha);

            // Draw the arrow images
            spriteBatch.Draw(arrowTexture, new Vector2(640f + stringLength.X / 2 + 20, 102 + font.LineSpacing / 2), null, Color.White * TransitionAlpha, 0f, arrowOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(arrowTexture, new Vector2(640f - stringLength.X / 2 - 20, 102 + font.LineSpacing / 2), null, Color.White * TransitionAlpha, MathHelper.Pi, arrowOrigin, 1f, SpriteEffects.None, 0f);


            // Draw the image of the thumbstick layout.
            spriteBatch.Draw(controllerImage, new Vector2(640f, 360f), null, Color.White * TransitionAlpha, 0f, origin, 0.4f, SpriteEffects.None, 0f);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
