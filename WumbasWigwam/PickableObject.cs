// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.PickableObject
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public class PickableObject
  {
    public byte[] m_colorID = new byte[3];
    private static byte[] gColorID = new byte[3]
    {
      (byte) 1,
      (byte) 0,
      (byte) 0
    };

    public PickableObject()
    {
      this.m_colorID[0] = PickableObject.gColorID[0];
      this.m_colorID[1] = PickableObject.gColorID[1];
      this.m_colorID[2] = PickableObject.gColorID[2];
      ++PickableObject.gColorID[0];
      if (PickableObject.gColorID[0] < byte.MaxValue)
        return;
      PickableObject.gColorID[0] = (byte) 0;
      ++PickableObject.gColorID[1];
      if (PickableObject.gColorID[1] < byte.MaxValue)
        return;
      PickableObject.gColorID[1] = (byte) 0;
      ++PickableObject.gColorID[2];
    }

    public static void reset()
    {
      PickableObject.gColorID[0] = (byte) 1;
      PickableObject.gColorID[1] = (byte) 1;
      PickableObject.gColorID[2] = (byte) 1;
    }
  }
}
