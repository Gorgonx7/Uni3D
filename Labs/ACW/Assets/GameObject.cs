using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;
namespace Labs.ACW.Assets
{
    /// <summary>
    /// the material struct defines the colour of the mesh/model
    /// </summary>
    public struct Material
    {
        Vector4 m_AmbientReflectivity;
        Vector4 m_DiffuseReflectivity;
        Vector4 m_SpecularReflectivity;
        Vector4 m_Emissive; // defines the emissive property of the material
        float m_Shininess;
        //Change this to take vec3s? create a new constructor?
        public Material(Vector4 AmbientReflectivtiy, Vector4 DiffuseReflectivity, Vector4 SpecularReflectivity, float Shininess)
        {
            m_AmbientReflectivity = AmbientReflectivtiy;
            m_DiffuseReflectivity = DiffuseReflectivity;
            m_SpecularReflectivity = SpecularReflectivity;
            m_Shininess = Shininess * 128;
            m_Emissive = new Vector4(0, 0, 0, 1);
        }
        public Material(Vector4 AmbientReflectivtiy, Vector4 DiffuseReflectivity, Vector4 SpecularReflectivity, float Shininess, Vector4 Emissive)
        {
            m_AmbientReflectivity = AmbientReflectivtiy;
            m_DiffuseReflectivity = DiffuseReflectivity;
            m_SpecularReflectivity = SpecularReflectivity;
            m_Shininess = Shininess * 128;
            m_Emissive = Emissive;
        }
        public void Bind(int ShaderID)
        {
            int Amb = GL.GetUniformLocation(ShaderID, "uMaterial.AmbientReflectivity");
            GL.Uniform4(Amb, m_AmbientReflectivity);
            int Dif = GL.GetUniformLocation(ShaderID, "uMaterial.DiffuseReflectivity");
            GL.Uniform4(Dif, m_DiffuseReflectivity);
            int Spec = GL.GetUniformLocation(ShaderID, "uMaterial.SpecularReflectivity");
            GL.Uniform4(Spec, m_SpecularReflectivity);
            int Shin = GL.GetUniformLocation(ShaderID, "uMaterial.Shininess");
            GL.Uniform1(Shin, m_Shininess);
            int Emi = GL.GetUniformLocation(ShaderID, "uMaterial.Emissive");
            GL.Uniform4(Emi, m_Emissive);
        }
    }
    abstract class GameObject
    {
        public static List<GameObject> s_Objects = new List<GameObject>();
        public Matrix4 Transformation;
        protected GeoHelper Geometry;
        protected Material m_Material;
        protected Texture m_Texture = null;
        public GameObject()
        {
            s_Objects.Add(this);
            m_Material = new Material(new Vector4(.1f, .1f, .1f, 1.0f), new Vector4(0.55f, 0.55f, 0.55f, 1.0f), new Vector4(0.70f, 0.70f, 0.70f, 1.0f), 0.25f);
            Transformation = Matrix4.Identity;
        }
        /// <summary>
        /// prepares the model/object to be drawn
        /// </summary>
        /// <param name="ShaderID"></param>
        public virtual void Draw(int ShaderID)
        {
            GL.UseProgram(ShaderID);
            GL.BindVertexArray(GetGeometry().GetVAO_ID());
            int ModelLocation = GL.GetUniformLocation(ShaderID, "uModel");
            GL.UniformMatrix4(ModelLocation, true, ref Transformation);
            m_Material.Bind(ShaderID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>The transformation matrix</returns>
        public Matrix4 GetTransform()
        {
            return Transformation;
        }
        /// <summary>
        /// Transforms all the models in the list
        /// </summary>
        /// <param name="pTransform"></param>
        public static void GlobalTransform(Matrix4 pTransform)
        {
            for(int x = 0; x < s_Objects.Count; x++)
            {
                s_Objects[x].Transform(pTransform);
            }
        }
        /// <summary>
        /// moves a object relative to it's current position
        /// </summary>
        /// <param name="pTransform"></param>
        public void Transform(Matrix4 pTransform)
        {
            Transformation *= Matrix4.CreateTranslation(-Transformation.ExtractTranslation()) * pTransform * Matrix4.CreateTranslation(Transformation.ExtractTranslation());
        }
        /// <summary>
        /// sets the transformation matrix
        /// </summary>
        /// <param name="pTransform"></param>
        public void SetTransform(Matrix4 pTransform)
        {
            Transformation = pTransform;
        }
        /// <summary>
        /// rotates the object around a point in 3D space over a given angle
        /// </summary>
        /// <param name="pPoint"></param>
        /// <param name="angle"></param>
        public void Transform(Vector4 pPoint, float angle)
        {
            Vector4 currentPosition = new Vector4(Transformation.ExtractTranslation(),1);
            Vector4 Distance = pPoint - currentPosition ;
            Transformation *= Matrix4.CreateTranslation(Distance.Xyz) * Matrix4.CreateRotationY(angle) * Matrix4.CreateTranslation(-Distance.Xyz) * Matrix4.CreateRotationY(-angle);
            //Vector4.Transform(Distance,Matrix4.CreateRotationX(angle));
            


        }
        /// <summary>
        /// sets the material properties
        /// </summary>
        /// <param name="pMaterial"></param>
        public void setMaterial(Material pMaterial)
        {
            m_Material = pMaterial;
        }
        /// <summary>
        /// sets the texture coordinate
        /// </summary>
        /// <param name="pTexture"></param>
        public void setTexture(Texture pTexture)
        {
            m_Texture = pTexture;
        }
        /// <summary>
        /// returns the geometry
        /// </summary>
        /// <returns></returns>
        public GeoHelper GetGeometry()
        {
            return Geometry;
        }
        /// <summary>
        /// binds the data to the shader
        /// </summary>
        /// <param name="ShaderID"></param>
        public virtual void BindData(int ShaderID)
        {
            Geometry.BindBuffer();
            Geometry.GenerateArrayBuffers();
            if (m_Texture != null);
            m_Texture.BindData();
        }
        /// <summary>
        /// allows the model to be updated
        /// </summary>
        public virtual void Update()
        {

        }
    }
}
