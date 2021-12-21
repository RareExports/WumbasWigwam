// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.F3DEX2
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using NLog;
using System;
using System.Collections.Generic;
using Tao.OpenGl;

namespace WumbasWigwam
{
  public class F3DEX2
  {
    private static Logger logger = LogManager.GetCurrentClassLogger();
    public static int F3DEX2_MTX_STACKSIZE = 18;
    public static int F3DEX2_MTX_MODELVIEW = 0;
    public static int F3DEX2_MTX_PROJECTION = 4;
    public static int F3DEX2_MTX_MUL = 0;
    public static int F3DEX2_MTX_LOAD = 2;
    public static int F3DEX2_MTX_NOPUSH = 0;
    public static int F3DEX2_MTX_PUSH = 1;
    public static int F3DEX2_TEXTURE_ENABLE = 0;
    public static int F3DEX2_SHADING_SMOOTH = 2097152;
    public static int F3DEX2_CULL_FRONT = 512;
    public static int F3DEX2_CULL_BACK = 1024;
    public static int F3DEX2_CULL_BOTH = 1536;
    public static int F3DEX2_CLIPPING = 8388608;
    public static int F3DEX2_MV_VIEWPORT = 8;
    public static int F3DEX2_MWO_aLIGHT_1 = 0;
    public static int F3DEX2_MWO_bLIGHT_1 = 4;
    public static int F3DEX2_MWO_aLIGHT_2 = 24;
    public static int F3DEX2_MWO_bLIGHT_2 = 28;
    public static int F3DEX2_MWO_aLIGHT_3 = 48;
    public static int F3DEX2_MWO_bLIGHT_3 = 52;
    public static int F3DEX2_MWO_aLIGHT_4 = 72;
    public static int F3DEX2_MWO_bLIGHT_4 = 76;
    public static int F3DEX2_MWO_aLIGHT_5 = 96;
    public static int F3DEX2_MWO_bLIGHT_5 = 100;
    public static int F3DEX2_MWO_aLIGHT_6 = 120;
    public static int F3DEX2_MWO_bLIGHT_6 = 124;
    public static int F3DEX2_MWO_aLIGHT_7 = 144;
    public static int F3DEX2_MWO_bLIGHT_7 = 148;
    public static int F3DEX2_MWO_aLIGHT_8 = 168;
    public static int F3DEX2_MWO_bLIGHT_8 = 172;
    public static int F3DEX2_RDPHALF_2 = 241;
    public static int F3DEX2_SETOTHERMODE_H = 227;
    public static int F3DEX2_SETOTHERMODE_L = 226;
    public static int F3DEX2_RDPHALF_1 = 225;
    public static int F3DEX2_SPNOOP = 224;
    public static int F3DEX2_ENDDL = 223;
    public static int F3DEX2_DL = 222;
    public static int F3DEX2_LOAD_UCODE = 221;
    public static int F3DEX2_MOVEMEM = 220;
    public static int F3DEX2_MOVEWORD = 219;
    public static int F3DEX2_MTX = 218;
    public static int F3DEX2_GEOMETRYMODE = 217;
    public static int F3DEX2_POPMTX = 216;
    public static int F3DEX2_TEXTURE = 215;
    public static int F3DEX2_DMA_IO = 214;
    public static int F3DEX2_SPECIAL_1 = 213;
    public static int F3DEX2_SPECIAL_2 = 212;
    public static int F3DEX2_SPECIAL_3 = 211;
    public static int F3DEX2_VTX = 1;
    public static int F3DEX2_MODIFYVTX = 2;
    public static int F3DEX2_CULLDL = 3;
    public static int F3DEX2_BRANCH_Z = 4;
    public static int F3DEX2_TRI1 = 5;
    public static int F3DEX2_TRI2 = 6;
    public static int F3DEX2_QUAD = 7;
    public int textureFormat = -1;
    public int texelSize;
    public int lineSize;
    public int cms;
    public int cmt;
    public int tileNo = -1;
    public bool firstSetSize = true;
    public static int FMT_RGBA = 0;
    public static int FMT_YUV = 1;
    public static int FMT_CI = 2;
    public static int FMT_IA = 3;
    public static int FMT_I = 3;
    public static int PS_4 = 0;
    public static int PS_8 = 1;
    public static int PS_16 = 2;
    public static int PS_32 = 3;

