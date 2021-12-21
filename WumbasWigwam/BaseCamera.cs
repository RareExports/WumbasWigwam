// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.BaseCamera
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public class BaseCamera
  {
    public byte[] m_colorID = new byte[3];
    private static byte[] gColorID = new byte[3]
    {
      (byte) 1,
      (byte) 0,
      (byte) 0
    };

    public BaseCamera()
    {
      this.m_colorID[0] = BaseCamera.gColorID[0];
      this.m_colorID[1] = BaseCamera.gColorID[1];
      this.m_colorID[2] = BaseCamera.gColorID[2];
      ++BaseCamera.gColorID[0];
      if (BaseCamera.gColorID[0] < byte.MaxValue)
        return;
      BaseCamera.gColorID[0] = (byte) 0;
      ++BaseCamera.gColorID[1];
      if (BaseCamera.gColorID[1] < byte.MaxValue)
        return;
      BaseCamera.gColorID[1] = (byte) 0;
      ++BaseCamera.gColorID[2];
    }
  }
}
