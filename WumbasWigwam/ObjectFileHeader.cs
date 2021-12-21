// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.ObjectFileHeader
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public class ObjectFileHeader
  {
    private byte[] originalHeader;
    public int[] DecodedHeader = new int[4];

    public ObjectFileHeader(byte[] header, int chk1, int chk2)
    {
      if (header.Length != 16)
        return;
      int index1 = 0;
      int index2 = 0;
      while (index1 < 16)
      {
        this.DecodedHeader[index2] = ((int) header[index1] << 24) + ((int) header[index1 + 1] << 16) + ((int) header[index1 + 2] << 8) + (int) header[index1 + 3];
        index1 += 4;
        ++index2;
      }
      this.DecodedHeader[0] ^= chk1;
      this.DecodedHeader[2] ^= chk2;
    }
  }
}
