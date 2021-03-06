﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class frmSetGPSDRMode : Form
    {
        private Button btn_Cancel;
        private Button btn_Send;
        private CommunicationManager comm;
        private IContainer components;
        private RadioButton rdBtn_DRNavOnly;
        private RadioButton rdBtn_DRNavwCurrentCal;
        private RadioButton rdBtn_DrNavwStoredCal;
        private RadioButton rdBtn_GPSOnly;

        public frmSetGPSDRMode(CommunicationManager parentComm)
        {
            this.InitializeComponent();
            this.comm = parentComm;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            string messageName = "Set GPS/DR Navigation Mode";
            byte num = 0;
            if (this.rdBtn_GPSOnly.Checked)
            {
                num = 1;
            }
            else if (this.rdBtn_DrNavwStoredCal.Checked)
            {
                num = 2;
            }
            else if (this.rdBtn_DRNavwCurrentCal.Checked)
            {
                num = 4;
            }
            else if (this.rdBtn_DRNavOnly.Checked)
            {
                num = 8;
            }
            else
            {
                num = 1;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("172,2,{0},0", num));
            string msg = this.comm.m_Protocols.ConvertFieldsToRaw(builder.ToString(), messageName, "SSB");
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str3 in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    if (!(str3 == clsGlobal.FilePlayBackPortName))
                    {
                        PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str3];
                        if (manager != null)
                        {
                            manager.comm.WriteData(msg);
                        }
                    }
                }
                clsGlobal.PerformOnAll = false;
            }
            else
            {
                this.comm.WriteData(msg);
            }
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

        private void frmSetGPSDRMode_Load(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmSetGPSDRMode));
            this.rdBtn_GPSOnly = new RadioButton();
            this.rdBtn_DRNavOnly = new RadioButton();
            this.rdBtn_DrNavwStoredCal = new RadioButton();
            this.rdBtn_DRNavwCurrentCal = new RadioButton();
            this.btn_Send = new Button();
            this.btn_Cancel = new Button();
            base.SuspendLayout();
            this.rdBtn_GPSOnly.AutoSize = true;
            this.rdBtn_GPSOnly.Location = new Point(0x18, 0x13);
            this.rdBtn_GPSOnly.Name = "rdBtn_GPSOnly";
            this.rdBtn_GPSOnly.Size = new Size(0x5e, 0x11);
            this.rdBtn_GPSOnly.TabIndex = 0;
            this.rdBtn_GPSOnly.TabStop = true;
            this.rdBtn_GPSOnly.Text = "GPS Nav Only";
            this.rdBtn_GPSOnly.UseVisualStyleBackColor = true;
            this.rdBtn_DRNavOnly.AutoSize = true;
            this.rdBtn_DRNavOnly.Location = new Point(0x18, 50);
            this.rdBtn_DRNavOnly.Name = "rdBtn_DRNavOnly";
            this.rdBtn_DRNavOnly.Size = new Size(0x58, 0x11);
            this.rdBtn_DRNavOnly.TabIndex = 0;
            this.rdBtn_DRNavOnly.TabStop = true;
            this.rdBtn_DRNavOnly.Text = "DR Nav Only";
            this.rdBtn_DRNavOnly.UseVisualStyleBackColor = true;
            this.rdBtn_DrNavwStoredCal.AutoSize = true;
            this.rdBtn_DrNavwStoredCal.Location = new Point(0x18, 0x51);
            this.rdBtn_DrNavwStoredCal.Name = "rdBtn_DrNavwStoredCal";
            this.rdBtn_DrNavwStoredCal.Size = new Size(0xe3, 0x11);
            this.rdBtn_DrNavwStoredCal.TabIndex = 0;
            this.rdBtn_DrNavwStoredCal.TabStop = true;
            this.rdBtn_DrNavwStoredCal.Text = "DR Nav Ok w/ stored or default calibration";
            this.rdBtn_DrNavwStoredCal.UseVisualStyleBackColor = true;
            this.rdBtn_DRNavwCurrentCal.AutoSize = true;
            this.rdBtn_DRNavwCurrentCal.Location = new Point(0x18, 0x70);
            this.rdBtn_DRNavwCurrentCal.Name = "rdBtn_DRNavwCurrentCal";
            this.rdBtn_DRNavwCurrentCal.Size = new Size(0xcc, 0x11);
            this.rdBtn_DRNavwCurrentCal.TabIndex = 0;
            this.rdBtn_DRNavwCurrentCal.TabStop = true;
            this.rdBtn_DRNavwCurrentCal.Text = "Dr Nav Ok w/ current GPS calibration";
            this.rdBtn_DRNavwCurrentCal.UseVisualStyleBackColor = true;
            this.btn_Send.Location = new Point(0x12b, 0x17);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new Size(0x4d, 0x17);
            this.btn_Send.TabIndex = 1;
            this.btn_Send.Text = "Send";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new EventHandler(this.btn_Send_Click);
            this.btn_Cancel.Location = new Point(0x12b, 0x3e);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new Size(0x4d, 0x17);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x192, 0x9c);
            base.Controls.Add(this.btn_Cancel);
            base.Controls.Add(this.btn_Send);
            base.Controls.Add(this.rdBtn_DRNavwCurrentCal);
            base.Controls.Add(this.rdBtn_DrNavwStoredCal);
            base.Controls.Add(this.rdBtn_DRNavOnly);
            base.Controls.Add(this.rdBtn_GPSOnly);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmSetGPSDRMode";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Set GPS DR Mode";
            base.Load += new EventHandler(this.frmSetGPSDRMode_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

