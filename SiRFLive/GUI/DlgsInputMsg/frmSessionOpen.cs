﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using SiRFLive.Communication;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmSessionOpen : Form
    {
        private Button button_Cancel;
        private Button button_OK;
        private ComboBox comboBox_SessionOpenOrResume;
        private CommunicationManager comm;
        private IContainer components;

        public frmSessionOpen()
        {
            this.InitializeComponent();
            this.comboBox_SessionOpenOrResume.SelectedIndex = 0;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            byte sessionOpenReqInfo = 0x71;
            if (this.comboBox_SessionOpenOrResume.SelectedIndex == 1)
            {
                sessionOpenReqInfo = 0x80;
            }
            this.comm.RxCtrl.OpenSession(sessionOpenReqInfo);
            base.Close();
        }

        private void comboBox_SessionOpenOrResume_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmSessionOpen));
            this.comboBox_SessionOpenOrResume = new ComboBox();
            this.button_OK = new Button();
            this.button_Cancel = new Button();
            base.SuspendLayout();
            this.comboBox_SessionOpenOrResume.FormattingEnabled = true;
            this.comboBox_SessionOpenOrResume.Items.AddRange(new object[] { "Open Session", "Resume Session" });
            this.comboBox_SessionOpenOrResume.Location = new Point(0x53, 0x15);
            this.comboBox_SessionOpenOrResume.Name = "comboBox_SessionOpenOrResume";
            this.comboBox_SessionOpenOrResume.Size = new Size(0x79, 0x15);
            this.comboBox_SessionOpenOrResume.TabIndex = 1;
            this.comboBox_SessionOpenOrResume.SelectedIndexChanged += new EventHandler(this.comboBox_SessionOpenOrResume_SelectedIndexChanged);
            this.button_OK.Location = new Point(60, 0x3d);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new Size(0x4b, 0x17);
            this.button_OK.TabIndex = 4;
            this.button_OK.Text = "&OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new EventHandler(this.button_OK_Click);
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new Point(0x98, 0x3d);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new Size(0x4b, 0x17);
            this.button_Cancel.TabIndex = 3;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new EventHandler(this.button_Cancel_Click);
            base.AcceptButton = this.button_OK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.button_Cancel;
            base.ClientSize = new Size(0x11e, 0x68);
            base.Controls.Add(this.button_OK);
            base.Controls.Add(this.button_Cancel);
            base.Controls.Add(this.comboBox_SessionOpenOrResume);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmSessionOpen";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Session Open";
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

