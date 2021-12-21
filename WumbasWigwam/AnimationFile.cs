// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.AnimationFile
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System.Collections.Generic;

namespace WumbasWigwam
{
  public class AnimationFile
  {
    public int id;
    public int offset;
    public string name = "";
    public int pointer;
    public List<int> modelPointers = new List<int>();

    public AnimationFile(int id_, int offset_, string name_, List<int> modelPointers_)
    {
      this.id = id_;
      this.pointer = 20872 + this.id * 4;
      this.modelPointers = modelPointers_;
      this.offset = offset_;
      this.name = name_;
    }

    public override string ToString() => this.name;
  }
}
