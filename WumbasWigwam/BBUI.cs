// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.BBUI
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using OpenTK.Graphics;

namespace WumbasWigwam
{
  public class BBUI
  {
    private static bool setup;

    public static void Setup()
    {
    }

    private static void GenerateTexture(ref int textureName, byte[] pixels, int w, int h)
    {
      OpenTK.Graphics.OpenGL.GL.GenTextures(1, out textureName);
      OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, textureName);
      OpenTK.Graphics.OpenGL.GL.TexImage2D<byte>(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0, OpenTK.Graphics.OpenGL.PixelInternalFormat.Rgba, w, h, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, pixels);
      OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, 9729);
      OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, 9729);
    }

    private static void Draw2DRectangle(float min_x, float min_y, float max_x, float max_y)
    {
      OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Triangles);
      OpenTK.Graphics.OpenGL.GL.TexCoord2(0, 0);
      OpenTK.Graphics.OpenGL.GL.Vertex3(0.0f, max_y, 0.0f);
      OpenTK.Graphics.OpenGL.GL.TexCoord2(0, 1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(0.0f, min_y, 0.0f);
      OpenTK.Graphics.OpenGL.GL.TexCoord2(1, 1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(1f, min_y, 0.0f);
      OpenTK.Graphics.OpenGL.GL.TexCoord2(1, 0);
      OpenTK.Graphics.OpenGL.GL.Vertex3(1f, max_y, 0.0f);
      OpenTK.Graphics.OpenGL.GL.TexCoord2(0, 0);
      OpenTK.Graphics.OpenGL.GL.Vertex3(0.0f, max_y, 0.0f);
      OpenTK.Graphics.OpenGL.GL.TexCoord2(1, 1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(1f, min_y, 0.0f);
      OpenTK.Graphics.OpenGL.GL.End();
    }

    public static void DrawAxis(int x, int y, GLCamera BBCamera)
    {
      OpenTK.Graphics.OpenGL.GL.PushMatrix();
      OpenTK.Graphics.OpenGL.GL.Viewport(x, y, 100, 100);
      OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Projection);
      OpenTK.Graphics.OpenGL.GL.LoadIdentity();
      Glu.Perspective(45.0, 1.0, 1.0, 100000.0);
      OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
      OpenTK.Graphics.OpenGL.GL.LoadIdentity();
      OpenTK.Graphics.OpenGL.GL.Translate(0.0f, 0.0f, -5f);
      OpenTK.Graphics.OpenGL.GL.Rotate(BBCamera.GetXRotation(), 1f, 0.0f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Rotate(BBCamera.GetYRotation(), 0.0f, 1f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Rotate(BBCamera.GetZRotation(), 0.0f, 0.0f, 1f);
      OpenTK.Graphics.OpenGL.GL.PushMatrix();
      OpenTK.Graphics.OpenGL.GL.LineWidth(4f);
      OpenTK.Graphics.OpenGL.GL.Color3(1f, 0.0f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Lines);
      OpenTK.Graphics.OpenGL.GL.Vertex3(0.0f, 0.0f, -1f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(0, 0, 0);
      OpenTK.Graphics.OpenGL.GL.End();
      OpenTK.Graphics.OpenGL.GL.Color3(0.0f, 1f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Lines);
      OpenTK.Graphics.OpenGL.GL.Vertex3(0.0f, 1f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(0, 0, 0);
      OpenTK.Graphics.OpenGL.GL.End();
      OpenTK.Graphics.OpenGL.GL.Color3(0.0f, 0.0f, 1f);
      OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Lines);
      OpenTK.Graphics.OpenGL.GL.Vertex3(-1f, 0.0f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(0.0f, 0.0f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.End();
      OpenTK.Graphics.OpenGL.GL.PopMatrix();
      OpenTK.Graphics.OpenGL.GL.PopMatrix();
    }
  }
}
