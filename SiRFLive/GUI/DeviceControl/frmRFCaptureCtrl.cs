﻿namespace SiRFLive.GUI.DeviceControl
{
    using SiRFLive.TestAutomation;
    using SiRFLive.Utilities;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class frmRFCaptureCtrl : Form
    {
        private Label cLabel2;
        private Label clLabel1;
        private IContainer components;
        private Label cpCaptureBytesLabel;
        private Label cpCaptureBytesVal;
        public RFPlaybackInterface cpCtrl = new RFPlaybackInterface();
        private GroupBox cpCtrlBox;
        public SiRFLiveEvent cpDoneEvent = new SiRFLiveEvent();
        private CheckBox cpDoneVal;
        private Label cpElapsedTimeLabel;
        private Label cpElapsedTimeVal;
        private Button cpFileBrowser;
        private Label cpFilePathLabel;
        private TextBox cpFilePathVal;
        public ObjectInterface cpGuiCtrl = new ObjectInterface();
        private TextBox cpHourVal;
        private object cpLockUpdate = new object();
        private CheckBox cpManualStopChk;
        private TextBox cpMinuteVal;
        private TextBox cpSecVal;
        private CheckBox cpSizeChk;
        private TextBox cpSizeVal;
        private Button cpStartBut;
        private GroupBox cpStatusBox;
        private Button cpStopBut;
        private CheckBox cpTimeChk;
        private uint currPlayedBytes;
        private int currPlayedStatus;
        private byte[] elapsedT = new byte[3];
        private static frmRFCaptureCtrl m_SChildform;

        public frmRFCaptureCtrl()
        {
            this.InitializeComponent();
        }

        public void CloseRFReplayCaptureWindow()
        {
			base.BeginInvoke((MethodInvoker)delegate
			{
                base.Close();
                m_SChildform = null;
            });
        }

        private int cpDiskFilled(byte percentage)
        {
            this.cpCtrl.CPBK_ERROR_ID = 7;
            return 1;
        }

        private int cpDisplayProgress(ref CPBK_Time_Config timeInfo, uint playedBytes, int playedStatus)
        {
            lock (this.cpLockUpdate)
            {
                if (!this.cpCtrl.IsFinished)
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

        private int cpErrorLog(uint i32ErrorId, char[] szErrorString, uint i32LineNo)
        {
            this.cpCtrl.CPBK_ERROR_ID = i32ErrorId;
            this.cpCtrl.CPBK_ERROR_STRING = new string(szErrorString);
            return 1;
        }

        private int cpExacqErrorLog(uint i32ErrorId, char[] szErrorString, uint i32LineNo)
        {
            this.cpCtrl.EXACQ_ERROR_ID = i32ErrorId;
            this.cpCtrl.EXACQ_ERROR_STRING = new string(szErrorString);
            return 1;
        }

        private void cpFileBrowser_Click(object sender, EventArgs e)
        {
            this.cpGuiCtrl.FileBrowserNewFile(this.cpFilePathVal);
        }

        private void cpManualStopChk_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cpManualStopChk.Checked)
            {
                this.cpHourVal.ReadOnly = true;
                this.cpMinuteVal.ReadOnly = true;
                this.cpSecVal.ReadOnly = true;
                this.cpTimeChk.Enabled = false;
                this.cpSizeVal.ReadOnly = true;
                this.cpSizeChk.Enabled = false;
                this.cpCtrl.SetManual(this.cpCtrl.CAPTURE);
            }
            else
            {
                this.cpHourVal.ReadOnly = false;
                this.cpMinuteVal.ReadOnly = false;
                this.cpSecVal.ReadOnly = false;
                this.cpTimeChk.Enabled = true;
                this.cpSizeVal.ReadOnly = false;
                this.cpSizeChk.Enabled = true;
            }
        }

        private void cpReInitWindow()
        {
            this.cpGuiCtrl.SetCheckBoxState(this.cpDoneVal, true);
            this.cpGuiCtrl.SetCheckBoxChecked(this.cpDoneVal, true);
            this.cpGuiCtrl.SetButtonState(this.cpStartBut, true);
            this.cpGuiCtrl.SetCheckBoxState(this.cpManualStopChk, true);
            this.cpGuiCtrl.SetCheckBoxState(this.cpSizeChk, true);
            this.cpGuiCtrl.SetCheckBoxState(this.cpTimeChk, true);
            this.cpGuiCtrl.SetButtonState(this.cpFileBrowser, true);
            this.cpGuiCtrl.SetTextBoxState(this.cpSizeVal, true);
            this.cpGuiCtrl.SetTextBoxState(this.cpHourVal, true);
            this.cpGuiCtrl.SetTextBoxState(this.cpMinuteVal, true);
            this.cpGuiCtrl.SetTextBoxState(this.cpSecVal, true);
            this.cpCtrl.IsFinished = true;
        }

        private void cpSizeChk_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cpSizeChk.Checked)
            {
                this.cpHourVal.ReadOnly = true;
                this.cpMinuteVal.ReadOnly = true;
                this.cpSecVal.ReadOnly = true;
                this.cpTimeChk.Enabled = false;
                this.cpSizeVal.ReadOnly = false;
                this.cpManualStopChk.Enabled = false;
                if (this.cpSizeSet() == 0)
                {
                }
            }
            else
            {
                this.cpHourVal.ReadOnly = false;
                this.cpMinuteVal.ReadOnly = false;
                this.cpSecVal.ReadOnly = false;
                this.cpTimeChk.Enabled = true;
                this.cpSizeVal.ReadOnly = false;
                this.cpManualStopChk.Enabled = true;
            }
        }

        private int cpSizeSet()
        {
            uint num;
            if (this.cpSizeVal.Text.Length == 0)
            {
                if (this.cpCtrl.IsManual)
                {
                    MessageBox.Show("Length is blank", "ERROR!");
                    return 1;
                }
                return 1;
            }
            if (this.cpSizeVal.Text.Contains(".") || this.cpSizeVal.Text.Contains(","))
            {
                MessageBox.Show("Invalid input\nInteger only", "ERROR!");
                return 1;
            }
            try
            {
                num = Convert.ToUInt32(this.cpSizeVal.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
                return 1;
            }
            if (num < 10)
            {
                if (this.cpCtrl.IsManual)
                {
                    MessageBox.Show("Size to play too small < 10 MB", "ERROR!");
                    return 1;
                }
                return 1;
            }
            this.cpCtrl.SetSpace(num, this.cpCtrl.CAPTURE);
            return 0;
        }

        public int cpStart()
        {
            this.cpCtrl.IsFinished = true;
            Thread thread = new Thread(new ThreadStart(this.cpUpdateProgress));
            try
            {
                thread.Start();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Thread failed start: " + exception.Message, "ERROR!");
            }
            int num = this.cpCtrl.Start(this.cpCtrl.CAPTURE);
            this.cpGuiCtrl.SetButtonState(this.cpStartBut, false);
            this.cpGuiCtrl.SetCheckBoxState(this.cpDoneVal, false);
            this.cpGuiCtrl.SetCheckBoxChecked(this.cpDoneVal, false);
            this.cpGuiCtrl.SetCheckBoxState(this.cpManualStopChk, false);
            this.cpGuiCtrl.SetCheckBoxState(this.cpSizeChk, false);
            this.cpGuiCtrl.SetCheckBoxState(this.cpTimeChk, false);
            this.cpGuiCtrl.SetTextBoxState(this.cpSizeVal, false);
            this.cpGuiCtrl.SetButtonState(this.cpFileBrowser, false);
            this.cpGuiCtrl.SetTextBoxState(this.cpHourVal, false);
            this.cpGuiCtrl.SetTextBoxState(this.cpMinuteVal, false);
            this.cpGuiCtrl.SetTextBoxState(this.cpSecVal, false);
            return num;
        }

        private void cpStartBut_Click(object sender, EventArgs e)
        {
            if (this.cpFilePathVal.Text.Length == 0)
            {
                if (this.cpCtrl.IsManual)
                {
                    MessageBox.Show("No output file", "ERROR!");
                    this.cpStartBut.Enabled = true;
                }
                else
                {
                    this.cpStartBut.Enabled = true;
                }
            }
            else
            {
                int index = this.cpFilePathVal.Text.IndexOf(".");
                string str = this.cpFilePathVal.Text.Substring(index + 1, 3);
                if (str != "pcm")
                {
                    if (this.cpCtrl.IsManual)
                    {
                        MessageBox.Show("Error: " + str + " not of pcm type", "ERROR!");
                        this.cpStartBut.Enabled = true;
                    }
                    else
                    {
                        this.cpStartBut.Enabled = true;
                    }
                }
                else
                {
                    int errorCode = this.cpCtrl.SetFile(this.cpFilePathVal.Text, this.cpCtrl.CAPTURE);
                    if (errorCode != 1)
                    {
                        if (this.cpCtrl.IsManual)
                        {
                            MessageBox.Show("Error set file: " + this.cpCtrl.ErrorToString(errorCode), "ERROR!");
                            this.cpStartBut.Enabled = true;
                        }
                        else
                        {
                            this.cpStartBut.Enabled = true;
                        }
                    }
                    else if ((!this.cpSizeChk.Checked && !this.cpManualStopChk.Checked) && !this.cpTimeChk.Checked)
                    {
                        if (this.cpCtrl.IsManual)
                        {
                            MessageBox.Show("No type selected", "ERROR!");
                            this.cpStartBut.Enabled = true;
                        }
                        else
                        {
                            this.cpStartBut.Enabled = true;
                        }
                    }
                    else if ((!this.cpTimeChk.Checked || (this.cpTimeSet() <= 0)) && (!this.cpSizeChk.Checked || (this.cpSizeSet() <= 0)))
                    {
                        errorCode = this.cpStart();
                        if (errorCode != 1)
                        {
                            if (this.cpCtrl.IsManual)
                            {
                                MessageBox.Show("Capture encoutered error" + this.cpCtrl.ErrorToString(errorCode), "ERROR!");
                                this.cpStartBut.Enabled = true;
                                this.cpCtrl.IsFinished = true;
                            }
                            else
                            {
                                this.cpStop();
                            }
                        }
                    }
                }
            }
        }

        public void cpStop()
        {
            this.cpCtrl.Stop(this.cpCtrl.CAPTURE);
            this.cpStartBut.Enabled = true;
        }

        private void cpStopBut_Click(object sender, EventArgs e)
        {
            this.cpStop();
        }

        private void cpTimeChk_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cpTimeChk.Checked)
            {
                this.cpHourVal.ReadOnly = false;
                this.cpMinuteVal.ReadOnly = false;
                this.cpSecVal.ReadOnly = false;
                this.cpSizeChk.Enabled = false;
                this.cpSizeVal.ReadOnly = true;
                this.cpManualStopChk.Enabled = false;
                this.cpTimeSet();
            }
            else
            {
                this.cpSizeChk.Enabled = true;
                this.cpSizeVal.ReadOnly = false;
                this.cpManualStopChk.Enabled = true;
            }
        }

        private int cpTimeSet()
        {
            int[] numArray2 = new int[3];
            numArray2[1] = 1;
            numArray2[2] = 1;
            int[] inTime = numArray2;
            if (((this.cpHourVal.Text.Length == 0) | (this.cpMinuteVal.Text.Length == 0)) | (this.cpSecVal.Text.Length == 0))
            {
                this.cpHourVal.Text = "0";
                this.cpMinuteVal.Text = "0";
                this.cpSecVal.Text = "0";
            }
            if (((this.cpHourVal.Text.Contains(".") || this.cpHourVal.Text.Contains(",")) || (this.cpMinuteVal.Text.Contains(".") || this.cpMinuteVal.Text.Contains(","))) || (this.cpSecVal.Text.Contains(".") || this.cpSecVal.Text.Contains(",")))
            {
                MessageBox.Show("Invalid input for time\nInteger only", "ERROR!");
                return 1;
            }
            try
            {
                inTime[0] = Convert.ToInt32(this.cpHourVal.Text);
                inTime[1] = Convert.ToInt32(this.cpMinuteVal.Text);
                inTime[2] = Convert.ToInt32(this.cpSecVal.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
                return 1;
            }
            if (((inTime[0] == 0) && (inTime[1] == 0)) && (inTime[2] == 0))
            {
                if (this.cpCtrl.IsManual)
                {
                    MessageBox.Show("Invalid input for time", "ERROR!");
                    return 1;
                }
                return 1;
            }
            if (((inTime[0] < 0) || (inTime[1] < 0)) || (((inTime[1] > 0x3b) || (inTime[2] < 0)) || (inTime[2] > 0x3b)))
            {
                if (this.cpCtrl.IsManual)
                {
                    MessageBox.Show("Invalide time input", "ERROR!");
                    return 1;
                }
                return 1;
            }
            this.cpCtrl.SetTime(inTime, this.cpCtrl.CAPTURE);
            return 0;
        }

        private void cpUpdateProgress()
        {
            byte[] timeInfo = new byte[3];
            uint currPlayedBytes = 0;
            int currPlayedStatus = 0;
            lock (this.cpLockUpdate)
            {
                this.cpCtrl.UnregisterCallbacks(this.cpCtrl.CAPTURE);
                this.elapsedT[0] = 0;
                this.elapsedT[1] = 0;
                this.elapsedT[2] = 0;
                this.currPlayedBytes = 0;
                this.currPlayedStatus = 1;
            }
            PROGRESSCALLBACK pFnCallBack = new PROGRESSCALLBACK(this.cpDisplayProgress);
            fnRegisterProgressCallback(pFnCallBack, this.cpCtrl.CAPTURE);
            DISKFILLEDCALLBACK diskfilledcallback = new DISKFILLEDCALLBACK(this.cpDiskFilled);
            fnRegisterDiskSpaceExceededCallback(diskfilledcallback);
            ERRORCALLBACK pFnErrorCallBack = new ERRORCALLBACK(this.cpErrorLog);
            fnRegisterErrorCallback(pFnErrorCallBack);
            ERRORCALLBACK pFnExacqErrorCallBack = new ERRORCALLBACK(this.cpExacqErrorLog);
            fnRegisterExacqErrorCallback(pFnExacqErrorCallBack);
            GCHandle handle = GCHandle.Alloc(pFnCallBack);
            GCHandle handle2 = GCHandle.Alloc(diskfilledcallback);
            GCHandle handle3 = GCHandle.Alloc(pFnErrorCallBack);
            GCHandle handle4 = GCHandle.Alloc(pFnExacqErrorCallBack);
            while (true)
            {
                lock (this.cpLockUpdate)
                {
                    timeInfo[0] = this.elapsedT[0];
                    timeInfo[1] = this.elapsedT[1];
                    timeInfo[2] = this.elapsedT[2];
                    currPlayedBytes = this.currPlayedBytes;
                    currPlayedStatus = this.currPlayedStatus;
                }
                string text = string.Format("{0}:{1}:{2}", Convert.ToString(timeInfo[0]), Convert.ToString(timeInfo[1]), Convert.ToString(timeInfo[2]));
                this.cpGuiCtrl.SetLabelText(this.cpElapsedTimeVal, text);
                this.cpGuiCtrl.SetLabelText(this.cpCaptureBytesVal, Convert.ToString(currPlayedBytes));
                if ((this.cpCtrl.TotalFileTime != 0) && (this.cpCtrl.GetTotalTimeInSec(timeInfo) > this.cpCtrl.TotalFileTime))
                {
                    this.cpCtrl.Stop(this.cpCtrl.CAPTURE);
                    currPlayedStatus = 5;
                }
                if ((this.cpCtrl.TotalPlaySize != 0) && (currPlayedBytes >= this.cpCtrl.TotalPlaySize))
                {
                    this.cpCtrl.Stop(this.cpCtrl.CAPTURE);
                    currPlayedStatus = 5;
                }
                if ((currPlayedStatus == 5) | (currPlayedStatus == 3))
                {
                    this.cpReInitWindow();
                    this.cpCtrl.TotalPlaySize = 0;
                    this.cpCtrl.TotalFileTime = 0;
                    this.cpCtrl.IsFinished = true;
                    break;
                }
                this.cpGuiCtrl.SetButtonState(this.cpStartBut, false);
                this.cpCtrl.IsFinished = false;
                Thread.Sleep(500);
            }
            handle.Free();
            handle2.Free();
            handle3.Free();
            handle4.Free();
            new Thread(new ThreadStart(this.cpDoneEvent.SiRFLiveEventSet)).Start();
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
        public static extern int fnRegisterDiskSpaceExceededCallback(DISKFILLEDCALLBACK pFnCallBack);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRegisterErrorCallback(ERRORCALLBACK pFnErrorCallBack);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRegisterExacqErrorCallback(ERRORCALLBACK pFnExacqErrorCallBack);
        [DllImport("RFReplayDLL.dll")]
        public static extern int fnRegisterProgressCallback(PROGRESSCALLBACK pFnCallBack, int type);
        public static frmRFCaptureCtrl GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmRFCaptureCtrl();
            }
            return m_SChildform;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmRFCaptureCtrl));
            this.cpStopBut = new Button();
            this.cpStartBut = new Button();
            this.cpManualStopChk = new CheckBox();
            this.cpSizeVal = new TextBox();
            this.cpSizeChk = new CheckBox();
            this.cLabel2 = new Label();
            this.cpSecVal = new TextBox();
            this.clLabel1 = new Label();
            this.cpMinuteVal = new TextBox();
            this.cpHourVal = new TextBox();
            this.cpTimeChk = new CheckBox();
            this.cpFileBrowser = new Button();
            this.cpFilePathLabel = new Label();
            this.cpFilePathVal = new TextBox();
            this.cpCtrlBox = new GroupBox();
            this.cpStatusBox = new GroupBox();
            this.cpElapsedTimeVal = new Label();
            this.cpCaptureBytesVal = new Label();
            this.cpDoneVal = new CheckBox();
            this.cpElapsedTimeLabel = new Label();
            this.cpCaptureBytesLabel = new Label();
            this.cpCtrlBox.SuspendLayout();
            this.cpStatusBox.SuspendLayout();
            base.SuspendLayout();
            this.cpStopBut.Location = new Point(0x15c, 0x43);
            this.cpStopBut.Name = "cpStopBut";
            this.cpStopBut.Size = new Size(0x4b, 0x17);
            this.cpStopBut.TabIndex = 0x12;
            this.cpStopBut.Text = "Stop";
            this.cpStopBut.UseVisualStyleBackColor = true;
            this.cpStopBut.Click += new EventHandler(this.cpStopBut_Click);
            this.cpStartBut.Location = new Point(0x15c, 0x18);
            this.cpStartBut.Name = "cpStartBut";
            this.cpStartBut.Size = new Size(0x4b, 0x17);
            this.cpStartBut.TabIndex = 0x11;
            this.cpStartBut.Text = "&Start";
            this.cpStartBut.UseVisualStyleBackColor = true;
            this.cpStartBut.Click += new EventHandler(this.cpStartBut_Click);
            this.cpManualStopChk.AutoSize = true;
            this.cpManualStopChk.Location = new Point(14, 0x69);
            this.cpManualStopChk.Name = "cpManualStopChk";
            this.cpManualStopChk.Size = new Size(0x56, 0x11);
            this.cpManualStopChk.TabIndex = 11;
            this.cpManualStopChk.Text = "Manual Stop";
            this.cpManualStopChk.UseVisualStyleBackColor = true;
            this.cpManualStopChk.CheckedChanged += new EventHandler(this.cpManualStopChk_CheckedChanged);
            this.cpSizeVal.Location = new Point(0x6d, 70);
            this.cpSizeVal.Name = "cpSizeVal";
            this.cpSizeVal.Size = new Size(0x6d, 20);
            this.cpSizeVal.TabIndex = 10;
            this.cpSizeVal.Text = "10";
            this.cpSizeChk.AutoSize = true;
            this.cpSizeChk.Location = new Point(14, 70);
            this.cpSizeChk.Name = "cpSizeChk";
            this.cpSizeChk.Size = new Size(0x5c, 0x11);
            this.cpSizeChk.TabIndex = 9;
            this.cpSizeChk.Text = "Capture Up to";
            this.cpSizeChk.UseVisualStyleBackColor = true;
            this.cpSizeChk.CheckedChanged += new EventHandler(this.cpSizeChk_CheckedChanged);
            this.cLabel2.AutoSize = true;
            this.cLabel2.Location = new Point(0xb3, 0x1f);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new Size(10, 13);
            this.cLabel2.TabIndex = 7;
            this.cLabel2.Text = ":";
            this.cpSecVal.Location = new Point(0xbf, 0x1b);
            this.cpSecVal.Name = "cpSecVal";
            this.cpSecVal.Size = new Size(0x1b, 20);
            this.cpSecVal.TabIndex = 8;
            this.clLabel1.AutoSize = true;
            this.clLabel1.Location = new Point(0x8a, 0x1f);
            this.clLabel1.Name = "clLabel1";
            this.clLabel1.Size = new Size(10, 13);
            this.clLabel1.TabIndex = 5;
            this.clLabel1.Text = ":";
            this.cpMinuteVal.Location = new Point(150, 0x1b);
            this.cpMinuteVal.Name = "cpMinuteVal";
            this.cpMinuteVal.Size = new Size(0x1b, 20);
            this.cpMinuteVal.TabIndex = 6;
            this.cpHourVal.Location = new Point(0x6d, 0x1b);
            this.cpHourVal.Name = "cpHourVal";
            this.cpHourVal.Size = new Size(0x1b, 20);
            this.cpHourVal.TabIndex = 4;
            this.cpTimeChk.AutoSize = true;
            this.cpTimeChk.Location = new Point(14, 0x1f);
            this.cpTimeChk.Name = "cpTimeChk";
            this.cpTimeChk.Size = new Size(0x51, 0x11);
            this.cpTimeChk.TabIndex = 3;
            this.cpTimeChk.Text = "Capture For";
            this.cpTimeChk.UseVisualStyleBackColor = true;
            this.cpTimeChk.CheckedChanged += new EventHandler(this.cpTimeChk_CheckedChanged);
            this.cpFileBrowser.Location = new Point(0x1c6, 0x30);
            this.cpFileBrowser.Name = "cpFileBrowser";
            this.cpFileBrowser.Size = new Size(0x1a, 0x17);
            this.cpFileBrowser.TabIndex = 2;
            this.cpFileBrowser.Text = "...";
            this.cpFileBrowser.UseVisualStyleBackColor = true;
            this.cpFileBrowser.Click += new EventHandler(this.cpFileBrowser_Click);
            this.cpFilePathLabel.AutoSize = true;
            this.cpFilePathLabel.Location = new Point(0x16, 30);
            this.cpFilePathLabel.Name = "cpFilePathLabel";
            this.cpFilePathLabel.Size = new Size(0x58, 13);
            this.cpFilePathLabel.TabIndex = 0;
            this.cpFilePathLabel.Text = "Capture File Path";
            this.cpFilePathVal.Location = new Point(0x19, 0x33);
            this.cpFilePathVal.Name = "cpFilePathVal";
            this.cpFilePathVal.Size = new Size(0x1a7, 20);
            this.cpFilePathVal.TabIndex = 1;
            this.cpCtrlBox.Controls.Add(this.cpStopBut);
            this.cpCtrlBox.Controls.Add(this.cpTimeChk);
            this.cpCtrlBox.Controls.Add(this.cpStartBut);
            this.cpCtrlBox.Controls.Add(this.cpHourVal);
            this.cpCtrlBox.Controls.Add(this.cpManualStopChk);
            this.cpCtrlBox.Controls.Add(this.cpMinuteVal);
            this.cpCtrlBox.Controls.Add(this.cpSizeVal);
            this.cpCtrlBox.Controls.Add(this.clLabel1);
            this.cpCtrlBox.Controls.Add(this.cpSizeChk);
            this.cpCtrlBox.Controls.Add(this.cpSecVal);
            this.cpCtrlBox.Controls.Add(this.cLabel2);
            this.cpCtrlBox.Location = new Point(0x19, 0x5f);
            this.cpCtrlBox.Name = "cpCtrlBox";
            this.cpCtrlBox.Size = new Size(0x1c7, 0x8b);
            this.cpCtrlBox.TabIndex = 0x1b;
            this.cpCtrlBox.TabStop = false;
            this.cpCtrlBox.Text = "Control";
            this.cpStatusBox.Controls.Add(this.cpElapsedTimeVal);
            this.cpStatusBox.Controls.Add(this.cpCaptureBytesVal);
            this.cpStatusBox.Controls.Add(this.cpDoneVal);
            this.cpStatusBox.Controls.Add(this.cpElapsedTimeLabel);
            this.cpStatusBox.Controls.Add(this.cpCaptureBytesLabel);
            this.cpStatusBox.Location = new Point(0x19, 0xfb);
            this.cpStatusBox.Name = "cpStatusBox";
            this.cpStatusBox.Size = new Size(0x1c7, 0x61);
            this.cpStatusBox.TabIndex = 0x1c;
            this.cpStatusBox.TabStop = false;
            this.cpStatusBox.Text = "Status";
            this.cpElapsedTimeVal.AutoSize = true;
            this.cpElapsedTimeVal.Location = new Point(0x70, 60);
            this.cpElapsedTimeVal.Name = "cpElapsedTimeVal";
            this.cpElapsedTimeVal.Size = new Size(0x1f, 13);
            this.cpElapsedTimeVal.TabIndex = 15;
            this.cpElapsedTimeVal.Text = "0:0:0";
            this.cpCaptureBytesVal.AutoSize = true;
            this.cpCaptureBytesVal.Location = new Point(0x6d, 0x1f);
            this.cpCaptureBytesVal.Name = "cpCaptureBytesVal";
            this.cpCaptureBytesVal.Size = new Size(13, 13);
            this.cpCaptureBytesVal.TabIndex = 13;
            this.cpCaptureBytesVal.Text = "0";
            this.cpDoneVal.AutoSize = true;
            this.cpDoneVal.Location = new Point(0x15c, 0x1d);
            this.cpDoneVal.Name = "cpDoneVal";
            this.cpDoneVal.Size = new Size(0x3a, 0x11);
            this.cpDoneVal.TabIndex = 0x10;
            this.cpDoneVal.Text = "Done?";
            this.cpDoneVal.UseVisualStyleBackColor = true;
            this.cpElapsedTimeLabel.AutoSize = true;
            this.cpElapsedTimeLabel.Location = new Point(14, 60);
            this.cpElapsedTimeLabel.Name = "cpElapsedTimeLabel";
            this.cpElapsedTimeLabel.Size = new Size(0x4a, 13);
            this.cpElapsedTimeLabel.TabIndex = 14;
            this.cpElapsedTimeLabel.Text = "Elapsed Time:";
            this.cpCaptureBytesLabel.AutoSize = true;
            this.cpCaptureBytesLabel.Location = new Point(14, 0x1f);
            this.cpCaptureBytesLabel.Name = "cpCaptureBytesLabel";
            this.cpCaptureBytesLabel.Size = new Size(0x4c, 13);
            this.cpCaptureBytesLabel.TabIndex = 0x79;
            this.cpCaptureBytesLabel.Text = "Capture Bytes:";
            base.AcceptButton = this.cpStartBut;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.ClientSize = new Size(0x1f7, 0x17a);
            base.Controls.Add(this.cpStatusBox);
            base.Controls.Add(this.cpCtrlBox);
            base.Controls.Add(this.cpFileBrowser);
            base.Controls.Add(this.cpFilePathLabel);
            base.Controls.Add(this.cpFilePathVal);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmRFCaptureCtrl";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "RF Capture Control";
            this.cpCtrlBox.ResumeLayout(false);
            this.cpCtrlBox.PerformLayout();
            this.cpStatusBox.ResumeLayout(false);
            this.cpStatusBox.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosed(EventArgs e)
        {
            m_SChildform = null;
        }

        public void updateCpManualStopCheck()
        {
            this.cpGuiCtrl.SetCheckBoxState(this.cpManualStopChk, false);
            this.cpGuiCtrl.SetCheckBoxChecked(this.cpManualStopChk, true);
        }

        public void updatePlayFileText(string fileToPlay)
        {
            this.cpGuiCtrl.SetTextBoxText(this.cpFilePathVal, fileToPlay);
        }

        public void updatePlayForTime(byte hr, byte minute, byte sec)
        {
            byte[] timeInfo = new byte[3];
            timeInfo[0] = hr;
            timeInfo[1] = minute;
            timeInfo[2] = sec;
            this.cpCtrl.TotalFileTime = this.cpCtrl.GetTotalTimeInSec(timeInfo);
            this.cpGuiCtrl.SetTextBoxText(this.cpHourVal, Convert.ToString(hr));
            this.cpGuiCtrl.SetTextBoxText(this.cpMinuteVal, Convert.ToString(minute));
            this.cpGuiCtrl.SetTextBoxText(this.cpSecVal, Convert.ToString(sec));
            this.cpGuiCtrl.SetCheckBoxState(this.cpManualStopChk, false);
            this.cpGuiCtrl.SetCheckBoxState(this.cpSizeChk, false);
            this.cpGuiCtrl.SetCheckBoxState(this.cpTimeChk, false);
            this.cpGuiCtrl.SetCheckBoxChecked(this.cpManualStopChk, false);
            this.cpGuiCtrl.SetCheckBoxChecked(this.cpSizeChk, false);
            this.cpGuiCtrl.SetCheckBoxChecked(this.cpTimeChk, true);
        }

        public void updatePlaySize(uint size)
        {
            this.cpGuiCtrl.SetTextBoxText(this.cpSizeVal, Convert.ToString(size));
            this.cpGuiCtrl.SetCheckBoxState(this.cpManualStopChk, false);
            this.cpGuiCtrl.SetCheckBoxState(this.cpSizeChk, false);
            this.cpGuiCtrl.SetCheckBoxState(this.cpTimeChk, false);
            this.cpGuiCtrl.SetCheckBoxChecked(this.cpManualStopChk, false);
            this.cpGuiCtrl.SetCheckBoxChecked(this.cpSizeChk, true);
            this.cpGuiCtrl.SetCheckBoxChecked(this.cpTimeChk, false);
            this.cpGuiCtrl.SetTextBoxState(this.cpSizeVal, false);
        }
    }
}

