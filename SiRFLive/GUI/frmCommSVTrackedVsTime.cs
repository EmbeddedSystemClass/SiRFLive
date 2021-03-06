﻿namespace SiRFLive.GUI
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using ZedGraph;

    public class frmCommSVTrackedVsTime : Form
    {
        private double[,] _cnos;
        private Color[] _color;
        private int _length;
        private static int _numberOpen;
        private int _numSvs;
        private string _persistedWindowName;
        private RectangleF _rect;
        private RectangleF _rect2;
        private int[] _svids;
        private double[] _tows;
        private double[] _tows_Back;
        private CommunicationManager comm;
        private IContainer components;
        private bool IsRealTime;
        private ToolStripButton SVTrackHelpBtn;
        private ToolStripButton svTrackRefreshBtn;
        private ToolStripComboBox svTrackXAxisComboBox;
        private ToolStrip toolStrip1;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;
        private ZedGraphControl zg1;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmCommSVTrackedVsTime()
        {
            this._persistedWindowName = "SV Average CNo Window";
            this._color = new Color[] { Color.Red, Color.Blue };
            this.IsRealTime = true;
            this.InitializeComponent();
            _numberOpen++;
            this._persistedWindowName = "SV Tracked vs Time Window " + _numberOpen.ToString();
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
        }

        public frmCommSVTrackedVsTime(int numSvs, int[] svids, int length, double[] tows, double[,] cnos)
        {
            this._persistedWindowName = "SV Average CNo Window";
            this._color = new Color[] { Color.Red, Color.Blue };
            this.IsRealTime = false;
            this._numSvs = numSvs;
            this._length = length;
            this._svids = new int[this._numSvs];
            this._tows = new double[this._length];
            this._tows_Back = new double[this._length];
            this._cnos = new double[this._numSvs, this._length];
            for (int i = 0; i < this._numSvs; i++)
            {
                this._svids[i] = svids[i];
            }
            for (int j = 0; j < this._length; j++)
            {
                this._tows[j] = tows[j];
                this._tows_Back[j] = tows[j];
            }
            for (int k = 0; k < this._numSvs; k++)
            {
                for (int m = 0; m < this._length; m++)
                {
                    this._cnos[k, m] = cnos[k, m];
                }
            }
            this.InitializeComponent();
        }

        public void CreateChart()
        {
            GraphPane graphPane = this.zg1.GraphPane;
            graphPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45f);
            int numTrackedSVs = 0;
            int numSamplesTrackVsTimePlot = 0;
            if (this.IsRealTime)
            {
                if (this.comm.dataPlot != null)
                {
                    numTrackedSVs = this.comm.dataPlot.GetNumTrackedSVs();
                    numSamplesTrackVsTimePlot = this.comm.dataPlot.GetNumSamplesTrackVsTimePlot();
                }
            }
            else
            {
                numTrackedSVs = this._numSvs;
                numSamplesTrackVsTimePlot = this._length;
            }
            double[] x = new double[numSamplesTrackVsTimePlot];
            double[] y = new double[numSamplesTrackVsTimePlot];
            if (numSamplesTrackVsTimePlot > 0)
            {
                for (int i = 0; i < numTrackedSVs; i++)
                {
                    int num4 = 0;
                    if (this.IsRealTime)
                    {
                        if (this.comm.dataPlot != null)
                        {
                            num4 = this.comm.dataPlot.SVTrkr.SVIDs[i];
                        }
                    }
                    else
                    {
                        num4 = this._svids[i];
                    }
                    for (int j = 0; j < numSamplesTrackVsTimePlot; j++)
                    {
                        if (this.IsRealTime)
                        {
                            if (this.comm.dataPlot != null)
                            {
                                if (this.svTrackXAxisComboBox.SelectedIndex == 1)
                                {
                                    x[j] = j + 1;
                                }
                                else
                                {
                                    x[j] = this.comm.dataPlot.tows[j];
                                }
                                if (this.comm.dataPlot.cnos[i, j] > 1.0)
                                {
                                    y[j] = num4;
                                }
                                else
                                {
                                    y[j] = 0.0;
                                }
                            }
                        }
                        else
                        {
                            x[j] = this._tows[j];
                            if (this._cnos[i, j] > 1.0)
                            {
                                y[j] = num4;
                            }
                            else
                            {
                                y[j] = 0.0;
                            }
                        }
                    }
                    LineItem item = graphPane.AddCurve("", x, y, Color.Blue, SymbolType.Diamond);
                    item.Line.IsVisible = false;
                    item.Symbol.Size = 3f;
                    item.Symbol.Fill = new Fill(Color.Red);
                }
                graphPane.Title.Text = "SVs Tracked vs Time";
                graphPane.XAxis.Title.Text = "Time";
                graphPane.YAxis.Title.Text = "Satellite ID";
                graphPane.XAxis.Scale.FormatAuto = true;
                if (this.svTrackXAxisComboBox.SelectedIndex == 1)
                {
                    graphPane.Title.Text = "SVs Tracked in Test Sequence";
                    graphPane.XAxis.Title.Text = "Sequence";
                }
                graphPane.YAxis.Scale.MaxAuto = false;
                graphPane.YAxis.Scale.MinAuto = false;
                graphPane.YAxis.MajorGrid.IsVisible = true;
                graphPane.YAxis.MinorGrid.IsVisible = true;
                graphPane.YAxis.Scale.Max = 33.0;
                graphPane.YAxis.Scale.Min = 0.5;
                graphPane.XAxis.Scale.FontSpec.Size = 10f;
                graphPane.IsFontsScaled = true;
                this.zg1.AxisChange();
				base.Invoke((MethodInvoker)delegate
				{
                    this.zg1.Refresh();
                    this.zg1.Update();
                });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmCommSVTrackedVsTime_Load(object sender, EventArgs e)
        {
            this.svTrackXAxisComboBox.Items.AddRange(new object[] { "Plot vs TOW", "Sequential Plot" });
            this.svTrackXAxisComboBox.SelectedIndex = 0;
            this.CreateChart();
        }

        private void frmCommSVTrackedVsTime_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmCommSVTrackedVsTime_ResizeEnd(object sender, EventArgs e)
        {
            this.WinWidth = base.Width;
            this.WinHeight = base.Height;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private int getBin(double cno)
        {
            if (cno > 35.0)
            {
                return 0;
            }
            return 1;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmCommSVTrackedVsTime));
            this.zg1 = new ZedGraphControl();
            this.toolStrip1 = new ToolStrip();
            this.svTrackRefreshBtn = new ToolStripButton();
            this.svTrackXAxisComboBox = new ToolStripComboBox();
            this.SVTrackHelpBtn = new ToolStripButton();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.zg1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.zg1.AutoSize = true;
            this.zg1.BorderStyle = BorderStyle.Fixed3D;
            this.zg1.Location = new Point(0x16, 0x2c);
            this.zg1.Name = "zg1";
            this.zg1.ScrollGrace = 0.0;
            this.zg1.ScrollMaxX = 0.0;
            this.zg1.ScrollMaxY = 0.0;
            this.zg1.ScrollMaxY2 = 0.0;
            this.zg1.ScrollMinX = 0.0;
            this.zg1.ScrollMinY = 0.0;
            this.zg1.ScrollMinY2 = 0.0;
            this.zg1.Size = new Size(0x2b4, 0x143);
            this.zg1.TabIndex = 15;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.svTrackRefreshBtn, this.svTrackXAxisComboBox, this.SVTrackHelpBtn });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x2e2, 0x19);
            this.toolStrip1.TabIndex = 0x17;
            this.toolStrip1.Text = "toolStrip1";
            this.svTrackRefreshBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.svTrackRefreshBtn.Image = Resources.refreshHS;
            this.svTrackRefreshBtn.ImageTransparentColor = Color.Magenta;
            this.svTrackRefreshBtn.Name = "svTrackRefreshBtn";
            this.svTrackRefreshBtn.Size = new Size(0x17, 0x16);
            this.svTrackRefreshBtn.Text = "Refresh";
            this.svTrackRefreshBtn.Click += new EventHandler(this.svTrackRefreshBtn_Click);
            this.svTrackXAxisComboBox.Name = "svTrackXAxisComboBox";
            this.svTrackXAxisComboBox.Size = new Size(0x79, 0x19);
            this.svTrackXAxisComboBox.SelectedIndexChanged += new EventHandler(this.svTrackXAxisComboBox_SelectedIndexChanged);
            this.SVTrackHelpBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.SVTrackHelpBtn.Image = Resources.Help;
            this.SVTrackHelpBtn.ImageTransparentColor = Color.Magenta;
            this.SVTrackHelpBtn.Name = "SVTrackHelpBtn";
            this.SVTrackHelpBtn.Size = new Size(0x17, 0x16);
            this.SVTrackHelpBtn.Text = "Help";
            this.SVTrackHelpBtn.Click += new EventHandler(this.SVTrackHelpBtn_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x2e2, 390);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.zg1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmCommSVTrackedVsTime";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SV Tracked vs Time";
            base.Load += new EventHandler(this.frmCommSVTrackedVsTime_Load);
            base.LocationChanged += new EventHandler(this.frmCommSVTrackedVsTime_LocationChanged);
            base.ResizeEnd += new EventHandler(this.frmCommSVTrackedVsTime_ResizeEnd);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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

        private void refreshPlot()
        {
            if (this.svTrackXAxisComboBox.SelectedIndex == 1)
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
            this._rect = this.zg1.GraphPane.Rect;
            this._rect2 = this.zg1.MasterPane.Rect;
            this.zg1.MasterPane = new MasterPane();
            this.zg1.MasterPane.Rect = this._rect2;
            this.zg1.GraphPane = new GraphPane();
            this.zg1.GraphPane.Rect = this._rect;
            this.CreateChart();
        }

        private void SVTrackHelpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(clsGlobal.PlotHelpString, "Plot Help", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void svTrackRefreshBtn_Click(object sender, EventArgs e)
        {
            this.refreshPlot();
        }

        private void svTrackXAxisComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.refreshPlot();
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
                this.Text = this.comm.sourceDeviceName + ": SV CNo ";
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

