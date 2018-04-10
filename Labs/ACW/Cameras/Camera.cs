using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Labs.ACW.Lighting;
using Labs.Utility;
using Labs.ACW.Assets;
using System.Drawing;

namespace Labs.ACW.Cameras
{
    abstract class Camera 
    {
        /*
         * Position
         * Direction
         * Up
         * Right
         * 
         */
        
        public static Camera s_ActiveCamera { get; protected set; } 
        private Vector3 m_Position;
        private Vector3 m_Direction;
        private Vector3 m_Up;
        private Vector3 m_Right;
        protected Matrix4 m_View;
        protected Matrix4 m_Projection;
        public Camera(Vector3 pPosition, Vector3 pDirection, Vector3 pRight, Matrix4 pProjection)
        {
            m_Position = pPosition;
            m_Direction = pDirection;
            m_Right = pRight;
            m_Up = Vector3.Cross(pDirection, pRight);
            m_Projection = pProjection;
            //right up forward position
           /* m_View = new Matrix4(m_Right[0], m_Up[0], m_Direction[0], m_Position[0], 
                                 m_Right[1], m_Up[1], m_Direction[1], m_Position[1], 
                                 m_Right[2], m_Up[2], m_Direction[2], m_Position[2], 
                                 0,          0,       0,              1);*/
            
        }
        public Camera(Matrix4 pTransform, Matrix4 pProjection)
        {
            m_Projection = pProjection;
            m_View = pTransform;
            m_Position = pTransform.ExtractTranslation();
        }
        public virtual void Resize(Rectangle pScreen)
        {
            GL.Viewport(pScreen);

            for (int x = 0; x < ShaderUtility.ShaderIDs.Count; x++)
            {
                int uProjectionLocation = GL.GetUniformLocation(ShaderUtility.ShaderIDs[x], "uProjection");
                int windowHeight = pScreen.Height;
                int windowWidth = pScreen.Width;
                if (windowHeight > windowWidth)
                {
                    if (windowWidth < 1)
                    {

                        windowWidth = 1;
                    }
                    float ratio = windowHeight / windowWidth;
                    Matrix4 mProjection = Matrix4.CreatePerspectiveFieldOfView(1, (float)pScreen.Width / pScreen.Height, 0.1f, 100);
                    GL.UniformMatrix4(uProjectionLocation, true, ref mProjection);
                    int uProjection = GL.GetUniformLocation(ShaderUtility.ShaderIDs[x], "uProjection");

                    GL.UniformMatrix4(uProjection, true, ref mProjection);
                }
                else
                {
                    if (windowHeight < 1)
                    {
                        windowHeight = 1;
                    }
                    float ratio = windowWidth / windowHeight;
                    Matrix4 mProjection = Matrix4.CreatePerspectiveFieldOfView(1, (float)pScreen.Width / pScreen.Height, 0.1f, 100);
                    GL.UniformMatrix4(uProjectionLocation, true, ref mProjection);
                    int uProjection = GL.GetUniformLocation(ShaderUtility.ShaderIDs[x], "uProjection");

                    GL.UniformMatrix4(uProjection, true, ref mProjection);
                }
            }
        }
        public void SetViewMatrix(Matrix4 pMatrix)
        {
            m_View = pMatrix;
        }
        public void SetViewMatrix(Vector3 pTarget)
        {
            m_View = Matrix4.LookAt(m_Position, pTarget, m_Up);
        }
        public void Activate()
        {
            
            s_ActiveCamera = this;
            for (int x = 0; x < ShaderUtility.ShaderIDs.Count; x++)
            {
                GL.UseProgram(ShaderUtility.ShaderIDs[x]);
                int uView = GL.GetUniformLocation(ShaderUtility.ShaderIDs[x], "uView");
                GL.UniformMatrix4(uView, true, ref m_View);
                int uProjection = GL.GetUniformLocation(ShaderUtility.ShaderIDs[x], "uProjection");
                GL.UniformMatrix4(uProjection, true, ref m_Projection);
            }

        }
        public void Transform(Matrix4 pTransform) {

            MoveCamera(pTransform);
            
        }
        
        protected virtual void MoveCamera(Matrix4 pTransform)
        {
            m_View *= pTransform;
            for (int x = 0; x < Light.GetLights().Count; x++)
            {
                //Light.GetLights()[x].SetPosition(new Vector4(new Vector3(Light.GetLights()[x].GetPosition().Xyz) + pTransform.ExtractTranslation(), 1));
                Light.GetLights()[x].SetPosition(Vector4.Transform(Light.GetLights()[x].GetPosition(), pTransform));
            }
            Activate();
        }
    }
}
