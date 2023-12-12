# ParserObjFormat
```C#
ParseObjClass parse = new ParseObjClass();
parse.ParseObj("model.obj", "model.mtl");

List<int> array_material_indices = parse.GetIndicesMaterialArray();
Dictionary<string, string> dict_material = parse.GetDictionaryMaterialPath();
Dictionary<string, Dictionary<string, Vector3>> material_vectors = GetDictionaryMaterialVectors3();
float[] vertices = parse.GetResultFloatArray();
count_triangle_polygons_object = parse.triangle_polygon;
```
```C#
texture_1 = Texture.LoadFromFile(dict_material["Material"]);
texture_2 = Texture.LoadFromFile(dict_material["Material_2"]);
```
```C#
//System.Numeric format vector
Dictionary<string, Dictionary<string, Vector3>> material_vectors = parse.GetDictionaryMaterialVectors3();
Vector3 ka_vector = material_vectores_dict["Material"]["Ka"];
```

```C#
texture_1.Use(TextureUnit.Texture0);
_lightingShader.Use();
float[] vertices_1 = vertices[..array_material_indices[0]];
GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
GL.BufferData(BufferTarget.ArrayBuffer, vertices_1.Length * sizeof(float), vertices_1, BufferUsageHint.DynamicDraw);
GL.BindVertexArray(_vaoModel);
GL.DrawArrays(PrimitiveType.Triangles, 0, count_triangle_polygons_object * 3); 

texture_2.Use(TextureUnit.Texture0);
_lightingShader.Use();
float[] vertices_2 = vertices[array_material_indices[0]..array_material_indices[1]];
GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
GL.BufferData(BufferTarget.ArrayBuffer, vertices_2.Length * sizeof(float), vertices_2, BufferUsageHint.DynamicDraw);
GL.BindVertexArray(_vaoModel);
GL.DrawArrays(PrimitiveType.Triangles, 0, count_triangle_polygons_object * 3); 
```

# Vertices array format
![parse_obj](https://github.com/omysov/ParserObjFormat/assets/97920323/e826c443-7696-4f90-b93d-08c0afdac6e2)

