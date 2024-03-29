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
    /// <summary>
    /// Helps with the loading and managing of textures
    /// </summary>
    class Texture
    {
        public static List<int> s_TextureIDs = new List<int>();
        Bitmap m_TextureBitmap;
        BitmapData m_TextureData;
        TextureUnit m_Unit;
        static int textureNumber = 1;
        int m_TextureID;
        int m_Index;
        /// <summary>
        /// generates a texture form a path
        /// </summary>
        /// <param name="Path"></param>
        public Texture(string Path)
        {
            m_Unit = TextureUnit.Texture0 + textureNumber;
            m_Index = textureNumber;
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
        /// <summary>
        /// legasy code to create a texture from a bitmap for post rendering
        /// </summary>
        /// <param name="pBitmap"></param>
        public Texture(Bitmap pBitmap) {
            m_Unit = TextureUnit.Texture0;
            m_Index = 0;
            

            m_TextureBitmap = pBitmap;
            m_TextureData = m_TextureBitmap.LockBits(new Rectangle(0, 0, m_TextureBitmap.Width, m_TextureBitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

        }
        /// <summary>
        /// Binds the texture to the texture unit
        /// </summary>
        public void BindData()
        {
            GL.ActiveTexture(m_Unit);
            m_TextureID = GL.GenTexture();
            s_TextureIDs.Add(m_TextureID);
            GL.BindTexture(TextureTarget.Texture2D, m_TextureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, m_TextureData.Width, m_TextureData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, m_TextureData.Scan0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            m_TextureBitmap.UnlockBits(m_TextureData);
        }
        /// <summary>
        /// flips a texture on a axis
        /// </summary>
        public void FlipX()
        {
            m_TextureBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }
        /// <summary>
        /// binds the data to a buffer
        /// </summary>
        /// <param name="ShaderID"></param>
        public void Bind(int ShaderID)
        {
            int uTextureSamplerLocation = GL.GetUniformLocation(ShaderID, "uTexture0");
            
                GL.Uniform1(uTextureSamplerLocation, m_Index);
            

        }
        /// <summary>
        /// binds the data to the shader as an array
        /// </summary>
        /// <param name="ShaderID"></param>
        /// <param name="TextureNumber"></param>
        public void Bind(int ShaderID, int TextureNumber)
        {
            int uTextureSamplerLocation = GL.GetUniformLocation(ShaderID, "uTexture[" + TextureNumber + "]");
            GL.Uniform1(uTextureSamplerLocation, m_Index);
        }
        /// <summary>
        /// deletes all the data
        /// </summary>
        public static void Delete()
        {
            GL.DeleteTextures(s_TextureIDs.Count, s_TextureIDs.ToArray());
        }
        /// <summary>
        /// deletes a indervidual data member
        /// </summary>
        public void Dispose()
        {
            GL.DeleteTexture(m_TextureID);
            s_TextureIDs.Remove(m_TextureID);
            m_TextureData = null;
            m_TextureBitmap.Dispose();
        }

 
    }
}
