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
 9;
 0.000000;-0.653000;0.000000;,
 -3.000000;-0.653000;0.000000;,
 -3.300000;-0.800000;0.000000;,
 -13.000000;-10.500000;0.000000;,
 0.000000;-0.653000;5.250000;,
 -3.000000;-0.653000;5.250000;,
 -3.300000;-0.800000;5.250000;,
 -13.000000;-10.500000;5.250000;,
 -3.600000;-10.500000;0.000000;;
 7;
 3;0,1,5;,
 3;0,5,4;,
 3;1,2,6;,
 3;1,6,5;,
 3;2,3,7;,
 3;2,7,6;,
 3;2,8,3;;

 MeshMaterialList  {
  1;
  7;
  0,
  0,
  0,
  0,
  0,
  0,
  0;

  Material  {
   1.000000;1.000000;1.000000;1.000000;;
   0.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;

   TextureFilename  {
    "Grass.png";
   }
  }
 }

 MeshTextureCoords  {
  9;
  0.333000;0.000000;,
  0.333000;0.175000;,
  0.333000;0.200000;,
  0.333000;1.500000;,
  0.000000;0.000000;,
  0.000000;0.175000;,
  0.000000;0.200000;,
  0.000000;1.500000;,
  1.500000;0.000000;;
 }

 MeshNormals  {
  7;
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  -0.440015;0.897990;0.000000;,
  -0.440015;0.897990;0.000000;,
  -0.707107;0.707107;0.000000;,
  -0.707107;0.707107;0.000000;,
  0.000000;0.000000;-1.000000;;
  7;
  3;0,0,0;,
  3;1,1,1;,
  3;2,2,2;,
  3;3,3,3;,
  3;4,4,4;,
  3;5,5,5;,
  3;6,6,6;;
 }
}