﻿namespace SiRFLive.GUI
{
    using SiRFLive.Analysis;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using ZedGraph;

    public class frmCommNavAccVsTime : Form
    {
        private double[] _alt;
        private double[] _lat;
        private int _length;
        private double[] _lon;
        private RectangleF _rect;
        private RectangleF _rect2;
        private double _refAlt;
        private double _refLat;
        private double _refLon;
        private double[] _tows;
        private double[] _tows_Back;
        private CommunicationManager comm;
        private CommunicationManager comm_forFile;
        private IContainer components;
        private bool IsRealTime;
        private ToolStripComboBox navAccuracyComboBox;
        private ToolStripButton navAccuracyHelpBtn;
        private ToolStripButton navAccuracyRefreshBtn;
        private ToolStripButton navAccuracySetRefLocBtn;
        private ToolStrip toolStrip1;
        private ToolTip toolTip1;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;
        private ZedGraphControl zedGraphControl1;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmCommNavAccVsTime()
        {
            this._refLat = 37.375062;
            this._refLon = -121.914245;
            this._refAlt = -13.8;
            this.IsRealTime = true;
            this.InitializeComponent();
        }

        public frmCommNavAccVsTime(CommunicationManager mainComWin)
        {
            this._refLat = 37.375062;
            this._refLon = -121.914245;
            this._refAlt = -13.8;
            this.IsRealTime = true;
            this.InitializeComponent();
            this.CommWindow = mainComWin;
        }

        public frmCommNavAccVsTime(int length, double[] lat, double[] lon, double[] alt, double[] tows)
        {
            this._refLat = 37.375062;
            this._refLon = -121.914245;
            this._refAlt = -13.8;
            this.comm_forFile = new CommunicationManager("");
            this.IsRealTime = false;
            this._length = length;
            this._lat = new double[length];
            this._lon = new double[length];
            this._alt = new double[length];
            this._tows = new double[length];
            this._tows_Back = new double[length];
            for (int i = 0; i < length; i++)
            {
                this._lat[i] = lat[i];
                this._lon[i] = lon[i];
                this._alt[i] = alt[i];
                this._tows[i] = tows[i];
                this._tows_Back[i] = tows[i];
            }
            this.comm_forFile.RxCtrl.RxNavData.RefLat = this._refLat;
            this.comm_forFile.RxCtrl.RxNavData.RefLon = this._refLon;
            this.comm_forFile.RxCtrl.RxNavData.RefAlt = this._refAlt;
            this.InitializeComponent();
        }

        public frmCommNavAccVsTime(int length, double[] lat, double[] lon, double[] alt, double[] tows, double reflat, double reflon, double refalt)
        {
            this._refLat = 37.375062;
            this._refLon = -121.914245;
            this._refAlt = -13.8;
            this.comm_forFile = new CommunicationManager("");
            this.IsRealTime = false;
            this._length = length;
            this._lat = new double[length];
            this._lon = new double[length];
            this._alt = new double[length];
            this._tows = new double[length];
            this._tows_Back = new double[length];
            for (int i = 0; i < length; i++)
            {
                this._lat[i] = lat[i];
                this._lon[i] = lon[i];
                this._alt[i] = alt[i];
                this._tows[i] = tows[i];
                this._tows_Back[i] = tows[i];
            }
            this.comm_forFile.RxCtrl.RxNavData.RefLat = refalt;
            this.comm_forFile.RxCtrl.RxNavData.RefLon = reflon;
            this.comm_forFile.RxCtrl.RxNavData.RefAlt = refalt;
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

        private void frmCommNavAccVsTime_Load(object sender, EventArgs e)
        {
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
            this.navAccuracyComboBox.Items.AddRange(new object[] { "Plot vs TOW", "Sequential Plot" });
            this.navAccuracyComboBox.SelectedIndex = 0;
            this.plotChart();
        }

        private void frmCommNavAccVsTime_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmCommNavAccVsTime_ResizeEnd(object sender, EventArgs e)
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmCommNavAccVsTime));
            this.zedGraphControl1 = new ZedGraphControl();
            this.toolTip1 = new ToolTip(this.components);
            this.toolStrip1 = new ToolStrip();
            this.navAccuracyRefreshBtn = new ToolStripButton();
            this.navAccuracySetRefLocBtn = new ToolStripButton();
            this.navAccuracyComboBox = new ToolStripComboBox();
            this.navAccuracyHelpBtn = new ToolStripButton();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.zedGraphControl1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.zedGraphControl1.AutoSize = true;
            this.zedGraphControl1.BorderStyle = BorderStyle.Fixed3D;
            this.zedGraphControl1.Location = new Point(12, 0x29);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0.0;
            this.zedGraphControl1.ScrollMaxX = 0.0;
            this.zedGraphControl1.ScrollMaxY = 0.0;
            this.zedGraphControl1.ScrollMaxY2 = 0.0;
            this.zedGraphControl1.ScrollMinX = 0.0;
            this.zedGraphControl1.ScrollMinY = 0.0;
            this.zedGraphControl1.ScrollMinY2 = 0.0;
            this.zedGraphControl1.Size = new Size(0x255, 0x155);
            this.zedGraphControl1.TabIndex = 0x10;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.navAccuracyRefreshBtn, this.navAccuracySetRefLocBtn, this.navAccuracyComboBox, this.navAccuracyHelpBtn });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x26d, 0x19);
            this.toolStrip1.TabIndex = 0x17;
            this.toolStrip1.Text = "toolStrip1";
            this.navAccuracyRefreshBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.navAccuracyRefreshBtn.Image = Resources.refreshHS;
            this.navAccuracyRefreshBtn.ImageTransparentColor = Color.Magenta;
            this.navAccuracyRefreshBtn.Name = "navAccuracyRefreshBtn";
            this.navAccuracyRefreshBtn.Size = new Size(0x17, 0x16);
            this.navAccuracyRefreshBtn.Text = "Refresh";
            this.navAccuracyRefreshBtn.Click += new EventHandler(this.navAccuracyRefreshBtn_Click);
            this.navAccuracySetRefLocBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.navAccuracySetRefLocBtn.Image = Resources.RefLocation;
            this.navAccuracySetRefLocBtn.ImageTransparentColor = Color.Magenta;
            this.navAccuracySetRefLocBtn.Name = "navAccuracySetRefLocBtn";
            this.navAccuracySetRefLocBtn.Size = new Size(0x17, 0x16);
            this.navAccuracySetRefLocBtn.Text = "Set Reference Location";
            this.navAccuracySetRefLocBtn.Click += new EventHandler(this.navAccuracySetRefLocBtn_Click);
            this.navAccuracyComboBox.Name = "navAccuracyComboBox";
            this.navAccuracyComboBox.Size = new Size(0x79, 0x19);
            this.navAccuracyComboBox.Text = "X Axis Data Type";
            this.navAccuracyComboBox.ToolTipText = "X Axis Data Type";
            this.navAccuracyComboBox.SelectedIndexChanged += new EventHandler(this.navAccuracyComboBox_SelectedIndexChanged);
            this.navAccuracyHelpBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.navAccuracyHelpBtn.Image = Resources.Help;
            this.navAccuracyHelpBtn.ImageTransparentColor = Color.Magenta;
            this.navAccuracyHelpBtn.Name = "navAccuracyHelpBtn";
            this.navAccuracyHelpBtn.Size = new Size(0x17, 0x16);
            this.navAccuracyHelpBtn.Text = "Help";
            this.navAccuracyHelpBtn.Click += new EventHandler(this.navAccuracyHelpBtn_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x26d, 0x18a);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.zedGraphControl1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmCommNavAccVsTime";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Nav Accuracy vs Time";
            base.Load += new EventHandler(this.frmCommNavAccVsTime_Load);
            base.LocationChanged += new EventHandler(this.frmCommNavAccVsTime_LocationChanged);
            base.ResizeEnd += new EventHandler(this.frmCommNavAccVsTime_ResizeEnd);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void navAccuracyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.refreshPlot();
        }

        private void navAccuracyHelpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(clsGlobal.PlotHelpString, "Plot Help", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void navAccuracyRefreshBtn_Click(object sender, EventArgs e)
        {
            this.refreshPlot();
        }

        private void navAccuracySetRefLocBtn_Click(object sender, EventArgs e)
        {
            try
            {
                frmSetReferenceLocation location = new frmSetReferenceLocation();
                if (this.IsRealTime)
                {
                    location.CommWindow = this.comm;
                }
                else
                {
                    location.CommWindow = this.comm_forFile;
                }
                location.ShowDialog();
                location.CommWindow.Dispose();
                location.Dispose();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
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
            this.zedGraphControl1.Dispose();
            base.OnClosed(e);
        }

        private void plotChart()
        {
            GraphPane graphPane = this.zedGraphControl1.GraphPane;
            graphPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45f);
            int num = 0;
            if (this.IsRealTime)
            {
                if (this.comm.dataPlot != null)
                {
                    num = this.comm.dataPlot.GetNumSample_nvplot();
                }
            }
            else
            {
                num = this._length;
            }
            if (num >= 1)
            {
                double[] x = new double[num];
                double[] y = new double[num];
                PositionErrorCalc calc = new PositionErrorCalc();
                for (uint i = 0; i < num; i++)
                {
                    if (this.IsRealTime)
                    {
                        if (this.comm.dataPlot != null)
                        {
                            if (this.navAccuracyComboBox.SelectedIndex == 1)
                            {
                                x[i] = i + 1;
                            }
                            else
                            {
                                x[i] = this.comm.dataPlot.tows_nvplot[i];
                            }
                            calc.GetPositionErrorsInMeter(this.comm.dataPlot.Lat_nvplot[i], this.comm.dataPlot.Lon_nvplot[i], this.comm.dataPlot.Alt_nvplot[i], this.comm.dataPlot.RefLat, this.comm.dataPlot.RefLon, this.comm.dataPlot.RefAlt);
                        }
                    }
                    else
                    {
                        x[i] = this._tows[i];
                        calc.GetPositionErrorsInMeter(this._lat[i], this._lon[i], this._alt[i], this.comm_forFile.RxCtrl.RxNavData.RefLat, this.comm_forFile.RxCtrl.RxNavData.RefLon, this.comm_forFile.RxCtrl.RxNavData.RefAlt);
                    }
                    y[i] = calc.HorizontalError;
                }
                LineItem item = graphPane.AddCurve("", x, y, Color.Blue, SymbolType.Diamond);
                item.Symbol.Size = 3f;
                item.Symbol.Fill = new Fill(Color.Red);
                graphPane.Title.Text = "Nav Accuracy vs Time Plot";
                graphPane.XAxis.Title.Text = "TOW";
                graphPane.YAxis.Title.Text = "Horizontal Error (m)";
                graphPane.XAxis.Scale.FormatAuto = true;
                if (this.navAccuracyComboBox.SelectedIndex == 1)
                {
                    graphPane.Title.Text = "Nav Accuracy in Test Sequence";
                    graphPane.XAxis.Title.Text = "Sequence";
                }
                graphPane.XAxis.Scale.MaxAuto = true;
                graphPane.XAxis.Scale.MajorStepAuto = true;
                graphPane.YAxis.MajorGrid.IsVisible = true;
                graphPane.YAxis.MinorGrid.IsVisible = true;
                graphPane.YAxis.Scale.MaxAuto = true;
                graphPane.YAxis.Scale.MinAuto = true;
                graphPane.YAxis.Scale.FontSpec.FontColor = Color.Blue;
                graphPane.XAxis.Scale.FontSpec.Size = 8f;
                this.zedGraphControl1.AxisChange();
				base.Invoke((MethodInvoker)delegate
				{
                    this.zedGraphControl1.Refresh();
                    this.zedGraphControl1.Update();
                });
            }
        }

        private void refreshPlot()
        {
            if (this.navAccuracyComboBox.SelectedIndex == 1)
            {
                for (int i = 0; i < this._length; i++)
                {
                    this._tows[i] = i + 1;
                }
            }
            else
            {
                for (int j = 0; j < this._length; j++)
                {
                    this._tows[j] = this._tows_Back[j];
                }
            }
            this._rect = this.zedGraphControl1.GraphPane.Rect;
            this._rect2 = this.zedGraphControl1.MasterPane.Rect;
            this.zedGraphControl1.MasterPane = new MasterPane();
            this.zedGraphControl1.MasterPane.Rect = this._rect2;
            this.zedGraphControl1.GraphPane = new GraphPane();
            this.zedGraphControl1.GraphPane.Rect = this._rect;
            this.plotChart();
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
                this.Text = this.comm.sourceDeviceName + "Nav Accuracy vs Time";
            }
        }

        public delegate void updateParentEventHandler(string titleString);

        public delegate void UpdateWindowEventHandler(string titleString, int left, int top, int width, int height, bool state);
    }
}

