using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK;
using Labs.ACW.Cameras;
namespace Labs.ACW.SceneGraph
{
    class RenderVisitor
    {
        List<Matrix4> m_Transform = new List<Matrix4>();
        List<Shader> m_Shaders;
        Camera m_Camera;

        public RenderVisitor(Camera pCamera, List<Shader> pShaders)
        {
            m_Camera = pCamera;
            m_Shaders = pShaders;
        }
        public void Visit(Node pNode)
        {
            
        }
        private void Visit(GroupNode pGroupNode)
        {

            foreach (Node i in pGroupNode.GetNodes())
            {
                i.Accept(this);
            }
        }
        private void Visit(TransformNode pTransformNode)
        {
            m_Transform.Add(pTransformNode.GetTransform());
            foreach (Node i in pTransformNode.GetNodes())
            {
                i.Accept(this);
            }
            m_Transform.RemoveAt(m_Transform.Count - 1);
        }
        private void Visit(GeometryNode pGeometryNode)
        {

        }
        public void Draw()
        {

        }
        
    }
}
