﻿namespace SiRFLive.GUI
{
    using SiRFLive.General;
    using SiRFLive.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using ZedGraph;

    public class frmMPMStatsPlots : Form
    {
        private Color _curveColor;
        private string _curveLabel;
        private GraphPane _myMainPane = new GraphPane();
        private double[] _myMainXaxisData;
        private double[] _myMainYaxisData;
        private string _reportTitle = string.Empty;
        private IContainer components;
        private string imageFilename = string.Empty;
        private ToolStripButton mpmPlotHelpBtn;
        private ToolStripButton sirfawarePlotRefreshBtn;
        private ToolStrip toolStrip1;
        private ZedGraphControl zedGraphControl1;

        public event UpdateParentEventHandler UpdateStatsData;

        public event UpdateParentEventHandler UpdateStatusToParentWin;

        public frmMPMStatsPlots()
        {
            this.InitializeComponent();
            this.zedGraphControl1.GraphPane = this._myMainPane;
        }

        public void AddCurveToPlot()
        {
            EventHandler method = null;
            try
            {
                if (this._myMainXaxisData.Length > 0)
                {
                    this._myMainPane.AddCurve(this._curveLabel, this._myMainXaxisData, this._myMainYaxisData, this._curveColor, SymbolType.Circle);
                    this.zedGraphControl1.AxisChange();
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

        public void CreateChart(ZedGraphControl zgc)
        {
            EventHandler method = null;
            this._myMainPane.Title.Text = this._reportTitle;
            this._myMainPane.XAxis.Title.Text = "TOW";
            this._myMainPane.YAxis.Title.Text = "Data";
            this._myMainPane.XAxis.IsVisible = true;
            this._myMainPane.XAxis.Type = AxisType.Linear;
            this._myMainPane.YAxis.MajorGrid.IsVisible = true;
            this._myMainPane.YAxis.MinorGrid.IsVisible = true;
            this._myMainPane.CurveList.Clear();
            this._myMainPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45f);
            if ((this._myMainXaxisData != null) && (this._myMainXaxisData.Length > 0))
            {
                this._myMainPane.AddCurve(this._curveLabel, this._myMainXaxisData, this._myMainYaxisData, this._curveColor, SymbolType.Diamond);
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
            zgc.Size = new Size(base.ClientRectangle.Width - 0x19, base.ClientRectangle.Height - 40);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmMPMStatsPlots_Load(object sender, EventArgs e)
        {
            this.CreateChart(this.zedGraphControl1);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmMPMStatsPlots));
            this.zedGraphControl1 = new ZedGraphControl();
            this.toolStrip1 = new ToolStrip();
            this.sirfawarePlotRefreshBtn = new ToolStripButton();
            this.mpmPlotHelpBtn = new ToolStripButton();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.zedGraphControl1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.zedGraphControl1.AutoSize = true;
            this.zedGraphControl1.BorderStyle = BorderStyle.Fixed3D;
            this.zedGraphControl1.Location = new Point(10, 0x21);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0.0;
            this.zedGraphControl1.ScrollMaxX = 0.0;
            this.zedGraphControl1.ScrollMaxY = 0.0;
            this.zedGraphControl1.ScrollMaxY2 = 0.0;
            this.zedGraphControl1.ScrollMinX = 0.0;
            this.zedGraphControl1.ScrollMinY = 0.0;
            this.zedGraphControl1.ScrollMinY2 = 0.0;
            this.zedGraphControl1.Size = new Size(0x1f7, 0x178);
            this.zedGraphControl1.TabIndex = 14;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.sirfawarePlotRefreshBtn, this.mpmPlotHelpBtn });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x20a, 0x19);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            this.sirfawarePlotRefreshBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.sirfawarePlotRefreshBtn.Image = Resources.refreshHS;
            this.sirfawarePlotRefreshBtn.ImageTransparentColor = Color.Magenta;
            this.sirfawarePlotRefreshBtn.Name = "sirfawarePlotRefreshBtn";
            this.sirfawarePlotRefreshBtn.Size = new Size(0x17, 0x16);
            this.sirfawarePlotRefreshBtn.Text = "Refresh";
            this.sirfawarePlotRefreshBtn.Click += new EventHandler(this.sirfawarePlotRefreshBtn_Click);
            this.mpmPlotHelpBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.mpmPlotHelpBtn.Image = Resources.Help;
            this.mpmPlotHelpBtn.ImageTransparentColor = Color.Magenta;
            this.mpmPlotHelpBtn.Name = "mpmPlotHelpBtn";
            this.mpmPlotHelpBtn.Size = new Size(0x17, 0x16);
            this.mpmPlotHelpBtn.Text = "Help";
            this.mpmPlotHelpBtn.Click += new EventHandler(this.mpmPlotHelpBtn_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x20a, 0x1a5);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.zedGraphControl1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmMPMStatsPlots";
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SiRFaware Plots";
            base.Load += new EventHandler(this.frmMPMStatsPlots_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void mpmPlotHelpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(clsGlobal.PlotHelpString, "Plot Help", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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

        public void RefreshChart()
        {
            this.CreateChart(this.zedGraphControl1);
        }

        public void SavePlots(string filePath)
        {
            Bitmap image = new Bitmap(1, 1);
            using (Graphics.FromImage(image))
            {
                this._myMainPane.GetImage().Save(filePath, ImageFormat.Jpeg);
            }
        }

        public void SetPlotData(double[] xData, double[] yData, string title, string label, Color crveColor)
        {
            try
            {
                this._myMainXaxisData = xData;
                this._myMainYaxisData = yData;
                this._curveColor = crveColor;
                this._curveLabel = label;
                this._reportTitle = title;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void sirfawarePlotRefreshBtn_Click(object sender, EventArgs e)
        {
            if (this.UpdateStatsData != null)
            {
                this.UpdateStatsData();
            }
        }

        public delegate void UpdateParentEventHandler();
    }
}

