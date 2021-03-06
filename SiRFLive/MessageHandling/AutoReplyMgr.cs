﻿namespace SiRFLive.MessageHandling
{
    using SiRFLive.Communication;
    using SiRFLive.Configuration;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.IO;

    public class AutoReplyMgr
    {
        private string _acqAssistDataMsg = string.Empty;
        private string _acqAssistFilePath = string.Empty;
        protected string _ai3ProtocolFile;
        internal byte _aidingFlag;
        protected string _aidingProtocolVersion = string.Empty;
        private string _almMsgFromTTB = string.Empty;
        private ApproxPosParams _approxPositionParams = new ApproxPosParams();
        private string _approxPosRespMsg = string.Empty;
        private autoReplyCfgParams _autoReplyParams = new autoReplyCfgParams();
        private string _auxNavMsgTTB = string.Empty;
        protected string _controlChannelVersion = "1.0";
        private string _ephDataMsg = string.Empty;
        private string _ephDataMsgBackup = string.Empty;
        private string _ephFilePath = string.Empty;
        private byte _forceAidingRequestMask;
        private FreqTransParams _freqTransferParams = new FreqTransParams();
        private string _freqTransferRespMsg = string.Empty;
        private string _freqTransRequestMsg = string.Empty;
        private string _gpsTowAssistTTB = string.Empty;
        private HWConfigParams _hwCfgParams = new HWConfigParams();
        private string _hwCfgRespMsg = string.Empty;
        private string _hwCfgRespMsgToTTB = string.Empty;
        private PositionRequestParams _positionRequestParams = new PositionRequestParams();
        private bool _posReqAck;
        private string _posRequestMsg = string.Empty;
        protected string _protocolFile;
        private bool _pushAidingAvailability;
        private byte _pushAidingMask;
        private int _pushingAidingDelay;
        private string _sf123MsgFromTTB = string.Empty;
        private string _sf45DataSet0MsgFromTTB = string.Empty;
        private string _sf45DataSet1MsgFromTTB = string.Empty;
        private string _timeAidingCfgMsgToTTB = string.Empty;
        private TimeTransParams _timeTransferParams = new TimeTransParams();
        private string _timeTransferRequestMsg = string.Empty;
        private string _timeTransferRespMsg = string.Empty;
        private TTBTimeAidingCfgParams _ttbTimeAidingParams = new TTBTimeAidingCfgParams();
        private string _utcModelMsgTTB = string.Empty;

        public virtual string AcqAssistFromTTBRespMsg(string ttbAcqAssistData)
        {
            string str = string.Empty;
            string[] separator = new string[] { "\r\n" };
            if (!(ttbAcqAssistData != ""))
            {
                return str;
            }
            string[] strArray2 = ttbAcqAssistData.Split(separator, StringSplitOptions.None);
            strArray2.GetLength(0);
            string str2 = string.Empty;
            string str3 = string.Empty;
            foreach (string str4 in strArray2)
            {
                if ((str4 != null) && !(str4 == string.Empty))
                {
                    string str5 = str4.Replace(" ", "");
                    str3 = str5.Substring(0x12, 8);
                    int length = str5.Length;
                    str2 = str2 + str5.Substring(0x1a, length - 0x22);
                }
            }
            return (str + str3 + str2);
        }

        public virtual void AutoReplyApproxPositionResp()
        {
        }

        public virtual string AutoReplyFreqTransferResp()
        {
            return string.Empty;
        }

        public virtual void AutoReplyHWCfgResp()
        {
        }

        public virtual void AutoReplyTimeTransferResp()
        {
        }

        public virtual string AutoSendPositionRequestMsg()
        {
            return string.Empty;
        }

        public static string BuildOSPEphAidingMsg(string ephCsvString, string version)
        {
            int num = 0x19;
            string inputMsgFile = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\protocols\Protocols_F.xml";
            ArrayList list = new ArrayList();
            list = utils_AutoReply.GetMessageStructure(inputMsgFile, CommunicationManager.ReceiverType.OSP, 0xd3, 2, "OSP", version);
            ArrayList fieldList = new ArrayList();
            if (ephCsvString == "")
            {
                return ephCsvString;
            }
            string[] strArray = ephCsvString.Split(new char[] { ',' });
            int num2 = strArray.GetLength(0) / num;
            int num3 = 0;
            while ((num3 < list.Count) && (((SLCMsgStructure) list[num3]).fieldName != "NUM_SVS"))
            {
                num3++;
            }
            SLCMsgStructure structure = (SLCMsgStructure) list[num3];
            structure.defaultValue = num2.ToString();
            list[num3] = structure;
            num3 = 0;
            while ((num3 < list.Count) && (((SLCMsgStructure) list[num3]).fieldName != "1st EPH_FLAG"))
            {
                num3++;
            }
            int num4 = num3;
            for (int i = 0; num4 < (num3 + strArray.GetLength(0)); i++)
            {
                structure = (SLCMsgStructure) list[num4];
                structure.defaultValue = strArray[i];
                list[num4] = structure;
                num4++;
            }
            for (num3 = 0; num3 < (strArray.Length + 3); num3++)
            {
                fieldList.Add(list[num3]);
            }
            return utils_AutoReply.FieldList_to_HexString(false, fieldList, 0);
        }

        public virtual string EphFromTTBRespMsg(string ttbEphData)
        {
            string str = string.Empty;
            string[] separator = new string[] { "\r\n" };
            if (ttbEphData != "")
            {
                string[] strArray2 = ttbEphData.Split(separator, StringSplitOptions.None);
                strArray2.GetLength(0);
                foreach (string str2 in strArray2)
                {
                    if ((str2 != null) && !(str2 == string.Empty))
                    {
                        string str3 = str2.Replace(" ", "");
                        int length = str3.Length;
                        str = str + str3.Substring(0x10, length - 0x18);
                    }
                }
            }
            return str;
        }

        public virtual string Get2HoursEphAidingMsgFromFile(string version, string ephFilePath)
        {
            if ((ephFilePath.Length == 0) || !File.Exists(ephFilePath))
            {
                return ("Error: File does not exist: " + ephFilePath);
            }
            return utils_AutoReply.get2HoursEphFromFile(ephFilePath);
        }

        public virtual string GetAcqAssistMsgFromFile(string version, string acqAssistFilePath, double gpsTowNow)
        {
            if ((acqAssistFilePath.Length == 0) || !File.Exists(acqAssistFilePath))
            {
                return ("Error: File does not exist: " + acqAssistFilePath);
            }
            return utils_AutoReply.getAcqAssistDataFromFile(acqAssistFilePath, gpsTowNow);
        }

        public virtual string GetEphAidingMsgFromFile(string version, string ephFilePath, string gpsTimeStr)
        {
            if ((ephFilePath.Length == 0) || !File.Exists(ephFilePath))
            {
                return ("Error: File does not exist: " + ephFilePath);
            }
            return utils_AutoReply.getEphFromFile(ephFilePath, gpsTimeStr);
        }

        public string GetTTBTimeAidingCfgMsg()
        {
            ArrayList fieldList = new ArrayList();
            byte num = 0;
            byte type = 0;
            if (this.TTBTimeAidingParams.Enable)
            {
                num = 1;
            }
            type = this.TTBTimeAidingParams.Type;
            fieldList = utils_AutoReply.GetMessageStructure(this.ProtocolFile, CommunicationManager.ReceiverType.OSP, 0xe1, -1, "TTB", "1.0");
            int num3 = 0;
            while ((num3 < fieldList.Count) && (((SLCMsgStructure) fieldList[num3]).fieldName != "ENABLE"))
            {
                num3++;
            }
            SLCMsgStructure structure = (SLCMsgStructure) fieldList[num3];
            structure.defaultValue = num.ToString();
            fieldList[num3] = structure;
            num3 = 0;
            while ((num3 < fieldList.Count) && (((SLCMsgStructure) fieldList[num3]).fieldName != "COARSE_TIME_TRANSFER"))
            {
                num3++;
            }
            structure = (SLCMsgStructure) fieldList[num3];
            structure.defaultValue = type.ToString();
            fieldList[num3] = structure;
            num3 = 0;
            while ((num3 < fieldList.Count) && (((SLCMsgStructure) fieldList[num3]).fieldName != "ACCURACY"))
            {
                num3++;
            }
            structure = (SLCMsgStructure) fieldList[num3];
            structure.defaultValue = this.TTBTimeAidingParams.Accuracy.ToString();
            fieldList[num3] = structure;
            num3 = 0;
            while ((num3 < fieldList.Count) && (((SLCMsgStructure) fieldList[num3]).fieldName != "SKEW"))
            {
                num3++;
            }
            structure = (SLCMsgStructure) fieldList[num3];
            structure.defaultValue = this.TTBTimeAidingParams.Skew.ToString();
            fieldList[num3] = structure;
            string str = utils_AutoReply.FieldList_to_HexString(true, fieldList, 0);
            fieldList.Clear();
            return str;
        }

        public void SetAutoReplyParameters(string configFile)
        {
            try
            {
                IniHelper helper = new IniHelper(configFile);
                string key = string.Empty;
                string category = string.Empty;
                category = "HW_CONFIG";
                key = "REPLY";
                this._hwCfgParams.Reply = helper.GetIniFileString(category, key, "") == "1";
                key = "PRECISE_TIME_ENABLED";
                this._hwCfgParams.PreciseTimeEnabled = Convert.ToByte(helper.GetIniFileString(category, key, ""));
                key = "PRECISE_TIME_DIRECTION";
                this._hwCfgParams.PreciseTimeDirection = Convert.ToByte(helper.GetIniFileString(category, key, ""));
                key = "FREQ_AIDED_ENABLED";
                this._hwCfgParams.FreqAidEnabled = Convert.ToByte(helper.GetIniFileString(category, key, ""));
                key = "FREQ_AIDED_METHOD";
                this._hwCfgParams.FreqAidMethod = Convert.ToByte(helper.GetIniFileString(category, key, ""));
                key = "RTC_AVAILABLE";
                this._hwCfgParams.RTCAvailabe = Convert.ToByte(helper.GetIniFileString(category, key, ""));
                key = "RTC_SOURCE";
                this._hwCfgParams.RTCSource = Convert.ToByte(helper.GetIniFileString(category, key, ""));
                key = "COARSE_TIME_ENABLE";
                this._hwCfgParams.CoarseTimeEnabled = Convert.ToByte(helper.GetIniFileString(category, key, ""));
                key = "REF_CLOCK_ENABLED";
                this._hwCfgParams.RefClkEnabled = Convert.ToByte(helper.GetIniFileString(category, key, ""));
                key = "NORMINAL_FREQ_HZ";
                this._hwCfgParams.NorminalFreqHz = Convert.ToInt64(helper.GetIniFileString(category, key, ""));
                key = "ENHANCED_NETWORK";
                this._hwCfgParams.NetworkEnhanceType = Convert.ToByte(helper.GetIniFileString(category, key, ""));
                category = "APPROXIMATE_POSITION";
                key = "REPLY";
                this._approxPositionParams.Reply = helper.GetIniFileString(category, key, "") == "1";
                key = "LAT";
                this._approxPositionParams.Lat = Convert.ToDouble(helper.GetIniFileString(category, key, ""));
                key = "LON";
                this._approxPositionParams.Lon = Convert.ToDouble(helper.GetIniFileString(category, key, ""));
                key = "ALT";
                this._approxPositionParams.Alt = Convert.ToDouble(helper.GetIniFileString(category, key, ""));
                key = "EST_HOR_ERR";
                this._approxPositionParams.EstHorrErr = Convert.ToDouble(helper.GetIniFileString(category, key, ""));
                key = "EST_VER_ERR";
                this._approxPositionParams.EstVertiErr = Convert.ToDouble(helper.GetIniFileString(category, key, ""));
                key = "LAT_SKEW";
                this._approxPositionParams.DistanceSkew = Convert.ToDouble(helper.GetIniFileString(category, key, ""));
                key = "LON_SKEW";
                this._approxPositionParams.HeadingSkew = Convert.ToDouble(helper.GetIniFileString(category, key, ""));
                category = "TIME_AIDING";
                key = "TIME_AIDING_TYPE";
                this._ttbTimeAidingParams.Type = Convert.ToByte(helper.GetIniFileString(category, key, ""));
                key = "TIME_ACC";
                this._ttbTimeAidingParams.Accuracy = Convert.ToUInt32(helper.GetIniFileString(category, key, ""));
                key = "TIME_SKEW";
                this._ttbTimeAidingParams.Skew = Convert.ToUInt32(helper.GetIniFileString(category, key, ""));
                category = "FREQ_AIDING";
                key = "REPLY";
                this._freqTransferParams.Reply = helper.GetIniFileString(category, key, "") == "1";
                key = "FREQ_AIDING_TYPE";
                this._freqTransferParams.FreqAidingMethod = Convert.ToInt32(helper.GetIniFileString(category, key, ""));
                key = "SCALED_FREQ_OFFSET";
                this._freqTransferParams.ScaledFreqOffset = Convert.ToInt16(helper.GetIniFileString(category, key, ""));
                key = "REF_CLOCK_INFO";
                this._freqTransferParams.RefClkInfo = Convert.ToByte(helper.GetIniFileString(category, key, ""));
                key = "REL_FREQ_ACC";
                this._freqTransferParams.Accuracy = Convert.ToDouble(helper.GetIniFileString(category, key, ""));
                key = "ECLK_SKEW";
                this._freqTransferParams.ExtClkSkewppm = Convert.ToDouble(helper.GetIniFileString(category, key, ""));
                key = "NORMINAL_FREQ";
                this._freqTransferParams.NomFreq = Convert.ToInt64(helper.GetIniFileString(category, key, ""));
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public virtual string SetupAlmfromTTB(string fullMsg)
        {
            return string.Empty;
        }

        public virtual string SetupAuxNavMsgFromTTB(string fullMsg)
        {
            return string.Empty;
        }

        public virtual string SetupNavSF123fromTTB(string fullMsg)
        {
            return fullMsg;
        }

        public virtual string SetupNavSF45FromTTB(string fullMsg)
        {
            return fullMsg;
        }

        public virtual string TranslateOSPFreqTransferRequestMsgToTTB(string hexString)
        {
            return hexString;
        }

        public virtual string TranslateSLCFreqTransRespMsgToTTB(string hexString)
        {
            return hexString;
        }

        public virtual string TranslateTTBFreqTransferRespMsgToSLC(string hexString)
        {
            return hexString;
        }

        public virtual string TranslateTTBTimeTransferRespSLC(string hexString)
        {
            return hexString;
        }

        public void UpdateAutoReplyStatus()
        {
            this.AutoReplyParams.AutoReply = ((this.AutoReplyParams.AutoReplyHWCfg || this.AutoReplyParams.AutoReplyApproxPos) || this.AutoReplyParams.AutoReplyFreqTrans) || this.AutoReplyParams.AutoReplyTimeTrans;
        }

        public string AcqAssistDataMsg
        {
            get
            {
                return this._acqAssistDataMsg;
            }
            set
            {
                this._acqAssistDataMsg = value;
            }
        }

        public string AcqDataFilePath
        {
            get
            {
                return this._acqAssistFilePath;
            }
            set
            {
                this._acqAssistFilePath = value;
            }
        }

        public byte AidingFlag
        {
            get
            {
                return this._aidingFlag;
            }
            set
            {
                this._aidingFlag = value;
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

        public string AlmMsgFromTTB
        {
            get
            {
                return this._almMsgFromTTB;
            }
            set
            {
                this._almMsgFromTTB = value;
            }
        }

        public ApproxPosParams ApproxPositionCtrl
        {
            get
            {
                return this._approxPositionParams;
            }
            set
            {
                this._approxPositionParams = value;
            }
        }

        public string ApproxPosRespMsg
        {
            get
            {
                return this._approxPosRespMsg;
            }
            set
            {
                this._approxPosRespMsg = value;
            }
        }

        public autoReplyCfgParams AutoReplyParams
        {
            get
            {
                return this._autoReplyParams;
            }
            set
            {
                this._autoReplyParams = value;
            }
        }

        public string AuxNavMsgFromTTB
        {
            get
            {
                return this._auxNavMsgTTB;
            }
            set
            {
                this._auxNavMsgTTB = value;
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

        public string EphDataMsg
        {
            get
            {
                return this._ephDataMsg;
            }
            set
            {
                this._ephDataMsg = value;
            }
        }

        public string EphDataMsgBackup
        {
            get
            {
                return this._ephDataMsgBackup;
            }
            set
            {
                this._ephDataMsgBackup = value;
            }
        }

        public string EphFilePath
        {
            get
            {
                return this._ephFilePath;
            }
            set
            {
                this._ephFilePath = value;
            }
        }

        public byte ForceAidingRequestMask
        {
            get
            {
                return this._forceAidingRequestMask;
            }
            set
            {
                this._forceAidingRequestMask = value;
            }
        }

        public FreqTransParams FreqTransferCtrl
        {
            get
            {
                return this._freqTransferParams;
            }
            set
            {
                this._freqTransferParams = value;
            }
        }

        public string FreqTransferRequestMsg
        {
            get
            {
                return this._freqTransRequestMsg;
            }
            set
            {
                this._freqTransRequestMsg = value;
            }
        }

        public string FreqTransferRespMsg
        {
            get
            {
                return this._freqTransferRespMsg;
            }
            set
            {
                this._freqTransferRespMsg = value;
            }
        }

        public string GPSTOWAssistFromTTB
        {
            get
            {
                return this._gpsTowAssistTTB;
            }
            set
            {
                this._gpsTowAssistTTB = value;
            }
        }

        public HWConfigParams HWCfgCtrl
        {
            get
            {
                return this._hwCfgParams;
            }
            set
            {
                this._hwCfgParams = value;
            }
        }

        public string HWCfgRespMsg
        {
            get
            {
                return this._hwCfgRespMsg;
            }
            set
            {
                this._hwCfgRespMsg = value;
            }
        }

        public string HwCfgRespMsgToTTB
        {
            get
            {
                return this._hwCfgRespMsgToTTB;
            }
            set
            {
                this._hwCfgRespMsgToTTB = value;
            }
        }

        public PositionRequestParams PositionRequestCtrl
        {
            get
            {
                return this._positionRequestParams;
            }
            set
            {
                this._positionRequestParams = value;
            }
        }

        public string PositionRequestMsg
        {
            get
            {
                return this._posRequestMsg;
            }
            set
            {
                this._posRequestMsg = value;
            }
        }

        public bool PosReqAck
        {
            get
            {
                return this._posReqAck;
            }
            set
            {
                this._posReqAck = value;
            }
        }

        public string ProtocolFile
        {
            get
            {
                return this._protocolFile;
            }
            set
            {
                this._protocolFile = value;
            }
        }

        public bool PushAidingAvailability
        {
            get
            {
                return this._pushAidingAvailability;
            }
            set
            {
                this._pushAidingAvailability = value;
            }
        }

        public int PushAidingDelay
        {
            get
            {
                return this._pushingAidingDelay;
            }
            set
            {
                this._pushingAidingDelay = value;
            }
        }

        public byte PushAidingMask
        {
            get
            {
                return this._pushAidingMask;
            }
            set
            {
                this._pushAidingMask = value;
            }
        }

        public string SF123MsgFromTTB
        {
            get
            {
                return this._sf123MsgFromTTB;
            }
            set
            {
                this._sf123MsgFromTTB = value;
            }
        }

        public string SF45DataSet0MsgFromTTB
        {
            get
            {
                return this._sf45DataSet0MsgFromTTB;
            }
            set
            {
                this._sf45DataSet0MsgFromTTB = value;
            }
        }

        public string SF45DataSet1MsgFromTTB
        {
            get
            {
                return this._sf45DataSet1MsgFromTTB;
            }
            set
            {
                this._sf45DataSet1MsgFromTTB = value;
            }
        }

        public string TimeAidingCfgMsgToTTB
        {
            get
            {
                return this._timeAidingCfgMsgToTTB;
            }
            set
            {
                this._timeAidingCfgMsgToTTB = value;
            }
        }

        public TimeTransParams TimeTransferCtrl
        {
            get
            {
                return this._timeTransferParams;
            }
            set
            {
                this._timeTransferParams = value;
            }
        }

        public string TimeTransferRequestMsgToTTB
        {
            get
            {
                return this._timeTransferRequestMsg;
            }
            set
            {
                this._timeTransferRequestMsg = value;
            }
        }

        public string TimeTransferRespMsg
        {
            get
            {
                return this._timeTransferRespMsg;
            }
            set
            {
                this._timeTransferRespMsg = value;
            }
        }

        public TTBTimeAidingCfgParams TTBTimeAidingParams
        {
            get
            {
                return this._ttbTimeAidingParams;
            }
            set
            {
                this._ttbTimeAidingParams = value;
            }
        }

        public string UTCModelMsgFromTTB
        {
            get
            {
                return this._utcModelMsgTTB;
            }
            set
            {
                this._utcModelMsgTTB = value;
            }
        }

        public class ApproxPosParams
        {
            private double _alt = 205.1;
            private double _distanceSkew;
            private double _estHorrErr = 30000.0;
            private double _estVertiErr = 100.0;
            private double _headingSkew;
            private double _lat = 37.000005;
            private double _lon = -120.999989;
            private bool _reject;
            private bool _reply;

            public double Alt
            {
                get
                {
                    return this._alt;
                }
                set
                {
                    this._alt = value;
                }
            }

            public double DistanceSkew
            {
                get
                {
                    return this._distanceSkew;
                }
                set
                {
                    this._distanceSkew = value;
                }
            }

            public double EstHorrErr
            {
                get
                {
                    return this._estHorrErr;
                }
                set
                {
                    this._estHorrErr = value;
                }
            }

            public double EstVertiErr
            {
                get
                {
                    return this._estVertiErr;
                }
                set
                {
                    this._estVertiErr = value;
                }
            }

            public double HeadingSkew
            {
                get
                {
                    return this._headingSkew;
                }
                set
                {
                    this._headingSkew = value;
                }
            }

            public double Lat
            {
                get
                {
                    return this._lat;
                }
                set
                {
                    this._lat = value;
                }
            }

            public double Lon
            {
                get
                {
                    return this._lon;
                }
                set
                {
                    this._lon = value;
                }
            }

            public bool Reject
            {
                get
                {
                    return this._reject;
                }
                set
                {
                    this._reject = value;
                }
            }

            public bool Reply
            {
                get
                {
                    return this._reply;
                }
                set
                {
                    this._reply = value;
                }
            }
        }

        public class autoReplyCfgParams
        {
            private bool _autoAid_AcqData;
            private bool _autoAid_AcqData_fromFile;
            private bool _autoAid_AcqData_fromTTB;
            private bool _autoAid_Alm;
            private bool _autoAid_Eph;
            private bool _autoAid_Eph_fromFile;
            private bool _autoAid_Eph_fromTTB;
            private bool _autoAid_ExtEph_fromFile;
            private bool _autoAid_NavBit;
            private bool _autoPosReq;
            private bool _autoReply;
            private bool _autoReplyApproxPos;
            private bool _autoReplyFreqTrans;
            private bool _autoReplyHWCfg;
            private bool _autoReplyTimeTrans;
            private bool _ignoreXO;
            private bool _useDOS_ForTimeAid;
            private bool _useTTB_ForFreqAid;
            private bool _useTTB_ForHwCfg;
            private bool _useTTB_ForTimeAid;

            public bool AutoAid_AcqData
            {
                get
                {
                    return this._autoAid_AcqData;
                }
                set
                {
                    this._autoAid_AcqData = value;
                }
            }

            public bool AutoAid_AcqData_fromFile
            {
                get
                {
                    return this._autoAid_AcqData_fromFile;
                }
                set
                {
                    this._autoAid_AcqData_fromFile = value;
                }
            }

            public bool AutoAid_AcqData_fromTTB
            {
                get
                {
                    return this._autoAid_AcqData_fromTTB;
                }
                set
                {
                    this._autoAid_AcqData_fromTTB = value;
                }
            }

            public bool AutoAid_Alm
            {
                get
                {
                    return this._autoAid_Alm;
                }
                set
                {
                    this._autoAid_Alm = value;
                }
            }

            public bool AutoAid_Eph
            {
                get
                {
                    return this._autoAid_Eph;
                }
                set
                {
                    this._autoAid_Eph = value;
                }
            }

            public bool AutoAid_Eph_fromFile
            {
                get
                {
                    return this._autoAid_Eph_fromFile;
                }
                set
                {
                    this._autoAid_Eph_fromFile = value;
                }
            }

            public bool AutoAid_Eph_fromTTB
            {
                get
                {
                    return this._autoAid_Eph_fromTTB;
                }
                set
                {
                    this._autoAid_Eph_fromTTB = value;
                }
            }

            public bool AutoAid_ExtEph_fromFile
            {
                get
                {
                    return this._autoAid_ExtEph_fromFile;
                }
                set
                {
                    this._autoAid_ExtEph_fromFile = value;
                }
            }

            public bool AutoAid_NavBit
            {
                get
                {
                    return this._autoAid_NavBit;
                }
                set
                {
                    this._autoAid_NavBit = value;
                }
            }

            public bool AutoPosReq
            {
                get
                {
                    return this._autoPosReq;
                }
                set
                {
                    this._autoPosReq = value;
                }
            }

            public bool AutoReply
            {
                get
                {
                    return this._autoReply;
                }
                set
                {
                    this._autoReply = value;
                }
            }

            public bool AutoReplyApproxPos
            {
                get
                {
                    return this._autoReplyApproxPos;
                }
                set
                {
                    this._autoReplyApproxPos = value;
                }
            }

            public bool AutoReplyFreqTrans
            {
                get
                {
                    return this._autoReplyFreqTrans;
                }
                set
                {
                    this._autoReplyFreqTrans = value;
                }
            }

            public bool AutoReplyHWCfg
            {
                get
                {
                    return this._autoReplyHWCfg;
                }
                set
                {
                    this._autoReplyHWCfg = value;
                }
            }

            public bool AutoReplyTimeTrans
            {
                get
                {
                    return this._autoReplyTimeTrans;
                }
                set
                {
                    this._autoReplyTimeTrans = value;
                }
            }

            public bool FreqAidingIgnoreXO
            {
                get
                {
                    return this._ignoreXO;
                }
                set
                {
                    this._ignoreXO = value;
                }
            }

            public bool UseDOS_ForTimeAid
            {
                get
                {
                    return this._useDOS_ForTimeAid;
                }
                set
                {
                    this._useDOS_ForTimeAid = value;
                }
            }

            public bool UseTTB_ForFreqAid
            {
                get
                {
                    return this._useTTB_ForFreqAid;
                }
                set
                {
                    this._useTTB_ForFreqAid = value;
                }
            }

            public bool UseTTB_ForHwCfg
            {
                get
                {
                    return this._useTTB_ForHwCfg;
                }
                set
                {
                    this._useTTB_ForHwCfg = value;
                }
            }

            public bool UseTTB_ForTimeAid
            {
                get
                {
                    return this._useTTB_ForTimeAid;
                }
                set
                {
                    this._useTTB_ForTimeAid = value;
                }
            }
        }

        public class FreqTransParams
        {
            private double _accuracy;
            private int _defaultFreqGuiIndex = 2;
            private double _extClkScewppm;
            private int _extRefClockGuiIndex;
            private double _freqAccFromRxGui;
            private double _freqAccUserSpecifiedGui;
            private int _freqAidingMethod = 1;
            private double _freqOffset;
            private double _freqOffsetFromRxGui;
            private int _freqOffsetScaleGuiIndex;
            private double _freqOffsetUserSpecifiedGui;
            private bool _includeNormFreq;
            private long _nomFreq;
            private byte _refClkInfo;
            private int _refClockOnOffGuiIndex;
            private int _refClockRequestGuiIndex;
            private bool _reject;
            private bool _reply;
            private short _scaledFreqOffset;
            private bool _slcReportFreqGui;
            private bool _specifiedFreqGui;
            private uint _timeTag;
            private int _useFreqAiding;

            public double Accuracy
            {
                get
                {
                    return this._accuracy;
                }
                set
                {
                    this._accuracy = value;
                }
            }

            public int DefaultFreqIndex
            {
                get
                {
                    return this._defaultFreqGuiIndex;
                }
                set
                {
                    this._defaultFreqGuiIndex = value;
                }
            }

            public double ExtClkSkewppm
            {
                get
                {
                    return this._extClkScewppm;
                }
                set
                {
                    this._extClkScewppm = value;
                }
            }

            public int ExtRefClockGuiIndex
            {
                get
                {
                    return this._extRefClockGuiIndex;
                }
                set
                {
                    this._extRefClockGuiIndex = value;
                }
            }

            public double FreqAccFromRxGui
            {
                get
                {
                    return this._freqAccFromRxGui;
                }
                set
                {
                    this._freqAccFromRxGui = value;
                }
            }

            public double FreqAccUserSpecifiedGui
            {
                get
                {
                    return this._freqAccUserSpecifiedGui;
                }
                set
                {
                    this._freqAccUserSpecifiedGui = value;
                }
            }

            public int FreqAidingMethod
            {
                get
                {
                    return this._freqAidingMethod;
                }
                set
                {
                    this._freqAidingMethod = value;
                }
            }

            public double FreqOffset
            {
                get
                {
                    return this._freqOffset;
                }
                set
                {
                    this._freqOffset = value;
                }
            }

            public double FreqOffsetFromRxGui
            {
                get
                {
                    return this._freqOffsetFromRxGui;
                }
                set
                {
                    this._freqOffsetFromRxGui = value;
                }
            }

            public double FreqOffsetUserSpecifiedGui
            {
                get
                {
                    return this._freqOffsetUserSpecifiedGui;
                }
                set
                {
                    this._freqOffsetUserSpecifiedGui = value;
                }
            }

            public bool IncludeNormFreq
            {
                get
                {
                    return this._includeNormFreq;
                }
                set
                {
                    this._includeNormFreq = value;
                }
            }

            public long NomFreq
            {
                get
                {
                    return this._nomFreq;
                }
                set
                {
                    this._nomFreq = value;
                }
            }

            public byte RefClkInfo
            {
                get
                {
                    return this._refClkInfo;
                }
                set
                {
                    this._refClkInfo = value;
                }
            }

            public int RefClockOnOffGuiIndex
            {
                get
                {
                    return this._refClockOnOffGuiIndex;
                }
                set
                {
                    this._refClockOnOffGuiIndex = value;
                }
            }

            public int RefClockRequestGuiIndex
            {
                get
                {
                    return this._refClockRequestGuiIndex;
                }
                set
                {
                    this._refClockRequestGuiIndex = value;
                }
            }

            public bool Reject
            {
                get
                {
                    return this._reject;
                }
                set
                {
                    this._reject = value;
                }
            }

            public bool Reply
            {
                get
                {
                    return this._reply;
                }
                set
                {
                    this._reply = value;
                }
            }

            public short ScaledFreqOffset
            {
                get
                {
                    return this._scaledFreqOffset;
                }
                set
                {
                    this._scaledFreqOffset = value;
                }
            }

            public int ScaledFreqOffsetGuiIndex
            {
                get
                {
                    return this._freqOffsetScaleGuiIndex;
                }
                set
                {
                    this._freqOffsetScaleGuiIndex = value;
                }
            }

            public bool SLCReportFreqGuiIndex
            {
                get
                {
                    return this._slcReportFreqGui;
                }
                set
                {
                    this._slcReportFreqGui = value;
                }
            }

            public bool SpecifiedRefFreq
            {
                get
                {
                    return this._specifiedFreqGui;
                }
                set
                {
                    this._specifiedFreqGui = value;
                }
            }

            public uint TimeTag
            {
                get
                {
                    return this._timeTag;
                }
                set
                {
                    this._timeTag = value;
                }
            }

            public int UseFreqAiding
            {
                get
                {
                    return this._useFreqAiding;
                }
                set
                {
                    this._useFreqAiding = value;
                }
            }
        }

        public class HWConfigParams
        {
            private byte _coarseTimeEnabled = 1;
            private byte _freqAidEnabled = 1;
            private byte _freqAidMethod = 1;
            private byte _networkEnhanceType;
            private long _norminalFreqHz = 0x124f800L;
            private byte _preciseTimeDirection = 1;
            private byte _preciseTimeEnabled;
            private byte _refClkEnabled;
            private bool _reply;
            private byte _RTCAvailabe;
            private byte _RTCSource;

            public byte CoarseTimeEnabled
            {
                get
                {
                    return this._coarseTimeEnabled;
                }
                set
                {
                    this._coarseTimeEnabled = value;
                }
            }

            public byte FreqAidEnabled
            {
                get
                {
                    return this._freqAidEnabled;
                }
                set
                {
                    this._freqAidEnabled = value;
                }
            }

            public byte FreqAidMethod
            {
                get
                {
                    return this._freqAidMethod;
                }
                set
                {
                    this._freqAidMethod = value;
                }
            }

            public byte NetworkEnhanceType
            {
                get
                {
                    return this._networkEnhanceType;
                }
                set
                {
                    this._networkEnhanceType = value;
                }
            }

            public long NorminalFreqHz
            {
                get
                {
                    return this._norminalFreqHz;
                }
                set
                {
                    this._norminalFreqHz = value;
                }
            }

            public byte PreciseTimeDirection
            {
                get
                {
                    return this._preciseTimeDirection;
                }
                set
                {
                    this._preciseTimeDirection = value;
                }
            }

            public byte PreciseTimeEnabled
            {
                get
                {
                    return this._preciseTimeEnabled;
                }
                set
                {
                    this._preciseTimeEnabled = value;
                }
            }

            public byte RefClkEnabled
            {
                get
                {
                    return this._refClkEnabled;
                }
                set
                {
                    this._refClkEnabled = value;
                }
            }

            public bool Reply
            {
                get
                {
                    return this._reply;
                }
                set
                {
                    this._reply = value;
                }
            }

            public byte RTCAvailabe
            {
                get
                {
                    return this._RTCAvailabe;
                }
                set
                {
                    this._RTCAvailabe = value;
                }
            }

            public byte RTCSource
            {
                get
                {
                    return this._RTCSource;
                }
                set
                {
                    this._RTCSource = value;
                }
            }
        }

        public class PositionRequestParams
        {
            private byte _acqAssistReply;
            private byte _acqAssistSource = 1;
            private byte _almReply;
            private byte _almSource = 1;
            private bool _autoSend;
            private byte _ephReply = 1;
            private byte _ephSource = 1;
            private byte _hErrMax = 40;
            private byte _locMethod = 1;
            private byte _navBitReply;
            private byte _navBitSource = 1;
            private byte _numFixes;
            private byte _respTimeMax;
            private byte _timeAccPriority;
            private byte _timeBtwFixes = 1;
            private byte _vErrMax = 7;

            public byte AcqAssistReply
            {
                get
                {
                    return this._acqAssistReply;
                }
                set
                {
                    this._acqAssistReply = value;
                }
            }

            public byte AcqAssistSource
            {
                get
                {
                    return this._acqAssistSource;
                }
                set
                {
                    this._acqAssistSource = value;
                }
            }

            public byte AlmReply
            {
                get
                {
                    return this._almReply;
                }
                set
                {
                    this._almReply = value;
                }
            }

            public byte AlmSource
            {
                get
                {
                    return this._almSource;
                }
                set
                {
                    this._almSource = value;
                }
            }

            public bool AutoSend
            {
                get
                {
                    return this._autoSend;
                }
                set
                {
                    this._autoSend = value;
                }
            }

            public byte EphReply
            {
                get
                {
                    return this._ephReply;
                }
                set
                {
                    this._ephReply = value;
                }
            }

            public byte EphSource
            {
                get
                {
                    return this._ephSource;
                }
                set
                {
                    this._ephSource = value;
                }
            }

            public byte HorrErrMax
            {
                get
                {
                    return this._hErrMax;
                }
                set
                {
                    this._hErrMax = value;
                }
            }

            public byte LocMethod
            {
                get
                {
                    return this._locMethod;
                }
                set
                {
                    this._locMethod = value;
                }
            }

            public byte NavBitReply
            {
                get
                {
                    return this._navBitReply;
                }
                set
                {
                    this._navBitReply = value;
                }
            }

            public byte NavBitSource
            {
                get
                {
                    return this._navBitSource;
                }
                set
                {
                    this._navBitSource = value;
                }
            }

            public byte NumFixes
            {
                get
                {
                    return this._numFixes;
                }
                set
                {
                    this._numFixes = value;
                }
            }

            public byte RespTimeMax
            {
                get
                {
                    return this._respTimeMax;
                }
                set
                {
                    this._respTimeMax = value;
                }
            }

            public byte TimeAccPriority
            {
                get
                {
                    return this._timeAccPriority;
                }
                set
                {
                    this._timeAccPriority = value;
                }
            }

            public byte TimeBtwFixes
            {
                get
                {
                    return this._timeBtwFixes;
                }
                set
                {
                    this._timeBtwFixes = value;
                }
            }

            public byte VertErrMax
            {
                get
                {
                    return this._vErrMax;
                }
                set
                {
                    this._vErrMax = value;
                }
            }
        }

        public class TimeTransParams
        {
            private double _accuracy;
            private bool _reject;
            private bool _reply;
            private double _skew;
            private ulong _timeOfWeek;
            private byte _ttType;
            private ushort _weekNum;

            public double Accuracy
            {
                get
                {
                    return this._accuracy;
                }
                set
                {
                    this._accuracy = value;
                }
            }

            public bool Reject
            {
                get
                {
                    return this._reject;
                }
                set
                {
                    this._reject = value;
                }
            }

            public bool Reply
            {
                get
                {
                    return this._reply;
                }
                set
                {
                    this._reply = value;
                }
            }

            public double Skew
            {
                get
                {
                    return this._skew;
                }
                set
                {
                    this._skew = value;
                }
            }

            public ulong TimeOfWeek
            {
                get
                {
                    return this._timeOfWeek;
                }
                set
                {
                    this._timeOfWeek = value;
                }
            }

            public byte TTType
            {
                get
                {
                    return this._ttType;
                }
                set
                {
                    this._ttType = value;
                }
            }

            public ushort WeekNum
            {
                get
                {
                    return this._weekNum;
                }
                set
                {
                    this._weekNum = value;
                }
            }
        }

        public class TTBTimeAidingCfgParams
        {
            private uint _accuracy = 1;
            private bool _enable;
            private uint _skew = 1;
            private byte _type = 1;

            public uint Accuracy
            {
                get
                {
                    return this._accuracy;
                }
                set
                {
                    this._accuracy = value;
                }
            }

            public bool Enable
            {
                get
                {
                    return this._enable;
                }
                set
                {
                    this._enable = value;
                }
            }

            public uint Skew
            {
                get
                {
                    return this._skew;
                }
                set
                {
                    this._skew = value;
                }
            }

            public byte Type
            {
                get
                {
                    return this._type;
                }
                set
                {
                    this._type = value;
                }
            }
        }
    }
}