    public static bool ReadModel(
      ref byte[] bytesInFile,
      ref int collStart,
      ref int F3DStart,
      ref int F3DCommands,
      ref int F3DEnd,
      ref int vertStart,
      ref int VTCount,
      ref int textureCount,
      ref F3DEXVert[] verts,
      ref List<byte[]> commands,
      ref F3DEXTexture[] textures)
    {
      if (!F3DEX2.readBase(ref bytesInFile, ref collStart, ref F3DStart, ref F3DCommands, ref F3DEnd, ref vertStart, ref VTCount, ref textureCount))
        return false;
      verts = new F3DEXVert[VTCount];
      F3DEX2.ripVerts(ref bytesInFile, ref verts, VTCount, vertStart);
      textures = F3DEX2.getTextures(ref bytesInFile);
      int num1 = 0;
      int num2 = F3DStart;
      for (; num1 < F3DCommands; ++num1)
      {
        byte[] numArray = new byte[8];
        for (int index = 0; index < 8; ++index)
          numArray[index] = bytesInFile[num2 + index];
        num2 += 8;
        commands.Add(numArray);
      }
      return true;
    }

    private static bool readBase(
      ref byte[] bytesInFile,
      ref int collStart,
      ref int F3DStart,
      ref int F3DCommands,
      ref int F3DEnd,
      ref int vertStart,
      ref int VTCount,
      ref int textureCount)
    {
      if (bytesInFile[3] != (byte) 11)
        return false;
      collStart = (int) bytesInFile[4] * 16777216 + (int) bytesInFile[5] * 65536 + (int) bytesInFile[6] * 256 + (int) bytesInFile[7] + 24;
      F3DStart = (int) bytesInFile[12] * 16777216 + (int) bytesInFile[13] * 65536 + (int) bytesInFile[14] * 256 + (int) bytesInFile[15] + 8;
      F3DCommands = (int) bytesInFile[F3DStart - 6] * 256 + (int) bytesInFile[F3DStart - 5];
      F3DEnd = F3DStart + F3DCommands * 8;
      vertStart = (int) bytesInFile[16] * 16777216 + (int) bytesInFile[17] * 65536 + (int) bytesInFile[18] * 256 + (int) bytesInFile[19] + 24;
      VTCount = (int) bytesInFile[70] * 256 + (int) bytesInFile[71];
      textureCount = (int) bytesInFile[84] * 256 + (int) bytesInFile[85];
      return true;
    }

