// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.DrawCommand
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public class DrawCommand
  {
    public byte cmdNo;
    public int commandLength;
    public int dlStartOffset;
    public int dlEndOffset;
    public int boneID = -1;
    public bool nested;

    public DrawCommand(
      byte cmdNo_,
      int cmdLength_,
      int dlStartOffset_,
      int dlEndOffset_,
      int boneID_,
      bool nested_)
    {
      this.cmdNo = cmdNo_;
      this.commandLength = cmdLength_;
      this.dlStartOffset = dlStartOffset_;
      this.dlEndOffset = dlEndOffset_;
      this.boneID = boneID_;
      this.nested = nested_;
    }
  }
}
