﻿namespace SiRFLive.GUI.Commmunication
{
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.MessageHandling;
    using SiRFLive.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmCommSignalView : Form
    {
        private static int _numberOpen = 0;
        private string _persistedWindowName = "Signal Strength Window";
        private int _SatType;
        private List<SignalData> _signalBufferList = new List<SignalData>();
        private List<SignalData> _signalBufferList_all = new List<SignalData>();
        private static float azimStartPoint = ((elevStartPoint + (fontSize * 4f)) + x_offset);
        private int boxHeight;
        private int boxWidth;
        private static int cno_MAX = 60;
        private static float cnoHorzStartPoint = ((stateHorzStartPoint + (fontSize * 4f)) + x_offset);
        private CommunicationManager comm;
        private IContainer components;
        private static float elevStartPoint = ((fontSize * 3f) + x_offset);
        private static Font fn = new Font(clsGlobal.SiRFLiveMainFont, 9f);
        private static float fontSize = fn.Size;
        private MyPanel myPanel;
        private MyPanel panel_signal;
        private static int signalRegHorzStartPoint = (((int) (cnoHorzStartPoint + (fontSize * 6f))) + x_offset);
        private int startY;
        private static float stateHorzStartPoint = ((azimStartPoint + (fontSize * 4f)) + x_offset);
        private int stopY;
        private ToolStrip toolStrip1;
        private ToolStripComboBox toolStripComboBox_Sattype;
        private ToolTip toolTip1;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;
        private static int x_offset = (((int) fontSize) / 2);
        private static int y_offset = 40;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmCommSignalView()
        {
            this.InitializeComponent();
            _numberOpen++;
            this._persistedWindowName = "Signal View Window " + _numberOpen.ToString();
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

        private void draw_1sec_data(Graphics g, SignalData sd, int secIndex)
        {
            int k = 1;
            for (int i = 0; i < 12; i++)
            {
                if (sd.CHAN_Arr_ID[i] != 0)
                {
                    if (Math.Abs(sd.CHAN_Arr_CNO[i]) <= 0.001)
                    {
                        this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.Red);
                    }
                    else if ((this.comm.dataGui.PRN_Arr_PRNforSolution[sd.CHAN_Arr_ID[i]] == 1) && (this.comm.dataGui._PMODE > 0))
                    {
                        if (this.comm.dataGui.PRN_Arr_UseCGEE[sd.CHAN_Arr_ID[i]] || this.comm.dataGui.PRN_Arr_UseSGEE[sd.CHAN_Arr_ID[i]])
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.Purple);
                        }
                        else if (this.comm.ABPModeIndicator)
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.DarkOrange);
                        }
                        else
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.DarkGreen);
                        }
                    }
                    else if (this.comm.dataGui.Positions.PositionList.Count > 0)
                    {
                        byte num3 = (byte) (((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).NavType & 0x80);
                        if ((sd.CHAN_Arr_ID[i] > 100) && (num3 == 0x80))
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, 12, Brushes.DeepSkyBlue);
                        }
                        else
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.Blue);
                        }
                    }
                    else
                    {
                        this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.Blue);
                    }
                    k++;
                }
            }
        }

        private void draw_1sec_data_all(Graphics g, SignalData sd, int secIndex)
        {
            int k = 1;
            for (int i = 0; i < 60; i++)
            {
                if (sd.CHAN_Arr_ID_All[i] != 0)
                {
                    if (Math.Abs(sd.CHAN_Arr_CNO_All[i]) <= 0.001)
                    {
                        this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.Red);
                    }
                    else if ((this.comm.dataGui_ALL.PRN_Arr_PRNforSolution[sd.CHAN_Arr_ID_All[i]] == 1) && ((this.comm.dataGui_ALL.PRN_Arr_Status[i] & 0x8000) != 0))
                    {
                        if (this.comm.dataGui_ALL.PRN_Arr_UseCGEE[sd.CHAN_Arr_ID_All[i]] || this.comm.dataGui_ALL.PRN_Arr_UseSGEE[sd.CHAN_Arr_ID_All[i]])
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.Purple);
                        }
                        else if (this.comm.ABPModeIndicator)
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.DarkOrange);
                        }
                        else
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.DarkGreen);
                        }
                    }
                    else if (this.comm.dataGui_ALL.Positions.PositionList.Count > 0)
                    {
                        byte num3 = (byte) (((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).NavType & 0x80);
                        if ((sd.CHAN_Arr_ID[i] > 100) && (num3 == 0x80))
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, 12, Brushes.DeepSkyBlue);
                        }
                        else
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.Blue);
                        }
                    }
                    else
                    {
                        this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.Blue);
                    }
                    k++;
                }
            }
        }

        private void draw_1sec_data_glonass(Graphics g, SignalData sd, int secIndex)
        {
            int k = 1;
            for (int i = 0; i < 60; i++)
            {
                if ((sd.CHAN_Arr_ID_All[i] != 0) && ((sd.CHAN_SV_Info[i] & 0xe000) == 0x40))
                {
                    if (Math.Abs(sd.CHAN_Arr_CNO_All[i]) <= 0.001)
                    {
                        this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.Red);
                    }
                    else if ((this.comm.dataGui_ALL.PRN_Arr_PRNforSolution[sd.CHAN_Arr_ID[i]] == 1) && ((this.comm.dataGui_ALL.PRN_Arr_Status[i] & 0x8000) != 0))
                    {
                        if (this.comm.dataGui_ALL.PRN_Arr_UseCGEE[sd.CHAN_Arr_ID_All[i]] || this.comm.dataGui_ALL.PRN_Arr_UseSGEE[sd.CHAN_Arr_ID_All[i]])
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.Purple);
                        }
                        else if (this.comm.ABPModeIndicator)
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.DarkOrange);
                        }
                        else
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.DarkGreen);
                        }
                    }
                    else if (this.comm.dataGui_ALL.Positions.PositionList.Count > 0)
                    {
                        byte num3 = (byte) (((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).NavType & 0x80);
                        if ((sd.CHAN_Arr_ID_All[i] > 100) && (num3 == 0x80))
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, 12, Brushes.DeepSkyBlue);
                        }
                        else
                        {
                            this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.Blue);
                        }
                    }
                    else
                    {
                        this.helper_Paint_Bar(g, sd, secIndex, i, k, Brushes.Blue);
                    }
                    k++;
                }
            }
        }

        private void draw_cno_boxes(Graphics g)
        {
            g.DrawRectangle(Pens.Black, signalRegHorzStartPoint, this.startY, this.boxWidth, this.boxHeight);
            for (int i = 1; i < this.comm.MAX_SIG_BUFFER; i++)
            {
                int x = signalRegHorzStartPoint + ((i * this.boxWidth) / 5);
                Point point = new Point(x, this.startY);
                Point point2 = new Point(x, this.boxHeight + this.startY);
                g.DrawLine(Pens.Black, point, point2);
            }
            for (int j = 1; j < 60; j++)
            {
                int y = this.startY + ((j * this.boxHeight) / 12);
                Point point3 = new Point(signalRegHorzStartPoint, y);
                Point point4 = new Point(signalRegHorzStartPoint + this.boxWidth, y);
                g.DrawLine(Pens.Black, point3, point4);
            }
        }

        private void frmCommSignalView_Load(object sender, EventArgs e)
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
            this.toolStripComboBox_Sattype.SelectedIndex = 0;
            if (clsGlobal.IsMarketingUser())
            {
                this.toolStrip1.Enabled = false;
            }
        }

        private void frmCommSignalView_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmCommSignalView_ResizeEnd(object sender, EventArgs e)
        {
            this.WinWidth = base.Width;
            this.WinHeight = base.Height;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void helper_Paint_Bar(Graphics g, SignalData sd, int secIndex, int i, int k, Brush brush)
        {
            int num = (this.startY + ((k * this.boxHeight) / 12)) - 12;
            g.DrawString(sd.CHAN_Arr_ID[i].ToString("00"), fn, brush, 0f, (float) num);
            g.DrawString(sd.CHAN_Arr_CNO[i].ToString("00.0"), fn, brush, cnoHorzStartPoint, (float) num);
            g.DrawString(sd.CHAN_Arr_State[i].ToString("X"), fn, brush, stateHorzStartPoint + 5f, (float) num);
            g.DrawString(sd.CHAN_Arr_Elev[i].ToString("00.0"), fn, brush, elevStartPoint, (float) num);
            g.DrawString(sd.CHAN_Arr_Azimuth[i].ToString("000.0"), fn, brush, azimStartPoint, (float) num);
            int num2 = (signalRegHorzStartPoint + ((secIndex * this.boxWidth) / 5)) + 5;
            int num3 = this.boxHeight / 12;
            int y = this.startY + ((k * this.boxHeight) / 12);
            int x = num2;
            for (int j = 0; j < 10; j++)
            {
                int num7 = sd.CHAN_MEAS_CNO[i][j];
                int num8 = y - ((num3 * num7) / cno_MAX);
                Point point = new Point(x, y);
                Point point2 = new Point(x, num8);
                Pen pen = new Pen(brush);
                g.DrawLine(pen, point, point2);
                x += this.boxWidth / 50;
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmCommSignalView));
            this.toolTip1 = new ToolTip(this.components);
            this.toolStrip1 = new ToolStrip();
            this.toolStripComboBox_Sattype = new ToolStripComboBox();
            this.panel_signal = new MyPanel();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripComboBox_Sattype });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x176, 0x19);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip2";
            this.toolStripComboBox_Sattype.Items.AddRange(new object[] { "GPS", "GLONASS", "ALL" });
            this.toolStripComboBox_Sattype.Name = "toolStripComboBox_Sattype";
            this.toolStripComboBox_Sattype.Size = new Size(90, 0x19);
            this.toolStripComboBox_Sattype.SelectedIndexChanged += new EventHandler(this.toolStripComboBox_Sattype_SelectedIndexChanged);
            this.panel_signal.AutoScroll = true;
            this.panel_signal.AutoSize = true;
            this.panel_signal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel_signal.Dock = DockStyle.Fill;
            this.panel_signal.Location = new Point(0, 0x19);
            this.panel_signal.Name = "panel_signal";
            this.panel_signal.Size = new Size(0x176, 0xe4);
            this.panel_signal.TabIndex = 0;
            this.panel_signal.Paint += new PaintEventHandler(this.panel_Signal_Paint);
            this.panel_signal.Resize += new EventHandler(this.panel_signal_Resize);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.ClientSize = new Size(0x176, 0xfd);
            base.Controls.Add(this.panel_signal);
            base.Controls.Add(this.toolStrip1);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmCommSignalView";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Signal View";
            base.Load += new EventHandler(this.frmCommSignalView_Load);
            base.LocationChanged += new EventHandler(this.frmCommSignalView_LocationChanged);
            base.ResizeEnd += new EventHandler(this.frmCommSignalView_ResizeEnd);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.comm != null)
            {
                this.comm.EnableSignalView = false;
            }
            if (this.updateMainWindow != null)
            {
                this.updateMainWindow(base.Name);
            }
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, false);
            }
            this._signalBufferList.Clear();
            this._signalBufferList_all.Clear();
            base.OnClosed(e);
        }

        private void panel_Signal_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                int height = this.myPanel.Height;
                int width = this.myPanel.Width;
                this.startY = (height / 10) + y_offset;
                this.stopY = height - this.startY;
                this.boxHeight = this.stopY - 10;
                this.boxWidth = (width - signalRegHorzStartPoint) - 12;
                string s = "Mode: No Fix";
                string str2 = "";
                if ((this.comm.dataGui.Positions.PositionList != null) && (this.comm.dataGui.Positions.PositionList.Count > 0))
                {
                    s = ((PositionInfo.PositionStruct) this.comm.dataGui.Positions.PositionList[this.comm.dataGui.Positions.PositionList.Count - 1]).NavModeString;
                    if (this.comm.ABPModeIndicator)
                    {
                        s = s + " + ABP";
                    }
                    if (this.comm.dataGui.AGC_Gain != 0)
                    {
                        str2 = string.Format("AGC Gain: {0}", this.comm.dataGui.AGC_Gain);
                    }
                }
                using (BufferedGraphics graphics = BufferedGraphicsManager.Current.Allocate(e.Graphics, e.ClipRectangle))
                {
                    Graphics g = graphics.Graphics;
                    g.FillRectangle(Brushes.White, 0, 0, width, height);
                    float num3 = HelperFunctions.FindAverageFloat(this.comm.dataGui.SignalDataForGUI.CHAN_Arr_CNO);
                    string str3 = string.Empty;
                    if (num3 > (cno_MAX - 20))
                    {
                        str3 = string.Format("{0}", "Power: High");
                    }
                    else if ((num3 <= (cno_MAX - 20)) && (num3 >= 30f))
                    {
                        str3 = string.Format("{0}", "Power: Nominal");
                    }
                    else if ((num3 < 30f) && (num3 >= 15f))
                    {
                        str3 = string.Format("{0}", "Power: Low");
                    }
                    else
                    {
                        str3 = string.Format("{0}", "Power: Very Low");
                    }
                    g.DrawString(str3, fn, Brushes.Black, (float) 0f, (float) 25f);
                    g.DrawString(string.Format("Avg CNo: {0:F1} dBHz", num3), fn, Brushes.Black, (float) signalRegHorzStartPoint, 25f);
                    Pen pen = new Pen(Brushes.Gray, 2f);
                    pen.StartCap = LineCap.AnchorMask;
                    pen.EndCap = LineCap.AnchorMask;
                    g.DrawLine(pen, (float) signalRegHorzStartPoint, (fontSize + y_offset) - 3f, (float) (width - 5), (fontSize + y_offset) - 3f);
                    g.DrawString(s, fn, Brushes.Black, (float) 0f, (float) 0f);
                    g.DrawString(str2, fn, Brushes.Black, (float) signalRegHorzStartPoint, 0f);
                    float y = y_offset + x_offset;
                    g.DrawString("SV", fn, Brushes.Black, 0f, y);
                    g.DrawString("Elev", fn, Brushes.Black, elevStartPoint, y);
                    g.DrawString("Azim", fn, Brushes.Black, azimStartPoint, y);
                    g.DrawString("State", fn, Brushes.Black, stateHorzStartPoint, y);
                    g.DrawString("C/N0", fn, Brushes.Black, cnoHorzStartPoint, y);
                    g.DrawString("0", fn, Brushes.Black, signalRegHorzStartPoint - 3f, y);
                    g.DrawString(string.Format("-{0}", this.comm.MAX_SIG_BUFFER), fn, Brushes.Black, (float) (width - 20), y);
                    if (this.toolStripComboBox_Sattype.SelectedIndex == 0)
                    {
                        this.draw_cno_boxes(g);
                        if (this._signalBufferList.Count > 0)
                        {
                            this._signalBufferList.RemoveAt(0);
                        }
                        lock (this.comm.LockSignalDataUpdate)
                        {
                            while (this.comm.SignalDataQ.Count > 0)
                            {
                                SignalData item = this.comm.SignalDataQ.Dequeue();
                                if (item != null)
                                {
                                    if (this._signalBufferList.Count >= this.comm.MAX_SIG_BUFFER)
                                    {
                                        this._signalBufferList.RemoveAt(0);
                                    }
                                    this._signalBufferList.Add(item);
                                }
                            }
                        }
                        int count = this._signalBufferList.Count;
                        if (count < this.comm.MAX_SIG_BUFFER)
                        {
                            for (int j = 0; j < (this.comm.MAX_SIG_BUFFER - count); j++)
                            {
                                this._signalBufferList.Add(new SignalData(12));
                            }
                        }
                        int num7 = this.comm.MAX_SIG_BUFFER - 1;
                        for (int i = 0; num7 >= 0; i++)
                        {
                            this.draw_1sec_data(g, this._signalBufferList[num7], i);
                            num7--;
                        }
                        graphics.Render(e.Graphics);
                    }
                    else if (this.toolStripComboBox_Sattype.SelectedIndex == 1)
                    {
                        this.draw_cno_boxes(g);
                        if (this._signalBufferList_all.Count > 0)
                        {
                            this._signalBufferList_all.RemoveAt(0);
                        }
                        lock (this.comm.LockSignalDataUpdate_ALL)
                        {
                            while (this.comm.SignalDataQ_ALL.Count > 0)
                            {
                                SignalData data2 = this.comm.SignalDataQ_ALL.Dequeue();
                                if (data2 != null)
                                {
                                    if (this._signalBufferList_all.Count >= this.comm.MAX_SIG_BUFFER)
                                    {
                                        this._signalBufferList_all.RemoveAt(0);
                                    }
                                    this._signalBufferList_all.Add(data2);
                                }
                            }
                        }
                        int num9 = this._signalBufferList_all.Count;
                        if (num9 < this.comm.MAX_SIG_BUFFER)
                        {
                            for (int m = 0; m < (this.comm.MAX_SIG_BUFFER - num9); m++)
                            {
                                this._signalBufferList_all.Add(new SignalData(60));
                            }
                        }
                        int num11 = this.comm.MAX_SIG_BUFFER - 1;
                        for (int k = 0; num11 >= 0; k++)
                        {
                            this.draw_1sec_data_glonass(g, this._signalBufferList_all[num11], k);
                            num11--;
                        }
                        graphics.Render(e.Graphics);
                    }
                    else if (this.toolStripComboBox_Sattype.SelectedIndex == 2)
                    {
                        this.draw_cno_boxes(g);
                        if (this._signalBufferList_all.Count > 0)
                        {
                            this._signalBufferList_all.RemoveAt(0);
                        }
                        lock (this.comm.LockSignalDataUpdate_ALL)
                        {
                            while (this.comm.SignalDataQ_ALL.Count > 0)
                            {
                                SignalData data3 = this.comm.SignalDataQ_ALL.Dequeue();
                                if (data3 != null)
                                {
                                    if (this._signalBufferList_all.Count >= this.comm.MAX_SIG_BUFFER)
                                    {
                                        this._signalBufferList_all.RemoveAt(0);
                                    }
                                    this._signalBufferList_all.Add(data3);
                                }
                            }
                        }
                        int num13 = this._signalBufferList_all.Count;
                        if (num13 < this.comm.MAX_SIG_BUFFER)
                        {
                            for (int num14 = 0; num14 < (this.comm.MAX_SIG_BUFFER - num13); num14++)
                            {
                                this._signalBufferList_all.Add(new SignalData(60));
                            }
                        }
                        int num15 = this.comm.MAX_SIG_BUFFER - 1;
                        for (int n = 0; num15 >= 0; n++)
                        {
                            this.draw_1sec_data_all(g, this._signalBufferList_all[num15], n);
                            num15--;
                        }
                        graphics.Render(e.Graphics);
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void panel_signal_Resize(object sender, EventArgs e)
        {
            this.Refresh();
            this.panel_signal.Refresh();
        }

        private void toolStripComboBox_Sattype_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._SatType = this.toolStripComboBox_Sattype.SelectedIndex;
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
                this.comm.DisplayPanelSignal = this.panel_signal;
                this.myPanel = this.panel_signal;
                this.comm.EnableSignalView = true;
                this.Text = this.comm.sourceDeviceName + ": Signal View X";
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

