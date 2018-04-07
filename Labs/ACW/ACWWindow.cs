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
    public class ACWWindow : GameWindow
    {
        public ACWWindow()
            : base(
                800, // Width
                600, // Height
                GraphicsMode.Default,
                "Assessed Coursework",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, // major
                3, // minor
                GraphicsContextFlags.ForwardCompatible
                )
        {
        }
        private ShaderUtility mShader, mTextureShader;
        private GroupNode mRoot;
        private Matrix4 mView, mProjection;
        //private Model model;
        private Model m_Teapot;
        private Plane m_Plane;
        private StaticCamera mStaticCamera;
        FrameBuffer test;
        protected override void OnLoad(EventArgs e)
        {
            PositionLight light = new PositionLight(new Vector3(1, 2f, -10), new Vector3(1f,0.015f,0.0000025f));
            
            light.SetDiffuse(new Vector3(1f, 1f, 1));
            light.SetSpecular(new Vector3(1f, 1, 1f));
            DirectionalLight dLight = new DirectionalLight(new Vector3(0, 1, 4));
            dLight.SetDiffuse(new Vector3(0f, 1f, 1f));
            dLight.SetSpecular(new Vector3(0, 1f, 1f));
            
            Vector3 CameraPosition = new Vector3(0, -1f, -2f);
            Vector3 CameraDirection = CameraPosition - new Vector3(0, 0, 0);
            //mView = Matrix4.CreateRotationX(1.571f) * Matrix4.CreateTranslation(0f, -4, -25f);
            mView = Matrix4.CreateRotationX(0.3f) * Matrix4.CreateTranslation(0, -6, -10);
            mStaticCamera = new StaticCamera(mView.ExtractTranslation(), CameraDirection, new Vector3(1, 0, 0));
           
            mStaticCamera.SetViewMatrix(mView);
            mStaticCamera.Activate();
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.CullFace);
            
            GL.Enable(EnableCap.Lighting);
            mTextureShader = new ShaderUtility("ACW/Shaders/Texture.vert", "ACW/Shaders/Texture.frag");
            mShader = new ShaderUtility("ACW/Shaders/Model.vert", "ACW/Shaders/Model.frag");
            
            //model = new Model("SphereTri.obj", "Earth.jpg");
            m_Teapot = new Model("Plane.obj", "Teapot.jpg");
            m_Plane = new Plane(0,0);
            GL.UseProgram(mShader.ShaderProgramID);
            //GL.UseProgram(mTextureShader.ShaderProgramID);
            GlobalLight.setAmbiantLightColour(new Vector4(1f, 1f, 1f, 1f), mShader.ShaderProgramID);
           
            Spotlight sLight = new Spotlight(new Vector3(0, 3, 5), new Vector3(1f, 0, 0), 89, 20f, (m_Plane.GetTransform() * Matrix4.CreateTranslation(new Vector3(0,-2.5f,-0f))).ExtractTranslation());
            m_Plane.BindData(mShader.ShaderProgramID) ;
            mStaticCamera.Bind(mShader.ShaderProgramID);
             mProjection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.1f, 100);
            //mProjection = Matrix4.CreateOrthographic((float)ClientRectangle.Width/8, ClientRectangle.Height/8, 0.1f, 100);
            int uProjection = GL.GetUniformLocation(mShader.ShaderProgramID, "uProjection");
            GL.UniformMatrix4(uProjection, true, ref mProjection);
            //light.Bind(mShader.ShaderProgramID);
            sLight.Bind(mShader.ShaderProgramID);
           // model.BindData(mShader.ShaderProgramID);
    
            GL.UseProgram(mTextureShader.ShaderProgramID);
            GlobalLight.setAmbiantLightColour(new Vector4(1f, 1f, 1f, 1f), mTextureShader.ShaderProgramID);

            
           // model.BindData(mTextureShader.ShaderProgramID);
            m_Teapot.BindData(mTextureShader.ShaderProgramID);
            //m_Plane.BindData(mTextureShader.ShaderProgramID);
            mStaticCamera.Bind(mTextureShader.ShaderProgramID);
            uProjection = GL.GetUniformLocation(mTextureShader.ShaderProgramID, "uProjection");
            
            GL.UniformMatrix4(uProjection, true, ref mProjection);
            light.Bind(mTextureShader.ShaderProgramID);
            dLight.Bind(mTextureShader.ShaderProgramID);
            sLight.Bind(mTextureShader.ShaderProgramID);
            GL.BindVertexArray(0);
            
            
            


           
            //model.Transform(Matrix4.CreateScale(0.5f));
           // model.Transform(m_Plane.GetTransform() * Matrix4.CreateTranslation(new Vector3(-0, 7, -3f)));
            m_Teapot.Transform(Matrix4.CreateScale(0.1f) * m_Plane.GetTransform() * Matrix4.CreateTranslation(0f, 4, -0.5f));
            base.OnLoad(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(this.ClientRectangle);
            if (mShader != null)
            {
                int uProjectionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uProjection");
                int windowHeight = this.ClientRectangle.Height;
                int windowWidth = this.ClientRectangle.Width;
                if (windowHeight > windowWidth)
                {
                    if (windowWidth < 1)
                    {

                        windowWidth = 1;
                    }
                    float ratio = windowHeight / windowWidth;
                    mProjection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.1f, 100);
                    GL.UniformMatrix4(uProjectionLocation, true, ref mProjection);
                    int uProjection = GL.GetUniformLocation(mTextureShader.ShaderProgramID, "uProjection");

                    GL.UniformMatrix4(uProjection, true, ref mProjection);
                }
                else
                {
                    if (windowHeight < 1)
                    {
                        windowHeight = 1;
                    }
                    float ratio = windowWidth / windowHeight;
                    Matrix4 mProjection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.1f, 100);
                    GL.UniformMatrix4(uProjectionLocation, true, ref mProjection);
                    int uProjection = GL.GetUniformLocation(mTextureShader.ShaderProgramID, "uProjection");

                    GL.UniformMatrix4(uProjection, true, ref mProjection);
                }
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
 	        base.OnUpdateFrame(e);
            //model.Transform(Matrix4.CreateRotationZ(0.1f));
           // m_Teapot.Transform(new Vector4(model.GetTransform().ExtractTranslation(),1), 0.01f);
          //  model.Transform(Matrix4.CreateRotationY(0.01f));
            //model.Transform(Matrix4.CreateTranslation(new Vector3(0,0,-0.09f)));
           // model.Transform(Matrix4.CreateRotationX(0.1f));
            
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            PostProcessor.RenderToBuffer();
            test = new FrameBuffer();
            base.OnRenderFrame(e);
            test.Draw();PostProcessor.SimpleRender(test.GetData(), ClientRectangle);
            GL.Viewport(ClientRectangle);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            
           
            m_Plane.Draw(mShader.ShaderProgramID);
            m_Teapot.Draw(mTextureShader.ShaderProgramID);
            //Bitmap Frame = test.GetData();
            //Just render the quad from (-1,-1) to (1,1) and set your modelview and projection matrices to the identity. 
            //Turn off depth testing. Your quad then goes into and covers the screen in homogeneous coords. 
            //By setting glViewport you can make it cover the whole screen or even (for debug) cover some part of the screen.

            //m_Teapot.setTexture(new Texture(test.GetData()));
            
           // m_Teapot.Draw(mTextureShader.ShaderProgramID);
            //m_Plane.Draw(mShader.ShaderProgramID);
            //m_Teapot.Draw(mTextureShader.ShaderProgramID);
            GL.BindVertexArray(0);
            this.SwapBuffers();
        }
        private void MakeTree()
        {
            mRoot = new GroupNode();
            TransformNode WorldTransform = new TransformNode(Matrix4.CreateTranslation(new Vector3(-0, 6, -30f)));
           // GeometryNode geometryNode = new GeometryNode(model, mShader.ShaderProgramID);
           // WorldTransform.AddNode(geometryNode);
            mRoot.AddNode(WorldTransform);

        }
        
        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            Texture.Delete();
            GL.BindVertexArray(0);
            GeoHelper.DeleteBuffers();
            mShader.Delete();
            mTextureShader.Delete();
            base.OnUnload(e);
        }
    }
}
