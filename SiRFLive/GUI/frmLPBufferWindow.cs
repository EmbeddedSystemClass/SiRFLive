﻿namespace SiRFLive.GUI
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class frmLPBufferWindow : Form
    {
        private int CMD_COUNT;
        private Label cmdBufferOK2SendFlaglabel;
        private CommunicationManager comm;
        private IContainer components;
        private RadioButton frmCmdBufferForceRadioBtn;
        private RadioButton frmCmdBufferOnInputRadioBtn;
        private RadioButton frmLPCmdBufferAlwaysRadioBtn;
        private Button frmLPCmdBufferCancelBtn;
        private Button frmLPCmdBufferClearBtn;
        private Label frmLPCmdBufferCounter;
        private RadioButton frmLPCmdBufferForceFalseRadioBtn;
        private RichTextBox frmLPCmdBufferOK2SendTextBox;
        private Button frmLPCmdBufferOKBtn;
        private Button frmLPCmdBufferSendNowBtn;
        private Label frmLPCmdBufferStatus;
        private GroupBox groupBox1;
        private Label label2;

        public frmLPBufferWindow(CommunicationManager pComm)
        {
            this.InitializeComponent();
            this.comm = pComm;
            this.setFlagStatus(ref pComm);
        }

        private void clearBuffer(ref CommunicationManager targetComm)
        {
            this.clearCommands();
            this.setFlagStatus(ref targetComm);
            if (targetComm != null)
            {
                targetComm.ToSendMsgQueue.Clear();
            }
        }

        private void clearCommands()
        {
            this.frmLPCmdBufferOK2SendTextBox.Text = "";
            string str = Convert.ToString(this.CMD_COUNT);
            this.frmLPCmdBufferCounter.Text = "Command count: " + str;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmLPBufferWindow_Load(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                switch (this.comm.SendFlag)
                {
                    case 0:
                        this.frmLPCmdBufferAlwaysRadioBtn.Checked = true;
                        break;

                    case 1:
                        this.frmCmdBufferOnInputRadioBtn.Checked = true;
                        break;

                    case 2:
                        this.frmCmdBufferForceRadioBtn.Checked = true;
                        break;

                    case 3:
                        this.frmLPCmdBufferForceFalseRadioBtn.Checked = true;
                        break;
                }
            }
            this.frmLPLoadCommands();
        }

        private void frmLPCmdBufferCancelBtn_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void frmLPCmdBufferClearBtn_Click(object sender, EventArgs e)
        {
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                    this.clearBuffer(ref manager.comm);
                }
                clsGlobal.PerformOnAll = false;
            }
            else
            {
                this.clearBuffer(ref this.comm);
            }
        }

        private void frmLPCmdBufferOK2SendTextBox_DoubleClick(object sender, EventArgs e)
        {
            this.clearCommands();
        }

        private void frmLPCmdBufferOK2SendTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!this.comm.OK_TO_SEND)
            {
                this.frmLPLoadCommands();
            }
        }

        private void frmLPCmdBufferOKBtn_Click(object sender, EventArgs e)
        {
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                    this.sendFlag(ref manager.comm);
                }
                clsGlobal.PerformOnAll = false;
            }
            else
            {
                this.sendFlag(ref this.comm);
            }
            base.Close();
        }

        private void frmLPCmdBufferSendNowBtn_Click(object sender, EventArgs e)
        {
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                    this.sendBuffer(ref manager.comm);
                }
                clsGlobal.PerformOnAll = false;
            }
            else
            {
                this.sendBuffer(ref this.comm);
            }
        }

        private void frmLPLoadCommands()
        {
            try
            {
                if ((this.comm != null) && (this.comm.ToSendMsgQueue.Count > 0))
                {
                    StringBuilder builder = new StringBuilder();
                    object[] objArray = this.comm.ToSendMsgQueue.ToArray();
                    for (int i = 0; i < objArray.Length; i++)
                    {
                        builder.Append(((string) objArray[i]) + "\r\n");
                    }
                    this.frmLPCmdBufferOK2SendTextBox.Text = builder.ToString();
                    this.frmLPCmdBufferCounter.Text = "Command count: " + this.comm.ToSendMsgQueue.Count;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmLPBufferWindow));
            this.frmLPCmdBufferOKBtn = new Button();
            this.frmLPCmdBufferCancelBtn = new Button();
            this.frmLPCmdBufferSendNowBtn = new Button();
            this.frmLPCmdBufferAlwaysRadioBtn = new RadioButton();
            this.frmCmdBufferOnInputRadioBtn = new RadioButton();
            this.frmCmdBufferForceRadioBtn = new RadioButton();
            this.label2 = new Label();
            this.frmLPCmdBufferOK2SendTextBox = new RichTextBox();
            this.frmLPCmdBufferStatus = new Label();
            this.cmdBufferOK2SendFlaglabel = new Label();
            this.groupBox1 = new GroupBox();
            this.frmLPCmdBufferForceFalseRadioBtn = new RadioButton();
            this.frmLPCmdBufferClearBtn = new Button();
            this.frmLPCmdBufferCounter = new Label();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.frmLPCmdBufferOKBtn.Location = new Point(0x169, 30);
            this.frmLPCmdBufferOKBtn.Name = "frmLPCmdBufferOKBtn";
            this.frmLPCmdBufferOKBtn.Size = new Size(0x4b, 0x17);
            this.frmLPCmdBufferOKBtn.TabIndex = 1;
            this.frmLPCmdBufferOKBtn.Text = "&OK";
            this.frmLPCmdBufferOKBtn.UseVisualStyleBackColor = true;
            this.frmLPCmdBufferOKBtn.Click += new EventHandler(this.frmLPCmdBufferOKBtn_Click);
            this.frmLPCmdBufferCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.frmLPCmdBufferCancelBtn.Location = new Point(0x169, 0x3b);
            this.frmLPCmdBufferCancelBtn.Name = "frmLPCmdBufferCancelBtn";
            this.frmLPCmdBufferCancelBtn.Size = new Size(0x4b, 0x17);
            this.frmLPCmdBufferCancelBtn.TabIndex = 2;
            this.frmLPCmdBufferCancelBtn.Text = "&Cancel";
            this.frmLPCmdBufferCancelBtn.UseVisualStyleBackColor = true;
            this.frmLPCmdBufferCancelBtn.Click += new EventHandler(this.frmLPCmdBufferCancelBtn_Click);
            this.frmLPCmdBufferSendNowBtn.Location = new Point(0x169, 0xc6);
            this.frmLPCmdBufferSendNowBtn.Name = "frmLPCmdBufferSendNowBtn";
            this.frmLPCmdBufferSendNowBtn.Size = new Size(0x4b, 0x17);
            this.frmLPCmdBufferSendNowBtn.TabIndex = 3;
            this.frmLPCmdBufferSendNowBtn.Text = "&Send Now";
            this.frmLPCmdBufferSendNowBtn.UseVisualStyleBackColor = true;
            this.frmLPCmdBufferSendNowBtn.Click += new EventHandler(this.frmLPCmdBufferSendNowBtn_Click);
            this.frmLPCmdBufferAlwaysRadioBtn.AutoSize = true;
            this.frmLPCmdBufferAlwaysRadioBtn.Checked = true;
            this.frmLPCmdBufferAlwaysRadioBtn.Location = new Point(12, 0x12);
            this.frmLPCmdBufferAlwaysRadioBtn.Name = "frmLPCmdBufferAlwaysRadioBtn";
            this.frmLPCmdBufferAlwaysRadioBtn.Size = new Size(170, 0x11);
            this.frmLPCmdBufferAlwaysRadioBtn.TabIndex = 0;
            this.frmLPCmdBufferAlwaysRadioBtn.TabStop = true;
            this.frmLPCmdBufferAlwaysRadioBtn.Text = "&Always (Obey OK to Send flag)";
            this.frmLPCmdBufferAlwaysRadioBtn.UseVisualStyleBackColor = true;
            this.frmCmdBufferOnInputRadioBtn.AutoSize = true;
            this.frmCmdBufferOnInputRadioBtn.Location = new Point(12, 40);
            this.frmCmdBufferOnInputRadioBtn.Name = "frmCmdBufferOnInputRadioBtn";
            this.frmCmdBufferOnInputRadioBtn.Size = new Size(0x119, 0x11);
            this.frmCmdBufferOnInputRadioBtn.TabIndex = 1;
            this.frmCmdBufferOnInputRadioBtn.Text = "On &Input (Received Data implies OK to Send = TRUE)";
            this.frmCmdBufferOnInputRadioBtn.UseVisualStyleBackColor = true;
            this.frmCmdBufferForceRadioBtn.AutoSize = true;
            this.frmCmdBufferForceRadioBtn.Location = new Point(12, 0x3e);
            this.frmCmdBufferForceRadioBtn.Name = "frmCmdBufferForceRadioBtn";
            this.frmCmdBufferForceRadioBtn.Size = new Size(270, 0x11);
            this.frmCmdBufferForceRadioBtn.TabIndex = 2;
            this.frmCmdBufferForceRadioBtn.Text = "&Force OK to Send = TRUE (Send Data Immediately)";
            this.frmCmdBufferForceRadioBtn.UseVisualStyleBackColor = true;
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Italic | FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(0x22, 0x13d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x157, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Commands are buffered when the OK to Send flag = FALSE";
            this.frmLPCmdBufferOK2SendTextBox.Location = new Point(0x25, 0x94);
            this.frmLPCmdBufferOK2SendTextBox.Name = "frmLPCmdBufferOK2SendTextBox";
            this.frmLPCmdBufferOK2SendTextBox.Size = new Size(0x134, 0x99);
            this.frmLPCmdBufferOK2SendTextBox.TabIndex = 8;
            this.frmLPCmdBufferOK2SendTextBox.Text = "";
            this.frmLPCmdBufferOK2SendTextBox.WordWrap = false;
            this.frmLPCmdBufferOK2SendTextBox.DoubleClick += new EventHandler(this.frmLPCmdBufferOK2SendTextBox_DoubleClick);
            this.frmLPCmdBufferOK2SendTextBox.TextChanged += new EventHandler(this.frmLPCmdBufferOK2SendTextBox_TextChanged);
            this.frmLPCmdBufferStatus.AutoSize = true;
            this.frmLPCmdBufferStatus.Location = new Point(0x27, 0x83);
            this.frmLPCmdBufferStatus.Name = "frmLPCmdBufferStatus";
            this.frmLPCmdBufferStatus.Size = new Size(0x3d, 13);
            this.frmLPCmdBufferStatus.TabIndex = 9;
            this.frmLPCmdBufferStatus.Text = "Flag status:";
            this.cmdBufferOK2SendFlaglabel.AutoSize = true;
            this.cmdBufferOK2SendFlaglabel.Location = new Point(0x6a, 0x83);
            this.cmdBufferOK2SendFlaglabel.Name = "cmdBufferOK2SendFlaglabel";
            this.cmdBufferOK2SendFlaglabel.Size = new Size(0, 13);
            this.cmdBufferOK2SendFlaglabel.TabIndex = 10;
            this.groupBox1.Controls.Add(this.frmLPCmdBufferForceFalseRadioBtn);
            this.groupBox1.Controls.Add(this.frmLPCmdBufferAlwaysRadioBtn);
            this.groupBox1.Controls.Add(this.frmCmdBufferOnInputRadioBtn);
            this.groupBox1.Controls.Add(this.frmCmdBufferForceRadioBtn);
            this.groupBox1.Location = new Point(0x25, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x134, 0x6d);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Use OK to Send";
            this.frmLPCmdBufferForceFalseRadioBtn.AutoSize = true;
            this.frmLPCmdBufferForceFalseRadioBtn.Location = new Point(12, 0x54);
            this.frmLPCmdBufferForceFalseRadioBtn.Name = "frmLPCmdBufferForceFalseRadioBtn";
            this.frmLPCmdBufferForceFalseRadioBtn.Size = new Size(0xfc, 0x11);
            this.frmLPCmdBufferForceFalseRadioBtn.TabIndex = 3;
            this.frmLPCmdBufferForceFalseRadioBtn.TabStop = true;
            this.frmLPCmdBufferForceFalseRadioBtn.Text = "Force OK to Send = FALSE (Do Not Send Data)";
            this.frmLPCmdBufferForceFalseRadioBtn.UseVisualStyleBackColor = true;
            this.frmLPCmdBufferClearBtn.Location = new Point(0x169, 0xf2);
            this.frmLPCmdBufferClearBtn.Name = "frmLPCmdBufferClearBtn";
            this.frmLPCmdBufferClearBtn.Size = new Size(0x4b, 0x17);
            this.frmLPCmdBufferClearBtn.TabIndex = 4;
            this.frmLPCmdBufferClearBtn.Text = "Clear &Buffer";
            this.frmLPCmdBufferClearBtn.UseVisualStyleBackColor = true;
            this.frmLPCmdBufferClearBtn.Click += new EventHandler(this.frmLPCmdBufferClearBtn_Click);
            this.frmLPCmdBufferCounter.AutoSize = true;
            this.frmLPCmdBufferCounter.Location = new Point(220, 0x83);
            this.frmLPCmdBufferCounter.Name = "frmLPCmdBufferCounter";
            this.frmLPCmdBufferCounter.Size = new Size(0x60, 13);
            this.frmLPCmdBufferCounter.TabIndex = 14;
            this.frmLPCmdBufferCounter.Text = "Command count: 0";
            base.AcceptButton = this.frmLPCmdBufferOKBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.frmLPCmdBufferCancelBtn;
            base.ClientSize = new Size(0x1bc, 350);
            base.Controls.Add(this.frmLPCmdBufferCounter);
            base.Controls.Add(this.frmLPCmdBufferClearBtn);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.cmdBufferOK2SendFlaglabel);
            base.Controls.Add(this.frmLPCmdBufferStatus);
            base.Controls.Add(this.frmLPCmdBufferOK2SendTextBox);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.frmLPCmdBufferSendNowBtn);
            base.Controls.Add(this.frmLPCmdBufferCancelBtn);
            base.Controls.Add(this.frmLPCmdBufferOKBtn);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmLPBufferWindow";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Low Power Command Buffer Window";
            base.Load += new EventHandler(this.frmLPBufferWindow_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void sendBuffer(ref CommunicationManager targetComm)
        {
            bool flag = targetComm.OK_TO_SEND;
            targetComm.OK_TO_SEND = true;
            this.setFlagStatus(ref targetComm);
            targetComm.WriteData();
            this.clearCommands();
            targetComm.OK_TO_SEND = flag;
        }

        private void sendFlag(ref CommunicationManager targetComm)
        {
            if (this.frmLPCmdBufferAlwaysRadioBtn.Checked)
            {
                targetComm.SendFlag = 0;
            }
            else if (this.frmCmdBufferOnInputRadioBtn.Checked)
            {
                targetComm.SendFlag = 1;
            }
            else if (this.frmCmdBufferForceRadioBtn.Checked)
            {
                targetComm.SendFlag = 2;
                targetComm.OK_TO_SEND = true;
                targetComm.WriteData();
            }
            else if (this.frmLPCmdBufferForceFalseRadioBtn.Checked)
            {
                targetComm.SendFlag = 3;
                targetComm.OK_TO_SEND = false;
            }
        }

        private void setFlagStatus(ref CommunicationManager targetComm)
        {
            string str = Convert.ToString(targetComm.OK_TO_SEND);
            this.frmLPCmdBufferStatus.Text = "Flag status: " + str;
        }
    }
}

