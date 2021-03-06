﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using CommonClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.MessageHandling;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class frmSwitchProtocol : Form
    {
        private CommunicationManager comm;
        private IContainer components;
        private ComboBox frmSwitchProtocolBaudRateComboBox;
        private Label frmSwitchProtocolBaudRateLable;
        private Button frmSwitchProtocolCancelBtn;
        private Label frmSwitchProtocolGGALable;
        private ComboBox frmSwitchProtocolGGARateComboBox;
        private Label frmSwitchProtocolGLLLabel;
        private ComboBox frmSwitchProtocolGLLRateComboBox;
        private GroupBox frmSwitchProtocolGrpBox;
        private Label frmSwitchProtocolGSALable;
        private ComboBox frmSwitchProtocolGSARateComboBox;
        private Label frmSwitchProtocolGSVLabel;
        private ComboBox frmSwitchProtocolGSVRateComboBox;
        private RadioButton frmSwitchProtocolNMEARadioBtn;
        private Label frmSwitchProtocolRMCLabel;
        private ComboBox frmSwitchProtocolRMCRateComboBox;
        private RadioButton frmSwitchProtocolSSBRadioBtn;
        private Button frmSwitchProtocolsSetBtn;
        private GroupBox frmSwitchProtocolUpdateRateGrpBox;
        private Label frmSwitchProtocolVTGLabel;
        private ComboBox frmSwitchProtocolVTGRateComboBox;

        public frmSwitchProtocol(CommunicationManager parentComm)
        {
            this.InitializeComponent();
            this.comm = parentComm;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmSwitchProtocol_Load(object sender, EventArgs e)
        {
            this.loadDefaults();
            if ((this.comm != null) && (this.comm.MessageProtocol == "OSP"))
            {
                this.frmSwitchProtocolNMEARadioBtn.Checked = true;
            }
        }

        private void frmSwitchProtocolCancelBtn_Click(object sender, EventArgs e)
        {
            clsGlobal.ToSwitchBaudRate = this.comm.BaudRate;
            clsGlobal.ToSwitchProtocol = this.comm.MessageProtocol;
            base.Close();
        }

        private void frmSwitchProtocolNMEARadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmSwitchProtocolNMEARadioBtn.Checked)
            {
                this.frmSwitchProtocolUpdateRateGrpBox.Enabled = true;
                this.frmSwitchProtocolBaudRateComboBox.SelectedIndex = 4;
                this.frmSwitchProtocolBaudRateComboBox.Text = this.frmSwitchProtocolBaudRateComboBox.Items[this.frmSwitchProtocolBaudRateComboBox.SelectedIndex].ToString();
            }
            else
            {
                this.frmSwitchProtocolUpdateRateGrpBox.Enabled = false;
            }
        }

        private void frmSwitchProtocolSSBRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.frmSwitchProtocolSSBRadioBtn.Checked)
            {
                this.frmSwitchProtocolUpdateRateGrpBox.Enabled = false;
                this.frmSwitchProtocolBaudRateComboBox.SelectedIndex = 9;
                this.frmSwitchProtocolBaudRateComboBox.Text = this.frmSwitchProtocolBaudRateComboBox.Items[this.frmSwitchProtocolBaudRateComboBox.SelectedIndex].ToString();
            }
            else
            {
                this.frmSwitchProtocolUpdateRateGrpBox.Enabled = true;
            }
        }

        private void frmSwitchProtocolsSetBtn_Click(object sender, EventArgs e)
        {
            if (CommunicationManager.ValidateCommManager(this.comm))
            {
                string msg = string.Empty;
                StringBuilder builder = new StringBuilder();
                if (this.comm.MessageProtocol == "NMEA")
                {
                    if (this.frmSwitchProtocolNMEARadioBtn.Checked)
                    {
                        return;
                    }
                }
                else if (this.frmSwitchProtocolSSBRadioBtn.Checked)
                {
                    return;
                }
                if (this.frmSwitchProtocolNMEARadioBtn.Checked)
                {
                    int num = 1;
                    string messageName = "Switch to NMEA Protocol";
                    clsGlobal.ToSwitchProtocol = "NMEA";
                    this.comm.ToSwitchProtocol = "NMEA";
                    builder.Append("129,2,");
                    try
                    {
                        num = Convert.ToByte(this.frmSwitchProtocolGGARateComboBox.Text);
                    }
                    catch
                    {
                        num = 1;
                    }
                    builder.Append(num);
                    builder.Append(",1,");
                    try
                    {
                        num = Convert.ToByte(this.frmSwitchProtocolGLLRateComboBox.Text);
                    }
                    catch
                    {
                        num = 1;
                    }
                    builder.Append(num);
                    builder.Append(",1,");
                    try
                    {
                        num = Convert.ToByte(this.frmSwitchProtocolGSARateComboBox.Text);
                    }
                    catch
                    {
                        num = 1;
                    }
                    builder.Append(num);
                    builder.Append(",1,");
                    try
                    {
                        num = Convert.ToByte(this.frmSwitchProtocolGSVRateComboBox.Text);
                    }
                    catch
                    {
                        num = 1;
                    }
                    builder.Append(num);
                    builder.Append(",1,");
                    try
                    {
                        num = Convert.ToByte(this.frmSwitchProtocolRMCRateComboBox.Text);
                    }
                    catch
                    {
                        num = 1;
                    }
                    builder.Append(num);
                    builder.Append(",1,");
                    try
                    {
                        num = Convert.ToByte(this.frmSwitchProtocolVTGRateComboBox.Text);
                    }
                    catch
                    {
                        num = 1;
                    }
                    builder.Append(num);
                    builder.Append(",1,0,1,0,0,0,1,0,0,");
                    builder.Append(this.frmSwitchProtocolBaudRateComboBox.Text);
                    msg = this.comm.m_Protocols.ConvertFieldsToRaw(builder.ToString(), messageName, "SSB");
                    if (msg == string.Empty)
                    {
                        clsGlobal.PerformOnAll = false;
                        return;
                    }
                }
                else
                {
                    builder.Append("$PSRF100,0,");
                    builder.Append(this.frmSwitchProtocolBaudRateComboBox.Text);
                    builder.Append(",8,1,0");
                    msg = NMEAReceiver.NMEA_AddCheckSum(builder.ToString());
                    msg = NMEAReceiver.NMEA_AddCheckSum("$PSRF105,1") + "\r\n" + msg;
                    clsGlobal.ToSwitchProtocol = "OSP";
                    this.comm.ToSwitchProtocol = "OSP";
                }
                clsGlobal.ToSwitchBaudRate = this.frmSwitchProtocolBaudRateComboBox.Text;
                if (clsGlobal.PerformOnAll)
                {
                    foreach (string str3 in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                    {
                        PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str3];
                        if (((manager != null) && manager.comm.IsSourceDeviceOpen()) && (manager.comm.ProductFamily == CommonClass.ProductType.GSD4e))
                        {
                            manager.comm.ToSwitchProtocol = clsGlobal.ToSwitchProtocol;
                            manager.comm.ToSwitchBaud = clsGlobal.ToSwitchBaudRate;
                            manager.comm.WriteData(msg);
                        }
                    }
                    clsGlobal.PerformOnAll = false;
                }
                else
                {
                    this.comm.ToSwitchProtocol = clsGlobal.ToSwitchProtocol;
                    this.comm.ToSwitchBaud = clsGlobal.ToSwitchBaudRate;
                    this.comm.WriteData(msg);
                }
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmSwitchProtocol));
            this.frmSwitchProtocolGrpBox = new GroupBox();
            this.frmSwitchProtocolNMEARadioBtn = new RadioButton();
            this.frmSwitchProtocolSSBRadioBtn = new RadioButton();
            this.frmSwitchProtocolsSetBtn = new Button();
            this.frmSwitchProtocolCancelBtn = new Button();
            this.frmSwitchProtocolBaudRateComboBox = new ComboBox();
            this.frmSwitchProtocolBaudRateLable = new Label();
            this.frmSwitchProtocolGGALable = new Label();
            this.frmSwitchProtocolGGARateComboBox = new ComboBox();
            this.frmSwitchProtocolGLLLabel = new Label();
            this.frmSwitchProtocolGLLRateComboBox = new ComboBox();
            this.frmSwitchProtocolGSALable = new Label();
            this.frmSwitchProtocolGSARateComboBox = new ComboBox();
            this.frmSwitchProtocolGSVLabel = new Label();
            this.frmSwitchProtocolGSVRateComboBox = new ComboBox();
            this.frmSwitchProtocolRMCLabel = new Label();
            this.frmSwitchProtocolRMCRateComboBox = new ComboBox();
            this.frmSwitchProtocolVTGLabel = new Label();
            this.frmSwitchProtocolVTGRateComboBox = new ComboBox();
            this.frmSwitchProtocolUpdateRateGrpBox = new GroupBox();
            this.frmSwitchProtocolGrpBox.SuspendLayout();
            this.frmSwitchProtocolUpdateRateGrpBox.SuspendLayout();
            base.SuspendLayout();
            this.frmSwitchProtocolGrpBox.Controls.Add(this.frmSwitchProtocolNMEARadioBtn);
            this.frmSwitchProtocolGrpBox.Controls.Add(this.frmSwitchProtocolSSBRadioBtn);
            this.frmSwitchProtocolGrpBox.Location = new Point(0x10, 0x13);
            this.frmSwitchProtocolGrpBox.Name = "frmSwitchProtocolGrpBox";
            this.frmSwitchProtocolGrpBox.Size = new Size(0x87, 0x54);
            this.frmSwitchProtocolGrpBox.TabIndex = 0;
            this.frmSwitchProtocolGrpBox.TabStop = false;
            this.frmSwitchProtocolGrpBox.Text = "Protocols";
            this.frmSwitchProtocolNMEARadioBtn.AutoSize = true;
            this.frmSwitchProtocolNMEARadioBtn.Location = new Point(0x17, 0x31);
            this.frmSwitchProtocolNMEARadioBtn.Name = "frmSwitchProtocolNMEARadioBtn";
            this.frmSwitchProtocolNMEARadioBtn.Size = new Size(0x38, 0x11);
            this.frmSwitchProtocolNMEARadioBtn.TabIndex = 0;
            this.frmSwitchProtocolNMEARadioBtn.TabStop = true;
            this.frmSwitchProtocolNMEARadioBtn.Text = "NMEA";
            this.frmSwitchProtocolNMEARadioBtn.UseVisualStyleBackColor = true;
            this.frmSwitchProtocolNMEARadioBtn.CheckedChanged += new EventHandler(this.frmSwitchProtocolNMEARadioBtn_CheckedChanged);
            this.frmSwitchProtocolSSBRadioBtn.AutoSize = true;
            this.frmSwitchProtocolSSBRadioBtn.Location = new Point(0x17, 0x1a);
            this.frmSwitchProtocolSSBRadioBtn.Name = "frmSwitchProtocolSSBRadioBtn";
            this.frmSwitchProtocolSSBRadioBtn.Size = new Size(0x2f, 0x11);
            this.frmSwitchProtocolSSBRadioBtn.TabIndex = 0;
            this.frmSwitchProtocolSSBRadioBtn.TabStop = true;
            this.frmSwitchProtocolSSBRadioBtn.Text = "OSP";
            this.frmSwitchProtocolSSBRadioBtn.UseVisualStyleBackColor = true;
            this.frmSwitchProtocolSSBRadioBtn.CheckedChanged += new EventHandler(this.frmSwitchProtocolSSBRadioBtn_CheckedChanged);
            this.frmSwitchProtocolsSetBtn.Location = new Point(0xc4, 0x24);
            this.frmSwitchProtocolsSetBtn.Name = "frmSwitchProtocolsSetBtn";
            this.frmSwitchProtocolsSetBtn.Size = new Size(0x4b, 0x17);
            this.frmSwitchProtocolsSetBtn.TabIndex = 1;
            this.frmSwitchProtocolsSetBtn.Text = "&Set";
            this.frmSwitchProtocolsSetBtn.UseVisualStyleBackColor = true;
            this.frmSwitchProtocolsSetBtn.Click += new EventHandler(this.frmSwitchProtocolsSetBtn_Click);
            this.frmSwitchProtocolCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.frmSwitchProtocolCancelBtn.Location = new Point(0xc4, 0x41);
            this.frmSwitchProtocolCancelBtn.Name = "frmSwitchProtocolCancelBtn";
            this.frmSwitchProtocolCancelBtn.Size = new Size(0x4b, 0x17);
            this.frmSwitchProtocolCancelBtn.TabIndex = 1;
            this.frmSwitchProtocolCancelBtn.Text = "&Cancel";
            this.frmSwitchProtocolCancelBtn.UseVisualStyleBackColor = true;
            this.frmSwitchProtocolCancelBtn.Click += new EventHandler(this.frmSwitchProtocolCancelBtn_Click);
            this.frmSwitchProtocolBaudRateComboBox.FormattingEnabled = true;
            this.frmSwitchProtocolBaudRateComboBox.Items.AddRange(new object[] { "300", "600", "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200", "230400", "460800", "921600", "1228800" });
            this.frmSwitchProtocolBaudRateComboBox.Location = new Point(0xca, 0xb0);
            this.frmSwitchProtocolBaudRateComboBox.Name = "frmSwitchProtocolBaudRateComboBox";
            this.frmSwitchProtocolBaudRateComboBox.Size = new Size(0x4c, 0x15);
            this.frmSwitchProtocolBaudRateComboBox.TabIndex = 0x11;
            this.frmSwitchProtocolBaudRateLable.AutoSize = true;
            this.frmSwitchProtocolBaudRateLable.Location = new Point(0xc7, 0x95);
            this.frmSwitchProtocolBaudRateLable.Name = "frmSwitchProtocolBaudRateLable";
            this.frmSwitchProtocolBaudRateLable.Size = new Size(0x3d, 13);
            this.frmSwitchProtocolBaudRateLable.TabIndex = 0x12;
            this.frmSwitchProtocolBaudRateLable.Text = "Baud Rate:";
            this.frmSwitchProtocolGGALable.AutoSize = true;
            this.frmSwitchProtocolGGALable.Location = new Point(0x12, 30);
            this.frmSwitchProtocolGGALable.Name = "frmSwitchProtocolGGALable";
            this.frmSwitchProtocolGGALable.Size = new Size(0x21, 13);
            this.frmSwitchProtocolGGALable.TabIndex = 0x13;
            this.frmSwitchProtocolGGALable.Text = "GGA:";
            this.frmSwitchProtocolGGARateComboBox.FormattingEnabled = true;
            this.frmSwitchProtocolGGARateComboBox.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            this.frmSwitchProtocolGGARateComboBox.Location = new Point(0x36, 0x1a);
            this.frmSwitchProtocolGGARateComboBox.Name = "frmSwitchProtocolGGARateComboBox";
            this.frmSwitchProtocolGGARateComboBox.Size = new Size(0x42, 0x15);
            this.frmSwitchProtocolGGARateComboBox.TabIndex = 20;
            this.frmSwitchProtocolGLLLabel.AutoSize = true;
            this.frmSwitchProtocolGLLLabel.Location = new Point(0x12, 0x34);
            this.frmSwitchProtocolGLLLabel.Name = "frmSwitchProtocolGLLLabel";
            this.frmSwitchProtocolGLLLabel.Size = new Size(30, 13);
            this.frmSwitchProtocolGLLLabel.TabIndex = 0x13;
            this.frmSwitchProtocolGLLLabel.Text = "GLL:";
            this.frmSwitchProtocolGLLRateComboBox.FormattingEnabled = true;
            this.frmSwitchProtocolGLLRateComboBox.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            this.frmSwitchProtocolGLLRateComboBox.Location = new Point(0x36, 0x2f);
            this.frmSwitchProtocolGLLRateComboBox.Name = "frmSwitchProtocolGLLRateComboBox";
            this.frmSwitchProtocolGLLRateComboBox.Size = new Size(0x42, 0x15);
            this.frmSwitchProtocolGLLRateComboBox.TabIndex = 20;
            this.frmSwitchProtocolGSALable.AutoSize = true;
            this.frmSwitchProtocolGSALable.Location = new Point(0x12, 0x4a);
            this.frmSwitchProtocolGSALable.Name = "frmSwitchProtocolGSALable";
            this.frmSwitchProtocolGSALable.Size = new Size(0x20, 13);
            this.frmSwitchProtocolGSALable.TabIndex = 0x13;
            this.frmSwitchProtocolGSALable.Text = "GSA:";
            this.frmSwitchProtocolGSARateComboBox.FormattingEnabled = true;
            this.frmSwitchProtocolGSARateComboBox.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            this.frmSwitchProtocolGSARateComboBox.Location = new Point(0x36, 0x44);
            this.frmSwitchProtocolGSARateComboBox.Name = "frmSwitchProtocolGSARateComboBox";
            this.frmSwitchProtocolGSARateComboBox.Size = new Size(0x42, 0x15);
            this.frmSwitchProtocolGSARateComboBox.TabIndex = 20;
            this.frmSwitchProtocolGSVLabel.AutoSize = true;
            this.frmSwitchProtocolGSVLabel.Location = new Point(0x12, 0x60);
            this.frmSwitchProtocolGSVLabel.Name = "frmSwitchProtocolGSVLabel";
            this.frmSwitchProtocolGSVLabel.Size = new Size(0x20, 13);
            this.frmSwitchProtocolGSVLabel.TabIndex = 0x13;
            this.frmSwitchProtocolGSVLabel.Text = "GSV:";
            this.frmSwitchProtocolGSVRateComboBox.FormattingEnabled = true;
            this.frmSwitchProtocolGSVRateComboBox.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            this.frmSwitchProtocolGSVRateComboBox.Location = new Point(0x36, 0x59);
            this.frmSwitchProtocolGSVRateComboBox.Name = "frmSwitchProtocolGSVRateComboBox";
            this.frmSwitchProtocolGSVRateComboBox.Size = new Size(0x42, 0x15);
            this.frmSwitchProtocolGSVRateComboBox.TabIndex = 20;
            this.frmSwitchProtocolRMCLabel.AutoSize = true;
            this.frmSwitchProtocolRMCLabel.Location = new Point(0x12, 0x76);
            this.frmSwitchProtocolRMCLabel.Name = "frmSwitchProtocolRMCLabel";
            this.frmSwitchProtocolRMCLabel.Size = new Size(0x22, 13);
            this.frmSwitchProtocolRMCLabel.TabIndex = 0x13;
            this.frmSwitchProtocolRMCLabel.Text = "RMC:";
            this.frmSwitchProtocolRMCRateComboBox.FormattingEnabled = true;
            this.frmSwitchProtocolRMCRateComboBox.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            this.frmSwitchProtocolRMCRateComboBox.Location = new Point(0x36, 110);
            this.frmSwitchProtocolRMCRateComboBox.Name = "frmSwitchProtocolRMCRateComboBox";
            this.frmSwitchProtocolRMCRateComboBox.Size = new Size(0x42, 0x15);
            this.frmSwitchProtocolRMCRateComboBox.TabIndex = 20;
            this.frmSwitchProtocolVTGLabel.AutoSize = true;
            this.frmSwitchProtocolVTGLabel.Location = new Point(0x12, 140);
            this.frmSwitchProtocolVTGLabel.Name = "frmSwitchProtocolVTGLabel";
            this.frmSwitchProtocolVTGLabel.Size = new Size(0x20, 13);
            this.frmSwitchProtocolVTGLabel.TabIndex = 0x13;
            this.frmSwitchProtocolVTGLabel.Text = "VTG:";
            this.frmSwitchProtocolVTGRateComboBox.FormattingEnabled = true;
            this.frmSwitchProtocolVTGRateComboBox.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            this.frmSwitchProtocolVTGRateComboBox.Location = new Point(0x36, 0x83);
            this.frmSwitchProtocolVTGRateComboBox.Name = "frmSwitchProtocolVTGRateComboBox";
            this.frmSwitchProtocolVTGRateComboBox.Size = new Size(0x42, 0x15);
            this.frmSwitchProtocolVTGRateComboBox.TabIndex = 20;
            this.frmSwitchProtocolUpdateRateGrpBox.Controls.Add(this.frmSwitchProtocolVTGRateComboBox);
            this.frmSwitchProtocolUpdateRateGrpBox.Controls.Add(this.frmSwitchProtocolGGALable);
            this.frmSwitchProtocolUpdateRateGrpBox.Controls.Add(this.frmSwitchProtocolGGARateComboBox);
            this.frmSwitchProtocolUpdateRateGrpBox.Controls.Add(this.frmSwitchProtocolVTGLabel);
            this.frmSwitchProtocolUpdateRateGrpBox.Controls.Add(this.frmSwitchProtocolGLLLabel);
            this.frmSwitchProtocolUpdateRateGrpBox.Controls.Add(this.frmSwitchProtocolRMCRateComboBox);
            this.frmSwitchProtocolUpdateRateGrpBox.Controls.Add(this.frmSwitchProtocolGLLRateComboBox);
            this.frmSwitchProtocolUpdateRateGrpBox.Controls.Add(this.frmSwitchProtocolRMCLabel);
            this.frmSwitchProtocolUpdateRateGrpBox.Controls.Add(this.frmSwitchProtocolGSALable);
            this.frmSwitchProtocolUpdateRateGrpBox.Controls.Add(this.frmSwitchProtocolGSVRateComboBox);
            this.frmSwitchProtocolUpdateRateGrpBox.Controls.Add(this.frmSwitchProtocolGSARateComboBox);
            this.frmSwitchProtocolUpdateRateGrpBox.Controls.Add(this.frmSwitchProtocolGSVLabel);
            this.frmSwitchProtocolUpdateRateGrpBox.Location = new Point(0x10, 0x7c);
            this.frmSwitchProtocolUpdateRateGrpBox.Name = "frmSwitchProtocolUpdateRateGrpBox";
            this.frmSwitchProtocolUpdateRateGrpBox.Size = new Size(0x87, 0xaf);
            this.frmSwitchProtocolUpdateRateGrpBox.TabIndex = 0x16;
            this.frmSwitchProtocolUpdateRateGrpBox.TabStop = false;
            this.frmSwitchProtocolUpdateRateGrpBox.Text = "Update Rate (s)";
            base.AcceptButton = this.frmSwitchProtocolsSetBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.frmSwitchProtocolCancelBtn;
            base.ClientSize = new Size(0x144, 0x148);
            base.Controls.Add(this.frmSwitchProtocolUpdateRateGrpBox);
            base.Controls.Add(this.frmSwitchProtocolBaudRateComboBox);
            base.Controls.Add(this.frmSwitchProtocolBaudRateLable);
            base.Controls.Add(this.frmSwitchProtocolCancelBtn);
            base.Controls.Add(this.frmSwitchProtocolsSetBtn);
            base.Controls.Add(this.frmSwitchProtocolGrpBox);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmSwitchProtocol";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Switch Protocol";
            base.Load += new EventHandler(this.frmSwitchProtocol_Load);
            this.frmSwitchProtocolGrpBox.ResumeLayout(false);
            this.frmSwitchProtocolGrpBox.PerformLayout();
            this.frmSwitchProtocolUpdateRateGrpBox.ResumeLayout(false);
            this.frmSwitchProtocolUpdateRateGrpBox.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void loadDefaults()
        {
            if (this.comm != null)
            {
                if (this.comm.MessageProtocol == "OSP")
                {
                    this.frmSwitchProtocolNMEARadioBtn.Checked = true;
                    this.frmSwitchProtocolUpdateRateGrpBox.Enabled = true;
                    this.frmSwitchProtocolBaudRateComboBox.SelectedIndex = 4;
                    this.frmSwitchProtocolBaudRateComboBox.Text = this.frmSwitchProtocolBaudRateComboBox.Items[this.frmSwitchProtocolBaudRateComboBox.SelectedIndex].ToString();
                }
                else
                {
                    this.frmSwitchProtocolSSBRadioBtn.Checked = true;
                    this.frmSwitchProtocolUpdateRateGrpBox.Enabled = false;
                    this.frmSwitchProtocolBaudRateComboBox.SelectedIndex = 9;
                    this.frmSwitchProtocolBaudRateComboBox.Text = this.frmSwitchProtocolBaudRateComboBox.Items[this.frmSwitchProtocolBaudRateComboBox.SelectedIndex].ToString();
                }
                this.frmSwitchProtocolGGARateComboBox.SelectedIndex = 1;
                this.frmSwitchProtocolGLLRateComboBox.SelectedIndex = 0;
                this.frmSwitchProtocolGSARateComboBox.SelectedIndex = 1;
                this.frmSwitchProtocolGSVRateComboBox.SelectedIndex = 5;
                this.frmSwitchProtocolRMCRateComboBox.SelectedIndex = 1;
                this.frmSwitchProtocolVTGRateComboBox.SelectedIndex = 0;
            }
        }
    }
}

