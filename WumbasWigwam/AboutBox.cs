// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.AboutBox
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WumbasWigwam
{
  public class AboutBox : Form
  {
    private IContainer components;
    private Label label1;
    private Label label3;
    private Label label5;
    private Label label7;
    private LinkLabel linkLabel1;
    private Label label8;
    private Label label9;
    private Label label10;

    public AboutBox()
    {
      this.InitializeComponent();
      this.linkLabel1.Links.Add(0, 40, (object) "http://www.banjosbackpack.com/");
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(e.Link.LinkData.ToString());

    private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(e.Link.LinkData.ToString());

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.label3 = new Label();
      this.label5 = new Label();
      this.label7 = new Label();
      this.linkLabel1 = new LinkLabel();
      this.label8 = new Label();
      this.label9 = new Label();
      this.label10 = new Label();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = Color.Black;
      this.label1.Location = new Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(147, 17);
      this.label1.TabIndex = 0;
      this.label1.Text = "Development Team";
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.ForeColor = Color.Black;
      this.label3.Location = new Point(13, 36);
      this.label3.Name = "label3";
      this.label3.Size = new Size(223, 15);
      this.label3.TabIndex = 2;
      this.label3.Text = "Skill - Software Developer / File Analysis";
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label5.ForeColor = Color.Black;
      this.label5.Location = new Point(12, 61);
      this.label5.Name = "label5";
      this.label5.Size = new Size(231, 15);
      this.label5.TabIndex = 4;
      this.label5.Text = "koolboyman - Rom Research / Rom Map";
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.ForeColor = Color.Black;
      this.label7.Location = new Point(12, 86);
      this.label7.Name = "label7";
      this.label7.Size = new Size(390, 15);
      this.label7.TabIndex = 6;
      this.label7.Text = "Subdrag - Compression/Descompression / Midi Tool - Encrypt/ Decrypt";
      this.linkLabel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.BackColor = Color.Transparent;
      this.linkLabel1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.linkLabel1.ForeColor = Color.Black;
      this.linkLabel1.LinkColor = Color.Black;
      this.linkLabel1.Location = new Point(355, 235);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new Size(176, 17);
      this.linkLabel1.TabIndex = 7;
      this.linkLabel1.TabStop = true;
      this.linkLabel1.Text = "http://banjosbackpack.com";
      this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
      this.label8.AutoSize = true;
      this.label8.BackColor = Color.Transparent;
      this.label8.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label8.ForeColor = Color.Black;
      this.label8.Location = new Point(12, 179);
      this.label8.Name = "label8";
      this.label8.Size = new Size(56, 17);
      this.label8.TabIndex = 8;
      this.label8.Text = "Testers";
      this.label9.AutoSize = true;
      this.label9.BackColor = Color.Transparent;
      this.label9.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label9.ForeColor = Color.Black;
      this.label9.Location = new Point(12, 205);
      this.label9.Name = "label9";
      this.label9.Size = new Size(265, 15);
      this.label9.TabIndex = 9;
      this.label9.Text = "Comet (wwwarea), Tee-Hee, Pokekid, jombo23";
      this.label10.AutoSize = true;
      this.label10.BackColor = Color.Transparent;
      this.label10.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label10.ForeColor = Color.Black;
      this.label10.Location = new Point(12, 235);
      this.label10.Name = "label10";
      this.label10.Size = new Size(243, 15);
      this.label10.TabIndex = 10;
      this.label10.Text = "Special Thanks to Tee-Hee for the 3D icons";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackgroundImageLayout = ImageLayout.Stretch;
      this.ClientSize = new Size(543, 256);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.linkLabel1);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label1);
      this.DoubleBuffered = true;
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AboutBox);
      this.ShowIcon = false;
      this.Text = "About";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
