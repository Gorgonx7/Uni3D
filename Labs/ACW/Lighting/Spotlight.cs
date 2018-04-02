using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
