﻿namespace SiRFLive.GUI.Automation
{
    using CommonClassLibrary;
    using SiRFLive.Configuration;
    using SiRFLive.General;
    using SiRFLive.TestAutomation;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class frmStationSetup : Form
    {
        private bool _errorFound;
        private string _testName = string.Empty;
        private ComboBox attenSourceComboBox;
        private Label attenSourceLabel;
        private Button cancelBtn;
        private IContainer components;
        private Button doneBtn;
        private GroupBox playbackFileGprBox;
        private Label playbackFileListLabel;
        private TextBox playbackFileListTxtBox;
        private TextBox playbackTimeListTxtBox;
        private ComboBox powerSourceComboBox;
        private Label powerSourceLabel;
        private Button resultLogDirectoryPathBrowserBtn;
        private TextBox resultLogDirectoryTxtBox;
        private ComboBox signalSourceComboBox;
        private Label signalSourceLabel;
        private Button simFilePathBrowserBtn;
        private TextBox simFilePathTxtBox;
        private Label simInitialAttenLabel;
        private TextBox simInitialAttenTxtBox;
        private TextBox simIPAddressTxtBox;
        private Label simplexAddressLabel;
        private GroupBox simplexGrpBox;
        private Label simPortLabel;
        private TextBox simPortTxtBox;
        private Label titleLable;
        private Button updatePlaybackFileBtn;
        private Button updatePlaybackTimeBtn;
        private Button updateReceiverBtn;

        public frmStationSetup(string testName)
        {
            this.InitializeComponent();
            this._testName = testName;
        }

        private void attenSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.attenSourceComboBox.Text = this.attenSourceComboBox.SelectedItem.ToString();
            if (clsGlobal.AutomationParamsHash.ContainsKey("ATTEN_SOURCE"))
            {
                clsGlobal.AutomationParamsHash["ATTEN_SOURCE"] = this.attenSourceComboBox.Text;
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._errorFound = false;
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void doneBtn_Click(object sender, EventArgs e)
        {
            this._errorFound = false;
            if (clsGlobal.AutomationParamsHash.ContainsKey("BASE_TEST_LOG") && (this.resultLogDirectoryTxtBox.Text != string.Empty))
            {
                clsGlobal.AutomationParamsHash["BASE_TEST_LOG"] = this.resultLogDirectoryTxtBox.Text;
            }
            if (clsGlobal.AutomationParamsHash.ContainsKey("PLAYBACK_FILES") && (this.playbackFileListTxtBox.Text != string.Empty))
            {
                clsGlobal.AutomationParamsHash["PLAYBACK_FILES"] = this.playbackFileListTxtBox.Text;
            }
            if (clsGlobal.AutomationParamsHash.ContainsKey("PLAY_TIME_LISTS") && (this.playbackTimeListTxtBox.Text != string.Empty))
            {
                clsGlobal.AutomationParamsHash["PLAY_TIME_LISTS"] = this.playbackTimeListTxtBox.Text;
            }
            if (clsGlobal.AutomationParamsHash.ContainsKey("SIM_ADDRESS") && (this.simIPAddressTxtBox.Text != string.Empty))
            {
                clsGlobal.AutomationParamsHash["SIM_ADDRESS"] = this.simIPAddressTxtBox.Text;
            }
            if (clsGlobal.AutomationParamsHash.ContainsKey("SIM_PORT") && (this.simPortTxtBox.Text != string.Empty))
            {
                clsGlobal.AutomationParamsHash["SIM_PORT"] = this.simPortTxtBox.Text;
            }
            if (clsGlobal.AutomationParamsHash.ContainsKey("SIM_FILE") && (this.simFilePathTxtBox.Text != string.Empty))
            {
                clsGlobal.AutomationParamsHash["SIM_FILE"] = this.simFilePathTxtBox.Text;
            }
            if (clsGlobal.AutomationParamsHash.ContainsKey("SIM_START_ATTEN") && (this.simInitialAttenTxtBox.Text != string.Empty))
            {
                clsGlobal.AutomationParamsHash["SIM_START_ATTEN"] = this.simInitialAttenTxtBox.Text;
            }
            string path = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\scripts\SiRFLiveAutomationSetup.cfg";
            if (!File.Exists(path))
            {
                MessageBox.Show(string.Format("Config File does not exist!\n {0}", path), "Automation Test Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this._errorFound = true;
            }
            else
            {
                if (clsGlobal.IsMarketingUser())
                {
                    this.updateConfigFile();
                }
                if (this.saveConfigToHash(path) == 0)
                {
                    if ((this._testName == "3GPP") || (this._testName == "TIA916"))
                    {
                        string str2 = string.Empty;
                        if (this._testName == "3GPP")
                        {
                            str2 = clsGlobal.InstalledDirectory + @"\scripts\3GPP\3GPP.cfg";
                        }
                        else
                        {
                            str2 = clsGlobal.InstalledDirectory + @"\scripts\TIA916\TIA916.cfg";
                        }
                        if (File.Exists(str2))
                        {
                            if ((File.GetAttributes(str2) & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
                            {
                                IniHelper helper = new IniHelper(str2);
                                if (this.simIPAddressTxtBox.Text != string.Empty)
                                {
                                    helper.IniWriteValue("SIM", "SIM_ADDRESS", this.simIPAddressTxtBox.Text);
                                }
                                if (this.simPortTxtBox.Text != string.Empty)
                                {
                                    helper.IniWriteValue("SIM", "SIM_PORT", this.simPortTxtBox.Text.Replace(" ", ""));
                                }
                            }
                            else
                            {
                                MessageBox.Show(string.Format("Readonly file! Please make sure file is not readonly before proceeding!\n{0}", str2), "Station Setup Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                                this._errorFound = true;
                                return;
                            }
                        }
                    }
                    base.DialogResult = DialogResult.OK;
                    clsGlobal.AutomationParamsHash.Clear();
                    base.Close();
                }
            }
        }

        private void frmStationSetup_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = this._errorFound;
        }

        private void frmStationSetup_Load(object sender, EventArgs e)
        {
            this._errorFound = false;
            string path = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\scripts\SiRFLiveAutomationSetup.cfg";
            if (!File.Exists(path))
            {
                MessageBox.Show(string.Format("Config File does not exist!\n {0}", path), "Automation Test Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            else
            {
                this.readConfigToHash(path);
                string key = string.Empty;
                key = "AVAILABLE_SIGNAL_SOURCE";
                if (clsGlobal.AutomationParamsHash.ContainsKey(key))
                {
                    string[] strArray = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                    if (strArray.Length > 0)
                    {
                        foreach (string str3 in strArray)
                        {
                            string item = str3.TrimStart(new char[] { ' ' });
                            item = str3.TrimEnd(new char[] { ' ' });
                            this.signalSourceComboBox.Items.Add(item);
                        }
                    }
                }
                key = "AVAILABLE_ATTEN_SOURCE";
                if (clsGlobal.AutomationParamsHash.ContainsKey(key))
                {
                    string[] strArray2 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                    if (strArray2.Length > 0)
                    {
                        foreach (string str5 in strArray2)
                        {
                            string str6 = str5.TrimStart(new char[] { ' ' });
                            str6 = str5.TrimEnd(new char[] { ' ' });
                            this.attenSourceComboBox.Items.Add(str6);
                        }
                    }
                }
                key = "AVAILABLE_POWER_SOURCE";
                if (clsGlobal.AutomationParamsHash.ContainsKey(key))
                {
                    string[] strArray3 = ((string) clsGlobal.AutomationParamsHash[key]).Split(new char[] { ',' });
                    if (strArray3.Length > 0)
                    {
                        foreach (string str7 in strArray3)
                        {
                            string str8 = str7.TrimStart(new char[] { ' ' });
                            str8 = str7.TrimEnd(new char[] { ' ' });
                            this.powerSourceComboBox.Items.Add(str8);
                        }
                    }
                }
                if (clsGlobal.AutomationParamsHash.ContainsKey("SIGNAL_SOURCE"))
                {
                    this.signalSourceComboBox.Text = (string) clsGlobal.AutomationParamsHash["SIGNAL_SOURCE"];
                }
                this.updateSignalSource();
                if (clsGlobal.AutomationParamsHash.ContainsKey("ATTEN_SOURCE"))
                {
                    this.attenSourceComboBox.Text = (string) clsGlobal.AutomationParamsHash["ATTEN_SOURCE"];
                }
                if (clsGlobal.AutomationParamsHash.ContainsKey("POWER_SOURCE"))
                {
                    this.powerSourceComboBox.Text = (string) clsGlobal.AutomationParamsHash["POWER_SOURCE"];
                }
                if (clsGlobal.AutomationParamsHash.ContainsKey("BASE_TEST_LOG"))
                {
                    this.resultLogDirectoryTxtBox.Text = (string) clsGlobal.AutomationParamsHash["BASE_TEST_LOG"];
                }
                this.updateTestParams();
                if (clsGlobal.IsMarketingUser())
                {
                    this.updateReceiverBtn.Visible = false;
                }
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmStationSetup));
            this.titleLable = new Label();
            this.signalSourceComboBox = new ComboBox();
            this.signalSourceLabel = new Label();
            this.attenSourceComboBox = new ComboBox();
            this.attenSourceLabel = new Label();
            this.powerSourceComboBox = new ComboBox();
            this.powerSourceLabel = new Label();
            this.resultLogDirectoryPathBrowserBtn = new Button();
            this.resultLogDirectoryTxtBox = new TextBox();
            this.simplexAddressLabel = new Label();
            this.simIPAddressTxtBox = new TextBox();
            this.simPortLabel = new Label();
            this.simPortTxtBox = new TextBox();
            this.simFilePathTxtBox = new TextBox();
            this.simFilePathBrowserBtn = new Button();
            this.simplexGrpBox = new GroupBox();
            this.simInitialAttenTxtBox = new TextBox();
            this.simInitialAttenLabel = new Label();
            this.updatePlaybackFileBtn = new Button();
            this.updatePlaybackTimeBtn = new Button();
            this.updateReceiverBtn = new Button();
            this.playbackFileListTxtBox = new TextBox();
            this.playbackFileListLabel = new Label();
            this.playbackTimeListTxtBox = new TextBox();
            this.playbackFileGprBox = new GroupBox();
            this.doneBtn = new Button();
            this.cancelBtn = new Button();
            Label label = new Label();
            Label label2 = new Label();
            Label label3 = new Label();
            this.simplexGrpBox.SuspendLayout();
            this.playbackFileGprBox.SuspendLayout();
            base.SuspendLayout();
            label.AutoSize = true;
            label.Location = new Point(40, 0xa5);
            label.Name = "resultLogDirectoryPathLabel";
            label.Size = new Size(0x6a, 13);
            label.TabIndex = 0x1d;
            label.Text = "Result Log Directory:";
            label2.AutoSize = true;
            label2.Location = new Point(11, 0x4e);
            label2.Name = "simFilePathLabel";
            label2.Size = new Size(0x47, 13);
            label2.TabIndex = 0x1d;
            label2.Text = "Sim File Path:";
            label3.AutoSize = true;
            label3.Location = new Point(11, 0x38);
            label3.Name = "playbackTimeListLabel";
            label3.Size = new Size(0x63, 13);
            label3.TabIndex = 0x24;
            label3.Text = "Playback Time List:";
            this.titleLable.AutoSize = true;
            this.titleLable.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.titleLable.Location = new Point(0xd5, 0x1d);
            this.titleLable.Name = "titleLable";
            this.titleLable.Size = new Size(0xcf, 0x19);
            this.titleLable.TabIndex = 0;
            this.titleLable.Text = "Test Station Setup";
            this.signalSourceComboBox.FormattingEnabled = true;
            this.signalSourceComboBox.Location = new Point(0x9b, 0x4a);
            this.signalSourceComboBox.Name = "signalSourceComboBox";
            this.signalSourceComboBox.Size = new Size(0x66, 0x15);
            this.signalSourceComboBox.TabIndex = 1;
            this.signalSourceComboBox.SelectedIndexChanged += new EventHandler(this.signalSourceComboBox_SelectedIndexChanged);
            this.signalSourceLabel.AutoSize = true;
            this.signalSourceLabel.Location = new Point(40, 0x4e);
            this.signalSourceLabel.Name = "signalSourceLabel";
            this.signalSourceLabel.Size = new Size(0x4c, 13);
            this.signalSourceLabel.TabIndex = 2;
            this.signalSourceLabel.Text = "Signal Source:";
            this.attenSourceComboBox.FormattingEnabled = true;
            this.attenSourceComboBox.Location = new Point(0x9b, 0x65);
            this.attenSourceComboBox.Name = "attenSourceComboBox";
            this.attenSourceComboBox.Size = new Size(0x66, 0x15);
            this.attenSourceComboBox.TabIndex = 1;
            this.attenSourceComboBox.SelectedIndexChanged += new EventHandler(this.attenSourceComboBox_SelectedIndexChanged);
            this.attenSourceLabel.AutoSize = true;
            this.attenSourceLabel.Location = new Point(40, 0x69);
            this.attenSourceLabel.Name = "attenSourceLabel";
            this.attenSourceLabel.Size = new Size(0x65, 13);
            this.attenSourceLabel.TabIndex = 2;
            this.attenSourceLabel.Text = "Attenuation Source:";
            this.powerSourceComboBox.FormattingEnabled = true;
            this.powerSourceComboBox.Location = new Point(0x9b, 0x80);
            this.powerSourceComboBox.Name = "powerSourceComboBox";
            this.powerSourceComboBox.Size = new Size(0x66, 0x15);
            this.powerSourceComboBox.TabIndex = 1;
            this.powerSourceComboBox.SelectedIndexChanged += new EventHandler(this.powerSourceComboBox_SelectedIndexChanged);
            this.powerSourceLabel.AutoSize = true;
            this.powerSourceLabel.Location = new Point(40, 0x84);
            this.powerSourceLabel.Name = "powerSourceLabel";
            this.powerSourceLabel.Size = new Size(0x4d, 13);
            this.powerSourceLabel.TabIndex = 2;
            this.powerSourceLabel.Text = "Power Source:";
            this.resultLogDirectoryPathBrowserBtn.Location = new Point(0x218, 0x9f);
            this.resultLogDirectoryPathBrowserBtn.Name = "resultLogDirectoryPathBrowserBtn";
            this.resultLogDirectoryPathBrowserBtn.Size = new Size(0x1f, 0x17);
            this.resultLogDirectoryPathBrowserBtn.TabIndex = 0x1f;
            this.resultLogDirectoryPathBrowserBtn.Text = "...";
            this.resultLogDirectoryPathBrowserBtn.UseVisualStyleBackColor = true;
            this.resultLogDirectoryPathBrowserBtn.Click += new EventHandler(this.resultLogDirectoryPathBrowserBtn_Click);
            this.resultLogDirectoryTxtBox.Location = new Point(0x9b, 0xa1);
            this.resultLogDirectoryTxtBox.Name = "resultLogDirectoryTxtBox";
            this.resultLogDirectoryTxtBox.Size = new Size(0x16d, 20);
            this.resultLogDirectoryTxtBox.TabIndex = 30;
            this.simplexAddressLabel.AutoSize = true;
            this.simplexAddressLabel.Location = new Point(11, 0x1a);
            this.simplexAddressLabel.Name = "simplexAddressLabel";
            this.simplexAddressLabel.Size = new Size(0x44, 13);
            this.simplexAddressLabel.TabIndex = 0x20;
            this.simplexAddressLabel.Text = "Sim Address:";
            this.simIPAddressTxtBox.AccessibleRole = System.Windows.Forms.AccessibleRole.IpAddress;
            this.simIPAddressTxtBox.Location = new Point(0x74, 0x16);
            this.simIPAddressTxtBox.Name = "simIPAddressTxtBox";
            this.simIPAddressTxtBox.Size = new Size(0x61, 20);
            this.simIPAddressTxtBox.TabIndex = 0x21;
            this.simPortLabel.AutoSize = true;
            this.simPortLabel.Location = new Point(0xf6, 0x1a);
            this.simPortLabel.Name = "simPortLabel";
            this.simPortLabel.Size = new Size(0x31, 13);
            this.simPortLabel.TabIndex = 0x20;
            this.simPortLabel.Text = "Sim Port:";
            this.simPortTxtBox.AccessibleRole = System.Windows.Forms.AccessibleRole.IpAddress;
            this.simPortTxtBox.Location = new Point(0x137, 0x16);
            this.simPortTxtBox.Name = "simPortTxtBox";
            this.simPortTxtBox.Size = new Size(0x61, 20);
            this.simPortTxtBox.TabIndex = 0x21;
            this.simFilePathTxtBox.Location = new Point(0x74, 0x4a);
            this.simFilePathTxtBox.Name = "simFilePathTxtBox";
            this.simFilePathTxtBox.Size = new Size(0x16d, 20);
            this.simFilePathTxtBox.TabIndex = 30;
            this.simFilePathBrowserBtn.Location = new Point(0x1f0, 0x47);
            this.simFilePathBrowserBtn.Name = "simFilePathBrowserBtn";
            this.simFilePathBrowserBtn.Size = new Size(0x1f, 0x17);
            this.simFilePathBrowserBtn.TabIndex = 0x1f;
            this.simFilePathBrowserBtn.Text = "...";
            this.simFilePathBrowserBtn.UseVisualStyleBackColor = true;
            this.simFilePathBrowserBtn.Click += new EventHandler(this.simFilePathBrowserBtn_Click);
            this.simplexGrpBox.Controls.Add(this.simInitialAttenTxtBox);
            this.simplexGrpBox.Controls.Add(this.simInitialAttenLabel);
            this.simplexGrpBox.Controls.Add(this.simPortTxtBox);
            this.simplexGrpBox.Controls.Add(this.simPortLabel);
            this.simplexGrpBox.Controls.Add(this.simFilePathBrowserBtn);
            this.simplexGrpBox.Controls.Add(this.simIPAddressTxtBox);
            this.simplexGrpBox.Controls.Add(this.simplexAddressLabel);
            this.simplexGrpBox.Controls.Add(label2);
            this.simplexGrpBox.Controls.Add(this.simFilePathTxtBox);
            this.simplexGrpBox.Location = new Point(0x21, 0xc9);
            this.simplexGrpBox.Name = "simplexGrpBox";
            this.simplexGrpBox.Size = new Size(0x21f, 0x68);
            this.simplexGrpBox.TabIndex = 0x22;
            this.simplexGrpBox.TabStop = false;
            this.simplexGrpBox.Text = "Simulator Parameters";
            this.simInitialAttenTxtBox.AccessibleRole = System.Windows.Forms.AccessibleRole.IpAddress;
            this.simInitialAttenTxtBox.Location = new Point(0x74, 0x30);
            this.simInitialAttenTxtBox.Name = "simInitialAttenTxtBox";
            this.simInitialAttenTxtBox.Size = new Size(0x61, 20);
            this.simInitialAttenTxtBox.TabIndex = 0x21;
            this.simInitialAttenLabel.AutoSize = true;
            this.simInitialAttenLabel.Location = new Point(11, 0x34);
            this.simInitialAttenLabel.Name = "simInitialAttenLabel";
            this.simInitialAttenLabel.Size = new Size(0x52, 13);
            this.simInitialAttenLabel.TabIndex = 0x20;
            this.simInitialAttenLabel.Text = "Sim Initial Atten:";
            this.updatePlaybackFileBtn.Location = new Point(0x143, 0x63);
            this.updatePlaybackFileBtn.Name = "updatePlaybackFileBtn";
            this.updatePlaybackFileBtn.Size = new Size(0x87, 0x17);
            this.updatePlaybackFileBtn.TabIndex = 0x23;
            this.updatePlaybackFileBtn.Text = "Update Playback &Files";
            this.updatePlaybackFileBtn.UseVisualStyleBackColor = true;
            this.updatePlaybackFileBtn.Click += new EventHandler(this.updatePlaybackFileBtn_Click);
            this.updatePlaybackTimeBtn.Location = new Point(0x143, 0x7e);
            this.updatePlaybackTimeBtn.Name = "updatePlaybackTimeBtn";
            this.updatePlaybackTimeBtn.Size = new Size(0x87, 0x17);
            this.updatePlaybackTimeBtn.TabIndex = 0x23;
            this.updatePlaybackTimeBtn.Text = "Update Playback &Time";
            this.updatePlaybackTimeBtn.UseVisualStyleBackColor = true;
            this.updatePlaybackTimeBtn.Click += new EventHandler(this.updatePlaybackTime_Click);
            this.updateReceiverBtn.Location = new Point(0x143, 0x48);
            this.updateReceiverBtn.Name = "updateReceiverBtn";
            this.updateReceiverBtn.Size = new Size(0x87, 0x17);
            this.updateReceiverBtn.TabIndex = 0x23;
            this.updateReceiverBtn.Text = "Update &Receiver";
            this.updateReceiverBtn.UseVisualStyleBackColor = true;
            this.updateReceiverBtn.Click += new EventHandler(this.updateReceiver_Click);
            this.playbackFileListTxtBox.Location = new Point(0x6f, 0x1a);
            this.playbackFileListTxtBox.Name = "playbackFileListTxtBox";
            this.playbackFileListTxtBox.Size = new Size(0x16d, 20);
            this.playbackFileListTxtBox.TabIndex = 0x27;
            this.playbackFileListTxtBox.MouseDoubleClick += new MouseEventHandler(this.playbackFileListTxtBox_MouseDoubleClick);
            this.playbackFileListLabel.AutoSize = true;
            this.playbackFileListLabel.Location = new Point(11, 30);
            this.playbackFileListLabel.Name = "playbackFileListLabel";
            this.playbackFileListLabel.Size = new Size(0x5c, 13);
            this.playbackFileListLabel.TabIndex = 0x26;
            this.playbackFileListLabel.Text = "Playback File List:";
            this.playbackTimeListTxtBox.Location = new Point(0x6f, 0x34);
            this.playbackTimeListTxtBox.Name = "playbackTimeListTxtBox";
            this.playbackTimeListTxtBox.Size = new Size(0x16d, 20);
            this.playbackTimeListTxtBox.TabIndex = 0x25;
            this.playbackTimeListTxtBox.MouseDoubleClick += new MouseEventHandler(this.playbackTimeListTxtBox_MouseDoubleClick);
            this.playbackFileGprBox.Controls.Add(this.playbackFileListTxtBox);
            this.playbackFileGprBox.Controls.Add(this.playbackFileListLabel);
            this.playbackFileGprBox.Controls.Add(label3);
            this.playbackFileGprBox.Controls.Add(this.playbackTimeListTxtBox);
            this.playbackFileGprBox.Location = new Point(0x21, 0xec);
            this.playbackFileGprBox.Name = "playbackFileGprBox";
            this.playbackFileGprBox.Size = new Size(0x1e7, 90);
            this.playbackFileGprBox.TabIndex = 40;
            this.playbackFileGprBox.TabStop = false;
            this.playbackFileGprBox.Text = "Playback Files";
            this.doneBtn.Location = new Point(0x1ec, 0x48);
            this.doneBtn.Name = "doneBtn";
            this.doneBtn.Size = new Size(0x4b, 0x17);
            this.doneBtn.TabIndex = 0x24;
            this.doneBtn.Text = "&Done";
            this.doneBtn.UseVisualStyleBackColor = true;
            this.doneBtn.Click += new EventHandler(this.doneBtn_Click);
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new Point(0x1ec, 0x63);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new Size(0x4b, 0x17);
            this.cancelBtn.TabIndex = 0x25;
            this.cancelBtn.Text = "&Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
            base.AcceptButton = this.doneBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.cancelBtn;
            base.ClientSize = new Size(0x25c, 0x157);
            base.Controls.Add(this.playbackFileGprBox);
            base.Controls.Add(this.cancelBtn);
            base.Controls.Add(this.doneBtn);
            base.Controls.Add(this.updatePlaybackTimeBtn);
            base.Controls.Add(this.simplexGrpBox);
            base.Controls.Add(this.updateReceiverBtn);
            base.Controls.Add(this.updatePlaybackFileBtn);
            base.Controls.Add(label);
            base.Controls.Add(this.resultLogDirectoryPathBrowserBtn);
            base.Controls.Add(this.resultLogDirectoryTxtBox);
            base.Controls.Add(this.powerSourceLabel);
            base.Controls.Add(this.attenSourceLabel);
            base.Controls.Add(this.signalSourceLabel);
            base.Controls.Add(this.powerSourceComboBox);
            base.Controls.Add(this.attenSourceComboBox);
            base.Controls.Add(this.signalSourceComboBox);
            base.Controls.Add(this.titleLable);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmStationSetup";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Station Setup";
            base.Load += new EventHandler(this.frmStationSetup_Load);
            base.FormClosing += new FormClosingEventHandler(this.frmStationSetup_FormClosing);
            this.simplexGrpBox.ResumeLayout(false);
            this.simplexGrpBox.PerformLayout();
            this.playbackFileGprBox.ResumeLayout(false);
            this.playbackFileGprBox.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void playbackFileListTxtBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.playbackFileListTxtBox.Text != string.Empty)
            {
                MessageBox.Show(this.playbackFileListTxtBox.Text.Replace(',', '\n'), "Playback Files List", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void playbackTimeListTxtBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.playbackTimeListTxtBox.Text != string.Empty)
            {
                MessageBox.Show(this.playbackTimeListTxtBox.Text.Replace(',', '\n'), "Playback Time List", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void powerSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.powerSourceComboBox.Text = this.powerSourceComboBox.SelectedItem.ToString();
            if (clsGlobal.AutomationParamsHash.ContainsKey("POWER_SOURCE"))
            {
                clsGlobal.AutomationParamsHash["POWER_SOURCE"] = this.powerSourceComboBox.Text;
            }
        }

        private void readConfigToHash(string configFilePath)
        {
            IniHelper helper = new IniHelper(configFilePath);
            List<string> sections = helper.GetSections();
            List<string> list2 = new List<string>();
            char[] trimChars = new char[] { '\n', '\r', '\t', '\0' };
            foreach (string str in sections)
            {
                foreach (string str2 in helper.GetKeys(str))
                {
                    if (!str2.Contains("#"))
                    {
                        string str3 = helper.GetIniFileString(str, str2, "").TrimEnd(trimChars);
                        if (!clsGlobal.AutomationParamsHash.ContainsKey(str2))
                        {
                            if ((str2 != "BASE_TEST_LOG") && (str2 != "SIM_FILE"))
                            {
                                clsGlobal.AutomationParamsHash.Add(str2, str3.Replace(" ", ""));
                            }
                            else
                            {
                                clsGlobal.AutomationParamsHash.Add(str2, str3);
                            }
                        }
                    }
                }
            }
        }

        private void resultLogDirectoryPathBrowserBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = ConfigurationManager.AppSettings["InstalledDirectory"];
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.resultLogDirectoryTxtBox.Text = dialog.SelectedPath;
            }
        }

        private int saveConfigToHash(string configFilePath)
        {
            if (File.Exists(configFilePath) && ((File.GetAttributes(configFilePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly))
            {
                MessageBox.Show(configFilePath + "\nFile is read only! Please change property and retry.", "Error");
                this.Cursor = Cursors.Default;
                return 1;
            }
            this.Cursor = Cursors.WaitCursor;
            IniHelper helper = new IniHelper(configFilePath);
            List<string> sections = helper.GetSections();
            List<string> list2 = new List<string>();
            foreach (string str in sections)
            {
                foreach (string str2 in helper.GetKeys(str))
                {
                    if (!str2.Contains("#") && clsGlobal.AutomationParamsHash.ContainsKey(str2))
                    {
                        helper.IniWriteValue(str, str2, (string) clsGlobal.AutomationParamsHash[str2]);
                    }
                }
            }
            int num = helper.IniSiRFLiveRxSetupErrorCheck(configFilePath);
            this.Cursor = Cursors.Default;
            return num;
        }

        private void signalSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.signalSourceComboBox.Text = this.signalSourceComboBox.SelectedItem.ToString();
            this.updateSignalSource();
        }

        private void simFilePathBrowserBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Sim File Locator";
            dialog.Filter = "Simulator files (*.sim)|*.sim|All files (*.*)|*.*";
            dialog.FilterIndex = 0;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.simFilePathTxtBox.Text = dialog.FileName;
            }
        }

        private bool updateConfigFile()
        {
            PortManager firstAvailablePort = clsGlobal.g_objfrmMDIMain.GetFirstAvailablePort();
            char[] trimChars = new char[] { 'C', 'c', 'O', 'o', 'M', 'm' };
            if (firstAvailablePort == null)
            {
                return false;
            }
            string key = "PROD_FAMILY";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.ProductFamily.ToString();
            }
            key = "RX_TYPES";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.RxType.ToString();
            }
            key = "RX_BAUDS";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.BaudRate.ToString();
            }
            key = "RX_NAMES";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                if (firstAvailablePort.comm.RxName == string.Empty)
                {
                    clsGlobal.AutomationParamsHash[key] = "SiRF_EVK";
                }
                else
                {
                    clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.RxName;
                }
            }
            key = "ACTIVE_PORTS";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.PortNum;
            }
            key = "PHY_COMM_PROTOCOL";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                if (firstAvailablePort.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
                {
                    clsGlobal.AutomationParamsHash[key] = "UART";
                }
                else if (firstAvailablePort.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Server)
                {
                    clsGlobal.AutomationParamsHash[key] = "TCP/IP_SERVER";
                }
                else if (firstAvailablePort.comm.InputDeviceMode == CommonClass.InputDeviceModes.FilePlayBack)
                {
                    clsGlobal.AutomationParamsHash[key] = "PLAYBACK_FILE";
                }
                else
                {
                    clsGlobal.AutomationParamsHash[key] = "TCP/IP_CLIENT";
                }
            }
            key = "TCPIP_IP_ADDRESS";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.CMC.HostAppClient.TCPClientHostName;
            }
            key = "TTB_PORTS";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.TTBPortNum;
            }
            key = "DEFAULT_CLK";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = "96250";
            }
            key = "EXT_CLK_FREQ";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.DefaultTCXOFreq;
            }
            key = "LNA_TYPE";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.LNAType.ToString();
            }
            key = "LDO_MODE";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.LDOMode.ToString();
            }
            key = "AI3_ICD";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.RxCtrl.AidingProtocolVersion;
            }
            key = "F_ICD";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.RxCtrl.ControlChannelVersion;
            }
            key = "RX_LOG_TYPES";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.RxCurrentTransmissionType.ToString();
            }
            key = "TX_LOG_TYPES";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = "GP2";
            }
            key = "MESSAGE_PROTOCOL";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.MessageProtocol;
            }
            key = "AIDING_PROTOCOL";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.AidingProtocol;
            }
            key = "REQUIRED_HOSTS";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = "0";
            }
            key = "REQUIRED_PATCH";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = "0";
            }
            key = "TRACKER_PORTS";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                if (firstAvailablePort.comm.RequireHostRun)
                {
                    clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.TrackerPort.Trim(trimChars);
                }
                else
                {
                    clsGlobal.AutomationParamsHash[key] = "-1";
                }
            }
            key = "HOST_PORTS";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                if (firstAvailablePort.comm.RequireHostRun)
                {
                    if (firstAvailablePort.comm.HostPair1 == string.Empty)
                    {
                        clsGlobal.AutomationParamsHash[key] = "-1";
                    }
                    else
                    {
                        clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.HostPair1.Trim(trimChars);
                    }
                }
                else
                {
                    clsGlobal.AutomationParamsHash[key] = "-1";
                }
            }
            key = "RESET_PORTS";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                if (firstAvailablePort.comm.RequireHostRun)
                {
                    clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.ResetPort.Trim(trimChars);
                }
                else
                {
                    clsGlobal.AutomationParamsHash[key] = "-1";
                }
            }
            key = "EXTRA_HOST_APP_ARGVS";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = "-1";
            }
            key = "HOST_APP";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = firstAvailablePort.comm.HostSWFilePath;
            }
            key = "PATCH_FILE";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = "-1";
            }
            key = "HOST_APP_DIR";
            if (clsGlobal.AutomationParamsHash.ContainsKey(key))
            {
                clsGlobal.AutomationParamsHash[key] = "-1";
            }
            return true;
        }

        private void updatePlaybackFileBtn_Click(object sender, EventArgs e)
        {
            e_configParams type = e_configParams.E_PLAYBACK_FILES;
            frmUpdatePbFiles childInstance = frmUpdatePbFiles.GetChildInstance(type, this.playbackFileListTxtBox.Text);
            if (childInstance.IsDisposed)
            {
                childInstance = new frmUpdatePbFiles(type, this.playbackFileListTxtBox.Text);
            }
            childInstance.updateParent += new frmUpdatePbFiles.updateParentEventHandler(this.updatePlaybackFileList);
            childInstance.MdiParent = base.MdiParent;
            childInstance.ShowDialog();
        }

        internal void updatePlaybackFileList(string updatedData)
        {
            this.playbackFileListTxtBox.Text = updatedData;
        }

        private void updatePlaybackTime_Click(object sender, EventArgs e)
        {
            frmUpdatePlaybackTime childInstance = frmUpdatePlaybackTime.GetChildInstance();
            if (childInstance.IsDisposed)
            {
                childInstance = new frmUpdatePlaybackTime();
            }
            childInstance.updateParent += new frmUpdatePlaybackTime.updateParentEventHandler(this.updatePlaybackTimeList);
            childInstance.MdiParent = base.MdiParent;
            childInstance.ShowDialog();
        }

        internal void updatePlaybackTimeList(string updatedData)
        {
            this.playbackTimeListTxtBox.Text = updatedData;
        }

        private void updateReceiver_Click(object sender, EventArgs e)
        {
            frmReceiverConfig childInstance = frmReceiverConfig.GetChildInstance();
            if (childInstance.IsDisposed)
            {
                childInstance = new frmReceiverConfig();
            }
            childInstance.MdiParent = base.MdiParent;
            childInstance.updateParent += new frmReceiverConfig.updateParentEventHandler(this.updateReceiverCallBack);
            childInstance.ShowDialog();
        }

        private void updateReceiverCallBack(Hashtable updateHash)
        {
            clsGlobal.AutomationParamsHash = updateHash;
        }

        private void updateSignalSource()
        {
            if (clsGlobal.AutomationParamsHash.ContainsKey("SIGNAL_SOURCE"))
            {
                clsGlobal.AutomationParamsHash["SIGNAL_SOURCE"] = this.signalSourceComboBox.Text;
            }
            if (this.signalSourceComboBox.Text == "SIM")
            {
                this.simplexGrpBox.Visible = true;
                this.playbackFileGprBox.Visible = false;
                if (clsGlobal.AutomationParamsHash.ContainsKey("SIM_ADDRESS"))
                {
                    this.simIPAddressTxtBox.Text = (string) clsGlobal.AutomationParamsHash["SIM_ADDRESS"];
                }
                if (clsGlobal.AutomationParamsHash.ContainsKey("SIM_PORT"))
                {
                    this.simPortTxtBox.Text = (string) clsGlobal.AutomationParamsHash["SIM_PORT"];
                }
                if (clsGlobal.AutomationParamsHash.ContainsKey("SIM_FILE"))
                {
                    this.simFilePathTxtBox.Text = (string) clsGlobal.AutomationParamsHash["SIM_FILE"];
                }
                if (clsGlobal.AutomationParamsHash.ContainsKey("SIM_START_ATTEN"))
                {
                    this.simInitialAttenTxtBox.Text = (string) clsGlobal.AutomationParamsHash["SIM_START_ATTEN"];
                }
                this.updatePlaybackFileBtn.Visible = false;
                this.updatePlaybackTimeBtn.Visible = false;
            }
            else if (this.signalSourceComboBox.Text == "RF_PLAYBACK")
            {
                this.simplexGrpBox.Visible = false;
                this.playbackFileGprBox.Visible = true;
                if (clsGlobal.AutomationParamsHash.ContainsKey("PLAY_TIME_LISTS"))
                {
                    this.playbackTimeListTxtBox.Text = (string) clsGlobal.AutomationParamsHash["PLAY_TIME_LISTS"];
                }
                if (clsGlobal.AutomationParamsHash.ContainsKey("PLAYBACK_FILES"))
                {
                    this.playbackFileListTxtBox.Text = (string) clsGlobal.AutomationParamsHash["PLAYBACK_FILES"];
                }
                this.updatePlaybackFileBtn.Visible = true;
                this.updatePlaybackTimeBtn.Visible = true;
            }
            else
            {
                this.simplexGrpBox.Visible = false;
                this.playbackFileGprBox.Visible = false;
                this.updatePlaybackFileBtn.Visible = false;
                this.updatePlaybackTimeBtn.Visible = false;
            }
            if (clsGlobal.AutomationParamsHash.ContainsKey("SIGNAL_SOURCE"))
            {
                clsGlobal.AutomationParamsHash["SIGNAL_SOURCE"] = this.signalSourceComboBox.Text;
            }
        }

        private void updateTestParams()
        {
            EventHandler method = null;
            EventHandler handler2 = null;
            EventHandler handler3 = null;
            if (this._testName == "3GPP")
            {
                if (method == null)
                {
                    method = delegate {
                        this.signalSourceComboBox.Text = "SIM";
                        this.attenSourceComboBox.Text = "SIM";
                        this.powerSourceComboBox.Text = "Manual";
                        this.signalSourceComboBox.Enabled = false;
                        this.attenSourceComboBox.Enabled = false;
                        this.attenSourceComboBox.Enabled = false;
                        this.simFilePathTxtBox.Text = @"C:\Program Files\Spirent Communications\SimPLEX\Scenarios\3GPP\Reference\SimPLEX_Spirent_3GPP_Normal_Accuracy_1\Spirent_3GPP_Normal_Accuracy_1.sim";
                        this.simFilePathTxtBox.Enabled = false;
                        this.simFilePathBrowserBtn.Enabled = false;
                    };
                }
                base.Invoke(method);
            }
            else if (this._testName == "TIA916")
            {
                if (handler2 == null)
                {
                    handler2 = delegate {
                        this.signalSourceComboBox.Text = "SIM";
                        this.attenSourceComboBox.Text = "SIM";
                        this.powerSourceComboBox.Text = "Manual";
                        this.signalSourceComboBox.Enabled = false;
                        this.attenSourceComboBox.Enabled = false;
                        this.attenSourceComboBox.Enabled = false;
                        this.simFilePathTxtBox.Text = @"C:\Program Files\Spirent Communications\SimPLEX\Scenarios\TA916\GPS_Accuracy_wfs\SimPLEX_GPS_Accuracy_wfs\GPS_Accuracy_wfs.sim";
                        this.simFilePathTxtBox.Enabled = false;
                        this.simFilePathBrowserBtn.Enabled = false;
                    };
                }
                base.Invoke(handler2);
            }
            else
            {
                if (handler3 == null)
                {
                    handler3 = delegate {
                        this.signalSourceComboBox.Enabled = true;
                        this.attenSourceComboBox.Enabled = true;
                        this.attenSourceComboBox.Enabled = true;
                        this.simFilePathTxtBox.Enabled = true;
                        this.simFilePathBrowserBtn.Enabled = true;
                    };
                }
                base.Invoke(handler3);
            }
        }
    }
}

