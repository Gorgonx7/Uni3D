using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
namespace Labs.Utility
{
    /// <summary>
    /// Model Utility reads a very simple, inefficient file format that uses 
    /// Triangles only. This is not good practice, but it does the job :)
    /// </summary>
    public class ModelUtility
    {
        public float[] Vertices { get; private set; }
        public uint[] Indices { get; private set; }
        public float[] verts { get; private set; }
        public float[] noms { get; private set; }
        public float[] tex { get; private set; }

        private ModelUtility() { }

        private static ModelUtility LoadFromBIN(string pModelFile)
        {
            ModelUtility model = new ModelUtility();
            BinaryReader reader = new BinaryReader(new FileStream(pModelFile, FileMode.Open));

            int numberOfVertices = reader.ReadInt32();
            int floatsPerVertex = 6;

            model.Vertices = new float[numberOfVertices * floatsPerVertex];

            byte[]  byteArray = new byte[model.Vertices.Length * sizeof(float)];
            byteArray = reader.ReadBytes(byteArray.Length);

            Buffer.BlockCopy(byteArray, 0, model.Vertices, 0, byteArray.Length);

            int numberOfTriangles = reader.ReadInt32();

            model.Indices = new uint[numberOfTriangles * 3];
            
            byteArray = new byte[model.Indices.Length * sizeof(int)];
            byteArray = reader.ReadBytes(model.Indices.Length * sizeof(int));
            Buffer.BlockCopy(byteArray, 0, model.Indices, 0, byteArray.Length);

            reader.Close();
            return model;
        }     
        private static ModelUtility LoadFromObj(string pModelFile)
        {
            ModelUtility modelUtility = new ModelUtility();
            StreamReader reader = new StreamReader(pModelFile);
            List<float> vertexHolder = new List<float>();
            List<Vector2> TextureHolder = new List<Vector2>();
            List<float> NormalHolder = new List<float>();
            List<uint> VertexIndices = new List<uint>();
            List<uint> NormalIndices = new List<uint>();
            List<uint> TextureIndices = new List<uint>();
            List<Vector3> Hvertex = new List<Vector3>();
            List<Vector3> Hnormals = new List<Vector3>();
            List<Vector2> HTextures = new List<Vector2>();
            while (!reader.EndOfStream)
            {
                string LineHeader = "";
                int holder = reader.Read();
                char CharacterHolder = (char)holder;
                
                do
                {
                    LineHeader += CharacterHolder;
                    holder = reader.Read();
                    if(holder == -1)
                    {
                        break;
                    }
                    CharacterHolder = (char)holder;
                    if(CharacterHolder == '\n' || CharacterHolder == '\r' || CharacterHolder == ' ' || CharacterHolder == -1)
                    {
                        break;
                    }

                } while (true);

                switch (LineHeader.Trim())
                {
                    case "v":
                        
                            string LineHolder = reader.ReadLine().Trim();
                            string[] VertexHolder = LineHolder.Split(' ');
                        Vector3 vertex = new Vector3(0,0,0);
                        for (int x = 0; x < VertexHolder.Length; x++)
                            {
                            if (VertexHolder[x] != "")
                            {
                                vertexHolder.Add(float.Parse(VertexHolder[x]));
                                vertex[x] = float.Parse(VertexHolder[x]);
                            }
                            }
                        Hvertex.Add(vertex);
                        
                        break;
                    case "vt":
                        string Holder = reader.ReadLine();
                        string[] TextureVertexHolder = Holder.Split(' ');
                        Vector2 texturec = new Vector2(float.Parse(TextureVertexHolder[0]), float.Parse(TextureVertexHolder[1]));
                        TextureHolder.Add(texturec);
                        //HTextures.Add(texturec);

                        break;
                    case "vn":
                        string NormalLineHolder = reader.ReadLine().Trim();
                        string[] NormalVertexHolder = NormalLineHolder.Split(' ');
                        Vector3 norm = new Vector3(0, 0, 0);
                        for (int x = 0; x < NormalVertexHolder.Length; x++)
                        {
                            NormalHolder.Add(float.Parse(NormalVertexHolder[x]));
                            norm[x] = float.Parse(NormalVertexHolder[x]);
                        }
                        Hnormals.Add(norm);
                        break;
                    case "f":
                        string FaceLineHolder = reader.ReadLine();
                        string[] FaceHolder = FaceLineHolder.Split(' ');
                        for(int x = 0; x < FaceHolder.Length; x++)
                        {
                            string[] FaceVertexHolder = FaceHolder[x].Split('/');
                            if (FaceHolder[x] != "")
                            {
                                VertexIndices.Add(uint.Parse(FaceVertexHolder[0]));
                                TextureIndices.Add(uint.Parse(FaceVertexHolder[1]));
                                if (FaceVertexHolder.Length > 2)
                                    NormalIndices.Add(uint.Parse(FaceVertexHolder[2]));
                            }
                            // we have the indices now what do I do? how can I convert this into the drawing code correctly
                        }
                        break;
                        
                }
            }
            float[][] DataHolder = new float[VertexIndices.Count][];
            for (int x = 0; x < VertexIndices.Count; x++)
            {

                float[] VertexListHolder = new float[8];
                Vector3 vertex = new Vector3(0,0,0);
                VertexListHolder[0] = Hvertex[(int)VertexIndices[x] - 1].X;
                VertexListHolder[1] = Hvertex[(int)VertexIndices[x] - 1].Y;
                VertexListHolder[2] = Hvertex[(int)VertexIndices[x] - 1].Z;
                Hvertex.Add(vertex);
                HTextures.Add(TextureHolder[(int)TextureIndices[x] - 1]);
                VertexListHolder[3] = TextureHolder[(int)TextureIndices[x] - 1].X;
                VertexListHolder[4] = TextureHolder[(int)TextureIndices[x] - 1].Y;
                VertexListHolder[5] = Hnormals[(int)NormalIndices[x] - 1].X;
                VertexListHolder[6] = Hnormals[(int)NormalIndices[x] - 1].Y;
                VertexListHolder[7] = Hnormals[(int)NormalIndices[x] - 1].Z;
                DataHolder[x] = VertexListHolder;
            }
            float[] verts = new float[DataHolder[0].Length * DataHolder.Length];
            List<uint> IndicesHolder = new List<uint>();
            List<float[]> ProcessedVertex = new List<float[]>();
            for (int x = 0; x < DataHolder.Length; x++)
            {
               // if (!ProcessedVertex.Contains(DataHolder[x]))
               // {
                    // add it to the list
                 //   ProcessedVertex.Add(DataHolder[x]);
                    for (int y = 0; y < DataHolder[x].Length; y++)
                    {
                        verts[DataHolder[x].Length * x + y] = DataHolder[x][y];
                    }
                    IndicesHolder.Add((uint)x);
               // }
                //else
               // {
                    // index
                //    IndicesHolder.Add((uint)ProcessedVertex.LastIndexOf(DataHolder[x]));

               // }
            }
            float[] HHV = new float[Hvertex.Count * 3];
            for (int x = 0; x < Hvertex.Count; x++)
            {

                HHV[x] = Hvertex[x].X;
                HHV[x + 1] = Hvertex[x].Y;
                HHV[x + 2] = Hvertex[x].Z;
            }
            float[] HHN = new float[Hnormals.Count * 3];
            for (int x = 0; x < Hnormals.Count; x++)
            {

                HHN[x] = Hnormals[x].X;
                HHN[x + 1] = Hnormals[x].Y;
                HHN[x + 2] = Hnormals[x].Z;
            }
            float[] HHT = new float[HTextures.Count * 2];
            for(int x = 0; x < HTextures.Count; x++)
            {
                HHT[x] = HTextures[x].X;
                HHT[x + 1] = HTextures[x].Y; 
            }
            modelUtility.Indices = IndicesHolder.ToArray();
            modelUtility.Vertices = verts;
            modelUtility.verts = HHV;
            modelUtility.noms = HHN;
            modelUtility.tex = HHT;
            return modelUtility;
        }
        private static ModelUtility LoadFromSJG(string pModelFile)
        {
            ModelUtility model = new ModelUtility();
            StreamReader reader;
            reader = new StreamReader(pModelFile);
            string line = reader.ReadLine(); // vertex format
            int numberOfVertices = 0;
            int floatsPerVertex = 6;
            if (!int.TryParse(reader.ReadLine(), out numberOfVertices))
            {
                throw new Exception("Error when reading number of vertices in model file " + pModelFile);
            }

            model.Vertices = new float[numberOfVertices * floatsPerVertex];

            string[] values;
            for (int i = 0; i < model.Vertices.Length; )
            {
                line = reader.ReadLine();
                values = line.Split(',');
                foreach(string s in values)
                {
                    if (!float.TryParse(s, out model.Vertices[i]))
                    {
                        throw new Exception("Error when reading vertices in model file " + pModelFile + " " + s + " is not a valid number");
                    }
                    ++i;
                }
            }

            reader.ReadLine();
            int numberOfTriangles = 0;
            line = reader.ReadLine();
            if (!int.TryParse(line, out numberOfTriangles))
            {
                throw new Exception("Error when reading number of triangles in model file " + pModelFile);
            }

            model.Indices = new uint[numberOfTriangles * 3];

            for(int i = 0; i < numberOfTriangles * 3;)
            {
                line = reader.ReadLine();
                values = line.Split(',');
                foreach(string s in values)
                {
                    if (!uint.TryParse(s, out model.Indices[i]))
                    {
                        throw new Exception("Error when reading indices in model file " + pModelFile + " " + s + " is not a valid index");
                    }
                    ++i;
                }
            }

            reader.Close();
            return model;
        }

        public static ModelUtility LoadModel(string pModelFile)
        {
            string extension = pModelFile.Substring(pModelFile.IndexOf('.'));

            if (extension == ".sjg")
            {
                return LoadFromSJG(pModelFile);
            }
            else if (extension == ".bin")
            {
                return LoadFromBIN(pModelFile);
            } else if(extension == ".obj")
            {
                return LoadFromObj(pModelFile);
            }
            else
            {
                throw new Exception("Unknown file extension " + extension);
            }
        }

    }
}
