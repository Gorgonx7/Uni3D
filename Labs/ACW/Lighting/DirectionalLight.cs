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
        public DirectionalLight(Vector3 pPosition) : base(new Vector4(pPosition,0))
        {

        }

    }
}