    private static F3DEXTexture[] getTextures(ref byte[] bytesInFile)
    {
      int length1 = (int) bytesInFile[84] * 256 + (int) bytesInFile[85];
      F3DEXTexture[] f3DexTextureArray = new F3DEXTexture[length1];
      if (length1 == 0 && length1 == 0)
        return new F3DEXTexture[1]
        {
          new F3DEXTexture(0, 0U, 1, 1)
        };
      int length2 = (int) bytesInFile[82] * 256 + (int) bytesInFile[83];
      int num1 = 88;
      int num2 = length1 * 8 + 88;
      if ((int) bytesInFile[12] * 16777216 + (int) bytesInFile[13] * 65536 + (int) bytesInFile[14] * 256 + (int) bytesInFile[15] == num2)
      {
        int num3 = 0;
        int index = 0;
        for (int textureTableOffset_ = num1; textureTableOffset_ < num2; textureTableOffset_ += 8)
        {
          int fileNo = (int) bytesInFile[textureTableOffset_] * 16777216 + (int) bytesInFile[textureTableOffset_ + 1] * 65536 + (int) bytesInFile[textureTableOffset_ + 2] * 256 + (int) bytesInFile[textureTableOffset_ + 3];
          int textureWidth_ = (int) bytesInFile[textureTableOffset_ + 6];
          int textureHeight_ = (int) bytesInFile[textureTableOffset_ + 7];
          f3DexTextureArray[index] = new F3DEXTexture(textureTableOffset_, (uint) num3, textureWidth_, textureHeight_);
          f3DexTextureArray[index].external = true;
          f3DexTextureArray[index].externalFile = RomHandler.DecompressTextureFile(fileNo);
          num3 += f3DexTextureArray[index].externalFile.Length;
          ++index;
        }
        return f3DexTextureArray;
      }
      if (length1 == 0)
        return new F3DEXTexture[1]
        {
          new F3DEXTexture(0, 0U, 1, 1)
        };
      int index1 = 0;
      for (int textureTableOffset_ = num1; textureTableOffset_ < num2; textureTableOffset_ += 8)
      {
        int num4 = (int) bytesInFile[textureTableOffset_] * 16777216 + (int) bytesInFile[textureTableOffset_ + 1] * 65536 + (int) bytesInFile[textureTableOffset_ + 2] * 256 + (int) bytesInFile[textureTableOffset_ + 3];
        int textureWidth_ = (int) bytesInFile[textureTableOffset_ + 6];
        int textureHeight_ = (int) bytesInFile[textureTableOffset_ + 7];
        f3DexTextureArray[index1] = new F3DEXTexture(textureTableOffset_, (uint) num4, textureWidth_, textureHeight_);
        ++index1;
      }
      if (length2 > 0)
      {
        byte[] numArray = new byte[length2];
        if (num2 + length2 <= bytesInFile.Length)
        {
          for (int index2 = 0; index2 < length2; ++index2)
            numArray[index2] = bytesInFile[num2 + index2];
        }
      }
      else
        f3DexTextureArray[0] = new F3DEXTexture(0, 0U, 1, 1);
      return f3DexTextureArray;
    }

    private static void ripVerts(
      ref byte[] bytesInFile,
      ref F3DEXVert[] verts,
      int VTCount,
      int offset)
    {
      for (int index = 0; index < VTCount; ++index)
      {
        int num1 = (int) (short) ((int) bytesInFile[offset] * 256 + (int) bytesInFile[offset + 1]);
        short num2 = (short) ((int) bytesInFile[offset + 2] * 256 + (int) bytesInFile[offset + 3]);
        short num3 = (short) ((int) bytesInFile[offset + 4] * 256 + (int) bytesInFile[offset + 5]);
        short num4 = (short) ((int) bytesInFile[offset + 8] * 256 + (int) bytesInFile[offset + 9]);
        short num5 = (short) ((int) bytesInFile[offset + 10] * 256 + (int) bytesInFile[offset + 11]);
        byte num6 = bytesInFile[offset + 12];
        byte num7 = bytesInFile[offset + 13];
        byte num8 = bytesInFile[offset + 14];
        byte maxValue = byte.MaxValue;
        int num9 = (int) num2;
        int num10 = (int) num3;
        int num11 = (int) num4;
        int num12 = (int) num5;
        int num13 = (int) num6;
        int num14 = (int) num7;
        int num15 = (int) num8;
        int num16 = (int) maxValue;
        F3DEXVert f3DexVert = new F3DEXVert((short) num1, (short) num9, (short) num10, (short) num11, (short) num12, (byte) num13, (byte) num14, (byte) num15, (byte) num16);
        verts[index] = f3DexVert;
        offset += 16;
      }
    }

