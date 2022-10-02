// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.GEOBJ
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace WumbasWigwam
{
    internal class GEOBJ
    {

        public static string convertVerts(
          float[] vertexData,
          byte[] vertexColorData,
          float[] vertexTextureCoordData)
        {
            string str = "";
            int index1 = 0;
            int index2 = 0;
            while (index1 < vertexData.Length)
            {
                str = str + string.Format("v {0} {1} {2} {3} {4} {5} {6}" + Environment.NewLine, (object)vertexData[index1], (object)vertexData[index1 + 1], (object)vertexData[index1 + 2], (object)((float)vertexColorData[index2] / 255), (object)((float)vertexColorData[index2 + 1] / 255), (object)((float)vertexColorData[index2 + 2] / 255), (object)((float)vertexColorData[index2 + 3] / 255));
                index1 += 3;
                index2 += 4;
            }
            //for (int index3 = 0; index3 < vertexTextureCoordData.Length; index3 += 2)
            //  str += string.Format("vt {0} {1}" + Environment.NewLine, (object) vertexTextureCoordData[index3], (object) (float) (((double) vertexTextureCoordData[index3 + 1] * -1.0) + 1.0));
            return str;
        }
        public static string convertFace(uint v1, uint v2, uint v3, int vtIndex)
        {
            return $"f {v1}/{vtIndex} {v2}/{vtIndex + 1} {v3}/{vtIndex + 2} {Environment.NewLine}";
        }

        public static void writeTexture(string file, byte[] pixels, int width, int height)
        {
            byte[] source = new byte[pixels.Length];
            for (int index = 0; index < pixels.Length; index += 4)
            {
                source[index] = pixels[index + 2];
                source[index + 1] = pixels[index + 1];
                source[index + 2] = pixels[index];
                source[index + 3] = pixels[index + 3];
            }
            Rectangle rect = new Rectangle(0, 0, width, height);
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            IntPtr scan0 = bitmapdata.Scan0;
            Marshal.Copy(source, 0, scan0, source.Length);
            bitmap.UnlockBits(bitmapdata);
            bitmap.Save(file, ImageFormat.Png);
        }

        public static string convertSkeleton(List<Bone> skeleton)
        {
            string str = "";
            foreach (Bone bone in skeleton)
            {
                str = str + "#joint " + (object)bone.id + Environment.NewLine;
                str = str + "#jointposition " + bone.x.ToString() + " " + bone.y.ToString() + " " + bone.z.ToString() + Environment.NewLine;
            }
            foreach (Bone bone in skeleton)
            {
                if (bone.parent != (short)-1)
                    str = str + "#connection " + (object)bone.parent + " " + (object)bone.id + Environment.NewLine;
            }
            return str;
        }
    }
}
