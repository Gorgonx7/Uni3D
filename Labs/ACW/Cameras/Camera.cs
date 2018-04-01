using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
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
        private Vector3 m_Position;
        private Vector3 m_Direction;
        private Vector3 m_Up;
        private Vector3 m_Right;
        private Matrix4 m_View;
        public Camera(Vector3 pPosition, Vector3 pDirection, Vector3 pRight)
        {
            m_Position = pPosition;
            m_Direction = pDirection;
            m_Right = pRight;
            m_Up = Vector3.Cross(pDirection, pRight);
            //right up forward position
            m_View = new Matrix4(m_Right[0], m_Up[0], m_Direction[0], m_Position[0], 
                                 m_Right[1], m_Up[1], m_Direction[1], m_Position[1], 
                                 m_Right[2], m_Up[2], m_Direction[2], m_Position[2], 
                                 0,          0,       0,              1);
            
        }
        public void SetViewMatrix(Matrix4 pMatrix)
        {
            m_View = pMatrix;
        }
        public void SetViewMatrix(Vector3 pTarget)
        {
            m_View = Matrix4.LookAt(m_Position, pTarget, m_Up);
        }

        public void Transform(Matrix4 pTransform) {
            
            m_View = Matrix4.LookAt(m_Position, Vector3.Transform(m_Direction, pTransform), m_Up);
            m_Direction = Vector3.Transform(m_Direction, pTransform);
            
        }
        public void Bind(int pShaderProgram)
        {
            
            
            int uView = GL.GetUniformLocation(pShaderProgram, "uView");
            GL.UniformMatrix4(uView, true, ref m_View);            
            Vector3 cameraPosition = m_View.ExtractTranslation();
            Vector4 cameraPosition2 = new Vector4(cameraPosition, 1f);
            int uEyePosition = GL.GetUniformLocation(pShaderProgram, "uEyePosition");
            GL.Uniform4(uEyePosition, ref cameraPosition2);
        }
    }
}
