using System;
using System.Collections.Generic;
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
        public FrameBuffer()
        {
            FramebufferID = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferID);
            FrameTextureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, FrameTextureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 1024, 768, 0, PixelFormat.Rgb, PixelType.UnsignedByte, (IntPtr)0);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, new int[] { (int)TextureMinFilter.Nearest });
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, new int[] { (int)TextureMagFilter.Nearest });
            DepthBufferID = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, DepthBufferID);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent32, 1024, 768);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, DepthBufferID);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, FrameTextureID, 0);
            DrawBuffersEnum[] drawBuffers = { DrawBuffersEnum.ColorAttachment0 };

            GL.DrawBuffers(1, drawBuffers);
            if(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception("Framebuffer incomplete");
            }
        }

        public void Draw()
        {
            GL.BindFramebuffer( FramebufferTarget.Framebuffer, FramebufferID);
            GL.Viewport(0, 0, 1024, 768);

        }
    }
}
