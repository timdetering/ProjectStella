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

        public struct WorldObject
        {
            public Vector3 position;
            public Model model;
            public Texture2D texture2D;
        }

        WorldObject[] worldObjects;

        ContentManager content;

        ScreenManager screenManager;
        GameManager gameManager;

        SpriteFont font;

        Skybox skybox;

        Model sphere;
        Texture2D sphereTexture;

        Camera camera = new Camera();
        Random rand = new Random();

        int numOfWorldObjects;
        int worldX;
        int worldY;
        int worldZ;

        // Creates matrixes for 3d
        Matrix world = Matrix.Identity;
        Matrix view = Matrix.CreateLookAt(new Vector3(20, 0, 0), new Vector3(0, 0, 0), Vector3.UnitY);
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 400f / 300f,0.1f, 100f);

        GamePadState gamePadState;
        GamePadState oldGamePadState;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor.
        /// </summary>
        public Level(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
            this.gameManager = screenManager.gameManager;

            screenManager.graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Initialize();
        }

        public void Initialize()
        {
            numOfWorldObjects = rand.Next(1, 100);
            
            worldObjects = new WorldObject[numOfWorldObjects];

            for (int i = 0; i < worldObjects.Length; i++)
            {
                worldX = rand.Next(-100, 100);
                worldY = rand.Next(-100, 100);
                worldZ = rand.Next(-100, 100);

                worldObjects[i] = new WorldObject();
                worldObjects[i].model = sphere;
                worldObjects[i].position = new Vector3(worldX,worldY, worldZ);
                worldObjects[i].texture2D = sphereTexture;
            }
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

            sphere = content.Load<Model>("sphere");
            sphereTexture = content.Load<Texture2D>("unselectedtexture");
            for (int i = 0; i < worldObjects.Length; i++)
            {
                worldObjects[i].model = sphere;
                worldObjects[i].texture2D = sphereTexture;
            }
        }


        /// <summary>
        /// Disposes of the content when gameplay screen exits.
        /// </summary>
        public void UnloadContent()
        {
            content.Dispose();   
        }

        #endregion

        #region Update

        /// <summary>
        /// Runs the update method.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);

            HandleInput();

            camera.Update();

            oldGamePadState = gamePadState;
        }

        public void HandleInput()
        {

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
             

            //Draws the skybox.
            skybox.Draw(view, projection, camera);

            
            screenManager.graphics.GraphicsDevice.RasterizerState = originalRasterizerState;


            BlendState bs = screenManager.GraphicsDevice.BlendState;
            DepthStencilState ds = screenManager.GraphicsDevice.DepthStencilState;

            screenManager.GraphicsDevice.BlendState = BlendState.Opaque;
            screenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            for (int i = 0; i < worldObjects.Length; i++)
            {
                Matrix world =
                    Matrix.CreateTranslation(worldObjects[i].position);

                DrawModel(worldObjects[i].model, world,
                    worldObjects[i].texture2D);
            }

            screenManager.GraphicsDevice.BlendState = bs;
            screenManager.GraphicsDevice.DepthStencilState = ds;
        }

        void DrawModel(Model model, Matrix world, Texture2D texture)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.PreferPerPixelLighting = true;
                    be.Projection = camera.ProjectionMatrix;
                    be.View = camera.ViewMatrix;
                    be.World = world;
                    be.Texture = texture;
                    be.TextureEnabled = true;
                }
                mesh.Draw();
            }
        }

        #endregion
    }
}
