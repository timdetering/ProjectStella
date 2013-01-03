using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectStella
{
    public class Camera
    {
        #region Fields

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
            set { viewMatrix = value; }
        }

        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
            set { projectionMatrix = value; }
        }

        private Vector3 position;
        private Vector3 target;
        public Matrix viewMatrix;
         public Matrix projectionMatrix= Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), 400f / 300f,0.1f,500f);

        private float yaw, pitch, roll;
        private float speed;
        private Matrix cameraRotation;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Camera()
        {
            ResetCamera();
        }

        /// <summary>
        /// Resets the camera to its default values.
        /// </summary>
        public void ResetCamera()
        {
            yaw = 0.0f;
            pitch = 0.0f;
            roll = 0.0f;

            speed = .5f;

            cameraRotation = Matrix.Identity;
        }

        /// <summary>
        /// Updates the camera
        /// </summary>
        public void Update()
        {
            HandleInput();
            UpdateViewMatrix();
        }

        private void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);

            // Rotate Camera Right and Left
            if (gamePad.ThumbSticks.Right.X > 0.1f || gamePad.ThumbSticks.Right.X < -0.1f)
            {
                yaw += .02f * -gamePad.ThumbSticks.Right.X;
            }

            // Rotate Camera Up and Down
            if (gamePad.ThumbSticks.Right.Y > 0.1f || gamePad.ThumbSticks.Right.Y < -0.1f)
            {
                pitch += .02f * gamePad.ThumbSticks.Right.Y;
            }

            // Roll Camera Left
            if (gamePad.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                roll += -.02f;
            }
            // Roll Camera Right
            if (gamePad.Buttons.RightShoulder == ButtonState.Pressed)
            {
                roll += .02f;
            }
           

            if (gamePad.ThumbSticks.Left.Y > 0.1f)
            {
                MoveCamera(cameraRotation.Forward);
            }
            if (gamePad.ThumbSticks.Left.Y  < -0.1f)
            {
                MoveCamera(-cameraRotation.Forward);
            }
            if (gamePad.ThumbSticks.Left.X > 0.1f)
            {
                MoveCamera(cameraRotation.Right);
            }
            if (gamePad.ThumbSticks.Left.X < -0.1f)
            {
                MoveCamera(-cameraRotation.Right);
            }
            if (keyboardState.IsKeyDown(Keys.E))
            {
                MoveCamera(cameraRotation.Up);
            }
            if (keyboardState.IsKeyDown(Keys.Q))
            {
                MoveCamera(-cameraRotation.Up);
            }
        }

        private void MoveCamera(Vector3 addedVector)
        {
            position += speed * addedVector;
        }

        /// <summary>
        /// Updates the camera's view matrix.
        /// </summary>
        private void UpdateViewMatrix()
        {
            cameraRotation.Forward.Normalize();
            cameraRotation.Up.Normalize();
            cameraRotation.Right.Normalize();

            cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Right, pitch);
            cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Up, yaw);
            cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll);

            yaw = 0.0f;
            pitch = 0.0f;
            roll = 0.0f;

            target = position + cameraRotation.Forward;

            viewMatrix = Matrix.CreateLookAt(position, target, cameraRotation.Up);
        }
    }
}