    public static void F3DEX_2_GL_TEXTURE(
      ref byte[] bytesInFile,
      ref F3DEXTexture texture,
      int textureCount,
      ref int texturesGL,
      bool deleteTextureGL)
    {
      Gl.glEnable(3553);
      byte[] textureN64Bytes = new byte[texture.textureSize];
      if (texture.external)
      {
        textureN64Bytes = !texture.palLoaded ? texture.externalFile : texture.GetIndices();
      }
      else
      {
        int num = !texture.palLoaded ? (int) texture.textureOffset + 88 + textureCount * 8 : (int) texture.indexOffset + 88 + textureCount * 8;
        try
        {
          for (int index = 0; index < textureN64Bytes.Length; ++index)
          {
            if (num + index < bytesInFile.Length)
              textureN64Bytes[index] = bytesInFile[num + index];
            else
              break;
          }
        }
        catch
        {
          F3DEX2.logger.Log(NLog.LogLevel.Error, "Calculated texture size wrong, continuing with loaded buffer");
        }
      }
      texture.loadPixels(ref textureN64Bytes);
      if (texture.pixels != null)
      {
        Gl.glEnable(3553);
      }
      else
      {
        F3DEX2.logger.Log(NLog.LogLevel.Debug, "Texture Disabled for texture - " + texture.textureTableOffset.ToString("x"));
        Gl.glDisable(3553);
      }
      if (Tile.texelSize > 2 && Tile.textureFormat != 0)
      {
        if (deleteTextureGL)
          Gl.glDeleteTextures(1, ref texturesGL);
        Gl.glEnable(3553);
        Gl.glGenTextures(1, out texturesGL);
        Gl.glEnable(3553);
        Gl.glBindTexture(3553, texturesGL);
        try
        {
          Gl.glTexImage2D(3553, 0, 6408, texture.textureWidth, texture.textureHeight, 0, 6408, 5121, (object) textureN64Bytes);
        }
        catch (Exception ex)
        {
          F3DEX2.logger.Log(NLog.LogLevel.Error, "Falied to generate the texture");
          F3DEX2.logger.Log(NLog.LogLevel.Debug, ex.Message);
        }
      }
      else if (texture.pixels != null)
      {
        if (deleteTextureGL)
          Gl.glDeleteTextures(1, ref texturesGL);
        Gl.glEnable(3553);
        Gl.glGenTextures(1, out texturesGL);
        Gl.glEnable(3553);
        Gl.glBindTexture(3553, texturesGL);
        Gl.glTexImage2D(3553, 0, 6408, texture.textureWidth, texture.textureHeight, 0, 6408, 5121, (object) texture.pixels);
      }
      Gl.glTexParameterf(3553, 34046, 16f);
      Gl.glTexParameteri(3553, 10240, 9729);
      Gl.glTexParameteri(3553, 10241, 9729);
      if (Tile.cms == 0)
        Gl.glTexParameteri(3553, 10242, 10497);
      if (Tile.cms == 2)
        Gl.glTexParameteri(3553, 10242, 33071);
      if (Tile.cmt == 0)
        Gl.glTexParameteri(3553, 10243, 10497);
      if (Tile.cmt != 2)
        return;
      Gl.glTexParameteri(3553, 10243, 33071);
    }

    public static void GL_VTX(
      ref byte[] bytesInFile,
      byte[] cmd,
      ref F3DEXVert[] cache,
      ref F3DEXVert[] verts,
      ref F3DEXTexture texture,
      int textureCount,
      ref int texturesGL,
      ref bool newTexture)
    {
      int num1 = (int) cmd[4] * 16777216 + (int) cmd[5] * 65536 + (int) cmd[6] * 256 + (int) cmd[7];
      int num2 = (int) cmd[1] * 65536 + (int) cmd[2] * 256 + (int) cmd[3];
      byte num3 = (byte) ((uint) num2 >> 12 & (uint) byte.MaxValue);
      byte num4 = (byte) (((uint) num2 >> 1 & (uint) sbyte.MaxValue) - (uint) num3);
      if (num4 > (byte) 63)
        num4 = (byte) 63;
      uint num5 = ((uint) (num1 << 8) >> 8) / 16U;
      try
      {
        for (int index = (int) num4; index < (int) num3 + (int) num4; ++index)
        {
          if ((long) num5 < (long) verts.Length)
            cache[index] = verts[(int) num5];
          ++num5;
        }
      }
      catch (Exception ex)
      {
      }
      if (!newTexture)
        return;
      F3DEX2.F3DEX_2_GL_TEXTURE(ref bytesInFile, ref texture, textureCount, ref texturesGL, true);
      newTexture = false;
    }

