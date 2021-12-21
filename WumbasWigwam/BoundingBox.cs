// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.BoundingBox
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace WumbasWigwam
{
  public struct BoundingBox
  {
    public int smallX;
    public int smallY;
    public int smallZ;
    public int largeX;
    public int largeY;
    public int largeZ;

    public void SetSize(int size)
    {
      this.smallX = -size;
      this.smallY = -size;
      this.smallZ = -size;
      this.largeX = size;
      this.largeY = size;
      this.largeZ = size;
    }

    public int getSize() => ((IEnumerable<int>) new int[6]
    {
      Math.Abs(this.smallX),
      Math.Abs(this.smallY),
      Math.Abs(this.smallZ),
      this.largeX,
      this.largeY,
      this.largeZ
    }).Max();
  }
}
