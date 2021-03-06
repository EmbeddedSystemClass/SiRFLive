﻿namespace SiRFLive.GUI.Commmunication
{
    using CommonUtilsClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmCommDisplayBufferSizeUpdate : Form
    {
        private CommunicationManager comm;
        private IContainer components;
        private CommonUtilsClass CUC;
        private Label frmCommOpenBufferSizeLabel;
        private TextBox frmCommOpenBufferSizeTxtBox;
        private Button frmCommSettingsCancelBtn;
        private Button frmCommSettingsOkBtn;

        public frmCommDisplayBufferSizeUpdate(CommunicationManager parentCommWindow)
        {
            this.CUC = new CommonUtilsClass();
            this.InitializeComponent();
            this.CommWindow = parentCommWindow;
            this.frmCommOpenBufferSizeTxtBox.Text = this.CUC.DisplayBuffer.ToString();
        }

        public frmCommDisplayBufferSizeUpdate(string updateString)
        {
            this.CUC = new CommonUtilsClass();
            this.InitializeComponent();
            this.frmCommOpenBufferSizeTxtBox.Text = updateString;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmCommSettingsCancelBtn_Click(object sender, EventArgs e)
        {
            base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            base.Close();
        }

        private void frmCommSettingsOkBtn_Click(object sender, EventArgs e)
        {
            CommonUtilsClass class2 = new CommonUtilsClass();
            try
            {
                if (this.comm != null)
                {
                    int num = Convert.ToInt32(this.frmCommOpenBufferSizeTxtBox.Text);
                    if (num > 0x1388)
                    {
                        MessageBox.Show("Max lines is 5000", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        num = 0x1388;
                    }
                    class2.DisplayBuffer = num;
                }
                else
                {
                    clsGlobal.MiscStringUpdate = this.frmCommOpenBufferSizeTxtBox.Text;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmCommDisplayBufferSizeUpdate));
            this.frmCommOpenBufferSizeTxtBox = new TextBox();
            this.frmCommSettingsCancelBtn = new Button();
            this.frmCommSettingsOkBtn = new Button();
            this.frmCommOpenBufferSizeLabel = new Label();
            base.SuspendLayout();
            this.frmCommOpenBufferSizeTxtBox.Location = new Point(0x93, 0x21);
            this.frmCommOpenBufferSizeTxtBox.Name = "frmCommOpenBufferSizeTxtBox";
            this.frmCommOpenBufferSizeTxtBox.Size = new Size(0x4d, 20);
            this.frmCommOpenBufferSizeTxtBox.TabIndex = 1;
            this.frmCommSettingsCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.frmCommSettingsCancelBtn.Location = new Point(0x95, 0x4f);
            this.frmCommSettingsCancelBtn.Name = "frmCommSettingsCancelBtn";
            this.frmCommSettingsCancelBtn.Size = new Size(0x4b, 0x17);
            this.frmCommSettingsCancelBtn.TabIndex = 3;
            this.frmCommSettingsCancelBtn.Text = "&Cancel";
            this.frmCommSettingsCancelBtn.UseVisualStyleBackColor = true;
            this.frmCommSettingsCancelBtn.Click += new EventHandler(this.frmCommSettingsCancelBtn_Click);
            this.frmCommSettingsOkBtn.Location = new Point(0x2c, 0x4f);
            this.frmCommSettingsOkBtn.Name = "frmCommSettingsOkBtn";
            this.frmCommSettingsOkBtn.Size = new Size(0x4b, 0x17);
            this.frmCommSettingsOkBtn.TabIndex = 2;
            this.frmCommSettingsOkBtn.Text = "&OK";
            this.frmCommSettingsOkBtn.UseVisualStyleBackColor = true;
            this.frmCommSettingsOkBtn.Click += new EventHandler(this.frmCommSettingsOkBtn_Click);
            this.frmCommOpenBufferSizeLabel.AutoSize = true;
            this.frmCommOpenBufferSizeLabel.Location = new Point(0x2c, 0x25);
            this.frmCommOpenBufferSizeLabel.Name = "frmCommOpenBufferSizeLabel";
            this.frmCommOpenBufferSizeLabel.Size = new Size(0x58, 13);
            this.frmCommOpenBufferSizeLabel.TabIndex = 0;
            this.frmCommOpenBufferSizeLabel.Text = "Buffer Size (lines)";
            base.AcceptButton = this.frmCommSettingsOkBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.frmCommSettingsCancelBtn;
            base.ClientSize = new Size(0x10c, 0x86);
            base.Controls.Add(this.frmCommOpenBufferSizeTxtBox);
            base.Controls.Add(this.frmCommSettingsCancelBtn);
            base.Controls.Add(this.frmCommSettingsOkBtn);
            base.Controls.Add(this.frmCommOpenBufferSizeLabel);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmCommDisplayBufferSizeUpdate";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Display Buffer Size Update";
            base.ResumeLayout(false);
            base.PerformLayout();
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

