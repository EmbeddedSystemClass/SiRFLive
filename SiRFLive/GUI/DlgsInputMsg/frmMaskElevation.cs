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

    public class frmMaskElevation : Form
    {
        private Button button_Cancel;
        private Button button_Send;
        private CommunicationManager comm;
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox maskElevNavigation_txtBx;
        private TextBox maskElevTracking_txtBx;

        public frmMaskElevation()
        {
            this.InitializeComponent();
            this.maskElevNavigation_txtBx.Text = "5.0";
        }

        public frmMaskElevation(CommunicationManager parentComm)
        {
            this.InitializeComponent();
            this.comm = parentComm;
            this.maskElevNavigation_txtBx.Text = "5.0";
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            try
            {
                Convert.ToDouble(this.maskElevNavigation_txtBx.Text);
            }
            catch
            {
                MessageBox.Show("Incorrect value entered. Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if ((Convert.ToDouble(this.maskElevNavigation_txtBx.Text) <= 90.0) && (Convert.ToDouble(this.maskElevNavigation_txtBx.Text) >= 0.0))
            {
                this.SetElevationMaskControl(this.maskElevNavigation_txtBx.Text);
                base.Close();
            }
            else
            {
                MessageBox.Show("Range is 0.0 to 90.0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmMaskElevation));
            this.button_Send = new Button();
            this.button_Cancel = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.maskElevTracking_txtBx = new TextBox();
            this.maskElevNavigation_txtBx = new TextBox();
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
            this.label1.Size = new Size(150, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Set minimum satellite elevation";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x15, 30);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x77, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "angle to be required for:";
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new Point(0x15, 0x3d);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x4c, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Tracking (deg)";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x15, 0x5b);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x55, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Navigation (deg)";
            this.maskElevTracking_txtBx.Enabled = false;
            this.maskElevTracking_txtBx.Location = new Point(0x6b, 0x39);
            this.maskElevTracking_txtBx.Name = "maskElevTracking_txtBx";
            this.maskElevTracking_txtBx.Size = new Size(0x2b, 20);
            this.maskElevTracking_txtBx.TabIndex = 6;
            this.maskElevTracking_txtBx.Text = "5.0";
            this.maskElevNavigation_txtBx.Location = new Point(0x6b, 0x58);
            this.maskElevNavigation_txtBx.Name = "maskElevNavigation_txtBx";
            this.maskElevNavigation_txtBx.Size = new Size(0x2b, 20);
            this.maskElevNavigation_txtBx.TabIndex = 7;
            this.maskElevNavigation_txtBx.Text = "5.0";
            base.AcceptButton = this.button_Send;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.button_Cancel;
            base.ClientSize = new Size(0x124, 0x83);
            base.Controls.Add(this.maskElevNavigation_txtBx);
            base.Controls.Add(this.maskElevTracking_txtBx);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.button_Cancel);
            base.Controls.Add(this.button_Send);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmMaskElevation";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Elevation Mask";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public virtual void SetElevationMaskControl(string elevMask)
        {
            ArrayList list = new ArrayList();
            StringBuilder builder = new StringBuilder();
            list = this.comm.m_Protocols.GetInputMessageStructure(0x8b, clsGlobal.noSID, "Elevation Mask", "SSB");
            for (int i = 0; i < list.Count; i++)
            {
                if (((InputMsg) list[i]).fieldName == "Navigation Mask")
                {
                    builder.Append(elevMask);
                }
                else
                {
                    builder.Append(((InputMsg) list[i]).defaultValue);
                }
                builder.Append(",");
            }
            string msg = this.comm.m_Protocols.ConvertFieldsToRaw(builder.ToString().TrimEnd(new char[] { ',' }), "Elevation Mask", "OSP");
            this.comm.WriteData(msg);
        }
    }
}

