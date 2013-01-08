#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace ProjectStella
{
    /// <summary>
    /// Our Camera class.
    /// </summary>
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
        public Matrix CameraRotation
        {
            get { return cameraRotation; }
        }
        public float Yaw
        {
            get { return yaw; }
        }
        public float Pitch
        {
            get { return pitch; }
        }

        private Vector3 position;
        private Vector3 target = new Vector3();
        public Matrix viewMatrix;

        private float yaw, pitch, roll;
        private float speed;
        private Matrix cameraRotation;
        private Vector3 offset = new Vector3(0,0,35);
        private Vector3 cameraReference = new Vector3(0, 0, 0);
        public Matrix projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 400f / 300f, 0.1f, 5000000f);

        #endregion

        #region Initialize

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

        #endregion

        #region Update

        /// <summary>
        /// Updates the camera
        /// </summary>
        public void Update(PlayerShip player, bool thirdPerson)
        {

            switch (thirdPerson)
            {
                case true:
                    position = player.Position + player.Rotation.Backward * 1f + player.Rotation.Up * .1f;

                    break;

                case false:
                    position = player.Position + player.Rotation.Forward * .075f;

                    break;
            }
            cameraRotation = player.Rotation;

            roll = player.Roll;
            yaw = player.Yaw;
            pitch = player.Pitch;

            UpdateViewMatrix();
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 400f / 300f, 0.1f, 50000f);
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

        #endregion
    }
}
