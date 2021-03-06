﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class frmSetDRGyroFacCal : Form
    {
        private Button btn_Cancel;
        private Button btn_Send;
        private CommunicationManager comm;
        private IContainer components;
        private RadioButton rdBtn_StartGRBiasCal;
        private RadioButton rdBtnStartGRSclfacCal;

        public frmSetDRGyroFacCal(CommunicationManager parentComm)
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
            string messageName = "Set DR Gyro Factory Calibration";
            byte num = 0;
            if (this.rdBtn_StartGRBiasCal.Checked)
            {
                num = 1;
            }
            else if (this.rdBtnStartGRSclfacCal.Checked)
            {
                num = 2;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("172,3,{0},0", num));
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

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmSetDRGyroFacCal));
            this.rdBtn_StartGRBiasCal = new RadioButton();
            this.rdBtnStartGRSclfacCal = new RadioButton();
            this.btn_Send = new Button();
            this.btn_Cancel = new Button();
            base.SuspendLayout();
            this.rdBtn_StartGRBiasCal.AutoSize = true;
            this.rdBtn_StartGRBiasCal.Location = new Point(0x34, 0x18);
            this.rdBtn_StartGRBiasCal.Name = "rdBtn_StartGRBiasCal";
            this.rdBtn_StartGRBiasCal.Size = new Size(0x8f, 0x11);
            this.rdBtn_StartGRBiasCal.TabIndex = 0;
            this.rdBtn_StartGRBiasCal.TabStop = true;
            this.rdBtn_StartGRBiasCal.Text = "Start gyro bias calibration";
            this.rdBtn_StartGRBiasCal.UseVisualStyleBackColor = true;
            this.rdBtnStartGRSclfacCal.AutoSize = true;
            this.rdBtnStartGRSclfacCal.Location = new Point(0x34, 0x3a);
            this.rdBtnStartGRSclfacCal.Name = "rdBtnStartGRSclfacCal";
            this.rdBtnStartGRSclfacCal.Size = new Size(0xb3, 0x11);
            this.rdBtnStartGRSclfacCal.TabIndex = 0;
            this.rdBtnStartGRSclfacCal.TabStop = true;
            this.rdBtnStartGRSclfacCal.Text = "Start gyro scale factor calibration";
            this.rdBtnStartGRSclfacCal.UseVisualStyleBackColor = true;
            this.btn_Send.Location = new Point(0x12b, 0x18);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new Size(0x4b, 0x1b);
            this.btn_Send.TabIndex = 1;
            this.btn_Send.Text = "Send";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new EventHandler(this.btn_Send_Click);
            this.btn_Cancel.Location = new Point(0x12b, 0x39);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new Size(0x4b, 0x1c);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x18c, 0x66);
            base.Controls.Add(this.btn_Cancel);
            base.Controls.Add(this.btn_Send);
            base.Controls.Add(this.rdBtnStartGRSclfacCal);
            base.Controls.Add(this.rdBtn_StartGRBiasCal);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmSetDRGyroFacCal";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "DR Gyro Calibration";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

