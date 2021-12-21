// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.SetupFile
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public class SetupFile
  {
    public string name = "";
    public int pointer;
    public int modelAPointer;
    public int modelBPointer;
    public int sceneID;
    public int levelID;
    public BoundingBox bounds;
    public BoundingBox boundsAlphaModel;

    public SetupFile(string name, int pointer, int sceneID, int modelAPointer, int modelBPointer)
    {
      this.name = name;
      this.pointer = pointer;
      this.modelAPointer = modelAPointer;
      this.modelBPointer = modelBPointer;
      this.sceneID = sceneID;
    }
  }
}
