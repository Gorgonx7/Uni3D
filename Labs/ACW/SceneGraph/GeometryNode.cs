using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labs.ACW.Assets;
namespace Labs.ACW.SceneGraph
{
    class GeometryNode : Node
    {
        int m_ShaderID;
        GameObject m_Drawable;
        public GeometryNode(GameObject pDrawable, int pShaderID)
        {
            m_Drawable = pDrawable;
            m_ShaderID = pShaderID;
        }
        public int GetShaderID()
        {
            return m_ShaderID;
        }
        public GameObject GetDrawable()
        {
            return m_Drawable;
        }
        /*
        public void Draw()
        {
            m_Drawable.Draw();
        }*/
    }
}
