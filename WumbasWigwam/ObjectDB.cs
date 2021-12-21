// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.ObjectDB
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System.Collections.Generic;

namespace WumbasWigwam
{
  public static class ObjectDB
  {
    private static List<ObjectData> Objects = new List<ObjectData>();
    private static List<ObjectData> Structs = new List<ObjectData>();

    public static void InitializeData() => BBXML.readObjectsXML(ref ObjectDB.Objects, ref ObjectDB.Structs);

    public static ObjectData GetObject(string name)
    {
      ObjectData objectData = new ObjectData((short) 0, 0, (short) 0, (short) 0, (short) 0, (short) 0, (short) 0, (short) 0);
      for (int index = 0; index < ObjectDB.Objects.Count; ++index)
      {
        if (ObjectDB.Objects[index].name == name)
          return ObjectData.clone(ObjectDB.Objects[index]);
      }
      return objectData;
    }

    public static ObjectData GetStruct(string name)
    {
      ObjectData objectData = new ObjectData((short) 0, 0, (short) 0, (short) 0, (short) 0, (short) 0, (short) 0, (short) 0);
      for (int index = 0; index < ObjectDB.Structs.Count; ++index)
      {
        if (ObjectDB.Structs[index].name == name)
          objectData = ObjectData.clone(ObjectDB.Structs[index]);
      }
      return objectData;
    }

    public static void FillObjectDetails(ref ObjectData o)
    {
      o.modelFile = "";
      o.modelFile2 = "";
      o.jiggyID = -1;
      o.cameraID = -1;
      o.name = "";
      if (o.specialScript == (short) 20358)
      {
        o.name = "warp";
        o.modelFile = ".\\resources\\warp.mw";
        o.file = 1;
      }
      else
      {
        for (int index = 0; index < ObjectDB.Objects.Count; ++index)
        {
          if ((int) ObjectDB.Objects[index].objectID == (int) o.objectID)
          {
            o.file = ObjectDB.Objects[index].file;
            o.file2 = ObjectDB.Objects[index].file2;
            o.modelFile = ObjectDB.Objects[index].modelFile;
            o.modelFile2 = ObjectDB.Objects[index].modelFile2;
            o.jiggyID = ObjectDB.Objects[index].jiggyID;
            o.cameraID = ObjectDB.Objects[index].cameraID;
            o.name = ObjectDB.Objects[index].name;
          }
        }
      }
    }
  }
}
