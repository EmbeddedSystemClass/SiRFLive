﻿namespace SiRFLive.GUI
{
    using SiRFLive.Analysis;
    using SiRFLive.General;
    using SiRFLive.Properties;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmTTFSView : Form
    {
        private frmCDFPlots _CDFPlotWin;
        private IContainer components;
        private DataGridView dataGridView1;
        private SaveFileDialog ExportCSVsaveFileDialog;
        private ToolStripButton exportToFileBtn;
        private ToolStrip toolStrip1;
        private DataGridViewTextBoxColumn TTFS;
        private ToolStripButton ttfsClearBtn;
        public TTFSData TTFSDataElement = new TTFSData();
        private ToolStripButton ttfsPlotBtn;
        private DataGridViewTextBoxColumn UTCTime;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmTTFSView()
        {
            this.InitializeComponent();
            this.TTFSDataElement.TTFSDataView = this.dataGridView1;
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
            this.dataGridView1.ReadOnly = true;
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            if ((this.TTFSDataElement != null) && this.TTFSDataElement.isValid)
            {
                if (this.dataGridView1.RowCount > 0x2710)
                {
                    this.dataGridView1.Rows.RemoveAt(0);
                }
                GPSDateTime time = new GPSDateTime();
                time.SetUTCOffset(15);
                time.SetTime(this.TTFSDataElement.Week, this.TTFSDataElement.TOW);
                DateTime time2 = time.GetTime();
                string str = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", new object[] { time2.Hour, time2.Minute, time2.Second, time2.Millisecond });
                int num = this.dataGridView1.Rows.Add();
                this.dataGridView1["TTFS", num].Value = this.TTFSDataElement.TTFS.ToString();
                this.dataGridView1["UTCTime", num].Value = str;
                this.dataGridView1.Rows[num].HeaderCell.Value = (num + 1).ToString();
                if (this.dataGridView1.Rows.Count >= 2)
                {
                    this.dataGridView1.FirstDisplayedScrollingRowIndex = this.dataGridView1.Rows.Count - 2;
                }
                else
                {
                    this.dataGridView1.FirstDisplayedScrollingRowIndex = 0;
                }
                this.TTFSDataElement.isValid = false;
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

        private void exportToFileBtn_Click(object sender, EventArgs e)
        {
            this.ExportCSVsaveFileDialog.CreatePrompt = true;
            this.ExportCSVsaveFileDialog.OverwritePrompt = true;
            this.ExportCSVsaveFileDialog.DefaultExt = "csv";
            this.ExportCSVsaveFileDialog.FileName = "ttfs.csv";
            this.ExportCSVsaveFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            if (this.ExportCSVsaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string siRFLiveVersion = clsGlobal.SiRFLiveVersion;
                StreamWriter writer = new StreamWriter(this.ExportCSVsaveFileDialog.FileName);
                string str2 = string.Format("TTFS Data from SiRFLive Version {0}...", siRFLiveVersion);
                writer.WriteLine(str2);
                writer.WriteLine();
                str2 = string.Format("TTFS,UTC Time", new object[0]);
                writer.WriteLine(str2);
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    writer.WriteLine("{0:F2},{1}", this.dataGridView1["TTFS", i].Value, this.dataGridView1["UTCTime", i].Value);
                }
                writer.Close();
            }
        }

        private void frmTTFSView_Load(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
        }

        private void frmTTFSView_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmTTFSView_ResizeEnd(object sender, EventArgs e)
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
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmTTFSView));
            this.dataGridView1 = new DataGridView();
            this.TTFS = new DataGridViewTextBoxColumn();
            this.UTCTime = new DataGridViewTextBoxColumn();
            this.toolStrip1 = new ToolStrip();
            this.ttfsPlotBtn = new ToolStripButton();
            this.ttfsClearBtn = new ToolStripButton();
            this.exportToFileBtn = new ToolStripButton();
            this.ExportCSVsaveFileDialog = new SaveFileDialog();
            ((ISupportInitialize) this.dataGridView1).BeginInit();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.BackColor = SystemColors.Control;
            style.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            style.ForeColor = SystemColors.WindowText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = style;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[] { this.TTFS, this.UTCTime });
            style2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style2.BackColor = SystemColors.Window;
            style2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            style2.ForeColor = SystemColors.ControlText;
            style2.SelectionBackColor = SystemColors.Highlight;
            style2.SelectionForeColor = SystemColors.HighlightText;
            style2.WrapMode = DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = style2;
            this.dataGridView1.Dock = DockStyle.Fill;
            this.dataGridView1.Location = new Point(0, 0x19);
            this.dataGridView1.Name = "dataGridView1";
            style3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style3.BackColor = SystemColors.Control;
            style3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            style3.ForeColor = SystemColors.WindowText;
            style3.SelectionBackColor = SystemColors.Highlight;
            style3.SelectionForeColor = SystemColors.HighlightText;
            style3.WrapMode = DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = style3;
            this.dataGridView1.Size = new Size(0x125, 0xf7);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Paint += new PaintEventHandler(this.dataGridView1_Paint);
            this.TTFS.HeaderText = "TTFS(s)";
            this.TTFS.Name = "TTFS";
            this.UTCTime.HeaderText = "UTC Time";
            this.UTCTime.Name = "UTCTime";
            this.UTCTime.Width = 150;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.ttfsPlotBtn, this.ttfsClearBtn, this.exportToFileBtn });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x125, 0x19);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.ttfsPlotBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.ttfsPlotBtn.Image = Resources.ttff;
            this.ttfsPlotBtn.ImageTransparentColor = Color.Magenta;
            this.ttfsPlotBtn.Name = "ttfsPlotBtn";
            this.ttfsPlotBtn.Size = new Size(0x17, 0x16);
            this.ttfsPlotBtn.Text = "Plot";
            this.ttfsPlotBtn.Click += new EventHandler(this.ttfsPlotBtn_Click);
            this.ttfsClearBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.ttfsClearBtn.Image = Resources.clearTableHS;
            this.ttfsClearBtn.ImageTransparentColor = Color.Magenta;
            this.ttfsClearBtn.Name = "ttfsClearBtn";
            this.ttfsClearBtn.Size = new Size(0x17, 0x16);
            this.ttfsClearBtn.Text = "Clear";
            this.ttfsClearBtn.Click += new EventHandler(this.ttfsClear_Click);
            this.exportToFileBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.exportToFileBtn.Image = Resources.log;
            this.exportToFileBtn.ImageTransparentColor = Color.Magenta;
            this.exportToFileBtn.Name = "exportToFileBtn";
            this.exportToFileBtn.Size = new Size(0x17, 0x16);
            this.exportToFileBtn.Text = "Export To File";
            this.exportToFileBtn.Click += new EventHandler(this.exportToFileBtn_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x125, 0x110);
            base.Controls.Add(this.dataGridView1);
            base.Controls.Add(this.toolStrip1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmTTFSView";
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Time To First Sync";
            base.Load += new EventHandler(this.frmTTFSView_Load);
            base.LocationChanged += new EventHandler(this.frmTTFSView_LocationChanged);
            base.ResizeEnd += new EventHandler(this.frmTTFSView_ResizeEnd);
            ((ISupportInitialize) this.dataGridView1).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, false);
            }
            if (this._CDFPlotWin != null)
            {
                this._CDFPlotWin.Close();
            }
        }

        private void RefreshCDFPlot()
        {
            if (this._CDFPlotWin != null)
            {
                Stats dataClass = new Stats();
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        dataClass.InsertSample(Convert.ToDouble(this.dataGridView1["TTFS", i].Value.ToString()));
                    }
                    catch
                    {
                    }
                }
                string label = "Time To First Sync";
                if ((dataClass != null) && (dataClass.Samples > 0))
                {
                    this._CDFPlotWin.SetPlotData(dataClass, dataClass.Samples, "CDF Plot", label, Color.Blue);
                    this._CDFPlotWin.RefreshPlot();
                    this._CDFPlotWin.SetXAxisTitle("Time To First Frame Sync(s)");
                }
            }
        }

        private void ttfsClear_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
        }

        private void ttfsPlotBtn_Click(object sender, EventArgs e)
        {
            this.ttfsPlotBtn.Enabled = false;
            if (this._CDFPlotWin == null)
            {
                this._CDFPlotWin = new frmCDFPlots();
                this._CDFPlotWin.MdiParent = base.MdiParent;
                this._CDFPlotWin.UpdateStatusToParentWin += new frmCDFPlots.UpdateParentEventHandler(this.updatePlotStatus);
                this._CDFPlotWin.UpdateStatsData += new frmCDFPlots.UpdateParentEventHandler(this.RefreshCDFPlot);
            }
            Stats dataClass = new Stats();
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                try
                {
                    dataClass.InsertSample(Convert.ToDouble(this.dataGridView1["TTFS", i].Value.ToString()));
                }
                catch
                {
                    this.ttfsPlotBtn.Enabled = true;
                }
            }
            string label = "Time To First Sync";
            if ((dataClass != null) && (dataClass.Samples > 0))
            {
                this._CDFPlotWin.SetPlotData(dataClass, dataClass.Samples, "CDF Plot", label, Color.Blue);
                this._CDFPlotWin.Show();
                this._CDFPlotWin.SetXAxisTitle("Time To First Frame Sync(s)");
            }
        }

        private void updatePlotStatus()
        {
            this.ttfsPlotBtn.Enabled = true;
            this._CDFPlotWin = null;
        }

        public delegate void UpdateWindowEventHandler(string titleString, int left, int top, int width, int height, bool state);
    }
}

