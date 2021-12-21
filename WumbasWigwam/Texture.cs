// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.Texture
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

namespace WumbasWigwam
{
  public class Texture
  {
    public int paletteOffset;
    public int glname = -1;
    public int width;
    public int height;
    public int cms;
    public int cmt;
    public float textureHRatio;
    public float textureWRatio;
    public CullMode cullMode = CullMode.CULLBACK;
    public string name = "";
    public bool fromSolid;
    public bool fromAlpha;
    public bool hasAlpha;
    public int palSize = 16;
    public bool processed;
    public byte textureMode = 8;
    public int textureData;
    public bool ISALPHA;

    public Texture(int w_, int h_, int cms_, int cmt_, int textureData_)
    {
      this.width = w_;
      this.height = h_;
      this.cms = cms_;
      this.cmt = cmt_;
      this.textureData = textureData_;
    }

    public Texture(
      int w_,
      int h_,
      int cms_,
      int cmt_,
      int glname_,
      int textureData_,
      CullMode cm,
      byte texMode,
      float texHR,
      float texWR,
      int palSize_,
      bool isA,
      bool proc)
    {
      this.glname = glname_;
      this.textureMode = texMode;
      this.width = w_;
      this.height = h_;
      this.cms = cms_;
      this.cmt = cmt_;
      this.textureData = textureData_;
      this.cullMode = cm;
      this.textureHRatio = texHR;
      this.textureWRatio = texWR;
      this.palSize = palSize_;
      this.ISALPHA = isA;
      this.processed = proc;
    }

    public void UpdateCullMode(CullMode cm) => this.cullMode = cm;

    public byte getCullMode() => (byte) this.cullMode;

    public void setTextureMode(byte mode) => this.textureMode = mode;

    public void setRatio(float sScale, float tScale)
    {
      this.textureHRatio = tScale / 32f / (float) this.height;
      this.textureWRatio = sScale / 32f / (float) this.width;
    }

    public static Texture clone(Texture t) => new Texture(t.width, t.height, t.cms, t.cmt, t.glname, t.textureData, t.cullMode, t.textureMode, t.textureHRatio, t.textureWRatio, t.palSize, t.ISALPHA, t.processed);

    public bool equal(Texture t)
    {
      bool flag = false;
      if (this.width == t.width && this.height == t.height && this.textureData == t.textureData && this.cmt == t.cmt && this.cms == t.cms && this.cullMode == t.cullMode && (int) this.textureMode == (int) t.textureMode)
        flag = true;
      return flag;
    }
  }
}
