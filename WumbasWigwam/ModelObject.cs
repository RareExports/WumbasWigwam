// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.ModelObject
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WumbasWigwam
{
  public class ModelObject
  {
    private float Scale = 1f;
    private List<Vertex> VertexBuffer = new List<Vertex>();
    private List<Triangle> Triangles = new List<Triangle>();
    private List<Texture> Textures = new List<Texture>();
    public List<WumbasWigwam.TextureData> TextureData = new List<WumbasWigwam.TextureData>();
    public uint vboVertexHandle;
    public uint vboColorHandle;
    public uint vboTexCoordHandle;
    public List<uint> iboHandles = new List<uint>();
    public List<ushort[]> iboData = new List<ushort[]>();
    public short[] seqVBO = new short[1];
    public List<uint> seqIBOHandles = new List<uint>();
    public List<ushort[]> seqIBOData = new List<ushort[]>();
    public uint seqLineHandle;
    public ushort[] seqLineData = new ushort[1];
    public short[] vbo = new short[1];
    public byte[] cbo = new byte[1];
    public float[] uvBuffer = new float[1];
    public List<int> TextureBuffer = new List<int>();
    private List<Bone> skeleton = new List<Bone>();

    public ModelObject()
    {
      this.vboVertexHandle = Core.GenBuffer();
      this.vboColorHandle = Core.GenBuffer();
      this.vboTexCoordHandle = Core.GenBuffer();
    }

    public ModelObject(
      List<Vertex> verts,
      List<Triangle> triangles,
      List<WumbasWigwam.TextureData> textureData,
      List<Texture> textures)
    {
      this.vboVertexHandle = Core.GenBuffer();
      this.vboColorHandle = Core.GenBuffer();
      this.vboTexCoordHandle = Core.GenBuffer();
      this.VertexBuffer = verts;
      this.Triangles = triangles;
      this.Textures = textures;
      this.TextureData = textureData;
      foreach (Texture texture in this.Textures)
        this.GenerateTexture(texture, false);
      this.GenerateBuffers();
    }

    public void SetSkeleton(List<Bone> skeleton_) => this.skeleton = skeleton_;

    public ModelObject(
      float[] vertexData,
      byte[] vertexColorData,
      float[] vertexTextureCoordData,
      List<ushort[]> iboData,
      List<WumbasWigwam.TextureData> textureData,
      List<Texture> textures,
      List<int> TextureSettingsBuffer,
      uint vboVertexHandle,
      uint vboColorHandle,
      uint vboTexCoordHandle,
      List<uint> iboHandles)
    {
      this.vboVertexHandle = vboVertexHandle;
      this.vboColorHandle = vboColorHandle;
      this.vboTexCoordHandle = vboTexCoordHandle;
      this.iboHandles = iboHandles;
      this.iboData = iboData;
      this.TextureBuffer = TextureSettingsBuffer;
      int index1 = 0;
      int index2 = 0;
      int index3 = 0;
      while (index1 < vertexData.Length)
      {
        this.VertexBuffer.Add(new Vertex((short) vertexData[index1], (short) vertexData[index1 + 1], (short) vertexData[index1 + 2], vertexColorData[index2], vertexColorData[index2 + 1], vertexColorData[index2 + 2], vertexColorData[index2 + 3], vertexTextureCoordData[index3], vertexTextureCoordData[index3 + 1]));
        index1 += 3;
        index2 += 4;
        index3 += 2;
      }
      this.Textures = textures;
      this.TextureData = textureData;
      for (int index4 = 0; index4 < iboData.Count; ++index4)
      {
        for (int index5 = 0; index5 < iboData[index4].Length; index5 += 3)
          this.Triangles.Add(new Triangle(new ushort[3]
          {
            iboData[index4][index5],
            iboData[index4][index5 + 1],
            iboData[index4][index5 + 2]
          }, TextureSettingsBuffer[index4]));
      }
      this.GenerateBuffers();
    }

    public List<int> GetAllAlphaTriangles()
    {
      List<int> intList = new List<int>();
      for (int index = 0; index < this.Triangles.Count; ++index)
      {
        Triangle triangle = this.Triangles[index];
        if (this.VertexBuffer[(int) triangle.verts[0]].a != byte.MaxValue || this.VertexBuffer[(int) triangle.verts[1]].a != byte.MaxValue || this.VertexBuffer[(int) triangle.verts[2]].a != byte.MaxValue || this.Textures[triangle.textureSetting].ISALPHA)
          intList.Add(index);
      }
      return intList;
    }

    public void SortTrianglesForBKModelExport()
    {
      IEnumerable<IGrouping<int, Triangle>> groupedTris1 = this.Triangles.Where<Triangle>((Func<Triangle, bool>) (triangle => this.VertexBuffer[(int) triangle.verts[0]].a == byte.MaxValue && this.VertexBuffer[(int) triangle.verts[1]].a == byte.MaxValue && this.VertexBuffer[(int) triangle.verts[2]].a == byte.MaxValue && !this.Textures[triangle.textureSetting].ISALPHA)).GroupBy<Triangle, int>((Func<Triangle, int>) (x => x.textureSetting));
      IEnumerable<IGrouping<int, Triangle>> groupedTris2 = this.Triangles.Where<Triangle>((Func<Triangle, bool>) (triangle => (this.VertexBuffer[(int) triangle.verts[0]].a != byte.MaxValue || this.VertexBuffer[(int) triangle.verts[1]].a != byte.MaxValue || this.VertexBuffer[(int) triangle.verts[2]].a != byte.MaxValue) && !this.Textures[triangle.textureSetting].ISALPHA)).GroupBy<Triangle, int>((Func<Triangle, int>) (x => x.textureSetting));
      IEnumerable<IGrouping<int, Triangle>> groupedTris3 = this.Triangles.Where<Triangle>((Func<Triangle, bool>) (triangle => this.Textures[triangle.textureSetting].ISALPHA)).GroupBy<Triangle, int>((Func<Triangle, int>) (x => x.textureSetting));
      List<Triangle> triangleList = new List<Triangle>();
      triangleList.AddRange((IEnumerable<Triangle>) this.ArrangeCullModes(groupedTris1));
      triangleList.AddRange((IEnumerable<Triangle>) this.ArrangeCullModes(groupedTris3));
      triangleList.AddRange((IEnumerable<Triangle>) this.ArrangeCullModes(groupedTris2));
      this.Triangles = triangleList;
    }

    private List<Triangle> ArrangeCullModes(
      IEnumerable<IGrouping<int, Triangle>> groupedTris)
    {
      List<Triangle> triangleList1 = new List<Triangle>();
      List<Triangle> triangleList2 = new List<Triangle>();
      List<Triangle> triangleList3 = new List<Triangle>();
      List<Triangle> triangleList4 = new List<Triangle>();
      foreach (IGrouping<int, Triangle> groupedTri in groupedTris)
      {
        Texture texture = this.Textures[groupedTri.First<Triangle>().textureSetting];
        if (texture.getCullMode() == (byte) 2)
          triangleList1.AddRange((IEnumerable<Triangle>) groupedTri);
        if (texture.getCullMode() == (byte) 34)
          triangleList2.AddRange((IEnumerable<Triangle>) groupedTri);
        if (texture.getCullMode() == (byte) 18)
          triangleList3.AddRange((IEnumerable<Triangle>) groupedTri);
        if (texture.getCullMode() == (byte) 50)
          triangleList4.AddRange((IEnumerable<Triangle>) groupedTri);
      }
      List<Triangle> triangleList5 = new List<Triangle>();
      triangleList5.AddRange((IEnumerable<Triangle>) triangleList1);
      triangleList5.AddRange((IEnumerable<Triangle>) triangleList2);
      triangleList5.AddRange((IEnumerable<Triangle>) triangleList3);
      triangleList5.AddRange((IEnumerable<Triangle>) triangleList4);
      return triangleList5;
    }

    public byte[] CreateCollisionData()
    {
      int num1 = int.MaxValue;
      int num2 = int.MaxValue;
      int num3 = int.MaxValue;
      int num4 = int.MinValue;
      int num5 = int.MinValue;
      int num6 = int.MinValue;
      foreach (Vertex vertex in this.VertexBuffer)
      {
        if ((int) vertex.x < num1)
          num1 = (int) vertex.x;
        if ((int) vertex.y < num2)
          num2 = (int) vertex.y;
        if ((int) vertex.z < num3)
          num3 = (int) vertex.z;
        if ((int) vertex.x > num4)
          num4 = (int) vertex.x;
        if ((int) vertex.y > num5)
          num5 = (int) vertex.y;
        if ((int) vertex.z > num6)
          num6 = (int) vertex.z;
      }
      int num7 = 0;
      int num8 = 10000000;
      int num9 = 0;
      int num10 = 0;
      int num11 = 0;
      int num12 = 0;
      int num13 = 0;
      int num14 = 0;
      int num15;
      int num16;
      int num17;
      for (; num8 > 1000 && num7 != 10000; num8 = num16 * num17 * num15)
      {
        num7 += 1000;
        num9 = num1 / num7;
        if (num1 % num7 != 0)
          --num9;
        num11 = num2 / num7;
        if (num2 % num7 != 0)
          --num11;
        num13 = num3 / num7;
        if (num3 % num7 != 0)
          --num13;
        num10 = num4 / num7;
        num12 = num5 / num7;
        num14 = num6 / num7;
        num16 = Math.Abs(num9) + num10 + 1;
        int num18 = Math.Abs(num11) + num12 + 1;
        num15 = Math.Abs(num13) + num14 + 1;
        num17 = num18;
      }
      List<byte> byteList = new List<byte>();
      byte[] byteArray1 = Core.Int16ToByteArray((short) num9);
      byte[] byteArray2 = Core.Int16ToByteArray((short) num11);
      byte[] byteArray3 = Core.Int16ToByteArray((short) num13);
      byte[] byteArray4 = Core.Int16ToByteArray((short) num10);
      byte[] byteArray5 = Core.Int16ToByteArray((short) num12);
      byte[] byteArray6 = Core.Int16ToByteArray((short) num14);
      byteList.Add(byteArray1[0]);
      byteList.Add(byteArray1[1]);
      byteList.Add(byteArray2[0]);
      byteList.Add(byteArray2[1]);
      byteList.Add(byteArray3[0]);
      byteList.Add(byteArray3[1]);
      byteList.Add(byteArray4[0]);
      byteList.Add(byteArray4[1]);
      byteList.Add(byteArray5[0]);
      byteList.Add(byteArray5[1]);
      byteList.Add(byteArray6[0]);
      byteList.Add(byteArray6[1]);
      byteList.Add((byte) 0);
      byteList.Add((byte) 4);
      byteList.Add((byte) 0);
      byteList.Add((byte) 4);
      byteList.Add((byte) 0);
      byteList.Add((byte) 16);
      byteList.Add((byte) (num7 >> 8));
      byteList.Add((byte) num7);
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      ArrayList arrayList = new ArrayList();
      List<int[]> numArrayList = new List<int[]>();
      int num19 = Math.Abs(num9) + num10 + 1;
      if (num9 > 0)
        num19 = num10 - num9 + 1;
      int num20 = Math.Abs(num11) + num12 + 1;
      if (num11 > 0)
        num20 = num12 - num11 + 1;
      int num21 = Math.Abs(num13) + num14 + 1;
      if (num13 > 0)
        num21 = num14 - num13 + 1;
      for (int index1 = 0; index1 < num21; ++index1)
      {
        for (int index2 = 0; index2 < num20; ++index2)
        {
          for (int index3 = 0; index3 < num19; ++index3)
          {
            List<int> intList = new List<int>();
            int num22 = (num9 + index3) * num7;
            int num23 = (num11 + index2) * num7;
            int num24 = (num13 + index1) * num7;
            int num25 = num7 / 2;
            int num26 = num22 + num25;
            int num27 = num23 + num7 / 2;
            int num28 = num24 + num7 / 2;
            for (int triangleIndex = 0; triangleIndex < this.Triangles.Count; ++triangleIndex)
            {
              Vertex[] triangleVerts = this.GetTriangleVerts(triangleIndex);
              if (Point_.isectboxtri(new float[3]
              {
                (float) num26,
                (float) num27,
                (float) num28
              }, new float[3]
              {
                (float) (num7 / 2),
                (float) (num7 / 2),
                (float) (num7 / 2)
              }, new float[3][]
              {
                new float[3]
                {
                  (float) triangleVerts[0].x,
                  (float) triangleVerts[0].y,
                  (float) triangleVerts[0].z
                },
                new float[3]
                {
                  (float) triangleVerts[1].x,
                  (float) triangleVerts[1].y,
                  (float) triangleVerts[1].z
                },
                new float[3]
                {
                  (float) triangleVerts[2].x,
                  (float) triangleVerts[2].y,
                  (float) triangleVerts[2].z
                }
              }))
                intList.Add(triangleIndex);
            }
            numArrayList.Add(intList.ToArray());
          }
        }
      }
      List<int> source = new List<int>();
      List<byte> collision = new List<byte>();
      for (int index = 0; index < numArrayList.Count; ++index)
      {
        int num29 = this.CalcColGroup(numArrayList[index], ref collision);
        source.Add(num29);
      }
      int num30 = 0;
      for (int index = 0; index < source.Count; ++index)
      {
        byteList.Add((byte) (num30 >> 8));
        byteList.Add((byte) num30);
        byteList.Add((byte) (source[index] >> 8));
        byteList.Add((byte) source[index]);
        num30 += source[index];
      }
      byteList[12] = (byte) (num19 >> 8);
      byteList[13] = (byte) num19;
      short num31 = (short) (num19 * num20);
      byteList[14] = (byte) ((uint) num31 >> 8);
      byteList[15] = (byte) num31;
      byteList[16] = (byte) (source.Count<int>() >> 8);
      byteList[17] = (byte) source.Count<int>();
      byteList[16] = (byte) (source.Count<int>() >> 8);
      byteList[17] = (byte) source.Count<int>();
      byteList[20] = (byte) (num30 >> 8);
      byteList[21] = (byte) num30;
      byteList.AddRange((IEnumerable<byte>) collision.ToArray());
      for (float num32 = (float) (byteList.Count % 8); (double) num32 != 0.0; num32 = (float) (byteList.Count % 8))
        byteList.Add((byte) 0);
      return byteList.ToArray();
    }

    private int CalcColGroup(int[] triangleIndicies, ref List<byte> collision)
    {
      int length = triangleIndicies.Length;
      foreach (int triangleIndicy in triangleIndicies)
      {
        if (this.Triangles[triangleIndicy].collisionType != CollisionType.NoCollision)
        {
          for (int index = 0; index < 3; ++index)
          {
            collision.Add((byte) ((uint) this.Triangles[triangleIndicy].verts[index] >> 8));
            collision.Add((byte) this.Triangles[triangleIndicy].verts[index]);
          }
          collision.Add((byte) 0);
          collision.Add((byte) 0);
          if (this.Triangles[triangleIndicy].collisionType != CollisionType.Water && this.Triangles[triangleIndicy].collisionType != CollisionType.Water2)
          {
            collision.Add((byte) 136);
            collision.Add((byte) this.Triangles[triangleIndicy].collisionType);
            collision.Add((byte) this.Triangles[triangleIndicy].soundType);
            collision.Add((byte) this.Triangles[triangleIndicy].groundType);
          }
          else
          {
            collision.Add((byte) 0);
            collision.Add((byte) this.Triangles[triangleIndicy].collisionType);
            collision.Add((byte) 0);
            collision.Add((byte) 0);
          }
        }
      }
      return length;
    }

    private int CalcColGroup(
      List<bool> remIndex,
      List<Triangle> triangles,
      ref byte[] collision,
      ref int collisionLoc)
    {
      int num1 = 0;
      int num2 = 0;
      for (int index = 0; index < triangles.Count; ++index)
      {
        if (remIndex[index])
        {
          if (triangles[index].collisionType != CollisionType.NoCollision)
          {
            collision[collisionLoc] = (byte) (num2 >> 8);
            collision[collisionLoc + 1] = (byte) num2;
            int num3 = num2 + 1;
            collision[collisionLoc + 2] = (byte) (num3 >> 8);
            collision[collisionLoc + 3] = (byte) num3;
            int num4 = num3 + 1;
            collision[collisionLoc + 4] = (byte) (num4 >> 8);
            collision[collisionLoc + 5] = (byte) num4;
            num2 = num4 + 1;
            collision[collisionLoc + 6] = (byte) 0;
            collision[collisionLoc + 7] = (byte) 0;
            collision[collisionLoc + 8] = (byte) 136;
            collision[collisionLoc + 9] = (byte) triangles[index].collisionType;
            collision[collisionLoc + 10] = (byte) triangles[index].soundType;
            collision[collisionLoc + 11] = (byte) triangles[index].groundType;
            if (triangles[index].collisionType == CollisionType.Water)
            {
              collision[collisionLoc + 8] = (byte) 0;
              collision[collisionLoc + 10] = (byte) 0;
              collision[collisionLoc + 11] = (byte) 0;
            }
            collisionLoc += 12;
            ++num1;
          }
          else
            num2 += 3;
        }
        else
          num2 += 3;
      }
      return num1;
    }

    public Bitmap GetTextureBitmap(int textureDataIndex) => ImageHandler.RGBA8888ToBitmap(this.TextureData[textureDataIndex].gl, this.TextureData[textureDataIndex].width, this.TextureData[textureDataIndex].height);

    public void ClampU(int textureDataIndex, bool clamp)
    {
      int num = clamp ? 2 : 0;
      foreach (Texture texture in this.Textures)
      {
        if (texture.textureData == textureDataIndex)
          texture.cms = num;
      }
    }

    public void ClampV(int textureDataIndex, bool clamp)
    {
      int num = clamp ? 2 : 0;
      foreach (Texture texture in this.Textures)
      {
        if (texture.textureData == textureDataIndex)
          texture.cmt = num;
      }
    }

    private void GenerateTexture(Texture texture, bool delete = true)
    {
      if (texture.textureData == -1)
        return;
      if (delete)
        Core.DeleteTexture(texture.glname);
      Core.GenerateTexture(ref texture.glname, this.TextureData[texture.textureData].gl, texture.width, texture.height, texture.cms, texture.cmt);
    }

    public void Transform(float scale, int x, int y, int z)
    {
      foreach (Vertex vertex in this.VertexBuffer)
        vertex.Transform(scale, x, y, z);
      this.Scale = scale;
      this.GenerateBuffers();
    }

    public byte[] GetCollisionBuffer()
    {
      byte[] numArray = new byte[this.cbo.Length];
      int index1 = 0;
      foreach (Triangle triangle in this.Triangles)
      {
        Color color = Color.Black;
        if (triangle.collisionType == CollisionType.Water)
          color = Color.Blue;
        else if (triangle.collisionType == CollisionType.Ground && triangle.groundType < GroundType.Talon)
          color = Color.Green;
        else if (triangle.collisionType == CollisionType.DoubleSided)
          color = Color.Cyan;
        else if (triangle.groundType >= GroundType.Talon && triangle.groundType < GroundType.Unclimbable)
          color = Color.Red;
        else if (triangle.groundType >= GroundType.Unclimbable)
          color = Color.Gray;
        if (triangle.collisionType == CollisionType.NoCollision)
          color = Color.Black;
        for (int index2 = 0; index2 < 3; ++index2)
        {
          numArray[index1] = color.R;
          numArray[index1 + 1] = color.G;
          numArray[index1 + 2] = color.B;
          numArray[index1 + 3] = color.A;
          index1 += 4;
        }
      }
      return numArray;
    }

    public byte[] GetSoundBuffer()
    {
      byte[] numArray = new byte[this.cbo.Length];
      int index1 = 0;
      foreach (Triangle triangle in this.Triangles)
      {
        Color color = Color.Black;
        if (triangle.soundType == SoundType.Normal)
          color = Color.Green;
        else if (triangle.soundType == SoundType.Metal)
          color = Color.Gray;
        else if (triangle.soundType == SoundType.HardGround)
          color = Color.DarkGray;
        else if (triangle.soundType == SoundType.Stone)
          color = Color.SlateGray;
        else if (triangle.soundType == SoundType.Wood)
          color = Color.BurlyWood;
        else if (triangle.soundType == SoundType.Snow)
          color = Color.White;
        else if (triangle.soundType == SoundType.Leaves)
          color = Color.Orange;
        else if (triangle.soundType == SoundType.Swamp)
          color = Color.LightGreen;
        else if (triangle.soundType == SoundType.Sand)
          color = Color.Yellow;
        else if (triangle.soundType == SoundType.Slush)
          color = Color.WhiteSmoke;
        for (int index2 = 0; index2 < 3; ++index2)
        {
          numArray[index1] = color.R;
          numArray[index1 + 1] = color.G;
          numArray[index1 + 2] = color.B;
          numArray[index1 + 3] = color.A;
          index1 += 4;
        }
      }
      return numArray;
    }

    public byte[] GetPickingBuffer()
    {
      List<byte> byteList = new List<byte>();
      foreach (Triangle triangle in this.Triangles)
      {
        TriangleColor color = triangle.color;
        for (int index = 0; index < 3; ++index)
        {
          byteList.Add(color.m_colorID[0]);
          byteList.Add(color.m_colorID[1]);
          byteList.Add(color.m_colorID[2]);
          byteList.Add(byte.MaxValue);
        }
      }
      return byteList.ToArray();
    }

    public void Delete()
    {
      foreach (Texture texture in this.Textures)
        Core.DeleteTexture(texture.glname);
      this.TextureData.Clear();
      this.Textures.Clear();
      this.Triangles.Clear();
      this.VertexBuffer.Clear();
    }

    public Vector GetTriangleNormal(int triangleIndex)
    {
      Triangle triangle = this.Triangles[triangleIndex];
      Vector vector1 = new Vector((float) this.VertexBuffer[(int) triangle.verts[0]].x, (float) this.VertexBuffer[(int) triangle.verts[0]].y, (float) this.VertexBuffer[(int) triangle.verts[0]].z);
      Vector vector2 = new Vector((float) this.VertexBuffer[(int) triangle.verts[1]].x, (float) this.VertexBuffer[(int) triangle.verts[1]].y, (float) this.VertexBuffer[(int) triangle.verts[1]].z);
      Vector vector3 = new Vector((float) this.VertexBuffer[(int) triangle.verts[2]].x, (float) this.VertexBuffer[(int) triangle.verts[2]].y, (float) this.VertexBuffer[(int) triangle.verts[2]].z);
      Vector vector4 = vector1;
      return Vector.Cross(vector2 - vector4, vector3 - vector1);
    }

    private int HasTexture(Texture texture)
    {
      for (int index = 0; index < this.Textures.Count; ++index)
      {
        if (this.Textures[index].equal(texture))
          return index;
      }
      return -1;
    }

    public void UpdateTextureCullMode(Texture t, CullMode cullMode)
    {
      t.cullMode = cullMode;
      this.GenerateTexture(t);
    }

    public void SetTriangleTexture(int triangleIndex, Texture newTexture)
    {
      for (int index = 0; index < this.Textures.Count; ++index)
      {
        if (this.Textures[index].equal(newTexture))
        {
          this.Triangles[triangleIndex].textureSetting = index;
          return;
        }
      }
      this.GenerateTexture(newTexture);
      this.Textures.Add(newTexture);
      this.Triangles[triangleIndex].textureSetting = this.Textures.Count - 1;
    }

    public void AddTriangle(
      Vertex[] verts,
      Triangle triangle,
      Texture texture,
      WumbasWigwam.TextureData textureData)
    {
      this.VertexBuffer.AddRange((IEnumerable<Vertex>) verts);
      ushort num = (ushort) (this.VertexBuffer.Count - 1);
      triangle.verts = new ushort[3]
      {
        (ushort) ((uint) num - 2U),
        (ushort) ((uint) num - 1U),
        num
      };
      bool flag1 = false;
      bool flag2 = false;
      if (textureData == (WumbasWigwam.TextureData) null)
      {
        flag1 = true;
      }
      else
      {
        for (int index = 0; index < this.TextureData.Count; ++index)
        {
          if (this.TextureData[index].GetHashCode() == textureData.GetHashCode())
          {
            flag1 = true;
            texture.textureData = index;
            break;
          }
        }
      }
      if (flag1)
      {
        for (int index = 0; index < this.Textures.Count; ++index)
        {
          if (this.Textures[index].equal(texture))
          {
            flag2 = true;
            triangle.textureSetting = index;
            break;
          }
        }
      }
      if (flag1 & flag2)
      {
        this.Triangles.Add(triangle);
      }
      else
      {
        if (!flag1)
        {
          this.TextureData.Add(textureData);
          texture.textureData = this.TextureData.Count<WumbasWigwam.TextureData>() - 1;
        }
        if (!flag2)
        {
          this.Textures.Add(texture);
          this.GenerateTexture(texture, false);
        }
        triangle.textureSetting = this.Textures.Count<Texture>() - 1;
        this.Triangles.Add(triangle);
      }
    }

    public void DeleteTriangle(int triangleIndex)
    {
      bool[] flagArray = new bool[3]{ true, true, true };
      bool flag1 = true;
      bool flag2 = true;
      Triangle triangle1 = this.Triangles[triangleIndex];
      for (int index1 = 0; index1 < this.Triangles.Count; ++index1)
      {
        if (index1 != triangleIndex)
        {
          for (int index2 = 0; index2 < 3; ++index2)
          {
            if ((int) this.Triangles[index1].verts[index2] == (int) triangle1.verts[index2])
              flagArray[index2] = false;
          }
          if (flag2 && this.Triangles[index1].textureSetting == triangle1.textureSetting)
            flag2 = false;
          if (flag1 && this.Textures[this.Triangles[index1].textureSetting].textureData == this.Textures[triangle1.textureSetting].textureData)
            flag1 = false;
        }
      }
      if (flag1)
      {
        this.TextureData.RemoveAt(this.Textures[triangle1.textureSetting].textureData);
        foreach (Texture texture in this.Textures)
        {
          if (texture.textureData > this.Textures[triangle1.textureSetting].textureData)
            --texture.textureData;
        }
      }
      if (flag2)
      {
        Core.DeleteTexture(this.Textures[triangle1.textureSetting].glname);
        this.Textures.RemoveAt(triangle1.textureSetting);
        foreach (Triangle triangle2 in this.Triangles)
        {
          if (triangle2.textureSetting > triangle1.textureSetting)
            --triangle2.textureSetting;
        }
      }
      if (!flagArray[0] && !flagArray[1] && !flagArray[2])
        return;
      List<ushort> ushortList = new List<ushort>()
      {
        this.Triangles[triangleIndex].verts[0],
        this.Triangles[triangleIndex].verts[1],
        this.Triangles[triangleIndex].verts[2]
      };
      ushortList.Sort();
      ushortList.Reverse();
      for (int index = 0; index < 3; ++index)
      {
        if (flagArray[index])
          this.VertexBuffer.RemoveAt((int) ushortList[index]);
      }
      this.Triangles.RemoveAt(triangleIndex);
      foreach (Triangle triangle3 in this.Triangles)
      {
        for (int index = 0; index < 3; ++index)
        {
          if ((int) triangle3.verts[index] > (int) ushortList[0])
            triangle3.verts[index] -= (ushort) 3;
          else if ((int) triangle3.verts[index] > (int) ushortList[1])
            triangle3.verts[index] -= (ushort) 2;
          else if ((int) triangle3.verts[index] > (int) ushortList[2])
            --triangle3.verts[index];
        }
      }
    }

    public Triangle GetTriangle(int triangleIndex) => this.Triangles[triangleIndex];

    public int GetTriangle(byte[] colour)
    {
      int num = -1;
      for (int index = 0; index < this.Triangles.Count; ++index)
      {
        Triangle triangle = this.Triangles[index];
        if ((int) triangle.color.m_colorID[0] == (int) colour[0] && (int) triangle.color.m_colorID[1] == (int) colour[1] && (int) triangle.color.m_colorID[2] == (int) colour[2])
          num = index;
      }
      return num;
    }

    public void FlipV()
    {
      foreach (Vertex vertex in this.VertexBuffer)
        vertex.v *= -1f;
      this.GenerateBuffers();
    }

    public void FlipTriangle(int triangleIndex)
    {
      Triangle triangle = this.Triangles[triangleIndex];
      ushort[] numArray = new ushort[3]
      {
        triangle.verts[0],
        triangle.verts[1],
        triangle.verts[2]
      };
      triangle.verts[0] = numArray[2];
      triangle.verts[2] = numArray[0];
    }

    public void ShadeVert(int triangleIndex, int vertexIndex, byte r, byte g, byte b, byte a) => this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[vertexIndex]].UpdateColor(r, g, b, a);

    public void ShadeModel(byte r, byte g, byte b, byte a)
    {
      foreach (Vertex vertex in this.VertexBuffer)
        vertex.UpdateColor(r, g, b, a);
    }

    public void AnimateVert(int triangleIndex, int vertexIndex, bool animate) => this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[vertexIndex]].animateVertColour = animate;

    public void ScrollVert(int triangleIndex, int vertexIndex, ScrollMode mode)
    {
      this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[vertexIndex]].scrollTexture = false;
      this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[vertexIndex]].scrollTextureFast = false;
      this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[vertexIndex]].scrollTextureSlow = false;
      switch (mode)
      {
        case ScrollMode.Slow:
          this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[vertexIndex]].scrollTextureSlow = true;
          break;
        case ScrollMode.Normal:
          this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[vertexIndex]].scrollTexture = true;
          break;
        case ScrollMode.Fast:
          this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[vertexIndex]].scrollTextureFast = true;
          break;
      }
    }

    public void ScrollTriangle(int triangleIndex, ScrollMode mode)
    {
      for (int index = 0; index < 3; ++index)
      {
        this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[index]].scrollTexture = false;
        this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[index]].scrollTextureFast = false;
        this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[index]].scrollTextureSlow = false;
        switch (mode)
        {
          case ScrollMode.Slow:
            this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[index]].scrollTextureSlow = true;
            break;
          case ScrollMode.Normal:
            this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[index]].scrollTexture = true;
            break;
          case ScrollMode.Fast:
            this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[index]].scrollTextureFast = true;
            break;
        }
      }
    }

    public void GenerateWaves(int triangleIndex, bool generate) => this.Triangles[triangleIndex].genWave = generate;

    public void SetCollision(int triangleIndex, CollisionType type) => this.Triangles[triangleIndex].collisionType = type;

    public void SetSound(int triangleIndex, SoundType type) => this.Triangles[triangleIndex].soundType = type;

    public void SetGround(int triangleIndex, GroundType type) => this.Triangles[triangleIndex].groundType = type;

    public void ShadeTriangle(int triangleIndex, byte r, byte g, byte b, byte a)
    {
      for (int index = 0; index < 3; ++index)
        this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[index]].UpdateColor(r, g, b, a);
    }

    public void AnimateTriangle(int triangleIndex, bool animate)
    {
      for (int index = 0; index < 3; ++index)
        this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[index]].animateVertColour = animate;
    }

    public void UpdateTexture(
      int textureDataIndex,
      byte r,
      byte g,
      byte b,
      byte a,
      CullMode cullMode,
      CollisionType collisionType,
      bool updateCullMode,
      bool updateCollision)
    {
      foreach (Triangle triangle in this.Triangles)
      {
        if (this.Textures[triangle.textureSetting].textureData == textureDataIndex)
        {
          for (int index = 0; index < 3; ++index)
          {
            this.VertexBuffer[(int) triangle.verts[index]].r = r;
            this.VertexBuffer[(int) triangle.verts[index]].g = g;
            this.VertexBuffer[(int) triangle.verts[index]].b = b;
          }
          triangle.collisionType = collisionType;
          this.UpdateTextureCullMode(this.Textures[triangle.textureSetting], cullMode);
        }
      }
      this.GenerateBuffers();
    }

    public Texture GetTexture(int triangleIndex) => this.Textures[this.Triangles[triangleIndex].textureSetting];

    public Texture GetTextureFromBuffer(int index) => this.Textures[this.TextureBuffer[index]];

    public Vertex[] GetTriangleVerts(int triangleIndex)
    {
      Triangle triangle = this.Triangles[triangleIndex];
      Vertex[] vertexArray = new Vertex[3];
      for (int index = 0; index < 3; ++index)
        vertexArray[index] = this.VertexBuffer[(int) triangle.verts[index]];
      return vertexArray;
    }

    public Vertex GetVertex(int triangleIndex, int vertexIndex) => this.VertexBuffer[(int) this.Triangles[triangleIndex].verts[vertexIndex]];

    private void SortTriangles() => this.Triangles.Sort((Comparison<Triangle>) ((x, y) => x.textureSetting.CompareTo(y.textureSetting)));

    public void GenerateBuffers()
    {
      Core.DeleteBuffers(this.iboHandles);
      Core.DeleteBuffers(this.seqIBOHandles);
      Core.DeleteBuffer(this.seqLineHandle);
      this.iboHandles.Clear();
      this.seqIBOHandles.Clear();
      this.VertexBufferTOVBO();
      this.TrianglesTOIBO();
      this.VertexBufferTOVBOSequential();
      this.TrianglesTOIBOSequential();
      this.TrianglesTOLineSequential();
    }

    private void VertexBufferTOVBO()
    {
      this.vbo = new short[this.VertexBuffer.Count * 3];
      this.cbo = new byte[this.VertexBuffer.Count * 4];
      this.uvBuffer = new float[this.VertexBuffer.Count * 2];
      int index1 = 0;
      int index2 = 0;
      int index3 = 0;
      int index4 = 0;
      while (index1 < this.VertexBuffer.Count)
      {
        this.vbo[index2] = this.VertexBuffer[index1].x;
        this.vbo[index2 + 1] = this.VertexBuffer[index1].y;
        this.vbo[index2 + 2] = this.VertexBuffer[index1].z;
        this.cbo[index3] = this.VertexBuffer[index1].r;
        this.cbo[index3 + 1] = this.VertexBuffer[index1].g;
        this.cbo[index3 + 2] = this.VertexBuffer[index1].b;
        this.cbo[index3 + 3] = this.VertexBuffer[index1].a;
        this.uvBuffer[index4] = this.VertexBuffer[index1].u;
        this.uvBuffer[index4 + 1] = this.VertexBuffer[index1].v;
        ++index1;
        index2 += 3;
        index3 += 4;
        index4 += 2;
      }
    }

    private void TrianglesTOIBO()
    {
      try
      {
        this.SortTriangles();
        this.iboData.Clear();
        this.TextureBuffer.Clear();
        List<ushort> ushortList = new List<ushort>();
        int textureSetting = this.Triangles[0].textureSetting;
        this.TextureBuffer.Add(textureSetting);
        foreach (Triangle triangle in this.Triangles)
        {
          if (triangle.textureSetting != textureSetting)
          {
            this.iboData.Add(ushortList.ToArray());
            ushortList.Clear();
            textureSetting = triangle.textureSetting;
            this.TextureBuffer.Add(textureSetting);
            ushortList.Add(triangle.verts[0]);
            ushortList.Add(triangle.verts[1]);
            ushortList.Add(triangle.verts[2]);
          }
          else
          {
            ushortList.Add(triangle.verts[0]);
            ushortList.Add(triangle.verts[1]);
            ushortList.Add(triangle.verts[2]);
          }
        }
        this.iboData.Add(ushortList.ToArray());
        foreach (ushort[] buffer in this.iboData)
          this.iboHandles.Add(Core.GenBuffer(buffer));
      }
      catch (Exception ex)
      {
      }
    }

    private void VertexBufferTOVBOSequential()
    {
      List<short> shortList = new List<short>();
      foreach (Triangle triangle in this.Triangles)
      {
        try
        {
          for (int index = 0; index < 3; ++index)
          {
            shortList.Add(this.VertexBuffer[(int) triangle.verts[index]].x);
            shortList.Add(this.VertexBuffer[(int) triangle.verts[index]].y);
            shortList.Add(this.VertexBuffer[(int) triangle.verts[index]].z);
          }
        }
        catch (Exception ex)
        {
        }
      }
      this.seqVBO = shortList.ToArray();
    }

    private void TrianglesTOIBOSequential()
    {
      try
      {
        this.seqIBOData = new List<ushort[]>();
        List<ushort> ushortList = new List<ushort>();
        int textureSetting = this.Triangles[0].textureSetting;
        ushort num1 = 0;
        foreach (Triangle triangle in this.Triangles)
        {
          if (triangle.textureSetting != textureSetting)
          {
            this.seqIBOData.Add(ushortList.ToArray());
            ushortList.Clear();
            textureSetting = triangle.textureSetting;
          }
          ushortList.Add(num1);
          ushort num2 = (ushort) ((uint) num1 + 1U);
          ushortList.Add(num2);
          ushort num3 = (ushort) ((uint) num2 + 1U);
          ushortList.Add(num3);
          num1 = (ushort) ((uint) num3 + 1U);
        }
        this.seqIBOData.Add(ushortList.ToArray());
        foreach (ushort[] buffer in this.seqIBOData)
          this.seqIBOHandles.Add(Core.GenBuffer(buffer));
      }
      catch (Exception ex)
      {
      }
    }

    private void TrianglesTOLineSequential()
    {
      try
      {
        List<ushort> ushortList = new List<ushort>();
        ushort num = 0;
        foreach (Triangle triangle in this.Triangles)
        {
          ushortList.Add(num);
          ushortList.Add((ushort) ((uint) num + 1U));
          ushortList.Add(num);
          ushortList.Add((ushort) ((uint) num + 2U));
          ushortList.Add((ushort) ((uint) num + 1U));
          ushortList.Add((ushort) ((uint) num + 2U));
          num += (ushort) 3;
        }
        this.seqLineData = ushortList.ToArray();
        this.seqLineHandle = Core.GenBuffer(this.seqLineData);
      }
      catch (Exception ex)
      {
      }
    }

    public byte[] calculateVertexColourEffect()
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 0);
      byteList.Add((byte) 200);
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      for (int index = 0; index < this.VertexBuffer.Count; ++index)
      {
        if (this.VertexBuffer[index].animateVertColour)
          byteList.Add((byte) (index >> 8));
        byteList.Add((byte) index);
      }
      byteList[2] = (byte) ((byteList.Count - 4) / 2 >> 8);
      byteList[3] = (byte) ((byteList.Count - 4) / 2);
      if (byteList.Count < 5)
        byteList.Clear();
      return byteList.ToArray();
    }

    public byte[] calculateWaves()
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 2);
      byteList.Add((byte) 196);
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      foreach (Triangle triangle in this.Triangles)
      {
        if (triangle.genWave)
        {
          byteList.Add((byte) ((uint) triangle.verts[0] >> 8));
          byteList.Add((byte) triangle.verts[0]);
          byteList.Add((byte) ((uint) triangle.verts[1] >> 8));
          byteList.Add((byte) triangle.verts[1]);
          byteList.Add((byte) ((uint) triangle.verts[2] >> 8));
          byteList.Add((byte) triangle.verts[2]);
        }
      }
      byteList[2] = (byte) ((byteList.Count - 4) / 2 >> 8);
      byteList[3] = (byte) ((byteList.Count - 4) / 2);
      return byteList.ToArray();
    }

    public byte[] calculateTextureScrollEffect(ScrollSpeed speed)
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 0);
      byteList.Add((byte) speed);
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      for (int index = 0; index < this.VertexBuffer.Count; ++index)
      {
        if (this.VertexBuffer[index].scrollTexture && speed == ScrollSpeed.Normal || this.VertexBuffer[index].scrollTextureFast && speed == ScrollSpeed.Fast || this.VertexBuffer[index].scrollTextureSlow && speed == ScrollSpeed.Slow)
        {
          byteList.Add((byte) (index >> 8));
          byteList.Add((byte) index);
        }
      }
      byteList[2] = (byte) ((byteList.Count - 4) / 2 >> 8);
      byteList[3] = (byte) ((byteList.Count - 4) / 2);
      if (byteList.Count < 5)
        byteList.Clear();
      return byteList.ToArray();
    }
  }
}
