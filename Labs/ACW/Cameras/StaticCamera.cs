using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.ACW.Cameras
{
    class StaticCamera : Camera
    {
        public StaticCamera(Vector3 pPosition, Vector3 pDirection, Vector3 pRight) : base(pPosition, pDirection, pRight) {
            
        }
    }
}
