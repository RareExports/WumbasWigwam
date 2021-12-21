// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.BBHelper
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System;

namespace WumbasWigwam
{
  public class BBHelper
  {
    public static int ByteArrayToInt32(byte[] bytes) => ((int) bytes[0] << 24) + ((int) bytes[1] << 16) + ((int) bytes[2] << 8) + (int) bytes[3];

    public static string HexToASCII(byte[] hex)
    {
      string str = "";
      for (int index = 0; index < hex.Length; ++index)
      {
        try
        {
          str += Convert.ToChar(hex[index]).ToString();
        }
        catch
        {
          str += " ";
        }
      }
      return str;
    }
  }
}
