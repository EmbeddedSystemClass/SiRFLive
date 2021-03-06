﻿namespace SiRFLive.GUI.Commmunication
{
    using CommonClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmCommResponseView : Form
    {
        private CommunicationManager comm;
        private IContainer components;
        private CommonClass.MyRichTextBox rtbDisplay;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmCommResponseView()
        {
            this.InitializeComponent();
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmCommResponseView_Load(object sender, EventArgs e)
        {
            base.Top = this.WinTop;
            base.Left = this.WinLeft;
            if (this.WinWidth != 0)
            {
                base.Width = this.WinWidth;
            }
            if (this.WinHeight != 0)
            {
                base.Height = this.WinHeight;
            }
        }

        private void frmCommResponseView_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmCommResponseView_Resize(object sender, EventArgs e)
        {
            this.WinWidth = base.Width;
            this.WinHeight = base.Height;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmCommResponseView));
            this.rtbDisplay = new CommonClass.MyRichTextBox();
            base.SuspendLayout();
            this.rtbDisplay.BackColor = SystemColors.Window;
            this.rtbDisplay.Dock = DockStyle.Fill;
            this.rtbDisplay.Location = new Point(0, 0);
            this.rtbDisplay.Name = "rtbDisplay";
            this.rtbDisplay.ReadOnly = true;
            this.rtbDisplay.Size = new Size(0x18b, 0x11e);
            this.rtbDisplay.TabIndex = 1;
            this.rtbDisplay.Text = "";
            this.rtbDisplay.DoubleClick += new EventHandler(this.rtbDisplay_DoubleClick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x18b, 0x11e);
            base.Controls.Add(this.rtbDisplay);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmCommResponseView";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Response View";
            base.Load += new EventHandler(this.frmCommResponseView_Load);
            base.Resize += new EventHandler(this.frmCommResponseView_Resize);
            base.LocationChanged += new EventHandler(this.frmCommResponseView_LocationChanged);
            base.ResumeLayout(false);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.updateMainWindow != null)
            {
                this.updateMainWindow(base.Name);
            }
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, false);
            }
            base.OnClosed(e);
        }

        private void rtbDisplay_DoubleClick(object sender, EventArgs e)
        {
            this.rtbDisplay.Text = string.Empty;
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
                this.comm.ResponseViewRTBDisplay.DisplayWindow = this.rtbDisplay;
                this.Text = this.comm.sourceDeviceName + ": Response View";
            }
        }

        public delegate void updateParentEventHandler(string titleString);

        public delegate void UpdateWindowEventHandler(string titleString, int left, int top, int width, int height, bool state);
    }
}

