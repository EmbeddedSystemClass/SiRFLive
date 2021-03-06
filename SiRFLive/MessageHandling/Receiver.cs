﻿namespace SiRFLive.MessageHandling
{
    using LogManagerClassLibrary;
    using SiRFLive.Analysis;
    using SiRFLive.Communication;
    using SiRFLive.Configuration;
    using SiRFLive.General;
    using SiRFLive.Reporting;
    using SiRFLive.TruthData;
    using SiRFLive.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;

    public class Receiver : IDisposable
    {
        protected string _aidingProtocol;
        protected string _aidingProtocolVersion;
        protected uint _clkDrift;
        protected CommunicationManager _commWindow;
        private string _controlChannelProtocolFile;
        protected string _controlChannelVersion;
        protected string _defaultFreqOffset;
        protected int _ExitTunnelAcqTime;
        protected GPSTimer _gpsTimer;
        protected HelperFunctions _helperFunctions;
        protected int _LostLockTimeAllSVs;
        protected int _LostLockTimeFirstSV;
        protected string _messageProtocol;
        protected ReceiverReset _resetCtrl;
        protected Hashtable _rxLogFileHash;
        protected NavigationAnalysisData _rxNavData;
        protected TestSetup _rxTestSetup;
        protected TunnelTimestampFile _Tunnel_Truth_Data;
        protected byte _utcOffset;
        public bool AllowSessOpen;
        private bool isDisposed;
        protected MsgFactory m_Protocols;
        protected static int TIME_NOT_SET = -1;
        public TrackingAnalysisData Tracking_Analysis_Data;
        public LogManager TunnelAnalysisLog;

        public Receiver()
        {
            this._LostLockTimeFirstSV = TIME_NOT_SET;
            this._LostLockTimeAllSVs = TIME_NOT_SET;
            this._ExitTunnelAcqTime = TIME_NOT_SET;
            this.AllowSessOpen = true;
            this._helperFunctions = new HelperFunctions();
            this.m_Protocols = new MsgFactory(ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Protocols\Protocols.xml");
            this._controlChannelVersion = string.Empty;
            this._aidingProtocolVersion = string.Empty;
            this._messageProtocol = string.Empty;
            this._aidingProtocol = string.Empty;
            this._gpsTimer = new GPSTimer();
            this._defaultFreqOffset = "98254";
            this._rxLogFileHash = new Hashtable();
            this.Tracking_Analysis_Data = new TrackingAnalysisData();
            this.TunnelAnalysisLog = new LogManager();
            this._utcOffset = 15;
        }

        public Receiver(string str)
        {
            this._LostLockTimeFirstSV = TIME_NOT_SET;
            this._LostLockTimeAllSVs = TIME_NOT_SET;
            this._ExitTunnelAcqTime = TIME_NOT_SET;
            this.AllowSessOpen = true;
            this._helperFunctions = new HelperFunctions();
            this.m_Protocols = new MsgFactory(ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Protocols\Protocols.xml");
            this._controlChannelVersion = string.Empty;
            this._aidingProtocolVersion = string.Empty;
            this._messageProtocol = string.Empty;
            this._aidingProtocol = string.Empty;
            this._gpsTimer = new GPSTimer();
            this._defaultFreqOffset = "98254";
            this._rxLogFileHash = new Hashtable();
            this.Tracking_Analysis_Data = new TrackingAnalysisData();
            this.TunnelAnalysisLog = new LogManager();
            this._utcOffset = 15;
            this._rxNavData = new NavigationAnalysisData(ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Protocols\ReferenceLocation.xml");
        }

        public void AutoDetectBaudAfterFacRst4E(ref CommunicationManager targetComm)
        {
            if (CommunicationManager.ValidateCommManager(targetComm))
            {
                targetComm.AutoDetectProtocolAndBaudDone = false;
                targetComm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Text;
                targetComm.TxCurrentTransmissionType = CommunicationManager.TransmissionType.Text;
                targetComm.BaudRate = "4800";
                targetComm.Flag_didFacRst_atSSB = true;
                uint baud = uint.Parse(targetComm.BaudRate);
                targetComm.comPort.UpdateBaudSettings(baud);
                targetComm.portDataInit();
                targetComm.AutoDetectProtocolAndBaudDone = true;
                string msg = NMEAReceiver.NMEA_AddCheckSum(string.Format("$PSRF100,0,{0},8,1,0", targetComm.ToSwitchBaud));
                msg = NMEAReceiver.NMEA_AddCheckSum("$PSRF105,1") + "\r\n" + msg;
                targetComm.WriteData(msg);
                Thread.Sleep(0x3e8);
                targetComm.RxType = CommunicationManager.ReceiverType.SLC;
                targetComm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GPS;
                targetComm.TxCurrentTransmissionType = CommunicationManager.TransmissionType.GP2;
                targetComm.BaudRate = targetComm.ToSwitchBaud;
                int num2 = 0;
                uint num3 = uint.Parse(targetComm.BaudRate);
                while (num2++ < 5)
                {
                    if (targetComm.comPort.UpdateBaudSettings(num3))
                    {
                        break;
                    }
                    Thread.Sleep(1);
                }
                targetComm.portDataInit();
                targetComm.AutoDetectProtocolAndBaudDone = true;
            }
        }

        public virtual void CloseChannel()
        {
        }

        public virtual void CloseSession(byte sessionCloseReqInfo)
        {
        }

        public LogManager CreateLog(string logName)
        {
            if (!this._rxLogFileHash.ContainsKey(logName))
            {
                LogManager manager = new LogManager();
                this._rxLogFileHash.Add(logName, manager);
                return manager;
            }
            return (LogManager) this._rxLogFileHash[logName];
        }

        public string DecodeApproxPositionAccuracy(byte posAcc)
        {
            switch (posAcc)
            {
                case 0:
                    return "Acc<24";

                case 1:
                    return "24.0<=Acc< 25.5";

                case 0xfe:
                    return "1425408<=Acc<1474560";

                case 0xff:
                    return "Acc>=1474560";
            }
            int num = posAcc >> 4;
            int num2 = posAcc & 15;
            return string.Format(clsGlobal.MyCulture, "{0:F3}", new object[] { (24.0 * (1.0 + (((double) num2) / 16.0))) * (((int) 1) << num) });
        }

        public virtual string[] DecodeClockStatus(string message)
        {
            return new string[0];
        }

        public static string DecodeCoarseTTaccuracy(byte TimeUncert)
        {
            switch (TimeUncert)
            {
                case 0:
                    return "Acc<1.0";

                case 1:
                    return "1.0<Acc<1.0624";

                case 0xfe:
                    return "59391<Acc<61440.0";

                case 0xff:
                    return "Acc>61439";
            }
            int num = TimeUncert >> 4;
            int num2 = TimeUncert & 15;
            return string.Format(clsGlobal.MyCulture, "{0:F3}", new object[] { (int) ((1.0 * (1.0 + (((double) num2) / 16.0))) * (((int) 1) << num)) });
        }

        public string DecodeFTR_Acc(byte FreqAcc)
        {
            byte num = (byte) (FreqAcc >> 4);
            byte num2 = (byte) (FreqAcc & 15);
            if (FreqAcc == 0)
            {
                return "Acc<0.00390625";
            }
            if (FreqAcc == 0xfe)
            {
                return "232<Acc<240";
            }
            if (FreqAcc == 0xff)
            {
                return "Acc>=240";
            }
            double num3 = (float) ((0.00390625 * (((int) 1) << num)) * (1.0 + (((double) num2) / 16.0)));
            return string.Format(clsGlobal.MyCulture, "{0:F3}", new object[] { num3 });
        }

        public virtual string DecodeGeodeticNavigationDataToCSV(string message)
        {
            return string.Empty;
        }

        public virtual SiRFawareStatsParams DecodeMPMStats(Hashtable SiRFAwareScanResHash)
        {
            if (SiRFAwareScanResHash == null)
            {
                return null;
            }
            SiRFawareStatsParams @params = new SiRFawareStatsParams();
            if (SiRFAwareScanResHash.Contains("Message Sub ID"))
            {
                @params.UpdateType = byte.Parse((string) SiRFAwareScanResHash["Message Sub ID"]);
            }
            if (SiRFAwareScanResHash.Contains("TimeInFullPwrSec"))
            {
                @params.TimeSpentInFullPowerSec = ushort.Parse((string) SiRFAwareScanResHash["TimeInFullPwrSec"]);
                @params.isValid_TimeSpentInFullPowerSec = true;
            }
            if (SiRFAwareScanResHash.Contains("RTC_WAKEUP_UNC"))
            {
                @params.RTCWakeupUncUs = ushort.Parse((string) SiRFAwareScanResHash["RTC_WAKEUP_UNC"]);
                @params.isValid_RTCWakeupUncUs = true;
            }
            if (SiRFAwareScanResHash.Contains("TOTAL_RTC_CORR"))
            {
                @params.RTCCorrectionPerform = ushort.Parse((string) SiRFAwareScanResHash["TOTAL_RTC_CORR"]);
                @params.isValid_RTCCorrectionPerform = true;
            }
            if (SiRFAwareScanResHash.Contains("TOW_ms"))
            {
                @params.TOW = uint.Parse((string) SiRFAwareScanResHash["TOW_ms"]);
                @params.isValid_TOW = true;
            }
            if (SiRFAwareScanResHash.Contains("Unused_Token"))
            {
                @params.UnusedTokenLeft = byte.Parse((string) SiRFAwareScanResHash["Unused_Token"]);
                @params.isValid_UnusedTokenLeft = true;
            }
            if (SiRFAwareScanResHash.Contains("TemperatureT"))
            {
                @params.TempRecordT = byte.Parse((string) SiRFAwareScanResHash["TemperatureT"]);
                @params.TempRecordC = (@params.TempRecordT * 0.546875) - 40.0;
                @params.isValid_TempRecord = true;
            }
            if (SiRFAwareScanResHash.Contains("SVsBeforeCollection"))
            {
                @params.SVBeforeEphCollection = uint.Parse((string) SiRFAwareScanResHash["SVsBeforeCollection"]);
                @params.isValid_SVBeforeEphCollection = true;
            }
            if (SiRFAwareScanResHash.Contains("SVsAfterCollection"))
            {
                @params.SVAfterEphCollection = uint.Parse((string) SiRFAwareScanResHash["SVsAfterCollection"]);
                @params.isValid_SVAfterEphCollection = true;
            }
            if (SiRFAwareScanResHash.Contains("IsNav"))
            {
                @params.IsNav = byte.Parse((string) SiRFAwareScanResHash["IsNav"]);
                @params.isValid_IsNav = true;
            }
            if (SiRFAwareScanResHash.Contains("Alm_ID"))
            {
                @params.AlmID = byte.Parse((string) SiRFAwareScanResHash["Alm_ID"]);
                @params.isValid_AlmID = true;
            }
            if (SiRFAwareScanResHash.Contains("IsCollectionSuccessful"))
            {
                @params.IsSuccessAlmCollection = byte.Parse((string) SiRFAwareScanResHash["IsCollectionSuccessful"]);
                @params.isValid_IsSuccessAlmCollection = true;
            }
            if (SiRFAwareScanResHash.Contains("BE_SVS"))
            {
                @params.TotalSVMeasureWithBE = byte.Parse((string) SiRFAwareScanResHash["BE_SVS"]);
                @params.isValid_TotalSVMeasureWithBE = true;
            }
            if (SiRFAwareScanResHash.Contains("EE_SVS"))
            {
                @params.TotalSVMeasureWithEE = byte.Parse((string) SiRFAwareScanResHash["EE_SVS"]);
                @params.isValid_TotalSVMeasureWithEE = true;
            }
            if (SiRFAwareScanResHash.Contains("ALM_SVS"))
            {
                @params.TotalSVMeasureWithAlm = byte.Parse((string) SiRFAwareScanResHash["ALM_SVS"]);
                @params.isValid_TotalSVMeasureWithAlm = true;
            }
            if (SiRFAwareScanResHash.Contains("UNAV_TIME_CORR"))
            {
                @params.uNavTimeCorrection = short.Parse((string) SiRFAwareScanResHash["UNAV_TIME_CORR"]);
                @params.isValid_uNavTimeCorrection = true;
            }
            if (SiRFAwareScanResHash.Contains("MEAN_CODE_PHASE_CORR"))
            {
                @params.MeanCodePhaseCorrection = int.Parse((string) SiRFAwareScanResHash["MEAN_CODE_PHASE_CORR"]);
                @params.isValid_MeanCodePhaseCorrection = true;
            }
            if (SiRFAwareScanResHash.Contains("STD_PR"))
            {
                @params.StdPseudoRanges = int.Parse((string) SiRFAwareScanResHash["STD_PR"]);
                @params.isValid_StdPseudoRanges = true;
            }
            if (SiRFAwareScanResHash.Contains("MEAN_DOPP_RES"))
            {
                @params.MeanDopplerResidual = int.Parse((string) SiRFAwareScanResHash["MEAN_DOPP_RES"]);
                @params.isValid_MeanDopplerResidual = true;
            }
            if (SiRFAwareScanResHash.Contains("STD_DR"))
            {
                @params.StdDeltaRanges = int.Parse((string) SiRFAwareScanResHash["STD_DR"]);
                @params.isValid_StdDeltaRanges = true;
            }
            if (SiRFAwareScanResHash.Contains("IsBitSync"))
            {
                @params.IsBitSynch = byte.Parse((string) SiRFAwareScanResHash["IsBitSync"]);
                @params.isValid_IsBitSynch = true;
            }
            if (SiRFAwareScanResHash.Contains("IsFrmSync"))
            {
                @params.IsFrameSynch = byte.Parse((string) SiRFAwareScanResHash["IsFrmSync"]);
                @params.isValid_IsFrameSynch = true;
            }
            if (SiRFAwareScanResHash.Contains("TotalTimeCorr"))
            {
                @params.TotalTimeCorrection = int.Parse((string) SiRFAwareScanResHash["TotalTimeCorr"]);
                @params.isValid_TotalTimeCorrection = true;
            }
            return @params;
        }

        public virtual string DecodeNavLibMeasurementDataToCSV(string message)
        {
            return string.Empty;
        }

        public virtual int DecodePostionFromSSB(string logtime, string message)
        {
            return 0;
        }

        public string DecodePreciseTTaccuracy(byte TimeUncert)
        {
            switch (TimeUncert)
            {
                case 0:
                    return "Acc<0.000125";

                case 1:
                    return "0.000125<Acc<0.0001328125";

                case 0xfe:
                    return "7.424<Acc<7.680";

                case 0xff:
                    return "Acc>7.679";
            }
            int num = TimeUncert >> 4;
            int num2 = TimeUncert & 15;
            return string.Format(clsGlobal.MyCulture, "{0:F3}", new object[] { ((0.125 * (1.0 + (((double) num2) / 16.0))) * (((int) 1) << num)) * 0.001 });
        }

        public virtual string DecodeRawMeasuredNavigationDataToCSV(string message)
        {
            return string.Empty;
        }

        public virtual string DecodeRawMessageToCSV(string protocol, string message)
        {
            return string.Empty;
        }

        public virtual Hashtable DecodeRawMessageToHash(string protocol, string message)
        {
            return new Hashtable();
        }

        public double DecodeStdErrICD(byte icdCrap)
        {
            switch (icdCrap)
            {
                case 0:
                    return 0.125;

                case 1:
                    return 0.1328125;

                case 0xfe:
                    return 7680.0;

                case 0xff:
                    return 7681.0;
            }
            int num = icdCrap >> 4;
            int num2 = icdCrap & 15;
            return ((0.125 * (1.0 + (((double) num2) / 16.0))) * (((int) 1) << num));
        }

        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed && disposing)
            {
                this.m_Protocols.Dispose();
                this._rxNavData = null;
            }
            this.isDisposed = true;
        }

        ~Receiver()
        {
            this.Dispose(false);
        }

        public virtual string FormatDRSensorDataString(string csvString)
        {
            return string.Empty;
        }

        public virtual string FormatDRStateString(string csvString)
        {
            return string.Empty;
        }

        public virtual string FormatDRStatusString(string csvString)
        {
            return string.Empty;
        }

        public string FormatFreqTransferResponse(string hexString)
        {
            string str = string.Empty;
            try
            {
                string str2 = hexString.Replace(" ", "");
                int startIndex = 0;
                short num2 = short.Parse(str2.Substring(startIndex, 4), NumberStyles.HexNumber);
                startIndex += 4;
                byte freqAcc = (byte) uint.Parse(str2.Substring(startIndex, 2), NumberStyles.HexNumber);
                startIndex += 2;
                uint num4 = uint.Parse(str2.Substring(startIndex, 8), NumberStyles.HexNumber);
                startIndex += 8;
                int num5 = int.Parse(str2.Substring(startIndex, 2), NumberStyles.HexNumber);
                string str3 = string.Empty;
                string str4 = string.Empty;
                string str5 = string.Empty;
                string str6 = string.Empty;
                double num6 = 0.0;
                if ((num5 & 1) == 1)
                {
                    str3 = "SLC Clk";
                }
                else
                {
                    str3 = "Ref Clk";
                }
                if ((num5 & 2) == 2)
                {
                    str4 = "Ref clk off";
                }
                else
                {
                    str4 = "Ref clk on";
                }
                if ((num5 & 4) == 4)
                {
                    str5 = "Request Ref clk off";
                }
                else
                {
                    str5 = "Request Ref clk on";
                }
                if ((num5 & 8) == 8)
                {
                    startIndex += 2;
                    num6 = ((double) ulong.Parse(str2.Substring(startIndex, 10), NumberStyles.HexNumber)) / 1000.0;
                    str6 = "Nominal Freq Present";
                }
                else
                {
                    str6 = "Nominal Freq Absent";
                }
                return string.Format(clsGlobal.MyCulture, "Freq Transfer Response {0}: Freq Offset: {1}-{2}={3} hz  Freq Accuracy: {4} ppm  TimeTag: 0x{5:X8}  ClockRef: {6} {7} {8} {9}: Nominal plus skew: {10:F3} (Hz)", new object[] { str3, clsGlobal.DEFAULT_RF_FREQ[this._commWindow.AutoReplyCtrl.FreqTransferCtrl.DefaultFreqIndex], clsGlobal.DEFAULT_RF_FREQ[this._commWindow.AutoReplyCtrl.FreqTransferCtrl.DefaultFreqIndex] - num2, num2, this.DecodeFTR_Acc(freqAcc), num4, str3, str4, str5, str6, num6 });
            }
            catch
            {
                return str;
            }
        }

        public virtual string FormatInputCarBusDataString(string csvString)
        {
            return string.Empty;
        }

        public virtual string FormatMeasurementResponse(string csvString)
        {
            return string.Empty;
        }

        public virtual string[] FormatMPMStatsMessage(SiRFawareStatsParams DisplayStatsData)
        {
            string[] strArray = new string[2];
            List<string> list = new List<string>();
            if (DisplayStatsData.isValid_TimeSpentInFullPowerSec)
            {
                list.Add(DisplayStatsData.TimeSpentInFullPowerSec.ToString());
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_RTCWakeupUncUs)
            {
                list.Add(DisplayStatsData.RTCWakeupUncUs.ToString());
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_RTCCorrectionPerform)
            {
                list.Add(DisplayStatsData.RTCCorrectionPerform.ToString());
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_TOW)
            {
                list.Add(DisplayStatsData.TOW.ToString());
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_UnusedTokenLeft)
            {
                list.Add(DisplayStatsData.UnusedTokenLeft.ToString());
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_TempRecord)
            {
                list.Add(DisplayStatsData.TempRecordC.ToString("F1"));
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_TempRecord)
            {
                list.Add(DisplayStatsData.TempRecordT.ToString());
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_AlmID)
            {
                list.Add(DisplayStatsData.AlmID.ToString());
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_TotalSVMeasureWithBE)
            {
                list.Add(DisplayStatsData.TotalSVMeasureWithBE.ToString());
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_TotalSVMeasureWithEE)
            {
                list.Add(DisplayStatsData.TotalSVMeasureWithEE.ToString());
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_TotalSVMeasureWithAlm)
            {
                list.Add(DisplayStatsData.TotalSVMeasureWithAlm.ToString());
            }
            else
            {
                list.Add("N/A");
            }
            int num = 0;
            strArray[0] = string.Format("\n--- Time in Full Power(s):\t\t\t{0}\n--- RTC Initial Time Uncertainty(us):\t\t{1}\n--- Total RTC Correction:\t\t\t{2}\n--- GPS TOW(ms):\t\t\t\t{3}\n--- Available Maintenance Power Time(s):\t{4}\n--- Temperature(C):\t\t\t\t{5}\n--- Temperature(T):\t\t\t\t{6}\n--- ALM ID:\t\t\t\t{7}\n--- Total SVs with BE:\t\t\t{8}\n--- Total SVs with EE:\t\t\t{9}\n--- Total SVs with ALM:\t\t\t{10}\n", new object[] { list[num++], list[num++], list[num++], list[num++], list[num++], list[num++], list[num++], list[num++], list[num++], list[num++], list[num++] });
            list.Clear();
            if (DisplayStatsData.isValid_uNavTimeCorrection)
            {
                list.Add(DisplayStatsData.uNavTimeCorrection.ToString());
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_MeanCodePhaseCorrection)
            {
                list.Add((((double) DisplayStatsData.MeanCodePhaseCorrection) / 1000.0).ToString("F3"));
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_StdPseudoRanges)
            {
                list.Add((((double) DisplayStatsData.StdPseudoRanges) / 1000.0).ToString("F3"));
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_MeanDopplerResidual)
            {
                list.Add((((double) DisplayStatsData.MeanDopplerResidual) / 1000.0).ToString("F3"));
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_StdDeltaRanges)
            {
                list.Add((((double) DisplayStatsData.StdDeltaRanges) / 1000.0).ToString("F3"));
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_IsBitSynch)
            {
                list.Add((DisplayStatsData.IsBitSynch == 1) ? "Yes" : "No");
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_IsFrameSynch)
            {
                list.Add((DisplayStatsData.IsFrameSynch == 1) ? "Yes" : "No");
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_TotalTimeCorrection)
            {
                list.Add(DisplayStatsData.TotalTimeCorrection.ToString());
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_SVBeforeEphCollection)
            {
                list.Add("0x" + DisplayStatsData.SVBeforeEphCollection.ToString("X"));
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_SVAfterEphCollection)
            {
                list.Add("0x" + DisplayStatsData.SVAfterEphCollection.ToString("X"));
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_IsNav)
            {
                list.Add((DisplayStatsData.IsNav == 1) ? "Yes" : "No");
            }
            else
            {
                list.Add("N/A");
            }
            if (DisplayStatsData.isValid_IsSuccessAlmCollection)
            {
                list.Add((DisplayStatsData.IsSuccessAlmCollection == 1) ? "Yes" : "No");
            }
            else
            {
                list.Add("N/A");
            }
            num = 0;
            strArray[1] = string.Format("\n--- uNav Correction(us):\t\t{0}\n--- Mean Code Phase Correction(m):\t{1}\n--- Pseudo Range Std(m):\t\t{2}\n--- Mean Doppler residual(Hz):\t\t{3}\n--- Delta Ranges Std(Hz):\t\t{4}\n--- Bit Synch achieved:\t\t{5}\n--- Frame Synch achieved:\t\t{6}\n--- Total Time Correction:\t\t{7}\n--- SV mask before Eph collection:\t{8}\n--- SV mask after Eph collection:\t{9}\n--- Successful Nav:\t\t\t{10}\n--- Successful ALM collection:\t\t{11}\n", new object[] { list[num++], list[num++], list[num++], list[num++], list[num++], list[num++], list[num++], list[num++], list[num++], list[num++], list[num++], list[num++] });
            return strArray;
        }

        public virtual string FormatMsgSeven(string logStr)
        {
            return string.Empty;
        }

        public virtual string FormatNavParameters(string csvString)
        {
            float num16;
            if (csvString == string.Empty)
            {
                return string.Empty;
            }
            string[] strArray = csvString.Split(new char[] { ',' });
            StringBuilder builder = new StringBuilder();
            ushort num = 0;
            ushort num2 = 0;
            ushort num3 = 0;
            ushort num4 = 0;
            ushort num5 = 0;
            ushort num6 = 0;
            ushort num7 = 0;
            ushort num8 = 0;
            short num9 = 0;
            short num10 = 0;
            byte num11 = 0;
            int num12 = 0;
            int num13 = 0;
            byte num14 = 0;
            byte num15 = 0;
            string str = null;
            num = Convert.ToUInt16(strArray[3]);
            num2 = Convert.ToUInt16(strArray[4]);
            num3 = Convert.ToUInt16(strArray[5]);
            num4 = Convert.ToUInt16(strArray[7]);
            num5 = Convert.ToUInt16(strArray[10]);
            num6 = Convert.ToUInt16(strArray[11]);
            num7 = Convert.ToUInt16(strArray[12]);
            num8 = Convert.ToUInt16(strArray[14]);
            double num17 = Convert.ToDouble(strArray[15]) / 10.0;
            num9 = Convert.ToInt16(strArray[0x12]);
            num10 = Convert.ToInt16(strArray[0x13]);
            num11 = Convert.ToByte(strArray[0x16]);
            num12 = Convert.ToInt32(strArray[0x17]);
            num13 = Convert.ToInt32(strArray[0x18]);
            num15 = Convert.ToByte(strArray[0x1b]);
            builder.Append("### Navigation Parameters ###\n");
            if ((num & 1) == 1)
            {
                builder.Append("ABPMode: on");
            }
            else
            {
                builder.Append("ABPMode: off");
            }
            builder.Append("\r\n");
            if ((num & 4) == 4)
            {
                builder.Append("5Hz Mode: on");
            }
            else
            {
                builder.Append("5Hz Mode: off");
            }
            builder.Append("\r\nAltMode: ");
            switch (num2)
            {
                case 1:
                    builder.Append("always");
                    break;

                case 2:
                    builder.Append("never use");
                    break;

                case 3:
                    builder.Append("unk");
                    break;

                default:
                    builder.Append("auto");
                    break;
            }
            builder.Append("\r\nAltSource: ");
            switch (num3)
            {
                case 1:
                    builder.Append("fixed");
                    break;

                case 2:
                    builder.Append("dynamic");
                    break;

                case 3:
                    builder.Append("unk");
                    break;

                default:
                    builder.Append("last KF alt");
                    break;
            }
            builder.Append(string.Format(clsGlobal.MyCulture, "\r\nAltitude: {0}", new object[] { strArray[6] }));
            builder.Append("\r\nDegradedMode: ");
            switch (num4)
            {
                case 1:
                    builder.Append("TThenD");
                    break;

                case 2:
                    builder.Append("DirOnly");
                    break;

                case 3:
                    builder.Append("TimeOnly");
                    break;

                case 4:
                    builder.Append("Disabled");
                    break;

                case 5:
                    builder.Append("unk");
                    break;

                default:
                    builder.Append("DThenT");
                    break;
            }
            builder.Append(string.Format(clsGlobal.MyCulture, "\r\nDegradedTimeout: {0} s", new object[] { strArray[8] }));
            builder.Append(string.Format(clsGlobal.MyCulture, "\r\nDRTimeout: {0} s", new object[] { strArray[9] }));
            builder.Append("\r\nTrkSmoothMode: ");
            switch (num5)
            {
                case 1:
                    builder.Append("enabled");
                    break;

                case 2:
                    builder.Append("unk");
                    break;

                default:
                    builder.Append("disabled");
                    break;
            }
            builder.Append("\r\nStaticNav: ");
            switch (num6)
            {
                case 1:
                    builder.Append("enabled");
                    break;

                case 2:
                    builder.Append("unk");
                    break;

                default:
                    builder.Append("disabled");
                    break;
            }
            builder.Append("\r\n3SV LSQ: ");
            switch (num7)
            {
                case 1:
                    builder.Append("enabled");
                    break;

                case 2:
                    builder.Append("unk");
                    break;

                default:
                    builder.Append("disabled");
                    break;
            }
            builder.Append("\r\nDOPMaskMode: ");
            switch (num8)
            {
                case 1:
                    builder.Append("PDOP");
                    break;

                case 2:
                    builder.Append("HDOP");
                    break;

                case 3:
                    builder.Append("GDOP");
                    break;

                case 4:
                    builder.Append("disabled");
                    break;

                case 5:
                    builder.Append("unk");
                    break;

                default:
                    builder.Append("auto");
                    break;
            }
            builder.Append(string.Format(clsGlobal.MyCulture, "\r\nElevMask: {0:} deg", new object[] { num17.ToString("N1") }));
            builder.Append(string.Format(clsGlobal.MyCulture, "\r\nPwrMask: {0} dBHz", new object[] { strArray[0x10] }));
            builder.Append("\r\nDGPSSrc: ");
            switch (num9)
            {
                case 1:
                    builder.Append("WAAS");
                    break;

                case 2:
                    builder.Append("Serial");
                    break;

                case 3:
                    builder.Append("Internal Beacon");
                    break;

                case 4:
                    builder.Append("Software");
                    break;

                case 5:
                    builder.Append("unk");
                    break;

                default:
                    builder.Append("None");
                    break;
            }
            builder.Append("\r\nDGPSMode: ");
            switch (num10)
            {
                case 1:
                    builder.Append("exclusive");
                    break;

                case 2:
                    builder.Append("never use");
                    break;

                case 3:
                    builder.Append("mixed");
                    break;

                case 4:
                    builder.Append("unk");
                    break;

                default:
                    builder.Append("auto");
                    break;
            }
            builder.Append(string.Format(clsGlobal.MyCulture, "\r\nDGPSTimeout: {0} s\n", new object[] { strArray[20] }));
            if ((num12 == 0) || (num13 == 0))
            {
                num16 = 0f;
            }
            else
            {
                num16 = ((float) (100 * num12)) / ((float) num13);
            }
            if (num11 == 1)
            {
                str = str + "Push-To-Fix enabled\n";
            }
            else if (num15 == 0)
            {
                str = str + "Continuous power enabled\n";
            }
            else
            {
                str = str + string.Format(clsGlobal.MyCulture, "TricklePower enabled with {0} ms on, {1} duty cycle\n", new object[] { num12, num16 });
            }
            if (num14 == 1)
            {
                str = str + string.Format(clsGlobal.MyCulture, "User tasks enabled, period = {0}\n", new object[] { strArray[0x1a] });
            }
            else
            {
                str = str + "User tasks disabled";
            }
            builder.Append(str);
            builder.Append(string.Format(clsGlobal.MyCulture, "MaxAcqTime = {0} ms; MaxOffTime = {1} ms\n", new object[] { strArray[0x1c], strArray[0x1d] }));
            return builder.ToString();
        }

        public virtual string FormatNavSubSysString(string csvString)
        {
            return string.Empty;
        }

        public virtual string FormatPositionResponse(string csvString)
        {
            return string.Empty;
        }

        public string FormatTimeTransferResponse(string hexString)
        {
            string str = string.Empty;
            try
            {
                string str2 = hexString.Replace(" ", "");
                int startIndex = 0;
                byte num2 = (byte) uint.Parse(str2.Substring(startIndex, 2), NumberStyles.HexNumber);
                startIndex += 2;
                ushort num3 = ushort.Parse(str2.Substring(startIndex, 4), NumberStyles.HexNumber);
                startIndex += 4;
                ulong num4 = ulong.Parse(str2.Substring(startIndex, 10), NumberStyles.HexNumber);
                startIndex += 10;
                int num5 = int.Parse(str2.Substring(startIndex, 6), NumberStyles.HexNumber);
                startIndex += 6;
                byte timeUncert = (byte) uint.Parse(str2.Substring(startIndex, 2), NumberStyles.HexNumber);
                return string.Format(clsGlobal.MyCulture, "Time Transfer Response Type:{0} WeekNum:{1}  TOW(us):{2} DeltaUTC(ms): {3} Accuracy: {4}", new object[] { (num2 == 0xff) ? "Precise" : "Coarse", num3, num4, num5, (num2 == 0xff) ? this.DecodePreciseTTaccuracy(timeUncert) : DecodeCoarseTTaccuracy(timeUncert) });
            }
            catch
            {
                return str;
            }
        }

        public virtual void GetAutoReplyParameters(string configFilePath)
        {
        }

        public double GetClkDrift(ushort clk_drift)
        {
            ushort num = (ushort) (clk_drift >> 15);
            ushort num2 = (ushort) ((clk_drift >> 11) & 15);
            ushort num3 = (ushort) (clk_drift & 0x7ff);
            if (num != 0)
            {
                return (double) ((float) ((-0.005 * (1.0 + (((double) num3) / 2048.0))) * (((int) 1) << num2)));
            }
            return (float) ((0.005 * (1.0 + (((double) num3) / 2048.0))) * (((int) 1) << num2));
        }

        public int GetExitTunnelAcqTime(string TOW)
        {
            int gpsTOW = Convert.ToInt32(Convert.ToDecimal(TOW));
            if (this._Tunnel_Truth_Data != null)
            {
                if (this._Tunnel_Truth_Data.RecentTunnelExit(gpsTOW, 60))
                {
                    if (((this._ExitTunnelAcqTime == TIME_NOT_SET) && (this.Tracking_Analysis_Data.NumSVSWithBFState() != 0)) && (this.Tracking_Analysis_Data.NumSVSGainedBFState() == this.Tracking_Analysis_Data.NumSVSWithBFState()))
                    {
                        this._ExitTunnelAcqTime = this._Tunnel_Truth_Data.GetTimeSinceExitingTunnel(gpsTOW, 60);
                        return this._ExitTunnelAcqTime;
                    }
                }
                else
                {
                    this._ExitTunnelAcqTime = TIME_NOT_SET;
                }
            }
            return 0;
        }

        public virtual string GetGWControllerScanResult(string message)
        {
            return string.Empty;
        }

        public string GetInsideTunnelNum(string TOW)
        {
            if (this._Tunnel_Truth_Data != null)
            {
                int num = this._Tunnel_Truth_Data.InsideTunnelNum(Convert.ToInt32(Convert.ToDecimal(TOW)));
                return string.Format("{0:D}", num);
            }
            return "0";
        }

        public virtual string[] GetLatLonAltPosition(string csvString)
        {
            return new string[] { "-9999", "-9999", "-9999" };
        }

        public virtual int GetMeasurement(string logTime, string message)
        {
            return 0;
        }

        public virtual byte GetNavigationMode(string message)
        {
            return 0;
        }

        public virtual string GetNumSVSTrk(string message)
        {
            return string.Empty;
        }

        public virtual string GetPerChanAvgCNo(string message)
        {
            return string.Empty;
        }

        public virtual int GetPosition(string logTime, string message)
        {
            return 0;
        }

        public virtual int GetPositionFromMeasurement(string logTime, string message)
        {
            return 0;
        }

        internal virtual QoSSetting getQoSSettings(string ICDVersion)
        {
            return null;
        }

        public string GetRecentlyExitedTunnelNum(string TOW, int maxTimeOutOfTunnel)
        {
            if (this._Tunnel_Truth_Data != null)
            {
                int num = this._Tunnel_Truth_Data.RecentTunnelExitNum(Convert.ToInt32(Convert.ToDecimal(TOW)), maxTimeOutOfTunnel);
                return string.Format("{0:D}", num);
            }
            return "0";
        }

        public virtual void GetReferencePosition(string siteName, string configFilePath)
        {
            string locationName = string.Empty;
            if (((configFilePath != string.Empty) && File.Exists(configFilePath)) && (siteName != string.Empty))
            {
                locationName = new IniHelper(configFilePath).GetIniFileString("SITE_NAME", siteName, "");
                if (this._commWindow != null)
                {
                    PositionInLatLonAlt referencePosition = this._commWindow.m_NavData.GetReferencePosition(locationName);
                    this._commWindow.m_NavData.RefAlt = referencePosition.altitude;
                    this._commWindow.m_NavData.RefLat = referencePosition.latitude;
                    this._commWindow.m_NavData.RefLon = referencePosition.longitude;
                    this._commWindow.AutoReplyCtrl.ApproxPositionCtrl.Lat = referencePosition.latitude;
                    this._commWindow.AutoReplyCtrl.ApproxPositionCtrl.Lon = referencePosition.longitude;
                    this._commWindow.AutoReplyCtrl.ApproxPositionCtrl.Alt = referencePosition.altitude;
                }
            }
        }

        public virtual string GetSiRFAwareScanResult(string message)
        {
            return string.Empty;
        }

        public virtual string GetSVSTrkTOW(string message)
        {
            return string.Empty;
        }

        public virtual void GetSWVersion(string message)
        {
        }

        public virtual string GetTTFF()
        {
            return string.Empty;
        }

        public virtual string GetTTFFFromHash(Hashtable msgH)
        {
            return string.Empty;
        }

        public virtual bool GetTTFFValues(string logTime, string message, int type)
        {
            return false;
        }

        public int GetTunnelLostLockTimeAllSVs(string TOW)
        {
            int gpsTOW = Convert.ToInt32(Convert.ToDecimal(TOW));
            if (this._Tunnel_Truth_Data != null)
            {
                if (this._Tunnel_Truth_Data.InsideTunnel(gpsTOW))
                {
                    if (((this._LostLockTimeAllSVs == TIME_NOT_SET) && (this.Tracking_Analysis_Data.NumSVSLostBFState() > 0)) && (this.Tracking_Analysis_Data.NumSVSWithBFState() == 0))
                    {
                        this._LostLockTimeAllSVs = this._Tunnel_Truth_Data.GetTimeSinceEnteringTunnel(gpsTOW);
                        return this._LostLockTimeAllSVs;
                    }
                }
                else
                {
                    this._LostLockTimeAllSVs = TIME_NOT_SET;
                }
            }
            return 0;
        }

        public int GetTunnelLostLockTimeFirstSV(string TOW)
        {
            int gpsTOW = Convert.ToInt32(Convert.ToDecimal(TOW));
            if (this._Tunnel_Truth_Data != null)
            {
                if (this._Tunnel_Truth_Data.InsideTunnel(gpsTOW))
                {
                    if ((this._LostLockTimeFirstSV == TIME_NOT_SET) && (this.Tracking_Analysis_Data.NumSVSLostBFState() > 0))
                    {
                        this._LostLockTimeFirstSV = this._Tunnel_Truth_Data.GetTimeSinceEnteringTunnel(gpsTOW);
                        return this._LostLockTimeFirstSV;
                    }
                }
                else
                {
                    this._LostLockTimeFirstSV = TIME_NOT_SET;
                }
            }
            return 0;
        }

        public string HorizontalErrorCalc(string latStr, string lonStr, string refLatStr, string refLonStr)
        {
            string str;
            try
            {
                double lat = Convert.ToDouble(latStr) / 10000000.0;
                double lon = Convert.ToDouble(lonStr) / 10000000.0;
                double num3 = Convert.ToDouble(refLatStr);
                double num4 = Convert.ToDouble(refLonStr);
                double num5 = PositionErrorCalc.Get2DPositionErrorInMeters(lat, lon, num3, num4);
                str = string.Format(clsGlobal.MyCulture, "{0:F6}", new object[] { num5 });
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        protected int ImportINT16(byte[] nArray, int nIdx)
        {
            return ((nArray[nIdx] << 8) | nArray[nIdx + 1]);
        }

        protected int ImportINT32(byte[] nArray, int nIdx)
        {
            return ((((nArray[nIdx] << 0x18) | (nArray[nIdx + 1] << 0x10)) | (nArray[nIdx + 2] << 8)) | nArray[nIdx + 3]);
        }

        public virtual byte IsNavigated(string message)
        {
            return 0;
        }

        public bool LoadTunnelTruthData(string filename)
        {
            bool flag;
            try
            {
                this._Tunnel_Truth_Data = new TunnelTimestampFile(filename);
                if (this._Tunnel_Truth_Data != null)
                {
                    return true;
                }
                flag = false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }

        public void LogCleanup()
        {
            foreach (string str in this._rxLogFileHash.Keys)
            {
                LogManager manager = (LogManager) this._rxLogFileHash[str];
                if (manager.IsFileOpen())
                {
                    manager.CloseFile();
                }
            }
            this._rxLogFileHash.Clear();
        }

        public void LogClose(string logName)
        {
            if (this._rxLogFileHash.ContainsKey(logName))
            {
                LogManager manager = (LogManager) this._rxLogFileHash[logName];
                if (manager.IsFileOpen())
                {
                    manager.CloseFile();
                }
            }
        }

        public void LogWrite(string logName, string str)
        {
            if ((str.Length != 0) && this._rxLogFileHash.ContainsKey(logName))
            {
                LogManager manager = (LogManager) this._rxLogFileHash[logName];
                if (manager.IsFileOpen())
                {
                    manager.WriteLine(str);
                }
            }
        }

        public void NavDataInit()
        {
            this._rxNavData.IsNav = false;
            this._rxNavData.Valid = false;
            this._rxNavData.MeasAlt = -9999.0;
            this._rxNavData.MeasLat = -9999.0;
            this._rxNavData.MeasLon = -9999.0;
            this._rxNavData.Nav2DPositionError = -9999.0;
            this._rxNavData.Nav3DPositionError = -9999.0;
            this._rxNavData.NavVerticalPositionError = -9999.0;
            if (this._commWindow != null)
            {
                this._commWindow.MonitorNav = true;
            }
        }

        public virtual void OpenChannel(string channel)
        {
        }

        public virtual void OpenSession(byte sessionOpenReqInfo)
        {
            this.AllowSessOpen = false;
        }

        public virtual void PollClockStatus()
        {
            string csvMessage = string.Empty;
            string msg = string.Empty;
            ArrayList list = new ArrayList();
            StringBuilder builder = new StringBuilder();
            list = this._commWindow.m_Protocols.GetInputMessageStructure(0x90, -1, "Poll Clock Status", "SSB");
            if (this._commWindow._rxType == CommunicationManager.ReceiverType.SLC)
            {
                builder.Append("238");
                builder.Append(clsGlobal.SiRFLiveDelimeter);
            }
            for (int i = 0; i < list.Count; i++)
            {
                builder.Append(((InputMsg) list[i]).defaultValue);
                builder.Append(",");
            }
            csvMessage = builder.ToString().TrimEnd(new char[] { clsGlobal.SiRFLiveDelimeter });
            msg = this._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, "Poll Clock Status", "SSB");
            this._commWindow.WriteData(msg);
        }

        public virtual void PollNavigationParameters()
        {
        }

        public virtual void PollSWVersion()
        {
        }

        public string PostionErrorCalc(string latStr, string lonStr, string altStr, string refLatStr, string refLonStr, string refAltStr)
        {
            string str;
            try
            {
                PositionErrorCalc calc = new PositionErrorCalc();
                double lat = Convert.ToDouble(latStr) / 10000000.0;
                double lon = Convert.ToDouble(lonStr) / 10000000.0;
                double alt = Convert.ToDouble(altStr) / 100.0;
                double num4 = Convert.ToDouble(refLatStr);
                double num5 = Convert.ToDouble(refLonStr);
                double num6 = Convert.ToDouble(refAltStr);
                calc.GetPositionErrorsInMeter(lat, lon, alt, num4, num5, num6);
                str = string.Format(clsGlobal.MyCulture, "{0:F6},{1:F6}", new object[] { calc.HorizontalError, calc.VerticalErrorInMeter });
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        public virtual string PrintFormatedMPMStatus(Hashtable myHash)
        {
            SiRFawareStatsParams displayStatsData = this.DecodeMPMStats(myHash);
            string[] strArray = this.FormatMPMStatsMessage(displayStatsData);
            return (strArray[0] + strArray[1]);
        }

        public void SaveTestSetup(string key, string inputValue)
        {
            string str = key;
            if (str != null)
            {
                if (!(str == "RX COM Port"))
                {
                    if (str == "RX Serial Number")
                    {
                        this._resetCtrl.DutStationSetup.RxSN = inputValue;
                    }
                    else if (str == "Attenuation")
                    {
                        try
                        {
                            this._resetCtrl.DutStationSetup.Atten = Convert.ToDouble(inputValue);
                        }
                        catch
                        {
                            this._resetCtrl.DutStationSetup.Atten = -9999.0;
                        }
                    }
                    else if (str == "SV List")
                    {
                        this._resetCtrl.DutStationSetup.SVList = inputValue;
                    }
                    else if (str == "QoS")
                    {
                        this._resetCtrl.DutStationSetup.qosParams.ICDSTR = inputValue;
                        this._resetCtrl.DutStationSetup.qosParams.PosReqType = "0";
                        this._resetCtrl.DutStationSetup.qosParams.NumFixes = this._commWindow.AutoReplyCtrl.PositionRequestCtrl.NumFixes.ToString();
                        this._resetCtrl.DutStationSetup.qosParams.TBFixes = this._commWindow.AutoReplyCtrl.PositionRequestCtrl.TimeBtwFixes.ToString();
                        this._resetCtrl.DutStationSetup.qosParams.Position2DError = this._commWindow.AutoReplyCtrl.PositionRequestCtrl.HorrErrMax.ToString();
                        this._resetCtrl.DutStationSetup.qosParams.Position3DError = this._commWindow.AutoReplyCtrl.PositionRequestCtrl.VertErrMax.ToString();
                        this._resetCtrl.DutStationSetup.qosParams.RespTMax = this._commWindow.AutoReplyCtrl.PositionRequestCtrl.RespTimeMax.ToString();
                        this._resetCtrl.DutStationSetup.qosParams.TAccPriority = this._commWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority.ToString();
                        this._resetCtrl.DutStationSetup.qosParams.LocMethod = this._commWindow.AutoReplyCtrl.PositionRequestCtrl.LocMethod.ToString();
                        this._resetCtrl.DutStationSetup.qosParams.AlmReqFlag = this._commWindow.AutoReplyCtrl.PositionRequestCtrl.AlmReply.ToString();
                    }
                }
                else
                {
                    this._resetCtrl.DutStationSetup.RXCommIntf = inputValue;
                }
            }
            this._resetCtrl.DutStationSetup.isInit = true;
        }

        public virtual void SendApproximatePositionAiding(string site)
        {
        }

        public virtual void SendFreqAiding()
        {
        }

        public virtual void SendHWConfig()
        {
        }

        public virtual void SendMPM_V2(byte timeout, byte control)
        {
        }

        public virtual void SendPositionRequest(byte flag)
        {
        }

        public virtual void SendPositionRequest(string EphSite)
        {
        }

        public virtual void SendPushAiding(byte availMask, byte reqMask)
        {
        }

        public void SendRaw(string rawString)
        {
            try
            {
                string msg = rawString.Replace(" ", "");
                this._commWindow.WriteData(msg);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public virtual void SendReject(int chanID, int msgId, int msgSubId, int reason, string messagName)
        {
        }

        public virtual void SendTimeAiding()
        {
        }

        public virtual void SendTrackerConfig()
        {
        }

        public virtual void SendTTBTimeAidingConfig()
        {
            if (this._commWindow != null)
            {
                string tTBTimeAidingCfgMsg = this._commWindow.AutoReplyCtrl.GetTTBTimeAidingCfgMsg();
                this._commWindow.WriteData_TTB(tTBTimeAidingCfgMsg);
                this._commWindow.waitforMsgFromTTB(0xcc, 80);
            }
        }

        public virtual void SetAidingVersion(string version)
        {
            this._aidingProtocolVersion = version;
            this._commWindow.AutoReplyCtrl.AidingProtocolVersion = version;
        }

        public virtual void SetCGEEPrediction(uint secondsCGEEdisable)
        {
        }

        public virtual void SetClockDrift(string message)
        {
        }

        public virtual void SetControlChannelVersion(string version)
        {
            this._controlChannelVersion = version;
            this._commWindow.AutoReplyCtrl.ControlChannelVersion = version;
        }

        public void SetGPSTime(string gpsWeek, string gpsTow)
        {
            try
            {
                GPSDateTime inTime = new GPSDateTime();
                int inWeek = Convert.ToInt32(gpsWeek) + 0x400;
                double inTOW = Convert.ToDouble(gpsTow) / 100.0;
                inTime.SetUTCOffset(this._utcOffset);
                inTime.SetTime(inWeek, inTOW);
                this._gpsTimer.SetTime(inTime);
                if ((this._resetCtrl != null) && (this._resetCtrl.ResetGPSTimer != null))
                {
                    double gPSTOW = this._resetCtrl.ResetGPSTimer.GetTime().GetGPSTOW();
                    if ((gPSTOW < (inTOW - 10.0)) || (gPSTOW > (inTOW + 10.0)))
                    {
                        inTime.SetUTCOffset(this._utcOffset);
                        inTime.SetTime(inWeek, inTOW);
                        this._gpsTimer.SetTime(inTime);
                    }
                }
            }
            catch (Exception exception)
            {
                this._commWindow.WriteApp("Set GPS Time error: " + exception.Message);
            }
        }

        public virtual void SetMEMSMode(int state)
        {
        }

        public void SetMessageRate(int mid, int sendNow, int rate, string protocol)
        {
            string messageName = "Set Message Rate";
            StringBuilder builder = new StringBuilder();
            if ((this._commWindow.RxType == CommunicationManager.ReceiverType.SLC) && (this._commWindow.MessageProtocol != "OSP"))
            {
                builder.Append(0xee);
                builder.Append(",");
            }
            builder.Append(0xa6);
            builder.Append(",");
            builder.Append(sendNow);
            builder.Append(",");
            builder.Append(mid);
            builder.Append(",");
            builder.Append(rate);
            builder.Append(",0,0,0,0");
            string msg = this._commWindow.m_Protocols.ConvertFieldsToRaw(builder.ToString(), messageName, protocol);
            this._commWindow.WriteData(msg);
        }

        public void SetMessageRateForFactory()
        {
            this.SetMessageRate(0xe1, 0, 1, "SSB");
            this.SetMessageRate(0xff, 0, 1, "SSB");
            this.SetMessageRate(4, 0, 1, "SSB");
            this.SetMessageRate(2, 0, 1, "SSB");
            this.SetMessageRate(0x29, 0, 1, "SSB");
        }

        public virtual void SetPosCalcMode(int state)
        {
        }

        public virtual void SetPowerMode(bool warning)
        {
        }

        public void SetRxNavData(ref NavigationAnalysisData target)
        {
            this._rxNavData = target;
        }

        public virtual void SetSBASRangingMode(int state)
        {
        }

        public virtual void SetValidatePostionFlag(bool inputValue)
        {
            this._rxNavData.ValidatePosition = inputValue;
        }

        public virtual void SiRFNavStart()
        {
        }

        public virtual void SiRFNavStop()
        {
        }

        public virtual void SSBP2PAccuracy()
        {
        }

        public void UpdateRefLocation(string lat, string lon, string alt)
        {
            try
            {
                this._rxNavData.RefAlt = Convert.ToDouble(alt);
                this._rxNavData.RefLat = Convert.ToDouble(lat);
                this._rxNavData.RefLon = Convert.ToDouble(lon);
            }
            catch
            {
            }
        }

        public void UpdateResetInitParams()
        {
            try
            {
                if (this._resetCtrl != null)
                {
                    this._resetCtrl.ResetInitParams.ECEFX = this._commWindow.NavSolutionParams.ECEFX.ToString();
                    this._resetCtrl.ResetInitParams.ECEFY = this._commWindow.NavSolutionParams.ECEFY.ToString();
                    this._resetCtrl.ResetInitParams.ECEFZ = this._commWindow.NavSolutionParams.ECEFZ.ToString();
                    this._resetCtrl.ResetInitParams.WeekNumber = this._commWindow.NavSolutionParams.WeekNumber.ToString();
                    this._resetCtrl.ResetInitParams.TOW = this._commWindow.NavSolutionParams.TOW.ToString();
                    this._resetCtrl.ResetInitParams.ClockDrift = this._commWindow.LastClockDrift.ToString();
                }
            }
            catch
            {
            }
        }

        public string AidingProtocol
        {
            get
            {
                return this._aidingProtocol;
            }
            set
            {
                this._aidingProtocol = value;
            }
        }

        public string AidingProtocolVersion
        {
            get
            {
                return this._aidingProtocolVersion;
            }
            set
            {
                this._aidingProtocolVersion = value;
            }
        }

        public string ControlChannelProtocolFile
        {
            get
            {
                return this._controlChannelProtocolFile;
            }
            set
            {
                this._controlChannelProtocolFile = value;
            }
        }

        public string ControlChannelVersion
        {
            get
            {
                return this._controlChannelVersion;
            }
            set
            {
                this._controlChannelVersion = value;
            }
        }

        public string DefaultFreqOffset
        {
            get
            {
                return this._defaultFreqOffset;
            }
            set
            {
                this._defaultFreqOffset = value;
            }
        }

        public TestSetup DutStationSetup
        {
            get
            {
                return this._rxTestSetup;
            }
            set
            {
                this._rxTestSetup = value;
            }
        }

        public GPSTimer GPSTimerEngine
        {
            get
            {
                return this._gpsTimer;
            }
        }

        public string MessageProtocol
        {
            get
            {
                return this._messageProtocol;
            }
            set
            {
                this._messageProtocol = value;
            }
        }

        public ReceiverReset ResetCtrl
        {
            get
            {
                return this._resetCtrl;
            }
            set
            {
                this._resetCtrl = value;
            }
        }

        public double Rx2DPositionError
        {
            get
            {
                return this.RxNavData.Nav2DPositionError;
            }
        }

        public CommunicationManager RxCommWindow
        {
            get
            {
                return this._commWindow;
            }
            set
            {
                this._commWindow = value;
            }
        }

        public double RxFirstFix2DPositionError
        {
            get
            {
                return this.RxNavData.FirstFix2DPositionError;
            }
        }

        public NavigationAnalysisData RxNavData
        {
            get
            {
                return this._rxNavData;
            }
            set
            {
                this._rxNavData = value;
            }
        }

        public double RxTTFF
        {
            get
            {
                return this.RxNavData.TTFFReport;
            }
        }

        public byte UTCOffset
        {
            get
            {
                return this._utcOffset;
            }
            set
            {
                this._utcOffset = value;
            }
        }
    }
}

