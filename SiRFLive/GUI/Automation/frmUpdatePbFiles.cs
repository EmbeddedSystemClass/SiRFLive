﻿namespace SiRFLive.GUI.Automation
{
    using SiRFLive.TestAutomation;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmUpdatePbFiles : Form
    {
        private IContainer components;
        private string[] filesArray;
        private List<string> filesFullPathList;
        private Label frmUpdatePbFilesCntLabel;
        private static frmUpdatePbFiles m_SChildform;
        internal string outputStr;
        internal e_configParams selectUpdateElmName;
        private List<string> toRunList;
        private Label updateAvailablePbFilesLabel;
        private Button updatePbAddAllBut;
        private Button updatePbAddBut;
        private ListBox updatePbAvailableFilesListVal;
        private Button updatePbClearAvailableFilesBut;
        private Button updatePbDirBrowser;
        private TextBox updatePbDirVal;
        private Button updatePbDoneBut;
        private Button updatePbExitBut;
        private Label updatePbFilePathLabel;
        private Button updatePbRemoveAllBut;
        private Button updatePbRemoveBut;
        private Label updatePbRunFilesLabel;
        private ListBox updatePbToRunFilesListVal;

        internal event updateParentEventHandler updateParent;

        internal frmUpdatePbFiles(e_configParams type)
        {
            this.outputStr = string.Empty;
            this.selectUpdateElmName = e_configParams.E_CONFIG_UNKNOWN;
            this.toRunList = new List<string>();
            this.filesFullPathList = new List<string>();
            this.filesArray = new string[0];
            this.InitializeComponent();
            this.selectUpdateElmName = type;
            this.toRunList.Clear();
            this.filesFullPathList.Clear();
            this.updatePbAvailableFilesListVal.Items.Clear();
            this.updatePbToRunFilesListVal.Items.Clear();
            switch (type)
            {
                case e_configParams.E_PLAYBACK_FILES:
                    this.updatePbFilePathLabel.Text = "Playback File Directory";
                    return;

                case e_configParams.E_LOG_DIR:
                case e_configParams.E_HOST_DIR:
                    break;

                case e_configParams.E_HOST_APP_NAME:
                    this.updatePbFilePathLabel.Text = "Host App Directory";
                    return;

                case e_configParams.E_PATCH_NAME:
                    this.updatePbFilePathLabel.Text = "Patch File Directory";
                    break;

                default:
                    return;
            }
        }

        internal frmUpdatePbFiles(e_configParams type, string inStr)
        {
            this.outputStr = string.Empty;
            this.selectUpdateElmName = e_configParams.E_CONFIG_UNKNOWN;
            this.toRunList = new List<string>();
            this.filesFullPathList = new List<string>();
            this.filesArray = new string[0];
            this.InitializeComponent();
            this.toRunList.Clear();
            this.filesFullPathList.Clear();
            this.updatePbAvailableFilesListVal.Items.Clear();
            this.updatePbToRunFilesListVal.Items.Clear();
            this.selectUpdateElmName = type;
            switch (type)
            {
                case e_configParams.E_PLAYBACK_FILES:
                    this.updatePbFilePathLabel.Text = "Playback File Directory";
                    break;

                case e_configParams.E_HOST_APP_NAME:
                    this.updatePbFilePathLabel.Text = "Host App Directory";
                    break;

                case e_configParams.E_PATCH_NAME:
                    this.updatePbFilePathLabel.Text = "Patch File Directory";
                    break;
            }
            string[] strArray = inStr.Split(new char[] { ',' });
            if (strArray.Length != 0)
            {
                this.filesFullPathList.Clear();
                foreach (string str in strArray)
                {
                    this.updateAvalableList(str);
                }
            }
            this.filesArray = this.filesFullPathList.ToArray();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        internal static frmUpdatePbFiles GetChildInstance(e_configParams type)
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmUpdatePbFiles(type);
            }
            return m_SChildform;
        }

        internal static frmUpdatePbFiles GetChildInstance(e_configParams type, string inStr)
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmUpdatePbFiles(type, inStr);
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmUpdatePbFiles));
            this.updatePbDoneBut = new Button();
            this.updatePbRemoveAllBut = new Button();
            this.updatePbRemoveBut = new Button();
            this.updatePbAddAllBut = new Button();
            this.updatePbAddBut = new Button();
            this.updatePbExitBut = new Button();
            this.updatePbAvailableFilesListVal = new ListBox();
            this.updatePbToRunFilesListVal = new ListBox();
            this.updatePbDirBrowser = new Button();
            this.updatePbRunFilesLabel = new Label();
            this.updateAvailablePbFilesLabel = new Label();
            this.updatePbFilePathLabel = new Label();
            this.updatePbDirVal = new TextBox();
            this.updatePbClearAvailableFilesBut = new Button();
            this.frmUpdatePbFilesCntLabel = new Label();
            base.SuspendLayout();
            this.updatePbDoneBut.Location = new Point(0x126, 0x1d3);
            this.updatePbDoneBut.Name = "updatePbDoneBut";
            this.updatePbDoneBut.Size = new Size(0x4b, 0x17);
            this.updatePbDoneBut.TabIndex = 9;
            this.updatePbDoneBut.Text = "&Done";
            this.updatePbDoneBut.UseVisualStyleBackColor = true;
            this.updatePbDoneBut.Click += new EventHandler(this.updatePbDoneBut_Click);
            this.updatePbRemoveAllBut.Location = new Point(0x14d, 0x133);
            this.updatePbRemoveAllBut.Name = "updatePbRemoveAllBut";
            this.updatePbRemoveAllBut.Size = new Size(0x63, 0x1a);
            this.updatePbRemoveAllBut.TabIndex = 7;
            this.updatePbRemoveAllBut.Text = "Re&move All <<";
            this.updatePbRemoveAllBut.UseVisualStyleBackColor = true;
            this.updatePbRemoveAllBut.Click += new EventHandler(this.updatePbRemoveAllBut_Click);
            this.updatePbRemoveBut.Location = new Point(0x14d, 0x116);
            this.updatePbRemoveBut.Name = "updatePbRemoveBut";
            this.updatePbRemoveBut.Size = new Size(0x63, 0x17);
            this.updatePbRemoveBut.TabIndex = 6;
            this.updatePbRemoveBut.Text = "&Remove <";
            this.updatePbRemoveBut.UseVisualStyleBackColor = true;
            this.updatePbRemoveBut.Click += new EventHandler(this.updatePbRemoveBut_Click);
            this.updatePbAddAllBut.Location = new Point(0x14d, 0xf9);
            this.updatePbAddAllBut.Name = "updatePbAddAllBut";
            this.updatePbAddAllBut.Size = new Size(0x63, 0x17);
            this.updatePbAddAllBut.TabIndex = 5;
            this.updatePbAddAllBut.Text = "Add A&ll >>";
            this.updatePbAddAllBut.UseVisualStyleBackColor = true;
            this.updatePbAddAllBut.Click += new EventHandler(this.updatePbAddAllBut_Click);
            this.updatePbAddBut.Location = new Point(0x14d, 220);
            this.updatePbAddBut.Name = "updatePbAddBut";
            this.updatePbAddBut.Size = new Size(0x63, 0x17);
            this.updatePbAddBut.TabIndex = 4;
            this.updatePbAddBut.Text = "&Add >";
            this.updatePbAddBut.UseVisualStyleBackColor = true;
            this.updatePbAddBut.Click += new EventHandler(this.updatePbAddBut_Click);
            this.updatePbExitBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.updatePbExitBut.Location = new Point(0x18e, 0x1d3);
            this.updatePbExitBut.Name = "updatePbExitBut";
            this.updatePbExitBut.Size = new Size(0x4b, 0x17);
            this.updatePbExitBut.TabIndex = 10;
            this.updatePbExitBut.Text = "E&xit";
            this.updatePbExitBut.UseVisualStyleBackColor = true;
            this.updatePbExitBut.Click += new EventHandler(this.updatePbExitBut_Click);
            this.updatePbAvailableFilesListVal.FormattingEnabled = true;
            this.updatePbAvailableFilesListVal.HorizontalScrollbar = true;
            this.updatePbAvailableFilesListVal.Location = new Point(0x1d, 0x7c);
            this.updatePbAvailableFilesListVal.Name = "updatePbAvailableFilesListVal";
            this.updatePbAvailableFilesListVal.ScrollAlwaysVisible = true;
            this.updatePbAvailableFilesListVal.Size = new Size(290, 0x13c);
            this.updatePbAvailableFilesListVal.TabIndex = 3;
            this.updatePbAvailableFilesListVal.DoubleClick += new EventHandler(this.updatePbAvailableFilesListVal_DoubleClick);
            this.updatePbToRunFilesListVal.FormattingEnabled = true;
            this.updatePbToRunFilesListVal.HorizontalScrollbar = true;
            this.updatePbToRunFilesListVal.Location = new Point(0x1bf, 0x7c);
            this.updatePbToRunFilesListVal.Name = "updatePbToRunFilesListVal";
            this.updatePbToRunFilesListVal.ScrollAlwaysVisible = true;
            this.updatePbToRunFilesListVal.Size = new Size(0x127, 0x13c);
            this.updatePbToRunFilesListVal.TabIndex = 8;
            this.updatePbDirBrowser.Location = new Point(0x2c7, 0x2b);
            this.updatePbDirBrowser.Name = "updatePbDirBrowser";
            this.updatePbDirBrowser.Size = new Size(0x1a, 0x17);
            this.updatePbDirBrowser.TabIndex = 1;
            this.updatePbDirBrowser.Text = "...";
            this.updatePbDirBrowser.UseVisualStyleBackColor = true;
            this.updatePbDirBrowser.Click += new EventHandler(this.updatePbDirBrowser_Click);
            this.updatePbRunFilesLabel.AutoSize = true;
            this.updatePbRunFilesLabel.Location = new Point(0x1bc, 0x63);
            this.updatePbRunFilesLabel.Name = "updatePbRunFilesLabel";
            this.updatePbRunFilesLabel.Size = new Size(0x43, 13);
            this.updatePbRunFilesLabel.TabIndex = 10;
            this.updatePbRunFilesLabel.Text = "Files To Run";
            this.updateAvailablePbFilesLabel.AutoSize = true;
            this.updateAvailablePbFilesLabel.Location = new Point(0x1a, 0x63);
            this.updateAvailablePbFilesLabel.Name = "updateAvailablePbFilesLabel";
            this.updateAvailablePbFilesLabel.Size = new Size(0x4a, 13);
            this.updateAvailablePbFilesLabel.TabIndex = 3;
            this.updateAvailablePbFilesLabel.Text = "Available Files";
            this.updatePbFilePathLabel.AutoSize = true;
            this.updatePbFilePathLabel.Location = new Point(0x1a, 0x19);
            this.updatePbFilePathLabel.Name = "updatePbFilePathLabel";
            this.updatePbFilePathLabel.Size = new Size(120, 13);
            this.updatePbFilePathLabel.TabIndex = 0;
            this.updatePbFilePathLabel.Text = "Playback Files Directory";
            this.updatePbDirVal.AllowDrop = true;
            this.updatePbDirVal.Location = new Point(0x1d, 0x2d);
            this.updatePbDirVal.Name = "updatePbDirVal";
            this.updatePbDirVal.Size = new Size(0x298, 20);
            this.updatePbDirVal.TabIndex = 0;
            this.updatePbClearAvailableFilesBut.Location = new Point(0xf3, 0x5e);
            this.updatePbClearAvailableFilesBut.Name = "updatePbClearAvailableFilesBut";
            this.updatePbClearAvailableFilesBut.Size = new Size(0x4b, 0x17);
            this.updatePbClearAvailableFilesBut.TabIndex = 2;
            this.updatePbClearAvailableFilesBut.Text = "&Clear";
            this.updatePbClearAvailableFilesBut.UseVisualStyleBackColor = true;
            this.updatePbClearAvailableFilesBut.Click += new EventHandler(this.updatePbClearAvailableFilesBut_Click);
            this.frmUpdatePbFilesCntLabel.AutoSize = true;
            this.frmUpdatePbFilesCntLabel.Location = new Point(0x205, 0x63);
            this.frmUpdatePbFilesCntLabel.Name = "frmUpdatePbFilesCntLabel";
            this.frmUpdatePbFilesCntLabel.Size = new Size(13, 13);
            this.frmUpdatePbFilesCntLabel.TabIndex = 11;
            this.frmUpdatePbFilesCntLabel.Text = "0";
            base.AcceptButton = this.updatePbDoneBut;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.CancelButton = this.updatePbExitBut;
            base.ClientSize = new Size(0x301, 0x203);
            base.Controls.Add(this.frmUpdatePbFilesCntLabel);
            base.Controls.Add(this.updatePbClearAvailableFilesBut);
            base.Controls.Add(this.updatePbDoneBut);
            base.Controls.Add(this.updatePbRemoveAllBut);
            base.Controls.Add(this.updatePbRemoveBut);
            base.Controls.Add(this.updatePbAddAllBut);
            base.Controls.Add(this.updatePbAddBut);
            base.Controls.Add(this.updatePbExitBut);
            base.Controls.Add(this.updatePbAvailableFilesListVal);
            base.Controls.Add(this.updatePbToRunFilesListVal);
            base.Controls.Add(this.updatePbDirBrowser);
            base.Controls.Add(this.updatePbRunFilesLabel);
            base.Controls.Add(this.updateAvailablePbFilesLabel);
            base.Controls.Add(this.updatePbFilePathLabel);
            base.Controls.Add(this.updatePbDirVal);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmUpdatePbFiles";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Update Files";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            m_SChildform = null;
        }

        private void updateAvalableList(string inStr)
        {
            string[] strArray = new string[] { "" };
            int length = 0;
            this.filesFullPathList.Add(inStr);
            strArray = inStr.Split(new char[] { '\\' });
            length = strArray.Length;
            this.updatePbAvailableFilesListVal.Items.Add(strArray[length - 1]);
        }

        private void updatePbAddAllBut_Click(object sender, EventArgs e)
        {
            this.updatePbToRunFilesListVal.Items.Clear();
            this.toRunList.Clear();
            foreach (string str in this.updatePbAvailableFilesListVal.Items)
            {
                this.updatePbToRunFilesListVal.Items.Add(str);
            }
            foreach (string str2 in this.filesFullPathList)
            {
                this.toRunList.Add(str2);
            }
            this.updatePBFilesCnt();
        }

        private void updatePbAddBut_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.updatePbAvailableFilesListVal.SelectedIndex;
            if (selectedIndex >= 0)
            {
                this.updatePbToRunFilesListVal.Items.Add(this.updatePbAvailableFilesListVal.SelectedItem);
                this.toRunList.Add(this.filesArray[selectedIndex]);
                this.updatePBFilesCnt();
            }
        }

        private void updatePbAvailableFilesListVal_DoubleClick(object sender, EventArgs e)
        {
            if (this.filesFullPathList.Count != 0)
            {
                string text = string.Empty;
                foreach (string str2 in this.filesFullPathList)
                {
                    text = text + str2 + "\n";
                }
                MessageBox.Show(text, "Information");
            }
        }

        private void updatePbClearAvailableFilesBut_Click(object sender, EventArgs e)
        {
            this.updatePbAvailableFilesListVal.Items.Clear();
            this.filesFullPathList.Clear();
        }

        private void updatePbDirBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.updatePbDirVal.Text = dialog.SelectedPath;
                foreach (string str in Directory.GetFiles(dialog.SelectedPath))
                {
                    if ((str.EndsWith(".pcm") || str.EndsWith(".exe")) || str.EndsWith(".pd2"))
                    {
                        this.updateAvalableList(str);
                    }
                }
            }
            this.filesArray = this.filesFullPathList.ToArray();
        }

        private void updatePbDoneBut_Click(object sender, EventArgs e)
        {
            this.outputStr = string.Empty;
            if ((this.selectUpdateElmName == e_configParams.E_HOST_APP_NAME) || (this.selectUpdateElmName == e_configParams.E_PATCH_NAME))
            {
                foreach (string str in this.updatePbToRunFilesListVal.Items)
                {
                    if (this.outputStr.Length != 0)
                    {
                        this.outputStr = this.outputStr + "," + str;
                    }
                    else
                    {
                        this.outputStr = str;
                    }
                }
            }
            else
            {
                foreach (string str2 in this.toRunList)
                {
                    if (this.outputStr.Length != 0)
                    {
                        this.outputStr = this.outputStr + "," + str2;
                    }
                    else
                    {
                        this.outputStr = str2;
                    }
                }
            }
            this.outputStr = this.outputStr.TrimEnd(new char[] { ',' });
            if ((this.updateParent != null) && (this.outputStr != string.Empty))
            {
                this.updateParent(this.outputStr);
            }
            base.Close();
            m_SChildform = null;
        }

        private void updatePbExitBut_Click(object sender, EventArgs e)
        {
            this.outputStr = string.Empty;
            base.Close();
            m_SChildform = null;
        }

        private void updatePBFilesCnt()
        {
            string str = this.updatePbToRunFilesListVal.Items.Count.ToString();
            this.frmUpdatePbFilesCntLabel.Text = str;
        }

        private void updatePbRemoveAllBut_Click(object sender, EventArgs e)
        {
            this.updatePbToRunFilesListVal.Items.Clear();
            this.toRunList.Clear();
            this.filesFullPathList.Clear();
            this.updatePBFilesCnt();
        }

        private void updatePbRemoveBut_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.updatePbToRunFilesListVal.SelectedIndex;
            string[] strArray = this.toRunList.ToArray();
            if (selectedIndex >= 0)
            {
                this.updatePbToRunFilesListVal.Items.Remove(this.updatePbToRunFilesListVal.SelectedItem);
                this.toRunList.Remove(strArray[selectedIndex]);
                this.updatePBFilesCnt();
            }
        }

        internal delegate void updateParentEventHandler(string updatedData);
    }
}

