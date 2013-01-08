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
    /// Base class for any 3d object.
    /// </summary>
    public class Object
    {
        #region Draw

        /// <summary>
        /// Draws 3d Model
        /// </summary>
        virtual public void DrawModel(Model model, Matrix world, Texture2D texture, Camera camera, bool debugOn)
        {
            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = camera.ProjectionMatrix;
                    be.View = camera.ViewMatrix;
                    be.World = modelTransforms[mesh.ParentBone.Index] * world;
                    be.Texture = texture;
                    be.TextureEnabled = true;
                }
                mesh.Draw();
            }
        }

        #endregion
    }
}
