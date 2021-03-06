﻿namespace SiRFLive.GUI
{
    using SiRFLive.Analysis;
    using SiRFLive.General;
    using SiRFLive.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using ZedGraph;

    public class frmCDFPlots : Form
    {
        private Color _curveColor;
        private string _curveLabel;
        private int _curveNumber;
        private GraphPane _myMainPane = new GraphPane();
        private double[] _myMainXaxisData;
        private string _reportTitle = string.Empty;
        private ToolStripButton cdfPlotHelpBtn;
        private ToolStripButton cdfPlotRefreshBtn;
        private IContainer components;
        private const int MAXCURVES = 10;
        private ToolStrip toolStrip1;
        private ZedGraphControl zedGraphControl1;

        public event UpdateParentEventHandler UpdateStatsData;

        public event UpdateParentEventHandler UpdateStatusToParentWin;

        public frmCDFPlots()
        {
            this.InitializeComponent();
            this.zedGraphControl1.GraphPane = this._myMainPane;
        }

        public void AddCurveToPlot(Stats dataClass, int Length, string label, Color crveColor)
        {
            EventHandler method = null;
            try
            {
                this._myMainXaxisData = new double[Length];
                for (int i = 0; i < Length; i++)
                {
                    this._myMainXaxisData[i] = dataClass.DataList[i];
                }
                this._curveColor = crveColor;
                this._curveLabel = label;
                if (this._myMainXaxisData.Length > 0)
                {
                    Stats stats = new Stats();
                    double[] y = new double[this._myMainXaxisData.Length];
                    for (int j = 0; j < this._myMainXaxisData.Length; j++)
                    {
                        y[j] = (j + 1) * (1.0 / ((double) this._myMainXaxisData.Length));
                    }
                    stats.SortArray(this._myMainXaxisData.Length, this._myMainXaxisData);
                    this._myMainPane.AddCurve(this._curveLabel, this._myMainXaxisData, y, this._curveColor, SymbolType.Circle);
                    this._myMainPane.AxisChange();
                    if (method == null)
                    {
                        method = delegate {
                            this.zedGraphControl1.Refresh();
                            this.zedGraphControl1.Update();
                        };
                    }
                    base.Invoke(method);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void cdfPlotHelpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(clsGlobal.PlotHelpString, "Plot Help", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void cdfPlotRefreshBtn_Click(object sender, EventArgs e)
        {
            if (this.UpdateStatsData != null)
            {
                this.UpdateStatsData();
            }
        }

        public void CreateChart(ZedGraphControl zgc)
        {
            EventHandler method = null;
            this._myMainPane.YAxis.MajorGrid.IsVisible = true;
            this._myMainPane.YAxis.MinorGrid.IsVisible = true;
            this._myMainPane.CurveList.Clear();
            this._myMainPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45f);
            this._myMainPane.Title.Text = this._reportTitle;
            this._myMainPane.XAxis.Title.Text = "Data";
            this._myMainPane.YAxis.Title.Text = "Probability";
            this._myMainPane.YAxis.Scale.MaxAuto = false;
            this._myMainPane.YAxis.Scale.MinAuto = false;
            this._myMainPane.YAxis.Scale.Max = 1.0;
            this._myMainPane.YAxis.Scale.Min = 0.0;
            if ((this._myMainXaxisData != null) && (this._myMainXaxisData.Length > 0))
            {
                Stats stats = new Stats();
                double[] y = new double[this._myMainXaxisData.Length];
                for (int i = 0; i < this._myMainXaxisData.Length; i++)
                {
                    y[i] = (i + 1) * (1.0 / ((double) this._myMainXaxisData.Length));
                }
                stats.SortArray(this._myMainXaxisData.Length, this._myMainXaxisData);
                this._myMainPane.AddCurve(this._curveLabel, this._myMainXaxisData, y, this._curveColor, SymbolType.Diamond);
                this._myMainPane.AxisChange();
                if (method == null)
                {
                    method = delegate {
                        zgc.Update();
                        zgc.Refresh();
                    };
                }
                base.Invoke(method);
            }
            zgc.Size = new Size(base.ClientRectangle.Width - 0x19, base.ClientRectangle.Height - 0x2d);
        }

        private string CreateImageFileName(string baseName)
        {
            DateTime now = DateTime.Now;
            string str = baseName;
            string str2 = string.Format("{0:ddMMyyyy_hhmmss}", now);
            return (str + "_" + str2 + ".jpg");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmCDFPlots_Load(object sender, EventArgs e)
        {
            this.CreateChart(this.zedGraphControl1);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmCDFPlots));
            this.zedGraphControl1 = new ZedGraphControl();
            this.toolStrip1 = new ToolStrip();
            this.cdfPlotRefreshBtn = new ToolStripButton();
            this.cdfPlotHelpBtn = new ToolStripButton();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.zedGraphControl1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.zedGraphControl1.AutoSize = true;
            this.zedGraphControl1.Location = new Point(12, 0x27);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0.0;
            this.zedGraphControl1.ScrollMaxX = 0.0;
            this.zedGraphControl1.ScrollMaxY = 0.0;
            this.zedGraphControl1.ScrollMaxY2 = 0.0;
            this.zedGraphControl1.ScrollMinX = 0.0;
            this.zedGraphControl1.ScrollMinY = 0.0;
            this.zedGraphControl1.ScrollMinY2 = 0.0;
            this.zedGraphControl1.Size = new Size(0x260, 0x13c);
            this.zedGraphControl1.TabIndex = 13;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.cdfPlotRefreshBtn, this.cdfPlotHelpBtn });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x278, 0x19);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            this.cdfPlotRefreshBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.cdfPlotRefreshBtn.Image = Resources.refreshHS;
            this.cdfPlotRefreshBtn.ImageTransparentColor = Color.Magenta;
            this.cdfPlotRefreshBtn.Name = "cdfPlotRefreshBtn";
            this.cdfPlotRefreshBtn.Size = new Size(0x17, 0x16);
            this.cdfPlotRefreshBtn.Text = "Refresh";
            this.cdfPlotRefreshBtn.Click += new EventHandler(this.cdfPlotRefreshBtn_Click);
            this.cdfPlotHelpBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.cdfPlotHelpBtn.Image = Resources.Help;
            this.cdfPlotHelpBtn.ImageTransparentColor = Color.Magenta;
            this.cdfPlotHelpBtn.Name = "cdfPlotHelpBtn";
            this.cdfPlotHelpBtn.Size = new Size(0x17, 0x16);
            this.cdfPlotHelpBtn.Text = "Help";
            this.cdfPlotHelpBtn.Click += new EventHandler(this.cdfPlotHelpBtn_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.ClientSize = new Size(0x278, 0x171);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.zedGraphControl1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmCDFPlots";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "CDF Plots";
            base.Load += new EventHandler(this.frmCDFPlots_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.UpdateStatusToParentWin != null)
            {
                this.UpdateStatusToParentWin();
            }
            this._myMainPane = null;
            base.OnClosed(e);
        }

        public void RefreshPlot()
        {
            this.CreateChart(this.zedGraphControl1);
        }

        public void SetPlotData(Stats dataClass, int Length, string title, string label, Color crveColor)
        {
            try
            {
                this._myMainXaxisData = new double[Length];
                for (int i = 0; i < Length; i++)
                {
                    this._myMainXaxisData[i] = dataClass.DataList[i];
                }
                this._curveColor = crveColor;
                this._curveLabel = label;
                this._reportTitle = title;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public void SetXAxisTitle(string name)
        {
            EventHandler method = null;
            try
            {
                this._myMainPane.XAxis.Title.Text = name;
                if (method == null)
                {
                    method = delegate {
                        this.zedGraphControl1.Update();
                        this.zedGraphControl1.Refresh();
                    };
                }
                base.Invoke(method);
            }
            catch
            {
            }
        }

        public delegate void UpdateParentEventHandler();
    }
}

