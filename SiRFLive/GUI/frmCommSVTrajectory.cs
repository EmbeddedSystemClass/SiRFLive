﻿namespace SiRFLive.GUI
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmCommSVTrajectory : Form
    {
        private float[,] _azi;
        private float[,] _ele;
        private int[] _idx_P;
        private static int _numberOpen = 0;
        private string _persistedWindowName;
        private int[] _svids;
        private CommunicationManager comm;
        private IContainer components;
        private bool IsRealTime;
        private MyPanel myPanel;
        private static int NUM_MAXP = DataForPlotting.MAX_P;
        private static int NUM_MAXSV = TrackSVRec.MAX_SVT;
        private MyPanel panel_SVs;
        private Brush[] svBrush;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmCommSVTrajectory()
        {
            this._persistedWindowName = "SV Trajectory Window";
            this.svBrush = new Brush[] { Brushes.Red, Brushes.Purple, Brushes.Brown, Brushes.BlueViolet, Brushes.DarkMagenta, Brushes.ForestGreen, Brushes.Goldenrod, Brushes.Green, Brushes.Brown, Brushes.Indigo, Brushes.Crimson, Brushes.Maroon, Brushes.DeepPink, Brushes.DarkMagenta, Brushes.DarkRed };
            this.IsRealTime = true;
            this.InitializeComponent();
            _numberOpen++;
            this._persistedWindowName = "SV Trajectory Window " + _numberOpen.ToString();
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
        }

        public frmCommSVTrajectory(int[] idx_P, int[] svids, float[,] ele, float[,] azi)
        {
            this._persistedWindowName = "SV Trajectory Window";
            this.svBrush = new Brush[] { Brushes.Red, Brushes.Purple, Brushes.Brown, Brushes.BlueViolet, Brushes.DarkMagenta, Brushes.ForestGreen, Brushes.Goldenrod, Brushes.Green, Brushes.Brown, Brushes.Indigo, Brushes.Crimson, Brushes.Maroon, Brushes.DeepPink, Brushes.DarkMagenta, Brushes.DarkRed };
            this.IsRealTime = false;
            this._svids = new int[NUM_MAXSV];
            this._idx_P = new int[NUM_MAXSV];
            this._ele = new float[NUM_MAXSV, NUM_MAXP];
            this._azi = new float[NUM_MAXSV, NUM_MAXP];
            for (int i = 0; i < NUM_MAXSV; i++)
            {
                this._svids[i] = svids[i];
                this._idx_P[i] = idx_P[i];
            }
            for (int j = 0; j < NUM_MAXSV; j++)
            {
                for (int k = 0; k < NUM_MAXP; k++)
                {
                    this._ele[j, k] = ele[j, k];
                    this._azi[j, k] = azi[j, k];
                }
            }
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmCommSVTrajectory_Load(object sender, EventArgs e)
        {
            if (this.IsRealTime)
            {
                base.Top = this.WinTop;
                base.Left = this.WinLeft;
                if (this.WinWidth != 0)
                {
                    base.Width = this.WinWidth;
                }
                if (this.WinHeight != 0)
                {
                    base.Height = this.WinHeight;
                }
            }
        }

        private void frmCommSVTrajectory_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmCommSVTrajectory_ResizeEnd(object sender, EventArgs e)
        {
            this.WinWidth = base.Width;
            this.WinHeight = base.Height;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmCommSVTrajectory));
            this.panel_SVs = new MyPanel();
            base.SuspendLayout();
            this.panel_SVs.AutoSize = true;
            this.panel_SVs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel_SVs.Dock = DockStyle.Fill;
            this.panel_SVs.Location = new Point(0, 0);
            this.panel_SVs.Name = "panel_SVs";
            this.panel_SVs.Size = new Size(0x2c8, 0x2c8);
            this.panel_SVs.TabIndex = 1;
            this.panel_SVs.Paint += new PaintEventHandler(this.panel_SVs_Paint);
            this.panel_SVs.Resize += new EventHandler(this.panel_SVs_Resize);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x2c8, 0x2c8);
            base.Controls.Add(this.panel_SVs);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmCommSVTrajectory";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Satellite Trajectory";
            base.Load += new EventHandler(this.frmCommSVTrajectory_Load);
            base.LocationChanged += new EventHandler(this.frmCommSVTrajectory_LocationChanged);
            base.ResizeEnd += new EventHandler(this.frmCommSVTrajectory_ResizeEnd);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.IsRealTime)
            {
                if (this.updateMainWindow != null)
                {
                    this.updateMainWindow(base.Name);
                }
                if (this.UpdatePortManager != null)
                {
                    this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, false);
                }
            }
            base.OnClosed(e);
        }

        private void panel_SVs_Paint(object sender, PaintEventArgs e)
        {
            float height = this.panel_SVs.Height;
            float width = this.panel_SVs.Width;
            int num3 = 9;
            float num4 = height;
            float num5 = width;
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
                graphics2.SmoothingMode = SmoothingMode.AntiAlias;
                graphics2.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point((int) num5, (int) num4), Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xf5, 0xea));
                new Pen(brush);
                graphics2.FillRectangle(brush, 0f, 0f, num5, num4);
                graphics2.FillEllipse(Brushes.White, (float) 0f, (float) 0f, (float) (width - 1f), (float) (height - 1f));
                graphics2.DrawLine(Pens.Black, (float) 0f, (float) (height / 2f), (float) (width * 1f), (float) (height / 2f));
                graphics2.DrawLine(Pens.Black, (float) (width / 2f), (float) 0f, (float) (width / 2f), (float) (height * 1f));
                graphics2.DrawLine(Pens.Black, (float) ((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828))), (float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828))), (float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828))), (float) ((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828))));
                graphics2.DrawLine(Pens.Black, (float) ((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828))), (float) ((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828))), (float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828))), (float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828))));
                graphics2.DrawString("N", new Font("Segoe UI", (float) num3), Brushes.Black, (float) (width / 2f), (float) 0f);
                graphics2.DrawString("S", new Font("Segoe UI", (float) num3), Brushes.Black, (float) (width / 2f), (float) ((height * 1f) - 15f));
                graphics2.DrawString("E", new Font("Segoe UI", (float) num3), Brushes.Black, (float) ((width * 1f) - 12f), (float) (height / 2f));
                graphics2.DrawString("W", new Font("Segoe UI", (float) num3), Brushes.Black, (float) 0f, (float) (height / 2f));
                graphics2.DrawString("45", new Font("Segoe UI", (float) num3), Brushes.Black, (float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828))), ((float) ((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828)))) - 10f);
                graphics2.DrawString("135", new Font("Segoe UI", (float) num3), Brushes.Black, (float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828))), (float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828))));
                graphics2.DrawString("225", new Font("Segoe UI", (float) num3), Brushes.Black, (float) (((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828))) - 15.0), ((float) ((((double) width) / 2.0) * (1.0 + Math.Sin(0.78539816339744828)))) + 5f);
                graphics2.DrawString("315", new Font("Segoe UI", (float) num3), Brushes.Black, (float) (((float) ((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828)))) - 15f), (float) (((float) ((((double) width) / 2.0) * (1.0 - Math.Sin(0.78539816339744828)))) - 20f));
                graphics2.DrawEllipse(Pens.Black, (float) 0f, (float) 0f, (float) (width - 1f), (float) (height - 1f));
                for (int i = 1; i < 10; i++)
                {
                    graphics2.DrawEllipse(Pens.Black, (float) ((width * i) / 18f), (float) ((height * i) / 18f), (float) ((width * (9 - i)) / 9f), (float) ((height * (9 - i)) / 9f));
                    if (i != 1)
                    {
                        graphics2.DrawString(string.Format("{0}", (i - 1) * 10), new Font("Segoe UI", (float) num3), Brushes.Black, (width * (i - 1)) / 18f, (float) ((((double) height) / 2.0) - 2.0));
                        graphics2.DrawString(string.Format("{0}", (i - 1) * 10), new Font("Segoe UI", (float) num3), Brushes.Black, width - ((width * (i - 1)) / 18f), (float) ((((double) height) / 2.0) - 2.0));
                    }
                }
                graphics2.TranslateTransform(width / 2f, height / 2f);
                graphics2.DrawString("EI=90", new Font("Segoe UI", (float) num3), Brushes.White, (float) 0f, (float) 0f);
                graphics2.DrawString("AZ=0", new Font("Segoe UI", (float) num3), Brushes.White, (float) 0f, (float) (-height / 2f));
                num3 = 9;
                float num7 = 0f;
                float y = 0f;
                float num9 = 10f;
                int index = 0;
                int num11 = 0;
                if (this.IsRealTime)
                {
                    if (this.comm.dataPlot != null)
                    {
                        int num12 = this.comm.dataPlot.SVTrkr.SVIDs[num11];
                        while ((num12 > 0) && (num11 < TrackSVRec.MAX_SVT))
                        {
                            for (int j = 0; j < this.comm.dataPlot.idx_P[num11]; j++)
                            {
                                if (this.comm.dataGui.PRN_Arr_ID[num12] != 0)
                                {
                                    num7 = (float) (((((double) (90f - this.comm.dataPlot.elevation[num11, j])) / 90.0) * (((double) width) / 2.0)) * Math.Sin((this.comm.dataPlot.azimuth[num11, j] * 3.1415926535897931) / 180.0));
                                    y = -((float) (((((double) (90f - this.comm.dataPlot.elevation[num11, j])) / 90.0) * (((double) height) / 2.0)) * Math.Cos((this.comm.dataPlot.azimuth[num11, j] * 3.1415926535897931) / 180.0)));
                                    num9 = 2f;
                                    graphics2.FillEllipse(this.svBrush[index], (float) (num7 - num9), (float) (y - num9), (float) (2f * num9), (float) (2f * num9));
                                }
                            }
                            if (this.comm.dataGui.PRN_Arr_ID[num12] != 0)
                            {
                                graphics2.DrawString(string.Format("{0}", num12), new Font("Segoe UI", (float) num3, FontStyle.Bold), this.svBrush[index], num7 + 10f, y);
                                index++;
                                if (index >= 15)
                                {
                                    index = 0;
                                }
                            }
                            num11++;
                            if (num11 < TrackSVRec.MAX_SVT)
                            {
                                num12 = this.comm.dataPlot.SVTrkr.SVIDs[num11];
                            }
                        }
                    }
                }
                else
                {
                    int num14 = this._svids[num11];
                    while ((num14 > 0) && (num11 < NUM_MAXSV))
                    {
                        for (int k = 0; k < this._idx_P[num11]; k++)
                        {
                            if (num14 != 0)
                            {
                                num7 = (float) (((((double) (90f - this._ele[num11, k])) / 90.0) * (((double) width) / 2.0)) * Math.Sin((this._azi[num11, k] * 3.1415926535897931) / 180.0));
                                y = -((float) (((((double) (90f - this._ele[num11, k])) / 90.0) * (((double) height) / 2.0)) * Math.Cos((this._azi[num11, k] * 3.1415926535897931) / 180.0)));
                                num9 = 2f;
                                graphics2.FillEllipse(this.svBrush[index], (float) (num7 - num9), (float) (y - num9), (float) (2f * num9), (float) (2f * num9));
                            }
                        }
                        if (num14 != 0)
                        {
                            graphics2.DrawString(string.Format("{0}", num14), new Font("Segoe UI", (float) num3, FontStyle.Bold), this.svBrush[index], num7 + 10f, y);
                            index++;
                            if (index >= 15)
                            {
                                index = 0;
                            }
                        }
                        num11++;
                        if (num11 < TrackSVRec.MAX_SVT)
                        {
                            num14 = this._svids[num11];
                        }
                    }
                }
                graphics.Render(e.Graphics);
            }
        }

        private void panel_SVs_Resize(object sender, EventArgs e)
        {
            this.Refresh();
            this.panel_SVs.Refresh();
        }

        public CommunicationManager CommWindow
        {
            get
            {
                return this.comm;
            }
            set
            {
                this.comm = value;
                this.comm.DisplayPanelSVTraj = this.panel_SVs;
                this.myPanel = this.panel_SVs;
                this.Text = this.comm.sourceDeviceName + ": SV Trajectory ";
            }
        }

        public string PersistedWindowName
        {
            get
            {
                return this._persistedWindowName;
            }
            set
            {
                this._persistedWindowName = value;
            }
        }

        public delegate void updateParentEventHandler(string titleString);

        public delegate void UpdateWindowEventHandler(string titleString, int left, int top, int width, int height, bool state);
    }
}

