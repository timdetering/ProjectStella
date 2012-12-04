#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace ProjectStella
{
    class ShipSelectionScreen : GameScreen
    {
        #region Fields

        ContentManager content;

        ScreenManager screenManager;    // screen manager
        GameManager gameManager;         // game manager

        const int NumberShips = 2;    // number of available ships to choose from

        // name for each ship
        String[] ships = new String[NumberShips] { "ship2", "ship1" };

        // model for each ship
        Model[] shipModels = new Model[NumberShips];

        Model padModel;           // ship pad model
        Model padHaloModel;       // ship pad halo model
        Model padSelectModel;     // ship pad select model

        Texture2D textureChangeShip;      // change ship texture
        Texture2D textureRotateShip;      // rotate ship texture
        Texture2D textureSelectBack;      // select and back texture
        Texture2D textureSelectCancel;    // select and cancel texture
        Texture2D textureInvertYCheck;    // checked invert y texture
        Texture2D textureInvertYUncheck;  // unchecked invert y texture

        LightList lights;     // lights for scene

        static TextureCube reflectCube;

        // ship selection for each player
        int[] selection = new int[2] { 0, 1 };

        // confirmed status for each player
        bool[] confirmed = new bool[2] { false, false };

        // invert Y flags (bit flag for each player)
        uint invertY = 0;

        // rotation matrix for each player ship model
        Matrix[] rotation = new Matrix[2] { Matrix.Identity, Matrix.Identity };

        // total elapsed time for ship model rotation
        float elapsedTime = 0.0f;

        #endregion

        #region Initialize

        public ShipSelectionScreen(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
            gameManager = screenManager.gameManager;
        }

        public override void LoadContent()
        {
            // If the content manager doesn't exist, then one is created
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            base.LoadContent();

            // load all resources
            confirmed[0] = false;
            confirmed[1] = (gameManager.GameMode == GameMode.SinglePlayer);

            rotation[0] = Matrix.Identity;
            rotation[1] = Matrix.Identity;

            lights = LightList.Load("content/screens/player_lights.xml");

            for (int i = 0; i < NumberShips; i++)
            {
                shipModels[i] = content.Load<Model>(
                                        "ships/" + ships[i]);
                FixupShip(shipModels[i], "ships/" + ships[i]);
            }

            padModel = content.Load<Model>("ships/pad");
            padHaloModel = content.Load<Model>("ships/pad_halo");
            padSelectModel = content.Load<Model>("ships/pad_select");

            textureChangeShip = content.Load<Texture2D>(
                                        "screens/change_ship");
            textureRotateShip = content.Load<Texture2D>(
                                        "screens/rotate_ship");
            textureSelectBack = content.Load<Texture2D>(
                                        "screens/select_back");
            textureSelectCancel = content.Load<Texture2D>(
                                        "screens/select_cancel");
            textureInvertYCheck = content.Load<Texture2D>(
                                        "screens/inverty_check");
            textureInvertYUncheck = content.Load<Texture2D>(
                                        "screens/inverty_uncheck");
        }

        public override void UnloadContent()
        {
            content.Dispose();

            base.UnloadContent();
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            elapsedTime += .005f;
        }

        #endregion

        #region Handle Input

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
        }

        #endregion

        public override void Draw(GameTime gameTime)
        {
            //YOUR MOM US A WHORE JUST LIKE YOU
            base.Draw(gameTime);

            GraphicsDevice gd = ScreenManager.GraphicsDevice;

            // screen aspect
            float aspect = (float)gd.Viewport.Width / (float)gd.Viewport.Height;

            // camera position
            Vector3 cameraPosition = new Vector3(0, 240, -800);

            // view and projection matrices
            Matrix view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            Matrix projection =
                Matrix.CreatePerspectiveFieldOfView(0.25f, aspect, 1, 1000);
            Matrix viewProjection = view * projection;

            // translation matrix
            Matrix transform = Matrix.CreateTranslation(0, -40, 0);

            // draw ship model
            gameManager.DrawModel(gd, shipModels[selection[0]],
                RenderTechnique.NormalMapping,
                cameraPosition, rotation[0], viewProjection, lights);

            // draw pad model
            gameManager.DrawModel(gd, padModel,
                RenderTechnique.NormalMapping,
                cameraPosition, transform, viewProjection, lights);

            // set additive blend
            gd.DepthStencilState = DepthStencilState.DepthRead;
            gd.BlendState = BlendState.Additive;


            // disable glow (zero in alpha)
            //gd.RenderState.SeparateAlphaBlendEnabled = true;
            //gd.RenderState.AlphaBlendOperation = BlendFunction.Add;
            //gd.RenderState.AlphaSourceBlend = Blend.Zero;
            //gd.RenderState.AlphaDestinationBlend = Blend.Zero;

            // draw pad halo model
            gameManager.DrawModel(gd, padHaloModel, RenderTechnique.PlainMapping,
                cameraPosition, transform, viewProjection, null);

            // enable glow (alpha not zero)
            //gd.RenderState.SeparateAlphaBlendEnabled = false;
            gd.BlendState = BlendState.AlphaBlend;

            // if not confirmed, draw animated selection circle
            if (confirmed[0] == false)
            {
                transform = Matrix.CreateRotationY(elapsedTime);
                float scale = 1.0f + 0.03f * (float)Math.Cos(elapsedTime * 7);
                transform = transform * Matrix.CreateScale(scale);
                transform.M42 = -10;
                gameManager.DrawModel(gd, padSelectModel,
                    RenderTechnique.PlainMapping, cameraPosition, transform,
                    viewProjection, null);
            }

            // restore blend modes
            gd.DepthStencilState = DepthStencilState.Default;
            gd.BlendState = BlendState.Opaque;
        }

        /// <summary>
        /// Performs effect initialization, which is required in XNA 4.0
        /// </summary>
        /// <param name="model"></param>
        private void FixupShip(Model model, string path)
        {
            Game game = Game.GetInstance();

            foreach (ModelMesh mesh in model.Meshes)
            {
                // for each mesh part
                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["Reflect"].SetValue(GetReflectCube());
                }
            }
        }

        /// <summary>
        /// Creates a reflection textureCube
        /// </summary>
        static TextureCube GetReflectCube()
        {
            if (reflectCube != null)
                return reflectCube;

            Color[] cc = new Color[]
            {
                new Color(1,0,0), new Color(0.9f,0,0.1f), 
                new Color(0.8f,0,0.2f), new Color(0.7f,0,0.3f),
                new Color(0.6f,0,0.4f), new Color(0.5f,0,0.5f),
                new Color(0.4f,0,0.6f), new Color(0.3f,0,0.7f),
                new Color(0.2f,0,0.8f), new Color(0.1f,0,0.9f),
                new Color(0.1f,0,0.9f), new Color(0.0f,0,1.0f),
            };

            reflectCube = new TextureCube(Game.GetInstance().GraphicsDevice,
                8, true, SurfaceFormat.Color);

            Random rand = new Random();

            for (int s = 0; s < 6; s++)
            {
                Color[] sideData = new Color[reflectCube.Size * reflectCube.Size];
                for (int i = 0; i < sideData.Length; i++)
                {
                    sideData[i] = cc[rand.Next(cc.Length)];
                }
                reflectCube.SetData((CubeMapFace)s, sideData);
            }

            return reflectCube;
        }
    }
}