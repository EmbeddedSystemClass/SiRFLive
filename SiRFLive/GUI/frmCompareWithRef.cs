﻿namespace SiRFLive.GUI
{
    using SiRFLive.Reporting;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class frmCompareWithRef : Form
    {
        private double _endPoint;
        private double _startPoint;
        private Button BtnRefresh;
        private TextBox CompareDirVal;
        private Button CompareWithRefDirBrowser;
        private IContainer components;
        private Label endTOWLabel;
        private NumericUpDown endTowNumericBox;
        private ListBox fileAvailableFilesListVal;
        private string[] filesArray = new string[0];
        private List<string> filesFullPathLists = new List<string>();
        private ListBox fileToBeProcessedFilesListVal;
        private Label label1;
        private Label label2;
        private TextBox RefDirFileVal;
        private Button RefFileBrowser;
        private Button ReportAbortBtn;
        private Button ReportAddAllBtn;
        private Button ReportAddBtn;
        private Button ReportCancelBtn;
        private Button ReportRemoveAllBtn;
        private Button ReportRemoveBtn;
        private Button ReportRunBtn;
        private Label startTOWLabel;
        private NumericUpDown startTowNumericBox;
        private List<string> ToProcessList = new List<string>();

        public frmCompareWithRef()
        {
            this.InitializeComponent();
        }

        private void addAvailableFiles(string fileStr)
        {
            string[] strArray = new string[] { "" };
            int length = 0;
            this.filesFullPathLists.Add(fileStr);
            strArray = fileStr.Split(new char[] { '\\' });
            length = strArray.Length;
            this.fileAvailableFilesListVal.Items.Add(strArray[length - 1]);
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.updateAvailableFiles();
        }

        private void CompareWithRefDirBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = @"c:\";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.CompareDirVal.Text = dialog.SelectedPath;
                foreach (string str in Directory.GetFiles(dialog.SelectedPath))
                {
                    if (str.EndsWith(".gps"))
                    {
                        this.addAvailableFiles(str);
                    }
                }
            }
            this.filesArray = this.filesFullPathLists.ToArray();
        }

        private void conversionAbort()
        {
            MessageBox.Show("Abort conversion?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void endTowNumericBox_ValueChanged(object sender, EventArgs e)
        {
            this._endPoint = ((double) this.endTowNumericBox.Value) / 1000.0;
        }

        private void fileAvailableFilesListVal_DoubleClick(object sender, EventArgs e)
        {
            int selectedIndex = this.fileAvailableFilesListVal.SelectedIndex;
            if (selectedIndex >= 0)
            {
                this.fileToBeProcessedFilesListVal.Items.Add(this.fileAvailableFilesListVal.SelectedItem);
                this.ToProcessList.Add(this.filesArray[selectedIndex]);
            }
        }

        public void FileBrowser(TextBox anything, string initialDir)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "File Locator";
            dialog.InitialDirectory = initialDir;
            dialog.Filter = "All files (*.*)|*.*";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.SetTextBoxText(anything, dialog.FileName);
            }
        }

        private void fileToBeProcessedFilesListVal_DoubleClick(object sender, EventArgs e)
        {
            int selectedIndex = this.fileToBeProcessedFilesListVal.SelectedIndex;
            string[] strArray = this.ToProcessList.ToArray();
            if (selectedIndex >= 0)
            {
                this.fileToBeProcessedFilesListVal.Items.Remove(this.fileToBeProcessedFilesListVal.SelectedItem);
                this.ToProcessList.Remove(strArray[selectedIndex]);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmCompareWithRef));
            this.fileAvailableFilesListVal = new ListBox();
            this.fileToBeProcessedFilesListVal = new ListBox();
            this.CompareDirVal = new TextBox();
            this.label1 = new Label();
            this.RefDirFileVal = new TextBox();
            this.label2 = new Label();
            this.CompareWithRefDirBrowser = new Button();
            this.RefFileBrowser = new Button();
            this.ReportRunBtn = new Button();
            this.ReportAbortBtn = new Button();
            this.ReportCancelBtn = new Button();
            this.ReportAddBtn = new Button();
            this.ReportAddAllBtn = new Button();
            this.ReportRemoveBtn = new Button();
            this.ReportRemoveAllBtn = new Button();
            this.BtnRefresh = new Button();
            this.startTOWLabel = new Label();
            this.endTOWLabel = new Label();
            this.startTowNumericBox = new NumericUpDown();
            this.endTowNumericBox = new NumericUpDown();
            this.startTowNumericBox.BeginInit();
            this.endTowNumericBox.BeginInit();
            base.SuspendLayout();
            this.fileAvailableFilesListVal.FormattingEnabled = true;
            this.fileAvailableFilesListVal.HorizontalScrollbar = true;
            this.fileAvailableFilesListVal.Location = new Point(0x12, 0xa7);
            this.fileAvailableFilesListVal.Name = "fileAvailableFilesListVal";
            this.fileAvailableFilesListVal.ScrollAlwaysVisible = true;
            this.fileAvailableFilesListVal.Size = new Size(0x137, 0xee);
            this.fileAvailableFilesListVal.TabIndex = 0;
            this.fileAvailableFilesListVal.DoubleClick += new EventHandler(this.fileAvailableFilesListVal_DoubleClick);
            this.fileToBeProcessedFilesListVal.FormattingEnabled = true;
            this.fileToBeProcessedFilesListVal.HorizontalScrollbar = true;
            this.fileToBeProcessedFilesListVal.Location = new Point(0x1c8, 0xa8);
            this.fileToBeProcessedFilesListVal.Name = "fileToBeProcessedFilesListVal";
            this.fileToBeProcessedFilesListVal.ScrollAlwaysVisible = true;
            this.fileToBeProcessedFilesListVal.Size = new Size(0x138, 0xee);
            this.fileToBeProcessedFilesListVal.TabIndex = 1;
            this.fileToBeProcessedFilesListVal.DoubleClick += new EventHandler(this.fileToBeProcessedFilesListVal_DoubleClick);
            this.CompareDirVal.Location = new Point(0x15, 0x53);
            this.CompareDirVal.Name = "CompareDirVal";
            this.CompareDirVal.Size = new Size(0x2c2, 20);
            this.CompareDirVal.TabIndex = 2;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x12, 0x40);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x5f, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "File Load Directory";
            this.RefDirFileVal.Location = new Point(0x15, 0x22);
            this.RefDirFileVal.Name = "RefDirFileVal";
            this.RefDirFileVal.Size = new Size(0x2c2, 20);
            this.RefDirFileVal.TabIndex = 4;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4c, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Reference File";
            this.CompareWithRefDirBrowser.Location = new Point(0x2e7, 0x51);
            this.CompareWithRefDirBrowser.Name = "CompareWithRefDirBrowser";
            this.CompareWithRefDirBrowser.Size = new Size(0x19, 0x18);
            this.CompareWithRefDirBrowser.TabIndex = 6;
            this.CompareWithRefDirBrowser.Text = "...";
            this.CompareWithRefDirBrowser.UseVisualStyleBackColor = true;
            this.CompareWithRefDirBrowser.Click += new EventHandler(this.CompareWithRefDirBrowser_Click);
            this.RefFileBrowser.Location = new Point(0x2e7, 0x20);
            this.RefFileBrowser.Name = "RefFileBrowser";
            this.RefFileBrowser.Size = new Size(0x19, 0x18);
            this.RefFileBrowser.TabIndex = 7;
            this.RefFileBrowser.Text = "...";
            this.RefFileBrowser.UseVisualStyleBackColor = true;
            this.RefFileBrowser.Click += new EventHandler(this.RefFileBrowser_Click);
            this.ReportRunBtn.Location = new Point(0xdb, 0x1a6);
            this.ReportRunBtn.Name = "ReportRunBtn";
            this.ReportRunBtn.Size = new Size(0x52, 0x1a);
            this.ReportRunBtn.TabIndex = 8;
            this.ReportRunBtn.Text = "&Start";
            this.ReportRunBtn.UseVisualStyleBackColor = true;
            this.ReportRunBtn.Click += new EventHandler(this.ReportRunBtn_Click);
            this.ReportAbortBtn.Location = new Point(0x165, 0x1a6);
            this.ReportAbortBtn.Name = "ReportAbortBtn";
            this.ReportAbortBtn.Size = new Size(0x51, 0x1b);
            this.ReportAbortBtn.TabIndex = 9;
            this.ReportAbortBtn.Text = "A&bort";
            this.ReportAbortBtn.UseVisualStyleBackColor = true;
            this.ReportAbortBtn.Click += new EventHandler(this.ReportAbortBtn_Click);
            this.ReportCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ReportCancelBtn.Location = new Point(0x1ee, 0x1a6);
            this.ReportCancelBtn.Name = "ReportCancelBtn";
            this.ReportCancelBtn.Size = new Size(0x51, 0x1c);
            this.ReportCancelBtn.TabIndex = 10;
            this.ReportCancelBtn.Text = "&Cancel";
            this.ReportCancelBtn.UseVisualStyleBackColor = true;
            this.ReportCancelBtn.Click += new EventHandler(this.ReportCancelBtn_Click);
            this.ReportAddBtn.Location = new Point(0x15a, 0xe5);
            this.ReportAddBtn.Name = "ReportAddBtn";
            this.ReportAddBtn.Size = new Size(0x5d, 0x1b);
            this.ReportAddBtn.TabIndex = 11;
            this.ReportAddBtn.Text = "&Add >";
            this.ReportAddBtn.UseVisualStyleBackColor = true;
            this.ReportAddBtn.Click += new EventHandler(this.ReportAddBtn_Click);
            this.ReportAddAllBtn.Location = new Point(0x15a, 0x100);
            this.ReportAddAllBtn.Name = "ReportAddAllBtn";
            this.ReportAddAllBtn.Size = new Size(0x5d, 0x1a);
            this.ReportAddAllBtn.TabIndex = 12;
            this.ReportAddAllBtn.Text = "Add A&ll >>";
            this.ReportAddAllBtn.UseVisualStyleBackColor = true;
            this.ReportAddAllBtn.Click += new EventHandler(this.ReportAddAllBtn_Click);
            this.ReportRemoveBtn.Location = new Point(0x15a, 0x11a);
            this.ReportRemoveBtn.Name = "ReportRemoveBtn";
            this.ReportRemoveBtn.Size = new Size(0x5d, 0x1b);
            this.ReportRemoveBtn.TabIndex = 13;
            this.ReportRemoveBtn.Text = "< &Re&move";
            this.ReportRemoveBtn.UseVisualStyleBackColor = true;
            this.ReportRemoveBtn.Click += new EventHandler(this.ReportRemoveBtn_Click);
            this.ReportRemoveAllBtn.Location = new Point(0x15a, 0x135);
            this.ReportRemoveAllBtn.Name = "ReportRemoveAllBtn";
            this.ReportRemoveAllBtn.Size = new Size(0x5d, 0x19);
            this.ReportRemoveAllBtn.TabIndex = 14;
            this.ReportRemoveAllBtn.Text = "<< Remo&ve All";
            this.ReportRemoveAllBtn.UseVisualStyleBackColor = true;
            this.ReportRemoveAllBtn.Click += new EventHandler(this.ReportRemoveAllBtn_Click);
            this.BtnRefresh.Location = new Point(0x109, 0x7b);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new Size(0x42, 0x17);
            this.BtnRefresh.TabIndex = 15;
            this.BtnRefresh.Text = "&Refresh";
            this.BtnRefresh.UseVisualStyleBackColor = true;
            this.BtnRefresh.Click += new EventHandler(this.BtnRefresh_Click);
            this.startTOWLabel.AutoSize = true;
            this.startTOWLabel.Location = new Point(0x12, 0x77);
            this.startTOWLabel.Name = "startTOWLabel";
            this.startTOWLabel.Size = new Size(80, 13);
            this.startTOWLabel.TabIndex = 0x10;
            this.startTOWLabel.Text = "Start TOW(ms):";
            this.endTOWLabel.AutoSize = true;
            this.endTOWLabel.Location = new Point(0x12, 0x8b);
            this.endTOWLabel.Name = "endTOWLabel";
            this.endTOWLabel.Size = new Size(0x4d, 13);
            this.endTOWLabel.TabIndex = 0x10;
            this.endTOWLabel.Text = "End TOW(ms):";
            this.startTowNumericBox.Location = new Point(0x6c, 0x73);
            int[] bits = new int[4];
            bits[1] = 1;
            this.startTowNumericBox.Maximum = new decimal(bits);
            this.startTowNumericBox.Name = "startTowNumericBox";
            this.startTowNumericBox.Size = new Size(80, 20);
            this.startTowNumericBox.TabIndex = 0x11;
            this.startTowNumericBox.ValueChanged += new EventHandler(this.startTowNumericBox_ValueChanged);
            this.endTowNumericBox.Location = new Point(0x6c, 0x87);
            int[] numArray2 = new int[4];
            numArray2[1] = 1;
            this.endTowNumericBox.Maximum = new decimal(numArray2);
            this.endTowNumericBox.Name = "endTowNumericBox";
            this.endTowNumericBox.Size = new Size(80, 20);
            this.endTowNumericBox.TabIndex = 0x11;
            this.endTowNumericBox.ValueChanged += new EventHandler(this.endTowNumericBox_ValueChanged);
            base.AcceptButton = this.ReportRunBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.ReportCancelBtn;
            base.ClientSize = new Size(0x31a, 470);
            base.Controls.Add(this.endTowNumericBox);
            base.Controls.Add(this.startTowNumericBox);
            base.Controls.Add(this.endTOWLabel);
            base.Controls.Add(this.startTOWLabel);
            base.Controls.Add(this.BtnRefresh);
            base.Controls.Add(this.ReportRemoveAllBtn);
            base.Controls.Add(this.ReportRemoveBtn);
            base.Controls.Add(this.ReportAddAllBtn);
            base.Controls.Add(this.ReportAddBtn);
            base.Controls.Add(this.ReportCancelBtn);
            base.Controls.Add(this.ReportAbortBtn);
            base.Controls.Add(this.ReportRunBtn);
            base.Controls.Add(this.RefFileBrowser);
            base.Controls.Add(this.CompareWithRefDirBrowser);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.RefDirFileVal);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.CompareDirVal);
            base.Controls.Add(this.fileToBeProcessedFilesListVal);
            base.Controls.Add(this.fileAvailableFilesListVal);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmCompareWithRef";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Point To Point Analysis";
            this.startTowNumericBox.EndInit();
            this.endTowNumericBox.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void ProcessFiles()
        {
            Report report = new Report();
			base.Invoke((MethodInvoker)delegate
			{
                this.Cursor = Cursors.WaitCursor;
            });
            foreach (string str in this.ToProcessList)
            {
                report.PointToPointErrorCalculation(str, this.RefDirFileVal.Text, this._startPoint, this._endPoint);
            }
            report.Summary_CompareWithRef(this.CompareDirVal.Text);
			base.Invoke((MethodInvoker)delegate
			{
                this.Cursor = Cursors.Default;
            });
        }

        private void RefFileBrowser_Click(object sender, EventArgs e)
        {
            this.FileBrowser(this.RefDirFileVal, "Select Ref .gps file...");
        }

        private void ReportAbortBtn_Click(object sender, EventArgs e)
        {
            this.conversionAbort();
        }

        private void ReportAddAllBtn_Click(object sender, EventArgs e)
        {
            this.fileToBeProcessedFilesListVal.Items.Clear();
            this.ToProcessList.Clear();
            foreach (string str in this.fileAvailableFilesListVal.Items)
            {
                this.fileToBeProcessedFilesListVal.Items.Add(str);
            }
            foreach (string str2 in this.filesFullPathLists)
            {
                this.ToProcessList.Add(str2);
            }
        }

        private void ReportAddBtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.fileAvailableFilesListVal.SelectedIndex;
            if (selectedIndex >= 0)
            {
                this.fileToBeProcessedFilesListVal.Items.Add(this.fileAvailableFilesListVal.SelectedItem);
                this.ToProcessList.Add(this.filesArray[selectedIndex]);
            }
        }

        private void ReportCancelBtn_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void ReportRemoveAllBtn_Click(object sender, EventArgs e)
        {
            this.fileToBeProcessedFilesListVal.Items.Clear();
            this.ToProcessList.Clear();
        }

        private void ReportRemoveBtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.fileToBeProcessedFilesListVal.SelectedIndex;
            string[] strArray = this.ToProcessList.ToArray();
            if (selectedIndex >= 0)
            {
                this.fileToBeProcessedFilesListVal.Items.Remove(this.fileToBeProcessedFilesListVal.SelectedItem);
                this.ToProcessList.Remove(strArray[selectedIndex]);
            }
        }

        private void ReportRunBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this._startPoint = ((double) this.startTowNumericBox.Value) / 1000.0;
                this._endPoint = ((double) this.endTowNumericBox.Value) / 1000.0;
                if (((this._startPoint > 0.0) && (this._endPoint > 0.0)) && (this._startPoint > this._endPoint))
                {
                    MessageBox.Show("Start Point can't be larger than end point!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    Thread.Sleep(100);
                    Thread thread = new Thread(new ThreadStart(this.ProcessFiles));
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error: " + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public void SetTextBoxText(TextBox anything, string text)
        {
			anything.BeginInvoke((MethodInvoker)delegate
			{
                anything.Text = text;
            });
        }

        private void startTowNumericBox_ValueChanged(object sender, EventArgs e)
        {
            this._startPoint = ((double) this.startTowNumericBox.Value) / 1000.0;
        }

        private void updateAvailableFiles()
        {
            this.fileAvailableFilesListVal.Items.Clear();
            this.filesFullPathLists.Clear();
            try
            {
                foreach (string str in Directory.GetFiles(this.CompareDirVal.Text))
                {
                    if (str.EndsWith(".gps"))
                    {
                        this.addAvailableFiles(str);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error!");
            }
            this.filesArray = this.filesFullPathLists.ToArray();
        }
    }
}

