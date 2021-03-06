﻿namespace SiRFLive.MessageHandling
{
    using SiRFLive.Communication;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;

    public class AutoReplyMgr_OSP : AutoReplyMgr
    {
        internal const byte OSP_ID_APPROX_POS = 0xd7;
        internal const byte OSP_ID_APPROX_POS_REQUEST = 0x49;
        internal const byte OSP_ID_FREQ_TRANS = 0xd7;
        internal const byte OSP_ID_FREQ_TRANS_REQUEST = 0x49;
        internal const byte OSP_ID_HW_CONFIG = 0xd6;
        internal const byte OSP_ID_POS_REQUEST = 210;
        internal const byte OSP_ID_REJECT = 0xd8;
        internal const byte OSP_ID_SESSION_CLOSE = 0xd5;
        internal const byte OSP_ID_SESSION_OPEN = 0xd5;
        internal const byte OSP_ID_TIME_TRANS = 0xd7;
        internal const byte OSP_ID_TIME_TRANS_REQUEST = 0x49;
        internal const string OSP_PROTOCOL_NAME = "OSP";
        internal const byte OSP_SubID_APPROX_POS = 1;
        internal const byte OSP_SubID_APPROX_POS_REQUEST = 1;
        internal const byte OSP_SubID_FREQ_TRANS = 3;
        internal const byte OSP_SubID_FREQ_TRANS_REQUEST = 3;
        internal const byte OSP_SubID_REJECT = 2;
        internal const byte OSP_SubID_SESSION_CLOSE = 2;
        internal const byte OSP_SubID_SESSION_OPEN = 1;
        internal const byte OSP_SubID_TIME_TRANS = 2;
        internal const byte OSP_SubID_TIME_TRANS_REQUEST = 2;
        public double ref_altitude;
        public double ref_horizontalErr;
        public double ref_latitude;
        public double ref_longitude;
        public double ref_verticalErr;

        public AutoReplyMgr_OSP()
        {
            base.ProtocolFile = @"..\Protocols\Protocols_F.xml";
            base.ControlChannelVersion = "1.0";
        }

        public AutoReplyMgr_OSP(string xmlFile)
        {
            base.ProtocolFile = xmlFile;
            base.ControlChannelVersion = "1.0";
        }

        public override string AcqAssistFromTTBRespMsg(string ttbAcqAssistData)
        {
            string str = string.Empty;
            string[] separator = new string[] { "\r\n" };
            if (!(ttbAcqAssistData != ""))
            {
                return str;
            }
            string[] strArray2 = ttbAcqAssistData.Split(separator, StringSplitOptions.None);
            byte length = (byte) strArray2.GetLength(0);
            string message = "D304";
            string str3 = string.Empty;
            string str4 = string.Empty;
            foreach (string str5 in strArray2)
            {
                if ((str5 != null) && !(str5 == string.Empty))
                {
                    string str6 = str5.Replace(" ", "");
                    str4 = str6.Substring(0x12, 8);
                    int num2 = str6.Length;
                    str3 = str3 + str6.Substring(0x1a, num2 - 0x22);
                }
            }
            message = message + str4 + length.ToString("X").PadLeft(2, '0') + str3;
            int num3 = message.Length / 2;
            return ("A0A2" + num3.ToString("X").PadLeft(4, '0') + message + utils_AutoReply.GetChecksum(message, false) + "B0B3");
        }

        public override void AutoReplyApproxPositionResp()
        {
            base.ApproxPosRespMsg = this.OSPApproxPositionDataToHex(base.ControlChannelVersion, base.ApproxPositionCtrl.Reject, base.ApproxPositionCtrl.Lat, base.ApproxPositionCtrl.Lon, base.ApproxPositionCtrl.Alt, base.ApproxPositionCtrl.DistanceSkew, base.ApproxPositionCtrl.HeadingSkew, base.ApproxPositionCtrl.EstHorrErr, base.ApproxPositionCtrl.EstVertiErr);
        }

        public override string AutoReplyFreqTransferResp()
        {
            base.FreqTransferRespMsg = this.OSPFreqTransRespDataToHex(base.ControlChannelVersion, base.FreqTransferCtrl.UseFreqAiding, base.FreqTransferCtrl.TimeTag, base.FreqTransferCtrl.RefClkInfo, base.FreqTransferCtrl.Accuracy, base.FreqTransferCtrl.ScaledFreqOffset, base.FreqTransferCtrl.ExtClkSkewppm, base.FreqTransferCtrl.NomFreq, base.FreqTransferCtrl.IncludeNormFreq);
            return base.FreqTransferRespMsg;
        }

        public override void AutoReplyHWCfgResp()
        {
            if (base.HWCfgCtrl.Reply)
            {
                base.HWCfgRespMsg = this.OSPHWConfigRespDataToHex(base.ControlChannelVersion, base.HWCfgCtrl.PreciseTimeEnabled, base.HWCfgCtrl.PreciseTimeDirection, base.HWCfgCtrl.FreqAidEnabled, base.HWCfgCtrl.FreqAidMethod, base.HWCfgCtrl.RTCAvailabe, base.HWCfgCtrl.RTCSource, base.HWCfgCtrl.CoarseTimeEnabled, base.HWCfgCtrl.RefClkEnabled, base.HWCfgCtrl.NorminalFreqHz, base.HWCfgCtrl.NetworkEnhanceType);
                string message = "0210" + base.HWCfgRespMsg.Substring(10, 12);
                base.HwCfgRespMsgToTTB = "A0A20008" + message + utils_AutoReply.GetChecksum(message, true) + "B0B3";
            }
        }

        public override void AutoReplyTimeTransferResp()
        {
            base.TimeTransferRespMsg = this.OSPTimeTransferRespDataToHex(base.ControlChannelVersion, base.TimeTransferCtrl.Reject, base.TimeTransferCtrl.TTType, base.TimeTransferCtrl.WeekNum, base.TimeTransferCtrl.TimeOfWeek, base.TimeTransferCtrl.Accuracy);
        }

        public override string AutoSendPositionRequestMsg()
        {
            ArrayList fieldList = new ArrayList();
            fieldList = utils_AutoReply.GetMessageStructure(base.ProtocolFile, CommunicationManager.ReceiverType.OSP, 210, 0, "OSP", base.ControlChannelVersion);
            for (int i = 0; i < fieldList.Count; i++)
            {
                SLCMsgStructure structure = (SLCMsgStructure) fieldList[i];
                switch (structure.fieldName)
                {
                    case "NUM_FIXES":
                        structure.defaultValue = base.PositionRequestCtrl.NumFixes.ToString();
                        break;

                    case "TIME_BTW_FIXES":
                        structure.defaultValue = base.PositionRequestCtrl.TimeBtwFixes.ToString();
                        break;

                    case "HORI_ERROR_MAX":
                        structure.defaultValue = base.PositionRequestCtrl.HorrErrMax.ToString();
                        break;

                    case "VERT_ERROR_MAX":
                        structure.defaultValue = base.PositionRequestCtrl.VertErrMax.ToString();
                        break;

                    case "RESP_TIME_MAX":
                        structure.defaultValue = base.PositionRequestCtrl.RespTimeMax.ToString();
                        break;

                    case "TIME_ACC_PRIORITY":
                        structure.defaultValue = base.PositionRequestCtrl.TimeAccPriority.ToString();
                        break;

                    case "LOCATION_METHOD":
                        structure.defaultValue = base.PositionRequestCtrl.LocMethod.ToString();
                        break;
                }
                fieldList[i] = structure;
            }
            base.PositionRequestMsg = utils_AutoReply.FieldList_to_HexString(false, fieldList, 0);
            fieldList.Clear();
            return base.PositionRequestMsg;
        }

        public override string EphFromTTBRespMsg(string ttbEphData)
        {
            string str = string.Empty;
            string[] separator = new string[] { "\r\n" };
            if (!(ttbEphData != ""))
            {
                return str;
            }
            string[] strArray2 = ttbEphData.Split(separator, StringSplitOptions.None);
            byte length = (byte) strArray2.GetLength(0);
            string message = "D302" + length.ToString("X").PadLeft(2, '0');
            foreach (string str3 in strArray2)
            {
                if ((str3 != null) && !(str3 == string.Empty))
                {
                    string str4 = str3.Replace(" ", "");
                    int num2 = str4.Length;
                    message = message + str4.Substring(0x10, num2 - 0x18);
                }
            }
            int num3 = message.Length / 2;
            return ("A0A2" + num3.ToString("X").PadLeft(4, '0') + message + utils_AutoReply.GetChecksum(message, false) + "B0B3");
        }

        public override string Get2HoursEphAidingMsgFromFile(string version, string ephFilePath)
        {
            return AutoReplyMgr.BuildOSPEphAidingMsg(utils_AutoReply.get2HoursEphFromFile(ephFilePath), version);
        }

        public override string GetAcqAssistMsgFromFile(string version, string acqAssistFilePath, double gpsTowNow)
        {
            int num = 10;
            string inputMsgFile = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\protocols\Protocols_F.xml";
            ArrayList list = new ArrayList();
            list = utils_AutoReply.GetMessageStructure(inputMsgFile, CommunicationManager.ReceiverType.OSP, 0xd3, 4, "OSP", version);
            ArrayList fieldList = new ArrayList();
            string str2 = utils_AutoReply.getAcqAssistDataFromFile(acqAssistFilePath, gpsTowNow);
            if (str2 == "")
            {
                return "";
            }
            string[] strArray = str2.Split(new char[] { ',' });
            double num2 = Convert.ToDouble(strArray[0]);
            int num3 = 0;
            while ((num3 < list.Count) && (((SLCMsgStructure) list[num3]).fieldName != "REFERENCE_TIME"))
            {
                num3++;
            }
            SLCMsgStructure structure = (SLCMsgStructure) list[num3];
            structure.defaultValue = num2.ToString();
            list[num3] = structure;
            int num4 = (strArray.Length - 1) / num;
            num3 = 0;
            while ((num3 < list.Count) && (((SLCMsgStructure) list[num3]).fieldName != "NUM_SVS"))
            {
                num3++;
            }
            structure = (SLCMsgStructure) list[num3];
            structure.defaultValue = num4.ToString();
            list[num3] = structure;
            string[] strArray2 = new string[num4 * 11];
            int num5 = 0;
            for (int i = 1; num5 < (num4 * 11); i += num)
            {
                strArray2[num5++] = "1";
                for (int k = 0; k < num; k++)
                {
                    double num8 = Convert.ToDouble(strArray[k + i]);
                    switch (k)
                    {
                        case 2:
                            if (num8 == 0.0)
                            {
                                strArray[k + i] = "-1.015265";
                            }
                            break;

                        case 3:
                            if (num8 >= 200.0)
                            {
                                strArray[k + i] = "0";
                            }
                            else if (num8 >= 100.0)
                            {
                                strArray[k + i] = "1";
                            }
                            else if (num8 >= 50.0)
                            {
                                strArray[k + i] = "2";
                            }
                            else if (num8 >= 25.0)
                            {
                                strArray[k + i] = "3";
                            }
                            else if (num8 >= 12.5)
                            {
                                strArray[k + i] = "4";
                            }
                            else if (num8 > 0.0)
                            {
                                strArray[k + i] = "0";
                            }
                            else
                            {
                                strArray[k + i] = "255";
                            }
                            break;

                        default:
                            if ((k == 4) && (num8 != 0.0))
                            {
                                strArray[k + i] = ((int) (1023.0 - num8)).ToString();
                            }
                            break;
                    }
                    strArray2[num5++] = strArray[k + i];
                }
            }
            num3 = 0;
            while ((num3 < list.Count) && (((SLCMsgStructure) list[num3]).fieldName != "1st ACQ_ASSIST_VALID_FLAG"))
            {
                num3++;
            }
            int num10 = num3;
            for (int j = 0; num10 < (num3 + strArray2.Length); j++)
            {
                structure = (SLCMsgStructure) list[num10];
                structure.defaultValue = strArray2[j];
                list[num10] = structure;
                num10++;
            }
            for (num3 = 0; num3 < (strArray2.Length + 4); num3++)
            {
                fieldList.Add(list[num3]);
            }
            return utils_AutoReply.FieldList_to_HexString(false, fieldList, 0);
        }

        public override string GetEphAidingMsgFromFile(string version, string ephFilePath, string gpsTimeStr)
        {
            return AutoReplyMgr.BuildOSPEphAidingMsg(utils_AutoReply.getEphFromFile(ephFilePath, gpsTimeStr), version);
        }

        public string OSPApproxPositionDataToHex(string osp_ICD, bool reject, double latSkew, double lonSkew)
        {
            string str;
            ArrayList fieldList = new ArrayList();
            if (reject)
            {
                str = this.OSPReject(osp_ICD, 0x49, 1, 4);
            }
            else
            {
                Convert.ToDouble(osp_ICD);
                fieldList = utils_AutoReply.GetMessageStructure(base.ProtocolFile, CommunicationManager.ReceiverType.OSP, 0xd7, 1, "OSP", osp_ICD);
                int num9 = 0;
                while ((num9 < fieldList.Count) && (((SLCMsgStructure) fieldList[num9]).fieldName != "LAT"))
                {
                    num9++;
                }
                this.ref_latitude = Convert.ToDouble(((SLCMsgStructure) fieldList[num9]).defaultValue);
                double num = this.ref_latitude + latSkew;
                ulong num4 = (ulong) ((num * 4294967296) / 180.0);
                SLCMsgStructure structure = (SLCMsgStructure) fieldList[num9];
                structure.defaultValue = num4.ToString();
                fieldList[num9] = structure;
                num9 = 0;
                while ((num9 < fieldList.Count) && (((SLCMsgStructure) fieldList[num9]).fieldName != "LON"))
                {
                    num9++;
                }
                this.ref_longitude = Convert.ToDouble(((SLCMsgStructure) fieldList[num9]).defaultValue);
                double num2 = this.ref_longitude + lonSkew;
                ulong num5 = (ulong) (((num2 * 4294967296) / 360.0) + 4294967296);
                structure = (SLCMsgStructure) fieldList[num9];
                structure.defaultValue = num5.ToString();
                fieldList[num9] = structure;
                num9 = 0;
                while ((num9 < fieldList.Count) && (((SLCMsgStructure) fieldList[num9]).fieldName != "ALT"))
                {
                    num9++;
                }
                this.ref_altitude = Convert.ToDouble(((SLCMsgStructure) fieldList[num9]).defaultValue);
                ushort num6 = (ushort) ((this.ref_altitude + 500.0) / 0.1);
                structure = (SLCMsgStructure) fieldList[num9];
                structure.defaultValue = num6.ToString();
                fieldList[num9] = structure;
                num9 = 0;
                while ((num9 < fieldList.Count) && (((SLCMsgStructure) fieldList[num9]).fieldName != "EST_HOR_ER"))
                {
                    num9++;
                }
                this.ref_horizontalErr = Convert.ToDouble(((SLCMsgStructure) fieldList[num9]).defaultValue);
                byte num7 = utils_AutoReply.metersToICDHorzErr(this.ref_horizontalErr);
                structure = (SLCMsgStructure) fieldList[num9];
                structure.defaultValue = num7.ToString();
                fieldList[num9] = structure;
                num9 = 0;
                while ((num9 < fieldList.Count) && (((SLCMsgStructure) fieldList[num9]).fieldName != "EST_VER_ER"))
                {
                    num9++;
                }
                this.ref_verticalErr = Convert.ToDouble(((SLCMsgStructure) fieldList[num9]).defaultValue);
                ushort num8 = (ushort) (10.0 * this.ref_verticalErr);
                structure = (SLCMsgStructure) fieldList[num9];
                structure.defaultValue = num8.ToString();
                fieldList[num9] = structure;
                num9 = 0;
                while ((num9 < fieldList.Count) && (((SLCMsgStructure) fieldList[num9]).fieldName != "USE_ALT_AIDING"))
                {
                    num9++;
                }
                structure = (SLCMsgStructure) fieldList[num9];
                structure.defaultValue = (num8 > 0) ? "1" : "0";
                fieldList[num9] = structure;
                str = utils_AutoReply.FieldList_to_HexString(false, fieldList, 0);
            }
            fieldList.Clear();
            return str;
        }

        public string OSPApproxPositionDataToHex(string osp_ICD, bool reject, double latitude, double longitude, double altitude, double distanceSkew, double headingSkew, double horr_Err, double vert_Err)
        {
            string str;
            ArrayList fieldList = new ArrayList();
            if (reject)
            {
                str = this.OSPReject(osp_ICD, 0x49, 1, 4);
            }
            else
            {
                ulong num4;
                ulong num5;
                Convert.ToDouble(osp_ICD);
                List<double> list2 = utils_AutoReply.skewLatLon(latitude, longitude, distanceSkew, headingSkew);
                fieldList = utils_AutoReply.GetMessageStructure(base.ProtocolFile, CommunicationManager.ReceiverType.OSP, 0xd7, 1, "OSP", osp_ICD);
                int num9 = 0;
                while ((num9 < fieldList.Count) && (((SLCMsgStructure) fieldList[num9]).fieldName != "LAT"))
                {
                    num9++;
                }
                double num = list2[0];
                if (num >= 0.0)
                {
                    num4 = (ulong) ((num * 4294967296) / 180.0);
                }
                else
                {
                    num4 = (ulong) (((num * 4294967296) / 180.0) + 4294967296);
                }
                SLCMsgStructure structure = (SLCMsgStructure) fieldList[num9];
                structure.defaultValue = num4.ToString();
                fieldList[num9] = structure;
                num9 = 0;
                while ((num9 < fieldList.Count) && (((SLCMsgStructure) fieldList[num9]).fieldName != "LON"))
                {
                    num9++;
                }
                double num2 = list2[1];
                if (num2 >= 0.0)
                {
                    num5 = (ulong) ((num2 * 4294967296) / 360.0);
                }
                else
                {
                    num5 = (ulong) (((num2 * 4294967296) / 360.0) + 4294967296);
                }
                structure = (SLCMsgStructure) fieldList[num9];
                structure.defaultValue = num5.ToString();
                fieldList[num9] = structure;
                num9 = 0;
                while ((num9 < fieldList.Count) && (((SLCMsgStructure) fieldList[num9]).fieldName != "ALT"))
                {
                    num9++;
                }
                double num3 = altitude;
                ushort num6 = (ushort) ((num3 + 500.0) / 0.1);
                structure = (SLCMsgStructure) fieldList[num9];
                structure.defaultValue = num6.ToString();
                fieldList[num9] = structure;
                num9 = 0;
                while ((num9 < fieldList.Count) && (((SLCMsgStructure) fieldList[num9]).fieldName != "EST_HOR_ER"))
                {
                    num9++;
                }
                byte num7 = utils_AutoReply.metersToICDHorzErr(horr_Err);
                structure = (SLCMsgStructure) fieldList[num9];
                structure.defaultValue = num7.ToString();
                fieldList[num9] = structure;
                num9 = 0;
                while ((num9 < fieldList.Count) && (((SLCMsgStructure) fieldList[num9]).fieldName != "EST_VER_ER"))
                {
                    num9++;
                }
                ushort num8 = (ushort) (10.0 * vert_Err);
                structure = (SLCMsgStructure) fieldList[num9];
                structure.defaultValue = num8.ToString();
                fieldList[num9] = structure;
                num9 = 0;
                while ((num9 < fieldList.Count) && (((SLCMsgStructure) fieldList[num9]).fieldName != "USE_ALT_AIDING"))
                {
                    num9++;
                }
                structure = (SLCMsgStructure) fieldList[num9];
                structure.defaultValue = (num8 > 0) ? "1" : "0";
                fieldList[num9] = structure;
                str = utils_AutoReply.FieldList_to_HexString(false, fieldList, 0);
            }
            fieldList.Clear();
            return str;
        }

        public string OSPFreqTransRespDataToHex(string osp_ICD, int useFreqAiding, uint timeTag, int refClkInfo, double fAccuracy, short scaledFreqOffset, double fEclkScewppm, long nomFreq, bool includeNormFreq)
        {
            string str;
            ArrayList fieldList = new ArrayList();
            if (useFreqAiding == 2)
            {
                str = this.OSPReject(osp_ICD, 0x49, 3, 4);
            }
            else
            {
                fieldList = utils_AutoReply.GetMessageStructure(base.ProtocolFile, CommunicationManager.ReceiverType.OSP, 0xd7, 3, "OSP", osp_ICD);
                int num = 0;
                while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "SCALED_FREQ_OFFSET"))
                {
                    num++;
                }
                SLCMsgStructure structure = (SLCMsgStructure) fieldList[num];
                structure.defaultValue = scaledFreqOffset.ToString();
                fieldList[num] = structure;
                num = 0;
                while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "REL_FREQ_ACC"))
                {
                    num++;
                }
                byte num2 = utils_AutoReply.get_REL_FREQ_ACC(fAccuracy);
                structure = (SLCMsgStructure) fieldList[num];
                structure.defaultValue = num2.ToString();
                fieldList[num] = structure;
                num = 0;
                while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "TIME_TAG"))
                {
                    num++;
                }
                structure = (SLCMsgStructure) fieldList[num];
                structure.defaultValue = timeTag.ToString();
                fieldList[num] = structure;
                num = 0;
                while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "REF_CLOCK_INFO"))
                {
                    num++;
                }
                structure = (SLCMsgStructure) fieldList[num];
                structure.defaultValue = ((byte) refClkInfo).ToString();
                fieldList[num] = structure;
                double num4 = nomFreq * (1.0 + (fEclkScewppm * 1E-06));
                ulong num3 = (ulong) (num4 * 1000.0);
                ulong num5 = num3 & ((ulong) 0xffffffffffL);
                num = 0;
                while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "NOMINAL_FREQ"))
                {
                    num++;
                }
                structure = (SLCMsgStructure) fieldList[num];
                structure.defaultValue = num5.ToString();
                fieldList[num] = structure;
                if (!includeNormFreq)
                {
                    fieldList.RemoveAt(fieldList.Count - 1);
                }
                str = utils_AutoReply.FieldList_to_HexString(false, fieldList, 0);
            }
            fieldList.Clear();
            return str;
        }

        public string OSPHWConfigRespDataToHex(string osp_ICD, byte precTimeEnabled, byte precTimeDir, byte freqAidEnabled, byte freqAidMethod, byte rtcAvailble, byte rtcSrc, byte coarseTimeEnabled, byte refClkEnabled, long normFreq, byte networkEnhanceType)
        {
            ArrayList fieldList = new ArrayList();
            fieldList = utils_AutoReply.GetMessageStructure(base.ProtocolFile, CommunicationManager.ReceiverType.OSP, 0xd6, 0, "OSP", osp_ICD);
            int num = 0;
            while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "HW_CONFIG"))
            {
                num++;
            }
            byte num2 = (byte) (((((((precTimeEnabled | (precTimeDir << 1)) | (freqAidEnabled << 2)) | (freqAidMethod << 3)) | (rtcAvailble << 4)) | (rtcSrc << 5)) | (coarseTimeEnabled << 6)) | (refClkEnabled << 7));
            SLCMsgStructure structure = (SLCMsgStructure) fieldList[num];
            structure.defaultValue = num2.ToString();
            fieldList[num] = structure;
            num = 0;
            while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "NOMINAL_FREQ"))
            {
                num++;
            }
            structure = (SLCMsgStructure) fieldList[num];
            if ((freqAidEnabled == 1) && (freqAidMethod == 0))
            {
                long num3 = normFreq * 0x3e8L;
                structure.defaultValue = (num3 & 0xffffffffffL).ToString();
            }
            else
            {
                structure.defaultValue = "0000000000";
            }
            fieldList[num] = structure;
            num = 0;
            while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "NW_ENHANCE_TYPE"))
            {
                num++;
            }
            structure = (SLCMsgStructure) fieldList[num];
            structure.defaultValue = networkEnhanceType.ToString();
            fieldList[num] = structure;
            string str = utils_AutoReply.FieldList_to_HexString(false, fieldList, 0);
            fieldList.Clear();
            return str;
        }

        public string OSPReject(string osp_ICD, byte msgID, byte msgSubID, byte reason)
        {
            ArrayList fieldList = new ArrayList();
            fieldList = utils_AutoReply.GetMessageStructure(base.ProtocolFile, CommunicationManager.ReceiverType.OSP, 0xd8, 2, "OSP", osp_ICD);
            int num = 0;
            while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "REJ_MESS_ID"))
            {
                num++;
            }
            SLCMsgStructure structure = (SLCMsgStructure) fieldList[num];
            structure.defaultValue = msgID.ToString();
            fieldList[num] = structure;
            num = 0;
            while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "REJ_MESS_SUB_ID"))
            {
                num++;
            }
            structure = (SLCMsgStructure) fieldList[num];
            structure.defaultValue = msgSubID.ToString();
            fieldList[num] = structure;
            num = 0;
            while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "REJ_REASON"))
            {
                num++;
            }
            structure = (SLCMsgStructure) fieldList[num];
            structure.defaultValue = reason.ToString();
            fieldList[num] = structure;
            string str = utils_AutoReply.FieldList_to_HexString(false, fieldList, 0);
            fieldList.Clear();
            return str;
        }

        public string OSPTimeTransferRespDataToHex(string osp_ICD, bool reject, byte ttType, ushort wkNum, ulong TOW, double acc)
        {
            string str;
            ArrayList fieldList = new ArrayList();
            if (reject)
            {
                str = this.OSPReject(osp_ICD, 0x49, 2, 4);
            }
            else
            {
                fieldList = utils_AutoReply.GetMessageStructure(base.ProtocolFile, CommunicationManager.ReceiverType.OSP, 0xd7, 2, "OSP", osp_ICD);
                int num = 0;
                while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "TT_TYPE"))
                {
                    num++;
                }
                SLCMsgStructure structure = (SLCMsgStructure) fieldList[num];
                structure.defaultValue = ttType.ToString();
                fieldList[num] = structure;
                num = 0;
                while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "GPS_WEEK_NUM"))
                {
                    num++;
                }
                structure = (SLCMsgStructure) fieldList[num];
                structure.defaultValue = wkNum.ToString();
                fieldList[num] = structure;
                num = 0;
                while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "GPS_TIME"))
                {
                    num++;
                }
                ulong num2 = TOW * ((ulong) 0xf4240L);
                num2 &= (ulong) 0xffffffffffL;
                structure = (SLCMsgStructure) fieldList[num];
                structure.defaultValue = num2.ToString();
                fieldList[num] = structure;
                num = 0;
                while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "DELTAT_UTC"))
                {
                    num++;
                }
                uint num3 = 0x32c8;
                structure = (SLCMsgStructure) fieldList[num];
                structure.defaultValue = num3.ToString();
                fieldList[num] = structure;
                num = 0;
                while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "TIME_ACCURACY"))
                {
                    num++;
                }
                byte num4 = utils_AutoReply.EncodeTimeAccuracy(acc, ttType);
                structure = (SLCMsgStructure) fieldList[num];
                structure.defaultValue = num4.ToString();
                fieldList[num] = structure;
                str = utils_AutoReply.FieldList_to_HexString(false, fieldList, 0);
            }
            fieldList.Clear();
            return str;
        }

        public override string SetupAlmfromTTB(string fullMsg)
        {
            string str = string.Empty;
            if (fullMsg != "")
            {
                int length = fullMsg.Length;
                string message = ("D303" + fullMsg.Substring(12, 2) + "20") + fullMsg.Substring(0x10, length - 0x18);
                int num2 = message.Length / 2;
                str = "A0A2" + num2.ToString("X").PadLeft(4, '0') + message + utils_AutoReply.GetChecksum(message, false) + "B0B3";
            }
            return str;
        }

        public override string SetupAuxNavMsgFromTTB(string fullMsg)
        {
            string str = string.Empty;
            if (fullMsg != "")
            {
                string str2 = fullMsg.Replace(" ", "");
                int length = str2.Length;
                int num = 0x4c0;
                int num2 = 0x1c;
                int num3 = 0x100;
                int startIndex = 14;
                int num5 = (startIndex + num) + 2;
                int num6 = (num5 + num2) + 2;
                int num7 = 0x20;
                string message = "D308" + num7.ToString("X").PadLeft(2, '0') + str2.Substring(startIndex, num);
                int num8 = message.Length / 2;
                base.AuxNavMsgFromTTB = "A0A2" + num8.ToString("X").PadLeft(4, '0') + message + utils_AutoReply.GetChecksum(message, false) + "B0B3";
                string str4 = "D306" + str2.Substring(num5, num2);
                num8 = str4.Length / 2;
                base.UTCModelMsgFromTTB = "A0A2" + num8.ToString("X").PadLeft(4, '0') + str4 + utils_AutoReply.GetChecksum(str4, false) + "B0B3";
                string str5 = "D307" + num7.ToString("X").PadLeft(2, '0') + str2.Substring(num6, num3);
                num8 = str5.Length / 2;
                base.GPSTOWAssistFromTTB = "A0A2" + num8.ToString("X").PadLeft(4, '0') + str5 + utils_AutoReply.GetChecksum(str5, false) + "B0B3";
            }
            return str;
        }

        public override string SetupNavSF123fromTTB(string fullMsg)
        {
            string str = string.Empty;
            if (fullMsg != "")
            {
                string str2 = fullMsg.Replace(" ", "");
                int length = str2.Length;
                string message = "D704" + str2.Substring(14, length - 0x16);
                int num2 = message.Length / 2;
                str = "A0A2" + num2.ToString("X").PadLeft(4, '0') + message + utils_AutoReply.GetChecksum(message, false) + "B0B3";
            }
            return str;
        }

        public override string SetupNavSF45FromTTB(string fullMsg)
        {
            string str = string.Empty;
            if (fullMsg != "")
            {
                string str2 = fullMsg.Replace(" ", "");
                int length = str2.Length;
                string message = "D705" + str2.Substring(14, length - 0x16);
                int num2 = message.Length / 2;
                str = "A0A2" + num2.ToString("X").PadLeft(4, '0') + message + utils_AutoReply.GetChecksum(message, false) + "B0B3";
                if (MsgFactory.ConvertHexToDecimal(str2.Substring(14, 8), "UINT32", 1.0) == "0")
                {
                    str = "Invalid: " + str;
                }
            }
            return str;
        }

        public override string TranslateOSPFreqTransferRequestMsgToTTB(string hexString)
        {
            if (hexString.Length < 20)
            {
                return string.Empty;
            }
            string str = hexString.Replace(" ", "");
            string message = "0212" + str.Substring(12, 2);
            return ("A0A20003" + message + utils_AutoReply.GetChecksum(message, true) + "B0B3");
        }

        public override string TranslateSLCFreqTransRespMsgToTTB(string hexString)
        {
            if (hexString.Length < 20)
            {
                return string.Empty;
            }
            string message = "0212" + hexString.Substring(12, hexString.Length - 20);
            string str2 = hexString.Substring(4, 4);
            return ("A0A2" + str2 + message + utils_AutoReply.GetChecksum(message, true) + "B0B3");
        }

        public override string TranslateTTBFreqTransferRespMsgToSLC(string hexString)
        {
            string message = string.Empty;
            string str2 = string.Empty;
            if (hexString.Length < 0x24)
            {
                return string.Empty;
            }
            if ((byte.Parse(hexString.Substring(0x1a, 2), NumberStyles.HexNumber) & 8) == 8)
            {
                message = "D703" + hexString.Substring(12, hexString.Length - 20);
                str2 = hexString.Substring(4, 4);
            }
            else
            {
                if (hexString.Length > 40)
                {
                    message = "D703" + hexString.Substring(12, hexString.Length - 30);
                }
                else
                {
                    message = "D703" + hexString.Substring(12, hexString.Length - 20);
                }
                int num2 = message.Length / 2;
                str2 = num2.ToString("X").PadLeft(4, '0');
            }
            return ("A0A2" + str2 + message + utils_AutoReply.GetChecksum(message, false) + "B0B3");
        }

        public override string TranslateTTBTimeTransferRespSLC(string hexString)
        {
            if (hexString.Length < 12)
            {
                return string.Empty;
            }
            string message = "D702" + hexString.Substring(12, hexString.Length - 20);
            string str2 = hexString.Substring(4, 4);
            return ("A0A2" + str2 + message + utils_AutoReply.GetChecksum(message, false) + "B0B3");
        }
    }
}

