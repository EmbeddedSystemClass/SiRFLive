﻿namespace SiRFLive.MessageHandling
{
    using LogManagerClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.Reporting;
    using SiRFLive.Utilities;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Timers;

    public class ReceiverReset
    {
        public TestSetup _dutStationSetup;
        private readonly HelperFunctions _helperFunctions = new HelperFunctions();
        private int _lastLogResetNumber = -1;
        private object _lockAi3PosResp = new object();
        private readonly object _lockTTFF = new object();
        private LogManager _log_ttff = new LogManager();
        private bool _logThisReset = true;
        private bool _loopitInprogress;
        protected NavigationAnalysisData _navData;
        private uint _oneSecondCount = 1;
        private int _resetCnt;
        private readonly SiRFLiveEvent _resetDoneEvent = new SiRFLiveEvent();
        private bool _resetEarlyTerminate;
        private volatile bool _resetGotAi3Pos;
        private volatile bool _resetGotTTFF;
        protected GPSTimer _resetGPSTimer;
        private uint _resetInterval = 1;
        private uint _resetRandomRange;
        private uint _resetSaveInterval = 1;
        private readonly System.Timers.Timer _resetTimer = new System.Timers.Timer();
        private string _resetType = "FACTORY";
        protected CommunicationManager _rxComm;
        private string _rxSwVer = "SW Version: Not detected";
        private int _totalNumberOfResets = -1;
        public string DisplayResetType = string.Empty;
        public bool IsAidingPerformedOnFactory;
        public bool IsFirstTTFS;
        public bool IsProtocolSwitchedOnFactory = true;
        public object OneSecondLock = new object();
        public object ResetCountLock = new object();
        public resetInitParams ResetInitParams = new resetInitParams();
        public long StartResetTime = DateTime.Now.Ticks;

        public ReceiverReset()
        {
            this._resetTimer.Elapsed += new ElapsedEventHandler(this.OnResetTimedEvent);
            this._resetTimer.Enabled = false;
        }

        public void CloseTTFFLog()
        {
            if (this._log_ttff.IsFileOpen())
            {
                this._log_ttff.CloseFile();
            }
        }

        public string GetTestSetup()
        {
            if (this._dutStationSetup == null)
            {
                return string.Empty;
            }
            this._dutStationSetup.SWVersion = this._rxSwVer.Replace(':', '=');
            StringBuilder builder = new StringBuilder();
            builder.Append(this._dutStationSetup.SWVersion);
            builder.Append("\r\n");
            builder.Append(string.Format("DUT ID = {0}\r\n", this._dutStationSetup.RxSN));
            builder.Append(string.Format("Tool Name = {0}\r\n", clsGlobal.SiRFLiveVersion));
            builder.Append(string.Format("Test ID = {0}\r\n", this._dutStationSetup.TestID));
            builder.Append(string.Format("Test Name = {0}\r\n", this._dutStationSetup.TestName));
            builder.Append(string.Format("Test Description = {0}\r\n", this._dutStationSetup.TestDescription));
            builder.Append(string.Format("Test Data Classification = {0}\r\n", this._dutStationSetup.DataClass));
            builder.Append(string.Format("Test Group = {0}\r\n", this._dutStationSetup.TestGroup));
            builder.Append(string.Format("Test Run = {0}\r\n", this._dutStationSetup.TestRun));
            builder.Append(string.Format("Test Start Time = {0}\r\n", this._dutStationSetup.StartTime));
            builder.Append(string.Format("Test End Time = {0}\r\n", this._dutStationSetup.EndTime));
            builder.Append(string.Format("Test Operator = {0}\r\n", this._dutStationSetup.TestOperator));
            builder.Append(string.Format("Test Station = {0}\r\n", Environment.MachineName));
            builder.Append(string.Format("DUT Platform Type = {0}\r\n", this._dutStationSetup.RXPlatformType));
            builder.Append(string.Format("DUT Package Type = {0}\r\n", this._dutStationSetup.RXPackageType));
            builder.Append(string.Format("DUT Product Family = {0}\r\n", this._dutStationSetup.RXProductType));
            builder.Append(string.Format("DUT Comm Interface = {0}\r\n", this._dutStationSetup.RXCommIntf));
            builder.Append(string.Format("DUT Revision = {0}\r\n", this._dutStationSetup.RXRev));
            builder.Append(string.Format("DUT Voltage|V = {0}\r\n", this._dutStationSetup.RXVoltage));
            builder.Append(string.Format("DUT Power Supply Config = {0}\r\n", (this._dutStationSetup.RXPwrMode == 0) ? "Switcher" : "LDO"));
            if (this._dutStationSetup.RXRefFreq == 0)
            {
                builder.Append(string.Format("DUT Ref Freq|Hz = {0}\r\n", "N/A"));
            }
            else
            {
                builder.Append(string.Format("DUT Ref Freq|Hz = {0}\r\n", this._dutStationSetup.RXRefFreq));
            }
            builder.Append(string.Format("DUT Ref Freq Source = {0}\r\n", this._dutStationSetup.RXRefFreqSrc));
            builder.Append(string.Format("DUT LNA Config = {0}\r\n", (this._dutStationSetup.RXLNAType == 0) ? "Internal" : "External"));
            builder.Append(string.Format("DUT Manufacturer Name = {0}\r\n", this._dutStationSetup.RXManufacture));
            builder.Append(string.Format("Antenna Source = {0}\r\n", this._dutStationSetup.RXAntType));
            builder.Append(string.Format("Temperature|{0} = {1}\r\n", this._dutStationSetup.RXTempUnit, HelperFunctions.FormatDigitsToString(this._dutStationSetup.RXTempVal, 2)));
            if (this._dutStationSetup.SignalLevel == 0.0)
            {
                builder.Append(string.Format("Signal Level|{0} = {1}\r\n", this._dutStationSetup.SignalType, "N/A"));
            }
            else
            {
                builder.Append(string.Format("Signal Level|{0} = {1}\r\n", this._dutStationSetup.SignalType, HelperFunctions.FormatDigitsToString(this._dutStationSetup.SignalLevel, 2)));
            }
            builder.Append(string.Format("Margin Level|dB = {0}\r\n", HelperFunctions.FormatDigitsToString(this._dutStationSetup.SignalMargin, 2)));
            builder.Append(string.Format("TTFF 95% Limit|s = {0}\r\n", HelperFunctions.FormatDigitsToString(this._dutStationSetup.TTFFLimit, 2)));
            builder.Append(string.Format("2D Position Error 95% Limit|m = {0}\r\n", HelperFunctions.FormatDigitsToString(this._dutStationSetup.HrzErrorLimit, 2)));
            builder.Append(string.Format("TTFF Timeout = {0}\r\n", HelperFunctions.FormatDigitsToString(this._dutStationSetup.TTFFTimeout, 2)));
            builder.Append(string.Format(clsGlobal.MyCulture, "Antenna Attenuation: {0}\r\n", new object[] { HelperFunctions.FormatDigitsToString(this._dutStationSetup.Atten, 2) }));
            if (this._rxComm.AutoReplyCtrl.AutoReplyParams.AutoReply)
            {
                string str = utils_AutoReply.gettext_PosReq(this._rxComm);
                if (str != null)
                {
                    string[] strArray = str.Split(new char[] { '\n' });
                    if (strArray.Length > 2)
                    {
                        for (int i = 1; i < strArray.Length; i++)
                        {
                            builder.Append(strArray[i].Replace("\t", ""));
                            builder.Append("\r\n");
                        }
                    }
                }
            }
            builder.Append("End Summary\r\n");
            builder.Append("Time, Reset#, Reset Type, TTFF Report,TTFF SiRFLive, TTFF Reset, TTFF Aided, TTFF First Nav, 2D Accuracy, Vert. Accuracy, Meas Lat, Meas Lon, Meas Alt, Ref Lat, Ref Lon, Ref Alt, Time Error, Time Unc, Freq Error, Freq Unc, CNo" + new string(',', 11) + ", Avg CNo, #SV in Fix, Flag, TOW");
            return builder.ToString();
        }

        public string LogTTFFCsv()
        {
            string msg = string.Empty;
            if (this._resetCnt != 0)
            {
                if (!this._resetGotTTFF)
                {
                    if (this._resetGotAi3Pos)
                    {
                        this._navData.TTFFReset = 0.0;
                        this._navData.TTFFAided = 0.0;
                        this._navData.TTFFFirstNav = 0.0;
                        this._navData.Valid = true;
                    }
                    else
                    {
                        this._navData.TTFFReset = this._resetInterval;
                        this._navData.TTFFAided = this._resetInterval;
                        this._navData.TTFFFirstNav = this._resetInterval;
                        this._navData.TTFFSiRFLive = this._resetInterval;
                    }
                    this._navData.TTFFLogTime = HelperFunctions.GetTimeStampInString();
                    this._navData.TimeErr = "-9999";
                    this._navData.TimeUncer = "-9999";
                    this._navData.FreqErr = "-9999";
                    this._navData.FreqUncer = "-9999";
                }
                else
                {
                    this._navData.TTFFReport = this._navData.TTFFAided;
                }
                msg = string.Format(clsGlobal.MyCulture, "{0},{1},{2},{3:F1},{4:F1},{5:F1},{6:F1}, {7:F1}", new object[] { this._navData.TTFFLogTime, this._resetCnt, this.DisplayResetType, this._navData.TTFFReport, this._navData.TTFFSiRFLive, this._navData.TTFFReset, this._navData.TTFFAided, this._navData.TTFFFirstNav });
                if (!this._resetGotAi3Pos)
                {
                    this._navData.FirstFix2DPositionError = -9999.0;
                    this._navData.FirstFixVerticalPositionError = -9999.0;
                    this._navData.FirstFixMeasLat = -9999.0;
                    this._navData.FirstFixMeasLon = -9999.0;
                    this._navData.FirstFixMeasAlt = -9999.0;
                    this._rxComm.LastAidingSessionReportedClockDrift = 0.0;
                }
                string str2 = string.Format(clsGlobal.MyCulture, ",{0:F2},{1:F2},{2:F6},{3:F6},{4:F6},{5:F6},{6:F6},{7:F6},{8},{9},{10},{11},{12},{13},{14},{15}\r\n", new object[] { this._navData.FirstFix2DPositionError, this._navData.FirstFixVerticalPositionError, this._navData.FirstFixMeasLat, this._navData.FirstFixMeasLon, this._navData.FirstFixMeasAlt, this._navData.RefLat, this._navData.RefLon, this._navData.RefAlt, this._navData.TimeErr, this._navData.TimeUncer, this._navData.FreqErr, this._navData.FreqUncer, this._navData.AvgCNo, this._navData.NumSVsInFix, this._navData.AidingFlags, this._navData.FirstFixTOW });
                msg = msg + str2;
                lock (this._lockTTFF)
                {
                    if (((this._resetCnt != this._lastLogResetNumber) && this.LogThisReset) && this._log_ttff.IsFileOpen())
                    {
                        this._log_ttff.Write(msg);
                        this._lastLogResetNumber = this._resetCnt;
                        this.LogThisReset = false;
                    }
                }
            }
            return msg;
        }

        public void OnResetTimedEvent(object source, ElapsedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
            if (this._oneSecondCount < this._resetInterval)
            {
                string title = string.Format(clsGlobal.MyCulture, "Reset {0}/{1} ({2}/{3}) -- {4}", new object[] { this._resetCnt, this._totalNumberOfResets, this._oneSecondCount, this._resetInterval, this._rxComm.WindowTitle });
                this._rxComm.UpdateWindowTitleBar(title);
                this.OneSecondCount++;
            }
            else
            {
                this.OneSecondCount = 1;
                if (!this._resetGotTTFF)
                {
                    this.LogTTFFCsv();
                }
                if (this._resetCnt > this._totalNumberOfResets)
                {
                    if (this._totalNumberOfResets > 0)
                    {
                        this._resetTimer.Stop();
                        this._resetTimer.Close();
                        this.CloseTTFFLog();
                        this._rxComm.Log.CloseFile();
                        this._loopitInprogress = false;
                        string str2 = string.Format(clsGlobal.MyCulture, "Done Loopit -- {0}", new object[] { this._rxComm.WindowTitle });
                        this._rxComm.UpdateWindowTitleBar(str2);
                    }
                    else
                    {
                        this.Reset(this._resetType);
                        string msg = string.Format(clsGlobal.MyCulture, "####### Reset Number {0} #######", new object[] { this._resetCnt });
                        this._rxComm.WriteApp(msg);
                        if (this._rxComm != null)
                        {
                            string str4 = string.Format(clsGlobal.MyCulture, "Reset {0}/{1} ({2}/{3}) -- {4}", new object[] { this._resetCnt, this._totalNumberOfResets, this._oneSecondCount, this._resetInterval, this._rxComm.WindowTitle });
                            this._rxComm.UpdateWindowTitleBar(str4);
                        }
                    }
                }
                else if (this._resetCnt < this._totalNumberOfResets)
                {
                    this.Reset(this._resetType);
                    string str5 = string.Format(clsGlobal.MyCulture, "####### Reset Number {0} #######", new object[] { this._resetCnt });
                    this._rxComm.WriteApp(str5);
                    if (this._rxComm != null)
                    {
                        string str6 = string.Format(clsGlobal.MyCulture, "Reset {0}/{1} -- {2}", new object[] { this._resetCnt, this._totalNumberOfResets, this._rxComm.WindowTitle });
                        this._rxComm.UpdateWindowTitleBar(str6);
                    }
                    if ((this._resetEarlyTerminate && !clsGlobal.WaitForEph) && (this._resetSaveInterval != this._resetInterval))
                    {
                        this._resetInterval = this._resetSaveInterval;
                    }
                }
                else
                {
                    this._resetCnt++;
                    this.OneSecondCount = this._resetInterval;
                }
            }
        }

        public bool OpenTTFFLog(string filename)
        {
            this._resetGotTTFF = false;
            this._resetGotAi3Pos = false;
            this._resetCnt = 0;
            if (this._log_ttff.IsFileOpen())
            {
                return true;
            }
            try
            {
                if (!this._log_ttff.OpenFile(filename))
                {
                    this._rxComm.WriteApp(string.Format(clsGlobal.MyCulture, "Error open {0}", new object[] { filename }));
                    return false;
                }
                this._log_ttff.WriteLine(this.GetTestSetup());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual void Reset(string resetType)
        {
            this.ResetTTFFAvailable = false;
            this.ResetPositionAvailable = false;
            this.LogThisReset = true;
            this.ResetCount++;
        }

        public void ResetDataInit()
        {
            this._rxComm.AutoReplyCtrl.PosReqAck = false;
            this._navData.TTFFReport = this._resetInterval;
            this.ResetTTFFAvailable = false;
            this.ResetPositionAvailable = false;
            this.ResetTTFFAvailable = false;
            this.ResetPositionAvailable = false;
            this.LogThisReset = true;
            this.ResetCount++;
        }

        public void ResetTimerClose()
        {
            this.ResetTimerStop(true);
            this._resetTimer.Close();
        }

        public void ResetTimerStart(bool clearCount)
        {
            this.OneSecondCount = 1;
            if (clearCount)
            {
                this._resetCnt = 0;
                this._lastLogResetNumber = -1;
                this._resetSaveInterval = this._resetInterval;
            }
            this._resetTimer.AutoReset = true;
            if (this._resetRandomRange > 0)
            {
                Random random = new Random();
                this._resetInterval += (uint) random.Next(0, (int) this._resetRandomRange);
            }
            this._resetTimer.Interval = 1000.0;
            if (this._resetCnt == 0)
            {
                this.Reset(this._resetType);
                string msg = string.Format(clsGlobal.MyCulture, "####### Reset Number {0} ({1}/{2}) #######", new object[] { this._resetCnt, this._oneSecondCount, this._resetInterval });
                this._rxComm.WriteApp(msg);
            }
            this._resetTimer.Start();
        }

        public void ResetTimerStop(bool clearCount)
        {
            this._resetTimer.Stop();
            if (clearCount)
            {
                this._resetCnt = 0;
                this._lastLogResetNumber = -1;
            }
        }

        public TestSetup DutStationSetup
        {
            get
            {
                return this._dutStationSetup;
            }
            set
            {
                this._dutStationSetup = value;
            }
        }

        public bool LogThisReset
        {
            get
            {
                return this._logThisReset;
            }
            set
            {
                this._logThisReset = value;
            }
        }

        public bool LoopitInprogress
        {
            get
            {
                return this._loopitInprogress;
            }
            set
            {
                this._loopitInprogress = value;
            }
        }

        public uint OneSecondCount
        {
            get
            {
                return this._oneSecondCount;
            }
            set
            {
                lock (this.OneSecondLock)
                {
                    this._oneSecondCount = value;
                }
            }
        }

        public CommunicationManager ResetComm
        {
            get
            {
                return this._rxComm;
            }
            set
            {
                this._rxComm = value;
            }
        }

        public int ResetCount
        {
            get
            {
                return this._resetCnt;
            }
            set
            {
                lock (this.ResetCountLock)
                {
                    this._resetCnt = value;
                }
            }
        }

        public SiRFLiveEvent ResetDoneEvent
        {
            get
            {
                return this._resetDoneEvent;
            }
        }

        public bool ResetEarlyTerminate
        {
            get
            {
                return this._resetEarlyTerminate;
            }
            set
            {
                this._resetEarlyTerminate = value;
            }
        }

        public GPSTimer ResetGPSTimer
        {
            get
            {
                return this._resetGPSTimer;
            }
            set
            {
                this._resetGPSTimer = value;
            }
        }

        public uint ResetInterval
        {
            get
            {
                return this._resetInterval;
            }
            set
            {
                this._resetInterval = value;
            }
        }

        public LogManager ResetLogTTFF
        {
            get
            {
                return this._log_ttff;
            }
            set
            {
                this._log_ttff = value;
            }
        }

        public NavigationAnalysisData ResetNavData
        {
            get
            {
                return this._navData;
            }
            set
            {
                this._navData = value;
            }
        }

        public bool ResetPositionAvailable
        {
            get
            {
                return this._resetGotAi3Pos;
            }
            set
            {
                this._resetGotAi3Pos = value;
            }
        }

        public uint ResetRandomRange
        {
            get
            {
                return this._resetRandomRange;
            }
            set
            {
                this._resetRandomRange = value;
            }
        }

        public string ResetRxSwVersion
        {
            get
            {
                return this._rxSwVer;
            }
            set
            {
                this._rxSwVer = value;
            }
        }

        public bool ResetTTFFAvailable
        {
            get
            {
                return this._resetGotTTFF;
            }
            set
            {
                this._resetGotTTFF = value;
            }
        }

        public string ResetType
        {
            get
            {
                return this._resetType;
            }
            set
            {
                this._resetType = value;
            }
        }

        public int TotalNumberOfResets
        {
            get
            {
                return this._totalNumberOfResets;
            }
            set
            {
                this._totalNumberOfResets = value;
            }
        }
    }
}

