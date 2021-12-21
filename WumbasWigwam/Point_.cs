// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.Point_
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WumbasWigwam
{
  public struct Point_
  {
    public float x;
    public float y;
    public float z;

    public Point_(float a, float b, float c)
    {
      this.x = a;
      this.y = b;
      this.z = c;
    }

    public static bool LineIntersectsRect(Point p1, Point p2, Rectangle r)
    {
      if (r.Y >= 0 && r.X >= 0 && r.Height >= 0 && r.Width >= 0 && (r.X < p1.X && r.Width > p1.X && r.Y < p1.Y && r.Height > p1.Y || r.X < p2.X && r.Width > p2.X && r.Y < p2.Y && r.Height > p2.Y) || r.Y >= 0 && r.X <= 0 && r.Height > 0 && (r.X > p1.X && r.Width < p1.X && r.Y < p1.Y && r.Height > p1.Y || r.X > p2.X && r.Width < p2.X && r.Y < p2.Y && r.Height > p2.Y) || r.Y >= 0 && r.X >= 0 && r.Height < 0 && r.Width >= 0 && (r.X < p1.X && r.Width > p1.X && r.Y > p1.Y && r.Height < p1.Y || r.X < p2.X && r.Width > p2.X && r.Y > p2.Y && r.Height < p2.Y) || r.Y >= 0 && r.X < 0 && r.Height > 0 && (r.X > p1.X && r.Width < p1.X && r.Y < p1.Y && r.Height > p1.Y || r.X > p2.X && r.Width < p2.X && r.Y < p2.Y && r.Height > p2.Y) || r.Y >= 0 && r.X < 0 && r.Height < 0 && (r.X > p1.X && r.Width < p1.X && r.Y > p1.Y && r.Height < p1.Y || r.X > p2.X && r.Width < p2.X && r.Y > p2.Y && r.Height < p2.Y) || r.Y < 0 && r.X >= 0 && r.Width > 0 && (r.X < p1.X && r.Width > p1.X && r.Y > p1.Y && r.Height < p1.Y || r.X < p2.X && r.Width > p2.X && r.Y > p2.Y && r.Height < p2.Y) || r.Y < 0 && r.X >= 0 && r.Width < 0 && (r.X > p1.X && r.Width < p1.X && r.Y > p1.Y && r.Height < p1.Y || r.X > p2.X && r.Width < p2.X && r.Y > p2.Y && r.Height < p2.Y) || r.Y < 0 && r.X < 0 && r.Width < 0 && r.Height < 0 && (r.X > p1.X && r.Width < p1.X && r.Y > p1.Y && r.Height < p1.Y || r.X > p2.X && r.Width < p2.X && r.Y > p2.Y && r.Height < p2.Y) || r.Y < 0 && r.X < 0 && r.Width > 0 && r.Height < 0 && (r.X < p1.X && r.X + r.Width > p1.X && r.Y > p1.Y && r.Height < p1.Y || r.X < p2.X && r.X + r.Width > p2.X && r.Y > p2.Y && r.Height < p2.Y) || r.Y < 0 && r.X < 0 && r.Width < 0 && r.Height > 0 && (r.X > p1.X && r.Width < p1.X && r.Y < p1.Y && r.Y + r.Height > p1.Y || r.X > p2.X && r.Width < p2.X && r.Y < p2.Y && r.Y + r.Height > p2.Y) || Point_.LineIntersectsLine(p1, p2, new Point(r.X, r.Y), new Point(r.X + r.Width, r.Y)) || Point_.LineIntersectsLine(p1, p2, new Point(r.X + r.Width, r.Y), new Point(r.X + r.Width, r.Y + r.Height)) || Point_.LineIntersectsLine(p1, p2, new Point(r.X + r.Width, r.Y + r.Height), new Point(r.X, r.Y + r.Height)) || Point_.LineIntersectsLine(p1, p2, new Point(r.X, r.Y + r.Height), new Point(r.X, r.Y)))
        return true;
      return r.Contains(p1) && r.Contains(p2);
    }

    private static bool LineIntersectsLine(Point l1p1, Point l1p2, Point l2p1, Point l2p2)
    {
      float num1 = (float) ((l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y));
      float num2 = (float) ((l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X));
      if ((double) num2 == 0.0)
        return false;
      float num3 = num1 / num2;
      float num4 = (float) ((l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y)) / num2;
      return (double) num3 >= 0.0 && (double) num3 <= 1.0 && (double) num4 >= 0.0 && (double) num4 <= 1.0;
    }

    public static Vector operator -(Point_ p1, Point_ p2)
    {
      Vector vector;
      vector.x = p1.x - p2.x;
      vector.y = p1.y - p2.y;
      vector.z = p1.z - p2.z;
      return vector;
    }

    public static Point_ operator +(Point_ p, Vector v)
    {
      Point_ point;
      point.x = p.x + v.x;
      point.y = p.y + v.y;
      point.z = p.z + v.z;
      return point;
    }

    public static Point_ operator +(Point_ p, Point_ p2)
    {
      Point_ point;
      point.x = p.x + p2.x;
      point.y = p.y + p2.y;
      point.z = p.z + p2.z;
      return point;
    }

    public static Point_ operator *(float scale, Point_ p)
    {
      Point_ point;
      point.x = scale * p.x;
      point.y = scale * p.y;
      point.z = scale * p.z;
      return point;
    }

    public static float Dot(Vector v1, Vector v2) => (float) ((double) v1.x * (double) v2.x + (double) v1.y * (double) v2.y + (double) v1.z * (double) v2.z);

    public static float Dot(Point_ p1, Vector v2) => (float) ((double) p1.x * (double) v2.x + (double) p1.y * (double) v2.y + (double) p1.z * (double) v2.z);

    private Vector ReduceToUnit(Vector v)
    {
      float num = (float) Math.Sqrt((double) v.x * (double) v.x + (double) v.y * (double) v.y + (double) v.z * (double) v.z);
      if ((double) num == 0.0)
        num = 1f;
      return new Vector()
      {
        x = v.x / num,
        y = v.y / num,
        z = v.z / num
      };
    }

    public static float getmin(List<Point_> points, Vector axis)
    {
      float num1 = float.MaxValue;
      for (int index = 0; index < points.Count<Point_>(); ++index)
      {
        float num2 = Point_.Dot(points[index], axis);
        if ((double) num2 < (double) num1)
          num1 = num2;
      }
      return num1;
    }

    public static float getmax(List<Point_> points, Vector axis)
    {
      float num1 = float.MinValue;
      for (int index = 0; index < points.Count<Point_>(); ++index)
      {
        float num2 = Point_.Dot(points[index], axis);
        if ((double) num2 > (double) num1)
          num1 = num2;
      }
      return num1;
    }

    public static bool isect(List<Point_> points1, List<Point_> points2, Vector axis) => (double) Point_.getmin(points1, axis) <= (double) Point_.getmax(points2, axis) && (double) Point_.getmax(points1, axis) >= (double) Point_.getmin(points2, axis);

    public static bool isectboxtri(float[] center, float[] r, float[][] triverts)
    {
      List<Point_> points1 = new List<Point_>();
      points1.Add(new Point_(center[0] + r[0], center[1] + r[1], center[2] + r[2]));
      points1.Add(new Point_(center[0] + r[0], center[1] + r[1], center[2] - r[2]));
      points1.Add(new Point_(center[0] + r[0], center[1] - r[1], center[2] + r[2]));
      points1.Add(new Point_(center[0] + r[0], center[1] - r[1], center[2] - r[2]));
      points1.Add(new Point_(center[0] - r[0], center[1] + r[1], center[2] + r[2]));
      points1.Add(new Point_(center[0] - r[0], center[1] + r[1], center[2] - r[2]));
      points1.Add(new Point_(center[0] - r[0], center[1] - r[1], center[2] + r[2]));
      points1.Add(new Point_(center[0] - r[0], center[1] - r[1], center[2] - r[2]));
      List<Point_> points2 = new List<Point_>();
      points2.Add(new Point_(triverts[0][0], triverts[0][1], triverts[0][2]));
      points2.Add(new Point_(triverts[1][0], triverts[1][1], triverts[1][2]));
      points2.Add(new Point_(triverts[2][0], triverts[2][1], triverts[2][2]));
      if (!Point_.isect(points1, points2, new Vector(1f, 0.0f, 0.0f)) || !Point_.isect(points1, points2, new Vector(0.0f, 1f, 0.0f)) || !Point_.isect(points1, points2, new Vector(0.0f, 0.0f, 1f)))
        return false;
      Vector vector = points2[1] - points2[0];
      Vector v2_1 = points2[2] - points2[1];
      Vector axis = Vector.Cross(vector, v2_1);
      if (!Point_.isect(points1, points2, axis))
        return false;
      Vector v2_2 = points2[0] - points2[2];
      Vector v1_1 = new Vector(1f, 0.0f, 0.0f);
      Vector v1_2 = new Vector(0.0f, 1f, 0.0f);
      Vector v1_3 = new Vector(0.0f, 0.0f, 1f);
      return Point_.isect(points1, points2, Vector.Cross(v1_1, vector)) && Point_.isect(points1, points2, Vector.Cross(v1_1, v2_1)) && Point_.isect(points1, points2, Vector.Cross(v1_1, v2_2)) && Point_.isect(points1, points2, Vector.Cross(v1_2, vector)) && Point_.isect(points1, points2, Vector.Cross(v1_2, v2_1)) && Point_.isect(points1, points2, Vector.Cross(v1_2, v2_2)) && Point_.isect(points1, points2, Vector.Cross(v1_3, vector)) && Point_.isect(points1, points2, Vector.Cross(v1_3, v2_1)) && Point_.isect(points1, points2, Vector.Cross(v1_3, v2_2));
    }
  }
}
