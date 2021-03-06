﻿namespace SiRFLive.GUI.Reporting
{
    using SiRFLive.Reporting;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
	using System.Configuration;

    public class frmLSMTestReport : Form
    {
        private int _defaultWidth;
        private int _maxItemWidth;
        private Label AltIndexLabel;
        private Label cnoIndexLabel;
        private IContainer components;
        private Button e911ReportCancelBut;
        private Button e911ReportDirBrowser;
        private Button e911ReportGenBut;
        private ComboBox frmLSMRefLocationComboBox;
        private Label frmLSMReportLongitudeLabel;
        private Label frmLSMReportRefAltitudeLabel;
        private TextBox frmLSMReportRefAltitudeTextBox;
        private Label frmLSMReportRefLatitudeLabel;
        private TextBox frmLSMReportRefLatitudeTextBox;
        private TextBox frmLSMReportRefLongitudeTextBox;
        private Button frmLSMReportSetAsDefaultBtn;
        private GroupBox frmRxInitCmdRefLocationGrpBox;
        private Label latIndexLabel;
        private Label lonIndexLabel;
        private TextBox LSMAltIndexTextBox;
        private TextBox LSMLonIndexTextBox;
        private TextBox LSMReportCNoIndexTextBox;
        private Label LSMReportHrErrLimLabel;
        private TextBox LSMReportHrErrPerVal;
        private TextBox LSMReportLatIndexTextBox;
        private Label LSMReportPerLabel;
        private TextBox LSMReportPerVal;
        private Label LSMReportTimeoutLabel;
        private TextBox LSMReportTimeoutVal;
        private TextBox LSMReportTTFFIndexTextBox;
        private Label LSMReportTTFFLimitLabel;
        private TextBox LSMReportTTFFLimitVal;
        private Label LSMReportTTFFLimLabel;
        private TextBox LSMReportTTFFPerVal;
        private NavigationAnalysisData nav_data = new NavigationAnalysisData(ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Protocols\ReferenceLocation.xml");
        private TextBox reportDirVal;
        private Label ttffIndexLabel;
        private Label updateTestLogPathLabel;

        public frmLSMTestReport()
        {
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

        private void frmLSMRefLocationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.updateReferencePositionTextBox();
        }

        private void frmLSMTestReport_Load(object sender, EventArgs e)
        {
            this.updateDefautlReferenceLocationComboBox();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmLSMTestReport));
            this.frmRxInitCmdRefLocationGrpBox = new GroupBox();
            this.frmLSMReportSetAsDefaultBtn = new Button();
            this.frmLSMReportRefLatitudeTextBox = new TextBox();
            this.frmLSMReportRefLatitudeLabel = new Label();
            this.frmLSMReportRefLongitudeTextBox = new TextBox();
            this.frmLSMReportLongitudeLabel = new Label();
            this.frmLSMReportRefAltitudeTextBox = new TextBox();
            this.frmLSMReportRefAltitudeLabel = new Label();
            this.frmLSMRefLocationComboBox = new ComboBox();
            this.LSMReportHrErrLimLabel = new Label();
            this.LSMReportTTFFLimitLabel = new Label();
            this.LSMReportTimeoutLabel = new Label();
            this.LSMReportPerLabel = new Label();
            this.LSMReportTTFFLimLabel = new Label();
            this.LSMReportHrErrPerVal = new TextBox();
            this.LSMReportTTFFLimitVal = new TextBox();
            this.LSMReportTimeoutVal = new TextBox();
            this.LSMReportPerVal = new TextBox();
            this.LSMReportTTFFPerVal = new TextBox();
            this.e911ReportCancelBut = new Button();
            this.e911ReportGenBut = new Button();
            this.e911ReportDirBrowser = new Button();
            this.updateTestLogPathLabel = new Label();
            this.reportDirVal = new TextBox();
            this.LSMReportLatIndexTextBox = new TextBox();
            this.LSMReportCNoIndexTextBox = new TextBox();
            this.latIndexLabel = new Label();
            this.cnoIndexLabel = new Label();
            this.LSMLonIndexTextBox = new TextBox();
            this.lonIndexLabel = new Label();
            this.LSMAltIndexTextBox = new TextBox();
            this.AltIndexLabel = new Label();
            this.LSMReportTTFFIndexTextBox = new TextBox();
            this.ttffIndexLabel = new Label();
            this.frmRxInitCmdRefLocationGrpBox.SuspendLayout();
            base.SuspendLayout();
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmLSMReportSetAsDefaultBtn);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmLSMReportRefLatitudeTextBox);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmLSMReportRefLatitudeLabel);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmLSMReportRefLongitudeTextBox);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmLSMReportLongitudeLabel);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmLSMReportRefAltitudeTextBox);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmLSMReportRefAltitudeLabel);
            this.frmRxInitCmdRefLocationGrpBox.Controls.Add(this.frmLSMRefLocationComboBox);
            this.frmRxInitCmdRefLocationGrpBox.Location = new Point(0x19, 0x17);
            this.frmRxInitCmdRefLocationGrpBox.Name = "frmRxInitCmdRefLocationGrpBox";
            this.frmRxInitCmdRefLocationGrpBox.Size = new Size(0x199, 0x62);
            this.frmRxInitCmdRefLocationGrpBox.TabIndex = 0x13;
            this.frmRxInitCmdRefLocationGrpBox.TabStop = false;
            this.frmRxInitCmdRefLocationGrpBox.Text = "Reference Location";
            this.frmLSMReportSetAsDefaultBtn.Location = new Point(0x19, 0x36);
            this.frmLSMReportSetAsDefaultBtn.Name = "frmLSMReportSetAsDefaultBtn";
            this.frmLSMReportSetAsDefaultBtn.Size = new Size(0x77, 0x17);
            this.frmLSMReportSetAsDefaultBtn.TabIndex = 0x11;
            this.frmLSMReportSetAsDefaultBtn.Text = "Set as Default";
            this.frmLSMReportSetAsDefaultBtn.UseVisualStyleBackColor = true;
            this.frmLSMReportRefLatitudeTextBox.Location = new Point(0x126, 20);
            this.frmLSMReportRefLatitudeTextBox.Name = "frmLSMReportRefLatitudeTextBox";
            this.frmLSMReportRefLatitudeTextBox.Size = new Size(0x44, 20);
            this.frmLSMReportRefLatitudeTextBox.TabIndex = 10;
            this.frmLSMReportRefLatitudeTextBox.Text = "-2682834";
            this.frmLSMReportRefLatitudeLabel.AutoSize = true;
            this.frmLSMReportRefLatitudeLabel.Location = new Point(0xdd, 0x18);
            this.frmLSMReportRefLatitudeLabel.Name = "frmLSMReportRefLatitudeLabel";
            this.frmLSMReportRefLatitudeLabel.Size = new Size(0x30, 13);
            this.frmLSMReportRefLatitudeLabel.TabIndex = 11;
            this.frmLSMReportRefLatitudeLabel.Text = "Latitude:";
            this.frmLSMReportRefLongitudeTextBox.Location = new Point(0x126, 40);
            this.frmLSMReportRefLongitudeTextBox.Name = "frmLSMReportRefLongitudeTextBox";
            this.frmLSMReportRefLongitudeTextBox.Size = new Size(0x44, 20);
            this.frmLSMReportRefLongitudeTextBox.TabIndex = 12;
            this.frmLSMReportRefLongitudeTextBox.Text = "-4307681";
            this.frmLSMReportLongitudeLabel.AutoSize = true;
            this.frmLSMReportLongitudeLabel.Location = new Point(0xdd, 0x2c);
            this.frmLSMReportLongitudeLabel.Name = "frmLSMReportLongitudeLabel";
            this.frmLSMReportLongitudeLabel.Size = new Size(0x39, 13);
            this.frmLSMReportLongitudeLabel.TabIndex = 13;
            this.frmLSMReportLongitudeLabel.Text = "Longitude:";
            this.frmLSMReportRefAltitudeTextBox.Location = new Point(0x126, 60);
            this.frmLSMReportRefAltitudeTextBox.Name = "frmLSMReportRefAltitudeTextBox";
            this.frmLSMReportRefAltitudeTextBox.Size = new Size(0x44, 20);
            this.frmLSMReportRefAltitudeTextBox.TabIndex = 14;
            this.frmLSMReportRefAltitudeTextBox.Text = "3850571";
            this.frmLSMReportRefAltitudeLabel.AutoSize = true;
            this.frmLSMReportRefAltitudeLabel.Location = new Point(0xdd, 0x40);
            this.frmLSMReportRefAltitudeLabel.Name = "frmLSMReportRefAltitudeLabel";
            this.frmLSMReportRefAltitudeLabel.Size = new Size(0x2d, 13);
            this.frmLSMReportRefAltitudeLabel.TabIndex = 15;
            this.frmLSMReportRefAltitudeLabel.Text = "Altitude:";
            this.frmLSMRefLocationComboBox.FormattingEnabled = true;
            this.frmLSMRefLocationComboBox.Location = new Point(0x19, 20);
            this.frmLSMRefLocationComboBox.Name = "frmLSMRefLocationComboBox";
            this.frmLSMRefLocationComboBox.Size = new Size(0x79, 0x15);
            this.frmLSMRefLocationComboBox.TabIndex = 0;
            this.frmLSMRefLocationComboBox.SelectedIndexChanged += new EventHandler(this.frmLSMRefLocationComboBox_SelectedIndexChanged);
            this.LSMReportHrErrLimLabel.AutoSize = true;
            this.LSMReportHrErrLimLabel.Location = new Point(0x1a, 0xf2);
            this.LSMReportHrErrLimLabel.Name = "LSMReportHrErrLimLabel";
            this.LSMReportHrErrLimLabel.Size = new Size(0x42, 13);
            this.LSMReportHrErrLimLabel.TabIndex = 30;
            this.LSMReportHrErrLimLabel.Text = "Hrz Err Limit:";
            this.LSMReportTTFFLimitLabel.AutoSize = true;
            this.LSMReportTTFFLimitLabel.Location = new Point(300, 0xde);
            this.LSMReportTTFFLimitLabel.Name = "LSMReportTTFFLimitLabel";
            this.LSMReportTTFFLimitLabel.Size = new Size(90, 13);
            this.LSMReportTTFFLimitLabel.TabIndex = 0x20;
            this.LSMReportTTFFLimitLabel.Text = "TTFF Spec (sec):";
            this.LSMReportTimeoutLabel.AutoSize = true;
            this.LSMReportTimeoutLabel.Location = new Point(300, 0xca);
            this.LSMReportTimeoutLabel.Name = "LSMReportTimeoutLabel";
            this.LSMReportTimeoutLabel.Size = new Size(0x4a, 13);
            this.LSMReportTimeoutLabel.TabIndex = 0x1d;
            this.LSMReportTimeoutLabel.Text = "Timeout (sec):";
            this.LSMReportPerLabel.AutoSize = true;
            this.LSMReportPerLabel.Location = new Point(0x1a, 0xc9);
            this.LSMReportPerLabel.Name = "LSMReportPerLabel";
            this.LSMReportPerLabel.Size = new Size(0x5c, 13);
            this.LSMReportPerLabel.TabIndex = 0x17;
            this.LSMReportPerLabel.Text = "Report Percentile:";
            this.LSMReportTTFFLimLabel.AutoSize = true;
            this.LSMReportTTFFLimLabel.Location = new Point(0x1a, 0xde);
            this.LSMReportTTFFLimLabel.Name = "LSMReportTTFFLimLabel";
            this.LSMReportTTFFLimLabel.Size = new Size(60, 13);
            this.LSMReportTTFFLimLabel.TabIndex = 0x1a;
            this.LSMReportTTFFLimLabel.Text = "TTFF Limit:";
            this.LSMReportHrErrPerVal.Location = new Point(0x81, 0xee);
            this.LSMReportHrErrPerVal.Name = "LSMReportHrErrPerVal";
            this.LSMReportHrErrPerVal.Size = new Size(0x80, 20);
            this.LSMReportHrErrPerVal.TabIndex = 0x1f;
            this.LSMReportHrErrPerVal.Text = "50,150";
            this.LSMReportTTFFLimitVal.Location = new Point(0x195, 0xda);
            this.LSMReportTTFFLimitVal.Name = "LSMReportTTFFLimitVal";
            this.LSMReportTTFFLimitVal.Size = new Size(0x1b, 20);
            this.LSMReportTTFFLimitVal.TabIndex = 0x21;
            this.LSMReportTTFFLimitVal.Text = "20";
            this.LSMReportTimeoutVal.Location = new Point(0x195, 0xc6);
            this.LSMReportTimeoutVal.Name = "LSMReportTimeoutVal";
            this.LSMReportTimeoutVal.Size = new Size(0x1b, 20);
            this.LSMReportTimeoutVal.TabIndex = 0x1c;
            this.LSMReportTimeoutVal.Text = "31";
            this.LSMReportPerVal.Location = new Point(0x81, 0xc6);
            this.LSMReportPerVal.Name = "LSMReportPerVal";
            this.LSMReportPerVal.Size = new Size(0x80, 20);
            this.LSMReportPerVal.TabIndex = 0x18;
            this.LSMReportPerVal.Text = "50,95";
            this.LSMReportTTFFPerVal.Location = new Point(0x81, 0xda);
            this.LSMReportTTFFPerVal.Name = "LSMReportTTFFPerVal";
            this.LSMReportTTFFPerVal.Size = new Size(0x80, 20);
            this.LSMReportTTFFPerVal.TabIndex = 0x1b;
            this.LSMReportTTFFPerVal.Text = "20,20";
            this.e911ReportCancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.e911ReportCancelBut.Location = new Point(0x11b, 0x161);
            this.e911ReportCancelBut.Name = "e911ReportCancelBut";
            this.e911ReportCancelBut.Size = new Size(0x4b, 0x17);
            this.e911ReportCancelBut.TabIndex = 0x23;
            this.e911ReportCancelBut.Text = "&Cancel";
            this.e911ReportCancelBut.UseVisualStyleBackColor = true;
            this.e911ReportCancelBut.Click += new EventHandler(this.LSMReportCancelBut_Click);
            this.e911ReportGenBut.Cursor = Cursors.Default;
            this.e911ReportGenBut.Location = new Point(0xb6, 0x161);
            this.e911ReportGenBut.Name = "e911ReportGenBut";
            this.e911ReportGenBut.Size = new Size(0x4b, 0x17);
            this.e911ReportGenBut.TabIndex = 0x22;
            this.e911ReportGenBut.Text = "&Generate";
            this.e911ReportGenBut.UseVisualStyleBackColor = true;
            this.e911ReportGenBut.Click += new EventHandler(this.LSMReportGenBut_Click);
            this.e911ReportDirBrowser.Location = new Point(0x1c5, 0x9a);
            this.e911ReportDirBrowser.Name = "e911ReportDirBrowser";
            this.e911ReportDirBrowser.Size = new Size(0x1a, 0x17);
            this.e911ReportDirBrowser.TabIndex = 0x16;
            this.e911ReportDirBrowser.Text = "...";
            this.e911ReportDirBrowser.UseVisualStyleBackColor = true;
            this.e911ReportDirBrowser.Click += new EventHandler(this.LSMReportDirBrowser_Click);
            this.updateTestLogPathLabel.AutoSize = true;
            this.updateTestLogPathLabel.Location = new Point(0x1a, 0x87);
            this.updateTestLogPathLabel.Name = "updateTestLogPathLabel";
            this.updateTestLogPathLabel.Size = new Size(0x5e, 13);
            this.updateTestLogPathLabel.TabIndex = 20;
            this.updateTestLogPathLabel.Text = "Test Log Directory";
            this.reportDirVal.Location = new Point(0x1d, 0x9c);
            this.reportDirVal.Name = "reportDirVal";
            this.reportDirVal.Size = new Size(0x197, 20);
            this.reportDirVal.TabIndex = 0x15;
            this.LSMReportLatIndexTextBox.Location = new Point(0x81, 0x11b);
            this.LSMReportLatIndexTextBox.Name = "LSMReportLatIndexTextBox";
            this.LSMReportLatIndexTextBox.Size = new Size(0x1b, 20);
            this.LSMReportLatIndexTextBox.TabIndex = 0x1c;
            this.LSMReportLatIndexTextBox.Text = "7";
            this.LSMReportCNoIndexTextBox.Location = new Point(230, 0x12f);
            this.LSMReportCNoIndexTextBox.Name = "LSMReportCNoIndexTextBox";
            this.LSMReportCNoIndexTextBox.Size = new Size(0x1b, 20);
            this.LSMReportCNoIndexTextBox.TabIndex = 0x21;
            this.LSMReportCNoIndexTextBox.Text = "17";
            this.latIndexLabel.AutoSize = true;
            this.latIndexLabel.Location = new Point(0x18, 0x11f);
            this.latIndexLabel.Name = "latIndexLabel";
            this.latIndexLabel.Size = new Size(0x39, 13);
            this.latIndexLabel.TabIndex = 0x1d;
            this.latIndexLabel.Text = "Lat. Index:";
            this.cnoIndexLabel.AutoSize = true;
            this.cnoIndexLabel.Location = new Point(0xa4, 0x134);
            this.cnoIndexLabel.Name = "cnoIndexLabel";
            this.cnoIndexLabel.Size = new Size(60, 13);
            this.cnoIndexLabel.TabIndex = 0x20;
            this.cnoIndexLabel.Text = "CNo Index:";
            this.LSMLonIndexTextBox.Location = new Point(0x81, 0x12f);
            this.LSMLonIndexTextBox.Name = "LSMLonIndexTextBox";
            this.LSMLonIndexTextBox.Size = new Size(0x1b, 20);
            this.LSMLonIndexTextBox.TabIndex = 0x21;
            this.LSMLonIndexTextBox.Text = "8";
            this.lonIndexLabel.AutoSize = true;
            this.lonIndexLabel.Location = new Point(0x18, 0x133);
            this.lonIndexLabel.Name = "lonIndexLabel";
            this.lonIndexLabel.Size = new Size(60, 13);
            this.lonIndexLabel.TabIndex = 0x20;
            this.lonIndexLabel.Text = "Lon. Index:";
            this.LSMAltIndexTextBox.Location = new Point(0x81, 0x143);
            this.LSMAltIndexTextBox.Name = "LSMAltIndexTextBox";
            this.LSMAltIndexTextBox.Size = new Size(0x1b, 20);
            this.LSMAltIndexTextBox.TabIndex = 0x21;
            this.LSMAltIndexTextBox.Text = "9";
            this.AltIndexLabel.AutoSize = true;
            this.AltIndexLabel.Location = new Point(0x18, 0x147);
            this.AltIndexLabel.Name = "AltIndexLabel";
            this.AltIndexLabel.Size = new Size(0x36, 13);
            this.AltIndexLabel.TabIndex = 0x20;
            this.AltIndexLabel.Text = "Alt. Index:";
            this.LSMReportTTFFIndexTextBox.Location = new Point(230, 0x11b);
            this.LSMReportTTFFIndexTextBox.Name = "LSMReportTTFFIndexTextBox";
            this.LSMReportTTFFIndexTextBox.Size = new Size(0x1b, 20);
            this.LSMReportTTFFIndexTextBox.TabIndex = 0x1c;
            this.LSMReportTTFFIndexTextBox.Text = "3";
            this.ttffIndexLabel.AutoSize = true;
            this.ttffIndexLabel.Location = new Point(0xa4, 0x120);
            this.ttffIndexLabel.Name = "ttffIndexLabel";
            this.ttffIndexLabel.Size = new Size(0x41, 13);
            this.ttffIndexLabel.TabIndex = 0x1d;
            this.ttffIndexLabel.Text = "TTFF Index:";
            base.AcceptButton = this.e911ReportGenBut;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.e911ReportCancelBut;
            base.ClientSize = new Size(0x1f9, 0x18f);
            base.Controls.Add(this.LSMReportHrErrLimLabel);
            base.Controls.Add(this.AltIndexLabel);
            base.Controls.Add(this.lonIndexLabel);
            base.Controls.Add(this.cnoIndexLabel);
            base.Controls.Add(this.LSMReportTTFFLimitLabel);
            base.Controls.Add(this.ttffIndexLabel);
            base.Controls.Add(this.latIndexLabel);
            base.Controls.Add(this.LSMReportTimeoutLabel);
            base.Controls.Add(this.LSMReportPerLabel);
            base.Controls.Add(this.LSMReportTTFFLimLabel);
            base.Controls.Add(this.LSMReportHrErrPerVal);
            base.Controls.Add(this.LSMAltIndexTextBox);
            base.Controls.Add(this.LSMLonIndexTextBox);
            base.Controls.Add(this.LSMReportCNoIndexTextBox);
            base.Controls.Add(this.LSMReportTTFFIndexTextBox);
            base.Controls.Add(this.LSMReportLatIndexTextBox);
            base.Controls.Add(this.LSMReportTTFFLimitVal);
            base.Controls.Add(this.LSMReportTimeoutVal);
            base.Controls.Add(this.LSMReportPerVal);
            base.Controls.Add(this.LSMReportTTFFPerVal);
            base.Controls.Add(this.e911ReportCancelBut);
            base.Controls.Add(this.e911ReportGenBut);
            base.Controls.Add(this.e911ReportDirBrowser);
            base.Controls.Add(this.updateTestLogPathLabel);
            base.Controls.Add(this.reportDirVal);
            base.Controls.Add(this.frmRxInitCmdRefLocationGrpBox);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmLSMTestReport";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "LSM Reset Report";
            base.Load += new EventHandler(this.frmLSMTestReport_Load);
            this.frmRxInitCmdRefLocationGrpBox.ResumeLayout(false);
            this.frmRxInitCmdRefLocationGrpBox.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LSMReportCancelBut_Click(object sender, EventArgs e)
        {
            base.Dispose();
            base.Close();
        }

        private void LSMReportDirBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.reportDirVal.Text = dialog.SelectedPath;
            }
        }

        private void LSMReportGenBut_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Report report = new Report();
            if (this.reportDirVal.Text.Length > 0)
            {
                report.Percentile = this.LSMReportPerVal.Text;
                report.TTFFLimit = this.LSMReportTTFFPerVal.Text;
                report.HrErrLimit = this.LSMReportHrErrPerVal.Text;
                report.TimeoutVal = this.LSMReportTimeoutVal.Text;
                report.LimitVal = this.LSMReportTTFFLimitVal.Text;
                double refLat = 0.0;
                double refLon = 0.0;
                double refAlt = 0.0;
                int ttffIdx = 0;
                int latIdx = 0;
                int lonIdx = 0;
                int altIdx = 0;
                int startCNoIdx = 0;
                try
                {
                    refLat = Convert.ToDouble(this.frmLSMReportRefLatitudeTextBox.Text);
                    refLon = Convert.ToDouble(this.frmLSMReportRefLongitudeTextBox.Text);
                    refAlt = Convert.ToDouble(this.frmLSMReportRefAltitudeTextBox.Text);
                    latIdx = Convert.ToInt32(this.LSMReportLatIndexTextBox.Text);
                    lonIdx = Convert.ToInt32(this.LSMLonIndexTextBox.Text);
                    altIdx = Convert.ToInt32(this.LSMAltIndexTextBox.Text);
                    ttffIdx = Convert.ToInt32(this.LSMReportTTFFIndexTextBox.Text);
                    startCNoIdx = Convert.ToInt32(this.LSMReportCNoIndexTextBox.Text);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    return;
                }
                report.Summary_LSM(this.reportDirVal.Text, refLat, refLon, refAlt, latIdx, lonIdx, altIdx, ttffIdx, startCNoIdx);
            }
            else
            {
                MessageBox.Show("No directory found", "Error");
            }
            this.Cursor = Cursors.Default;
        }

        private void updateDefautlReferenceLocationComboBox()
        {
            this._maxItemWidth = this._defaultWidth;
            if (this.frmLSMRefLocationComboBox.Items.Count != 0)
            {
                this.frmLSMRefLocationComboBox.Items.Clear();
            }
            ArrayList referenceLocationName = new ArrayList();
            referenceLocationName = this.nav_data.GetReferenceLocationName();
            for (int i = 0; i < referenceLocationName.Count; i++)
            {
                this.frmLSMRefLocationComboBox.Items.Add(referenceLocationName[i]);
                if (this._maxItemWidth < (referenceLocationName[i].ToString().Length * 6))
                {
                    this._maxItemWidth = referenceLocationName[i].ToString().Length * 6;
                }
            }
            this.frmLSMRefLocationComboBox.Items.Add("USER_DEFINED");
            if (this.nav_data.RefLocationName == "")
            {
                this.nav_data.RefLocationName = "Default";
            }
            this.frmLSMRefLocationComboBox.Text = this.nav_data.RefLocationName;
            this.updateReferencePositionTextBox();
        }

        private void updateReferencePositionTextBox()
        {
            PositionInLatLonAlt referencePosition = this.nav_data.GetReferencePosition(this.frmLSMRefLocationComboBox.Text);
            this.frmLSMReportRefLatitudeTextBox.Text = referencePosition.latitude.ToString();
            this.frmLSMReportRefLongitudeTextBox.Text = referencePosition.longitude.ToString();
            this.frmLSMReportRefAltitudeTextBox.Text = referencePosition.altitude.ToString();
        }
    }
}

