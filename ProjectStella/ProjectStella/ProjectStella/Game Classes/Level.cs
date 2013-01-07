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

        bool debugOn = false;

        int frameRate;
        int frameCounter;
        TimeSpan elapsedTime;

        List<WorldObject> worldObjects;

        ContentManager content;
        ScreenManager screenManager;
        SpriteFont font;
        Skybox skybox;

        Texture2D blankTexture;

        TimeSpan fireRate = TimeSpan.FromSeconds(.25f);
        TimeSpan previousShot = TimeSpan.Zero;

        Effect effect;

        List<Bullet> bullets;

        Texture2D crossHair;

        Model sphere;
        Texture2D sphereTexture;

        Model ship;
        Texture2D shipTexture;
        PlayerShip player;

        int messageDisplayTime = 0;

        Texture2D bulletSprite;

        Camera camera = new Camera();
        Random rand = new Random();

        int immuneCounter = 0;

        int numOfWorldObjects;

        // Creates matrixes for 3d
        Matrix world = Matrix.Identity;

        GamePadState gamePadState;
        GamePadState oldGamePadState;

        Vector3 bowOffset = new Vector3(0, 0, 5);

        BoundingSphere sphere1;

        SoundEffect primarySound;
        SoundEffectInstance primaryInstance;

        bool thirdPersonOn = false;
        bool overHeated = false;
        bool bulletsHitting = false;

        float heat = 0;
        int k = 1;

        Color heatColor;

        SoundEffect explosion;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor.
        /// </summary>
        public Level(ScreenManager screenManager)
        {
            this.screenManager = screenManager;

            screenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Initialize();
        }

        public void Initialize()
        {

        }

        /// <summary>
        /// Loads the required content for the game.
        /// </summary>
        public void LoadContent()
        {
            if (content == null)
                content = new ContentManager(screenManager.Game.Services, "Content");

            skybox = new Skybox(content);

            font = content.Load<SpriteFont>("Fonts/gameFont");

            bulletSprite = content.Load<Texture2D>("bulletSprite");

            crossHair = content.Load<Texture2D>("crosshair");

            // Texture and model for ship.
            ship = content.Load<Model>("spaceship");
            shipTexture = content.Load<Texture2D>("interceptormap2");

            primarySound = content.Load<SoundEffect>("lasershot");

            blankTexture = content.Load<Texture2D>("whiteText");

            effect = content.Load<Effect>("effects");

            // Texture and model for asteroids.
            sphere = content.Load<Model>("sphere");
            sphereTexture = content.Load<Texture2D>("AM1");

            explosion = content.Load<SoundEffect>("Explo4");

            player = new PlayerShip(ship, shipTexture, 60);

            numOfWorldObjects = rand.Next(500, 500);
            worldObjects = new List<WorldObject>();

            for (int i = 0; i < numOfWorldObjects; i++)
            {
                int worldX = rand.Next(-500, 500);
                int worldY = rand.Next(-500, 500);
                int worldZ = rand.Next(-500, 500);
                int rotX = rand.Next(1, 50);
                int rotY = rand.Next(1, 50);
                int rotZ = rand.Next(1, 50);
                int moving = rand.Next(2,2 );
                float scale;
                Vector3 direction;


                if (moving == 2)
                    direction = new Vector3(rand.Next(-1, 1), rand.Next(-1, 1), rand.Next(-1, 1));
                else
                    direction = new Vector3(0, 0, 0);

                float scaleChance = rand.Next(1, 100);

                if (scaleChance > 90)
                    scale = rand.Next(75, 100);
                else if (scaleChance > 75)
                    scale = rand.Next(35, 75);
                else if (scaleChance > 50)
                    scale = rand.Next(10, 35);
                else
                    scale = rand.Next(1, 10);

                Vector3 position = new Vector3(worldX, worldY, worldZ);

                worldObjects.Add(new WorldObject(sphere, sphereTexture, position, scale, rotX, rotY, rotZ, direction));
            }

            bullets = new List<Bullet>();

            primaryInstance = primarySound.CreateInstance();
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

            HandleInput(gameTime);

            player.Update();
            camera.Update(player, thirdPersonOn);

            immuneCounter--;

            for(int i = 0; i < bullets.Count; i++)
                bullets[i].Update();
                

            for (int i = 0; i < worldObjects.Count; i++)
            {
                worldObjects[i].Update(gameTime);

                if (ShipCollision(ship, sphere, player.World, worldObjects[i].World))
                {
                    if (immuneCounter < 0)
                    {
                        player.Reset();
                        immuneCounter = 300;
                        messageDisplayTime = 300;
                        explosion.Play();
                    }
                }
                if (k < worldObjects.Count)
                {

                    if (ShipCollision(sphere, sphere, worldObjects[k].World, worldObjects[i].World))
                    {
                        frameRate = 1000;
                        worldObjects[i].ReverseDirection();
                        worldObjects[k].ReverseDirection();
                    }
                    k++;
                }
                else
                    k = 0;


                for (int j = 0; j < bullets.Count; j++)
                {
                    BoundingSphere bulletSphere = new BoundingSphere(bullets[j].Position, 5f);

                    for (int meshIndex1 = 0; meshIndex1 < sphere.Meshes.Count; meshIndex1++)
                    {
                        sphere1 = sphere.Meshes[meshIndex1].BoundingSphere;
                        sphere1 = sphere1.Transform(worldObjects[i].World);

                        if (BulletCollision(bulletSphere, sphere1))
                        {
                            bullets[j].IsAlive = false;
                            worldObjects[i].Health -= bullets[j].Damage;
                            bulletsHitting = true;
                        }
                        else
                            bulletsHitting = false;
                    }

                    if (bullets[j].IsAlive != true)
                    {
                        bullets.RemoveAt(j);
                    }
                }

                if (worldObjects[i].IsAlive != true)
                    worldObjects.RemoveAt(i);
            }

            HandleHeat();

            oldGamePadState = gamePadState;

            // Measure our framerate.
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            } 

        }

        private void HandleHeat()
        {
            if (heat >= 100 && overHeated == false)
                overHeated = true;

            if (overHeated == true)
                heat -= 0.2f;
            else
                heat -= 0.05f;

            if (overHeated == true)
                heatColor = Color.Red;
            else if (heat >= 75)
                heatColor = Color.OrangeRed;
            else if (heat >= 50)
                heatColor = Color.Orange;
            else if (heat >= 25)
                heatColor = Color.Yellow;
            else
                heatColor = Color.Green;

            if (heat <= 0)
            {
                overHeated = false;
                heat = 0;
            }
        }

        private bool ShipCollision(Model model1, Model model2, Matrix world1, Matrix world2)
        {
            for (int meshIndex1 = 0; meshIndex1 < model1.Meshes.Count; meshIndex1++)
            {
                BoundingSphere sphere1 = model1.Meshes[meshIndex1].BoundingSphere;
                sphere1 = sphere1.Transform(world1);

                for (int meshIndex2 = 0; meshIndex2 < model2.Meshes.Count; meshIndex2++)
                {
                    BoundingSphere sphere2 = model2.Meshes[meshIndex2].BoundingSphere;
                    sphere2 = sphere2.Transform(world2);

                    if (sphere1.Intersects(sphere2))
                        return true;
                }
            }
            return false;
        }

        private bool BulletCollision(BoundingSphere sphere1, BoundingSphere sphere2)
        {

            if (sphere1.Intersects(sphere2))
                return true;
            return false;
        }

        public void HandleInput(GameTime gameTime)
        {
            if (gamePadState.Buttons.LeftStick == ButtonState.Pressed && debugOn == false)
                debugOn = true;
            else if (gamePadState.Buttons.RightStick == ButtonState.Pressed && debugOn == true)
                debugOn = false;

            if (gamePadState.Triggers.Right > 0.1f && gameTime.TotalGameTime - previousShot >= fireRate && overHeated == false)
            {
                bullets.Add(new Bullet(player.Position + player.World.Right * 5, player.Rotation, bulletSprite, camera, effect, screenManager));
                bullets.Add(new Bullet(player.Position + player.World.Left * 5, player.Rotation, bulletSprite, camera, effect, screenManager));
                previousShot = gameTime.TotalGameTime;
                primarySound.Play(.15f, 0, 0);

                heat += 2;
            }

            if (gamePadState.DPad.Right == ButtonState.Pressed && oldGamePadState.DPad.Right == ButtonState.Pressed && thirdPersonOn == false)
                thirdPersonOn = true;

            if (gamePadState.DPad.Left == ButtonState.Pressed && oldGamePadState.DPad.Left == ButtonState.Pressed && thirdPersonOn == true)
                thirdPersonOn = false;
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            screenManager.GraphicsDevice.Clear(Color.Black);

            RasterizerState originalRasterizerState = screenManager.graphics.GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;

            screenManager.graphics.GraphicsDevice.RasterizerState = rasterizerState;


            //Draws the skybox.
            skybox.Draw(camera);

            screenManager.graphics.GraphicsDevice.RasterizerState = originalRasterizerState;

            BlendState bs = screenManager.GraphicsDevice.BlendState;
            DepthStencilState ds = screenManager.GraphicsDevice.DepthStencilState;

            RasterizerState originalState = screenManager.GraphicsDevice.RasterizerState;

            if (debugOn)
            {
                RasterizerState rs = new RasterizerState();
                rs.FillMode = FillMode.WireFrame;
                screenManager.GraphicsDevice.RasterizerState = rs;
            }


            screenManager.GraphicsDevice.BlendState = BlendState.Opaque;
            screenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw();
            }

            for (int j = 0; j < worldObjects.Count; j++)
            {
                worldObjects[j].Draw(camera, debugOn);
            }

            player.Draw(camera, debugOn);


            screenManager.GraphicsDevice.BlendState = bs;
            screenManager.GraphicsDevice.DepthStencilState = ds;



            if (debugOn)
                screenManager.graphics.GraphicsDevice.RasterizerState = originalState;

            spriteBatch.Begin();

            if(debugOn == false && thirdPersonOn == false)
                spriteBatch.Draw(crossHair, new Vector2(screenManager.GraphicsDevice.Viewport.Width, screenManager.GraphicsDevice.Viewport.Height) / 2, null, Color.White, 0, new Vector2(crossHair.Width, crossHair.Height) / 2, 0.5f,SpriteEffects.None, 0f);

            spriteBatch.Draw(blankTexture, new Rectangle((1280 / 2 - 50), 360 + crossHair.Height / 3, (int)heat , 20), heatColor);

            Vector2 messageLength = font.MeasureString("Watch out for asteroids");

            if (messageDisplayTime > 0)
            {
                spriteBatch.DrawString(font, "Watch out for asteroids", new Vector2(screenManager.GraphicsDevice.Viewport.Width - messageLength.X, screenManager.GraphicsDevice.Viewport.Height - messageLength.Y) / 2, Color.White);
                messageDisplayTime--;
            }

            if (debugOn)
                spriteBatch.DrawString(screenManager.Font,
                    "Debug Menu: \n"
                    + "FPS: " + frameRate.ToString() + "\n"
                    + "Asteroids: " + worldObjects.Count.ToString() + "\n"
                    + "Bullets Active: " + bullets.Count.ToString() + "\n"
                    + "Third Person On: " + thirdPersonOn.ToString() + "\n"
                    + "Heat: " + heat.ToString() + "\n"
                    + "Overheated: " + overHeated.ToString() + "\n"
                    + "Bullets Colliding: " + bulletsHitting.ToString(), Vector2.Zero, Color.White);


            spriteBatch.End();

            frameCounter++;
        }

        #endregion
    }
}
