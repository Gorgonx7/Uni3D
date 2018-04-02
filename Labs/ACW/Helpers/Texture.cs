﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Labs.ACW
{
    class Texture
    {
        Bitmap m_TextureBitmap;
        BitmapData m_TextureData;
        TextureUnit m_Unit;
        static int textureNumber = 0;
        int m_TextureID;
        public Texture(string Path)
        {
            m_Unit = TextureUnit.Texture0 + textureNumber;
            textureNumber++;
            string filepath = @Path;
            

            
            if (System.IO.File.Exists(filepath))
            {
                m_TextureBitmap = new Bitmap(filepath);
                m_TextureBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                m_TextureData = m_TextureBitmap.LockBits(new Rectangle(0, 0, m_TextureBitmap.Width, m_TextureBitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            }
            else
            {
                throw new Exception("Could not find file " + filepath);
            }
        }
        public void BindData()
        {
            GL.ActiveTexture(m_Unit);
            m_TextureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, m_TextureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, m_TextureData.Width, m_TextureData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, m_TextureData.Scan0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            m_TextureBitmap.UnlockBits(m_TextureData);
        }
        public void Bind(int ShaderID, int SamplerNumber)
        {
            int uTextureSamplerLocation = GL.GetUniformLocation(ShaderID, "uTexture" + SamplerNumber);
            GL.Uniform1(uTextureSamplerLocation, 0);

        }

    }
}