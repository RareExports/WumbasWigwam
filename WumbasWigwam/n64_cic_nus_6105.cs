// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.n64_cic_nus_6105
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public static class n64_cic_nus_6105
  {
    private static sbyte[] lut0 = new sbyte[16]
    {
      (sbyte) 4,
      (sbyte) 7,
      (sbyte) 10,
      (sbyte) 7,
      (sbyte) 14,
      (sbyte) 5,
      (sbyte) 14,
      (sbyte) 1,
      (sbyte) 12,
      (sbyte) 15,
      (sbyte) 8,
      (sbyte) 15,
      (sbyte) 6,
      (sbyte) 3,
      (sbyte) 6,
      (sbyte) 9
    };
    private static sbyte[] lut1 = new sbyte[16]
    {
      (sbyte) 4,
      (sbyte) 1,
      (sbyte) 10,
      (sbyte) 7,
      (sbyte) 14,
      (sbyte) 5,
      (sbyte) 14,
      (sbyte) 1,
      (sbyte) 12,
      (sbyte) 9,
      (sbyte) 8,
      (sbyte) 5,
      (sbyte) 6,
      (sbyte) 3,
      (sbyte) 12,
      (sbyte) 9
    };

    public static void GenerateCICResult(sbyte[] chl, sbyte[] rsp, int len)
    {
      sbyte num1 = 11;
      sbyte[] numArray = n64_cic_nus_6105.lut0;
      for (int index = 0; index < len; ++index)
      {
        rsp[index] = (sbyte) ((int) num1 + 5 * (int) chl[index] & 15);
        num1 = numArray[(int) rsp[index]];
        int num2 = (int) rsp[index] >> 3 & 1;
        int num3 = ((num2 == 1 ? (int) ~rsp[index] : (int) rsp[index]) & 7) % 3 == 1 ? num2 : 1 - num2;
        if (numArray == n64_cic_nus_6105.lut1 && (rsp[index] == (sbyte) 1 || rsp[index] == (sbyte) 9))
          num3 = 1;
        if (numArray == n64_cic_nus_6105.lut1 && (rsp[index] == (sbyte) 11 || rsp[index] == (sbyte) 14))
          num3 = 0;
        numArray = num3 == 1 ? n64_cic_nus_6105.lut1 : n64_cic_nus_6105.lut0;
      }
    }
  }
}
