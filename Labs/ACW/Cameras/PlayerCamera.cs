using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace Labs.ACW.Cameras
{
    class PlayerCamera : Camera
    {
        public PlayerCamera(Vector3 pPosition, Vector3 pDirection, Vector3 pRight) : base(pPosition, pDirection, pRight)
        {

        }
        protected override void MoveCamera(Matrix4 pTransform)
        {
            
            base.MoveCamera(pTransform);

        }
    }
}
