// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.BTDecryption
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public static class BTDecryption
  {
    public static void DecryptBTFile(int fileNumber, byte[] input, byte[] output, int size)
    {
      sbyte[] rsp = new sbyte[32];
      byte[] numArray1 = new byte[16];
      short num1 = (short) (fileNumber - 2389);
      int num2 = 16;
      for (int index = 0; index < 14; index += 2)
      {
        int num3 = (int) num1 >> index;
        int num4 = num2 - index;
        int num5 = (int) num1 << num4;
        int num6 = num3 | num5;
        numArray1[index] = (byte) (num6 & (int) byte.MaxValue);
        numArray1[index + 1] = (byte) (num6 & 65280 & (int) byte.MaxValue);
      }
      numArray1[14] = (byte) 0;
      numArray1[15] = (byte) 2;
      sbyte[] chl = new sbyte[32];
      for (int index = 0; index < 32; ++index)
        chl[index] = index % 2 != 0 ? (sbyte) ((int) numArray1[index / 2] & 15) : (sbyte) ((int) numArray1[index / 2] >> 4 & 15);
      n64_cic_nus_6105.GenerateCICResult(chl, rsp, 30);
      rsp[30] = (sbyte) 0;
      rsp[31] = (sbyte) 0;
      byte[] numArray2 = new byte[16];
      for (int index = 0; index < 32; index += 2)
        numArray2[index / 2] = (byte) ((uint) (byte) rsp[index] << 4 | (uint) (byte) rsp[index + 1]);
      for (int index = 0; index < size; ++index)
      {
        byte num7 = input[index];
        output[index] = (byte) ((uint) num7 ^ (uint) numArray2[index % 14]);
      }
    }
  }
}
