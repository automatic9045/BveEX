xof 0302txt 0064
template Header {
 <3D82AB43-62DA-11cf-AB39-0020AF71E433>
 WORD major;
 WORD minor;
 DWORD flags;
}

template Vector {
 <3D82AB5E-62DA-11cf-AB39-0020AF71E433>
 FLOAT x;
 FLOAT y;
 FLOAT z;
}

template Coords2d {
 <F6F23F44-7686-11cf-8F52-0040333594A3>
 FLOAT u;
 FLOAT v;
}

template Matrix4x4 {
 <F6F23F45-7686-11cf-8F52-0040333594A3>
 array FLOAT matrix[16];
}

template ColorRGBA {
 <35FF44E0-6C7C-11cf-8F52-0040333594A3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
 FLOAT alpha;
}

template ColorRGB {
 <D3E16E81-7835-11cf-8F52-0040333594A3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
}

template IndexedColor {
 <1630B820-7842-11cf-8F52-0040333594A3>
 DWORD index;
 ColorRGBA indexColor;
}

template Boolean {
 <4885AE61-78E8-11cf-8F52-0040333594A3>
 WORD truefalse;
}

template Boolean2d {
 <4885AE63-78E8-11cf-8F52-0040333594A3>
 Boolean u;
 Boolean v;
}

template MaterialWrap {
 <4885AE60-78E8-11cf-8F52-0040333594A3>
 Boolean u;
 Boolean v;
}

template TextureFilename {
 <A42790E1-7810-11cf-8F52-0040333594A3>
 STRING filename;
}

template Material {
 <3D82AB4D-62DA-11cf-AB39-0020AF71E433>
 ColorRGBA faceColor;
 FLOAT power;
 ColorRGB specularColor;
 ColorRGB emissiveColor;
 [...]
}

template MeshFace {
 <3D82AB5F-62DA-11cf-AB39-0020AF71E433>
 DWORD nFaceVertexIndices;
 array DWORD faceVertexIndices[nFaceVertexIndices];
}

template MeshFaceWraps {
 <4885AE62-78E8-11cf-8F52-0040333594A3>
 DWORD nFaceWrapValues;
 Boolean2d faceWrapValues;
}

template MeshTextureCoords {
 <F6F23F40-7686-11cf-8F52-0040333594A3>
 DWORD nTextureCoords;
 array Coords2d textureCoords[nTextureCoords];
}

template MeshMaterialList {
 <F6F23F42-7686-11cf-8F52-0040333594A3>
 DWORD nMaterials;
 DWORD nFaceIndexes;
 array DWORD faceIndexes[nFaceIndexes];
 [Material]
}

template MeshNormals {
 <F6F23F43-7686-11cf-8F52-0040333594A3>
 DWORD nNormals;
 array Vector normals[nNormals];
 DWORD nFaceNormals;
 array MeshFace faceNormals[nFaceNormals];
}

template MeshVertexColors {
 <1630B821-7842-11cf-8F52-0040333594A3>
 DWORD nVertexColors;
 array IndexedColor vertexColors[nVertexColors];
}

template Mesh {
 <3D82AB44-62DA-11cf-AB39-0020AF71E433>
 DWORD nVertices;
 array Vector vertices[nVertices];
 DWORD nFaces;
 array MeshFace faces[nFaces];
 [...]
}

Header{
1;
0;
1;
}

