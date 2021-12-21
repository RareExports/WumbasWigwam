// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.F3DEXTexture
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using NLog;
using System.Collections.Generic;
using System.Linq;

namespace WumbasWigwam
{
  public class F3DEXTexture
  {
    private static Logger logger = LogManager.GetCurrentClassLogger();
    public int textureSize;
    public int textureWidth;
    public int textureHeight;
    public float textureHRatio;
    public float textureWRatio;
    public uint textureOffset;
    public uint indexOffset;
    public int textureTableOffset;
    public int palSize;
    public bool external;
    public byte[] externalFile;
    public byte[] palette;
    public byte[] red;
    public byte[] green;
    public byte[] blue;
    public byte[] alpha;
    public bool palLoaded;
    public byte[] pixels;
    public byte[] n64TextureBytes;

    public F3DEXTexture(
      int textureTableOffset_,
      uint textureOffset_,
      int textureWidth_,
      int textureHeight_)
    {
      this.textureTableOffset = textureTableOffset_;
      this.textureOffset = textureOffset_;
      this.textureWidth = textureWidth_;
      this.textureHeight = textureHeight_;
      this.indexOffset = this.textureOffset + 32U;
      this.textureSize = this.textureWidth * this.textureHeight * 2;
    }

    public byte[] GetIndices() => ((IEnumerable<byte>) this.externalFile).Skip<byte>(this.palSize * 2).ToArray<byte>();

    public void setRatio(float sScale, float tScale)
    {
      this.textureHRatio = tScale / (float) (this.textureHeight << 5);
      this.textureWRatio = sScale / (float) (this.textureWidth << 5);
    }

    public void loadPalette(byte[] bytesInFile, int textureCount, int palSize)
    {
      this.palSize = palSize / 2;
      this.textureSize = palSize != 32 ? this.textureWidth * this.textureHeight : this.textureWidth * this.textureHeight / 2;
      if (this.palLoaded)
        return;
      this.palette = new byte[palSize];
      this.red = new byte[palSize / 2];
      this.green = new byte[palSize / 2];
      this.blue = new byte[palSize / 2];
      this.alpha = new byte[palSize / 2];
      this.indexOffset = (uint) ((ulong) this.textureOffset + (ulong) palSize);
      int index1 = 0;
      if (this.external)
      {
        for (uint index2 = 0; (long) index2 < (long) palSize; ++index2)
        {
          this.palette[index1] = this.externalFile[(int) index2];
          ++index1;
        }
      }
      else
      {
        for (uint textureOffset = this.textureOffset; (long) textureOffset < (long) this.textureOffset + (long) palSize; ++textureOffset)
        {
          this.palette[index1] = bytesInFile[(long) (textureOffset + 88U) + (long) (textureCount * 8)];
          ++index1;
        }
      }
      int index3 = 0;
      for (int index4 = 0; index4 < palSize / 2; ++index4)
      {
        this.red[index4] = this.palette[index3];
        this.red[index4] >>= 3;
        this.red[index4] *= (byte) 8;
        ushort num = (ushort) ((uint) (ushort) ((uint) (ushort) ((uint) (ushort) ((uint) this.palette[index3] * 256U + (uint) this.palette[index3 + 1]) << 5) >> 3) >> 8);
        this.green[index4] = (byte) num;
        this.green[index4] *= (byte) 8;
        this.blue[index4] = this.palette[index3 + 1];
        this.blue[index4] <<= 2;
        this.blue[index4] >>= 3;
        this.blue[index4] *= (byte) 8;
        this.alpha[index4] = this.palette[index3 + 1];
        this.alpha[index4] <<= 7;
        this.alpha[index4] >>= 7;
        this.alpha[index4] = this.alpha[index4] != (byte) 1 ? (byte) 0 : byte.MaxValue;
        index3 += 2;
      }
      this.palLoaded = true;
    }

    public void loadPixels(ref byte[] textureN64Bytes)
    {
      if (this.pixels == null)
      {
        switch (Tile.textureFormat)
        {
          case 0:
            if (Tile.texelSize == 2)
              this.pixels = ImageHandler.ConvertRGBA551ToRGBA8888(this.textureWidth, this.textureHeight, textureN64Bytes);
            if (Tile.texelSize == 3)
            {
              try
              {
                this.pixels = new byte[this.textureHeight * this.textureWidth * 4];
                for (int index = 0; index < this.pixels.Length; ++index)
                  this.pixels[index] = textureN64Bytes[index];
                break;
              }
              catch
              {
                F3DEXTexture.logger.Log(LogLevel.Error, "Calculated texture size wrong, continuing with loaded buffer");
                break;
              }
            }
            else
              break;
          case 2:
            if (Tile.texelSize == 0)
              this.pixels = ImageHandler.ConvertCI4ToRGBA8888(this, ref textureN64Bytes);
            if (Tile.texelSize == 1)
              this.pixels = ImageHandler.ConvertCI8ToRGBA8888(this, ref textureN64Bytes);
            if (Tile.texelSize == 2)
            {
              this.pixels = ImageHandler.ConvertCI4ToRGBA8888_2(this, ref textureN64Bytes);
              break;
            }
            break;
          case 3:
            if (Tile.texelSize == 0)
              this.pixels = ImageHandler.ConvertIA4ToRGBA8888(this, ref textureN64Bytes);
            if (Tile.texelSize == 1)
              this.pixels = ImageHandler.ConvertIA8ToRGBA8888(this, ref textureN64Bytes);
            if (Tile.texelSize == 2)
            {
              this.pixels = ImageHandler.ConvertIA16ToRGBA8888(this, ref textureN64Bytes);
              break;
            }
            break;
        }
        this.n64TextureBytes = textureN64Bytes;
      }
      byte[] pixels = this.pixels;
    }
  }
}
