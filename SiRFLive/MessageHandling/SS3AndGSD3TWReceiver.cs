﻿namespace SiRFLive.MessageHandling
{
    using SiRFLive.Analysis;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;

    public class SS3AndGSD3TWReceiver : Receiver
    {
        private F_and_AI3_Mgr _F_AI3 = new F_and_AI3_Mgr(ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Protocols\Protocols_AI3_Request.xml");
        private Ack_Responses ack_response_flag;
        private bool isDisposed;
        private List<ProtocolTestResults> ProtocolTestResultsList = new List<ProtocolTestResults>();
        private StreamWriter prtclTSw;
        private bool system_crash_flag;

        public SS3AndGSD3TWReceiver()
        {
            base._messageProtocol = "SSB";
        }

        private static void AddListOfUnTestedCommandsToReport(StreamWriter Sw, string prtcl)
        {
            if (prtcl == "SSB")
            {
                Sw.WriteLine("128,0,Receiver Init, untested");
                Sw.WriteLine("137,0,DOP Mask Control, untested");
                Sw.WriteLine("148,0,Flash Update, untested");
                Sw.WriteLine("172,1,Initialize GPS/DR Navigation mode, untested");
                Sw.WriteLine("172,4,Set DR Sensors Parameters, untested");
                Sw.WriteLine("205,0,Software Control, untested");
                Sw.WriteLine("232,2,Format, untested");
                Sw.WriteLine("232,255,Extended Ephemeris Debug, untested");
            }
            else if (prtcl == "OSP")
            {
                Sw.WriteLine("128,0,Receiver Init, untested");
                Sw.WriteLine("209,0,Query Request, untested");
                Sw.WriteLine("211,2,Set Satellite Ephemeris and Clock Correction, untested");
                Sw.WriteLine("211,3,Set Almanac Assist Data, untested");
                Sw.WriteLine("211,4,Set Acquisition Assistance Data, untested");
                Sw.WriteLine("211,5,Set real-Time Interity, untested");
                Sw.WriteLine("211,7,OSP Revision Request, untested");
                Sw.WriteLine("211,8,Set Auxiliary Navigation Model Parameters, untested");
                Sw.WriteLine("212,1,Ephemeris Status Request, untested");
                Sw.WriteLine("212,2,Almanac Request, untested");
                Sw.WriteLine("212,3,Broadcast Ephemeris Request, untested");
                Sw.WriteLine("215,4,Nav Subframe 1_2_3 Aiding Response Message -TBD, untested");
                Sw.WriteLine("215,5,Nav Subframe 4-5 Aiding Response Message -TBD, untested");
                Sw.WriteLine("218,8,Serial Port Setting Request, untested");
                Sw.WriteLine("220,4,CW Controller Input_CUSTOM_MON_CONFIG, untested");
                Sw.WriteLine("232,1,Test Mode Config Request - SSB_EE_SEA_PROVIDE_EPH, untested");
                Sw.WriteLine("232,2,Test Mode Config Request - SSB_EE_POLL_STATE, untested");
                Sw.WriteLine("232,16,Test Mode Config Request - SSB_EE_FILE_DOWNLOAD, untested");
                Sw.WriteLine("232,17,est Mode Config Request - SSB_EE_QUERY_AGE, untested");
                Sw.WriteLine("232,18,Test Mode Config Request - SSB_EE_FILE_PART, untested");
                Sw.WriteLine("232,19,Test Mode Config Request - SSB_EE_DOWNLOAD_TCP, untested");
                Sw.WriteLine("232,20,Test Mode Config Request - SSB_EE_SET_EPHEMERIS, untested");
                Sw.WriteLine("232,21,Test Mode Config Request - SSB_EE_FILE_STATUS, untested");
                Sw.WriteLine("232,254,Test Mode Config Request - SSB_EE_DISABLE_EE_SPEC, untested");
                Sw.WriteLine("232,255,Test Mode Config Request - SSB_EE_DEBUG, untested");
            }
        }

        public void ClearSystemCrashFlag()
        {
            this.system_crash_flag = false;
        }

        public override void CloseSession(byte sessionCloseReqInfo)
        {
            if ((base._commWindow._rxType == CommunicationManager.ReceiverType.SLC) && (base._commWindow.AidingProtocol == "AI3"))
            {
                base._commWindow.WriteData("A0 A2 00 03 02 01 00 80 03 B0 B3");
            }
        }

        private void CreateProtocolTestResponseRecord(IDPairs IDp, string name, ProtocolTestTypes ptt, Ack_Responses ar, bool sc)
        {
            ProtocolTestResults item = new ProtocolTestResults(IDp.midID, IDp.subID, name, ptt, ar, sc);
            this.ProtocolTestResultsList.Add(item);
        }

        private void CreateReportOfProtocolTestResponseRecords(string prtcl, string baseName)
        {
            string text1 = ConfigurationManager.AppSettings["InstalledDirectory"];
            string path = baseName + ".txt";
            this.prtclTSw = new StreamWriter(path);
            foreach (ProtocolTestResults results in this.ProtocolTestResultsList)
            {
                string str2 = string.Format("{0},{1},{2}, test: {3}, Ack Response: {4}, Sys Crash: {5}", new object[] { results.midID, results.subID, results.name, results.protclTestType, results.ack_resp, results.system_crash });
                this.prtclTSw.WriteLine(str2);
            }
            AddListOfUnTestedCommandsToReport(this.prtclTSw, prtcl);
            this.prtclTSw.Close();
        }

        private int decodeAi3PosResp(string logTime, string message)
        {
            int num = 0;
            if ((message == null) || (message.Length == 0))
            {
                return 1;
            }
            if (!base._resetCtrl.ResetPositionAvailable)
            {
                base._rxNavData.TTFFSiRFLive = ((double) (DateTime.Now.Ticks - base._resetCtrl.StartResetTime)) / 10000000.0;
                if (base._rxNavData.TTFFSiRFLive > 1.0)
                {
                    base._rxNavData.TTFFSiRFLive--;
                }
                string str = message.Replace(" ", "");
                byte[] input = HelperFunctions.HexToByte(str.Substring(0x10, str.Length - 0x18));
                byte[] output = new byte[0xe1a];
                this._F_AI3.decmprss(input, (uint) input.Length, output, 0xe10);
                if (base._commWindow.AutoReplyCtrl.PositionRequestCtrl.LocMethod == 0)
                {
                    byte[] destinationArray = new byte[170];
                    Array.Copy(output, 0x4e, destinationArray, 0, 170);
                    string str3 = "45 02" + HelperFunctions.ByteToHex(destinationArray);
                    StringBuilder builder = new StringBuilder();
                    builder.Append("A0A2");
                    int num2 = 0xac;
                    builder.Append(num2.ToString("X").PadLeft(4, '0'));
                    builder.Append(GetChecksum(str3));
                    builder.Append("B0B3");
                    return this.GetPositionFromMeasurement(logTime, builder.ToString());
                }
                if (output[1] == 0)
                {
                    int nIdx = 15;
                    int index = nIdx + 4;
                    int num6 = index + 4;
                    double lat = (base.ImportINT32(output, 11) * 180.0) / 4294967296;
                    double lon = (base.ImportINT32(output, nIdx) * 360.0) / 4294967296;
                    double alt = -9999.0;
                    if ((output[index] & 2) == 2)
                    {
                        alt = (base.ImportINT16(output, num6) * 0.1) - 500.0;
                    }
                    num = 0;
                    PositionErrorCalc calc = new PositionErrorCalc();
                    if (base._rxNavData.ValidatePosition)
                    {
                        double num10 = Convert.ToDouble(base._rxNavData.RefLat);
                        double num11 = Convert.ToDouble(base._rxNavData.RefLon);
                        double num12 = Convert.ToDouble(base._rxNavData.RefAlt);
                        calc.GetPositionErrorsInMeter(lat, lon, alt, num10, num11, num12);
                        base._rxNavData.Nav2DPositionError = calc.HorizontalError;
                        if (alt != -9999.0)
                        {
                            base._rxNavData.Nav3DPositionError = calc.Position3DError;
                            base._rxNavData.NavVerticalPositionError = calc.VerticalErrorInMeter;
                        }
                        else
                        {
                            base._rxNavData.Nav3DPositionError = -9999.0;
                            base._rxNavData.NavVerticalPositionError = -9999.0;
                        }
                    }
                    base._rxNavData.MeasLat = lat;
                    base._rxNavData.MeasLon = lon;
                    base._rxNavData.MeasAlt = alt;
                    base._resetCtrl.ResetPositionAvailable = true;
                    return num;
                }
                num = 1;
                base._rxNavData.Nav2DPositionError = -9999.0;
                base._rxNavData.Nav3DPositionError = -9999.0;
                base._rxNavData.MeasLat = -9999.0;
                base._rxNavData.MeasLon = -9999.0;
                base._rxNavData.MeasLat = -9999.0;
                base._resetCtrl.ResetPositionAvailable = false;
            }
            return num;
        }

        public override string[] DecodeClockStatus(string message)
        {
            if ((message == null) || (message.Length == 0))
            {
                return new string[] { "" };
            }
            byte[] comByte = HelperFunctions.HexToByte(message);
            return base.m_Protocols.ConvertRawToFields(comByte).Split(new char[] { ',' });
        }

        public override string DecodeGeodeticNavigationDataToCSV(string message)
        {
            return this.decodeSSBMsgCsv(message);
        }

        public override string DecodeNavLibMeasurementDataToCSV(string message)
        {
            return this.decodeSSBMsgCsv(message);
        }

        public override int DecodePostionFromSSB(string logtime, string message)
        {
            if ((message.Length == 0) || base._resetCtrl.ResetPositionAvailable)
            {
                return 1;
            }
            Hashtable hashtable = base.m_Protocols.ConvertRawToHash(HelperFunctions.HexToByte(message), "SSB");
            double lat = 0.0;
            double lon = 0.0;
            double alt = 0.0;
            PositionErrorCalc calc = new PositionErrorCalc();
            if (hashtable.ContainsKey("NAV Type") && (((byte) (Convert.ToUInt16((string) hashtable["NAV Type"]) & 7)) == 0))
            {
                base._resetCtrl.ResetPositionAvailable = false;
                return 1;
            }
            if (hashtable.ContainsKey("Latitude"))
            {
                lat = Convert.ToDouble((string) hashtable["Latitude"]) / 10000000.0;
            }
            if (hashtable.ContainsKey("Longitude"))
            {
                lon = Convert.ToDouble((string) hashtable["Longitude"]) / 10000000.0;
            }
            if (hashtable.ContainsKey("Altitude from Ellipsoid"))
            {
                alt = Convert.ToDouble((string) hashtable["Altitude from Ellipsoid"]) / 100.0;
            }
            if (base._rxNavData.ValidatePosition)
            {
                double refLat = base._rxNavData.RefLat;
                double refLon = base._rxNavData.RefLon;
                double refAlt = base._rxNavData.RefAlt;
                calc.GetPositionErrorsInMeter(lat, lon, alt, refLat, refLon, refAlt);
                base._rxNavData.Nav2DPositionError = calc.HorizontalError;
                base._rxNavData.Nav3DPositionError = calc.Position3DError;
                base._rxNavData.NavVerticalPositionError = calc.VerticalErrorInMeter;
            }
            else
            {
                base._rxNavData.Nav2DPositionError = -9999.0;
                base._rxNavData.Nav3DPositionError = -9999.0;
                base._rxNavData.NavVerticalPositionError = -9999.0;
            }
            base._rxNavData.MeasLat = lat;
            base._rxNavData.MeasLon = lon;
            base._rxNavData.MeasAlt = alt;
            base._resetCtrl.ResetPositionAvailable = true;
            return 0;
        }

        public override string DecodeRawMeasuredNavigationDataToCSV(string message)
        {
            return this.decodeSSBMsgCsv(message);
        }

        public override string DecodeRawMessageToCSV(string protocol, string message)
        {
            string str = string.Empty;
            switch (protocol)
            {
                case "SSB":
                    this.decodeSSBMsgCsv(message);
                    return str;

                case "F":
                case "AI3":
                    return str;
            }
            return message;
        }

        public override Hashtable DecodeRawMessageToHash(string protocol, string message)
        {
            Hashtable hashtable = new Hashtable();
            string str = protocol;
            if (str == null)
            {
                return hashtable;
            }
            if (!(str == "SSB"))
            {
                if ((str == "F") || (str == "AI3"))
                {
                }
                return hashtable;
            }
            return this.decodeSSBMsgToHashtable(message);
        }

        private string[] decodeSSBMsg2(string message)
        {
            if ((message == null) || (message.Length == 0))
            {
                return new string[] { "" };
            }
            byte[] comByte = HelperFunctions.HexToByte(message);
            return base.m_Protocols.ConvertRawToFields(comByte).Split(new char[] { ',' });
        }

        protected string decodeSSBMsgCsv(string message)
        {
            byte[] comByte = HelperFunctions.HexToByte(message);
            return base.m_Protocols.ConvertRawToFields(comByte);
        }

        protected Hashtable decodeSSBMsgToHashtable(string message)
        {
            if ((message == null) || (message.Length == 0))
            {
                return new Hashtable();
            }
            byte[] comByte = HelperFunctions.HexToByte(message);
            return base.m_Protocols.ConvertRawToHash(comByte, "SSB");
        }

        public override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.isDisposed && disposing)
            {
                base.m_Protocols.Dispose();
                base._rxNavData = null;
                this._F_AI3 = null;
            }
            this.isDisposed = true;
        }

        ~SS3AndGSD3TWReceiver()
        {
            this.Dispose(false);
        }

        public override string FormatDRSensorDataString(string csvString)
        {
            if (csvString == string.Empty)
            {
                return string.Empty;
            }
            string[] strArray = csvString.Split(new char[] { ',' });
            StringBuilder builder = new StringBuilder();
            Convert.ToByte(strArray[2]);
            byte num = Convert.ToByte(strArray[3]);
            ushort num2 = Convert.ToUInt16(strArray[4]);
            ArrayList list = new ArrayList();
            SensorDataSets sets = new SensorDataSets();
            int num3 = 5;
            int num4 = 0;
            for (num4 = 0; num4 < num; num4++)
            {
                sets.ValidDataIndication = Convert.ToByte(strArray[num3++]);
                sets.DataSetTimeTag = Convert.ToUInt32(strArray[num3++]);
                sets.Data0 = Convert.ToInt16(strArray[num3++]);
                sets.Data1 = Convert.ToInt16(strArray[num3++]);
                sets.Data2 = Convert.ToInt16(strArray[num3++]);
                sets.Data3 = Convert.ToInt16(strArray[num3++]);
                sets.Data4 = Convert.ToInt16(strArray[num3++]);
                sets.Data5 = Convert.ToInt16(strArray[num3++]);
                sets.Data6 = Convert.ToInt16(strArray[num3++]);
                sets.Data7 = Convert.ToInt16(strArray[num3++]);
                sets.Data8 = Convert.ToInt16(strArray[num3++]);
                list.Add(sets);
            }
            for (num4 = 0; num4 < num; num4++)
            {
                SensorDataSets sets2 = (SensorDataSets) list[num4];
                builder.Append(string.Format("Data0: {0}", sets2.Data0));
                builder.Append(string.Format("Data1: {0}", sets2.Data1));
                builder.Append(string.Format("Data2: {0}", sets2.Data2));
                builder.Append(string.Format("Data3: {0}", sets2.Data3));
                builder.Append(string.Format("Data4: {0}", sets2.Data4));
                builder.Append(string.Format("Data5: {0}", sets2.Data5));
                builder.Append(string.Format("Data6: {0}", sets2.Data6));
                builder.Append(string.Format("Data7: {0}", sets2.Data7));
                builder.Append(string.Format("Data8: {0}", sets2.Data8));
            }
            builder.Append(string.Format("# of Valid: {0}", num));
            builder.Append(string.Format("Reserved: {0}", num2));
            for (num4 = 0; num4 < num; num4++)
            {
                SensorDataSets sets3 = (SensorDataSets) list[num4];
                builder.Append(string.Format("Time: {0}", sets3.DataSetTimeTag));
                builder.Append(string.Format("Validity: {0}{1}{2}{3}{4}{5}{6}{7}", new object[] { ((sets3.ValidDataIndication & 0x80) != 0) ? 1 : 0, ((sets3.ValidDataIndication & 0x40) != 0) ? 1 : 0, ((sets3.ValidDataIndication & 0x20) != 0) ? 1 : 0, ((sets3.ValidDataIndication & 0x10) != 0) ? 1 : 0, ((sets3.ValidDataIndication & 8) != 0) ? 1 : 0, ((sets3.ValidDataIndication & 4) != 0) ? 1 : 0, ((sets3.ValidDataIndication & 2) != 0) ? 1 : 0, ((sets3.ValidDataIndication & 1) != 0) ? 1 : 0 }));
            }
            return builder.ToString();
        }

        public override string FormatDRStateString(string csvString)
        {
            if (csvString == string.Empty)
            {
                return string.Empty;
            }
            string[] strArray = csvString.Split(new char[] { ',' });
            StringBuilder builder = new StringBuilder();
            string dRVersion = clsGlobal.DRVersion;
            string str2 = strArray[2];
            string str3 = strArray[3];
            string str4 = strArray[4];
            string str5 = strArray[5];
            string str6 = strArray[6];
            string str7 = strArray[7];
            string str8 = strArray[8];
            string str9 = strArray[9];
            string str10 = strArray[10];
            string str11 = strArray[11];
            string str12 = strArray[12];
            string str13 = strArray[13];
            string text1 = strArray[14];
            string str14 = strArray[15];
            string str15 = strArray[0x10];
            builder.Append("DR Spd: " + str2);
            builder.Append("\t\t\tError: " + str3 + "\r\n");
            builder.Append("DR Spd SF: " + str4);
            builder.Append("\t\t\tError: " + str5 + "\r\n");
            builder.Append("DR Hdg Rte: " + str6);
            builder.Append("\t\t\tError: " + str7 + "\r\n");
            if (dRVersion == "1.0")
            {
                builder.Append("DR Gyr Bias: " + str8);
                builder.Append("\t\tError: " + str9 + "\r\n");
            }
            builder.Append("DR Gyr SF: " + str10);
            builder.Append("\t\t\tError: " + str11 + "\r\n");
            builder.Append(string.Format("DR Hdg:{0}", str15));
            builder.Append(string.Format("\t\t\tDir:     {0}\r\n", (str14 != "0") ? "R" : "F"));
            builder.Append("Total DR Hdg Error: " + str13 + "\r\n");
            builder.Append("Total DR Pos Error: " + str12 + "\r\n");
            builder.Append("Nav Ctrl:");
            if (dRVersion == "2.0")
            {
                string str16 = strArray[0x11];
                string str17 = strArray[0x12];
                string str18 = strArray[0x13];
                string str19 = strArray[20];
                string str20 = strArray[0x15];
                string str21 = strArray[0x16];
                string str22 = strArray[0x17];
                string str23 = strArray[0x18];
                string str24 = strArray[0x19];
                string str25 = strArray[0x1a];
                string str26 = strArray[0x1b];
                string str27 = strArray[0x1c];
                string str28 = strArray[0x1d];
                string str29 = strArray[30];
                string str30 = strArray[0x1f];
                string str31 = strArray[0x20];
                string str32 = strArray[0x21];
                string str33 = strArray[0x22];
                string str34 = strArray[0x23];
                string str35 = strArray[0x24];
                string str36 = strArray[0x25];
                string str37 = strArray[0x25];
                if (str16 == "0")
                {
                    builder.Append(string.Format("Sensor Package: Gyro && Odo\r\n", new object[0]));
                }
                else if (str16 == "1")
                {
                    builder.Append(string.Format("Sensor Package: Wheel Speed && Odo\r\n", new object[0]));
                }
                else
                {
                    builder.Append(string.Format("Sensor Package: Unknown\r\n", new object[0]));
                }
                builder.Append(string.Format("Gyro Bias: {0}", str8));
                builder.Append(string.Format("\t\t\tError: {0}\r\n", str9));
                builder.Append(string.Format("Odo Speed:  {0}\r\n", str17));
                builder.Append(string.Format("Odo Spd SF: {0}", str18));
                builder.Append(string.Format("\t\tError: {0}\r\n", str19));
                builder.Append(string.Format("LF Wheel Spd SF: {0}", str20));
                builder.Append(string.Format("\t\tError: {0}\r\n", str21));
                builder.Append(string.Format("RF Wheel Spd SF: {0}", str22));
                builder.Append(string.Format("\t\tError: {0}\r\n", str23));
                builder.Append(string.Format("LR Wheel Spd SF: {0}", str24));
                builder.Append(string.Format("\t\tError: {0}\r\n", str25));
                builder.Append(string.Format("RR Wheel Spd SF: {0}", str26));
                builder.Append(string.Format("\t\tError: {0}\r\n", str27));
                builder.Append(string.Format("F-Axle Spd Delta: {0}\r\n", str33));
                builder.Append(string.Format("F-Axle Avg Spd: {0}", str34));
                builder.Append(string.Format("\t\t\tError: {0}\r\n", str35));
                builder.Append(string.Format("F-Axle Hdg Rte: {0}", str36));
                builder.Append(string.Format("\t\t\tError: {0}\r\n", str37));
                builder.Append(string.Format("R-Axle Spd Delta: {0}\r\n", str28));
                builder.Append(string.Format("R-Axle Avg Spd: {0}", str29));
                builder.Append(string.Format("\t\t\tError: {0}\r\n", str30));
                builder.Append(string.Format("R-Axle Hdg Rte: {0}", str31));
                builder.Append(string.Format("\t\t\tError: {0}\r\n", str32));
            }
            return builder.ToString();
        }

        public override string FormatDRStatusString(string csvString)
        {
            if (csvString == string.Empty)
            {
                return string.Empty;
            }
            string[] strArray = csvString.Split(new char[] { ',' });
            StringBuilder builder = new StringBuilder();
            byte num = Convert.ToByte(strArray[2]);
            ushort num2 = Convert.ToUInt16(strArray[3]);
            byte num3 = Convert.ToByte(strArray[4]);
            byte num4 = Convert.ToByte(strArray[5]);
            byte num5 = Convert.ToByte(strArray[6]);
            byte num6 = Convert.ToByte(strArray[7]);
            byte num7 = Convert.ToByte(strArray[8]);
            byte num8 = Convert.ToByte(strArray[9]);
            byte num9 = Convert.ToByte(strArray[10]);
            byte num10 = Convert.ToByte(strArray[11]);
            byte num11 = Convert.ToByte(strArray[12]);
            byte num12 = Convert.ToByte(strArray[13]);
            byte num13 = 0;
            byte num14 = 0;
            ushort num15 = 0;
            byte num16 = 0;
            string dRVersion = clsGlobal.DRVersion;
            if (dRVersion == "1.0")
            {
                Convert.ToUInt16(strArray[14]);
            }
            else if (dRVersion == "2.0")
            {
                num13 = Convert.ToByte(strArray[14]);
                num14 = Convert.ToByte(strArray[15]);
                num15 = Convert.ToUInt16(strArray[0x10]);
                num16 = Convert.ToByte(strArray[0x11]);
            }
            builder.Append("### Nav Status  ###\r\n");
            if ((num & 0x7f) != 0)
            {
                builder.Append(string.Format("DR Nav:\t\t\t Invalid ({0} {1} {2} {3} {4} {5} {6})\r\n", new object[] { ((num & 0x40) != 0) ? "C" : "c", ((num & 0x20) != 0) ? "D" : "d", ((num & 0x10) != 0) ? "C" : "c", ((num & 8) != 0) ? "H" : "h", ((num & 4) != 0) ? "P" : "p", ((num & 2) != 0) ? "S" : "s", ((num & 1) != 0) ? "G" : "g" }));
            }
            else
            {
                builder.Append("DR Nav:\t\t\t Valid\r\n");
            }
            if ((num2 & 0x7ff) != 0)
            {
                builder.Append(string.Format("DR Data:\t\t\t Invalid ({0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10})\r\n", new object[] { ((num2 & 0x400) != 0) ? "T" : "t", ((num2 & 0x200) != 0) ? "M" : "m", ((num2 & 0x100) != 0) ? "I" : "i", ((num2 & 0x80) != 0) ? "W" : "w", ((num2 & 0x40) != 0) ? "B" : "b", ((num2 & 0x20) != 0) ? "T" : "t", ((num2 & 0x10) != 0) ? "D" : "d", ((num2 & 8) != 0) ? "C" : "c", ((num2 & 4) != 0) ? "M" : "m", ((num2 & 2) != 0) ? "S" : "s", ((num2 & 1) != 0) ? "G" : "g" }));
            }
            else
            {
                builder.Append("DR Data:\t\t\t Valid\r\n");
            }
            if ((num3 & 15) != 0)
            {
                builder.Append(string.Format("DR Cal:\t\t\t Invalid ({0} {1} {2} {3})\r\n", new object[] { ((num3 & 8) != 0) ? "G" : "g", ((num3 & 4) != 0) ? "S" : "s", ((num3 & 2) != 0) ? "G" : "g", ((num3 & 1) != 0) ? "G" : "g" }));
            }
            else
            {
                builder.Append("DR Cal:\t\t\t Valid\r\n");
            }
            if ((num5 & 120) != 0)
            {
                builder.Append(string.Format("DR Pos:\t\t\t Invalid ({0} {1} {2} {3})\r\n", new object[] { ((num5 & 0x40) != 0) ? "D" : "d", ((num5 & 0x20) != 0) ? "C" : "c", ((num5 & 0x10) != 0) ? "P" : "p", ((num5 & 8) != 0) ? "S" : "s" }));
            }
            else
            {
                builder.Append("DR Pos:\t\t\t Valid\r\n");
            }
            if ((num6 & 0x7f) != 0)
            {
                builder.Append(string.Format("DR Hdg:\t\t\t Invalid ({0} {1} {2} {3} {4} {5} {6})\r\n", new object[] { ((num6 & 0x40) != 0) ? "D" : "d", ((num6 & 0x20) != 0) ? "C" : "c", ((num6 & 0x10) != 0) ? "T" : "t", ((num6 & 8) != 0) ? "H" : "h", ((num6 & 4) != 0) ? "S" : "s", ((num6 & 2) != 0) ? "P" : "p", ((num6 & 1) != 0) ? "S" : "s" }));
            }
            else
            {
                builder.Append("DR Hdg:\t\t\t Valid\r\n");
            }
            if ((num5 & 3) != 0)
            {
                builder.Append(string.Format("DR Nav across Reset:\t Invalid ({0} {1})\r\n", ((num5 & 2) != 0) ? "S" : "s", ((num5 & 1) != 0) ? "N" : "n"));
            }
            else
            {
                builder.Append("DR Nav across Reset:\t Valid\r\n");
            }
            if ((num8 & 7) != 0)
            {
                builder.Append(string.Format("DR Nav St Int Ran:\t\t Invalid ({0} {1} {2})\r\n", ((num8 & 4) != 0) ? "D" : "d", ((num8 & 2) != 0) ? "H" : "h", ((num8 & 1) != 0) ? "P" : "p"));
            }
            else
            {
                builder.Append("DR Nav St Int Ran:\t\t Valid\r\n");
            }
            if ((num9 & 0x80) != 0)
            {
                builder.Append("DR Nav State Upd:\t\t Invalid\r\n");
            }
            else
            {
                builder.Append("DR Nav State Upd:\t\t Valid\r\n");
            }
            if ((num10 & 0x1f) != 0)
            {
                builder.Append(string.Format("Gps Pos Upd:\t\t Invalid ({0} {1} {2} {3} {4})\r\n", new object[] { ((num10 & 0x10) != 0) ? "D" : "d", ((num10 & 8) != 0) ? "G" : "g", ((num10 & 4) != 0) ? "P" : "p", ((num10 & 2) != 0) ? "E" : "e", ((num10 & 1) != 0) ? "M" : "m" }));
            }
            else
            {
                builder.Append("Gps Pos Upd:\t\t Valid\r\n");
            }
            if ((num11 & 0x7f) != 0)
            {
                builder.Append(string.Format("Gps Hdg Upd:\t\t Invalid ({0} {1} {2} {3} {4} {5} {6})\r\n", new object[] { ((num11 & 0x40) != 0) ? "I" : "i", ((num11 & 0x20) != 0) ? "K" : "k", ((num11 & 0x10) != 0) ? "H" : "h", ((num11 & 8) != 0) ? "V" : "v", ((num11 & 4) != 0) ? "F" : "f", ((num11 & 2) != 0) ? "S" : "s", ((num11 & 1) != 0) ? "M" : "m" }));
            }
            else
            {
                builder.Append("Gps Hdg Upd:\t\t Valid\r\n");
            }
            if ((num12 & 7) != 0)
            {
                builder.Append(string.Format("GPS Pos for DR:\t\t Invalid ({0} {1} {2})\r\n", ((num12 & 4) != 0) ? "P" : "p", ((num12 & 2) != 0) ? "E" : "e", ((num12 & 1) != 0) ? "F" : "f"));
            }
            else
            {
                builder.Append("Gps Pos for DR:\t\t Valid\r\n");
            }
            if ((num12 & 0x70) != 0)
            {
                builder.Append(string.Format("GPS Vel for DR:\t\t Invalid ({0} {1} {2} {3})\r\n", new object[] { ((num12 & 0x80) != 0) ? "H" : "h", ((num12 & 0x40) != 0) ? "S" : "s", ((num12 & 0x20) != 0) ? "E" : "e", ((num12 & 0x10) != 0) ? "F" : "f" }));
            }
            else
            {
                builder.Append("Gps Vel for DR:\t\t Valid\r\n");
            }
            if ((num7 & 7) != 0)
            {
                builder.Append(string.Format("Gyro Subsys Op:\t\t Invalid ({0} {1} {2})\r\n", ((num7 & 4) != 0) ? "R" : "r", ((num7 & 2) != 0) ? "L" : "l", ((num7 & 1) != 0) ? "H" : "h"));
            }
            else
            {
                builder.Append("Gyro Subsys Op:\t\t Valid\r\n");
            }
            if ((num7 & 0x70) != 0)
            {
                builder.Append(string.Format("Odo Spd Subsys Op:\t Invalid ({0} {1} {2})\r\n", ((num7 & 0x40) != 0) ? "R" : "r", ((num7 & 0x20) != 0) ? "D" : "d", ((num7 & 0x10) != 0) ? "G" : "g"));
            }
            else
            {
                builder.Append("Odo Spd Subsys Op:\t Valid\r\n");
            }
            if ((num3 & 0x70) != 0)
            {
                builder.Append(string.Format("Gyro Bias Cal:\t\t Invalid ({0} {1} {2})\r\n", ((num3 & 0x40) != 0) ? "H" : "h", ((num3 & 0x20) != 0) ? "C" : "c", ((num3 & 0x10) != 0) ? "D" : "d"));
            }
            else
            {
                builder.Append("Gyro Bias Cal:\t\t Valid\r\n");
            }
            if ((num8 & 0x70) != 0)
            {
                builder.Append(string.Format("0-Spd Gyro Bias Cal Upd:\t Invalid ({0} {1} {2})\r\n", ((num8 & 0x40) != 0) ? "P" : "p", ((num8 & 0x20) != 0) ? "D" : "d", ((num8 & 0x10) != 0) ? "G" : "g"));
            }
            else
            {
                builder.Append("0-Spd Gyro Bias Cal Upd:\t Valid\r\n");
            }
            if ((num4 & 15) != 0)
            {
                builder.Append(string.Format("Gyro SF Cal:\t\t Invalid ({0} {1} {2} {3})\r\n", new object[] { ((num4 & 8) != 0) ? "H" : "h", ((num4 & 4) != 0) ? "P" : "p", ((num4 & 2) != 0) ? "D" : "d", ((num4 & 1) != 0) ? "H" : "h" }));
            }
            else
            {
                builder.Append("Gyro SF Cal:\t\t Valid\r\n");
            }
            if ((num9 & 15) != 0)
            {
                builder.Append(string.Format("Gyro Bias & SF Cal Upd:\t Invalid ({0} {1} {2} {3})\r\n", new object[] { ((num9 & 8) != 0) ? "H" : "h", ((num9 & 4) != 0) ? "V" : "v", ((num9 & 2) != 0) ? "P" : "p", ((num9 & 1) != 0) ? "D" : "d" }));
            }
            else
            {
                builder.Append("Gyro Bias & SF Cal Upd:\t Valid\r\n");
            }
            if ((num4 & 240) != 0)
            {
                builder.Append(string.Format("Odo Spd SF Cal:\t\t Invalid ({0} {1} {2} {3})\r\n", new object[] { ((num4 & 0x80) != 0) ? "S" : "s", ((num4 & 0x40) != 0) ? "V" : "v", ((num4 & 0x20) != 0) ? "P" : "p", ((num4 & 0x10) != 0) ? "D" : "d" }));
            }
            else
            {
                builder.Append("Odo Spd SF Cal:\t\t Valid\r\n");
            }
            if ((num9 & 0x70) != 0)
            {
                builder.Append(string.Format("Odo Spd Cal Upd:\t\t Invalid ({0} {1} {2})\r\n", ((num9 & 0x40) != 0) ? "V" : "v", ((num9 & 0x20) != 0) ? "P" : "p", ((num9 & 0x10) != 0) ? "D" : "d"));
            }
            else
            {
                builder.Append("Odo Spd Cal Upd:\t\t Valid\r\n");
            }
            if (dRVersion == "2.0")
            {
                if (num13 != 0)
                {
                    builder.Append(string.Format("DWS Hdg Rate SF Cal:\t Invalid ({0})\r\n", ((num13 & 1) != 0) ? "S" : "s"));
                }
                else
                {
                    builder.Append("DWS Hdg Rate SF Cal:\t Valid\r\n");
                }
                if (num14 != 0)
                {
                    builder.Append(string.Format("DWS Hdg Rate SF Upd:\t Invalid ({0} {1} {2} {3} {4} {5} {6})\r\n", new object[] { ((num14 & 0x40) != 0) ? "G" : "g", ((num14 & 0x20) != 0) ? "P" : "p", ((num14 & 0x10) != 0) ? "R" : "r", ((num14 & 8) != 0) ? "L" : "l", ((num14 & 4) != 0) ? "H" : "h", ((num14 & 2) != 0) ? "L" : "l", ((num14 & 1) != 0) ? "V" : "v" }));
                }
                else
                {
                    builder.Append("DWS Hdg Rate SF Upd:\t Valid\r\n");
                }
                if (num15 != 0)
                {
                    builder.Append(string.Format("DWS Speed SF Cal:\t Invalid ({0} {1} {2} {3})\r\n", new object[] { ((num15 & 0x1000) != 0) ? "L" : "l", ((num15 & 0x100) != 0) ? "R" : "r", ((num15 & 0x10) != 0) ? "L" : "l", ((num15 & 1) != 0) ? "R" : "r" }));
                }
                else
                {
                    builder.Append("DWS Speed SF Cal:\t Valid\r\n");
                }
                if (num16 != 0)
                {
                    builder.Append(string.Format("DWS Speed SF Upd:\t Invalid ({0} {1} {2} {3} {4} {5})\r\n", new object[] { ((num16 & 0x20) != 0) ? "D" : "d", ((num16 & 0x10) != 0) ? "A" : "a", ((num16 & 8) != 0) ? "E" : "e", ((num16 & 4) != 0) ? "A" : "a", ((num16 & 2) != 0) ? "H" : "h", ((num16 & 1) != 0) ? "S" : "s" }));
                }
                else
                {
                    builder.Append("DWS Speed SF Upd:\t Valid\r\n");
                }
            }
            return builder.ToString();
        }

        public override string FormatNavSubSysString(string csvString)
        {
            if (csvString == string.Empty)
            {
                return string.Empty;
            }
            string[] strArray = csvString.Split(new char[] { ',' });
            StringBuilder builder = new StringBuilder();
            builder.Append("Gps Hdg Rte: " + strArray[2]);
            builder.Append("\t\tError: " + strArray[3] + "\r\n");
            builder.Append("Gps Hdg (T): " + strArray[4]);
            builder.Append("\t\tError: " + strArray[5] + "\r\n");
            builder.Append("Gps Spd: " + strArray[6]);
            builder.Append("\t\tError: " + strArray[7] + "\r\n");
            builder.Append("Gps Pos Error: " + strArray[8] + "\r\n");
            builder.Append("DR Hdg Rte: " + strArray[9]);
            builder.Append("\t\tError: " + strArray[10] + "\r\n");
            builder.Append("DR Hdg (T): " + strArray[11]);
            builder.Append("\t\tError:: " + strArray[12] + "\r\n");
            builder.Append("DR Spd: " + strArray[13]);
            builder.Append("\t\tError: " + strArray[14] + "\r\n");
            builder.Append("DR Pos Error: " + strArray[15] + "\r\n");
            return builder.ToString();
        }

        public override void GetAutoReplyParameters(string configFilePath)
        {
            base._commWindow.AutoReplyCtrl.SetAutoReplyParameters(configFilePath);
        }

        private static string GetChecksum(string message)
        {
            int num = 0;
            string str = message.Replace(" ", "");
            for (int i = 0; i < str.Length; i += 2)
            {
                num += Convert.ToByte(str.Substring(i, 2), 0x10);
            }
            return num.ToString("X2").PadLeft(4, '0');
        }

        public override string GetGWControllerScanResult(string message)
        {
            string str = string.Empty;
            if ((message != null) && (message.Length != 0))
            {
                string[] strArray = message.Split(new char[] { ' ' });
                int length = strArray.GetLength(0);
                try
                {
                    int num2 = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        if ((num2 + 9) < length)
                        {
                            string str3 = str;
                            str = str3 + strArray[num2 + 6] + strArray[num2 + 7] + strArray[num2 + 8] + strArray[num2 + 9] + " ";
                            num2 += 4;
                        }
                    }
                    num2 = 0x24;
                    for (int j = 0; j < 8; j++)
                    {
                        if ((num2 + 3) < length)
                        {
                            str = str + strArray[num2 + 2] + strArray[num2 + 3] + " ";
                            num2 += 2;
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Receiver.cs: GetGWControllerScanResult()-" + exception.ToString());
                }
            }
            return str;
        }

        public override string[] GetLatLonAltPosition(string csvString)
        {
            string[] strArray = new string[] { "-9999", "-9999", "-9999" };
            try
            {
                string[] strArray2 = csvString.Split(new char[] { ',' });
                int index = 12;
                strArray[0] = strArray2[index++];
                strArray[1] = strArray2[index++];
                strArray[2] = strArray2[index];
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return strArray;
        }

        public override int GetMeasurement(string logTime, string message)
        {
            if ((message == null) || (message.Length == 0))
            {
                return 1;
            }
            if (!base._resetCtrl.ResetPositionAvailable)
            {
                string str = message.Replace(" ", "");
                byte[] input = HelperFunctions.HexToByte(str.Substring(0x10, str.Length - 0x18));
                byte[] output = new byte[0xe1a];
                this._F_AI3.decmprss(input, (uint) input.Length, output, 0xe10);
                byte[] destinationArray = new byte[170];
                Array.Copy(output, 0x60, destinationArray, 0, 170);
                string str3 = "45 02 01" + HelperFunctions.ByteToHex(destinationArray);
                StringBuilder builder = new StringBuilder();
                builder.Append("A0A2");
                int num2 = 0xac;
                builder.Append(num2.ToString("X").PadLeft(4, '0'));
                builder.Append(str3);
                builder.Append(GetChecksum(str3));
                builder.Append("B0B3");
                this.GetPositionFromMeasurement(logTime, builder.ToString());
            }
            return 0;
        }

        public override byte GetNavigationMode(string message)
        {
            byte num = 0;
            if ((message != null) && (message.Length != 0))
            {
                byte[] comByte = HelperFunctions.HexToByte(message);
                Hashtable hashtable = base.m_Protocols.ConvertRawToHash(comByte, "SSB");
                if (hashtable.ContainsKey("Mode 1"))
                {
                    num = (byte) (Convert.ToByte((string) hashtable["Mode 1"]) & 7);
                }
            }
            return num;
        }

        public override string GetNumSVSTrk(string message)
        {
            int num = 0;
            string str = string.Format("%d", num);
            if ((message == null) || (message.Length == 0))
            {
                return str;
            }
            byte[] comByte = HelperFunctions.HexToByte(message);
            Hashtable hashtable = base.m_Protocols.ConvertRawToHash(comByte, "SSB");
            int num2 = 12;
            for (int i = 1; i <= num2; i++)
            {
                string key = string.Format(clsGlobal.MyCulture, "State SVID{0:D}", new object[] { i });
                if (hashtable.ContainsKey(key) && (Convert.ToInt32((string) hashtable[key]) != 0))
                {
                    num++;
                }
            }
            return string.Format("{0:D}", num);
        }

        public override string GetPerChanAvgCNo(string message)
        {
            string str = string.Empty;
            if ((message != null) && (message.Length != 0))
            {
                byte[] comByte = HelperFunctions.HexToByte(message);
                Hashtable hashtable = base.m_Protocols.ConvertRawToHash(comByte, "SSB");
                int num = 12;
                int num2 = 10;
                double num3 = 0.0;
                int num4 = 0;
                StringBuilder builder = new StringBuilder();
                for (int i = 1; i <= num; i++)
                {
                    int num6 = 0;
                    int num7 = 0;
                    for (int j = 1; j <= num2; j++)
                    {
                        string key = string.Format(clsGlobal.MyCulture, "C/NO{1:D} SVID{0:D}", new object[] { j, i });
                        if (hashtable.ContainsKey(key))
                        {
                            int num9 = Convert.ToInt32((string) hashtable[key]);
                            if (num9 > 0)
                            {
                                num6 += num9;
                                num7++;
                            }
                        }
                    }
                    string str3 = string.Empty;
                    double num10 = num6 / ((num7 > 0) ? num7 : 1);
                    str3 = string.Format(clsGlobal.MyCulture, "{0:F1}", new object[] { num10 });
                    if (num10 > 0.0)
                    {
                        num4++;
                        num3 += num10;
                    }
                    builder.Append(str3);
                    builder.Append(",");
                }
                str = builder.ToString();
                if (num4 <= 0)
                {
                    num4 = 1;
                }
                string str4 = string.Format(clsGlobal.MyCulture, "{0:F1}", new object[] { num3 / ((double) num4) });
                base._rxNavData.AvgCNo = str + str4;
            }
            return str;
        }

        public override int GetPosition(string logTime, string message)
        {
            int num = 0;
            string str = base._aidingProtocol;
            if (str == null)
            {
                return num;
            }
            if (!(str == "AI3"))
            {
                if (str != "SSB")
                {
                    return num;
                }
            }
            else
            {
                return this.decodeAi3PosResp(logTime, message);
            }
            return this.DecodePostionFromSSB(logTime, message);
        }

        public override int GetPositionFromMeasurement(string logTime, string message)
        {
            int num = 0;
            PositionErrorCalc calc = new PositionErrorCalc();
            if ((message.Length == 0) || (message == null))
            {
                return 1;
            }
            byte num2 = Convert.ToByte(message.Replace(" ", "").Substring(0x10, 2));
            double lat = Convert.ToDouble(base._rxNavData.RefLat);
            double lon = Convert.ToDouble(base._rxNavData.RefLon);
            double num5 = Convert.ToDouble(base._rxNavData.RefAlt);
            int estHorrErr = (int) base._commWindow.AutoReplyCtrl.ApproxPositionCtrl.EstHorrErr;
            if (num2 != 0)
            {
                num = 1;
                base._rxNavData.Nav2DPositionError = -9999.0;
                base._rxNavData.Nav3DPositionError = -9999.0;
                base._rxNavData.NavVerticalPositionError = -9999.0;
                base._rxNavData.TTFFSiRFLive = -9999.0;
                base._commWindow.WriteApp("### Need more time ###");
                return num;
            }
            base._rxNavData.TTFFSiRFLive = ((double) (DateTime.Now.Ticks - base._resetCtrl.StartResetTime)) / 10000000.0;
            if (base._rxNavData.TTFFSiRFLive > 1.0)
            {
                base._rxNavData.TTFFSiRFLive -= clsGlobal.SiRFLive_TTFF_OFFSET;
            }
            NavLibInterface interface2 = new NavLibInterface();
            interface2.MeasRespMsg = message;
            interface2.EphClkMsg = base._commWindow.AutoReplyCtrl.EphDataMsgBackup;
            interface2.SetUncertValues(0, 0);
            interface2.SetAPRValues(lat, lon, (int) num5, estHorrErr);
            if (!interface2.GetPositionValues())
            {
                base._commWindow.WriteApp("### Error get position in NavLib ###");
                return num;
            }
            double num7 = interface2.Ai3ResultValues.lat * 4.1909515857696533E-08;
            double num8 = interface2.Ai3ResultValues.lon * 8.3819031715393066E-08;
            double alt = (interface2.Ai3ResultValues.height * 0.1) - 500.0;
            base._rxNavData.MeasLat = num7;
            base._rxNavData.MeasLon = num8;
            base._rxNavData.MeasAlt = alt;
            base._rxNavData.TOW = interface2.Ai3ResultValues.TOW;
            if (base._rxNavData.ValidatePosition)
            {
                calc.GetPositionErrorsInMeter(num7, num8, alt, lat, lon, num5);
                base._rxNavData.Nav2DPositionError = calc.HorizontalError;
                base._rxNavData.Nav3DPositionError = calc.Position3DError;
                base._rxNavData.NavVerticalPositionError = calc.VerticalErrorInMeter;
            }
            else
            {
                base._rxNavData.Nav2DPositionError = -9999.0;
                base._rxNavData.Nav3DPositionError = -9999.0;
                base._rxNavData.NavVerticalPositionError = -9999.0;
            }
            if (!base._resetCtrl.ResetPositionAvailable)
            {
                base._rxNavData.TTFFReport = base._rxNavData.TTFFSiRFLive;
                base._rxNavData.FirstFixMeasLat = base._rxNavData.MeasLat;
                base._rxNavData.FirstFixMeasLon = base._rxNavData.MeasLon;
                base._rxNavData.FirstFixMeasAlt = base._rxNavData.MeasAlt;
                base._rxNavData.FirstFix2DPositionError = base._rxNavData.Nav2DPositionError;
                base._rxNavData.FirstFix3DPositionError = base._rxNavData.Nav3DPositionError;
                base._rxNavData.FirstFixVerticalPositionError = base._rxNavData.NavVerticalPositionError;
                base._rxNavData.FirstFixTOW = base._rxNavData.TOW;
                base._resetCtrl.ResetPositionAvailable = true;
            }
            return 0;
        }

        public override string GetSiRFAwareScanResult(string message)
        {
            return base.GetSiRFAwareScanResult(message);
        }

        public override string GetSVSTrkTOW(string message)
        {
            string str = string.Empty;
            if ((message != null) && (message.Length != 0))
            {
                byte[] comByte = HelperFunctions.HexToByte(message);
                Hashtable hashtable = base.m_Protocols.ConvertRawToHash(comByte, "SSB");
                string key = string.Format(clsGlobal.MyCulture, "GPS TOW", new object[0]);
                if (hashtable.ContainsKey(key))
                {
                    str = (string) hashtable[key];
                }
            }
            return str;
        }

        public override void GetSWVersion(string message)
        {
            if ((message != null) && (message.Length != 0))
            {
                byte[] comByte = HelperFunctions.HexToByte(message);
                base._resetCtrl.ResetRxSwVersion = base.m_Protocols.ConvertRawToFields(comByte);
            }
        }

        public override string GetTTFF()
        {
            string str = base._resetCtrl.LogTTFFCsv();
            string str2 = string.Empty;
            if (str.Length != 0)
            {
                string[] strArray = str.Split(new char[] { ',' });
                int index = 1;
                int num2 = index + 12;
                if (strArray.Length >= 7)
                {
                    for (index = 1; index < 7; index++)
                    {
                        str2 = str2 + strArray[index] + ",";
                    }
                }
                if (strArray.Length >= 0x12)
                {
                    for (index = 0; index < 4; index++)
                    {
                        str2 = str2 + strArray[num2] + ",";
                        num2++;
                    }
                }
                str2 = str2.TrimEnd(new char[] { ',' });
            }
            if (((base._resetCtrl.ResetEarlyTerminate && !clsGlobal.WaitForEph) && ((base._resetCtrl.ResetCount <= base._resetCtrl.TotalNumberOfResets) || (base._resetCtrl.TotalNumberOfResets < 0))) && base._resetCtrl.LoopitInprogress)
            {
                Random random = new Random();
                int num3 = ((int) base._resetCtrl.ResetInterval) - random.Next(2, clsGlobal.ResetPeriodRandomizationSec);
                if (num3 < 0)
                {
                    base._resetCtrl.OneSecondCount = base._resetCtrl.ResetInterval - ((uint) random.Next(2, 8));
                }
                else
                {
                    base._resetCtrl.OneSecondCount = (uint) num3;
                }
            }
            base._rxNavData.Valid = true;
            return str2;
        }

        public override string GetTTFFFromHash(Hashtable msgH)
        {
            try
            {
                base._rxNavData.TTFFReset = -9999.0;
                base._rxNavData.TTFFAided = -9999.0;
                base._rxNavData.TTFFFirstNav = -9999.0;
                byte num = 0;
                double num2 = 0.0;
                double num3 = 0.0;
                int num4 = 0;
                base._resetCtrl.ResetTTFFAvailable = false;
                if (msgH.ContainsKey("TTFF since reset"))
                {
                    double num5 = Convert.ToDouble(msgH["TTFF since reset"]) / 10.0;
                    base._rxNavData.TTFFReset = num5;
                }
                if (msgH.ContainsKey("TTFF since all aiding received"))
                {
                    double num6 = Convert.ToDouble(msgH["TTFF since all aiding received"]) / 10.0;
                    base._rxNavData.TTFFAided = num6;
                }
                if (msgH.ContainsKey("TTFF first nav since reset"))
                {
                    double num7 = Convert.ToDouble(msgH["TTFF first nav since reset"]) / 10.0;
                    base._rxNavData.TTFFFirstNav = num7;
                }
                base._resetCtrl.ResetTTFFAvailable = true;
                if (msgH.ContainsKey("Reserved1"))
                {
                    num = Convert.ToByte((string) msgH["Reserved1"]);
                    base._rxNavData.AidingFlags = num.ToString();
                    if ((num & 1) == 1)
                    {
                        num4 = 1;
                    }
                    else
                    {
                        num4 = 0;
                    }
                }
                if (msgH.ContainsKey("Time Uncertainty"))
                {
                    if (num4 == 1)
                    {
                        base._rxNavData.TimeUncer = base.DecodePreciseTTaccuracy(Convert.ToByte((string) msgH["Time Uncertainty"]));
                    }
                    else
                    {
                        base._rxNavData.TimeUncer = Receiver.DecodeCoarseTTaccuracy(Convert.ToByte((string) msgH["Time Uncertainty"]));
                    }
                }
                if (msgH.ContainsKey("Frequency Uncertainty"))
                {
                    base._rxNavData.FreqUncer = base.DecodeFTR_Acc(Convert.ToByte((string) msgH["Frequency Uncertainty"]));
                }
                if (msgH.ContainsKey("Time Aiding Error"))
                {
                    num2 = Convert.ToDouble((string) msgH["Time Aiding Error"]);
                    if (num4 == 1)
                    {
                        base._rxNavData.TimeErr = string.Format(clsGlobal.MyCulture, "{0:F6}", new object[] { num2 * 0.001 });
                    }
                    else
                    {
                        base._rxNavData.TimeErr = string.Format(clsGlobal.MyCulture, "{0:F6}", new object[] { num2 });
                    }
                }
                if (msgH.ContainsKey("Frequency Aiding Error"))
                {
                    num3 = Convert.ToDouble((string) msgH["Frequency Aiding Error"]);
                    base._rxNavData.FreqErr = string.Format(clsGlobal.MyCulture, "{0:F6}", new object[] { num3 * 0.001 });
                }
                return string.Format(clsGlobal.MyCulture, "{0:F1},{1:F1},{2:F1},{3},{4},{5},{6}", new object[] { base._rxNavData.TTFFReset, base._rxNavData.TTFFAided, base._rxNavData.TTFFFirstNav, base._rxNavData.TimeUncer, base._rxNavData.TimeErr, base._rxNavData.FreqUncer, base._rxNavData.FreqErr });
            }
            catch
            {
                return string.Empty;
            }
        }

        public override bool GetTTFFValues(string logTime, string message, int type)
        {
            try
            {
                if ((message == null) || (message.Length == 0))
                {
                    return false;
                }
                base._rxNavData.TTFFLogTime = logTime;
                byte[] comByte = HelperFunctions.HexToByte(message);
                Hashtable msgH = base.m_Protocols.ConvertRawToHash(comByte, "SSB");
                this.GetTTFFFromHash(msgH);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override byte IsNavigated(string message)
        {
            byte num = 0;
            if ((message != null) && (message.Length != 0))
            {
                byte[] comByte = HelperFunctions.HexToByte(message);
                string gpsTow = string.Empty;
                string gpsWeek = string.Empty;
                Hashtable hashtable = base.m_Protocols.ConvertRawToHash(comByte, "SSB");
                if (hashtable.ContainsKey("Mode 1"))
                {
                    num = (byte) (Convert.ToByte((string) hashtable["Mode 1"]) & 7);
                }
                if (num < 4)
                {
                    return num;
                }
                if (hashtable.ContainsKey("GPS TOW"))
                {
                    gpsTow = (string) hashtable["GPS TOW"];
                }
                if (hashtable.ContainsKey("GPS Week"))
                {
                    gpsWeek = (string) hashtable["GPS Week"];
                }
                base.SetGPSTime(gpsWeek, gpsTow);
                base._rxNavData.IsNav = true;
            }
            return num;
        }

        public static string msgContruct(uint chan, uint msgId, string msgPayload)
        {
            string str = "A0A2";
            string str2 = " B0B3";
            string msg = string.Empty;
            string str4 = string.Empty;
            string str5 = Convert.ToString((long) msgId, 0x10).PadLeft(2, '0');
            if (chan > 0)
            {
                str4 = Convert.ToString((long) chan, 0x10).PadLeft(2, '0');
            }
            msg = str4 + str5 + msgPayload;
            byte[] buffer = HelperFunctions.HexToByte(msg);
            StringBuilder builder = new StringBuilder(buffer.Length + 8);
            uint num = 0;
            uint num2 = 0;
            foreach (byte num3 in buffer)
            {
                num += num3;
            }
            num2 = (num & 0x7fff) | 0x8000;
            builder.Append(str);
            builder.Append(Convert.ToString(buffer.Length, 0x10).PadLeft(4, '0'));
            builder.Append(msg);
            builder.Append(Convert.ToString((long) num2, 0x10).PadLeft(4, '0'));
            builder.Append(str2);
            return builder.ToString().ToUpper();
        }

        public override void OpenChannel(string channel)
        {
            try
            {
                ArrayList list;
                int num = 0;
                string protocol = "F";
                string csvMessage = "2,";
                string msg = string.Empty;
                string str4 = channel;
                if (str4 != null)
                {
                    if (!(str4 == "SSB"))
                    {
                        if (str4 == "STAT")
                        {
                            goto Label_0040;
                        }
                    }
                    else
                    {
                        num = 0xee;
                    }
                }
                goto Label_0046;
            Label_0040:
                num = 0xbb;
            Label_0046:
                list = new ArrayList();
                StringBuilder builder = new StringBuilder();
                builder.Append("2,");
                list = base._commWindow.m_Protocols.GetInputMessageStructure(160, -1, "Channel Open Request", protocol);
                for (int i = 0; i < list.Count; i++)
                {
                    if (((InputMsg) list[i]).fieldName == "CHANNEL_TYPE")
                    {
                        builder.Append(num.ToString());
                        builder.Append(",");
                    }
                    else
                    {
                        builder.Append(((InputMsg) list[i]).defaultValue);
                        builder.Append(",");
                    }
                }
                csvMessage = builder.ToString().TrimEnd(new char[] { ',' });
                msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, "Channel Open Request", protocol);
                if ((base._commWindow._rxType == CommunicationManager.ReceiverType.SLC) && (num != 0))
                {
                    base._commWindow.WriteData(msg);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public override void OpenSession(byte sessionOpenReqInfo)
        {
            if ((base._commWindow._rxType == CommunicationManager.ReceiverType.SLC) && (base._commWindow.AidingProtocol == "AI3"))
            {
                base.AllowSessOpen = false;
                base._commWindow.WriteData("A0 A2 00 03 02 00 71 80 73 B0 B3");
            }
        }

        public override void PollNavigationParameters()
        {
            try
            {
                string csvMessage = string.Empty;
                if ((base._commWindow.RxType == CommunicationManager.ReceiverType.SLC) && (base._commWindow.MessageProtocol != "OSP"))
                {
                    csvMessage = "238,152,0";
                }
                else if (base._commWindow.RxType == CommunicationManager.ReceiverType.TTB)
                {
                    csvMessage = "204,152,0";
                }
                else
                {
                    csvMessage = "152,0";
                }
                string msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, "Poll Navigation Parameters", "SSB");
                base._commWindow.WriteData(msg);
            }
            catch
            {
            }
        }

        public override void PollSWVersion()
        {
            string csvMessage = string.Empty;
            string msg = string.Empty;
            ArrayList list = new ArrayList();
            StringBuilder builder = new StringBuilder();
            list = base._commWindow.m_Protocols.GetInputMessageStructure(0x84, -1, "Software Version Request", "SSB");
            if (base._commWindow._rxType == CommunicationManager.ReceiverType.SLC)
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
            msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, "Software Version Request", "SSB");
            base._commWindow.WriteData(msg);
        }

        public void ProtocolTestOSPCommands(string baseName)
        {
            string protocol = "OSP";
            string str2 = ConfigurationManager.AppSettings["InstalledDirectory"];
            if (this.ProtocolTestResultsList.Count > 0)
            {
                this.ProtocolTestResultsList.Clear();
            }
            List<IDPairs> list = new List<IDPairs>();
            list.Add(new IDPairs(0x35, 1));
            list.Add(new IDPairs(0x35, 2));
            list.Add(new IDPairs(0x35, 3));
            list.Add(new IDPairs(0x35, 4));
            list.Add(new IDPairs(0x80, 0));
            list.Add(new IDPairs(0x84, 0));
            list.Add(new IDPairs(210, 0));
            list.Add(new IDPairs(0xd3, 0));
            list.Add(new IDPairs(0xd3, 1));
            list.Add(new IDPairs(0xd3, 6));
            list.Add(new IDPairs(0xd3, 9));
            list.Add(new IDPairs(0xd4, 4));
            list.Add(new IDPairs(0xd4, 5));
            list.Add(new IDPairs(0xd4, 6));
            list.Add(new IDPairs(0xd4, 7));
            list.Add(new IDPairs(0xd4, 8));
            list.Add(new IDPairs(0xd4, 9));
            list.Add(new IDPairs(0xd5, 1));
            list.Add(new IDPairs(0xd5, 2));
            list.Add(new IDPairs(0xd6, 2));
            list.Add(new IDPairs(0xd7, 1));
            list.Add(new IDPairs(0xd7, 2));
            list.Add(new IDPairs(0xd7, 3));
            list.Add(new IDPairs(0xd8, 1));
            list.Add(new IDPairs(0xd8, 2));
            list.Add(new IDPairs(0xd9, 0));
            list.Add(new IDPairs(0xda, 0));
            list.Add(new IDPairs(0xda, 1));
            list.Add(new IDPairs(0xda, 2));
            list.Add(new IDPairs(0xda, 3));
            list.Add(new IDPairs(0xda, 4));
            list.Add(new IDPairs(0xdb, 0));
            list.Add(new IDPairs(220, 1));
            list.Add(new IDPairs(220, 2));
            list.Add(new IDPairs(220, 3));
            list.Add(new IDPairs(220, 5));
            list.Add(new IDPairs(0xde, 1));
            list.Add(new IDPairs(0xde, 2));
            list.Add(new IDPairs(0xde, 3));
            list.Add(new IDPairs(0xde, 4));
            list.Add(new IDPairs(0xde, 5));
            list.Add(new IDPairs(0xde, 6));
            list.Add(new IDPairs(0xde, 7));
            list.Add(new IDPairs(0xde, 8));
            list.Add(new IDPairs(0xea, 2));
            string filename = str2 + @"\Protocols\Protocols.xml";
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNodeList list2 = document.SelectNodes("/protocols/protocol[@name='" + protocol + "']/input/message");
            foreach (IDPairs pairs in list)
            {
                foreach (XmlNode node in list2)
                {
                    try
                    {
                        int num = int.Parse(node.Attributes["mid"].Value);
                        string msgName = node.Attributes["name"].Value;
                        int num2 = 0;
                        if (node.Attributes["subid"].Value != "")
                        {
                            num2 = int.Parse(node.Attributes["subid"].Value);
                        }
                        if ((num == pairs.midID) && (num2 == pairs.subID))
                        {
                            Console.WriteLine(string.Format("mid:{0} subid:{1} name: ", num, num2) + msgName);
                            for (int i = 1; i <= 7; i++)
                            {
                                this.system_crash_flag = true;
                                this.ack_response_flag = Ack_Responses.none;
                                this.SendMessageWithDefaultDataAndErrorInjection(num, num2, msgName, protocol, i);
                                Thread.Sleep(0x3e8);
                                this.CreateProtocolTestResponseRecord(pairs, msgName, (ProtocolTestTypes) i, this.ack_response_flag, this.system_crash_flag);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                    }
                }
            }
            this.CreateReportOfProtocolTestResponseRecords(protocol, baseName);
        }

        public void ProtocolTestSSBCommands(string baseName)
        {
            string protocol = "SSB";
            string str2 = ConfigurationManager.AppSettings["InstalledDirectory"];
            if (this.ProtocolTestResultsList.Count > 0)
            {
                this.ProtocolTestResultsList.Clear();
            }
            List<IDPairs> list = new List<IDPairs>();
            list.Add(new IDPairs(0x35, 0));
            list.Add(new IDPairs(0x84, 0));
            list.Add(new IDPairs(0x88, 0));
            list.Add(new IDPairs(0x8a, 0));
            list.Add(new IDPairs(140, 0));
            list.Add(new IDPairs(0x8f, 0));
            list.Add(new IDPairs(0x90, 0));
            list.Add(new IDPairs(0x91, 0));
            list.Add(new IDPairs(0x92, 0));
            list.Add(new IDPairs(0x93, 0));
            list.Add(new IDPairs(0x95, 0));
            list.Add(new IDPairs(0x97, 0));
            list.Add(new IDPairs(0x98, 0));
            list.Add(new IDPairs(0xa6, 0));
            list.Add(new IDPairs(0xa7, 0));
            list.Add(new IDPairs(0xa8, 0));
            list.Add(new IDPairs(170, 0));
            list.Add(new IDPairs(0xac, 3));
            list.Add(new IDPairs(0xac, 6));
            list.Add(new IDPairs(0xac, 7));
            list.Add(new IDPairs(0xac, 10));
            list.Add(new IDPairs(0xac, 11));
            string filename = str2 + @"\Protocols\Protocols.xml";
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNodeList list2 = document.SelectNodes("/protocols/protocol[@name='" + protocol + "']/input/message");
            try
            {
                foreach (IDPairs pairs in list)
                {
                    foreach (XmlNode node in list2)
                    {
                        int num = int.Parse(node.Attributes["mid"].Value);
                        string msgName = node.Attributes["name"].Value;
                        int num2 = 0;
                        if (node.Attributes["subid"].Value != "")
                        {
                            num2 = int.Parse(node.Attributes["subid"].Value);
                        }
                        if ((num == pairs.midID) && (num2 == pairs.subID))
                        {
                            Console.WriteLine(string.Format("mid:{0} subid:{1} name: ", num, num2) + msgName);
                            for (int i = 1; i <= 7; i++)
                            {
                                this.system_crash_flag = true;
                                this.ack_response_flag = Ack_Responses.none;
                                this.SendMessageWithDefaultDataAndErrorInjection(num, num2, msgName, protocol, i);
                                Thread.Sleep(0x7d0);
                                this.CreateProtocolTestResponseRecord(pairs, msgName, (ProtocolTestTypes) i, this.ack_response_flag, this.system_crash_flag);
                            }
                        }
                    }
                }
                this.CreateReportOfProtocolTestResponseRecords(protocol, baseName);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        public void SendAllMessagesWithExclude(string protocol)
        {
            string str = ConfigurationManager.AppSettings["InstalledDirectory"];
            string str2 = DateTime.Now.ToString("MM-dd-yyyy-HH-MM-ss");
            string path = str + @"\Log\ProtocolTest-" + str2 + ".txt";
            this.prtclTSw = new StreamWriter(path);
            List<IDPairs> list = new List<IDPairs>();
            list.Add(new IDPairs(0x81, 0));
            list.Add(new IDPairs(0x80, 0));
            list.Add(new IDPairs(0x81, 0));
            list.Add(new IDPairs(0x85, 0));
            list.Add(new IDPairs(0x86, 0));
            list.Add(new IDPairs(0x87, 0));
            list.Add(new IDPairs(0x88, 0));
            list.Add(new IDPairs(0x89, 0));
            list.Add(new IDPairs(0x92, 0));
            list.Add(new IDPairs(0x94, 0));
            list.Add(new IDPairs(150, 0));
            list.Add(new IDPairs(0xa5, 0));
            list.Add(new IDPairs(0xac, 1));
            list.Add(new IDPairs(0xac, 4));
            list.Add(new IDPairs(0xcd, 0));
            list.Add(new IDPairs(0xe8, 2));
            string filename = str + @"\Protocols\Protocols.xml";
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNodeList list2 = document.SelectNodes("/protocols/protocol[@name='" + protocol + "']/input/message");
            try
            {
                foreach (XmlNode node in list2)
                {
                    int num = int.Parse(node.Attributes["mid"].Value);
                    string msgName = node.Attributes["name"].Value;
                    int num2 = 0;
                    if (node.Attributes["subid"].Value != "")
                    {
                        num2 = int.Parse(node.Attributes["subid"].Value);
                    }
                    foreach (IDPairs pairs in list)
                    {
                        if ((num == pairs.midID) && (num2 == pairs.subID))
                        {
                            goto Label_02A0;
                        }
                    }
                    string str6 = string.Format("mid:{0} subid:{1} name: ", num, num2) + msgName;
                    this.prtclTSw.WriteLine(str6);
                    this.SendMessageWithDefaultData(num, num2, msgName, protocol);
                Label_02A0:
                    Thread.Sleep(0x3e8);
                }
                this.prtclTSw.Close();
            }
            catch (Exception)
            {
                this.prtclTSw.Close();
            }
        }

        public void SendAllMessagesWithInclude(string protocol)
        {
            string str = ConfigurationManager.AppSettings["InstalledDirectory"];
            string str2 = DateTime.Now.ToString("MM-dd-yyyy-HH-MM-ss");
            string path = str + @"\Log\ProtocolTest-" + str2 + ".txt";
            this.prtclTSw = new StreamWriter(path);
            List<IDPairs> list = new List<IDPairs>();
            list.Add(new IDPairs(0x35, 0));
            list.Add(new IDPairs(0x84, 0));
            list.Add(new IDPairs(0x88, 0));
            list.Add(new IDPairs(0x8a, 0));
            list.Add(new IDPairs(140, 0));
            list.Add(new IDPairs(0x8f, 0));
            list.Add(new IDPairs(0x90, 0));
            list.Add(new IDPairs(0x91, 0));
            list.Add(new IDPairs(0x92, 0));
            list.Add(new IDPairs(0x93, 0));
            list.Add(new IDPairs(0x95, 0));
            list.Add(new IDPairs(0x97, 0));
            list.Add(new IDPairs(0x98, 0));
            list.Add(new IDPairs(0xa6, 0));
            list.Add(new IDPairs(0xa7, 0));
            list.Add(new IDPairs(0xa8, 0));
            list.Add(new IDPairs(170, 0));
            list.Add(new IDPairs(0xac, 3));
            list.Add(new IDPairs(0xac, 6));
            list.Add(new IDPairs(0xac, 7));
            list.Add(new IDPairs(0xac, 10));
            list.Add(new IDPairs(0xac, 11));
            string filename = str + @"\Protocols\Protocols.xml";
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNodeList list2 = document.SelectNodes("/protocols/protocol[@name='" + protocol + "']/input/message");
            try
            {
                foreach (XmlNode node in list2)
                {
                    int num = int.Parse(node.Attributes["mid"].Value);
                    string msgName = node.Attributes["name"].Value;
                    int num2 = 0;
                    if (node.Attributes["subid"].Value != "")
                    {
                        num2 = int.Parse(node.Attributes["subid"].Value);
                    }
                    foreach (IDPairs pairs in list)
                    {
                        if ((num == pairs.midID) && (num2 == pairs.subID))
                        {
                            string str6 = string.Format("mid:{0} subid:{1} name: ", num, num2) + msgName;
                            this.prtclTSw.WriteLine(str6);
                            this.SendMessageWithDefaultData(num, num2, msgName, protocol);
                            Thread.Sleep(0x7d0);
                        }
                    }
                }
                this.prtclTSw.Close();
            }
            catch (Exception)
            {
                this.prtclTSw.Close();
            }
        }

        public override void SendApproximatePositionAiding(string site)
        {
            try
            {
                bool flag1 = base._aidingProtocol == "AI3";
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public override void SendFreqAiding()
        {
        }

        public void SendMessageWithDefaultData(int mid, int sid, string msgName, string protocol)
        {
            string csvMessage = string.Empty;
            string msg = string.Empty;
            ArrayList list = new ArrayList();
            StringBuilder builder = new StringBuilder();
            list = base._commWindow.m_Protocols.GetInputMessageStructure(mid, sid, msgName, protocol);
            if (base._commWindow._rxType == CommunicationManager.ReceiverType.SLC)
            {
                builder.Append("238,");
            }
            for (int i = 0; i < list.Count; i++)
            {
                builder.Append(((InputMsg) list[i]).defaultValue);
                builder.Append(",");
            }
            csvMessage = builder.ToString().TrimEnd(new char[] { ',' });
            msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, msgName, protocol);
            base._commWindow.WriteData(msg);
        }

        public void SendMessageWithDefaultDataAndErrorInjection(int mid, int sid, string msgName, string protocol, int errorType)
        {
            string csvMessage = string.Empty;
            string msg = string.Empty;
            ArrayList list = new ArrayList();
            StringBuilder builder = new StringBuilder();
            list = base._commWindow.m_Protocols.GetInputMessageStructure(mid, sid, msgName, protocol);
            if (base._commWindow._rxType == CommunicationManager.ReceiverType.SLC)
            {
                builder.Append("238,");
            }
            for (int i = 0; i < list.Count; i++)
            {
                builder.Append(((InputMsg) list[i]).defaultValue);
                builder.Append(",");
            }
            csvMessage = builder.ToString().TrimEnd(new char[] { ',' });
            msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, msgName, protocol);
            if (errorType == 1)
            {
                base._commWindow.WriteData(msg);
            }
            else if (errorType == 2)
            {
                string str3 = msg.Substring(0, msg.Length - 7);
                uint num2 = Convert.ToUInt32("0x" + msg.Substring(msg.Length - 7, 2), 0x10) + 1;
                str3 = str3 + num2.ToString("X2").PadLeft(2, '0') + " B0B3";
                base._commWindow.WriteData(str3);
            }
            else if (errorType == 4)
            {
                uint num3 = Convert.ToUInt32("0x" + msg.Substring(7, 2), 0x10) + 1;
                string str6 = msg.Substring(10);
                string message = str6.Substring(0, str6.Length - 7);
                string str8 = "A0A2 " + num3.ToString("X").PadLeft(4, '0') + " " + message + " " + GetChecksum(message) + " B0B3";
                base._commWindow.WriteData(str8);
            }
            else if (errorType == 3)
            {
                uint num4 = Convert.ToUInt32("0x" + msg.Substring(7, 2), 0x10) - 1;
                string str10 = msg.Substring(10);
                string str11 = str10.Substring(0, str10.Length - 7);
                string str12 = "A0A2 " + num4.ToString("X").PadLeft(4, '0') + " " + str11 + " " + GetChecksum(str11) + " B0B3";
                base._commWindow.WriteData(str12);
            }
            else if (errorType == 7)
            {
                uint num5 = Convert.ToUInt32("0x" + msg.Substring(7, 2), 0x10);
                string str14 = msg.Substring(13);
                string str15 = "03 " + str14.Substring(0, str14.Length - 7);
                string str16 = "A0A2 " + num5.ToString("X").PadLeft(4, '0') + " " + str15 + " " + GetChecksum(str15) + " B0B3";
                base._commWindow.WriteData(str16);
            }
            else if (errorType == 5)
            {
                uint num6 = Convert.ToUInt32("0x" + msg.Substring(7, 2), 0x10);
                string str18 = msg.Substring(10);
                string str19 = str18.Substring(0, str18.Length - 7);
                string str20 = "A0A2 " + num6.ToString("X").PadLeft(4, '0') + " " + str19 + " " + GetChecksum(str19) + " B0C3";
                base._commWindow.WriteData(str20);
            }
            else if (errorType == 6)
            {
                uint num7 = Convert.ToUInt32("0x" + msg.Substring(7, 2), 0x10);
                string str22 = msg.Substring(10);
                string str23 = str22.Substring(0, str22.Length - 7);
                string str24 = "A0C2 " + num7.ToString("X").PadLeft(4, '0') + " " + str23 + " " + GetChecksum(str23) + " B0B3";
                base._commWindow.WriteData(str24);
            }
        }

        public override void SendPositionRequest(byte flag)
        {
            base._commWindow.WriteApp("Sending Ai3 position request");
            if ((flag & 1) != 1)
            {
                base._commWindow.AutoReplyCtrl.EphDataMsg = string.Empty;
            }
            if ((flag & 2) != 2)
            {
                base._commWindow.AutoReplyCtrl.AcqAssistDataMsg = string.Empty;
            }
            else if (base._commWindow.AutoReplyCtrl.AutoReplyParams.AutoAid_AcqData_fromTTB && base._commWindow.TTBPort.IsOpen)
            {
                string acqAssistDataFromTTB = base._commWindow.GetAcqAssistDataFromTTB();
                if (acqAssistDataFromTTB != string.Empty)
                {
                    base._commWindow.AutoReplyCtrl.AcqAssistDataMsg = acqAssistDataFromTTB;
                }
            }
            else if (base._commWindow.AutoReplyCtrl.AutoReplyParams.AutoAid_AcqData_fromFile)
            {
                double gPSTOW = base._gpsTimer.GetTime().GetGPSTOW();
                string str2 = base._commWindow.AutoReplyCtrl.GetAcqAssistMsgFromFile(base._commWindow.RxCtrl.ControlChannelVersion, base._commWindow.AutoReplyCtrl.AcqDataFilePath, gPSTOW);
                if (str2 != string.Empty)
                {
                    if (str2.Contains("Error"))
                    {
                        base._commWindow.WriteApp("### " + str2 + "###");
                    }
                    else
                    {
                        base._commWindow.AutoReplyCtrl.AcqAssistDataMsg = str2;
                    }
                }
            }
            if ((flag & 4) != 4)
            {
                base._commWindow.AutoReplyCtrl.AlmMsgFromTTB = string.Empty;
            }
            if ((flag & 8) == 8)
            {
                base._commWindow.AutoReplyCtrl.AuxNavMsgFromTTB = string.Empty;
                base._commWindow.AutoReplyCtrl.UTCModelMsgFromTTB = string.Empty;
                base._commWindow.AutoReplyCtrl.GPSTOWAssistFromTTB = string.Empty;
            }
            string str3 = base._commWindow.AutoReplyCtrl.AutoSendPositionRequestMsg();
            string[] separator = new string[] { "\r\n" };
            foreach (string str4 in str3.Split(separator, StringSplitOptions.None))
            {
                if (str4 != "")
                {
                    base._commWindow.WriteData(str4);
                }
            }
            base._commWindow.RxCtrl.ResetCtrl.StartResetTime = DateTime.Now.Ticks;
        }

        public override void SendPositionRequest(string EphSite)
        {
            if ((base._commWindow._rxType == CommunicationManager.ReceiverType.SLC) && (base._aidingProtocol == "AI3"))
            {
                base._commWindow.WriteApp("Sending Ai3 position request");
                GPSDateTime time = base._gpsTimer.GetTime();
                int gPSWeek = time.GetGPSWeek();
                double gPSTOW = time.GetGPSTOW();
                double num3 = (gPSWeek * 0x93a80) + gPSTOW;
                string[] strArray = num3.ToString().Split(new char[] { '.' });
                foreach (string str2 in this._F_AI3.AI3_ConvertInputDataToHex(this._F_AI3.AI3_ICD, 1, 1, EphSite, strArray[0]).Split("\r\n".ToCharArray()))
                {
                    if (str2 != string.Empty)
                    {
                        base._commWindow.WriteData(str2);
                    }
                }
                base._commWindow.AutoReplyCtrl.PosReqAck = false;
            }
        }

        public override void SendReject(int chanID, int msgId, int msgSubId, int reason, string messagName)
        {
            string str3 = messagName;
            if (str3 != null)
            {
                if (!(str3 == "HwCfg"))
                {
                    if (str3 == "TimeCfg")
                    {
                        msgId = 0x11;
                    }
                    else if (str3 == "FreqCfg")
                    {
                        msgId = 0x12;
                    }
                    else if (str3 == "ApproxPosCfg")
                    {
                        msgId = 0x13;
                    }
                }
                else
                {
                    msgId = 0x10;
                }
            }
            string csvMessage = string.Format(clsGlobal.MyCulture, "{0},,{1},{2}", new object[] { chanID, msgId, reason });
            string msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, "Reject", "F");
            base._commWindow.WriteData(msg);
        }

        public override void SendTimeAiding()
        {
            try
            {
                GPSDateTime time = base._gpsTimer.GetTime();
                time.GetGPSWeek();
                time.GetGPSTOW();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void SetAckResponseFlag()
        {
            this.ack_response_flag = Ack_Responses.ack_received;
        }

        public override void SetAidingVersion(string version)
        {
            this._F_AI3.AI3_ICD = version;
        }

        public override void SetClockDrift(string message)
        {
            if ((message != null) && (message.Length != 0))
            {
                byte[] comByte = HelperFunctions.HexToByte(message);
                Hashtable hashtable = base.m_Protocols.ConvertRawToHash(comByte, "SSB");
                if (hashtable.ContainsKey("Clock Drift"))
                {
                    base._clkDrift = Convert.ToUInt32((string) hashtable["Clock Drift"]);
                }
                else
                {
                    base._clkDrift = 0;
                }
            }
        }

        public override void SetControlChannelVersion(string version)
        {
            this._F_AI3.F_ICD = version;
        }

        public override void SetMEMSMode(int state)
        {
            try
            {
                string csvMessage = string.Empty;
                csvMessage = Convert.ToString(string.Concat(new object[] { 0xea, ",", 2, ",", state * 3 }));
                string msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, "Sensor Control Switch", "OSP");
                base._commWindow.WriteData(msg);
            }
            catch
            {
            }
        }

        public void SetNAckResponseFlag()
        {
            this.ack_response_flag = Ack_Responses.nack_received;
        }

        public override void SetPosCalcMode(int state)
        {
            try
            {
                List<string> inputStringList = new List<string>();
                if ((base._commWindow.RxType == CommunicationManager.ReceiverType.SLC) && (base._commWindow.MessageProtocol != "OSP"))
                {
                    inputStringList.Add("238");
                }
                inputStringList.Add("136,0");
                inputStringList.Add(base._commWindow.NavigationParamrters.DegradedMode.ToString());
                inputStringList.Add(state.ToString());
                inputStringList.Add("0");
                inputStringList.Add(base._commWindow.NavigationParamrters.AltitudeSourceInput.ToString());
                inputStringList.Add(base._commWindow.NavigationParamrters.AltitudeHoldMode.ToString());
                inputStringList.Add(base._commWindow.NavigationParamrters.AltitudeHoldSource.ToString());
                inputStringList.Add("0");
                inputStringList.Add(base._commWindow.NavigationParamrters.DegradedTimout.ToString());
                inputStringList.Add(base._commWindow.NavigationParamrters.DRTimeout.ToString());
                inputStringList.Add(base._commWindow.NavigationParamrters.TrackSmoothMode.ToString());
                string csvMessage = HelperFunctions.BuildCSVString(inputStringList);
                string msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, "Mode Control", "SSB");
                base._commWindow.WriteData(msg);
                inputStringList.Clear();
            }
            catch
            {
            }
        }

        public override void SetPowerMode(bool warning)
        {
        }

        public override void SetSBASRangingMode(int state)
        {
            try
            {
                Convert.ToString(0x88);
            }
            catch
            {
            }
        }

        public override void SSBP2PAccuracy()
        {
            try
            {
                if (base._resetCtrl.ResetPositionAvailable)
                {
                    double num = base._commWindow.m_NavData.TOW / 1000.0;
                    string key = string.Format(clsGlobal.MyCulture, "{0:F1}", new object[] { num });
                    if ((key != string.Empty) && base._commWindow.NavTruthDataHash.ContainsKey(key))
                    {
                        ReferenceData data = (ReferenceData) base._commWindow.NavTruthDataHash[key];
                        PositionErrorCalc calc = new PositionErrorCalc();
                        calc.GetPositionErrorsInMeter(base._commWindow.m_NavData.MeasLat, base._commWindow.m_NavData.MeasLon, base._commWindow.m_NavData.MeasAlt, data.DegreeLatitude, data.DegreeLongitude, data.MeterAltitude);
                        double horizontalError = calc.HorizontalError;
                        double num1 = calc.Position3DError;
                        double verticalErrorInMeter = calc.VerticalErrorInMeter;
                        string str = string.Format(clsGlobal.MyCulture, "{0},0,N/A,{1:F1},{2:F1},{3:F1},{4:F2},{5:F2},{6:F6},{7:F6},{8:F6},{9:F6},{10:F6},{11:F6},{12},{13},{14},{15},{16},{17}{18},{19},{20}", new object[] { 
                            num, base._commWindow.m_NavData.TTFFReport, base._commWindow.m_NavData.TTFFSiRFLive, base._commWindow.m_NavData.TTFFReset, base._commWindow.m_NavData.TTFFAided, base._commWindow.m_NavData.TTFFFirstNav, horizontalError, verticalErrorInMeter, base._commWindow.m_NavData.MeasLat, base._commWindow.m_NavData.MeasLon, base._commWindow.m_NavData.MeasAlt, data.DegreeLatitude, data.DegreeLongitude, data.MeterAltitude, base._commWindow.m_NavData.TimeErr, base._commWindow.m_NavData.TimeUncer, 
                            base._commWindow.m_NavData.FreqErr, base._commWindow.m_NavData.FreqUncer, base._commWindow.m_NavData.AvgCNo, base._commWindow.m_NavData.NumSVsInFix, base._commWindow.m_NavData.TOW
                         });
                        base.LogWrite(clsGlobal.P2PAccuracy, str);
                    }
                }
            }
            catch
            {
            }
        }

        public void WriteToProtocolTestFile(string line)
        {
            this.prtclTSw.WriteLine(line);
        }

        public enum Ack_Responses
        {
            none,
            ack_received,
            nack_received
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DataSets
        {
            public byte ValidSensorIndication;
            public uint DataSetTimeTag;
            public ushort OdoSpeed;
            public short Data1;
            public short Data2;
            public short Data3;
            public short Data4;
            public byte Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IDPairs
        {
            public int midID;
            public int subID;
            public IDPairs(int m, int s)
            {
                this.midID = m;
                this.subID = s;
            }
        }

        public class ProtocolTestResults
        {
            public SS3AndGSD3TWReceiver.Ack_Responses ack_resp;
            public int midID;
            public string name;
            public SS3AndGSD3TWReceiver.ProtocolTestTypes protclTestType;
            public int subID;
            public bool system_crash;

            public ProtocolTestResults(int mID, int sID, string nm, SS3AndGSD3TWReceiver.ProtocolTestTypes ptt, SS3AndGSD3TWReceiver.Ack_Responses ar, bool sc)
            {
                this.midID = mID;
                this.subID = sID;
                this.name = nm;
                this.protclTestType = ptt;
                this.ack_resp = ar;
                this.system_crash = sc;
            }
        }

        public enum ProtocolTestTypes
        {
            invalid_checksum = 2,
            invalid_header = 6,
            invalid_ID = 7,
            invalid_trailer = 5,
            normal = 1,
            payload_too_long = 4,
            payload_too_short = 3
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SensorDataSets
        {
            public byte ValidDataIndication;
            public uint DataSetTimeTag;
            public short Data0;
            public short Data1;
            public short Data2;
            public short Data3;
            public short Data4;
            public short Data5;
            public short Data6;
            public short Data7;
            public short Data8;
        }
    }
}

