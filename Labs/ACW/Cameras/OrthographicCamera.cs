using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.ACW.Cameras
{
    class OrthographicCamera : Camera
    {
        public OrthographicCamera(Matrix4 pTransform, Rectangle pScreen) : base(pTransform, Matrix4.CreateOrthographic((float)pScreen.Width / 8, pScreen.Height / 8, 0.1f, 100))
        {

        }
        protected override void MoveCamera(Matrix4 pTransform)
        {
           
        }

    }
}
