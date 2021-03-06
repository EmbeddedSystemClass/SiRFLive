﻿namespace SiRFLive.GUI.Reporting
{
    using SiRFLive.Reporting;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmNavPerformanceReport : Form
    {
        private IContainer components;
        private static frmNavPerformanceReport m_SChildform;
        private Button reportCancelBut;
        private Button reportDirBrowser;
        private TextBox reportDirVal;
        private Button reportGenBut;
        private Label reportHrErrLimLabel;
        private TextBox reportInvSVVal;
        private TextBox reportNumHrErrval;
        private ComboBox reportSelectionCB;
        private Label reportSingleHrErrLabel;
        private TextBox reportSingleHrErrVal;
        private Label reportSvLimLabel;
        private Label reportTypeLabel;
        private Label reportUpdateTestLogPathLabel;

        public frmNavPerformanceReport()
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

        public static frmNavPerformanceReport GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmNavPerformanceReport();
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmNavPerformanceReport));
            this.reportDirVal = new TextBox();
            this.reportUpdateTestLogPathLabel = new Label();
            this.reportDirBrowser = new Button();
            this.reportHrErrLimLabel = new Label();
            this.reportSvLimLabel = new Label();
            this.reportNumHrErrval = new TextBox();
            this.reportInvSVVal = new TextBox();
            this.reportCancelBut = new Button();
            this.reportGenBut = new Button();
            this.reportSingleHrErrVal = new TextBox();
            this.reportSingleHrErrLabel = new Label();
            this.reportSelectionCB = new ComboBox();
            this.reportTypeLabel = new Label();
            base.SuspendLayout();
            this.reportDirVal.Location = new Point(0x22, 40);
            this.reportDirVal.Name = "reportDirVal";
            this.reportDirVal.Size = new Size(0x1b1, 20);
            this.reportDirVal.TabIndex = 1;
            this.reportUpdateTestLogPathLabel.AutoSize = true;
            this.reportUpdateTestLogPathLabel.Location = new Point(0x20, 0x13);
            this.reportUpdateTestLogPathLabel.Name = "reportUpdateTestLogPathLabel";
            this.reportUpdateTestLogPathLabel.Size = new Size(0x5e, 13);
            this.reportUpdateTestLogPathLabel.TabIndex = 0;
            this.reportUpdateTestLogPathLabel.Text = "Test Log Directory";
            this.reportDirBrowser.Location = new Point(0x1db, 0x27);
            this.reportDirBrowser.Name = "reportDirBrowser";
            this.reportDirBrowser.Size = new Size(0x1a, 0x17);
            this.reportDirBrowser.TabIndex = 2;
            this.reportDirBrowser.Text = ".&..";
            this.reportDirBrowser.UseVisualStyleBackColor = true;
            this.reportDirBrowser.Click += new EventHandler(this.reportDirBrowser_Click);
            this.reportHrErrLimLabel.AutoSize = true;
            this.reportHrErrLimLabel.Location = new Point(0x20, 0x63);
            this.reportHrErrLimLabel.Name = "reportHrErrLimLabel";
            this.reportHrErrLimLabel.Size = new Size(0x79, 13);
            this.reportHrErrLimLabel.TabIndex = 5;
            this.reportHrErrLimLabel.Text = "Total Bad Hrz Error Limit";
            this.reportSvLimLabel.AutoSize = true;
            this.reportSvLimLabel.Location = new Point(0x20, 0x4f);
            this.reportSvLimLabel.Name = "reportSvLimLabel";
            this.reportSvLimLabel.Size = new Size(0x6f, 13);
            this.reportSvLimLabel.TabIndex = 3;
            this.reportSvLimLabel.Text = "Total Invalid SVs Limit";
            this.reportNumHrErrval.Location = new Point(0xb7, 0x5f);
            this.reportNumHrErrval.Name = "reportNumHrErrval";
            this.reportNumHrErrval.Size = new Size(40, 20);
            this.reportNumHrErrval.TabIndex = 6;
            this.reportNumHrErrval.Text = "100";
            this.reportInvSVVal.Location = new Point(0xb7, 0x4b);
            this.reportInvSVVal.Name = "reportInvSVVal";
            this.reportInvSVVal.Size = new Size(40, 20);
            this.reportInvSVVal.TabIndex = 4;
            this.reportInvSVVal.Text = "20";
            this.reportCancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.reportCancelBut.Location = new Point(0x116, 0xbd);
            this.reportCancelBut.Name = "reportCancelBut";
            this.reportCancelBut.Size = new Size(0x4b, 0x17);
            this.reportCancelBut.TabIndex = 12;
            this.reportCancelBut.Text = "&Cancel";
            this.reportCancelBut.UseVisualStyleBackColor = true;
            this.reportCancelBut.Click += new EventHandler(this.reportCancelBut_Click);
            this.reportGenBut.Cursor = Cursors.Default;
            this.reportGenBut.Location = new Point(0xb7, 0xbd);
            this.reportGenBut.Name = "reportGenBut";
            this.reportGenBut.Size = new Size(0x4b, 0x17);
            this.reportGenBut.TabIndex = 11;
            this.reportGenBut.Text = "&Generate";
            this.reportGenBut.UseVisualStyleBackColor = true;
            this.reportGenBut.Click += new EventHandler(this.reportGenBut_Click);
            this.reportSingleHrErrVal.Location = new Point(0xb7, 0x73);
            this.reportSingleHrErrVal.Name = "reportSingleHrErrVal";
            this.reportSingleHrErrVal.Size = new Size(40, 20);
            this.reportSingleHrErrVal.TabIndex = 8;
            this.reportSingleHrErrVal.Text = "100";
            this.reportSingleHrErrLabel.AutoSize = true;
            this.reportSingleHrErrLabel.Location = new Point(0x20, 0x77);
            this.reportSingleHrErrLabel.Name = "reportSingleHrErrLabel";
            this.reportSingleHrErrLabel.Size = new Size(0x59, 13);
            this.reportSingleHrErrLabel.TabIndex = 7;
            this.reportSingleHrErrLabel.Text = "Hrz Error Limit (m)";
            this.reportSelectionCB.FormattingEnabled = true;
            this.reportSelectionCB.Items.AddRange(new object[] { "CW Report", "Spur Report" });
            this.reportSelectionCB.Location = new Point(0xb7, 0x87);
            this.reportSelectionCB.Name = "reportSelectionCB";
            this.reportSelectionCB.Size = new Size(0x79, 0x15);
            this.reportSelectionCB.TabIndex = 10;
            this.reportSelectionCB.Text = "CW Report";
            this.reportTypeLabel.AutoSize = true;
            this.reportTypeLabel.Location = new Point(0x20, 0x8b);
            this.reportTypeLabel.Name = "reportTypeLabel";
            this.reportTypeLabel.Size = new Size(0x42, 13);
            this.reportTypeLabel.TabIndex = 9;
            this.reportTypeLabel.Text = "Report Type";
            base.AcceptButton = this.reportGenBut;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.CancelButton = this.reportCancelBut;
            base.ClientSize = new Size(0x218, 0xe7);
            base.Controls.Add(this.reportTypeLabel);
            base.Controls.Add(this.reportSelectionCB);
            base.Controls.Add(this.reportSingleHrErrLabel);
            base.Controls.Add(this.reportHrErrLimLabel);
            base.Controls.Add(this.reportSvLimLabel);
            base.Controls.Add(this.reportSingleHrErrVal);
            base.Controls.Add(this.reportNumHrErrval);
            base.Controls.Add(this.reportInvSVVal);
            base.Controls.Add(this.reportCancelBut);
            base.Controls.Add(this.reportGenBut);
            base.Controls.Add(this.reportDirBrowser);
            base.Controls.Add(this.reportUpdateTestLogPathLabel);
            base.Controls.Add(this.reportDirVal);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmNavPerformanceReport";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Nav Performance Report";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            m_SChildform = null;
        }

        private void reportCancelBut_Click(object sender, EventArgs e)
        {
            base.Close();
            m_SChildform = null;
        }

        private void reportDirBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.reportDirVal.Text = dialog.SelectedPath;
            }
        }

        private void reportGenBut_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Report report = new Report();
            if (this.reportDirVal.Text.Length > 0)
            {
                report.HrErrLimit = this.reportSingleHrErrVal.Text;
                report.LimitVal = this.reportNumHrErrval.Text;
                report.InvalidSV = this.reportInvSVVal.Text;
                switch (this.reportSelectionCB.SelectedIndex)
                {
                    case 0:
                        report.CW_Summary(this.reportDirVal.Text);
                        break;

                    case 1:
                        report.Spur_Summary(this.reportDirVal.Text);
                        break;
                }
            }
            else
            {
                MessageBox.Show("No directory found", "Error");
            }
            this.Cursor = Cursors.Default;
        }
    }
}

