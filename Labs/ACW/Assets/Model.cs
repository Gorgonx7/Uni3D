using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labs.Utility;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Labs.ACW;
namespace Labs.ACW.Assets
{
    
    class Model : GameObject
    {
        private ModelUtility m_Utility;       
        private ShaderUtility m_Shader;
        private int[] m_VBO_IDs; 
        private bool OBJ = false;
        
        public Model(string modelName)
        {
            if(modelName.Substring(modelName.IndexOf('.')) == ".obj"){
                OBJ = true;
            }
            m_Utility = ModelUtility.LoadModel(@"Utility/Models/" + modelName);
            Geometry = new GeoHelper(m_Utility);
            Transformation = Matrix4.CreateRotationY(0.8f) * Matrix4.CreateTranslation(0, 0f, 0f);
        }
        public Model(string modelName, string TextureName)
        {
            if (modelName.Substring(modelName.IndexOf('.')) == ".obj")
            {
                OBJ = true;
            }
            m_Utility = ModelUtility.LoadModel(@"Utility/Models/" + modelName);
            Geometry = new GeoHelper(m_Utility);
            Transformation = Matrix4.CreateRotationY(0.8f) * Matrix4.CreateTranslation(0, 0f, 0f);
            m_Texture = new Texture(@"ACW/Assets/Textures/" + TextureName);

        }
        
        public override void BindData(int ShaderID)
        {
           
            Geometry.GenerateArrayBuffers();
            GL.BindVertexArray(Geometry.GetVAO_ID());
            //GL.UseProgram(ShaderID);
            int vPositionLocation = GL.GetAttribLocation(ShaderID, "vPosition");
            int vNormallocation = GL.GetAttribLocation(ShaderID, "vNormal");
            int vTextureLocation = GL.GetAttribLocation(ShaderID, "vTexture");
            if(m_Texture != null)
                m_Texture.BindData();

            if (!OBJ)
            {
                // GL.UseProgram(Shader.ShaderProgramID);
                Geometry.BindBuffer();
                
                
                
                GL.EnableVertexAttribArray(vPositionLocation);
                GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
                
                GL.EnableVertexAttribArray(vNormallocation);
                GL.VertexAttribPointer(vNormallocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            }
            else
            {
                Geometry.BindBuffer();
                
                GL.EnableVertexAttribArray(vPositionLocation);
                GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
                GL.EnableVertexAttribArray(vNormallocation);
                GL.VertexAttribPointer(vNormallocation, 3, VertexAttribPointerType.Float, true, 8 * sizeof(float), 5 * sizeof(float));
                if(vTextureLocation != -1)
                {
                    GL.EnableVertexAttribArray(vTextureLocation);
                    GL.VertexAttribPointer(vTextureLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
                }

                /*GL.GenBuffers(m_VBO_IDs.Length, m_VBO_IDs);
                GL.BindBuffer(BufferTarget.ArrayBuffer, m_VBO_IDs[0]);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(m_Utility.verts.Length * sizeof(float)), m_Utility.verts, BufferUsageHint.StaticDraw);
                GL.EnableVertexAttribArray(vPositionLocation);
                GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, m_VBO_IDs[1]);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(m_Utility.noms.Length * sizeof(float)), m_Utility.noms, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_VBO_IDs[2]);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(m_Utility.Indices.Length * sizeof(float)), m_Utility.Indices, BufferUsageHint.StaticDraw);
                
                GL.EnableVertexAttribArray(vNormallocation);
                GL.VertexAttribPointer(vNormallocation, 3, VertexAttribPointerType.Float, true, 3 * sizeof(float), 0);
                if(vTextureLocation != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, m_VBO_IDs[3]);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(m_Utility.tex.Length * sizeof(float)), m_Utility.tex, BufferUsageHint.StaticDraw);
                    GL.EnableVertexAttribArray(vTextureLocation);
                    GL.VertexAttribPointer(vTextureLocation, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 3 * sizeof(float));
                }*/

            }
            

        }
        public void DeleteTexture()
        {
            this.m_Texture.Dispose();
        }
        public override void Draw(int ShaderID)
        {

            base.Draw(ShaderID);

            if (m_Texture != null)
            {
                m_Texture.Bind(ShaderID);
            }
           
            
            GL.DrawElements(PrimitiveType.Triangles, GetGeometry().mIndices.Length, DrawElementsType.UnsignedInt, 0);
        }
        
        public void PassShader(ShaderUtility ShaderUtility)
        {
            m_Shader = ShaderUtility;
        }
        
    }
}
