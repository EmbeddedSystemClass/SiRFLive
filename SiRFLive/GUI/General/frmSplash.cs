﻿namespace SiRFLive.GUI.General
{
    using SiRFLive.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmSplash : Form
    {
        private IContainer components;
        private Timer timer1;

        public frmSplash()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmSplash));
            this.timer1 = new Timer(this.components);
            base.SuspendLayout();
            this.timer1.Enabled = true;
            this.timer1.Interval = 0x7d0;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.BackgroundImage = Resources.Splash_CSR_SiRF;
            this.BackgroundImageLayout = ImageLayout.Center;
            base.ClientSize = new Size(0x14b, 0xc1);
            base.ControlBox = false;
            this.Cursor = Cursors.WaitCursor;
            this.DoubleBuffered = true;
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //!base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmSplash";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "SiRFLive Splash";
            base.Load += new EventHandler(this.frmSplash_Load);
            base.ResumeLayout(false);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            base.Close();
        }
    }
}

