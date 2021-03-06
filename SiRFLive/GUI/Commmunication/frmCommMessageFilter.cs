﻿namespace SiRFLive.GUI.Commmunication
{
    using CommonClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;

    public class frmCommMessageFilter : Form
    {
        private int _count;
        private CommonClass.MyRichTextBox _displayWindow;
        private HelperFunctions _helperFunctions = new HelperFunctions();
        private bool _isMatchRegularExpression;
        private int _lineCount;
        private int _MAX_LINE = 0x1388;
        private List<string> _messageFilterQNames = new List<string>();
        private static int _numberOpen;
        private string _persistedWindowName = "Input Command Window";
        private bool _viewPause;
        private CommunicationManager.TransmissionType _viewType = CommunicationManager.TransmissionType.Hex;
        private ToolStripMenuItem actionToolStripMenuItem;
        private ToolStripMenuItem aSCIIToolStripMenuItem;
        private DataGridViewTextBoxColumn Column1;
        private CommunicationManager comm;
        private IContainer components;
        private ToolStripMenuItem cSVToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private Label frmCommMessageFilterOccurrence;
        private CheckBox frmMessageFilterMatchChkBox;
        private Label frmMessageFilterMsgIdLabel;
        private TextBox frmMessageFilterMsgIdTxtBox;
        private TextBox frmMessageFilterRegExpressionTxtBox;
        private ToolStripStatusLabel frmMessageFilterStatusLabel;
        private ToolStripMenuItem gP2ToolStripMenuItem;
        private ToolStripMenuItem hexToolStripMenuItem;
        private MenuStrip menuStrip1;
        private GroupBox MessageFilterGroupBox;
        private ToolStripMenuItem pauseToolStripMenuItem;
        private CommonClass.MyRichTextBox rtbDisplay;
        private ToolStripMenuItem sSBToolStripMenuItem;
        private ToolStripMenuItem startToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripMenuItem stopToolStripMenuItem;
        private ToolTip toolTip1;
        private ToolTip toolTip2;
        private ToolStripMenuItem viewModeToolStripMenuItem;
        private ToolStripMenuItem viewModeToolStripMenuItem1;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmCommMessageFilter()
        {
            this.InitializeComponent();
            this._displayWindow = this.rtbDisplay;
            this._count = 0;
            this.frmCommMessageFilterOccurrence.Text = this._count.ToString();
            _numberOpen++;
            this._persistedWindowName = "Message Filter Window " + _numberOpen.ToString();
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
        }

        private void aSCIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._viewType = CommunicationManager.TransmissionType.Text;
        }

        private void cSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._viewType = CommunicationManager.TransmissionType.GPS;
        }

        [STAThread]
        private void DisplayData(Color msgColor, string msg)
        {
            EventHandler method = null;
            try
            {
                if (this._displayWindow.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            if (!this._viewPause)
                            {
                                if (this._lineCount > this._MAX_LINE)
                                {
                                    string text = this._displayWindow.Text.Substring(this._displayWindow.Text.IndexOf("\n", (int) (this._displayWindow.Text.Length / 2)));
                                    this._displayWindow.Clear();
                                    this._displayWindow.AppendText(text);
                                    this._lineCount = 0;
                                }
                                this._displayWindow.SelectionColor = msgColor;
                                if (msgColor == Color.Blue)
                                {
                                    this._displayWindow.SelectionFont = new Font(this._displayWindow.SelectionFont, FontStyle.Bold);
                                }
                                else
                                {
                                    this._displayWindow.SelectionFont = new Font(this._displayWindow.SelectionFont, FontStyle.Regular);
                                }
                                this._displayWindow.AppendText(msg + "\r\n");
                                this._displayWindow.ScrollToBottom();
                            }
                        };
                    }
                    this._displayWindow.BeginInvoke(method);
                }
                else if (!this._viewPause)
                {
                    if (this._lineCount > this._MAX_LINE)
                    {
                        string str = this._displayWindow.Text.Substring(this._displayWindow.Text.IndexOf("\n", (int) (this._displayWindow.Text.Length / 2)));
                        this._displayWindow.Clear();
                        this._displayWindow.AppendText(str);
                        this._lineCount = 0;
                    }
                    this._displayWindow.SelectionColor = msgColor;
                    this._displayWindow.AppendText(msg + "\r\n");
                    this._displayWindow.ScrollToBottom();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error: " + exception.Message);
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                foreach (string str in this._messageFilterQNames)
                {
                    if (this.comm.ListenersCtrl != null)
                    {
                        this.comm.ListenersCtrl.Stop(str);
                        this.comm.ListenersCtrl.Delete(str);
                        this.comm.ListenersCtrl.DeleteListener(str);
                        this.comm.ListenersCtrl.DeleteListener(str);
                    }
                }
                this._messageFilterQNames.Clear();
            }
            base.Close();
        }

        private void frmCommMessageFilter_Load(object sender, EventArgs e)
        {
            if (this.WinTop != 0)
            {
                base.Top = this.WinTop;
            }
            if (this.WinLeft != 0)
            {
                base.Left = this.WinLeft;
            }
            if (this.WinWidth != 0)
            {
                base.Width = this.WinWidth;
            }
            if (this.WinHeight != 0)
            {
                base.Height = this.WinHeight;
            }
        }

        private void frmCommMessageFilter_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmCommMessageFilter_ResizeEnd(object sender, EventArgs e)
        {
            this.WinWidth = base.Width;
            this.WinHeight = base.Height;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmMessageFilterMatchChkBox_CheckedChanged(object sender, EventArgs e)
        {
            this._isMatchRegularExpression = this.frmMessageFilterMatchChkBox.Checked;
            this._count = 0;
        }

        private void frmMessageFilterMsgIdTxtBox_MouseEnter(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Enter filter in <Channel ID>,<Message ID>,<Message sub ID>\r\n");
            builder.Append("Use -1 for field to be ignored\r\n");
            builder.Append("For OSP and GSW RX -- Channel ID is -1\r\n");
            builder.Append("For example: To set filter for message ID 7 in OSP and GSW, enter\r\n");
            builder.Append("-1,7,-1\r\n");
            builder.Append("To filter multiple messages, separate them with %\r\n");
            builder.Append("Hit Enter key or with \"start\" in \"Message Filter Action\" menu to activate the listeners\r\n");
            this.toolTip2.Show(builder.ToString(), this.frmMessageFilterMsgIdTxtBox, 0x7530);
        }

        private void frmMessageFilterMsgIdTxtBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (((e.KeyValue == 13) && (this.comm != null)) && (this.frmMessageFilterMsgIdTxtBox.Text != string.Empty))
            {
                this.StartListeners();
            }
        }

        private void frmMessageFilterQueueHandler(object sender, DoWorkEventArgs myQContent)
        {
            EventHandler method = null;
            Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
            MessageQData argument = (MessageQData) myQContent.Argument;
            Color black = Color.Black;
            try
            {
                string messageText = argument.MessageText;
                if (this._viewPause)
                {
                    return;
                }
                switch (messageText)
                {
                    case "":
                    case "START":
                        goto Label_0113;

                    default:
                        switch (this._viewType)
                        {
                            case CommunicationManager.TransmissionType.GP2:
                                messageText = argument.MessageTime + "\t" + messageText;
                                goto Label_00A6;

                            case CommunicationManager.TransmissionType.GPS:
                                messageText = this.comm.m_Protocols.ConvertRawToFields(HelperFunctions.HexToByte(messageText));
                                goto Label_00A6;
                        }
                        break;
                }
            Label_00A6:
                if (this._isMatchRegularExpression && (this.frmMessageFilterRegExpressionTxtBox.Text.Length != 0))
                {
                    Regex regex = new Regex(this.frmMessageFilterRegExpressionTxtBox.Text, RegexOptions.Compiled);
                    if (regex.IsMatch(messageText))
                    {
                        black = Color.Blue;
                        this._count++;
                        if (method == null)
                        {
                            method = delegate {
                                this.frmCommMessageFilterOccurrence.Text = this._count.ToString();
                            };
                        }
                        this.frmCommMessageFilterOccurrence.Invoke(method);
                    }
                }
            Label_0113:
                this.DisplayData(black, messageText);
            }
            catch
            {
            }
        }

        private void frmMessageFilterRegExpressionTxtBox_MouseEnter(object sender, EventArgs e)
        {
            string text = "Check the \"Match Regular Expression\" box. Enter text to match and hit enter key to activate matching";
            this.toolTip1.Show(text, this.frmMessageFilterRegExpressionTxtBox, 0x7530);
        }

        private void frmMessageFilterRegExpressionTxtBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (((e.KeyValue == 13) && (this.comm != null)) && (this.frmMessageFilterRegExpressionTxtBox.Text != string.Empty))
            {
                this.frmMessageFilterMatchChkBox.Checked = true;
            }
        }

        private void gP2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._viewType = CommunicationManager.TransmissionType.GP2;
        }

        private void hexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._viewType = CommunicationManager.TransmissionType.Hex;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmCommMessageFilter));
            this.MessageFilterGroupBox = new GroupBox();
            this.rtbDisplay = new CommonClass.MyRichTextBox();
            this.Column1 = new DataGridViewTextBoxColumn();
            this.frmMessageFilterMsgIdTxtBox = new TextBox();
            this.frmMessageFilterMsgIdLabel = new Label();
            this.frmMessageFilterMatchChkBox = new CheckBox();
            this.frmMessageFilterRegExpressionTxtBox = new TextBox();
            this.frmCommMessageFilterOccurrence = new Label();
            this.menuStrip1 = new MenuStrip();
            this.actionToolStripMenuItem = new ToolStripMenuItem();
            this.startToolStripMenuItem = new ToolStripMenuItem();
            this.pauseToolStripMenuItem = new ToolStripMenuItem();
            this.stopToolStripMenuItem = new ToolStripMenuItem();
            this.exitToolStripMenuItem = new ToolStripMenuItem();
            this.viewModeToolStripMenuItem = new ToolStripMenuItem();
            this.viewModeToolStripMenuItem1 = new ToolStripMenuItem();
            this.hexToolStripMenuItem = new ToolStripMenuItem();
            this.aSCIIToolStripMenuItem = new ToolStripMenuItem();
            this.sSBToolStripMenuItem = new ToolStripMenuItem();
            this.gP2ToolStripMenuItem = new ToolStripMenuItem();
            this.cSVToolStripMenuItem = new ToolStripMenuItem();
            this.statusStrip1 = new StatusStrip();
            this.frmMessageFilterStatusLabel = new ToolStripStatusLabel();
            this.toolTip1 = new ToolTip(this.components);
            this.toolTip2 = new ToolTip(this.components);
            this.MessageFilterGroupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            base.SuspendLayout();
            this.MessageFilterGroupBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.MessageFilterGroupBox.Controls.Add(this.rtbDisplay);
            this.MessageFilterGroupBox.Location = new Point(0x16, 0x8e);
            this.MessageFilterGroupBox.Name = "MessageFilterGroupBox";
            this.MessageFilterGroupBox.Size = new Size(0x2e6, 0x160);
            this.MessageFilterGroupBox.TabIndex = 6;
            this.MessageFilterGroupBox.TabStop = false;
            this.MessageFilterGroupBox.Text = "Message Filter";
            this.rtbDisplay.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.rtbDisplay.BackColor = SystemColors.ControlLightLight;
            this.rtbDisplay.Location = new Point(6, 0x13);
            this.rtbDisplay.Name = "rtbDisplay";
            this.rtbDisplay.ReadOnly = true;
            this.rtbDisplay.Size = new Size(730, 320);
            this.rtbDisplay.TabIndex = 5;
            this.rtbDisplay.Text = "";
            this.rtbDisplay.DoubleClick += new EventHandler(this.rtbDisplay_DoubleClick);
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 0x1388;
            this.frmMessageFilterMsgIdTxtBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.frmMessageFilterMsgIdTxtBox.Location = new Point(0x1c, 0x31);
            this.frmMessageFilterMsgIdTxtBox.Name = "frmMessageFilterMsgIdTxtBox";
            this.frmMessageFilterMsgIdTxtBox.Size = new Size(730, 20);
            this.frmMessageFilterMsgIdTxtBox.TabIndex = 1;
            this.frmMessageFilterMsgIdTxtBox.PreviewKeyDown += new PreviewKeyDownEventHandler(this.frmMessageFilterMsgIdTxtBox_PreviewKeyDown);
            this.frmMessageFilterMsgIdTxtBox.MouseEnter += new EventHandler(this.frmMessageFilterMsgIdTxtBox_MouseEnter);
            this.frmMessageFilterMsgIdLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.frmMessageFilterMsgIdLabel.AutoSize = true;
            this.frmMessageFilterMsgIdLabel.Location = new Point(0x19, 0x20);
            this.frmMessageFilterMsgIdLabel.Name = "frmMessageFilterMsgIdLabel";
            this.frmMessageFilterMsgIdLabel.Size = new Size(0x194, 13);
            this.frmMessageFilterMsgIdLabel.TabIndex = 0;
            this.frmMessageFilterMsgIdLabel.Text = "Message(s) to filter (CHAN ID, MSG ID, SUB ID % CHAN ID, MSG ID, SUB ID % ... )";
            this.frmMessageFilterMatchChkBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.frmMessageFilterMatchChkBox.AutoSize = true;
            this.frmMessageFilterMatchChkBox.Location = new Point(0x1c, 80);
            this.frmMessageFilterMatchChkBox.Name = "frmMessageFilterMatchChkBox";
            this.frmMessageFilterMatchChkBox.Size = new Size(0x9c, 0x11);
            this.frmMessageFilterMatchChkBox.TabIndex = 2;
            this.frmMessageFilterMatchChkBox.Text = "Match Regular Expression?";
            this.frmMessageFilterMatchChkBox.UseVisualStyleBackColor = true;
            this.frmMessageFilterMatchChkBox.CheckedChanged += new EventHandler(this.frmMessageFilterMatchChkBox_CheckedChanged);
            this.frmMessageFilterRegExpressionTxtBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.frmMessageFilterRegExpressionTxtBox.Location = new Point(0x1c, 0x67);
            this.frmMessageFilterRegExpressionTxtBox.Name = "frmMessageFilterRegExpressionTxtBox";
            this.frmMessageFilterRegExpressionTxtBox.Size = new Size(730, 20);
            this.frmMessageFilterRegExpressionTxtBox.TabIndex = 4;
            this.frmMessageFilterRegExpressionTxtBox.PreviewKeyDown += new PreviewKeyDownEventHandler(this.frmMessageFilterRegExpressionTxtBox_PreviewKeyDown);
            this.frmMessageFilterRegExpressionTxtBox.MouseEnter += new EventHandler(this.frmMessageFilterRegExpressionTxtBox_MouseEnter);
            this.frmCommMessageFilterOccurrence.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.frmCommMessageFilterOccurrence.AutoSize = true;
            this.frmCommMessageFilterOccurrence.Location = new Point(0xb8, 0x52);
            this.frmCommMessageFilterOccurrence.Name = "frmCommMessageFilterOccurrence";
            this.frmCommMessageFilterOccurrence.Size = new Size(0x2b, 13);
            this.frmCommMessageFilterOccurrence.TabIndex = 3;
            this.frmCommMessageFilterOccurrence.Text = "Match: ";
            this.menuStrip1.AllowMerge = false;
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.actionToolStripMenuItem, this.viewModeToolStripMenuItem });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(0x313, 0x18);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.actionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.startToolStripMenuItem, this.pauseToolStripMenuItem, this.stopToolStripMenuItem, this.exitToolStripMenuItem });
            this.actionToolStripMenuItem.Name = "actionToolStripMenuItem";
            this.actionToolStripMenuItem.Size = new Size(0x79, 20);
            this.actionToolStripMenuItem.Text = "Message Filter &Action";
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new Size(0x72, 0x16);
            this.startToolStripMenuItem.Text = "&Start";
            this.startToolStripMenuItem.Click += new EventHandler(this.startToolStripMenuItem_Click);
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.Size = new Size(0x72, 0x16);
            this.pauseToolStripMenuItem.Text = "&Pause";
            this.pauseToolStripMenuItem.Click += new EventHandler(this.pauseToolStripMenuItem_Click);
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new Size(0x72, 0x16);
            this.stopToolStripMenuItem.Text = "Sto&p";
            this.stopToolStripMenuItem.Click += new EventHandler(this.stopToolStripMenuItem_Click);
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new Size(0x72, 0x16);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new EventHandler(this.exitToolStripMenuItem_Click);
            this.viewModeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.viewModeToolStripMenuItem1 });
            this.viewModeToolStripMenuItem.Name = "viewModeToolStripMenuItem";
            this.viewModeToolStripMenuItem.Size = new Size(0x71, 20);
            this.viewModeToolStripMenuItem.Text = "Message Filter &View";
            this.viewModeToolStripMenuItem.Click += new EventHandler(this.viewModeToolStripMenuItem_Click);
            this.viewModeToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { this.hexToolStripMenuItem, this.aSCIIToolStripMenuItem, this.sSBToolStripMenuItem, this.gP2ToolStripMenuItem, this.cSVToolStripMenuItem });
            this.viewModeToolStripMenuItem1.Name = "viewModeToolStripMenuItem1";
            this.viewModeToolStripMenuItem1.Size = new Size(0x98, 0x16);
            this.viewModeToolStripMenuItem1.Text = "View &Mode";
            this.viewModeToolStripMenuItem1.DropDownOpened += new EventHandler(this.viewModeToolStripMenuItem1_DropDownOpened);
            this.hexToolStripMenuItem.Name = "hexToolStripMenuItem";
            this.hexToolStripMenuItem.Size = new Size(0x98, 0x16);
            this.hexToolStripMenuItem.Text = "&Hex";
            this.hexToolStripMenuItem.Click += new EventHandler(this.hexToolStripMenuItem_Click);
            this.aSCIIToolStripMenuItem.Name = "aSCIIToolStripMenuItem";
            this.aSCIIToolStripMenuItem.Size = new Size(0x98, 0x16);
            this.aSCIIToolStripMenuItem.Text = "&ASCII";
            this.aSCIIToolStripMenuItem.Click += new EventHandler(this.aSCIIToolStripMenuItem_Click);
            this.sSBToolStripMenuItem.Name = "sSBToolStripMenuItem";
            this.sSBToolStripMenuItem.Size = new Size(0x98, 0x16);
            this.sSBToolStripMenuItem.Text = "&SSB";
            this.sSBToolStripMenuItem.Click += new EventHandler(this.sSBToolStripMenuItem_Click);
            this.gP2ToolStripMenuItem.Name = "gP2ToolStripMenuItem";
            this.gP2ToolStripMenuItem.Size = new Size(0x98, 0x16);
            this.gP2ToolStripMenuItem.Text = "GP&2";
            this.gP2ToolStripMenuItem.Click += new EventHandler(this.gP2ToolStripMenuItem_Click);
            this.cSVToolStripMenuItem.Name = "cSVToolStripMenuItem";
            this.cSVToolStripMenuItem.Size = new Size(0x98, 0x16);
            this.cSVToolStripMenuItem.Text = "&GPS";
            this.cSVToolStripMenuItem.Click += new EventHandler(this.cSVToolStripMenuItem_Click);
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.frmMessageFilterStatusLabel });
            this.statusStrip1.Location = new Point(0, 0x1f8);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(0x313, 0x16);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            this.frmMessageFilterStatusLabel.Name = "frmMessageFilterStatusLabel";
            this.frmMessageFilterStatusLabel.Size = new Size(110, 0x11);
            this.frmMessageFilterStatusLabel.Text = "Message Filter Status";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.ClientSize = new Size(0x313, 0x20e);
            base.Controls.Add(this.statusStrip1);
            base.Controls.Add(this.frmCommMessageFilterOccurrence);
            base.Controls.Add(this.frmMessageFilterRegExpressionTxtBox);
            base.Controls.Add(this.frmMessageFilterMatchChkBox);
            base.Controls.Add(this.frmMessageFilterMsgIdLabel);
            base.Controls.Add(this.frmMessageFilterMsgIdTxtBox);
            base.Controls.Add(this.MessageFilterGroupBox);
            base.Controls.Add(this.menuStrip1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new Size(300, 200);
            base.Name = "frmCommMessageFilter";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Message(s) Filter";
            base.Load += new EventHandler(this.frmCommMessageFilter_Load);
            base.LocationChanged += new EventHandler(this.frmCommMessageFilter_LocationChanged);
            base.ResizeEnd += new EventHandler(this.frmCommMessageFilter_ResizeEnd);
            this.MessageFilterGroupBox.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.comm != null)
            {
                this.StopListeners();
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

        protected override void OnResize(EventArgs e)
        {
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._viewPause = !this._viewPause;
            if (this._viewPause)
            {
                if (this.comm != null)
                {
                    this.frmMessageFilterStatusLabel.Text = "pause ...";
                    this.actionToolStripMenuItem.DropDownItems[1].Text = "Resume";
                }
            }
            else if (this.comm != null)
            {
                this.frmMessageFilterStatusLabel.Text = "running ...";
                this.actionToolStripMenuItem.DropDownItems[1].Text = "Pause";
            }
        }

        private void rtbDisplay_DoubleClick(object sender, EventArgs e)
        {
            this.rtbDisplay.Clear();
        }

        public void SetMessageFilter(string inStr)
        {
            this.frmMessageFilterMsgIdTxtBox.Text = inStr;
        }

        public void SetMessageFilterTextState(bool state)
        {
			this.frmMessageFilterMsgIdTxtBox.Invoke((MethodInvoker)delegate
			{
                this.frmMessageFilterMsgIdTxtBox.Enabled = state;
            });
        }

        public void SetStatus(bool status)
        {
            if (status)
            {
                this.frmMessageFilterStatusLabel.Text = "running ...";
                this.frmMessageFilterMsgIdTxtBox.Enabled = false;
            }
            else
            {
                this.frmMessageFilterStatusLabel.Text = "idle ...";
                this.frmMessageFilterMsgIdTxtBox.Enabled = true;
            }
        }

        private void sSBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._viewType = CommunicationManager.TransmissionType.SSB;
        }

        internal void StartListeners()
        {
            if (((this.comm != null) && this.comm.IsSourceDeviceOpen()) && (this.frmMessageFilterMsgIdTxtBox.Text.Length != 0))
            {
                string[] strArray = this.frmMessageFilterMsgIdTxtBox.Text.Replace(" ", "").Split(new char[] { '%' });
                List<string[]> list = new List<string[]>();
                if (strArray.Length == 0)
                {
                    MessageBox.Show("Not a valid format!", "ERROR!");
                }
                else
                {
                    foreach (string str2 in strArray)
                    {
                        string[] item = str2.Split(new char[] { ',' });
                        if (item.Length != 3)
                        {
                            MessageBox.Show("Not a valid format: " + str2, "ERROR!");
                            return;
                        }
                        int num = 0;
                        foreach (string str3 in item)
                        {
                            bool flag = false;
                            if (str3 != "-1")
                            {
                                uint num2;
                                if ((!uint.TryParse(str3, out num2) || (num2 < 0)) || (num2 > 0xff))
                                {
                                    flag = true;
                                }
                                else if ((num2 == 0) && (num != 2))
                                {
                                    flag = true;
                                }
                                if (flag)
                                {
                                    MessageBox.Show("Not a valid format: " + str3, "ERROR!");
                                    return;
                                }
                            }
                            num++;
                        }
                        list.Add(item);
                    }
                    string text = this.Text;
                    int num3 = 1;
                    foreach (string[] strArray3 in list)
                    {
                        string listenerName = text + num3.ToString();
                        ListenerContent content = this.comm.ListenersCtrl.Create(listenerName, 0, Convert.ToInt32(strArray3[0]), Convert.ToInt32(strArray3[1]), Convert.ToInt32(strArray3[2]), this.comm.PortName);
                        if (content != null)
                        {
                            content.DoUserWork.DoWork += new DoWorkEventHandler(this.frmMessageFilterQueueHandler);
                            num3++;
                            this._messageFilterQNames.Add(content.ListenerName);
                            this.comm.ListenersCtrl.Start(content.ListenerName, this.comm.PortName);
                        }
                    }
                    this.rtbDisplay.Text = "Message Filter started\r\n";
                    this.startToolStripMenuItem.Enabled = false;
                    this.stopToolStripMenuItem.Enabled = true;
                    this.frmMessageFilterMsgIdTxtBox.Enabled = false;
                    this.frmMessageFilterStatusLabel.Text = "running ...";
                }
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((this.comm != null) && this.comm.IsSourceDeviceOpen())
            {
                try
                {
                    this.StartListeners();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else
            {
                MessageBox.Show("Source Device not open", "Information");
            }
        }

        internal void StopListeners()
        {
            foreach (string str in this._messageFilterQNames)
            {
                if (this.comm.ListenersCtrl != null)
                {
                    this.comm.ListenersCtrl.Stop(str);
                    this.comm.ListenersCtrl.DeleteListener(str);
                }
            }
            this._messageFilterQNames.Clear();
			base.BeginInvoke((MethodInvoker)delegate
			{
                this.startToolStripMenuItem.Enabled = true;
                this.stopToolStripMenuItem.Enabled = false;
                this.frmMessageFilterMsgIdTxtBox.Enabled = true;
                this.frmMessageFilterStatusLabel.Text = "idle...";
            });
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (string str in this._messageFilterQNames)
            {
                if (this.comm.ListenersCtrl != null)
                {
                    this.comm.ListenersCtrl.Stop(str);
                    this.comm.ListenersCtrl.Delete(str);
                    this.comm.ListenersCtrl.DeleteListener(str);
                }
            }
            this._messageFilterQNames.Clear();
            this.actionToolStripMenuItem.DropDownItems[0].Enabled = true;
            this.actionToolStripMenuItem.DropDownItems[2].Enabled = false;
            this.frmMessageFilterMsgIdTxtBox.Enabled = true;
            this.frmMessageFilterStatusLabel.Text = "idle ...";
        }

        private void viewModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void viewModeToolStripMenuItem1_DropDownOpened(object sender, EventArgs e)
        {
            this.hexToolStripMenuItem.CheckState = CheckState.Unchecked;
            this.sSBToolStripMenuItem.CheckState = CheckState.Unchecked;
            this.aSCIIToolStripMenuItem.CheckState = CheckState.Unchecked;
            this.cSVToolStripMenuItem.CheckState = CheckState.Unchecked;
            this.gP2ToolStripMenuItem.CheckState = CheckState.Unchecked;
            switch (this._viewType)
            {
                case CommunicationManager.TransmissionType.Text:
                    this.aSCIIToolStripMenuItem.CheckState = CheckState.Checked;
                    return;

                case CommunicationManager.TransmissionType.Hex:
                case CommunicationManager.TransmissionType.Bin:
                    this.hexToolStripMenuItem.CheckState = CheckState.Checked;
                    return;

                case CommunicationManager.TransmissionType.SSB:
                    this.sSBToolStripMenuItem.CheckState = CheckState.Checked;
                    return;

                case CommunicationManager.TransmissionType.GP2:
                    this.gP2ToolStripMenuItem.CheckState = CheckState.Checked;
                    return;

                case CommunicationManager.TransmissionType.GPS:
                    this.cSVToolStripMenuItem.CheckState = CheckState.Checked;
                    return;
            }
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
                this.Text = this.comm.sourceDeviceName + ": Message Filter";
                this._viewType = this.comm.RxCurrentTransmissionType;
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

