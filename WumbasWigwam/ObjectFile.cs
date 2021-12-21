// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.ObjectFile
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System;
using System.Collections.Generic;

namespace WumbasWigwam
{
  public class ObjectFile
  {
    public static string result = "";
    private ObjectFileHeader objectHeader;
    private int animationOffset;
    private List<short> animations;
    private string internalName;
    private short effectID;
    private short objectID;
    private short model;
    private int checksum1;
    private int checksum2;

    public ObjectFile(byte[] header, byte[] file)
    {
      foreach (byte num in file)
      {
        this.checksum1 += (int) num;
        this.checksum2 ^= (int) num << (this.checksum1 & 23);
      }
      this.objectHeader = new ObjectFileHeader(header, this.checksum1, this.checksum2);
      int sourceIndex = 40 + ((int) (((long) this.objectHeader.DecodedHeader[2] & 4294901760L) >> 16) << 2);
      byte[] hex = new byte[16];
      Array.Copy((Array) file, sourceIndex, (Array) hex, 0, hex.Length);
      this.internalName = BBHelper.HexToASCII(hex);
      int index1 = (this.objectHeader.DecodedHeader[0] >> 16 << 4) + (((sourceIndex + ((this.objectHeader.DecodedHeader[3] & 65280) >> 8) + 3 & 65532) + ((this.objectHeader.DecodedHeader[2] & (int) ushort.MaxValue) << 1) + 3 & 65532) + ((int) ((long) this.objectHeader.DecodedHeader[3] & 4294901760L) >> 16 << 2) + 15 & 65520);
      for (int index2 = index1; index2 + 8 < file.Length; index2 += 8)
      {
        int num = 0;
        for (int index3 = 0; index3 < 8; ++index3)
          num += (int) file[index2 + index3];
        if (num == 0)
        {
          index1 = index2 + 8;
          break;
        }
      }
      if (file[index1] == (byte) 0)
      {
        for (int index4 = index1; index4 + 8 < file.Length; index4 += 8)
        {
          int num = 0;
          for (int index5 = 0; index5 < 8; ++index5)
            num += (int) file[index4 + index5];
          if (num == 0)
          {
            index1 = index4 + 8;
            break;
          }
        }
      }
      List<short> shortList = new List<short>();
      for (int index6 = index1; index1 + 8 < file.Length && file[index6] != (byte) 0; index6 += 8)
        shortList.Add((short) (((int) file[index6 + 4] << 8) + (int) file[index6 + 5]));
      ObjectFile.result = ObjectFile.result + this.internalName + " ";
      foreach (short num in shortList)
        ObjectFile.result = ObjectFile.result + num.ToString("x") + " ";
      ObjectFile.result += Environment.NewLine;
    }
  }
}
