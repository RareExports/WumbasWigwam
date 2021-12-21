// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.Vertex
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public class Vertex
  {
    public bool animateVertColour;
    public bool scrollTexture;
    public bool scrollTextureSlow;
    public bool scrollTextureFast;
    public float X;
    public float Y;
    public float Z;
    public short x;
    public short y;
    public short z;
    public float u;
    public float v;
    public byte r;
    public byte g;
    public byte b;
    public byte a;

    public Vertex(
      short x_,
      short y_,
      short z_,
      byte r_,
      byte g_,
      byte b_,
      byte a_,
      float u_,
      float v_)
    {
      this.X = (float) x_;
      this.Y = (float) y_;
      this.Z = (float) z_;
      this.x = x_;
      this.y = y_;
      this.z = z_;
      this.r = r_;
      this.g = g_;
      this.b = b_;
      this.a = a_;
      this.u = u_;
      this.v = v_;
    }

    public Vertex(
      float X_,
      float Y_,
      float Z_,
      short x_,
      short y_,
      short z_,
      byte r_,
      byte g_,
      byte b_,
      byte a_,
      float u_,
      float v_)
    {
      this.X = X_;
      this.Y = Y_;
      this.Z = Z_;
      this.x = x_;
      this.y = y_;
      this.z = z_;
      this.r = r_;
      this.g = g_;
      this.b = b_;
      this.a = a_;
      this.u = u_;
      this.v = v_;
    }

    public void Transform(float scale, int x_, int y_, int z_)
    {
      this.x = (short) (((double) this.X + (double) x_) * (double) scale);
      this.y = (short) (((double) this.Y + (double) y_) * (double) scale);
      this.z = (short) (((double) this.Z + (double) z_) * (double) scale);
    }

    public void UpdateColor(byte r_, byte g_, byte b_, byte a_)
    {
      this.r = r_;
      this.g = g_;
      this.b = b_;
      this.a = a_;
    }

    public static Vertex clone(Vertex v) => new Vertex(v.X, v.Y, v.Z, v.x, v.y, v.z, v.r, v.g, v.b, v.a, v.u, v.v);

    public override bool Equals(object obj)
    {
      Vertex vertex = obj as Vertex;
      byte num = 3;
      return (int) this.x == (int) vertex.x && (int) this.y == (int) vertex.y && (int) this.z == (int) vertex.z && (double) this.u == (double) vertex.u && (double) this.v == (double) vertex.v && (int) this.r >= (int) vertex.r - (int) num && (int) this.r <= (int) vertex.r + (int) num && (int) this.g >= (int) vertex.g - (int) num && (int) this.g <= (int) vertex.g + (int) num && (int) this.b >= (int) vertex.b - (int) num && (int) this.b <= (int) vertex.b + (int) num && (int) this.a >= (int) vertex.a - (int) num && (int) this.a <= (int) vertex.a + (int) num;
    }
  }
}
