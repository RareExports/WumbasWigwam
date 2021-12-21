// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.BKAnimationCommand
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System.Collections.Generic;

namespace WumbasWigwam
{
  public class BKAnimationCommand
  {
    public ushort unknown;
    public ushort frameNumber;
    public short transformFactor;

    public BKAnimationCommand(ushort unknown_, ushort frameNumber_, short transformFactor_)
    {
      this.unknown = unknown_;
      this.frameNumber = frameNumber_;
      this.transformFactor = transformFactor_;
    }

    public List<byte> CommandToByteArray() => new List<byte>()
    {
      (byte) ((uint) this.frameNumber >> 8),
      (byte) this.frameNumber,
      (byte) ((uint) this.transformFactor >> 8),
      (byte) this.transformFactor
    };
  }
}
