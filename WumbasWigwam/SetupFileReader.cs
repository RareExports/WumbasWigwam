// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.SetupFileReader
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WumbasWigwam
{
  internal class SetupFileReader
  {
    private string myNewSetup = "";

    public void init(string file_) => this.myNewSetup = file_;

    public List<ObjectData> ReadSetupFile(byte[] setupfile)
    {
      List<ObjectData> objectDataList = new List<ObjectData>();
      try
      {
        int length = setupfile.Length;
        bool flag = false;
        try
        {
          int num1 = 1;
          int index = 6;
          while (index < setupfile.Length)
          {
            if (!flag)
            {
              byte num2 = setupfile[index];
              if (setupfile[index] == (byte) 7 && setupfile[index + 1] == (byte) 10)
              {
                num1 = ((int) setupfile[index + 4] << 8) + (int) setupfile[index + 5];
                flag = true;
                index += 6;
              }
              else
              {
                switch (num2)
                {
                  case 0:
                    ++index;
                    continue;
                  case 1:
                  case 2:
                  case 8:
                  case 9:
                  case 11:
                  case 12:
                  case 13:
                  case 14:
                  case 15:
                  case 16:
                  case 18:
                  case 19:
                    index += 5;
                    continue;
                  case 3:
                  case 4:
                  case 5:
                  case 17:
                    index += 13;
                    continue;
                  case 6:
                  case 7:
                    index += 9;
                    continue;
                  default:
                    continue;
                }
              }
            }
            else if (objectDataList.Count < num1)
            {
              switch ((byte) ((uint) (short) ((int) setupfile[index + 6] * 256 + (int) setupfile[index + 7]) & (uint) sbyte.MaxValue))
              {
                case 6:
                case 8:
                case 12:
                case 14:
                case 16:
                case 18:
                case 20:
                  int num3 = (int) (short) ((int) setupfile[index + 8] * 256 + (int) setupfile[index + 9]);
                  int num4 = index;
                  short num5 = (short) ((int) setupfile[index] * 256 + (int) setupfile[index + 1]);
                  short num6 = (short) ((int) setupfile[index + 2] * 256 + (int) setupfile[index + 3]);
                  short num7 = (short) ((int) setupfile[index + 4] * 256 + (int) setupfile[index + 5]);
                  short num8 = (short) ((int) setupfile[index + 12] * 2);
                  short num9 = 100;
                  short num10 = (short) ((int) setupfile[index + 6] * 256 + (int) setupfile[index + 7]);
                  int address_ = num4;
                  int num11 = (int) num5;
                  int num12 = (int) num6;
                  int num13 = (int) num7;
                  int num14 = (int) num8;
                  int num15 = (int) num9;
                  int num16 = (int) num10;
                  int num17 = (int) setupfile[index + 10];
                  int num18 = (int) setupfile[index + 11];
                  int num19 = (int) setupfile[index + 13];
                  int num20 = (int) setupfile[index + 16];
                  int num21 = (int) setupfile[index + 17];
                  int num22 = (int) setupfile[index + 18];
                  int num23 = (int) setupfile[index + 19];
                  ObjectData o = new ObjectData((short) num3, address_, (short) num11, (short) num12, (short) num13, (short) num14, (short) num15, (short) num16, (byte) num17, (byte) num18, (byte) num19, (byte) num20, (byte) num21, (byte) num22, (byte) num23);
                  ObjectDB.FillObjectDetails(ref o);
                  objectDataList.Add(o);
                  break;
              }
              index += 20;
            }
            else
              break;
          }
        }
        catch
        {
        }
      }
      catch
      {
      }
      return objectDataList;
    }

    public List<ObjectData> ReadSetupFile()
    {
      if (!File.Exists(this.myNewSetup))
        return new List<ObjectData>();
      try
      {
        BinaryReader binaryReader = new BinaryReader((Stream) File.Open(this.myNewSetup, FileMode.Open));
        long length = binaryReader.BaseStream.Length;
        byte[] numArray = new byte[length];
        binaryReader.BaseStream.Read(numArray, 0, (int) length);
        return this.ReadSetupFile(numArray);
      }
      catch
      {
        return new List<ObjectData>();
      }
    }

    public List<CameraObject> RipCameras(List<byte> camBytes)
    {
      List<CameraObject> cameraObjectList = new List<CameraObject>();
      try
      {
        if (camBytes[3] != (byte) 4)
        {
          int index1 = 2;
          while (index1 + 32 < camBytes.Count<byte>())
          {
            short id_ = (short) ((int) camBytes[index1 + 1] * 256 + (int) camBytes[index1 + 2]);
            int camByte = (int) camBytes[index1 + 4];
            if (camBytes[index1] == (byte) 1)
            {
              switch (camByte)
              {
                case 1:
                case 2:
                case 3:
                  int index2 = index1 + 6;
                  byte[] numArray1 = new byte[4]
                  {
                    camBytes[index2 + 3],
                    camBytes[index2 + 2],
                    camBytes[index2 + 1],
                    camBytes[index2]
                  };
                  int index3 = index2 + 4;
                  byte[] numArray2 = new byte[4]
                  {
                    camBytes[index3 + 3],
                    camBytes[index3 + 2],
                    camBytes[index3 + 1],
                    camBytes[index3]
                  };
                  int index4 = index3 + 4;
                  byte[] numArray3 = new byte[4]
                  {
                    camBytes[index4 + 3],
                    camBytes[index4 + 2],
                    camBytes[index4 + 1],
                    camBytes[index4]
                  };
                  index1 = index4 + 4;
                  float single1 = BitConverter.ToSingle(numArray1, 0);
                  float single2 = BitConverter.ToSingle(numArray2, 0);
                  float single3 = BitConverter.ToSingle(numArray3, 0);
                  CameraObject cameraObject1 = new CameraObject(id_, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, camByte);
                  if (camByte == 2)
                  {
                    int index5 = index1 + 1;
                    byte[] numArray4 = new byte[4]
                    {
                      camBytes[index5 + 3],
                      camBytes[index5 + 2],
                      camBytes[index5 + 1],
                      camBytes[index5]
                    };
                    int index6 = index5 + 4;
                    byte[] numArray5 = new byte[4]
                    {
                      camBytes[index6 + 3],
                      camBytes[index6 + 2],
                      camBytes[index6 + 1],
                      camBytes[index6]
                    };
                    int index7 = index6 + 4;
                    byte[] numArray6 = new byte[4]
                    {
                      camBytes[index7 + 3],
                      camBytes[index7 + 2],
                      camBytes[index7 + 1],
                      camBytes[index7]
                    };
                    index1 = index7 + 4 + 1;
                    float single4 = BitConverter.ToSingle(numArray4, 0);
                    float single5 = BitConverter.ToSingle(numArray5, 0);
                    float single6 = BitConverter.ToSingle(numArray6, 0);
                    CameraObject cameraObject2 = new CameraObject(id_, single1, single2, single3, single4, single5, single6, camByte);
                    if (cameraObject2.type == 2 || cameraObject2.type == 3 || cameraObject2.type == 1)
                    {
                      cameraObjectList.Add(cameraObject2);
                      continue;
                    }
                    continue;
                  }
                  if (camByte == 3 || camByte == 1)
                  {
                    int index8 = index1 + 1;
                    byte[] numArray7 = new byte[4]
                    {
                      camBytes[index8 + 3],
                      camBytes[index8 + 2],
                      camBytes[index8 + 1],
                      camBytes[index8]
                    };
                    int index9 = index8 + 4;
                    byte[] numArray8 = new byte[4]
                    {
                      camBytes[index9 + 3],
                      camBytes[index9 + 2],
                      camBytes[index9 + 1],
                      camBytes[index9]
                    };
                    int index10 = index9 + 4 + 1;
                    byte[] numArray9 = new byte[4]
                    {
                      camBytes[index10 + 3],
                      camBytes[index10 + 2],
                      camBytes[index10 + 1],
                      camBytes[index10]
                    };
                    int index11 = index10 + 4;
                    byte[] numArray10 = new byte[4]
                    {
                      camBytes[index11 + 3],
                      camBytes[index11 + 2],
                      camBytes[index11 + 1],
                      camBytes[index11]
                    };
                    int index12 = index11 + 4 + 1;
                    byte[] numArray11 = new byte[4]
                    {
                      camBytes[index12 + 3],
                      camBytes[index12 + 2],
                      camBytes[index12 + 1],
                      camBytes[index12]
                    };
                    int index13 = index12 + 4;
                    byte[] numArray12 = new byte[4]
                    {
                      camBytes[index13 + 3],
                      camBytes[index13 + 2],
                      camBytes[index13 + 1],
                      camBytes[index13]
                    };
                    int index14 = index13 + 4;
                    byte[] numArray13 = new byte[4]
                    {
                      camBytes[index14 + 3],
                      camBytes[index14 + 2],
                      camBytes[index14 + 1],
                      camBytes[index14]
                    };
                    int index15 = index14 + 4 + 1;
                    int t3a5 = (int) camBytes[index15] * 16777216 + (int) camBytes[index15 + 1] * 65536 + (int) camBytes[index15 + 2] * 256 + (int) camBytes[index15 + 3];
                    int num = index15 + 4;
                    float single7 = BitConverter.ToSingle(numArray7, 0);
                    float single8 = BitConverter.ToSingle(numArray8, 0);
                    float single9 = BitConverter.ToSingle(numArray9, 0);
                    float single10 = BitConverter.ToSingle(numArray10, 0);
                    float single11 = BitConverter.ToSingle(numArray11, 0);
                    float single12 = BitConverter.ToSingle(numArray12, 0);
                    float single13 = BitConverter.ToSingle(numArray13, 0);
                    CameraObject cameraObject3;
                    if (camByte == 3)
                    {
                      int index16 = num + 1;
                      byte[] numArray14 = new byte[4]
                      {
                        camBytes[index16 + 3],
                        camBytes[index16 + 2],
                        camBytes[index16 + 1],
                        camBytes[index16]
                      };
                      int index17 = index16 + 4;
                      byte[] numArray15 = new byte[4]
                      {
                        camBytes[index17 + 3],
                        camBytes[index17 + 2],
                        camBytes[index17 + 1],
                        camBytes[index17]
                      };
                      num = index17 + 4;
                      float single14 = BitConverter.ToSingle(numArray14, 0);
                      float single15 = BitConverter.ToSingle(numArray15, 0);
                      cameraObject3 = new CameraObject(id_, single1, single2, single3, single7, single8, single9, single10, single11, single12, single13, t3a5, single14, single15, camByte);
                    }
                    else
                      cameraObject3 = new CameraObject(id_, single1, single2, single3, single7, single8, single9, single10, single11, single12, single13, t3a5, camByte);
                    index1 = num + 1;
                    if (cameraObject3.type == 2 || cameraObject3.type == 3 || cameraObject3.type == 1)
                    {
                      cameraObjectList.Add(cameraObject3);
                      continue;
                    }
                    continue;
                  }
                  continue;
                case 4:
                  index1 += 11;
                  continue;
                default:
                  int num1 = (int) MessageBox.Show("TYPE " + (object) camByte + " CAMERA: " + index1.ToString("x"));
                  index1 += 64;
                  continue;
              }
            }
            else
            {
              int num2 = (int) MessageBox.Show("Contains lighting data not parsed by BB");
              index1 = camBytes.Count;
            }
          }
        }
      }
      catch (Exception ex)
      {
      }
      return cameraObjectList;
    }

    public int GetListDec(string file, int skipCount)
    {
      byte[] numArray = new byte[3]
      {
        (byte) 3,
        (byte) 10,
        (byte) 11
      };
      int num1 = 0;
      int num2 = 0;
      ArrayList arrayList = new ArrayList();
      bool flag = false;
      if (File.Exists(file))
      {
        try
        {
          BinaryReader binaryReader = new BinaryReader((Stream) File.Open(file, FileMode.Open));
          long length = binaryReader.BaseStream.Length;
          byte[] buffer = new byte[length];
          binaryReader.BaseStream.Read(buffer, 0, (int) length);
          binaryReader.Close();
          try
          {
            for (int index = 0; (long) index < length; ++index)
            {
              if (!flag)
              {
                if ((int) buffer[index] == (int) numArray[0] && (int) buffer[index + 1] == (int) numArray[1] && (int) buffer[index + 3] == (int) numArray[2])
                {
                  ++num1;
                  if (skipCount < num1)
                  {
                    flag = true;
                    num2 = index;
                  }
                }
              }
              else
                break;
            }
          }
          catch (Exception ex)
          {
            binaryReader.Close();
          }
        }
        catch
        {
        }
      }
      return num2;
    }

    public int GetListDec(List<byte> bytesInFile, int skipCount)
    {
      byte[] numArray = new byte[3]
      {
        (byte) 3,
        (byte) 10,
        (byte) 11
      };
      int num1 = 0;
      long count = (long) bytesInFile.Count;
      int num2 = 0;
      ArrayList arrayList = new ArrayList();
      bool flag = false;
      try
      {
        for (int index = 0; (long) index < count; ++index)
        {
          if (!flag)
          {
            if ((int) bytesInFile[index] == (int) numArray[0] && (int) bytesInFile[index + 1] == (int) numArray[1] && (int) bytesInFile[index + 3] == (int) numArray[2])
            {
              ++num1;
              if (skipCount < num1)
              {
                flag = true;
                num2 = index;
              }
            }
          }
          else
            break;
        }
      }
      catch (Exception ex)
      {
      }
      return num2;
    }

    public int GetStructListDec(List<byte> bytesInFile, int skipCount)
    {
      byte[] numArray = new byte[2]{ (byte) 8, (byte) 9 };
      int num1 = 0;
      long count = (long) bytesInFile.Count;
      int num2 = 0;
      ArrayList arrayList = new ArrayList();
      bool flag = false;
      try
      {
        for (int index = 0; (long) index < count; ++index)
        {
          if (!flag)
          {
            if ((int) bytesInFile[index] == (int) numArray[0] && (int) bytesInFile[index + 2] == (int) numArray[1])
            {
              ++num1;
              if (skipCount < num1)
              {
                flag = true;
                num2 = index;
              }
            }
          }
          else
            break;
        }
      }
      catch (Exception ex)
      {
      }
      return num2;
    }
  }
}
