// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.RomHandler
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WumbasWigwam
{
  public static class RomHandler
  {
    public const int TABLE_SETUPS_START = 38776;
    private static byte[] rom;
    public static string tmpDir = "";

    public static byte[] Rom
    {
      set => RomHandler.rom = value;
      get => RomHandler.rom;
    }

    private static void DumpAllObjectFiles()
    {
      foreach (int address in ((IEnumerable<string>) Directory.GetFiles("C:\\Users\\admin\\Documents\\BTObjects")).Select<string, string>((Func<string, string>) (f => Path.GetFileName(f))).Select<string, string>((Func<string, string>) (f => f.Substring(0, f.Length - 4))).Select<string, int>((Func<string, int>) (f => Convert.ToInt32(f, 16))).ToArray<int>())
      {
        if (address > 32254420)
        {
          try
          {
            ObjectFile.result = ObjectFile.result + address.ToString("x") + " ";
            RomHandler.DecompressFileEOR(address);
          }
          catch
          {
          }
        }
      }
      File.WriteAllText("C:\\Users\\admin\\Documents\\BTObjects\\data.txt", ObjectFile.result);
    }

    private static void DecompressFileEOR(int address)
    {
      byte[] header = new byte[16];
      for (int index = 0; index < header.Length; ++index)
        header[index] = RomHandler.rom[address - 16 + index];
      int compressedSize = 2720;
      byte[] Buffer = new byte[compressedSize];
      for (int index = 0; index < compressedSize; ++index)
        Buffer[index] = RomHandler.rom[address + index];
      GECompression geCompression = new GECompression();
      geCompression.SetCompressedBuffer(Buffer, Buffer.Length);
      int fileSize = 0;
      byte[] file = geCompression.OutputDecompressedBuffer(ref fileSize, ref compressedSize);
      ObjectFile objectFile = new ObjectFile(header, file);
    }

    public static int getNextPointer(int pntr_)
    {
      int num1 = (int) RomHandler.rom[pntr_] * 16777216 + (int) RomHandler.rom[pntr_ + 1] * 65536 + (int) RomHandler.rom[pntr_ + 2] * 256 + (int) RomHandler.rom[pntr_ + 3];
      int num2 = (int) RomHandler.rom[pntr_ + 8] * 16777216 + (int) RomHandler.rom[pntr_ + 1 + 8] * 65536 + (int) RomHandler.rom[pntr_ + 2 + 8] * 256 + (int) RomHandler.rom[pntr_ + 3 + 8];
      while (num1 - num2 == 0)
      {
        num2 = (int) RomHandler.rom[pntr_ + 8] * 16777216 + (int) RomHandler.rom[pntr_ + 1 + 8] * 65536 + (int) RomHandler.rom[pntr_ + 2 + 8] * 256 + (int) RomHandler.rom[pntr_ + 3 + 8];
        pntr_ += 8;
      }
      return num2;
    }

    public static byte[] DecompressFileToByteArray(int pntr)
    {
      int num = ((int) RomHandler.rom[pntr] * 16777216 + (int) RomHandler.rom[pntr + 1] * 65536 + (int) RomHandler.rom[pntr + 2] * 256 + (int) RomHandler.rom[pntr + 3] >> 8) * 4 + 76580;
      int compressedSize = (RomHandler.getNextPointer(pntr) >> 8) * 4 + 76580 - num;
      byte[] Buffer = new byte[compressedSize];
      for (int index = 0; index < compressedSize; ++index)
        Buffer[index] = RomHandler.rom[num + index];
      GECompression geCompression = new GECompression();
      geCompression.SetCompressedBuffer(Buffer, Buffer.Length);
      int fileSize = 0;
      return geCompression.OutputDecompressedBuffer(ref fileSize, ref compressedSize);
    }

    public static byte[] DecompressEncryptedFileToByteArray(int pntr)
    {
      int num = ((int) RomHandler.rom[pntr] * 16777216 + (int) RomHandler.rom[pntr + 1] * 65536 + (int) RomHandler.rom[pntr + 2] * 256 + (int) RomHandler.rom[pntr + 3] >> 8) * 4 + 76580;
      int compressedSize = (RomHandler.getNextPointer(pntr) >> 8) * 4 + 76580 - num;
      byte[] numArray = new byte[compressedSize];
      for (int index = 0; index < compressedSize; ++index)
        numArray[index] = RomHandler.rom[num + index];
      BTDecryption.DecryptBTFile((pntr - 20872) / 4, numArray, numArray, numArray.Length);
      GECompression geCompression = new GECompression();
      geCompression.SetCompressedBuffer(numArray, numArray.Length);
      int fileSize = 0;
      return geCompression.OutputDecompressedBuffer(ref fileSize, ref compressedSize);
    }

    public static byte[] GetDecompressedFile(int pntr, int length)
    {
      byte[] numArray = new byte[length];
      for (int index = 0; index < length; ++index)
        numArray[index] = RomHandler.rom[pntr + index];
      return numArray;
    }

    public static void DecompressFileToHDD(int pntr)
    {
      if (File.Exists(RomHandler.tmpDir + pntr.ToString("x")))
        return;
      try
      {
        byte[] byteArray = RomHandler.DecompressFileToByteArray(pntr);
        FileStream fileStream = File.Create(RomHandler.tmpDir + pntr.ToString("x"));
        BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream);
        try
        {
          binaryWriter.Write(byteArray);
          binaryWriter.Close();
          fileStream.Close();
        }
        catch
        {
          binaryWriter.Close();
          fileStream.Close();
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static byte[] DecompressTextureFile(int fileNo)
    {
      int pntr = (fileNo + 7926 << 2) + 20872;
      try
      {
        return RomHandler.DecompressFileToByteArray(pntr);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
