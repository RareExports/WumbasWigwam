// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.ModelDrawCommand
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System.Collections.Generic;

namespace WumbasWigwam
{
  public class ModelDrawCommand
  {
    public DrawCommandType CommandType = DrawCommandType.DrawDistance;
    public List<byte> command = new List<byte>();

    public static ModelDrawCommand CreateDrawDistanceCommand(
      short minX,
      short minY,
      short minZ,
      short maxX,
      short maxY,
      short maxZ)
    {
      ModelDrawCommand modelDrawCommand = new ModelDrawCommand();
      modelDrawCommand.CommandType = DrawCommandType.DrawDistance;
      List<byte> command = modelDrawCommand.command;
      byte[] numArray = new byte[8];
      numArray[3] = (byte) 13;
      numArray[7] = (byte) 40;
      command.AddRange((IEnumerable<byte>) numArray);
      modelDrawCommand.command.Add((byte) ((uint) minX >> 8));
      modelDrawCommand.command.Add((byte) minX);
      modelDrawCommand.command.Add((byte) ((uint) minY >> 8));
      modelDrawCommand.command.Add((byte) minY);
      modelDrawCommand.command.Add((byte) ((uint) minZ >> 8));
      modelDrawCommand.command.Add((byte) minZ);
      modelDrawCommand.command.Add((byte) ((uint) maxX >> 8));
      modelDrawCommand.command.Add((byte) maxX);
      modelDrawCommand.command.Add((byte) ((uint) maxY >> 8));
      modelDrawCommand.command.Add((byte) maxY);
      modelDrawCommand.command.Add((byte) ((uint) maxZ >> 8));
      modelDrawCommand.command.Add((byte) maxZ);
      modelDrawCommand.command.Add((byte) 0);
      modelDrawCommand.command.Add((byte) 24);
      modelDrawCommand.command.Add((byte) 8);
      modelDrawCommand.command.Add((byte) 211);
      return modelDrawCommand;
    }

    public static ModelDrawCommand CreateDisplayListCommand(
      short displayListPointer,
      bool link)
    {
      ModelDrawCommand modelDrawCommand = new ModelDrawCommand();
      modelDrawCommand.CommandType = DrawCommandType.DisplayList;
      List<byte> command = modelDrawCommand.command;
      byte[] numArray = new byte[8];
      numArray[3] = (byte) 3;
      command.AddRange((IEnumerable<byte>) numArray);
      modelDrawCommand.command.Add((byte) ((uint) displayListPointer >> 8));
      modelDrawCommand.command.Add((byte) displayListPointer);
      modelDrawCommand.command.Add((byte) 0);
      modelDrawCommand.command.Add((byte) 0);
      modelDrawCommand.command.Add((byte) 0);
      modelDrawCommand.command.Add((byte) 0);
      modelDrawCommand.command.Add((byte) 0);
      modelDrawCommand.command.Add((byte) 0);
      if (link)
        modelDrawCommand.command[7] = (byte) 16;
      return modelDrawCommand;
    }

    public static ModelDrawCommand CreateBoneCommand(short boneID, int length)
    {
      ModelDrawCommand modelDrawCommand = new ModelDrawCommand();
      modelDrawCommand.CommandType = DrawCommandType.Bone;
      List<byte> command = modelDrawCommand.command;
      byte[] numArray = new byte[8];
      numArray[3] = (byte) 2;
      command.AddRange((IEnumerable<byte>) numArray);
      modelDrawCommand.command.Add((byte) 16);
      modelDrawCommand.command.Add((byte) boneID);
      modelDrawCommand.command.Add((byte) 0);
      modelDrawCommand.command.Add((byte) 0);
      modelDrawCommand.command.Add((byte) 0);
      modelDrawCommand.command.Add((byte) 0);
      modelDrawCommand.command.Add((byte) 0);
      modelDrawCommand.command.Add((byte) 0);
      if (length > 0)
      {
        modelDrawCommand.command[6] = (byte) (length >> 8);
        modelDrawCommand.command[7] = (byte) length;
      }
      return modelDrawCommand;
    }
  }
}