Mesh {
 36;
 0.06000;0.00000;0.05000;,
 0.00000;0.00000;0.05000;,
 0.00000;0.00000;0.00000;,
 0.06000;0.00000;0.00000;,
 0.00000;1.47052;0.05000;,
 0.06000;1.47052;0.05000;,
 0.06000;1.47052;0.00000;,
 0.00000;1.47052;0.00000;,
 0.06000;1.16052;0.05000;,
 0.06000;1.16052;0.00000;,
 0.06000;1.47052;0.00000;,
 0.06000;1.47052;0.05000;,
 0.00000;1.16052;0.05000;,
 0.00000;1.47052;0.05000;,
 0.00000;1.47052;0.00000;,
 0.00000;1.16052;0.00000;,
 0.06000;0.91052;0.05000;,
 0.06000;0.91052;0.00000;,
 0.06000;1.16052;0.00000;,
 0.06000;1.16052;0.05000;,
 0.00000;0.91052;0.00000;,
 0.00000;0.91052;0.05000;,
 0.00000;1.16052;0.05000;,
 0.00000;1.16052;0.00000;,
 0.27217;1.22158;-0.00629;,
 0.27217;1.73060;-0.00629;,
 0.27217;1.73060;-0.00024;,
 0.27217;1.22158;-0.00024;,
 -0.20967;1.22158;-0.00629;,
 -0.20967;1.22158;-0.00024;,
 -0.20967;1.73060;-0.00629;,
 -0.20967;1.73060;-0.00024;,
 0.27217;1.22158;-0.00024;,
 0.27217;1.73060;-0.00024;,
 -0.20967;1.73060;-0.00024;,
 -0.20967;1.22158;-0.00024;;
 
 20;
 4;0,1,2,3;,
 4;4,5,6,7;,
 4;8,9,10,11;,
 4;12,13,14,15;,
 4;8,11,13,12;,
 4;9,15,14,10;,
 4;16,0,3,17;,
 4;16,17,18,19;,
 4;20,2,1,21;,
 4;20,21,22,23;,
 4;21,1,0,16;,
 4;21,16,19,22;,
 4;17,3,2,20;,
 4;17,20,23,18;,
 4;24,25,26,27;,
 4;28,24,27,29;,
 4;30,28,29,31;,
 4;25,30,31,26;,
 4;25,24,28,30;,
 4;32,33,34,35;;
 
 MeshMaterialList {
  5;
  20;
  3,
  0,
  1,
  1,
  1,
  1,
  3,
  2,
  3,
  2,
  3,
  2,
  3,
  2,
  4,
  4,
  4,
  4,
  4,
  0;;
  Material {
   0.800000;0.800000;0.800000;1.000000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;
   TextureFilename {
    "stop-mat1.png";
   }
  }
  Material {
   0.800000;0.800000;0.800000;1.000000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;
   TextureFilename {
    "stop-mat1.png";
   }
  }
  Material {
   0.800000;0.800000;0.800000;1.000000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;
   TextureFilename {
    "stop-mat2.png";
   }
  }
  Material {
   0.800000;0.800000;0.800000;1.000000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;
   TextureFilename {
    "stop-mat3.png";
   }
  }
  Material {
   0.800000;0.800000;0.800000;1.000000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;
   TextureFilename {
    "stop-V.png";
   }
  }
 }
 MeshTextureCoords {
  36;
  -40.500000;12.333334;,
  -39.500000;12.333334;,
  -39.500000;12.333334;,
  -40.500000;12.333334;,
  0.564854;0.510957;,
  0.440330;0.510957;,
  0.440330;0.510957;,
  0.564854;0.510957;,
  -40.500000;14.505326;,
  -40.500000;14.505326;,
  -40.500000;12.454519;,
  -40.500000;12.454519;,
  -39.500000;14.505326;,
  -39.500000;12.454519;,
  -39.500000;12.454519;,
  -39.500000;14.505326;,
  -40.500000;9.298284;,
  -40.500000;9.298284;,
  -40.500000;8.464951;,
  -40.500000;8.464951;,
  -39.500000;9.298284;,
  -39.500000;9.298284;,
  -39.500000;8.464951;,
  -39.500000;8.464951;,
  1.000000;-0.001244;,
  1.000000;-1.001244;,
  1.000000;-1.001244;,
  1.000000;-0.001244;,
  0.000000;-0.001244;,
  0.000000;-0.001244;,
  0.000000;-1.001244;,
  0.000000;-1.001244;,
  0.000000;1.000000;,
  0.000000;0.000000;,
  1.000000;0.000000;,
  1.000000;1.000000;;
 }
}
