﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using SiRFLive.Communication;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmTrackerSwitchIOFormat : Form
    {
        private CommunicationManager comm;
        private IContainer components;
        private Button frmSwitchIOCancelBtn;
        private Label frmSwitchIOConfigKeywordLabel;
        private Label frmSwitchIOConfigTypeLabel;
        private TextBox frmSwitchIOConfigTypeTextBox;
        private Label frmSwitchIOConfigVersionLabel;
        private TextBox frmSwitchIOConfigVersionTextBox;
        private TextBox frmSwitchIOKeywordTextBox;
        private Button frmSwitchIOOkBtn;
        private GroupBox groupBox1;

        public frmTrackerSwitchIOFormat(CommunicationManager parentComm)
        {
            this.comm = parentComm;
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

        private void frmSwitchIOCancelBtn_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void frmSwitchIOOkBtn_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            int num = 0;
            if (this.comm == null)
            {
                text = "Comm window in NULL";
                MessageBox.Show(text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                base.Close();
            }
            else
            {
                try
                {
                    this.comm.TrackerICCtrl.ioType = Convert.ToByte(this.frmSwitchIOConfigTypeTextBox.Text);
                }
                catch (Exception exception)
                {
                    text = text + "Error set config type: " + exception.Message + "\n";
                    num++;
                }
                try
                {
                    this.comm.TrackerICCtrl.ioVersion = Convert.ToByte(this.frmSwitchIOConfigVersionTextBox.Text);
                }
                catch (Exception exception2)
                {
                    text = text + "Error set config version: " + exception2.Message + "\n";
                }
                try
                {
                    this.comm.TrackerICCtrl.ioPin = Convert.ToByte(this.frmSwitchIOKeywordTextBox.Text);
                }
                catch (Exception exception3)
                {
                    text = "Error set config keyword: " + exception3.Message + "\n";
                }
                if (num > 0)
                {
                    MessageBox.Show(text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    base.Close();
                }
            }
        }

        private void frmTrackerSwitchIOFormat_Load(object sender, EventArgs e)
        {
            string text = string.Empty;
            int num = 0;
            try
            {
                this.frmSwitchIOConfigTypeTextBox.Text = this.comm.TrackerICCtrl.ioType.ToString();
            }
            catch (Exception exception)
            {
                text = text + "Error Config Type: " + exception.Message + "\n";
                num++;
            }
            try
            {
                this.frmSwitchIOConfigVersionTextBox.Text = this.comm.TrackerICCtrl.ioVersion.ToString();
            }
            catch (Exception exception2)
            {
                text = text + "Error Config Version: " + exception2.Message + "\n";
                num++;
            }
            try
            {
                this.frmSwitchIOKeywordTextBox.Text = this.comm.TrackerICCtrl.ioPin.ToString();
            }
            catch (Exception exception3)
            {
                text = text + "Error keyword: " + exception3.Message + "\n";
                num++;
            }
            if (num > 0)
            {
                MessageBox.Show(text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmTrackerSwitchIOFormat));
            this.groupBox1 = new GroupBox();
            this.frmSwitchIOCancelBtn = new Button();
            this.frmSwitchIOOkBtn = new Button();
            this.frmSwitchIOConfigKeywordLabel = new Label();
            this.frmSwitchIOConfigVersionLabel = new Label();
            this.frmSwitchIOKeywordTextBox = new TextBox();
            this.frmSwitchIOConfigVersionTextBox = new TextBox();
            this.frmSwitchIOConfigTypeTextBox = new TextBox();
            this.frmSwitchIOConfigTypeLabel = new Label();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.frmSwitchIOCancelBtn);
            this.groupBox1.Controls.Add(this.frmSwitchIOOkBtn);
            this.groupBox1.Controls.Add(this.frmSwitchIOConfigKeywordLabel);
            this.groupBox1.Controls.Add(this.frmSwitchIOConfigVersionLabel);
            this.groupBox1.Controls.Add(this.frmSwitchIOKeywordTextBox);
            this.groupBox1.Controls.Add(this.frmSwitchIOConfigVersionTextBox);
            this.groupBox1.Controls.Add(this.frmSwitchIOConfigTypeTextBox);
            this.groupBox1.Controls.Add(this.frmSwitchIOConfigTypeLabel);
            this.groupBox1.Location = new Point(0x19, 0x1c);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x124, 180);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Set I/O Configuration";
            this.frmSwitchIOCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.frmSwitchIOCancelBtn.Location = new Point(0xcc, 0x84);
            this.frmSwitchIOCancelBtn.Name = "frmSwitchIOCancelBtn";
            this.frmSwitchIOCancelBtn.Size = new Size(0x4b, 0x17);
            this.frmSwitchIOCancelBtn.TabIndex = 4;
            this.frmSwitchIOCancelBtn.Text = "&Cancel";
            this.frmSwitchIOCancelBtn.UseVisualStyleBackColor = true;
            this.frmSwitchIOCancelBtn.Click += new EventHandler(this.frmSwitchIOCancelBtn_Click);
            this.frmSwitchIOOkBtn.Location = new Point(120, 0x84);
            this.frmSwitchIOOkBtn.Name = "frmSwitchIOOkBtn";
            this.frmSwitchIOOkBtn.Size = new Size(0x4b, 0x17);
            this.frmSwitchIOOkBtn.TabIndex = 4;
            this.frmSwitchIOOkBtn.Text = "&OK";
            this.frmSwitchIOOkBtn.UseVisualStyleBackColor = true;
            this.frmSwitchIOOkBtn.Click += new EventHandler(this.frmSwitchIOOkBtn_Click);
            this.frmSwitchIOConfigKeywordLabel.AutoSize = true;
            this.frmSwitchIOConfigKeywordLabel.Location = new Point(13, 0x68);
            this.frmSwitchIOConfigKeywordLabel.Name = "frmSwitchIOConfigKeywordLabel";
            this.frmSwitchIOConfigKeywordLabel.Size = new Size(0x33, 13);
            this.frmSwitchIOConfigKeywordLabel.TabIndex = 3;
            this.frmSwitchIOConfigKeywordLabel.Text = "Keyword:";
            this.frmSwitchIOConfigVersionLabel.AutoSize = true;
            this.frmSwitchIOConfigVersionLabel.Location = new Point(13, 0x47);
            this.frmSwitchIOConfigVersionLabel.Name = "frmSwitchIOConfigVersionLabel";
            this.frmSwitchIOConfigVersionLabel.Size = new Size(0x61, 13);
            this.frmSwitchIOConfigVersionLabel.TabIndex = 2;
            this.frmSwitchIOConfigVersionLabel.Text = "I/O Config Version:";
            this.frmSwitchIOKeywordTextBox.Location = new Point(120, 0x61);
            this.frmSwitchIOKeywordTextBox.Name = "frmSwitchIOKeywordTextBox";
            this.frmSwitchIOKeywordTextBox.Size = new Size(0x9f, 20);
            this.frmSwitchIOKeywordTextBox.TabIndex = 1;
            this.frmSwitchIOConfigVersionTextBox.Location = new Point(120, 0x40);
            this.frmSwitchIOConfigVersionTextBox.Name = "frmSwitchIOConfigVersionTextBox";
            this.frmSwitchIOConfigVersionTextBox.Size = new Size(0x9f, 20);
            this.frmSwitchIOConfigVersionTextBox.TabIndex = 1;
            this.frmSwitchIOConfigTypeTextBox.Location = new Point(120, 0x19);
            this.frmSwitchIOConfigTypeTextBox.Name = "frmSwitchIOConfigTypeTextBox";
            this.frmSwitchIOConfigTypeTextBox.Size = new Size(0x9f, 20);
            this.frmSwitchIOConfigTypeTextBox.TabIndex = 1;
            this.frmSwitchIOConfigTypeLabel.AutoSize = true;
            this.frmSwitchIOConfigTypeLabel.Location = new Point(13, 0x1d);
            this.frmSwitchIOConfigTypeLabel.Name = "frmSwitchIOConfigTypeLabel";
            this.frmSwitchIOConfigTypeLabel.Size = new Size(0x56, 13);
            this.frmSwitchIOConfigTypeLabel.TabIndex = 0;
            this.frmSwitchIOConfigTypeLabel.Text = "I/O Config Type:";
            base.AcceptButton = this.frmSwitchIOOkBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.frmSwitchIOCancelBtn;
            base.ClientSize = new Size(0x157, 0xec);
            base.Controls.Add(this.groupBox1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmTrackerSwitchIOFormat";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Custom I/O";
            base.Load += new EventHandler(this.frmTrackerSwitchIOFormat_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        public CommunicationManager CommWindow
        {
            get
            {
                return this.comm;
            }
            set
            {
                this.comm = value;
            }
        }
    }
}

