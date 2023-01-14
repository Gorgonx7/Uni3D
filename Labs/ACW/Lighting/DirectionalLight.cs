using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace Labs.ACW.Lighting
{
    class DirectionalLight : Light
    {
        /// <summary>
        /// creates a directional light
        /// </summary>
        /// <param name="pPosition"></param>
        public DirectionalLight(Vector3 pPosition) : base(new Vector4(pPosition,0))
        {

        }

    }
}
