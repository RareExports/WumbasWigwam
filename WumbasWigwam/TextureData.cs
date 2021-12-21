// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.TextureData
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System.Collections.Generic;
using System.Linq;

namespace WumbasWigwam
{
  public class TextureData
  {
    public int textureOffset;
    public byte[] n64 = new byte[1];
    public byte[] gl = new byte[1];
    public int width;
    public int height;
    public int cms;
    public int cmt;

    public TextureData()
    {
    }

    public TextureData(byte[] n64_, byte[] gl_, int w_, int h_, int cms_, int cmt_)
    {
      this.n64 = n64_;
      this.gl = gl_;
      this.width = w_;
      this.height = h_;
      this.cms = cms_;
      this.cmt = cmt_;
    }

    public override bool Equals(object obj) => (object) (obj as TextureData) != null && ((IEnumerable<byte>) this.gl).SequenceEqual<byte>((IEnumerable<byte>) ((TextureData) obj).gl);

    public override int GetHashCode() => this.gl.GetHashCode();

    public static bool operator ==(TextureData x, TextureData y)
    {
      if ((object) x == null && (object) y == null)
        return true;
      return ((object) x != null || (object) y == null) && ((object) x == null || (object) y != null) && ((IEnumerable<byte>) x.gl).SequenceEqual<byte>((IEnumerable<byte>) y.gl);
    }

    public static bool operator !=(TextureData x, TextureData y) => !((IEnumerable<byte>) x.gl).SequenceEqual<byte>((IEnumerable<byte>) y.gl);
  }
}
