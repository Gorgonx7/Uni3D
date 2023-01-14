using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Labs.ACW.Lighting
{
    static class GlobalLight
    {
        private static Vector4 AmbiantColour;
        /// <summary>
        /// sets a universial global ambiance
        /// </summary>
        /// <param name="LightColour"></param>
        /// <param name="ShaderID"></param>
        public static void setAmbiantLightColour(Vector4 LightColour, int ShaderID)
        {
            AmbiantColour = LightColour;
            int amb = GL.GetUniformLocation(ShaderID, "SceneAmbiance");
            GL.Uniform4(amb, AmbiantColour);
        }
    }
}
