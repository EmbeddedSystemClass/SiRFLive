﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.MessageHandling;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class frmMaskPower : Form
    {
        private Button button_Cancel;
        private Button button_Send;
        private CommunicationManager comm;
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox maskPowerNavigation_txtBx;
        private TextBox maskPowerTracking_txtBx;

        public frmMaskPower()
        {
            this.InitializeComponent();
            this.maskPowerNavigation_txtBx.Text = "8";
        }

        public frmMaskPower(CommunicationManager parentComm)
        {
            this.InitializeComponent();
            this.comm = parentComm;
            this.maskPowerNavigation_txtBx.Text = "8";
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            try
            {
                Convert.ToInt16(this.maskPowerNavigation_txtBx.Text);
            }
            catch
            {
                MessageBox.Show("Incorrect value entered. Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if ((Convert.ToInt16(this.maskPowerNavigation_txtBx.Text) <= 50) && (Convert.ToInt16(this.maskPowerNavigation_txtBx.Text) >= 8))
            {
                this.SetPowerMaskControl(this.maskPowerNavigation_txtBx.Text);
                base.Close();
            }
            else
            {
                MessageBox.Show("Range is 8 to 50", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmMaskPower));
            this.button_Send = new Button();
            this.button_Cancel = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.maskPowerTracking_txtBx = new TextBox();
            this.maskPowerNavigation_txtBx = new TextBox();
            base.SuspendLayout();
            this.button_Send.Location = new Point(0xcd, 12);
            this.button_Send.Name = "button_Send";
            this.button_Send.Size = new Size(0x4b, 0x17);
            this.button_Send.TabIndex = 0;
            this.button_Send.Text = "&Send";
            this.button_Send.UseVisualStyleBackColor = true;
            this.button_Send.Click += new EventHandler(this.button_Send_Click);
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new Point(0xcd, 0x2a);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new Size(0x4b, 0x17);
            this.button_Cancel.TabIndex = 1;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new EventHandler(this.button_Cancel_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x86, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Set minimum satellite signal";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x15, 30);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x7a, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "power to be required for:";
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new Point(0x15, 0x3d);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x54, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Tracking (dBHz)";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x15, 0x5b);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x5d, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Navigation (dBHz)";
            this.maskPowerTracking_txtBx.Enabled = false;
            this.maskPowerTracking_txtBx.Location = new Point(0x73, 0x39);
            this.maskPowerTracking_txtBx.Name = "maskPowerTracking_txtBx";
            this.maskPowerTracking_txtBx.Size = new Size(0x2b, 20);
            this.maskPowerTracking_txtBx.TabIndex = 6;
            this.maskPowerTracking_txtBx.Text = "8";
            this.maskPowerNavigation_txtBx.Location = new Point(0x73, 0x58);
            this.maskPowerNavigation_txtBx.Name = "maskPowerNavigation_txtBx";
            this.maskPowerNavigation_txtBx.Size = new Size(0x2b, 20);
            this.maskPowerNavigation_txtBx.TabIndex = 7;
            this.maskPowerNavigation_txtBx.Text = "8";
            base.AcceptButton = this.button_Send;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.button_Cancel;
            base.ClientSize = new Size(0x124, 0x83);
            base.Controls.Add(this.maskPowerNavigation_txtBx);
            base.Controls.Add(this.maskPowerTracking_txtBx);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.button_Cancel);
            base.Controls.Add(this.button_Send);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmMaskPower";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Power Mask";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public virtual void SetPowerMaskControl(string navPower)
        {
            ArrayList list = new ArrayList();
            StringBuilder builder = new StringBuilder();
            list = this.comm.m_Protocols.GetInputMessageStructure(140, clsGlobal.noSID, "Power Mask", "SSB");
            for (int i = 0; i < list.Count; i++)
            {
                if (((InputMsg) list[i]).fieldName == "Navigation Mask")
                {
                    builder.Append(navPower);
                }
                else
                {
                    builder.Append(((InputMsg) list[i]).defaultValue);
                }
                builder.Append(",");
            }
            string msg = this.comm.m_Protocols.ConvertFieldsToRaw(builder.ToString().TrimEnd(new char[] { ',' }), "Power Mask", "OSP");
            this.comm.WriteData(msg);
        }
    }
}

