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
        private static List<Light> s_Lights = new List<Light>();
        private Vector4 m_AmbiantColour;
        private Vector4 m_DiffuseColour;
        private Vector4 m_SpecularColour;
        protected float m_ConstantAttenuation, m_LinearAttenuation, m_QuadraticAttenuation;
        protected float m_SpotCutOff, m_SpotExponent;
        protected Vector3 m_SpotDirection;
        protected int m_LightNumber;
        private List<int> m_Shader_IDs = new List<int>();
        public Light(Vector4 pPosition)
        {
            m_LightNumber = s_LightNumber;
            s_LightNumber++;
            m_Position = pPosition;
            m_AmbiantColour = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            m_DiffuseColour = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            m_SpecularColour = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            m_ConstantAttenuation = 0;
            m_LinearAttenuation = 1;
            m_QuadraticAttenuation = 0;
            m_SpotCutOff = 180;
            m_SpotExponent = 0;
            m_SpotDirection = new Vector3(0, 0, 0);
            s_Lights.Add(this);
        }
        public static List<Light> GetLights()
        {
            return s_Lights;
        }
        public Vector4 GetPosition()
        {
            return m_Position;
        }
        public void SetPosition(Vector4 pPosition)
        {
            m_Position = pPosition;
            for (int x = 0; x < m_Shader_IDs.Count; x++)
            {
                int uLightPosition = GL.GetUniformLocation(m_Shader_IDs[x],"uLight[" + m_LightNumber + "].position");
                GL.Uniform4(uLightPosition, m_Position);
            }
        }
        public virtual void Bind(int ShaderID)
        {
            m_Shader_IDs.Add(ShaderID);
            // bind these then check the vertex shader to ensure each of the attributes are being created
            int uLightPosition = GL.GetUniformLocation(ShaderID, "uLightPosition");
            GL.Uniform4(uLightPosition, ref m_Position);
            int Position = GL.GetUniformLocation(ShaderID, "uLight[" + m_LightNumber + "].position");
            GL.Uniform4(Position, ref m_Position);
            int Ambinat = GL.GetUniformLocation(ShaderID, "uLight[" + m_LightNumber + "].ambiant");
            GL.Uniform4(Ambinat, m_AmbiantColour);
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
        public void SetDiffuse(Vector3 pColour)
        {
            m_DiffuseColour = new Vector4(pColour, 1);
        }
        public void SetSpecular(Vector3 pColour)
        {
            m_SpecularColour = new Vector4(pColour, 1);
        }
        public void SetAmbinat(Vector3 pColour)
        {
            m_AmbiantColour = new Vector4(pColour, 1);
        }

    }
}
