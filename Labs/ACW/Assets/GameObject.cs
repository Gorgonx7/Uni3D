﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;
namespace Labs.ACW.Assets
{
    public struct Material
    {
        Vector4 m_AmbientReflectivity;
        Vector4 m_DiffuseReflectivity;
        Vector4 m_SpecularReflectivity;
        float m_Shininess;
        //Change this to take vec3s? create a new constructor?
        public Material(Vector4 AmbientReflectivtiy, Vector4 DiffuseReflectivity, Vector4 SpecularReflectivity, float Shininess)
        {
            m_AmbientReflectivity = AmbientReflectivtiy;
            m_DiffuseReflectivity = DiffuseReflectivity;
            m_SpecularReflectivity = SpecularReflectivity;
            m_Shininess = Shininess * 128;
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
        }
    }
    abstract class GameObject
    {
        protected Matrix4 Transformation;
        protected GeoHelper Geometry;
        protected Material m_Material;
        public GameObject()
        {
            m_Material = new Material(new Vector4(0f, 0f, 0f, 1.0f), new Vector4(0.55f, 0.55f, 0.55f, 1.0f), new Vector4(0.70f, 0.70f, 0.70f, 1.0f), 0.25f);
        }
        public virtual void Draw(int ShaderID)
        {
            int vModelLocation = GL.GetUniformLocation(ShaderID, "uModel");
            GL.UniformMatrix4(vModelLocation, true, ref Transformation);
            m_Material.Bind(ShaderID);
        }
        public void Transform(Matrix4 pTransform)
        {
            Transformation *= Matrix4.CreateTranslation(-Transformation.ExtractTranslation()) * pTransform * Matrix4.CreateTranslation(Transformation.ExtractTranslation());
        }
        public GeoHelper GetGeometry()
        {
            return Geometry;
        }
        public virtual void BindData(int ShaderID)
        {
            Geometry.BindBuffer();
            Geometry.GenerateArrayBuffers();
        }
    }
}
