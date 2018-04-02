using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace Labs.ACW.Lighting
{
    abstract class Light
    {
        /*\
        |*| Position
        |*| Colour - ambiant, diffuse and specular
        |*|
        |*| vec4 position;   
        |*| vec4 diffuse;
        |*| vec4 specular; 
        |*| float constantAttenuation, linearAttenuation, quadraticAttenuation;
        |*| float spotCutoff, spotExponent; 
        |*| vec3 spotDirection;
        \*/
        private static int s_LightNumber = 0;
        private Vector4 m_Position;
        private Vector4 m_DiffuseColour;
        private Vector4 m_SpecularColour;
        private float m_ConstantAttenuation, m_LinearAttenuation, m_QuadraticAttenuation;
        protected float m_SpotCutOff, m_SpotExponent;
        protected Vector3 m_SpotDirection;
        protected int m_LightNumber;
        public Light(Vector4 pPosition)
        {
            m_LightNumber = s_LightNumber;
            s_LightNumber++;
            m_Position = pPosition;
            m_DiffuseColour = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            m_SpecularColour = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            m_ConstantAttenuation = 0;
            m_LinearAttenuation = 1;
            m_QuadraticAttenuation = 0;
            m_SpotCutOff = 180;
            m_SpotExponent = 0;
            m_SpotDirection = new Vector3(0, 0, 0);
        }
        public virtual void Bind(int ShaderID)
        {
            // bind these then check the vertex shader to ensure each of the attributes are being created
            int uLightPosition = GL.GetUniformLocation(ShaderID, "uLightPosition");
            GL.Uniform4(uLightPosition, ref m_Position);
            int Position = GL.GetUniformLocation(ShaderID, "uLight[" + m_LightNumber + "].position");
            GL.Uniform4(Position, ref m_Position);
            int Diffuse = GL.GetUniformLocation(ShaderID, "uLight[" + m_LightNumber + "].diffuse");
            GL.Uniform4(Diffuse, m_DiffuseColour);
            int Specular = GL.GetUniformLocation(ShaderID, "uLight[" + m_LightNumber + "].specular");
            GL.Uniform4(Specular, m_SpecularColour);
            int constantAttenuation = GL.GetUniformLocation(ShaderID, "uLight[" + m_LightNumber + "].constantAttenuation");
            GL.Uniform1(constantAttenuation, m_ConstantAttenuation);
            int linearAttenuation = GL.GetUniformLocation(ShaderID, "uLight[" + m_LightNumber + "].linearAttenuation");
            GL.Uniform1(linearAttenuation, m_LinearAttenuation);
            int quadraticAttenuation = GL.GetUniformLocation(ShaderID, "uLight[" + m_LightNumber + "].quadraticAttenuation");
            GL.Uniform1(quadraticAttenuation, m_QuadraticAttenuation);
            int SpotlightCutoff = GL.GetUniformLocation(ShaderID, "uLight[" + m_LightNumber + "].spotCutoff");
            GL.Uniform1(SpotlightCutoff, m_SpotCutOff);
            int SpotExponent = GL.GetUniformLocation(ShaderID, "uLight[" + m_LightNumber + "].spotExponent");
            GL.Uniform1(SpotExponent, m_SpotExponent);
            int SpotlightDirection = GL.GetUniformLocation(ShaderID, "uLight[" + m_LightNumber.ToString() + "].spotDirection");
            GL.Uniform3(SpotlightDirection, m_SpotDirection);

        }


    }
}
