﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using CommonClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class frmEE : Form
    {
        private Button button_Close;
        private Button button_EEStorage;
        private Button button_SendCGEEPrediction;
        private Button button_SetEEState;
        private CommunicationManager comm;
        private IContainer components;
        private RadioButton disableCGEE_radioBtn;
        private ComboBox eeControlComboBox;
        private GroupBox EEControlGrpBox;
        private RadioButton eeStorageEEPromRadioBtn;
        private RadioButton eeStorageFlashRadioBtn;
        private GroupBox EEStorageGrpBox;
        private RadioButton eeStorageHostRadioBtn;
        private RadioButton EEStorageNoneRadioBtn;
        private RadioButton enableCGEE_radioBtn;
        private RadioButton enableCGEEtemp_radioBtn;
        private Label label4;
        private LinkLabel linkLabel1;
        private GroupBox predictionGroupBox;
        private TextBox secondsCGEEdisable;
        private long secsCGEE;
        private ToolTip toolTip1;

        public frmEE()
        {
            this.InitializeComponent();
        }

        public frmEE(CommunicationManager parentComm)
        {
            this.InitializeComponent();
            this.comm = parentComm;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void button_EEStorage_Click(object sender, EventArgs e)
        {
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        this.setEEStorage();
                    }
                }
                clsGlobal.PerformOnAll = false;
            }
            else if (this.comm.IsSourceDeviceOpen())
            {
                this.setEEStorage();
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void button_SendCGEEPrediction_Click(object sender, EventArgs e)
        {
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        this.disableCGEEprediction();
                    }
                }
                clsGlobal.PerformOnAll = false;
            }
            else if (this.comm.IsSourceDeviceOpen())
            {
                this.disableCGEEprediction();
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void button_SetEEState_Click(object sender, EventArgs e)
        {
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        this.setEEState();
                    }
                }
                clsGlobal.PerformOnAll = false;
            }
            else if (this.comm.IsSourceDeviceOpen())
            {
                this.setEEState();
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void disableCGEE_radioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.disableCGEE_radioBtn.Checked)
            {
                this.secondsCGEEdisable.Enabled = false;
            }
            else
            {
                this.secondsCGEEdisable.Enabled = true;
            }
        }

        private void disableCGEEprediction()
        {
            string messageName = "Test Mode Configuration Request - SSB_EE_DISABLE_EE_SECS";
            StringBuilder builder = new StringBuilder();
            if (((this.comm.RxType == CommunicationManager.ReceiverType.SLC) || (this.comm.RxType == CommunicationManager.ReceiverType.GSW)) && (this.comm.MessageProtocol != "NMEA"))
            {
                builder.Append(0xe8);
                builder.Append(",");
                builder.Append(0xfe);
                builder.Append(",");
                if (this.secondsCGEEdisable.Enabled)
                {
                    builder.Append(this.secondsCGEEdisable.Text);
                }
                else if (this.enableCGEE_radioBtn.Checked)
                {
                    builder.Append(uint.MaxValue);
                }
                else
                {
                    builder.Append("0");
                }
                string msg = this.comm.m_Protocols.ConvertFieldsToRaw(builder.ToString(), messageName, "OSP");
                this.comm.WriteData(msg);
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

        private void enableCGEE_radioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.enableCGEE_radioBtn.Checked)
            {
                this.secondsCGEEdisable.Enabled = false;
            }
            else
            {
                this.secondsCGEEdisable.Enabled = true;
            }
        }

        private void frmCGEE_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((base.DialogResult != DialogResult.Cancel) && this.enableCGEEtemp_radioBtn.Checked)
            {
                if (this.secondsCGEEdisable.Text == "")
                {
                    MessageBox.Show("Please enter a valid number for seconds", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                }
                else
                {
                    try
                    {
                        this.secsCGEE = Convert.ToInt64(this.secondsCGEEdisable.Text);
                        if (this.secsCGEE < 0L)
                        {
                            this.secsCGEE = -1L;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Invalid entry for seconds", "Error");
                        e.Cancel = true;
                    }
                }
            }
        }

        private void frmCGEE_Load(object sender, EventArgs e)
        {
            this.enableCGEE_radioBtn.Checked = true;
            this.secondsCGEEdisable.Enabled = false;
            this.eeStorageHostRadioBtn.Checked = true;
            this.eeControlComboBox.SelectedIndex = 0;
            if ((this.comm.ProductFamily != CommonClass.ProductType.GSD4e) && !clsGlobal.PerformOnAll)
            {
                this.EEStorageGrpBox.Enabled = false;
                this.EEControlGrpBox.Enabled = false;
            }
            else
            {
                this.EEStorageGrpBox.Enabled = true;
                this.EEControlGrpBox.Enabled = true;
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmEE));
            this.button_SendCGEEPrediction = new Button();
            this.button_Close = new Button();
            this.enableCGEE_radioBtn = new RadioButton();
            this.disableCGEE_radioBtn = new RadioButton();
            this.secondsCGEEdisable = new TextBox();
            this.label4 = new Label();
            this.toolTip1 = new ToolTip(this.components);
            this.linkLabel1 = new LinkLabel();
            this.enableCGEEtemp_radioBtn = new RadioButton();
            this.predictionGroupBox = new GroupBox();
            this.EEStorageGrpBox = new GroupBox();
            this.EEStorageNoneRadioBtn = new RadioButton();
            this.eeStorageFlashRadioBtn = new RadioButton();
            this.eeStorageEEPromRadioBtn = new RadioButton();
            this.button_EEStorage = new Button();
            this.eeStorageHostRadioBtn = new RadioButton();
            this.EEControlGrpBox = new GroupBox();
            this.eeControlComboBox = new ComboBox();
            this.button_SetEEState = new Button();
            this.predictionGroupBox.SuspendLayout();
            this.EEStorageGrpBox.SuspendLayout();
            this.EEControlGrpBox.SuspendLayout();
            base.SuspendLayout();
            this.button_SendCGEEPrediction.Location = new Point(0xeb, 0x13);
            this.button_SendCGEEPrediction.Name = "button_SendCGEEPrediction";
            this.button_SendCGEEPrediction.Size = new Size(0x4b, 0x17);
            this.button_SendCGEEPrediction.TabIndex = 0;
            this.button_SendCGEEPrediction.Text = "&Set";
            this.button_SendCGEEPrediction.UseVisualStyleBackColor = true;
            this.button_SendCGEEPrediction.Click += new EventHandler(this.button_SendCGEEPrediction_Click);
            this.button_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Close.Location = new Point(0x8d, 0xf1);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new Size(0x4b, 0x17);
            this.button_Close.TabIndex = 1;
            this.button_Close.Text = "&Close";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new EventHandler(this.button_Cancel_Click);
            this.enableCGEE_radioBtn.AutoSize = true;
            this.enableCGEE_radioBtn.Location = new Point(12, 0x16);
            this.enableCGEE_radioBtn.Name = "enableCGEE_radioBtn";
            this.enableCGEE_radioBtn.Size = new Size(0x3a, 0x11);
            this.enableCGEE_radioBtn.TabIndex = 2;
            this.enableCGEE_radioBtn.TabStop = true;
            this.enableCGEE_radioBtn.Text = "&Enable";
            this.enableCGEE_radioBtn.UseVisualStyleBackColor = true;
            this.enableCGEE_radioBtn.CheckedChanged += new EventHandler(this.enableCGEE_radioBtn_CheckedChanged);
            this.disableCGEE_radioBtn.AutoSize = true;
            this.disableCGEE_radioBtn.Location = new Point(12, 0x27);
            this.disableCGEE_radioBtn.Name = "disableCGEE_radioBtn";
            this.disableCGEE_radioBtn.Size = new Size(60, 0x11);
            this.disableCGEE_radioBtn.TabIndex = 3;
            this.disableCGEE_radioBtn.TabStop = true;
            this.disableCGEE_radioBtn.Text = "&Disable";
            this.disableCGEE_radioBtn.UseVisualStyleBackColor = true;
            this.disableCGEE_radioBtn.CheckedChanged += new EventHandler(this.disableCGEE_radioBtn_CheckedChanged);
            this.secondsCGEEdisable.Location = new Point(130, 0x36);
            this.secondsCGEEdisable.Name = "secondsCGEEdisable";
            this.secondsCGEEdisable.Size = new Size(70, 20);
            this.secondsCGEEdisable.TabIndex = 4;
            this.secondsCGEEdisable.Text = "1800";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0xca, 0x3a);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x18, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "sec";
            this.linkLabel1.ActiveLinkColor = Color.Blue;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new Point(0xee, 0x3a);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new Size(0x41, 13);
            this.linkLabel1.TabIndex = 9;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "What's this?";
            this.linkLabel1.VisitedLinkColor = Color.Blue;
            this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            this.enableCGEEtemp_radioBtn.AutoSize = true;
            this.enableCGEEtemp_radioBtn.Location = new Point(12, 0x38);
            this.enableCGEEtemp_radioBtn.Name = "enableCGEEtemp_radioBtn";
            this.enableCGEEtemp_radioBtn.Size = new Size(0x6f, 0x11);
            this.enableCGEEtemp_radioBtn.TabIndex = 10;
            this.enableCGEEtemp_radioBtn.TabStop = true;
            this.enableCGEEtemp_radioBtn.Text = "Enable &temporarily";
            this.enableCGEEtemp_radioBtn.UseVisualStyleBackColor = true;
            this.predictionGroupBox.Controls.Add(this.enableCGEEtemp_radioBtn);
            this.predictionGroupBox.Controls.Add(this.linkLabel1);
            this.predictionGroupBox.Controls.Add(this.button_SendCGEEPrediction);
            this.predictionGroupBox.Controls.Add(this.label4);
            this.predictionGroupBox.Controls.Add(this.secondsCGEEdisable);
            this.predictionGroupBox.Controls.Add(this.disableCGEE_radioBtn);
            this.predictionGroupBox.Controls.Add(this.enableCGEE_radioBtn);
            this.predictionGroupBox.Location = new Point(0x10, 12);
            this.predictionGroupBox.Name = "predictionGroupBox";
            this.predictionGroupBox.Size = new Size(0x146, 0x59);
            this.predictionGroupBox.TabIndex = 11;
            this.predictionGroupBox.TabStop = false;
            this.predictionGroupBox.Text = "CGEE Prediction";
            this.EEStorageGrpBox.Controls.Add(this.EEStorageNoneRadioBtn);
            this.EEStorageGrpBox.Controls.Add(this.eeStorageFlashRadioBtn);
            this.EEStorageGrpBox.Controls.Add(this.eeStorageEEPromRadioBtn);
            this.EEStorageGrpBox.Controls.Add(this.button_EEStorage);
            this.EEStorageGrpBox.Controls.Add(this.eeStorageHostRadioBtn);
            this.EEStorageGrpBox.Location = new Point(0xb7, 0x76);
            this.EEStorageGrpBox.Name = "EEStorageGrpBox";
            this.EEStorageGrpBox.Size = new Size(0x9f, 0x6a);
            this.EEStorageGrpBox.TabIndex = 14;
            this.EEStorageGrpBox.TabStop = false;
            this.EEStorageGrpBox.Text = "EE Storage";
            this.EEStorageNoneRadioBtn.AutoSize = true;
            this.EEStorageNoneRadioBtn.Location = new Point(0x59, 0x29);
            this.EEStorageNoneRadioBtn.Name = "EEStorageNoneRadioBtn";
            this.EEStorageNoneRadioBtn.Size = new Size(0x33, 0x11);
            this.EEStorageNoneRadioBtn.TabIndex = 0;
            this.EEStorageNoneRadioBtn.TabStop = true;
            this.EEStorageNoneRadioBtn.Text = "&None";
            this.EEStorageNoneRadioBtn.UseVisualStyleBackColor = true;
            this.eeStorageFlashRadioBtn.AutoSize = true;
            this.eeStorageFlashRadioBtn.Location = new Point(0x59, 0x18);
            this.eeStorageFlashRadioBtn.Name = "eeStorageFlashRadioBtn";
            this.eeStorageFlashRadioBtn.Size = new Size(0x3b, 0x11);
            this.eeStorageFlashRadioBtn.TabIndex = 0;
            this.eeStorageFlashRadioBtn.TabStop = true;
            this.eeStorageFlashRadioBtn.Text = "&FLASH";
            this.eeStorageFlashRadioBtn.UseVisualStyleBackColor = true;
            this.eeStorageEEPromRadioBtn.AutoSize = true;
            this.eeStorageEEPromRadioBtn.Location = new Point(12, 0x29);
            this.eeStorageEEPromRadioBtn.Name = "eeStorageEEPromRadioBtn";
            this.eeStorageEEPromRadioBtn.Size = new Size(0x47, 0x11);
            this.eeStorageEEPromRadioBtn.TabIndex = 0;
            this.eeStorageEEPromRadioBtn.TabStop = true;
            this.eeStorageEEPromRadioBtn.Text = "EE&PROM";
            this.eeStorageEEPromRadioBtn.UseVisualStyleBackColor = true;
            this.button_EEStorage.Location = new Point(0x2a, 0x44);
            this.button_EEStorage.Name = "button_EEStorage";
            this.button_EEStorage.Size = new Size(0x4b, 0x17);
            this.button_EEStorage.TabIndex = 0;
            this.button_EEStorage.Text = "&Set";
            this.button_EEStorage.UseVisualStyleBackColor = true;
            this.button_EEStorage.Click += new EventHandler(this.button_EEStorage_Click);
            this.eeStorageHostRadioBtn.AutoSize = true;
            this.eeStorageHostRadioBtn.Location = new Point(12, 0x18);
            this.eeStorageHostRadioBtn.Name = "eeStorageHostRadioBtn";
            this.eeStorageHostRadioBtn.Size = new Size(0x2f, 0x11);
            this.eeStorageHostRadioBtn.TabIndex = 0;
            this.eeStorageHostRadioBtn.TabStop = true;
            this.eeStorageHostRadioBtn.Text = "&Host";
            this.eeStorageHostRadioBtn.UseVisualStyleBackColor = true;
            this.EEControlGrpBox.Controls.Add(this.eeControlComboBox);
            this.EEControlGrpBox.Controls.Add(this.button_SetEEState);
            this.EEControlGrpBox.Location = new Point(0x10, 0x76);
            this.EEControlGrpBox.Name = "EEControlGrpBox";
            this.EEControlGrpBox.Size = new Size(0x9f, 0x6a);
            this.EEControlGrpBox.TabIndex = 15;
            this.EEControlGrpBox.TabStop = false;
            this.EEControlGrpBox.Text = "EE State Control";
            this.eeControlComboBox.FormattingEnabled = true;
            this.eeControlComboBox.Items.AddRange(new object[] { "Enable SGEE", "Disable SGEE", "Enable CGEE", "Disable CGEE", "Enable Both", "Disable Both" });
            this.eeControlComboBox.Location = new Point(12, 0x18);
            this.eeControlComboBox.Name = "eeControlComboBox";
            this.eeControlComboBox.Size = new Size(0x79, 0x15);
            this.eeControlComboBox.TabIndex = 1;
            this.button_SetEEState.Location = new Point(0x2a, 0x44);
            this.button_SetEEState.Name = "button_SetEEState";
            this.button_SetEEState.Size = new Size(0x4b, 0x17);
            this.button_SetEEState.TabIndex = 0;
            this.button_SetEEState.Text = "&Set";
            this.button_SetEEState.UseVisualStyleBackColor = true;
            this.button_SetEEState.Click += new EventHandler(this.button_SetEEState_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.button_Close;
            base.ClientSize = new Size(360, 0x115);
            base.Controls.Add(this.EEControlGrpBox);
            base.Controls.Add(this.EEStorageGrpBox);
            base.Controls.Add(this.predictionGroupBox);
            base.Controls.Add(this.button_Close);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmEE";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Set EE ";
            base.Load += new EventHandler(this.frmCGEE_Load);
            base.FormClosing += new FormClosingEventHandler(this.frmCGEE_FormClosing);
            this.predictionGroupBox.ResumeLayout(false);
            this.predictionGroupBox.PerformLayout();
            this.EEStorageGrpBox.ResumeLayout(false);
            this.EEStorageGrpBox.PerformLayout();
            this.EEControlGrpBox.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string text = "After enabling, the number of seconds to wait before CGEE is disabled\r\n\nRange: 0 - 4294967295\r\n 0 = disable immediately\r\n4294967295 = enable permanently (select 'Enable' radio button instead)";
            MessageBox.Show(text, "Seconds before disabling CGEE", MessageBoxButtons.OK);
        }

        private void setEEState()
        {
            string messageName = "MID_EE_INPUT EE_STATE_CONTROL";
            StringBuilder builder = new StringBuilder();
            if (this.comm.MessageProtocol != "NMEA")
            {
                builder.Append(0xe8);
                builder.Append(",");
                builder.Append(0x20);
                int num = 2;
                int num2 = 2;
                switch (this.eeControlComboBox.SelectedIndex)
                {
                    case 0:
                        num2 = 0;
                        break;

                    case 1:
                        num2 = 1;
                        break;

                    case 2:
                        num = 0;
                        break;

                    case 3:
                        num = 1;
                        break;

                    case 4:
                        num2 = 0;
                        num = 0;
                        break;

                    case 5:
                        num2 = 1;
                        num = 1;
                        break;
                }
                builder.Append(string.Format(",{0},{1}", num2, num));
                string msg = this.comm.m_Protocols.ConvertFieldsToRaw(builder.ToString(), messageName, "OSP");
                this.comm.WriteData(msg);
            }
        }

        private void setEEStorage()
        {
            string messageName = "MID_EE_INPUT EE_STORAGE_CONTROL";
            StringBuilder builder = new StringBuilder();
            if (this.comm.MessageProtocol != "NMEA")
            {
                builder.Append(0xe8);
                builder.Append(",");
                builder.Append(0xfd);
                builder.Append(",");
                int num = 0;
                if (this.eeStorageEEPromRadioBtn.Checked)
                {
                    num = 1;
                }
                else if (this.eeStorageFlashRadioBtn.Checked)
                {
                    num = 2;
                }
                else if (this.EEStorageNoneRadioBtn.Checked)
                {
                    num = 3;
                }
                else
                {
                    num = 0;
                }
                builder.Append(num);
                string msg = this.comm.m_Protocols.ConvertFieldsToRaw(builder.ToString(), messageName, "OSP");
                this.comm.WriteData(msg);
            }
        }
    }
}

