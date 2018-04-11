using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
namespace Labs.ACW.Lighting
{
    class Spotlight : PositionLight
    {
        public Spotlight(Vector3 pPosition, Vector3 pAttenuationFactor, float pCutoff, float pExp, Vector3 pDirection) : base(pPosition, pAttenuationFactor)
        {
            m_SpotCutOff = pCutoff;
            m_SpotExponent = pExp;
            m_SpotDirection = pDirection;
        }
        public override void SetPosition(Vector4 pPosition, Matrix4 pTransform)
        {
            m_SpotDirection = Vector3.Transform(m_SpotDirection, pTransform);
            for (int x = 0; x < m_Shader_IDs.Count; x++)
            {
                GL.UseProgram(m_Shader_IDs[x]);
                int uLightDirection = GL.GetUniformLocation(m_Shader_IDs[x], "uLight[" + m_LightNumber + "].spotDirection");
                GL.Uniform3(uLightDirection, m_SpotDirection);
            }
            base.SetPosition(pPosition, pTransform);
        }
    }
}
