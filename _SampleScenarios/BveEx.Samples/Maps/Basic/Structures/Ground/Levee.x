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


Mesh  {
 20;
 -450.00000;-3.000000;24.100000;,
 450.00000;-3.000000;24.100000;,
 450.00000;-3.000000;0.000000;,
 -450.00000;-3.000000;0.000000;,
 -450.00000;-3.100000;0.000000;,
 -450.00000;-6.000000;25.000000;,
 450.00000;-6.000000;25.000000;,
 450.00000;-3.100000;0.000000;,
 -450.00000;0.000000;25.000000;,
 -50.000000;0.000000;25.000000;,
 50.000000;0.000000;25.000000;,
 450.00000;0.000000;25.000000;,
 -450.00000;-3.000000;24.100000;,
 -50.000000;-3.000000;24.100000;,
 50.000000;-3.000000;24.100000;,
 450.00000;-3.000000;24.100000;,
 -450.00000;-6.000000;25.000000;,
 -50.000000;-6.000000;25.000000;,
 50.000000;-6.000000;25.000000;,
 450.00000;-6.000000;25.000000;;
 8;
 4;0,1,2,3;,
 4;4,5,6,7;,
 4;8,9,13,12;,
 4;9,10,14,13;,
 4;10,11,15,14;,
 4;12,13,17,16;,
 4;13,14,18,17;,
 4;14,15,19,18;;

 MeshTextureCoords  {
  20;
  0.000000;0.000000;,
  0.000000;0.000000;,
  0.000000;0.000000;,
  0.000000;0.000000;,
  0.000000;1.000000;,
  0.000000;0.000000;,
  10.000000;0.000000;,
  10.000000;1.000000;,
  0.000000;0.000000;,
  2.000000;0.000000;,
  18.000000;0.000000;,
  20.000000;0.000000;,
  0.000000;1.000000;,
  2.000000;1.000000;,
  18.000000;1.000000;,
  20.000000;1.000000;,
  0.000000;0.000000;,
  2.000000;0.000000;,
  18.000000;0.000000;,
  20.000000;0.000000;;
 }

 MeshNormals  {
  8;
  0.000000;1.000000;0.000000;,
  0.000000;0.993339;0.115227;,
  0.000000;0.287348;-0.957826;,
  0.000000;0.287348;-0.957826;,
  0.000000;0.287348;-0.957826;,
  0.000000;-0.287348;-0.957826;,
  0.000000;-0.287348;-0.957826;,
  0.000000;-0.287348;-0.957826;;
  8;
  4;0,0,0,0;,
  4;1,1,1,1;,
  4;2,2,2,2;,
  4;3,3,3,3;,
  4;4,4,4,4;,
  4;5,5,5,5;,
  4;6,6,6,6;,
  4;7,7,7,7;;
 }

 MeshMaterialList  {
  3;
  8;
  0,
  1,
  2,
  2,
  2,
  2,
  2,
  2;

  Material  {
   0.125490;0.250980;0.188235;0.250980;;
   0.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;
  }

  Material  {
   1.000000;1.000000;1.000000;1.000000;;
   0.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;

   TextureFilename  {
    "River.png";
   }
  }

  Material  {
   1.000000;1.000000;1.000000;1.000000;;
   0.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;

   TextureFilename  {
    "ConcreteTileWall.png";
   }
  }
 }
}