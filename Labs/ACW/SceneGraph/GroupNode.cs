using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.ACW.SceneGraph
{
    class GroupNode : Node
    {
        List<Node> m_Nodes = new List<Node>();
        public GroupNode() : base()
        {
            
        }
        
        public List<Node> GetNodes()
        {
            return m_Nodes;
        }
        public void AddNode(Node pNode)
        {
            m_Nodes.Add(pNode);
        }
    }
}
