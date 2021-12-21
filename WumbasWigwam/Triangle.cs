// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.Triangle
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public class Triangle
  {
    public string geometryID = "";
    public int boneId = -1;
    public TriangleColor color;
    public ushort[] verts = new ushort[3];
    public int textureSetting = -1;
    public CollisionType collisionType;
    public GroundType groundType;
    public SoundType soundType;
    public bool genWave;

    public Triangle() => this.color = new TriangleColor();

    public Triangle(ushort[] verts_, int textureSetting_)
    {
      this.verts = verts_;
      this.textureSetting = textureSetting_;
      this.color = new TriangleColor();
    }

    public Triangle(ushort[] verts_, int textureSetting_, TriangleColor pickObj_)
    {
      this.verts = verts_;
      this.textureSetting = textureSetting_;
      this.color = pickObj_;
    }

    public static Triangle clone(Triangle t) => new Triangle(new ushort[3]
    {
      t.verts[0],
      t.verts[1],
      t.verts[2]
    }, t.textureSetting);
  }
}
