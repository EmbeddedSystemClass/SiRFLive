﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using SiRFLive.Communication;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmMPMConfigure : Form
    {
        private lowPowerParams _mpmParams;
        private IContainer components;
        private Button mpmConfigCancelBtn;
        private ComboBox mpmConfigMessageModeComboBox;
        private Label mpmConfigMessageModeLabel;
        private ComboBox mpmConfigRTCUncertaintyComboBox;
        private Label mpmConfigRTCUncertaintyLabel;
        private NumericUpDown mpmConfigTimeNumericBox;
        private Button mpmConfigUpdateBtn;
        private Label mpmTimeoutLabel;

        public frmMPMConfigure(ref lowPowerParams target)
        {
            this.InitializeComponent();
            this._mpmParams = target;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmMPMConfigure_Load(object sender, EventArgs e)
        {
            if (this._mpmParams == null)
            {
                this.mpmConfigMessageModeComboBox.SelectedIndex = 1;
                this.mpmConfigRTCUncertaintyComboBox.SelectedIndex = 0;
                this.mpmConfigTimeNumericBox.Value = 60M;
            }
            else
            {
                if (this._mpmParams.MPM_Timeout > 0xff)
                {
                    this.mpmConfigTimeNumericBox.Value = 255M;
                }
                else if (this._mpmParams.MPM_Timeout < 0)
                {
                    this.mpmConfigTimeNumericBox.Value = 0M;
                }
                else
                {
                    this.mpmConfigTimeNumericBox.Value = this._mpmParams.MPM_Timeout;
                }
                switch ((this._mpmParams.MPM_Control & 3))
                {
                    case 0:
                        this.mpmConfigRTCUncertaintyComboBox.SelectedIndex = 0;
                        break;

                    case 1:
                        this.mpmConfigRTCUncertaintyComboBox.SelectedIndex = 1;
                        break;
                }
                switch (((this._mpmParams.MPM_Control >> 2) & 3))
                {
                    case 0:
                        this.mpmConfigMessageModeComboBox.SelectedIndex = 0;
                        return;

                    case 1:
                        this.mpmConfigMessageModeComboBox.SelectedIndex = 1;
                        return;
                }
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmMPMConfigure));
            this.mpmTimeoutLabel = new Label();
            this.mpmConfigRTCUncertaintyLabel = new Label();
            this.mpmConfigTimeNumericBox = new NumericUpDown();
            this.mpmConfigUpdateBtn = new Button();
            this.mpmConfigCancelBtn = new Button();
            this.mpmConfigRTCUncertaintyComboBox = new ComboBox();
            this.mpmConfigMessageModeLabel = new Label();
            this.mpmConfigMessageModeComboBox = new ComboBox();
            this.mpmConfigTimeNumericBox.BeginInit();
            base.SuspendLayout();
            this.mpmTimeoutLabel.AutoSize = true;
            this.mpmTimeoutLabel.Location = new Point(0x15, 0x21);
            this.mpmTimeoutLabel.Name = "mpmTimeoutLabel";
            this.mpmTimeoutLabel.Size = new Size(0x30, 13);
            this.mpmTimeoutLabel.TabIndex = 0;
            this.mpmTimeoutLabel.Text = "Timeout:";
            this.mpmConfigRTCUncertaintyLabel.AutoSize = true;
            this.mpmConfigRTCUncertaintyLabel.Location = new Point(0x15, 0x3e);
            this.mpmConfigRTCUncertaintyLabel.Name = "mpmConfigRTCUncertaintyLabel";
            this.mpmConfigRTCUncertaintyLabel.Size = new Size(0x6d, 13);
            this.mpmConfigRTCUncertaintyLabel.TabIndex = 1;
            this.mpmConfigRTCUncertaintyLabel.Text = "RTC Uncertainty (\x00b5s):";
            this.mpmConfigTimeNumericBox.Location = new Point(0x8d, 0x1d);
            int[] bits = new int[4];
            bits[0] = 0xff;
            this.mpmConfigTimeNumericBox.Maximum = new decimal(bits);
            this.mpmConfigTimeNumericBox.Name = "mpmConfigTimeNumericBox";
            this.mpmConfigTimeNumericBox.Size = new Size(0x5e, 20);
            this.mpmConfigTimeNumericBox.TabIndex = 2;
            this.mpmConfigUpdateBtn.Location = new Point(0x1b, 0x91);
            this.mpmConfigUpdateBtn.Name = "mpmConfigUpdateBtn";
            this.mpmConfigUpdateBtn.Size = new Size(0x4b, 0x17);
            this.mpmConfigUpdateBtn.TabIndex = 4;
            this.mpmConfigUpdateBtn.Text = "Update";
            this.mpmConfigUpdateBtn.UseVisualStyleBackColor = true;
            this.mpmConfigUpdateBtn.Click += new EventHandler(this.mpmConfigUpdateBtn_Click);
            this.mpmConfigCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.mpmConfigCancelBtn.Location = new Point(0x80, 0x91);
            this.mpmConfigCancelBtn.Name = "mpmConfigCancelBtn";
            this.mpmConfigCancelBtn.Size = new Size(0x4b, 0x17);
            this.mpmConfigCancelBtn.TabIndex = 5;
            this.mpmConfigCancelBtn.Text = "Cancel";
            this.mpmConfigCancelBtn.UseVisualStyleBackColor = true;
            this.mpmConfigCancelBtn.Click += new EventHandler(this.mpmConfigCancelBtn_Click);
            this.mpmConfigRTCUncertaintyComboBox.FormattingEnabled = true;
            this.mpmConfigRTCUncertaintyComboBox.Items.AddRange(new object[] { "250", "125" });
            this.mpmConfigRTCUncertaintyComboBox.Location = new Point(140, 0x3a);
            this.mpmConfigRTCUncertaintyComboBox.Name = "mpmConfigRTCUncertaintyComboBox";
            this.mpmConfigRTCUncertaintyComboBox.Size = new Size(0x5e, 0x15);
            this.mpmConfigRTCUncertaintyComboBox.TabIndex = 6;
            this.mpmConfigMessageModeLabel.AutoSize = true;
            this.mpmConfigMessageModeLabel.Location = new Point(0x15, 0x5c);
            this.mpmConfigMessageModeLabel.Name = "mpmConfigMessageModeLabel";
            this.mpmConfigMessageModeLabel.Size = new Size(0x53, 13);
            this.mpmConfigMessageModeLabel.TabIndex = 1;
            this.mpmConfigMessageModeLabel.Text = "Message Mode:";
            this.mpmConfigMessageModeComboBox.FormattingEnabled = true;
            this.mpmConfigMessageModeComboBox.Items.AddRange(new object[] { "Production", "Evaluation" });
            this.mpmConfigMessageModeComboBox.Location = new Point(140, 0x58);
            this.mpmConfigMessageModeComboBox.Name = "mpmConfigMessageModeComboBox";
            this.mpmConfigMessageModeComboBox.Size = new Size(0x5e, 0x15);
            this.mpmConfigMessageModeComboBox.TabIndex = 6;
            base.AcceptButton = this.mpmConfigUpdateBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.mpmConfigCancelBtn;
            base.ClientSize = new Size(0x101, 0xc5);
            base.Controls.Add(this.mpmConfigMessageModeComboBox);
            base.Controls.Add(this.mpmConfigRTCUncertaintyComboBox);
            base.Controls.Add(this.mpmConfigCancelBtn);
            base.Controls.Add(this.mpmConfigUpdateBtn);
            base.Controls.Add(this.mpmConfigMessageModeLabel);
            base.Controls.Add(this.mpmConfigTimeNumericBox);
            base.Controls.Add(this.mpmConfigRTCUncertaintyLabel);
            base.Controls.Add(this.mpmTimeoutLabel);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmMPMConfigure";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SiRFaware Configuration";
            base.Load += new EventHandler(this.frmMPMConfigure_Load);
            this.mpmConfigTimeNumericBox.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void mpmConfigCancelBtn_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void mpmConfigUpdateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this._mpmParams.MPM_Timeout = Convert.ToByte(this.mpmConfigTimeNumericBox.Value);
            }
            catch
            {
                MessageBox.Show("Error in timeout input", "MPM Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            try
            {
                this._mpmParams.MPM_Control = (byte) (this.mpmConfigRTCUncertaintyComboBox.SelectedIndex | (this.mpmConfigMessageModeComboBox.SelectedIndex << 2));
                if (this.mpmConfigMessageModeComboBox.SelectedIndex == 0)
                {
                    string text = "No information will be available in production mode!\n\nProceed?\n";
                    if (MessageBox.Show(text, "MPM Configuration Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error in Message Mode and RTC inputs", "MPM Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            base.Close();
        }
    }
}

