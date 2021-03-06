﻿namespace SiRFLive.GUI.Commmunication
{
    using CommonClassLibrary;
    using CommonUtilsClassLibrary;
    using SiRFLive.Analysis;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.GUI;
    using SiRFLive.MessageHandling;
    using SiRFLive.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;

    public class frmFileReplay : Form
    {
        private int _count;
        private BackgroundWorker _displayDataBG;
        private int _epochIndex;
        private List<long> _epochList = new List<long>();
        private LargeFileHandler _fileHdlr;
        private long _fileIndex;
        private CommonClass.TransmissionType _fileType = CommonClass.TransmissionType.GP2;
        internal frmInterferenceReport _interferenceReport;
        private bool _isMatchRegularExpression;
        private _playStates _lastPlayState;
        internal frmCommLocationMap _locationViewPanel;
        private string _logFileName = string.Empty;
        internal frmCommMessageFilter _messageFilter;
        internal Thread _parseThread;
        private string _persistedWindowName = "File Replay Window";
        private _playStates _playState;
        private FormWindowState _previousWindowState;
        private string _processFileLog = string.Empty;
        private string _regMatchString = string.Empty;
        internal frmCommRadarMap _signalMapPanel;
        internal frmCommSignalView _signalStrengthPanel;
        private long _totalFileSize;
        internal frmTTFFDisplay _ttffDisplay;
        private CommonClass.TransmissionType _viewType = CommonClass.TransmissionType.GP2;
        private ToolStripMenuItem aSCIIToolStripMenuItem;
        private Button btn_logFileBroswer;
        private ComboBox cboProtocols;
        private CommonClass CC = new CommonClass();
        private CheckBox chkboxSLC;
        private DataGridViewTextBoxColumn Column1;
        public CommunicationManager comm = new CommunicationManager();
        private IContainer components;
        private ToolStripMenuItem cSVToolStripMenuItem;
        private CommonUtilsClass CUC = new CommonUtilsClass();
        private ToolStripDropDownButton frmCommOpenToolLog;
        private Label frmFileplaybackDelayLabel;
        private TextBox frmFileplaybackDelayTextBox;
        private ToolStripMenuItem frmFileReplayCloseToolStripMenuItem;
        private ToolStripMenuItem frmFileReplayFileToolStripMenuItem;
        private Label frmFileReplayMarkerLabel;
        private CheckBox frmFileReplayMatchChkBox;
        private Label frmFileReplayOccurrence;
        private ToolStripMenuItem frmFileReplayOpenToolStripMenuItem;
        private Label frmFileReplayProtocolsLabel;
        private TextBox frmFileReplayRegExpressionTxtBox;
        private ToolStripMenuItem frmFileReplaySaveToolStripMenuItem;
        private ToolStripButton frmFileReplayToolBackward;
        private ToolStripDropDownButton frmFileReplayToolFilter;
        private ToolStripButton frmFileReplayToolForward;
        private ToolStripButton frmFileReplayToolPause;
        private ToolStripButton frmFileReplayToolPlay;
        private ToolStripDropDownButton frmFileReplayToolSignalView;
        private ToolStripButton frmFileReplayToolStop;
        private TrackBar frmFileReplayTrackBar;
        private ToolStripMenuItem gP2ToolStripMenuItem;
        private GroupBox GroupBox1;
        private ToolStripMenuItem hexToolStripMenuItem;
        private ToolStripMenuItem interferenceReportToolStripMenuItem;
        private Label label1;
        private ToolStripMenuItem locationViewToolStripMenuItem;
        private TextBox logFileNameTextBox;
        private Label logStatusLabel;
        private static frmFileReplay m_SChildform;
        private TextBox markerText;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem messagesToolStripMenuItem;
        private ToolStripMenuItem replayFileViewToolStripMenuItem;
        private CommonClass.MyRichTextBox rtbDisplay;
        private ToolStripMenuItem satelliteMapToolStripMenuItem;
        private Button setRefLocationBtn;
        private ToolStripMenuItem signalViewToolStripMenuItem;
        private int speedDelay = 0x19;
        private ToolStripMenuItem sSBToolStripMenuItem;
        private ToolStripMenuItem startLogToolStripMenuItem;
        private ToolStripMenuItem stopLogToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolTip toolTip1;
        private ToolTip toolTip2;
        private ToolTip toolTip3;
        private ToolTip toolTip4;
        private ToolStripMenuItem tTFFToolStripMenuItem;
        private ToolStripMenuItem viewAllToolStripMenuItem;
        private ToolStripMenuItem viewModeToolStripMenuItem;

        public frmFileReplay()
        {
            this.InitializeComponent();
            this.CUC.DisplayWindow = this.rtbDisplay;
            this.frmFileReplayTrackBar.Value = 0;
            this.frmFileReplayTrackBar.Maximum = 100;
            this.frmFileReplayTrackBar.Minimum = 0;
            this.frmFileReplayTrackBar.LargeChange = 5;
            this.frmFileReplayTrackBar.SmallChange = 1;
            this.frmFileReplayTrackBar.TickFrequency = 10;
            this.LoadProtocolList();
            this.cboProtocols.SelectedItem = "SSB";
            this.SetViewTypeAndUpdateMenuCheck(CommonClass.TransmissionType.GP2);
            this.frmFileplaybackDelayTextBox.Text = this.speedDelay.ToString();
            this._parseThread = new Thread(new ThreadStart(this.parseFile));
        }

        private void aSCIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetViewTypeAndUpdateMenuCheck(CommonClass.TransmissionType.Text);
        }

        private void btn_logFileBroswer_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Specify log file name:";
            dialog.InitialDirectory = @"..\..\logs\";
            dialog.Filter = "(*.txt)|*.txt|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.CheckPathExists = false;
            dialog.CheckFileExists = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.logFileNameTextBox.Text = dialog.FileName;
            }
        }

        private void cboProtocols_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comm.MessageProtocol = this.cboProtocols.SelectedItem.ToString();
            if (this.comm.MessageProtocol == "OSP")
            {
                this.chkboxSLC.Checked = true;
                this.chkboxSLC.Enabled = false;
            }
            else
            {
                this.chkboxSLC.Checked = false;
                this.chkboxSLC.Enabled = true;
            }
            if (this.comm.RxCtrl != null)
            {
                this.comm.RxCtrl.MessageProtocol = this.comm.MessageProtocol;
            }
            this.comm.File_ResetProtocol();
        }

        private void chkboxSLC_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkboxSLC.Checked)
            {
                this.comm.RxType = CommunicationManager.ReceiverType.SLC;
            }
            else
            {
                this.comm.RxType = CommunicationManager.ReceiverType.GSW;
            }
        }

        public frmTTFFDisplay CreateTTFFWin()
        {
            EventHandler method = null;
            if (!base.IsDisposed)
            {
                if (this.comm != null)
                {
                    if (method == null)
                    {
                        method = delegate {
                            string str = "File replay: TTFF/Nav Accuracy";
                            if ((this._ttffDisplay == null) || this._ttffDisplay.IsDisposed)
                            {
                                this._ttffDisplay = new frmTTFFDisplay(this.comm);
                                this._ttffDisplay.Show();
                            }
                            this._ttffDisplay.Text = str;
                            this._ttffDisplay.BringToFront();
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    MessageBox.Show("COM window not initialized!", "Information");
                }
            }
            return this._ttffDisplay;
        }

        private void cSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetViewTypeAndUpdateMenuCheck(CommonClass.TransmissionType.GPS);
        }

        private void displayBufferSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmCommDisplayBufferSizeUpdate(this.comm).ShowDialog();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmFileplaybackDelayTextBox_MouseEnter(object sender, EventArgs e)
        {
            string text = "Enter value and hit Enter key for change to take effect";
            this.toolTip2.Show(text, this.frmFileplaybackDelayTextBox, 0x7530);
        }

        private void frmFileplaybackDelayTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                if (this.frmFileplaybackDelayTextBox.Text.Length == 0)
                {
                    this.speedDelay = 1;
                }
                try
                {
                    int num = Convert.ToInt32(this.frmFileplaybackDelayTextBox.Text);
                    if (num <= 0)
                    {
                        num = 1;
                    }
                    this.speedDelay = num;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Delay setting error: \n" + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void frmFileReplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._playState = _playStates.QUIT;
        }

        private void frmFileReplayCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._playState = _playStates.IDLE;
            if (this._fileHdlr != null)
            {
                this._fileHdlr.Close();
            }
            this._fileHdlr = null;
            this._logFileName = string.Empty;
            this.Text = "Idle";
            this.frmFileReplayOpenToolStripMenuItem.Enabled = true;
        }

        private void frmFileReplayMatchChkBox_CheckedChanged(object sender, EventArgs e)
        {
            this._isMatchRegularExpression = this.frmFileReplayMatchChkBox.Checked;
            this._count = 0;
            this.frmFileReplayOccurrence.Text = "Match: " + this._count.ToString();
        }

        private void frmFileReplayOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Specify log file name:";
            dialog.InitialDirectory = @"..\..\logs\";
            dialog.Filter = "GP2 (*.gp2)|*.gp2|GPS (*.gps)|*.gps|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.CheckPathExists = false;
            dialog.CheckFileExists = false;
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            this._logFileName = dialog.FileName;
            this.Text = "Open: " + this._logFileName;
            this._fileHdlr = new LargeFileHandler(this._logFileName);
            this._totalFileSize = this._fileHdlr.Length;
            this.frmFileReplayTrackBar.Value = 0;
            this.frmFileReplayTrackBar.Maximum = 100;
            this.frmFileReplayTrackBar.Minimum = 0;
            this._playState = _playStates.IDLE;
            this._lastPlayState = this._playState;
            this.comm.MessageProtocol = this.cboProtocols.SelectedItem.ToString();
            string[] strArray = this._logFileName.Split(new char[] { '.' });
            if (strArray.Length != 2)
            {
                return;
            }
            string str = strArray[1].ToUpper();
            if (str != null)
            {
                if (!(str == "GP2") && !(str == "GPX"))
                {
                    if (str == "GPS")
                    {
                        this._viewType = CommonClass.TransmissionType.GPS;
                        this._fileType = CommonClass.TransmissionType.GPS;
                        goto Label_01A1;
                    }
                    if (str == "TXT")
                    {
                        this._viewType = CommonClass.TransmissionType.Text;
                        this._fileType = CommonClass.TransmissionType.Text;
                        goto Label_01A1;
                    }
                    if (str == "BIN")
                    {
                        this._viewType = CommonClass.TransmissionType.Hex;
                        this._fileType = CommonClass.TransmissionType.Hex;
                        goto Label_01A1;
                    }
                }
                else
                {
                    this._viewType = CommonClass.TransmissionType.GP2;
                    this._fileType = CommonClass.TransmissionType.GP2;
                    goto Label_01A1;
                }
            }
            this._viewType = CommonClass.TransmissionType.Hex;
            this._fileType = CommonClass.TransmissionType.Hex;
        Label_01A1:
            this._processFileLog = strArray[0] + ".par";
            this.frmFileReplayOpenToolStripMenuItem.Enabled = false;
        }

        private void frmFileReplayRegExpressionTxtBox_MouseEnter(object sender, EventArgs e)
        {
            string text = "Check the \"Match Regular Expression\" box. Enter text to match and hit enter key to activate matching";
            this.toolTip1.Show(text, this.frmFileReplayRegExpressionTxtBox, 0x7530);
        }

        private void frmFileReplayRegExpressionTxtBox_TextChanged(object sender, EventArgs e)
        {
            this._regMatchString = this.frmFileReplayRegExpressionTxtBox.Text;
        }

        private void frmFileReplaySaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void frmFileReplayToolBackward_Click(object sender, EventArgs e)
        {
            this._lastPlayState = this._playState;
            this._playState = _playStates.BACKWARD;
            if (this._playState != this._lastPlayState)
            {
                this.comm.WriteApp("User marker: Go backward to last epoch");
            }
            this.Text = "Backward: " + this._logFileName;
            this.frmFileReplayTrackBar.Enabled = false;
        }

        private void frmFileReplayToolFilter_Click(object sender, EventArgs e)
        {
            if (!base.IsDisposed)
            {
                string str = "File Playback Message Filter";
                if ((this._messageFilter == null) || this._messageFilter.IsDisposed)
                {
                    this._messageFilter = new frmCommMessageFilter();
                }
                this._messageFilter.CommWindow = this.comm;
                this._messageFilter.Text = str;
                this._messageFilter.Show();
                this._messageFilter.BringToFront();
            }
            else
            {
                MessageBox.Show("File Playback not initialized", "Information");
            }
        }

        private void frmFileReplayToolForward_Click(object sender, EventArgs e)
        {
            this._lastPlayState = this._playState;
            this._playState = _playStates.FORWARD;
            if (this._playState != this._lastPlayState)
            {
                this.comm.WriteApp("User marker: Go forward to next epoch");
            }
            this.Text = "Forward: " + this._logFileName;
            this.frmFileReplayTrackBar.Enabled = false;
        }

        private void frmFileReplayToolPause_Click(object sender, EventArgs e)
        {
            this._lastPlayState = this._playState;
            this._playState = _playStates.PAUSE;
            this.Text = "Pause: " + this._logFileName;
            this.frmFileReplayTrackBar.Enabled = true;
        }

        private void frmFileReplayToolPlay_Click(object sender, EventArgs e)
        {
            if (this._logFileName != string.Empty)
            {
                this._lastPlayState = this._playState;
                this._playState = _playStates.PLAY;
                this.Text = "Play: " + this._logFileName;
                if (this._lastPlayState == _playStates.IDLE)
                {
                    this.comm.WriteApp("User marker: Start playing " + this._logFileName);
                }
                this.frmFileReplayTrackBar.Enabled = false;
            }
            else
            {
                MessageBox.Show("No file open", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void frmFileReplayToolSave_Click(object sender, EventArgs e)
        {
        }

        private void frmFileReplayToolStop_Click(object sender, EventArgs e)
        {
            this._playState = _playStates.IDLE;
            this.comm.Log.CloseFile();
            this.comm.File_ResetProtocol();
            this.Text = "Stop: " + this._logFileName;
            this.frmFileReplayTrackBar.Enabled = true;
        }

        private void frmFileReplayTrackBar_MouseHover(object sender, EventArgs e)
        {
            string text = "Drag to change file location";
            this.toolTip3.Show(text, this.frmFileReplayTrackBar, 0x7530);
        }

        private void frmFileReplayTrackBar_Scroll(object sender, EventArgs e)
        {
            if ((this._playState == _playStates.PAUSE) || (this._playState == _playStates.IDLE))
            {
                this.comm.WriteApp("User marker: user changes file position");
                this._fileIndex = (long) ((this.frmFileReplayTrackBar.Value * this._totalFileSize) / 100.0);
                this._epochList.Clear();
                this._epochIndex = 0;
            }
        }

        public static frmFileReplay GetChildInstance()
        {
            if (m_SChildform == null)
            {
                m_SChildform = new frmFileReplay();
            }
            return m_SChildform;
        }

        private void getOSPPosition(string line)
        {
            string[] strArray = line.Split(new char[] { ',' });
            int num = 0;
            int num2 = 0;
            this.comm.RxCtrl.RxNavData.Nav2DPositionError = -9999.0;
            this.comm.RxCtrl.RxNavData.Nav3DPositionError = -9999.0;
            this.comm.RxCtrl.RxNavData.NavVerticalPositionError = -9999.0;
            try
            {
                Convert.ToInt32(strArray[2]);
                num = Convert.ToInt32(strArray[3]);
                num2 = Convert.ToInt32(strArray[4]);
            }
            catch
            {
            }
            if (num == 0)
            {
                this.comm.RxCtrl.ResetCtrl.ResetPositionAvailable = false;
            }
            else if (num2 == 0)
            {
                double lat = 0.0;
                double lon = 0.0;
                int num5 = 0;
                double alt = 0.0;
                int num7 = 0;
                int num8 = 0;
                try
                {
                    Convert.ToInt32(strArray[5]);
                    Convert.ToInt32(strArray[6]);
                    Convert.ToInt32(strArray[7]);
                    Convert.ToInt32(strArray[8]);
                    double num1 = Convert.ToDouble(strArray[9]) / 1000.0;
                    lat = (Convert.ToDouble(strArray[10]) * 180.0) / 4294967296;
                    lon = (Convert.ToDouble(strArray[11]) * 360.0) / 4294967296;
                    num5 = Convert.ToInt32(strArray[12]);
                    double num14 = (Convert.ToDouble(strArray[13]) * 180.0) / 256.0;
                    alt = (Convert.ToDouble(strArray[0x10]) * 0.1) - 500.0;
                    Convert.ToDouble(strArray[0x12]);
                    double num15 = (Convert.ToDouble(strArray[0x13]) * 360.0) / 65536.0;
                    Convert.ToDouble(strArray[20]);
                    Convert.ToDouble(strArray[0x15]);
                    Convert.ToDouble(strArray[0x16]);
                    Convert.ToDouble(strArray[0x17]);
                    Convert.ToDouble(strArray[0x18]);
                    Convert.ToInt32(strArray[0x19]);
                    Convert.ToUInt16(strArray[0x1a]);
                    Convert.ToDouble(strArray[0x1c]);
                    Convert.ToInt32(strArray[0x1d]);
                    num7 = Convert.ToInt32(strArray[30]);
                    int[] numArray = new int[num7];
                    int[] numArray2 = new int[num7];
                    int[] numArray3 = new int[num7];
                    num8 = num5 & 2;
                    int index = 0;
                    int num10 = 0x1f;
                    while (index < num7)
                    {
                        numArray[index] = Convert.ToInt32(strArray[num10++]);
                        numArray2[index] = Convert.ToInt32(strArray[num10++]);
                        numArray3[index] = Convert.ToInt32(strArray[num10++]);
                        index++;
                    }
                    if (num8 != 2)
                    {
                        alt = -9999.0;
                    }
                }
                catch
                {
                }
                PositionErrorCalc calc = new PositionErrorCalc();
                double num11 = Convert.ToDouble(this.comm.RxCtrl.RxNavData.RefLat);
                double num12 = Convert.ToDouble(this.comm.RxCtrl.RxNavData.RefLon);
                double num13 = Convert.ToDouble(this.comm.RxCtrl.RxNavData.RefAlt);
                calc.GetPositionErrorsInMeter(lat, lon, alt, num11, num12, num13);
                this.comm.RxCtrl.RxNavData.Nav2DPositionError = calc.HorizontalError;
                if (num8 == 2)
                {
                    this.comm.RxCtrl.RxNavData.Nav3DPositionError = calc.Position3DError;
                    this.comm.RxCtrl.RxNavData.NavVerticalPositionError = calc.VerticalErrorInMeter;
                }
                else
                {
                    this.comm.RxCtrl.RxNavData.Nav3DPositionError = -9999.0;
                    this.comm.RxCtrl.RxNavData.NavVerticalPositionError = -9999.0;
                }
                this.comm.RxCtrl.RxNavData.MeasLat = lat;
                this.comm.RxCtrl.RxNavData.MeasLon = lon;
                this.comm.RxCtrl.RxNavData.MeasAlt = alt;
                this.comm.RxCtrl.ResetCtrl.ResetPositionAvailable = true;
            }
        }

        private void GetTTFFFromCSV(string line)
        {
            string[] strArray = line.Split(new char[] { ',' });
            if (strArray.Length >= 0x15)
            {
                Hashtable msgH = new Hashtable();
                string[] strArray2 = new string[] { 
                    "Message ID", "Message Sub ID", "TTFF since reset", "TTFF since all aiding received", "TTFF first nav since reset", "Position Aiding Error North", "Position Aiding Error East", "Position Aiding Error Down", "Time Aiding Error", "Frequency Aiding Error", "Horizontal Position Uncertainty", "Vertical Position Uncertainty", "Time Uncertainty", "Frequency Uncertainty", "Number of Aided Ephemeris", "Number of Aided Acquisition Assistance", 
                    "Navigation Mode", "Position Mode", "Status", "Start Mode", "Reserved1"
                 };
                for (int i = 0; i < strArray2.Length; i++)
                {
                    msgH.Add(strArray2[i], strArray[i]);
                }
                this.comm.RxCtrl.GetTTFFFromHash(msgH);
            }
        }

        private void gP2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetViewTypeAndUpdateMenuCheck(CommonClass.TransmissionType.GP2);
        }

        private void hexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetViewTypeAndUpdateMenuCheck(CommonClass.TransmissionType.Hex);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmFileReplay));
            this.toolStrip1 = new ToolStrip();
            this.frmFileReplayToolPlay = new ToolStripButton();
            this.frmFileReplayToolBackward = new ToolStripButton();
            this.frmFileReplayToolPause = new ToolStripButton();
            this.frmFileReplayToolForward = new ToolStripButton();
            this.frmFileReplayToolStop = new ToolStripButton();
            this.frmCommOpenToolLog = new ToolStripDropDownButton();
            this.startLogToolStripMenuItem = new ToolStripMenuItem();
            this.stopLogToolStripMenuItem = new ToolStripMenuItem();
            this.frmFileReplayToolSignalView = new ToolStripDropDownButton();
            this.signalViewToolStripMenuItem = new ToolStripMenuItem();
            this.satelliteMapToolStripMenuItem = new ToolStripMenuItem();
            this.locationViewToolStripMenuItem = new ToolStripMenuItem();
            this.interferenceReportToolStripMenuItem = new ToolStripMenuItem();
            this.tTFFToolStripMenuItem = new ToolStripMenuItem();
            this.frmFileReplayToolFilter = new ToolStripDropDownButton();
            this.GroupBox1 = new GroupBox();
            this.rtbDisplay = new CommonClass.MyRichTextBox();
            this.Column1 = new DataGridViewTextBoxColumn();
            this.markerText = new TextBox();
            this.chkboxSLC = new CheckBox();
            this.frmFileReplayProtocolsLabel = new Label();
            this.cboProtocols = new ComboBox();
            this.menuStrip1 = new MenuStrip();
            this.frmFileReplayFileToolStripMenuItem = new ToolStripMenuItem();
            this.frmFileReplayOpenToolStripMenuItem = new ToolStripMenuItem();
            this.frmFileReplaySaveToolStripMenuItem = new ToolStripMenuItem();
            this.frmFileReplayCloseToolStripMenuItem = new ToolStripMenuItem();
            this.replayFileViewToolStripMenuItem = new ToolStripMenuItem();
            this.viewModeToolStripMenuItem = new ToolStripMenuItem();
            this.hexToolStripMenuItem = new ToolStripMenuItem();
            this.aSCIIToolStripMenuItem = new ToolStripMenuItem();
            this.sSBToolStripMenuItem = new ToolStripMenuItem();
            this.gP2ToolStripMenuItem = new ToolStripMenuItem();
            this.cSVToolStripMenuItem = new ToolStripMenuItem();
            this.messagesToolStripMenuItem = new ToolStripMenuItem();
            this.viewAllToolStripMenuItem = new ToolStripMenuItem();
            this.frmFileReplayOccurrence = new Label();
            this.frmFileReplayRegExpressionTxtBox = new TextBox();
            this.frmFileReplayMatchChkBox = new CheckBox();
            this.frmFileReplayTrackBar = new TrackBar();
            this.frmFileReplayMarkerLabel = new Label();
            this.label1 = new Label();
            this.logFileNameTextBox = new TextBox();
            this.logStatusLabel = new Label();
            this.btn_logFileBroswer = new Button();
            this.frmFileplaybackDelayTextBox = new TextBox();
            this.frmFileplaybackDelayLabel = new Label();
            this._displayDataBG = new BackgroundWorker();
            this.setRefLocationBtn = new Button();
            this.toolTip1 = new ToolTip(this.components);
            this.toolTip2 = new ToolTip(this.components);
            this.toolTip3 = new ToolTip(this.components);
            this.toolTip4 = new ToolTip(this.components);
            this.toolStrip1.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.frmFileReplayTrackBar.BeginInit();
            base.SuspendLayout();
            this.toolStrip1.BackColor = SystemColors.MenuBar;
            this.toolStrip1.ImageScalingSize = new Size(0x12, 0x12);
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.frmFileReplayToolPlay, this.frmFileReplayToolBackward, this.frmFileReplayToolPause, this.frmFileReplayToolForward, this.frmFileReplayToolStop, this.frmCommOpenToolLog, this.frmFileReplayToolSignalView, this.frmFileReplayToolFilter });
            this.toolStrip1.Location = new Point(0, 0x18);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x2dc, 0x19);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0x12;
            this.toolStrip1.Text = "User Action";
            this.frmFileReplayToolPlay.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.frmFileReplayToolPlay.Image = (Image) resources.GetObject("frmFileReplayToolPlay.Image");
            this.frmFileReplayToolPlay.ImageTransparentColor = Color.Magenta;
            this.frmFileReplayToolPlay.Name = "frmFileReplayToolPlay";
            this.frmFileReplayToolPlay.Size = new Size(0x17, 0x16);
            this.frmFileReplayToolPlay.Text = "Play";
            this.frmFileReplayToolPlay.Click += new EventHandler(this.frmFileReplayToolPlay_Click);
            this.frmFileReplayToolBackward.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.frmFileReplayToolBackward.Image = (Image) resources.GetObject("frmFileReplayToolBackward.Image");
            this.frmFileReplayToolBackward.ImageTransparentColor = Color.Magenta;
            this.frmFileReplayToolBackward.Name = "frmFileReplayToolBackward";
            this.frmFileReplayToolBackward.Size = new Size(0x17, 0x16);
            this.frmFileReplayToolBackward.Text = "Backward";
            this.frmFileReplayToolBackward.Click += new EventHandler(this.frmFileReplayToolBackward_Click);
            this.frmFileReplayToolPause.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.frmFileReplayToolPause.Image = (Image) resources.GetObject("frmFileReplayToolPause.Image");
            this.frmFileReplayToolPause.ImageTransparentColor = Color.Magenta;
            this.frmFileReplayToolPause.Name = "frmFileReplayToolPause";
            this.frmFileReplayToolPause.Size = new Size(0x17, 0x16);
            this.frmFileReplayToolPause.Text = "Pause";
            this.frmFileReplayToolPause.Click += new EventHandler(this.frmFileReplayToolPause_Click);
            this.frmFileReplayToolForward.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.frmFileReplayToolForward.Image = (Image) resources.GetObject("frmFileReplayToolForward.Image");
            this.frmFileReplayToolForward.ImageTransparentColor = Color.Magenta;
            this.frmFileReplayToolForward.Name = "frmFileReplayToolForward";
            this.frmFileReplayToolForward.Size = new Size(0x17, 0x16);
            this.frmFileReplayToolForward.Text = "Forward";
            this.frmFileReplayToolForward.Click += new EventHandler(this.frmFileReplayToolForward_Click);
            this.frmFileReplayToolStop.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.frmFileReplayToolStop.Image = (Image) resources.GetObject("frmFileReplayToolStop.Image");
            this.frmFileReplayToolStop.ImageTransparentColor = Color.Magenta;
            this.frmFileReplayToolStop.Name = "frmFileReplayToolStop";
            this.frmFileReplayToolStop.Size = new Size(0x17, 0x16);
            this.frmFileReplayToolStop.Text = "Stop";
            this.frmFileReplayToolStop.Click += new EventHandler(this.frmFileReplayToolStop_Click);
            this.frmCommOpenToolLog.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.frmCommOpenToolLog.DropDownItems.AddRange(new ToolStripItem[] { this.startLogToolStripMenuItem, this.stopLogToolStripMenuItem });
            this.frmCommOpenToolLog.Image = (Image) resources.GetObject("frmCommOpenToolLog.Image");
            this.frmCommOpenToolLog.ImageTransparentColor = Color.Magenta;
            this.frmCommOpenToolLog.Name = "frmCommOpenToolLog";
            this.frmCommOpenToolLog.Size = new Size(0x1f, 0x16);
            this.frmCommOpenToolLog.Text = "Logging";
            this.startLogToolStripMenuItem.Name = "startLogToolStripMenuItem";
            this.startLogToolStripMenuItem.Size = new Size(0x81, 0x16);
            this.startLogToolStripMenuItem.Text = "Start Log";
            this.startLogToolStripMenuItem.Click += new EventHandler(this.startLogToolStripMenuItem_Click);
            this.stopLogToolStripMenuItem.Name = "stopLogToolStripMenuItem";
            this.stopLogToolStripMenuItem.Size = new Size(0x81, 0x16);
            this.stopLogToolStripMenuItem.Text = "Stop Log";
            this.stopLogToolStripMenuItem.Click += new EventHandler(this.stopLogToolStripMenuItem_Click);
            this.frmFileReplayToolSignalView.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.frmFileReplayToolSignalView.DropDownItems.AddRange(new ToolStripItem[] { this.signalViewToolStripMenuItem, this.satelliteMapToolStripMenuItem, this.locationViewToolStripMenuItem, this.interferenceReportToolStripMenuItem, this.tTFFToolStripMenuItem });
            this.frmFileReplayToolSignalView.Image = (Image) resources.GetObject("frmFileReplayToolSignalView.Image");
            this.frmFileReplayToolSignalView.ImageTransparentColor = Color.Magenta;
            this.frmFileReplayToolSignalView.Name = "frmFileReplayToolSignalView";
            this.frmFileReplayToolSignalView.Size = new Size(0x1f, 0x16);
            this.frmFileReplayToolSignalView.Text = "View";
            this.signalViewToolStripMenuItem.Name = "signalViewToolStripMenuItem";
            this.signalViewToolStripMenuItem.Size = new Size(0xc7, 0x16);
            this.signalViewToolStripMenuItem.Text = "Satellite Signal Strength";
            this.signalViewToolStripMenuItem.Click += new EventHandler(this.signalStrengthToolStripMenuItem_Click);
            this.satelliteMapToolStripMenuItem.Name = "satelliteMapToolStripMenuItem";
            this.satelliteMapToolStripMenuItem.Size = new Size(0xc7, 0x16);
            this.satelliteMapToolStripMenuItem.Text = "Satellite Map";
            this.satelliteMapToolStripMenuItem.Click += new EventHandler(this.satelliteMapToolStripMenuItem_Click);
            this.locationViewToolStripMenuItem.Name = "locationViewToolStripMenuItem";
            this.locationViewToolStripMenuItem.Size = new Size(0xc7, 0x16);
            this.locationViewToolStripMenuItem.Text = "Map";
            this.locationViewToolStripMenuItem.Click += new EventHandler(this.locationViewToolStripMenuItem_Click);
            this.interferenceReportToolStripMenuItem.Name = "interferenceReportToolStripMenuItem";
            this.interferenceReportToolStripMenuItem.Size = new Size(0xc7, 0x16);
            this.interferenceReportToolStripMenuItem.Text = "Interference Report";
            this.interferenceReportToolStripMenuItem.Click += new EventHandler(this.interferenceReportToolStripMenuItem_Click);
            this.tTFFToolStripMenuItem.Name = "tTFFToolStripMenuItem";
            this.tTFFToolStripMenuItem.Size = new Size(0xc7, 0x16);
            this.tTFFToolStripMenuItem.Text = "TTFF";
            this.tTFFToolStripMenuItem.Click += new EventHandler(this.tTFFToolStripMenuItem_Click);
            this.frmFileReplayToolFilter.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.frmFileReplayToolFilter.Image = (Image) resources.GetObject("frmFileReplayToolFilter.Image");
            this.frmFileReplayToolFilter.ImageTransparentColor = Color.Magenta;
            this.frmFileReplayToolFilter.Name = "frmFileReplayToolFilter";
            this.frmFileReplayToolFilter.Size = new Size(0x1f, 0x16);
            this.frmFileReplayToolFilter.Text = "Message Filter";
            this.frmFileReplayToolFilter.Click += new EventHandler(this.frmFileReplayToolFilter_Click);
            this.GroupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.GroupBox1.Controls.Add(this.rtbDisplay);
            this.GroupBox1.Location = new Point(14, 0xe9);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new Size(0x2b3, 0x171);
            this.GroupBox1.TabIndex = 0x13;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Replay File Data";
            this.rtbDisplay.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.rtbDisplay.BackColor = SystemColors.ControlLightLight;
            this.rtbDisplay.Location = new Point(12, 0x1b);
            this.rtbDisplay.Name = "rtbDisplay";
            this.rtbDisplay.ReadOnly = true;
            this.rtbDisplay.Size = new Size(0x296, 0x145);
            this.rtbDisplay.TabIndex = 9;
            this.rtbDisplay.Text = "";
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 0x1388;
            this.markerText.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.markerText.Location = new Point(0x58, 160);
            this.markerText.Name = "markerText";
            this.markerText.Size = new Size(0x269, 20);
            this.markerText.TabIndex = 7;
            this.markerText.PreviewKeyDown += new PreviewKeyDownEventHandler(this.markerText_PreviewKeyDown);
            this.markerText.MouseEnter += new EventHandler(this.markerText_MouseEnter);
            this.chkboxSLC.Location = new Point(14, 0x52);
            this.chkboxSLC.Name = "chkboxSLC";
            this.chkboxSLC.Size = new Size(0x33, 0x18);
            this.chkboxSLC.TabIndex = 0;
            this.chkboxSLC.Text = "SLC";
            this.chkboxSLC.CheckedChanged += new EventHandler(this.chkboxSLC_CheckedChanged);
            this.frmFileReplayProtocolsLabel.AutoSize = true;
            this.frmFileReplayProtocolsLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.frmFileReplayProtocolsLabel.Location = new Point(0x4b, 0x58);
            this.frmFileReplayProtocolsLabel.Name = "frmFileReplayProtocolsLabel";
            this.frmFileReplayProtocolsLabel.Size = new Size(0x33, 13);
            this.frmFileReplayProtocolsLabel.TabIndex = 1;
            this.frmFileReplayProtocolsLabel.Text = "Protocols";
            this.cboProtocols.FormattingEnabled = true;
            this.cboProtocols.Location = new Point(0x86, 0x54);
            this.cboProtocols.Name = "cboProtocols";
            this.cboProtocols.Size = new Size(0x54, 0x15);
            this.cboProtocols.TabIndex = 2;
            this.cboProtocols.SelectedIndexChanged += new EventHandler(this.cboProtocols_SelectedIndexChanged);
            this.menuStrip1.AllowMerge = false;
            this.menuStrip1.BackColor = SystemColors.Menu;
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.frmFileReplayFileToolStripMenuItem, this.replayFileViewToolStripMenuItem });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(0x2dc, 0x18);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            this.frmFileReplayFileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.frmFileReplayOpenToolStripMenuItem, this.frmFileReplaySaveToolStripMenuItem, this.frmFileReplayCloseToolStripMenuItem });
            this.frmFileReplayFileToolStripMenuItem.Name = "frmFileReplayFileToolStripMenuItem";
            this.frmFileReplayFileToolStripMenuItem.Size = new Size(0x47, 20);
            this.frmFileReplayFileToolStripMenuItem.Text = "Replay File";
            this.frmFileReplayOpenToolStripMenuItem.Name = "frmFileReplayOpenToolStripMenuItem";
            this.frmFileReplayOpenToolStripMenuItem.Size = new Size(0x6f, 0x16);
            this.frmFileReplayOpenToolStripMenuItem.Text = "Open";
            this.frmFileReplayOpenToolStripMenuItem.Click += new EventHandler(this.frmFileReplayOpenToolStripMenuItem_Click);
            this.frmFileReplaySaveToolStripMenuItem.Name = "frmFileReplaySaveToolStripMenuItem";
            this.frmFileReplaySaveToolStripMenuItem.Size = new Size(0x6f, 0x16);
            this.frmFileReplaySaveToolStripMenuItem.Text = "Save";
            this.frmFileReplaySaveToolStripMenuItem.Click += new EventHandler(this.frmFileReplaySaveToolStripMenuItem_Click);
            this.frmFileReplayCloseToolStripMenuItem.Name = "frmFileReplayCloseToolStripMenuItem";
            this.frmFileReplayCloseToolStripMenuItem.Size = new Size(0x6f, 0x16);
            this.frmFileReplayCloseToolStripMenuItem.Text = "Close";
            this.frmFileReplayCloseToolStripMenuItem.Click += new EventHandler(this.frmFileReplayCloseToolStripMenuItem_Click);
            this.replayFileViewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.viewModeToolStripMenuItem, this.messagesToolStripMenuItem });
            this.replayFileViewToolStripMenuItem.Name = "replayFileViewToolStripMenuItem";
            this.replayFileViewToolStripMenuItem.Size = new Size(0x4d, 20);
            this.replayFileViewToolStripMenuItem.Text = "Replay View";
            this.viewModeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.hexToolStripMenuItem, this.aSCIIToolStripMenuItem, this.sSBToolStripMenuItem, this.gP2ToolStripMenuItem, this.cSVToolStripMenuItem });
            this.viewModeToolStripMenuItem.Name = "viewModeToolStripMenuItem";
            this.viewModeToolStripMenuItem.Size = new Size(0x88, 0x16);
            this.viewModeToolStripMenuItem.Text = "View Mode";
            this.viewModeToolStripMenuItem.Click += new EventHandler(this.viewModeToolStripMenuItem_Click);
            this.hexToolStripMenuItem.Name = "hexToolStripMenuItem";
            this.hexToolStripMenuItem.Size = new Size(0x71, 0x16);
            this.hexToolStripMenuItem.Text = "HEX";
            this.hexToolStripMenuItem.Click += new EventHandler(this.hexToolStripMenuItem_Click);
            this.aSCIIToolStripMenuItem.Name = "aSCIIToolStripMenuItem";
            this.aSCIIToolStripMenuItem.Size = new Size(0x71, 0x16);
            this.aSCIIToolStripMenuItem.Text = "ASCII";
            this.aSCIIToolStripMenuItem.Click += new EventHandler(this.aSCIIToolStripMenuItem_Click);
            this.sSBToolStripMenuItem.Name = "sSBToolStripMenuItem";
            this.sSBToolStripMenuItem.Size = new Size(0x71, 0x16);
            this.sSBToolStripMenuItem.Text = "SSB";
            this.sSBToolStripMenuItem.Click += new EventHandler(this.sSBToolStripMenuItem_Click);
            this.gP2ToolStripMenuItem.Name = "gP2ToolStripMenuItem";
            this.gP2ToolStripMenuItem.Size = new Size(0x71, 0x16);
            this.gP2ToolStripMenuItem.Text = "GP2";
            this.gP2ToolStripMenuItem.Click += new EventHandler(this.gP2ToolStripMenuItem_Click);
            this.cSVToolStripMenuItem.Name = "cSVToolStripMenuItem";
            this.cSVToolStripMenuItem.Size = new Size(0x71, 0x16);
            this.cSVToolStripMenuItem.Text = "GPS";
            this.cSVToolStripMenuItem.Click += new EventHandler(this.cSVToolStripMenuItem_Click);
            this.messagesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.viewAllToolStripMenuItem });
            this.messagesToolStripMenuItem.Name = "messagesToolStripMenuItem";
            this.messagesToolStripMenuItem.Size = new Size(0x88, 0x16);
            this.messagesToolStripMenuItem.Text = "Messages";
            this.viewAllToolStripMenuItem.Name = "viewAllToolStripMenuItem";
            this.viewAllToolStripMenuItem.Size = new Size(0x92, 0x16);
            this.viewAllToolStripMenuItem.Text = "All Messages";
            this.viewAllToolStripMenuItem.Click += new EventHandler(this.viewAllToolStripMenuItem_Click);
            this.frmFileReplayOccurrence.AutoSize = true;
            this.frmFileReplayOccurrence.Location = new Point(0xc0, 0x72);
            this.frmFileReplayOccurrence.Name = "frmFileReplayOccurrence";
            this.frmFileReplayOccurrence.Size = new Size(0x2b, 13);
            this.frmFileReplayOccurrence.TabIndex = 4;
            this.frmFileReplayOccurrence.Text = "Match: ";
            this.frmFileReplayRegExpressionTxtBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.frmFileReplayRegExpressionTxtBox.Location = new Point(14, 0x87);
            this.frmFileReplayRegExpressionTxtBox.Name = "frmFileReplayRegExpressionTxtBox";
            this.frmFileReplayRegExpressionTxtBox.Size = new Size(0x2b3, 20);
            this.frmFileReplayRegExpressionTxtBox.TabIndex = 5;
            this.frmFileReplayRegExpressionTxtBox.PreviewKeyDown += new PreviewKeyDownEventHandler(this.markerText_PreviewKeyDown);
            this.frmFileReplayRegExpressionTxtBox.MouseEnter += new EventHandler(this.frmFileReplayRegExpressionTxtBox_MouseEnter);
            this.frmFileReplayRegExpressionTxtBox.TextChanged += new EventHandler(this.frmFileReplayRegExpressionTxtBox_TextChanged);
            this.frmFileReplayMatchChkBox.AutoSize = true;
            this.frmFileReplayMatchChkBox.Location = new Point(14, 0x70);
            this.frmFileReplayMatchChkBox.Name = "frmFileReplayMatchChkBox";
            this.frmFileReplayMatchChkBox.Size = new Size(0x9c, 0x11);
            this.frmFileReplayMatchChkBox.TabIndex = 3;
            this.frmFileReplayMatchChkBox.Text = "Match Regular Expression?";
            this.frmFileReplayMatchChkBox.UseVisualStyleBackColor = true;
            this.frmFileReplayMatchChkBox.CheckedChanged += new EventHandler(this.frmFileReplayMatchChkBox_CheckedChanged);
            this.frmFileReplayTrackBar.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.frmFileReplayTrackBar.Location = new Point(0x69, 0xba);
            this.frmFileReplayTrackBar.Name = "frmFileReplayTrackBar";
            this.frmFileReplayTrackBar.Size = new Size(600, 0x2a);
            this.frmFileReplayTrackBar.TabIndex = 8;
            this.frmFileReplayTrackBar.Scroll += new EventHandler(this.frmFileReplayTrackBar_Scroll);
            this.frmFileReplayTrackBar.MouseHover += new EventHandler(this.frmFileReplayTrackBar_MouseHover);
            this.frmFileReplayMarkerLabel.AutoSize = true;
            this.frmFileReplayMarkerLabel.Location = new Point(14, 0xa4);
            this.frmFileReplayMarkerLabel.Name = "frmFileReplayMarkerLabel";
            this.frmFileReplayMarkerLabel.Size = new Size(0x44, 13);
            this.frmFileReplayMarkerLabel.TabIndex = 6;
            this.frmFileReplayMarkerLabel.Text = "User Marker:";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(13, 60);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x45, 13);
            this.label1.TabIndex = 0x15;
            this.label1.Text = "Log File Path";
            this.logFileNameTextBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.logFileNameTextBox.Location = new Point(0x58, 0x38);
            this.logFileNameTextBox.Name = "logFileNameTextBox";
            this.logFileNameTextBox.Size = new Size(0x249, 20);
            this.logFileNameTextBox.TabIndex = 0x16;
            this.logStatusLabel.AutoSize = true;
            this.logStatusLabel.Location = new Point(0x1a9, 0x58);
            this.logStatusLabel.Name = "logStatusLabel";
            this.logStatusLabel.Size = new Size(0x3a, 13);
            this.logStatusLabel.TabIndex = 0x17;
            this.logStatusLabel.Text = "Log Status";
            this.btn_logFileBroswer.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btn_logFileBroswer.Location = new Point(0x2ab, 0x36);
            this.btn_logFileBroswer.Name = "btn_logFileBroswer";
            this.btn_logFileBroswer.Size = new Size(0x16, 0x17);
            this.btn_logFileBroswer.TabIndex = 0x18;
            this.btn_logFileBroswer.Text = "...";
            this.btn_logFileBroswer.UseVisualStyleBackColor = true;
            this.btn_logFileBroswer.Click += new EventHandler(this.btn_logFileBroswer_Click);
            this.frmFileplaybackDelayTextBox.Location = new Point(14, 200);
            this.frmFileplaybackDelayTextBox.Name = "frmFileplaybackDelayTextBox";
            this.frmFileplaybackDelayTextBox.Size = new Size(0x49, 20);
            this.frmFileplaybackDelayTextBox.TabIndex = 0x19;
            this.frmFileplaybackDelayTextBox.PreviewKeyDown += new PreviewKeyDownEventHandler(this.frmFileplaybackDelayTextBox_PreviewKeyDown);
            this.frmFileplaybackDelayTextBox.MouseEnter += new EventHandler(this.frmFileplaybackDelayTextBox_MouseEnter);
            this.frmFileplaybackDelayLabel.AutoSize = true;
            this.frmFileplaybackDelayLabel.Location = new Point(14, 0xb6);
            this.frmFileplaybackDelayLabel.Name = "frmFileplaybackDelayLabel";
            this.frmFileplaybackDelayLabel.Size = new Size(0x38, 13);
            this.frmFileplaybackDelayLabel.TabIndex = 0x1a;
            this.frmFileplaybackDelayLabel.Text = "Delay (ms)";
            this._displayDataBG.WorkerReportsProgress = true;
            this._displayDataBG.WorkerSupportsCancellation = true;
            this.setRefLocationBtn.Location = new Point(0xf5, 0x53);
            this.setRefLocationBtn.Name = "setRefLocationBtn";
            this.setRefLocationBtn.Size = new Size(130, 0x17);
            this.setRefLocationBtn.TabIndex = 0x1b;
            this.setRefLocationBtn.Text = "Set Reference Position";
            this.setRefLocationBtn.UseVisualStyleBackColor = true;
            this.setRefLocationBtn.Click += new EventHandler(this.setRefLocationBtn_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.ClientSize = new Size(0x2dc, 0x266);
            base.Controls.Add(this.setRefLocationBtn);
            base.Controls.Add(this.frmFileplaybackDelayLabel);
            base.Controls.Add(this.frmFileplaybackDelayTextBox);
            base.Controls.Add(this.btn_logFileBroswer);
            base.Controls.Add(this.logStatusLabel);
            base.Controls.Add(this.logFileNameTextBox);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.frmFileReplayProtocolsLabel);
            base.Controls.Add(this.chkboxSLC);
            base.Controls.Add(this.cboProtocols);
            base.Controls.Add(this.frmFileReplayMarkerLabel);
            base.Controls.Add(this.markerText);
            base.Controls.Add(this.frmFileReplayTrackBar);
            base.Controls.Add(this.frmFileReplayOccurrence);
            base.Controls.Add(this.frmFileReplayRegExpressionTxtBox);
            base.Controls.Add(this.frmFileReplayMatchChkBox);
            base.Controls.Add(this.GroupBox1);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.menuStrip1);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MainMenuStrip = this.menuStrip1;
            base.Name = "frmFileReplay";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Replay File";
            base.FormClosing += new FormClosingEventHandler(this.frmFileReplay_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.frmFileReplayTrackBar.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void interferenceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!base.IsDisposed)
            {
                string str = "File Playback Interference Report";
                if ((this._interferenceReport == null) || this._interferenceReport.IsDisposed)
                {
                    this._interferenceReport = new frmInterferenceReport(this.comm);
                }
                this._interferenceReport.CommWindow = this.comm;
                this._interferenceReport.Text = str;
                this._interferenceReport.Show();
                this._interferenceReport.BringToFront();
            }
            else
            {
                MessageBox.Show("File Playback not initialized", "Information");
            }
        }

        private void LoadProtocolList()
        {
            ArrayList protocols = new ArrayList();
            protocols = this.comm.m_Protocols.GetProtocols();
            for (int i = 0; i < protocols.Count; i++)
            {
                if (((protocols[i].ToString() != "LPL") && (protocols[i].ToString() != "F")) && (protocols[i].ToString() != "AI3"))
                {
                    this.cboProtocols.Items.Add(protocols[i].ToString());
                }
            }
        }

        private void locationViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!base.IsDisposed)
            {
                string str = "File Playback Map";
                if ((this._locationViewPanel == null) || this._locationViewPanel.IsDisposed)
                {
                    this._locationViewPanel = new frmCommLocationMap();
                }
                this._locationViewPanel.CommWindow = this.comm;
                this._locationViewPanel.Text = str;
                this._locationViewPanel.Show();
                this._locationViewPanel.BringToFront();
            }
            else
            {
                MessageBox.Show("File Playback not initialized", "Information");
            }
        }

        private void markerText_MouseEnter(object sender, EventArgs e)
        {
            string text = "Enter text and hit Enter key write it to log file";
            this.toolTip4.Show(text, this.markerText, 0x7530);
        }

        private void markerText_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.comm.WriteApp("User marker: " + this.markerText.Text);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            this._playState = _playStates.QUIT;
            m_SChildform = null;
            this.comm.Log.CloseFile();
            if (this._fileHdlr != null)
            {
                this._fileHdlr.Close();
            }
            if (this._parseThread != null)
            {
                this._parseThread.Abort();
                this._parseThread.Join();
            }
            if (this._signalMapPanel != null)
            {
                this._signalMapPanel.Close();
            }
            if (this._signalStrengthPanel != null)
            {
                this._signalStrengthPanel.Close();
            }
            if (this._locationViewPanel != null)
            {
                this._locationViewPanel.Close();
            }
            if (this._interferenceReport != null)
            {
                this._interferenceReport.Close();
            }
            if (this._messageFilter != null)
            {
                this._messageFilter.Close();
            }
            if (this._ttffDisplay != null)
            {
                this._ttffDisplay.Close();
            }
            base.OnClosed(e);
        }

        private void parseFile()
        {
            EventHandler method = null;
            EventHandler handler2 = null;
            EventHandler handler3 = null;
            EventHandler handler4 = null;
            EventHandler handler5 = null;
            EventHandler handler6 = null;
            EventHandler handler7 = null;
            EventHandler handler8 = null;
            CommonUtilsClass class2 = new CommonUtilsClass();
            string timeStamp = string.Empty;
            int millisecondsTimeout = 500;
            bool flag = false;
        Label_002C:
            try
            {
                switch (this._playState)
                {
                    case _playStates.IDLE:
                        millisecondsTimeout = 500;
                        this._fileIndex = 0L;
                        if (method == null)
                        {
                            method = delegate {
                                this.frmFileReplayTrackBar.Enabled = true;
                            };
                        }
                        this.frmFileReplayTrackBar.Invoke(method);
                        goto Label_080E;

                    case _playStates.PLAY:
                        millisecondsTimeout = this.speedDelay;
                        class2.viewPause = false;
                        if (handler3 == null)
                        {
                            handler3 = delegate {
                                this.frmFileReplayTrackBar.Enabled = false;
                            };
                        }
                        this.frmFileReplayTrackBar.Invoke(handler3);
                        if (this._fileHdlr != null)
                        {
                            string csvString = this._fileHdlr[this._fileIndex];
                            try
                            {
                                string str4;
                                if (csvString == "EOF")
                                {
                                    if (MessageBox.Show("End of file reached! -- Rewind?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                                    {
                                        this._fileIndex = 0L;
                                        this.comm.WriteApp("User marker: Rewind and retart");
                                    }
                                    else
                                    {
                                        this._playState = _playStates.IDLE;
                                        if (handler4 == null)
                                        {
                                            handler4 = delegate {
                                                this.Text = "Stop: " + this._logFileName;
                                            };
                                        }
                                        base.Invoke(handler4);
                                    }
                                    if (handler5 == null)
                                    {
                                        handler5 = delegate {
                                            this.frmFileReplayTrackBar.Value = this.frmFileReplayTrackBar.Maximum;
                                        };
                                    }
                                    this.frmFileReplayTrackBar.Invoke(handler5);
                                    goto Label_080E;
                                }
                                string inputString = string.Empty;
                                bool flag2 = false;
                                CommonClass.MessageType normal = CommonClass.MessageType.Normal;
                                csvString = csvString.TrimEnd(new char[] { '\n' });
                                csvString = csvString.TrimEnd(new char[] { '\r' });
                                switch (this._fileType)
                                {
                                    case CommonClass.TransmissionType.Text:
                                        inputString = csvString;
                                        normal = CommonClass.MessageType.Incoming;
                                        timeStamp = string.Empty;
                                        goto Label_04F9;

                                    case CommonClass.TransmissionType.GP2:
                                    {
                                        csvString = csvString.ToUpper();
                                        if (!csvString.Contains("A0 A2"))
                                        {
                                            goto Label_0280;
                                        }
                                        int index = csvString.IndexOf("A0 A2");
                                        int num3 = csvString.IndexOf("(");
                                        inputString = csvString.Substring(index);
                                        str4 = csvString.Substring(num3 + 1, 1);
                                        if (!(str4 == "0"))
                                        {
                                            break;
                                        }
                                        normal = CommonClass.MessageType.Incoming;
                                        goto Label_04F9;
                                    }
                                    case CommonClass.TransmissionType.GPS:
                                    {
                                        inputString = csvString;
                                        normal = CommonClass.MessageType.Normal;
                                        csvString = csvString.Replace(" ", "");
                                        string messageProtocol = this.comm.MessageProtocol;
                                        if (this.comm.MessageProtocol == "OSP")
                                        {
                                            messageProtocol = "SSB";
                                        }
                                        if (csvString.StartsWith("2,"))
                                        {
                                            this.comm.getSatellitesDataForGUIFromCSV(2, 0, messageProtocol, csvString);
                                        }
                                        else if (csvString.StartsWith("4,"))
                                        {
                                            this.comm.getSatellitesDataForGUIFromCSV(4, 0, messageProtocol, csvString);
                                        }
                                        else if (csvString.StartsWith("41,"))
                                        {
                                            this.comm.getSatellitesDataForGUIFromCSV(0x29, 0, messageProtocol, csvString);
                                        }
                                        else
                                        {
                                            if (!csvString.StartsWith("225,"))
                                            {
                                                goto Label_04A1;
                                            }
                                            if (!csvString.StartsWith("225,00"))
                                            {
                                                goto Label_03D1;
                                            }
                                            if (!clsGlobal.IsMarketingUser() && (csvString.Length > 1))
                                            {
                                                string str6 = string.Empty;
                                                for (int i = 6; i < csvString.Length; i += 2)
                                                {
                                                    byte num5 = (byte) int.Parse(csvString.Substring(i, 2), NumberStyles.HexNumber);
                                                    num5 = (byte) (num5 ^ 0xff);
                                                    str6 = str6 + Convert.ToChar(num5);
                                                }
                                                inputString = str6;
                                            }
                                        }
                                        goto Label_04F9;
                                    }
                                    default:
                                        inputString = csvString;
                                        normal = CommonClass.MessageType.Normal;
                                        timeStamp = string.Empty;
                                        goto Label_04F9;
                                }
                                switch (str4)
                                {
                                    case "1":
                                    case "2":
                                        normal = CommonClass.MessageType.Incoming;
                                        break;
                                }
                                goto Label_04F9;
                            Label_0280:
                                inputString = csvString;
                                goto Label_04F9;
                            Label_03D1:
                                if (csvString.StartsWith("225,6"))
                                {
                                    this.GetTTFFFromCSV(csvString);
                                    if (!this.comm.AutoReplyCtrl.AutoReplyParams.AutoReply && (this._ttffDisplay != null))
                                    {
                                        ReceiverReset resetCtrl = this.comm.RxCtrl.ResetCtrl;
                                        resetCtrl.ResetCount++;
                                        this._ttffDisplay.updateTTFFNow();
                                    }
                                }
                                else if (csvString.StartsWith("225,7"))
                                {
                                    this.GetTTFFFromCSV(csvString);
                                    if (this.comm.AutoReplyCtrl.AutoReplyParams.AutoReply && (this._ttffDisplay != null))
                                    {
                                        ReceiverReset reset2 = this.comm.RxCtrl.ResetCtrl;
                                        reset2.ResetCount++;
                                        this._ttffDisplay.updateTTFFNow();
                                    }
                                }
                                goto Label_04F9;
                            Label_04A1:
                                if (csvString.StartsWith("69,"))
                                {
                                    this.comm.AutoReplyCtrl.AutoReplyParams.AutoReply = true;
                                    if (csvString.StartsWith("69,1"))
                                    {
                                        this.getOSPPosition(csvString);
                                    }
                                }
                            Label_04F9:
                                flag2 = this.comm.File_DataReceived(normal, inputString, this._isMatchRegularExpression, this._regMatchString, timeStamp);
                                this.rtbDisplayProccess();
                                if (flag2)
                                {
                                    this._count++;
                                    if ((this._lastPlayState != _playStates.FORWARD) && (this._lastPlayState != _playStates.BACKWARD))
                                    {
                                        if (handler6 == null)
                                        {
                                            handler6 = delegate {
                                                this.frmFileReplayOccurrence.Text = "Match: " + this._count.ToString() + " found";
                                            };
                                        }
                                        this.frmFileReplayOccurrence.Invoke(handler6);
                                    }
                                    this._playState = _playStates.PAUSE;
                                }
                                this._fileIndex = this._fileHdlr.Index + 1L;
                                if (this._totalFileSize != 0L)
                                {
                                    int processPercentage = (int) ((((double) this._fileHdlr.Index) / ((double) this._totalFileSize)) * 100.0);
									this.frmFileReplayTrackBar.Invoke((MethodInvoker)delegate
									{
                                        this.frmFileReplayTrackBar.Value = processPercentage;
                                    });
                                }
                                if (this.comm._isEpochMessage)
                                {
                                    this.comm._isEpochMessage = false;
                                    if (this._lastPlayState != _playStates.BACKWARD)
                                    {
                                        if (this._epochList.Count >= 100)
                                        {
                                            this._epochList.RemoveAt(0);
                                        }
                                        if (!this._epochList.Contains(this._fileIndex - 1L))
                                        {
                                            this._epochList.Add(this._fileIndex - 1L);
                                            this._epochIndex = this._epochList.Count - 1;
                                        }
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                if (this._playState != _playStates.QUIT)
                                {
                                    MessageBox.Show(string.Format("Error: {0}\n{1}", exception.Message, csvString), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                    this._fileIndex = this._fileHdlr.Index + 1L;
                                }
                            }
                        }
                        goto Label_080E;

                    case _playStates.PAUSE:
                        millisecondsTimeout = 500;
                        if (handler2 == null)
                        {
                            handler2 = delegate {
                                this.frmFileReplayTrackBar.Enabled = true;
                            };
                        }
                        this.frmFileReplayTrackBar.Invoke(handler2);
                        goto Label_080E;

                    case _playStates.FORWARD:
                        if (handler7 == null)
                        {
                            handler7 = delegate {
                                this.frmFileReplayTrackBar.Enabled = false;
                            };
                        }
                        this.frmFileReplayTrackBar.Invoke(handler7);
                        this.comm._isCheckEpoch = true;
                        this._lastPlayState = this._playState;
                        this._playState = _playStates.PLAY;
                        this._epochIndex++;
                        if (this._epochIndex > (this._epochList.Count - 1))
                        {
                            this._epochIndex = this._epochList.Count - 1;
                            if (this._epochIndex < 0)
                            {
                                this._epochIndex = 0;
                            }
                        }
                        goto Label_080E;

                    case _playStates.BACKWARD:
                        if (handler8 == null)
                        {
                            handler8 = delegate {
                                this.frmFileReplayTrackBar.Enabled = false;
                            };
                        }
                        this.frmFileReplayTrackBar.Invoke(handler8);
                        this.comm._isCheckEpoch = true;
                        this._lastPlayState = this._playState;
                        if (this._epochIndex < 1)
                        {
                            MessageBox.Show("Reached end of backward list", "Warning");
                            this._epochIndex = 0;
                            this._playState = _playStates.PAUSE;
                        }
                        else
                        {
                            this._fileIndex = this._epochList[this._epochIndex - 1];
                            this._epochIndex--;
                            this._playState = _playStates.PLAY;
                        }
                        goto Label_080E;

                    case _playStates.QUIT:
                        flag = true;
                        goto Label_080E;
                }
                millisecondsTimeout = this.speedDelay;
            }
            catch (Exception exception2)
            {
                if (this._playState != _playStates.QUIT)
                {
                    MessageBox.Show(string.Format("Error: frmFileReplay: parseFile() {0}", exception2.ToString()), "ERROR", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand);
                    this._fileIndex = this._fileHdlr.Index + 1L;
                }
            }
        Label_080E:
            if (flag)
            {
                return;
            }
            Thread.Sleep(millisecondsTimeout);
            goto Label_002C;
        }

        private void parseTTFFMessage(object sender, DoWorkEventArgs e)
        {
            try
            {
                MessageQData argument = (MessageQData) e.Argument;
                this.comm.RxCtrl.GetTTFF();
            }
            catch
            {
            }
        }

        private void rtbDisplayProccess()
        {
            MessageQData argument = new MessageQData();
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            bool flag = true;
            string messageText = string.Empty;
            while (this.comm.DisplayQueue.Count > 0)
            {
                argument = (MessageQData) this.comm.DisplayQueue.Dequeue();
                if (((argument != null) && (argument.MessageText != null)) && (argument.MessageText != string.Empty))
                {
                    CommonClass.MessageType messageType = argument.MessageType;
                    if (messageText == argument.MessageText)
                    {
                        messageText = argument.MessageText;
                        continue;
                    }
                    messageText = argument.MessageText;
                    if (argument.MessageId == 0xe1)
                    {
                        if (argument.MessageSubId == 6)
                        {
                            if (this.comm.MessageProtocol != "OSP")
                            {
                                BackgroundWorker worker = new BackgroundWorker();
                                worker.DoWork += new DoWorkEventHandler(this.parseTTFFMessage);
                                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.updateTTFFWindow);
                                worker.WorkerReportsProgress = true;
                                worker.WorkerSupportsCancellation = true;
                                worker.RunWorkerAsync(argument);
                            }
                            else if (!this.comm.AutoReplyCtrl.AutoReplyParams.AutoReply)
                            {
                                BackgroundWorker worker2 = new BackgroundWorker();
                                worker2.DoWork += new DoWorkEventHandler(this.parseTTFFMessage);
                                worker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.updateTTFFWindow);
                                worker2.WorkerReportsProgress = true;
                                worker2.WorkerSupportsCancellation = true;
                                worker2.RunWorkerAsync(argument);
                            }
                        }
                        else if (argument.MessageSubId == 7)
                        {
                            BackgroundWorker worker3 = new BackgroundWorker();
                            worker3.DoWork += new DoWorkEventHandler(this.parseTTFFMessage);
                            worker3.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.updateTTFFWindow);
                            worker3.WorkerReportsProgress = true;
                            worker3.WorkerSupportsCancellation = true;
                            worker3.RunWorkerAsync(argument);
                        }
                    }
                    else if (argument.MessageId == 6)
                    {
                        if (argument.MessageChanId == 0xbb)
                        {
                            BackgroundWorker worker4 = new BackgroundWorker();
                            worker4.DoWork += new DoWorkEventHandler(this.parseTTFFMessage);
                            worker4.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.updateTTFFWindow);
                            worker4.WorkerReportsProgress = true;
                            worker4.WorkerSupportsCancellation = true;
                            worker4.RunWorkerAsync(argument);
                        }
                    }
                    else if (argument.MessageId == 0x45)
                    {
                        this.comm.AutoReplyCtrl.AutoReplyParams.AutoReply = true;
                    }
                    string str2 = string.Empty;
                    switch (this.comm.RxTransType)
                    {
                        case CommunicationManager.TransmissionType.SSB:
                            this.comm.dataGui.AGC_Gain = 0;
                            str2 = argument.MessageText;
                            break;

                        case CommunicationManager.TransmissionType.GP2:
                            str2 = CommonUtilsClass.LogToGP2(argument.MessageText, argument.MessageTime);
                            this.comm.dataGui.AGC_Gain = 0;
                            break;

                        case CommunicationManager.TransmissionType.GPS:
                            str2 = this.comm.LogToCSV(argument.MessageText);
                            break;

                        default:
                            str2 = argument.MessageText;
                            flag = false;
                            break;
                    }
                    if ((str2 != null) || (str2 != string.Empty))
                    {
                        builder.Append(str2);
                        if (!flag)
                        {
                            continue;
                        }
                        builder.Append("\r\n");
                        if (((argument.MessageId != 0xe1) && (argument.MessageId != 0xff)) && ((argument.MessageId != 0x40) && (argument.MessageId != 0x44)))
                        {
                            continue;
                        }
                        builder2.Append(str2);
                        if (!str2.EndsWith("\n"))
                        {
                            builder2.Append("\r\n");
                        }
                        string pattern = @"gain (?<gain>\d+)";
                        Regex regex = new Regex(pattern, RegexOptions.Compiled);
                        if (regex.IsMatch(str2))
                        {
                            try
                            {
                                this.comm.dataGui.AGC_Gain = Convert.ToInt32(regex.Match(str2).Result("${gain}"));
                            }
                            catch
                            {
                            }
                        }
                        lock (this.comm.LockErrorLog)
                        {
                            foreach (string str4 in this.comm.ErrorStringList)
                            {
                                if (str2.Contains(str4))
                                {
                                    this.comm.Log.ErrorWriteLine(str2);
                                }
                            }
                            continue;
                        }
                    }
                }
            }
            string msg = builder.ToString();
            if (msg != string.Empty)
            {
                if (!this.comm.ViewAll)
                {
                    bool flag1 = builder2.ToString() != string.Empty;
                }
                this.comm.Log.Write(msg);
            }
        }

        private void satelliteMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!base.IsDisposed)
            {
                string str = "File Playback SVs Map";
                if ((this._signalMapPanel == null) || this._signalMapPanel.IsDisposed)
                {
                    this._signalMapPanel = new frmCommRadarMap();
                }
                this._signalMapPanel.CommWindow = this.comm;
                this._signalMapPanel.Text = str;
                this._signalMapPanel.Show();
                this._signalMapPanel.BringToFront();
            }
            else
            {
                MessageBox.Show("File Playback not initialized", "Information");
            }
        }

        private void setRefLocationBtn_Click(object sender, EventArgs e)
        {
            frmSetReferenceLocation location = new frmSetReferenceLocation();
            location.CommWindow = this.comm;
            location.ShowDialog();
            location.CommWindow.Dispose();
            location.Dispose();
            if (this._ttffDisplay != null)
            {
                this._ttffDisplay.SetReferenceLocation();
            }
        }

        private void SetViewTypeAndUpdateMenuCheck(CommonClass.TransmissionType viewType)
        {
            this.hexToolStripMenuItem.Checked = false;
            this.aSCIIToolStripMenuItem.Checked = false;
            this.sSBToolStripMenuItem.Checked = false;
            this.gP2ToolStripMenuItem.Checked = false;
            this.cSVToolStripMenuItem.Checked = false;
            this._viewType = CommonClass.TransmissionType.GP2;
            switch (viewType)
            {
                case CommonClass.TransmissionType.Text:
                    this.aSCIIToolStripMenuItem.Checked = true;
                    this._viewType = CommonClass.TransmissionType.Text;
                    this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Text;
                    return;

                case CommonClass.TransmissionType.Hex:
                    this.hexToolStripMenuItem.Checked = true;
                    this._viewType = CommonClass.TransmissionType.Hex;
                    this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Hex;
                    return;

                case CommonClass.TransmissionType.SSB:
                    this.sSBToolStripMenuItem.Checked = true;
                    this._viewType = CommonClass.TransmissionType.SSB;
                    this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.SSB;
                    return;

                case CommonClass.TransmissionType.GP2:
                    this.gP2ToolStripMenuItem.Checked = true;
                    this._viewType = CommonClass.TransmissionType.GP2;
                    this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GP2;
                    return;

                case CommonClass.TransmissionType.GPS:
                    this.cSVToolStripMenuItem.Checked = true;
                    this._viewType = CommonClass.TransmissionType.GPS;
                    this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GPS;
                    return;
            }
        }

        private void signalStrengthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!base.IsDisposed)
            {
                string str = "File Playback Signal View";
                if ((this._signalStrengthPanel == null) || this._signalStrengthPanel.IsDisposed)
                {
                    this._signalStrengthPanel = new frmCommSignalView();
                }
                this._signalStrengthPanel.CommWindow = this.comm;
                this._signalStrengthPanel.Text = str;
                this._signalStrengthPanel.Show();
                this._signalStrengthPanel.BringToFront();
            }
            else
            {
                MessageBox.Show("File Playback not initialized", "Information");
            }
        }

        private void sSBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetViewTypeAndUpdateMenuCheck(CommonClass.TransmissionType.SSB);
        }

        private void startLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string inFilename = string.Empty;
            string path = clsGlobal.InstalledDirectory + @"\Log";
            if (this._logFileName != string.Empty)
            {
                if (this.logFileNameTextBox.Text.Length == 0)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    inFilename = this._processFileLog;
                    this.logFileNameTextBox.Text = inFilename;
                }
                else
                {
                    Regex regex = new Regex("^(([a-zA-Z]\\:)|(\\\\))(\\\\{1}|((\\\\{1})[^\\\\]([^/:*?<>\"|]*))+)$");
                    if (!regex.IsMatch(this.logFileNameTextBox.Text))
                    {
                        MessageBox.Show("Invalid File Path");
                        return;
                    }
                    inFilename = this.logFileNameTextBox.Text;
                }
                if (this.comm.Log.IsFileOpen())
                {
                    this.logStatusLabel.Text = " logging ...";
                }
                else if (this.comm.Log.OpenFile(inFilename))
                {
                    this.logStatusLabel.Text = " logging ...";
                    this.btn_logFileBroswer.Enabled = false;
                    this.logFileNameTextBox.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Replay File is not opened", "Information");
            }
        }

        public void StartParseThread()
        {
            if (!this._parseThread.IsAlive)
            {
                this._parseThread.Start();
            }
        }

        private void stopLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.comm.Log.CloseFile();
            this.logStatusLabel.Text = "idle";
            this.btn_logFileBroswer.Enabled = true;
            this.logFileNameTextBox.Enabled = true;
        }

        private void tTFFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateTTFFWin();
        }

        private void updateGui(object sender, DoWorkEventArgs e)
        {
        Label_0000:
            if (this._displayDataBG.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                if (this.comm.DisplayQueue.Count > 0)
                {
                    this._displayDataBG.ReportProgress(100);
                }
                Thread.Sleep(100);
                goto Label_0000;
            }
        }

        private void updateTTFFWindow(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this._ttffDisplay != null)
            {
                this._ttffDisplay.updateTTFFNow();
            }
        }

        private void viewAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                if (this.comm.ViewAll)
                {
                    this.viewAllToolStripMenuItem.CheckState = CheckState.Unchecked;
                    this.comm.ViewAll = false;
                }
                else
                {
                    this.viewAllToolStripMenuItem.CheckState = CheckState.Checked;
                    this.comm.ViewAll = true;
                }
            }
        }

        private void viewModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                switch (this.comm.RxCurrentTransmissionType)
                {
                    case CommunicationManager.TransmissionType.Text:
                        this.aSCIIToolStripMenuItem.Select();
                        return;

                    case CommunicationManager.TransmissionType.Hex:
                        this.hexToolStripMenuItem.Select();
                        return;

                    case CommunicationManager.TransmissionType.SSB:
                        this.sSBToolStripMenuItem.Select();
                        return;

                    case CommunicationManager.TransmissionType.GP2:
                        this.gP2ToolStripMenuItem.Select();
                        return;

                    case CommunicationManager.TransmissionType.GPS:
                        this.cSVToolStripMenuItem.Select();
                        return;
                }
                this.gP2ToolStripMenuItem.Select();
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

        private enum _playStates
        {
            IDLE,
            PLAY,
            PAUSE,
            FORWARD,
            BACKWARD,
            QUIT,
            UNKNOWN
        }
    }
}

