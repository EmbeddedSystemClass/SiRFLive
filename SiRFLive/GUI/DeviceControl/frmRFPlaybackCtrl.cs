﻿namespace SiRFLive.GUI.DeviceControl
{
    using SiRFLive.TestAutomation;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class frmRFPlaybackCtrl : Form
    {
        private IContainer components;
        private uint currPlayedBytes;
        private int currPlayedStatus;
        private byte[] elapsedT = new byte[3];
        private Label label1;
        private static frmRFPlaybackCtrl m_SChildform;
        private CheckBox pbAllFileChk;
        private GroupBox pbCtrlGroup;
        private CheckBox pbDoneVal;
        private Label pbElapsedTimeVal;
        private Label pbFullFileTimeVal;
        private TextBox pbHourVal;
        private TextBox pbMinuteVal;
        private Label pbPlayedBytesLabel;
        private Label pbPlayedBytesVal;
        private TextBox pbSecVal;
        private CheckBox pbSizeChk;
        private TextBox pbSizeVal;
        private Button pbStartBut;
        private GroupBox pbStatusGroup;
        private Button pbStopBut;
        private CheckBox pbTimeChk;
        private Label pbTimeDivider1;
        private Label pbTimeDivider2;
        public RFPlaybackInterface PlaybackCtrl = new RFPlaybackInterface();
        public SiRFLiveEvent PlaybackDoneEvent = new SiRFLiveEvent();
        private Button playbackFileBrowser;
        private Label playbackFilePathLabel;
        private TextBox playbackFilePathVal;
        public ObjectInterface PlaybackGuiCtrl = new ObjectInterface();
        private object playbackLockUpdate = new object();

        public frmRFPlaybackCtrl()
        {
            this.InitializeComponent();
        }

        public void CloseRFReplayPlaybackWindow()
        {
			base.BeginInvoke((MethodInvoker)delegate
			{
                base.Close();
                m_SChildform = null;
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRegisterErrorCallback(ERRORCALLBACK pFnErrorCallBack);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRegisterExacqErrorCallback(ERRORCALLBACK pFnExacqErrorCallBack);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRegisterProgressCallback(PROGRESSCALLBACK pFnProgCallBack, int type);
        public static frmRFPlaybackCtrl GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmRFPlaybackCtrl();
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmRFPlaybackCtrl));
            this.playbackFilePathVal = new TextBox();
            this.playbackFilePathLabel = new Label();
            this.playbackFileBrowser = new Button();
            this.pbTimeChk = new CheckBox();
            this.pbHourVal = new TextBox();
            this.pbMinuteVal = new TextBox();
            this.pbTimeDivider1 = new Label();
            this.pbTimeDivider2 = new Label();
            this.pbSecVal = new TextBox();
            this.pbSizeChk = new CheckBox();
            this.pbSizeVal = new TextBox();
            this.pbAllFileChk = new CheckBox();
            this.pbStartBut = new Button();
            this.pbStopBut = new Button();
            this.pbCtrlGroup = new GroupBox();
            this.pbFullFileTimeVal = new Label();
            this.pbStatusGroup = new GroupBox();
            this.pbElapsedTimeVal = new Label();
            this.pbPlayedBytesVal = new Label();
            this.pbDoneVal = new CheckBox();
            this.label1 = new Label();
            this.pbPlayedBytesLabel = new Label();
            this.pbCtrlGroup.SuspendLayout();
            this.pbStatusGroup.SuspendLayout();
            base.SuspendLayout();
            this.playbackFilePathVal.Location = new Point(0x17, 0x2b);
            this.playbackFilePathVal.Name = "playbackFilePathVal";
            this.playbackFilePathVal.Size = new Size(0x1b3, 20);
            this.playbackFilePathVal.TabIndex = 1;
            this.playbackFilePathLabel.AutoSize = true;
            this.playbackFilePathLabel.Location = new Point(0x19, 0x16);
            this.playbackFilePathLabel.Name = "playbackFilePathLabel";
            this.playbackFilePathLabel.Size = new Size(0x5f, 13);
            this.playbackFilePathLabel.TabIndex = 0;
            this.playbackFilePathLabel.Text = "Playback File Path";
            this.playbackFileBrowser.Location = new Point(0x1d0, 0x2b);
            this.playbackFileBrowser.Name = "playbackFileBrowser";
            this.playbackFileBrowser.Size = new Size(0x1a, 0x17);
            this.playbackFileBrowser.TabIndex = 2;
            this.playbackFileBrowser.Text = "...";
            this.playbackFileBrowser.UseVisualStyleBackColor = true;
            this.playbackFileBrowser.Click += new EventHandler(this.playbackFileBrowser_Click);
            this.pbTimeChk.AutoSize = true;
            this.pbTimeChk.Location = new Point(0x10, 0x20);
            this.pbTimeChk.Name = "pbTimeChk";
            this.pbTimeChk.Size = new Size(0x40, 0x11);
            this.pbTimeChk.TabIndex = 3;
            this.pbTimeChk.Text = "Play For";
            this.pbTimeChk.UseVisualStyleBackColor = true;
            this.pbTimeChk.CheckedChanged += new EventHandler(this.plTimeChk_CheckedChanged);
            this.pbHourVal.Location = new Point(120, 0x1b);
            this.pbHourVal.Name = "pbHourVal";
            this.pbHourVal.Size = new Size(0x1b, 20);
            this.pbHourVal.TabIndex = 4;
            this.pbMinuteVal.Location = new Point(0xa1, 0x1b);
            this.pbMinuteVal.Name = "pbMinuteVal";
            this.pbMinuteVal.Size = new Size(0x1b, 20);
            this.pbMinuteVal.TabIndex = 6;
            this.pbTimeDivider1.AutoSize = true;
            this.pbTimeDivider1.Location = new Point(0x95, 0x1f);
            this.pbTimeDivider1.Name = "pbTimeDivider1";
            this.pbTimeDivider1.Size = new Size(10, 13);
            this.pbTimeDivider1.TabIndex = 5;
            this.pbTimeDivider1.Text = ":";
            this.pbTimeDivider2.AutoSize = true;
            this.pbTimeDivider2.Location = new Point(190, 0x1f);
            this.pbTimeDivider2.Name = "pbTimeDivider2";
            this.pbTimeDivider2.Size = new Size(10, 13);
            this.pbTimeDivider2.TabIndex = 7;
            this.pbTimeDivider2.Text = ":";
            this.pbSecVal.Location = new Point(0xca, 0x1b);
            this.pbSecVal.Name = "pbSecVal";
            this.pbSecVal.Size = new Size(0x1b, 20);
            this.pbSecVal.TabIndex = 8;
            this.pbSizeChk.AutoSize = true;
            this.pbSizeChk.Location = new Point(0x10, 0x47);
            this.pbSizeChk.Name = "pbSizeChk";
            this.pbSizeChk.Size = new Size(100, 0x11);
            this.pbSizeChk.TabIndex = 9;
            this.pbSizeChk.Text = "Play Up to (MB)";
            this.pbSizeChk.UseVisualStyleBackColor = true;
            this.pbSizeChk.CheckedChanged += new EventHandler(this.pbSizeChk_CheckedChanged);
            this.pbSizeVal.Location = new Point(0x81, 0x44);
            this.pbSizeVal.Name = "pbSizeVal";
            this.pbSizeVal.Size = new Size(100, 20);
            this.pbSizeVal.TabIndex = 10;
            this.pbSizeVal.Text = "10";
            this.pbAllFileChk.AutoSize = true;
            this.pbAllFileChk.Location = new Point(0x10, 0x6a);
            this.pbAllFileChk.Name = "pbAllFileChk";
            this.pbAllFileChk.Size = new Size(0x5f, 0x11);
            this.pbAllFileChk.TabIndex = 11;
            this.pbAllFileChk.Text = "Play Entire File";
            this.pbAllFileChk.UseVisualStyleBackColor = true;
            this.pbAllFileChk.CheckedChanged += new EventHandler(this.pbAllFileChk_CheckedChanged);
            this.pbStartBut.Location = new Point(0x16e, 0x1c);
            this.pbStartBut.Name = "pbStartBut";
            this.pbStartBut.Size = new Size(0x4b, 0x17);
            this.pbStartBut.TabIndex = 0x12;
            this.pbStartBut.Text = "&Start";
            this.pbStartBut.UseVisualStyleBackColor = true;
            this.pbStartBut.Click += new EventHandler(this.pbStartBut_Click);
            this.pbStopBut.Location = new Point(0x16e, 0x43);
            this.pbStopBut.Name = "pbStopBut";
            this.pbStopBut.Size = new Size(0x4b, 0x17);
            this.pbStopBut.TabIndex = 0x13;
            this.pbStopBut.Text = "Stop";
            this.pbStopBut.UseVisualStyleBackColor = true;
            this.pbStopBut.Click += new EventHandler(this.pbStopBut_Click);
            this.pbCtrlGroup.Controls.Add(this.pbFullFileTimeVal);
            this.pbCtrlGroup.Controls.Add(this.pbStopBut);
            this.pbCtrlGroup.Controls.Add(this.pbStartBut);
            this.pbCtrlGroup.Controls.Add(this.pbAllFileChk);
            this.pbCtrlGroup.Controls.Add(this.pbSizeVal);
            this.pbCtrlGroup.Controls.Add(this.pbSizeChk);
            this.pbCtrlGroup.Controls.Add(this.pbTimeDivider2);
            this.pbCtrlGroup.Controls.Add(this.pbSecVal);
            this.pbCtrlGroup.Controls.Add(this.pbTimeDivider1);
            this.pbCtrlGroup.Controls.Add(this.pbMinuteVal);
            this.pbCtrlGroup.Controls.Add(this.pbHourVal);
            this.pbCtrlGroup.Controls.Add(this.pbTimeChk);
            this.pbCtrlGroup.Location = new Point(0x17, 0x57);
            this.pbCtrlGroup.Name = "pbCtrlGroup";
            this.pbCtrlGroup.Size = new Size(0x1d3, 0x8d);
            this.pbCtrlGroup.TabIndex = 13;
            this.pbCtrlGroup.TabStop = false;
            this.pbCtrlGroup.Text = "Control";
            this.pbFullFileTimeVal.AutoSize = true;
            this.pbFullFileTimeVal.Location = new Point(0x81, 0x6a);
            this.pbFullFileTimeVal.Name = "pbFullFileTimeVal";
            this.pbFullFileTimeVal.Size = new Size(13, 13);
            this.pbFullFileTimeVal.TabIndex = 12;
            this.pbFullFileTimeVal.Text = "0";
            this.pbStatusGroup.Controls.Add(this.pbElapsedTimeVal);
            this.pbStatusGroup.Controls.Add(this.pbPlayedBytesVal);
            this.pbStatusGroup.Controls.Add(this.pbDoneVal);
            this.pbStatusGroup.Controls.Add(this.label1);
            this.pbStatusGroup.Controls.Add(this.pbPlayedBytesLabel);
            this.pbStatusGroup.Location = new Point(0x17, 250);
            this.pbStatusGroup.Name = "pbStatusGroup";
            this.pbStatusGroup.Size = new Size(0x1d3, 100);
            this.pbStatusGroup.TabIndex = 14;
            this.pbStatusGroup.TabStop = false;
            this.pbStatusGroup.Text = "Status";
            this.pbElapsedTimeVal.AutoSize = true;
            this.pbElapsedTimeVal.Location = new Point(0x81, 0x44);
            this.pbElapsedTimeVal.Name = "pbElapsedTimeVal";
            this.pbElapsedTimeVal.Size = new Size(0x1f, 13);
            this.pbElapsedTimeVal.TabIndex = 0x10;
            this.pbElapsedTimeVal.Text = "0:0:0";
            this.pbPlayedBytesVal.AutoSize = true;
            this.pbPlayedBytesVal.Location = new Point(0x7e, 0x1c);
            this.pbPlayedBytesVal.Name = "pbPlayedBytesVal";
            this.pbPlayedBytesVal.Size = new Size(13, 13);
            this.pbPlayedBytesVal.TabIndex = 14;
            this.pbPlayedBytesVal.Text = "0";
            this.pbDoneVal.AutoSize = true;
            this.pbDoneVal.Enabled = false;
            this.pbDoneVal.Location = new Point(0x16e, 0x24);
            this.pbDoneVal.Name = "pbDoneVal";
            this.pbDoneVal.Size = new Size(0x3a, 0x11);
            this.pbDoneVal.TabIndex = 0x11;
            this.pbDoneVal.Text = "Done?";
            this.pbDoneVal.UseVisualStyleBackColor = true;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x11, 0x45);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4a, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Elapsed Time:";
            this.pbPlayedBytesLabel.AutoSize = true;
            this.pbPlayedBytesLabel.Location = new Point(0x11, 0x1c);
            this.pbPlayedBytesLabel.Name = "pbPlayedBytesLabel";
            this.pbPlayedBytesLabel.Size = new Size(0x60, 13);
            this.pbPlayedBytesLabel.TabIndex = 13;
            this.pbPlayedBytesLabel.Text = "Played Bytes (MB):";
            base.AcceptButton = this.pbStartBut;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.ClientSize = new Size(0x201, 0x174);
            base.Controls.Add(this.pbStatusGroup);
            base.Controls.Add(this.pbCtrlGroup);
            base.Controls.Add(this.playbackFileBrowser);
            base.Controls.Add(this.playbackFilePathLabel);
            base.Controls.Add(this.playbackFilePathVal);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmRFPlaybackCtrl";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "RF Playback Control";
            this.pbCtrlGroup.ResumeLayout(false);
            this.pbCtrlGroup.PerformLayout();
            this.pbStatusGroup.ResumeLayout(false);
            this.pbStatusGroup.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            m_SChildform = null;
        }

        private void pbAllFileChk_CheckedChanged(object sender, EventArgs e)
        {
            if (this.pbAllFileChk.Checked)
            {
                this.pbHourVal.ReadOnly = true;
                this.pbMinuteVal.ReadOnly = true;
                this.pbSecVal.ReadOnly = true;
                this.pbTimeChk.Enabled = false;
                this.pbSizeVal.ReadOnly = true;
                this.pbSizeChk.Enabled = false;
                this.PlaybackCtrl.SetManual(this.PlaybackCtrl.PLAYBACK);
            }
            else
            {
                this.pbHourVal.ReadOnly = false;
                this.pbMinuteVal.ReadOnly = false;
                this.pbSecVal.ReadOnly = false;
                this.pbTimeChk.Enabled = true;
                this.pbSizeVal.ReadOnly = false;
                this.pbSizeChk.Enabled = true;
            }
        }

        private int pbDisplayProgress(ref CPBK_Time_Config timeInfo, uint playedBytes, int playedStatus)
        {
            lock (this.playbackLockUpdate)
            {
                if (!this.PlaybackCtrl.IsFinished)
                {
                    this.elapsedT[0] = timeInfo.u8Hours;
                    this.elapsedT[1] = timeInfo.u8Minutes;
                    this.elapsedT[2] = timeInfo.u8Seconds;
                    this.currPlayedBytes = playedBytes;
                    this.currPlayedStatus = playedStatus;
                }
            }
            return 1;
        }

        private int pbErrorLog(uint i32ErrorId, char[] szErrorString, uint i32LineNo)
        {
            this.PlaybackCtrl.CPBK_ERROR_ID = i32ErrorId;
            this.PlaybackCtrl.CPBK_ERROR_STRING = new string(szErrorString);
            return 1;
        }

        private int pbExacqErrorLog(uint i32ErrorId, char[] szErrorString, uint i32LineNo)
        {
            this.PlaybackCtrl.EXACQ_ERROR_ID = i32ErrorId;
            this.PlaybackCtrl.EXACQ_ERROR_STRING = new string(szErrorString);
            return 1;
        }

        private void pbReInitWindow()
        {
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbDoneVal, true);
            this.PlaybackGuiCtrl.SetCheckBoxChecked(this.pbDoneVal, true);
            this.PlaybackGuiCtrl.SetButtonState(this.pbStartBut, true);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbAllFileChk, true);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbSizeChk, true);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbTimeChk, true);
            this.PlaybackGuiCtrl.SetCheckBoxChecked(this.pbAllFileChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxChecked(this.pbSizeChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxChecked(this.pbTimeChk, false);
            this.PlaybackGuiCtrl.SetTextBoxState(this.pbSizeVal, true);
            this.PlaybackGuiCtrl.SetButtonState(this.playbackFileBrowser, true);
            this.PlaybackGuiCtrl.SetTextBoxState(this.pbHourVal, true);
            this.PlaybackGuiCtrl.SetTextBoxState(this.pbMinuteVal, true);
            this.PlaybackGuiCtrl.SetTextBoxState(this.pbSecVal, true);
            this.PlaybackGuiCtrl.SetLabelText(this.pbFullFileTimeVal, "0");
            this.PlaybackGuiCtrl.SetLabelText(this.pbPlayedBytesVal, "0");
            this.PlaybackGuiCtrl.SetLabelText(this.pbElapsedTimeVal, "0:0:0");
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbDoneVal, false);
        }

        private void pbSizeChk_CheckedChanged(object sender, EventArgs e)
        {
            if (this.pbSizeChk.Checked)
            {
                this.pbHourVal.ReadOnly = true;
                this.pbMinuteVal.ReadOnly = true;
                this.pbSecVal.ReadOnly = true;
                this.pbTimeChk.Enabled = false;
                this.pbSizeVal.ReadOnly = false;
                this.pbAllFileChk.Enabled = false;
                this.PlaySizeSet();
            }
            else
            {
                this.pbHourVal.ReadOnly = false;
                this.pbMinuteVal.ReadOnly = false;
                this.pbSecVal.ReadOnly = false;
                this.pbTimeChk.Enabled = true;
                this.pbSizeVal.ReadOnly = false;
                this.pbAllFileChk.Enabled = true;
            }
        }

        private void pbStartBut_Click(object sender, EventArgs e)
        {
            this.PlaybackCtrl.CPBK_ERROR_ID = 1;
            this.PlaybackCtrl.EXACQ_ERROR_ID = 1;
            this.PlaybackCtrl.CPBK_ERROR_STRING = "NO_ERROR";
            this.PlaybackCtrl.EXACQ_ERROR_STRING = "NO_ERROR";
            if (this.playbackFilePathVal.Text.Length == 0)
            {
                if (this.PlaybackCtrl.IsManual)
                {
                    MessageBox.Show("No input file", "ERROR!");
                    this.pbStartBut.Enabled = true;
                }
                else
                {
                    this.pbStartBut.Enabled = true;
                }
            }
            else
            {
                int index = this.playbackFilePathVal.Text.IndexOf(".");
                string str = this.playbackFilePathVal.Text.Substring(index + 1, 3);
                if (!File.Exists(this.playbackFilePathVal.Text))
                {
                    if (this.PlaybackCtrl.IsManual)
                    {
                        MessageBox.Show(this.playbackFilePathVal.Text + " : File does not exist", "ERROR!");
                        this.pbStartBut.Enabled = true;
                    }
                    else
                    {
                        this.pbStartBut.Enabled = true;
                    }
                }
                else if (str != "pcm")
                {
                    if (this.PlaybackCtrl.IsManual)
                    {
                        MessageBox.Show(str + " not a pcm type", "ERROR!");
                        this.pbStartBut.Enabled = true;
                    }
                    else
                    {
                        this.pbStartBut.Enabled = true;
                    }
                }
                else
                {
                    int errorCode = this.PlaybackCtrl.SetFile(this.playbackFilePathVal.Text, this.PlaybackCtrl.PLAYBACK);
                    if (errorCode != 1)
                    {
                        if (this.PlaybackCtrl.IsManual)
                        {
                            MessageBox.Show("Error set file: " + this.PlaybackCtrl.ErrorToString(errorCode), "ERROR!");
                            this.pbStartBut.Enabled = true;
                        }
                        else
                        {
                            this.pbStartBut.Enabled = true;
                        }
                    }
                    else if ((!this.pbSizeChk.Checked && !this.pbAllFileChk.Checked) && !this.pbTimeChk.Checked)
                    {
                        if (this.PlaybackCtrl.IsManual)
                        {
                            MessageBox.Show("No playback type selected", "ERROR!");
                            this.pbStartBut.Enabled = true;
                        }
                        else
                        {
                            this.pbStartBut.Enabled = true;
                        }
                    }
                    else if ((!this.pbTimeChk.Checked || (this.PlayTimeSet() <= 0)) && (!this.pbSizeChk.Checked || (this.PlaySizeSet() <= 0)))
                    {
                        if (this.pbAllFileChk.Checked)
                        {
                            this.SetPlayAllFile(this.playbackFilePathVal.Text);
                        }
                        errorCode = this.PlaybackStart();
                        if (errorCode != 1)
                        {
                            if (this.PlaybackCtrl.IsManual)
                            {
                                this.pbStartBut.Enabled = true;
                                this.PlaybackCtrl.IsFinished = true;
                                this.PlaybackCtrl.CPBK_ERROR_ID = 0;
                                this.PlaybackCtrl.CPBK_ERROR_STRING = this.PlaybackCtrl.ErrorToString(errorCode);
                                this.PlaybackStop();
                                MessageBox.Show("Playback encountered error: " + this.PlaybackCtrl.CPBK_ERROR_STRING, "ERROR!");
                            }
                            else
                            {
                                this.PlaybackStop();
                            }
                        }
                    }
                }
            }
        }

        private void pbStopBut_Click(object sender, EventArgs e)
        {
            this.PlaybackStop();
        }

        private void pbUpdateProgress()
        {
            object obj3;
            byte[] timeInfo = new byte[3];
            uint currPlayedBytes = 0;
            int currPlayedStatus = 0;
            lock (this.playbackLockUpdate)
            {
                this.PlaybackCtrl.UnregisterCallbacks(this.PlaybackCtrl.PLAYBACK);
                this.elapsedT[0] = 0;
                this.elapsedT[1] = 0;
                this.elapsedT[2] = 0;
                this.currPlayedBytes = 0;
                this.currPlayedStatus = 1;
            }
            PROGRESSCALLBACK pFnProgCallBack = new PROGRESSCALLBACK(this.pbDisplayProgress);
            fnRegisterProgressCallback(pFnProgCallBack, this.PlaybackCtrl.PLAYBACK);
            ERRORCALLBACK pFnErrorCallBack = new ERRORCALLBACK(this.pbErrorLog);
            fnRegisterErrorCallback(pFnErrorCallBack);
            ERRORCALLBACK pFnExacqErrorCallBack = new ERRORCALLBACK(this.pbExacqErrorLog);
            fnRegisterExacqErrorCallback(pFnExacqErrorCallBack);
            GCHandle handle = GCHandle.Alloc(pFnProgCallBack);
            GCHandle handle2 = GCHandle.Alloc(pFnErrorCallBack);
            GCHandle handle3 = GCHandle.Alloc(pFnExacqErrorCallBack);
            int num3 = 0;
        Label_00CF:
            Monitor.Enter(obj3 = this.playbackLockUpdate);
            try
            {
                timeInfo[0] = this.elapsedT[0];
                timeInfo[1] = this.elapsedT[1];
                timeInfo[2] = this.elapsedT[2];
                if (currPlayedBytes == this.currPlayedBytes)
                {
                    num3++;
                }
                else
                {
                    num3 = 0;
                }
                currPlayedBytes = this.currPlayedBytes;
                currPlayedStatus = this.currPlayedStatus;
            }
            finally
            {
                Monitor.Exit(obj3);
            }
            string text = string.Format("{0}:{1}:{2}", Convert.ToString(timeInfo[0]), Convert.ToString(timeInfo[1]), Convert.ToString(timeInfo[2]));
            this.PlaybackGuiCtrl.SetLabelText(this.pbElapsedTimeVal, text);
            this.PlaybackGuiCtrl.SetLabelText(this.pbPlayedBytesVal, Convert.ToString(currPlayedBytes));
            if ((this.PlaybackCtrl.TotalFileTime != 0) && ((this.PlaybackCtrl.GetTotalTimeInSec(timeInfo) > this.PlaybackCtrl.TotalFileTime) || (num3 > 0x3e8)))
            {
                this.PlaybackCtrl.Stop(this.PlaybackCtrl.PLAYBACK);
                currPlayedStatus = 5;
            }
            if ((this.PlaybackCtrl.TotalPlaySize != 0) && ((currPlayedBytes >= this.PlaybackCtrl.TotalPlaySize) || (num3 > 0x3e8)))
            {
                this.PlaybackCtrl.Stop(this.PlaybackCtrl.PLAYBACK);
                currPlayedStatus = 5;
            }
            if ((currPlayedStatus == 5) | (currPlayedStatus == 3))
            {
                this.pbReInitWindow();
                this.PlaybackCtrl.TotalPlaySize = 0;
                this.PlaybackCtrl.TotalFileTime = 0;
                this.PlaybackCtrl.IsFinished = true;
            }
            else
            {
                this.PlaybackGuiCtrl.SetButtonState(this.pbStartBut, false);
                this.PlaybackCtrl.IsFinished = false;
                Thread.Sleep(500);
                goto Label_00CF;
            }
            handle.Free();
            handle2.Free();
            handle3.Free();
            new Thread(new ThreadStart(this.PlaybackDoneEvent.SiRFLiveEventSet)).Start();
        }

        private void playbackFileBrowser_Click(object sender, EventArgs e)
        {
            this.PlaybackGuiCtrl.FileBrowser(this.playbackFilePathVal);
        }

        public int PlaybackStart()
        {
            this.PlaybackCtrl.IsFinished = true;
            int num = this.PlaybackCtrl.Start(this.PlaybackCtrl.PLAYBACK);
            if (num == 1)
            {
                Thread thread = new Thread(new ThreadStart(this.pbUpdateProgress));
                thread.IsBackground = true;
                try
                {
                    thread.Start();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Updated progress thread fails to start: " + exception.Message, "ERROR!");
                }
                this.PlaybackGuiCtrl.SetButtonState(this.pbStartBut, false);
                this.PlaybackGuiCtrl.SetCheckBoxState(this.pbDoneVal, false);
                this.PlaybackGuiCtrl.SetCheckBoxChecked(this.pbDoneVal, false);
                this.PlaybackGuiCtrl.SetTextBoxState(this.pbHourVal, false);
                this.PlaybackGuiCtrl.SetTextBoxState(this.pbMinuteVal, false);
                this.PlaybackGuiCtrl.SetTextBoxState(this.pbSecVal, false);
                this.PlaybackGuiCtrl.SetTextBoxState(this.pbSizeVal, false);
                this.PlaybackGuiCtrl.SetButtonState(this.playbackFileBrowser, false);
                this.PlaybackGuiCtrl.SetCheckBoxState(this.pbAllFileChk, false);
                this.PlaybackGuiCtrl.SetCheckBoxState(this.pbSizeChk, false);
                this.PlaybackGuiCtrl.SetCheckBoxState(this.pbTimeChk, false);
            }
            return num;
        }

        public void PlaybackStop()
        {
            this.PlaybackCtrl.Stop(this.PlaybackCtrl.PLAYBACK);
            this.pbReInitWindow();
        }

        private int PlaySizeSet()
        {
            uint num;
            if (this.pbSizeVal.Text.Length == 0)
            {
                this.pbSizeVal.Text = "0";
            }
            if (this.pbSizeVal.Text.Contains(".") || this.pbSizeVal.Text.Contains(","))
            {
                MessageBox.Show("Invalid input!\nInteger only", "ERROR!");
                this.pbReInitWindow();
                return 1;
            }
            try
            {
                num = Convert.ToUInt32(this.pbSizeVal.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
                return 1;
            }
            if ((num < 10) && this.PlaybackCtrl.IsManual)
            {
                MessageBox.Show("Size to play too small < 10 MB", "ERROR!");
                this.pbReInitWindow();
                return 1;
            }
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbAllFileChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbSizeChk, true);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbTimeChk, false);
            this.PlaybackCtrl.SetSpace(num, this.PlaybackCtrl.PLAYBACK);
            return 0;
        }

        public int PlayTimeSet()
        {
            int[] numArray2 = new int[3];
            numArray2[1] = 1;
            numArray2[2] = 1;
            int[] inTime = numArray2;
            if (((this.pbHourVal.Text.Length == 0) | (this.pbMinuteVal.Text.Length == 0)) | (this.pbSecVal.Text.Length == 0))
            {
                this.UpdatePlayForTime(0, 0, 0);
            }
            if (((this.pbHourVal.Text.Contains(".") || this.pbMinuteVal.Text.Contains(".")) || (this.pbSecVal.Text.Contains(".") || this.pbHourVal.Text.Contains(","))) || (this.pbMinuteVal.Text.Contains(",") || this.pbSecVal.Text.Contains(",")))
            {
                MessageBox.Show("Invalid time input", "ERROR!");
                this.pbReInitWindow();
                return 1;
            }
            try
            {
                inTime[0] = Convert.ToInt32(this.pbHourVal.Text);
                inTime[1] = Convert.ToInt32(this.pbMinuteVal.Text);
                inTime[2] = Convert.ToInt32(this.pbSecVal.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
                return 1;
            }
            if (((inTime[0] < 0) || (inTime[1] < 0)) || (((inTime[1] > 0x3b) || (inTime[2] < 0)) || (inTime[2] > 0x3b)))
            {
                if (this.PlaybackCtrl.IsManual)
                {
                    MessageBox.Show("Invalid time input", "ERROR!");
                    this.pbReInitWindow();
                    return 1;
                }
                return 1;
            }
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbAllFileChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbSizeChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbTimeChk, true);
            this.PlaybackCtrl.SetTime(inTime, this.PlaybackCtrl.PLAYBACK);
            return 0;
        }

        private void plTimeChk_CheckedChanged(object sender, EventArgs e)
        {
            this.pbHourVal.ReadOnly = false;
            this.pbMinuteVal.ReadOnly = false;
            this.pbSecVal.ReadOnly = false;
            this.pbHourVal.Enabled = true;
            this.pbMinuteVal.Enabled = true;
            this.pbSecVal.Enabled = true;
            if (this.pbTimeChk.Checked)
            {
                this.pbSizeChk.Enabled = false;
                this.pbSizeVal.ReadOnly = true;
                this.pbAllFileChk.Enabled = false;
                this.PlayTimeSet();
            }
            else
            {
                this.pbSizeChk.Enabled = true;
                this.pbSizeVal.ReadOnly = false;
                this.pbAllFileChk.Enabled = true;
            }
        }

        public void SetPlayAllFile(string fileToPlay)
        {
            byte[] timeInfo = new byte[3];
            this.PlaybackCtrl.GetTotalTime(timeInfo, fileToPlay);
            string text = string.Format("{0}:{1}:{2}", Convert.ToString(timeInfo[0]), Convert.ToString(timeInfo[1]), Convert.ToString(timeInfo[2]));
            this.PlaybackGuiCtrl.SetLabelText(this.pbFullFileTimeVal, text);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbAllFileChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbSizeChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbTimeChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxChecked(this.pbAllFileChk, true);
            this.PlaybackGuiCtrl.SetCheckBoxChecked(this.pbSizeChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxChecked(this.pbTimeChk, false);
            this.PlaybackCtrl.SetManual(this.PlaybackCtrl.PLAYBACK);
        }

        private void SiRFLiveNotificationSet(object sender, SiRFLiveEventArgs e)
        {
        }

        public void UpdatePlayFileText(string fileToPlay)
        {
            this.PlaybackGuiCtrl.SetTextBoxText(this.playbackFilePathVal, fileToPlay);
            this.PlaybackGuiCtrl.SetButtonState(this.playbackFileBrowser, false);
        }

        public void UpdatePlayForTime(byte hr, byte minute, byte sec)
        {
            byte[] timeInfo = new byte[3];
            timeInfo[0] = hr;
            timeInfo[1] = minute;
            timeInfo[2] = sec;
            this.PlaybackCtrl.TotalFileTime = this.PlaybackCtrl.GetTotalTimeInSec(timeInfo);
            this.PlaybackGuiCtrl.SetTextBoxText(this.pbHourVal, Convert.ToString(hr));
            this.PlaybackGuiCtrl.SetTextBoxText(this.pbMinuteVal, Convert.ToString(minute));
            this.PlaybackGuiCtrl.SetTextBoxText(this.pbSecVal, Convert.ToString(sec));
            this.PlaybackGuiCtrl.SetTextBoxState(this.pbHourVal, false);
            this.PlaybackGuiCtrl.SetTextBoxState(this.pbMinuteVal, false);
            this.PlaybackGuiCtrl.SetTextBoxState(this.pbSecVal, false);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbAllFileChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbSizeChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbTimeChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxChecked(this.pbAllFileChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxChecked(this.pbSizeChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxChecked(this.pbTimeChk, true);
        }

        public void UpdatePlaySize(uint size)
        {
            this.PlaybackGuiCtrl.SetTextBoxText(this.pbSizeVal, Convert.ToString(size));
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbAllFileChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbSizeChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxState(this.pbTimeChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxChecked(this.pbAllFileChk, false);
            this.PlaybackGuiCtrl.SetCheckBoxChecked(this.pbSizeChk, true);
            this.PlaybackGuiCtrl.SetCheckBoxChecked(this.pbTimeChk, false);
            this.PlaybackGuiCtrl.SetTextBoxState(this.pbSizeVal, false);
        }
    }
}

