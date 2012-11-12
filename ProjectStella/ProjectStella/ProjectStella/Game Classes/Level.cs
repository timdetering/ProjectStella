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
            gameManager.LoadFiles(content);    
        }

        public void UnloadContent()
        {
            gameManager.UnloadFiles();
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            gameManager.Update(elapsedTime);
           
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw the 3d game scene
            gameManager.Draw3D(screenManager.GraphicsDevice);

            // draw 2D game gui
            gameManager.Draw2D(screenManager.fontManager);
        }

        #endregion
    }
}
