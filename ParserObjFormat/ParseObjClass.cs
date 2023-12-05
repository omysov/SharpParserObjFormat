using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ParseObjFormat
{
    public class ParseObjClass
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> norm = new List<Vector3>();
        List<Vector3> text_coor = new List<Vector3>();

        //result
        Dictionary<string, string> material_path = new Dictionary<string, string>();
        List<int> indexes_material_array = new List<int>();
        Dictionary<string, List<Vector3>> material_vectors_dict = new Dictionary<string, List<Vector3>>();

        float[] result_coor_with_normal = new float[10000000];
        int number_item_result = 0;
        public int polygon = 0;  
        public int triangle_polygon = 0;

        private List<string> lines = new List<string>();
        private List<string> lines_mtl = new List<string>();

        public ParseObjClass() { }

        public void ParseObj(string path_obj, string path_mtl)
        {
            StreamReader reader = new StreamReader(path_obj);
            string line;
            lines = new List<string>();
            while ((line = reader.ReadLine()) != null) { lines.Add(line); }

            //mtl
            StreamReader reader_mtl = new StreamReader(path_mtl);
            string line_mtl;
            lines_mtl = new List<string>();
            while ((line_mtl = reader_mtl.ReadLine()) != null) { lines_mtl.Add(line_mtl); }


            ParseMtlFile();

            ParseObjFile();
        }

        private void ParseMtlFile()
        {
            string[] nameMatWithPath = new string[2];
            List<Vector3> vectors_this_mat = new List<Vector3>();
            bool Kd_existe = false;

            foreach (string current_line_mtl in lines_mtl)
            {

                if (current_line_mtl.StartsWith("newmtl"))
                {
                    if (nameMatWithPath[0] != null)
                    {
                        material_path.Add(nameMatWithPath[0], "base");
                        nameMatWithPath = new string[2];

                        string current_material = string.Empty;
                        for (int ch = 7; ch < current_line_mtl.Length; ch++)
                        {
                            current_material += current_line_mtl[ch];
                        }

                        nameMatWithPath[0] = current_material;
                    }
                    else
                    {
                        string current_material = string.Empty;
                        for (int ch = 7; ch < current_line_mtl.Length; ch++)
                        {
                            current_material += current_line_mtl[ch];
                        }

                        nameMatWithPath[0] = current_material;
                    }
                };

                if (current_line_mtl.StartsWith("Ka"))
                {
                    string[] items = current_line_mtl.Split(' ');


                    vectors_this_mat.Add(new Vector3(
                            float.Parse(items[1], CultureInfo.InvariantCulture),
                            float.Parse(items[2], CultureInfo.InvariantCulture),
                            float.Parse(items[3], CultureInfo.InvariantCulture)));
                }

                if (current_line_mtl.StartsWith("Kd"))
                {
                    string[] items = current_line_mtl.Split(' ');

                    Kd_existe = true;
                    vectors_this_mat.Add(new Vector3(
                            float.Parse(items[1], CultureInfo.InvariantCulture),
                            float.Parse(items[2], CultureInfo.InvariantCulture),
                            float.Parse(items[3], CultureInfo.InvariantCulture)));
                }

                if (current_line_mtl.StartsWith("Ks"))
                {
                    string[] items = current_line_mtl.Split(' ');


                    vectors_this_mat.Add(new Vector3(
                            float.Parse(items[1], CultureInfo.InvariantCulture),
                            float.Parse(items[2], CultureInfo.InvariantCulture),
                            float.Parse(items[3], CultureInfo.InvariantCulture)));
                }

                if (current_line_mtl.StartsWith("Ke"))
                {
                    string[] items = current_line_mtl.Split(' ');


                    vectors_this_mat.Add(new Vector3(
                            float.Parse(items[1], CultureInfo.InvariantCulture),
                            float.Parse(items[2], CultureInfo.InvariantCulture),
                            float.Parse(items[3], CultureInfo.InvariantCulture)));
                }

                if (current_line_mtl.StartsWith("map_Kd"))
                {
                    string current_path = string.Empty;
                    for (int ch = 7; ch < current_line_mtl.Length; ch++)
                    {
                        current_path += current_line_mtl[ch];
                    }
                    nameMatWithPath[1] = current_path;
                    material_path.Add(nameMatWithPath[0], nameMatWithPath[1]);
                    material_vectors_dict.Add(nameMatWithPath[0], vectors_this_mat);

                    nameMatWithPath = new string[2];

                }
            }
        }

        private void ParseObjFile()
        {
            string this_material = string.Empty;
            //List<int> indexes_material_array = new List<int>();

            foreach (string current_line in lines)
            {
                //last string

                string[] items = current_line.Split(' ');

                if (items[0] != "v" && items[0] != "vn" && items[0] != "f" && items[0] != "vt" && items[0] != "usemtl")
                {
                    continue;
                }

                switch (items[0])
                {
                    case "v":
                        AddVertice(items);
                        break;
                    case "vn":
                        AddNormalVector(items);
                        break;
                    case "vt":
                        AddVerticesTexture(items);
                        break;
                    case "usemtl":
                        if (this_material != string.Empty)
                        {
                            this_material = items[1];
                            indexes_material_array.Add(number_item_result);
                        }
                        else
                        {
                            this_material = items[1];
                        }
                        break;
                    case "f":
                        AddFragmentCoor(items);
                        break;
                }

                if (current_line == lines.Last())
                {
                    indexes_material_array.Add(number_item_result);
                }
            }
        }

        private void AddVertice(string[] line)
        {
            vertices.Add(
                new Vector3(float.Parse(line[1], CultureInfo.InvariantCulture),
                            float.Parse(line[2], CultureInfo.InvariantCulture),
                            float.Parse(line[3], CultureInfo.InvariantCulture)
            ));
        }

        private void AddNormalVector(string[] line)
        {
            norm.Add(
                new Vector3(float.Parse(line[1], CultureInfo.InvariantCulture),
                            float.Parse(line[2], CultureInfo.InvariantCulture),
                            float.Parse(line[3], CultureInfo.InvariantCulture)
                ));
        }

        private void AddVerticesTexture(string[] line)
        {
            text_coor.Add(
                new Vector3(float.Parse(line[1], CultureInfo.InvariantCulture),
                            float.Parse(line[2], CultureInfo.InvariantCulture),
                            0
                ));
        }

        private void AddFragmentCoor(string[] line)
        {
            Vector3[][] massive_coor = new Vector3[100][];
            Vector2[] array_texture = new Vector2[100];
            int i = 0;

            foreach (string current_line in line)
            {

                if (current_line == "f" || current_line == "")
                {
                    continue;
                }

                string[] items = current_line.Split('/');

                int index_vert = int.Parse(items[0]);
                int index_texture = int.Parse(items[1]);
                int index_norm = int.Parse(items[2]);

                Vector3[] vectors_this =
                {
                    vertices[index_vert - 1],
                    norm[index_norm - 1],
                    text_coor[index_texture - 1],
                };


                massive_coor[i] = vectors_this;
                i++;


            }
            if(i == 4)
            {
                AddFloatCoordWithNorm(massive_coor);
                polygon++;
            }
            if(i == 3)
            {
                AddFloatCoorTriangle(massive_coor);
                polygon++;
            }

            if(i > 4)
            {
                AddFloatCoorPolygon(massive_coor);
            }


        }

        private void AddFloatCoordWithNorm(Vector3[][] massive_coor_frag)
        {
            //Vertice 1
            WriteVertice(massive_coor_frag[0][0]);
            //Normal 1
            WriteNormal(massive_coor_frag[0][1]);
            //texture 1
            WriteTexture(massive_coor_frag[0][2]);


            //Vertice 2
            WriteVertice(massive_coor_frag[1][0]);
            //Normal 2
            WriteNormal(massive_coor_frag[1][1]);
            //texture 2
            WriteTexture(massive_coor_frag[1][2]);


            //Vertice 3
            WriteVertice(massive_coor_frag[2][0]);
            //Normal 3
            WriteNormal(massive_coor_frag[2][1]);
            //texture 3
            WriteTexture(massive_coor_frag[2][2]);

            triangle_polygon++;

            //Vertice 3
            WriteVertice(massive_coor_frag[2][0]);
            //Normal 3
            WriteNormal(massive_coor_frag[2][1]);
            //texture 3
            WriteTexture(massive_coor_frag[2][2]);

            //Vertice 4
            WriteVertice(massive_coor_frag[3][0]);
            //Normal 4
            WriteNormal(massive_coor_frag[3][1]);
            //texture 4
            WriteTexture(massive_coor_frag[3][2]);

            //Vertice 1
            WriteVertice(massive_coor_frag[0][0]);
            //Normal 1
            WriteNormal(massive_coor_frag[0][1]);
            //texture 1
            WriteTexture(massive_coor_frag[0][2]);

            triangle_polygon++;

        }

        private void AddFloatCoorTriangle(Vector3[][] massive_coor_frag)
        {
            //Vertice 1
            WriteVertice(massive_coor_frag[0][0]);
            //Normal 1
            WriteNormal(massive_coor_frag[0][1]);
            //texute 1
            WriteTexture(massive_coor_frag[0][2]);

            //Vertice 2
            WriteVertice(massive_coor_frag[1][0]);
            //Normal 2
            WriteNormal(massive_coor_frag[1][1]);
            //texute 2
            WriteTexture(massive_coor_frag[1][2]);

            //Vertice 3
            WriteVertice(massive_coor_frag[2][0]);
            //Normal 3
            WriteNormal(massive_coor_frag[2][1]);
            //texute 3
            WriteTexture(massive_coor_frag[2][2]);

            triangle_polygon++;
        }

        private void AddFloatCoorPolygon(Vector3[][] massive_coor_frag)
        {
            int count_vertices = 0;
            Vector3 vector_normal_center = new Vector3();
            Vector3 vector_center = new Vector3();
            foreach (Vector3[] massive_coor_ in  massive_coor_frag)
            {
                if(massive_coor_ != null)
                {
                    count_vertices++;
                }
            }

            //coor center
            for(int j = 0; j < count_vertices; j++)
            {
                vector_center += massive_coor_frag[j][0] / count_vertices;
                vector_normal_center += massive_coor_frag[j][1] / count_vertices;
            }


            for(int j = 0;j < count_vertices - 1; j++)
            {
                //Vertice 1
                WriteVertice(massive_coor_frag[j][0]);
                //Normal 1
                WriteNormal(massive_coor_frag[j][1]);
                //texture 1
                WriteTexture(massive_coor_frag[j][2]);

                //Vertice 2
                WriteVertice(massive_coor_frag[j + 1][0]);
                //Normal 2
                WriteNormal(massive_coor_frag[j + 1][1]);
                //texture 1
                WriteTexture(massive_coor_frag[j + 1][2]);

                //Vertice center
                result_coor_with_normal[number_item_result] = vector_center.X;
                number_item_result++;

                result_coor_with_normal[number_item_result] = vector_center.Y;
                number_item_result++;

                result_coor_with_normal[number_item_result] = vector_center.Z;
                number_item_result++;

                //Normal center
                result_coor_with_normal[number_item_result] = vector_normal_center.X;
                number_item_result++;

                result_coor_with_normal[number_item_result] = vector_normal_center.Y;
                number_item_result++;

                result_coor_with_normal[number_item_result] = vector_normal_center.Z;
                number_item_result++;

                //Texute center
                WriteTexture(massive_coor_frag[0][2]);

                //polygon
                triangle_polygon++;
                polygon++;
            }

            //last element => last and first vertices
            {
                //Vertice 1
                WriteVertice(massive_coor_frag[0][0]);
                //Normal 1
                WriteNormal(massive_coor_frag[0][1]);
                //texture 1
                WriteTexture(massive_coor_frag[0][2]);


                //Vertice 2
                WriteVertice(massive_coor_frag[count_vertices - 1][0]);
                //Normal 2
                WriteNormal(massive_coor_frag[count_vertices - 1][1]);
                //texture 2
                WriteTexture(massive_coor_frag[count_vertices - 1][2]);

                //Vertice center
                result_coor_with_normal[number_item_result] = vector_center.X;
                number_item_result++;

                result_coor_with_normal[number_item_result] = vector_center.Y;
                number_item_result++;

                result_coor_with_normal[number_item_result] = vector_center.Z;
                number_item_result++;

                //Normal center
                result_coor_with_normal[number_item_result] = vector_normal_center.X;
                number_item_result++;

                result_coor_with_normal[number_item_result] = vector_normal_center.Y;
                number_item_result++;

                result_coor_with_normal[number_item_result] = vector_normal_center.Z;
                number_item_result++;

                //Texute center
                WriteTexture(massive_coor_frag[0][2]);

                //polygon
                triangle_polygon++;
                polygon++;
            }

        }

        //Utils function
        private void WriteVertice(Vector3 vector_vertice)
        {
            result_coor_with_normal[number_item_result] = vector_vertice.X;
            number_item_result++;

            result_coor_with_normal[number_item_result] = vector_vertice.Y;
            number_item_result++;

            result_coor_with_normal[number_item_result] = vector_vertice.Z;
            number_item_result++;
        }

        private void WriteNormal(Vector3 vector_normal)
        {
            result_coor_with_normal[number_item_result] = vector_normal.X;
            number_item_result++;

            result_coor_with_normal[number_item_result] = vector_normal.Y;
            number_item_result++;

            result_coor_with_normal[number_item_result] = vector_normal.Z;
            number_item_result++;
        }

        private void WriteTexture(Vector3 texture_coor)
        {
            result_coor_with_normal[number_item_result] = texture_coor.X;
            number_item_result++;

            result_coor_with_normal[number_item_result] = texture_coor.Y;
            number_item_result++;
        }

        public List<int> GetIndecesMaterialArray()
        {
            return indexes_material_array;
        }

        public Dictionary<string, string> GetDictionaryMaterialPath()
        {
            return material_path;
        }

        public float[] GetResultFloatArray()
        {
            return result_coor_with_normal;
        }

    }
}
