xof 0303txt 0032
template Vector {
 <3d82ab5e-62da-11cf-ab39-0020af71e433>
 FLOAT x;
 FLOAT y;
 FLOAT z;
}

template MeshFace {
 <3d82ab5f-62da-11cf-ab39-0020af71e433>
 DWORD nFaceVertexIndices;
 array DWORD faceVertexIndices[nFaceVertexIndices];
}

template Mesh {
 <3d82ab44-62da-11cf-ab39-0020af71e433>
 DWORD nVertices;
 array Vector vertices[nVertices];
 DWORD nFaces;
 array MeshFace faces[nFaces];
 [...]
}

template ColorRGBA {
 <35ff44e0-6c7c-11cf-8f52-0040333594a3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
 FLOAT alpha;
}

template ColorRGB {
 <d3e16e81-7835-11cf-8f52-0040333594a3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
}

template Material {
 <3d82ab4d-62da-11cf-ab39-0020af71e433>
 ColorRGBA faceColor;
 FLOAT power;
 ColorRGB specularColor;
 ColorRGB emissiveColor;
 [...]
}

template MeshMaterialList {
 <f6f23f42-7686-11cf-8f52-0040333594a3>
 DWORD nMaterials;
 DWORD nFaceIndexes;
 array DWORD faceIndexes[nFaceIndexes];
 [Material <3d82ab4d-62da-11cf-ab39-0020af71e433>]
}

template TextureFilename {
 <a42790e1-7810-11cf-8f52-0040333594a3>
 STRING filename;
}

template Coords2d {
 <f6f23f44-7686-11cf-8f52-0040333594a3>
 FLOAT u;
 FLOAT v;
}

template MeshTextureCoords {
 <f6f23f40-7686-11cf-8f52-0040333594a3>
 DWORD nTextureCoords;
 array Coords2d textureCoords[nTextureCoords];
}

template MeshNormals {
 <f6f23f43-7686-11cf-8f52-0040333594a3>
 DWORD nNormals;
 array Vector normals[nNormals];
 DWORD nFaceNormals;
 array MeshFace faceNormals[nFaceNormals];
}


Mesh  {
 18;
 -2.700000;1.100000;0.000000;,
 -1.500000;1.100000;0.000000;,
 -2.700000;1.100000;5.100000;,
 -1.500000;1.100000;5.100000;,
 -1.500000;1.100000;0.000000;,
 -1.500000;0.900000;0.000000;,
 -1.900000;0.700000;0.000000;,
 -1.900000;-0.500000;0.000000;,
 -1.500000;1.100000;5.200000;,
 -1.500000;0.900000;5.200000;,
 -1.900000;0.700000;5.200000;,
 -1.900000;-0.500000;5.200000;,
 -2.700000;1.100000;0.000000;,
 -1.500000;1.100000;0.000000;,
 -1.500000;0.900000;0.000000;,
 -1.900000;0.700000;0.000000;,
 -1.900000;-0.500000;0.000000;,
 -2.700000;-0.500000;0.000000;;
 12;
 3;0,2,3;,
 3;0,3,1;,
 3;4,8,9;,
 3;4,9,5;,
 3;5,9,10;,
 3;5,10,6;,
 3;6,10,11;,
 3;6,11,7;,
 3;12,13,14;,
 3;12,14,15;,
 3;12,15,16;,
 3;12,16,17;;

 MeshMaterialList  {
  3;
  12;
  0,
  0,
  1,
  1,
  1,
  1,
  1,
  1,
  2,
  2,
  2,
  2;

  Material  {
   1.000000;1.000000;1.000000;1.000000;;
   0.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;

   TextureFilename  {
    "FormTop5.png";
   }
  }

  Material  {
   1.000000;1.000000;1.000000;1.000000;;
   0.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;

   TextureFilename  {
    "FormSide1.png";
   }
  }

  Material  {
   1.000000;1.000000;1.000000;1.000000;;
   0.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;

   TextureFilename  {
    "Wall.png";
   }
  }
 }

 MeshTextureCoords  {
  18;
  1.000000;1.000000;,
  0.000000;1.000000;,
  1.000000;0.000000;,
  0.000000;0.000000;,
  0.000000;0.000000;,
  0.000000;0.149000;,
  0.000000;0.336000;,
  0.000000;1.000000;,
  0.400000;0.000000;,
  0.400000;0.149000;,
  0.400000;0.336000;,
  0.400000;1.000000;,
  0.000000;0.000000;,
  0.166700;0.000000;,
  0.166700;0.125000;,
  0.111100;0.250000;,
  0.111100;1.000000;,
  0.000000;1.000000;;
 }

 MeshNormals  {
  12;
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  1.000000;0.000000;0.000000;,
  1.000000;0.000000;0.000000;,
  0.447214;-0.894427;0.000000;,
  0.447214;-0.894427;0.000000;,
  1.000000;0.000000;0.000000;,
  1.000000;0.000000;0.000000;,
  0.000000;0.000000;-1.000000;,
  0.000000;0.000000;-1.000000;,
  0.000000;0.000000;-1.000000;,
  0.000000;0.000000;-1.000000;;
  12;
  3;0,0,0;,
  3;1,1,1;,
  3;2,2,2;,
  3;3,3,3;,
  3;4,4,4;,
  3;5,5,5;,
  3;6,6,6;,
  3;7,7,7;,
  3;8,8,8;,
  3;9,9,9;,
  3;10,10,10;,
  3;11,11,11;;
 }
}