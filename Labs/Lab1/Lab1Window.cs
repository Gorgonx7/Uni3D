using System;
using Labs.Utility;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Labs.Lab1
{
    public class Lab1Window : GameWindow
    {
        private int[] mVertexBufferObjectID = new int[2];
        private ShaderUtility mShader;

        public Lab1Window()
            : base(
                800, // Width
                600, // Height
                GraphicsMode.Default,
                "Lab 1 Hello, Triangle",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, // major
                3, // minor
                GraphicsContextFlags.ForwardCompatible
                )
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Enable(EnableCap.CullFace);
            float[] vertices = new float[] {
                                              -0.6f, -0.6f,//0 
                                               0.2f, -0.6f,//1
                                               0.2f, -0.2f,//2
                                               0.4f, -0.2f,//3
                                               0.4f, -0.6f,//4
                                               0.6f, -0.6f,//5
                                               0.6f,  0.2f,//6
                                               0.4f,  0.2f,//Not used
                                               0.2f,  0.2f,//NA
                                               0.8f,  0.2f,//9
                                               0.4f,  0.6f,//10
                                               0.0f,  0.6f,//11
                                               0.0f,  0.8f,//12
                                              -0.2f,  0.8f,//13
                                              -0.2f,  0.6f,//14
                                              -0.4f,  0.6f,//15
                                              -0.8f,  0.2f,//16
                                              -0.6f,  0.2f,//17
                                               0.0f,  0.2f,//18
                                              -0.4f,  0.2f,//19
                                              -0.4f, -0.2f,//20
                                               0.0f, -0.2f //21
            };
            uint[] indices = new uint[] { 0, 1, 2, 21, 20, 19, 17, 6, 9, 10, 15, 16, 18, 21, 3, 4, 5, 12, 13, 11, 14   };
                                          
           /* float[] vertices = new float[] { 0.0f, 0.8f,
                                             0.8f, 0.4f,
                                             0.6f, -0.6f,
                                            -0.6f, -0.6f,
                                            -0.8f, 0.4f};
            uint[] indices = new uint[] { 1,0,2,
                                          4,
                                          3};*/

            GL.GenBuffers(2, mVertexBufferObjectID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferObjectID[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

            if (vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVertexBufferObjectID[1]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(uint)),
            indices, BufferUsageHint.StaticDraw);
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out
            size);
            if (indices.Length * sizeof(uint) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }





            #region Shader Loading Code - Can be ignored for now

            mShader = new ShaderUtility( @"Lab1/Shaders/vSimple.vert", @"Lab1/Shaders/fSimple.frag");

            #endregion

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferObjectID[0]);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVertexBufferObjectID[1]);

            // shader linking goes here
            #region Shader linking code - can be ignored for now

            GL.UseProgram(mShader.ShaderProgramID);
            int vPositionLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vPosition");
            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

            #endregion

            GL.DrawElements(PrimitiveType.TriangleFan, 7, DrawElementsType.UnsignedInt, 0);
            GL.DrawElements(PrimitiveType.TriangleFan, 10, DrawElementsType.UnsignedInt, 7 * sizeof(uint));
            GL.DrawElements(PrimitiveType.TriangleStrip, 4, DrawElementsType.UnsignedInt, 17 * sizeof(uint));

            this.SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            GL.DeleteBuffers(2, mVertexBufferObjectID);
            GL.UseProgram(0);
            mShader.Delete();
        }
    }
}
