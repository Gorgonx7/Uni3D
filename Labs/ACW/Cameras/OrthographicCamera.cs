using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Labs.Utility;
namespace Labs.ACW.Cameras
{
    /// <summary>
    /// creates a orthographic camera at the given position and creates a projection matrix
    /// </summary>
    class OrthographicCamera : Camera
    {
        public OrthographicCamera(Matrix4 pTransform, Rectangle pScreen) : base(pTransform, Matrix4.CreateOrthographic(-50 * (float)pScreen.Width/(float)pScreen.Height , 50 * pScreen.Width/pScreen.Height , 0.1f, 100))
        {

        }
        /// <summary>
        /// stops the camera from moving
        /// </summary>
        /// <param name="pTransform"></param>
        protected override void MoveCamera(Matrix4 pTransform)
        {
           
        }
        /// <summary>
        /// makes the camera unrotateable
        /// </summary>
        /// <param name="pRotation"></param>
        public override void Transform(float pRotation)
        {
           
        }
        /// <summary>
        /// makes it so that the camera adapts to the screen being resized
        /// </summary>
        /// <param name="pScreen"></param>
        public override void Resize(Rectangle pScreen)
        {
            float windowWidth = pScreen.Width;
                float windowHeight = pScreen.Height;
                float aspect = windowWidth / windowHeight;

                GL.Viewport(pScreen);
                if (windowHeight > windowWidth)
                {
                    if (windowWidth < 1) { windowWidth = 1; }
                    float ratio = windowHeight / windowWidth;
                    m_Projection = Matrix4.CreateOrthographic(ratio * 10, 10, -1, 1); 
                }
                else
                {
                    if (windowHeight < 1) { windowHeight = 1; }
                    float ratio = windowWidth / windowHeight;
                Matrix4 projection = Matrix4.CreateOrthographic(10, ratio * 10, -1, 1);
                
                }
            for (int x = 0; x < ShaderUtility.ShaderIDs.Count; x++)
            {
                int uProjectionLocation = GL.GetUniformLocation(ShaderUtility.ShaderIDs[x], "uProjection");
                GL.UniformMatrix4(uProjectionLocation, true, ref m_Projection);
            }
        }
    }
}
