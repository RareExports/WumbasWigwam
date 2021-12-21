// Decompiled with JetBrains decompiler
// Type: GradientPanel
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class GradientPanel : Panel
{
  public Color color1 = Color.Black;
  public Color color2 = Color.White;

  public GradientPanel() => this.ResizeRedraw = true;

  protected override void OnPaintBackground(PaintEventArgs e)
  {
    using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, this.color1, this.color2, LinearGradientMode.ForwardDiagonal))
      e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
  }

  public Color GetPixel(int x, int y)
  {
    Rectangle clientRectangle = this.ClientRectangle;
    int width = clientRectangle.Width;
    clientRectangle = this.ClientRectangle;
    int height = clientRectangle.Height;
    using (Bitmap bitmap = new Bitmap(width, height))
    {
      using (Graphics graphics = Graphics.FromImage((Image) bitmap))
      {
        using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Rectangle(0, 0, bitmap.Width, bitmap.Height), this.color1, this.color2, LinearGradientMode.Vertical))
        {
          graphics.FillRectangle((Brush) linearGradientBrush, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
          return bitmap.GetPixel(x, y);
        }
      }
    }
  }

  protected override void OnScroll(ScrollEventArgs se)
  {
    this.Invalidate();
    base.OnScroll(se);
  }
}
