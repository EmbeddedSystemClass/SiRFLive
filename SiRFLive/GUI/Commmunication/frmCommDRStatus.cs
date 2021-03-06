﻿namespace SiRFLive.GUI.Commmunication
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmCommDRStatus : Form
    {
        private static int _numberOpen;
        private string _persistedWindowName = "Input Command Window";
        private CommunicationManager comm;
        private IContainer components;
        private FileSystemWatcher fileSystemWatcher1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox7;
        private MyPanel myPanel1;
        private TextBox textBox_DRNavState;
        private TextBox textBox_DRNavStatus;
        private TextBox textBox_DRNavSubSysData;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmCommDRStatus()
        {
            this.InitializeComponent();
            _numberOpen++;
            this._persistedWindowName = "DR Status " + _numberOpen.ToString();
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

        private void frmCommDRStatus_Load(object sender, EventArgs e)
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

        private void frmCommDRStatus_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmCommDRStatus_Resize(object sender, EventArgs e)
        {
            this.Refresh();
            this.myPanel1.Refresh();
        }

        private void frmCommDRStatus_ResizeEnd(object sender, EventArgs e)
        {
            this.WinWidth = base.Width;
            this.WinHeight = base.Height;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmCommDRStatus));
            this.fileSystemWatcher1 = new FileSystemWatcher();
            this.myPanel1 = new MyPanel();
            this.groupBox7 = new GroupBox();
            this.textBox_DRNavStatus = new TextBox();
            this.groupBox2 = new GroupBox();
            this.textBox_DRNavSubSysData = new TextBox();
            this.groupBox1 = new GroupBox();
            this.textBox_DRNavState = new TextBox();
            this.fileSystemWatcher1.BeginInit();
            this.myPanel1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            this.myPanel1.Controls.Add(this.groupBox7);
            this.myPanel1.Controls.Add(this.groupBox2);
            this.myPanel1.Controls.Add(this.groupBox1);
            this.myPanel1.Dock = DockStyle.Fill;
            this.myPanel1.Location = new Point(0, 0);
            this.myPanel1.Name = "myPanel1";
            this.myPanel1.Size = new Size(0x2ef, 0x28c);
            this.myPanel1.TabIndex = 3;
            this.myPanel1.Paint += new PaintEventHandler(this.myPanel1_Paint);
            this.groupBox7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox7.BackColor = SystemColors.ControlLight;
            this.groupBox7.Controls.Add(this.textBox_DRNavStatus);
            this.groupBox7.Location = new Point(7, 9);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new Size(0x183, 640);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "DR NAV Status";
            this.groupBox7.Enter += new EventHandler(this.groupBox7_Enter);
            this.textBox_DRNavStatus.BackColor = SystemColors.ControlLight;
            this.textBox_DRNavStatus.BorderStyle = BorderStyle.None;
            this.textBox_DRNavStatus.Dock = DockStyle.Fill;
            this.textBox_DRNavStatus.Location = new Point(3, 0x10);
            this.textBox_DRNavStatus.Multiline = true;
            this.textBox_DRNavStatus.Name = "textBox_DRNavStatus";
            this.textBox_DRNavStatus.Size = new Size(0x17d, 0x26d);
            this.textBox_DRNavStatus.TabIndex = 9;
            this.textBox_DRNavStatus.Text = "status";
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.BackColor = SystemColors.ControlLight;
            this.groupBox2.Controls.Add(this.textBox_DRNavSubSysData);
            this.groupBox2.Location = new Point(400, 0x1cd);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x159, 0xbd);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "NAV Subsystem Data";
            this.groupBox2.Enter += new EventHandler(this.groupBox2_Enter);
            this.textBox_DRNavSubSysData.BackColor = SystemColors.ControlLight;
            this.textBox_DRNavSubSysData.BorderStyle = BorderStyle.None;
            this.textBox_DRNavSubSysData.Dock = DockStyle.Fill;
            this.textBox_DRNavSubSysData.Location = new Point(3, 0x10);
            this.textBox_DRNavSubSysData.Multiline = true;
            this.textBox_DRNavSubSysData.Name = "textBox_DRNavSubSysData";
            this.textBox_DRNavSubSysData.Size = new Size(0x153, 170);
            this.textBox_DRNavSubSysData.TabIndex = 9;
            this.textBox_DRNavSubSysData.Text = "Subsys data";
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.BackColor = SystemColors.ControlLight;
            this.groupBox1.Controls.Add(this.textBox_DRNavState);
            this.groupBox1.Location = new Point(400, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x159, 0x1bd);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DR NAV State";
            this.groupBox1.Enter += new EventHandler(this.groupBox1_Enter);
            this.textBox_DRNavState.BackColor = SystemColors.ControlLight;
            this.textBox_DRNavState.BorderStyle = BorderStyle.None;
            this.textBox_DRNavState.Dock = DockStyle.Fill;
            this.textBox_DRNavState.Location = new Point(3, 0x10);
            this.textBox_DRNavState.Multiline = true;
            this.textBox_DRNavState.Name = "textBox_DRNavState";
            this.textBox_DRNavState.Size = new Size(0x153, 0x1aa);
            this.textBox_DRNavState.TabIndex = 8;
            this.textBox_DRNavState.Text = "state";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x2ef, 0x28c);
            base.Controls.Add(this.myPanel1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmCommDRStatus";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SiRFDRive Status View";
            base.Load += new EventHandler(this.frmCommDRStatus_Load);
            base.Resize += new EventHandler(this.frmCommDRStatus_Resize);
            base.LocationChanged += new EventHandler(this.frmCommDRStatus_LocationChanged);
            base.ResizeEnd += new EventHandler(this.frmCommDRStatus_ResizeEnd);
            this.fileSystemWatcher1.EndInit();
            this.myPanel1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void myPanel1_Paint(object sender, PaintEventArgs e)
        {
            this.textBox_DRNavStatus.Text = this.comm.StringDRNavStatus;
            this.textBox_DRNavState.Text = this.comm.StringDRNavState;
            this.textBox_DRNavSubSysData.Text = this.comm.StringDRNavSubsystemData;
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.comm != null)
            {
                this.comm.EnableLocationMapView = false;
                if (this.updateMainWindow != null)
                {
                    this.updateMainWindow(base.Name);
                }
                if (this.UpdatePortManager != null)
                {
                    this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, false);
                }
            }
            base.OnClosed(e);
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
                this.comm.DisplayPanelDRStatusStates = this.myPanel1;
                this.Text = this.comm.sourceDeviceName + ": DR Status";
            }
        }

        public string PersistedWindowName
        {
            get
            {
                return this._persistedWindowName;
            }
            set
            {
                this._persistedWindowName = value;
            }
        }

        public delegate void updateParentEventHandler(string titleString);

        public delegate void UpdateWindowEventHandler(string titleString, int left, int top, int width, int height, bool state);
    }
}

