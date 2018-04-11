using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using Labs.Utility;
using Labs.ACW.Cameras;
using Labs.ACW.Assets;
using Labs.ACW.Lighting;
using Labs.ACW.SceneGraph;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Labs.ACW
{
    /// <summary>
    /// legasy code for post processing
    /// </summary>
    static class PostProcessor
    {

        private static ShaderUtility SimpleShader = new ShaderUtility("ACW/Shaders/FBOPassThrough.vert", "ACW/Shaders/FBOPassThrough.frag");
        private static float[] verts = new float[] { -1.0f,  1.0f,
                                                      1.0f,  1.0f,
                                                      1.0f, -1.0f,
                                                     -1.0f,  1.0f,
                                                     -1.0f, -1.0f,
                                                      1.0f, -1.0f};
        private static uint[] indices = new uint[] { 1,2,3,4,5,6 };
        private static int[] VBO = new int[2];
        private static int VAO;
        private static int TextureLocation;
        
        public static void RenderToBuffer()
        {
            GL.UseProgram(SimpleShader.ShaderProgramID);
            GL.GenBuffers(2,VBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verts.Length * sizeof(float)), verts, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, VBO[1]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(uint)), indices, BufferUsageHint.StaticDraw);

            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (verts.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[0]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[1]);
            int PositionLocation = GL.GetAttribLocation(SimpleShader.ShaderProgramID, "vertexIn");
            TextureLocation = GL.GetUniformLocation(SimpleShader.ShaderProgramID, "t");
            GL.EnableVertexAttribArray(PositionLocation);
            GL.VertexAttribPointer(PositionLocation, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);



        }

        public static void SimpleRender(Bitmap pBitmap, Rectangle pViewport)
        {
            //GL.Disable(EnableCap.DepthTest);
            //GL.Disable(EnableCap.CullFace);
            //GL.Enable(EnableCap.Texture2D);
            //GL.BindTexture(TextureTarget.Texture2D, )
            
            
            /*
            GL.UseProgram(SimpleShader.ShaderProgramID);
            GL.Disable(EnableCap.Lighting);
            GL.Viewport(pViewport);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Texture Frame = new Texture(pBitmap);
            
            GL.Uniform1(TextureLocation, 0);
            GL.BindVertexArray(VAO);
            Frame.BindData();
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            Frame.Dispose();*/
        }
        public static void SimpleRender(int TextureID, Rectangle pViewpoint)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, TextureID);
            GL.LoadIdentity();
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.TexCoord2(0, 1);
            GL.TexCoord2(1, 1);
            GL.TexCoord2(1, 0);
            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.End();
        }
    }
}
