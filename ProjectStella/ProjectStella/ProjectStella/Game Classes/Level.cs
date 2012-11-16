#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace ProjectStella
{
    public class Level
    {
        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;

        ScreenManager screenManager;
        GameManager gameManager;

        Texture2D background;

        SpriteFont font;
        #endregion

        #region Initialize

        public Level(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
            this.gameManager = screenManager.gameManager;
            content = new ContentManager(screenManager.Game.Services, "Content");
        }

        public void LoadContent()
        {
            font = content.Load<SpriteFont>("Fonts/gameFont");
        }

        public void UnloadContent()
        {
            
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
           
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(font, GameOptions.Language, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, GameOptions.DifficultyMod.ToString(), new Vector2(0, 35), Color.White);

            spriteBatch.End();
        }

        #endregion
    }
}