    public static void GL_VTX_PICKING(byte[] cmd, ref F3DEXVert[] cache, ref F3DEXVert[] verts)
    {
      int num1 = (int) cmd[4] * 16777216 + (int) cmd[5] * 65536 + (int) cmd[6] * 256 + (int) cmd[7];
      int num2 = (int) cmd[1] * 65536 + (int) cmd[2] * 256 + (int) cmd[3];
      byte num3 = (byte) ((uint) num2 >> 12 & (uint) byte.MaxValue);
      byte num4 = (byte) (((uint) num2 >> 1 & (uint) sbyte.MaxValue) - (uint) num3);
      if (num4 > (byte) 63)
        num4 = (byte) 63;
      uint num5 = ((uint) (num1 << 8) >> 8) / 16U;
      try
      {
        for (int index = (int) num4; index < (int) num3 + (int) num4; ++index)
        {
          if ((long) num5 < (long) verts.Length)
            cache[index] = verts[(int) num5];
          ++num5;
        }
      }
      catch (Exception ex)
      {
      }
    }

    public static void GL_TRI1(byte[] com, ref F3DEXVert[] cache, ref F3DEXTexture texture)
    {
      short num1 = (short) ((int) com[1] / 2);
      short num2 = (short) ((int) com[2] / 2);
      short num3 = (short) ((int) com[3] / 2);
      Gl.glBegin(4);
      Gl.glColor4f((float) cache[(int) num1].r / (float) byte.MaxValue, (float) cache[(int) num1].g / (float) byte.MaxValue, (float) cache[(int) num1].b / (float) byte.MaxValue, (float) cache[(int) num1].a / (float) byte.MaxValue);
      Gl.glTexCoord2f((float) cache[(int) num1].u * texture.textureWRatio, (float) cache[(int) num1].v * texture.textureHRatio);
      Gl.glVertex3f((float) cache[(int) num1].x, (float) cache[(int) num1].y, (float) cache[(int) num1].z);
      Gl.glColor4f((float) cache[(int) num2].r / (float) byte.MaxValue, (float) cache[(int) num2].g / (float) byte.MaxValue, (float) cache[(int) num2].b / (float) byte.MaxValue, (float) cache[(int) num2].a / (float) byte.MaxValue);
      Gl.glTexCoord2f((float) cache[(int) num2].u * texture.textureWRatio, (float) cache[(int) num2].v * texture.textureHRatio);
      Gl.glVertex3f((float) cache[(int) num2].x, (float) cache[(int) num2].y, (float) cache[(int) num2].z);
      Gl.glColor4f((float) cache[(int) num3].r / (float) byte.MaxValue, (float) cache[(int) num3].g / (float) byte.MaxValue, (float) cache[(int) num3].b / (float) byte.MaxValue, (float) cache[(int) num3].a / (float) byte.MaxValue);
      Gl.glTexCoord2f((float) cache[(int) num3].u * texture.textureWRatio, (float) cache[(int) num3].v * texture.textureHRatio);
      Gl.glVertex3f((float) cache[(int) num3].x, (float) cache[(int) num3].y, (float) cache[(int) num3].z);
      Gl.glEnd();
    }

    public static void GL_TRI1_PICKING(byte[] com, ref F3DEXVert[] cache)
    {
      short num1 = (short) ((int) com[1] / 2);
      short num2 = (short) ((int) com[2] / 2);
      short num3 = (short) ((int) com[3] / 2);
      Gl.glBegin(4);
      Gl.glVertex3f((float) cache[(int) num1].x, (float) cache[(int) num1].y, (float) cache[(int) num1].z);
      Gl.glVertex3f((float) cache[(int) num2].x, (float) cache[(int) num2].y, (float) cache[(int) num2].z);
      Gl.glVertex3f((float) cache[(int) num3].x, (float) cache[(int) num3].y, (float) cache[(int) num3].z);
      Gl.glEnd();
    }

