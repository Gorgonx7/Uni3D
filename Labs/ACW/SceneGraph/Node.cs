using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.ACW.SceneGraph
{
    abstract class Node
    {
        public void Accept(RenderVisitor pRenderVisitor)
        {
            pRenderVisitor.Visit(this);
        }
    }
}
