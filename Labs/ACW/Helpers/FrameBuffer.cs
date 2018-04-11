using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
namespace Labs.ACW
{
    /// <summary>
    /// creates a new frame buffer and manages it's content
    /// </summary>
    class FrameBuffer
    {
        private int FramebufferID;
        private int FrameTextureID;
        private int DepthBufferID;
        private const int ClientWidth = 1024;
        private const int ClientHeight = 768;
        /// <summary>
        /// constructs and generates all the buffers, binds them to one frame buffer objects
        /// </summary>
        public FrameBuffer()
        {
            // gen frame buffer
            GL.ActiveTexture(TextureUnit.Texture0);
            FramebufferID = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferID);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
            // gen colour buffer
            FrameTextureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, FrameTextureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, ClientWidth, ClientHeight, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte, (IntPtr)0);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, new int[] { (int)TextureMinFilter.Nearest });
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, new int[] { (int)TextureMagFilter.Nearest });
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, FrameTextureID, 0);

            // gen depth buffer
            DepthBufferID = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, DepthBufferID);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent32, ClientWidth, ClientHeight);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, DepthBufferID);
            
            
            if(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception("Framebuffer incomplete");
            }
        }
        
        /// <summary>
        /// prepares the frame buffer for drawing too
        /// </summary>
        public void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Enable(EnableCap.Texture2D);
            GL.BindFramebuffer( FramebufferTarget.Framebuffer, FramebufferID);
            GL.Viewport(0, 0, 1024, 768);

        }
        /// <summary>
        /// dumps the data to the main frame buffer
        /// </summary>
        /// <param name="ClientRectangle"></param>
        public void Dump(Rectangle ClientRectangle)
        {
            GL.BindFramebuffer( FramebufferTarget.ReadFramebuffer, FramebufferID);
            GL.ReadBuffer( ReadBufferMode.ColorAttachment0);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            GL.Viewport(ClientRectangle);
            GL.BlitFramebuffer(0, 0, ClientWidth, ClientHeight, 0, 0, ClientRectangle.Width, ClientRectangle.Height, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
        }
        /// <summary>
        /// returns the ID for the texture
        /// </summary>
        /// <returns></returns>
        public int GetTexture()
        {
            return FrameTextureID;
        }
    }
}
