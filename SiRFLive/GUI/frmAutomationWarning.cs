﻿namespace SiRFLive.GUI
{
    using SiRFLive.General;
    using SiRFLive.Properties;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class frmAutomationWarning : Form
    {
        private string _testName = string.Empty;
        private Button cancelBtn;
        private IContainer components;
        private CheckBox donotShowAgainChkBox;
        private Button okBtn;
        private PictureBox pictureBox1;
        private Label warningStr;

        public frmAutomationWarning(string testName)
        {
            this.InitializeComponent();
            this._testName = testName;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.updateShowWarningBox();
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

        private void frmAutomationWarning_Load(object sender, EventArgs e)
        {
            if ((this._testName == "3GPP") || (this._testName == "TIA916"))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("\n\n3GPP requires a Spirent STR4500 or GSS6700 Simulator\n");
                builder.Append("with special scenarios.\n\n");
                builder.Append("Please make sure everything is in place before proceeding!\n\n");
                builder.Append("For more information, please refer to the SiRFLive User Manual\n");
                builder.Append("\t\t\"Help -> User Manual\"\n\n");
                builder.Append("\t\tProceed?");
                this.warningStr.Text = this.warningStr.Text + builder.ToString();
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmAutomationWarning));
            this.warningStr = new Label();
            this.donotShowAgainChkBox = new CheckBox();
            this.okBtn = new Button();
            this.cancelBtn = new Button();
            this.pictureBox1 = new PictureBox();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.warningStr.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.warningStr.AutoSize = true;
            this.warningStr.BackColor = Color.Transparent;
            this.warningStr.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.warningStr.Location = new Point(0x3d, 0x1f);
            this.warningStr.Name = "warningStr";
            this.warningStr.Size = new Size(0x49, 0x10);
            this.warningStr.TabIndex = 0;
            this.warningStr.Text = "Warning: ";
            this.donotShowAgainChkBox.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.donotShowAgainChkBox.AutoSize = true;
            this.donotShowAgainChkBox.BackColor = Color.Transparent;
            this.donotShowAgainChkBox.Location = new Point(0x20, 0xee);
            this.donotShowAgainChkBox.Name = "donotShowAgainChkBox";
            this.donotShowAgainChkBox.Size = new Size(0xba, 0x11);
            this.donotShowAgainChkBox.TabIndex = 1;
            this.donotShowAgainChkBox.Text = "Do not display this message again";
            this.donotShowAgainChkBox.UseVisualStyleBackColor = false;
            this.okBtn.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.okBtn.Location = new Point(190, 200);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new Size(0x4b, 0x17);
            this.okBtn.TabIndex = 2;
            this.okBtn.Text = "&Yes";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new EventHandler(this.okBtn_Click);
            this.cancelBtn.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new Point(290, 200);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new Size(0x4b, 0x17);
            this.cancelBtn.TabIndex = 3;
            this.cancelBtn.Text = "&No";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
            this.pictureBox1.Image = Resources.BreakpointHS;
            this.pictureBox1.Location = new Point(0x20, 0x1f);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x19, 0x19);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            base.AcceptButton = this.okBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            base.CancelButton = this.cancelBtn;
            base.ClientSize = new Size(0x22a, 0x111);
            base.Controls.Add(this.pictureBox1);
            base.Controls.Add(this.cancelBtn);
            base.Controls.Add(this.okBtn);
            base.Controls.Add(this.donotShowAgainChkBox);
            base.Controls.Add(this.warningStr);
            this.DoubleBuffered = true;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            this.MinimumSize = new Size(0x232, 300);
            base.Name = "frmAutomationWarning";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Automation Warning";
            base.Load += new EventHandler(this.frmAutomationWarning_Load);
            ((ISupportInitialize) this.pictureBox1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            this.updateShowWarningBox();
            base.Close();
        }

        private void updateShowWarningBox()
        {
            if (this.donotShowAgainChkBox.Checked)
            {
                clsGlobal.SiRFLiveAppConfig.AppSettings.Settings.Remove("Show3GPPWarning");
                clsGlobal.SiRFLiveAppConfig.AppSettings.Settings.Add("Show3GPPWarning", "0");
                clsGlobal.SiRFLiveAppConfig.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
    }
}

