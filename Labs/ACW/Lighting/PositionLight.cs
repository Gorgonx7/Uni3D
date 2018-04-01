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
        private float m_AttenuationFactor;
        private float m_LiniarAttenuationFactor;
        private float m_QuadraticAttenuationFactor;
        public PositionLight(Vector4 pPosition) : base(pPosition)
        {

        }
    }
}
