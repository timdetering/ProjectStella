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
    /// This is our protaganist.
    /// </summary>
    public class PlayerShip : Object
    {
        #region Fields

        private Model model;
        private Texture2D texture;
        private Vector3 position;
        private float yaw, pitch, roll;
        private float speed;
        private Matrix world;
        private Matrix rotation = Matrix.Identity;
        private float velocity;


        public Vector3 Position
        {
            get { return position; }
        }
        public float Yaw
        {
            get { return yaw; }
        }
        public float Pitch
        {
            get { return pitch; }
        }
        public float Roll
        {
            get { return roll; }
        }
        public Matrix World
        {
            get { return world; }
        }
        public Matrix Rotation
        {
            get { return rotation; }
        }


        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public PlayerShip(Model model, Texture2D texture, float radius)
        {
            this.model = model;
            this.texture = texture;

            yaw = 0.0f;
            pitch = 0.0f;
            roll = 0.0f;
            speed = 5f;
        }

        /// <summary>
        /// Resets the player upon collision.
        /// </summary>
        public void Reset()
        {
            position = new Vector3(0, 0, 0);
            yaw = 0.0f;
            pitch = 0.0f;
            roll = 0.0f;
            velocity = 0;
            rotation = Matrix.Identity;
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the player
        /// </summary>
        public void Update()
        {
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);

            velocity = speed;

            position += rotation.Forward * velocity;

            HandleInput(gamePad);
        }

        /// <summary>
        /// Handles any input that might affect the player.
        /// </summary>
        /// <param name="gamePad"></param>
        private void HandleInput(GamePadState gamePad)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Rotate Camera Right and Left
            if (gamePad.ThumbSticks.Right.X > 0.1f || gamePad.ThumbSticks.Right.X < -0.1f)
            {
                yaw += .01f * -gamePad.ThumbSticks.Right.X;
            }

            // Rotate Camera Up and Down
            if (gamePad.ThumbSticks.Right.Y > 0.1f || gamePad.ThumbSticks.Right.Y < -0.1f)
            {
                pitch += .01f * gamePad.ThumbSticks.Right.Y;
            }

            // Roll Camera Left
            if (gamePad.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                roll += -.01f;
            }
            // Roll Camera Right
            if (gamePad.Buttons.RightShoulder == ButtonState.Pressed)
            {
                roll += .01f;
            }
            
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the 3d model
        /// </summary>
        public void Draw(Camera camera, bool debugOn)
        {
            rotation.Forward.Normalize();
            rotation.Up.Normalize();
            rotation.Right.Normalize();

            rotation *= Matrix.CreateFromAxisAngle(rotation.Right, pitch);
            rotation *= Matrix.CreateFromAxisAngle(rotation.Up, yaw);
            rotation *= Matrix.CreateFromAxisAngle(rotation.Forward, roll);

            world = Matrix.CreateScale(0.005f, 0.005f, 0.005f) * Matrix.CreateWorld(position, rotation.Forward, rotation.Up);

            roll = 0.0f;
            yaw = 0.0f;
            pitch = 0.0f;

            DrawModel(model, world, texture, camera, debugOn);
        }

        #endregion
    }
}
