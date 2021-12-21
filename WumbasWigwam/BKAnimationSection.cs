// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.BKAnimationSection
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System.Collections.Generic;
using System.Linq;

namespace WumbasWigwam
{
  public class BKAnimationSection
  {
    public ushort boneDL;
    public TransformationType tranformationType;
    public List<BKAnimationCommand> commands = new List<BKAnimationCommand>();

    public BKAnimationSection(ushort boneID_, TransformationType transformationType_)
    {
      this.boneDL = boneID_;
      this.tranformationType = transformationType_;
    }

    public List<byte> SectionToByteArray()
    {
      List<byte> byteList = new List<byte>();
      int num = (int) (((int) this.boneDL << 4) + this.tranformationType);
      byteList.Add((byte) (num >> 8));
      byteList.Add((byte) num);
      byteList.Add((byte) (this.commands.Count<BKAnimationCommand>() >> 8));
      byteList.Add((byte) this.commands.Count<BKAnimationCommand>());
      foreach (BKAnimationCommand command in this.commands)
        byteList.AddRange((IEnumerable<byte>) command.CommandToByteArray());
      return byteList;
    }
  }
}
