using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;

namespace Labs.ACW.Cameras
{
    /// <summary>
    /// creates a player controlled camera
    /// </summary>
    class PlayerCamera : Camera
    {
        public PlayerCamera(Vector3 pPosition, Vector3 pDirection, Vector3 pRight, Rectangle pScreen) : base(pPosition, pDirection, pRight, Matrix4.CreatePerspectiveFieldOfView(1, (float)pScreen.Width / pScreen.Height, 0.1f, 100))
        {

        }
        public PlayerCamera(Matrix4 pView, Rectangle pScreen) : base(pView, Matrix4.CreatePerspectiveFieldOfView(1, (float)pScreen.Width / pScreen.Height, 0.1f, 100))
        {

        }
        protected override void MoveCamera(Matrix4 pTransform)
        {
            
            base.MoveCamera(pTransform);

        }
    }
}
