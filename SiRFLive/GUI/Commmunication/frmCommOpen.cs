﻿namespace SiRFLive.GUI.Commmunication
{
    using CommMgrClassLibrary;
    using CommonClassLibrary;
    using CommonUtilsClassLibrary;
    using LogManagerClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.GUI;
    using SiRFLive.GUI.DlgsInputMsg;
    using SiRFLive.MessageHandling;
    using SiRFLive.Properties;
    using SiRFLive.Utilities;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Timers;
    using System.Windows.Forms;
    using System.Xml;

    public class frmCommOpen : Form
    {
        private string _almDataFilename = string.Empty;
        private StreamWriter _almDataStreamWriter;
        private XmlDocument _appWindowsSettings = new XmlDocument();
        internal frmCompassView _compassView;
        private string _currentBaud = "4800";
        private string _currentProtocol = "OSP";
        private string _defaultWindowsRestoredFilePath;
        private BackgroundWorker _displayDataBG;
        internal frmEncryCtrl _encryCtrl;
        private int _eph_Msg_SV_ID;
        private string _eph_non_str = string.Empty;
        private bool _eph_rcvd;
        private string _ephDataFilename = string.Empty;
        private StreamWriter _ephDataStreamWriter;
        internal frmCommErrorView _errorView;
        private frmAutoReply _formautoReply;
        private frmAutoReplySummary _formautoReplySum;
        private int _hostAppCmdWinId;
        internal frmCommInputMessage _inputCommands;
        internal frmInterferenceReport _interferenceReport;
        private bool _isIdle = true;
        private bool _isInit;
        private string _lastWindowsRestoredFilePath;
        private string _lastWinTitle = string.Empty;
        internal frmCommLocationMap _locationViewPanel;
        internal frmCommMessageFilter _messageFilterCustom;
        internal frmCommMessageFilter _messageFilterDebug;
        internal frmCommMessageFilter _messageFilterResponse;
        private static int _openWinCount;
        private BackgroundWorker _parseDataBG;
        private BackgroundWorker _readPortDataBG;
        private frmRXInit_cmd _resetCmd;
        internal frmCommResponseView _responseView;
        internal frmSatelliteStats _SatelliteStats;
        private frmSessionClose _SessionClose;
        private frmSessionOpen _SessionOpen;
        internal frmCommRadarMap _signalMapPanel;
        internal frmCommSignalView _signalStrengthPanel;
        internal frmCommSiRFaware _SiRFAware;
        private frmTTBOpen _TTBConnect;
        internal frmCommSignalView _ttbSigWin;
        private frm_TTBTimeAidingCfg _TTBtimeAidCfg;
        internal frmCommOpen _ttbWin;
        internal frmTTFFDisplay _ttffDisplay;
        private ToolStripMenuItem allMessagesMenu;
        private ToolStripMenuItem autoReplySettingsMenu;
        private ToolStripMenuItem autoReplySummaryMenu;
        private Button btn_logFileBroswer;
        private Button button1;
        private ToolStripMenuItem closeSessionMenu;
        public CommunicationManager comm;
        public ObjectInterface commElements = new ObjectInterface();
        private IContainer components;
        private CommonUtilsClass CUC = new CommonUtilsClass();
        private ToolStripMenuItem cwInterfenceDetectionMenu;
        private ToolStripMenuItem errorLogConfigMenuItem;
        private ToolStripMenuItem errorToolStripMenuItem;
        private DataGridViewTextBoxColumn frmCommDisplayColumn;
        private Label frmCommOpenDurationLoggingStatusLabel;
        private Label frmCommOpenLogStatusLabel;
        private Label frmCommOpenMarkerLabel;
        private MenuStrip frmCommOpenMenuStrip;
        private System.Timers.Timer gcTimerPollAlm = new System.Timers.Timer();
        private System.Timers.Timer gcTimerPollEph = new System.Timers.Timer();
        private ToolStripMenuItem inputCommandMenuItem;
        private Label label_autoDetect;
        private Label label9;
        private ToolStripMenuItem locationMapMenu;
        private TextBox logFileName;
        private ToolStripMenuItem lowPowerCommandBufferMenu;
        private ToolStripMenuItem lowPowerMenu;
        private TextBox markerText;
        public frmMEMSView MEMSViewForm;
        private ToolStripMenuItem messageFilterGeneralMenu;
        private ToolStripMenuItem messagesFilterDebugMenu;
        private ToolStripMenuItem messagesFilterMenuItem;
        private ToolStripMenuItem messagesFilterResponseMenu;
        private ToolStripMenuItem messagesMenuItem;
        private ToolStripMenuItem openSessionMenu;
        private ToolStripMenuItem pollAlmanacMenu;
        private ToolStripMenuItem pollEphemerisMenu;
        private ToolStripMenuItem pollNavParametersMenu;
        private ToolStripMenuItem pollSWVersionMenu;
        private ToolStripMenuItem predefinedMessageMenu;
        private ToolStripMenuItem radarMapMenu;
        private ToolStripMenuItem resetMenu;
        private ToolStripMenuItem responseMenuItem;
        private CommonClass.MyRichTextBox rtbDisplay;
        private ToolStripMenuItem rxCommandsMenuItem;
        private ToolStripMenuItem rxSessionMenuItem;
        private ToolStripMenuItem rxSettingsMenuItem;
        private ToolStripMenuItem rxTTBConfigTimeAiding;
        private ToolStripMenuItem rxTTBConnectMenu;
        private ToolStripMenuItem rxTTBViewMenu;
        private ToolStripMenuItem rxViewModeMenuItem;
        private ToolStripMenuItem satelliteStatisticsMenuItem;
        public frmSatelliteStats SatelliteStatsForm;
        private ToolStripMenuItem setABPDisableMenu;
        private ToolStripMenuItem setABPEnableMenu;
        private ToolStripMenuItem setABPMenuItem;
        private ToolStripMenuItem setDevelopersDebugMenu;
        private System.Timers.Timer setFocusTimer = new System.Timers.Timer();
        private ToolStripMenuItem setMEMSDisableMenu;
        private ToolStripMenuItem setMEMSEnableMenu;
        private ToolStripMenuItem setMEMSMenuItem;
        private ToolStripMenuItem signalViewMenu;
        private ToolStripMenuItem SiRFAwareMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripMenuItem switchOperationModeMenu;
        private ToolStripMenuItem switchProtocolMenu;
        private ToolStripMenuItem timeFreqApproxPosStatusRequest;
        private ToolStripButton toolBarConnectBtn;
        private ToolStripButton toolBarErrorViewBtn;
        private ToolStripButton toolBarLocationBtn;
        private ToolStripDropDownButton toolBarLogBtn;
        private ToolStripMenuItem toolBarLogBtnDurationLogMenu;
        private ToolStripMenuItem toolBarLogBtnStartLogMenu;
        private ToolStripMenuItem toolBarLogBtnStopLogMenu;
        private ToolStripButton toolBarPauseBtn;
        private ToolStripButton toolBarRadarBtn;
        private ToolStripButton toolBarResetBtn;
        private ToolStripButton toolBarResponseViewBtn;
        private ToolStripButton toolBarSignalViewBtn;
        private ToolStripButton toolBarTTFFBtn;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripMenuItem toolStripMenuItem_SetAlm;
        private ToolStripMenuItem toolStripMenuItem_SetEph;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolTip toolTip1;
        private ToolStripMenuItem trackerConfigurationMenu;
        private ToolStripMenuItem TrackerConfigVer2;
        private ToolStripMenuItem trackerICPeekPokeMenu;
        public string transType = string.Empty;
        private ToolStripMenuItem TTBMenuItem;
        private ToolStripMenuItem TTFFMenuItem;
        private ToolStripMenuItem userInputMenu;
        private ToolStripMenuItem viewModeGP2Menu;
        private ToolStripMenuItem viewModeGPSMenu;
        private ToolStripMenuItem viewModeHexMenu;
        private ToolStripMenuItem viewModeMenuItem;
        private ToolStripMenuItem viewModeNmeaTextMenu;
        private ToolStripMenuItem viewModeSSBMenu;

        public event UpdateWindowEventHandler UpdatePortManager;

        public event updateParentEventHandler updateTTBPort;

        public frmCommOpen()
        {
            this.InitializeComponent();
            this.comm = new CommunicationManager(false);
            this._defaultWindowsRestoredFilePath = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Config\DefaultWindowsRestore.xml";
            this._lastWindowsRestoredFilePath = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Config\LastWindowsRestore_CommOpen.xml";
            this.logFileName.DataBindings.Add("Text", this.comm.Log, "filename");
            this.comm.Parity = "None";
            this.comm.StopBits = "One";
            this.comm.DataBits = "8";
            this.comm.BaudRate = "115200";
            this.comm.PortName = "COM6";
            this.comm.EnableLocationMapView = false;
            this.comm.EnableSignalView = false;
            this.comm.EnableSVsMap = false;
            this.comm.EnableSatelliteStats = false;
            if (this.comm.IMUPositionAvailable)
            {
                this.comm.GetIMUDataForGUI();
            }
            _openWinCount++;
            this.Text = "Main COM" + _openWinCount.ToString();
            this.setFocusTimer.Elapsed += new ElapsedEventHandler(this.setWinComFocus);
            this.setFocusTimer.Interval = 1000.0;
            this.setFocusTimer.AutoReset = false;
        }

        private void allMessagesMenu_Click(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                if (this.comm.ViewAll)
                {
                    this.allMessagesMenu.CheckState = CheckState.Unchecked;
                    this.comm.ViewAll = false;
                }
                else
                {
                    this.allMessagesMenu.CheckState = CheckState.Checked;
                    this.comm.ViewAll = true;
                }
            }
        }

        private void autoReplySettingsMenu_Click(object sender, EventArgs e)
        {
            this.createAutoReplyWindow();
        }

        private void autoReplySummaryMenu_Click(object sender, EventArgs e)
        {
            this.createAutoReplySummaryWindow();
        }

        private void autoReplyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.createAutoReplyWindow();
        }

        public void AutoTestCloseMicsWindows()
        {
			base.BeginInvoke((MethodInvoker)delegate
			{
                if (this._locationViewPanel != null)
                {
                    this._locationViewPanel.Close();
                }
                if (this._inputCommands != null)
                {
                    this._inputCommands.Close();
                }
                if (this._messageFilterDebug != null)
                {
                    this._messageFilterDebug.Close();
                }
                if (this._messageFilterResponse != null)
                {
                    this._messageFilterResponse.Close();
                }
                if (this._messageFilterCustom != null)
                {
                    this._messageFilterCustom.Close();
                }
                if (this._signalMapPanel != null)
                {
                    this._signalMapPanel.Close();
                }
                if (this._encryCtrl != null)
                {
                    this._encryCtrl.Close();
                }
                if (this._SatelliteStats != null)
                {
                    this._SatelliteStats.Close();
                }
                if (this._interferenceReport != null)
                {
                    this._interferenceReport.Close();
                }
                if (this._SiRFAware != null)
                {
                    this._SiRFAware.Close();
                }
                if (this._messageFilterDebug != null)
                {
                    this._messageFilterDebug.StopListeners();
                    this._messageFilterDebug.Close();
                }
                if (this._messageFilterResponse != null)
                {
                    this._messageFilterResponse.StopListeners();
                    this._messageFilterResponse.Close();
                }
                if (this._messageFilterCustom != null)
                {
                    this._messageFilterCustom.StopListeners();
                    this._messageFilterCustom.Close();
                }
                if (this._interferenceReport != null)
                {
                    this._interferenceReport.StopListeners();
                    this._interferenceReport.Close();
                }
                if (this._responseView != null)
                {
                    this._responseView.Close();
                }
                if (this._errorView != null)
                {
                    this._errorView.Close();
                }
                if (this._compassView != null)
                {
                    this._compassView.Close();
                }
            });
        }

        private void btn_logFileBroswer_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Specify log file name:";
            dialog.InitialDirectory = @"..\..\logs\";
            dialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.CheckPathExists = false;
            dialog.CheckFileExists = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.logFileName.Text = dialog.FileName;
                this.startLog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.rtbDisplay.GetLineFromCharIndex(this.rtbDisplay.SelectionStart);
            MessageBox.Show(this.rtbDisplay.Lines.Length.ToString());
        }

        private void clearGUIDataTimerHandler(object source, ElapsedEventArgs e)
        {
            try
            {
                if (this.comm != null)
                {
                    for (int i = 0; i < DataForGUI.MAX_PRN; i++)
                    {
                        this.comm.dataGui.PRN_Arr_CNO[i] = 0f;
                        this.comm.dataGui.PRN_Arr_Azimuth[i] = 0f;
                        this.comm.dataGui.PRN_Arr_Elev[i] = 0f;
                        this.comm.dataGui.PRN_Arr_State[i] = 0;
                        this.comm.dataGui.PRN_Arr_ID[i] = 0;
                    }
                    for (int j = 0; j < 12; j++)
                    {
                        this.comm.dataGui.SignalDataForGUI.CHAN_Arr_CNO[j] = 0f;
                        this.comm.dataGui.SignalDataForGUI.CHAN_Arr_Azimuth[j] = 0f;
                        this.comm.dataGui.SignalDataForGUI.CHAN_Arr_Elev[j] = 0f;
                        this.comm.dataGui.SignalDataForGUI.CHAN_Arr_State[j] = 0;
                        this.comm.dataGui.SignalDataForGUI.CHAN_Arr_ID[j] = 0;
                    }
                    for (int k = 0; k < 60; k++)
                    {
                        this.comm.dataGui.SignalDataForGUI_All.CHAN_Arr_CNO[k] = 0f;
                        this.comm.dataGui.SignalDataForGUI_All.CHAN_Arr_Azimuth[k] = 0f;
                        this.comm.dataGui.SignalDataForGUI_All.CHAN_Arr_Elev[k] = 0f;
                        this.comm.dataGui.SignalDataForGUI_All.CHAN_Arr_State[k] = 0;
                        this.comm.dataGui.SignalDataForGUI_All.CHAN_Arr_ID[k] = 0;
                    }
                    this.comm.dataGui.Positions.PositionList.Clear();
                    if (this.comm.DisplayPanelLocation != null)
                    {
                        this.comm.DisplayPanelLocation.Invalidate();
                    }
                    if (this.comm.DisplayPanelSignal != null)
                    {
                        this.comm.DisplayPanelSignal.Invalidate();
                    }
                    if (this.comm.DisplayPanelSVs != null)
                    {
                        this.comm.DisplayPanelSVs.Invalidate();
                    }
                    if (this.comm.DisplayPanelSVTraj != null)
                    {
                        this.comm.DisplayPanelSVTraj.Invalidate();
                    }
                    if (this.comm.DisplayPanelSatelliteStats != null)
                    {
                        this.comm.DisplayPanelSatelliteStats.Invalidate();
                    }
                }
            }
            catch
            {
            }
        }

        public void CloseComWin()
        {
			base.Invoke((MethodInvoker)delegate
			{
                base.Close();
            });
        }

        private void closeSessionBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateSessionCloseWindow();
        }

        private void closeSessionMenu_Click(object sender, EventArgs e)
        {
            this.CreateSessionCloseWindow();
        }

        private void commWindowCleanup()
        {
            this._isIdle = true;
            if (this._signalStrengthPanel != null)
            {
                this._signalStrengthPanel.Close();
            }
            if (this._locationViewPanel != null)
            {
                this._locationViewPanel.Close();
            }
            if (this._inputCommands != null)
            {
                this._inputCommands.Close();
            }
            if (this._messageFilterDebug != null)
            {
                this._messageFilterDebug.Close();
            }
            if (this._messageFilterResponse != null)
            {
                this._messageFilterResponse.Close();
            }
            if (this._messageFilterCustom != null)
            {
                this._messageFilterCustom.Close();
            }
            if (this._signalMapPanel != null)
            {
                this._signalMapPanel.Close();
            }
            if (this._encryCtrl != null)
            {
                this._encryCtrl.Close();
            }
            if (this._SatelliteStats != null)
            {
                this._SatelliteStats.Close();
            }
            if (this._ttffDisplay != null)
            {
                this._ttffDisplay.Close();
            }
            if (this._interferenceReport != null)
            {
                this._interferenceReport.Close();
            }
            if (this._SiRFAware != null)
            {
                this._SiRFAware.Close();
            }
            if (this._messageFilterDebug != null)
            {
                this._messageFilterDebug.StopListeners();
                this._messageFilterDebug.Close();
            }
            if (this._messageFilterResponse != null)
            {
                this._messageFilterResponse.StopListeners();
                this._messageFilterResponse.Close();
            }
            if (this._messageFilterCustom != null)
            {
                this._messageFilterCustom.StopListeners();
                this._messageFilterCustom.Close();
            }
            if (this._interferenceReport != null)
            {
                this._interferenceReport.StopListeners();
                this._interferenceReport.Close();
            }
            if (this._responseView != null)
            {
                this._responseView.Close();
            }
            if (this._errorView != null)
            {
                this._errorView.Close();
            }
            if (this._compassView != null)
            {
                this._compassView.Close();
            }
            if (this.comm != null)
            {
                if (this.comm.RxType == CommunicationManager.ReceiverType.TTB)
                {
                    this.comm.WriteData("A0A2 0009 CCA6 0102 0100 0000 0081 76B0 B3");
                    this.comm.WriteData("A0A2 0009 CCA6 0104 0100 0000 0081 78B0 B3");
                }
                if (this.comm.ListenersCtrl != null)
                {
                    this.comm.ListenersCtrl.Cleanup();
                }
                if (this.comm.RxCtrl != null)
                {
                    if (this.comm.RxCtrl.ResetCtrl != null)
                    {
                        this.comm.RxCtrl.ResetCtrl = null;
                    }
                    this.comm.RxCtrl.Dispose();
                    this.comm.RxCtrl = null;
                }
                if (this.comm.TTBPort.IsOpen)
                {
                    this.comm.TTBPort.Close();
                    if (this._ttbWin != null)
                    {
                        this._ttbWin.Close();
                        this._ttbWin = null;
                    }
                }
            }
            if (clsGlobal.CommWinRef.Contains(this.comm.PortName))
            {
                clsGlobal.CommWinRef.Remove(this.comm.PortName);
            }
        }

        private void configTTBTimeAidingBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateTTBTimeAidCfgWindow();
        }

        private void connectTTBBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateTTBConnectWindow();
        }

        private void connectTTBPort()
        {
            if ((this.comm != null) && !this.comm.TTBPort.IsOpen)
            {
                this.comm.TTBPort.Open();
                this.TTBMenuItem.Enabled = true;
            }
        }

        private int CountStringOccurrences(string text, string pattern)
        {
            int num = 0;
            int startIndex = 0;
            while ((startIndex = text.IndexOf(pattern, startIndex)) != -1)
            {
                startIndex += pattern.Length;
                num++;
            }
            return num;
        }

        private void createAutoReplySummaryWindow()
        {
            if (!base.IsDisposed)
            {
                string str = this.comm.sourceDeviceName + ": Auto Reply Summary";
                if ((this._formautoReplySum == null) || this._formautoReplySum.IsDisposed)
                {
                    this._formautoReplySum = new frmAutoReplySummary();
                }
                this._formautoReplySum.CommWindow = this.comm;
                this._formautoReplySum.Text = str;
                this._formautoReplySum.ShowDialog();
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
        }

        private void createAutoReplyWindow()
        {
            if (!base.IsDisposed)
            {
                string str = this.comm.sourceDeviceName + ": Auto Reply";
                if ((this._formautoReply == null) || this._formautoReply.IsDisposed)
                {
                    this._formautoReply = new frmAutoReply();
                }
                this._formautoReply.CommWindow = this.comm;
                this._formautoReply.Text = str;
                this._formautoReply.ShowDialog();
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
            if (this._ttffDisplay != null)
            {
                this._ttffDisplay.SetTTFFMsgIndication();
            }
        }

        internal frmEncryCtrl CreateEncrypCtrlWin()
        {
            if (!base.IsDisposed)
            {
                string str = this.comm.sourceDeviceName + ": Set Developer Debug Levels";
                if ((this._encryCtrl == null) || this._encryCtrl.IsDisposed)
                {
                    this._encryCtrl = new frmEncryCtrl(this.comm);
                    this._encryCtrl.Show();
                }
                this._encryCtrl.CommWindow = this.comm;
                this._encryCtrl.Text = str;
                this._encryCtrl.updateMainWindow += new frmEncryCtrl.updateParentEventHandler(this.updateSubWindowState);
                this._encryCtrl.BringToFront();
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
            return this._encryCtrl;
        }

        public frmCommErrorView CreateErrorViewWin()
        {
            EventHandler method = null;
            if (!base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        if (this.toolBarErrorViewBtn.CheckState == CheckState.Indeterminate)
                        {
                            this.toolBarErrorViewBtn.CheckState = CheckState.Unchecked;
                            if ((this._errorView != null) && !this._errorView.IsDisposed)
                            {
                                this._errorView.Close();
                            }
                        }
                        else
                        {
                            string str = this.comm.sourceDeviceName + ": Error View";
                            if ((this._errorView == null) || this._errorView.IsDisposed)
                            {
                                this._errorView = new frmCommErrorView();
                                this._errorView.Show();
                            }
                            this._errorView.CommWindow = this.comm;
                            this._errorView.Text = str;
                            this._errorView.updateMainWindow += new frmCommErrorView.updateParentEventHandler(this.updateSubWindowState);
                            this._errorView.BringToFront();
                            this.toolBarErrorViewBtn.CheckState = CheckState.Indeterminate;
                        }
                    };
                }
                base.Invoke(method);
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
            return this._errorView;
        }

        internal frmCommInputMessage CreateInputCommandsWin()
        {
            if (!base.IsDisposed)
            {
                string str = this.comm.sourceDeviceName + ": Input Commands";
                if ((this._inputCommands == null) || this._inputCommands.IsDisposed)
                {
                    this._inputCommands = new frmCommInputMessage();
                    this._inputCommands.Show();
                }
                this._inputCommands.CommWindow = this.comm;
                this._inputCommands.Text = str;
                this._inputCommands.updateMainWindow += new frmCommInputMessage.updateParentEventHandler(this.updateSubWindowState);
                this._inputCommands.BringToFront();
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
            return this._inputCommands;
        }

        internal frmInterferenceReport CreateInterferenceReportWindow()
        {
            EventHandler method = null;
            if (!base.IsDisposed)
            {
                if (this.comm != null)
                {
                    if (method == null)
                    {
                        method = delegate {
                            string str = this.comm.sourceDeviceName + ": Interference";
                            if ((this._interferenceReport == null) || this._interferenceReport.IsDisposed)
                            {
                                this._interferenceReport = new frmInterferenceReport(this.comm);
                                this._interferenceReport.Show();
                            }
                            else
                            {
                                this._interferenceReport.StartListen();
                            }
                            this._interferenceReport.Text = str;
                            this._interferenceReport.updateMainWindow += new frmInterferenceReport.updateParentEventHandler(this.updateSubWindowState);
                            this._interferenceReport.BringToFront();
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    MessageBox.Show("COM window not initialized!", "Information");
                }
            }
            return this._interferenceReport;
        }

        internal frmCommLocationMap CreateLocationMapWin()
        {
            EventHandler method = null;
            if (!base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        if (this.toolBarLocationBtn.CheckState == CheckState.Indeterminate)
                        {
                            this.toolBarLocationBtn.CheckState = CheckState.Unchecked;
                            if ((this._locationViewPanel != null) && !this._locationViewPanel.IsDisposed)
                            {
                                this._locationViewPanel.Close();
                            }
                        }
                        else
                        {
                            string str = this.comm.sourceDeviceName + ": Location View";
                            if ((this._locationViewPanel == null) || this._locationViewPanel.IsDisposed)
                            {
                                this._locationViewPanel = new frmCommLocationMap();
                                this._locationViewPanel.Show();
                            }
                            this._locationViewPanel.Text = str;
                            this._locationViewPanel.CommWindow = this.comm;
                            this._locationViewPanel.updateMainWindow += new frmCommLocationMap.updateParentEventHandler(this.updateSubWindowState);
                            this._locationViewPanel.BringToFront();
                            this.toolBarLocationBtn.CheckState = CheckState.Indeterminate;
                        }
                    };
                }
                base.Invoke(method);
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
            return this._locationViewPanel;
        }

        private void createLowPowerInputWindow()
        {
            if ((this.comm != null) && this.IsSourceDeviceOpen())
            {
                new frmLowPower(this.comm).ShowDialog();
            }
        }

        private void CreatePeekPokeWin()
        {
            if (!base.IsDisposed)
            {
                string str = this.comm.sourceDeviceName + ": Tracker IC Peek Poke";
                frmPeekPokeMem mem = new frmPeekPokeMem(this.comm);
                mem.Text = str;
                mem.ShowDialog();
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
        }

        public frmRXInit_cmd CreateResetWindow()
        {
            if (!base.IsDisposed)
            {
                if (this.comm.TTBPort.IsOpen && (this._ttbWin != null))
                {
                    this._ttbWin.Close();
                }
                string str = this.comm.sourceDeviceName + ": Reset";
                if ((this._resetCmd == null) || this._resetCmd.IsDisposed)
                {
                    this._resetCmd = new frmRXInit_cmd();
                }
                this._resetCmd.CommWindow = this.comm;
                this._resetCmd.Text = str;
                if (this._resetCmd.ShowDialog() != DialogResult.Cancel)
                {
                    System.Timers.Timer timer = new System.Timers.Timer();
                    timer.Elapsed += new ElapsedEventHandler(this.clearGUIDataTimerHandler);
                    timer.Interval = 1000.0;
                    timer.AutoReset = false;
                    timer.Start();
                    this.comm.dataGui.AGC_Gain = 0;
                    if ((this.comm.ProductFamily == CommonClass.ProductType.GSD4e) && (this.comm.RxCtrl.ResetCtrl.ResetType == "FACTORY"))
                    {
                        this.setNMEARxMenuStatus();
                    }
                }
                if (this._ttffDisplay != null)
                {
                    this._ttffDisplay.SetTTFFMsgIndication();
                }
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
            return this._resetCmd;
        }

        public frmCommResponseView CreateResponseViewWin()
        {
            EventHandler method = null;
            if (!base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        if (this.toolBarResponseViewBtn.CheckState == CheckState.Indeterminate)
                        {
                            this.toolBarResponseViewBtn.CheckState = CheckState.Unchecked;
                            if ((this._responseView != null) && !this._responseView.IsDisposed)
                            {
                                this._responseView.Close();
                            }
                        }
                        else
                        {
                            string str = this.comm.sourceDeviceName + ": Response View";
                            if ((this._responseView == null) || this._responseView.IsDisposed)
                            {
                                this._responseView = new frmCommResponseView();
                                this._responseView.Show();
                            }
                            this._responseView.CommWindow = this.comm;
                            this._responseView.Text = str;
                            this._responseView.updateMainWindow += new frmCommResponseView.updateParentEventHandler(this.updateSubWindowState);
                            this._responseView.BringToFront();
                            this.toolBarResponseViewBtn.CheckState = CheckState.Indeterminate;
                        }
                    };
                }
                base.Invoke(method);
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
            return this._responseView;
        }

        private bool createRxSettingsWindow()
        {
            bool flag = false;
            if (((this.comm != null) && !this.comm.comPort.IsOpen) && (!this.comm.CMC.HostAppClient.IsOpen() && !this.comm.CMC.HostAppServer.IsOpen()))
            {
                frmCommSettings settings = new frmCommSettings(ref this.comm);
                if (settings.ShowDialog() != DialogResult.Cancel)
                {
                    this._isInit = true;
                    flag = true;
                }
                return flag;
            }
            MessageBox.Show("Port is connected! Disconnect port before configuring.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return flag;
        }

        internal frmSatelliteStats CreateSatelliteStatsWin()
        {
            EventHandler method = null;
            if (!base.IsDisposed)
            {
                if (this.comm != null)
                {
                    if (method == null)
                    {
                        method = delegate {
                            string str = this.comm.sourceDeviceName + ": Satellite Statistics";
                            if ((this._SatelliteStats == null) || this._SatelliteStats.IsDisposed)
                            {
                                this._SatelliteStats = new frmSatelliteStats(this.comm);
                                this._SatelliteStats.Show();
                            }
                            this._SatelliteStats.Text = str;
                            this._SatelliteStats.updateMainWindow += new frmSatelliteStats.updateParentEventHandler(this.updateSubWindowState);
                            this._SatelliteStats.BringToFront();
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    MessageBox.Show("COM window not initialized!", "Information");
                }
            }
            return this._SatelliteStats;
        }

        public frmSessionClose CreateSessionCloseWindow()
        {
            if (!base.IsDisposed)
            {
                string str = "Close Session";
                if ((this._SessionClose == null) || this._SessionClose.IsDisposed)
                {
                    this._SessionClose = new frmSessionClose();
                }
                this._SessionClose.CommWindow = this.comm;
                this._SessionClose.Text = str;
                this._SessionClose.ShowDialog();
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
            return this._SessionClose;
        }

        public frmSessionOpen CreateSessionOpenWindow()
        {
            if (!base.IsDisposed)
            {
                string str = "Open Session";
                if ((this._SessionOpen == null) || this._SessionOpen.IsDisposed)
                {
                    this._SessionOpen = new frmSessionOpen();
                }
                this._SessionOpen.CommWindow = this.comm;
                this._SessionOpen.Text = str;
                this._SessionOpen.ShowDialog();
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
            return this._SessionOpen;
        }

        public frmCommSignalView CreateSignalViewWin()
        {
            EventHandler method = null;
            if (!base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        if (this.toolBarSignalViewBtn.CheckState == CheckState.Indeterminate)
                        {
                            this.toolBarSignalViewBtn.CheckState = CheckState.Unchecked;
                            if ((this._signalStrengthPanel != null) && !this._signalStrengthPanel.IsDisposed)
                            {
                                this._signalStrengthPanel.Close();
                            }
                        }
                        else
                        {
                            string str = this.comm.sourceDeviceName + ": Signal View";
                            if ((this._signalStrengthPanel == null) || this._signalStrengthPanel.IsDisposed)
                            {
                                this._signalStrengthPanel = new frmCommSignalView();
                                this._signalStrengthPanel.Show();
                            }
                            this._signalStrengthPanel.CommWindow = this.comm;
                            this._signalStrengthPanel.Text = str;
                            this._signalStrengthPanel.updateMainWindow += new frmCommSignalView.updateParentEventHandler(this.updateSubWindowState);
                            this._signalStrengthPanel.BringToFront();
                            this.toolBarSignalViewBtn.CheckState = CheckState.Indeterminate;
                        }
                    };
                }
                base.Invoke(method);
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
            return this._signalStrengthPanel;
        }

        internal frmCommSiRFaware CreateSiRFAwareWin()
        {
            EventHandler method = null;
            if (!base.IsDisposed)
            {
                if (this.comm != null)
                {
                    if (method == null)
                    {
                        method = delegate {
                            string str = this.comm.sourceDeviceName + ": SiRFaware";
                            if ((this._SiRFAware == null) || this._SiRFAware.IsDisposed)
                            {
                                this._SiRFAware = new frmCommSiRFaware(this.comm);
                                this._SiRFAware.Show();
                                this._SiRFAware.StartListen();
                            }
                            this._SiRFAware.Text = str;
                            this._SiRFAware.updateMainWindow += new frmCommSiRFaware.updateParentEventHandler(this.updateSubWindowState);
                            this._SiRFAware.BringToFront();
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    MessageBox.Show("COM window not initialized!", "Information");
                }
            }
            return this._SiRFAware;
        }

        internal frmCommRadarMap CreateSVsMapWin()
        {
            EventHandler method = null;
            if (!base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        if (this.toolBarRadarBtn.CheckState == CheckState.Indeterminate)
                        {
                            this.toolBarRadarBtn.CheckState = CheckState.Unchecked;
                            if ((this._signalMapPanel != null) && !this._signalMapPanel.IsDisposed)
                            {
                                this._signalMapPanel.Close();
                            }
                        }
                        else
                        {
                            string str = this.comm.sourceDeviceName + ": Radar View";
                            if ((this._signalMapPanel == null) || this._signalMapPanel.IsDisposed)
                            {
                                this._signalMapPanel = new frmCommRadarMap();
                                this._signalMapPanel.Show();
                            }
                            this._signalMapPanel.CommWindow = this.comm;
                            this._signalMapPanel.Text = str;
                            this._signalMapPanel.updateMainWindow += new frmCommRadarMap.updateParentEventHandler(this.updateSubWindowState);
                            this._signalMapPanel.BringToFront();
                            this.toolBarRadarBtn.CheckState = CheckState.Indeterminate;
                        }
                    };
                }
                base.Invoke(method);
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
            return this._signalMapPanel;
        }

        private void CreateSwitchOperationModeWin()
        {
            if (!base.IsDisposed)
            {
                string str = this.comm.sourceDeviceName + ": Switch Operation Mode";
                frmSwitchOperationMode mode = new frmSwitchOperationMode(this.comm);
                mode.Text = str;
                mode.ShowDialog();
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
        }

        private DialogResult createSwitchProtocolWindow()
        {
            if ((this.comm != null) && this.IsSourceDeviceOpen())
            {
                frmSwitchProtocol protocol = new frmSwitchProtocol(this.comm);
                return protocol.ShowDialog();
            }
            return DialogResult.Cancel;
        }

        private void CreateTrackerConfigWin()
        {
            if (!base.IsDisposed)
            {
                string str = this.comm.sourceDeviceName + ": Tracker IC Configuration";
                frmTrackerICConfiguration configuration = new frmTrackerICConfiguration(this.comm);
                configuration.Text = str;
                configuration.ShowDialog();
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
        }

        private void CreateTrackerConfigWin_Ver2()
        {
            if (!base.IsDisposed)
            {
                string str = this.comm.sourceDeviceName + ": IC Configuration";
                frmTrackerICConfig_Ver2 ver = new frmTrackerICConfig_Ver2(this.comm);
                ver.Text = str;
                ver.ShowDialog();
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
        }

        private void CreateTransmitSerialMessageWin()
        {
            if (!base.IsDisposed)
            {
                string str = this.comm.sourceDeviceName + ": Transmit Serial Message";
                frmTransmitSerialMessage message = new frmTransmitSerialMessage(this.comm);
                message.Text = str;
                message.ShowDialog();
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
        }

        public frmTTBOpen CreateTTBConnectWindow()
        {
            if (!base.IsDisposed)
            {
                string str = "Connect TTB";
                if ((this._TTBConnect == null) || this._TTBConnect.IsDisposed)
                {
                    this._TTBConnect = new frmTTBOpen();
                }
                this._TTBConnect.CommWindow = this.comm;
                this._TTBConnect.Text = str;
                this._TTBConnect.ShowDialog();
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
            return this._TTBConnect;
        }

        public frm_TTBTimeAidingCfg CreateTTBTimeAidCfgWindow()
        {
            if (!base.IsDisposed)
            {
                string str = "Configure Time Aiding";
                if ((this._TTBtimeAidCfg == null) || this._TTBtimeAidCfg.IsDisposed)
                {
                    this._TTBtimeAidCfg = new frm_TTBTimeAidingCfg();
                }
                this._TTBtimeAidCfg.CommWindow = this.comm;
                this._TTBtimeAidCfg.Text = str;
                this._TTBtimeAidCfg.ShowDialog();
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
            return this._TTBtimeAidCfg;
        }

        public frmTTFFDisplay CreateTTFFWin()
        {
            EventHandler method = null;
            if (!base.IsDisposed)
            {
                if (this.comm != null)
                {
                    if (this.comm.MessageProtocol == "NMEA")
                    {
                        return null;
                    }
                    if (method == null)
                    {
                        method = delegate {
                            if (this.toolBarTTFFBtn.CheckState == CheckState.Indeterminate)
                            {
                                this.toolBarTTFFBtn.CheckState = CheckState.Unchecked;
                                if ((this._ttffDisplay == null) || !this._ttffDisplay.IsDisposed)
                                {
                                    this._ttffDisplay.Close();
                                }
                            }
                            else
                            {
                                string str = this.comm.sourceDeviceName + ": TTFF/Nav Accuracy";
                                if ((this._ttffDisplay == null) || this._ttffDisplay.IsDisposed)
                                {
                                    this._ttffDisplay = new frmTTFFDisplay(this.comm);
                                    this._ttffDisplay.Show();
                                }
                                this._ttffDisplay.Text = str;
                                this._ttffDisplay.updateMainWindow += new frmTTFFDisplay.updateParentEventHandler(this.updateSubWindowState);
                                this._ttffDisplay.BringToFront();
                                this.toolBarTTFFBtn.CheckState = CheckState.Indeterminate;
                            }
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

        private void cwInterfenceDetectionMenu_Click(object sender, EventArgs e)
        {
            this.CreateInterferenceReportWindow();
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        public void DisconnectPort()
        {
            this.StopAsyncProcess();
            this._isIdle = true;
            string text1 = "Main " + this.comm.PortName + ": Idle";
            if (this._messageFilterDebug != null)
            {
                this._messageFilterDebug.StopListeners();
                this._messageFilterDebug.SetStatus(false);
            }
            if (this._messageFilterResponse != null)
            {
                this._messageFilterResponse.StopListeners();
                this._messageFilterResponse.SetStatus(false);
            }
            if (this._messageFilterCustom != null)
            {
                this._messageFilterCustom.StopListeners();
                this._messageFilterCustom.SetStatus(false);
            }
            if (this._interferenceReport != null)
            {
                this._interferenceReport.StopListeners();
            }
            if (this.comm != null)
            {
                if (this.comm.RxType == CommunicationManager.ReceiverType.TTB)
                {
                    this.comm.WriteData("A0A2 0009 CCA6 0102 0100 0000 0081 76B0 B3");
                    this.comm.WriteData("A0A2 0009 CCA6 0104 0100 0000 0081 78B0 B3");
                }
                if (this.comm.ListenersCtrl != null)
                {
                    this.comm.ListenersCtrl.Cleanup();
                }
                if (this.comm.TTBPort.IsOpen)
                {
                    this.comm.TTBPort.Close();
                    if (this._ttbWin != null)
                    {
                        this._ttbWin.Close();
                        this._ttbWin = null;
                    }
                }
                if (this.comm.ListenersCtrl != null)
                {
                    this.comm.ListenersCtrl = null;
                }
                if (this.comm.RxCtrl != null)
                {
                    if (this.comm.RxCtrl.ResetCtrl != null)
                    {
                        this.comm.RxCtrl.ResetCtrl = null;
                    }
                    this.comm.RxCtrl.Dispose();
                    this.comm.RxCtrl = null;
                }
                this.comm.ClosePort();
            }
            if (clsGlobal.CommWinRef.Contains(this.comm.PortName))
            {
                clsGlobal.CommWinRef.Remove(this.comm.PortName);
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

        private void errorLogConfigMenuItem_Click(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                new frmErrorLogConfig(this.comm, 0).ShowDialog();
            }
        }

        private void errorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateErrorViewWin();
        }

        private void frmCommOpen_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void frmCommOpen_Load(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                this.comm.ViewAll = true;
                this.setRxMenuStatus(this.comm.comPort.IsOpen);
            }
            this.viewModeHexMenu.Visible = false;
            this.viewModeSSBMenu.Visible = false;
            this.frmSaveSettingsLoad_CommOpen(this._lastWindowsRestoredFilePath);
            this.SearchMenu(clsGlobal.g_objfrmMDIMain.CurrentUser);
            this.logFileName.Text = this.comm.Log.filename;
        }

        private void frmCommOpenToolFilterCustom_Click(object sender, EventArgs e)
        {
            this.frmCommOpenToolFilterHelper(sender, e, this._messageFilterCustom, ": Custom Message Filter", "", true);
        }

        public void frmCommOpenToolFilterCustom_Create()
        {
            object sender = null;
            EventArgs e = null;
            this.frmCommOpenToolFilterHelper(sender, e, this._messageFilterCustom, ": Custom Message Filter", "", true);
        }

        private void frmCommOpenToolFilterDebug_Click(object sender, EventArgs e)
        {
            string inFilter = "-1,255,-1%238,255,-1%-1,68,255%238,68,255%-1,225,0%238,225,0%-1,68,225";
            this.frmCommOpenToolFilterHelper(sender, e, this._messageFilterDebug, ": Debug View", inFilter, false);
        }

        private void frmCommOpenToolFilterHelper(object sender, EventArgs e, frmCommMessageFilter f, string inTitle, string inFilter, bool enableMessageFilterInput)
        {
            if (base.IsDisposed)
            {
                MessageBox.Show("COM window not initialized!", "Information");
                return;
            }
            if (this.comm.RxCurrentTransmissionType == CommunicationManager.TransmissionType.Text)
            {
                MessageBox.Show("Filter not supported in this mode", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            string str = this.comm.sourceDeviceName + inTitle;
            if ((f == null) || f.IsDisposed)
            {
                f = new frmCommMessageFilter();
                f.Show();
                string str2 = inTitle;
                if (str2 == null)
                {
                    goto Label_008B;
                }
                if (!(str2 == ": Debug View"))
                {
                    if (str2 == ": Response View")
                    {
                        this._messageFilterResponse = f;
                        goto Label_0092;
                    }
                    goto Label_008B;
                }
                this._messageFilterDebug = f;
            }
            goto Label_0092;
        Label_008B:
            this._messageFilterCustom = f;
        Label_0092:
            f.CommWindow = this.comm;
            f.Text = str;
            f.SetMessageFilter(inFilter);
            f.updateMainWindow += new frmCommMessageFilter.updateParentEventHandler(this.updateSubWindowState);
            f.SetMessageFilterTextState(enableMessageFilterInput);
            if (inFilter != "")
            {
                f.StartListeners();
            }
        }

        private void frmCommOpenToolFilterResponse_Click(object sender, EventArgs e)
        {
            string inFilter = "-1,6,-1%238,6,-1%-1,7,-1%238,7,-1%-1,11,-1%238,11,-1%-1,12,-1%238,12,-1%-1,19,-1%238,19,-1";
            this.frmCommOpenToolFilterHelper(sender, e, this._messageFilterResponse, ": Response View", inFilter, false);
        }

        private void frmCommOpenUpdateConnectBtnImage()
        {
			base.BeginInvoke((MethodInvoker)delegate
			{
                if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
                {
                    if (this.comm.comPort.IsOpen)
                    {
                        this.toolBarConnectBtn.Image = Resources.connect;
                        this.toolBarConnectBtn.Text = "Disconnect";
                    }
                    else
                    {
                        this.toolBarConnectBtn.Image = Resources.disconnect;
                        this.toolBarConnectBtn.Text = "Connect";
                    }
                }
                else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client)
                {
                    if (this.comm.CMC.HostAppClient.IsOpen())
                    {
                        this.toolBarConnectBtn.Image = Resources.connect;
                        this.toolBarConnectBtn.Text = "Disconnect";
                    }
                    else
                    {
                        this.toolBarConnectBtn.Image = Resources.disconnect;
                        this.toolBarConnectBtn.Text = "Connect";
                    }
                }
                else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Server)
                {
                    if (this.comm.CMC.HostAppServer.IsOpen())
                    {
                        this.toolBarConnectBtn.Image = Resources.connect;
                        this.toolBarConnectBtn.Text = "Disconnect";
                    }
                    else
                    {
                        this.toolBarConnectBtn.Image = Resources.disconnect;
                        this.toolBarConnectBtn.Text = "Connect";
                    }
                }
                else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C)
                {
                    if (this.comm.CMC.HostAppI2CSlave.IsOpen())
                    {
                        this.toolBarConnectBtn.Image = Resources.connect;
                        this.toolBarConnectBtn.Text = "Disconnect";
                    }
                    else
                    {
                        this.toolBarConnectBtn.Image = Resources.disconnect;
                        this.toolBarConnectBtn.Text = "Connect";
                    }
                }
            });
            this.frmCommOpenUpdateLogBtnImage();
            this.frmCommOpenUpdatePauseBtnImage();
            this.frmCommOpenUpdateStatusString();
        }

        private void frmCommOpenUpdateLogBtnImage()
        {
            EventHandler method = null;
            EventHandler handler2 = null;
            if (this.comm.Log.IsFileOpen())
            {
                if (method == null)
                {
                    method = delegate {
                        this.toolBarLogBtn.Image = Resources.stopLog;
                        this.toolBarLogBtn.Text = "Logging";
                        this.logFileName.Enabled = false;
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                if (handler2 == null)
                {
                    handler2 = delegate {
                        this.toolBarLogBtn.Image = Resources.log;
                        this.toolBarLogBtn.Text = "Idle";
                        this.logFileName.Enabled = true;
                    };
                }
                base.BeginInvoke(handler2);
            }
        }

        private void frmCommOpenUpdatePauseBtnImage()
        {
			base.BeginInvoke((MethodInvoker)delegate
			{
            });
        }

        private void frmCommOpenUpdateStatusString()
        {
            string statusStr = string.Empty;
            string cViewStr = string.Empty;
            switch (this.comm.RxCurrentTransmissionType)
            {
                case CommunicationManager.TransmissionType.Text:
                    cViewStr = "NMEA/Text";
                    break;

                case CommunicationManager.TransmissionType.Hex:
                    cViewStr = "Hex";
                    break;

                case CommunicationManager.TransmissionType.SSB:
                    cViewStr = "SSB";
                    break;

                case CommunicationManager.TransmissionType.GP2:
                    cViewStr = "GP2";
                    break;

                case CommunicationManager.TransmissionType.GPS:
                    cViewStr = "GPS";
                    break;

                default:
                    cViewStr = string.Empty;
                    break;
            }
			base.BeginInvoke((MethodInvoker)delegate
			{
                if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
                {
                    statusStr = string.Format("{0}[{1}:{2}:{3}:{4}] | Protocol: {5} | View: {6} ", new object[] { this.comm.PortName, this.comm.BaudRate, this.comm.Parity, this.comm.StopBits, this.comm.DataBits, this.comm.MessageProtocol, cViewStr });
                }
                else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client)
                {
                    statusStr = string.Format("TCP Client[{0}:{1}] | Protocol: {2} | View: {3} ", new object[] { this.comm.CMC.HostAppClient.TCPClientHostName, this.comm.CMC.HostAppClient.TCPClientPortNum, this.comm.MessageProtocol, cViewStr });
                }
                else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Server)
                {
                    statusStr = string.Format("TCP Client[{0}:{1}] | Protocol:{2} | View: {3} ", new object[] { this.comm.CMC.HostAppServer.TCPServerHostName, this.comm.CMC.HostAppServer.TCPServerPortNum, this.comm.MessageProtocol, cViewStr });
                }
                else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C)
                {
                    if (this.comm.CMC.HostAppI2CSlave.I2CTalkMode == CommMgrClass.I2CSlave.I2CCommMode.COMM_MODE_I2C_SLAVE)
                    {
                        statusStr = string.Format("I2C[{0}:{1}] | Protocol:{2} | View: {3} ", new object[] { this.comm.CMC.HostAppI2CSlave.I2CDevicePortNumMaster, this.comm.CMC.HostAppI2CSlave.I2CMasterAddress, this.comm.MessageProtocol, cViewStr });
                    }
                    else
                    {
                        statusStr = string.Format("I2C[{0}:{1}] | Protocol:{2} | View: {3} ", new object[] { this.comm.CMC.HostAppI2CSlave.I2CDevicePortNum, this.comm.CMC.HostAppI2CSlave.I2CSlaveAddress, this.comm.MessageProtocol, cViewStr });
                    }
                }
                this.toolStripStatusLabel1.Text = statusStr;
            });
        }

        private void frmSaveSettingsLoad_CommOpen(string filePath)
        {
            CommonUtilsClass class2 = new CommonUtilsClass();
            if (!File.Exists(filePath))
            {
                MessageBox.Show(string.Format("{0}\n not found use default", filePath), "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                filePath = this._defaultWindowsRestoredFilePath;
            }
            if (File.Exists(filePath))
            {
                try
                {
                    this._appWindowsSettings.Load(filePath);
                    foreach (XmlNode node in this._appWindowsSettings.SelectNodes("/windows/mainWindow"))
                    {
                        string str2;
                        string str4;
                        string str5;
                        if (((str2 = node.Attributes["name"].Value.ToString()) == null) || !(str2 == "frmCommOpen"))
                        {
                            goto Label_0706;
                        }
                        this.comm.MessageProtocol = node.Attributes["messageProtocol"].Value.ToString();
                        this.comm.PortName = node.Attributes["comport"].Value.ToString();
                        this.comm.BaudRate = node.Attributes["baud"].Value.ToString();
                        this.comm.CMC.HostAppClient.TCPClientPortNum = Convert.ToInt32(node.Attributes["TCPClientPortNum"].Value);
                        this.comm.CMC.HostAppClient.TCPClientHostName = node.Attributes["TCPClientHostName"].Value.ToString();
                        this.comm.CMC.HostAppServer.TCPServerPortNum = Convert.ToInt32(node.Attributes["TCPServerPortNum"].Value);
                        this.comm.CMC.HostAppServer.TCPServerHostName = node.Attributes["TCPServerHostName"].Value.ToString();
                        this.comm.TrackerPort = node.Attributes["TrackerPort"].Value.ToString();
                        this.comm.ResetPort = node.Attributes["ResetPort"].Value.ToString();
                        this.comm.HostPair1 = node.Attributes["HostPort1"].Value.ToString();
                        this.comm.HostSWFilePath = node.Attributes["HostAppFilePath"].Value.ToString();
                        this.comm.DefaultTCXOFreq = node.Attributes["DefaultTCXOFreq"].Value.ToString();
                        this.comm.LNAType = Convert.ToInt32(node.Attributes["LNAType"].Value.ToString());
                        this.comm.ReadBuffer = Convert.ToInt32(node.Attributes["ReadBuffer"].Value.ToString());
                        this.comm.LDOMode = Convert.ToInt32(node.Attributes["LDOMode"].Value.ToString());
                        this.comm.RxName = node.Attributes["RxName"].Value.ToString();
                        this.comm.EESelect = node.Attributes["EESelect"].Value.ToString();
                        this.comm.ServerName = node.Attributes["ServerName"].Value.ToString();
                        this.comm.ServerPort = node.Attributes["ServerPort"].Value.ToString();
                        this.comm.AuthenticationCode = node.Attributes["AuthenticationCode"].Value.ToString();
                        this.comm.EEDayNum = node.Attributes["EEDayNum"].Value.ToString();
                        this.comm.BankTime = node.Attributes["BankTime"].Value.ToString();
                        this.comm.ProductFamily = (CommonClass.ProductType) Convert.ToInt32(node.Attributes["ProdFamily"].Value.ToString());
                        if (node.Attributes["RequiredHostRun"].Value.ToString() == "1")
                        {
                            this.comm.RequireHostRun = true;
                            if (node.Attributes["RequireEE"].Value.ToString() == "True")
                            {
                                this.comm.RequireEE = true;
                            }
                            else
                            {
                                this.comm.RequireEE = false;
                            }
                        }
                        else
                        {
                            this.comm.RequireHostRun = false;
                        }
                        string str3 = node.Attributes["InputDeviceMode"].Value.ToString();
                        if (str3 == null)
                        {
                            goto Label_0544;
                        }
                        if (!(str3 == "1"))
                        {
                            if (str3 == "2")
                            {
                                goto Label_051A;
                            }
                            if (str3 == "3")
                            {
                                goto Label_0528;
                            }
                            if (str3 == "4")
                            {
                                goto Label_0536;
                            }
                            goto Label_0544;
                        }
                        this.comm.InputDeviceMode = CommonClass.InputDeviceModes.RS232;
                        goto Label_0550;
                    Label_051A:
                        this.comm.InputDeviceMode = CommonClass.InputDeviceModes.TCP_Client;
                        goto Label_0550;
                    Label_0528:
                        this.comm.InputDeviceMode = CommonClass.InputDeviceModes.TCP_Server;
                        goto Label_0550;
                    Label_0536:
                        this.comm.InputDeviceMode = CommonClass.InputDeviceModes.I2C;
                        goto Label_0550;
                    Label_0544:
                        this.comm.InputDeviceMode = CommonClass.InputDeviceModes.FilePlayBack;
                    Label_0550:
                        if ((str4 = node.Attributes["rxType"].Value.ToString()) != null)
                        {
                            if (!(str4 == "SLC"))
                            {
                                if (str4 == "GSW")
                                {
                                    goto Label_05B7;
                                }
                                if (str4 == "TTB")
                                {
                                    goto Label_05C5;
                                }
                                if (str4 == "NMEA")
                                {
                                    goto Label_05D3;
                                }
                            }
                            else
                            {
                                this.comm.RxType = CommunicationManager.ReceiverType.SLC;
                            }
                        }
                        goto Label_05DF;
                    Label_05B7:
                        this.comm.RxType = CommunicationManager.ReceiverType.GSW;
                        goto Label_05DF;
                    Label_05C5:
                        this.comm.RxType = CommunicationManager.ReceiverType.TTB;
                        goto Label_05DF;
                    Label_05D3:
                        this.comm.RxType = CommunicationManager.ReceiverType.NMEA;
                    Label_05DF:
                        if ((str5 = node.Attributes["viewType"].Value.ToString()) != null)
                        {
                            if (!(str5 == "CSV"))
                            {
                                if (str5 == "GP2")
                                {
                                    goto Label_0657;
                                }
                                if (str5 == "HEX")
                                {
                                    goto Label_0665;
                                }
                                if (str5 == "SSB")
                                {
                                    goto Label_0673;
                                }
                                if (str5 == "TEXT")
                                {
                                    goto Label_0681;
                                }
                            }
                            else
                            {
                                this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GPS;
                            }
                        }
                        goto Label_068D;
                    Label_0657:
                        this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GP2;
                        goto Label_068D;
                    Label_0665:
                        this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Hex;
                        goto Label_068D;
                    Label_0673:
                        this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.SSB;
                        goto Label_068D;
                    Label_0681:
                        this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Text;
                    Label_068D:
                        class2.DisplayBuffer = Convert.ToInt32(node.Attributes["bufferSize"].Value.ToString());
                        this.comm.AutoReplyCtrl.ControlChannelVersion = node.Attributes["controlVersion"].Value.ToString();
                        this.comm.AutoReplyCtrl.AidingProtocolVersion = node.Attributes["aidingVersion"].Value.ToString();
                    Label_0706:
                        this.loadLocation(this, node.Attributes["top"].Value.ToString(), node.Attributes["left"].Value.ToString(), node.Attributes["width"].Value.ToString(), node.Attributes["height"].Value.ToString(), node.Attributes["windowState"].Value.ToString());
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("frmMDIMain() + frmSaveSettingsLoad() " + exception.ToString());
                }
            }
            this.Refresh();
        }

        private void frmSaveSettingsOnClosing_CommOpen(string filePath)
        {
            StreamWriter writer;
            CommonUtilsClass class2 = new CommonUtilsClass();
            if (File.Exists(filePath))
            {
                if ((File.GetAttributes(filePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    MessageBox.Show(string.Format("File is read only - Window locations were not saved!\n{0}", filePath), "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.Cursor = Cursors.Default;
                    return;
                }
                writer = new StreamWriter(filePath);
            }
            else
            {
                writer = File.CreateText(filePath);
            }
            if (writer != null)
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                writer.WriteLine("<windows>");
                string str2 = string.Empty;
                int num = this.comm.RequireHostRun ? 1 : 0;
                int num2 = 0;
                if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
                {
                    num2 = 1;
                }
                else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client)
                {
                    num2 = 2;
                }
                else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Server)
                {
                    num2 = 3;
                }
                else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C)
                {
                    num2 = 4;
                }
                else
                {
                    num2 = 0;
                }
                string format = "<mainWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\" comport=\"{6}\" baud=\"{7}\" rxType=\"{8}\" messageProtocol=\"{9}\" viewType=\"{10}\" bufferSize=\"{11}\" controlVersion=\"{12}\" aidingVersion=\"{13}\" TCPClientPortNum=\"{14}\" TCPClientHostName=\"{15}\" TCPServerPortNum=\"{16}\" TCPServerHostName=\"{17}\" TrackerPort=\"{18}\" ResetPort=\"{19}\" HostPort1=\"{20}\" RequiredHostRun=\"{21}\" InputDeviceMode=\"{22}\" HostAppFilePath=\"{23}\" DefaultTCXOFreq=\"{24}\" LNAType=\"{25}\" ReadBuffer=\"{26}\" LDOMode=\"{27}\" RequireEE=\"{28}\" EESelect=\"{29}\" ServerName=\"{30}\" ServerPort=\"{31}\" AuthenticationCode=\"{32}\" EEDayNum=\"{33}\" BankTime=\"{34}\" ProdFamily=\"{35}\" RxName=\"{36}\" >";
                str2 = string.Format(format, new object[] { 
                    base.Name, base.Top.ToString(), base.Left.ToString(), base.Width.ToString(), base.Height.ToString(), base.WindowState.ToString(), this.comm.PortName, this.comm.BaudRate, this.comm.RxType, this.comm.MessageProtocol, this.comm.RxCurrentTransmissionType.ToString(), class2.DisplayBuffer.ToString(), this.comm.AutoReplyCtrl.ControlChannelVersion.ToString(), this.comm.AutoReplyCtrl.AidingProtocolVersion.ToString(), this.comm.CMC.HostAppClient.TCPClientPortNum, this.comm.CMC.HostAppClient.TCPClientHostName, 
                    this.comm.CMC.HostAppServer.TCPServerPortNum, this.comm.CMC.HostAppServer.TCPServerHostName, this.comm.TrackerPort, this.comm.ResetPort, this.comm.HostPair1, num, num2, this.comm.HostSWFilePath, this.comm.DefaultTCXOFreq, this.comm.LNAType, this.comm.ReadBuffer.ToString(), this.comm.LDOMode.ToString(), this.comm.RequireEE, this.comm.EESelect, this.comm.ServerName, this.comm.ServerPort, 
                    this.comm.AuthenticationCode, this.comm.EEDayNum, this.comm.BankTime, ((int) this.comm.ProductFamily).ToString(), this.comm.RxName
                 });
                writer.WriteLine(str2);
                writer.WriteLine("</mainWindow>");
                writer.WriteLine("</windows>");
                writer.Close();
            }
        }

        private void fromProtocolFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateInputCommandsWin();
        }

        private void generalToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void hWConfigRespToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!base.IsDisposed)
            {
                string str = this.comm.PortName + ": HW Config Response";
                if ((this._formautoReply == null) || this._formautoReply.IsDisposed)
                {
                    this._formautoReply = new frmAutoReply();
                }
                this._formautoReply.CommWindow = this.comm;
                this._formautoReply.Text = str;
                this._formautoReply.ShowDialog();
            }
            else
            {
                MessageBox.Show("COM window not initialized!", "Information");
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmCommOpen));
            this.frmCommDisplayColumn = new DataGridViewTextBoxColumn();
            this.frmCommOpenLogStatusLabel = new Label();
            this.markerText = new TextBox();
            this.label9 = new Label();
            this.logFileName = new TextBox();
            this.toolStrip1 = new ToolStrip();
            this.toolBarConnectBtn = new ToolStripButton();
            this.toolBarPauseBtn = new ToolStripButton();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.toolBarLogBtn = new ToolStripDropDownButton();
            this.toolBarLogBtnStartLogMenu = new ToolStripMenuItem();
            this.toolBarLogBtnStopLogMenu = new ToolStripMenuItem();
            this.toolBarLogBtnDurationLogMenu = new ToolStripMenuItem();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.toolBarResetBtn = new ToolStripButton();
            this.toolStripSeparator7 = new ToolStripSeparator();
            this.toolBarSignalViewBtn = new ToolStripButton();
            this.toolBarRadarBtn = new ToolStripButton();
            this.toolBarLocationBtn = new ToolStripButton();
            this.toolBarTTFFBtn = new ToolStripButton();
            this.toolStripButton1 = new ToolStripButton();
            this.toolBarResponseViewBtn = new ToolStripButton();
            this.toolBarErrorViewBtn = new ToolStripButton();
            this.frmCommOpenMenuStrip = new MenuStrip();
            this.rxSettingsMenuItem = new ToolStripMenuItem();
            this.rxViewModeMenuItem = new ToolStripMenuItem();
            this.viewModeMenuItem = new ToolStripMenuItem();
            this.viewModeHexMenu = new ToolStripMenuItem();
            this.viewModeNmeaTextMenu = new ToolStripMenuItem();
            this.viewModeSSBMenu = new ToolStripMenuItem();
            this.viewModeGP2Menu = new ToolStripMenuItem();
            this.viewModeGPSMenu = new ToolStripMenuItem();
            this.messagesMenuItem = new ToolStripMenuItem();
            this.allMessagesMenu = new ToolStripMenuItem();
            this.messagesFilterMenuItem = new ToolStripMenuItem();
            this.messagesFilterDebugMenu = new ToolStripMenuItem();
            this.messagesFilterResponseMenu = new ToolStripMenuItem();
            this.messageFilterGeneralMenu = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.locationMapMenu = new ToolStripMenuItem();
            this.signalViewMenu = new ToolStripMenuItem();
            this.radarMapMenu = new ToolStripMenuItem();
            this.cwInterfenceDetectionMenu = new ToolStripMenuItem();
            this.lowPowerCommandBufferMenu = new ToolStripMenuItem();
            this.satelliteStatisticsMenuItem = new ToolStripMenuItem();
            this.SiRFAwareMenuItem = new ToolStripMenuItem();
            this.TTFFMenuItem = new ToolStripMenuItem();
            this.responseMenuItem = new ToolStripMenuItem();
            this.errorToolStripMenuItem = new ToolStripMenuItem();
            this.rxCommandsMenuItem = new ToolStripMenuItem();
            this.resetMenu = new ToolStripMenuItem();
            this.pollSWVersionMenu = new ToolStripMenuItem();
            this.pollAlmanacMenu = new ToolStripMenuItem();
            this.pollEphemerisMenu = new ToolStripMenuItem();
            this.pollNavParametersMenu = new ToolStripMenuItem();
            this.toolStripMenuItem_SetAlm = new ToolStripMenuItem();
            this.toolStripMenuItem_SetEph = new ToolStripMenuItem();
            this.setDevelopersDebugMenu = new ToolStripMenuItem();
            this.switchOperationModeMenu = new ToolStripMenuItem();
            this.lowPowerMenu = new ToolStripMenuItem();
            this.setMEMSMenuItem = new ToolStripMenuItem();
            this.setMEMSEnableMenu = new ToolStripMenuItem();
            this.setMEMSDisableMenu = new ToolStripMenuItem();
            this.setABPMenuItem = new ToolStripMenuItem();
            this.setABPEnableMenu = new ToolStripMenuItem();
            this.setABPDisableMenu = new ToolStripMenuItem();
            this.switchProtocolMenu = new ToolStripMenuItem();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.trackerConfigurationMenu = new ToolStripMenuItem();
            this.TrackerConfigVer2 = new ToolStripMenuItem();
            this.trackerICPeekPokeMenu = new ToolStripMenuItem();
            this.inputCommandMenuItem = new ToolStripMenuItem();
            this.predefinedMessageMenu = new ToolStripMenuItem();
            this.userInputMenu = new ToolStripMenuItem();
            this.rxSessionMenuItem = new ToolStripMenuItem();
            this.openSessionMenu = new ToolStripMenuItem();
            this.closeSessionMenu = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.autoReplySettingsMenu = new ToolStripMenuItem();
            this.autoReplySummaryMenu = new ToolStripMenuItem();
            this.toolStripSeparator6 = new ToolStripSeparator();
            this.timeFreqApproxPosStatusRequest = new ToolStripMenuItem();
            this.TTBMenuItem = new ToolStripMenuItem();
            this.rxTTBConnectMenu = new ToolStripMenuItem();
            this.rxTTBConfigTimeAiding = new ToolStripMenuItem();
            this.rxTTBViewMenu = new ToolStripMenuItem();
            this.errorLogConfigMenuItem = new ToolStripMenuItem();
            this.frmCommOpenMarkerLabel = new Label();
            this.frmCommOpenDurationLoggingStatusLabel = new Label();
            this._displayDataBG = new BackgroundWorker();
            this._parseDataBG = new BackgroundWorker();
            this._readPortDataBG = new BackgroundWorker();
            this.toolTip1 = new ToolTip(this.components);
            this.statusStrip1 = new StatusStrip();
            this.toolStripStatusLabel1 = new ToolStripStatusLabel();
            this.rtbDisplay = new CommonClass.MyRichTextBox();
            this.label_autoDetect = new Label();
            this.button1 = new Button();
            this.btn_logFileBroswer = new Button();
            this.toolStrip1.SuspendLayout();
            this.frmCommOpenMenuStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            base.SuspendLayout();
            this.frmCommDisplayColumn.HeaderText = "data";
            this.frmCommDisplayColumn.Name = "frmCommDisplayColumn";
            this.frmCommDisplayColumn.ReadOnly = true;
            this.frmCommDisplayColumn.Width = 0x1388;
            this.frmCommOpenLogStatusLabel.AutoSize = true;
            this.frmCommOpenLogStatusLabel.Location = new Point(0x15, 0x65);
            this.frmCommOpenLogStatusLabel.Margin = new Padding(4, 0, 4, 0);
            this.frmCommOpenLogStatusLabel.Name = "frmCommOpenLogStatusLabel";
            this.frmCommOpenLogStatusLabel.Size = new Size(0x1b, 13);
            this.frmCommOpenLogStatusLabel.TabIndex = 2;
            this.frmCommOpenLogStatusLabel.Text = " Idle";
            this.markerText.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.markerText.Location = new Point(0x6c, 0x76);
            this.markerText.Name = "markerText";
            this.markerText.Size = new Size(0x1b9, 20);
            this.markerText.TabIndex = 5;
            this.markerText.PreviewKeyDown += new PreviewKeyDownEventHandler(this.markerText_PreviewKeyDown);
            this.markerText.MouseEnter += new EventHandler(this.markerText_MouseEnter);
            this.label9.AutoSize = true;
            this.label9.BackColor = SystemColors.Control;
            this.label9.Location = new Point(0x15, 0x53);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x48, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Log File Path:";
            this.logFileName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.logFileName.Location = new Point(0x6c, 0x4f);
            this.logFileName.Name = "logFileName";
            this.logFileName.Size = new Size(0x18a, 20);
            this.logFileName.TabIndex = 3;
            this.logFileName.Text = "log.txt";
            this.logFileName.PreviewKeyDown += new PreviewKeyDownEventHandler(this.logFileName_PreviewKeyDown);
            this.toolStrip1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = SystemColors.Menu;
            this.toolStrip1.Dock = DockStyle.None;
            this.toolStrip1.GripMargin = new Padding(2, 2, 0, 2);
            this.toolStrip1.ImageScalingSize = new Size(0x12, 0x12);
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.toolBarConnectBtn, this.toolBarPauseBtn, this.toolStripSeparator4, this.toolBarLogBtn, this.toolStripSeparator5, this.toolBarResetBtn, this.toolStripSeparator7, this.toolBarSignalViewBtn, this.toolBarRadarBtn, this.toolBarLocationBtn, this.toolBarTTFFBtn, this.toolStripButton1, this.toolBarResponseViewBtn, this.toolBarErrorViewBtn });
            this.toolStrip1.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new Point(0, 0x1a);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x23f, 0x19);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0x11;
            this.toolStrip1.Text = "User Action";
            this.toolBarConnectBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolBarConnectBtn.Image = Resources.disconnect;
            this.toolBarConnectBtn.ImageTransparentColor = Color.Transparent;
            this.toolBarConnectBtn.Name = "toolBarConnectBtn";
            this.toolBarConnectBtn.Size = new Size(0x17, 0x16);
            this.toolBarConnectBtn.Text = "Connect";
            this.toolBarConnectBtn.Click += new EventHandler(this.toolBarConnectBtn_Click);
            this.toolBarPauseBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolBarPauseBtn.Image = Resources.Pause;
            this.toolBarPauseBtn.ImageTransparentColor = Color.Transparent;
            this.toolBarPauseBtn.Name = "toolBarPauseBtn";
            this.toolBarPauseBtn.Size = new Size(0x17, 0x16);
            this.toolBarPauseBtn.Text = "Pause";
            this.toolBarPauseBtn.Click += new EventHandler(this.toolBarPauseBtn_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(6, 0x19);
            this.toolBarLogBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolBarLogBtn.DropDownItems.AddRange(new ToolStripItem[] { this.toolBarLogBtnStartLogMenu, this.toolBarLogBtnStopLogMenu, this.toolBarLogBtnDurationLogMenu });
            this.toolBarLogBtn.Image = Resources.log;
            this.toolBarLogBtn.ImageTransparentColor = Color.Magenta;
            this.toolBarLogBtn.Name = "toolBarLogBtn";
            this.toolBarLogBtn.Size = new Size(0x1f, 0x16);
            this.toolBarLogBtn.Text = "Logging";
            this.toolBarLogBtnStartLogMenu.Name = "toolBarLogBtnStartLogMenu";
            this.toolBarLogBtnStartLogMenu.Size = new Size(0xb8, 0x16);
            this.toolBarLogBtnStartLogMenu.Text = "Start Log";
            this.toolBarLogBtnStartLogMenu.Click += new EventHandler(this.toolBarLogBtnStartLogMenu_Click);
            this.toolBarLogBtnStopLogMenu.Name = "toolBarLogBtnStopLogMenu";
            this.toolBarLogBtnStopLogMenu.Size = new Size(0xb8, 0x16);
            this.toolBarLogBtnStopLogMenu.Text = "Stop Log";
            this.toolBarLogBtnStopLogMenu.Click += new EventHandler(this.toolBarLogBtnStopLogMenu_Click);
            this.toolBarLogBtnDurationLogMenu.Name = "toolBarLogBtnDurationLogMenu";
            this.toolBarLogBtnDurationLogMenu.Size = new Size(0xb8, 0x16);
            this.toolBarLogBtnDurationLogMenu.Text = "Log for a Duration...";
            this.toolBarLogBtnDurationLogMenu.Click += new EventHandler(this.toolBarLogBtnDurationLogMenu_Click);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new Size(6, 0x19);
            this.toolBarResetBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolBarResetBtn.Image = (Image) resources.GetObject("toolBarResetBtn.Image");
            this.toolBarResetBtn.ImageTransparentColor = Color.Magenta;
            this.toolBarResetBtn.Name = "toolBarResetBtn";
            this.toolBarResetBtn.Size = new Size(0x17, 0x16);
            this.toolBarResetBtn.Text = "Reset";
            this.toolBarResetBtn.Click += new EventHandler(this.toolBarResetBtn_Click);
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new Size(6, 0x19);
            this.toolBarSignalViewBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolBarSignalViewBtn.Image = Resources.signal;
            this.toolBarSignalViewBtn.ImageTransparentColor = Color.Magenta;
            this.toolBarSignalViewBtn.Name = "toolBarSignalViewBtn";
            this.toolBarSignalViewBtn.Size = new Size(0x17, 0x16);
            this.toolBarSignalViewBtn.Text = "Signal View";
            this.toolBarSignalViewBtn.Click += new EventHandler(this.toolBarSignalViewBtn_Click);
            this.toolBarRadarBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolBarRadarBtn.Image = Resources.radar;
            this.toolBarRadarBtn.ImageTransparentColor = Color.Magenta;
            this.toolBarRadarBtn.Name = "toolBarRadarBtn";
            this.toolBarRadarBtn.Size = new Size(0x17, 0x16);
            this.toolBarRadarBtn.Text = "Radar View";
            this.toolBarRadarBtn.Click += new EventHandler(this.toolBarRadarBtn_Click);
            this.toolBarLocationBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolBarLocationBtn.Image = Resources.map;
            this.toolBarLocationBtn.ImageTransparentColor = Color.Transparent;
            this.toolBarLocationBtn.Name = "toolBarLocationBtn";
            this.toolBarLocationBtn.Size = new Size(0x17, 0x16);
            this.toolBarLocationBtn.Text = "Location View";
            this.toolBarLocationBtn.Click += new EventHandler(this.toolBarLocationBtn_Click);
            this.toolBarTTFFBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolBarTTFFBtn.Image = Resources.ttff;
            this.toolBarTTFFBtn.ImageTransparentColor = Color.Magenta;
            this.toolBarTTFFBtn.Name = "toolBarTTFFBtn";
            this.toolBarTTFFBtn.Size = new Size(0x17, 0x16);
            this.toolBarTTFFBtn.Text = "TTFF/Nav Accuracy";
            this.toolBarTTFFBtn.Click += new EventHandler(this.toolBarTTFFBtn_Click);
            this.toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = Resources.synchronization;
            this.toolStripButton1.ImageTransparentColor = Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new Size(0x17, 0x16);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Visible = false;
            this.toolStripButton1.Click += new EventHandler(this.toolStripButton1_Click);
            this.toolBarResponseViewBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolBarResponseViewBtn.Image = Resources.ResponseViewHS;
            this.toolBarResponseViewBtn.ImageTransparentColor = Color.Magenta;
            this.toolBarResponseViewBtn.Name = "toolBarResponseViewBtn";
            this.toolBarResponseViewBtn.Size = new Size(0x17, 0x16);
            this.toolBarResponseViewBtn.Text = "Response View";
            this.toolBarResponseViewBtn.Click += new EventHandler(this.toolBarResponseViewBtn_Click);
            this.toolBarErrorViewBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolBarErrorViewBtn.Image = Resources.ErrorHS;
            this.toolBarErrorViewBtn.ImageTransparentColor = Color.Magenta;
            this.toolBarErrorViewBtn.Name = "toolBarErrorViewBtn";
            this.toolBarErrorViewBtn.Size = new Size(0x17, 0x16);
            this.toolBarErrorViewBtn.Text = "toolStripButton2";
            this.toolBarErrorViewBtn.Click += new EventHandler(this.toolBarErrorViewBtn_Click);
            this.frmCommOpenMenuStrip.AllowMerge = false;
            this.frmCommOpenMenuStrip.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.frmCommOpenMenuStrip.AutoSize = false;
            this.frmCommOpenMenuStrip.BackColor = SystemColors.Menu;
            this.frmCommOpenMenuStrip.Dock = DockStyle.None;
            this.frmCommOpenMenuStrip.GripStyle = ToolStripGripStyle.Visible;
            this.frmCommOpenMenuStrip.Items.AddRange(new ToolStripItem[] { this.rxSettingsMenuItem, this.rxViewModeMenuItem, this.rxCommandsMenuItem, this.rxSessionMenuItem, this.TTBMenuItem, this.errorLogConfigMenuItem });
            this.frmCommOpenMenuStrip.Location = new Point(0, 0);
            this.frmCommOpenMenuStrip.MinimumSize = new Size(0x1ce, 0);
            this.frmCommOpenMenuStrip.Name = "frmCommOpenMenuStrip";
            this.frmCommOpenMenuStrip.Size = new Size(0x23f, 0x18);
            this.frmCommOpenMenuStrip.TabIndex = 0x12;
            this.frmCommOpenMenuStrip.Text = "frmCommOpenMenuStrip";
            this.rxSettingsMenuItem.Name = "rxSettingsMenuItem";
            this.rxSettingsMenuItem.Size = new Size(0x4a, 20);
            this.rxSettingsMenuItem.Text = "&Rx Settings";
            this.rxSettingsMenuItem.Click += new EventHandler(this.rxSettingsMenuItem_Click);
            this.rxViewModeMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.viewModeMenuItem, this.messagesMenuItem, this.toolStripSeparator2, this.locationMapMenu, this.signalViewMenu, this.radarMapMenu, this.cwInterfenceDetectionMenu, this.lowPowerCommandBufferMenu, this.satelliteStatisticsMenuItem, this.SiRFAwareMenuItem, this.TTFFMenuItem, this.responseMenuItem, this.errorToolStripMenuItem });
            this.rxViewModeMenuItem.Name = "rxViewModeMenuItem";
            this.rxViewModeMenuItem.Size = new Size(60, 20);
            this.rxViewModeMenuItem.Text = "Rx &View ";
            this.rxViewModeMenuItem.Click += new EventHandler(this.rxViewModeMenuItem_Click);
            this.viewModeMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.viewModeHexMenu, this.viewModeNmeaTextMenu, this.viewModeSSBMenu, this.viewModeGP2Menu, this.viewModeGPSMenu });
            this.viewModeMenuItem.Name = "viewModeMenuItem";
            this.viewModeMenuItem.Size = new Size(0xe8, 0x16);
            this.viewModeMenuItem.Text = "&View Mode";
            this.viewModeMenuItem.MouseHover += new EventHandler(this.viewModeToolStripMenuItem_Click);
            this.viewModeHexMenu.CheckOnClick = true;
            this.viewModeHexMenu.Name = "viewModeHexMenu";
            this.viewModeHexMenu.Size = new Size(0x8b, 0x16);
            this.viewModeHexMenu.Text = "&Hex";
            this.viewModeHexMenu.Click += new EventHandler(this.viewModeHexMenu_Click);
            this.viewModeNmeaTextMenu.CheckOnClick = true;
            this.viewModeNmeaTextMenu.Name = "viewModeNmeaTextMenu";
            this.viewModeNmeaTextMenu.Size = new Size(0x8b, 0x16);
            this.viewModeNmeaTextMenu.Text = "&NMEA/Text";
            this.viewModeNmeaTextMenu.Click += new EventHandler(this.viewModeNmeaTextMenu_Click);
            this.viewModeSSBMenu.CheckOnClick = true;
            this.viewModeSSBMenu.Name = "viewModeSSBMenu";
            this.viewModeSSBMenu.Size = new Size(0x8b, 0x16);
            this.viewModeSSBMenu.Text = "&SSB";
            this.viewModeSSBMenu.Click += new EventHandler(this.viewModeSSBMenu_Click);
            this.viewModeGP2Menu.CheckOnClick = true;
            this.viewModeGP2Menu.Name = "viewModeGP2Menu";
            this.viewModeGP2Menu.Size = new Size(0x8b, 0x16);
            this.viewModeGP2Menu.Text = "&GP2";
            this.viewModeGP2Menu.Click += new EventHandler(this.viewModeGP2Menu_Click);
            this.viewModeGPSMenu.CheckOnClick = true;
            this.viewModeGPSMenu.Name = "viewModeGPSMenu";
            this.viewModeGPSMenu.Size = new Size(0x8b, 0x16);
            this.viewModeGPSMenu.Text = "G&PS";
            this.viewModeGPSMenu.Click += new EventHandler(this.viewModeGPSMenu_Click);
            this.messagesMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.allMessagesMenu, this.messagesFilterMenuItem });
            this.messagesMenuItem.Name = "messagesMenuItem";
            this.messagesMenuItem.Size = new Size(0xe8, 0x16);
            this.messagesMenuItem.Text = "&Messages";
            this.allMessagesMenu.CheckOnClick = true;
            this.allMessagesMenu.Name = "allMessagesMenu";
            this.allMessagesMenu.Size = new Size(0x92, 0x16);
            this.allMessagesMenu.Text = "&All Messages";
            this.allMessagesMenu.Click += new EventHandler(this.allMessagesMenu_Click);
            this.messagesFilterMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.messagesFilterDebugMenu, this.messagesFilterResponseMenu, this.messageFilterGeneralMenu });
            this.messagesFilterMenuItem.Name = "messagesFilterMenuItem";
            this.messagesFilterMenuItem.Size = new Size(0x92, 0x16);
            this.messagesFilterMenuItem.Text = "Filter";
            this.messagesFilterDebugMenu.Name = "messagesFilterDebugMenu";
            this.messagesFilterDebugMenu.Size = new Size(0x90, 0x16);
            this.messagesFilterDebugMenu.Text = "Debug...";
            this.messagesFilterDebugMenu.Click += new EventHandler(this.messagesFilterDebugMenu_Click);
            this.messagesFilterResponseMenu.Name = "messagesFilterResponseMenu";
            this.messagesFilterResponseMenu.Size = new Size(0x90, 0x16);
            this.messagesFilterResponseMenu.Text = "Response...";
            this.messagesFilterResponseMenu.Click += new EventHandler(this.messagesFilterResponseMenu_Click);
            this.messageFilterGeneralMenu.Name = "messageFilterGeneralMenu";
            this.messageFilterGeneralMenu.Size = new Size(0x90, 0x16);
            this.messageFilterGeneralMenu.Text = "General...";
            this.messageFilterGeneralMenu.Click += new EventHandler(this.messageFilterGeneralMenu_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(0xe5, 6);
            this.locationMapMenu.Name = "locationMapMenu";
            this.locationMapMenu.Size = new Size(0xe8, 0x16);
            this.locationMapMenu.Text = "&Location...";
            this.locationMapMenu.Click += new EventHandler(this.locationMapMenu_Click);
            this.signalViewMenu.Name = "signalViewMenu";
            this.signalViewMenu.Size = new Size(0xe8, 0x16);
            this.signalViewMenu.Text = "Si&gnal";
            this.signalViewMenu.Click += new EventHandler(this.signalViewMenu_Click);
            this.radarMapMenu.Name = "radarMapMenu";
            this.radarMapMenu.Size = new Size(0xe8, 0x16);
            this.radarMapMenu.Text = "Ra&dar";
            this.radarMapMenu.Click += new EventHandler(this.radarMapMenu_Click);
            this.cwInterfenceDetectionMenu.Name = "cwInterfenceDetectionMenu";
            this.cwInterfenceDetectionMenu.Size = new Size(0xe8, 0x16);
            this.cwInterfenceDetectionMenu.Text = "&CW Interference Detection...";
            this.cwInterfenceDetectionMenu.Click += new EventHandler(this.cwInterfenceDetectionMenu_Click);
            this.lowPowerCommandBufferMenu.Name = "lowPowerCommandBufferMenu";
            this.lowPowerCommandBufferMenu.Size = new Size(0xe8, 0x16);
            this.lowPowerCommandBufferMenu.Text = "Low Power Command &Buffer...";
            this.lowPowerCommandBufferMenu.Click += new EventHandler(this.lowPowerCommandBufferMenu_Click);
            this.satelliteStatisticsMenuItem.Name = "satelliteStatisticsMenuItem";
            this.satelliteStatisticsMenuItem.Size = new Size(0xe8, 0x16);
            this.satelliteStatisticsMenuItem.Text = "Satellite &Statistics...";
            this.satelliteStatisticsMenuItem.Click += new EventHandler(this.satelliteStatisticsMenuItem_Click);
            this.SiRFAwareMenuItem.Name = "SiRFAwareMenuItem";
            this.SiRFAwareMenuItem.Size = new Size(0xe8, 0x16);
            this.SiRFAwareMenuItem.Text = "SiRF&aware...";
            this.SiRFAwareMenuItem.Click += new EventHandler(this.SiRFAwareMenuItem_Click);
            this.TTFFMenuItem.Name = "TTFFMenuItem";
            this.TTFFMenuItem.Size = new Size(0xe8, 0x16);
            this.TTFFMenuItem.Text = "&TTFF...";
            this.TTFFMenuItem.Click += new EventHandler(this.TTFFMenuItem_Click);
            this.responseMenuItem.Name = "responseMenuItem";
            this.responseMenuItem.Size = new Size(0xe8, 0x16);
            this.responseMenuItem.Text = "&Response";
            this.responseMenuItem.Click += new EventHandler(this.responseMenuItem_Click);
            this.errorToolStripMenuItem.Name = "errorToolStripMenuItem";
            this.errorToolStripMenuItem.Size = new Size(0xe8, 0x16);
            this.errorToolStripMenuItem.Text = "&Error";
            this.errorToolStripMenuItem.Click += new EventHandler(this.errorToolStripMenuItem_Click);
            this.rxCommandsMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 
                this.resetMenu, this.pollSWVersionMenu, this.pollAlmanacMenu, this.pollEphemerisMenu, this.pollNavParametersMenu, this.toolStripMenuItem_SetAlm, this.toolStripMenuItem_SetEph, this.setDevelopersDebugMenu, this.switchOperationModeMenu, this.lowPowerMenu, this.setMEMSMenuItem, this.setABPMenuItem, this.switchProtocolMenu, this.toolStripSeparator3, this.trackerConfigurationMenu, this.TrackerConfigVer2, 
                this.trackerICPeekPokeMenu, this.inputCommandMenuItem
             });
            this.rxCommandsMenuItem.Name = "rxCommandsMenuItem";
            this.rxCommandsMenuItem.Size = new Size(0x57, 20);
            this.rxCommandsMenuItem.Text = "Rx &Commands";
            this.rxCommandsMenuItem.Click += new EventHandler(this.rxCommandsToolStripMenuItem_Click);
            this.resetMenu.Name = "resetMenu";
            this.resetMenu.Size = new Size(0xe5, 0x16);
            this.resetMenu.Text = "&Reset...";
            this.resetMenu.Click += new EventHandler(this.resetMenu_Click);
            this.pollSWVersionMenu.Name = "pollSWVersionMenu";
            this.pollSWVersionMenu.Size = new Size(0xe5, 0x16);
            this.pollSWVersionMenu.Text = "Poll S/W &Version";
            this.pollSWVersionMenu.Click += new EventHandler(this.pollSWVersionMenu_Click);
            this.pollAlmanacMenu.Name = "pollAlmanacMenu";
            this.pollAlmanacMenu.Size = new Size(0xe5, 0x16);
            this.pollAlmanacMenu.Text = "Poll &Almanac...";
            this.pollAlmanacMenu.Click += new EventHandler(this.pollAlmanacMenu_Click);
            this.pollEphemerisMenu.Name = "pollEphemerisMenu";
            this.pollEphemerisMenu.Size = new Size(0xe5, 0x16);
            this.pollEphemerisMenu.Text = "Poll &Ephemeris...";
            this.pollEphemerisMenu.Click += new EventHandler(this.pollEphemerisMenu_Click);
            this.pollNavParametersMenu.Name = "pollNavParametersMenu";
            this.pollNavParametersMenu.Size = new Size(0xe5, 0x16);
            this.pollNavParametersMenu.Text = "Poll Nav Parameters...";
            this.pollNavParametersMenu.Click += new EventHandler(this.pollNavParametersMenu_Click);
            this.toolStripMenuItem_SetAlm.Name = "toolStripMenuItem_SetAlm";
            this.toolStripMenuItem_SetAlm.Size = new Size(0xe5, 0x16);
            this.toolStripMenuItem_SetAlm.Text = "Set Almanac...";
            this.toolStripMenuItem_SetAlm.Click += new EventHandler(this.toolStripMenuItem_SetAlm_Click);
            this.toolStripMenuItem_SetEph.Name = "toolStripMenuItem_SetEph";
            this.toolStripMenuItem_SetEph.Size = new Size(0xe5, 0x16);
            this.toolStripMenuItem_SetEph.Text = "Set Ephemeris...";
            this.toolStripMenuItem_SetEph.Click += new EventHandler(this.toolStripMenuItem_SetEph_Click_1);
            this.setDevelopersDebugMenu.Name = "setDevelopersDebugMenu";
            this.setDevelopersDebugMenu.Size = new Size(0xe5, 0x16);
            this.setDevelopersDebugMenu.Text = "Set &Developers Debug...";
            this.setDevelopersDebugMenu.Click += new EventHandler(this.setDevelopersDebugMenu_Click);
            this.switchOperationModeMenu.Name = "switchOperationModeMenu";
            this.switchOperationModeMenu.Size = new Size(0xe5, 0x16);
            this.switchOperationModeMenu.Text = "Switch &Operation Mode...";
            this.switchOperationModeMenu.Click += new EventHandler(this.switchOperationModeMenu_Click);
            this.lowPowerMenu.Name = "lowPowerMenu";
            this.lowPowerMenu.Size = new Size(0xe5, 0x16);
            this.lowPowerMenu.Text = "Switch &Power Mode...";
            this.lowPowerMenu.Click += new EventHandler(this.lowPowerMenu_Click);
            this.setMEMSMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.setMEMSEnableMenu, this.setMEMSDisableMenu });
            this.setMEMSMenuItem.Name = "setMEMSMenuItem";
            this.setMEMSMenuItem.Size = new Size(0xe5, 0x16);
            this.setMEMSMenuItem.Text = "&MEMS";
            this.setMEMSMenuItem.MouseHover += new EventHandler(this.setMEMSToolStripMenuItem_MouseHover);
            this.setMEMSEnableMenu.Name = "setMEMSEnableMenu";
            this.setMEMSEnableMenu.Size = new Size(0x77, 0x16);
            this.setMEMSEnableMenu.Text = "&Enable";
            this.setMEMSEnableMenu.Click += new EventHandler(this.setMEMSEnableMenu_Click);
            this.setMEMSDisableMenu.Name = "setMEMSDisableMenu";
            this.setMEMSDisableMenu.Size = new Size(0x77, 0x16);
            this.setMEMSDisableMenu.Text = "&Disable";
            this.setMEMSDisableMenu.Click += new EventHandler(this.setMEMSDisableMenu_Click);
            this.setABPMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.setABPEnableMenu, this.setABPDisableMenu });
            this.setABPMenuItem.Name = "setABPMenuItem";
            this.setABPMenuItem.Size = new Size(0xe5, 0x16);
            this.setABPMenuItem.Text = "Set A&BP Mode";
            this.setABPMenuItem.MouseHover += new EventHandler(this.setABPToolStripMenuItem_MouseHover);
            this.setABPEnableMenu.Name = "setABPEnableMenu";
            this.setABPEnableMenu.Size = new Size(0x77, 0x16);
            this.setABPEnableMenu.Text = "&Enable";
            this.setABPEnableMenu.Click += new EventHandler(this.setABPMenuItemEnable_Click);
            this.setABPDisableMenu.Name = "setABPDisableMenu";
            this.setABPDisableMenu.Size = new Size(0x77, 0x16);
            this.setABPDisableMenu.Text = "&Disable";
            this.setABPDisableMenu.Click += new EventHandler(this.setABPDisableMenu_Click);
            this.switchProtocolMenu.BackColor = SystemColors.Control;
            this.switchProtocolMenu.Name = "switchProtocolMenu";
            this.switchProtocolMenu.Size = new Size(0xe5, 0x16);
            this.switchProtocolMenu.Text = "Switch Pro&tocol...";
            this.switchProtocolMenu.Click += new EventHandler(this.switchProtocolMenu_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(0xe2, 6);
            this.trackerConfigurationMenu.Name = "trackerConfigurationMenu";
            this.trackerConfigurationMenu.Size = new Size(0xe5, 0x16);
            this.trackerConfigurationMenu.Text = "Tracker &Configuration_Ver1...";
            this.trackerConfigurationMenu.Visible = false;
            this.trackerConfigurationMenu.Click += new EventHandler(this.trackerConfigurationMenu_Click);
            this.TrackerConfigVer2.Name = "TrackerConfigVer2";
            this.TrackerConfigVer2.Size = new Size(0xe5, 0x16);
            this.TrackerConfigVer2.Text = "IC Configuration...";
            this.TrackerConfigVer2.Click += new EventHandler(this.TrackerConfigVer2_Click);
            this.trackerICPeekPokeMenu.Name = "trackerICPeekPokeMenu";
            this.trackerICPeekPokeMenu.Size = new Size(0xe5, 0x16);
            this.trackerICPeekPokeMenu.Text = "Tracker IC Pee&k Poke...";
            this.trackerICPeekPokeMenu.Click += new EventHandler(this.trackerICPeekPokeMenu_Click);
            this.inputCommandMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.predefinedMessageMenu, this.userInputMenu });
            this.inputCommandMenuItem.Name = "inputCommandMenuItem";
            this.inputCommandMenuItem.Size = new Size(0xe5, 0x16);
            this.inputCommandMenuItem.Text = "&Advanced Input Command";
            this.predefinedMessageMenu.Name = "predefinedMessageMenu";
            this.predefinedMessageMenu.Size = new Size(0xc7, 0x16);
            this.predefinedMessageMenu.Text = "Predefined Messages...";
            this.predefinedMessageMenu.Click += new EventHandler(this.predefinedMessageMenu_Click);
            this.userInputMenu.Name = "userInputMenu";
            this.userInputMenu.Size = new Size(0xc7, 0x16);
            this.userInputMenu.Text = "User Input...";
            this.userInputMenu.Click += new EventHandler(this.userInputMenu_Click);
            this.rxSessionMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.openSessionMenu, this.closeSessionMenu, this.toolStripSeparator1, this.autoReplySettingsMenu, this.autoReplySummaryMenu, this.toolStripSeparator6, this.timeFreqApproxPosStatusRequest });
            this.rxSessionMenuItem.Name = "rxSessionMenuItem";
            this.rxSessionMenuItem.Size = new Size(0x47, 20);
            this.rxSessionMenuItem.Text = "Rx &Session";
            this.rxSessionMenuItem.Click += new EventHandler(this.rxSessionToolStripMenuItem_Click);
            this.openSessionMenu.Name = "openSessionMenu";
            this.openSessionMenu.Size = new Size(0x108, 0x16);
            this.openSessionMenu.Text = "&Open Session...";
            this.openSessionMenu.Click += new EventHandler(this.openSessionMenu_Click);
            this.closeSessionMenu.Name = "closeSessionMenu";
            this.closeSessionMenu.Size = new Size(0x108, 0x16);
            this.closeSessionMenu.Text = "&Close Session...";
            this.closeSessionMenu.Click += new EventHandler(this.closeSessionMenu_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(0x105, 6);
            this.autoReplySettingsMenu.Name = "autoReplySettingsMenu";
            this.autoReplySettingsMenu.Size = new Size(0x108, 0x16);
            this.autoReplySettingsMenu.Text = "&Auto Reply Settings...";
            this.autoReplySettingsMenu.Click += new EventHandler(this.autoReplySettingsMenu_Click);
            this.autoReplySummaryMenu.Name = "autoReplySummaryMenu";
            this.autoReplySummaryMenu.Size = new Size(0x108, 0x16);
            this.autoReplySummaryMenu.Text = "Auto Reply &Summary";
            this.autoReplySummaryMenu.Click += new EventHandler(this.autoReplySummaryMenu_Click);
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new Size(0x105, 6);
            this.timeFreqApproxPosStatusRequest.Name = "timeFreqApproxPosStatusRequest";
            this.timeFreqApproxPosStatusRequest.Size = new Size(0x108, 0x16);
            this.timeFreqApproxPosStatusRequest.Text = "&Time Freq ApproxPos Status Request";
            this.TTBMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.rxTTBConnectMenu, this.rxTTBConfigTimeAiding, this.rxTTBViewMenu });
            this.TTBMenuItem.Name = "TTBMenuItem";
            this.TTBMenuItem.Size = new Size(0x35, 20);
            this.TTBMenuItem.Text = "Rx &TTB";
            this.TTBMenuItem.Click += new EventHandler(this.TTBToolStripMenuItem_Click);
            this.rxTTBConnectMenu.Name = "rxTTBConnectMenu";
            this.rxTTBConnectMenu.Size = new Size(0xb9, 0x16);
            this.rxTTBConnectMenu.Text = "&Connect...";
            this.rxTTBConnectMenu.Click += new EventHandler(this.rxTTBConnectMenu_Click);
            this.rxTTBConfigTimeAiding.Name = "rxTTBConfigTimeAiding";
            this.rxTTBConfigTimeAiding.Size = new Size(0xb9, 0x16);
            this.rxTTBConfigTimeAiding.Text = "Config &Time Aiding...";
            this.rxTTBConfigTimeAiding.Click += new EventHandler(this.rxTTBConfigTimeAiding_Click);
            this.rxTTBViewMenu.Name = "rxTTBViewMenu";
            this.rxTTBViewMenu.Size = new Size(0xb9, 0x16);
            this.rxTTBViewMenu.Text = "&View...";
            this.rxTTBViewMenu.Click += new EventHandler(this.rxTTBViewMenu_Click);
            this.errorLogConfigMenuItem.Name = "errorLogConfigMenuItem";
            this.errorLogConfigMenuItem.Size = new Size(0x61, 20);
            this.errorLogConfigMenuItem.Text = "&Error Log Config";
            this.errorLogConfigMenuItem.Click += new EventHandler(this.errorLogConfigMenuItem_Click);
            this.frmCommOpenMarkerLabel.AutoSize = true;
            this.frmCommOpenMarkerLabel.Location = new Point(0x15, 0x7a);
            this.frmCommOpenMarkerLabel.Name = "frmCommOpenMarkerLabel";
            this.frmCommOpenMarkerLabel.Size = new Size(0x4e, 13);
            this.frmCommOpenMarkerLabel.TabIndex = 4;
            this.frmCommOpenMarkerLabel.Text = "Add User Text:";
            this.frmCommOpenDurationLoggingStatusLabel.AutoSize = true;
            this.frmCommOpenDurationLoggingStatusLabel.Location = new Point(0x12e, 0x65);
            this.frmCommOpenDurationLoggingStatusLabel.Name = "frmCommOpenDurationLoggingStatusLabel";
            this.frmCommOpenDurationLoggingStatusLabel.Size = new Size(0, 13);
            this.frmCommOpenDurationLoggingStatusLabel.TabIndex = 0x13;
            this._displayDataBG.WorkerReportsProgress = true;
            this._displayDataBG.WorkerSupportsCancellation = true;
            this._displayDataBG.DoWork += new DoWorkEventHandler(this.updateGui);
            this._parseDataBG.WorkerSupportsCancellation = true;
            this._parseDataBG.DoWork += new DoWorkEventHandler(this.parseDataBGProcess);
            this._readPortDataBG.WorkerSupportsCancellation = true;
            this._readPortDataBG.DoWork += new DoWorkEventHandler(this.readPortDataProcess);
            this.statusStrip1.BackColor = SystemColors.ControlLight;
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripStatusLabel1 });
            this.statusStrip1.Location = new Point(0, 0xee);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(0x23d, 0x16);
            this.statusStrip1.TabIndex = 20;
            this.statusStrip1.Text = "statusStrip1";
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new Size(0x26, 0x11);
            this.toolStripStatusLabel1.Text = "Status";
            this.rtbDisplay.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.rtbDisplay.BackColor = SystemColors.Window;
            this.rtbDisplay.Location = new Point(0x18, 0xa1);
            this.rtbDisplay.Name = "rtbDisplay";
            this.rtbDisplay.ReadOnly = true;
            this.rtbDisplay.Size = new Size(0x20d, 0x3e);
            this.rtbDisplay.TabIndex = 6;
            this.rtbDisplay.Text = "";
            this.rtbDisplay.DoubleClick += new EventHandler(this.rtbDisplay_DoubleClick);
            this.label_autoDetect.Location = new Point(0x69, 0x39);
            this.label_autoDetect.Name = "label_autoDetect";
            this.label_autoDetect.Size = new Size(0x18a, 0x11);
            this.label_autoDetect.TabIndex = 0x15;
            this.label_autoDetect.TextAlign = ContentAlignment.MiddleLeft;
            this.button1.Location = new Point(12, 0x36);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 0x16;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.btn_logFileBroswer.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btn_logFileBroswer.Location = new Point(0x206, 0x4f);
            this.btn_logFileBroswer.Name = "btn_logFileBroswer";
            this.btn_logFileBroswer.Size = new Size(0, 0);
            this.btn_logFileBroswer.TabIndex = 0x17;
            this.btn_logFileBroswer.Text = "...";
            this.btn_logFileBroswer.UseVisualStyleBackColor = true;
            this.btn_logFileBroswer.Click += new EventHandler(this.btn_logFileBroswer_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = SystemColors.Control;
            base.ClientSize = new Size(0x23d, 260);
            base.Controls.Add(this.btn_logFileBroswer);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.label_autoDetect);
            base.Controls.Add(this.rtbDisplay);
            base.Controls.Add(this.statusStrip1);
            base.Controls.Add(this.frmCommOpenDurationLoggingStatusLabel);
            base.Controls.Add(this.frmCommOpenMarkerLabel);
            base.Controls.Add(this.frmCommOpenLogStatusLabel);
            base.Controls.Add(this.markerText);
            base.Controls.Add(this.frmCommOpenMenuStrip);
            base.Controls.Add(this.label9);
            base.Controls.Add(this.logFileName);
            base.Controls.Add(this.toolStrip1);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MainMenuStrip = this.frmCommOpenMenuStrip;
            base.Margin = new Padding(4);
            base.Name = "frmCommOpen";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Main";
            base.Load += new EventHandler(this.frmCommOpen_Load);
            base.FormClosed += new FormClosedEventHandler(this.frmCommOpen_FormClosed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.frmCommOpenMenuStrip.ResumeLayout(false);
            this.frmCommOpenMenuStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void interfenceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateInterferenceReportWindow();
        }

        private bool IsSourceDeviceOpen()
        {
            return this.comm.IsSourceDeviceOpen();
        }

        private void loadLocation(Form formWindow, string top, string left, string width, string height, string state)
        {
            formWindow.Left = Convert.ToInt32(left);
            formWindow.Top = Convert.ToInt32(top);
            formWindow.Width = Convert.ToInt32(width);
            formWindow.Height = Convert.ToInt32(height);
            if (state == "Maximized")
            {
                formWindow.WindowState = FormWindowState.Maximized;
            }
            else if (state == "Minimized")
            {
                formWindow.WindowState = FormWindowState.Minimized;
            }
            else
            {
                formWindow.WindowState = FormWindowState.Normal;
            }
        }

        private void locationMapMenu_Click(object sender, EventArgs e)
        {
            this.CreateLocationMapWin();
        }

        private void locationViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateLocationMapWin();
        }

        private void logFileName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.KeyValue == 13) && (this.comm != null))
            {
                this.comm.Log.filename = this.logFileName.Text;
            }
        }

        private void logTTFFAndNavAccuracy()
        {
            try
            {
                this.comm.RxCtrl.GetTTFF();
            }
            catch
            {
            }
        }

        private void lowPowerBarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.createLowPowerInputWindow();
        }

        private void lowPowerCommandBufferMenu_Click(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                new frmLPBufferWindow(this.comm).ShowDialog();
            }
        }

        private void lowPowerMenu_Click(object sender, EventArgs e)
        {
            this.createLowPowerInputWindow();
        }

        private void manualConnect()
        {
            try
            {
                bool flag;
                int num;
                this.updateTTBPort = null;
                this.comm.UpdateWinTitle -= new CommunicationManager.UpdateParentEventHandler(this.updateWinTitle);
                this.comm.UpdateWinTitle += new CommunicationManager.UpdateParentEventHandler(this.updateWinTitle);
                this._isIdle = this.comm.IsSourceDeviceOpen();
                if (this._isIdle)
                {
                    goto Label_0989;
                }
                if (!this._isInit)
                {
                    this.createRxSettingsWindow();
                    if (!this._isInit)
                    {
                        return;
                    }
                }
                if (!this.comm.RequireHostRun || (sysCmdExec.IsExistingWin(this.comm.HostSWFilePath) != 0))
                {
                    goto Label_0351;
                }
                string argStr = string.Empty;
                switch (this.comm.InputDeviceMode)
                {
                    case CommonClass.InputDeviceModes.RS232:
                        argStr = @"-a\\.\\" + this.comm.HostPair1 + " -y\"" + this.comm.HostAppCfgFilePath + "\" -e\"" + this.comm.HostAppMEMSCfgPath + "\"";
                        break;

                    case CommonClass.InputDeviceModes.TCP_Client:
                        argStr = "-n" + this.comm.CMC.HostAppClient.TCPClientPortNum.ToString() + " -y\"" + this.comm.HostAppCfgFilePath + "\" -e\"" + this.comm.HostAppMEMSCfgPath + "\"";
                        break;

                    case CommonClass.InputDeviceModes.TCP_Server:
                        argStr = "-n" + this.comm.CMC.HostAppServer.TCPServerPortNum.ToString() + " -y\"" + this.comm.HostAppCfgFilePath + "\" -e\"" + this.comm.HostAppMEMSCfgPath + "\"";
                        break;
                }
                if (!this.comm.RequireEE)
                {
                    goto Label_034A;
                }
                string eESelect = this.comm.EESelect;
                if (eESelect != null)
                {
                    if (!(eESelect == "CGEE"))
                    {
                        if (eESelect == "SGEE")
                        {
                            goto Label_0275;
                        }
                        if (eESelect == "Mixed SGEE + CGEE")
                        {
                            goto Label_0283;
                        }
                    }
                    else
                    {
                        argStr = argStr + " -mode \"ff4_cgee_only\"";
                    }
                }
                goto Label_028F;
            Label_0275:
                argStr = argStr + " -mode \"ff4_sgee_only\"";
                goto Label_028F;
            Label_0283:
                argStr = argStr + " -mode \"ff4_mixed_mode\"";
            Label_028F:;
                argStr = argStr + " -s\"" + this.comm.ServerName + "\" -d\"/diff/packedDifference.f2p" + this.comm.EEDayNum + "enc.ee\" -j\"" + this.comm.AuthenticationCode + "\" -k" + this.comm.ServerPort;
                if ((this.comm.EESelect == "CGEE") || (this.comm.EESelect == "Mixed SGEE + CGEE"))
                {
                    argStr = argStr + " -b" + this.comm.BankTime;
                }
            Label_034A:
                this.RunHostApp(argStr);
            Label_0351:
                flag = false;
                if ((this.comm.ProductFamily == CommonClass.ProductType.GSD4e) && (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232))
                {
                    this.toolBarConnectBtn.Enabled = false;
                    this.label_autoDetect.Text = "AutoDetect in progress, It may take up to 15 seconds, Please wait...";
                    this.label_autoDetect.ForeColor = Color.DeepPink;
                    num = this.searchforProtocolAndBaud();
                    this.label_autoDetect.Text = "";
                    this.toolBarConnectBtn.Enabled = true;
                    if (num > 0)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                    if (!flag)
                    {
                        return;
                    }
                    if (this.comm.MessageProtocol == "OSP")
                    {
                        this.viewModeGPSMenu.Checked = true;
                        this.viewModeHexMenu.CheckState = CheckState.Unchecked;
                        this.viewModeNmeaTextMenu.CheckState = CheckState.Unchecked;
                        this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
                        this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
                        this.viewModeGPSMenu.CheckState = CheckState.Checked;
                        this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GPS;
                        this.comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.GPS;
                        this.frmCommOpenUpdateStatusString();
                    }
                    else if (this.comm.MessageProtocol == "NMEA")
                    {
                        this.viewModeNmeaTextMenu.Checked = true;
                        this.viewModeHexMenu.CheckState = CheckState.Unchecked;
                        this.viewModeNmeaTextMenu.CheckState = CheckState.Checked;
                        this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
                        this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
                        this.viewModeGPSMenu.CheckState = CheckState.Unchecked;
                        this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Text;
                        this.comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.Text;
                        this.frmCommOpenUpdateStatusString();
                    }
                    else
                    {
                        this.viewModeHexMenu.Checked = true;
                        this.viewModeHexMenu.CheckState = CheckState.Checked;
                        this.viewModeNmeaTextMenu.CheckState = CheckState.Unchecked;
                        this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
                        this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
                        this.viewModeGPSMenu.CheckState = CheckState.Unchecked;
                        this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Hex;
                        this.comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.Hex;
                        this.frmCommOpenUpdateStatusString();
                    }
                    this.toolBarConnectBtn.Enabled = true;
                }
                if ((this.comm.ProductFamily == CommonClass.ProductType.GSD4e) && (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C))
                {
                    this.toolBarConnectBtn.Enabled = false;
                    this.label_autoDetect.Text = "Protocol Autodetect in progress, Please wait...";
                    this.label_autoDetect.ForeColor = Color.DeepPink;
                    num = this.searchforProtocol_I2C();
                    this.label_autoDetect.Text = "";
                    this.toolBarConnectBtn.Enabled = true;
                    if (num > 0)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                    if (!flag)
                    {
                        return;
                    }
                    if (this.comm.MessageProtocol == "OSP")
                    {
                        this.viewModeGPSMenu.Checked = true;
                        this.viewModeHexMenu.CheckState = CheckState.Unchecked;
                        this.viewModeNmeaTextMenu.CheckState = CheckState.Unchecked;
                        this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
                        this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
                        this.viewModeGPSMenu.CheckState = CheckState.Checked;
                        this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GPS;
                        this.comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.GPS;
                        this.frmCommOpenUpdateStatusString();
                    }
                    else if (this.comm.MessageProtocol == "NMEA")
                    {
                        this.viewModeNmeaTextMenu.Checked = true;
                        this.viewModeHexMenu.CheckState = CheckState.Unchecked;
                        this.viewModeNmeaTextMenu.CheckState = CheckState.Checked;
                        this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
                        this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
                        this.viewModeGPSMenu.CheckState = CheckState.Unchecked;
                        this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Text;
                        this.comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.Text;
                        this.frmCommOpenUpdateStatusString();
                    }
                    else
                    {
                        this.viewModeHexMenu.Checked = true;
                        this.viewModeHexMenu.CheckState = CheckState.Checked;
                        this.viewModeNmeaTextMenu.CheckState = CheckState.Unchecked;
                        this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
                        this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
                        this.viewModeGPSMenu.CheckState = CheckState.Unchecked;
                        this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Hex;
                        this.comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.Hex;
                        this.frmCommOpenUpdateStatusString();
                    }
                    this.toolBarConnectBtn.Enabled = true;
                }
                else
                {
                    flag = this.comm.OpenPort();
                    if (!flag)
                    {
                        return;
                    }
                }
                if (this.comm.RxType == CommunicationManager.ReceiverType.TTB)
                {
                    this.comm.WriteData("A0A2 0009 CCA6 0002 0100 0000 00 8175 B0B3");
                    this.comm.WriteData("A0A2 0009 CCA6 0004 0100 0000 00 8177 B0B3");
                    this.comm.WriteData("A0A2 0009 CCA6 0029 0100 0000 00 819C B0B3");
                }
                if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
                {
                    if (flag)
                    {
                        this._isIdle = false;
                        string portName = this.comm.PortName;
                        this.setRxMenuStatus(this.comm.comPort.IsOpen);
                        this.UpdateAllWindowTitles(this.comm.PortName);
                    }
                }
                else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client)
                {
                    this.UpdateAllWindowTitles(this.comm.PortName);
                    this.setRxMenuStatus(this.comm.CMC.HostAppClient.IsOpen());
                    this._isIdle = false;
                }
                else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Server)
                {
                    this.UpdateAllWindowTitles(this.comm.PortName);
                    this.setRxMenuStatus(this.comm.CMC.HostAppServer.IsOpen());
                    this._isIdle = false;
                }
                else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C)
                {
                    this.UpdateAllWindowTitles(this.comm.PortName);
                    this.setRxMenuStatus(this.comm.CMC.HostAppI2CSlave.IsOpen());
                    this._isIdle = false;
                }
                else
                {
                    CommonClass.InputDeviceModes inputDeviceMode = this.comm.InputDeviceMode;
                }
                if (this.comm.RequireHostRun && (sysCmdExec.IsExistingWin(this.comm.HostSWFilePath) == 1))
                {
                    sysCmdExec.SetWinSize(this.comm.HostSWFilePath, 2);
                }
                this.RunAsyncProcess();
                if (this._SiRFAware != null)
                {
                    this._SiRFAware.StartListen();
                }
                if (this._interferenceReport != null)
                {
                    this._interferenceReport.StartListen();
                }
                if (this.comm.MessageProtocol == "NMEA")
                {
                    this.setNMEARxMenuStatus();
                }
                else
                {
                    this.toolBarTTFFBtn.Enabled = true;
                }
                if (this.comm.RequireHostRun)
                {
                    this.comm.RxCtrl.SetMEMSMode(0);
                }
                goto Label_098F;
            Label_0989:
                this.manualDisconnect();
            Label_098F:
                this.frmCommOpenUpdateConnectBtnImage();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error: frmCommOpen: frmCommOpenToolConnect_Click() - " + exception.ToString());
            }
        }

        private void manualDisconnect()
        {
            try
            {
                if (((this.comm != null) && (this.comm.RxCtrl != null)) && ((this.comm.RxCtrl.ResetCtrl != null) && this.comm.RxCtrl.ResetCtrl.LoopitInprogress))
                {
                    MessageBox.Show("Test in progress -- Abort test before proceeding", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                this.DisconnectPort();
                this._isIdle = true;
                string str = "Main " + this.comm.PortName + ": Idle";
                this.Text = str;
                sysCmdExec.CloseWinByProcId(this._hostAppCmdWinId);
            }
            catch
            {
            }
            this.frmCommOpenLogStatusLabel.Text = "idle";
            this.setRxMenuStatus(false);
            this.btn_logFileBroswer.Enabled = true;
            this.logFileName.Enabled = true;
        }

        private void markerText_MouseEnter(object sender, EventArgs e)
        {
            string text = "Enter text and hit \"Enter\" key to write it to log file";
            this.toolTip1.Show(text, this.markerText, 0x7530);
        }

        private void markerText_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.comm.WriteApp("User marker: " + this.markerText.Text);
            }
        }

        private void messageFilterGeneralMenu_Click(object sender, EventArgs e)
        {
            this.frmCommOpenToolFilterHelper(sender, e, this._messageFilterCustom, ": Custom Message Filter", "", true);
        }

        private void messagesFilterDebugMenu_Click(object sender, EventArgs e)
        {
            string inFilter = "-1,255,-1%238,255,-1%-1,68,255%238,68,255%-1,225,0%238,225,0%-1,68,225";
            this.frmCommOpenToolFilterHelper(sender, e, this._messageFilterDebug, ": Debug View", inFilter, false);
        }

        private void messagesFilterResponseMenu_Click(object sender, EventArgs e)
        {
            string inFilter = "-1,6,-1%238,6,-1%-1,7,-1%238,7,-1%-1,11,-1%238,11,-1%-1,12,-1%238,12,-1%-1,19,-1%238,19,-1";
            this.frmCommOpenToolFilterHelper(sender, e, this._messageFilterResponse, ": Response View", inFilter, false);
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                if (this.comm.RxType != CommunicationManager.ReceiverType.TTB)
                {
                    this.frmSaveSettingsOnClosing_CommOpen(this._lastWindowsRestoredFilePath);
                }
            }
            catch
            {
            }
            this.StopAsyncProcess();
            Thread.Sleep(500);
            this.commWindowCleanup();
            Thread.Sleep(0x5dc);
            if (this.updateTTBPort != null)
            {
                this.updateTTBPort();
            }
            this.comm.Dispose();
            this.comm = null;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, false);
            }
            base.OnClosed(e);
        }

        public void OnClosing()
        {
            try
            {
                if (this.comm.RxType != CommunicationManager.ReceiverType.TTB)
                {
                    this.frmSaveSettingsOnClosing_CommOpen(this._lastWindowsRestoredFilePath);
                }
            }
            catch
            {
            }
        }

        private void OngcTimerPollAlmEvent(object source, ElapsedEventArgs e)
        {
            this.comm.ListenersCtrl.Stop("PollAlm_GUI");
            this.comm.ListenersCtrl.Delete("PollAlm_GUI");
            this._almDataStreamWriter.Close();
            this.gcTimerPollAlm.Stop();
            this.gcTimerPollAlm.Close();
        }

        private void OnGCTimerPollEphEvent(object source, ElapsedEventArgs e)
        {
            for (int i = this._eph_Msg_SV_ID; i < 0x20; i++)
            {
                this._ephDataStreamWriter.WriteLine(this._eph_non_str);
            }
            this.comm.ListenersCtrl.Stop("PollEph_GUI");
            this.comm.ListenersCtrl.Delete("PollEph_GUI");
            this._ephDataStreamWriter.Close();
            this.gcTimerPollEph.Stop();
            this.gcTimerPollEph.Close();
        }

        private void OpenLoggingFile(CommunicationManager comm)
        {
            string inFilename = string.Empty;
            string path = clsGlobal.InstalledDirectory + @"\Log";
            if (comm.IsSourceDeviceOpen())
            {
                if (this.logFileName.Text.Length == 0)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    inFilename = path + @"\SiRFLive.log";
                    this.logFileName.Text = inFilename;
                }
                else
                {
                    Regex regex = new Regex("^(([a-zA-Z]\\:)|(\\\\))(\\\\{1}|((\\\\{1})[^\\\\]([^/:*?<>\"|]*))+)$");
                    if (!regex.IsMatch(this.logFileName.Text))
                    {
                        MessageBox.Show("Invalid File Path");
                        return;
                    }
                    inFilename = this.logFileName.Text;
                }
                if (!comm.Log.IsFileOpen())
                {
                    if (comm.Log.OpenFile(inFilename))
                    {
                        this.frmCommOpenLogStatusLabel.Text = comm.Log.LoggingState.ToString() + " ...";
                        this.btn_logFileBroswer.Enabled = false;
                        this.logFileName.Enabled = false;
                    }
                }
                else
                {
                    this.frmCommOpenLogStatusLabel.Text = comm.Log.LoggingState.ToString() + " ...";
                }
                comm.Log.DurationLoggingStatusLabel = this.frmCommOpenDurationLoggingStatusLabel;
                comm.Log.LoggingStatusLabel = this.frmCommOpenLogStatusLabel;
                this.logFileName.Text = comm.Log.filename;
                comm.RxCtrl.PollSWVersion();
                comm.RxCtrl.PollNavigationParameters();
            }
            else
            {
                MessageBox.Show("Port not open", "Error logging", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void openSessionBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateSessionOpenWindow();
        }

        private void openSessionMenu_Click(object sender, EventArgs e)
        {
            this.CreateSessionOpenWindow();
        }

        private void parseDataBGProcess(object sender, DoWorkEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
        Label_000F:
            if (this._parseDataBG.CancellationPending || (this.comm == null))
            {
                e.Cancel = true;
            }
            else
            {
                if (this.comm.RxTransType == CommunicationManager.TransmissionType.Text)
                {
                    this.comm.ByteToNMEAText();
                }
                else
                {
                    this.comm.ByteToMsgQueue(new byte[1]);
                }
                Thread.Sleep(20);
                goto Label_000F;
            }
        }

        private void peekPokeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreatePeekPokeWin();
        }

        private void pollAlmanacMenu_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.CreatePrompt = true;
            dialog.OverwritePrompt = true;
            dialog.DefaultExt = "alm";
            dialog.FileName = "almanac.alm";
            dialog.Filter = "Almanac files (*.alm)|*.alm";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this._almDataFilename = dialog.FileName;
                this._almDataStreamWriter = new StreamWriter(this._almDataFilename);
            }
            else
            {
                return;
            }
            if (this.comm.ListenersCtrl != null)
            {
                string listenerName = "PollAlm_GUI";
                if (!this.comm.ListenersCtrl.Exists(listenerName, this.comm.PortName))
                {
                    ListenerContent content = this.comm.ListenersCtrl.Create(listenerName, this.comm.PortName);
                    if (content != null)
                    {
                        content.DoUserWork.DoWork += new DoWorkEventHandler(this.pollAlmHandler);
                        this.comm.ListenersCtrl.Start(listenerName, this.comm.PortName);
                    }
                }
                else
                {
                    this.comm.ListenersCtrl.Start(listenerName, this.comm.PortName);
                }
                string str2 = "//\r\n// Almanac Collection Time(UTC): ";
                DateTime time2 = DateTime.UtcNow.ToUniversalTime();
                string str3 = string.Format("{0:ddd MMM dd hh:mm:ss yyyy}", time2);
                str2 = str2 + str3 + "\r\n";
                for (int i = 2; i < 5; i++)
                {
                    str2 = str2 + "//\r\n";
                }
                this._almDataStreamWriter.Write(str2);
                this.gcTimerPollAlm.Elapsed += new ElapsedEventHandler(this.OngcTimerPollAlmEvent);
                this.gcTimerPollAlm.Interval = 5000.0;
                this.gcTimerPollAlm.AutoReset = false;
                this.gcTimerPollAlm.Start();
                string msg = this.comm.m_Protocols.GetDefaultMsgtoSend(false, 0x92, -1, "Poll Almanac", "SSB");
                this.comm.WriteData(msg);
                this.comm.WriteApp(msg);
            }
        }

        private void pollAlmHandler(object sender, DoWorkEventArgs myQContent)
        {
            Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
            string str = string.Empty;
            try
            {
                MessageQData argument = (MessageQData) myQContent.Argument;
                if (argument.MessageText != string.Empty)
                {
                    str = this.comm.m_Protocols.ConvertRawToFields(HelperFunctions.HexToByte(argument.MessageText));
                }
            }
            catch (Exception exception)
            {
                string msg = "poll Almanac GUI handler error -- " + exception.Message;
                this.comm.WriteApp(msg);
            }
            char[] anyOf = new char[] { ',' };
            int num = str.IndexOfAny(anyOf, 3);
            string str4 = str.Substring(num + 1).Replace(",", ", ");
            this._almDataStreamWriter.WriteLine(str4);
        }

        private void pollEphemerisMenu_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.CreatePrompt = true;
            dialog.OverwritePrompt = true;
            dialog.DefaultExt = "eph";
            dialog.FileName = "ephemeris.eph";
            dialog.Filter = "Ephemeris files (*.eph)|*.eph";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this._ephDataFilename = dialog.FileName;
                this._ephDataStreamWriter = new StreamWriter(this._ephDataFilename);
            }
            else
            {
                return;
            }
            if (this.comm.ListenersCtrl != null)
            {
                string listenerName = "PollEph_GUI";
                if (!this.comm.ListenersCtrl.Exists(listenerName, this.comm.PortName))
                {
                    ListenerContent content = this.comm.ListenersCtrl.Create(listenerName, this.comm.PortName);
                    if (content != null)
                    {
                        content.DoUserWork.DoWork += new DoWorkEventHandler(this.pollEphHandler);
                        this.comm.ListenersCtrl.Start(listenerName, this.comm.PortName);
                    }
                }
                else
                {
                    this.comm.ListenersCtrl.Start(listenerName, this.comm.PortName);
                }
                string str2 = "//\r\n// Ephemeris Collection Time(UTC): ";
                DateTime time2 = DateTime.UtcNow.ToUniversalTime();
                string str3 = string.Format("{0:ddd MMM dd hh:mm:ss yyyy}", time2);
                str2 = str2 + str3 + "\r\n";
                for (int i = 2; i < 5; i++)
                {
                    str2 = str2 + "//\r\n";
                }
                this._ephDataStreamWriter.Write(str2);
                this.gcTimerPollEph.Elapsed += new ElapsedEventHandler(this.OnGCTimerPollEphEvent);
                this.gcTimerPollEph.Interval = 5000.0;
                this.gcTimerPollEph.AutoReset = false;
                this.gcTimerPollEph.Start();
                string msg = string.Empty;
                if ((this.comm.RxType == CommunicationManager.ReceiverType.SLC) || (this.comm.RxType == CommunicationManager.ReceiverType.GSW))
                {
                    msg = this.comm.m_Protocols.GetDefaultMsgtoSend(false, 0x93, -1, "Poll Ephemeris", "SSB");
                }
                else
                {
                    CommunicationManager.ReceiverType rxType = this.comm.RxType;
                }
                this.comm.WriteData(msg);
                this.comm.WriteApp(msg);
            }
        }

        private void pollEphHandler(object sender, DoWorkEventArgs myQContent)
        {
            Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
            string str = string.Empty;
            try
            {
                MessageQData argument = (MessageQData) myQContent.Argument;
                if (argument.MessageText != string.Empty)
                {
                    str = this.comm.m_Protocols.ConvertRawToFields(HelperFunctions.HexToByte(argument.MessageText));
                }
            }
            catch (Exception exception)
            {
                string msg = "poll Ephemeris GUI handler error -- " + exception.Message;
                this.comm.WriteApp(msg);
            }
            char[] anyOf = new char[] { ',' };
            int num = str.IndexOfAny(anyOf, 3);
            string text = str.Substring(num + 1);
            if (!this._eph_rcvd)
            {
                int num2 = this.CountStringOccurrences(text, ",");
                for (int j = 0; j < num2; j++)
                {
                    this._eph_non_str = this._eph_non_str + "0, ";
                }
                this._eph_non_str = this._eph_non_str + "0";
                this._eph_rcvd = true;
            }
            string[] strArray = new string[2];
            string str4 = text.Split(anyOf, 2)[0];
            int num4 = Convert.ToInt32(str4);
            for (int i = this._eph_Msg_SV_ID + 1; num4 > i; i++)
            {
                this._ephDataStreamWriter.WriteLine(this._eph_non_str);
            }
            this._eph_Msg_SV_ID = num4;
            string str5 = text.Replace(",", ", ");
            this._ephDataStreamWriter.WriteLine(str5);
        }

        private void pollNavParametersMenu_Click(object sender, EventArgs e)
        {
            this.comm.RxCtrl.PollNavigationParameters();
        }

        private void pollSWVersionMenu_Click(object sender, EventArgs e)
        {
            bool isSLCRx = false;
            int mid = 0x84;
            this.comm.CMC.TxCurrentTransmissionType = (CommonClass.TransmissionType) this.comm.TxCurrentTransmissionType;
            string messageProtocol = this.comm.MessageProtocol;
            if ((this.comm.RxType == CommunicationManager.ReceiverType.SLC) || (this.comm.RxType == CommunicationManager.ReceiverType.TTB))
            {
                isSLCRx = true;
                if (this.comm.RxType == CommunicationManager.ReceiverType.TTB)
                {
                    messageProtocol = "TTB";
                }
            }
            string str3 = messageProtocol;
            if (str3 != null)
            {
                if (!(str3 == "SSB") && !(str3 == "TTB"))
                {
                    if (str3 == "F")
                    {
                        mid = 0x17;
                    }
                    else if (str3 == "OSP")
                    {
                        messageProtocol = "SSB";
                        isSLCRx = false;
                        mid = 0x84;
                    }
                }
                else
                {
                    mid = 0x84;
                }
            }
            string msg = this.comm.m_Protocols.GetDefaultMsgtoSend(isSLCRx, mid, -1, "Software Version Request", messageProtocol);
            this.comm.WriteData(msg);
        }

        private void predefinedMessageMenu_Click(object sender, EventArgs e)
        {
            this.CreateInputCommandsWin();
        }

        private void radarMapMenu_Click(object sender, EventArgs e)
        {
            this.CreateSVsMapWin();
        }

        private void readPortDataProcess(object sender, DoWorkEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
        Label_000F:
            if (this._readPortDataBG.CancellationPending || (this.comm == null))
            {
                e.Cancel = true;
            }
            else
            {
                if (this.comm.comPortDataReceivedHandler() <= 0)
                {
                    Thread.Sleep(10);
                }
                else
                {
                    Thread.Sleep(3);
                }
                goto Label_000F;
            }
        }

        private void resetBarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.CreateResetWindow();
        }

        private void resetMenu_Click(object sender, EventArgs e)
        {
            this.CreateResetWindow();
        }

        private void responseMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateResponseViewWin();
        }

        private void responseToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void rtbDisplay_DoubleClick(object sender, EventArgs e)
        {
            this.rtbDisplay.Text = string.Empty;
        }

        private void rtbDisplayProccess()
        {
            MessageQData argument = new MessageQData();
            StringBuilder builder = new StringBuilder();
            new StringBuilder();
            Queue queue = new Queue();
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
            lock (this.comm.DisplayDataLock)
            {
                while (this.comm.DisplayQueue.Count > 0)
                {
                    queue.Enqueue(this.comm.DisplayQueue.Dequeue());
                }
            }
            Thread.CurrentThread.Priority = ThreadPriority.Normal;
            while (queue.Count > 0)
            {
                if (this._displayDataBG.CancellationPending)
                {
                    return;
                }
                try
                {
                    argument = (MessageQData) queue.Dequeue();
                }
                catch
                {
                    argument = null;
                }
                if (((argument == null) || (argument.MessageText == null)) || !(argument.MessageText != string.Empty))
                {
                    continue;
                }
                CommonClass.MessageType messageType = argument.MessageType;
                string csvString = string.Empty;
                bool flag = false;
                if ((argument.MessageSource == CommonClass.MessageSource.RX_INPUT) || (argument.MessageSource == CommonClass.MessageSource.USER_TEXT))
                {
                    csvString = argument.MessageText;
                    flag = true;
                }
                else if (argument.MessageSource == CommonClass.MessageSource.RX_OUTPUT)
                {
                    flag = false;
                    if (argument.MessageId == 0xe1)
                    {
                        if (argument.MessageSubId == 6)
                        {
                            if (this.comm.MessageProtocol != "OSP")
                            {
                                BackgroundWorker worker = new BackgroundWorker();
                                worker.DoWork += new DoWorkEventHandler(this.updateTTFFWindow);
                                worker.WorkerReportsProgress = true;
                                worker.WorkerSupportsCancellation = true;
                                worker.RunWorkerAsync(argument);
                            }
                            else if (!this.comm.AutoReplyCtrl.AutoReplyParams.AutoReply)
                            {
                                BackgroundWorker worker2 = new BackgroundWorker();
                                worker2.DoWork += new DoWorkEventHandler(this.updateTTFFWindow);
                                worker2.WorkerReportsProgress = true;
                                worker2.WorkerSupportsCancellation = true;
                                worker2.RunWorkerAsync(argument);
                            }
                        }
                        else if (argument.MessageSubId == 7)
                        {
                            BackgroundWorker worker3 = new BackgroundWorker();
                            worker3.DoWork += new DoWorkEventHandler(this.updateTTFFWindow);
                            worker3.WorkerReportsProgress = true;
                            worker3.WorkerSupportsCancellation = true;
                            worker3.RunWorkerAsync(argument);
                        }
                    }
                    else if ((argument.MessageId == 6) && (argument.MessageChanId == 0xbb))
                    {
                        BackgroundWorker worker4 = new BackgroundWorker();
                        worker4.DoWork += new DoWorkEventHandler(this.updateTTFFWindow);
                        worker4.WorkerReportsProgress = true;
                        worker4.WorkerSupportsCancellation = true;
                        worker4.RunWorkerAsync(argument);
                    }
                    switch (this.comm.RxTransType)
                    {
                        case CommunicationManager.TransmissionType.SSB:
                            this.comm.dataGui.AGC_Gain = 0;
                            csvString = argument.MessageText;
                            goto Label_0331;

                        case CommunicationManager.TransmissionType.GP2:
                            csvString = CommonUtilsClass.LogToGP2(argument.MessageText, argument.MessageTime);
                            this.comm.dataGui.AGC_Gain = 0;
                            goto Label_0331;

                        case CommunicationManager.TransmissionType.GPS:
                            csvString = this.comm.LogToCSV(argument.MessageText);
                            if ((this.comm.MessageProtocol == "OSP") && (argument.MessageId == 0x45))
                            {
                                if (argument.MessageSubId != 1)
                                {
                                    break;
                                }
                                csvString = csvString + this.comm.RxCtrl.FormatPositionResponse(csvString);
                            }
                            goto Label_0331;

                        default:
                            csvString = argument.MessageText;
                            goto Label_0331;
                    }
                    if (argument.MessageSubId == 2)
                    {
                        csvString = csvString + this.comm.RxCtrl.FormatMeasurementResponse(csvString);
                    }
                }
            Label_0331:
                if ((csvString == null) && !(csvString != string.Empty))
                {
                    continue;
                }
                builder.Append(csvString);
                if (((argument.MessageId == 0xe1) || (argument.MessageId == 0xff)) || ((argument.MessageId == 0x40) || (argument.MessageId == 0x44)))
                {
                    flag = true;
                    string pattern = @"gain (?<gain>\d+)";
                    Regex regex = new Regex(pattern, RegexOptions.Compiled);
                    if (regex.IsMatch(csvString))
                    {
                        try
                        {
                            this.comm.dataGui.AGC_Gain = Convert.ToInt32(regex.Match(csvString).Result("${gain}"));
                        }
                        catch
                        {
                        }
                    }
                    lock (this.comm.LockErrorLog)
                    {
                        foreach (string str3 in this.comm.ErrorStringList)
                        {
                            if (csvString.Contains(str3))
                            {
                                this.comm.Log.ErrorWriteLine(csvString);
                            }
                        }
                        goto Label_0449;
                    }
                }
                if (this.comm.ViewAll)
                {
                    flag = true;
                }
            Label_0449:
                if (argument.MessageId == 7)
                {
                    csvString = csvString + this.comm.RxCtrl.FormatMsgSeven(csvString);
                }
                this.comm.Log.WriteLine(csvString);
            }
        }

        public void RunAsyncProcess()
        {
            Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
            try
            {
                if (!this._parseDataBG.IsBusy)
                {
                    this._parseDataBG.RunWorkerAsync();
                }
                if (!this._displayDataBG.IsBusy)
                {
                    this._displayDataBG.RunWorkerAsync();
                }
                if ((this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232) && !this._readPortDataBG.IsBusy)
                {
                    this._readPortDataBG.RunWorkerAsync();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public void RunHostApp(string argStr)
        {
            if (this.comm != null)
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(Directory.GetParent(this.comm.HostSWFilePath).FullName);
                Process process = sysCmdExec.OpenWin("\"" + this.comm.HostSWFilePath + "\"", argStr);
                this._hostAppCmdWinId = process.Id;
                Directory.SetCurrentDirectory(currentDirectory);
            }
        }

        private void rxCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
            {
                this.setRxMenuStatus(this.comm.comPort.IsOpen);
            }
            else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client)
            {
                this.setRxMenuStatus(this.comm.CMC.HostAppClient.IsOpen());
            }
            else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Server)
            {
                this.setRxMenuStatus(this.comm.CMC.HostAppServer.IsOpen());
            }
            else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C)
            {
                this.setRxMenuStatus(this.comm.CMC.HostAppI2CSlave.IsOpen());
            }
        }

        private void rxSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.autoReplySettingsMenu.Checked = this.comm.AutoReplyCtrl.AutoReplyParams.AutoReply;
            if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
            {
                this.setRxMenuStatus(this.comm.comPort.IsOpen);
            }
            else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client)
            {
                this.setRxMenuStatus(this.comm.CMC.HostAppClient.IsOpen());
            }
            else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C)
            {
                this.setRxMenuStatus(this.comm.CMC.HostAppI2CSlave.IsOpen());
            }
        }

        private void rxSettingsMenuItem_Click(object sender, EventArgs e)
        {
            if (this.createRxSettingsWindow())
            {
                this.manualConnect();
            }
        }

        private void rxTTBConfigTimeAiding_Click(object sender, EventArgs e)
        {
            this.CreateTTBTimeAidCfgWindow();
        }

        private void rxTTBConnectMenu_Click(object sender, EventArgs e)
        {
            this.CreateTTBConnectWindow();
        }

        private void rxTTBViewMenu_Click(object sender, EventArgs e)
        {
            if (!this.comm.TTBPort.IsOpen)
            {
                MessageBox.Show("TTB Port not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                if ((this._ttbWin == null) || this._ttbWin.IsDisposed)
                {
                    this._ttbWin = new frmCommOpen();
                }
                this._ttbWin.updateTTBPort = (updateParentEventHandler) Delegate.Combine(this._ttbWin.updateTTBPort, new updateParentEventHandler(this.connectTTBPort));
                this._ttbWin.MdiParent = base.MdiParent;
                this._ttbWin.comm.comPort = this.comm.TTBPort;
                this._ttbWin.comm.comPort.Close();
                Thread.Sleep(0x3e8);
                this._ttbWin.Show();
                this._ttbWin.comm.RequireHostRun = false;
                this._ttbWin.comm.MessageProtocol = "SSB";
                this._ttbWin.comm.RxType = CommunicationManager.ReceiverType.TTB;
                this._ttbWin.comm.InputDeviceMode = CommonClass.InputDeviceModes.RS232;
                this._ttbWin.comm.BaudRate = this.comm.TTBPort.BaudRate.ToString();
                this._ttbWin.comm.DataBits = this.comm.TTBPort.DataBits.ToString();
                this._ttbWin.comm.StopBits = "One";
                this._ttbWin.comm.Parity = "None";
                this._ttbWin.comm.PortName = this.comm.TTBPort.PortName;
                this._ttbWin.comm.ProductFamily = CommonClass.ProductType.GSW;
                this._ttbWin.comm.OpenPort();
                this._ttbWin.comm.WriteData("A0A2 0009 CCA6 0002 0100 0000 00 8175 B0B3");
                this._ttbWin.comm.WriteData("A0A2 0009 CCA6 0004 0100 0000 00 8177 B0B3");
                this._ttbWin.comm.WriteData("A0A2 0009 CCA6 0029 0100 0000 00 819C B0B3");
                this._ttbWin.RunAsyncProcess();
                this._ttbWin.Text = string.Format("TTB: {0}", this._ttbWin.comm.comPort.PortName);
                this._ttbWin.pollAlmanacMenu.Visible = false;
                this._ttbWin.pollSWVersionMenu.Visible = false;
                this._ttbWin.pollEphemerisMenu.Visible = false;
                this._ttbWin.pollNavParametersMenu.Visible = false;
                this._ttbWin.toolStrip1.Visible = false;
                this._ttbWin.TTBMenuItem.Visible = false;
                this._ttbWin.rxSessionMenuItem.Visible = false;
                this._ttbWin.cwInterfenceDetectionMenu.Visible = false;
                this._ttbWin.SiRFAwareMenuItem.Visible = false;
                this._ttbWin.setDevelopersDebugMenu.Visible = false;
                this._ttbWin.switchOperationModeMenu.Visible = false;
                this._ttbWin.lowPowerMenu.Visible = false;
                this._ttbWin.trackerICPeekPokeMenu.Visible = false;
                this._ttbWin.trackerConfigurationMenu.Visible = false;
                this._ttbWin.inputCommandMenuItem.Visible = false;
                this._ttbWin.locationMapMenu.Visible = false;
                this._ttbWin.lowPowerCommandBufferMenu.Visible = false;
                this._ttbWin.TTFFMenuItem.Visible = false;
                this._ttbWin.setABPMenuItem.Visible = false;
                this._ttbWin.switchProtocolMenu.Visible = false;
                this._ttbWin.setMEMSMenuItem.Visible = false;
                this._ttbWin.TrackerConfigVer2.Visible = false;
                this._ttbSigWin = this._ttbWin.CreateSignalViewWin();
                this._ttbSigWin.Text = string.Format("TTB: {0} Signal View", this._ttbWin.comm.comPort.PortName);
                this.TTBMenuItem.Enabled = false;
            }
        }

        private void rxViewModeMenuItem_Click(object sender, EventArgs e)
        {
            if (this.comm.ViewAll)
            {
                this.allMessagesMenu.Checked = true;
            }
            if (this._locationViewPanel != null)
            {
                this.locationMapMenu.Checked = true;
            }
            else
            {
                this.locationMapMenu.Checked = false;
            }
            if (this._signalMapPanel != null)
            {
                this.radarMapMenu.Checked = true;
            }
            else
            {
                this.radarMapMenu.Checked = false;
            }
            if (this._signalStrengthPanel != null)
            {
                this.signalViewMenu.Checked = true;
            }
            else
            {
                this.signalViewMenu.Checked = false;
            }
            if (this._ttffDisplay != null)
            {
                this.TTFFMenuItem.Checked = true;
            }
            else
            {
                this.TTFFMenuItem.Checked = false;
            }
            if (this._interferenceReport != null)
            {
                this.cwInterfenceDetectionMenu.Checked = true;
            }
            else
            {
                this.cwInterfenceDetectionMenu.Checked = false;
            }
            if (this._SatelliteStats != null)
            {
                this.satelliteStatisticsMenuItem.Checked = true;
            }
            else
            {
                this.satelliteStatisticsMenuItem.Checked = false;
            }
            if (this._SiRFAware != null)
            {
                this.SiRFAwareMenuItem.Checked = true;
            }
            else
            {
                this.SiRFAwareMenuItem.Checked = false;
            }
            if (this._responseView != null)
            {
                this.responseMenuItem.Checked = true;
            }
            else
            {
                this.responseMenuItem.Checked = false;
            }
            if (this._errorView != null)
            {
                this.errorToolStripMenuItem.Checked = true;
            }
            else
            {
                this.errorToolStripMenuItem.Checked = false;
            }
        }

        private void satelliteMapViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void satelliteStatisticsMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateSatelliteStatsWin();
        }

        private void satelliteStatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateSatelliteStatsWin();
        }

        private int searchforProtocol_I2C()
        {
            this._currentProtocol = this.comm.MessageProtocol;
            this.Refresh();
            if (!this.comm.OpenPort())
            {
                return 0;
            }
            this.comm.AutoDetectProtocolAndBaudDone = false;
            Thread.Sleep(5);
            if (this.comm.waitforNMEAMsg_I2C() != string.Empty)
            {
                this.comm.AutoDetectProtocolAndBaudDone = false;
                Thread.Sleep(50);
                if (!this.comm.ClosePort())
                {
                    return 0;
                }
                Thread.Sleep(5);
                this.comm.RxType = CommunicationManager.ReceiverType.NMEA;
                this.comm.RxType = CommunicationManager.ReceiverType.SLC;
                if (!this.comm.OpenPort())
                {
                    return 0;
                }
                this.comm.AutoDetectProtocolAndBaudDone = true;
                return 1;
            }
            if (this.comm.waitforSSBMsg_I2C() != string.Empty)
            {
                this.comm.AutoDetectProtocolAndBaudDone = false;
                Thread.Sleep(50);
                if (!this.comm.ClosePort())
                {
                    return 0;
                }
                Thread.Sleep(5);
                this.comm.MessageProtocol = "OSP";
                this.comm.RxType = CommunicationManager.ReceiverType.SLC;
                if (!this.comm.OpenPort())
                {
                    return 0;
                }
                this.comm.AutoDetectProtocolAndBaudDone = true;
                return 1;
            }
            this._isIdle = false;
            return 2;
        }

        private int searchforProtocolAndBaud()
        {
            this._currentProtocol = this.comm.MessageProtocol;
            this._currentBaud = this.comm.BaudRate;
            this.Refresh();
            if (!this.comm.OpenPort())
            {
                return 0;
            }
            Thread.Sleep(5);
            int num = this.searchforProtocolAndBaud_sub();
            if (num == 3)
            {
                this.comm.BaudRate = this._currentBaud;
                this.comm.MessageProtocol = this._currentProtocol;
                if (this._currentProtocol == "OSP")
                {
                    this.comm.RxType = CommunicationManager.ReceiverType.SLC;
                }
                else
                {
                    this.comm.RxType = CommunicationManager.ReceiverType.NMEA;
                }
                this.Refresh();
                if (!this.comm.ClosePort())
                {
                    return 0;
                }
                if (!this.comm.OpenPort())
                {
                    return 0;
                }
                this.comm.ErrorViewRTBDisplay.DisplayData(CommonClass.MessageType.Error, "Warning: no GPS device detected on " + this.comm.comPort.PortName);
            }
            return num;
        }

        private bool searchforProtocolAndBaud_NMEA()
        {
            this.comm.MessageProtocol = "NMEA";
            this.comm.RxType = CommunicationManager.ReceiverType.NMEA;
            string[] strArray = new string[5];
            strArray = "4800,115200,38400,57600,9600,19200".Split(new char[] { ',' });
            for (int i = 0; i < strArray.Length; i++)
            {
                if ((this._currentProtocol != "NMEA") || (this._currentBaud != strArray[i]))
                {
                    this.comm.BaudRate = strArray[i];
                    this.Refresh();
                    if (!this.comm.ClosePort())
                    {
                        return false;
                    }
                    if (!this.comm.OpenPort())
                    {
                        return false;
                    }
                    if (this.comm.waitforNMEAMsg())
                    {
                        return true;
                    }
                }
            }
            return (!this.comm.ClosePort() && false);
        }

        private bool searchforProtocolAndBaud_OSP()
        {
            this.comm.MessageProtocol = "OSP";
            this.comm.RxType = CommunicationManager.ReceiverType.SLC;
            string[] strArray = new string[5];
            strArray = "115200,4800,38400,9600,57600,19200".Split(new char[] { ',' });
            for (int i = 0; i < strArray.Length; i++)
            {
                if ((this._currentProtocol != "OSP") || (this._currentBaud != strArray[i]))
                {
                    this.comm.BaudRate = strArray[i];
                    this.Refresh();
                    if (!this.comm.ClosePort())
                    {
                        return false;
                    }
                    if (!this.comm.OpenPort())
                    {
                        return false;
                    }
                    if (this.comm.waitforSSBMsg())
                    {
                        return true;
                    }
                }
            }
            this._isIdle = false;
            return false;
        }

        private int searchforProtocolAndBaud_sub()
        {
            string[] strArray = new string[10];
            string[] strArray2 = new string[10];
            string str = string.Empty;
            string str2 = string.Empty;
            if (this.comm.MessageProtocol == "NMEA")
            {
                this.comm.RxType = CommunicationManager.ReceiverType.NMEA;
                if (this.comm.waitforNMEAMsg())
                {
                    return 1;
                }
                str = "OSP,NMEA,OSP,NMEA,OSP,OSP,OSP,OSP,NMEA,NMEA,NMEA,NMEA";
                str2 = "115200,4800,4800,115200,38400,9600,57600,19200,38400,9600,57600,19200";
            }
            else if (this.comm.MessageProtocol == "OSP")
            {
                this.comm.RxType = CommunicationManager.ReceiverType.SLC;
                if (this.comm.waitforSSBMsg())
                {
                    return 1;
                }
                str = "NMEA,OSP,NMEA,OSP,NMEA,NMEA,NMEA,NMEA,OSP,OSP,OSP,OSP";
                str2 = "4800,115200,115200,4800,38400,9600,57600,19200,38400,9600,57600,19200";
            }
            else
            {
                this._isIdle = false;
                return 2;
            }
            strArray = str2.Split(new char[] { ',' });
            strArray2 = str.Split(new char[] { ',' });
            for (int i = 0; i < strArray2.Length; i++)
            {
                if ((this._currentProtocol != strArray2[i]) || (this._currentBaud != strArray[i]))
                {
                    this.comm.BaudRate = strArray[i];
                    this.comm.MessageProtocol = strArray2[i];
                    if (strArray2[i] == "OSP")
                    {
                        this.comm.RxType = CommunicationManager.ReceiverType.SLC;
                    }
                    else
                    {
                        this.comm.RxType = CommunicationManager.ReceiverType.NMEA;
                    }
                    this.Refresh();
                    if (!this.comm.ClosePort())
                    {
                        return 0;
                    }
                    if (!this.comm.OpenPort())
                    {
                        return 0;
                    }
                    Thread.Sleep(1);
                    if (this.comm.MessageProtocol == "OSP")
                    {
                        if (this.comm.waitforSSBMsg())
                        {
                            return 1;
                        }
                    }
                    else if (this.comm.MessageProtocol == "NMEA")
                    {
                        if (this.comm.waitforNMEAMsg())
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        this._isIdle = false;
                        return 2;
                    }
                }
            }
            this._isIdle = false;
            return 3;
        }

        private void SearchMenu(string username)
        {
            Hashtable userAccessInfo = clsGlobal.g_objfrmMDIMain.UserAccess.GetUserAccessInfo(username);
            ToolStripMenuItem[] itemArray = new ToolStripMenuItem[] { 
                this.toolBarLogBtnStartLogMenu, this.toolBarLogBtnStopLogMenu, this.toolBarLogBtnDurationLogMenu, this.rxSettingsMenuItem, this.rxViewModeMenuItem, this.viewModeMenuItem, this.viewModeHexMenu, this.viewModeNmeaTextMenu, this.viewModeSSBMenu, this.viewModeGP2Menu, this.viewModeGPSMenu, this.allMessagesMenu, this.messagesFilterMenuItem, this.messagesFilterDebugMenu, this.messagesFilterResponseMenu, this.messageFilterGeneralMenu, 
                this.locationMapMenu, this.signalViewMenu, this.radarMapMenu, this.cwInterfenceDetectionMenu, this.lowPowerCommandBufferMenu, this.satelliteStatisticsMenuItem, this.SiRFAwareMenuItem, this.TTFFMenuItem, this.rxCommandsMenuItem, this.resetMenu, this.pollSWVersionMenu, this.pollAlmanacMenu, this.pollEphemerisMenu, this.setDevelopersDebugMenu, this.switchOperationModeMenu, this.lowPowerMenu, 
                this.setABPMenuItem, this.setABPEnableMenu, this.setABPDisableMenu, this.setMEMSDisableMenu, this.setMEMSEnableMenu, this.trackerConfigurationMenu, this.trackerICPeekPokeMenu, this.inputCommandMenuItem, this.predefinedMessageMenu, this.userInputMenu, this.rxSessionMenuItem, this.openSessionMenu, this.closeSessionMenu, this.autoReplySettingsMenu, this.autoReplySummaryMenu, this.timeFreqApproxPosStatusRequest, 
                this.TTBMenuItem, this.rxTTBConnectMenu, this.rxTTBConfigTimeAiding, this.errorLogConfigMenuItem, this.errorLogConfigMenuItem
             };
            for (int i = 0; i < itemArray.GetLength(0); i++)
            {
                ToolStripMenuItem item = itemArray[i];
                string key = item.Text.TrimStart("&".ToCharArray());
                if (userAccessInfo.ContainsKey(key))
                {
                    string str3 = (string) userAccessInfo[key];
                    if (str3 != null)
                    {
                        if (!(str3 == "Hidden"))
                        {
                            if (str3 == "Disabled")
                            {
                                goto Label_02D2;
                            }
                            if (str3 == "Enabled")
                            {
                                goto Label_02DB;
                            }
                        }
                        else
                        {
                            item.Visible = false;
                        }
                    }
                }
                continue;
            Label_02D2:
                item.Enabled = false;
                continue;
            Label_02DB:
                item.Visible = true;
                item.Enabled = true;
            }
        }

        private void setABPDisableMenu_Click(object sender, EventArgs e)
        {
            try
            {
                this.comm.ABPModeToSet = false;
                this.comm.ABPModePendingSet = true;
                this.comm.RxCtrl.PollNavigationParameters();
            }
            catch
            {
            }
        }

        private void setABPMenuItemEnable_Click(object sender, EventArgs e)
        {
            try
            {
                this.comm.ABPModeToSet = true;
                this.comm.ABPModePendingSet = true;
                this.comm.RxCtrl.PollNavigationParameters();
            }
            catch
            {
            }
        }

        private void setABPToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            try
            {
                this.comm.RxCtrl.PollNavigationParameters();
                if ((this.comm.NavigationParamrters.ABMMode & 1) == 1)
                {
                    this.setABPDisableMenu.Checked = false;
                    this.setABPEnableMenu.Checked = true;
                }
                else
                {
                    this.setABPDisableMenu.Checked = true;
                    this.setABPEnableMenu.Checked = false;
                }
            }
            catch
            {
            }
        }

        private void setDevelopersDebugMenu_Click(object sender, EventArgs e)
        {
            this.CreateEncrypCtrlWin();
        }

        private void setMEMSDisableMenu_Click(object sender, EventArgs e)
        {
            this.comm.MEMSModeToSet = false;
            if (this.comm.RxCtrl != null)
            {
                this.comm.RxCtrl.SetMEMSMode(0);
            }
        }

        private void setMEMSEnableMenu_Click(object sender, EventArgs e)
        {
            this.comm.MEMSModeToSet = true;
            if (this.comm.RxCtrl != null)
            {
                this.comm.RxCtrl.SetMEMSMode(1);
            }
        }

        private void setMEMSToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            if (this.comm.MEMSModeToSet)
            {
                this.setMEMSDisableMenu.Checked = false;
                this.setMEMSEnableMenu.Checked = true;
            }
            else
            {
                this.setMEMSDisableMenu.Checked = true;
                this.setMEMSEnableMenu.Checked = false;
            }
        }

        private void setNMEARxMenuStatus()
        {
            if (this.comm.MessageProtocol == "NMEA")
            {
                this.comm.ABPModeIndicator = false;
                this.rxSessionMenuItem.Enabled = false;
                this.TTBMenuItem.Enabled = false;
                this.errorLogConfigMenuItem.Enabled = false;
                this.cwInterfenceDetectionMenu.Enabled = false;
                this.lowPowerCommandBufferMenu.Enabled = false;
                this.SiRFAwareMenuItem.Enabled = false;
                this.satelliteStatisticsMenuItem.Enabled = false;
                this.TTFFMenuItem.Enabled = false;
                this.pollSWVersionMenu.Enabled = false;
                this.pollAlmanacMenu.Enabled = false;
                this.pollEphemerisMenu.Enabled = false;
                this.toolStripMenuItem_SetAlm.Enabled = false;
                this.toolStripMenuItem_SetEph.Enabled = false;
                this.pollNavParametersMenu.Enabled = false;
                this.setDevelopersDebugMenu.Enabled = false;
                this.switchOperationModeMenu.Enabled = false;
                this.lowPowerMenu.Enabled = false;
                this.trackerConfigurationMenu.Enabled = false;
                this.TrackerConfigVer2.Enabled = false;
                this.trackerICPeekPokeMenu.Enabled = false;
                this.responseMenuItem.Enabled = false;
                this.errorToolStripMenuItem.Enabled = false;
                this.setMEMSMenuItem.Enabled = false;
                this.setABPMenuItem.Visible = false;
                if (this._ttffDisplay != null)
                {
                    this._ttffDisplay.Close();
                }
                this.toolBarTTFFBtn.Enabled = false;
            }
            else
            {
                this.toolBarTTFFBtn.Enabled = true;
                this.setABPMenuItem.Visible = true;
            }
        }

        private void setRxMenuStatus(bool enableStatus)
        {
			base.BeginInvoke((MethodInvoker)delegate
			{
                this.resetMenu.Enabled = enableStatus;
                this.pollSWVersionMenu.Enabled = enableStatus;
                this.lowPowerMenu.Enabled = enableStatus;
                this.setDevelopersDebugMenu.Enabled = enableStatus;
                this.switchOperationModeMenu.Enabled = enableStatus;
                this.trackerICPeekPokeMenu.Enabled = enableStatus;
                this.trackerConfigurationMenu.Enabled = enableStatus;
                this.TrackerConfigVer2.Enabled = enableStatus;
                this.switchProtocolMenu.Enabled = enableStatus;
                this.setABPMenuItem.Enabled = enableStatus;
                this.inputCommandMenuItem.Enabled = enableStatus;
                this.setMEMSMenuItem.Enabled = enableStatus;
                this.cwInterfenceDetectionMenu.Enabled = true;
                this.lowPowerCommandBufferMenu.Enabled = true;
                this.satelliteStatisticsMenuItem.Enabled = true;
                this.SiRFAwareMenuItem.Enabled = true;
                this.TTFFMenuItem.Enabled = true;
                this.responseMenuItem.Enabled = true;
                this.errorToolStripMenuItem.Enabled = true;
                this.rxSessionMenuItem.Enabled = enableStatus;
                this.TTBMenuItem.Enabled = enableStatus;
                this.openSessionMenu.Enabled = enableStatus;
                this.closeSessionMenu.Enabled = enableStatus;
                this.autoReplySettingsMenu.Enabled = enableStatus;
                this.timeFreqApproxPosStatusRequest.Enabled = enableStatus;
                this.messagesFilterMenuItem.Enabled = enableStatus;
                this.errorLogConfigMenuItem.Enabled = enableStatus;
                this.pollSWVersionMenu.Enabled = enableStatus;
                this.pollAlmanacMenu.Enabled = enableStatus;
                this.pollEphemerisMenu.Enabled = enableStatus;
                this.toolStripMenuItem_SetAlm.Enabled = enableStatus;
                this.toolStripMenuItem_SetEph.Enabled = enableStatus;
                this.pollNavParametersMenu.Enabled = enableStatus;
                this.resetMenu.Enabled = enableStatus;
                this.setDevelopersDebugMenu.Enabled = enableStatus;
                this.switchOperationModeMenu.Enabled = enableStatus;
                this.lowPowerMenu.Enabled = enableStatus;
                this.trackerICPeekPokeMenu.Enabled = enableStatus;
                this.trackerConfigurationMenu.Enabled = enableStatus;
                this.TrackerConfigVer2.Enabled = enableStatus;
                this.switchProtocolMenu.Enabled = enableStatus;
                this.userInputMenu.Enabled = enableStatus;
                this.toolBarResetBtn.Enabled = enableStatus;
                this.timeFreqApproxPosStatusRequest.Visible = false;
                this.setNMEARxMenuStatus();
                if (clsGlobal.IsMarketingUser())
                {
                    this.messagesFilterMenuItem.Visible = false;
                    this.predefinedMessageMenu.Visible = false;
                    this.trackerICPeekPokeMenu.Visible = false;
                    this.openSessionMenu.Visible = false;
                    this.closeSessionMenu.Visible = false;
                    this.setDevelopersDebugMenu.Visible = false;
                }
                if (this.comm.ProductFamily != CommonClass.ProductType.GSD4e)
                {
                    this.switchProtocolMenu.Visible = false;
                    this.setABPMenuItem.Visible = false;
                }
                else
                {
                    this.switchProtocolMenu.Visible = true;
                    if (this.comm.MessageProtocol == "NMEA")
                    {
                        this.setABPMenuItem.Visible = false;
                    }
                    else
                    {
                        this.setABPMenuItem.Visible = true;
                    }
                    this.setNMEARxMenuStatus();
                }
            });
        }

        private void setWinComFocus(object source, ElapsedEventArgs e)
        {
            EventHandler method = null;
            try
            {
                if (method == null)
                {
                    method = delegate {
                        base.BringToFront();
                    };
                }
                base.Invoke(method);
            }
            catch
            {
            }
        }

        private void signalViewMenu_Click(object sender, EventArgs e)
        {
            this.CreateSignalViewWin();
        }

        private void signalViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void SiRFAwareMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateSiRFAwareWin();
        }

        private void siRFAwareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateSiRFAwareWin();
        }

        private void startLog()
        {
            this.OpenLoggingFile(this.comm);
            this.frmCommOpenUpdateLogBtnImage();
        }

        public void StopAsyncProcess()
        {
            try
            {
                if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
                {
                    this._readPortDataBG.CancelAsync();
                }
                this._displayDataBG.CancelAsync();
                this._parseDataBG.CancelAsync();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void switchOperationModeMenu_Click(object sender, EventArgs e)
        {
            this.CreateSwitchOperationModeWin();
        }

        private void switchOperationModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateSwitchOperationModeWin();
        }

        private void switchProtocolMenu_Click(object sender, EventArgs e)
        {
            if (this.createSwitchProtocolWindow() != DialogResult.Cancel)
            {
                Thread.Sleep(0x5dc);
                this.comm.SwitchProtocol();
                Thread.Sleep(200);
                this.UpdateGUIFromComm();
                if (this.comm.MessageProtocol == "NMEA")
                {
                    if (this._ttffDisplay != null)
                    {
                        this._ttffDisplay.Close();
                    }
                    this.toolBarTTFFBtn.Enabled = false;
                }
                else
                {
                    this.toolBarTTFFBtn.Enabled = true;
                }
            }
        }

        private void toolBarConnectBtn_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.ScriptDone)
            {
                MessageBox.Show("Test in progress -- Abort test before proceeding", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                this.manualConnect();
            }
        }

        private void toolBarErrorViewBtn_Click(object sender, EventArgs e)
        {
            this.CreateErrorViewWin();
        }

        private void toolBarLocationBtn_Click(object sender, EventArgs e)
        {
            this.CreateLocationMapWin();
        }

        private void toolBarLogBtnDurationLogMenu_Click(object sender, EventArgs e)
        {
            if (this.comm.IsSourceDeviceOpen())
            {
                frmLogDuration duration = new frmLogDuration(this.comm);
                this.comm.Log.LoggingStatusLabel = this.frmCommOpenLogStatusLabel;
                this.OpenLoggingFile(this.comm);
                duration.Show();
                this.frmCommOpenUpdateLogBtnImage();
            }
            else
            {
                MessageBox.Show("Port not open", "Error logging", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void toolBarLogBtnStartLogMenu_Click(object sender, EventArgs e)
        {
            this.startLog();
        }

        private void toolBarLogBtnStopLogMenu_Click(object sender, EventArgs e)
        {
            string text1 = "Main " + this.comm.PortName + ": Log Stopped";
            this.comm.Log.CloseFile();
            this.comm.Log.LoggingState = LogManager.LoggingStates.idle;
            this.frmCommOpenLogStatusLabel.Text = this.comm.Log.LoggingState.ToString();
            this.btn_logFileBroswer.Enabled = true;
            this.logFileName.Enabled = true;
            this.frmCommOpenUpdateLogBtnImage();
        }

        private void toolBarPauseBtn_Click(object sender, EventArgs e)
        {
            if (this.IsSourceDeviceOpen())
            {
                this.frmCommOpenUpdatePauseBtnImage();
            }
            this.frmCommOpenUpdateStatusString();
        }

        private void toolBarRadarBtn_Click(object sender, EventArgs e)
        {
            this.CreateSVsMapWin();
        }

        private void toolBarResetBtn_Click(object sender, EventArgs e)
        {
            this.CreateResetWindow();
        }

        private void toolBarResponseViewBtn_Click(object sender, EventArgs e)
        {
            this.CreateResponseViewWin();
        }

        private void toolBarSignalViewBtn_Click(object sender, EventArgs e)
        {
            this.CreateSignalViewWin();
        }

        private void toolBarTTFFBtn_Click(object sender, EventArgs e)
        {
            this.CreateTTFFWin();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!this.comm.comPort.IsOpen)
            {
                this.searchforProtocolAndBaud();
            }
        }

        private void toolStripMenuItem_SetAlm_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = "alm";
            dialog.Filter = "Almanac files (*.alm)|*.alm";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string msg = utils_AutoReply.getAlmFromFileForSet(dialog.FileName);
                this.comm.WriteData(msg);
                this.comm.WriteApp(msg);
            }
        }

        private void toolStripMenuItem_SetEph_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = "eph";
            dialog.Filter = "Ephemeris files (*.eph)|*.eph";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string[] strArray = utils_AutoReply.getEphFromFileForSet(dialog.FileName).Split(new char[] { ',' });
                for (int i = 0; i < strArray.GetLength(0); i++)
                {
                    string msg = strArray[i];
                    if (msg != string.Empty)
                    {
                        this.comm.WriteData(msg);
                        this.comm.WriteApp(msg);
                    }
                }
            }
        }

        private void trackerConfigurationMenu_Click(object sender, EventArgs e)
        {
            this.CreateTrackerConfigWin();
        }

        private void trackerConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateTrackerConfigWin();
        }

        private void TrackerConfigVer2_Click(object sender, EventArgs e)
        {
            this.CreateTrackerConfigWin_Ver2();
        }

        private void trackerICPeekPokeMenu_Click(object sender, EventArgs e)
        {
            this.CreatePeekPokeWin();
        }

        private void TTBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.updateTTBToolMenu();
        }

        private void TTFFMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateTTFFWin();
        }

        private string UpdateAllWindowTitles(string deviceName)
        {
            string str = string.Empty;
            if (this._signalStrengthPanel != null)
            {
                if (!this._signalStrengthPanel.IsDisposed)
                {
                    this._signalStrengthPanel.CommWindow = this.comm;
                    str = deviceName + ": Signal View";
                    this._signalStrengthPanel.Text = str;
                }
                this._signalStrengthPanel.updateMainWindow += new frmCommSignalView.updateParentEventHandler(this.updateSubWindowState);
            }
            if (this._signalMapPanel != null)
            {
                if (!this._signalMapPanel.IsDisposed)
                {
                    this._signalMapPanel.CommWindow = this.comm;
                    str = deviceName + ": Radar View";
                    this._signalMapPanel.Text = str;
                }
                this._signalMapPanel.updateMainWindow += new frmCommRadarMap.updateParentEventHandler(this.updateSubWindowState);
            }
            if (this._encryCtrl != null)
            {
                if (!this._encryCtrl.IsDisposed)
                {
                    this._encryCtrl.CommWindow = this.comm;
                    str = deviceName + ": Encrypt Ctrl";
                    this._encryCtrl.Text = str;
                }
                this._encryCtrl.updateMainWindow += new frmEncryCtrl.updateParentEventHandler(this.updateSubWindowState);
            }
            if (this._SatelliteStats != null)
            {
                if (!this._SatelliteStats.IsDisposed)
                {
                    this._SatelliteStats.CommWindow = this.comm;
                    str = deviceName + ": Satellite Stats View";
                    this._SatelliteStats.Text = str;
                }
                this._SatelliteStats.updateMainWindow += new frmSatelliteStats.updateParentEventHandler(this.updateSubWindowState);
            }
            if (this._locationViewPanel != null)
            {
                if (!this._locationViewPanel.IsDisposed)
                {
                    this._locationViewPanel.CommWindow = this.comm;
                    str = deviceName + ": Location View";
                    this._locationViewPanel.Text = str;
                }
                this._locationViewPanel.updateMainWindow += new frmCommLocationMap.updateParentEventHandler(this.updateSubWindowState);
            }
            if (this._inputCommands != null)
            {
                if (!this._inputCommands.IsDisposed)
                {
                    this._inputCommands.CommWindow = this.comm;
                    str = deviceName + ": Input Commands";
                    this._inputCommands.Text = str;
                }
                this._inputCommands.updateMainWindow += new frmCommInputMessage.updateParentEventHandler(this.updateSubWindowState);
            }
            if (this._messageFilterDebug != null)
            {
                if (!this._messageFilterDebug.IsDisposed)
                {
                    this._messageFilterDebug.CommWindow = this.comm;
                    this._messageFilterDebug.StartListeners();
                    str = deviceName + ": Debug View";
                    this._messageFilterDebug.Text = str;
                    this._messageFilterDebug.StartListeners();
                }
                this._messageFilterDebug.updateMainWindow += new frmCommMessageFilter.updateParentEventHandler(this.updateSubWindowState);
            }
            if (this._messageFilterResponse != null)
            {
                if (!this._messageFilterResponse.IsDisposed)
                {
                    this._messageFilterResponse.CommWindow = this.comm;
                    this._messageFilterResponse.StartListeners();
                    str = deviceName + ": Response View";
                    this._messageFilterResponse.Text = str;
                    this._messageFilterResponse.StartListeners();
                }
                this._messageFilterResponse.updateMainWindow += new frmCommMessageFilter.updateParentEventHandler(this.updateSubWindowState);
            }
            if (this._messageFilterCustom != null)
            {
                if (!this._messageFilterCustom.IsDisposed)
                {
                    this._messageFilterCustom.CommWindow = this.comm;
                    this._messageFilterCustom.StartListeners();
                    str = deviceName + ": Custom Message Filter";
                    this._messageFilterCustom.Text = str;
                }
                this._messageFilterCustom.updateMainWindow += new frmCommMessageFilter.updateParentEventHandler(this.updateSubWindowState);
            }
            if (this._ttffDisplay != null)
            {
                if (!this._ttffDisplay.IsDisposed)
                {
                    this._ttffDisplay.CommWindow = this.comm;
                    str = deviceName + ": TTFF/Nav Accuracy";
                    this._ttffDisplay.Text = str;
                }
                this._ttffDisplay.updateMainWindow += new frmTTFFDisplay.updateParentEventHandler(this.updateSubWindowState);
            }
            if (this._interferenceReport != null)
            {
                if (!this._interferenceReport.IsDisposed)
                {
                    this._interferenceReport.CommWindow = this.comm;
                    this._interferenceReport.StartListen();
                    str = deviceName + ": Interference";
                    this._interferenceReport.Text = str;
                }
                this._interferenceReport.updateMainWindow += new frmInterferenceReport.updateParentEventHandler(this.updateSubWindowState);
            }
            if (this._SiRFAware != null)
            {
                if (!this._SiRFAware.IsDisposed)
                {
                    this._SiRFAware.CommWindow = this.comm;
                    this._SiRFAware.StartListen();
                    str = deviceName + ": SiRFaware";
                    this._SiRFAware.Text = str;
                }
                this._SiRFAware.updateMainWindow += new frmCommSiRFaware.updateParentEventHandler(this.updateSubWindowState);
            }
            if (this._responseView != null)
            {
                if (!this._responseView.IsDisposed)
                {
                    this._responseView.CommWindow = this.comm;
                    str = deviceName + ": Response View";
                    this._responseView.Text = str;
                }
                this._responseView.updateMainWindow += new frmCommResponseView.updateParentEventHandler(this.updateSubWindowState);
            }
            if (this._errorView != null)
            {
                if (!this._errorView.IsDisposed)
                {
                    this._errorView.CommWindow = this.comm;
                    str = deviceName + ": Error View";
                    this._errorView.Text = str;
                }
                this._errorView.updateMainWindow += new frmCommErrorView.updateParentEventHandler(this.updateSubWindowState);
            }
            this._lastWinTitle = this.Text;
            return str;
        }

        private void updateGui(object sender, DoWorkEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
        Label_000F:
            if (this._displayDataBG.CancellationPending || (this.comm == null))
            {
                e.Cancel = true;
            }
            else
            {
                this.rtbDisplayProccess();
                Thread.Sleep(100);
                goto Label_000F;
            }
        }

        public void UpdateGUIFromComm()
        {
            EventHandler method = null;
            EventHandler handler2 = null;
            if (this.comm.Log.IsFileOpen())
            {
                if (method == null)
                {
                    method = delegate {
                        this.logFileName.Text = this.comm.Log.filename;
                        this.frmCommOpenLogStatusLabel.Text = "logging";
                        this.btn_logFileBroswer.Enabled = false;
                        this.logFileName.Enabled = false;
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                if (handler2 == null)
                {
                    handler2 = delegate {
                        this.logFileName.Text = this.comm.Log.filename;
                        this.frmCommOpenLogStatusLabel.Text = "idle";
                        this.btn_logFileBroswer.Enabled = true;
                        this.logFileName.Enabled = true;
                    };
                }
                base.BeginInvoke(handler2);
            }
            if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
            {
                this.setRxMenuStatus(this.comm.comPort.IsOpen);
            }
            else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client)
            {
                this.setRxMenuStatus(this.comm.CMC.HostAppClient.IsOpen());
            }
            else if (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C)
            {
                this.setRxMenuStatus(this.comm.CMC.HostAppI2CSlave.IsOpen());
            }
            this.frmCommOpenUpdateConnectBtnImage();
        }

        public void UpdateMenuForTTB()
        {
            this.pollAlmanacMenu.Visible = false;
            this.pollEphemerisMenu.Visible = false;
            this.pollNavParametersMenu.Visible = false;
            this.toolStrip1.Visible = false;
            this.TTBMenuItem.Visible = false;
            this.rxSessionMenuItem.Visible = false;
            this.cwInterfenceDetectionMenu.Visible = false;
            this.SiRFAwareMenuItem.Visible = false;
            this.setDevelopersDebugMenu.Visible = false;
            this.switchOperationModeMenu.Visible = false;
            this.lowPowerMenu.Visible = false;
            this.trackerICPeekPokeMenu.Visible = false;
            this.trackerConfigurationMenu.Visible = false;
            this.inputCommandMenuItem.Visible = false;
            this.locationMapMenu.Visible = false;
            this.lowPowerCommandBufferMenu.Visible = false;
            this.TTFFMenuItem.Visible = false;
            this.setABPMenuItem.Visible = false;
            this.switchProtocolMenu.Visible = false;
            this.setMEMSMenuItem.Visible = false;
            this.TrackerConfigVer2.Visible = false;
            this.errorLogConfigMenuItem.Visible = false;
            this.toolStripMenuItem_SetAlm.Visible = false;
            this.toolStripMenuItem_SetEph.Visible = false;
            this.toolStripSeparator3.Visible = false;
            this.viewModeNmeaTextMenu.Visible = false;
            this.messagesFilterMenuItem.Visible = false;
            this.label9.Visible = false;
            this.logFileName.Visible = false;
            this.btn_logFileBroswer.Visible = false;
            this.frmCommOpenLogStatusLabel.Visible = false;
            this.frmCommOpenMarkerLabel.Visible = false;
            this.markerText.Visible = false;
            this.rtbDisplay.Visible = false;
            this.rxSettingsMenuItem.Visible = false;
            this.viewModeMenuItem.Visible = false;
            this.messagesMenuItem.Visible = false;
            this.toolStripSeparator2.Visible = false;
        }

        internal void updateSubWindowState(string subWinName)
        {
            EventHandler method = null;
            EventHandler handler2 = null;
            EventHandler handler3 = null;
            EventHandler handler4 = null;
            EventHandler handler5 = null;
            EventHandler handler6 = null;
            switch (subWinName)
            {
                case "frmCommInputMessage":
                    this._inputCommands = null;
                    return;

                case "frmCommRadarMap":
                    this._signalMapPanel = null;
                    if (method == null)
                    {
                        method = delegate {
                            this.toolBarRadarBtn.CheckState = CheckState.Unchecked;
                        };
                    }
                    base.Invoke(method);
                    return;

                case "frmEncryCtrl":
                    this._encryCtrl = null;
                    return;

                case "frmSatelliteStats":
                    this._SatelliteStats = null;
                    return;

                case "frmCommLocationMap":
                    this._locationViewPanel = null;
                    if (handler2 == null)
                    {
                        handler2 = delegate {
                            this.toolBarLocationBtn.CheckState = CheckState.Unchecked;
                        };
                    }
                    base.Invoke(handler2);
                    return;

                case "frmCommSignalView":
                    this._signalStrengthPanel = null;
                    if (handler3 == null)
                    {
                        handler3 = delegate {
                            this.toolBarSignalViewBtn.CheckState = CheckState.Unchecked;
                        };
                    }
                    base.Invoke(handler3);
                    return;

                case "frmCommMessageFilter":
                    this._messageFilterCustom = null;
                    return;

                case "frmTTFFDisplay":
                    this._ttffDisplay = null;
                    if (handler4 == null)
                    {
                        handler4 = delegate {
                            this.toolBarTTFFBtn.CheckState = CheckState.Unchecked;
                        };
                    }
                    base.Invoke(handler4);
                    return;

                case "frmInterferenceReport":
                    this._interferenceReport = null;
                    return;

                case "frmSiRFAware":
                    this._SiRFAware = null;
                    return;

                case "frmCommResponseView":
                    this._responseView = null;
                    if (handler5 == null)
                    {
                        handler5 = delegate {
                            this.toolBarResponseViewBtn.CheckState = CheckState.Unchecked;
                        };
                    }
                    base.Invoke(handler5);
                    return;

                case "frmCommErrorView":
                    this._errorView = null;
                    if (handler6 == null)
                    {
                        handler6 = delegate {
                            this.toolBarErrorViewBtn.CheckState = CheckState.Unchecked;
                        };
                    }
                    base.Invoke(handler6);
                    return;
            }
        }

        private void updateTTBToolMenu()
        {
            if (this.comm.TTBPort.IsOpen)
            {
                this.rxTTBConnectMenu.Checked = true;
            }
            else
            {
                this.rxTTBConnectMenu.Checked = false;
            }
        }

        private void updateTTFFWindow(object sender, DoWorkEventArgs ee)
        {
            if (this._ttffDisplay != null)
            {
                this._ttffDisplay.updateTTFFNow();
            }
        }

        public void UpdateWindowState(int state)
        {
			base.Invoke((MethodInvoker)delegate
			{
                switch (state)
                {
                    case 0:
                        this.WindowState = FormWindowState.Minimized;
                        return;

                    case 1:
                        this.WindowState = FormWindowState.Normal;
                        return;

                    case 2:
                        this.WindowState = FormWindowState.Maximized;
                        return;
                }
                this.WindowState = FormWindowState.Normal;
            });
        }

        internal void updateWinTitle(string title)
        {
            EventHandler method = null;
            try
            {
                this._lastWinTitle = title;
                if (method == null)
                {
                    method = delegate {
                        this.Text = title;
                    };
                }
                base.Invoke(method);
                if (this.comm.RxType != CommunicationManager.ReceiverType.TTB)
                {
                    if (clsGlobal.CommWinRef.Contains(this.comm.PortName))
                    {
                        if (this != ((frmCommOpen) clsGlobal.CommWinRef[this.comm.PortName]))
                        {
                            clsGlobal.CommWinRef.Remove(this.comm.PortName);
                        }
                    }
                    else
                    {
                        clsGlobal.CommWinRef.Add(this.comm.PortName, this);
                    }
                }
                this.UpdateGUIFromComm();
            }
            catch
            {
            }
        }

        private void userInputMenu_Click(object sender, EventArgs e)
        {
            this.CreateTransmitSerialMessageWin();
        }

        private void userMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateTransmitSerialMessageWin();
        }

        private void viewModeGP2Menu_Click(object sender, EventArgs e)
        {
            this.viewModeGP2Menu.Checked = true;
            this.viewModeHexMenu.CheckState = CheckState.Unchecked;
            this.viewModeNmeaTextMenu.CheckState = CheckState.Unchecked;
            this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
            this.viewModeGP2Menu.CheckState = CheckState.Checked;
            this.viewModeGPSMenu.CheckState = CheckState.Unchecked;
            this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GP2;
            this.comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.GP2;
            this.frmCommOpenUpdateStatusString();
        }

        private void viewModeGPSMenu_Click(object sender, EventArgs e)
        {
            this.viewModeGPSMenu.Checked = true;
            this.viewModeHexMenu.CheckState = CheckState.Unchecked;
            this.viewModeNmeaTextMenu.CheckState = CheckState.Unchecked;
            this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
            this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
            this.viewModeGPSMenu.CheckState = CheckState.Checked;
            this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GPS;
            this.comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.GPS;
            this.frmCommOpenUpdateStatusString();
        }

        private void viewModeHexMenu_Click(object sender, EventArgs e)
        {
            this.viewModeHexMenu.Checked = true;
            this.viewModeHexMenu.CheckState = CheckState.Checked;
            this.viewModeNmeaTextMenu.CheckState = CheckState.Unchecked;
            this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
            this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
            this.viewModeGPSMenu.CheckState = CheckState.Unchecked;
            this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Hex;
            this.comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.Hex;
            this.frmCommOpenUpdateStatusString();
        }

        private void viewModeNmeaTextMenu_Click(object sender, EventArgs e)
        {
            this.viewModeNmeaTextMenu.Checked = true;
            this.viewModeHexMenu.CheckState = CheckState.Unchecked;
            this.viewModeNmeaTextMenu.CheckState = CheckState.Checked;
            this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
            this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
            this.viewModeGPSMenu.CheckState = CheckState.Unchecked;
            this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Text;
            this.comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.Text;
            this.frmCommOpenUpdateStatusString();
        }

        private void viewModeSSBMenu_Click(object sender, EventArgs e)
        {
            this.viewModeSSBMenu.Checked = true;
            this.viewModeHexMenu.CheckState = CheckState.Unchecked;
            this.viewModeNmeaTextMenu.CheckState = CheckState.Unchecked;
            this.viewModeSSBMenu.CheckState = CheckState.Checked;
            this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
            this.viewModeGPSMenu.CheckState = CheckState.Unchecked;
            this.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.SSB;
            this.comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.SSB;
            this.frmCommOpenUpdateStatusString();
        }

        private void viewModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                switch (this.comm.RxCurrentTransmissionType)
                {
                    case CommunicationManager.TransmissionType.Text:
                        this.viewModeNmeaTextMenu.Checked = true;
                        this.viewModeNmeaTextMenu.CheckState = CheckState.Checked;
                        this.viewModeGPSMenu.CheckState = CheckState.Unchecked;
                        this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
                        this.viewModeHexMenu.CheckState = CheckState.Unchecked;
                        this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
                        return;

                    case CommunicationManager.TransmissionType.Hex:
                        this.viewModeHexMenu.Checked = true;
                        this.viewModeHexMenu.CheckState = CheckState.Checked;
                        this.viewModeGPSMenu.CheckState = CheckState.Unchecked;
                        this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
                        this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
                        this.viewModeNmeaTextMenu.CheckState = CheckState.Unchecked;
                        return;

                    case CommunicationManager.TransmissionType.SSB:
                        this.viewModeSSBMenu.Checked = true;
                        this.viewModeSSBMenu.CheckState = CheckState.Checked;
                        this.viewModeGPSMenu.CheckState = CheckState.Unchecked;
                        this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
                        this.viewModeHexMenu.CheckState = CheckState.Unchecked;
                        this.viewModeNmeaTextMenu.CheckState = CheckState.Unchecked;
                        return;

                    case CommunicationManager.TransmissionType.GP2:
                        this.viewModeGP2Menu.Checked = true;
                        this.viewModeGP2Menu.CheckState = CheckState.Checked;
                        this.viewModeGPSMenu.CheckState = CheckState.Unchecked;
                        this.viewModeHexMenu.CheckState = CheckState.Unchecked;
                        this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
                        this.viewModeNmeaTextMenu.CheckState = CheckState.Unchecked;
                        return;

                    case CommunicationManager.TransmissionType.GPS:
                        this.viewModeGPSMenu.Checked = true;
                        this.viewModeGPSMenu.CheckState = CheckState.Checked;
                        this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
                        this.viewModeHexMenu.CheckState = CheckState.Unchecked;
                        this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
                        this.viewModeNmeaTextMenu.CheckState = CheckState.Unchecked;
                        return;
                }
                this.viewModeGPSMenu.Checked = true;
                this.viewModeGPSMenu.CheckState = CheckState.Checked;
                this.viewModeGP2Menu.CheckState = CheckState.Unchecked;
                this.viewModeHexMenu.CheckState = CheckState.Unchecked;
                this.viewModeSSBMenu.CheckState = CheckState.Unchecked;
                this.viewModeNmeaTextMenu.CheckState = CheckState.Unchecked;
            }
        }

        private void viewWindowBufferSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmCommDisplayBufferSizeUpdate(this.comm).ShowDialog();
        }

        public bool IsIdle
        {
            get
            {
                return this._isIdle;
            }
            set
            {
                this._isIdle = value;
            }
        }

        public bool IsInit
        {
            get
            {
                return this._isInit;
            }
            set
            {
                this._isInit = value;
            }
        }

        public string WindowTitle
        {
            get
            {
                return this.Text.ToString();
            }
            set
            {
                this.Text = value;
            }
        }

        public delegate void updateParentEventHandler();

        public delegate void UpdateWindowEventHandler(string titleString, int left, int top, int width, int height, bool state);
    }
}

