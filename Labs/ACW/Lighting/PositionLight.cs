using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.ACW.Lighting
{
    class PositionLight : Light
    {

        /// <summary>
        /// creates a positional light in the shader
        /// </summary>
        /// <param name="pPosition"></param>
        /// <param name="pAttenuationFactor"></param>
        public PositionLight(Vector3 pPosition, Vector3 pAttenuationFactor) : base(new Vector4(pPosition, 1))
        {
            m_ConstantAttenuation = pAttenuationFactor.X;
            m_LinearAttenuation = pAttenuationFactor.Y;
            m_QuadraticAttenuation = pAttenuationFactor.Z;
        }
    }
}
