// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.F3DEXVert
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public class F3DEXVert
  {
    public short x;
    public short y;
    public short z;
    public short u;
    public short v;
    public byte r;
    public byte g;
    public byte b;
    public byte a;

    public F3DEXVert(
      short x_,
      short y_,
      short z_,
      short u_,
      short v_,
      byte r_,
      byte g_,
      byte b_,
      byte a_)
    {
      this.x = x_;
      this.y = y_;
      this.z = z_;
      this.u = u_;
      this.v = v_;
      this.r = r_;
      this.g = g_;
      this.b = b_;
      this.a = a_;
    }

    public static F3DEXVert Clone(F3DEXVert v) => new F3DEXVert(v.x, v.y, v.z, v.u, v.v, v.r, v.g, v.b, v.a);
  }
}