    public static void GL_TRI1_PICKING(byte[] com, ref F3DEXVert[] cache, byte r, byte g, byte b)
    {
      short num1 = (short) ((int) com[1] / 2);
      short num2 = (short) ((int) com[2] / 2);
      short num3 = (short) ((int) com[3] / 2);
      Gl.glBegin(4);
      Gl.glColor3f((float) r / (float) byte.MaxValue, (float) g / (float) byte.MaxValue, (float) b / (float) byte.MaxValue);
      Gl.glVertex3f((float) cache[(int) num1].x, (float) cache[(int) num1].y, (float) cache[(int) num1].z);
      Gl.glVertex3f((float) cache[(int) num2].x, (float) cache[(int) num2].y, (float) cache[(int) num2].z);
      Gl.glVertex3f((float) cache[(int) num3].x, (float) cache[(int) num3].y, (float) cache[(int) num3].z);
      Gl.glEnd();
    }

    public static void GL_TRI2(byte[] com, ref F3DEXVert[] cache, ref F3DEXTexture texture)
    {
      short num1 = (short) ((int) com[1] / 2);
      short num2 = (short) ((int) com[2] / 2);
      short num3 = (short) ((int) com[3] / 2);
      short num4 = (short) ((int) com[5] / 2);
      short num5 = (short) ((int) com[6] / 2);
      short num6 = (short) ((int) com[7] / 2);
      Gl.glBegin(4);
      Gl.glColor4f((float) cache[(int) num1].r / (float) byte.MaxValue, (float) cache[(int) num1].g / (float) byte.MaxValue, (float) cache[(int) num1].b / (float) byte.MaxValue, (float) cache[(int) num1].a / (float) byte.MaxValue);
      Gl.glTexCoord2f((float) cache[(int) num1].u * texture.textureWRatio, (float) cache[(int) num1].v * texture.textureHRatio);
      Gl.glVertex3f((float) cache[(int) num1].x, (float) cache[(int) num1].y, (float) cache[(int) num1].z);
      Gl.glColor4f((float) cache[(int) num2].r / (float) byte.MaxValue, (float) cache[(int) num2].g / (float) byte.MaxValue, (float) cache[(int) num2].b / (float) byte.MaxValue, (float) cache[(int) num2].a / (float) byte.MaxValue);
      Gl.glTexCoord2f((float) cache[(int) num2].u * texture.textureWRatio, (float) cache[(int) num2].v * texture.textureHRatio);
      Gl.glVertex3f((float) cache[(int) num2].x, (float) cache[(int) num2].y, (float) cache[(int) num2].z);
      Gl.glColor4f((float) cache[(int) num3].r / (float) byte.MaxValue, (float) cache[(int) num3].g / (float) byte.MaxValue, (float) cache[(int) num3].b / (float) byte.MaxValue, (float) cache[(int) num3].a / (float) byte.MaxValue);
      Gl.glTexCoord2f((float) cache[(int) num3].u * texture.textureWRatio, (float) cache[(int) num3].v * texture.textureHRatio);
      Gl.glVertex3f((float) cache[(int) num3].x, (float) cache[(int) num3].y, (float) cache[(int) num3].z);
      Gl.glColor4f((float) cache[(int) num4].r / (float) byte.MaxValue, (float) cache[(int) num4].g / (float) byte.MaxValue, (float) cache[(int) num4].b / (float) byte.MaxValue, (float) cache[(int) num4].a / (float) byte.MaxValue);
      Gl.glTexCoord2f((float) cache[(int) num4].u * texture.textureWRatio, (float) cache[(int) num4].v * texture.textureHRatio);
      Gl.glVertex3f((float) cache[(int) num4].x, (float) cache[(int) num4].y, (float) cache[(int) num4].z);
      Gl.glColor4f((float) cache[(int) num5].r / (float) byte.MaxValue, (float) cache[(int) num5].g / (float) byte.MaxValue, (float) cache[(int) num5].b / (float) byte.MaxValue, (float) cache[(int) num5].a / (float) byte.MaxValue);
      Gl.glTexCoord2f((float) cache[(int) num5].u * texture.textureWRatio, (float) cache[(int) num5].v * texture.textureHRatio);
      Gl.glVertex3f((float) cache[(int) num5].x, (float) cache[(int) num5].y, (float) cache[(int) num5].z);
      Gl.glColor4f((float) cache[(int) num6].r / (float) byte.MaxValue, (float) cache[(int) num6].g / (float) byte.MaxValue, (float) cache[(int) num6].b / (float) byte.MaxValue, (float) cache[(int) num6].a / (float) byte.MaxValue);
      Gl.glTexCoord2f((float) cache[(int) num6].u * texture.textureWRatio, (float) cache[(int) num6].v * texture.textureHRatio);
      Gl.glVertex3f((float) cache[(int) num6].x, (float) cache[(int) num6].y, (float) cache[(int) num6].z);
      Gl.glEnd();
    }

