﻿namespace SiRFLive.GUI.DeviceControl
{
    using SiRFLive.Configuration;
    using SiRFLive.DeviceControl;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmSimplexCtrl : Form
    {
        private Ini_GUI _SimplexGUI = new Ini_GUI(@"..\Config\GUI_value_init.xml");
        private IContainer components;
        private Button endScenarioBtn;
        private CheckBox frmSimplexAbsoluteChk;
        private CheckBox frmSimplexAllChk;
        private CheckBox frmSimplexByChanChk;
        private Label frmSimplexChanSvLabel;
        private TextBox frmSimplexChanSVVal;
        private Button frmSimplexGetSimTimeBtn;
        private Label frmSimplexLevelLabel;
        private TextBox frmSimplexLevelVal;
        private Button frmSimplexPowerLevelBtn;
        private CheckBox frmSimplexPowerOnOffChk;
        private Button frmSimplexSetPowerBtn;
        private Label frmSimplexSimErrLabel;
        private Label frmSimplexSimStatusLabel;
        private Label frmSimplexTimeIntoRunLabel;
        private GroupBox groupBox1;
        private Label label_error;
        private Label label_simStatus;
        private Label label_status;
        private Label label1;
        private Label label2;
        private static frmSimplexCtrl m_SChildform;
        private Button selectScenarioBtn;
        private Simplex sim = new Simplex();
        public SiRFLiveEvent simDoneEvent = new SiRFLiveEvent();
        private Button SimExitBtn;
        private Button simFileBrowser;
        private TextBox simFilePathVal;
        public ObjectInterface simGuiCtrl = new ObjectInterface();
        private TextBox simIPAddress;
        private TextBox simPortVal;
        private Button simStatusBtn;
        private Button startScenarioBtn;

        public frmSimplexCtrl()
        {
            this.InitializeComponent();
            this.simIPAddress.Text = this._SimplexGUI.LoadControlDataFromFile("Simplex", "IPAddress");
            this.simPortVal.Text = this._SimplexGUI.LoadControlDataFromFile("Simplex", "Port");
            this.simFilePathVal.Text = this._SimplexGUI.LoadControlDataFromFile("Simplex", "Scenario");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void endScenarioBtn_Click(object sender, EventArgs e)
        {
            this.sim.IPAddress = this.simIPAddress.Text;
            this.sim.Port = this.simPortVal.Text;
            this.sim.PopUps(false);
            string inputStr = this.sim.EndScenario(false);
            this.sim.StatusCodeToDescription(inputStr);
            string str2 = this.sim.SimPlex_ParseError(inputStr);
            this.label_error.Text = str2;
        }

        private void frmSimplexGetSimTimeBtn_Click(object sender, EventArgs e)
        {
            this.sim.IPAddress = this.simIPAddress.Text;
            this.sim.Port = this.simPortVal.Text;
            string str = this.sim.GetTimeIntoRun() + " seconds into run";
            this.frmSimplexTimeIntoRunLabel.Text = str;
        }

        private void frmSimplexPowerLevelBtn_Click(object sender, EventArgs e)
        {
            this.sim.IPAddress = this.simIPAddress.Text;
            this.sim.Port = this.simPortVal.Text;
            this.label_status.Text = string.Empty;
            if (this.frmSimplexChanSVVal.Text.Length == 0)
            {
                MessageBox.Show("Chan/SV box is blank!", "Error");
            }
            else if (this.frmSimplexLevelVal.Text.Length == 0)
            {
                MessageBox.Show("Level box is blank!", "Error");
            }
            else
            {
                try
                {
                    string inputStr = this.sim.SetPowerLevel("-", (float) Convert.ToDouble(this.frmSimplexLevelVal.Text), Convert.ToUInt32(this.frmSimplexChanSVVal.Text), this.frmSimplexByChanChk.Checked, this.frmSimplexAllChk.Checked, this.frmSimplexAbsoluteChk.Checked);
                    string str2 = this.sim.StatusCodeToDescription(inputStr);
                    this.label_status.Text = str2;
                    string str3 = this.sim.SimPlex_ParseError(inputStr);
                    this.label_error.Text = str3;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error");
                }
            }
        }

        private void frmSimplexSetPowerBtn_Click(object sender, EventArgs e)
        {
            this.sim.IPAddress = this.simIPAddress.Text;
            this.sim.Port = this.simPortVal.Text;
            this.label_status.Text = string.Empty;
            if (this.frmSimplexChanSVVal.Text.Length == 0)
            {
                MessageBox.Show("Chan/SV box is blank!", "Error");
            }
            else
            {
                try
                {
                    string inputStr = this.sim.SetPowerOnOff("-", Convert.ToUInt32(this.frmSimplexChanSVVal.Text), this.frmSimplexPowerOnOffChk.Checked, this.frmSimplexByChanChk.Checked, this.frmSimplexAllChk.Checked);
                    string str2 = this.sim.StatusCodeToDescription(inputStr);
                    this.label_status.Text = str2;
                    string str3 = this.sim.SimPlex_ParseError(inputStr);
                    this.label_error.Text = str3;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error");
                }
            }
        }

        public static frmSimplexCtrl GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmSimplexCtrl();
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmSimplexCtrl));
            this.simFileBrowser = new Button();
            this.selectScenarioBtn = new Button();
            this.label_simStatus = new Label();
            this.simStatusBtn = new Button();
            this.endScenarioBtn = new Button();
            this.startScenarioBtn = new Button();
            this.SimExitBtn = new Button();
            this.simFilePathVal = new TextBox();
            this.simIPAddress = new TextBox();
            this.simPortVal = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label_error = new Label();
            this.label_status = new Label();
            this.frmSimplexSimStatusLabel = new Label();
            this.frmSimplexSimErrLabel = new Label();
            this.frmSimplexChanSvLabel = new Label();
            this.frmSimplexChanSVVal = new TextBox();
            this.frmSimplexGetSimTimeBtn = new Button();
            this.frmSimplexSetPowerBtn = new Button();
            this.frmSimplexPowerLevelBtn = new Button();
            this.frmSimplexByChanChk = new CheckBox();
            this.frmSimplexAllChk = new CheckBox();
            this.frmSimplexAbsoluteChk = new CheckBox();
            this.frmSimplexPowerOnOffChk = new CheckBox();
            this.frmSimplexLevelLabel = new Label();
            this.frmSimplexLevelVal = new TextBox();
            this.groupBox1 = new GroupBox();
            this.frmSimplexTimeIntoRunLabel = new Label();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.simFileBrowser.Location = new Point(0x13, 0x2f);
            this.simFileBrowser.Name = "simFileBrowser";
            this.simFileBrowser.Size = new Size(0x1a, 0x17);
            this.simFileBrowser.TabIndex = 4;
            this.simFileBrowser.Text = "...";
            this.simFileBrowser.UseVisualStyleBackColor = true;
            this.simFileBrowser.Click += new EventHandler(this.simFileBrowser_Click);
            this.selectScenarioBtn.Location = new Point(0x13, 0x4f);
            this.selectScenarioBtn.Name = "selectScenarioBtn";
            this.selectScenarioBtn.Size = new Size(130, 0x17);
            this.selectScenarioBtn.TabIndex = 6;
            this.selectScenarioBtn.Text = "Select Scenario";
            this.selectScenarioBtn.UseVisualStyleBackColor = true;
            this.selectScenarioBtn.Click += new EventHandler(this.selectScenarioBtn_Click);
            this.label_simStatus.AutoSize = true;
            this.label_simStatus.Location = new Point(0x72, 0x170);
            this.label_simStatus.Name = "label_simStatus";
            this.label_simStatus.Size = new Size(0, 13);
            this.label_simStatus.TabIndex = 0x11;
            this.simStatusBtn.Location = new Point(0x13, 0x70);
            this.simStatusBtn.Name = "simStatusBtn";
            this.simStatusBtn.Size = new Size(130, 0x17);
            this.simStatusBtn.TabIndex = 9;
            this.simStatusBtn.Text = "Get Sim Status";
            this.simStatusBtn.UseVisualStyleBackColor = true;
            this.simStatusBtn.Click += new EventHandler(this.simStatusBtn_Click);
            this.endScenarioBtn.BackColor = SystemColors.Control;
            this.endScenarioBtn.Location = new Point(0xa6, 0x70);
            this.endScenarioBtn.Name = "endScenarioBtn";
            this.endScenarioBtn.Size = new Size(130, 0x17);
            this.endScenarioBtn.TabIndex = 10;
            this.endScenarioBtn.Text = "End Scenario";
            this.endScenarioBtn.UseVisualStyleBackColor = false;
            this.endScenarioBtn.Click += new EventHandler(this.endScenarioBtn_Click);
            this.startScenarioBtn.Location = new Point(0x13, 0x91);
            this.startScenarioBtn.Name = "startScenarioBtn";
            this.startScenarioBtn.Size = new Size(130, 0x17);
            this.startScenarioBtn.TabIndex = 11;
            this.startScenarioBtn.Text = "Start Scenario";
            this.startScenarioBtn.UseVisualStyleBackColor = true;
            this.startScenarioBtn.Click += new EventHandler(this.startScenarioBtn_Click);
            this.SimExitBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SimExitBtn.Location = new Point(0x1d0, 0x16b);
            this.SimExitBtn.Name = "SimExitBtn";
            this.SimExitBtn.Size = new Size(0x3b, 0x17);
            this.SimExitBtn.TabIndex = 0x1b;
            this.SimExitBtn.Text = "E&xit";
            this.SimExitBtn.UseVisualStyleBackColor = true;
            this.SimExitBtn.Click += new EventHandler(this.SimExitBtn_Click);
            this.simFilePathVal.Location = new Point(0x34, 50);
            this.simFilePathVal.Name = "simFilePathVal";
            this.simFilePathVal.Size = new Size(0x1d7, 20);
            this.simFilePathVal.TabIndex = 5;
            this.simFilePathVal.Text = @"c:\Program Files\Spirent Communications\SimPLEX\Scenarios\3GPP\Test2-NominalAccuracy\3GPP_Nominal_01\Spirent_3GPP_Normal_Accuracy_1.sim";
            this.simIPAddress.Location = new Point(110, 13);
            this.simIPAddress.Name = "simIPAddress";
            this.simIPAddress.Size = new Size(0x100, 20);
            this.simIPAddress.TabIndex = 1;
            this.simIPAddress.Text = "192.168.52.000";
            this.simPortVal.Location = new Point(0x1da, 13);
            this.simPortVal.Name = "simPortVal";
            this.simPortVal.Size = new Size(0x31, 20);
            this.simPortVal.TabIndex = 3;
            this.simPortVal.Text = "15650";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x19, 0x11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4e, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sim IP Address";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x192, 0x11);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2e, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Sim Port";
            this.label_error.AutoSize = true;
            this.label_error.Location = new Point(0x72, 0x164);
            this.label_error.Name = "label_error";
            this.label_error.Size = new Size(0x1c, 13);
            this.label_error.TabIndex = 0x1a;
            this.label_error.Text = "error";
            this.label_status.AutoSize = true;
            this.label_status.Location = new Point(0x72, 0x14e);
            this.label_status.Name = "label_status";
            this.label_status.Size = new Size(0x23, 13);
            this.label_status.TabIndex = 0x18;
            this.label_status.Text = "status";
            this.frmSimplexSimStatusLabel.AutoSize = true;
            this.frmSimplexSimStatusLabel.Location = new Point(0x24, 0x14e);
            this.frmSimplexSimStatusLabel.Name = "frmSimplexSimStatusLabel";
            this.frmSimplexSimStatusLabel.Size = new Size(60, 13);
            this.frmSimplexSimStatusLabel.TabIndex = 0x17;
            this.frmSimplexSimStatusLabel.Text = "Sim Status:";
            this.frmSimplexSimErrLabel.AutoSize = true;
            this.frmSimplexSimErrLabel.Location = new Point(0x24, 0x164);
            this.frmSimplexSimErrLabel.Name = "frmSimplexSimErrLabel";
            this.frmSimplexSimErrLabel.Size = new Size(0x49, 13);
            this.frmSimplexSimErrLabel.TabIndex = 0x19;
            this.frmSimplexSimErrLabel.Text = "Sim error msg:";
            this.frmSimplexChanSvLabel.AutoSize = true;
            this.frmSimplexChanSvLabel.Location = new Point(0x163, 0x1d);
            this.frmSimplexChanSvLabel.Name = "frmSimplexChanSvLabel";
            this.frmSimplexChanSvLabel.Size = new Size(0x33, 13);
            this.frmSimplexChanSvLabel.TabIndex = 0x13;
            this.frmSimplexChanSvLabel.Text = "Chan/SV";
            this.frmSimplexChanSVVal.Location = new Point(0x19c, 0x19);
            this.frmSimplexChanSVVal.Name = "frmSimplexChanSVVal";
            this.frmSimplexChanSVVal.Size = new Size(0x26, 20);
            this.frmSimplexChanSVVal.TabIndex = 20;
            this.frmSimplexGetSimTimeBtn.Location = new Point(0xa6, 0x4f);
            this.frmSimplexGetSimTimeBtn.Name = "frmSimplexGetSimTimeBtn";
            this.frmSimplexGetSimTimeBtn.Size = new Size(130, 0x17);
            this.frmSimplexGetSimTimeBtn.TabIndex = 7;
            this.frmSimplexGetSimTimeBtn.Text = "Get Sim Time";
            this.frmSimplexGetSimTimeBtn.UseVisualStyleBackColor = true;
            this.frmSimplexGetSimTimeBtn.Click += new EventHandler(this.frmSimplexGetSimTimeBtn_Click);
            this.frmSimplexSetPowerBtn.Location = new Point(0x12, 0x21);
            this.frmSimplexSetPowerBtn.Name = "frmSimplexSetPowerBtn";
            this.frmSimplexSetPowerBtn.Size = new Size(0x6a, 0x17);
            this.frmSimplexSetPowerBtn.TabIndex = 12;
            this.frmSimplexSetPowerBtn.Text = "Power On/Off";
            this.frmSimplexSetPowerBtn.UseVisualStyleBackColor = true;
            this.frmSimplexSetPowerBtn.Click += new EventHandler(this.frmSimplexSetPowerBtn_Click);
            this.frmSimplexPowerLevelBtn.Location = new Point(0x12, 0x47);
            this.frmSimplexPowerLevelBtn.Name = "frmSimplexPowerLevelBtn";
            this.frmSimplexPowerLevelBtn.Size = new Size(0x6a, 0x17);
            this.frmSimplexPowerLevelBtn.TabIndex = 15;
            this.frmSimplexPowerLevelBtn.Text = "Power Level";
            this.frmSimplexPowerLevelBtn.UseVisualStyleBackColor = true;
            this.frmSimplexPowerLevelBtn.Click += new EventHandler(this.frmSimplexPowerLevelBtn_Click);
            this.frmSimplexByChanChk.AutoSize = true;
            this.frmSimplexByChanChk.Location = new Point(0x19c, 0x4d);
            this.frmSimplexByChanChk.Name = "frmSimplexByChanChk";
            this.frmSimplexByChanChk.Size = new Size(0x48, 0x11);
            this.frmSimplexByChanChk.TabIndex = 0x16;
            this.frmSimplexByChanChk.Text = "By Chan?";
            this.frmSimplexByChanChk.UseVisualStyleBackColor = true;
            this.frmSimplexAllChk.AutoSize = true;
            this.frmSimplexAllChk.Location = new Point(0x19c, 0x36);
            this.frmSimplexAllChk.Name = "frmSimplexAllChk";
            this.frmSimplexAllChk.Size = new Size(0x2b, 0x11);
            this.frmSimplexAllChk.TabIndex = 0x15;
            this.frmSimplexAllChk.Text = "All?";
            this.frmSimplexAllChk.UseVisualStyleBackColor = true;
            this.frmSimplexAbsoluteChk.AutoSize = true;
            this.frmSimplexAbsoluteChk.Location = new Point(0x114, 0x4a);
            this.frmSimplexAbsoluteChk.Name = "frmSimplexAbsoluteChk";
            this.frmSimplexAbsoluteChk.Size = new Size(0x49, 0x11);
            this.frmSimplexAbsoluteChk.TabIndex = 0x12;
            this.frmSimplexAbsoluteChk.Text = "Absolute?";
            this.frmSimplexAbsoluteChk.UseVisualStyleBackColor = true;
            this.frmSimplexPowerOnOffChk.AutoSize = true;
            this.frmSimplexPowerOnOffChk.Location = new Point(0xa5, 0x27);
            this.frmSimplexPowerOnOffChk.Name = "frmSimplexPowerOnOffChk";
            this.frmSimplexPowerOnOffChk.Size = new Size(0x2e, 0x11);
            this.frmSimplexPowerOnOffChk.TabIndex = 13;
            this.frmSimplexPowerOnOffChk.Text = "On?";
            this.frmSimplexPowerOnOffChk.UseVisualStyleBackColor = true;
            this.frmSimplexLevelLabel.AutoSize = true;
            this.frmSimplexLevelLabel.Location = new Point(0xa4, 0x4c);
            this.frmSimplexLevelLabel.Name = "frmSimplexLevelLabel";
            this.frmSimplexLevelLabel.Size = new Size(0x21, 13);
            this.frmSimplexLevelLabel.TabIndex = 0x10;
            this.frmSimplexLevelLabel.Text = "Level";
            this.frmSimplexLevelVal.Location = new Point(0xcb, 0x48);
            this.frmSimplexLevelVal.Name = "frmSimplexLevelVal";
            this.frmSimplexLevelVal.Size = new Size(0x23, 20);
            this.frmSimplexLevelVal.TabIndex = 0x11;
            this.groupBox1.Controls.Add(this.frmSimplexLevelVal);
            this.groupBox1.Controls.Add(this.frmSimplexChanSVVal);
            this.groupBox1.Controls.Add(this.frmSimplexLevelLabel);
            this.groupBox1.Controls.Add(this.frmSimplexChanSvLabel);
            this.groupBox1.Controls.Add(this.frmSimplexPowerOnOffChk);
            this.groupBox1.Controls.Add(this.frmSimplexAbsoluteChk);
            this.groupBox1.Controls.Add(this.frmSimplexAllChk);
            this.groupBox1.Controls.Add(this.frmSimplexByChanChk);
            this.groupBox1.Controls.Add(this.frmSimplexPowerLevelBtn);
            this.groupBox1.Controls.Add(this.frmSimplexSetPowerBtn);
            this.groupBox1.Location = new Point(0x13, 0xb9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x1f8, 0x72);
            this.groupBox1.TabIndex = 0x30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Power Control";
            this.frmSimplexTimeIntoRunLabel.AutoSize = true;
            this.frmSimplexTimeIntoRunLabel.Location = new Point(0x134, 0x59);
            this.frmSimplexTimeIntoRunLabel.Name = "frmSimplexTimeIntoRunLabel";
            this.frmSimplexTimeIntoRunLabel.Size = new Size(140, 13);
            this.frmSimplexTimeIntoRunLabel.TabIndex = 8;
            this.frmSimplexTimeIntoRunLabel.Text = "sec into run                          ";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.CancelButton = this.SimExitBtn;
            base.ClientSize = new Size(0x21e, 0x18e);
            base.Controls.Add(this.frmSimplexTimeIntoRunLabel);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.frmSimplexSimErrLabel);
            base.Controls.Add(this.frmSimplexSimStatusLabel);
            base.Controls.Add(this.label_status);
            base.Controls.Add(this.label_error);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.simPortVal);
            base.Controls.Add(this.simIPAddress);
            base.Controls.Add(this.simFilePathVal);
            base.Controls.Add(this.SimExitBtn);
            base.Controls.Add(this.startScenarioBtn);
            base.Controls.Add(this.endScenarioBtn);
            base.Controls.Add(this.frmSimplexGetSimTimeBtn);
            base.Controls.Add(this.simStatusBtn);
            base.Controls.Add(this.selectScenarioBtn);
            base.Controls.Add(this.simFileBrowser);
            base.Controls.Add(this.label_simStatus);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmSimplexCtrl";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SimPLEX Control";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            this.saveNExit();
            m_SChildform = null;
        }

        private void saveNExit()
        {
            this._SimplexGUI.SaveControlDataToFile("Simplex", "IPAddress", this.simIPAddress.Text);
            this._SimplexGUI.SaveControlDataToFile("Simplex", "Port", this.simPortVal.Text);
            this._SimplexGUI.SaveControlDataToFile("Simplex", "Scenario", this.simFilePathVal.Text);
        }

        private void selectScenarioBtn_Click(object sender, EventArgs e)
        {
            this.sim.IPAddress = this.simIPAddress.Text;
            this.sim.Port = this.simPortVal.Text;
            this.sim.PopUps(false);
            string text = this.simFilePathVal.Text;
            string inputStr = this.sim.SelectScenario(text);
            string str3 = this.sim.StatusCodeToDescription(inputStr);
            this.label_status.Text = str3;
            string str4 = this.sim.SimPlex_ParseError(inputStr);
            this.label_error.Text = str4;
        }

        private void SimExitBtn_Click(object sender, EventArgs e)
        {
            this.saveNExit();
            base.Close();
            m_SChildform = null;
        }

        private void simFileBrowser_Click(object sender, EventArgs e)
        {
            this.simGuiCtrl.FileBrowser(this.simFilePathVal);
        }

        private void simStatusBtn_Click(object sender, EventArgs e)
        {
            this.sim.IPAddress = this.simIPAddress.Text;
            this.sim.Port = this.simPortVal.Text;
            string simStatus = this.sim.GetSimStatus();
            this.label_status.Text = simStatus;
        }

        private void startScenarioBtn_Click(object sender, EventArgs e)
        {
            this.sim.IPAddress = this.simIPAddress.Text;
            this.sim.Port = this.simPortVal.Text;
            this.sim.PopUps(false);
            string inputStr = this.sim.RunScenario();
            string str2 = this.sim.StatusCodeToDescription(inputStr);
            this.label_status.Text = str2;
            string str3 = this.sim.SimPlex_ParseError(inputStr);
            this.label_error.Text = str3;
        }
    }
}

