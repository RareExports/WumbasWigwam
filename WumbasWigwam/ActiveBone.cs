// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.ActiveBone
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public class ActiveBone
  {
    public byte bone;
    public int length;
    public int parentCMDBoneID = -1;

    public ActiveBone(byte b, int l)
    {
      this.bone = b;
      this.length = l;
    }
  }
}
