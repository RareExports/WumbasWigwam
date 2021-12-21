// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.Core
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using NLog;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WumbasWigwam
{
  public static class Core
  {
    private static Logger logger = LogManager.GetCurrentClassLogger();
    public static string dir = "";
    private static float near = 100f;
    private static float far = 35000f;
    public static bool drawCamTrigRadius = false;
    public static bool drawFlagRadius = false;
    public static bool drawEnemyRadius = false;
    public static bool drawWarpRadius = false;
    public static bool drawUnknownRadius = false;
    private static int texturesGL = 0;
    private static byte[] rect_pixel = new byte[0];
    private static IntPtr wire;
    private static int height = 0;
    private static int width = 0;
    private static bool usingARB = false;

    public static double ToRadians(double val) => Math.PI / 180.0 * val;

    public static byte[] Int16ToByteArray(short number) => new byte[2]
    {
      (byte) ((uint) number >> 8),
      (byte) ((uint) number & (uint) byte.MaxValue)
    };

    public static byte[] Int32ToByteArray(int number) => new byte[4]
    {
      (byte) (number >> 24),
      (byte) (number >> 16),
      (byte) (number >> 8),
      (byte) (number & (int) byte.MaxValue)
    };

    public static void ReadINI()
    {
      try
      {
        StreamReader streamReader = new StreamReader(Application.StartupPath + "\\resources\\mw.ini");
        string end = streamReader.ReadToEnd();
        streamReader.Close();
        if (Regex.IsMatch(end, "NEAR:([^\\r\\n]*)"))
          Core.near = Convert.ToSingle(Regex.Match(end, "NEAR:([^\\r\\n]*)").Groups[1].Value);
        if (!Regex.IsMatch(end, "FAR:([^\\r\\n]*)"))
          return;
        Core.far = Convert.ToSingle(Regex.Match(end, "FAR:([^\\r\\n]*)").Groups[1].Value);
      }
      catch
      {
      }
    }

    public static void GenerateTexture(
      ref int textureName,
      byte[] pixels,
      int w,
      int h,
      int cms,
      int cmt)
    {
      OpenTK.Graphics.OpenGL.GL.GenTextures(1, out textureName);
      OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, textureName);
      OpenTK.Graphics.OpenGL.GL.TexImage2D<byte>(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0, OpenTK.Graphics.OpenGL.PixelInternalFormat.Rgba, w, h, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, pixels);
      OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, 9728);
      OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, 9728);
      OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, (OpenTK.Graphics.OpenGL.TextureParameterName) 34046, 16);
      OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, 9729);
      OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, 9729);
      if (Tile.cms == 0)
        OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, 10497);
      if (cms == 2)
        OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS, 33071);
      if (cmt == 0)
        OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, 10497);
      if (cmt != 2)
        return;
      OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, 33071);
    }

    public static void ClearScreenAndLoadIdentity()
    {
      OpenTK.Graphics.OpenGL.GL.Clear(OpenTK.Graphics.OpenGL.ClearBufferMask.DepthBufferBit | OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit);
      OpenTK.Graphics.OpenGL.GL.LoadIdentity();
    }

    public static void LoadIdentity() => OpenTK.Graphics.OpenGL.GL.LoadIdentity();

    public static uint GenBuffer(ushort[] buffer)
    {
      uint buffers = 0;
      OpenTK.Graphics.OpenGL.GL.GenBuffers(1, out buffers);
      OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, buffers);
      OpenTK.Graphics.OpenGL.GL.BufferData<ushort>(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, (IntPtr) (buffer.Length * 2), buffer, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
      OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, 0);
      return buffers;
    }

    public static uint GenBuffer()
    {
      uint buffers = 0;
      OpenTK.Graphics.OpenGL.GL.GenBuffers(1, out buffers);
      return buffers;
    }

    public static void DeleteDL(uint dl) => OpenTK.Graphics.OpenGL.GL.DeleteLists(dl, 1);

    public static void DeleteDLs(List<uint> dls)
    {
      foreach (uint dl in dls)
        OpenTK.Graphics.OpenGL.GL.DeleteLists(dl, 1);
    }

    public static void DeleteTexture(int name)
    {
      if (name == -1)
        return;
      uint textures = (uint) name;
      OpenTK.Graphics.OpenGL.GL.DeleteTextures(1, ref textures);
    }

    public static void DeleteTextures(List<int> dls)
    {
      for (int index = 0; index < dls.Count; ++index)
      {
        if (index != -1)
        {
          uint dl = (uint) dls[index];
          OpenTK.Graphics.OpenGL.GL.DeleteTextures(1, ref dl);
        }
      }
    }

    public static void DeleteBuffers(List<uint> buffers)
    {
      for (int index = 0; index < buffers.Count; ++index)
      {
        if (index != -1)
        {
          uint buffer = buffers[index];
          if (Core.usingARB)
            OpenTK.Graphics.OpenGL.GL.Arb.DeleteBuffers(1, ref buffer);
          else
            OpenTK.Graphics.OpenGL.GL.DeleteBuffers(1, ref buffer);
        }
      }
    }

    public static void DeleteBuffer(uint buffer)
    {
      if (Core.usingARB)
        OpenTK.Graphics.OpenGL.GL.Arb.DeleteBuffers(1, ref buffer);
      else
        OpenTK.Graphics.OpenGL.GL.DeleteBuffers(1, ref buffer);
    }

    public static uint GenerateDL()
    {
      try
      {
        int num = OpenTK.Graphics.OpenGL.GL.GenLists(1);
        OpenTK.Graphics.OpenGL.GL.NewList((uint) num, OpenTK.Graphics.OpenGL.ListMode.Compile);
        return (uint) num;
      }
      catch (Exception ex)
      {
        Core.logger.Log(NLog.LogLevel.Debug, ex.Message);
        Core.logger.Log(NLog.LogLevel.Error, "Something went wrong creating a new DL returning 0?");
        return 0;
      }
    }

    public static void EndDL() => OpenTK.Graphics.OpenGL.GL.EndList();

    public static void RenderDL(uint dl) => OpenTK.Graphics.OpenGL.GL.CallList(dl);

    public static void InitGl()
    {
      OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.DepthTest);
      OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.CullFace);
      OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
      OpenTK.Graphics.OpenGL.GL.ShadeModel(OpenTK.Graphics.OpenGL.ShadingModel.Smooth);
      OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
      OpenTK.Graphics.OpenGL.GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactorSrc.SrcAlpha, OpenTK.Graphics.OpenGL.BlendingFactorDest.OneMinusSrcAlpha);
      OpenTK.Graphics.OpenGL.GL.ClearColor(0.2f, 0.5f, 1f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0);
      OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, 9728);
      OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, 9728);
      OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS, 33071);
      OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, 33071);
      OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, (OpenTK.Graphics.OpenGL.TextureParameterName) 34046, 16);
    }

    public static void DrawCamera(CameraObject cam)
    {
      OpenTK.Graphics.OpenGL.GL.PushMatrix();
      int num1 = -50;
      int num2 = -50;
      int num3 = 0;
      int num4 = 50;
      int num5 = 50;
      int x1 = -40;
      int y1 = 55;
      int x2 = 40;
      int y2 = 85;
      int z2 = 50;
      OpenTK.Graphics.OpenGL.GL.Color3(1f, 1f, 1f);
      OpenTK.Graphics.OpenGL.GL.LineWidth(0.2f);
      OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Lines);
      Core.DrawLine(num1, num2, num3, num4, num2, num3);
      Core.DrawLine(num1, num5, num3, num4, num5, num3);
      Core.DrawLine(num1, num2, num3, num1, num5, num3);
      Core.DrawLine(num4, num2, num3, num4, num5, num3);
      Core.DrawLine(num1, num2, num3, 0, 0, z2);
      Core.DrawLine(num1, num5, num3, 0, 0, z2);
      Core.DrawLine(num4, num2, num3, 0, 0, z2);
      Core.DrawLine(num4, num5, num3, 0, 0, z2);
      OpenTK.Graphics.OpenGL.GL.End();
      OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Triangles);
      OpenTK.Graphics.OpenGL.GL.Color3(Color.Black);
      OpenTK.Graphics.OpenGL.GL.LineWidth(0.2f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y1, num3);
      OpenTK.Graphics.OpenGL.GL.Vertex3(0, y2, num3);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y1, num3);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y1, num3);
      OpenTK.Graphics.OpenGL.GL.Vertex3(0, y2, num3);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y1, num3);
      OpenTK.Graphics.OpenGL.GL.End();
      OpenTK.Graphics.OpenGL.GL.PopMatrix();
    }

    public static uint DrawCameraPicking(CameraObject cam)
    {
      int num = OpenTK.Graphics.OpenGL.GL.GenLists(1);
      OpenTK.Graphics.OpenGL.GL.NewList((uint) num, OpenTK.Graphics.OpenGL.ListMode.Compile);
      OpenTK.Graphics.OpenGL.GL.PushMatrix();
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.CullFace);
      int x1 = -50;
      int y1 = -50;
      int z1 = 50;
      int x2 = 50;
      int y2 = 90;
      int z2 = 0;
      OpenTK.Graphics.OpenGL.GL.Color3((float) cam.m_colorID[0] / (float) byte.MaxValue, (float) cam.m_colorID[1] / (float) byte.MaxValue, (float) cam.m_colorID[2] / (float) byte.MaxValue);
      OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Triangles);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y1, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y1, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y2, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y2, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y2, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y1, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y1, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y1, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y2, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y2, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y2, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y1, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y2, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y2, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y2, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y2, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y2, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y2, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y1, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y1, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y1, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y1, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y1, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y1, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y1, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y2, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y1, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y1, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y2, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y2, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y1, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y2, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y1, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y1, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y2, z2);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y2, z1);
      OpenTK.Graphics.OpenGL.GL.End();
      OpenTK.Graphics.OpenGL.GL.PopMatrix();
      OpenTK.Graphics.OpenGL.GL.EndList();
      OpenTK.Graphics.OpenGL.GL.Flush();
      return (uint) num;
    }

    public static void DrawInvisibleWall()
    {
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.CullFace);
      OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Quads);
      OpenTK.Graphics.OpenGL.GL.Color3(0.0f, 1f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(-10000f, 10000f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(10000f, 10000f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(10000f, -10000f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(-10000f, -10000f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.End();
      OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.CullFace);
    }

    public static void DrawPathController(float x, float y, float z)
    {
      float y1 = 50f;
      float y2 = -50f;
      float num = 25f;
      OpenTK.Graphics.OpenGL.GL.PushMatrix();
      OpenTK.Graphics.OpenGL.GL.Translate(x, y, z);
      OpenTK.Graphics.OpenGL.GL.Color3(0.0f, 0.0f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.TriangleFan);
      OpenTK.Graphics.OpenGL.GL.Vertex3(0.0f, y2, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(num, 0.0f, num);
      OpenTK.Graphics.OpenGL.GL.Vertex3(-num, 0.0f, num);
      OpenTK.Graphics.OpenGL.GL.Vertex3(-num, 0.0f, -num);
      OpenTK.Graphics.OpenGL.GL.Vertex3(num, 0.0f, -num);
      OpenTK.Graphics.OpenGL.GL.Vertex3(num, 0.0f, num);
      OpenTK.Graphics.OpenGL.GL.End();
      OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.TriangleFan);
      OpenTK.Graphics.OpenGL.GL.Vertex3(0.0f, y1, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(-num, 0.0f, num);
      OpenTK.Graphics.OpenGL.GL.Vertex3(num, 0.0f, num);
      OpenTK.Graphics.OpenGL.GL.Vertex3(num, 0.0f, -num);
      OpenTK.Graphics.OpenGL.GL.Vertex3(-num, 0.0f, -num);
      OpenTK.Graphics.OpenGL.GL.Vertex3(-num, 0.0f, num);
      OpenTK.Graphics.OpenGL.GL.End();
      OpenTK.Graphics.OpenGL.GL.PopMatrix();
      OpenTK.Graphics.OpenGL.GL.Flush();
      OpenTK.Graphics.OpenGL.GL.EndList();
    }

    public static void DrawCamTrigger(ushort radius, byte r, byte g, byte b)
    {
      int sy = -25;
      int sz = -25;
      int lx = 25;
      int ly = 25;
      int lz = 25;
      OpenTK.Graphics.OpenGL.GL.PushMatrix();
      OpenTK.Graphics.OpenGL.GL.Color3((float) r / (float) byte.MaxValue, (float) g / (float) byte.MaxValue, (float) b / (float) byte.MaxValue);
      Core.DrawCube(-25, sy, sz, lx, ly, lz);
      if (radius != (ushort) 0 && Core.drawCamTrigRadius)
      {
        Core.wire = OpenTK.Graphics.Glu.NewQuadric();
        OpenTK.Graphics.Glu.QuadricDrawStyle(Core.wire, QuadricDrawStyle.Line);
        OpenTK.Graphics.Glu.Sphere(Core.wire, (double) radius, 5, 5);
        OpenTK.Graphics.Glu.DeleteQuadric(Core.wire);
      }
      OpenTK.Graphics.OpenGL.GL.PopMatrix();
    }

    public static void SetView(int height_, int width_)
    {
      Core.height = height_;
      Core.width = width_;
      OpenTK.Graphics.OpenGL.GL.Viewport(0, 0, Core.width, Core.height);
      OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Projection);
      OpenTK.Graphics.OpenGL.GL.LoadIdentity();
      Tao.OpenGl.Glu.gluPerspective(45.0, 1.0 * (double) ((float) Core.width / (float) Core.height), (double) Core.near, (double) Core.far);
      OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
      OpenTK.Graphics.OpenGL.GL.LoadIdentity();
      Core.rect_pixel = new byte[(Core.width + 1) * (Core.height + 1) * 3 + 3];
    }

    public static byte[] BackBufferSelect(int x, int y, uint dl)
    {
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Fog);
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Lighting);
      OpenTK.Graphics.OpenGL.GL.ShadeModel(OpenTK.Graphics.OpenGL.ShadingModel.Flat);
      OpenTK.Graphics.OpenGL.GL.PushMatrix();
      OpenTK.Graphics.OpenGL.GL.Translate(0.0f, 0.0f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Rotate(0.0f, 1f, 0.0f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Rotate(0.0f, 0.0f, 1f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Rotate(0.0f, 0.0f, 0.0f, 1f);
      OpenTK.Graphics.OpenGL.GL.CallList(dl);
      byte[] pixels = new byte[3];
      int[] data = new int[4];
      OpenTK.Graphics.OpenGL.GL.GetInteger(OpenTK.Graphics.OpenGL.GetPName.Viewport, data);
      OpenTK.Graphics.OpenGL.GL.ReadPixels<byte>(x, data[3] - y, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, pixels);
      OpenTK.Graphics.OpenGL.GL.PopMatrix();
      return pixels;
    }

    public static byte[] BackBufferSelect(int x, int y, List<uint> dl)
    {
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Fog);
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Lighting);
      OpenTK.Graphics.OpenGL.GL.ShadeModel(OpenTK.Graphics.OpenGL.ShadingModel.Flat);
      OpenTK.Graphics.OpenGL.GL.PushMatrix();
      OpenTK.Graphics.OpenGL.GL.Translate(0.0f, 0.0f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Rotate(0.0f, 1f, 0.0f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Rotate(0.0f, 0.0f, 1f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Rotate(0.0f, 0.0f, 0.0f, 1f);
      for (int index = 0; index < dl.Count<uint>(); ++index)
        OpenTK.Graphics.OpenGL.GL.CallList(dl[index]);
      byte[] pixels = new byte[3];
      int[] data = new int[4];
      OpenTK.Graphics.OpenGL.GL.GetInteger(OpenTK.Graphics.OpenGL.GetPName.Viewport, data);
      OpenTK.Graphics.OpenGL.GL.ReadPixels<byte>(x, data[3] - y, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, pixels);
      OpenTK.Graphics.OpenGL.GL.PopMatrix();
      return pixels;
    }

    public static byte[] BackBufferSelect(int x, int y)
    {
      byte[] pixels = new byte[3];
      int[] data = new int[4];
      OpenTK.Graphics.OpenGL.GL.GetInteger(OpenTK.Graphics.OpenGL.GetPName.Viewport, data);
      OpenTK.Graphics.OpenGL.GL.ReadPixels<byte>(x, data[3] - y, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, pixels);
      return pixels;
    }

    private static void CalculateActiveBones(ref List<ActiveBone> activeSkeleton, int amount)
    {
      for (int index = 0; index < activeSkeleton.Count; ++index)
      {
        if (activeSkeleton[index].length != 16777216)
        {
          activeSkeleton[index].length -= amount;
          if (activeSkeleton[index].length < 7)
          {
            activeSkeleton.RemoveAt(index);
            --index;
          }
        }
      }
    }

    private static List<Bone> GetHierarchy(List<Bone> skeleton, Bone activeBone)
    {
      List<Bone> boneList = new List<Bone>();
      boneList.Add(activeBone);
      bool flag = false;
      while (boneList[0].parent != (short) -1 && !flag)
      {
        foreach (Bone bone in skeleton)
        {
          if ((int) bone.id == (int) boneList[0].parent)
          {
            boneList.Insert(0, bone);
            flag = false;
            break;
          }
          flag = true;
        }
      }
      boneList.Reverse();
      return boneList;
    }

    private static void CalculateMatrix(List<Bone> hierarchy, float translationFactor)
    {
      for (int index = 0; index < hierarchy.Count; ++index)
      {
        Bone bone = hierarchy[index];
        Matrix4 translationMatrix1 = Matrix4.GetTranslationMatrix(bone.frame_xTranslation * translationFactor, bone.frame_yTranslation * translationFactor, bone.frame_zTranslation * translationFactor);
        Matrix4 rotationMatrixX = Matrix4.GetRotationMatrixX(Core.DegreeToRadian((double) bone.frame_xRotation));
        Matrix4 rotationMatrixY = Matrix4.GetRotationMatrixY(Core.DegreeToRadian((double) bone.frame_yRotation));
        Matrix4 rotationMatrixZ = Matrix4.GetRotationMatrixZ(Core.DegreeToRadian((double) bone.frame_zRotation));
        Matrix4 scaleMatrix = Matrix4.GetScaleMatrix(bone.frame_xScale, bone.frame_yScale, bone.frame_zScale);
        Matrix4 translationMatrix2 = Matrix4.GetTranslationMatrix(-bone.x, -bone.y, -bone.z);
        Matrix4 translationMatrix3 = Matrix4.GetTranslationMatrix(bone.x, bone.y, bone.z);
        Matrix4 matrix4_1 = translationMatrix1 * translationMatrix3;
        bone.transformOrder.Reverse();
        foreach (int num in bone.transformOrder)
        {
          if (num == 3)
            matrix4_1 *= rotationMatrixX;
          if (num == 4)
            matrix4_1 *= rotationMatrixY;
          if (num == 5)
            matrix4_1 *= rotationMatrixZ;
        }
        bone.transformOrder.Reverse();
        Matrix4 matrix4_2 = matrix4_1 * (scaleMatrix * translationMatrix2);
        bone.computedMatrix = matrix4_2;
      }
    }

    public static double DegreeToRadian(double angle) => Math.PI * angle / 180.0;

    private static int GetParentBone(int parent, ref List<Bone> skeleton)
    {
      if (parent == 0 || parent == -1)
        return -1;
      for (int index = 0; index < skeleton.Count; ++index)
      {
        if ((int) skeleton[index].id == parent)
          return index;
      }
      return -1;
    }

    public static void GetBuffersFromBKModelFile(
      ref byte[] bytesInFile,
      ref uint vboVertexHandle,
      ref float[] vertexData,
      ref uint vboColorHandle,
      ref byte[] vertexColorData,
      ref uint vboTexCoordHandle,
      ref float[] vertexTextureCoordData,
      ref List<uint> iboHandles,
      ref List<ushort[]> iboData,
      ref List<TextureData> textureData,
      ref List<Texture> textureSetting,
      ref List<int> textureSettingsBuffer)
    {
      List<ushort> ushortList = new List<ushort>();
      iboHandles.Clear();
      textureData.Clear();
      textureSettingsBuffer.Clear();
      int F3DStart = 0;
      int F3DCommands = 0;
      int F3DEnd = 0;
      int vertStart = 0;
      List<byte[]> commands = new List<byte[]>();
      bool newTexture = false;
      bool flag1 = false;
      int currentTexture = 0;
      float sScale = 0.0f;
      float tScale = 0.0f;
      F3DEXVert[] verts = new F3DEXVert[32];
      ushort[] numArray1 = new ushort[32];
      F3DEXTexture[] textures = new F3DEXTexture[1];
      try
      {
        int collStart = 0;
        int VTCount = 0;
        int textureCount = 0;
        if (!F3DEX2.ReadModel(ref bytesInFile, ref collStart, ref F3DStart, ref F3DCommands, ref F3DEnd, ref vertStart, ref VTCount, ref textureCount, ref verts, ref commands, ref textures))
          return;
        vertexData = new float[VTCount * 3];
        vertexColorData = new byte[VTCount * 4];
        vertexTextureCoordData = new float[VTCount * 2];
        int index1 = 0;
        int index2 = 0;
        int index3 = 0;
        CullMode cm = CullMode.CULLNONE;
        foreach (F3DEXVert f3DexVert in verts)
        {
          vertexData[index1] = (float) f3DexVert.x;
          vertexData[index1 + 1] = (float) f3DexVert.y;
          vertexData[index1 + 2] = (float) f3DexVert.z;
          vertexColorData[index2] = f3DexVert.r;
          vertexColorData[index2 + 1] = f3DexVert.g;
          vertexColorData[index2 + 2] = f3DexVert.b;
          vertexColorData[index2 + 3] = f3DexVert.a;
          vertexTextureCoordData[index3] = (float) f3DexVert.u;
          vertexTextureCoordData[index3 + 1] = (float) f3DexVert.v;
          index1 += 3;
          index2 += 4;
          index3 += 2;
        }
        if (Core.usingARB)
        {
          OpenTK.Graphics.OpenGL.GL.Arb.GenBuffers(1, out vboVertexHandle);
          OpenTK.Graphics.OpenGL.GL.Arb.BindBuffer(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, vboVertexHandle);
          OpenTK.Graphics.OpenGL.GL.Arb.BufferData<float>(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, (IntPtr) (vertexData.Length * 4), vertexData, OpenTK.Graphics.OpenGL.BufferUsageArb.StaticDraw);
          OpenTK.Graphics.OpenGL.GL.Arb.BindBuffer(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, 0);
          OpenTK.Graphics.OpenGL.GL.Arb.GenBuffers(1, out vboColorHandle);
          OpenTK.Graphics.OpenGL.GL.Arb.BindBuffer(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, vboColorHandle);
          OpenTK.Graphics.OpenGL.GL.Arb.BufferData<byte>(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, (IntPtr) (vertexColorData.Length * 1), vertexColorData, OpenTK.Graphics.OpenGL.BufferUsageArb.StaticDraw);
          OpenTK.Graphics.OpenGL.GL.Arb.BindBuffer(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, 0);
        }
        else
        {
          OpenTK.Graphics.OpenGL.GL.GenBuffers(1, out vboVertexHandle);
          OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, vboVertexHandle);
          OpenTK.Graphics.OpenGL.GL.BufferData<float>(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, (IntPtr) (vertexData.Length * 4), vertexData, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
          OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, 0);
          OpenTK.Graphics.OpenGL.GL.GenBuffers(1, out vboColorHandle);
          OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, vboColorHandle);
          OpenTK.Graphics.OpenGL.GL.BufferData<byte>(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, (IntPtr) (vertexColorData.Length * 1), vertexColorData, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
          OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, 0);
        }
        for (int index4 = 0; index4 < F3DCommands; ++index4)
        {
          byte[] command = commands[index4];
          uint w1 = (uint) ((int) command[4] * 16777216 + (int) command[5] * 65536 + (int) command[6] * 256) + (uint) command[7];
          int num1 = (int) command[1];
          int num2 = (int) command[2];
          int num3 = (int) command[3];
          if ((int) command[0] == F3DEX2.F3DEX2_ENDDL)
          {
            if (index4 + 1 < F3DCommands)
            {
              int num4 = (int) commands[index4 + 1][0];
            }
            newTexture = false;
            flag1 = false;
          }
          if (command[0] == (byte) 253)
            F3DEX2.GL_G_SETTIMG(ref currentTexture, textureCount, w1, ref textures, commands[index4 + 2], ref newTexture, sScale, tScale);
          if (command[0] == (byte) 252)
            F3DEX2.GL_G_Combine(w1);
          if (command[0] == (byte) 183)
            cm = (CullMode) ((w1 & (uint) ushort.MaxValue) >> 8);
          if (command[0] == (byte) 245)
            F3DEX2.GL_G_SETTILE(command);
          if (command[0] == (byte) 240)
          {
            int palSize = (int) ((w1 << 8 >> 8 & 16773120U) >> 14) * 2 + 2;
            textures[currentTexture].loadPalette(bytesInFile, textureCount, palSize);
            if (commands[index4 + 4][0] == (byte) 186)
              newTexture = true;
          }
          if ((int) command[0] == F3DEX2.F3DEX2_TEXTURE)
          {
            sScale = (float) (w1 >> 16) / 65536f;
            tScale = (float) (w1 & (uint) ushort.MaxValue) / 65536f;
          }
          if ((int) command[0] == F3DEX2.F3DEX2_VTX)
          {
            byte[] numArray2 = commands[index4];
            int num5 = (int) numArray2[4] * 16777216 + (int) numArray2[5] * 65536 + (int) numArray2[6] * 256 + (int) numArray2[7];
            int num6 = (int) numArray2[1] * 65536 + (int) numArray2[2] * 256 + (int) numArray2[3];
            byte num7 = (byte) ((uint) num6 >> 12 & (uint) byte.MaxValue);
            byte num8 = (byte) (((uint) num6 >> 1 & (uint) sbyte.MaxValue) - (uint) num7);
            if (num8 > (byte) 63)
              num8 = (byte) 63;
            uint num9 = ((uint) (num5 << 8) >> 8) / 16U;
            try
            {
              for (int index5 = (int) num8; index5 < (int) num7 + (int) num8; ++index5)
              {
                if ((long) num9 < (long) verts.Length)
                  numArray1[index5] = (ushort) num9;
                ++num9;
              }
            }
            catch (Exception ex)
            {
            }
            if (textureSetting.Count > 0)
            {
              ushort[] array = ushortList.ToArray();
              uint buffers = 0;
              if (Core.usingARB)
              {
                OpenTK.Graphics.OpenGL.GL.Arb.GenBuffers(1, out buffers);
                OpenTK.Graphics.OpenGL.GL.Arb.BindBuffer(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, buffers);
                OpenTK.Graphics.OpenGL.GL.Arb.BufferData<ushort>(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, (IntPtr) (array.Length * 2), array, OpenTK.Graphics.OpenGL.BufferUsageArb.StaticDraw);
                OpenTK.Graphics.OpenGL.GL.Arb.BindBuffer(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, 0);
              }
              else
              {
                OpenTK.Graphics.OpenGL.GL.GenBuffers(1, out buffers);
                OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, buffers);
                OpenTK.Graphics.OpenGL.GL.BufferData<ushort>(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, (IntPtr) (array.Length * 2), array, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
                OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, 0);
              }
              iboHandles.Add(buffers);
              iboData.Add(array);
              ushortList.Clear();
            }
            if (newTexture)
            {
              newTexture = false;
              flag1 = true;
              int texturesGL = 0;
              F3DEX2.F3DEX_2_GL_TEXTURE(ref bytesInFile, ref textures[currentTexture], textureCount, ref texturesGL, false);
              bool flag2 = true;
              int textureData_ = 0;
              for (int index6 = 0; index6 < textureData.Count<TextureData>(); ++index6)
              {
                if (textureData[index6].gl.GetHashCode() == textures[currentTexture].pixels.GetHashCode())
                {
                  flag2 = false;
                  textureData_ = index6;
                  break;
                }
              }
              if (flag2)
              {
                textureData.Add(new TextureData(textures[currentTexture].n64TextureBytes, textures[currentTexture].pixels, textures[currentTexture].textureWidth, textures[currentTexture].textureHeight, Tile.cms, Tile.cmt));
                textureData_ = textureData.Count<TextureData>() - 1;
              }
              textureSetting.Add(new Texture(textures[currentTexture].textureWidth, textures[currentTexture].textureHeight, Tile.cms, Tile.cmt, texturesGL, textureData_, cm, (byte) 8, textures[currentTexture].textureHRatio, textures[currentTexture].textureWRatio, textures[currentTexture].palSize, false, false));
              textureSettingsBuffer.Add(textureSetting.Count - 1);
            }
            else if (flag1)
            {
              textureSettingsBuffer.Add(textureSetting.Count - 1);
            }
            else
            {
              textureSetting.Add(new Texture(0, 0, 0, 0, -1, -1, cm, (byte) 8, 0.0f, 0.0f, 0, false, false));
              textureSettingsBuffer.Add(textureSetting.Count - 1);
            }
          }
          if ((int) command[0] == F3DEX2.F3DEX2_TRI1)
          {
            short num10 = (short) ((int) command[1] / 2);
            short num11 = (short) ((int) command[2] / 2);
            short num12 = (short) ((int) command[3] / 2);
            ushortList.Add(numArray1[(int) num10]);
            ushortList.Add(numArray1[(int) num11]);
            ushortList.Add(numArray1[(int) num12]);
            vertexTextureCoordData[(int) numArray1[(int) num10] * 2] = (float) verts[(int) numArray1[(int) num10]].u * textures[currentTexture].textureWRatio;
            vertexTextureCoordData[(int) numArray1[(int) num10] * 2 + 1] = (float) verts[(int) numArray1[(int) num10]].v * textures[currentTexture].textureHRatio;
            vertexTextureCoordData[(int) numArray1[(int) num11] * 2] = (float) verts[(int) numArray1[(int) num11]].u * textures[currentTexture].textureWRatio;
            vertexTextureCoordData[(int) numArray1[(int) num11] * 2 + 1] = (float) verts[(int) numArray1[(int) num11]].v * textures[currentTexture].textureHRatio;
            vertexTextureCoordData[(int) numArray1[(int) num12] * 2] = (float) verts[(int) numArray1[(int) num12]].u * textures[currentTexture].textureWRatio;
            vertexTextureCoordData[(int) numArray1[(int) num12] * 2 + 1] = (float) verts[(int) numArray1[(int) num12]].v * textures[currentTexture].textureHRatio;
          }
          if ((int) command[0] == F3DEX2.F3DEX2_TRI2)
          {
            short num13 = (short) ((int) command[1] / 2);
            short num14 = (short) ((int) command[2] / 2);
            short num15 = (short) ((int) command[3] / 2);
            ushortList.Add(numArray1[(int) num13]);
            ushortList.Add(numArray1[(int) num14]);
            ushortList.Add(numArray1[(int) num15]);
            vertexTextureCoordData[(int) numArray1[(int) num13] * 2] = (float) verts[(int) numArray1[(int) num13]].u * textures[currentTexture].textureWRatio;
            vertexTextureCoordData[(int) numArray1[(int) num13] * 2 + 1] = (float) verts[(int) numArray1[(int) num13]].v * textures[currentTexture].textureHRatio;
            vertexTextureCoordData[(int) numArray1[(int) num14] * 2] = (float) verts[(int) numArray1[(int) num14]].u * textures[currentTexture].textureWRatio;
            vertexTextureCoordData[(int) numArray1[(int) num14] * 2 + 1] = (float) verts[(int) numArray1[(int) num14]].v * textures[currentTexture].textureHRatio;
            vertexTextureCoordData[(int) numArray1[(int) num15] * 2] = (float) verts[(int) numArray1[(int) num15]].u * textures[currentTexture].textureWRatio;
            vertexTextureCoordData[(int) numArray1[(int) num15] * 2 + 1] = (float) verts[(int) numArray1[(int) num15]].v * textures[currentTexture].textureHRatio;
            short num16 = (short) ((int) command[5] / 2);
            short num17 = (short) ((int) command[6] / 2);
            short num18 = (short) ((int) command[7] / 2);
            ushortList.Add(numArray1[(int) num16]);
            ushortList.Add(numArray1[(int) num17]);
            ushortList.Add(numArray1[(int) num18]);
            vertexTextureCoordData[(int) numArray1[(int) num16] * 2] = (float) verts[(int) numArray1[(int) num16]].u * textures[currentTexture].textureWRatio;
            vertexTextureCoordData[(int) numArray1[(int) num16] * 2 + 1] = (float) verts[(int) numArray1[(int) num16]].v * textures[currentTexture].textureHRatio;
            vertexTextureCoordData[(int) numArray1[(int) num17] * 2] = (float) verts[(int) numArray1[(int) num17]].u * textures[currentTexture].textureWRatio;
            vertexTextureCoordData[(int) numArray1[(int) num17] * 2 + 1] = (float) verts[(int) numArray1[(int) num17]].v * textures[currentTexture].textureHRatio;
            vertexTextureCoordData[(int) numArray1[(int) num18] * 2] = (float) verts[(int) numArray1[(int) num18]].u * textures[currentTexture].textureWRatio;
            vertexTextureCoordData[(int) numArray1[(int) num18] * 2 + 1] = (float) verts[(int) numArray1[(int) num18]].v * textures[currentTexture].textureHRatio;
          }
        }
        ushort[] array1 = ushortList.ToArray();
        uint buffers1 = 0;
        if (Core.usingARB)
        {
          OpenTK.Graphics.OpenGL.GL.Arb.GenBuffers(1, out buffers1);
          OpenTK.Graphics.OpenGL.GL.Arb.BindBuffer(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, buffers1);
          OpenTK.Graphics.OpenGL.GL.Arb.BufferData<ushort>(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, (IntPtr) (array1.Length * 2), array1, OpenTK.Graphics.OpenGL.BufferUsageArb.StaticDraw);
          OpenTK.Graphics.OpenGL.GL.Arb.BindBuffer(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, 0);
        }
        else
        {
          OpenTK.Graphics.OpenGL.GL.GenBuffers(1, out buffers1);
          OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, buffers1);
          OpenTK.Graphics.OpenGL.GL.BufferData<ushort>(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, (IntPtr) (array1.Length * 2), array1, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
          OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, 0);
        }
        iboHandles.Add(buffers1);
        iboData.Add(array1);
        ushortList.Clear();
        if (Core.usingARB)
        {
          OpenTK.Graphics.OpenGL.GL.Arb.GenBuffers(1, out vboTexCoordHandle);
          OpenTK.Graphics.OpenGL.GL.Arb.BindBuffer(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, vboTexCoordHandle);
          OpenTK.Graphics.OpenGL.GL.Arb.BufferData<float>(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, (IntPtr) (vertexTextureCoordData.Length * 4), vertexTextureCoordData, OpenTK.Graphics.OpenGL.BufferUsageArb.StaticDraw);
          OpenTK.Graphics.OpenGL.GL.Arb.BindBuffer(OpenTK.Graphics.OpenGL.BufferTargetArb.ArrayBuffer, 0);
        }
        else
        {
          OpenTK.Graphics.OpenGL.GL.GenBuffers(1, out vboTexCoordHandle);
          OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, vboTexCoordHandle);
          OpenTK.Graphics.OpenGL.GL.BufferData<float>(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, (IntPtr) (vertexTextureCoordData.Length * 4), vertexTextureCoordData, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
          OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, 0);
        }
      }
      catch (Exception ex)
      {
        Core.logger.Log(NLog.LogLevel.Error, "Failed to get buffers from BK model file");
        Core.logger.Log(NLog.LogLevel.Debug, ex.Message);
      }
    }

    public static float[] DrawDLAnimationFrameVBO(
      ref byte[] bytesInFile,
      List<Bone> skeleton,
      float translationFactor,
      float[] vertexData)
    {
      float[] transformedVertexData = (float[]) vertexData.Clone();
      int F3DStart = 0;
      int F3DCommands = 0;
      int F3DEnd = 0;
      int vertStart = 0;
      List<byte[]> commands = new List<byte[]>();
      F3DEXVert[] f3DexVertArray = new F3DEXVert[32];
      int[] cache = new int[32];
      F3DEXTexture[] textures = new F3DEXTexture[1];
      try
      {
        Stopwatch.StartNew();
        int collStart = 0;
        int VTCount = 0;
        int textureCount = 0;
        if (F3DEX2.ReadModel(ref bytesInFile, ref collStart, ref F3DStart, ref F3DCommands, ref F3DEnd, ref vertStart, ref VTCount, ref textureCount, ref f3DexVertArray, ref commands, ref textures))
        {
          int num1 = ((int) bytesInFile[4] << 24) + ((int) bytesInFile[5] << 16) + ((int) bytesInFile[6] << 8) + (int) bytesInFile[7];
          int length = bytesInFile.Length;
          int amount = 8;
          List<ActiveBone> activeSkeleton = new List<ActiveBone>();
          if (num1 != 0)
          {
            for (int index = num1; index + 4 < length; index += amount)
            {
              Core.CalculateActiveBones(ref activeSkeleton, amount);
              amount = 8;
              switch ((int) bytesInFile[index + 2] + (int) bytesInFile[index + 3])
              {
                case 2:
                  activeSkeleton.Add(new ActiveBone(bytesInFile[index + 9], ((int) bytesInFile[index + 6] << 8) + (int) bytesInFile[index + 7]));
                  if (activeSkeleton[activeSkeleton.Count - 1].length == 0)
                    activeSkeleton[activeSkeleton.Count - 1].length = activeSkeleton.Count != 1 ? (activeSkeleton[activeSkeleton.Count - 2].length != 0 ? activeSkeleton[activeSkeleton.Count - 2].length : 16777216) : 50331648;
                  amount = 16;
                  break;
                case 3:
                case 17:
                case 24:
                  int start = ((int) bytesInFile[index + 8] << 8) + (int) bytesInFile[index + 9];
                  if (activeSkeleton.Count > 0)
                    Core.RenderToEndDLVBO(skeleton, skeleton[(int) activeSkeleton[activeSkeleton.Count - 1].bone], (Bone) null, (int) activeSkeleton[0].bone, start, -1, ref bytesInFile, ref commands, textureCount, ref textures, ref f3DexVertArray, ref cache, ref vertexData, ref transformedVertexData, translationFactor);
                  else
                    Core.RenderToEndDLVBO(skeleton, skeleton[0], (Bone) null, 0, start, -1, ref bytesInFile, ref commands, textureCount, ref textures, ref f3DexVertArray, ref cache, ref vertexData, ref transformedVertexData, translationFactor);
                  amount = 16;
                  break;
                case 5:
                  amount = 24;
                  break;
                case 10:
                  amount = 16;
                  break;
                case 12:
                  int num2 = ((int) bytesInFile[index + 6] << 8) + (int) bytesInFile[index + 7];
                  amount = 16;
                  break;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Core.logger.Log(NLog.LogLevel.Error, "The animation could not be played - General Failure");
        Core.logger.Log(NLog.LogLevel.Debug, ex.Message);
      }
      return transformedVertexData;
    }

    private static void RenderToEndDLVBO(
      List<Bone> skeleton,
      Bone activeBone,
      Bone parentBone,
      int activeRoot,
      int start,
      int end,
      ref byte[] bytesInFile,
      ref List<byte[]> commands,
      int textureCount,
      ref F3DEXTexture[] textures,
      ref F3DEXVert[] f3d_verts,
      ref int[] cache,
      ref float[] vertexData,
      ref float[] transformedVertexData,
      float translationFactor)
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      try
      {
        List<Bone> hierarchy = end == -1 ? Core.GetHierarchy(skeleton, activeBone) : Core.GetHierarchy(skeleton, parentBone);
        stopwatch.Stop();
        long elapsedMilliseconds1 = stopwatch.ElapsedMilliseconds;
        stopwatch.Reset();
        stopwatch.Start();
        Core.CalculateMatrix(hierarchy, translationFactor);
        stopwatch.Stop();
        long elapsedMilliseconds2 = stopwatch.ElapsedMilliseconds;
        int count = commands.Count;
        bool flag1 = true;
        bool flag2 = false;
        if (end > -1)
          flag1 = false;
        F3DEXVert[] f3DexVertArray = new F3DEXVert[64];
        for (int index1 = start; index1 < count; ++index1)
        {
          byte[] numArray1 = commands[index1];
          int num1 = (int) numArray1[4];
          int num2 = (int) numArray1[5];
          int num3 = (int) numArray1[6];
          int num4 = (int) numArray1[7];
          int num5 = (int) numArray1[1];
          int num6 = (int) numArray1[2];
          int num7 = (int) numArray1[3];
          if ((int) numArray1[0] == F3DEX2.F3DEX2_ENDDL)
          {
            if (flag1)
              break;
            index1 = end - 1;
            flag1 = true;
            flag2 = true;
            hierarchy = Core.GetHierarchy(skeleton, activeBone);
            Core.CalculateMatrix(hierarchy, translationFactor);
          }
          if ((int) numArray1[0] == F3DEX2.F3DEX2_VTX)
          {
            byte[] numArray2 = commands[index1];
            int num8 = (int) numArray2[4] * 16777216 + (int) numArray2[5] * 65536 + (int) numArray2[6] * 256 + (int) numArray2[7];
            int num9 = (int) numArray2[1] * 65536 + (int) numArray2[2] * 256 + (int) numArray2[3];
            byte num10 = (byte) ((uint) num9 >> 12 & (uint) byte.MaxValue);
            byte num11 = (byte) (((uint) num9 >> 1 & (uint) sbyte.MaxValue) - (uint) num10);
            if (num11 > (byte) 63)
              num11 = (byte) 63;
            uint num12 = ((uint) (num8 << 8) >> 8) / 16U;
            try
            {
              for (int index2 = (int) num11; index2 < (int) num10 + (int) num11; ++index2)
              {
                if ((long) num12 < (long) f3d_verts.Length)
                  cache[index2] = (int) (ushort) num12;
                ++num12;
              }
            }
            catch (Exception ex)
            {
            }
            int num13 = flag2 ? 1 : 0;
            if (activeBone != null)
            {
              for (int index3 = 0; index3 < hierarchy.Count; ++index3)
              {
                for (int index4 = (int) num11; index4 < (int) num10 + (int) num11; ++index4)
                {
                  int num14 = cache[index4];
                  Vector3 vector3_1 = new Vector3(transformedVertexData[num14 * 3], transformedVertexData[num14 * 3 + 1], transformedVertexData[num14 * 3 + 2]);
                  if (index3 == 0)
                    vector3_1 = new Vector3(vertexData[num14 * 3], vertexData[num14 * 3 + 1], vertexData[num14 * 3 + 2]);
                  Vector3 vector3_2 = hierarchy[index3].computedMatrix * vector3_1;
                  transformedVertexData[num14 * 3] = (float) (short) vector3_2.x;
                  transformedVertexData[num14 * 3 + 1] = (float) (short) vector3_2.y;
                  transformedVertexData[num14 * 3 + 2] = (float) (short) vector3_2.z;
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Core.logger.Log(NLog.LogLevel.Error, "The animation could not be played - General Failure");
        Core.logger.Log(NLog.LogLevel.Debug, ex.Message);
      }
    }

    public static float[] DrawVertAnimationFrameVBO(
      ref byte[] bytesInFile,
      List<Bone> skeleton,
      float translationFactor,
      float[] vertexData)
    {
      float[] numArray = (float[]) vertexData.Clone();
      int F3DStart = 0;
      int F3DCommands = 0;
      int F3DEnd = 0;
      int vertStart = 0;
      List<byte[]> commands = new List<byte[]>();
      F3DEXVert[] verts = new F3DEXVert[32];
      F3DEXVert[] f3DexVertArray = new F3DEXVert[32];
      F3DEXTexture[] textures = new F3DEXTexture[1];
      try
      {
        int collStart = 0;
        int VTCount = 0;
        int textureCount = 0;
        if (F3DEX2.ReadModel(ref bytesInFile, ref collStart, ref F3DStart, ref F3DCommands, ref F3DEnd, ref vertStart, ref VTCount, ref textureCount, ref verts, ref commands, ref textures))
        {
          Core.CalculateMatrix(skeleton, translationFactor);
          for (int index1 = 0; index1 < skeleton.Count; ++index1)
          {
            List<Bone> hierarchy = Core.GetHierarchy(skeleton, skeleton[index1]);
            for (int index2 = 0; index2 < hierarchy.Count; ++index2)
            {
              foreach (int vert in skeleton[index1].verts)
              {
                if (vert < verts.Length)
                {
                  Vector3 vector3_1 = new Vector3(vertexData[vert * 3], vertexData[vert * 3 + 1], vertexData[vert * 3 + 2]);
                  Vector3 vector3_2 = hierarchy[index2].computedMatrix * vector3_1;
                  vertexData[vert * 3] = (float) (short) vector3_2.x;
                  vertexData[vert * 3 + 1] = (float) (short) vector3_2.y;
                  vertexData[vert * 3 + 2] = (float) (short) vector3_2.z;
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Core.logger.Log(NLog.LogLevel.Error, "The animation could not be played - General Failure (vertex animation)");
        Core.logger.Log(NLog.LogLevel.Debug, ex.Message);
      }
      return vertexData;
    }

    public static void unProject(
      float xrot,
      float yrot,
      float xpos,
      float ypos,
      float zpos,
      int x,
      int y,
      out double posX,
      out double posY,
      out double posZ,
      float depth)
    {
      OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
      OpenTK.Graphics.OpenGL.GL.LoadIdentity();
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Fog);
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Lighting);
      OpenTK.Graphics.OpenGL.GL.ShadeModel(OpenTK.Graphics.OpenGL.ShadingModel.Flat);
      OpenTK.Graphics.OpenGL.GL.Translate(-xpos, -ypos, -zpos);
      OpenTK.Graphics.OpenGL.GL.Rotate(xrot, 1f, 0.0f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Rotate(yrot, 0.0f, 1f, 0.0f);
      byte[] numArray1 = new byte[3];
      int[] numArray2 = new int[4];
      double[] numArray3 = new double[16];
      double[] numArray4 = new double[16];
      OpenTK.Graphics.OpenGL.GL.GetInteger(OpenTK.Graphics.OpenGL.GetPName.Viewport, numArray2);
      OpenTK.Graphics.OpenGL.GL.GetDouble(OpenTK.Graphics.OpenGL.GetPName.Modelview0MatrixExt, numArray3);
      OpenTK.Graphics.OpenGL.GL.GetDouble(OpenTK.Graphics.OpenGL.GetPName.ProjectionMatrix, numArray4);
      float[] pixels = new float[1];
      OpenTK.Graphics.OpenGL.GL.ReadPixels<float>(x, numArray2[3] - y, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, OpenTK.Graphics.OpenGL.PixelType.Float, pixels);
      Tao.OpenGl.Glu.gluUnProject((double) x, (double) (numArray2[3] - y), (double) depth, numArray3, numArray4, numArray2, out posX, out posY, out posZ);
      Core.InitGl();
    }

    public static void DrawModelBoundary(BoundingBox bb, bool diag)
    {
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
      OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Lines);
      OpenTK.Graphics.OpenGL.GL.Color3(Color.White);
      Core.DrawLine(bb.smallX, bb.smallY, bb.smallZ, bb.largeX, bb.smallY, bb.smallZ);
      Core.DrawLine(bb.smallX, bb.smallY, bb.smallZ, bb.smallX, bb.smallY, bb.largeZ);
      Core.DrawLine(bb.largeX, bb.smallY, bb.smallZ, bb.largeX, bb.smallY, bb.largeZ);
      Core.DrawLine(bb.smallX, bb.smallY, bb.largeZ, bb.largeX, bb.smallY, bb.largeZ);
      Core.DrawLine(bb.smallX, bb.largeY, bb.smallZ, bb.largeX, bb.largeY, bb.smallZ);
      Core.DrawLine(bb.smallX, bb.largeY, bb.smallZ, bb.smallX, bb.largeY, bb.largeZ);
      Core.DrawLine(bb.largeX, bb.largeY, bb.smallZ, bb.largeX, bb.largeY, bb.largeZ);
      Core.DrawLine(bb.smallX, bb.largeY, bb.largeZ, bb.largeX, bb.largeY, bb.largeZ);
      Core.DrawLine(bb.smallX, bb.smallY, bb.largeZ, bb.smallX, bb.largeY, bb.largeZ);
      Core.DrawLine(bb.largeX, bb.smallY, bb.largeZ, bb.largeX, bb.largeY, bb.largeZ);
      Core.DrawLine(bb.smallX, bb.smallY, bb.smallZ, bb.smallX, bb.largeY, bb.smallZ);
      Core.DrawLine(bb.largeX, bb.smallY, bb.smallZ, bb.largeX, bb.largeY, bb.smallZ);
      if (diag)
      {
        Core.DrawLine(bb.smallX, bb.smallY, bb.smallZ, bb.largeX, bb.smallY, bb.largeZ);
        Core.DrawLine(bb.smallX, bb.smallY, bb.largeZ, bb.largeX, bb.smallY, bb.smallZ);
        Core.DrawLine(bb.smallX, bb.largeY, bb.smallZ, bb.largeX, bb.largeY, bb.largeZ);
        Core.DrawLine(bb.smallX, bb.largeY, bb.largeZ, bb.largeX, bb.largeY, bb.smallZ);
        Core.DrawLine(bb.smallX, bb.smallY, bb.largeZ, bb.largeX, bb.largeY, bb.largeZ);
        Core.DrawLine(bb.largeX, bb.smallY, bb.largeZ, bb.smallX, bb.largeY, bb.largeZ);
        Core.DrawLine(bb.smallX, bb.smallY, bb.smallZ, bb.largeX, bb.largeY, bb.smallZ);
        Core.DrawLine(bb.largeX, bb.smallY, bb.smallZ, bb.smallX, bb.largeY, bb.smallZ);
        Core.DrawLine(bb.smallX, bb.smallY, bb.smallZ, bb.smallX, bb.largeY, bb.largeZ);
        Core.DrawLine(bb.smallX, bb.smallY, bb.largeZ, bb.smallX, bb.largeY, bb.smallZ);
        Core.DrawLine(bb.largeX, bb.smallY, bb.smallZ, bb.largeX, bb.largeY, bb.largeZ);
        Core.DrawLine(bb.largeX, bb.smallY, bb.largeZ, bb.largeX, bb.largeY, bb.smallZ);
      }
      OpenTK.Graphics.OpenGL.GL.End();
    }

    public static void drawObjectBoundingBox(ObjectData obj)
    {
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
      OpenTK.Graphics.OpenGL.GL.PushMatrix();
      OpenTK.Graphics.OpenGL.GL.Translate((float) obj.locX, (float) obj.locY, (float) obj.locZ);
      if (obj.Pointer == 32808 || obj.Pointer == 34432)
        OpenTK.Graphics.OpenGL.GL.Rotate((float) obj.rot, 0.0f, 0.0f, 1f);
      else if (obj.Pointer != 32872)
        OpenTK.Graphics.OpenGL.GL.Rotate((float) obj.rot, 0.0f, 1f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.LineWidth(2f);
      float num = (float) obj.size / 100f;
      if (obj.Pointer == 0)
        num = 1f;
      if (Core.check2D(ref obj) || obj.Pointer == 1)
        num = 1f;
      OpenTK.Graphics.OpenGL.GL.Scale(num, num, num);
      Core.DrawModelBoundary(obj.bb, false);
      if (obj.radius != (ushort) 0)
      {
        if (obj.colour == (byte) 0)
          OpenTK.Graphics.OpenGL.GL.Color3(Color.Red);
        if (obj.colour == (byte) 1)
          OpenTK.Graphics.OpenGL.GL.Color3(Color.Green);
        if (obj.colour == (byte) 2)
          OpenTK.Graphics.OpenGL.GL.Color3(Color.Blue);
        if (obj.colour == (byte) 3)
          OpenTK.Graphics.OpenGL.GL.Color3(Color.White);
        Core.wire = OpenTK.Graphics.Glu.NewQuadric();
        OpenTK.Graphics.Glu.QuadricDrawStyle(Core.wire, QuadricDrawStyle.Line);
        OpenTK.Graphics.Glu.Sphere(Core.wire, (double) obj.radius, 5, 5);
        OpenTK.Graphics.Glu.DeleteQuadric(Core.wire);
      }
      OpenTK.Graphics.OpenGL.GL.PopMatrix();
    }

    public static double[] screenToWorld(int x, int y, float xrot, float yrot, float zrot)
    {
      double num1 = (double) yrot / 180.0;
      double num2 = (double) xrot / 180.0;
      double num3 = (double) zrot / 180.0;
      int[] numArray1 = new int[4];
      double[] numArray2 = new double[16];
      double[] numArray3 = new double[16];
      OpenTK.Graphics.OpenGL.GL.GetInteger(OpenTK.Graphics.OpenGL.GetPName.Viewport, numArray1);
      OpenTK.Graphics.OpenGL.GL.GetDouble(OpenTK.Graphics.OpenGL.GetPName.Modelview0MatrixExt, numArray2);
      OpenTK.Graphics.OpenGL.GL.GetDouble(OpenTK.Graphics.OpenGL.GetPName.ProjectionMatrix, numArray3);
      int num4 = numArray1[3];
      int x1 = x;
      int num5 = y;
      int y1 = num4 - num5;
      float[] pixels = new float[1];
      OpenTK.Graphics.OpenGL.GL.ReadPixels<float>(x1, y1, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, OpenTK.Graphics.OpenGL.PixelType.Float, pixels);
      double objX = 0.0;
      double objY = 0.0;
      double objZ = 0.0;
      Tao.OpenGl.Glu.gluUnProject((double) x1, (double) y1, (double) pixels[0], numArray2, numArray3, numArray1, out objX, out objY, out objZ);
      return new double[3]{ objX, objY, objZ };
    }

    public static void DrawModelFilePicking(string model_)
    {
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.CullFace);
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Fog);
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Lighting);
      OpenTK.Graphics.OpenGL.GL.ShadeModel(OpenTK.Graphics.OpenGL.ShadingModel.Flat);
      int F3DStart = 0;
      int F3DCommands = 0;
      int F3DEnd = 0;
      int vertStart = 0;
      List<byte[]> commands = new List<byte[]>();
      F3DEXVert[] verts = new F3DEXVert[32];
      F3DEXVert[] cache = new F3DEXVert[32];
      F3DEXTexture[] textures = new F3DEXTexture[1];
      if (!File.Exists(model_))
        return;
      try
      {
        BinaryReader binaryReader = new BinaryReader((Stream) File.Open(model_, FileMode.Open));
        long length = binaryReader.BaseStream.Length;
        byte[] bytesInFile = new byte[length];
        binaryReader.BaseStream.Read(bytesInFile, 0, (int) length);
        binaryReader.Close();
        int collStart = 0;
        int VTCount = 0;
        int textureCount = 0;
        if (!F3DEX2.ReadModel(ref bytesInFile, ref collStart, ref F3DStart, ref F3DCommands, ref F3DEnd, ref vertStart, ref VTCount, ref textureCount, ref verts, ref commands, ref textures))
          return;
        OpenTK.Graphics.OpenGL.GL.PushMatrix();
        for (int index = 0; index < F3DCommands; ++index)
        {
          byte[] numArray = commands[index];
          int num1 = (int) numArray[4];
          int num2 = (int) numArray[5];
          int num3 = (int) numArray[6];
          int num4 = (int) numArray[7];
          int num5 = (int) numArray[1];
          int num6 = (int) numArray[2];
          int num7 = (int) numArray[3];
          if ((int) numArray[0] == F3DEX2.F3DEX2_VTX)
            F3DEX2.GL_VTX_PICKING(numArray, ref cache, ref verts);
          if ((int) numArray[0] == F3DEX2.F3DEX2_TRI1)
            F3DEX2.GL_TRI1_PICKING(numArray, ref cache);
          else if ((int) numArray[0] == F3DEX2.F3DEX2_TRI2)
            F3DEX2.GL_TRI2_PICKING(numArray, ref cache);
        }
        OpenTK.Graphics.OpenGL.GL.PopMatrix();
      }
      catch (Exception ex)
      {
        Core.logger.Log(NLog.LogLevel.Debug, ex.Message);
        Core.logger.Log(NLog.LogLevel.Error, "Could not read file " + model_);
        OpenTK.Graphics.OpenGL.GL.PopMatrix();
      }
    }

    public static byte[] pickRectSelect(Point p1, Point p2)
    {
      if (p1.X < 0)
        p1.X = 1;
      if (p2.X > Core.width)
        p2.X = Core.width - 3;
      if (p1.Y < 0)
        p1.Y = 1;
      if (p2.Y > Core.height)
        p2.Y = Core.height - 3;
      int width = p2.X - p1.X;
      int height = p2.Y - p1.Y;
      if (Core.rect_pixel.Length < (Core.width + 1) * (Core.height + 1) * 3 + 3)
        Core.rect_pixel = new byte[(Core.width + 1) * (Core.height + 1) * 3 + 3];
      int[] data = new int[4];
      OpenTK.Graphics.OpenGL.GL.GetInteger(OpenTK.Graphics.OpenGL.GetPName.Viewport, data);
      int num = (width + 1) * (height + 1) * 3 + 3;
      if (Core.rect_pixel.Length > num)
        OpenTK.Graphics.OpenGL.GL.ReadPixels<byte>(p1.X, data[3] - p2.Y, width, height, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, Core.rect_pixel);
      OpenTK.Graphics.OpenGL.GL.PopMatrix();
      return Core.rect_pixel;
    }

    public static void DrawLine(F3DEXVert v1, F3DEXVert v2)
    {
      OpenTK.Graphics.OpenGL.GL.Vertex3(v1.x, v1.y, v1.z);
      OpenTK.Graphics.OpenGL.GL.Vertex3(v2.x, v2.y, v2.z);
    }

    public static void DrawLine(int x1, int y1, int z1, int x2, int y2, int z2)
    {
      OpenTK.Graphics.OpenGL.GL.Vertex3(x1, y1, z1);
      OpenTK.Graphics.OpenGL.GL.Vertex3(x2, y2, z2);
    }

    public static void DrawCube(int sx, int sy, int sz, int lx, int ly, int lz)
    {
      OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Quads);
      OpenTK.Graphics.OpenGL.GL.Vertex3(lx, sy, sz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(lx, ly, sz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(lx, ly, lz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(lx, sy, lz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(sx, sy, sz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(sx, sy, lz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(sx, ly, lz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(sx, ly, sz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(lx, sy, lz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(lx, ly, lz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(sx, ly, lz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(sx, sy, lz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(sx, sy, sz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(sx, ly, sz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(lx, ly, sz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(lx, sy, sz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(sx, ly, sz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(sx, ly, lz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(lx, ly, lz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(lx, ly, sz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(sx, sy, lz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(sx, sy, sz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(lx, sy, sz);
      OpenTK.Graphics.OpenGL.GL.Vertex3(lx, sy, lz);
      OpenTK.Graphics.OpenGL.GL.End();
    }

    public static void DrawPriPick()
    {
      OpenTK.Graphics.OpenGL.GL.PushMatrix();
      Core.DrawPri();
      OpenTK.Graphics.OpenGL.GL.PopMatrix();
    }

    public static void DrawPri()
    {
      OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.TriangleFan);
      OpenTK.Graphics.OpenGL.GL.Vertex3(0.0f, 30f, 0.0f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(-25f, -25f, 25f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(25f, -25f, 25f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(25f, -25f, -25f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(-25f, -25f, -25f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(-25f, -25f, 25f);
      OpenTK.Graphics.OpenGL.GL.End();
      OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Quads);
      OpenTK.Graphics.OpenGL.GL.Vertex3(-25f, -25f, 25f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(-25f, -25f, -25f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(25f, -25f, -25f);
      OpenTK.Graphics.OpenGL.GL.Vertex3(25f, -25f, 25f);
      OpenTK.Graphics.OpenGL.GL.End();
    }

    public static void DrawPri(ushort radius, byte c)
    {
      OpenTK.Graphics.OpenGL.GL.PushMatrix();
      OpenTK.Graphics.OpenGL.GL.Color3(0.9f, 0.9f, 0.9f);
      Core.DrawPri();
      if (radius != (ushort) 0)
      {
        try
        {
          if ((!Core.drawEnemyRadius || c != (byte) 3) && (!Core.drawFlagRadius || c != (byte) 1))
          {
            if (Core.drawUnknownRadius)
            {
              if (c != (byte) 4)
                goto label_12;
            }
            else
              goto label_12;
          }
          if (c == (byte) 0)
            OpenTK.Graphics.OpenGL.GL.Color3(1f, 0.0f, 0.0f);
          if (c == (byte) 1)
            OpenTK.Graphics.OpenGL.GL.Color3(0.0f, 1f, 0.0f);
          if (c == (byte) 4)
            OpenTK.Graphics.OpenGL.GL.Color3(1f, 1f, 0.0f);
          Core.wire = OpenTK.Graphics.Glu.NewQuadric();
          OpenTK.Graphics.Glu.QuadricDrawStyle(Core.wire, QuadricDrawStyle.Line);
          OpenTK.Graphics.Glu.Sphere(Core.wire, (double) radius, 5, 5);
          OpenTK.Graphics.Glu.DeleteQuadric(Core.wire);
        }
        catch (Exception ex)
        {
          Core.logger.Log(NLog.LogLevel.Debug, ex.Message);
          Core.logger.Log(NLog.LogLevel.Error, "Could not draw triangle - Recommend enabling debug mode if not already");
        }
      }
label_12:
      OpenTK.Graphics.OpenGL.GL.PopMatrix();
    }

    private static bool check2D(ref ObjectData obj) => obj.objectID == (short) 32 || obj.objectID == (short) 33 || obj.objectID == (short) 34 && obj.specialScript == (short) 6412 || obj.objectID == (short) 35 || obj.objectID == (short) 39 || obj.objectID == (short) 45 || obj.objectID == (short) 81 || obj.objectID == (short) 82 || obj.objectID == (short) 5696 || obj.objectID == (short) 5712 || obj.objectID == (short) 5728 || obj.name == "Enemy Boundary" || obj.name.Contains("End Climb") || obj.name == "Start Climb" || obj.name == "Note" || obj.name.Contains("Start Point") || obj.name.Contains("Entry Point") || obj.name == "warp" || obj.name == "Warp To - Start of Level" || obj.objectID == (short) 389 || obj.objectID == (short) 395 || obj.objectID == (short) 414 || obj.objectID == (short) 297 || obj.objectID == (short) 224 || obj.objectID == (short) 511 || obj.objectID == (short) 512 || obj.objectID == (short) 880 || obj.objectID == (short) 73 || obj.specialScript == (short) 0 && (obj.objectID == (short) 3424 || obj.objectID == (short) 1125 || obj.objectID == (short) 1127 || obj.objectID == (short) 1136 || obj.objectID == (short) 1280 || obj.objectID == (short) 5696 || obj.objectID == (short) 5616 || obj.objectID == (short) 224 || obj.objectID == (short) 5712 || obj.objectID == (short) 5728);

    public static uint DrawObject(
      ObjectData obj,
      bool pick = false,
      bool createDL = true,
      float r = 0.0f,
      float g = 0.0f,
      float b = 0.0f)
    {
      if ((double) r == 0.0 && (double) g == 0.0 && (double) b == 0.0)
      {
        r = (float) obj.m_colorID[0] / (float) byte.MaxValue;
        g = (float) obj.m_colorID[1] / (float) byte.MaxValue;
        b = (float) obj.m_colorID[2] / (float) byte.MaxValue;
      }
      uint list = 0;
      if (createDL)
      {
        list = (uint) OpenTK.Graphics.OpenGL.GL.GenLists(1);
        OpenTK.Graphics.OpenGL.GL.NewList(list, OpenTK.Graphics.OpenGL.ListMode.Compile);
      }
      if (obj.Pointer != 0)
      {
        if (!Core.check2D(ref obj))
        {
          if (obj.Pointer != 1)
          {
            try
            {
              RomHandler.DecompressFileToHDD(obj.Pointer);
            }
            catch (Exception ex)
            {
              Core.logger.Log(NLog.LogLevel.Error, "Failed to decompress file");
              Core.logger.Log(NLog.LogLevel.Debug, ex.Message.ToString());
            }
            if (pick)
            {
              OpenTK.Graphics.OpenGL.GL.Color3(r, g, b);
              Core.DrawModelFilePicking(Core.dir + obj.Pointer.ToString("x"));
            }
            else
              obj.bb = Core.DrawModelFile(Core.dir + obj.Pointer.ToString("x"));
            if (obj.Pointer2 != 0)
            {
              try
              {
                RomHandler.DecompressFileToHDD(obj.Pointer);
              }
              catch (Exception ex)
              {
                Core.logger.Log(NLog.LogLevel.Error, "Failed to decompress file");
                Core.logger.Log(NLog.LogLevel.Debug, ex.Message.ToString());
              }
              if (pick)
              {
                OpenTK.Graphics.OpenGL.GL.Color3(r, g, b);
                Core.DrawModelFilePicking(Core.dir + obj.Pointer2.ToString("x"));
                goto label_32;
              }
              else
              {
                Core.DrawModelFile(Core.dir + obj.Pointer2.ToString("x"));
                goto label_32;
              }
            }
            else
              goto label_32;
          }
        }
        if (pick)
        {
          OpenTK.Graphics.OpenGL.GL.Color3(r, g, b);
          Core.DrawModelFilePicking(obj.modelFile);
        }
        else
          obj.bb = Core.DrawModelFile(obj.modelFile);
        if (obj.modelFile2 != "")
        {
          if (pick)
          {
            OpenTK.Graphics.OpenGL.GL.Color3(r, g, b);
            Core.DrawModelFilePicking(obj.modelFile2);
          }
          else
            Core.DrawModelFile(obj.modelFile2);
        }
      }
      else
      {
        obj.bb = new BoundingBox();
        obj.bb.SetSize(25);
        if (obj.name == "Camera Trigger")
        {
          if (pick)
            Core.DrawCamTrigger((ushort) 0, obj.m_colorID[0], obj.m_colorID[1], obj.m_colorID[2]);
          else
            Core.DrawCamTrigger(obj.radius, (byte) 0, (byte) 0, byte.MaxValue);
        }
        else if (pick)
        {
          OpenTK.Graphics.OpenGL.GL.Color3(r, g, b);
          Core.DrawPriPick();
        }
        else
          Core.DrawPri(obj.radius, obj.colour);
      }
label_32:
      OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
      if (createDL)
        OpenTK.Graphics.OpenGL.GL.EndList();
      OpenTK.Graphics.OpenGL.GL.Flush();
      return list;
    }

    public static BoundingBox DrawModelFile(int model)
    {
      try
      {
        RomHandler.DecompressFileToHDD(model);
        return Core.DrawModelFile(Core.dir + model.ToString("x"));
      }
      catch (Exception ex)
      {
        Core.logger.Log(NLog.LogLevel.Error, "Failed to decompress file");
        Core.logger.Log(NLog.LogLevel.Debug, ex.Message.ToString());
      }
      return new BoundingBox();
    }

    public static BoundingBox DrawModelFile(string model)
    {
      OpenTK.Graphics.OpenGL.GL.ShadeModel(OpenTK.Graphics.OpenGL.ShadingModel.Smooth);
      BoundingBox boundingBox = new BoundingBox();
      if (!File.Exists(model))
        return boundingBox;
      int F3DStart = 0;
      int F3DCommands = 0;
      int F3DEnd = 0;
      int vertStart = 0;
      List<byte[]> commands = new List<byte[]>();
      bool newTexture = false;
      int currentTexture = 0;
      float sScale = 0.0f;
      float tScale = 0.0f;
      F3DEXVert[] verts = new F3DEXVert[32];
      F3DEXVert[] cache = new F3DEXVert[32];
      F3DEXTexture[] textures = new F3DEXTexture[1];
      try
      {
        int collStart = 0;
        int VTCount = 0;
        int textureCount = 0;
        byte[] bytesInFile = File.ReadAllBytes(model);
        if (!F3DEX2.ReadModel(ref bytesInFile, ref collStart, ref F3DStart, ref F3DCommands, ref F3DEnd, ref vertStart, ref VTCount, ref textureCount, ref verts, ref commands, ref textures))
          return boundingBox;
        int index1 = vertStart - 24;
        int num1 = (int) (short) ((int) bytesInFile[index1] * 256 + (int) bytesInFile[index1 + 1]);
        int num2 = (int) (short) ((int) bytesInFile[index1 + 2] * 256 + (int) bytesInFile[index1 + 3]);
        int num3 = (int) (short) ((int) bytesInFile[index1 + 4] * 256 + (int) bytesInFile[index1 + 5]);
        int num4 = (int) (short) ((int) bytesInFile[index1 + 6] * 256 + (int) bytesInFile[index1 + 7]);
        int num5 = (int) (short) ((int) bytesInFile[index1 + 8] * 256 + (int) bytesInFile[index1 + 9]);
        int num6 = (int) (short) ((int) bytesInFile[index1 + 10] * 256 + (int) bytesInFile[index1 + 11]);
        boundingBox.smallX = num1;
        boundingBox.smallY = num2;
        boundingBox.smallZ = num3;
        boundingBox.largeX = num4;
        boundingBox.largeY = num5;
        boundingBox.largeZ = num6;
        OpenTK.Graphics.OpenGL.GL.PushMatrix();
        OpenTK.Graphics.OpenGL.GL.Scale(1f, 1f, 1f);
        for (int index2 = 0; index2 < F3DCommands; ++index2)
        {
          try
          {
            byte[] command = commands[index2];
            uint w1 = (uint) ((int) command[4] * 16777216 + (int) command[5] * 65536 + (int) command[6] * 256) + (uint) command[7];
            int num7 = (int) command[1];
            int num8 = (int) command[2];
            int num9 = (int) command[3];
            if ((int) command[0] == F3DEX2.F3DEX2_ENDDL)
            {
              F3DEX2.GL_EndDL(index2, commands.Count);
              newTexture = false;
            }
            if (command[0] == (byte) 253 && textures[0].textureWidth != 1)
              F3DEX2.GL_G_SETTIMG(ref currentTexture, textureCount, w1, ref textures, commands[index2 + 2], ref newTexture, sScale, tScale);
            if (command[0] == (byte) 252)
              F3DEX2.GL_G_Combine(w1);
            if (command[0] == (byte) 183)
              F3DEX2.GL_SETGEOMETRYMODE(w1);
            if (command[0] == (byte) 245 && textures[0].textureWidth != 1)
              F3DEX2.GL_G_SETTILE(command);
            if (command[0] == (byte) 240 && textures[0].textureWidth != 1)
            {
              int palSize = (int) ((w1 << 8 >> 8 & 16773120U) >> 14) * 2 + 2;
              textures[currentTexture].loadPalette(bytesInFile, textureCount, palSize);
              if (commands[index2 + 4][0] == (byte) 186)
                newTexture = true;
            }
            if ((int) command[0] == F3DEX2.F3DEX2_TEXTURE)
            {
              sScale = (float) (w1 >> 16) / 65536f;
              tScale = (float) (w1 & (uint) ushort.MaxValue) / 65536f;
            }
            if ((int) command[0] == F3DEX2.F3DEX2_VTX)
              F3DEX2.GL_VTX(ref bytesInFile, commands[index2], ref cache, ref verts, ref textures[currentTexture], textureCount, ref Core.texturesGL, ref newTexture);
            if ((int) command[0] == F3DEX2.F3DEX2_TRI1)
              F3DEX2.GL_TRI1(commands[index2], ref cache, ref textures[currentTexture]);
            if ((int) command[0] == F3DEX2.F3DEX2_TRI2)
              F3DEX2.GL_TRI2(commands[index2], ref cache, ref textures[currentTexture]);
          }
          catch (Exception ex)
          {
          }
        }
        OpenTK.Graphics.OpenGL.GL.PopMatrix();
        return boundingBox;
      }
      catch (Exception ex)
      {
        Core.logger.Log(NLog.LogLevel.Error, "Could not read model file " + model);
        Core.logger.Log(NLog.LogLevel.Debug, ex.Message);
      }
      return boundingBox;
    }

    public static void TranslateToGEOBJ(
      string outDir,
      string file,
      float[] vertexData,
      byte[] vertexColorData,
      float[] vertexTextureCoordData,
      List<TextureData> TextureDataList,
      List<Texture> TextureSettings,
      List<int> TextureSettingsBuffer,
      List<ushort[]> iboData)
    {
      try
      {
        string contents1 = "" + "# Exported with Wumba's Wigwam" + Environment.NewLine + "mtllib " + file + ".mtl" + Environment.NewLine + GEOBJ.convertVerts(vertexData, vertexColorData, vertexTextureCoordData);
        for (int index1 = 0; index1 < iboData.Count; ++index1)
        {
          if (TextureSettings[TextureSettingsBuffer[index1]].glname == -1)
            contents1 = contents1 + "usemtl none" + Environment.NewLine;
          else
            contents1 = contents1 + "usemtl " + (object) TextureDataList[TextureSettings[TextureSettingsBuffer[index1]].textureData].gl.GetHashCode() + Environment.NewLine;
          for (int index2 = 0; index2 < iboData[index1].Length; index2 += 3)
            contents1 += string.Format("f {0}/{0} {1}/{1} {2}/{2}" + Environment.NewLine, (object) ((int) iboData[index1][index2] + 1), (object) ((int) iboData[index1][index2 + 1] + 1), (object) ((int) iboData[index1][index2 + 2] + 1));
        }
        File.WriteAllText(outDir + file + ".obj", contents1);
        string contents2 = "# Exported with Wumba's Wigwam" + "newmtl none" + Environment.NewLine;
        foreach (TextureData textureData in TextureDataList)
        {
          contents2 = contents2 + "newmtl " + (object) textureData.gl.GetHashCode() + Environment.NewLine;
          if (textureData.cms == 2)
          	contents2 = contents2 + "clamp x" + Environment.NewLine;
          if (textureData.cmt == 2)
          	contents2 = contents2 + "clamp y" + Environment.NewLine;
          contents2 = contents2 + "map_Kd " + (object) textureData.gl.GetHashCode() + ".png" + Environment.NewLine;
          GEOBJ.writeTexture(outDir + (object) textureData.gl.GetHashCode() + ".png", textureData.gl, textureData.width, textureData.height);
        }
        File.WriteAllText(outDir + file + ".mtl", contents2);
      }
      catch (Exception ex)
      {
        Core.logger.Log(NLog.LogLevel.Error, "Could not read / save model to geOBJ - ");
        Core.logger.Log(NLog.LogLevel.Debug, ex.Message);
      }
    }
  }
}
