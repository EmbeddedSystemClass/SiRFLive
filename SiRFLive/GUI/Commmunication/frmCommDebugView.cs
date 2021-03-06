﻿namespace SiRFLive.GUI.Commmunication
{
    using CommonClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmCommDebugView : Form
    {
        private string _matchStr = string.Empty;
        private CommunicationManager comm;
        private IContainer components;
        private ToolStripButton debubViewToolStripPauseBtn;
        private TextBox debugViewRegExpressionTxtBox;
        private ToolStrip debugViewToolStrip;
        private ToolStripButton debugViewToolStripFilterBtn;
        private bool isMatchOn;
        private CommonClass.MyRichTextBox rtbDisplay;
        private ToolTip toolTip1;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmCommDebugView(CommunicationManager targetComm)
        {
            this.InitializeComponent();
            this.comm = targetComm;
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
        }

        private void debubViewToolStripPauseBtn_Click(object sender, EventArgs e)
        {
            this.comm.DebugViewRTBDisplay.viewPause = !this.comm.DebugViewRTBDisplay.viewPause;
            this.frmCommDebugViewPauseBtnImage();
        }

        private void debugViewRegExpressionTxtBox_MouseEnter(object sender, EventArgs e)
        {
            string text = "Click the D Filter button above then enter text to match and hit the enter key to activate";
            this.toolTip1.Show(text, this.debugViewRegExpressionTxtBox, 0x7530);
        }

        private void debugViewRegExpressionTxtBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.comm.DebugViewMatchString = this.debugViewRegExpressionTxtBox.Text;
            }
        }

        private void debugViewToolStripFilterBtn_Click(object sender, EventArgs e)
        {
            this.comm.DebugViewIsMatchEnable = !this.comm.DebugViewIsMatchEnable;
            if (this.comm.DebugViewIsMatchEnable)
            {
                this.debugViewToolStripFilterBtn.CheckState = CheckState.Checked;
            }
            else
            {
                this.debugViewToolStripFilterBtn.CheckState = CheckState.Unchecked;
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

        private void frmCommDebugView_Load(object sender, EventArgs e)
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
            this.frmCommDebugViewPauseBtnImage();
            if (!clsGlobal.IsMarketingUser())
            {
                this.comm.DebugViewRTBDisplay.MAX_LINE = 0xc350;
            }
        }

        private void frmCommDebugView_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmCommDebugView_ResizeEnd(object sender, EventArgs e)
        {
            this.WinWidth = base.Width;
            this.WinHeight = base.Height;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        public void frmCommDebugViewPauseBtnImage()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        this.localUpdateDebugViewBtnImage();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                this.localUpdateDebugViewBtnImage();
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmCommDebugView));
            this.rtbDisplay = new CommonClass.MyRichTextBox();
            this.debugViewToolStrip = new ToolStrip();
            this.debubViewToolStripPauseBtn = new ToolStripButton();
            this.debugViewToolStripFilterBtn = new ToolStripButton();
            this.debugViewRegExpressionTxtBox = new TextBox();
            this.toolTip1 = new ToolTip(this.components);
            this.debugViewToolStrip.SuspendLayout();
            base.SuspendLayout();
            this.rtbDisplay.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.rtbDisplay.BackColor = SystemColors.Window;
            this.rtbDisplay.Location = new Point(0, 0x34);
            this.rtbDisplay.Name = "rtbDisplay";
            this.rtbDisplay.ReadOnly = true;
            this.rtbDisplay.Size = new Size(530, 0xea);
            this.rtbDisplay.TabIndex = 1;
            this.rtbDisplay.Text = "";
            this.rtbDisplay.DoubleClick += new EventHandler(this.rtbDisplay_DoubleClick);
            this.debugViewToolStrip.Items.AddRange(new ToolStripItem[] { this.debubViewToolStripPauseBtn, this.debugViewToolStripFilterBtn });
            this.debugViewToolStrip.Location = new Point(0, 0);
            this.debugViewToolStrip.Name = "debugViewToolStrip";
            this.debugViewToolStrip.Size = new Size(530, 0x19);
            this.debugViewToolStrip.TabIndex = 2;
            this.debugViewToolStrip.Text = "Debug View Toolbar";
            this.debubViewToolStripPauseBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.debubViewToolStripPauseBtn.Image = Resources.Pause;
            this.debubViewToolStripPauseBtn.ImageTransparentColor = Color.Magenta;
            this.debubViewToolStripPauseBtn.Name = "debubViewToolStripPauseBtn";
            this.debubViewToolStripPauseBtn.Size = new Size(0x17, 0x16);
            this.debubViewToolStripPauseBtn.Text = "Pause";
            this.debubViewToolStripPauseBtn.Click += new EventHandler(this.debubViewToolStripPauseBtn_Click);
            this.debugViewToolStripFilterBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.debugViewToolStripFilterBtn.Image = Resources.Filter2HS;
            this.debugViewToolStripFilterBtn.ImageTransparentColor = Color.Magenta;
            this.debugViewToolStripFilterBtn.Name = "debugViewToolStripFilterBtn";
            this.debugViewToolStripFilterBtn.Size = new Size(0x17, 0x16);
            this.debugViewToolStripFilterBtn.Text = "Filter";
            this.debugViewToolStripFilterBtn.Click += new EventHandler(this.debugViewToolStripFilterBtn_Click);
            this.debugViewRegExpressionTxtBox.AllowDrop = true;
            this.debugViewRegExpressionTxtBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.debugViewRegExpressionTxtBox.Location = new Point(0, 0x1d);
            this.debugViewRegExpressionTxtBox.Name = "debugViewRegExpressionTxtBox";
            this.debugViewRegExpressionTxtBox.Size = new Size(530, 20);
            this.debugViewRegExpressionTxtBox.TabIndex = 3;
            this.debugViewRegExpressionTxtBox.Text = "Regular Expression";
            this.debugViewRegExpressionTxtBox.PreviewKeyDown += new PreviewKeyDownEventHandler(this.debugViewRegExpressionTxtBox_PreviewKeyDown);
            this.debugViewRegExpressionTxtBox.MouseEnter += new EventHandler(this.debugViewRegExpressionTxtBox_MouseEnter);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(530, 0x120);
            base.Controls.Add(this.debugViewRegExpressionTxtBox);
            base.Controls.Add(this.debugViewToolStrip);
            base.Controls.Add(this.rtbDisplay);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmCommDebugView";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Debug View";
            base.Load += new EventHandler(this.frmCommDebugView_Load);
            base.LocationChanged += new EventHandler(this.frmCommDebugView_LocationChanged);
            base.ResizeEnd += new EventHandler(this.frmCommDebugView_ResizeEnd);
            this.debugViewToolStrip.ResumeLayout(false);
            this.debugViewToolStrip.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void localUpdateDebugViewBtnImage()
        {
            if ((this.comm != null) && (this.comm.DebugViewRTBDisplay != null))
            {
                if (!this.comm.DebugViewRTBDisplay.viewPause)
                {
                    this.debubViewToolStripPauseBtn.Image = Resources.Pause;
                    this.debubViewToolStripPauseBtn.Text = "Pause";
                }
                else
                {
                    this.debubViewToolStripPauseBtn.Image = Resources.unpause;
                    this.debubViewToolStripPauseBtn.Text = "Continue";
                }
                if (this.comm.DebugViewIsMatchEnable)
                {
                    this.debugViewToolStripFilterBtn.CheckState = CheckState.Checked;
                }
                else
                {
                    this.debugViewToolStripFilterBtn.CheckState = CheckState.Unchecked;
                }
                this.debugViewRegExpressionTxtBox.Text = this.comm.DebugViewMatchString;
            }
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
                this.comm.DebugViewRTBDisplay.DisplayWindow = this.rtbDisplay;
                this.Text = this.comm.sourceDeviceName + ": Debug View";
            }
        }

        public delegate void updateParentEventHandler(string titleString);

        public delegate void UpdateWindowEventHandler(string titleString, int left, int top, int width, int height, bool state);
    }
}

