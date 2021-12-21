// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.Vector
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public struct Vector
  {
    public float x;
    public float y;
    public float z;

    public Vector(float a, float b, float c)
    {
      this.x = a;
      this.y = b;
      this.z = c;
    }

    public static Vector operator -(Vector v1, Vector v2)
    {
      Vector vector;
      vector.x = v1.x - v2.x;
      vector.y = v1.y - v2.y;
      vector.z = v1.z - v2.z;
      return vector;
    }

    public static Vector operator *(float scale, Vector v)
    {
      Vector vector;
      vector.x = scale * v.x;
      vector.y = scale * v.y;
      vector.z = scale * v.z;
      return vector;
    }

    public static Vector Cross(Vector v1, Vector v2)
    {
      Vector vector;
      vector.x = (float) ((double) v1.y * (double) v2.z - (double) v2.y * (double) v1.z);
      vector.y = (float) ((double) v1.z * (double) v2.x - (double) v1.x * (double) v2.z);
      vector.z = (float) ((double) v1.x * (double) v2.y - (double) v1.y * (double) v2.x);
      return vector;
    }
  }
}