    public static void GL_TRI2_PICKING(byte[] com, ref F3DEXVert[] cache)
    {
      short num1 = (short) ((int) com[1] / 2);
      short num2 = (short) ((int) com[2] / 2);
      short num3 = (short) ((int) com[3] / 2);
      short num4 = (short) ((int) com[5] / 2);
      short num5 = (short) ((int) com[6] / 2);
      short num6 = (short) ((int) com[7] / 2);
      Gl.glBegin(4);
      Gl.glVertex3f((float) cache[(int) num1].x, (float) cache[(int) num1].y, (float) cache[(int) num1].z);
      Gl.glVertex3f((float) cache[(int) num2].x, (float) cache[(int) num2].y, (float) cache[(int) num2].z);
      Gl.glVertex3f((float) cache[(int) num3].x, (float) cache[(int) num3].y, (float) cache[(int) num3].z);
      Gl.glVertex3f((float) cache[(int) num4].x, (float) cache[(int) num4].y, (float) cache[(int) num4].z);
      Gl.glVertex3f((float) cache[(int) num5].x, (float) cache[(int) num5].y, (float) cache[(int) num5].z);
      Gl.glVertex3f((float) cache[(int) num6].x, (float) cache[(int) num6].y, (float) cache[(int) num6].z);
      Gl.glEnd();
    }

    public static void GL_TRI2_PICKING(byte[] com, ref F3DEXVert[] cache, byte r, byte g, byte b)
    {
      short num1 = (short) ((int) com[1] / 2);
      short num2 = (short) ((int) com[2] / 2);
      short num3 = (short) ((int) com[3] / 2);
      short num4 = (short) ((int) com[5] / 2);
      short num5 = (short) ((int) com[6] / 2);
      short num6 = (short) ((int) com[7] / 2);
      Gl.glBegin(4);
      Gl.glColor3f((float) r / (float) byte.MaxValue, (float) g / (float) byte.MaxValue, (float) b / (float) byte.MaxValue);
      Gl.glVertex3f((float) cache[(int) num1].x, (float) cache[(int) num1].y, (float) cache[(int) num1].z);
      Gl.glVertex3f((float) cache[(int) num2].x, (float) cache[(int) num2].y, (float) cache[(int) num2].z);
      Gl.glVertex3f((float) cache[(int) num3].x, (float) cache[(int) num3].y, (float) cache[(int) num3].z);
      Gl.glVertex3f((float) cache[(int) num4].x, (float) cache[(int) num4].y, (float) cache[(int) num4].z);
      Gl.glVertex3f((float) cache[(int) num5].x, (float) cache[(int) num5].y, (float) cache[(int) num5].z);
      Gl.glVertex3f((float) cache[(int) num6].x, (float) cache[(int) num6].y, (float) cache[(int) num6].z);
      Gl.glEnd();
    }

