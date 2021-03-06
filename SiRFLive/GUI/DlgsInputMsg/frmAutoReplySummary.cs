﻿namespace SiRFLive.GUI.DlgsInputMsg
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.MessageHandling;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class frmAutoReplySummary : Form
    {
        private Button button_ok;
        private CommunicationManager comm;
        private IContainer components;
        private Panel panel1;
        private RichTextBox summaryTextBox;

        public frmAutoReplySummary()
        {
            this.InitializeComponent();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
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

        private void frmAutoReplySummary_Load(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            if (clsGlobal.PerformOnAll)
            {
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                    if ((manager != null) && (manager.comm != null))
                    {
                        builder.Append(utils_AutoReply.getAutoReplySummary(manager.comm));
                    }
                }
                clsGlobal.PerformOnAll = false;
            }
            else
            {
                builder.Append(utils_AutoReply.getAutoReplySummary(this.comm));
            }
            this.summaryTextBox.Text = builder.ToString();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmAutoReplySummary));
            this.panel1 = new Panel();
            this.summaryTextBox = new RichTextBox();
            this.button_ok = new Button();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.panel1.Controls.Add(this.summaryTextBox);
            this.panel1.Location = new Point(0x1a, 0x1d);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x1ff, 520);
            this.panel1.TabIndex = 2;
            this.summaryTextBox.Dock = DockStyle.Fill;
            this.summaryTextBox.Location = new Point(0, 0);
            this.summaryTextBox.Name = "summaryTextBox";
            this.summaryTextBox.ReadOnly = true;
            this.summaryTextBox.Size = new Size(0x1ff, 520);
            this.summaryTextBox.TabIndex = 0;
            this.summaryTextBox.Text = "";
            this.button_ok.Anchor = AnchorStyles.Bottom;
            this.button_ok.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_ok.Location = new Point(0xf2, 0x23b);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new Size(0x4d, 0x18);
            this.button_ok.TabIndex = 3;
            this.button_ok.Text = "&OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new EventHandler(this.button_OK_Click);
            base.AcceptButton = this.button_ok;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.button_ok;
            base.ClientSize = new Size(0x234, 0x25f);
            base.Controls.Add(this.button_ok);
            base.Controls.Add(this.panel1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Margin = new Padding(2);
            base.Name = "frmAutoReplySummary";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Auto Reply Configuration Information";
            base.Load += new EventHandler(this.frmAutoReplySummary_Load);
            this.panel1.ResumeLayout(false);
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

