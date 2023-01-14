using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace Labs.ACW.SceneGraph
{
    class TransformNode : Node
    {
        Matrix4 m_Transform;
        List<Node> m_Nodes = new List<Node>();
        public TransformNode(Matrix4 pTransformation) : base()
        {
            m_Transform = pTransformation;
        }
        public Matrix4 GetTransform()
        {
            return m_Transform;
        }
        public void AddNode(Node pNode)
        {
            m_Nodes.Add(pNode);
        }
        public List<Node> GetNodes()
        {
            return m_Nodes;
        }
    }
}
