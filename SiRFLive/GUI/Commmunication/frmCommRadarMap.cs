﻿namespace SiRFLive.GUI.Commmunication
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.MessageHandling;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmCommRadarMap : Form
    {
        private static int _numberOpen;
        private string _persistedWindowName = "Radar View Window";
        private CommunicationManager comm;
        private IContainer components;
        private MyPanel myPanel;
        private MyPanel panel_SVs;
        private ToolTip toolTip1;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmCommRadarMap()
        {
            this.InitializeComponent();
            _numberOpen++;
            this._persistedWindowName = "Radar View Window " + _numberOpen.ToString();
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmCommRadarMap_Load(object sender, EventArgs e)
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

        private void frmCommRadarMap_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmCommRadarMap_Resize(object sender, EventArgs e)
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
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmCommRadarMap));
            this.toolTip1 = new ToolTip(this.components);
            this.panel_SVs = new MyPanel();
            base.SuspendLayout();
            this.panel_SVs.AutoSize = true;
            this.panel_SVs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel_SVs.Dock = DockStyle.Fill;
            this.panel_SVs.Location = new Point(0, 0);
            this.panel_SVs.Name = "panel_SVs";
            this.panel_SVs.Size = new Size(0x117, 0x11b);
            this.panel_SVs.TabIndex = 0;
            this.panel_SVs.Paint += new PaintEventHandler(this.panel_SVs_Paint);
            this.panel_SVs.Resize += new EventHandler(this.panel_SVs_Resize);
            this.panel_SVs.MouseEnter += new EventHandler(this.panel_SVs_MouseEnter);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x117, 0x11b);
            base.Controls.Add(this.panel_SVs);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmCommRadarMap";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Radar View";
            base.Load += new EventHandler(this.frmCommRadarMap_Load);
            base.Resize += new EventHandler(this.frmCommRadarMap_Resize);
            base.LocationChanged += new EventHandler(this.frmCommRadarMap_LocationChanged);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.comm != null)
            {
                this.comm.EnableSVsMap = false;
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

        private void panel_SVs_MouseEnter(object sender, EventArgs e)
        {
            string text = "1. The satellites turn RED when their fields of C/N0 are zero" + Environment.NewLine + "2. The satellites turn BLUE when" + Environment.NewLine + "   a) Their fields of C/N0 are not zero AND" + Environment.NewLine + "   b) They are not used in the navigation solution." + Environment.NewLine + "3. The satellites turn GREEN when" + Environment.NewLine + "   a) Their fields of C/N0 are not zero AND" + Environment.NewLine + "   b) They are used in the navigation solution." + Environment.NewLine + "4. The satellites turn SKYBLUE when they are SBAS satellites." + Environment.NewLine + "5. The satellites turn ORANGE when ABP is being used." + Environment.NewLine + "6. The satellites turn MAGENTA when Extended Ephemeris is being used.";
            this.toolTip1.Show(text, this.panel_SVs, 0x7530);
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
                graphics2.FillEllipse(Brushes.Black, (float) 0f, (float) 0f, (float) (width - 1f), (float) (height - 1f));
                graphics2.DrawLine(Pens.White, (float) 0f, (float) (height / 2f), (float) (width * 1f), (float) (height / 2f));
                graphics2.DrawLine(Pens.White, (float) (height / 2f), (float) 0f, (float) (height / 2f), (float) (height * 1f));
                graphics2.DrawEllipse(Pens.White, (float) 0f, (float) 0f, (float) (width - 1f), (float) (height - 1f));
                graphics2.DrawEllipse(Pens.White, (float) (width / 4f), (float) (height / 4f), (float) (width / 2f), (float) (height / 2f));
                graphics2.DrawEllipse(Pens.White, (float) (width / 8f), (float) (height / 8f), (float) ((width * 3f) / 4f), (float) ((height * 3f) / 4f));
                graphics2.DrawEllipse(Pens.White, (float) ((width * 3f) / 8f), (float) ((height * 3f) / 8f), (float) (width / 4f), (float) (height / 4f));
                graphics2.TranslateTransform(width / 2f, height / 2f);
                graphics2.DrawString("EI=90", new Font("Segoe UI", (float) num3), Brushes.White, (float) 0f, (float) 0f);
                graphics2.DrawString("AZ=0", new Font("Segoe UI", (float) num3), Brushes.White, (float) 0f, (float) (-height / 2f));
                for (int i = 0; i < DataForGUI.MAX_PRN; i++)
                {
                    if (this.comm.dataGui.PRN_Arr_ID[i] != 0)
                    {
                        if (i > 100)
                        {
                            num3 = 7;
                        }
                        else
                        {
                            num3 = 9;
                        }
                        float num7 = (float) (((((double) (90f - this.comm.dataGui.PRN_Arr_Elev[i])) / 90.0) * (((double) width) / 2.0)) * Math.Sin((this.comm.dataGui.PRN_Arr_Azimuth[i] * 3.1415926535897931) / 180.0));
                        float num8 = -((float) (((((double) (90f - this.comm.dataGui.PRN_Arr_Elev[i])) / 90.0) * (((double) height) / 2.0)) * Math.Cos((this.comm.dataGui.PRN_Arr_Azimuth[i] * 3.1415926535897931) / 180.0)));
                        float num9 = 10f;
                        if (Math.Abs(this.comm.dataGui.PRN_Arr_CNO[i]) <= 1E-06)
                        {
                            graphics2.FillEllipse(Brushes.Red, (float) (num7 - num9), (float) (num8 - num9), (float) (2f * num9), (float) (2f * num9));
                        }
                        else if ((this.comm.dataGui.PRN_Arr_PRNforSolution[i] != 0) && (this.comm.dataGui._PMODE > 0))
                        {
                            if (this.comm.dataGui.PRN_Arr_UseCGEE[i] || this.comm.dataGui.PRN_Arr_UseSGEE[i])
                            {
                                graphics2.FillEllipse(Brushes.Purple, (float) (num7 - num9), (float) (num8 - num9), (float) (2f * num9), (float) (2f * num9));
                            }
                            else if (this.comm.ABPModeIndicator)
                            {
                                graphics2.FillEllipse(Brushes.DarkOrange, (float) (num7 - num9), (float) (num8 - num9), (float) (2f * num9), (float) (2f * num9));
                            }
                            else
                            {
                                graphics2.FillEllipse(Brushes.Green, (float) (num7 - num9), (float) (num8 - num9), (float) (2f * num9), (float) (2f * num9));
                            }
                        }
                        else if (this.comm.dataGui.Positions.PositionList.Count > 0)
                        {
                            byte num10 = (byte) (((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).NavType & 0x80);
                            if (((i > 100) && (i < DataForGUI.MAX_PRN)) && ((num10 == 0x80) && (this.comm.dataGui.PRN_Arr_Elev[i] > 0f)))
                            {
                                graphics2.FillEllipse(Brushes.DeepSkyBlue, (float) (num7 - num9), (float) (num8 - num9), (float) (2f * num9), (float) (2f * num9));
                            }
                            else
                            {
                                graphics2.FillEllipse(Brushes.Blue, (float) (num7 - num9), (float) (num8 - num9), (float) (2f * num9), (float) (2f * num9));
                            }
                        }
                        else
                        {
                            graphics2.FillEllipse(Brushes.Blue, (float) (num7 - num9), (float) (num8 - num9), (float) (2f * num9), (float) (2f * num9));
                        }
                        graphics2.DrawString(string.Format("{0:00}", this.comm.dataGui.PRN_Arr_ID[i]), new Font("Segoe UI", (float) num3), Brushes.White, (float) (num7 - 9f), (float) (num8 - 7f));
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
                this.comm.DisplayPanelSVs = this.panel_SVs;
                this.myPanel = this.panel_SVs;
                this.comm.EnableSVsMap = true;
                this.Text = this.comm.sourceDeviceName + ": Signal View";
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

