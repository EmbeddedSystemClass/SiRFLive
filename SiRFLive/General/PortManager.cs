﻿namespace SiRFLive.General
{
    using CommonClassLibrary;
    using CommonUtilsClassLibrary;
    using OpenNETCF.IO.Serial;
    using SiRFLive.Analysis;
    using SiRFLive.Communication;
    using SiRFLive.GUI;
    using SiRFLive.GUI.Commmunication;
    using SiRFLive.GUI.DlgsInputMsg;
    using SiRFLive.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.IO.Ports;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Timers;
    using System.Windows.Forms;

    public class PortManager
    {
        private string _almanacDataFilename = string.Empty;
        private StreamWriter _almanacDataStreamWriter;
        public frmCompassView _compassView;
        private int _CtSerialErrors;
        private BackgroundWorker _displayDataBG = new BackgroundWorker();
        public frmCommDRStatus _DRNavStatusViewPanel;
        public frmCommDRSensor _DRSensorViewPanel;
        private int _eph_Msg_SV_ID;
        private string _eph_non_str = string.Empty;
        private bool _eph_rcvd;
        private string _ephDataFilename = string.Empty;
        private StreamWriter _ephDataStreamWriter;
        public frmCommErrorView _errorView;
        public frmCommInputMessage _inputCommands;
        public frmInterferenceReport _interferenceReport;
        public frmCommLocationMap _locationViewPanel;
        public frmLPBufferWindow _lowPowerBufferView;
        public frmMEMSView _memsView;
        private BackgroundWorker _parseDataBG = new BackgroundWorker();
        public frmPeekPokeMem _peekPokeWin;
        private System.Timers.Timer _pollAlmanacTimer = new System.Timers.Timer();
        private BackgroundWorker _readPortDataBG = new BackgroundWorker();
        public frmRXInit_cmd _resetCmd;
        public frmCommResponseView _responseView;
        public frmSatelliteStats _SatelliteStats;
        public frmCommSignalView _signalStrengthPanel;
        public frmCommSiRFawareV2 _SiRFAware;
        public frmCommSVAvgCNo _svCNoView;
        public frmCommSVAvgCNo _svsCNoPanel;
        public frmCommRadarMap _svsMapPanel;
        public frmCommSVTrackedVsTime _svsTrackedVsTimePanel;
        public frmCommSVTrajectory _svsTrajPanel;
        public frmCommSVTrackedVsTime _svTrackedVsTimeView;
        public frmCommSVTrajectory _svTrajView;
        public frmTransmitSerialMessage _transmitSerialMessageWin;
        public frmCommSignalView _ttbSigWin;
        public frmCommOpen _ttbWin;
        public frmTTFFDisplay _ttffDisplay;
        public CommunicationManager comm = new CommunicationManager();
        public ObjectInterface commElements = new ObjectInterface();
        public WinLocation CompassViewLocation = new WinLocation();
        public CommonUtilsClass CUC = new CommonUtilsClass();
        public frmCommDebugView DebugView;
        public WinLocation DebugViewLocation = new WinLocation();
        public WinLocation ErrorViewLocation = new WinLocation();
        public WinLocation InputCommandLocation = new WinLocation();
        public WinLocation InterferenceLocation = new WinLocation();
        public WinLocation LocationDRSensorLocation = new WinLocation();
        public WinLocation LocationDRStatusLocation = new WinLocation();
        public WinLocation LocationMapLocation = new WinLocation();
        public frmLogDuration LogFileWin;
        public WinLocation MEMSLocation = new WinLocation();
        public frmCommMessageFilter MessageView;
        public WinLocation MessageViewLocation = new WinLocation();
        public WinLocation NavVsTimeLocation = new WinLocation();
        public frmCommNavAccVsTime NavVsTimeView;
        public WinLocation PeekPokeLocation = new WinLocation();
        public ToolStrip PerPortToolStrip;
        private System.Timers.Timer pollEphTimer = new System.Timers.Timer();
        public bool ReconnectTTB;
        public WinLocation ResponseViewLocation = new WinLocation();
        public WinLocation SatelliteStatsLocation = new WinLocation();
        public WinLocation SignalViewLocation = new WinLocation();
        public WinLocation SiRFawareLocation = new WinLocation();
        public WinLocation SVCNoViewLocation = new WinLocation();
        public WinLocation SVsMapLocation = new WinLocation();
        public WinLocation SVTrackedVsTimeViewLocation = new WinLocation();
        public WinLocation SVTrajViewLocation = new WinLocation();
        public WinLocation TransmitSerialMessageLocation = new WinLocation();
        public string transType = string.Empty;
        public UART_Properties TTBPortProperties;
        public WinLocation TTBWinLocation = new WinLocation();
        public WinLocation TTFFDisplayLocation = new WinLocation();
        public List<TTFSData> TTFSDataList = new List<TTFSData>();
        public frmTTFSView TTFSView;
        public WinLocation TTFSViewLocation = new WinLocation();

        public event updateParentEventHandler UpdateMainWindow;

        public PortManager()
        {
            this._displayDataBG.WorkerSupportsCancellation = true;
            this._parseDataBG.WorkerSupportsCancellation = true;
            this._readPortDataBG.WorkerSupportsCancellation = true;
            this._displayDataBG.DoWork += new DoWorkEventHandler(this.updateGui);
            this._parseDataBG.DoWork += new DoWorkEventHandler(this.parseDataBGProcess);
            this._readPortDataBG.DoWork += new DoWorkEventHandler(this.readPortDataProcess);
            this.RunAsyncProcess();
        }

        public void AutoTestCloseMicsWindows()
        {
            EventHandler method = null;
            EventHandler handler2 = null;
            EventHandler handler3 = null;
            EventHandler handler4 = null;
            EventHandler handler5 = null;
            EventHandler handler6 = null;
            EventHandler handler7 = null;
            EventHandler handler8 = null;
            EventHandler handler9 = null;
            EventHandler handler10 = null;
            EventHandler handler11 = null;
            EventHandler handler12 = null;
            EventHandler handler13 = null;
            if ((this._locationViewPanel != null) && this.LocationMapLocation.IsOpen)
            {
                if (method == null)
                {
                    method = delegate {
                        this._locationViewPanel.Close();
                    };
                }
                this._locationViewPanel.BeginInvoke(method);
            }
            if ((this._DRNavStatusViewPanel != null) && this.LocationDRStatusLocation.IsOpen)
            {
                if (handler2 == null)
                {
                    handler2 = delegate {
                        this._DRNavStatusViewPanel.Close();
                    };
                }
                this._DRNavStatusViewPanel.BeginInvoke(handler2);
            }
            if ((this._DRSensorViewPanel != null) && this.LocationDRSensorLocation.IsOpen)
            {
                if (handler3 == null)
                {
                    handler3 = delegate {
                        this._DRSensorViewPanel.Close();
                    };
                }
                this._DRSensorViewPanel.BeginInvoke(handler3);
            }
            if ((this._inputCommands != null) && this.InputCommandLocation.IsOpen)
            {
                if (handler4 == null)
                {
                    handler4 = delegate {
                        this._inputCommands.Close();
                    };
                }
                this._inputCommands.BeginInvoke(handler4);
            }
            if ((this._svsMapPanel != null) && this.SVsMapLocation.IsOpen)
            {
                if (handler5 == null)
                {
                    handler5 = delegate {
                        this._svsMapPanel.Close();
                    };
                }
                this._svsMapPanel.BeginInvoke(handler5);
            }
            if ((this._svsTrajPanel != null) && this.SVTrajViewLocation.IsOpen)
            {
                if (handler6 == null)
                {
                    handler6 = delegate {
                        this._svsTrajPanel.Close();
                    };
                }
                this._svsTrajPanel.BeginInvoke(handler6);
            }
            if ((this._svsCNoPanel != null) && this.SVCNoViewLocation.IsOpen)
            {
                if (handler7 == null)
                {
                    handler7 = delegate {
                        this._svsCNoPanel.Close();
                    };
                }
                this._svsCNoPanel.BeginInvoke(handler7);
            }
            if ((this._svsTrackedVsTimePanel != null) && this.SVTrackedVsTimeViewLocation.IsOpen)
            {
                if (handler8 == null)
                {
                    handler8 = delegate {
                        this._svsTrackedVsTimePanel.Close();
                    };
                }
                this._svsTrackedVsTimePanel.BeginInvoke(handler8);
            }
            if ((this._SatelliteStats != null) && this.SatelliteStatsLocation.IsOpen)
            {
                if (handler9 == null)
                {
                    handler9 = delegate {
                        this._SatelliteStats.Close();
                    };
                }
                this._SatelliteStats.BeginInvoke(handler9);
            }
            if ((this._interferenceReport != null) && this.InterferenceLocation.IsOpen)
            {
                if (handler10 == null)
                {
                    handler10 = delegate {
                        this._interferenceReport.Close();
                    };
                }
                this._interferenceReport.BeginInvoke(handler10);
            }
            if ((this._SiRFAware != null) && this.SiRFawareLocation.IsOpen)
            {
                if (handler11 == null)
                {
                    handler11 = delegate {
                        this._SiRFAware.Close();
                    };
                }
                this._SiRFAware.BeginInvoke(handler11);
            }
            if ((this._compassView != null) && this.CompassViewLocation.IsOpen)
            {
                if (handler12 == null)
                {
                    handler12 = delegate {
                        this._compassView.Close();
                    };
                }
                this._compassView.BeginInvoke(handler12);
            }
            if ((this._interferenceReport != null) && this.InterferenceLocation.IsOpen)
            {
                if (handler13 == null)
                {
                    handler13 = delegate {
                        this._interferenceReport.StopListeners();
                        this._interferenceReport.Close();
                    };
                }
                this._interferenceReport.BeginInvoke(handler13);
            }
        }

        public void clearGUIDataTimerHandler(object source, ElapsedEventArgs e)
        {
            this.ClearSubWindowsData(false);
        }

        public void ClearSubWindowsData(bool clearTTFF)
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
                        for (int m = 0; m < 10; m++)
                        {
                            this.comm.dataGui.SignalDataForGUI.CHAN_MEAS_CNO[j][m] = 0;
                        }
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
                    if (this.comm.DisplayPanelDRStatusStates != null)
                    {
                        this.comm.DisplayPanelDRStatusStates.Invalidate();
                    }
                    if (this.comm.DisplayPanelDRSensors != null)
                    {
                        this.comm.DisplayPanelDRSensors.Invalidate();
                    }
                    if (this.comm.DisplayPanelSatelliteStats != null)
                    {
                        this.comm.DisplayPanelSatelliteStats.Invalidate();
                    }
                    if (clearTTFF && (this._ttffDisplay != null))
                    {
                        this._ttffDisplay.ClearTTFFData();
                    }
                    clsGlobal.g_objfrmMDIMain.UpdateStatusString("All");
                }
            }
            catch
            {
            }
        }

        public void CloseAll()
        {
            clsGlobal.LoopitInProgress = false;
            this.StopAsyncProcess();
            this.portManagerCleanup();
        }

        private void comPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if ((this.comm.ProductFamily == CommonClass.ProductType.GSD4t) && (this.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232))
            {
                this._CtSerialErrors++;
                if (this._CtSerialErrors >= 100)
                {
                    if (this._CtSerialErrors > 0xffdc)
                    {
                        this._CtSerialErrors = 0;
                    }
                    System.Threading.Thread.Sleep(20);
                    string msg = string.Empty;
                    msg = e.EventType.ToString();
                    this.comm.ErrorViewRTBDisplay.DisplayData(CommonClass.MessageType.Error, msg);
                }
            }
        }

        private int countStringOccurrences(string text, string pattern)
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

        public void GetOSPPositionFormCSVString(string line)
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
                if (!this.comm.RxCtrl.ResetCtrl.ResetPositionAvailable)
                {
                    this.comm.RxCtrl.RxNavData.TTFFReport = this.comm.RxCtrl.RxNavData.TTFFSiRFLive;
                    this.comm.RxCtrl.RxNavData.FirstFixMeasLat = this.comm.RxCtrl.RxNavData.MeasLat;
                    this.comm.RxCtrl.RxNavData.FirstFixMeasLon = this.comm.RxCtrl.RxNavData.MeasLon;
                    this.comm.RxCtrl.RxNavData.FirstFixMeasAlt = this.comm.RxCtrl.RxNavData.MeasAlt;
                    this.comm.RxCtrl.RxNavData.FirstFix2DPositionError = this.comm.RxCtrl.RxNavData.Nav2DPositionError;
                    this.comm.RxCtrl.RxNavData.FirstFix3DPositionError = this.comm.RxCtrl.RxNavData.Nav3DPositionError;
                    this.comm.RxCtrl.RxNavData.FirstFixVerticalPositionError = this.comm.RxCtrl.RxNavData.NavVerticalPositionError;
                    this.comm.RxCtrl.RxNavData.FirstFixTOW = this.comm.RxCtrl.RxNavData.TOW;
                    this.comm.RxCtrl.ResetCtrl.ResetPositionAvailable = true;
                }
            }
        }

        public void GetTTFFFromCSVString(string line)
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

        private void onTimerPollAlmanacEvent(object source, ElapsedEventArgs e)
        {
            this.comm.ListenersCtrl.Stop("PollAlm_GUI");
            this.comm.ListenersCtrl.Delete("PollAlm_GUI");
            this._almanacDataStreamWriter.Close();
            this._pollAlmanacTimer.Stop();
            this._pollAlmanacTimer.Close();
        }

        private void onTimerPollEphEvent(object source, ElapsedEventArgs e)
        {
            for (int i = this._eph_Msg_SV_ID; i < 0x20; i++)
            {
                this._ephDataStreamWriter.WriteLine(this._eph_non_str);
            }
            this.comm.ListenersCtrl.Stop("PollEph_GUI");
            this.comm.ListenersCtrl.Delete("PollEph_GUI");
            this._ephDataStreamWriter.Close();
            this.pollEphTimer.Stop();
            this.pollEphTimer.Close();
        }

        private void parseDataBGProcess(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
        Label_000F:
            if (this._parseDataBG.CancellationPending)
            {
                e.Cancel = true;
            }
            else if (this.comm != null)
            {
                if (!this.comm.IsSourceDeviceOpen())
                {
                    System.Threading.Thread.Sleep(250);
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
                    System.Threading.Thread.Sleep(20);
                }
                goto Label_000F;
            }
        }

        public void PollAlmanacHandler()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.CreatePrompt = true;
            dialog.OverwritePrompt = true;
            dialog.DefaultExt = "alm";
            dialog.FileName = "almanac.alm";
            dialog.Filter = "Almanac files (*.alm)|*.alm";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this._almanacDataFilename = dialog.FileName;
                this._almanacDataStreamWriter = new StreamWriter(this._almanacDataFilename);
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
                        content.DoUserWork.DoWork += new DoWorkEventHandler(this.pollAlmanacListenerHandler);
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
                this._almanacDataStreamWriter.Write(str2);
                this._pollAlmanacTimer.Elapsed += new ElapsedEventHandler(this.onTimerPollAlmanacEvent);
                this._pollAlmanacTimer.Interval = 5000.0;
                this._pollAlmanacTimer.AutoReset = false;
                this._pollAlmanacTimer.Start();
                string msg = this.comm.m_Protocols.GetDefaultMsgtoSend(false, 0x92, -1, "Poll Almanac", "SSB");
                this.comm.WriteData(msg);
                this.comm.WriteApp(msg);
            }
        }

        private void pollAlmanacListenerHandler(object sender, DoWorkEventArgs myQContent)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
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
            if (this._almanacDataStreamWriter != null)
            {
                this._almanacDataStreamWriter.WriteLine(str4);
            }
        }

        public void PollEphemerisHandler()
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
                        content.DoUserWork.DoWork += new DoWorkEventHandler(this.pollEphListenerHandler);
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
                this.pollEphTimer.Elapsed += new ElapsedEventHandler(this.onTimerPollEphEvent);
                this.pollEphTimer.Interval = 5000.0;
                this.pollEphTimer.AutoReset = false;
                this.pollEphTimer.Start();
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

        private void pollEphListenerHandler(object sender, DoWorkEventArgs myQContent)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
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
                int num2 = this.countStringOccurrences(text, ",");
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

        public void portManagerCleanup()
        {
            EventHandler method = null;
            EventHandler handler2 = null;
            EventHandler handler3 = null;
            EventHandler handler4 = null;
            EventHandler handler5 = null;
            EventHandler handler6 = null;
            EventHandler handler7 = null;
            EventHandler handler8 = null;
            EventHandler handler9 = null;
            EventHandler handler10 = null;
            EventHandler handler11 = null;
            EventHandler handler12 = null;
            EventHandler handler13 = null;
            EventHandler handler14 = null;
            EventHandler handler15 = null;
            EventHandler handler16 = null;
            EventHandler handler17 = null;
            EventHandler handler18 = null;
            EventHandler handler19 = null;
            EventHandler handler20 = null;
            EventHandler handler21 = null;
            EventHandler handler22 = null;
            EventHandler handler23 = null;
            EventHandler handler24 = null;
            EventHandler handler25 = null;
            if (this._signalStrengthPanel != null)
            {
                if (this._signalStrengthPanel.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this._signalStrengthPanel.Close();
                        };
                    }
                    this._signalStrengthPanel.BeginInvoke(method);
                }
                else
                {
                    this._signalStrengthPanel.Close();
                }
            }
            if (this._locationViewPanel != null)
            {
                if (this._locationViewPanel.InvokeRequired)
                {
                    if (handler2 == null)
                    {
                        handler2 = delegate {
                            this._locationViewPanel.Close();
                        };
                    }
                    this._locationViewPanel.BeginInvoke(handler2);
                }
                else
                {
                    this._locationViewPanel.Close();
                }
            }
            if (this._DRNavStatusViewPanel != null)
            {
                if (this._DRNavStatusViewPanel.InvokeRequired)
                {
                    if (handler3 == null)
                    {
                        handler3 = delegate {
                            this._DRNavStatusViewPanel.Close();
                        };
                    }
                    this._DRNavStatusViewPanel.BeginInvoke(handler3);
                }
                else
                {
                    this._DRNavStatusViewPanel.Close();
                }
            }
            if (this._DRSensorViewPanel != null)
            {
                if (this._DRSensorViewPanel.InvokeRequired)
                {
                    if (handler4 == null)
                    {
                        handler4 = delegate {
                            this._DRSensorViewPanel.Close();
                        };
                    }
                    this._DRSensorViewPanel.BeginInvoke(handler4);
                }
                else
                {
                    this._DRSensorViewPanel.Close();
                }
            }
            if (this._inputCommands != null)
            {
                if (this._inputCommands.InvokeRequired)
                {
                    if (handler5 == null)
                    {
                        handler5 = delegate {
                            this._inputCommands.Close();
                        };
                    }
                    this._inputCommands.BeginInvoke(handler5);
                }
                else
                {
                    this._inputCommands.Close();
                }
            }
            if (this._transmitSerialMessageWin != null)
            {
                if (this._transmitSerialMessageWin.InvokeRequired)
                {
                    if (handler6 == null)
                    {
                        handler6 = delegate {
                            this._transmitSerialMessageWin.Close();
                        };
                    }
                    this._transmitSerialMessageWin.BeginInvoke(handler6);
                }
                else
                {
                    this._transmitSerialMessageWin.Close();
                }
            }
            if (this._svsMapPanel != null)
            {
                if (this._svsMapPanel.InvokeRequired)
                {
                    if (handler7 == null)
                    {
                        handler7 = delegate {
                            this._svsMapPanel.Close();
                        };
                    }
                    this._svsMapPanel.BeginInvoke(handler7);
                }
                else
                {
                    this._svsMapPanel.Close();
                }
            }
            if (this._svsTrajPanel != null)
            {
                if (this._svsTrajPanel.InvokeRequired)
                {
                    if (handler8 == null)
                    {
                        handler8 = delegate {
                            this._svsTrajPanel.Close();
                        };
                    }
                    this._svsTrajPanel.BeginInvoke(handler8);
                }
                else
                {
                    this._svsTrajPanel.Close();
                }
            }
            if (this._svsCNoPanel != null)
            {
                if (this._svsCNoPanel.InvokeRequired)
                {
                    if (handler9 == null)
                    {
                        handler9 = delegate {
                            this._svsCNoPanel.Close();
                        };
                    }
                    this._svsCNoPanel.BeginInvoke(handler9);
                }
                else
                {
                    this._svsCNoPanel.Close();
                }
            }
            if (this._SatelliteStats != null)
            {
                if (this._SatelliteStats.InvokeRequired)
                {
                    if (handler10 == null)
                    {
                        handler10 = delegate {
                            this._SatelliteStats.Close();
                        };
                    }
                    this._SatelliteStats.BeginInvoke(handler10);
                }
                else
                {
                    this._SatelliteStats.Close();
                }
            }
            if (this._ttffDisplay != null)
            {
                if (this._ttffDisplay.InvokeRequired)
                {
                    if (handler11 == null)
                    {
                        handler11 = delegate {
                            this._ttffDisplay.Close();
                        };
                    }
                    this._ttffDisplay.BeginInvoke(handler11);
                }
                else
                {
                    this._ttffDisplay.Close();
                }
            }
            if (this._interferenceReport != null)
            {
                if (this._interferenceReport.InvokeRequired)
                {
                    if (handler12 == null)
                    {
                        handler12 = delegate {
                            this._interferenceReport.Close();
                        };
                    }
                    this._interferenceReport.BeginInvoke(handler12);
                }
                else
                {
                    this._interferenceReport.Close();
                }
            }
            if (this._SiRFAware != null)
            {
                if (this._SiRFAware.InvokeRequired)
                {
                    if (handler13 == null)
                    {
                        handler13 = delegate {
                            this._SiRFAware.Close();
                        };
                    }
                    this._SiRFAware.BeginInvoke(handler13);
                }
                else
                {
                    this._SiRFAware.Close();
                }
            }
            if (this.MessageView != null)
            {
                if (this.MessageView.InvokeRequired)
                {
                    if (handler14 == null)
                    {
                        handler14 = delegate {
                            this.MessageView.StopListeners();
                            this.MessageView.Close();
                        };
                    }
                    this.MessageView.BeginInvoke(handler14);
                }
                else
                {
                    this.MessageView.StopListeners();
                    this.MessageView.Close();
                }
            }
            if (this._ttffDisplay != null)
            {
                if (this._ttffDisplay.InvokeRequired)
                {
                    if (handler15 == null)
                    {
                        handler15 = delegate {
                            this._ttffDisplay.Close();
                        };
                    }
                    this._ttffDisplay.BeginInvoke(handler15);
                }
                else
                {
                    this._ttffDisplay.Close();
                }
            }
            if (this._interferenceReport != null)
            {
                if (this._interferenceReport.InvokeRequired)
                {
                    if (handler16 == null)
                    {
                        handler16 = delegate {
                            this._interferenceReport.StopListeners();
                            this._interferenceReport.Close();
                        };
                    }
                    this._interferenceReport.BeginInvoke(handler16);
                }
                else
                {
                    this._interferenceReport.StopListeners();
                    this._interferenceReport.Close();
                }
            }
            if (this._responseView != null)
            {
                if (this._responseView.InvokeRequired)
                {
                    if (handler17 == null)
                    {
                        handler17 = delegate {
                            this._responseView.Close();
                        };
                    }
                    this._responseView.BeginInvoke(handler17);
                }
                else
                {
                    this._responseView.Close();
                }
            }
            if (this._errorView != null)
            {
                if (this._errorView.InvokeRequired)
                {
                    if (handler18 == null)
                    {
                        handler18 = delegate {
                            this._errorView.Close();
                        };
                    }
                    this._errorView.BeginInvoke(handler18);
                }
                else
                {
                    this._errorView.Close();
                }
            }
            if (this.DebugView != null)
            {
                if (this.DebugView.InvokeRequired)
                {
                    if (handler19 == null)
                    {
                        handler19 = delegate {
                            this.DebugView.Close();
                        };
                    }
                    this.DebugView.BeginInvoke(handler19);
                }
                else
                {
                    this.DebugView.Close();
                }
            }
            if (this._compassView != null)
            {
                if (this._compassView.InvokeRequired)
                {
                    if (handler20 == null)
                    {
                        handler20 = delegate {
                            this._compassView.Close();
                        };
                    }
                    this._compassView.BeginInvoke(handler20);
                }
                else
                {
                    this._compassView.Close();
                }
            }
            if (this._memsView != null)
            {
                if (this._memsView.InvokeRequired)
                {
                    if (handler21 == null)
                    {
                        handler21 = delegate {
                            this._memsView.Close();
                        };
                    }
                    this._memsView.BeginInvoke(handler21);
                }
                else
                {
                    this._memsView.Close();
                }
            }
            if (this.NavVsTimeView != null)
            {
                if (this.NavVsTimeView.InvokeRequired)
                {
                    if (handler22 == null)
                    {
                        handler22 = delegate {
                            this.NavVsTimeView.Close();
                        };
                    }
                    this.NavVsTimeView.BeginInvoke(handler22);
                }
                else
                {
                    this.NavVsTimeView.Close();
                }
            }
            if (this._svsTrackedVsTimePanel != null)
            {
                if (this._svsTrackedVsTimePanel.InvokeRequired)
                {
                    if (handler23 == null)
                    {
                        handler23 = delegate {
                            this._svsTrackedVsTimePanel.Close();
                        };
                    }
                    this._svsTrackedVsTimePanel.BeginInvoke(handler23);
                }
                else
                {
                    this._svsTrackedVsTimePanel.Close();
                }
            }
            if (this.TTFSView != null)
            {
                if (this.TTFSView.InvokeRequired)
                {
                    if (handler24 == null)
                    {
                        handler24 = delegate {
                            this.TTFSView.Close();
                        };
                    }
                    this.TTFSView.BeginInvoke(handler24);
                }
                else
                {
                    this.TTFSView.Close();
                }
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
                    this.comm.ListenersCtrl.Stop();
                    this.comm.ListenersCtrl.Cleanup();
                }
                if (this.comm.RxCtrl != null)
                {
                    if (this.comm.RxCtrl.ResetCtrl != null)
                    {
                        this.comm.RxCtrl.ResetCtrl.ResetTimerStop(true);
                        this.comm.RxCtrl.ResetCtrl.CloseTTFFLog();
                        this.comm.RxCtrl.ResetCtrl = null;
                    }
                    this.comm.RxCtrl.LogCleanup();
                    this.comm.RxCtrl.Dispose();
                    this.comm.RxCtrl = null;
                }
                if (this.comm.TTBPort.IsOpen)
                {
                    this.comm.TTBPort.Close();
                    if (this._ttbWin != null)
                    {
                        if (this._ttbWin.InvokeRequired)
                        {
                            if (handler25 == null)
                            {
                                handler25 = delegate {
                                    this._ttbWin.Close();
                                    this._ttbWin = null;
                                };
                            }
                            this._ttbWin.BeginInvoke(handler25);
                        }
                        else
                        {
                            this._ttbWin.Close();
                            this._ttbWin = null;
                        }
                    }
                }
                this.comm.ClosePort();
            }
            if (this.TTFSDataList != null)
            {
                this.TTFSDataList.Clear();
            }
            if (clsGlobal.CommWinRef.Contains(this.comm.PortName))
            {
                clsGlobal.CommWinRef.Remove(this.comm.PortName);
            }
        }

        private void readPortDataProcess(object sender, DoWorkEventArgs e)
        {
        Label_0000:
            if (this._readPortDataBG.CancellationPending)
            {
                e.Cancel = true;
            }
            else if (this.comm != null)
            {
                if (!this.comm.IsSourceDeviceOpen() || (this.comm.InputDeviceMode != CommonClass.InputDeviceModes.RS232))
                {
                    System.Threading.Thread.Sleep(250);
                }
                else if (this.comm.comPortDataReceivedHandler() <= 0)
                {
                    System.Threading.Thread.Sleep(10);
                }
                else
                {
                    System.Threading.Thread.Sleep(3);
                }
                goto Label_0000;
            }
        }

        private void rtbDisplayProccess()
        {
            MessageQData argument = new MessageQData();
            Queue queue = new Queue();
            int num = 0;
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.AboveNormal;
            lock (this.comm.DisplayDataLock)
            {
                while (this.comm.DisplayQueue.Count > 0)
                {
                    queue.Enqueue(this.comm.DisplayQueue.Dequeue());
                }
            }
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Normal;
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
                    num++;
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
                            goto Label_03AB;

                        case CommunicationManager.TransmissionType.GP2:
                            csvString = CommonUtilsClass.LogToGP2(argument.MessageText, argument.MessageTime);
                            this.comm.dataGui.AGC_Gain = 0;
                            goto Label_03AB;

                        case CommunicationManager.TransmissionType.GPS:
                            csvString = this.comm.LogToCSV(argument.MessageText);
                            if (this.comm.MessageProtocol == "OSP")
                            {
                                if (argument.MessageId != 0x45)
                                {
                                    goto Label_02EB;
                                }
                                if (argument.MessageSubId != 1)
                                {
                                    break;
                                }
                                csvString = csvString + this.comm.RxCtrl.FormatPositionResponse(csvString);
                            }
                            goto Label_03AB;

                        default:
                            csvString = argument.MessageText;
                            flag = true;
                            goto Label_03AB;
                    }
                    if (argument.MessageSubId == 2)
                    {
                        csvString = csvString + this.comm.RxCtrl.FormatMeasurementResponse(csvString);
                    }
                }
                goto Label_03AB;
            Label_02EB:
                if (argument.MessageId == 0x13)
                {
                    if (argument.MessageChanId != 2)
                    {
                        csvString = csvString + this.comm.StringRxNavParams;
                    }
                }
                else if (argument.MessageId == 0x4d)
                {
                    Hashtable myHash = this.comm.m_Protocols.ConvertRawToHash(HelperFunctions.HexToByte(argument.MessageText), "OSP");
                    csvString = csvString + this.comm.RxCtrl.PrintFormatedMPMStatus(myHash);
                }
            Label_03AB:
                if ((csvString == null) && !(csvString != string.Empty))
                {
                    continue;
                }
                if (((argument.MessageId == 0xe1) || (argument.MessageId == 0xff)) || (argument.MessageId == 0x44))
                {
                    flag = true;
                    num++;
                    if ((((argument.MessageId == 0xff) && (this.comm.RxCtrl.ResetCtrl != null)) && (!this.comm.RxCtrl.ResetCtrl.IsFirstTTFS && csvString.Contains("BEP:SetTime(FSf)"))) && ((this.TTFSView != null) && this.TTFSViewLocation.IsOpen))
                    {
                        string str2 = @"(?<time>\d+)\s*BEP:SetTime\(FSf\)\s*Y\w\s*T:(?<tow>\d+.\d+)\s*(?<week>\d+)";
                        Regex regex = new Regex(str2, RegexOptions.Compiled);
                        if (regex.IsMatch(csvString))
                        {
                            try
                            {
                                this.TTFSView.TTFSDataElement.TTFS = Convert.ToDouble(regex.Match(csvString).Result("${time}")) / 1000.0;
                                this.TTFSView.TTFSDataElement.TOW = Convert.ToDouble(regex.Match(csvString).Result("${tow}"));
                                this.TTFSView.TTFSDataElement.Week = Convert.ToInt32(regex.Match(csvString).Result("${week}"));
                                this.TTFSView.TTFSDataElement.isValid = true;
                                this.comm.RxCtrl.ResetCtrl.IsFirstTTFS = true;
                                this.TTFSView.TTFSDataElement.TTFSDataView.Invalidate();
                                csvString = csvString + string.Format("\r\n TTFS = {0:F2},{1:F6}", this.TTFSView.TTFSDataElement.TTFS, this.TTFSView.TTFSDataElement.TOW);
                            }
                            catch
                            {
                            }
                        }
                    }
                    string pattern = @"gain (?<gain>\d+)";
                    Regex regex2 = new Regex(pattern, RegexOptions.Compiled);
                    if (regex2.IsMatch(csvString))
                    {
                        try
                        {
                            this.comm.dataGui.AGC_Gain = Convert.ToInt32(regex2.Match(csvString).Result("${gain}"));
                        }
                        catch
                        {
                        }
                    }
                    lock (this.comm.LockErrorLog)
                    {
                        foreach (string str4 in this.comm.ErrorStringList)
                        {
                            if (csvString.Contains(str4))
                            {
                                this.comm.Log.ErrorWriteLine(csvString);
                            }
                        }
                        goto Label_063C;
                    }
                }
                if (this.comm.ViewAll)
                {
                    flag = true;
                }
            Label_063C:
                if (((this.comm.DebugViewRTBDisplay != null) && !this.comm.DebugViewRTBDisplay.viewPause) && flag)
                {
                    CommonUtilsClass debugViewRTBDisplay = this.comm.DebugViewRTBDisplay;
                    debugViewRTBDisplay.LineCount += num;
                    if (((this.comm.DebugRegExpressionHandler != null) && this.comm.DebugViewIsMatchEnable) && this.comm.DebugRegExpressionHandler.IsMatch(csvString))
                    {
                        argument.MessageType = CommonClass.MessageType.Matching;
                    }
                    this.comm.DebugViewRTBDisplay.DisplayData(argument.MessageType, csvString);
                }
                if (this.comm.Log.IsFileOpen())
                {
                    string msg = string.Empty;
                    if (((this.comm.RxTransType == this.comm.LogFormat) || (argument.MessageSource == CommonClass.MessageSource.RX_INPUT)) || (argument.MessageSource == CommonClass.MessageSource.USER_TEXT))
                    {
                        msg = csvString;
                    }
                    else
                    {
                        switch (this.comm.LogFormat)
                        {
                            case CommunicationManager.TransmissionType.GP2:
                                msg = CommonUtilsClass.LogToGP2(argument.MessageText, argument.MessageTime);
                                break;

                            case CommunicationManager.TransmissionType.GPS:
                                msg = this.comm.LogToCSV(argument.MessageText);
                                break;
                        }
                    }
                    if (msg != string.Empty)
                    {
                        if (!this.comm.IsUserSpecifiedMsgLog)
                        {
                            this.comm.Log.WriteLine(msg);
                            continue;
                        }
                        for (int i = 0; i < this.comm.UserSpecifiedMsgList.Count; i++)
                        {
                            if (this.comm.UserSpecifiedSubStringList[i] == "N")
                            {
                                if (msg.StartsWith(this.comm.UserSpecifiedMsgList[i]))
                                {
                                    this.comm.Log.WriteLine(msg);
                                }
                            }
                            else if (msg.Contains(this.comm.UserSpecifiedMsgList[i]))
                            {
                                this.comm.Log.WriteLine(msg);
                            }
                        }
                    }
                }
            }
        }

        public void RunAsyncProcess()
        {
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

        private void updateGui(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
            while (true)
            {
                try
                {
                    if (this._displayDataBG.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    if (this.comm == null)
                    {
                        break;
                    }
                    if (this.comm.IsSourceDeviceOpen())
                    {
                        this.rtbDisplayProccess();
                        System.Threading.Thread.Sleep(100);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(250);
                    }
                }
                catch
                {
                }
            }
        }

        public void UpdateSubWindowOnClosed(string subWinName, int left, int top, int width, int height, bool state)
        {
            switch (subWinName)
            {
                case "frmCommSignalView":
                    if (this.SignalViewLocation != null)
                    {
                        this.SignalViewLocation.Height = height;
                        this.SignalViewLocation.Width = width;
                        this.SignalViewLocation.Left = left;
                        this.SignalViewLocation.Top = top;
                        this.SignalViewLocation.IsOpen = state;
                    }
                    break;

                case "frmCommDebugView":
                    if (this.DebugViewLocation != null)
                    {
                        this.DebugViewLocation.Height = height;
                        this.DebugViewLocation.Width = width;
                        this.DebugViewLocation.Left = left;
                        this.DebugViewLocation.Top = top;
                        this.DebugViewLocation.IsOpen = state;
                    }
                    break;

                case "frmTTFFDisplay":
                    if (this.TTFFDisplayLocation != null)
                    {
                        this.TTFFDisplayLocation.Height = height;
                        this.TTFFDisplayLocation.Width = width;
                        this.TTFFDisplayLocation.Left = left;
                        this.TTFFDisplayLocation.Top = top;
                        this.TTFFDisplayLocation.IsOpen = state;
                    }
                    break;

                case "frmTTFSView":
                    if (this.TTFSViewLocation != null)
                    {
                        this.TTFSViewLocation.Height = height;
                        this.TTFSViewLocation.Width = width;
                        this.TTFSViewLocation.Left = left;
                        this.TTFSViewLocation.Top = top;
                        this.TTFSViewLocation.IsOpen = state;
                    }
                    break;

                case "frmCommRadarMap":
                    if (this.SVsMapLocation != null)
                    {
                        this.SVsMapLocation.Height = height;
                        this.SVsMapLocation.Width = width;
                        this.SVsMapLocation.Left = left;
                        this.SVsMapLocation.Top = top;
                        this.SVsMapLocation.IsOpen = state;
                    }
                    break;

                case "frmSatelliteStats":
                    if (this.SatelliteStatsLocation != null)
                    {
                        this.SatelliteStatsLocation.Height = height;
                        this.SatelliteStatsLocation.Width = width;
                        this.SatelliteStatsLocation.Left = left;
                        this.SatelliteStatsLocation.Top = top;
                        this.SatelliteStatsLocation.IsOpen = state;
                    }
                    break;

                case "frmCommLocationMap":
                    if (this.LocationMapLocation != null)
                    {
                        this.LocationMapLocation.Height = height;
                        this.LocationMapLocation.Width = width;
                        this.LocationMapLocation.Left = left;
                        this.LocationMapLocation.Top = top;
                        this.LocationMapLocation.IsOpen = state;
                    }
                    break;

                case "frmCommDRStatus":
                    if (this.LocationDRStatusLocation != null)
                    {
                        this.LocationDRStatusLocation.Height = height;
                        this.LocationDRStatusLocation.Width = width;
                        this.LocationDRStatusLocation.Left = left;
                        this.LocationDRStatusLocation.Top = top;
                        this.LocationDRStatusLocation.IsOpen = state;
                    }
                    break;

                case "frmCommDRSensor":
                    if (this.LocationDRSensorLocation != null)
                    {
                        this.LocationDRSensorLocation.Height = height;
                        this.LocationDRSensorLocation.Width = width;
                        this.LocationDRSensorLocation.Left = left;
                        this.LocationDRSensorLocation.Top = top;
                        this.LocationDRSensorLocation.IsOpen = state;
                    }
                    break;

                case "frmCommMessageFilter":
                    if (this.MessageViewLocation != null)
                    {
                        this.MessageViewLocation.Height = height;
                        this.MessageViewLocation.Width = width;
                        this.MessageViewLocation.Left = left;
                        this.MessageViewLocation.Top = top;
                        this.MessageViewLocation.IsOpen = state;
                    }
                    break;

                case "frmCommInputMessage":
                    if (this.InputCommandLocation != null)
                    {
                        this.InputCommandLocation.Height = height;
                        this.InputCommandLocation.Width = width;
                        this.InputCommandLocation.Left = left;
                        this.InputCommandLocation.Top = top;
                        this.InputCommandLocation.IsOpen = state;
                    }
                    break;

                case "frmTransmitSerialMessage":
                    if (this.TransmitSerialMessageLocation != null)
                    {
                        this.TransmitSerialMessageLocation.Height = height;
                        this.TransmitSerialMessageLocation.Width = width;
                        this.TransmitSerialMessageLocation.Left = left;
                        this.TransmitSerialMessageLocation.Top = top;
                        this.TransmitSerialMessageLocation.IsOpen = state;
                    }
                    break;

                case "frmInterferenceReport":
                    if (this.InterferenceLocation != null)
                    {
                        this.InterferenceLocation.Height = height;
                        this.InterferenceLocation.Width = width;
                        this.InterferenceLocation.Left = left;
                        this.InterferenceLocation.Top = top;
                        this.InterferenceLocation.IsOpen = state;
                    }
                    break;

                case "frmCommSiRFawareV2":
                    if (this.SiRFawareLocation != null)
                    {
                        this.SiRFawareLocation.Height = height;
                        this.SiRFawareLocation.Width = width;
                        this.SiRFawareLocation.Left = left;
                        this.SiRFawareLocation.Top = top;
                        this.SiRFawareLocation.IsOpen = state;
                    }
                    break;

                case "frmMEMSView":
                    if (this.MEMSLocation != null)
                    {
                        this.MEMSLocation.Height = height;
                        this.MEMSLocation.Width = width;
                        this.MEMSLocation.Left = left;
                        this.MEMSLocation.Top = top;
                        this.MEMSLocation.IsOpen = state;
                    }
                    break;

                case "frmCommResponseView":
                    if (this.ResponseViewLocation != null)
                    {
                        this.ResponseViewLocation.Height = height;
                        this.ResponseViewLocation.Width = width;
                        this.ResponseViewLocation.Left = left;
                        this.ResponseViewLocation.Top = top;
                        this.ResponseViewLocation.IsOpen = state;
                    }
                    break;

                case "frmCommErrorView":
                    if (this.ErrorViewLocation != null)
                    {
                        this.ErrorViewLocation.Height = height;
                        this.ErrorViewLocation.Width = width;
                        this.ErrorViewLocation.Left = left;
                        this.ErrorViewLocation.Top = top;
                        this.ErrorViewLocation.IsOpen = state;
                    }
                    break;

                case "frmPeekPokeMem":
                    if (this.PeekPokeLocation != null)
                    {
                        this.PeekPokeLocation.Height = height;
                        this.PeekPokeLocation.Width = width;
                        this.PeekPokeLocation.Left = left;
                        this.PeekPokeLocation.Top = top;
                        this.PeekPokeLocation.IsOpen = state;
                    }
                    break;

                case "frmCommOpen":
                    this.comm.TTBPort = null;
                    this.comm.TTBPort = new CommWrapper();
                    this.comm.TTBPort.PortName = this.TTBPortProperties.PortName;
                    this.comm.TTBPort.BaudRate = this.TTBPortProperties.BaudRate;
                    this.comm.TTBPort.DataBits = this.TTBPortProperties.DataBits;
                    this.comm.TTBPort.Parity = this.TTBPortProperties.Parity;
                    this.comm.TTBPort.StopBits = this.TTBPortProperties.StopBits;
                    this.comm.TTBPort.Open();
                    if (this.TTBWinLocation != null)
                    {
                        this.TTBWinLocation.IsOpen = state;
                    }
                    break;

                case "frmCompassView":
                    if (this.CompassViewLocation != null)
                    {
                        this.CompassViewLocation.Height = height;
                        this.CompassViewLocation.Width = width;
                        this.CompassViewLocation.Left = left;
                        this.CompassViewLocation.Top = top;
                        this.CompassViewLocation.IsOpen = state;
                    }
                    break;

                case "frmCommSVTrajectory":
                    if (this.SVTrajViewLocation != null)
                    {
                        this.SVTrajViewLocation.Height = height;
                        this.SVTrajViewLocation.Width = width;
                        this.SVTrajViewLocation.Left = left;
                        this.SVTrajViewLocation.Top = top;
                        this.SVTrajViewLocation.IsOpen = state;
                    }
                    break;

                case "frmCommSVAvgCNo":
                    if (this.SVCNoViewLocation != null)
                    {
                        this.SVCNoViewLocation.Height = height;
                        this.SVCNoViewLocation.Width = width;
                        this.SVCNoViewLocation.Left = left;
                        this.SVCNoViewLocation.Top = top;
                        this.SVCNoViewLocation.IsOpen = state;
                    }
                    break;

                case "frmCommSVTrackedVsTime":
                    if (this.SVTrackedVsTimeViewLocation != null)
                    {
                        this.SVTrackedVsTimeViewLocation.Height = height;
                        this.SVTrackedVsTimeViewLocation.Width = width;
                        this.SVTrackedVsTimeViewLocation.Left = left;
                        this.SVTrackedVsTimeViewLocation.Top = top;
                        this.SVTrackedVsTimeViewLocation.IsOpen = state;
                    }
                    break;

                case "frmCommNavAccVsTime":
                    if (this.NavVsTimeLocation != null)
                    {
                        this.NavVsTimeLocation.Height = height;
                        this.NavVsTimeLocation.Width = width;
                        this.NavVsTimeLocation.Left = left;
                        this.NavVsTimeLocation.Top = top;
                        this.NavVsTimeLocation.IsOpen = state;
                    }
                    break;
            }
            if (this.UpdateMainWindow != null)
            {
                this.UpdateMainWindow(" ");
            }
        }

        private void updateTTFFWindow(object sender, DoWorkEventArgs ee)
        {
            if ((this._ttffDisplay != null) && this.TTFFDisplayLocation.IsOpen)
            {
                this._ttffDisplay.updateTTFFNow();
            }
        }

        public delegate void updateParentEventHandler(string titleString);
    }
}

