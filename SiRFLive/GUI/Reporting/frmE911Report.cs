﻿namespace SiRFLive.GUI.Reporting
{
    using SiRFLive.Reporting;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmE911Report : Form
    {
        private string _testName = string.Empty;
        private IContainer components;
        private ComboBox e911ReporGroupByComBox;
        private Button e911ReportCancelBut;
        private Button e911ReportDirBrowser;
        private TextBox e911ReportDirVal;
        private Button e911ReportGenBut;
        private Label e911ReportHrErrLimLabel;
        private TextBox e911ReportHrErrPerVal;
        private Label e911ReportPerLabel;
        private TextBox e911ReportPerVal;
        private Label e911ReportTimeoutLabel;
        private TextBox e911ReportTimeoutVal;
        private Label e911ReportTTFFLimitLabel;
        private TextBox e911ReportTTFFLimitVal;
        private Label e911ReportTTFFLimLabel;
        private TextBox e911ReportTTFFPerVal;
        private ComboBox e911ReportTTFFTypeComBox;
        private Label label1;
        private Label label2;
        private static frmE911Report m_SChildform;
        private Label updateTestLogPathLabel;

        public frmE911Report(string testName)
        {
            this.InitializeComponent();
            this._testName = testName;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void e911ReportCancelBut_Click(object sender, EventArgs e)
        {
            base.Close();
            m_SChildform = null;
        }

        private void e911ReportDirBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.e911ReportDirVal.Text = dialog.SelectedPath;
            }
        }

        private void e911ReportGenBut_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Report report = new Report();
            if (this.e911ReportDirVal.Text.Length <= 0)
            {
                MessageBox.Show("No directory found", "Error");
            }
            else
            {
                report.Percentile = this.e911ReportPerVal.Text;
                report.TTFFLimit = this.e911ReportTTFFPerVal.Text;
                report.HrErrLimit = this.e911ReportHrErrPerVal.Text;
                report.TimeoutVal = this.e911ReportTimeoutVal.Text;
                report.LimitVal = this.e911ReportTTFFLimitVal.Text;
                report.TTFFReportType = this.e911ReportTTFFTypeComBox.SelectedIndex + 4;
                string str = this._testName;
                if (str != null)
                {
                    if (!(str == "E911"))
                    {
                        if (str == "3GPP")
                        {
                            report.TTFFReportType = 3;
                            report.Summary_3GPP(this.e911ReportDirVal.Text);
                        }
                        else if (str == "TIA916")
                        {
                            report.TTFFReportType = 3;
                            report.Summary_TIA916(this.e911ReportDirVal.Text);
                        }
                        else if (str == "Reset")
                        {
                            report.ReportLayout = (Report.ReportLayoutType) this.e911ReporGroupByComBox.SelectedIndex;
                            report.Summary_Reset_V2(this.e911ReportDirVal.Text);
                        }
                    }
                    else
                    {
                        report.TTFFReportType = 3;
                        report.E911_Summary(this.e911ReportDirVal.Text);
                    }
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void frmE911Report_Load(object sender, EventArgs e)
        {
            this.e911ReportTTFFTypeComBox.SelectedIndex = 1;
            this.e911ReporGroupByComBox.SelectedIndex = 0;
            switch (this._testName)
            {
                case "E911":
                    this.Text = "E911 Report";
                    this.e911ReportPerVal.Text = "50,95";
                    this.e911ReportTTFFPerVal.Text = "20,20";
                    this.e911ReportHrErrPerVal.Text = "50,150";
                    this.e911ReportTimeoutVal.Text = "26";
                    this.e911ReportTTFFLimitVal.Text = "20";
                    this.e911ReportTTFFTypeComBox.Enabled = false;
                    this.e911ReporGroupByComBox.Enabled = false;
                    return;

                case "3GPP":
                    this.Text = "3GPP Report";
                    this.e911ReportPerVal.Text = "95";
                    this.e911ReportTTFFPerVal.Text = "20";
                    this.e911ReportHrErrPerVal.Text = "100";
                    this.e911ReportTimeoutVal.Text = "20";
                    this.e911ReportTTFFLimitVal.Text = "20";
                    this.e911ReportTTFFTypeComBox.Enabled = false;
                    this.e911ReporGroupByComBox.Enabled = false;
                    return;

                case "TIA916":
                    this.Text = "TIA916 Report";
                    this.e911ReportPerVal.Text = "67,95";
                    this.e911ReportTTFFPerVal.Text = "16,16";
                    this.e911ReportHrErrPerVal.Text = "60,180";
                    this.e911ReportTimeoutVal.Text = "31";
                    this.e911ReportTTFFLimitVal.Text = "16";
                    this.e911ReportTTFFTypeComBox.Enabled = false;
                    this.e911ReporGroupByComBox.Enabled = false;
                    return;

                case "Reset":
                    this.Text = "Reset Report";
                    this.e911ReportPerVal.Text = "50,95";
                    this.e911ReportTTFFPerVal.Text = "180,180";
                    this.e911ReportHrErrPerVal.Text = "50,150";
                    this.e911ReportTimeoutVal.Text = "180";
                    this.e911ReportTTFFLimitVal.Text = "180";
                    this.e911ReportTTFFTypeComBox.Enabled = true;
                    this.e911ReporGroupByComBox.Enabled = true;
                    return;
            }
            this.Text = "E911 Report";
            this.e911ReportPerVal.Text = "50,95";
            this.e911ReportTTFFPerVal.Text = "20,20";
            this.e911ReportHrErrPerVal.Text = "50,150";
            this.e911ReportTimeoutVal.Text = "26";
            this.e911ReportTTFFLimitVal.Text = "20";
            this.e911ReportTTFFTypeComBox.Enabled = true;
            this.e911ReporGroupByComBox.Enabled = true;
        }

        public static frmE911Report GetChildInstance(string test)
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmE911Report(test);
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmE911Report));
            this.e911ReportCancelBut = new Button();
            this.e911ReportGenBut = new Button();
            this.e911ReportDirBrowser = new Button();
            this.updateTestLogPathLabel = new Label();
            this.e911ReportDirVal = new TextBox();
            this.e911ReportTTFFPerVal = new TextBox();
            this.e911ReportTTFFLimLabel = new Label();
            this.e911ReportHrErrPerVal = new TextBox();
            this.e911ReportHrErrLimLabel = new Label();
            this.e911ReportTimeoutVal = new TextBox();
            this.e911ReportTimeoutLabel = new Label();
            this.e911ReportTTFFLimitVal = new TextBox();
            this.e911ReportTTFFLimitLabel = new Label();
            this.e911ReportPerVal = new TextBox();
            this.e911ReportPerLabel = new Label();
            this.e911ReportTTFFTypeComBox = new ComboBox();
            this.label1 = new Label();
            this.e911ReporGroupByComBox = new ComboBox();
            this.label2 = new Label();
            base.SuspendLayout();
            this.e911ReportCancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.e911ReportCancelBut.Location = new Point(0x107, 0xb2);
            this.e911ReportCancelBut.Name = "e911ReportCancelBut";
            this.e911ReportCancelBut.Size = new Size(0x4b, 0x17);
            this.e911ReportCancelBut.TabIndex = 15;
            this.e911ReportCancelBut.Text = "&Cancel";
            this.e911ReportCancelBut.UseVisualStyleBackColor = true;
            this.e911ReportCancelBut.Click += new EventHandler(this.e911ReportCancelBut_Click);
            this.e911ReportGenBut.Cursor = Cursors.Default;
            this.e911ReportGenBut.Location = new Point(0xab, 0xb2);
            this.e911ReportGenBut.Name = "e911ReportGenBut";
            this.e911ReportGenBut.Size = new Size(0x4b, 0x17);
            this.e911ReportGenBut.TabIndex = 14;
            this.e911ReportGenBut.Text = "&Generate";
            this.e911ReportGenBut.UseVisualStyleBackColor = true;
            this.e911ReportGenBut.Click += new EventHandler(this.e911ReportGenBut_Click);
            this.e911ReportDirBrowser.Location = new Point(0x1c7, 0x23);
            this.e911ReportDirBrowser.Name = "e911ReportDirBrowser";
            this.e911ReportDirBrowser.Size = new Size(0x1a, 0x17);
            this.e911ReportDirBrowser.TabIndex = 2;
            this.e911ReportDirBrowser.Text = "...";
            this.e911ReportDirBrowser.UseVisualStyleBackColor = true;
            this.e911ReportDirBrowser.Click += new EventHandler(this.e911ReportDirBrowser_Click);
            this.updateTestLogPathLabel.AutoSize = true;
            this.updateTestLogPathLabel.Location = new Point(0x1c, 0x10);
            this.updateTestLogPathLabel.Name = "updateTestLogPathLabel";
            this.updateTestLogPathLabel.Size = new Size(0x5e, 13);
            this.updateTestLogPathLabel.TabIndex = 0;
            this.updateTestLogPathLabel.Text = "Test Log Directory";
            this.e911ReportDirVal.Location = new Point(0x1f, 0x25);
            this.e911ReportDirVal.Name = "e911ReportDirVal";
            this.e911ReportDirVal.Size = new Size(0x197, 20);
            this.e911ReportDirVal.TabIndex = 1;
            this.e911ReportTTFFPerVal.Location = new Point(0xa5, 0x63);
            this.e911ReportTTFFPerVal.Name = "e911ReportTTFFPerVal";
            this.e911ReportTTFFPerVal.Size = new Size(0x83, 20);
            this.e911ReportTTFFPerVal.TabIndex = 7;
            this.e911ReportTTFFPerVal.Text = "20,20";
            this.e911ReportTTFFLimLabel.AutoSize = true;
            this.e911ReportTTFFLimLabel.Location = new Point(0x1c, 0x67);
            this.e911ReportTTFFLimLabel.Name = "e911ReportTTFFLimLabel";
            this.e911ReportTTFFLimLabel.Size = new Size(60, 13);
            this.e911ReportTTFFLimLabel.TabIndex = 6;
            this.e911ReportTTFFLimLabel.Text = "TTFF Limit:";
            this.e911ReportHrErrPerVal.Location = new Point(0xa5, 0x77);
            this.e911ReportHrErrPerVal.Name = "e911ReportHrErrPerVal";
            this.e911ReportHrErrPerVal.Size = new Size(0x83, 20);
            this.e911ReportHrErrPerVal.TabIndex = 11;
            this.e911ReportHrErrPerVal.Text = "50,150";
            this.e911ReportHrErrLimLabel.AutoSize = true;
            this.e911ReportHrErrLimLabel.Location = new Point(0x1c, 0x7b);
            this.e911ReportHrErrLimLabel.Name = "e911ReportHrErrLimLabel";
            this.e911ReportHrErrLimLabel.Size = new Size(0x77, 13);
            this.e911ReportHrErrLimLabel.TabIndex = 10;
            this.e911ReportHrErrLimLabel.Text = "2-D Position  Error Limit:";
            this.e911ReportTimeoutVal.Location = new Point(440, 0x76);
            this.e911ReportTimeoutVal.Name = "e911ReportTimeoutVal";
            this.e911ReportTimeoutVal.Size = new Size(0x1b, 20);
            this.e911ReportTimeoutVal.TabIndex = 9;
            this.e911ReportTimeoutVal.Text = "31";
            this.e911ReportTimeoutLabel.AutoSize = true;
            this.e911ReportTimeoutLabel.Location = new Point(0x14d, 0x7a);
            this.e911ReportTimeoutLabel.Name = "e911ReportTimeoutLabel";
            this.e911ReportTimeoutLabel.Size = new Size(0x4a, 13);
            this.e911ReportTimeoutLabel.TabIndex = 9;
            this.e911ReportTimeoutLabel.Text = "Timeout: (sec)";
            this.e911ReportTTFFLimitVal.Location = new Point(440, 0x8a);
            this.e911ReportTTFFLimitVal.Name = "e911ReportTTFFLimitVal";
            this.e911ReportTTFFLimitVal.Size = new Size(0x1b, 20);
            this.e911ReportTTFFLimitVal.TabIndex = 13;
            this.e911ReportTTFFLimitVal.Text = "20";
            this.e911ReportTTFFLimitLabel.AutoSize = true;
            this.e911ReportTTFFLimitLabel.Location = new Point(0x14d, 0x8e);
            this.e911ReportTTFFLimitLabel.Name = "e911ReportTTFFLimitLabel";
            this.e911ReportTTFFLimitLabel.Size = new Size(90, 13);
            this.e911ReportTTFFLimitLabel.TabIndex = 12;
            this.e911ReportTTFFLimitLabel.Text = "TTFF Spec: (sec)";
            this.e911ReportPerVal.Location = new Point(0xa5, 0x4f);
            this.e911ReportPerVal.Name = "e911ReportPerVal";
            this.e911ReportPerVal.Size = new Size(0x83, 20);
            this.e911ReportPerVal.TabIndex = 4;
            this.e911ReportPerVal.Text = "50,95";
            this.e911ReportPerLabel.AutoSize = true;
            this.e911ReportPerLabel.Location = new Point(0x1c, 0x52);
            this.e911ReportPerLabel.Name = "e911ReportPerLabel";
            this.e911ReportPerLabel.Size = new Size(0x5c, 13);
            this.e911ReportPerLabel.TabIndex = 3;
            this.e911ReportPerLabel.Text = "Report Percentile:";
            this.e911ReportTTFFTypeComBox.FormattingEnabled = true;
            this.e911ReportTTFFTypeComBox.Items.AddRange(new object[] { "TTFF SiRFLive", "TTFF Since Reset", "TTFF Since Aided Recv", "TTFF Since First Nav" });
            this.e911ReportTTFFTypeComBox.Location = new Point(0xa5, 0x8b);
            this.e911ReportTTFFTypeComBox.Name = "e911ReportTTFFTypeComBox";
            this.e911ReportTTFFTypeComBox.Size = new Size(0x83, 0x15);
            this.e911ReportTTFFTypeComBox.TabIndex = 5;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x1c, 0x8f);
            this.label1.Name = "label1";
            this.label1.Size = new Size(100, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Report Reset Type:";
            this.e911ReporGroupByComBox.FormattingEnabled = true;
            this.e911ReporGroupByComBox.Items.AddRange(new object[] { "SW Version", "Reset Type" });
            this.e911ReporGroupByComBox.Location = new Point(0x189, 0x4e);
            this.e911ReporGroupByComBox.Name = "e911ReporGroupByComBox";
            this.e911ReporGroupByComBox.Size = new Size(0x59, 0x15);
            this.e911ReporGroupByComBox.TabIndex = 0x10;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x14d, 0x52);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x36, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Group By:";
            base.AcceptButton = this.e911ReportGenBut;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.CancelButton = this.e911ReportCancelBut;
            base.ClientSize = new Size(0x1fd, 0xdb);
            base.Controls.Add(this.e911ReporGroupByComBox);
            base.Controls.Add(this.e911ReportTTFFTypeComBox);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.e911ReportHrErrLimLabel);
            base.Controls.Add(this.e911ReportTTFFLimitLabel);
            base.Controls.Add(this.e911ReportTimeoutLabel);
            base.Controls.Add(this.e911ReportPerLabel);
            base.Controls.Add(this.e911ReportTTFFLimLabel);
            base.Controls.Add(this.e911ReportHrErrPerVal);
            base.Controls.Add(this.e911ReportTTFFLimitVal);
            base.Controls.Add(this.e911ReportTimeoutVal);
            base.Controls.Add(this.e911ReportPerVal);
            base.Controls.Add(this.e911ReportTTFFPerVal);
            base.Controls.Add(this.e911ReportCancelBut);
            base.Controls.Add(this.e911ReportGenBut);
            base.Controls.Add(this.e911ReportDirBrowser);
            base.Controls.Add(this.updateTestLogPathLabel);
            base.Controls.Add(this.e911ReportDirVal);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmE911Report";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "E911Report";
            base.Load += new EventHandler(this.frmE911Report_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            m_SChildform = null;
        }
    }
}

