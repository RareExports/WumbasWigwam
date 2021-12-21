// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.ObjectData
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public class ObjectData : PickableObject
  {
    public ObjectType type = ObjectType.Normal;
    public int zoneID;
    public string name = "";
    public short objectID;
    public int address;
    public short locX;
    public short locY;
    public short locZ;
    public short rot;
    public short size = 100;
    public short specialScript;
    public string modelFile = "";
    public string modelFile2 = "";
    public int file;
    public int file2;
    public BoundingBox bb;
    public bool hasJiggy;
    public int jiggyID = -1;
    public int flag = -1;
    public int cameraID = -1;
    public byte obj10;
    public byte obj11;
    public byte obj13;
    public byte obj16;
    public byte obj17;
    public byte obj18;
    public byte obj19;
    public short uid;
    public short nodeOutUID;
    public byte struct2;
    public byte struct3;
    public byte structA;
    public byte structB;
    public ushort radius;
    public float activationPercent;
    public int pw1;
    public int pw2;
    public int pw3;
    public uint unk3;
    public bool useAnimation;
    public bool usePause;
    public bool useSpeed;
    public int animation;
    public int pauseTime;
    public int speed;
    public byte pathID;
    public bool node_in;
    public bool node_out;
    public byte colour = 4;

    public int Pointer
    {
      get
      {
        int file = this.file;
        return this.file == 0 ? 0 : this.file * 4 + 20872;
      }
    }

    public int Pointer2 => this.file2 == 0 ? 0 : this.file2 * 4 + 20872;

    public ObjectData(
      string name_,
      short objectID_,
      short script_,
      int pointer_,
      int pointer2_,
      string modelfile_,
      string modelfile2_,
      int cameraid_,
      int jiggyid_)
    {
      this.name = name_;
      this.objectID = objectID_;
      this.specialScript = script_;
      this.file = pointer_;
      this.file2 = pointer2_;
      this.modelFile = modelfile_;
      this.modelFile2 = modelfile2_;
      this.cameraID = cameraid_;
      this.jiggyID = jiggyid_;
    }

    public static ObjectData clone(ObjectData o) => new ObjectData(o.name, o.objectID, o.specialScript, o.Pointer, o.Pointer2, o.modelFile, o.modelFile2, o.cameraID, o.jiggyID)
    {
      type = o.type,
      radius = o.radius
    };

    public static ObjectData fullClone(ObjectData o) => new ObjectData(o.name, o.objectID, o.specialScript, o.Pointer, o.Pointer2, o.modelFile, o.modelFile2, o.cameraID, o.jiggyID)
    {
      address = o.address,
      zoneID = o.zoneID,
      type = o.type,
      radius = o.radius,
      locX = o.locX,
      locY = o.locY,
      locZ = o.locZ,
      rot = o.rot,
      size = o.size,
      uid = o.uid,
      obj10 = o.obj10,
      obj11 = o.obj11,
      obj13 = o.obj13,
      obj16 = o.obj16,
      obj17 = o.obj17,
      obj18 = o.obj18,
      obj19 = o.obj19,
      struct2 = o.struct2,
      struct3 = o.struct3,
      structA = o.structA,
      structB = o.structB,
      hasJiggy = o.hasJiggy,
      flag = o.flag,
      activationPercent = o.activationPercent,
      pw1 = o.pw1,
      pw2 = o.pw2,
      pw3 = o.pw3,
      unk3 = o.unk3,
      useAnimation = o.useAnimation,
      usePause = o.usePause,
      useSpeed = o.useSpeed,
      animation = o.animation,
      pauseTime = o.pauseTime,
      speed = o.speed,
      pathID = o.pathID,
      node_in = o.node_in,
      node_out = o.node_out,
      nodeOutUID = o.nodeOutUID,
      colour = o.colour
    };

    public ObjectData(
      short objectID_,
      int address_,
      short x_,
      short y_,
      short z_,
      short rot_,
      short size_,
      short specialScript_,
      byte obj10_,
      byte obj11_,
      byte obj13_,
      byte obj16_,
      byte obj17_,
      byte obj18_,
      byte obj19_)
    {
      this.obj10 = obj10_;
      this.obj11 = obj11_;
      this.obj13 = obj13_;
      this.obj16 = obj16_;
      this.obj17 = obj17_;
      this.obj18 = obj18_;
      this.obj19 = obj19_;
      this.uid = (short) (((int) this.obj16 << 8) + (int) this.obj17);
      this.objectID = objectID_;
      this.address = address_;
      this.locX = x_;
      this.locY = y_;
      this.locZ = z_;
      this.rot = rot_;
      this.size = size_;
      this.specialScript = specialScript_;
      this.process();
    }

    public ObjectData(
      short objectID_,
      int address_,
      short x_,
      short y_,
      short z_,
      short rot_,
      short size_,
      short specialScript_,
      byte struct2_,
      byte struct3_,
      byte structA_,
      byte structB_)
    {
      this.struct2 = struct2_;
      this.struct3 = struct3_;
      this.structA = structA_;
      this.structB = structB_;
      this.objectID = objectID_;
      this.address = address_;
      this.locX = x_;
      this.locY = y_;
      this.locZ = z_;
      this.rot = rot_;
      this.size = size_;
      this.specialScript = specialScript_;
      this.process();
    }

    public ObjectData(
      short objectID_,
      int address_,
      short x_,
      short y_,
      short z_,
      short rot_,
      short size_,
      short specialScript_)
    {
      this.objectID = objectID_;
      this.address = address_;
      this.locX = x_;
      this.locY = y_;
      this.locZ = z_;
      this.rot = rot_;
      this.size = size_;
      this.specialScript = specialScript_;
      this.process();
    }

    public ObjectData(
      float activationPercent_,
      int w1,
      int w2,
      int w3,
      byte obj16_,
      byte obj17_,
      byte obj18_,
      byte obj19_,
      bool path,
      int add)
    {
      this.activationPercent = activationPercent_;
      this.pw1 = w1;
      this.pw2 = w2;
      this.pw3 = w3;
      this.unk3 = (uint) this.pw3 >> 23;
      this.speed = this.pw3 << 9 >> 9 >> 12;
      this.animation = this.pw2 >> 22 << 1;
      this.pauseTime = (this.pw3 << 20 >> 21) / 4;
      byte num = (byte) ((this.pw2 & 65280) >> 8);
      this.usePause = ((int) num & 1) == 1;
      this.useSpeed = ((int) num & 2) == 2;
      this.useAnimation = ((int) num & 4) == 4;
      this.pathID = (byte) (this.pw2 & (int) byte.MaxValue);
      this.obj16 = obj16_;
      this.obj17 = obj17_;
      this.obj18 = obj18_;
      this.obj19 = obj19_;
      this.type = ObjectType.SPath;
      this.uid = (short) (((int) this.obj16 << 8) + (int) this.obj17);
      this.address = add;
    }

    public void setControlFlags(bool usePause_, bool useSpeed_, bool useAnimation_)
    {
      this.usePause = usePause_;
      this.useSpeed = useSpeed_;
      this.useAnimation = useAnimation_;
      byte num = 0;
      if (this.usePause)
        ++num;
      if (this.useSpeed)
        num += (byte) 2;
      if (this.useAnimation)
        num += (byte) 20;
      this.pw2 = (int) ((long) this.pw2 & 4294902015L);
      this.pw2 += (int) num << 8;
    }

    public void setPathID(byte pid)
    {
      this.pw2 = (this.pw2 >> 8 << 8) + (int) pid;
      this.pathID = pid;
    }

    public void setSpeed(int speed_)
    {
      this.speed = speed_;
      this.pw3 = (int) ((long) this.pw3 & 4286582783L);
      this.pw3 += this.speed << 12;
    }

    public void setPauseTime(int pauseTime_)
    {
      this.pauseTime = pauseTime_;
      this.pw3 = (this.pw3 >> 12 << 12) + (this.pauseTime * 4 << 1);
    }

    public void setUNK3(int unk3_)
    {
      this.unk3 = (uint) unk3_;
      this.pw3 = (this.pw3 << 9 >> 9) + ((int) this.unk3 << 23);
    }

    private void process()
    {
      byte num = (byte) ((uint) this.specialScript & (uint) sbyte.MaxValue);
      switch (num)
      {
        case 0:
          break;
        case 12:
          break;
        default:
          this.radius = (ushort) ((uint) (ushort) this.specialScript >> 7);
          if (num == (byte) 6)
          {
            this.name = "warp";
            this.type = ObjectType.Warp;
            this.modelFile = ".\\resources\\warp.mw";
            this.file = 1;
            this.colour = (byte) 3;
          }
          if (num == (byte) 8)
          {
            if (this.objectID == (short) 77 || this.objectID == (short) 76)
            {
              this.name = "Magic Boundary";
              this.type = ObjectType.MagicBoundaryOrCameraTrigger;
              this.modelFile = ".\\resources\\magic_marker.mw";
              this.file = 1;
            }
            else
            {
              switch (this.objectID)
              {
                case 22:
                  this.name = "Camera Path Trigger 1";
                  break;
                case 23:
                  this.name = "Camera Path Trigger 2";
                  break;
                case 24:
                  this.name = "Camera Path Trigger 3";
                  break;
                case 25:
                  this.name = "Camera Path Trigger 4";
                  break;
                case 26:
                  this.name = "Camera Path Trigger 5";
                  break;
                case 27:
                  this.name = "Camera Path Trigger 6";
                  break;
                case 28:
                  this.name = "Camera Path Trigger 7";
                  break;
                case 29:
                  this.name = "Camera Path Trigger 8";
                  break;
                case 30:
                  this.name = "Camera Path Trigger 9";
                  break;
                case 31:
                  this.name = "Camera Path Trigger 10";
                  break;
                case 32:
                  this.name = "Camera Path Trigger 11";
                  break;
                case 33:
                  this.name = "Camera Path Trigger 12";
                  break;
                case 34:
                  this.name = "Camera Path Trigger 13";
                  break;
                case 35:
                  this.name = "Camera Path Trigger 14";
                  break;
                case 36:
                  this.name = "Camera Path Trigger 15";
                  break;
                case 37:
                  this.name = "Camera Path Trigger 16";
                  break;
                case 38:
                  this.name = "Camera Path Trigger 17";
                  break;
                case 39:
                  this.name = "Camera Path Trigger 18";
                  break;
                case 40:
                  this.name = "Camera Path Trigger 19";
                  break;
                case 41:
                  this.name = "Camera Path Trigger 20";
                  break;
                case 42:
                  this.name = "Camera Release";
                  break;
              }
              this.type = ObjectType.MagicBoundaryOrCameraTrigger;
              if (this.objectID >= (short) 22 && this.objectID <= (short) 41)
                this.modelFile = ".\\resources\\cam_start.mw";
              else if (this.objectID == (short) 42)
                this.modelFile = ".\\resources\\cam_end.mw";
              this.file = 1;
            }
          }
          if (num == (byte) 18)
          {
            this.name = "Camera Trigger";
            this.colour = (byte) 2;
            this.type = ObjectType.CameraTrigger;
          }
          if (num == (byte) 14)
          {
            this.name = "Enemy Boundary";
            this.colour = (byte) 0;
            this.file = 1;
            this.type = ObjectType.EnemeyBoundary;
            this.modelFile = ".\\resources\\enemy_marker.mw";
          }
          if (num == (byte) 16)
          {
            this.name = "Path Node";
            this.colour = (byte) 0;
            this.type = ObjectType.Path;
          }
          if (num != (byte) 20)
            break;
          this.name = "Flag";
          this.colour = (byte) 1;
          this.type = ObjectType.Flag;
          break;
      }
    }

    public bool compareTo(ObjectData od)
    {
      bool flag = false;
      if ((int) this.objectID == (int) od.objectID && (int) this.specialScript == (int) od.specialScript && (int) this.locX == (int) od.locX && (int) this.locY == (int) od.locY && (int) this.locZ == (int) od.locZ && this.address == od.address)
        flag = true;
      return flag;
    }
  }
}
