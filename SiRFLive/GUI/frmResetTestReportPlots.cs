﻿namespace SiRFLive.GUI
{
    using SiRFLive.Analysis;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows.Forms;
    using ZedGraph;

    public class frmResetTestReportPlots : Form
    {
        private string _PaneTitle = string.Empty;
        private IContainer components;
        private MasterPane myMaster;
        private GraphPane myPane;
        private ColorSymbolRotator rotator = new ColorSymbolRotator();
        private ZedGraphControl zedGraphControl1;

        public frmResetTestReportPlots()
        {
            this.InitializeComponent();
        }

        public void AddCurve(GraphPane myPane, Stats dataClass, int Length, string title)
        {
            PointPairList points = new PointPairList();
            for (int i = 0; i < Length; i++)
            {
                double x = Math.Abs(dataClass.DataList[i]);
                double y = (i + 1) * (1.0 / ((double) Length));
                points.Add(x, y);
            }
            myPane.AddCurve(title, points, this.rotator.NextColor, this.rotator.NextSymbol).Symbol.Fill = new Fill(Color.White);
            myPane.YAxis.Scale.MaxAuto = false;
            myPane.YAxis.Scale.MinAuto = false;
            myPane.YAxis.Scale.Max = 1.0;
            myPane.YAxis.Scale.Min = 0.0;
            this.zedGraphControl1.AxisChange();
        }

        public GraphPane AddPane(string title, string label)
        {
            this.myPane = new GraphPane(new Rectangle(10, 10, 10, 10), title, label, "Probability");
            this.myPane.Fill = new Fill(Color.White, Color.LightYellow, 45f);
            this.myPane.BaseDimension = 6f;
            this.myMaster.Add(this.myPane);
            using (Graphics graphics = base.CreateGraphics())
            {
                this.myMaster.SetLayout(graphics, PaneLayout.SingleColumn);
            }
            this.zedGraphControl1.AxisChange();
            this.myPane.YAxis.MajorGrid.IsVisible = true;
            return this.myPane;
        }

        public void CreateCharts()
        {
            this.myMaster = new MasterPane();
            this.myMaster = this.zedGraphControl1.MasterPane;
            this.myMaster.PaneList.Clear();
            this.myMaster.Title.Text = this._PaneTitle;
            this.myMaster.Title.IsVisible = true;
            this.myMaster.Fill = new Fill(Color.White, Color.MediumSlateBlue, 45f);
            this.myMaster.Margin.All = 10f;
        }

        private string CreateImageFileName(string baseName)
        {
            DateTime now = DateTime.Now;
            string str = baseName;
            string str2 = string.Format("{0:ddMMyyyy_hhmm}", now);
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

        private void frmResetTestReportPlots_Load(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmResetTestReportPlots));
            this.zedGraphControl1 = new ZedGraphControl();
            base.SuspendLayout();
            this.zedGraphControl1.Location = new Point(0x30, 0x2f);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0.0;
            this.zedGraphControl1.ScrollMaxX = 0.0;
            this.zedGraphControl1.ScrollMaxY = 0.0;
            this.zedGraphControl1.ScrollMaxY2 = 0.0;
            this.zedGraphControl1.ScrollMinX = 0.0;
            this.zedGraphControl1.ScrollMinY = 0.0;
            this.zedGraphControl1.ScrollMinY2 = 0.0;
            this.zedGraphControl1.Size = new Size(0x349, 0x2d9);
            this.zedGraphControl1.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x3a8, 0x314);
            base.Controls.Add(this.zedGraphControl1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmResetTestReportPlots";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Reset Test Report Plots";
            base.Load += new EventHandler(this.frmResetTestReportPlots_Load);
            base.ResumeLayout(false);
        }

        public void RefreshGraph()
        {
			base.Invoke((MethodInvoker)delegate
			{
                this.zedGraphControl1.Refresh();
                this.zedGraphControl1.Update();
            });
        }

        public void SaveGraphToFile(string directory, string imageFilename)
        {
            string imageFullPathName = directory + @"\" + imageFilename;
			base.Invoke((MethodInvoker)delegate
			{
                Bitmap image = new Bitmap(1, 1);
                using (Graphics.FromImage(image))
                {
                    this.myMaster.GetImage().Save(imageFullPathName, ImageFormat.Jpeg);
                }
            });
        }

        public string PaneTitle
        {
            get
            {
                return this._PaneTitle;
            }
            set
            {
                this._PaneTitle = value;
            }
        }
    }
}