    public static void GL_TRI2_PICKING(
      byte[] com,
      ref F3DEXVert[] cache,
      byte r,
      byte g,
      byte b,
      byte r2,
      byte g2,
      byte b2)
    {
      short num1 = (short) ((int) com[1] / 2);
      short num2 = (short) ((int) com[2] / 2);
      short num3 = (short) ((int) com[3] / 2);
      short num4 = (short) ((int) com[5] / 2);
      short num5 = (short) ((int) com[6] / 2);
      short num6 = (short) ((int) com[7] / 2);
      Gl.glBegin(4);
      Gl.glColor3f((float) r / (float) byte.MaxValue, (float) g / (float) byte.MaxValue, (float) b / (float) byte.MaxValue);
      Gl.glVertex3f((float) cache[(int) num1].x, (float) cache[(int) num1].y, (float) cache[(int) num1].z);
      Gl.glVertex3f((float) cache[(int) num2].x, (float) cache[(int) num2].y, (float) cache[(int) num2].z);
      Gl.glVertex3f((float) cache[(int) num3].x, (float) cache[(int) num3].y, (float) cache[(int) num3].z);
      Gl.glColor3f((float) r2 / (float) byte.MaxValue, (float) g2 / (float) byte.MaxValue, (float) b2 / (float) byte.MaxValue);
      Gl.glVertex3f((float) cache[(int) num4].x, (float) cache[(int) num4].y, (float) cache[(int) num4].z);
      Gl.glVertex3f((float) cache[(int) num5].x, (float) cache[(int) num5].y, (float) cache[(int) num5].z);
      Gl.glVertex3f((float) cache[(int) num6].x, (float) cache[(int) num6].y, (float) cache[(int) num6].z);
      Gl.glEnd();
    }

    public static void GL_EndDL(int cmdno, int count)
    {
      int num = cmdno + 4;
    }

    public static void GL_G_SETTIMG(
      ref int currentTexture,
      int textureCount,
      uint w1,
      ref F3DEXTexture[] textures,
      byte[] commandCheck,
      ref bool newTexture,
      float sScale,
      float tScale)
    {
      uint num = w1 << 8 >> 8;
      bool flag = false;
      for (int index = 0; index < textureCount && !flag; ++index)
      {
        if ((int) textures[index].textureOffset == (int) num || (int) textures[index].indexOffset == (int) num)
        {
          currentTexture = index;
          flag = true;
        }
      }
      if (commandCheck[0] == (byte) 240)
        return;
      newTexture = true;
      textures[currentTexture].setRatio(sScale, tScale);
    }

    public static void GL_G_Combine(uint w1)
    {
      if (w1 == 1058404863U)
        Gl.glDisable(3553);
      else
        Gl.glEnable(3553);
    }

    public static void GL_SETGEOMETRYMODE(uint w1)
    {
      Gl.glDisable(2884);
      int num = (int) ((uint) (((int) w1 & 16777215) << 8) >> 8);
      bool flag1 = (uint) (num & 4096) > 0U;
      bool flag2 = (uint) (num & 8192) > 0U;
      bool flag3 = (uint) (num & 12288) > 0U;
      if (flag1)
        Gl.glCullFace(1028);
      if (flag2)
        Gl.glCullFace(1029);
      if (flag2 & flag1)
        Gl.glCullFace(1032);
      if (!(flag1 | flag2 | flag3))
        return;
      Gl.glEnable(2884);
    }

    public static void GL_G_SETTILE(byte[] command)
    {
      int num1 = (int) command[4] * 16777216 + (int) command[5] * 65536 + (int) command[6] * 256 + (int) command[7];
      int num2 = (int) command[1] * 65536 + (int) command[2] * 256 + (int) command[3];
      Gl.glEnable(3553);
      Tile.textureFormat = (int) (byte) ((uint) command[1] >> 5);
      Tile.texelSize = (int) command[1] >> 3 & 3;
      Tile.lineSize = (int) ((uint) num2 >> 9) & 15;
      Tile.cmt = (int) ((uint) num1 >> 18) & 2;
      Tile.cms = (int) ((uint) num1 >> 8) & 3;
    }

    public static void GL_G_LOADTLUT()
    {
    }
  }
}
