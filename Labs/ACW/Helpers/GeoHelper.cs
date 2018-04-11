using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Labs.Utility;
namespace Labs.ACW
{
    /// <summary>
    /// struct that holds the information for binding to the shader
    /// </summary>
    struct ShaderAttribute {
        int m_Reference;
        int m_VertexNumber;
        string m_Name;
        int m_Offset;
        int m_SizeOfData;
        bool m_Nomralised;
        
        VertexAttribPointerType m_Type;

        public ShaderAttribute(int reference, int NumberOfVertex, VertexAttribPointerType PointerType, bool Normalise, int offset, int sizeofdata, string name)
        {
            m_Reference = reference;
            m_VertexNumber = NumberOfVertex;
            m_SizeOfData = sizeofdata;
            m_Offset = offset;
            m_Type = PointerType;
            m_Nomralised = Normalise;
            m_Name = name;
        }
        public int GetReference()
        {
            return m_Reference;
        }
        public int GetNumberOfVertex() {
            return m_VertexNumber;
        }
        public int GetOffset()
        {
            return m_Offset;
        }
        public int GetSizeOfData()
        {
            return m_SizeOfData;
        }
        public VertexAttribPointerType GetVertexType()
        {
            return m_Type;
        }
        public bool GetNormalised()
        {
            return m_Nomralised;
        }
    }
    class GeoHelper
    {
        private static List<int> s_VBO_IDs = new List<int>();
        private static List<int> s_VAO_IDs = new List<int>();
        private int[] mVBO_IDs;
        private int mLengthOfVertex;
        public float[] mVertices { get; private set; }
        public uint[] mIndices { get; private set; }
        private int mVAO_ID;
        /// <summary>
        /// Creates geometry that has just vertex
        /// </summary>
        /// <param name="pVertices">The data</param>
        /// <param name="pVertexLength">The length of a vertex</param>
        public GeoHelper(float[] pVertices, int pVertexLength)
        {
            mVBO_IDs = new int[1];
            mLengthOfVertex = pVertexLength;
            GL.GenBuffers(1, mVBO_IDs);
            for(int x = 0; x < mVBO_IDs.Length; x++)
            {
                s_VBO_IDs.Add(mVBO_IDs[x]);
            }
            mVertices = pVertices;
            
        }
        /// <summary>
        /// The same as the above constructor just with indices
        /// </summary>
        /// <param name="pVertices"></param>
        /// <param name="pIndices"></param>
        /// <param name="pVertexLength"></param>
        public GeoHelper(float[] pVertices, uint[] pIndices, int pVertexLength)
        {
            mVBO_IDs = new int[2];
            mLengthOfVertex = pVertexLength;
            GL.GenBuffers(2, mVBO_IDs);
            for (int x = 0; x < mVBO_IDs.Length; x++)
            {
                s_VBO_IDs.Add(mVBO_IDs[x]);
            }
            mVertices = pVertices;
            mIndices = pIndices;
        }
        /// <summary>
        /// creates geometry from a model utility
        /// </summary>
        /// <param name="modelUtility"></param>
        public GeoHelper(ModelUtility modelUtility)
        {
            mVBO_IDs = new int[2];
            GL.GenBuffers(2, mVBO_IDs);
            for(int x = 0; x < mVBO_IDs.Length; x++)
            {
                s_VBO_IDs.Add(mVBO_IDs[x]);
            }
            mVertices = modelUtility.Vertices;
            mIndices = modelUtility.Indices;
        }
        /// <summary>
        /// binds the geometry to the buffer
        /// </summary>
        public void BindBuffer()
        {
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mVertices.Length * sizeof(float)), mVertices, BufferUsageHint.StaticDraw);
            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mVertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }
            if (mVBO_IDs.Length > 1)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[1]);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mIndices.Length * sizeof(int)), mIndices, BufferUsageHint.StaticDraw);

                GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
                if (mIndices.Length * sizeof(float) != size)
                {
                    throw new ApplicationException("Index data not loaded onto graphics card correctly");
                }
            }
        }
        /// <summary>
        /// Generates teh array buffers and binds the properties to the shader
        /// </summary>
        /// <param name="properties"></param>
        public void GenerateArrayBuffers(ShaderAttribute[] properties)
        {
            mVAO_ID = GL.GenVertexArray();
            GL.BindVertexArray(mVAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, GetVBO_IDs()[0]);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, GetVBO_IDs()[1]);
            for(int x = 0; x < properties.Length; x++)
            {
                GL.VertexAttribPointer(properties[x].GetReference(), properties[x].GetNumberOfVertex(), properties[x].GetVertexType(), properties[x].GetNormalised(), mLengthOfVertex * properties[x].GetSizeOfData(), properties[x].GetOffset() * properties[x].GetSizeOfData());
                GL.EnableVertexAttribArray(properties[x].GetReference());
            }
            /*GL.BindVertexArray(mVertexArrayObjectIDs[1]);
             GL.BindBuffer(BufferTarget.ArrayBuffer, mSquareVertexBufferObjectIDArray[0]);
             GL.BindBuffer(BufferTarget.ElementArrayBuffer, mSquareVertexBufferObjectIDArray[1]);
             GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
             GL.VertexAttribPointer(vColourLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
             GL.EnableVertexAttribArray(vColourLocation);
             GL.EnableVertexAttribArray(vPositionLocation);*/
        }
        /// <summary>
        /// just generates array buffers but does not bind anything
        /// </summary>
        /// <returns></returns>
        public int GenerateArrayBuffers()
        {

            return mVAO_ID = GL.GenVertexArray();
        }
        /// <summary>
        /// returns the vbo ids
        /// </summary>
        /// <returns></returns>
        public int[] GetVBO_IDs() {
            return mVBO_IDs;
        }
        /// <summary>
        /// returns the VAO id
        /// </summary>
        /// <returns></returns>
        public int GetVAO_ID()
        {
            return mVAO_ID;
        }
        /// <summary>
        /// deletes the content in all the buffers
        /// </summary>
        public static void DeleteBuffers() {
            GL.DeleteBuffers(s_VBO_IDs.Count, s_VBO_IDs.ToArray());
            GL.DeleteVertexArrays(s_VAO_IDs.Count, s_VAO_IDs.ToArray());
        }
    }
}
