﻿namespace SiRFLive.MessageHandling
{
    using CommonClassLibrary;
    using SiRFLive.Analysis;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.GUI.DlgsInputMsg;
    using SiRFLive.Utilities;
    using System;
    using System.Collections;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class OSPReceiver : SS3AndGSD3TWReceiver
    {
        private bool isDisposed;

        public OSPReceiver()
        {
            base._messageProtocol = "OSP";
        }

        public override void CloseSession(byte sessionCloseReqInfo)
        {
            ArrayList fieldList = new ArrayList();
            fieldList = utils_AutoReply.GetMessageStructure(base.ControlChannelProtocolFile, CommunicationManager.ReceiverType.OSP, 0xd5, 2, "OSP", base.ControlChannelVersion);
            int num = 0;
            while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "SESSION_CLOSE_REQ_INFO"))
            {
                num++;
            }
            SLCMsgStructure structure = (SLCMsgStructure) fieldList[num];
            structure.defaultValue = sessionCloseReqInfo.ToString();
            fieldList[num] = structure;
            string msg = utils_AutoReply.FieldList_to_HexString(false, fieldList, 0);
            base._commWindow.WriteData(msg);
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
            }
            this.isDisposed = true;
        }

        ~OSPReceiver()
        {
            this.Dispose(false);
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

        public override string FormatInputCarBusDataString(string csvString)
        {
            if (csvString == string.Empty)
            {
                return string.Empty;
            }
            string[] strArray = csvString.Split(new char[] { ',' });
            StringBuilder builder = new StringBuilder();
            byte num = Convert.ToByte(strArray[2]);
            byte num2 = Convert.ToByte(strArray[3]);
            ushort num3 = Convert.ToUInt16(strArray[4]);
            ArrayList list = new ArrayList();
            SS3AndGSD3TWReceiver.DataSets sets = new SS3AndGSD3TWReceiver.DataSets();
            int num4 = 5;
            int num5 = 0;
            int num6 = 0;
            while (num4 < strArray.Length)
            {
                sets.ValidSensorIndication = Convert.ToByte(strArray[num4++]);
                sets.DataSetTimeTag = Convert.ToUInt32(strArray[num4++]);
                sets.OdoSpeed = Convert.ToUInt16(strArray[num4++]);
                sets.Data1 = Convert.ToInt16(strArray[num4++]);
                sets.Data2 = Convert.ToInt16(strArray[num4++]);
                sets.Data3 = Convert.ToInt16(strArray[num4++]);
                sets.Data4 = Convert.ToInt16(strArray[num4++]);
                sets.Reserved = Convert.ToByte(strArray[num4++]);
                list.Add(sets);
                num6++;
            }
            builder.Append("Sensor Pkg: Wheel Speed && Odo\r\n");
            builder.Append(string.Format("# of Valid: {0}\r\n", num2));
            builder.Append(string.Format("Reverse: {0} {0} {0} {0} {0} {0} {0} {0} {0} {0} {0}\r\n\r\n", new object[] { ((num3 & 1) != 0) ? 1 : 0, ((num3 & 2) != 0) ? 1 : 0, ((num3 & 4) != 0) ? 1 : 0, ((num3 & 8) != 0) ? 1 : 0, ((num3 & 0x10) != 0) ? 1 : 0, ((num3 & 0x20) != 0) ? 1 : 0, ((num3 & 0x40) != 0) ? 1 : 0, ((num3 & 0x80) != 0) ? 1 : 0, ((num3 & 0x100) != 0) ? 1 : 0, ((num3 & 0x200) != 0) ? 1 : 0, ((num3 & 0x400) != 0) ? 1 : 0 }));
            switch (num)
            {
                case 1:
                    builder.Append("Sensor Data Type: Gyro, Speed Data, and Reverse\r\n");
                    for (num5 = 0; num5 < num6; num5++)
                    {
                        SS3AndGSD3TWReceiver.DataSets sets2 = (SS3AndGSD3TWReceiver.DataSets) list[num5];
                        builder.Append(string.Format("Time: {0}\t", sets2.DataSetTimeTag));
                        builder.Append(string.Format("Validity: {0}{1}{2}{3}{4}{5}{6}{7}", new object[] { ((sets2.ValidSensorIndication & 0x80) != 0) ? 1 : 0, ((sets2.ValidSensorIndication & 0x40) != 0) ? 1 : 0, ((sets2.ValidSensorIndication & 0x20) != 0) ? 1 : 0, ((sets2.ValidSensorIndication & 0x10) != 0) ? 1 : 0, ((sets2.ValidSensorIndication & 8) != 0) ? 1 : 0, ((sets2.ValidSensorIndication & 4) != 0) ? 1 : 0, ((sets2.ValidSensorIndication & 2) != 0) ? 1 : 0, ((sets2.ValidSensorIndication & 1) != 0) ? 1 : 0 }));
                        builder.Append(string.Format("\tGyro: {0} deg/s", ((double) sets2.Data1) / 100.0));
                        builder.Append(string.Format("\tOdo Spd: {0} m/s", ((double) sets2.OdoSpeed) / 100.0));
                        builder.Append("\r\n\t\tn/a:\t\tn/a\r\n");
                    }
                    break;

                case 2:
                    builder.Append("Sensor Data Type: 4 Wheel Pulses, and Reverse\r\n");
                    for (num5 = 0; num5 < num6; num5++)
                    {
                        SS3AndGSD3TWReceiver.DataSets sets3 = (SS3AndGSD3TWReceiver.DataSets) list[num5];
                        builder.Append(string.Format("Time: {0}\t", sets3.DataSetTimeTag));
                        builder.Append(string.Format("Validity: {0}{1}{2}{3}{4}{5}{6}{7}", new object[] { ((sets3.ValidSensorIndication & 0x80) != 0) ? 1 : 0, ((sets3.ValidSensorIndication & 0x40) != 0) ? 1 : 0, ((sets3.ValidSensorIndication & 0x20) != 0) ? 1 : 0, ((sets3.ValidSensorIndication & 0x10) != 0) ? 1 : 0, ((sets3.ValidSensorIndication & 8) != 0) ? 1 : 0, ((sets3.ValidSensorIndication & 4) != 0) ? 1 : 0, ((sets3.ValidSensorIndication & 2) != 0) ? 1 : 0, ((sets3.ValidSensorIndication & 1) != 0) ? 1 : 0 }));
                        builder.Append(string.Format("\tOdo Spd: {0} m/s", ((double) sets3.OdoSpeed) / 100.0));
                        builder.Append(string.Format("\tRF: {0} pulses", sets3.Data1));
                        builder.Append(string.Format("\r\n\t\tLF: {0} pulses", sets3.Data2));
                        builder.Append(string.Format("\tRR: {0} pulses", sets3.Data3));
                        builder.Append(string.Format("\tLR: {0} pulses\r\n", sets3.Data4));
                    }
                    break;

                case 3:
                    builder.Append("Sensor Data Type: 4 Wheel Speed, and Reverse\r\n");
                    for (num5 = 0; num5 < num6; num5++)
                    {
                        SS3AndGSD3TWReceiver.DataSets sets4 = (SS3AndGSD3TWReceiver.DataSets) list[num5];
                        builder.Append(string.Format("Time: {0}\t", sets4.DataSetTimeTag));
                        builder.Append(string.Format("Validity: {0}{1}{2}{3}{4}{5}{6}{7}", new object[] { ((sets4.ValidSensorIndication & 0x80) != 0) ? 1 : 0, ((sets4.ValidSensorIndication & 0x40) != 0) ? 1 : 0, ((sets4.ValidSensorIndication & 0x20) != 0) ? 1 : 0, ((sets4.ValidSensorIndication & 0x10) != 0) ? 1 : 0, ((sets4.ValidSensorIndication & 8) != 0) ? 1 : 0, ((sets4.ValidSensorIndication & 4) != 0) ? 1 : 0, ((sets4.ValidSensorIndication & 2) != 0) ? 1 : 0, ((sets4.ValidSensorIndication & 1) != 0) ? 1 : 0 }));
                        builder.Append(string.Format("\tOdo Spd: {0} m/s", ((double) sets4.OdoSpeed) / 100.0));
                        builder.Append(string.Format("\tRF: {0} m/s", ((double) sets4.Data1) / 100.0));
                        builder.Append(string.Format("\r\n\t\tLF: {0} m/s", ((double) sets4.Data2) / 100.0));
                        builder.Append(string.Format("\tRR: {0} m/s", ((double) sets4.Data3) / 100.0));
                        builder.Append(string.Format("\tLR: {0} m/s\r\n", ((double) sets4.Data4) / 100.0));
                    }
                    break;

                case 4:
                    builder.Append("Sensor Data Type: 4 Wheel Angular Speed, and Reverse\r\n");
                    for (num5 = 0; num5 < num6; num5++)
                    {
                        SS3AndGSD3TWReceiver.DataSets sets5 = (SS3AndGSD3TWReceiver.DataSets) list[num5];
                        builder.Append(string.Format("Time: {0}\t", sets5.DataSetTimeTag));
                        builder.Append(string.Format("Validity: {0}{1}{2}{3}{4}{5}{6}{7}", new object[] { ((sets5.ValidSensorIndication & 0x80) != 0) ? 1 : 0, ((sets5.ValidSensorIndication & 0x40) != 0) ? 1 : 0, ((sets5.ValidSensorIndication & 0x20) != 0) ? 1 : 0, ((sets5.ValidSensorIndication & 0x10) != 0) ? 1 : 0, ((sets5.ValidSensorIndication & 8) != 0) ? 1 : 0, ((sets5.ValidSensorIndication & 4) != 0) ? 1 : 0, ((sets5.ValidSensorIndication & 2) != 0) ? 1 : 0, ((sets5.ValidSensorIndication & 1) != 0) ? 1 : 0 }));
                        builder.Append(string.Format("\tOdo Spd: {0} m/s", ((double) sets5.OdoSpeed) / 100.0));
                        builder.Append(string.Format("\tRF: {0} rad/s", ((double) sets5.Data1) / 100.0));
                        builder.Append(string.Format("\r\n\t\tLF: {0} rad/s", ((double) sets5.Data2) / 100.0));
                        builder.Append(string.Format("\tRR: {0} rad/s", ((double) sets5.Data3) / 100.0));
                        builder.Append(string.Format("\tLR: {0} rad/s\r\n", ((double) sets5.Data4) / 100.0));
                    }
                    break;

                case 5:
                    builder.Append("Sensor Data Type: Gyro, Speed Data, NO Reverse\r\n");
                    for (num5 = 0; num5 < num6; num5++)
                    {
                        SS3AndGSD3TWReceiver.DataSets sets6 = (SS3AndGSD3TWReceiver.DataSets) list[num5];
                        builder.Append(string.Format("Time: {0}\t", sets6.DataSetTimeTag));
                        builder.Append(string.Format("Validity: {0}{1}{2}{3}{4}{5}{6}{7}", new object[] { ((sets6.ValidSensorIndication & 0x80) != 0) ? 1 : 0, ((sets6.ValidSensorIndication & 0x40) != 0) ? 1 : 0, ((sets6.ValidSensorIndication & 0x20) != 0) ? 1 : 0, ((sets6.ValidSensorIndication & 0x10) != 0) ? 1 : 0, ((sets6.ValidSensorIndication & 8) != 0) ? 1 : 0, ((sets6.ValidSensorIndication & 4) != 0) ? 1 : 0, ((sets6.ValidSensorIndication & 2) != 0) ? 1 : 0, ((sets6.ValidSensorIndication & 1) != 0) ? 1 : 0 }));
                        builder.Append(string.Format("\tOdo Spd: {0} m/s", ((double) sets6.OdoSpeed) / 100.0));
                        builder.Append(string.Format("\tGyro: {0} deg/s", ((double) sets6.Data1) / 100.0));
                        builder.Append("\r\n\t\tn/a:");
                        builder.Append("\tn/a:");
                        builder.Append("\tn/a:\r\n");
                    }
                    break;

                case 6:
                    builder.Append("Sensor Data Type: 4 Wheel Pulses, NO Reverse\r\n");
                    for (num5 = 0; num5 < num6; num5++)
                    {
                        SS3AndGSD3TWReceiver.DataSets sets7 = (SS3AndGSD3TWReceiver.DataSets) list[num5];
                        builder.Append(string.Format("Time: {0}\t", sets7.DataSetTimeTag));
                        builder.Append(string.Format("Validity: {0}{1}{2}{3}{4}{5}{6}{7}", new object[] { ((sets7.ValidSensorIndication & 0x80) != 0) ? 1 : 0, ((sets7.ValidSensorIndication & 0x40) != 0) ? 1 : 0, ((sets7.ValidSensorIndication & 0x20) != 0) ? 1 : 0, ((sets7.ValidSensorIndication & 0x10) != 0) ? 1 : 0, ((sets7.ValidSensorIndication & 8) != 0) ? 1 : 0, ((sets7.ValidSensorIndication & 4) != 0) ? 1 : 0, ((sets7.ValidSensorIndication & 2) != 0) ? 1 : 0, ((sets7.ValidSensorIndication & 1) != 0) ? 1 : 0 }));
                        builder.Append(string.Format("\tOdo Spd: {0} m/s", ((double) sets7.OdoSpeed) / 100.0));
                        builder.Append(string.Format("\tRF: {0} pulses", sets7.Data1));
                        builder.Append(string.Format("\r\n\t\tLF: {0} pulses", sets7.Data2));
                        builder.Append(string.Format("\tRR: {0} pulses", sets7.Data3));
                        builder.Append(string.Format("\tLR: {0} pulses\r\n", sets7.Data4));
                    }
                    break;

                case 7:
                    builder.Append("Sensor Data Type: 4 Wheel Speed, NO Reverse\r\n");
                    for (num5 = 0; num5 < num6; num5++)
                    {
                        SS3AndGSD3TWReceiver.DataSets sets8 = (SS3AndGSD3TWReceiver.DataSets) list[num5];
                        builder.Append(string.Format("Time: {0}\t", sets8.DataSetTimeTag));
                        builder.Append(string.Format("Validity: {0}{1}{2}{3}{4}{5}{6}{7}", new object[] { ((sets8.ValidSensorIndication & 0x80) != 0) ? 1 : 0, ((sets8.ValidSensorIndication & 0x40) != 0) ? 1 : 0, ((sets8.ValidSensorIndication & 0x20) != 0) ? 1 : 0, ((sets8.ValidSensorIndication & 0x10) != 0) ? 1 : 0, ((sets8.ValidSensorIndication & 8) != 0) ? 1 : 0, ((sets8.ValidSensorIndication & 4) != 0) ? 1 : 0, ((sets8.ValidSensorIndication & 2) != 0) ? 1 : 0, ((sets8.ValidSensorIndication & 1) != 0) ? 1 : 0 }));
                        builder.Append(string.Format("\tOdo Spd: {0} m/s", ((double) sets8.OdoSpeed) / 100.0));
                        builder.Append(string.Format("\tRF: {0} m/s", ((double) sets8.Data1) / 100.0));
                        builder.Append(string.Format("\r\n\t\tLF: {0} m/s", ((double) sets8.Data2) / 100.0));
                        builder.Append(string.Format("\tRR: {0} m/s", ((double) sets8.Data3) / 100.0));
                        builder.Append(string.Format("\tLR: {0} m/s\r\n", ((double) sets8.Data4) / 100.0));
                    }
                    break;

                case 8:
                    builder.Append("Sensor Data Type: 4 Wheel Angular Speed, NO Reverse\r\n");
                    for (num5 = 0; num5 < num6; num5++)
                    {
                        SS3AndGSD3TWReceiver.DataSets sets9 = (SS3AndGSD3TWReceiver.DataSets) list[num5];
                        builder.Append(string.Format("Time: {0}\t", sets9.DataSetTimeTag));
                        builder.Append(string.Format("Validity: {0}{1}{2}{3}{4}{5}{6}{7}", new object[] { ((sets9.ValidSensorIndication & 0x80) != 0) ? 1 : 0, ((sets9.ValidSensorIndication & 0x40) != 0) ? 1 : 0, ((sets9.ValidSensorIndication & 0x20) != 0) ? 1 : 0, ((sets9.ValidSensorIndication & 0x10) != 0) ? 1 : 0, ((sets9.ValidSensorIndication & 8) != 0) ? 1 : 0, ((sets9.ValidSensorIndication & 4) != 0) ? 1 : 0, ((sets9.ValidSensorIndication & 2) != 0) ? 1 : 0, ((sets9.ValidSensorIndication & 1) != 0) ? 1 : 0 }));
                        builder.Append(string.Format("\tOdo Spd: {0} m/s", ((double) sets9.OdoSpeed) / 100.0));
                        builder.Append(string.Format("\tRF: {0} rad/s", ((double) sets9.Data1) / 100.0));
                        builder.Append(string.Format("\r\n\t\tLF: {0} rad/s", ((double) sets9.Data2) / 100.0));
                        builder.Append(string.Format("\tRR: {0} rad/s", ((double) sets9.Data3) / 100.0));
                        builder.Append(string.Format("\tLR: {0} rad/s\r\n", ((double) sets9.Data4) / 100.0));
                    }
                    break;

                case 9:
                    builder.Append("Sensor Data Type: Gyro, Speed Data, Reverse, Steering Wheel Angle, Longitudinal Acceleration, Lateral Acceleration\r\n");
                    for (num5 = 0; num5 < num6; num5++)
                    {
                        SS3AndGSD3TWReceiver.DataSets sets10 = (SS3AndGSD3TWReceiver.DataSets) list[num5];
                        builder.Append(string.Format("Time: {0}\t", sets10.DataSetTimeTag));
                        builder.Append(string.Format("Validity: {0}{1}{2}{3}{4}{5}{6}{7}", new object[] { ((sets10.ValidSensorIndication & 0x80) != 0) ? 1 : 0, ((sets10.ValidSensorIndication & 0x40) != 0) ? 1 : 0, ((sets10.ValidSensorIndication & 0x20) != 0) ? 1 : 0, ((sets10.ValidSensorIndication & 0x10) != 0) ? 1 : 0, ((sets10.ValidSensorIndication & 8) != 0) ? 1 : 0, ((sets10.ValidSensorIndication & 4) != 0) ? 1 : 0, ((sets10.ValidSensorIndication & 2) != 0) ? 1 : 0, ((sets10.ValidSensorIndication & 1) != 0) ? 1 : 0 }));
                        builder.Append(string.Format("\tOdo Spd: {0} m/s", ((double) sets10.OdoSpeed) / 100.0));
                        builder.Append(string.Format("\tGyro: {0} deg/s", ((double) sets10.Data1) / 100.0));
                        builder.Append(string.Format("\r\n\t\tSteering Wheel: {0} deg", ((double) sets10.Data2) / 20.0));
                        builder.Append(string.Format("\tLong Acc: {0} m/sec2", ((double) sets10.Data3) / 1000.0));
                        builder.Append(string.Format("\tLat Acc: {0} m/sec2\r\n", ((double) sets10.Data4) / 1000.0));
                    }
                    break;

                case 10:
                    builder.Append("Sensor Pkg: Dashboard DR II\r\n");
                    builder.Append("Sensor Data Type: Yaw Rate Gyro, Down Accel(Z), Long Accel(X), Lat Accel (Y)\r\n");
                    for (num5 = 0; num5 < num6; num5++)
                    {
                        SS3AndGSD3TWReceiver.DataSets sets11 = (SS3AndGSD3TWReceiver.DataSets) list[num5];
                        builder.Append(string.Format("Time: {0}\t", sets11.DataSetTimeTag));
                        builder.Append(string.Format("Validity: {0}{1}{2}{3}{4}{5}{6}{7}", new object[] { ((sets11.ValidSensorIndication & 0x80) != 0) ? 1 : 0, ((sets11.ValidSensorIndication & 0x40) != 0) ? 1 : 0, ((sets11.ValidSensorIndication & 0x20) != 0) ? 1 : 0, ((sets11.ValidSensorIndication & 0x10) != 0) ? 1 : 0, ((sets11.ValidSensorIndication & 8) != 0) ? 1 : 0, ((sets11.ValidSensorIndication & 4) != 0) ? 1 : 0, ((sets11.ValidSensorIndication & 2) != 0) ? 1 : 0, ((sets11.ValidSensorIndication & 1) != 0) ? 1 : 0 }));
                        builder.Append(string.Format("\tOdo Spd: {0} m/s", ((double) sets11.OdoSpeed) / 100.0));
                        builder.Append(string.Format("\tGyro: {0} deg/s", ((double) sets11.Data1) / 100.0));
                        builder.Append(string.Format("\r\n\t\tDown Acc: {0} m/s2", ((double) sets11.Data2) / 1668.0));
                        builder.Append(string.Format("\tLong Acc: {0} m/s2", ((double) sets11.Data3) / 1668.0));
                        builder.Append(string.Format("\tLat Acc: {0} m/s2\r\n", ((double) sets11.Data4) / 1668.0));
                    }
                    break;
            }
            return builder.ToString();
        }

        public override string FormatMeasurementResponse(string csvString)
        {
            if (csvString == string.Empty)
            {
                return string.Empty;
            }
            string[] strArray = csvString.Split(new char[] { ',' });
            int num = 0;
            int num2 = 0;
            StringBuilder builder = new StringBuilder();
            builder.Append("\r\n");
            builder.Append("### Measurement Response ");
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
                builder.Append(" ###\r\n Invalid\r\n");
                return builder.ToString();
            }
            if (num2 == 0)
            {
                int num3 = 0;
                double num4 = 0.0;
                double num5 = 0.0;
                int num6 = 0;
                try
                {
                    num3 = Convert.ToInt32(strArray[5]);
                    num4 = Convert.ToDouble(strArray[6]) / 1000.0;
                    num5 = base.DecodeStdErrICD(Convert.ToByte(strArray[7]));
                    num6 = Convert.ToInt32(strArray[8]);
                    int[] numArray = new int[num6];
                    int[] numArray2 = new int[num6];
                    int[] numArray3 = new int[num6];
                    int[] numArray4 = new int[num6];
                    int[] numArray5 = new int[num6];
                    int[] numArray6 = new int[num6];
                    int[] numArray7 = new int[num6];
                    int index = 0;
                    int num8 = 9;
                    while (index < num6)
                    {
                        numArray[index] = Convert.ToInt32(strArray[num8++]);
                        numArray2[index] = Convert.ToInt32(strArray[num8++]);
                        numArray3[index] = Convert.ToInt32(strArray[num8++]);
                        numArray4[index] = Convert.ToInt32(strArray[num8++]);
                        numArray5[index] = Convert.ToInt32(strArray[num8++]);
                        numArray6[index] = Convert.ToInt32(strArray[num8++]);
                        numArray7[index] = Convert.ToInt32(strArray[num8++]);
                        index++;
                    }
                    builder.Append("\r\nGPS Week: ");
                    builder.Append(num3);
                    builder.Append("\r\nTOW(s): ");
                    builder.Append(string.Format(clsGlobal.MyCulture, "{0:F3}", new object[] { num4 }));
                    builder.Append("\r\nTime Accuracy: ");
                    builder.Append(string.Format(clsGlobal.MyCulture, "{0:F8}", new object[] { num5 }));
                    builder.Append("\r\n  -- Measurement Section --\r\n");
                    builder.Append(string.Format(clsGlobal.MyCulture, "Number of SV's in Track: {0:D}\r\n", new object[] { num6 }));
                    for (int i = 0; i < num6; i++)
                    {
                        builder.Append(string.Format(clsGlobal.MyCulture, "PRN: {0:D2} c_No: {1:D2} Doppler: {2} CP_WH: {3} CP_FR: {4} MP: {5} PseudoRange RMS Error: {6}\r\n", new object[] { numArray[i], numArray2[i], numArray3[i], numArray4[i], numArray5[i], numArray6[i], numArray7[i] }));
                    }
                    return builder.ToString();
                }
                catch
                {
                }
                return string.Empty;
            }
            builder.Append(" ###\r\n");
            switch (num2)
            {
                case 1:
                    builder.Append("Not enough satellites tracked\r\n");
                    break;

                case 2:
                    builder.Append("GPS Aiding data missing(not supported)\r\n");
                    break;

                case 3:
                    builder.Append("Need more time\r\n");
                    break;

                case 0xff:
                    builder.Append("Request Location Method Not Supported\r\n");
                    break;

                default:
                    builder.Append("Unspecified Error\r\n");
                    break;
            }
            return builder.ToString();
        }

        public override string FormatMsgSeven(string logStr)
        {
            if ((logStr != string.Empty) && logStr.Contains(","))
            {
                string[] strArray = logStr.Split(new char[] { ',' });
                return string.Format(clsGlobal.MyCulture, "\r\nWeek:{0}  TOW:{1}  EstGPSTime:{2}  SVCnt:{3}  Clock Drift:{4} Hz  Clock Bias:{5} ns\r\n", new object[] { strArray[1], strArray[2], strArray[6], strArray[3], strArray[4], strArray[5] });
            }
            return string.Empty;
        }

        public override string FormatNavParameters(string csvString)
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
            builder.Append("\r\n");
            if ((num & 8) == 8)
            {
                builder.Append("SBAS Ranging: on");
            }
            else
            {
                builder.Append("SBAS Ranging: off");
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

        public override string FormatPositionResponse(string csvString)
        {
            if (csvString == string.Empty)
            {
                return string.Empty;
            }
            string[] strArray = csvString.Split(new char[] { ',' });
            int num = 0;
            int num2 = 0;
            StringBuilder builder = new StringBuilder();
            builder.Append("\r\n");
            builder.Append("### Position Response ");
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
                builder.Append(" ###\r\n Invalid\r\n");
                return builder.ToString();
            }
            if (num2 == 0)
            {
                int index = 0;
                int num4 = 0;
                int b = 0;
                int num6 = 0;
                double num7 = 0.0;
                double num8 = 0.0;
                double num9 = 0.0;
                int num10 = 0;
                double num11 = 0.0;
                double num12 = 0.0;
                double num13 = 0.0;
                double num14 = 0.0;
                double num15 = 0.0;
                double num16 = 0.0;
                double num17 = 0.0;
                double num18 = 0.0;
                double num19 = 0.0;
                double num20 = 0.0;
                double num21 = 0.0;
                double num22 = 0.0;
                int num23 = 0;
                ushort num24 = 0;
                double clkDrift = 0.0;
                int num26 = 0;
                int num27 = 0;
                string[] strArray2 = new string[] { "QoS Not Met", "QoS Met" };
                string[] strArray3 = new string[] { "No", "Local", "WAAS", "Others" };
                string[] strArray4 = new string[] { "Local", "Reserved" };
                try
                {
                    index = Convert.ToInt32(strArray[5]);
                    num4 = Convert.ToInt32(strArray[6]);
                    b = Convert.ToInt32(strArray[7]);
                    num6 = Convert.ToInt32(strArray[8]);
                    num7 = Convert.ToDouble(strArray[9]) / 1000.0;
                    num8 = (Convert.ToDouble(strArray[10]) * 180.0) / 4294967296;
                    num9 = (Convert.ToDouble(strArray[11]) * 360.0) / 4294967296;
                    num10 = Convert.ToInt32(strArray[12]);
                    num11 = (Convert.ToDouble(strArray[13]) * 180.0) / 256.0;
                    num12 = base.DecodeStdErrICD(Convert.ToByte(strArray[14]));
                    num13 = base.DecodeStdErrICD(Convert.ToByte(strArray[15]));
                    num14 = (Convert.ToDouble(strArray[0x10]) * 0.1) - 500.0;
                    num15 = base.DecodeStdErrICD(Convert.ToByte(strArray[0x11]));
                    num16 = Convert.ToDouble(strArray[0x12]) * 0.0625;
                    num17 = (Convert.ToDouble(strArray[0x13]) * 360.0) / 65536.0;
                    num18 = Convert.ToDouble(strArray[20]) * 0.5;
                    num19 = Convert.ToDouble(strArray[0x15]) * 0.75;
                    num20 = Convert.ToDouble(strArray[0x16]);
                    num21 = Convert.ToDouble(strArray[0x17]);
                    num22 = Convert.ToDouble(strArray[0x18]);
                    num23 = Convert.ToInt32(strArray[0x19]);
                    num24 = Convert.ToUInt16(strArray[0x1a]);
                    clkDrift = base.GetClkDrift(Convert.ToUInt16(strArray[0x1b]));
                    Convert.ToDouble(strArray[0x1c]);
                    num26 = Convert.ToInt32(strArray[0x1d]);
                    num27 = Convert.ToInt32(strArray[30]);
                    int[] numArray = new int[num27];
                    int[] numArray2 = new int[num27];
                    int[] numArray3 = new int[num27];
                    int num28 = 0;
                    int num29 = 0x1f;
                    while (num28 < num27)
                    {
                        numArray[num28] = Convert.ToInt32(strArray[num29++]);
                        numArray2[num28] = Convert.ToInt32(strArray[num29++]);
                        numArray3[num28] = Convert.ToInt32(strArray[num29++]);
                        num28++;
                    }
                    builder.Append("QoS status: ");
                    builder.Append(strArray2[index]);
                    builder.Append("\r\nPos Type:: ");
                    int num30 = num4 & 3;
                    int num31 = (num4 & 0x20) >> 5;
                    switch (num30)
                    {
                        case 0:
                            builder.Append("2D ");
                            break;

                        case 1:
                            builder.Append("3D ");
                            break;

                        default:
                            builder.Append("Reserved ");
                            break;
                    }
                    if (num31 == 0)
                    {
                        builder.Append("Not a trickle power solution\r\n");
                    }
                    else
                    {
                        builder.Append("Trickle power solution(QoP ignored)\r\n");
                    }
                    builder.Append("DGPS Correction: ");
                    builder.Append(strArray3[HelperFunctions.Min2Val(2, b)]);
                    builder.Append("\r\nGPS Week: ");
                    builder.Append(num6);
                    builder.Append("\r\nTOW(s): ");
                    builder.Append(string.Format(clsGlobal.MyCulture, "{0:F3}", new object[] { num7 }));
                    builder.Append("\r\nLat(degrees): ");
                    builder.Append(string.Format(clsGlobal.MyCulture, "{0:F8}", new object[] { num8 }));
                    builder.Append("\r\nLon(degrees): ");
                    builder.Append(string.Format(clsGlobal.MyCulture, "{0:F8}", new object[] { num9 }));
                    if ((num10 & 1) == 1)
                    {
                        builder.Append("\r\n  -- Horizontal Error Section --\r\n");
                        builder.Append(string.Format(clsGlobal.MyCulture, "Error Ellipse Angle(deg): {0:F3}\r\n", new object[] { num11 }));
                        builder.Append(string.Format(clsGlobal.MyCulture, "Error Ellipse Major Axis Std Dev Error(ICD): {0:F5}\r\n", new object[] { num12 }));
                        builder.Append(string.Format(clsGlobal.MyCulture, "Error Ellipse Minor Axis Std Dev Error(ICD): {0:F5}\r\n", new object[] { num13 }));
                    }
                    else
                    {
                        builder.Append(" -- Horizontal Error Section Not Valid --\r\n");
                    }
                    if ((num10 & 2) == 2)
                    {
                        builder.Append("  -- Vertical Position Section --\r\n");
                        builder.Append(string.Format(clsGlobal.MyCulture, "Height(m): {0:F2}\r\n", new object[] { num14 }));
                        builder.Append(string.Format(clsGlobal.MyCulture, "Height Std Dev(ICD): {0:F5}\r\n", new object[] { num15 }));
                    }
                    else
                    {
                        builder.Append("-- Vertical Position Section Not Valid --\r\n");
                    }
                    if ((num10 & 4) == 4)
                    {
                        builder.Append("  -- Velocity Section --\r\n");
                        builder.Append(string.Format(clsGlobal.MyCulture, "Horizontal Velocity(m/s): {0:F2}\r\n", new object[] { num16 }));
                        builder.Append(string.Format(clsGlobal.MyCulture, "Heading(deg): {0:F2}\r\n", new object[] { num17 }));
                        builder.Append(string.Format(clsGlobal.MyCulture, "Vertical Velocity(m/s): {0:F2}\r\n", new object[] { num18 }));
                        builder.Append(string.Format(clsGlobal.MyCulture, "Error Ellipse Major Axis Angle(deg): {0:F2}\r\n", new object[] { num19 }));
                        builder.Append(string.Format(clsGlobal.MyCulture, "Estimated Horizontal Velocity Error Major Axis Std Dev(ICD): {0:F5}\r\n", new object[] { num20 }));
                        builder.Append(string.Format(clsGlobal.MyCulture, "Estimated Horizontal Velocity Error Minor Axis Std Dev(ICD): {0:F5}\r\n", new object[] { num21 }));
                        builder.Append(string.Format(clsGlobal.MyCulture, "Estimated Vertical Velocity Error Std Dev(ICD): {0:F5}\r\n", new object[] { num22 }));
                    }
                    else
                    {
                        builder.Append("  -- Velocity Section Not Valid--\r\n");
                    }
                    if ((num10 & 8) == 8)
                    {
                        builder.Append("  -- Time Reference Section --\r\n");
                        builder.Append(string.Format(clsGlobal.MyCulture, "Time Reference: {0}\r\n", new object[] { strArray4[num23] }));
                        builder.Append(string.Format(clsGlobal.MyCulture, "Clock Bias(ICD): 0x{0:X4}\r\n", new object[] { num24 }));
                        builder.Append(string.Format(clsGlobal.MyCulture, "Clock Drift: {0:F2} ppm  offset: {1:F2} \r\n", new object[] { clkDrift, clkDrift * 1575.42 }));
                        builder.Append(string.Format(clsGlobal.MyCulture, "UTC Offset: {0:D}\r\n", new object[] { num26 }));
                    }
                    else
                    {
                        builder.Append("  -- Time Reference Section Not Valid--\r\n");
                    }
                    if ((num10 & 0x10) == 0x10)
                    {
                        builder.Append("  -- Position Correction Section --\r\n");
                        builder.Append(string.Format(clsGlobal.MyCulture, "Number of SV's in Track: {0:D}\r\n", new object[] { num27 }));
                        for (int i = 0; i < num27; i++)
                        {
                            builder.Append(string.Format(clsGlobal.MyCulture, "PRN: {0:D2}  INV Weight(ICD): 0x{1:X2}   c_No: {2:D2}\r\n", new object[] { numArray[i], numArray3[i], numArray2[i] }));
                        }
                    }
                    else
                    {
                        builder.Append("  -- Position Correction Section Not Valid --\r\n");
                    }
                    return builder.ToString();
                }
                catch
                {
                }
                return string.Empty;
            }
            builder.Append(" ###\r\n");
            switch (num2)
            {
                case 1:
                    builder.Append("Not enough satelites tracked\r\n");
                    break;

                case 2:
                    builder.Append("GPS Aiding data missing(not supported)\r\n");
                    break;

                case 3:
                    builder.Append("Need more time\r\n");
                    break;

                case 4:
                    builder.Append("No Fix available after full search\r\n");
                    break;

                case 6:
                    builder.Append("Position Reporting disable\r\n");
                    break;

                case 7:
                    builder.Append("WiFi Tag Accuracy\r\n");
                    break;

                default:
                    builder.Append("Unspecified Error\r\n");
                    break;
            }
            return builder.ToString();
        }

        public override void GetAutoReplyParameters(string configFilePath)
        {
        }

        public override int GetMeasurement(string logTime, string message)
        {
            int positionFromMeasurement = 0;
            if ((base._commWindow.AutoReplyCtrl.PositionRequestCtrl.LocMethod == 4) && ((base._commWindow.AutoReplyCtrl.AidingFlag & 2) != 2))
            {
                return 1;
            }
            positionFromMeasurement = this.GetPositionFromMeasurement(logTime, message);
            if ((positionFromMeasurement == 0) && (base._commWindow.AutoReplyCtrl.PositionRequestCtrl.LocMethod == 4))
            {
                base._commWindow.RxCtrl.GetTTFF();
            }
            return positionFromMeasurement;
        }

        public override int GetPosition(string logTime, string message)
        {
            if ((message == null) || (message.Length == 0))
            {
                return 1;
            }
            if ((base._commWindow.AutoReplyCtrl.PositionRequestCtrl.LocMethod == 4) && ((base._commWindow.AutoReplyCtrl.AidingFlag & 2) == 2))
            {
                return 1;
            }
            if (!base._resetCtrl.ResetPositionAvailable)
            {
                base._rxNavData.TTFFSiRFLive = ((double) (DateTime.Now.Ticks - base._resetCtrl.StartResetTime)) / 10000000.0;
                if (base._rxNavData.TTFFSiRFLive > 1.0)
                {
                    base._rxNavData.TTFFSiRFLive -= clsGlobal.SiRFLive_TTFF_OFFSET;
                }
            }
            byte[] comByte = HelperFunctions.HexToByte(message);
            Hashtable hashtable = base.m_Protocols.ConvertRawToHash(comByte, "OSP");
            double lat = 0.0;
            double lon = 0.0;
            double alt = 0.0;
            byte num4 = 0;
            PositionErrorCalc calc = new PositionErrorCalc();
            ushort num5 = 0;
            if (hashtable.ContainsKey("POSITION_ERROR_STATUS"))
            {
                num5 = Convert.ToUInt16((string) hashtable["POSITION_ERROR_STATUS"]);
            }
            if (num5 != 0)
            {
                base._resetCtrl.ResetPositionAvailable = false;
                return 1;
            }
            if (hashtable.ContainsKey("MEAS_LAT"))
            {
                lat = (Convert.ToInt32((string) hashtable["MEAS_LAT"]) * 180.0) / 4294967296;
            }
            if (hashtable.ContainsKey("MEAS_LONG"))
            {
                lon = (Convert.ToInt32((string) hashtable["MEAS_LONG"]) * 360.0) / 4294967296;
            }
            if (hashtable.ContainsKey("OTHER SECTIONS"))
            {
                num4 = (byte) (2 & Convert.ToByte((string) hashtable["OTHER SECTIONS"]));
                if (num4 == 2)
                {
                    if (hashtable.ContainsKey("HEIGHT"))
                    {
                        alt = (Convert.ToDouble((string) hashtable["HEIGHT"]) * 0.1) - 500.0;
                    }
                }
                else
                {
                    alt = -9999.0;
                }
            }
            if (hashtable.ContainsKey("MEAS_GPS_SECONDS"))
            {
                base._rxNavData.TOW = Convert.ToDouble((string) hashtable["MEAS_GPS_SECONDS"]);
            }
            if (base._rxNavData.ValidatePosition)
            {
                double num6 = Convert.ToDouble(base._rxNavData.RefLat);
                double num7 = Convert.ToDouble(base._rxNavData.RefLon);
                double num8 = Convert.ToDouble(base._rxNavData.RefAlt);
                calc.GetPositionErrorsInMeter(lat, lon, alt, num6, num7, num8);
                base._rxNavData.Nav2DPositionError = calc.HorizontalError;
                if (num4 == 2)
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
            else
            {
                base._rxNavData.Nav2DPositionError = -9999.0;
                base._rxNavData.Nav3DPositionError = -9999.0;
                base._rxNavData.NavVerticalPositionError = -9999.0;
            }
            base._rxNavData.MeasLat = lat;
            base._rxNavData.MeasLon = lon;
            base._rxNavData.MeasAlt = alt;
            int num9 = 4;
            if ((comByte[num9 + 0x16] & 8) == 8)
            {
                ushort num10 = (ushort) ((comByte[num9 + 0x29] * 0x100) + comByte[num9 + 0x2a]);
                ushort num11 = (ushort) (num10 >> 15);
                ushort num12 = (ushort) ((num10 >> 11) & 15);
                ushort num13 = (ushort) (num10 & 0x7ff);
                if (num11 != 0)
                {
                    base._commWindow.LastAidingSessionReportedClockDrift = (-0.005 * (1.0 + (((double) num13) / 2048.0))) * (((int) 1) << num12);
                }
                else
                {
                    base._commWindow.LastAidingSessionReportedClockDrift = (0.005 * (1.0 + (((double) num13) / 2048.0))) * (((int) 1) << num12);
                }
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

        public override void OpenChannel(string channel)
        {
        }

        public override void OpenSession(byte sessionOpenReqInfo)
        {
            base.AllowSessOpen = false;
            ArrayList fieldList = new ArrayList();
            fieldList = utils_AutoReply.GetMessageStructure(base.ControlChannelProtocolFile, CommunicationManager.ReceiverType.OSP, 0xd5, 1, "OSP", base.ControlChannelVersion);
            int num = 0;
            while ((num < fieldList.Count) && (((SLCMsgStructure) fieldList[num]).fieldName != "SESSION_OPEN_REQ_INFO"))
            {
                num++;
            }
            SLCMsgStructure structure = (SLCMsgStructure) fieldList[num];
            structure.defaultValue = sessionOpenReqInfo.ToString();
            fieldList[num] = structure;
            string msg = utils_AutoReply.FieldList_to_HexString(false, fieldList, 0);
            base._commWindow.WriteData(msg);
        }

        public override void PollSWVersion()
        {
            string csvMessage = string.Empty;
            string msg = string.Empty;
            ArrayList list = new ArrayList();
            StringBuilder builder = new StringBuilder();
            list = base._commWindow.m_Protocols.GetInputMessageStructure(0x84, -1, "Software Version Request", "SSB");
            for (int i = 0; i < list.Count; i++)
            {
                builder.Append(((InputMsg) list[i]).defaultValue);
                builder.Append(",");
            }
            csvMessage = builder.ToString().TrimEnd(new char[] { ',' });
            msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, "Software Version Request", "SSB");
            base._commWindow.WriteData(msg);
        }

        public override void SendApproximatePositionAiding(string site)
        {
        }

        public override void SendFreqAiding()
        {
        }

        public override void SendHWConfig()
        {
        }

        public override void SendMPM_V2(byte timeout, byte control)
        {
            string csvMessage = string.Format(clsGlobal.MyCulture, "218,2,{0},{1},0", new object[] { timeout, control });
            string msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, "Power Mode Request_MPM_REQ_Vers2", "OSP");
            base._commWindow.WriteData(msg);
        }

        public override void SendPositionRequest(byte flag)
        {
            if ((flag & 1) == 1)
            {
                base._commWindow.WriteApp("Sending eph");
                base._commWindow.WriteData(base._commWindow.AutoReplyCtrl.EphDataMsg);
            }
            if ((flag & 2) == 2)
            {
                if (base._commWindow.AutoReplyCtrl.AutoReplyParams.AutoAid_AcqData_fromTTB && base._commWindow.TTBPort.IsOpen)
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
                base._commWindow.WriteApp("Sending Acq data");
                base._commWindow.WriteData(base._commWindow.AutoReplyCtrl.AcqAssistDataMsg);
            }
            if ((flag & 4) == 4)
            {
                base._commWindow.WriteApp("Sending Alm");
                base._commWindow.WriteData(base._commWindow.AutoReplyCtrl.AlmMsgFromTTB);
            }
            if ((flag & 8) == 8)
            {
                base._commWindow.WriteData(base._commWindow.AutoReplyCtrl.AuxNavMsgFromTTB);
                base._commWindow.WriteData(base._commWindow.AutoReplyCtrl.UTCModelMsgFromTTB);
                base._commWindow.WriteData(base._commWindow.AutoReplyCtrl.GPSTOWAssistFromTTB);
                base._commWindow.WriteApp("Sending Aux, UTC and GPSTOWAssist");
            }
            base._commWindow.WriteData(base._commWindow.AutoReplyCtrl.PositionRequestMsg);
            base._commWindow.RxCtrl.ResetCtrl.StartResetTime = DateTime.Now.Ticks;
            base._commWindow.AutoReplyCtrl.PosReqAck = false;
            string[] strArray = new string[] { "MSA", "MSB", "MSB-Prefered", "MSA-Prefered", "Simultanous-MSA-MSB", "Coarse-Location" };
            base._commWindow.WriteApp(string.Format(clsGlobal.MyCulture, "Sending position request {0}  Num Fixes: {1} TBF: {2} Horz Err: {3}, Ver Err: {4}, Priority: {5}, Resp Time: {6}", new object[] { strArray[base._commWindow.AutoReplyCtrl.PositionRequestCtrl.LocMethod], base._commWindow.AutoReplyCtrl.PositionRequestCtrl.NumFixes, base._commWindow.AutoReplyCtrl.PositionRequestCtrl.TimeBtwFixes, base._commWindow.AutoReplyCtrl.PositionRequestCtrl.HorrErrMax, base._commWindow.AutoReplyCtrl.PositionRequestCtrl.VertErrMax, base._commWindow.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority, base._commWindow.AutoReplyCtrl.PositionRequestCtrl.RespTimeMax }));
        }

        public override void SendPositionRequest(string EphSite)
        {
        }

        public override void SendPushAiding(byte availMask, byte reqMask)
        {
            int mid = 0xd3;
            int sid = 9;
            string messageName = "Push Aiding Availability";
            string csvMessage = string.Empty;
            string str3 = string.Empty;
            ArrayList list = new ArrayList();
            StringBuilder builder = new StringBuilder();
            list = base._commWindow.m_Protocols.GetInputMessageStructure(mid, sid, messageName, base._commWindow.MessageProtocol);
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                InputMsg msg = (InputMsg) list[i];
                if (msg.fieldName == "AIDING_AVAILABILITY_MASK")
                {
                    msg.defaultValue = availMask.ToString();
                }
                if (msg.fieldName == "FORCED_AIDING_REQ_MASK")
                {
                    msg.defaultValue = reqMask.ToString();
                }
                if (msg.fieldName == "EST_HOR_ER")
                {
                    msg.defaultValue = utils_AutoReply.metersToICDHorzErr(base._commWindow.AutoReplyCtrl.ApproxPositionCtrl.EstHorrErr).ToString();
                }
                if (msg.fieldName == "EST_VER_ER")
                {
                    msg.defaultValue = ((ushort) (10.0 * base._commWindow.AutoReplyCtrl.ApproxPositionCtrl.EstVertiErr)).ToString();
                }
                if (msg.fieldName == "REL_FREQ_ACC")
                {
                    utils_AutoReply.get_REL_FREQ_ACC(base._commWindow.AutoReplyCtrl.FreqTransferCtrl.Accuracy);
                    msg.defaultValue = base._commWindow.LowPowerParams.APMNumFixes.ToString();
                }
                if (msg.fieldName == "TIME_ACCURACY_SCALE")
                {
                    if (base._commWindow.AutoReplyCtrl.AutoReplyParams.UseTTB_ForTimeAid)
                    {
                        if (base._commWindow.AutoReplyCtrl.TTBTimeAidingParams.Type == 1)
                        {
                            msg.defaultValue = "0";
                        }
                        else if (base._commWindow.AutoReplyCtrl.TTBTimeAidingParams.Type == 0)
                        {
                            msg.defaultValue = "1";
                        }
                        else
                        {
                            msg.defaultValue = "255";
                        }
                    }
                    else if (base._commWindow.AutoReplyCtrl.TimeTransferCtrl.TTType == 0)
                    {
                        msg.defaultValue = "0";
                    }
                    else if (base._commWindow.AutoReplyCtrl.TimeTransferCtrl.TTType == 1)
                    {
                        msg.defaultValue = "1";
                    }
                    else
                    {
                        msg.defaultValue = "255";
                    }
                }
                if (msg.fieldName == "TIME_ACCURACY")
                {
                    double acc = 0.0;
                    byte ttType = 0;
                    if (base._commWindow.AutoReplyCtrl.AutoReplyParams.UseTTB_ForTimeAid)
                    {
                        acc = base._commWindow.AutoReplyCtrl.TTBTimeAidingParams.Accuracy;
                        if (base._commWindow.AutoReplyCtrl.TTBTimeAidingParams.Type == 1)
                        {
                            ttType = 0;
                        }
                        else
                        {
                            ttType = 1;
                        }
                    }
                    else
                    {
                        acc = base._commWindow.AutoReplyCtrl.TimeTransferCtrl.Accuracy;
                        ttType = base._commWindow.AutoReplyCtrl.TimeTransferCtrl.TTType;
                    }
                    msg.defaultValue = utils_AutoReply.EncodeTimeAccuracy(acc, ttType).ToString();
                }
                builder.Append(msg.defaultValue);
                builder.Append(",");
            }
            csvMessage = builder.ToString().TrimEnd(new char[] { ',' });
            str3 = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, messageName, base._commWindow.MessageProtocol);
            base._commWindow.WriteData(str3);
        }

        public override void SendReject(int chanID, int msgId, int msgSubId, int reason, string mesgName)
        {
            string str3 = mesgName;
            if (str3 != null)
            {
                if (!(str3 == "HwCfg"))
                {
                    if (str3 == "TimeCfg")
                    {
                        msgId = 0x49;
                        msgSubId = 2;
                    }
                    else if (str3 == "FreqCfg")
                    {
                        msgId = 0x49;
                        msgSubId = 3;
                    }
                    else if (str3 == "ApproxPosCfg")
                    {
                        msgId = 0x49;
                        msgSubId = 1;
                    }
                }
                else
                {
                    msgId = 0x47;
                    msgSubId = 0;
                }
            }
            string csvMessage = string.Format(clsGlobal.MyCulture, "216,2,{0},{1},{2}", new object[] { msgId, msgSubId, reason });
            string msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, "Reject", "OSP");
            base._commWindow.WriteData(msg);
        }

        public override void SendTimeAiding()
        {
            try
            {
                ushort num = 0;
                ulong num2 = 0L;
                GPSDateTime time = base.GPSTimerEngine.GetTime();
                num = (ushort) (time.GetGPSWeek() + 0x400);
                num2 = ((ulong) time.GetGPSTOW()) + ((ulong) (base._commWindow.AutoReplyCtrl.TimeTransferCtrl.Skew * 100.0));
                base._commWindow.AutoReplyCtrl.TimeTransferCtrl.Reply = true;
                base._commWindow.AutoReplyCtrl.TimeTransferCtrl.WeekNum = num;
                base._commWindow.AutoReplyCtrl.TimeTransferCtrl.TimeOfWeek = num2;
                base._commWindow.AutoReplyCtrl.TimeTransferCtrl.Skew = 0.0;
                base._commWindow.AutoReplyCtrl.AutoReplyTimeTransferResp();
                base._commWindow.AutoReplyCtrl.AutoReplyParams.AutoReply = true;
                base._commWindow.AutoReplyCtrl.AutoReplyParams.AutoReplyTimeTrans = true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public override void SendTrackerConfig()
        {
            string messageName = "Tracker Configuration Version 2.0";
            StringBuilder builder = new StringBuilder();
            try
            {
                builder.Append("178,2,");
                builder.Append(base._commWindow.TrackerICCtrl.RefFreq.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.StartupDelay.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.RefClkUncertainty.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.RefClkOffset.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.LNASelect.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.IOPinConfigEnable.ToString());
                builder.Append(",");
                int num = 11;
                if (base._commWindow.TrackerICCtrl.Version > 1.0)
                {
                    num = 11;
                    messageName = "Tracker Configuration Version 2.0";
                }
                else
                {
                    num = 10;
                    messageName = "Tracker Configuration Version 1.0";
                }
                for (int i = 0; i < num; i++)
                {
                    builder.Append(base._commWindow.TrackerICCtrl.IOPinConfigs[i].ToString());
                    builder.Append(",");
                }
                builder.Append(base._commWindow.TrackerICCtrl.UARTPreambleMax.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.UARTWakeupDelay.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.UARTBaud.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.UARTFlowControlEnable.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.I2CMasterAddress.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.I2CSlaveAddress.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.I2CRate.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.I2CMode.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.I2CMaxMsgLength.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.PwrCtrlOnOff.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.TrackerICCtrl.LDOModeEnabled.ToString());
            }
            catch
            {
            }
            string csvMessage = builder.ToString();
            string msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, messageName, "OSP");
            base._commWindow.WriteData(msg);
        }

        public override void SetCGEEPrediction(uint secondsCGEEdisable)
        {
            string messageName = "Test Mode Configuration Request - SSB_EE_DISABLE_EE_SECS";
            StringBuilder builder = new StringBuilder();
            builder.Append(0xe8);
            builder.Append(",");
            builder.Append(0xfe);
            builder.Append(",");
            builder.Append(secondsCGEEdisable);
            string msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(builder.ToString(), messageName, "OSP");
            base._commWindow.WriteData(msg);
        }

        public override void SetMEMSMode(int state)
        {
            try
            {
                if ((base._commWindow.ProductFamily == CommonClass.ProductType.GSD4e) && (state != 0))
                {
                    string fpath = clsGlobal.InstalledDirectory + @"\Config\SensCfg4e.txt";
                    string mEMsConfigureFromFile = base._commWindow.m_Protocols.GetMEMsConfigureFromFile(fpath);
                    switch (mEMsConfigureFromFile)
                    {
                        case null:
                        case "":
                            base._commWindow.ErrorPrint("Error reading MEMs config file");
                            return;
                    }
                    int num = mEMsConfigureFromFile.Length / 2;
                    string str3 = num.ToString("X").PadLeft(4, '0');
                    string str4 = "A0A2" + str3 + mEMsConfigureFromFile + utils_AutoReply.GetChecksum(mEMsConfigureFromFile, false) + "B0B3";
                    base._commWindow.WriteData(str4);
                    Thread.Sleep(200);
                }
                string csvMessage = string.Empty;
                csvMessage = Convert.ToString(string.Concat(new object[] { 0xea, ",", 2, ",", state * 3 }));
                string msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, "Sensor Control Switch", "OSP");
                base._commWindow.WriteData(msg);
            }
            catch
            {
            }
        }

        public override void SetPowerMode(bool warning)
        {
            int mode = base._commWindow.LowPowerParams.Mode;
            string messageName = string.Empty;
            string csvMessage = string.Empty;
            string str3 = string.Empty;
            switch (mode)
            {
                case 1:
                    messageName = "Power Mode Request_APM_REQ";
                    break;

                case 2:
                    messageName = "Power Mode Request_MPM_REQ";
                    break;

                case 3:
                    messageName = "Power Mode Request_TP_REQ";
                    break;

                case 4:
                    messageName = "Power Mode Request_PTF_REQ";
                    break;

                default:
                    messageName = "Power Mode Request_FP_MODE_REQ";
                    break;
            }
            ArrayList list = new ArrayList();
            StringBuilder builder = new StringBuilder();
            list = base._commWindow.m_Protocols.GetInputMessageStructure(0xda, mode, messageName, base._commWindow.MessageProtocol);
            int count = list.Count;
            switch (mode)
            {
                case 1:
                    messageName = "Power Mode Request_APM_REQ";
                    for (int i = 0; i < count; i++)
                    {
                        InputMsg msg = (InputMsg) list[i];
                        if (msg.fieldName == "NUM_FIXES")
                        {
                            msg.defaultValue = base._commWindow.LowPowerParams.APMNumFixes.ToString();
                        }
                        else if (msg.fieldName == "TBF")
                        {
                            msg.defaultValue = base._commWindow.LowPowerParams.APMTBF.ToString();
                        }
                        else if (msg.fieldName == "DUTY_CYCLE")
                        {
                            msg.defaultValue = base._commWindow.LowPowerParams.APMDutyCycle.ToString();
                        }
                        else if (msg.fieldName == "MAX_HOR_ERR")
                        {
                            msg.defaultValue = base._commWindow.LowPowerParams.APMMaxHrzError.ToString();
                        }
                        else if (msg.fieldName == "MAX_VERT_ERR")
                        {
                            msg.defaultValue = base._commWindow.LowPowerParams.APMMaxVrtError.ToString();
                        }
                        else if (msg.fieldName == "PRIORITY")
                        {
                            msg.defaultValue = base._commWindow.LowPowerParams.APMPriority.ToString();
                        }
                        else if (msg.fieldName == "MAX_OFF_TIME")
                        {
                            msg.defaultValue = base._commWindow.LowPowerParams.APMMaxOffTime.ToString();
                        }
                        else if (msg.fieldName == "MAX_SEARCH_TIME")
                        {
                            msg.defaultValue = base._commWindow.LowPowerParams.APMMaxSearchTime.ToString();
                        }
                        else if (msg.fieldName == "TIME_ACC_PRIORITY")
                        {
                            msg.defaultValue = base._commWindow.LowPowerParams.APMTimeAccPriority.ToString();
                        }
                        builder.Append(msg.defaultValue);
                        builder.Append(",");
                    }
                    break;

                case 2:
                    messageName = "Power Mode Request_MPM_REQ";
                    for (int j = 0; j < count; j++)
                    {
                        InputMsg msg2 = (InputMsg) list[j];
                        builder.Append(msg2.defaultValue);
                        builder.Append(",");
                    }
                    break;

                case 3:
                    messageName = "Power Mode Request_TP_REQ";
                    for (int k = 0; k < count; k++)
                    {
                        InputMsg msg3 = (InputMsg) list[k];
                        if (msg3.fieldName == "DUTY_CYCLE")
                        {
                            string str4 = (((Convert.ToDouble(base._commWindow.LowPowerParams.TPOnTime.ToString()) / Convert.ToDouble(base._commWindow.LowPowerParams.TPUpdateRate.ToString())) / 100.0) * 10.0).ToString();
                            if (str4.Contains(".") && (str4.Split(new char[] { '.' })[1] != null))
                            {
                                str4 = str4.Split(new char[] { '.' })[0] + "." + str4.Split(new char[] { '.' })[1].Substring(0, 1);
                            }
                            msg3.defaultValue = str4;
                        }
                        else if (msg3.fieldName == "ON_TIME")
                        {
                            msg3.defaultValue = base._commWindow.LowPowerParams.TPOnTime.ToString();
                        }
                        else if (msg3.fieldName == "MAX_OFF_TIME")
                        {
                            msg3.defaultValue = base._commWindow.LowPowerParams.TPMaxOffTime.ToString();
                        }
                        else if (msg3.fieldName == "MAX_SEARCH_TIME")
                        {
                            msg3.defaultValue = base._commWindow.LowPowerParams.TPMaxSearchTime.ToString();
                        }
                        builder.Append(msg3.defaultValue);
                        builder.Append(",");
                    }
                    break;

                case 4:
                    messageName = "Power Mode Request_PTF_REQ";
                    for (int m = 0; m < count; m++)
                    {
                        InputMsg msg4 = (InputMsg) list[m];
                        if (msg4.fieldName == "PTF_PERIOD")
                        {
                            msg4.defaultValue = base._commWindow.LowPowerParams.PTFPeriod.ToString();
                        }
                        else if (msg4.fieldName == "MAX_SEARCH_TIME")
                        {
                            msg4.defaultValue = base._commWindow.LowPowerParams.PTFMaxSearchTime.ToString();
                        }
                        else if (msg4.fieldName == "MAX_OFF_TIME")
                        {
                            msg4.defaultValue = base._commWindow.LowPowerParams.PTFMaxOffTime.ToString();
                        }
                        builder.Append(msg4.defaultValue);
                        builder.Append(",");
                    }
                    break;

                default:
                    messageName = "Power Mode Request_FP_MODE_REQ";
                    for (int n = 0; n < count; n++)
                    {
                        InputMsg msg5 = (InputMsg) list[n];
                        builder.Append(msg5.defaultValue);
                        builder.Append(",");
                    }
                    if (base.ResetCtrl != null)
                    {
                        base.ResetCtrl.StartResetTime = DateTime.Now.Ticks;
                        base.RxNavData.ValidatePosition = true;
                    }
                    break;
            }
            csvMessage = builder.ToString().TrimEnd(new char[] { ',' });
            str3 = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, messageName, base._commWindow.MessageProtocol);
            base._commWindow.WriteData(str3);
            if ((warning && (base._commWindow.ProductFamily == CommonClass.ProductType.GSD4e)) && ((mode == 0) && (frmLowPower.priorLowPowerSetting == "APM")))
            {
                MessageBox.Show("Toggle the Pulse Switch on the 4e Rx to immediately exit APM mode then press 'OK'." + "\nOtherwise, wait until APM wakes up the Rx to get back to Full Power mode.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                frmLowPower.priorLowPowerSetting = "";
            }
            if (mode == 1)
            {
                frmLowPower.priorLowPowerSetting = "APM";
            }
        }

        public override void SiRFNavStart()
        {
            string messageName = "SiRFNav Start Version 1.0";
            StringBuilder builder = new StringBuilder();
            try
            {
                builder.Append("161,8,");
                builder.Append(base._commWindow.SiRFNavStartStop.StartMode.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.UARTMaxPreamble.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.UARTIdleByteWakeupDelay.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.ReferenceClockOffset.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.FlowControl.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.LNAType.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.DebugSettings.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.ReferenceClockWarmupDelay.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.ReferenceClockFrequency.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.ReferenceClockUncertainty.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.BaudRate.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.IOPinConfigurationMode.ToString());
                builder.Append(",");
                int num = 11;
                if (base._commWindow.SiRFNavStartStop.Version >= 2.0)
                {
                    messageName = "SiRFNav Start Version 2.0";
                    num = 11;
                }
                else
                {
                    num = 10;
                    messageName = "SiRFNav Start Version 1.0";
                }
                for (int i = 0; i < num; i++)
                {
                    builder.Append(base._commWindow.SiRFNavStartStop.IOPinConfiguration[i].ToString());
                    builder.Append(",");
                }
                builder.Append(base._commWindow.SiRFNavStartStop.I2CHostAddress.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.I2CTrackerAddress.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.I2CMode.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.I2CRate.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.SPIRate.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.ONOffControl.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.FlashMode.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.StorageMode.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.TrackerPort);
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.TrackerPortSeleted.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.WeakSignalEnable.ToString());
                builder.Append(",");
                builder.Append(base._commWindow.SiRFNavStartStop.LDOEnable.ToString());
            }
            catch
            {
            }
            string csvMessage = builder.ToString();
            string msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, messageName, "SSB");
            base._commWindow.RxCtrl.ResetCtrl.ResetDataInit();
            base._commWindow.RxCtrl.NavDataInit();
            base._commWindow.WriteData(msg);
        }

        public override void SiRFNavStop()
        {
            string messageName = "SiRFNav Stop";
            string csvMessage = "161,6," + base._commWindow.SiRFNavStartStop.StopMode.ToString();
            string msg = base._commWindow.m_Protocols.ConvertFieldsToRaw(csvMessage, messageName, "SSB");
            base._commWindow.WriteData(msg);
        }
    }
}

