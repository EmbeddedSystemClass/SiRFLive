﻿namespace SiRFLive.GUI.DeviceControl
{
    using SiRFLive.DeviceControl;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmGPIBCtrl : Form
    {
        private int _BoardID;
        private GPIB_Mgr_Agilent_HP8648C _GPIBCtrl;
        private byte _PrimaryAddress;
        private byte _SecondaryAddress;
        private Button btn_Cfg_Amplitude;
        private Button btn_Clear;
        private Button btn_Close;
        private Button btn_Config_Freq;
        private Button btn_Exit;
        private Button btn_IDN;
        private Button btn_Open;
        private Button btn_Query_Amplitude;
        private Button btn_Query_Output;
        private Button btn_QueryError;
        private Button btn_QueryFreq;
        private Button btn_Reset;
        private Button btn_SelfTest;
        private Button btn_SetRefAmp;
        private CheckBox checkBox_AutoAttenOnOff;
        private CheckBox checkBox_OutPut;
        private CheckBox checkBox_RefState_OnOff;
        private ComboBox comboBox_Amp_Unit;
        private ComboBox comboBox_FreqUnit;
        private IContainer components;
        public ObjectInterface cpGuiCtrl;
        private Label label_Amp_status;
        private Label label_ErrorStatus;
        private Label label_FreqStatus;
        private Label label_IDN;
        private Label label_OutPutStatus;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label8;
        private static frmGPIBCtrl m_SChildform;
        private TextBox textBox_Amplitude;
        private TextBox textBox_AmpRefVal;
        private TextBox textBox_Frequency;
        private TextBox textBox_GPIB_BoardID;
        private TextBox textBox_GPIBPrimaryAddress;

        public frmGPIBCtrl()
        {
            this.cpGuiCtrl = new ObjectInterface();
            try
            {
                this._GPIBCtrl = new GPIB_Mgr_Agilent_HP8648C();
                this.InitializeComponent();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        public frmGPIBCtrl(int boardID, byte primaryAddress, byte secondaryAddress)
        {
            this.cpGuiCtrl = new ObjectInterface();
            try
            {
                this._GPIBCtrl = new GPIB_Mgr_Agilent_HP8648C(boardID, primaryAddress, secondaryAddress);
                this.InitializeComponent();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void btn_AutoAttenOnOff_Click(object sender, EventArgs e)
        {
        }

        private void btn_Cfg_Amplitude_Click(object sender, EventArgs e)
        {
            try
            {
                this._GPIBCtrl.ConfigAmpliude(Convert.ToDouble(this.textBox_Amplitude.Text), (AmpUnit) this.comboBox_Amp_Unit.SelectedIndex);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR");
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            this.clearAllLabels();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            try
            {
                this._GPIBCtrl.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void btn_Config_Freq_Click(object sender, EventArgs e)
        {
            try
            {
                this._GPIBCtrl.ConfigFrequency(Convert.ToDouble(this.textBox_Frequency.Text), (FreqUnit) this.comboBox_FreqUnit.SelectedIndex);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR");
            }
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            try
            {
                base.Close();
                m_SChildform = null;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void btn_IDN_Click(object sender, EventArgs e)
        {
            try
            {
                this.label_IDN.Text = this._GPIBCtrl.getIDN();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
            try
            {
                this._GPIBCtrl.Open();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void btn_Query_Amplitude_Click(object sender, EventArgs e)
        {
            try
            {
                this.label_Amp_status.Text = this._GPIBCtrl.QueryParam_Amplitude().ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void btn_Query_Output_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._GPIBCtrl.QueryParam_Output())
                {
                    this.label_OutPutStatus.Text = "ON";
                }
                else
                {
                    this.label_OutPutStatus.Text = "OFF";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void btn_QueryError_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._GPIBCtrl.Error_Query())
                {
                    this.label_ErrorStatus.Text = "GPIB OK";
                }
                else
                {
                    this.label_ErrorStatus.Text = "GPIB Error!";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void btn_QueryFreq_Click(object sender, EventArgs e)
        {
            try
            {
                this.label_FreqStatus.Text = this._GPIBCtrl.QueryParam_Frequency().ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            try
            {
                this._GPIBCtrl.Reset();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void btn_RFOutPutOnOff_Click(object sender, EventArgs e)
        {
        }

        private void btn_SelfTest_Click(object sender, EventArgs e)
        {
            this._GPIBCtrl.SelfTest();
        }

        private void btn_SetRefAmp_Click(object sender, EventArgs e)
        {
            try
            {
                this._GPIBCtrl.SetRefenceAmpliude(this.checkBox_RefState_OnOff.Checked, Convert.ToDouble(this.textBox_AmpRefVal.Text));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR");
            }
        }

        private void checkBox_AutoAttenOnOff_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this._GPIBCtrl.AutoAttenuation(this.checkBox_AutoAttenOnOff.Checked);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void checkBox_OutPut_CheckedChanged(object sender, EventArgs e)
        {
            this._GPIBCtrl.RF_Output(this.checkBox_OutPut.Checked);
        }

        private void clearAllLabels()
        {
            this.label_Amp_status.Text = "";
            this.label_ErrorStatus.Text = "";
            this.label_FreqStatus.Text = "";
            this.label_OutPutStatus.Text = "";
            this.label_IDN.Text = "";
        }

        private void comboBox_Amp_Unit_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmGPIBCtrl_Load(object sender, EventArgs e)
        {
            try
            {
                this.setDefault();
                this.whenGPIBInfoChange();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        public static frmGPIBCtrl GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmGPIBCtrl();
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmGPIBCtrl));
            this.btn_SelfTest = new Button();
            this.btn_Reset = new Button();
            this.btn_Close = new Button();
            this.btn_Open = new Button();
            this.btn_Cfg_Amplitude = new Button();
            this.btn_Query_Output = new Button();
            this.btn_SetRefAmp = new Button();
            this.btn_Config_Freq = new Button();
            this.btn_Query_Amplitude = new Button();
            this.btn_QueryFreq = new Button();
            this.btn_QueryError = new Button();
            this.btn_IDN = new Button();
            this.checkBox_OutPut = new CheckBox();
            this.checkBox_AutoAttenOnOff = new CheckBox();
            this.checkBox_RefState_OnOff = new CheckBox();
            this.textBox_AmpRefVal = new TextBox();
            this.label1 = new Label();
            this.comboBox_Amp_Unit = new ComboBox();
            this.comboBox_FreqUnit = new ComboBox();
            this.label2 = new Label();
            this.textBox_Amplitude = new TextBox();
            this.label3 = new Label();
            this.textBox_Frequency = new TextBox();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label_IDN = new Label();
            this.label_OutPutStatus = new Label();
            this.label_Amp_status = new Label();
            this.label_FreqStatus = new Label();
            this.label_ErrorStatus = new Label();
            this.btn_Exit = new Button();
            this.btn_Clear = new Button();
            this.textBox_GPIBPrimaryAddress = new TextBox();
            this.label6 = new Label();
            this.textBox_GPIB_BoardID = new TextBox();
            this.label8 = new Label();
            base.SuspendLayout();
            this.btn_SelfTest.Enabled = false;
            this.btn_SelfTest.Location = new Point(0x2e, 0x6d);
            this.btn_SelfTest.Name = "btn_SelfTest";
            this.btn_SelfTest.Size = new Size(0x88, 0x17);
            this.btn_SelfTest.TabIndex = 6;
            this.btn_SelfTest.Text = "Self Test";
            this.btn_SelfTest.UseVisualStyleBackColor = true;
            this.btn_SelfTest.Click += new EventHandler(this.btn_SelfTest_Click);
            this.btn_Reset.Enabled = false;
            this.btn_Reset.Location = new Point(0x2e, 0x95);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new Size(0x88, 0x17);
            this.btn_Reset.TabIndex = 8;
            this.btn_Reset.Text = "Reset";
            this.btn_Reset.UseVisualStyleBackColor = true;
            this.btn_Reset.Click += new EventHandler(this.btn_Reset_Click);
            this.btn_Close.Location = new Point(0x2e, 0x225);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new Size(0x88, 0x17);
            this.btn_Close.TabIndex = 0x1f;
            this.btn_Close.Text = "GPIB Close";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Visible = false;
            this.btn_Close.Click += new EventHandler(this.btn_Close_Click);
            this.btn_Open.Location = new Point(0x2e, 0x45);
            this.btn_Open.Name = "btn_Open";
            this.btn_Open.Size = new Size(0x88, 0x17);
            this.btn_Open.TabIndex = 4;
            this.btn_Open.Text = "GPIB Open";
            this.btn_Open.UseVisualStyleBackColor = true;
            this.btn_Open.Visible = false;
            this.btn_Open.Click += new EventHandler(this.btn_Open_Click);
            this.btn_Cfg_Amplitude.Location = new Point(0x2e, 0x15d);
            this.btn_Cfg_Amplitude.Name = "btn_Cfg_Amplitude";
            this.btn_Cfg_Amplitude.Size = new Size(0x88, 0x17);
            this.btn_Cfg_Amplitude.TabIndex = 0x12;
            this.btn_Cfg_Amplitude.Text = "Config Amplitude";
            this.btn_Cfg_Amplitude.UseVisualStyleBackColor = true;
            this.btn_Cfg_Amplitude.Click += new EventHandler(this.btn_Cfg_Amplitude_Click);
            this.btn_Query_Output.Location = new Point(0x2e, 0x1ad);
            this.btn_Query_Output.Name = "btn_Query_Output";
            this.btn_Query_Output.Size = new Size(0x88, 0x17);
            this.btn_Query_Output.TabIndex = 0x1c;
            this.btn_Query_Output.Text = "Query Output";
            this.btn_Query_Output.UseVisualStyleBackColor = true;
            this.btn_Query_Output.Click += new EventHandler(this.btn_Query_Output_Click);
            this.btn_SetRefAmp.BackgroundImageLayout = ImageLayout.Zoom;
            this.btn_SetRefAmp.Location = new Point(0x2e, 0x135);
            this.btn_SetRefAmp.Name = "btn_SetRefAmp";
            this.btn_SetRefAmp.Size = new Size(0x88, 0x17);
            this.btn_SetRefAmp.TabIndex = 14;
            this.btn_SetRefAmp.Text = "Set Ref Amplitude";
            this.btn_SetRefAmp.UseVisualStyleBackColor = true;
            this.btn_SetRefAmp.Click += new EventHandler(this.btn_SetRefAmp_Click);
            this.btn_Config_Freq.Location = new Point(0x2e, 0x185);
            this.btn_Config_Freq.Name = "btn_Config_Freq";
            this.btn_Config_Freq.Size = new Size(0x88, 0x17);
            this.btn_Config_Freq.TabIndex = 0x17;
            this.btn_Config_Freq.Text = "Config Frequency";
            this.btn_Config_Freq.UseVisualStyleBackColor = true;
            this.btn_Config_Freq.Click += new EventHandler(this.btn_Config_Freq_Click);
            this.btn_Query_Amplitude.Location = new Point(0x2e, 0x1d5);
            this.btn_Query_Amplitude.Name = "btn_Query_Amplitude";
            this.btn_Query_Amplitude.Size = new Size(0x88, 0x17);
            this.btn_Query_Amplitude.TabIndex = 0x1d;
            this.btn_Query_Amplitude.Text = "Query Amplitude";
            this.btn_Query_Amplitude.UseVisualStyleBackColor = true;
            this.btn_Query_Amplitude.Click += new EventHandler(this.btn_Query_Amplitude_Click);
            this.btn_QueryFreq.Location = new Point(0x2e, 0x1fd);
            this.btn_QueryFreq.Name = "btn_QueryFreq";
            this.btn_QueryFreq.Size = new Size(0x88, 0x17);
            this.btn_QueryFreq.TabIndex = 30;
            this.btn_QueryFreq.Text = "Query Frequency";
            this.btn_QueryFreq.UseVisualStyleBackColor = true;
            this.btn_QueryFreq.Click += new EventHandler(this.btn_QueryFreq_Click);
            this.btn_QueryError.Location = new Point(0x2e, 0x10d);
            this.btn_QueryError.Name = "btn_QueryError";
            this.btn_QueryError.Size = new Size(0x88, 0x17);
            this.btn_QueryError.TabIndex = 12;
            this.btn_QueryError.Text = "Error Status";
            this.btn_QueryError.UseVisualStyleBackColor = true;
            this.btn_QueryError.Click += new EventHandler(this.btn_QueryError_Click);
            this.btn_IDN.Location = new Point(0x2e, 0xe5);
            this.btn_IDN.Name = "btn_IDN";
            this.btn_IDN.Size = new Size(0x88, 0x17);
            this.btn_IDN.TabIndex = 10;
            this.btn_IDN.Text = "IDN";
            this.btn_IDN.UseVisualStyleBackColor = true;
            this.btn_IDN.Click += new EventHandler(this.btn_IDN_Click);
            this.checkBox_OutPut.AutoSize = true;
            this.checkBox_OutPut.Location = new Point(0x10b, 0x73);
            this.checkBox_OutPut.Name = "checkBox_OutPut";
            this.checkBox_OutPut.Size = new Size(0x5e, 0x11);
            this.checkBox_OutPut.TabIndex = 7;
            this.checkBox_OutPut.Text = "Output On/Off";
            this.checkBox_OutPut.UseVisualStyleBackColor = true;
            this.checkBox_OutPut.CheckedChanged += new EventHandler(this.checkBox_OutPut_CheckedChanged);
            this.checkBox_AutoAttenOnOff.AutoSize = true;
            this.checkBox_AutoAttenOnOff.Location = new Point(0x10b, 0x4b);
            this.checkBox_AutoAttenOnOff.Name = "checkBox_AutoAttenOnOff";
            this.checkBox_AutoAttenOnOff.Size = new Size(0x8d, 0x11);
            this.checkBox_AutoAttenOnOff.TabIndex = 5;
            this.checkBox_AutoAttenOnOff.Text = "Auto Attenuation On/Off";
            this.checkBox_AutoAttenOnOff.UseVisualStyleBackColor = true;
            this.checkBox_AutoAttenOnOff.CheckedChanged += new EventHandler(this.checkBox_AutoAttenOnOff_CheckedChanged);
            this.checkBox_RefState_OnOff.AutoSize = true;
            this.checkBox_RefState_OnOff.Location = new Point(0x183, 0x135);
            this.checkBox_RefState_OnOff.Name = "checkBox_RefState_OnOff";
            this.checkBox_RefState_OnOff.Size = new Size(0x6b, 0x11);
            this.checkBox_RefState_OnOff.TabIndex = 0x11;
            this.checkBox_RefState_OnOff.Text = "Ref State On/Off";
            this.checkBox_RefState_OnOff.UseVisualStyleBackColor = true;
            this.textBox_AmpRefVal.Location = new Point(0x10b, 0x137);
            this.textBox_AmpRefVal.Name = "textBox_AmpRefVal";
            this.textBox_AmpRefVal.Size = new Size(100, 20);
            this.textBox_AmpRefVal.TabIndex = 0x10;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0xbd, 0x13b);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x48, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "AmpRefValue";
            this.comboBox_Amp_Unit.FormattingEnabled = true;
            this.comboBox_Amp_Unit.Items.AddRange(new object[] { "DBM", "MV", "UV", "MVEMF", "UVEMF", "DBUV", "DBUVEMF" });
            this.comboBox_Amp_Unit.Location = new Point(0x19f, 0x15f);
            this.comboBox_Amp_Unit.Name = "comboBox_Amp_Unit";
            this.comboBox_Amp_Unit.Size = new Size(0x79, 0x15);
            this.comboBox_Amp_Unit.TabIndex = 0x16;
            this.comboBox_Amp_Unit.SelectedIndexChanged += new EventHandler(this.comboBox_Amp_Unit_SelectedIndexChanged);
            this.comboBox_FreqUnit.FormattingEnabled = true;
            this.comboBox_FreqUnit.Items.AddRange(new object[] { "MHZ", "KHZ", "HZ" });
            this.comboBox_FreqUnit.Location = new Point(0x19f, 0x187);
            this.comboBox_FreqUnit.Name = "comboBox_FreqUnit";
            this.comboBox_FreqUnit.Size = new Size(0x79, 0x15);
            this.comboBox_FreqUnit.TabIndex = 0x1b;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0xbd, 0x164);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x35, 13);
            this.label2.TabIndex = 0x13;
            this.label2.Text = "Amplitude";
            this.textBox_Amplitude.Location = new Point(0x10b, 0x160);
            this.textBox_Amplitude.Name = "textBox_Amplitude";
            this.textBox_Amplitude.Size = new Size(100, 20);
            this.textBox_Amplitude.TabIndex = 20;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xbd, 0x18c);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x39, 13);
            this.label3.TabIndex = 0x18;
            this.label3.Text = "Frequency";
            this.textBox_Frequency.Location = new Point(0x10b, 0x188);
            this.textBox_Frequency.Name = "textBox_Frequency";
            this.textBox_Frequency.Size = new Size(100, 20);
            this.textBox_Frequency.TabIndex = 0x19;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x180, 0x163);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x1a, 13);
            this.label4.TabIndex = 0x15;
            this.label4.Text = "Unit";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x180, 0x18b);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x1a, 13);
            this.label5.TabIndex = 0x1a;
            this.label5.Text = "Unit";
            this.label_IDN.AutoSize = true;
            this.label_IDN.Location = new Point(0xd8, 0xea);
            this.label_IDN.Name = "label_IDN";
            this.label_IDN.Size = new Size(0x73, 13);
            this.label_IDN.TabIndex = 11;
            this.label_IDN.Text = "                                    ";
            this.label_OutPutStatus.AutoSize = true;
            this.label_OutPutStatus.Location = new Point(0xd8, 0x1b7);
            this.label_OutPutStatus.Name = "label_OutPutStatus";
            this.label_OutPutStatus.Size = new Size(0x7c, 13);
            this.label_OutPutStatus.TabIndex = 13;
            this.label_OutPutStatus.Text = "                                       ";
            this.label_Amp_status.AutoSize = true;
            this.label_Amp_status.Location = new Point(0xcc, 0x1df);
            this.label_Amp_status.Name = "label_Amp_status";
            this.label_Amp_status.Size = new Size(0x79, 13);
            this.label_Amp_status.TabIndex = 13;
            this.label_Amp_status.Text = "                                      ";
            this.label_FreqStatus.AutoSize = true;
            this.label_FreqStatus.Location = new Point(0xd8, 0x207);
            this.label_FreqStatus.Name = "label_FreqStatus";
            this.label_FreqStatus.Size = new Size(0x67, 13);
            this.label_FreqStatus.TabIndex = 13;
            this.label_FreqStatus.Text = "                                ";
            this.label_ErrorStatus.AutoSize = true;
            this.label_ErrorStatus.Location = new Point(0xe9, 0x112);
            this.label_ErrorStatus.Name = "label_ErrorStatus";
            this.label_ErrorStatus.Size = new Size(0x35, 13);
            this.label_ErrorStatus.TabIndex = 13;
            this.label_ErrorStatus.Text = "...status...";
            this.btn_Exit.DialogResult = DialogResult.Cancel;
            this.btn_Exit.Location = new Point(0x1cd, 0x225);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new Size(0x4b, 0x17);
            this.btn_Exit.TabIndex = 0x20;
            this.btn_Exit.Text = "&Exit";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new EventHandler(this.btn_Exit_Click);
            this.btn_Clear.Location = new Point(0x2e, 0xbd);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new Size(0x88, 0x17);
            this.btn_Clear.TabIndex = 9;
            this.btn_Clear.Text = "Clear Static Text";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new EventHandler(this.btn_Clear_Click);
            this.textBox_GPIBPrimaryAddress.Location = new Point(0x183, 0x1b);
            this.textBox_GPIBPrimaryAddress.Name = "textBox_GPIBPrimaryAddress";
            this.textBox_GPIBPrimaryAddress.Size = new Size(0x25, 20);
            this.textBox_GPIBPrimaryAddress.TabIndex = 3;
            this.textBox_GPIBPrimaryAddress.Text = "7";
            this.textBox_GPIBPrimaryAddress.TextChanged += new EventHandler(this.textBox_GPIBPrimaryAddress_TextChanged);
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x108, 0x1f);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x71, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "GPIB Primary Address:";
            this.textBox_GPIB_BoardID.Location = new Point(0x6a, 0x1b);
            this.textBox_GPIB_BoardID.Name = "textBox_GPIB_BoardID";
            this.textBox_GPIB_BoardID.Size = new Size(0x4c, 20);
            this.textBox_GPIB_BoardID.TabIndex = 1;
            this.textBox_GPIB_BoardID.Text = "0";
            this.textBox_GPIB_BoardID.TextChanged += new EventHandler(this.textBox_GPIB_BoardID_TextChanged);
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x2e, 0x1f);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x34, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Board ID:";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScroll = true;
            base.CancelButton = this.btn_Exit;
            base.ClientSize = new Size(600, 0x256);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.textBox_GPIB_BoardID);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.textBox_GPIBPrimaryAddress);
            base.Controls.Add(this.btn_Clear);
            base.Controls.Add(this.btn_Exit);
            base.Controls.Add(this.label_ErrorStatus);
            base.Controls.Add(this.label_FreqStatus);
            base.Controls.Add(this.label_Amp_status);
            base.Controls.Add(this.label_OutPutStatus);
            base.Controls.Add(this.label_IDN);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.textBox_Frequency);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.textBox_Amplitude);
            base.Controls.Add(this.comboBox_FreqUnit);
            base.Controls.Add(this.comboBox_Amp_Unit);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.textBox_AmpRefVal);
            base.Controls.Add(this.checkBox_RefState_OnOff);
            base.Controls.Add(this.checkBox_AutoAttenOnOff);
            base.Controls.Add(this.checkBox_OutPut);
            base.Controls.Add(this.btn_QueryError);
            base.Controls.Add(this.btn_QueryFreq);
            base.Controls.Add(this.btn_Query_Amplitude);
            base.Controls.Add(this.btn_Query_Output);
            base.Controls.Add(this.btn_Config_Freq);
            base.Controls.Add(this.btn_SetRefAmp);
            base.Controls.Add(this.btn_Cfg_Amplitude);
            base.Controls.Add(this.btn_Open);
            base.Controls.Add(this.btn_Close);
            base.Controls.Add(this.btn_Reset);
            base.Controls.Add(this.btn_IDN);
            base.Controls.Add(this.btn_SelfTest);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmGPIBCtrl";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            this.Text = "Agilent 8648C Signal Generator Control";
            base.Load += new EventHandler(this.frmGPIBCtrl_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            m_SChildform = null;
        }

        private void setDefault()
        {
            this.cpGuiCtrl.SetTextBoxText(this.textBox_Amplitude, "-126");
            this.cpGuiCtrl.SetTextBoxText(this.textBox_AmpRefVal, "0");
            this.cpGuiCtrl.SetTextBoxText(this.textBox_Frequency, "101");
            this.cpGuiCtrl.SetComboBoxSelectItem(this.comboBox_Amp_Unit, "DBM");
            this.cpGuiCtrl.SetComboBoxSelectItem(this.comboBox_FreqUnit, "MHZ");
        }

        private void textBox_GPIB_BoardID_TextChanged(object sender, EventArgs e)
        {
            this.whenGPIBInfoChange();
        }

        private void textBox_GPIBPrimaryAddress_TextChanged(object sender, EventArgs e)
        {
            this.whenGPIBInfoChange();
        }

        private void whenGPIBInfoChange()
        {
            if ((this.textBox_GPIB_BoardID.Text != "") && (this.textBox_GPIBPrimaryAddress.Text != ""))
            {
                try
                {
                    int boardID = Convert.ToInt32(this.textBox_GPIB_BoardID.Text);
                    byte primaryAddress = Convert.ToByte(this.textBox_GPIBPrimaryAddress.Text);
                    this._GPIBCtrl = new GPIB_Mgr_Agilent_HP8648C(boardID, primaryAddress, 0);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "ERROR");
                }
            }
        }
    }
}

