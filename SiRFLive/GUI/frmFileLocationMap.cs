﻿namespace SiRFLive.GUI
{
    using SiRFLive.Properties;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class frmFileLocationMap : Form
    {
        private double[] _lat;
        private int _length;
        private double[] _lon;
        private double _Radius;
        internal frmMapIt _SiRFMap;
        private IContainer components;
        private MyPanel panel_Location;
        private ToolStripLabel radiusLabel;
        private ToolStripTextBox radiusTextBox;
        private ToolStripButton setRadiusBtn;
        private ToolStripButton showMapBtn;
        private ToolStrip toolStrip1;

        public frmFileLocationMap()
        {
            this._Radius = 5.0;
            this.InitializeComponent();
        }

        public frmFileLocationMap(int length, double[] lat, double[] lon)
        {
            this._Radius = 5.0;
            this._length = length;
            this._lat = new double[length];
            this._lon = new double[length];
            for (int i = 0; i < length; i++)
            {
                this._lat[i] = lat[i];
                this._lon[i] = lon[i];
            }
            this.InitializeComponent();
        }

        internal frmMapIt CreateGoogleMapWindow()
        {
            if (!base.IsDisposed)
            {
                string str = "Google Map";
                if ((this._SiRFMap == null) || this._SiRFMap.IsDisposed)
                {
                    this._SiRFMap = new frmMapIt(this._length, this._lat, this._lon);
                    this._SiRFMap.MdiParent = base.MdiParent;
                    this._SiRFMap.Show();
                }
                this._SiRFMap.Text = str;
                this._SiRFMap.BringToFront();
            }
            return this._SiRFMap;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmFileLocationMap_Load(object sender, EventArgs e)
        {
        }

        private void frmFileLocationMap_Resize(object sender, EventArgs e)
        {
            this.panel_Location.Invalidate();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmFileLocationMap));
            this.toolStrip1 = new ToolStrip();
            this.showMapBtn = new ToolStripButton();
            this.radiusLabel = new ToolStripLabel();
            this.radiusTextBox = new ToolStripTextBox();
            this.setRadiusBtn = new ToolStripButton();
            this.panel_Location = new MyPanel();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.showMapBtn, this.setRadiusBtn, this.radiusTextBox, this.radiusLabel });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x1cd, 0x19);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            this.showMapBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.showMapBtn.Image = Resources.mapIt;
            this.showMapBtn.ImageTransparentColor = Color.Magenta;
            this.showMapBtn.Name = "showMapBtn";
            this.showMapBtn.Size = new Size(0x17, 0x16);
            this.showMapBtn.Text = "Show Map";
            this.showMapBtn.Click += new EventHandler(this.showMapBtn_Click);
            this.radiusLabel.Name = "radiusLabel";
            this.radiusLabel.Size = new Size(0x17, 0x16);
            this.radiusLabel.Text = "(m)";
            this.radiusTextBox.Name = "radiusTextBox";
            this.radiusTextBox.Size = new Size(100, 0x19);
            this.radiusTextBox.Text = "5.0";
            this.radiusTextBox.ToolTipText = "Radius in Meter";
            this.setRadiusBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.setRadiusBtn.Image = Resources.Config;
            this.setRadiusBtn.ImageTransparentColor = Color.Magenta;
            this.setRadiusBtn.Name = "setRadiusBtn";
            this.setRadiusBtn.Size = new Size(0x17, 0x16);
            this.setRadiusBtn.Text = "Set Radius";
            this.setRadiusBtn.Click += new EventHandler(this.setRadiusBtn_Click);
            this.panel_Location.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.panel_Location.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel_Location.Location = new Point(12, 0x29);
            this.panel_Location.Name = "panel_Location";
            this.panel_Location.Size = new Size(0x1b5, 0x138);
            this.panel_Location.TabIndex = 8;
            this.panel_Location.Paint += new PaintEventHandler(this.panel_Location_Paint);
            base.ClientSize = new Size(0x1cd, 0x16d);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.panel_Location);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmFileLocationMap";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Horizontal Trajectory";
            base.Load += new EventHandler(this.frmFileLocationMap_Load);
            base.Resize += new EventHandler(this.frmFileLocationMap_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void panel_Location_Paint(object sender, PaintEventArgs e)
        {
            int height = this.panel_Location.Height;
            int width = this.panel_Location.Width;
            int y = height;
            int x = width;
            if (width > height)
            {
                width = height;
            }
            else
            {
                height = width;
            }
            using (BufferedGraphics graphics = BufferedGraphicsManager.Current.Allocate(e.Graphics, e.ClipRectangle))
            {
                Graphics graphics2 = graphics.Graphics;
                graphics2.SmoothingMode = SmoothingMode.HighQuality;
                graphics2.PixelOffsetMode = PixelOffsetMode.HighQuality;
                LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(x, y), Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xff, 230));
                new Pen(brush);
                graphics2.FillRectangle(brush, 0, 0, x, y);
                graphics2.DrawLine(Pens.Gray, 0, height / 2, width, height / 2);
                graphics2.DrawLine(Pens.Gray, width / 2, 0, width / 2, height);
                graphics2.DrawEllipse(Pens.Gray, 0, 0, width, height);
                graphics2.DrawEllipse(Pens.Gray, (int) (width / 4), (int) (height / 4), (int) (width / 2), (int) (height / 2));
                if (this._length > 0)
                {
                    double d = 0.0;
                    double num6 = 0.0;
                    double num7 = this._Radius;
                    double num8 = 0.0;
                    double num9 = 0.0;
                    d = (this._lat[this._length - 1] * 3.1415926535897931) / 180.0;
                    num6 = (this._lon[this._length - 1] * 3.1415926535897931) / 180.0;
                    StringFormat format = new StringFormat();
                    format.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                    graphics2.DrawString(string.Format("R = {0:F2}m", num7), new Font("Segoe UI", 9f), Brushes.Gray, width * 1f, ((float) height) / 2f, format);
                    for (int i = 0; i < this._length; i++)
                    {
                        try
                        {
                            num8 = (this._lat[i] * 3.1415926535897931) / 180.0;
                            num9 = (this._lon[i] * 3.1415926535897931) / 180.0;
                            float num11 = ((float) (((((6378137.0 * Math.Acos(((Math.Cos(num6) * Math.Cos(num6)) * Math.Cos(num9 - d)) + (Math.Sin(num6) * Math.Sin(num6)))) * Math.Sign((double) (num9 - d))) * 0.5) * width) / num7)) - 1f;
                            float num12 = ((float) (((((6378137.0 * Math.Acos((Math.Cos(num8) * Math.Cos(num6)) + (Math.Sin(num8) * Math.Sin(num6)))) * Math.Sign((double) (num8 - num6))) * 0.5) * height) / num7)) - 1f;
                            graphics2.FillEllipse(Brushes.Black, (float) ((width / 2) + num11), (float) ((height / 2) - num12), (float) 2f, (float) 2f);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    for (int j = 0; j < this._length; j++)
                    {
                        try
                        {
                            num8 = (this._lat[j] * 3.1415926535897931) / 180.0;
                            num9 = (this._lon[j] * 3.1415926535897931) / 180.0;
                            float num14 = ((float) (((((6378137.0 * Math.Acos(((Math.Cos(d) * Math.Cos(d)) * Math.Cos(num9 - num6)) + (Math.Sin(d) * Math.Sin(d)))) * Math.Sign((double) (num9 - num6))) * 0.5) * width) / num7)) - 1f;
                            float num15 = ((float) (((((6378137.0 * Math.Acos((Math.Cos(num8) * Math.Cos(d)) + (Math.Sin(num8) * Math.Sin(d)))) * Math.Sign((double) (num8 - d))) * 0.5) * height) / num7)) - 1f;
                            if (j == (this._length - 1))
                            {
                                graphics2.FillEllipse(Brushes.Red, (float) ((width / 2) + num14), (float) ((height / 2) - num15), (float) 2f, (float) 2f);
                            }
                            else
                            {
                                graphics2.FillEllipse(Brushes.Blue, (float) ((width / 2) + num14), (float) ((height / 2) - num15), (float) 2f, (float) 2f);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    graphics.Render(e.Graphics);
                }
            }
        }

        private void setRadiusBtn_Click(object sender, EventArgs e)
        {
            double num = 0.0;
            bool flag = false;
            try
            {
                num = Convert.ToDouble(this.radiusTextBox.Text);
            }
            catch
            {
                flag = true;
            }
            if (num < 5.0)
            {
                flag = true;
                num = 5.0;
            }
            if (flag)
            {
                MessageBox.Show("Invalid input: minimum radius is 5m", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            this._Radius = num;
            this.panel_Location.Invalidate();
        }

        private void showMapBtn_Click(object sender, EventArgs e)
        {
            if (this._length >= 1)
            {
                this.CreateGoogleMapWindow();
            }
        }
    }
}

