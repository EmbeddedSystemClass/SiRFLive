﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class frmDGPS : Form
    {
        private Button button_Cancel;
        private Button button_Send;
        private CommunicationManager comm;
        private IContainer components;
        private RadioButton DGPS_TimeoutSource_DefaultRadioBtn;
        private RadioButton DGPS_TimeoutSource_UserDefinedRadioBtn;
        private RadioButton DGPSMode_AutomaticRadioBtn;
        private RadioButton DGPSMode_ExclusiveRadioBtn;
        private GroupBox DGPSMode_GrpBox;
        private RadioButton DGPSMode_NeverUseRadioBtn;
        private CheckBox DGPSParam_AutoScan_checkbox;
        private GroupBox DGPSParameters_GrpBox;
        private RadioButton DGPSParams_IntegrityRadioBtn;
        private RadioButton DGPSParams_TestingRadioBtn;
        private GroupBox DGPSSource_GrpBox;
        private RadioButton DGPSSource_NoneRadioBtn;
        private RadioButton DGPSSource_SBASRadioBtn;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label labelPRN;
        private Label labelTimeout;
        private TextBox textBox_DGPS_PRN;
        private TextBox textBox_DGPStimeout;

        public frmDGPS()
        {
            this.InitializeComponent();
        }

        public frmDGPS(CommunicationManager parentComm)
        {
            this.InitializeComponent();
            this.comm = parentComm;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        this.SetDGPSSource();
                        this.SetSBASParameters();
                        this.SetDGPSControl();
                        base.Close();
                    }
                }
                clsGlobal.PerformOnAll = false;
            }
            else if (this.comm.IsSourceDeviceOpen())
            {
                this.SetDGPSSource();
                this.SetSBASParameters();
                this.SetDGPSControl();
                base.Close();
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void DGPS_TimeoutSource_DefaultRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.DGPS_TimeoutSource_DefaultRadioBtn.Checked)
            {
                this.textBox_DGPStimeout.Enabled = false;
                this.labelTimeout.Enabled = false;
            }
            else
            {
                this.textBox_DGPStimeout.Enabled = true;
                this.labelTimeout.Enabled = true;
            }
        }

        private void DGPSParam_AutoScan_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.DGPSParam_AutoScan_checkbox.Checked)
            {
                this.textBox_DGPS_PRN.Enabled = false;
                this.labelPRN.Enabled = false;
                this.textBox_DGPS_PRN.Text = "";
            }
            else
            {
                this.textBox_DGPS_PRN.Enabled = true;
                this.labelPRN.Enabled = true;
            }
        }

        private void DGPSSource_NoneRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.DGPSSource_NoneRadioBtn.Checked)
            {
                this.DGPSMode_GrpBox.Enabled = false;
                this.DGPSParameters_GrpBox.Enabled = false;
            }
            else
            {
                this.DGPSMode_GrpBox.Enabled = true;
                this.DGPSParameters_GrpBox.Enabled = true;
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

        private void frmDGPS_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((base.DialogResult != DialogResult.Cancel) && !this.DGPSParam_AutoScan_checkbox.Checked) && (this.textBox_DGPS_PRN.Text == ""))
            {
                MessageBox.Show("Please enter a valid PRN", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
        }

        private void frmDGPS_Load(object sender, EventArgs e)
        {
            this.DGPSMode_AutomaticRadioBtn.Checked = true;
            this.DGPS_TimeoutSource_DefaultRadioBtn.Checked = true;
            this.DGPSParams_TestingRadioBtn.Checked = true;
            this.DGPSParam_AutoScan_checkbox.Checked = true;
            this.textBox_DGPS_PRN.Enabled = false;
            this.labelPRN.Enabled = false;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmDGPS));
            this.DGPSSource_GrpBox = new GroupBox();
            this.DGPSSource_NoneRadioBtn = new RadioButton();
            this.DGPSSource_SBASRadioBtn = new RadioButton();
            this.DGPSMode_GrpBox = new GroupBox();
            this.labelTimeout = new Label();
            this.textBox_DGPStimeout = new TextBox();
            this.DGPSMode_NeverUseRadioBtn = new RadioButton();
            this.DGPSMode_ExclusiveRadioBtn = new RadioButton();
            this.DGPSMode_AutomaticRadioBtn = new RadioButton();
            this.DGPSParameters_GrpBox = new GroupBox();
            this.groupBox2 = new GroupBox();
            this.DGPSParams_TestingRadioBtn = new RadioButton();
            this.DGPSParams_IntegrityRadioBtn = new RadioButton();
            this.groupBox1 = new GroupBox();
            this.DGPS_TimeoutSource_UserDefinedRadioBtn = new RadioButton();
            this.DGPS_TimeoutSource_DefaultRadioBtn = new RadioButton();
            this.textBox_DGPS_PRN = new TextBox();
            this.labelPRN = new Label();
            this.DGPSParam_AutoScan_checkbox = new CheckBox();
            this.button_Send = new Button();
            this.button_Cancel = new Button();
            this.DGPSSource_GrpBox.SuspendLayout();
            this.DGPSMode_GrpBox.SuspendLayout();
            this.DGPSParameters_GrpBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.DGPSSource_GrpBox.Controls.Add(this.DGPSSource_NoneRadioBtn);
            this.DGPSSource_GrpBox.Controls.Add(this.DGPSSource_SBASRadioBtn);
            this.DGPSSource_GrpBox.Location = new Point(13, 13);
            this.DGPSSource_GrpBox.Name = "DGPSSource_GrpBox";
            this.DGPSSource_GrpBox.Size = new Size(200, 50);
            this.DGPSSource_GrpBox.TabIndex = 0;
            this.DGPSSource_GrpBox.TabStop = false;
            this.DGPSSource_GrpBox.Text = "Source";
            this.DGPSSource_NoneRadioBtn.AutoSize = true;
            this.DGPSSource_NoneRadioBtn.Location = new Point(0x6d, 20);
            this.DGPSSource_NoneRadioBtn.Name = "DGPSSource_NoneRadioBtn";
            this.DGPSSource_NoneRadioBtn.Size = new Size(0x33, 0x11);
            this.DGPSSource_NoneRadioBtn.TabIndex = 1;
            this.DGPSSource_NoneRadioBtn.TabStop = true;
            this.DGPSSource_NoneRadioBtn.Text = "&None";
            this.DGPSSource_NoneRadioBtn.UseVisualStyleBackColor = true;
            this.DGPSSource_NoneRadioBtn.CheckedChanged += new EventHandler(this.DGPSSource_NoneRadioBtn_CheckedChanged);
            this.DGPSSource_SBASRadioBtn.AutoSize = true;
            this.DGPSSource_SBASRadioBtn.Location = new Point(0x1a, 20);
            this.DGPSSource_SBASRadioBtn.Name = "DGPSSource_SBASRadioBtn";
            this.DGPSSource_SBASRadioBtn.Size = new Size(0x35, 0x11);
            this.DGPSSource_SBASRadioBtn.TabIndex = 0;
            this.DGPSSource_SBASRadioBtn.TabStop = true;
            this.DGPSSource_SBASRadioBtn.Text = "&SBAS";
            this.DGPSSource_SBASRadioBtn.UseVisualStyleBackColor = true;
            this.DGPSMode_GrpBox.Controls.Add(this.labelTimeout);
            this.DGPSMode_GrpBox.Controls.Add(this.textBox_DGPStimeout);
            this.DGPSMode_GrpBox.Controls.Add(this.DGPSMode_NeverUseRadioBtn);
            this.DGPSMode_GrpBox.Controls.Add(this.DGPSMode_ExclusiveRadioBtn);
            this.DGPSMode_GrpBox.Controls.Add(this.DGPSMode_AutomaticRadioBtn);
            this.DGPSMode_GrpBox.Location = new Point(13, 70);
            this.DGPSMode_GrpBox.Name = "DGPSMode_GrpBox";
            this.DGPSMode_GrpBox.Size = new Size(0x10b, 100);
            this.DGPSMode_GrpBox.TabIndex = 1;
            this.DGPSMode_GrpBox.TabStop = false;
            this.DGPSMode_GrpBox.Text = "Mode";
            this.labelTimeout.AutoSize = true;
            this.labelTimeout.Location = new Point(0xad, 0x22);
            this.labelTimeout.Name = "labelTimeout";
            this.labelTimeout.Size = new Size(0x47, 13);
            this.labelTimeout.TabIndex = 4;
            this.labelTimeout.Text = "Timeout (sec)";
            this.textBox_DGPStimeout.BackColor = SystemColors.Window;
            this.textBox_DGPStimeout.Location = new Point(0xbb, 50);
            this.textBox_DGPStimeout.Name = "textBox_DGPStimeout";
            this.textBox_DGPStimeout.Size = new Size(0x30, 20);
            this.textBox_DGPStimeout.TabIndex = 3;
            this.textBox_DGPStimeout.Text = "30";
            this.DGPSMode_NeverUseRadioBtn.AutoSize = true;
            this.DGPSMode_NeverUseRadioBtn.Location = new Point(0x1a, 0x42);
            this.DGPSMode_NeverUseRadioBtn.Name = "DGPSMode_NeverUseRadioBtn";
            this.DGPSMode_NeverUseRadioBtn.Size = new Size(0x4a, 0x11);
            this.DGPSMode_NeverUseRadioBtn.TabIndex = 2;
            this.DGPSMode_NeverUseRadioBtn.TabStop = true;
            this.DGPSMode_NeverUseRadioBtn.Text = "Never &use";
            this.DGPSMode_NeverUseRadioBtn.UseVisualStyleBackColor = true;
            this.DGPSMode_ExclusiveRadioBtn.AutoSize = true;
            this.DGPSMode_ExclusiveRadioBtn.Location = new Point(0x1a, 0x2b);
            this.DGPSMode_ExclusiveRadioBtn.Name = "DGPSMode_ExclusiveRadioBtn";
            this.DGPSMode_ExclusiveRadioBtn.Size = new Size(70, 0x11);
            this.DGPSMode_ExclusiveRadioBtn.TabIndex = 1;
            this.DGPSMode_ExclusiveRadioBtn.TabStop = true;
            this.DGPSMode_ExclusiveRadioBtn.Text = "&Exclusive";
            this.DGPSMode_ExclusiveRadioBtn.UseVisualStyleBackColor = true;
            this.DGPSMode_AutomaticRadioBtn.AutoSize = true;
            this.DGPSMode_AutomaticRadioBtn.Location = new Point(0x1a, 20);
            this.DGPSMode_AutomaticRadioBtn.Name = "DGPSMode_AutomaticRadioBtn";
            this.DGPSMode_AutomaticRadioBtn.Size = new Size(0x48, 0x11);
            this.DGPSMode_AutomaticRadioBtn.TabIndex = 0;
            this.DGPSMode_AutomaticRadioBtn.TabStop = true;
            this.DGPSMode_AutomaticRadioBtn.Text = "&Automatic";
            this.DGPSMode_AutomaticRadioBtn.UseVisualStyleBackColor = true;
            this.DGPSParameters_GrpBox.Controls.Add(this.groupBox2);
            this.DGPSParameters_GrpBox.Controls.Add(this.groupBox1);
            this.DGPSParameters_GrpBox.Controls.Add(this.textBox_DGPS_PRN);
            this.DGPSParameters_GrpBox.Controls.Add(this.labelPRN);
            this.DGPSParameters_GrpBox.Controls.Add(this.DGPSParam_AutoScan_checkbox);
            this.DGPSParameters_GrpBox.Location = new Point(13, 0xb0);
            this.DGPSParameters_GrpBox.Name = "DGPSParameters_GrpBox";
            this.DGPSParameters_GrpBox.Size = new Size(0x10b, 0x84);
            this.DGPSParameters_GrpBox.TabIndex = 2;
            this.DGPSParameters_GrpBox.TabStop = false;
            this.DGPSParameters_GrpBox.Text = "Parameters";
            this.groupBox2.Controls.Add(this.DGPSParams_TestingRadioBtn);
            this.groupBox2.Controls.Add(this.DGPSParams_IntegrityRadioBtn);
            this.groupBox2.Location = new Point(6, 0x36);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x77, 0x48);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mode";
            this.DGPSParams_TestingRadioBtn.AutoSize = true;
            this.DGPSParams_TestingRadioBtn.Location = new Point(20, 0x13);
            this.DGPSParams_TestingRadioBtn.Name = "DGPSParams_TestingRadioBtn";
            this.DGPSParams_TestingRadioBtn.Size = new Size(60, 0x11);
            this.DGPSParams_TestingRadioBtn.TabIndex = 3;
            this.DGPSParams_TestingRadioBtn.TabStop = true;
            this.DGPSParams_TestingRadioBtn.Text = "Testing";
            this.DGPSParams_TestingRadioBtn.UseVisualStyleBackColor = true;
            this.DGPSParams_IntegrityRadioBtn.AutoSize = true;
            this.DGPSParams_IntegrityRadioBtn.Location = new Point(20, 0x2a);
            this.DGPSParams_IntegrityRadioBtn.Name = "DGPSParams_IntegrityRadioBtn";
            this.DGPSParams_IntegrityRadioBtn.Size = new Size(0x3e, 0x11);
            this.DGPSParams_IntegrityRadioBtn.TabIndex = 4;
            this.DGPSParams_IntegrityRadioBtn.TabStop = true;
            this.DGPSParams_IntegrityRadioBtn.Text = "Integrity";
            this.DGPSParams_IntegrityRadioBtn.UseVisualStyleBackColor = true;
            this.groupBox1.Controls.Add(this.DGPS_TimeoutSource_UserDefinedRadioBtn);
            this.groupBox1.Controls.Add(this.DGPS_TimeoutSource_DefaultRadioBtn);
            this.groupBox1.Location = new Point(0x8d, 0x36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(120, 0x48);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Timeout Source";
            this.DGPS_TimeoutSource_UserDefinedRadioBtn.AutoSize = true;
            this.DGPS_TimeoutSource_UserDefinedRadioBtn.Location = new Point(0x10, 0x2a);
            this.DGPS_TimeoutSource_UserDefinedRadioBtn.Name = "DGPS_TimeoutSource_UserDefinedRadioBtn";
            this.DGPS_TimeoutSource_UserDefinedRadioBtn.Size = new Size(0x55, 0x11);
            this.DGPS_TimeoutSource_UserDefinedRadioBtn.TabIndex = 1;
            this.DGPS_TimeoutSource_UserDefinedRadioBtn.TabStop = true;
            this.DGPS_TimeoutSource_UserDefinedRadioBtn.Text = "User defined";
            this.DGPS_TimeoutSource_UserDefinedRadioBtn.UseVisualStyleBackColor = true;
            this.DGPS_TimeoutSource_DefaultRadioBtn.AutoSize = true;
            this.DGPS_TimeoutSource_DefaultRadioBtn.Location = new Point(0x10, 0x13);
            this.DGPS_TimeoutSource_DefaultRadioBtn.Name = "DGPS_TimeoutSource_DefaultRadioBtn";
            this.DGPS_TimeoutSource_DefaultRadioBtn.Size = new Size(0x3b, 0x11);
            this.DGPS_TimeoutSource_DefaultRadioBtn.TabIndex = 0;
            this.DGPS_TimeoutSource_DefaultRadioBtn.TabStop = true;
            this.DGPS_TimeoutSource_DefaultRadioBtn.Text = "Default";
            this.DGPS_TimeoutSource_DefaultRadioBtn.UseVisualStyleBackColor = true;
            this.DGPS_TimeoutSource_DefaultRadioBtn.CheckedChanged += new EventHandler(this.DGPS_TimeoutSource_DefaultRadioBtn_CheckedChanged);
            this.textBox_DGPS_PRN.BackColor = SystemColors.Window;
            this.textBox_DGPS_PRN.Location = new Point(0xbb, 0x1c);
            this.textBox_DGPS_PRN.Name = "textBox_DGPS_PRN";
            this.textBox_DGPS_PRN.Size = new Size(0x30, 20);
            this.textBox_DGPS_PRN.TabIndex = 2;
            this.labelPRN.AutoSize = true;
            this.labelPRN.Location = new Point(0x94, 0x20);
            this.labelPRN.Name = "labelPRN";
            this.labelPRN.Size = new Size(0x21, 13);
            this.labelPRN.TabIndex = 1;
            this.labelPRN.Text = "PRN:";
            this.DGPSParam_AutoScan_checkbox.AutoSize = true;
            this.DGPSParam_AutoScan_checkbox.Checked = true;
            this.DGPSParam_AutoScan_checkbox.CheckState = CheckState.Checked;
            this.DGPSParam_AutoScan_checkbox.Location = new Point(0x1a, 30);
            this.DGPSParam_AutoScan_checkbox.Name = "DGPSParam_AutoScan_checkbox";
            this.DGPSParam_AutoScan_checkbox.Size = new Size(0x4a, 0x11);
            this.DGPSParam_AutoScan_checkbox.TabIndex = 0;
            this.DGPSParam_AutoScan_checkbox.Text = "Auto scan";
            this.DGPSParam_AutoScan_checkbox.UseVisualStyleBackColor = true;
            this.DGPSParam_AutoScan_checkbox.CheckedChanged += new EventHandler(this.DGPSParam_AutoScan_checkbox_CheckedChanged);
            this.button_Send.Location = new Point(0x3f, 0x145);
            this.button_Send.Name = "button_Send";
            this.button_Send.Size = new Size(0x4b, 0x17);
            this.button_Send.TabIndex = 3;
            this.button_Send.Text = "&Send";
            this.button_Send.UseVisualStyleBackColor = true;
            this.button_Send.Click += new EventHandler(this.button_Send_Click);
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new Point(0x9a, 0x145);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new Size(0x4b, 0x17);
            this.button_Cancel.TabIndex = 4;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new EventHandler(this.button_Cancel_Click);
            base.AcceptButton = this.button_Send;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.button_Cancel;
            base.ClientSize = new Size(0x124, 0x16d);
            base.Controls.Add(this.button_Cancel);
            base.Controls.Add(this.button_Send);
            base.Controls.Add(this.DGPSParameters_GrpBox);
            base.Controls.Add(this.DGPSMode_GrpBox);
            base.Controls.Add(this.DGPSSource_GrpBox);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmDGPS";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "DGPS";
            base.Load += new EventHandler(this.frmDGPS_Load);
            base.FormClosing += new FormClosingEventHandler(this.frmDGPS_FormClosing);
            this.DGPSSource_GrpBox.ResumeLayout(false);
            this.DGPSSource_GrpBox.PerformLayout();
            this.DGPSMode_GrpBox.ResumeLayout(false);
            this.DGPSMode_GrpBox.PerformLayout();
            this.DGPSParameters_GrpBox.ResumeLayout(false);
            this.DGPSParameters_GrpBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void SetDGPSControl()
        {
            if (!this.DGPSSource_NoneRadioBtn.Checked)
            {
                string messageName = "DGPS Control";
                StringBuilder builder = new StringBuilder();
                if (((this.comm.RxType == CommunicationManager.ReceiverType.SLC) || (this.comm.RxType == CommunicationManager.ReceiverType.GSW)) && (this.comm.MessageProtocol != "NMEA"))
                {
                    builder.Append("138,");
                    if (this.DGPSMode_AutomaticRadioBtn.Checked)
                    {
                        builder.Append("0,");
                    }
                    else if (this.DGPSMode_ExclusiveRadioBtn.Checked)
                    {
                        builder.Append("1,");
                    }
                    else
                    {
                        builder.Append("2,");
                    }
                    builder.Append(this.textBox_DGPStimeout.Text);
                    string msg = this.comm.m_Protocols.ConvertFieldsToRaw(builder.ToString(), messageName, "OSP");
                    this.comm.WriteData(msg);
                }
            }
        }

        private void SetDGPSSource()
        {
            string messageName = "DGPS Source";
            StringBuilder builder = new StringBuilder();
            if (((this.comm.RxType == CommunicationManager.ReceiverType.SLC) || (this.comm.RxType == CommunicationManager.ReceiverType.GSW)) && (this.comm.MessageProtocol != "NMEA"))
            {
                builder.Append("133,");
                if (this.DGPSSource_NoneRadioBtn.Checked)
                {
                    builder.Append("0,");
                }
                else
                {
                    builder.Append("1,");
                }
                builder.Append("0,0");
                string msg = this.comm.m_Protocols.ConvertFieldsToRaw(builder.ToString(), messageName, "OSP");
                this.comm.WriteData(msg);
            }
        }

        private void SetSBASParameters()
        {
            if (!this.DGPSSource_NoneRadioBtn.Checked)
            {
                string messageName = "Set SBAS Parameters";
                StringBuilder builder = new StringBuilder();
                if (((this.comm.RxType == CommunicationManager.ReceiverType.SLC) || (this.comm.RxType == CommunicationManager.ReceiverType.GSW)) && (this.comm.MessageProtocol != "NMEA"))
                {
                    builder.Append("170,");
                    byte num = 0;
                    if (this.DGPSParam_AutoScan_checkbox.Checked)
                    {
                        builder.Append("0,");
                    }
                    else if (this.textBox_DGPS_PRN.Text != "")
                    {
                        num = Convert.ToByte(this.textBox_DGPS_PRN.Text);
                        builder.Append(num);
                        builder.Append(",");
                    }
                    if (this.DGPSParams_TestingRadioBtn.Checked)
                    {
                        builder.Append("0,");
                    }
                    else
                    {
                        builder.Append("1,");
                    }
                    int num2 = 0;
                    if (this.DGPS_TimeoutSource_DefaultRadioBtn.Checked)
                    {
                        builder.Append("0,");
                        num2 = 0;
                    }
                    else
                    {
                        builder.Append("1,");
                        num2 = 1;
                    }
                    byte num3 = Convert.ToByte(num2);
                    if (num != 0)
                    {
                        num3 = (byte) (num3 | 8);
                    }
                    builder.Append(num3);
                    builder.Append(",");
                    builder.Append("0,0");
                    string msg = this.comm.m_Protocols.ConvertFieldsToRaw(builder.ToString(), messageName, "OSP");
                    this.comm.WriteData(msg);
                }
            }
        }
    }
}

