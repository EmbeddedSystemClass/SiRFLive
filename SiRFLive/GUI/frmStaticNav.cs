﻿namespace SiRFLive.GUI
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

    public class frmStaticNav : Form
    {
        private Button button_Cancel;
        private Button button_Send;
        private CommunicationManager comm;
        private IContainer components;
        private RadioButton StatNavDisable_rBtn;
        private RadioButton StatNavEnable_rBtn;

        public frmStaticNav()
        {
            this.InitializeComponent();
            this.StatNavDisable_rBtn.Checked = true;
        }

        public frmStaticNav(CommunicationManager parentComm)
        {
            this.InitializeComponent();
            this.comm = parentComm;
            this.StatNavDisable_rBtn.Checked = true;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            int flag = 0;
            if (this.StatNavEnable_rBtn.Checked)
            {
                flag = 1;
            }
            this.SetStaticNavControl(flag);
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmStaticNav));
            this.StatNavDisable_rBtn = new RadioButton();
            this.StatNavEnable_rBtn = new RadioButton();
            this.button_Send = new Button();
            this.button_Cancel = new Button();
            base.SuspendLayout();
            this.StatNavDisable_rBtn.AutoSize = true;
            this.StatNavDisable_rBtn.Location = new Point(0x1d, 0x12);
            this.StatNavDisable_rBtn.Name = "StatNavDisable_rBtn";
            this.StatNavDisable_rBtn.Size = new Size(60, 0x11);
            this.StatNavDisable_rBtn.TabIndex = 0;
            this.StatNavDisable_rBtn.TabStop = true;
            this.StatNavDisable_rBtn.Text = "Disable";
            this.StatNavDisable_rBtn.UseVisualStyleBackColor = true;
            this.StatNavEnable_rBtn.AutoSize = true;
            this.StatNavEnable_rBtn.Location = new Point(0x1d, 0x2d);
            this.StatNavEnable_rBtn.Name = "StatNavEnable_rBtn";
            this.StatNavEnable_rBtn.Size = new Size(0x3a, 0x11);
            this.StatNavEnable_rBtn.TabIndex = 1;
            this.StatNavEnable_rBtn.TabStop = true;
            this.StatNavEnable_rBtn.Text = "Enable";
            this.StatNavEnable_rBtn.UseVisualStyleBackColor = true;
            this.button_Send.Location = new Point(0x84, 12);
            this.button_Send.Name = "button_Send";
            this.button_Send.Size = new Size(0x4b, 0x17);
            this.button_Send.TabIndex = 2;
            this.button_Send.Text = "&Send";
            this.button_Send.UseVisualStyleBackColor = true;
            this.button_Send.Click += new EventHandler(this.button_Send_Click);
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new Point(0x84, 0x29);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new Size(0x4b, 0x17);
            this.button_Cancel.TabIndex = 3;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new EventHandler(this.button_Cancel_Click);
            base.AcceptButton = this.button_Send;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.button_Cancel;
            base.ClientSize = new Size(0xd9, 0x56);
            base.Controls.Add(this.button_Cancel);
            base.Controls.Add(this.button_Send);
            base.Controls.Add(this.StatNavEnable_rBtn);
            base.Controls.Add(this.StatNavDisable_rBtn);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmStaticNav";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Static Navigation";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public virtual void SetStaticNavControl(int flag)
        {
            ArrayList list = new ArrayList();
            StringBuilder builder = new StringBuilder();
            list = this.comm.m_Protocols.GetInputMessageStructure(0x8f, clsGlobal.noSID, "Static Navigation", "SSB");
            for (int i = 0; i < list.Count; i++)
            {
                if (((InputMsg) list[i]).fieldName == "Static Navigation Flag")
                {
                    builder.Append(flag);
                }
                else
                {
                    builder.Append(((InputMsg) list[i]).defaultValue);
                }
                builder.Append(",");
            }
            string msg = this.comm.m_Protocols.ConvertFieldsToRaw(builder.ToString().TrimEnd(new char[] { ',' }), "Static Navigation", "OSP");
            this.comm.WriteData(msg);
        }
    }
}

