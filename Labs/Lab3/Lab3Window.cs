using Labs.Utility;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace Labs.Lab3
{
    public class Lab3Window : GameWindow
    {
        public Lab3Window()
            : base(
                800, // Width
                600, // Height
                GraphicsMode.Default,
                "Lab 3 Lighting and Material Properties",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, // major
                3, // minor
                GraphicsContextFlags.ForwardCompatible
                )
        {
        }

        private int[] mVBO_IDs = new int[5];
        private int[] mVAO_IDs = new int[3];
        private ShaderUtility mShader;
        private ModelUtility mModelUtility, mCylinderModelUtility;
        private Matrix4 mView, mModel, mGroundModel, mCylinderModel;

        protected override void OnLoad(EventArgs e)
        {
            // Set some GL state
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            mShader = new ShaderUtility(@"Lab3/Shaders/vPassThrough.vert", @"Lab3/Shaders/fLighting.frag");
           // mShader = new ShaderUtility(@"Z:\svn general\3D Graphics\Labs\Lab3\Shaders\vPassThrough.vert", @"Z:\svn general\3D Graphics\Labs\Lab3\Shaders\fLighting.frag");
            GL.UseProgram(mShader.ShaderProgramID);
            int vPositionLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vPosition");
            int vNormallocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vNormal");
            GL.GenVertexArrays(mVAO_IDs.Length, mVAO_IDs);
            GL.GenBuffers(mVBO_IDs.Length, mVBO_IDs);





            #region buffer data
            float[] vertices = new float[] {-10, 0, -10,0,1,0,
                                             -10, 0, 10,0,1,0,
                                             10, 0, 10,0,1,0,
                                             10, 0, -10,0,1,0,};
            #endregion

            #region Vertex buffer object 1
            GL.BindVertexArray(mVAO_IDs[0]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vNormallocation);
            GL.VertexAttribPointer(vNormallocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            #endregion

            #region Vertex buffer object 2 - model loader
            mModelUtility = ModelUtility.LoadModel(@"Utility/Models/Model.bin"); 
            GL.BindVertexArray(mVAO_IDs[1]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mModelUtility.Vertices.Length * sizeof(float)), mModelUtility.Vertices, BufferUsageHint.StaticDraw);           
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[2]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mModelUtility.Indices.Length * sizeof(float)), mModelUtility.Indices, BufferUsageHint.StaticDraw);
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mModelUtility.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mModelUtility.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }
            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vNormallocation);
            GL.VertexAttribPointer(vNormallocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
           
            #endregion
            
            
            #region cylinder vertex buffer
            mCylinderModelUtility = ModelUtility.LoadModel(@"Utility/Models/cylinder.bin");
            GL.BindVertexArray(mVAO_IDs[2]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[3]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mCylinderModelUtility.Vertices.Length * sizeof(float)), mCylinderModelUtility.Vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[4]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mCylinderModelUtility.Indices.Length * sizeof(float)), mCylinderModelUtility.Indices, BufferUsageHint.StaticDraw);
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mCylinderModelUtility.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mCylinderModelUtility.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }
            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vNormallocation);
            GL.VertexAttribPointer(vNormallocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            #endregion


            GL.BindVertexArray(0);
            #region view matrix definition
            mView = Matrix4.CreateTranslation(0, -2.5f, 1);
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);
            int uEye = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");
            Vector3 cameraPosition = mView.ExtractTranslation();
            Vector4 cameraPosition2 = new Vector4(cameraPosition, 1f);
            GL.Uniform4(uEye, ref cameraPosition2);
            #endregion

            #region Light position definition
            // light 1 - position
            int uLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[0].Position");
            Vector4 lightPosition = new Vector4(2, 4, -8.5f, 1);
            lightPosition = Vector4.Transform(lightPosition, mView);
            GL.Uniform4(uLightPositionLocation, lightPosition);
            //light 1 - colour
            int uAmbientLightLocation1 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[0].AmbientLight");
            Vector3 colour = new Vector3(0.1f, 0.0f, 0.0f);
            GL.Uniform3(uAmbientLightLocation1, colour);
            int uDiffuseLightLocation1 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[0].DiffuseLight");
            GL.Uniform3(uDiffuseLightLocation1, colour);
            int uSpecularLightLocation1 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[0].SpecularLight");
            GL.Uniform3(uSpecularLightLocation1, colour);
            
            
            // light 2 - position
            int uLightPositionLocation2 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[1].Position");
            lightPosition = new Vector4(0, 4, 3f, 1);
            lightPosition = Vector4.Transform(lightPosition, mView);
            GL.Uniform4(uLightPositionLocation2, lightPosition);
            //light 2 - colour
            colour = new Vector3(0.0f, 0.0f, 0.1f);
            int uAmbientLightLocation2 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[1].AmbientLight");
            GL.Uniform3(uAmbientLightLocation2, colour);
            int uDiffuseLightLocation2 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[1].DiffuseLight");
            GL.Uniform3(uDiffuseLightLocation2, colour);
            int uSpecularLightLocation2 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[1].SpecularLight");
            GL.Uniform3(uSpecularLightLocation2, colour);
            //light 3 - position
            int uLightPositionLocation3 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[2].Position");
            lightPosition = new Vector4(6, 4, 3f, 1);
            lightPosition = Vector4.Transform(lightPosition, mView);
            GL.Uniform4(uLightPositionLocation3, lightPosition);
            //light 3 - colour
            colour = new Vector3(0.0f, 0.1f, 0.0f);
            int uAmbientLightLocation3 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[2].AmbientLight");
            GL.Uniform3(uAmbientLightLocation3, colour);
            int uDiffuseLightLocation3 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[2].DiffuseLight");
            GL.Uniform3(uDiffuseLightLocation3, colour);
            int uSpecularLightLocation3 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[2].SpecularLight");
            GL.Uniform3(uSpecularLightLocation3, colour);

            #endregion
            
            mGroundModel = Matrix4.CreateTranslation(0, 0, -5f);
            mModel = Matrix4.CreateRotationY(-1.5f) * Matrix4.CreateTranslation(0, 3, -5f)  ;
            mCylinderModel = Matrix4.CreateTranslation(0, 1f, -5f);
            base.OnLoad(e);
            
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(this.ClientRectangle);
            if (mShader != null)
            {
                int uProjectionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uProjection");
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.5f, 25);
                GL.UniformMatrix4(uProjectionLocation, true, ref projection);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (e.KeyChar == 'w')
            {
                mView = mView * Matrix4.CreateTranslation(0.0f, 0.0f, 0.05f);
                MoveCamera();
            }
            if (e.KeyChar == 'a')
            {
                mView = mView * Matrix4.CreateRotationY(-0.025f);
                MoveCamera();         
            }
            if (e.KeyChar == 's')
            {
                mView = mView * Matrix4.CreateTranslation(0.0f, 0.0f, -0.05f);
                MoveCamera();
            }
            if (e.KeyChar == 'd')
            {
                mView = mView * Matrix4.CreateRotationY(0.025f);
                MoveCamera();
            }
            if (e.KeyChar == 'z')
            {
                Vector3 t = mGroundModel.ExtractTranslation();
                Matrix4 translation = Matrix4.CreateTranslation(t);
                Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
                mGroundModel = mGroundModel * inverseTranslation * Matrix4.CreateRotationY(0.025f) * translation;
            }
            if (e.KeyChar == 'x')
            {
                Vector3 t = mGroundModel.ExtractTranslation();
                Matrix4 translation = Matrix4.CreateTranslation(t);
                Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
                mGroundModel = mGroundModel * inverseTranslation * Matrix4.CreateRotationY(-0.025f) * translation;
            }
            if (e.KeyChar == 'c')
            {
                Vector3 t = mModel.ExtractTranslation();
                Matrix4 translation = Matrix4.CreateTranslation(t);
                Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
                mModel = mModel * inverseTranslation * Matrix4.CreateRotationY(-0.025f) * translation;
            }
            if (e.KeyChar == 'v')
            {
                Vector3 t = mModel.ExtractTranslation();
                Matrix4 translation = Matrix4.CreateTranslation(t);
                Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
                mModel = mModel * inverseTranslation * Matrix4.CreateRotationY(0.025f) * translation;
            }
        }
        private void UpdateMaterialProperties(Vector3 ambiantReflection, Vector3 diffuseReflection, Vector3 specularReflection, float Shininess) {
            /*AmbientReflectivity; 
		vec3 DiffuseReflectivity; 
		vec3 SpecularReflectivity;*/
            int AmbiantReflectivity = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.AmbientReflectivity");
            GL.Uniform3(AmbiantReflectivity, ambiantReflection);
            int DiffuseReflectivity = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.DiffuseReflectivity");
            GL.Uniform3(DiffuseReflectivity, diffuseReflection);
            int SpecularReflectivity = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.SpecularReflectivity");
            GL.Uniform3(SpecularReflectivity, specularReflection);
            int Shininessref = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.Shininess");
            GL.Uniform1(Shininessref, Shininess);

        }
        private void MoveCamera()
        {

            /**/
            int uLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.Position");
            Vector4 lightPosition = new Vector4(2, 4, -8.5f, 1);
            lightPosition = Vector4.Transform(lightPosition, mView);
            GL.Uniform4(uLightPositionLocation, lightPosition);


            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);
            int uEye = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");
            Vector3 cameraPosition = mView.ExtractTranslation();
            Vector4 cameraPosition2 = new Vector4(cameraPosition, 1f);
            GL.Uniform4(uEye, ref cameraPosition2);
            
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            int uModel = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            GL.UniformMatrix4(uModel, true, ref mGroundModel);  

            GL.BindVertexArray(mVAO_IDs[0]);
            UpdateMaterialProperties(new Vector3(0.05f,0.05f,0.05f), new Vector3(0.5f,0.5f,0.5f), new Vector3(0.7f,0.7f,0.7f), .078125f);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

            mModel *= Matrix4.CreateRotationY(0.1f);
            Matrix4 m = mModel * mGroundModel;
            uModel = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            GL.UniformMatrix4(uModel, true, ref m);
            
            GL.BindVertexArray(mVAO_IDs[1]);
           // UpdateMaterialProperties(new Vector3(0.24725f,0.1995f,0.0745f), new Vector3(0.75164f,0.60648f,0.22648f), new Vector3(0.628281f,0.555802f,0.366065f), 0.4f);
            GL.DrawElements(PrimitiveType.Triangles, mModelUtility.Indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(mVAO_IDs[2]);
            m = mCylinderModel * mGroundModel;
            uModel = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            GL.UniformMatrix4(uModel, true, ref m);
          //  UpdateMaterialProperties(new Vector3(0.0f,0.0f,0.0f), new Vector3(.5f, .0f, .0f), new Vector3(0.7f, 0.6f, 0.6f), .25f);
            GL.DrawElements(PrimitiveType.Triangles, mCylinderModelUtility.Indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
            this.SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.DeleteBuffers(mVBO_IDs.Length, mVBO_IDs);
            GL.DeleteVertexArrays(mVAO_IDs.Length, mVAO_IDs);
            mShader.Delete();
            base.OnUnload(e);
        }
    }
}
