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
    class Bullet
    {
        private Vector3 position;
        private float yaw, pitch,roll;
        private float speed;
        private Texture2D bulletTexture;
        private Vector3 cameraPosition;
        private Vector3 cameraUpDirection;
        private Effect effect;
        private Camera camera;
        private ScreenManager screenManager;
        private Matrix rotation;
        private int timeAlive;
        private bool isAlive = true;
        private int damage = 30;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float Yaw
        {
            get { return yaw; }
            set { yaw = value; }
        }
        public float Pitch
        {
            get { return pitch; }
            set { pitch = value; }
        }
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }
        public int Damage
        {
            get { return damage; }
        }

        public Bullet(Vector3 position, Matrix rotation, Texture2D bulletTexture, Camera camera, Effect effect, ScreenManager screenManager)
        {
            this.position = position;
            this.bulletTexture = bulletTexture;
            this.effect = effect;
            this.camera = camera;
            this.screenManager = screenManager;
            this.rotation = rotation;

            cameraPosition = camera.Position;
            cameraUpDirection = camera.CameraRotation.Up;

            speed = 50f;
            roll = 0f;
        }

        public void Update()
        {
            Vector3 addVector = Vector3.Transform(new Vector3(0, 0, -1), rotation);
            position += addVector * speed;

            timeAlive++;

            if (timeAlive >= 300)
                isAlive = false;
        }

        public void Draw()
        {
            DrawBullet();
        }

        private void DrawBullet()
        {
            VertexPositionTexture[] bulletVertice = new VertexPositionTexture[6];
            Vector3 center = position;

            bulletVertice[0] = new VertexPositionTexture(center, new Vector2(1, 1));
            bulletVertice[1] = new VertexPositionTexture(center, new Vector2(0, 0));
            bulletVertice[2] = new VertexPositionTexture(center, new Vector2(1, 0));

            bulletVertice[3] = new VertexPositionTexture(center, new Vector2(1, 1));
            bulletVertice[4] = new VertexPositionTexture(center, new Vector2(0, 1));
            bulletVertice[5] = new VertexPositionTexture(center, new Vector2(0, 0));

            effect.CurrentTechnique = effect.Techniques["PointSprites"];
            effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            effect.Parameters["xProjection"].SetValue(camera.ProjectionMatrix);
            effect.Parameters["xView"].SetValue(camera.ViewMatrix);
            effect.Parameters["xCamPos"].SetValue(cameraPosition);
            effect.Parameters["xTexture"].SetValue(bulletTexture);
            effect.Parameters["xCamUp"].SetValue(cameraUpDirection);
            effect.Parameters["xPointSpriteSize"].SetValue(1f);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                screenManager.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bulletVertice, 0, 2);
            }
        }
    }
}
