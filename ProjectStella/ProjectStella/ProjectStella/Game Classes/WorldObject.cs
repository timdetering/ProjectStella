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
    /// <summary>
    /// Class for our world objects.
    /// </summary>
    class WorldObject : Object
    {
        #region Fields

        private Random rand = new Random();

        private Vector3 position;
        private Model model;
        private Texture2D texture;
        private Vector3 direction;
        bool isAlive = true;
        private int health = 100;
        private Matrix world;

        public Vector3 Position
        {
            get { return position; }
        }
        public Model Model
        {
            get { return model; }
        }
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }
        public Matrix World
        {
            get { return world; }
        }
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        float scale;
        float rotX;
        float rotY;
        float rotZ;

        float spinX;
        float spinY;
        float spinZ;

        Matrix rotation;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WorldObject(Model model, Texture2D texture, Vector3 position, float scale, float rotX, float rotY, float rotZ, Vector3 direction)
        {
            this.model = model;
            this.texture = texture;
            this.position = position;
            this.scale = scale;
            this.rotX = rotX;
            this.rotY = rotY;
            this.rotZ = rotZ;
            this.direction = direction;

            spinX = rand.Next(-1, 1);
            spinY = rand.Next(-1, 1);
            spinZ = rand.Next(-1, 1);


            rotation = Matrix.Identity;
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the WorldObject class.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (health <= 0)
                isAlive = false;

            position += direction * new Vector3(0.1f, 0.1f, 0.1f);

            Spin();
        }

        /// <summary>
        /// Makes our World object spin.
        /// </summary>
        private void Spin()
        {
            rotX += spinX * 0.002f;
            rotY += spinY * 0.002f;
            rotZ += spinZ * 0.002f;

            rotation.Forward.Normalize();
            rotation.Up.Normalize();
            rotation.Right.Normalize();

            rotation *= Matrix.CreateFromAxisAngle(rotation.Right, rotX);
            rotation *= Matrix.CreateFromAxisAngle(rotation.Up, rotY);
            rotation *= Matrix.CreateFromAxisAngle(rotation.Forward, rotZ);

            rotX = 0.0f;
            rotY = 0.0f;
            rotZ = 0.0f;
        }

        /// <summary>
        /// Reverses the worldObject's direction if there is a collision.
        /// </summary>
        public void ReverseDirection()
        {
            direction = direction * new Vector3(-1, -1, -1);
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the WorldObject.
        /// </summary>
        public void Draw(Camera camera, bool debugOn)
        {
            world = Matrix.CreateWorld(position, rotation.Forward, rotation.Up) * Matrix.CreateScale(scale * 0.05f);

            DrawModel(model, world, texture, camera, debugOn);
        }

        #endregion
    }
}
