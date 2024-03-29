﻿using OpenTK;
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
        /// <summary>
        /// Constructor for the window
        /// </summary>
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
        private ShaderUtility mShader, mTextureShader, mMultiTexture;
        private GroupNode mRoot;
        private Matrix4 mView;
        private Model model;
        private Model m_Teapot, m_Cylinder;
        private Plane m_Plane;
        private PlayerCamera mPlayerCamera;
        private StaticCamera mStaticCamera;
        private OrthographicCamera mOrthographicCamera, mFrontCamera;
        FrameBuffer test;
        /// <summary>
        /// Method to load all the data on to the graphics card
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            PositionLight light = new PositionLight(new Vector3(0, 0.5f, -5), new Vector3(5f,0.015f,0.0000025f));
            
            //light.SetDiffuse(new Vector3(1f, 1f, 1));
           // light.SetSpecular(new Vector3(1f, 1, 1f));
            DirectionalLight dLight = new DirectionalLight(new Vector3(0, 1, 4));
            //dLight.SetDiffuse(new Vector3(1f, 1f, 1f));
            //dLight.SetSpecular(new Vector3(1, 1f, 1f));
            
            Vector3 CameraPosition = new Vector3(0, -1f, -2f);
            Vector3 CameraDirection = CameraPosition - new Vector3(0, 0, 0);
            mView = Matrix4.CreateRotationX(1.571f) * Matrix4.CreateTranslation(0f, -4, -25f);
            mOrthographicCamera = new OrthographicCamera(mView, ClientRectangle);
            mView = Matrix4.CreateRotationX(0.3f) * Matrix4.CreateTranslation(0, -6, -20);
            mPlayerCamera = new PlayerCamera(mView, ClientRectangle);
            mView = Matrix4.CreateRotationY(-0.78f) * Matrix4.CreateRotationX(0.3f) * Matrix4.CreateTranslation(3, -6, -20);
            mStaticCamera = new StaticCamera(mView, ClientRectangle);
            mView = Matrix4.CreateTranslation(0, -6, -20);
            mFrontCamera = new OrthographicCamera(mView, ClientRectangle);
            //mStaticCamera.SetViewMatrix(mView);
           
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.CullFace);
            
            GL.Enable(EnableCap.Lighting);
            mTextureShader = new ShaderUtility("ACW/Shaders/Texture.vert", "ACW/Shaders/Texture.frag");
            mShader = new ShaderUtility("ACW/Shaders/Model.vert", "ACW/Shaders/Model.frag");
            mMultiTexture = new ShaderUtility("ACW/Shaders/Texture.vert", "ACW/Shaders/MultiTexture.frag");
            m_Cylinder = new Model("Cylinder.bin");
            model = new Model("SphereTri.obj", "Earth.jpg");
            m_Teapot = new Model("utah-teapot.obj", new string[] { "Teapot.jpg", "Texture.png" });
            //m_Teapot = new Model("utah-teapot.obj", "ACW.jpg");
            m_Plane = new Plane(0,0);
            Spotlight sLight = new Spotlight(new Vector3(0, 3, 5), new Vector3(1f, 0, 0), 20, 20f, (m_Plane.GetTransform() * Matrix4.CreateTranslation(new Vector3(0,-2.5f,-0f))).ExtractTranslation());
          
            GL.UseProgram(mShader.ShaderProgramID);
            //GL.UseProgram(mTextureShader.ShaderProgramID);
            GlobalLight.setAmbiantLightColour(new Vector4(1f, 1f, 1f, 1f), mShader.ShaderProgramID);
           
            m_Plane.BindData(mShader.ShaderProgramID) ;
            m_Cylinder.BindData(mShader.ShaderProgramID);
             
            //mProjection = Matrix4.CreateOrthographic((float)ClientRectangle.Width/8, ClientRectangle.Height/8, 0.1f, 100);
            
            light.Bind(mShader.ShaderProgramID);
            sLight.Bind(mShader.ShaderProgramID);
            //dLight.Bind(mShader.ShaderProgramID);
            // model.BindData(mShader.ShaderProgramID);
            #region shader 2
            GL.UseProgram(mTextureShader.ShaderProgramID);
            GlobalLight.setAmbiantLightColour(new Vector4(1f, 1f, 1f, 1f), mTextureShader.ShaderProgramID);
            m_Teapot.BindData(mTextureShader.ShaderProgramID);
            
            
           
            //m_Plane.BindData(mTextureShader.ShaderProgramID);
           
            
            
            light.Bind(mTextureShader.ShaderProgramID);
            dLight.Bind(mTextureShader.ShaderProgramID);
            sLight.Bind(mTextureShader.ShaderProgramID);
            #endregion

            GL.UseProgram(mMultiTexture.ShaderProgramID);
            model.BindData(mMultiTexture.ShaderProgramID);;
            light.Bind(mMultiTexture.ShaderProgramID);
            dLight.Bind(mMultiTexture.ShaderProgramID);
            sLight.Bind(mMultiTexture.ShaderProgramID);


            GL.BindVertexArray(0);

            /*GL.UseProgram(mSimpleTexture.ShaderProgramID);
           
            mStaticCamera.Bind(mSimpleTexture.ShaderProgramID);
            uProjection = GL.GetUniformLocation(mSimpleTexture.ShaderProgramID, "uProjection");
            GL.UniformMatrix4(uProjection, true, ref mProjection);*/




            //model.Transform(Matrix4.CreateScale(0.5f));
             model.Transform(Matrix4.CreateScale(2) * m_Plane.GetTransform() * Matrix4.CreateTranslation(new Vector3(-0, 7, -3f)));
            m_Cylinder.SetTransform(model.GetTransform() * Matrix4.CreateTranslation(5,-4,5));
            m_Cylinder.setMaterial(new Material(new Vector4(0.25f,0.25f,0.25f, 1f), new Vector4(0.4f, 0.4f, 0.4f, 1.0f), new Vector4(0.774597f, 0.774597f, 0.774597f, 1.0f), 0.6f));
            m_Teapot.Transform(Matrix4.CreateScale(0.1f) * m_Plane.GetTransform() * Matrix4.CreateTranslation(3f, 4, -0.5f));
            //m_Teapot.SetTransform(Matrix4.CreateTranslation(new Vector3(0, 0.5f, -10)));
            mPlayerCamera.Activate();
            test = new FrameBuffer();
            base.OnLoad(e);
        }
        /// <summary>
        /// Move the camera or change the active camera
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            switch (e.KeyChar)
            {
                case 'd':
                    //Camera.s_ActiveCamera.Transform(Matrix4.CreateTranslation(-0.05f, 0.0f, 0.0f));
                    Camera.s_ActiveCamera.Transform(0.05f);
                    break;
                case 'a':
                    //Camera.s_ActiveCamera.Transform(Matrix4.CreateTranslation(0.05f, 0.0f, 0.0f));
                    Camera.s_ActiveCamera.Transform(-0.05f);
                    break;
                case 's':
                    Camera.s_ActiveCamera.Transform(Matrix4.CreateTranslation(0.0f, 0.0f, -0.05f));

                    break;
                case 'w':
                    Camera.s_ActiveCamera.Transform(Matrix4.CreateTranslation(0.0f, 0.0f, 0.05f));

                    break;
                case 'z':
                    GameObject.GlobalTransform(Matrix4.CreateRotationY(-0.05f));
                    break;
                case 'x':
                    GameObject.GlobalTransform(Matrix4.CreateRotationY(0.05f));
                    break;
                case '1':
                    mPlayerCamera.Activate();
                    break;
                case '2':
                    mOrthographicCamera.Activate();
                    break;
                case '3':
                    mFrontCamera.Activate();
                    break;
                case '4':
                    mStaticCamera.Activate();
                    break;
            }
        }
        /// <summary>
        /// resizes the screen
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Camera.s_ActiveCamera.Resize(ClientRectangle);
        }
        /// <summary>
        /// updates the objects on the screen
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
 	        base.OnUpdateFrame(e);
            //model.Transform(Matrix4.CreateRotationZ(0.1f));
            m_Teapot.Transform(new Vector4(model.GetTransform().ExtractTranslation(),1), 0.01f);
            model.Transform(Matrix4.CreateRotationY(0.01f));
            //model.Transform(Matrix4.CreateTranslation(new Vector3(0,0,-0.09f)));
            // model.Transform(Matrix4.CreateRotationX(0.1f));
           
        }
        /// <summary>
        /// renders to the render buffer and then dumps the data to draw the frame buffer
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //PostProcessor.RenderToBuffer();
            
            base.OnRenderFrame(e);
            //Prepare();
            //Render();
            test.Draw();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            m_Teapot.Draw(mMultiTexture.ShaderProgramID);
            m_Plane.Draw(mShader.ShaderProgramID);
            m_Cylinder.Draw(mShader.ShaderProgramID);
            model.Draw(mTextureShader.ShaderProgramID);
            test.Dump(ClientRectangle);


            GL.BindVertexArray(0);
            this.SwapBuffers();
        }
        
        /// <summary>
        /// legasy code to make a scene graph
        /// </summary>
        private void MakeTree()
        {
            mRoot = new GroupNode();
            TransformNode WorldTransform = new TransformNode(Matrix4.CreateTranslation(new Vector3(-0, 6, -30f)));
           // GeometryNode geometryNode = new GeometryNode(model, mShader.ShaderProgramID);
           // WorldTransform.AddNode(geometryNode);
            mRoot.AddNode(WorldTransform);

        }
        /// <summary>
        /// deletes all the data
        /// </summary>
        /// <param name="e"></param>
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
