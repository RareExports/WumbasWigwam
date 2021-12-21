﻿// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.Sprite
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System.Collections.Generic;
using System.Drawing;

namespace WumbasWigwam
{
  public class Sprite
  {
    public int id;
    public string name = "";
    public int pointer;
    public List<Bitmap> frames = new List<Bitmap>();
    public short numberFrames;
    public SpriteTextureFormat textureFormat = SpriteTextureFormat.CI4;
    public byte animationByte;
    public int imagesPerFrame = 1;
    public bool compressed = true;

    public Sprite(int id_, string name_, int pointer_, bool compressed_)
    {
      this.id = id_;
      this.name = name_;
      this.pointer = pointer_;
      this.compressed = compressed_;
    }
  }
}
