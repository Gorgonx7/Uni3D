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
    class FrameBuffer
    {
        private int FramebufferID;
        private int FrameTextureID;
        private int DepthBufferID;
        private const int ClientWidth = 1024;
        private const int ClientHeight = 768;
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
        public Bitmap GetData()
        {
            Bitmap b = new Bitmap(ClientWidth, ClientHeight);
            var bits = b.LockBits(new Rectangle(0, 0, ClientWidth, ClientHeight), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.ReadPixels(0, 0, ClientWidth, ClientHeight, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bits.Scan0);
            GL.Ext.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Ext.DeleteFramebuffers(1, ref FramebufferID);
            b.UnlockBits(bits);
            return b;
        }
        public void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindFramebuffer( FramebufferTarget.Framebuffer, FramebufferID);
            GL.Viewport(0, 0, 1024, 768);

        }
        public int GetTexture()
        {
            return FrameTextureID;
        }
    }
}
