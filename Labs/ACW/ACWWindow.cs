using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using Labs.Utility;
using Labs.ACW.Cameras;
using Labs.ACW.Assets;
using Labs.ACW.Lighting;
using Labs.ACW.SceneGraph;
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
        private Model model;
        private Plane m_Plane;
        private StaticCamera mStaticCamera;
        protected override void OnLoad(EventArgs e)
        {
            DirectionalLight light = new DirectionalLight(new Vector3(4, 3, 1));
            
            Vector3 CameraPosition = new Vector3(0, -1f, -2f);
            Vector3 CameraDirection = CameraPosition - new Vector3(0, 0, 0);
            mView = Matrix4.CreateTranslation(0f, -1, -2f);
            mStaticCamera = new StaticCamera(mView.ExtractTranslation(), CameraDirection, new Vector3(1, 0, 0));
           
            mStaticCamera.SetViewMatrix(mView);
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            mTextureShader = new ShaderUtility("ACW/Shaders/Texture.vert", "ACW/Shaders/Texture.frag");
            mShader = new ShaderUtility("ACW/Shaders/Model.vert", "ACW/Shaders/Model.frag");
            
            
            GL.UseProgram(mShader.ShaderProgramID);
            //GL.UseProgram(mTextureShader.ShaderProgramID);
            GlobalLight.setAmbiantLightColour(new Vector4(1f, 1f, 1f, 1f), mShader.ShaderProgramID);
            m_Plane = new Plane(0, 0);
            m_Plane.BindData(mShader.ShaderProgramID) ;
            mStaticCamera.Bind(mShader.ShaderProgramID);
            mProjection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.1f, 100);
            int uProjection = GL.GetUniformLocation(mShader.ShaderProgramID, "uProjection");
            GL.UniformMatrix4(uProjection, true, ref mProjection);
            light.Bind(mShader.ShaderProgramID);
          
    
            GL.UseProgram(mTextureShader.ShaderProgramID);
            GlobalLight.setAmbiantLightColour(new Vector4(1f, 1f, 1f, 1f), mTextureShader.ShaderProgramID);

            model = new Model("utah-teapot.obj", "MarbleTiles.jpg");
            model.BindData(mTextureShader.ShaderProgramID);

            mStaticCamera.Bind(mTextureShader.ShaderProgramID);
            uProjection = GL.GetUniformLocation(mTextureShader.ShaderProgramID, "uProjection");
            
            GL.UniformMatrix4(uProjection, true, ref mProjection);
            light.Bind(mTextureShader.ShaderProgramID);

            GL.BindVertexArray(0);
            
            
            


           
            model.Transform(Matrix4.CreateScale(0.5f));
            model.Transform(Matrix4.CreateTranslation(new Vector3(-0, 6, -30f)));
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
                }
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
 	        base.OnUpdateFrame(e);
            //model.Transform(Matrix4.CreateRotationZ(0.1f));
            model.Transform(Matrix4.CreateRotationY(0.01f));
            //model.Transform(Matrix4.CreateTranslation(new Vector3(0,0,-0.09f)));
           // model.Transform(Matrix4.CreateRotationX(0.1f));
            
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            

            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            
           
            m_Plane.Draw(mShader.ShaderProgramID);
            
            model.Draw(mTextureShader.ShaderProgramID);


            GL.BindVertexArray(0);
            this.SwapBuffers();
        }
        private void MakeTree()
        {
            mRoot = new GroupNode();
            TransformNode WorldTransform = new TransformNode(Matrix4.CreateTranslation(new Vector3(-0, 6, -30f)));
            GeometryNode geometryNode = new GeometryNode(model, mShader.ShaderProgramID);
            WorldTransform.AddNode(geometryNode);
            mRoot.AddNode(WorldTransform);

        }
        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            
            GL.BindVertexArray(0);
            GeoHelper.DeleteBuffers();
            mShader.Delete();
            base.OnUnload(e);
        }
    }
}
