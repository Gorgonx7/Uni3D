﻿using Labs.Utility;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace Labs.Lab2
{
    class Lab2_1Window : GameWindow
    {
        private int[] mTriangleVertexBufferObjectIDArray = new int[2];
        private int[] mSquareVertexBufferObjectIDArray = new int[2];
        private int[] mVertexArrayObjectIDs = new int[2];
        private ShaderUtility mShader;

        public Lab2_1Window()
            : base(
                800, // Width
                600, // Height
                GraphicsMode.Default,
                "Lab 2_1 Linking to Shaders and VAOs",
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
            GL.ClearColor(Color4.CadetBlue);
            GL.Enable(EnableCap.DepthTest);


            #region Indices and Vertices
            uint[] triangleIndices = new uint[] { 0, 1, 2 };
            float[] squareVertices = new float[] { -0.2f, -0.4f, 0.2f, 0.5f, 0.0f, 1.0f,
                                                    0.8f, -0.4f, 0.2f, 0.0f, 0.0f, 1.0f,
                                                    0.8f, 0.6f, 0.2f, 0.5f, 0.0f, 1.0f,
                                                   -0.2f, 0.6f, 0.2f, 0.0f, 1.0f, 1.0f};

            float[] triangleVertices = new float[] { -0.8f, 0.8f, 0.4f, 0.0f, 0.0f, 1.0f,
                                                     -0.6f, -0.4f, 0.4f, 1.0f, 1.0f, 1.0f,
                                                      0.2f, 0.2f, 0.4f, 0.0f, 0.0f, 0.0f};
            uint[] squareIndices = new uint[] { 0, 1, 2, 3
            };
            #endregion
            
            #region Triangle Buffers
            GL.GenBuffers(2, mTriangleVertexBufferObjectIDArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mTriangleVertexBufferObjectIDArray[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(triangleVertices.Length * sizeof(float)), triangleVertices, BufferUsageHint.StaticDraw);

            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

            if (triangleVertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mTriangleVertexBufferObjectIDArray[1]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(triangleIndices.Length * sizeof(int)), triangleIndices, BufferUsageHint.StaticDraw);

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);

            if (triangleIndices.Length * sizeof(int) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }
            #endregion

            #region Square Buffers
            GL.GenBuffers(2, mSquareVertexBufferObjectIDArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mSquareVertexBufferObjectIDArray[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(squareVertices.Length * sizeof(float)), squareVertices, BufferUsageHint.StaticDraw);
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

            if (squareVertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("squareVertex data not loaded onto graphiccs card correctly");
            }
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mSquareVertexBufferObjectIDArray[1]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(squareIndices.Length * sizeof(int)), squareIndices, BufferUsageHint.StaticDraw);
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (squareIndices.Length * sizeof(int) != size)
            {
                throw new ApplicationException("squareIndex data not loaded onto graphics card corretly");
            }
            #endregion

            

            #region Shader Loading Code

            mShader = new ShaderUtility(@"Lab2/Shaders/vLab21.vert", @"Lab2/Shaders/fSimple.frag");
            int vColourLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vColour");
            int vPositionLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vPosition");
            GL.EnableVertexAttribArray(vColourLocation);

            #endregion

            #region Vertex Array Object Buffers
            GL.GenVertexArrays(2, mVertexArrayObjectIDs);
            GL.BindVertexArray(mVertexArrayObjectIDs[0]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mTriangleVertexBufferObjectIDArray[0]);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mTriangleVertexBufferObjectIDArray[1]);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.VertexAttribPointer(vColourLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(vColourLocation);
            GL.EnableVertexAttribArray(vPositionLocation);

            GL.BindVertexArray(mVertexArrayObjectIDs[1]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mSquareVertexBufferObjectIDArray[0]);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mSquareVertexBufferObjectIDArray[1]);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.VertexAttribPointer(vColourLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(vColourLocation);
            GL.EnableVertexAttribArray(vPositionLocation);

            #endregion

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(mShader.ShaderProgramID);
            GL.BindVertexArray(mVertexArrayObjectIDs[1]);
            GL.DrawElements(PrimitiveType.TriangleFan, 4, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(mVertexArrayObjectIDs[0]);
            GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
            this.SwapBuffers();

        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            GL.DeleteBuffers(2, mTriangleVertexBufferObjectIDArray);
            GL.DeleteBuffers(2, mSquareVertexBufferObjectIDArray);
            GL.BindVertexArray(0);
            GL.DeleteVertexArrays(2, mVertexArrayObjectIDs);
            GL.UseProgram(0);
            mShader.Delete();
        }
    }
}
