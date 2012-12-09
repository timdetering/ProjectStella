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

        // Arrow Textures
        Texture2D arrowTexture;

        // Background Texture
        Texture2D background;

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
            // Determines what position in the thumbstick layout array we are.
            DeterminePosition();

            // Calls the method to set the text on the screen.
            SetText();

            // Transitions on in zero seconds.
            TransitionOnTime = TimeSpan.FromSeconds(.15f);

            // Transitions off in zero seconds
            TransitionOffTime = TimeSpan.FromSeconds(.15f);
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

            // Arrow images
            arrowTexture = content.Load<Texture2D>("arrow");

            // Background Image
            background = content.Load<Texture2D>("backgroundPlaceholder");
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

                if (currentThumbstickLayoutNumber == -1)
                    currentThumbstickLayoutNumber = 2;

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

            // Origin for background
            Vector2 backgroundOrigin = new Vector2(background.Width, background.Height) / 2;

            #endregion

            // Sets the font to the ScreenManager's font.
            SpriteFont font = ScreenManager.Font;

            Color yColor = yInverted ? Color.Black : Color.White;
            Color xColor = xInverted ? Color.Black : Color.White;

            // Messages the thumbstick layout names
            Vector2 stringLength = font.MeasureString(message);

            // Measures the Invert X and Y messages.
            Vector2 measureInvertY = font.MeasureString("Invert Y");
            Vector2 measureInvertX = font.MeasureString("Invert X");

            spriteBatch.Begin();

            //Draw Background
            spriteBatch.Draw(background, new Vector2(viewport.Width, viewport.Height) / 2, null, Color.White * TransitionAlpha, 0f, backgroundOrigin, 1f, SpriteEffects.None, 0f);

            // Draw the name of the current thumbstick layout.
            spriteBatch.DrawString(font, message, new Vector2(640f - (stringLength.X / 2), 100), Color.White * TransitionAlpha);

            // Draw the arrow images
            spriteBatch.Draw(arrowTexture, new Vector2(640f + stringLength.X / 2 + 20, 102 + font.LineSpacing / 2), null, Color.White * TransitionAlpha, 0f, arrowOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(arrowTexture, new Vector2(640f - stringLength.X / 2 - 20, 102 + font.LineSpacing / 2), null, Color.White * TransitionAlpha, MathHelper.Pi, arrowOrigin, 1f, SpriteEffects.None, 0f);

            // Draw the image of the thumbstick layout.
            spriteBatch.Draw(controllerImage, new Vector2(640f, 360f), null, Color.White * TransitionAlpha, 0f, origin, 0.4f, SpriteEffects.None, 0f);

            // Draw the invert Y text and image.
            spriteBatch.Draw(yButtonTexture, new Vector2(620f - yButtonTexture.Width - measureInvertY.X / 2, 620f - yButtonTexture.Height / 2), null, Color.White * TransitionAlpha, 0f, yOrigin, .5f,SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "Invert Y", new Vector2(620f - measureInvertY.X / 2, 617f - font.LineSpacing), yColor * TransitionAlpha, 0f, measureInvertY / 2, 1f, SpriteEffects.None, 0f);

            // Draw the invert X text and image.
            spriteBatch.Draw(xButtonTexture, new Vector2(660f + xButtonTexture.Width / 2, 620f - xButtonTexture.Height / 2), null, Color.White * TransitionAlpha, 0f, xOrigin, .5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "Invert X", new Vector2(660f + xButtonTexture.Width + measureInvertX.X / 2 - 15, 617f - font.LineSpacing), xColor * TransitionAlpha, 0f, measureInvertX / 2, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        #endregion
    }
}
