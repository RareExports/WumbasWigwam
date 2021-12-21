// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.BKPath
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System.Collections.Generic;

namespace WumbasWigwam
{
  public class BKPath : PickableObject
  {
    public int pathObject = -1;
    public List<ObjectData> nodes = new List<ObjectData>();

    public static BKPath clone(BKPath p)
    {
      BKPath bkPath = new BKPath();
      bkPath.pathObject = p.pathObject;
      foreach (ObjectData node in p.nodes)
        bkPath.nodes.Add(ObjectData.fullClone(node));
      return bkPath;
    }

    public bool IsPathLooped()
    {
      bool flag = true;
      foreach (ObjectData node in this.nodes)
      {
        if (node.nodeOutUID == (short) 0)
          return false;
      }
      return flag;
    }

    public int GetFirstNode()
    {
      int num = 0;
      foreach (ObjectData node in this.nodes)
      {
        if (!node.node_in)
          return num;
        ++num;
      }
      return -1;
    }
  }
}
