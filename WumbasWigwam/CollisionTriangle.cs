// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.CollisionTriangle
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public class CollisionTriangle
  {
    public int[] verts;
    public CollisionType collisionType;
    public GroundType groundType;
    public SoundType soundType;

    public CollisionTriangle(
      int[] verts_,
      CollisionType collisionType_,
      GroundType groundType_,
      SoundType soundType_)
    {
      this.verts = verts_;
      this.collisionType = collisionType_;
      this.groundType = groundType_;
      this.soundType = soundType_;
    }
  }
}
