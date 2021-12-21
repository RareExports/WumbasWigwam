// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.ErasedObject
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System;

namespace WumbasWigwam
{
  internal class ErasedObject : IComparable
  {
    public int address;
    public int type;

    public ErasedObject(int address_, int type_)
    {
      this.address = address_;
      this.type = type_;
    }

    public int CompareTo(object obj) => this.address.CompareTo(((ErasedObject) obj).address);
  }
}
