using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labs.ACW.SceneGraph;
using Labs.Utility;
using OpenTK;
using Labs.ACW.Assets;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
namespace Labs.ACW
{
    struct Drawable
    {
        GeometryNode m_Drawable;
        Matrix4 m_Transform;
        public Drawable(GeometryNode pDrawable, Matrix4 pTransform)
        {
            m_Drawable = pDrawable;
            m_Transform = pTransform;
        }
        public GeometryNode GetDrawable()
        {
            return m_Drawable;
        }
    }
    class Shader
    {
        private List<Drawable> m_Que = new List<Drawable>();
        
        ShaderUtility m_Utility;
        public Shader(string pVertexShader, string pFragmentShader)
        {
            m_Utility = new ShaderUtility(@"ACW/Shaders/" + pVertexShader + @".vert", @"ACW/Shaders/" + pFragmentShader + @".frag");
            

        }
        public int GetShaderID()
        {
            return m_Utility.ShaderProgramID;
        }
        public void AddNodeToQue(GeometryNode pDrawable, Matrix4 pTransform)
        {
            m_Que.Add(new Drawable(pDrawable, pTransform));
        }
        public void BindData(GameObject pGameObject)
        {
            GL.UseProgram(GetShaderID());
            pGameObject.BindData(GetShaderID());
        }
        public void DrawQue()
        {
            GL.UseProgram(GetShaderID());
            int ModelLocation = GL.GetUniformLocation(GetShaderID(), "uModel");
            
            for (int x = 0; x < m_Que.Count; x++)
            {
               

                GameObject Holder = m_Que[x].GetDrawable().GetDrawable();
                Matrix4 Transform = Holder.GetTransform();
                GL.UniformMatrix4(ModelLocation, true, ref Transform );
                Holder.Draw(GetShaderID());
            }
        }
    }
}
