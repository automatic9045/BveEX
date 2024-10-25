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
 12;
 -7.250000;2.850000;0.000000;,
 -7.250000;1.100000;0.000000;,
 -7.199999;2.800000;0.000000;,
 -7.199999;1.100000;0.000000;,
 -7.199999;2.800000;5.099999;,
 -7.199999;1.100000;5.099999;,
 -7.250000;2.850000;0.000000;,
 -7.250000;2.850000;5.099999;,
 -7.200000;1.100000;0.000000;,
 -7.200000;-0.500000;0.000000;,
 -7.200000;1.100000;5.200000;,
 -7.200000;-0.500000;5.200000;;
 10;
 3;0,2,3;,
 3;0,3,1;,
 3;2,4,5;,
 3;2,5,3;,
 3;3,5,4;,
 3;3,4,2;,
 3;6,7,4;,
 3;6,4,2;,
 3;8,9,11;,
 3;8,11,10;;

 MeshMaterialList  {
  2;
  10;
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  1,
  1;

  Material  {
   1.000000;1.000000;1.000000;1.000000;;
   0.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;

   TextureFilename  {
    "Fence6.png";
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
  12;
  0.010000;0.000000;,
  0.010000;1.000000;,
  0.000000;0.030000;,
  0.000000;1.000000;,
  2.000000;0.030000;,
  2.000000;1.000000;,
  0.000000;0.000000;,
  2.000000;0.000000;,
  0.000000;0.000000;,
  0.000000;1.000000;,
  0.400000;0.000000;,
  0.400000;1.000000;;
 }

 MeshNormals  {
  10;
  0.000000;0.000000;-1.000000;,
  0.000000;0.000000;-1.000000;,
  1.000000;0.000000;0.000000;,
  1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  0.707103;0.707110;0.000000;,
  0.707103;0.707110;0.000000;,
  -1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;;
  10;
  3;0,0,0;,
  3;1,1,1;,
  3;2,2,2;,
  3;3,3,3;,
  3;4,4,4;,
  3;5,5,5;,
  3;6,6,6;,
  3;7,7,7;,
  3;8,8,8;,
  3;9,9,9;;
 }
}