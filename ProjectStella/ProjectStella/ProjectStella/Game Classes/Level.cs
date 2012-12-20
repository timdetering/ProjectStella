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

        ScreenManager screenManager;
        GameManager gameManager;

        SpriteFont font;

        Skybox skybox;

        // Creates matrixes for 3d
        Matrix world = Matrix.Identity;
        Matrix view = Matrix.CreateLookAt(new Vector3(20, 0, 0), new Vector3(0, 0, 0), Vector3.UnitY);
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 400f / 300f,0.1f, 100f);

        Vector3 cameraPosition;
        float angle = 0;
        float distance = 30;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor.
        /// </summary>
        public Level(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
            this.gameManager = screenManager.gameManager;
        }

        /// <summary>
        /// Loads the required content for the game.
        /// </summary>
        public void LoadContent()
        {
            if(content == null)
                content = new ContentManager(screenManager.Game.Services, "Content");

            skybox = new Skybox(content);

            font = content.Load<SpriteFont>("Fonts/gameFont");
        }


        /// <summary>
        /// Disposes of the content when gameplay screen exits.
        /// </summary>
        public void UnloadContent()
        {
            //content.Dispose();   
        }

        #endregion

        #region Update

        /// <summary>
        /// Runs the update method.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            angle += 0.002f;
            cameraPosition = distance * new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle));
            view = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 0, 0), Vector3.UnitY);
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            screenManager.GraphicsDevice.Clear(Color.Black);

            RasterizerState originalRasterizerState = screenManager.graphics.GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            screenManager.graphics.GraphicsDevice.RasterizerState = rasterizerState;

            skybox.Draw(view, projection, cameraPosition);

            screenManager.graphics.GraphicsDevice.RasterizerState = originalRasterizerState;
        }

        #endregion
    }
}
