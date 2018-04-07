using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
namespace Labs.ACW.Assets
{
    class Plane : GameObject
    {
        // create a plane in the world that is of width w and height h for use when testing texturing and lighting, 
        //should be easy enough to create using a standared array with scaling as I have no need for collision;

        float[] vertices = new float[] {-10, 0, -10, 0, 1, 0, 0, 0,
                                        -10, 0,  10, 0, 1, 0, 0, 1,
                                         10, 0,  10, 0, 1, 0, 1, 1,
                                         10, 0, -10, 0, 1, 0, 1, 0};


        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        /// <param name="ShaderID"></param>
        public Plane(int pWidth, int pHeight) : base()
        {/*
            
            mVAO_ID = GL.GenVertexArray();
            mVBO_ID = GL.GenBuffer();
            GL.BindVertexArray(mVAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_ID);
            
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.BindVertexArray(mVAO_ID);
            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vNormallocation);
            GL.VertexAttribPointer(vNormallocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            
            */
            Transformation = Matrix4.CreateTranslation(0, 0, -5f);
            
        }
        public Plane(Texture texture)
        {
            m_Texture = texture;
        }
        public override void BindData(int ShaderID)
        {
            int vPositionLocation = GL.GetAttribLocation(ShaderID, "vPosition");
            int vNormallocation = GL.GetAttribLocation(ShaderID, "vNormal");
            Geometry = new GeoHelper(vertices, 8);
            
            Geometry.GenerateArrayBuffers();
            GL.BindVertexArray(Geometry.GetVAO_ID());
            Geometry.BindBuffer();
            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vNormallocation);
            GL.VertexAttribPointer(vNormallocation, 3, VertexAttribPointerType.Float, true, 8 * sizeof(float), 3 * sizeof(float));
            if(m_Texture != null)
            {
                int vTextureLocation = GL.GetAttribLocation(ShaderID, "vTexture");
                GL.VertexAttribPointer(vTextureLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            }
            //base.BindData(ShaderID);
        }
        public override void Draw(int ShaderID)
        {
            base.Draw(ShaderID);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
        }

    }
}
