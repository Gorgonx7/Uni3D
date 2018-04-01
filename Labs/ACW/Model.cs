using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labs.Utility;
using OpenTK.Graphics.OpenGL;
using OpenTK;
namespace Labs.ACW
{
    class Model
    {
        private ModelUtility utility;
        private GeoHelper Geometry;
        private ShaderUtility Shader;
        private Matrix4 Transformation;
        private bool OBJ = false;
        public Model(string modelName)
        {
            if(modelName.Substring(modelName.IndexOf('.')) == ".obj"){
                OBJ = true;
            }
            utility = ModelUtility.LoadModel(@"Utility/Models/" + modelName);
            Geometry = new GeoHelper(utility);
            Transformation = Matrix4.CreateRotationY(0.8f) * Matrix4.CreateTranslation(0, 1f, -5f);
        }
        public void BindData(int ShaderID)
        {
            if (!OBJ)
            {
                // GL.UseProgram(Shader.ShaderProgramID);
                int vNormallocation = GL.GetAttribLocation(ShaderID, "vNormal");
                int vPositionLocation = GL.GetAttribLocation(ShaderID, "vPosition");
                Geometry.BindBuffer();
                GL.EnableVertexAttribArray(vPositionLocation);
                GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
                
                GL.EnableVertexAttribArray(vNormallocation);
                GL.VertexAttribPointer(vNormallocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            }

        }
        public void Draw(int ShaderID)
        {
            int vModelLocation = GL.GetUniformLocation(ShaderID, "uModel");
            GL.UniformMatrix4(vModelLocation, true, ref Transformation);
        }
        public void Transform(Matrix4 pTransform)
        {
            Transformation *= Matrix4.CreateTranslation(-Transformation.ExtractTranslation()) * pTransform * Matrix4.CreateTranslation(Transformation.ExtractTranslation());
        }
        public void SetTransform(Matrix4 pTransform)
        {
            Transformation = pTransform;
        }
        public void PassShader(ShaderUtility ShaderUtility)
        {
            Shader = ShaderUtility;
        }
        public GeoHelper GetGeometry()
        {
            return Geometry;
        }
    }
}
