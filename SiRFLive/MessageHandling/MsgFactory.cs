﻿namespace SiRFLive.MessageHandling
{
    using SiRFLive.General;
    using SiRFLive.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;

    public class MsgFactory : IDisposable
    {
        private ArrayList getOutputTime = new ArrayList();
        private bool isDisposed;
        internal string m_messageProtocol = "SSB";
        private int m_MsgChannelSize;
        private const int m_MsgChecksumSize = 2;
        private Hashtable m_msgHash = new Hashtable();
        private const int m_MsgHeaderSize = 2;
        private int m_MsgLength;
        private const int m_MsgPayloadSize = 2;
        private const int m_MsgTrailerSize = 2;
        private readonly string m_XmlFile;

        public MsgFactory(string xmlFile)
        {
            this.m_XmlFile = xmlFile;
            this.MsgFactoryInit(xmlFile);
        }

        public Hashtable ConvertCSVToHash(int mid, int sid, string protocol, string csvString)
        {
            Hashtable hashtable = new Hashtable();
            ArrayList list = new ArrayList();
            list = this.GetOutputMessageStructure(mid, sid, protocol);
            string[] strArray = csvString.Split(new char[] { ',' });
            int count = list.Count;
            if (strArray.Length < list.Count)
            {
                count = strArray.Length;
            }
            for (int i = 0; i < count; i++)
            {
                OutputMsg msg = (OutputMsg) list[i];
                double num3 = 0.0;
                if (mid != 4)
                {
                    num3 = Convert.ToDouble(strArray[i]) / msg.scale;
                }
                else
                {
                    num3 = Convert.ToDouble(strArray[i]);
                }
                hashtable.Add(msg.fieldName, num3.ToString());
            }
            return hashtable;
        }

        public static string ConvertDecimalToHex(string decimalString, string dataType, double scale)
        {
            double a = 0.0;
            try
            {
                string str;
                char ch;
                string str5;
                int num15;
                if ((dataType != "TEXT") && (dataType != "RAW_HEX"))
                {
                    a = double.Parse(decimalString) * Convert.ToDouble(scale);
                }
                switch (dataType)
                {
                    case "UINT8":
                    {
                        byte num5 = (byte) a;
                        return num5.ToString("X").PadLeft(2, '0');
                    }
                    case "UINT16":
                    {
                        ushort num6 = (ushort) a;
                        return num6.ToString("X").PadLeft(4, '0');
                    }
                    case "UINT24":
                    {
                        a = (uint) Math.Round(a);
                        uint num7 = (uint) a;
                        return num7.ToString("X").PadLeft(6, '0');
                    }
                    case "UINT32":
                    {
                        uint num8 = (uint) a;
                        return num8.ToString("X").PadLeft(8, '0');
                    }
                    case "UINT40":
                    {
                        ulong num9 = (ulong) Math.Round(a);
                        return num9.ToString("X").PadLeft(10, '0');
                    }
                    case "UINT64":
                    {
                        ulong num10 = (ulong) a;
                        return num10.ToString("X").PadLeft(0x10, '0');
                    }
                    case "SINT8":
                    {
                        sbyte num11 = (sbyte) a;
                        return num11.ToString("X").PadLeft(2, '0');
                    }
                    case "SINT16":
                    {
                        short num12 = (short) a;
                        return num12.ToString("X").PadLeft(4, '0');
                    }
                    case "SINT32":
                    {
                        int num13 = (int) a;
                        return num13.ToString("X").PadLeft(8, '0');
                    }
                    case "SINT64":
                    {
                        long num14 = (long) a;
                        return num14.ToString("X").PadLeft(0x10, '0');
                    }
                    case "TEXT":
                        str = "";
                        str5 = decimalString;
                        num15 = 0;
                        goto Label_02D8;

                    case "RAW_HEX":
                        int num3;
                        Math.DivRem(decimalString.Length, 2, out num3);
                        if (num3 != 1)
                        {
                            return decimalString;
                        }
                        return ("0" + decimalString);

                    default:
                    {
                        double num16 = uint.Parse(decimalString) * Convert.ToDouble(scale);
                        return num16.ToString("X").PadLeft(2, '0');
                    }
                }
            Label_0291:
                ch = str5[num15];
                int num2 = ch;
                str = str + string.Format(clsGlobal.MyCulture, "{0:X2}", new object[] { Convert.ToUInt32(num2.ToString()) });
                num15++;
            Label_02D8:
                if (num15 < str5.Length)
                {
                    goto Label_0291;
                }
                return str;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string ConvertFieldsToRaw(string csvMessage, string messageName, string protocol)
        {
            if (protocol == "NMEA")
            {
                string nMEAChecksum = GetNMEAChecksum("PSRF" + csvMessage);
                return ("$PSRF" + csvMessage + "*" + nMEAChecksum);
            }
            return this.GetRawData(csvMessage, messageName, protocol);
        }

        public static string ConvertHexToDecimal(string hexString, string dataType, double scale)
        {
            if (hexString == "")
            {
                return "";
            }
            try
            {
                byte[] buffer2;
                int num;
                int num2;
                StringBuilder builder;
                int num3;
                switch (dataType)
                {
                    case "UINT8":
                    {
                        double num5 = ((double) byte.Parse(hexString, NumberStyles.HexNumber)) / scale;
                        return num5.ToString(clsGlobal.MyCulture);
                    }
                    case "UINT16":
                    {
                        double num6 = ((double) ushort.Parse(hexString, NumberStyles.HexNumber)) / scale;
                        return num6.ToString(clsGlobal.MyCulture);
                    }
                    case "UINT32":
                    {
                        double num7 = ((double) uint.Parse(hexString, NumberStyles.HexNumber)) / scale;
                        return num7.ToString(clsGlobal.MyCulture);
                    }
                    case "UINT64":
                    {
                        double num8 = ((double) ulong.Parse(hexString, NumberStyles.HexNumber)) / scale;
                        return num8.ToString(clsGlobal.MyCulture);
                    }
                    case "SINT8":
                    case "INT8":
                    {
                        double num9 = ((double) sbyte.Parse(hexString, NumberStyles.HexNumber)) / scale;
                        return num9.ToString(clsGlobal.MyCulture);
                    }
                    case "SINT16":
                    case "INT16":
                    {
                        double num10 = ((double) short.Parse(hexString, NumberStyles.HexNumber)) / scale;
                        return num10.ToString(clsGlobal.MyCulture);
                    }
                    case "SINT32":
                    case "INT32":
                    {
                        double num11 = ((double) int.Parse(hexString, NumberStyles.HexNumber)) / scale;
                        return num11.ToString(clsGlobal.MyCulture);
                    }
                    case "SINT64":
                    case "INT64":
                    {
                        double num12 = ((double) long.Parse(hexString, NumberStyles.HexNumber)) / scale;
                        return num12.ToString(clsGlobal.MyCulture);
                    }
                    case "DOUBLE":
                    {
                        byte[] buffer = new byte[hexString.Length / 2];
                        buffer[0] = byte.Parse(hexString.Substring(6, 2), NumberStyles.HexNumber);
                        buffer[1] = byte.Parse(hexString.Substring(4, 2), NumberStyles.HexNumber);
                        buffer[2] = byte.Parse(hexString.Substring(2, 2), NumberStyles.HexNumber);
                        buffer[3] = byte.Parse(hexString.Substring(0, 2), NumberStyles.HexNumber);
                        buffer[4] = byte.Parse(hexString.Substring(14, 2), NumberStyles.HexNumber);
                        buffer[5] = byte.Parse(hexString.Substring(12, 2), NumberStyles.HexNumber);
                        buffer[6] = byte.Parse(hexString.Substring(10, 2), NumberStyles.HexNumber);
                        buffer[7] = byte.Parse(hexString.Substring(8, 2), NumberStyles.HexNumber);
                        return BitConverter.ToDouble(buffer, 0).ToString("e16");
                    }
                    case "SINGLE":
                        buffer2 = new byte[hexString.Length / 2];
                        num = hexString.Length - 2;
                        num2 = 0;
                        goto Label_0388;

                    case "TEXT":
                        builder = new StringBuilder();
                        num3 = 0;
                        goto Label_03D0;

                    default:
                    {
                        double num15 = ((double) uint.Parse(hexString, NumberStyles.HexNumber)) / scale;
                        return num15.ToString(clsGlobal.MyCulture);
                    }
                }
            Label_0368:
                buffer2[num2] = byte.Parse(hexString.Substring(num, 2), NumberStyles.HexNumber);
                num -= 2;
                num2++;
            Label_0388:
                if (num >= 0)
                {
                    goto Label_0368;
                }
                return BitConverter.ToSingle(buffer2, 0).ToString("e7");
            Label_03B1:
                builder.Append((char) Convert.ToUInt32(hexString.Substring(num3, 2), 0x10));
                num3 += 2;
            Label_03D0:
                if (num3 < hexString.Length)
                {
                    goto Label_03B1;
                }
                return builder.ToString();
            }
            catch
            {
                return "0";
            }
        }

        public string ConvertRawToFields(byte[] comByte)
        {
            int msgChannelSize = this.m_MsgChannelSize;
            byte msgId = 0;
            byte subId = 0;
            if (comByte.Length < 6)
            {
                return "";
            }
            uint msgCheckSum = 0;
            uint checksum = 0;
            if (((clsGlobal.DRVersion != "2.0") || (comByte[4] != 0x30)) && !this.VerifyChecksum(comByte, ref checksum, ref msgCheckSum))
            {
                return (" ### INVALID CHECKSUM: Embedded=" + msgCheckSum.ToString("X4") + " Computed=" + checksum.ToString("X4") + ": " + HelperFunctions.ByteToHex(comByte) + " ### \r\n");
            }
            if ((((comByte[4] == 0xff) || ((comByte[4] == 0xee) && (comByte[5] == 0xff))) || ((comByte[4] == 0x44) && (comByte[5] == 0xff))) || (((comByte[4] == 0xee) && (comByte[5] == 0x44)) && (comByte[6] == 0xff)))
            {
                int offset = 5;
                msgId = comByte[4];
                subId = 0;
                if ((comByte[4] == 0xee) || (comByte[4] == 0x44))
                {
                    offset++;
                    if (comByte[4] == 0x44)
                    {
                        msgId = comByte[4];
                        subId = comByte[5];
                    }
                    else
                    {
                        msgId = comByte[5];
                        subId = 0;
                    }
                }
                if (comByte[5] == 0x44)
                {
                    offset++;
                    msgId = comByte[5];
                    subId = comByte[6];
                }
                return DecodeDebugDataMsg(comByte, offset, msgId, subId);
            }
            if ((((comByte[4] == 0xe1) && (comByte[5] == 0)) || (((comByte[4] == 0x44) && (comByte[5] == 0xe1)) && (comByte[6] == 0))) || ((((comByte[4] == 0xee) && (comByte[5] == 0xe1)) && (comByte[6] == 0)) || (((comByte[4] == 0xee) && (comByte[5] == 0x44)) && ((comByte[6] == 0xe1) && (comByte[7] == 0)))))
            {
                int num7 = 6;
                msgId = comByte[4];
                subId = comByte[5];
                if ((comByte[4] == 0xee) || (comByte[4] == 0x44))
                {
                    num7++;
                    if (comByte[4] == 0x44)
                    {
                        msgId = comByte[4];
                        subId = comByte[5];
                    }
                    else
                    {
                        msgId = comByte[5];
                        subId = comByte[6];
                    }
                }
                if (comByte[5] == 0x44)
                {
                    num7++;
                    msgId = comByte[5];
                    subId = comByte[6];
                }
                if (clsGlobal.userAccessNum == "57849679589")
                {
                    return DecodeEncryptedDebugDataMsg(comByte, num7, msgId, subId);
                }
                return LogMarketingEncryptedDebugDataMsg(comByte, num7, msgId, subId);
            }
            if (((comByte[4] == 4) || ((comByte[4] == 0xee) && (comByte[5] == 4))) || ((comByte[4] == 0xcc) && (comByte[5] == 4)))
            {
                return this.DecodeSSBMessage4(comByte);
            }
            if (((comByte[4] == 2) || ((comByte[4] == 0xee) && (comByte[5] == 2))) || ((comByte[4] == 0xcc) && (comByte[5] == 2)))
            {
                return this.DecodeSSBMessage2(comByte);
            }
            if (((comByte[4] == 13) || ((comByte[4] == 0xee) && (comByte[5] == 13))) || ((comByte[4] == 0xcc) && (comByte[5] == 13)))
            {
                return this.DecodeSSBMessage13(comByte);
            }
            if (((comByte[4] == 8) || ((comByte[4] == 0xee) && (comByte[5] == 8))) || ((comByte[4] == 0xcc) && (comByte[5] == 8)))
            {
                return this.DecodeSSBMessage8(comByte);
            }
            if (((comByte[4] == 6) || ((comByte[4] == 0xee) && (comByte[5] == 6))) || ((comByte[4] == 0xcc) && (comByte[5] == 6)))
            {
                int num8 = 5;
                if (this.m_messageProtocol == "OSP")
                {
                    num8 += 2;
                }
                else if ((comByte[4] == 0xee) && (comByte[5] == 6))
                {
                    num8++;
                }
                return DecodeSoftwareVersion(comByte, num8);
            }
            this.m_MsgChannelSize = 0;
            if ((comByte[4] == 0xbb) && (comByte[5] == 6))
            {
                comByte[4] = 0xe1;
                msgChannelSize = this.m_MsgChannelSize;
                this.m_MsgChannelSize = 0;
            }
            string protocol = "SSB";
            if (comByte[4] == 0xee)
            {
                this.m_MsgChannelSize = 1;
            }
            else if (comByte[4] == 0xb1)
            {
                this.m_MsgChannelSize = 1;
                protocol = "LPL";
            }
            else if (comByte[4] == 0xcc)
            {
                this.m_MsgChannelSize = 1;
            }
            ArrayList list = new ArrayList();
            list = this.GetOutputMessageStructure(comByte[4 + this.m_MsgChannelSize], comByte[5 + this.m_MsgChannelSize], protocol);
            if ((list.Count == 0) && (protocol == "SSB"))
            {
                list = this.GetOutputMessageStructure(comByte[4 + this.m_MsgChannelSize], comByte[5 + this.m_MsgChannelSize], "OSP");
            }
            StringBuilder builder = new StringBuilder();
            int count = list.Count;
            if (count == 0)
            {
                string str3 = HelperFunctions.ByteToHex(comByte);
                return ("Can't convert: " + str3);
            }
            for (int i = 0; i < count; i++)
            {
                string str4 = "";
                OutputMsg msg = (OutputMsg) list[i];
                if (msg.endByte <= (comByte.Length - 4))
                {
                    if (msg.referenceField == "")
                    {
                        str4 = this.GetFieldValueInDecimal(comByte, msg.startByte + this.m_MsgChannelSize, msg.endByte + this.m_MsgChannelSize, msg.datatype, msg.scale);
                    }
                    else if (msg.referenceField == "LoopCount")
                    {
                        StringBuilder builder2 = new StringBuilder();
                        int num11 = int.Parse(this.GetFieldValueInDecimal(comByte, msg.startByte + this.m_MsgChannelSize, msg.endByte + this.m_MsgChannelSize, msg.datatype, msg.scale));
                        builder2.Append(num11);
                        builder2.Append(",");
                        int num12 = i++;
                        int num13 = 0;
                        int startByte = 0;
                        int endByte = 0;
                        string str5 = string.Empty;
                        for (int j = 0; j < num11; j++)
                        {
                            num12 = i;
                            msg = (OutputMsg) list[num12];
                            do
                            {
                                msg = (OutputMsg) list[num12++];
                                startByte = (msg.startByte + this.m_MsgChannelSize) + (num13 * j);
                                endByte = (msg.endByte + this.m_MsgChannelSize) + (num13 * j);
                                str5 = this.GetFieldValueInDecimal(comByte, startByte, endByte, msg.datatype, msg.scale);
                                builder2.Append(str5);
                                builder2.Append(",");
                                if (j == 0)
                                {
                                    num13 += (msg.endByte - msg.startByte) + 1;
                                }
                            }
                            while ((num12 < count) && (msg.referenceField != "EndLoop"));
                        }
                        str4 = builder2.ToString();
                        i += num11;
                    }
                    else
                    {
                        string s = "";
                        for (int k = 0; k < count; k++)
                        {
                            OutputMsg msg2 = (OutputMsg) list[k];
                            if (msg2.fieldName == msg.referenceField)
                            {
                                s = this.GetFieldValueInDecimal(comByte, msg2.startByte + this.m_MsgChannelSize, msg2.endByte + this.m_MsgChannelSize, msg2.datatype, msg2.scale);
                                break;
                            }
                        }
                        int num18 = (msg.startByte + this.m_MsgChannelSize) + int.Parse(s);
                        str4 = this.GetFieldValueInDecimal(comByte, msg.startByte + this.m_MsgChannelSize, num18, msg.datatype, msg.scale);
                    }
                    builder.Append(str4);
                    builder.Append(",");
                }
            }
            list.Clear();
            this.m_MsgChannelSize = msgChannelSize;
            return builder.ToString().TrimEnd(new char[] { ',' });
        }

        public string ConvertRawToFieldsForInput(byte[] comByte)
        {
            int msgChannelSize = this.m_MsgChannelSize;
            byte num1 = comByte[4 + this.m_MsgChannelSize];
            byte num5 = comByte[5 + this.m_MsgChannelSize];
            string str = "";
            string str2 = "";
            string protocol = "SSB";
            ArrayList list = new ArrayList();
            list = this.GetInputMsgStructureReadOnly(comByte[4 + this.m_MsgChannelSize], comByte[5 + this.m_MsgChannelSize], protocol);
            if ((list.Count == 0) && (protocol == "SSB"))
            {
                list = this.GetInputMsgStructureReadOnly(comByte[4 + this.m_MsgChannelSize], comByte[5 + this.m_MsgChannelSize], "OSP");
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                OutputMsg msg = (OutputMsg) list[i];
                if (msg.endByte <= (comByte.Length - 4))
                {
                    if (msg.referenceField == "")
                    {
                        str2 = this.GetFieldValueInDecimal(comByte, msg.startByte + this.m_MsgChannelSize, msg.endByte + this.m_MsgChannelSize, msg.datatype, msg.scale);
                    }
                    else
                    {
                        string s = "";
                        for (int j = 0; j < list.Count; j++)
                        {
                            OutputMsg msg2 = (OutputMsg) list[j];
                            if (msg2.fieldName == msg.referenceField)
                            {
                                s = this.GetFieldValueInDecimal(comByte, msg2.startByte + this.m_MsgChannelSize, msg2.endByte + this.m_MsgChannelSize, msg2.datatype, msg2.scale);
                                break;
                            }
                        }
                        int endByte = (msg.startByte + int.Parse(s)) + this.m_MsgChannelSize;
                        str2 = this.GetFieldValueInDecimal(comByte, msg.startByte + this.m_MsgChannelSize, endByte, msg.datatype, msg.scale);
                    }
                    builder.Append(str2);
                    builder.Append(",");
                }
            }
            str = builder.ToString().TrimEnd(new char[] { ',' });
            this.m_MsgChannelSize = msgChannelSize;
            return str;
        }

        public string ConvertRawToFieldsLegacyMsg8(byte[] comByte)
        {
            return "";
        }

        public Hashtable ConvertRawToHash(byte[] comByte, string protocol)
        {
            Hashtable hashtable = new Hashtable();
            int msgChannelSize = this.m_MsgChannelSize;
            byte msgId = 0;
            byte subId = 0;
            if (comByte.Length >= 6)
            {
                uint msgCheckSum = 0;
                uint checksum = 0;
                if (!this.VerifyChecksum(comByte, ref checksum, ref msgCheckSum))
                {
                    return hashtable;
                }
                if ((((comByte[4] == 0xff) || ((comByte[4] == 0xee) && (comByte[5] == 0xff))) || ((comByte[4] == 0x44) && (comByte[5] == 0xff))) || (((comByte[4] == 0xee) && (comByte[5] == 0x44)) && (comByte[6] == 0xff)))
                {
                    int offset = 5;
                    msgId = comByte[4];
                    subId = 0;
                    if ((comByte[4] == 0xee) || (comByte[4] == 0x44))
                    {
                        offset++;
                        if (comByte[4] == 0x44)
                        {
                            msgId = comByte[4];
                            subId = comByte[5];
                        }
                        else
                        {
                            msgId = comByte[5];
                            subId = 0;
                        }
                    }
                    if (comByte[5] == 0x44)
                    {
                        offset++;
                        msgId = comByte[5];
                        subId = comByte[6];
                    }
                    hashtable.Add("Debug", DecodeDebugDataMsg(comByte, offset, msgId, subId));
                    return hashtable;
                }
                if ((((comByte[4] == 0xe1) && (comByte[5] == 0)) || (((comByte[4] == 0x44) && (comByte[5] == 0xe1)) && (comByte[6] == 0))) || ((((comByte[4] == 0xee) && (comByte[5] == 0xe1)) && (comByte[6] == 0)) || (((comByte[4] == 0xee) && (comByte[5] == 0x44)) && ((comByte[6] == 0xe1) && (comByte[7] == 0)))))
                {
                    int num7 = 7;
                    msgId = comByte[4];
                    subId = comByte[5];
                    if ((comByte[4] == 0xee) || (comByte[4] == 0x44))
                    {
                        num7++;
                        if (comByte[4] == 0x44)
                        {
                            msgId = comByte[4];
                            subId = comByte[5];
                        }
                        else
                        {
                            msgId = comByte[5];
                            subId = comByte[6];
                        }
                    }
                    if (comByte[5] == 0x44)
                    {
                        num7++;
                        msgId = comByte[5];
                        subId = comByte[6];
                    }
                    if ((comByte[4] == 0xe1) && (comByte[5] == 0))
                    {
                        num7--;
                        msgId = comByte[4];
                        subId = comByte[5];
                    }
                    if (clsGlobal.userAccessNum == "57849679589")
                    {
                        hashtable.Add("Debug", DecodeEncryptedDebugDataMsg(comByte, num7, msgId, subId));
                        return hashtable;
                    }
                    hashtable.Add("Debug", LogMarketingEncryptedDebugDataMsg(comByte, num7, msgId, subId));
                    return hashtable;
                }
                if ((comByte[4] == 6) || ((comByte[4] == 0xee) && (comByte[5] == 6)))
                {
                    int num8 = 5;
                    if (this.m_messageProtocol == "OSP")
                    {
                        num8 += 2;
                    }
                    else if ((comByte[4] == 0xee) && (comByte[5] == 6))
                    {
                        num8++;
                    }
                    hashtable.Add("Debug", DecodeSoftwareVersion(comByte, num8));
                    return hashtable;
                }
                this.m_MsgChannelSize = 0;
                if ((comByte[4] == 0xbb) && (comByte[5] == 6))
                {
                    comByte[4] = 0xe1;
                    msgChannelSize = this.m_MsgChannelSize;
                    this.m_MsgChannelSize = 0;
                }
                if (comByte[4] == 0xee)
                {
                    this.m_MsgChannelSize = 1;
                }
                else if (comByte[4] == 0xb1)
                {
                    this.m_MsgChannelSize = 1;
                    protocol = "LPL";
                }
                else if (comByte[4] == 0xcc)
                {
                    this.m_MsgChannelSize = 1;
                }
                ArrayList list = new ArrayList();
                list = this.GetOutputMessageStructure(comByte[4 + this.m_MsgChannelSize], comByte[5 + this.m_MsgChannelSize], protocol);
                if ((list.Count == 0) && (protocol == "SSB"))
                {
                    list = this.GetOutputMessageStructure(comByte[4 + this.m_MsgChannelSize], comByte[5 + this.m_MsgChannelSize], "OSP");
                }
                new StringBuilder();
                int count = list.Count;
                if (count != 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        string str = "";
                        OutputMsg msg = (OutputMsg) list[i];
                        if (msg.endByte <= (comByte.Length - 4))
                        {
                            if (msg.referenceField == "")
                            {
                                str = this.GetFieldValueInDecimal(comByte, msg.startByte + this.m_MsgChannelSize, msg.endByte + this.m_MsgChannelSize, msg.datatype, msg.scale);
                                hashtable.Add(msg.fieldName, str);
                                continue;
                            }
                            if (msg.referenceField == "LoopCount")
                            {
                                StringBuilder builder = new StringBuilder();
                                int num11 = int.Parse(this.GetFieldValueInDecimal(comByte, msg.startByte + this.m_MsgChannelSize, msg.endByte + this.m_MsgChannelSize, msg.datatype, msg.scale));
                                builder.Append(num11);
                                builder.Append(",");
                                int num12 = i++;
                                int num13 = 0;
                                int startByte = 0;
                                int num15 = 0;
                                string str2 = string.Empty;
                                for (int k = 0; k < num11; k++)
                                {
                                    num12 = i;
                                    msg = (OutputMsg) list[num12];
                                    do
                                    {
                                        msg = (OutputMsg) list[num12++];
                                        startByte = (msg.startByte + this.m_MsgChannelSize) + (num13 * k);
                                        num15 = (msg.endByte + this.m_MsgChannelSize) + (num13 * k);
                                        str2 = this.GetFieldValueInDecimal(comByte, startByte, num15, msg.datatype, msg.scale);
                                        hashtable.Add(string.Format("{0}{1}", msg.fieldName, k), str2);
                                        if (k == 0)
                                        {
                                            num13 += (msg.endByte - msg.startByte) + 1;
                                        }
                                    }
                                    while ((num12 < count) && (msg.referenceField != "EndLoop"));
                                }
                                i += num11;
                                continue;
                            }
                            string s = "";
                            for (int j = 0; j < count; j++)
                            {
                                OutputMsg msg2 = (OutputMsg) list[j];
                                if (msg2.fieldName == msg.referenceField)
                                {
                                    s = this.GetFieldValueInDecimal(comByte, msg2.startByte + this.m_MsgChannelSize, msg2.endByte + this.m_MsgChannelSize, msg2.datatype, msg2.scale);
                                    break;
                                }
                            }
                            int endByte = (msg.startByte + this.m_MsgChannelSize) + int.Parse(s);
                            str = this.GetFieldValueInDecimal(comByte, msg.startByte + this.m_MsgChannelSize, endByte, msg.datatype, msg.scale);
                            hashtable.Add(msg.fieldName, str);
                        }
                    }
                    this.m_MsgChannelSize = msgChannelSize;
                }
            }
            return hashtable;
        }

        public Hashtable ConvertRawToHash_NMEAMsgs(string nmeaMsg)
        {
            Hashtable hashtable = new Hashtable();
            string mid = nmeaMsg.Substring(1, 5);
            string[] strArray = this.GetOutputMessageStructure_nmea(mid).Split(new char[] { ',' });
            string[] strArray2 = nmeaMsg.Split(new char[] { ',' });
            int length = strArray.GetLength(0);
            int num2 = strArray2.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                string key = strArray[i];
                if (i < num2)
                {
                    string str4 = (strArray2[i] == string.Empty) ? "" : strArray2[i];
                    hashtable.Add(key, str4);
                }
                else
                {
                    hashtable.Add(key, "");
                }
            }
            return hashtable;
        }

        private static string DecodeDebugDataMsg(byte[] comByte, int offset, byte msgId, byte subId)
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                if (msgId != 0xff)
                {
                    builder.Append(msgId.ToString());
                    builder.Append(",");
                    builder.Append(subId.ToString());
                    builder.Append(",");
                }
                int num = comByte.Length - 5;
                for (int i = offset; i <= num; i++)
                {
                    builder.Append(Convert.ToChar(comByte[i]));
                }
                return builder.ToString();
            }
            catch
            {
                return HelperFunctions.ByteToHex(comByte);
            }
        }

        private static string DecodeEncryptedDebugDataMsg(byte[] comByte, int offset, int msgId, int subId)
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                builder.Append(msgId.ToString());
                builder.Append(",");
                builder.Append(subId.ToString());
                builder.Append(",");
                if (msgId == 0x44)
                {
                    builder.Append("0,");
                }
                byte num = 0;
                int num2 = comByte.Length - 5;
                for (int i = offset; i <= num2; i++)
                {
                    num = (byte) (comByte[i] ^ 0xff);
                    builder.Append(Convert.ToChar(num));
                }
                return builder.ToString();
            }
            catch
            {
                return HelperFunctions.ByteToHex(comByte);
            }
        }

        private static string DecodeSoftwareVersion(byte[] comByte, int offset)
        {
            int num = comByte.Length - 5;
            StringBuilder builder = new StringBuilder();
            builder.Append("SW Version: ");
            try
            {
                for (int i = offset; i < num; i++)
                {
                    builder.Append(Convert.ToChar(comByte[i]));
                }
                return builder.ToString().Replace('\0', ' ');
            }
            catch
            {
                return HelperFunctions.ByteToHex(comByte);
            }
        }

        private string DecodeSSBMessage13(byte[] comByte)
        {
            int index = 4;
            int num2 = 0;
            if ((comByte[index] == 0xee) || (comByte[index] == 0xcc))
            {
                num2 = 1;
            }
            ArrayList list = new ArrayList();
            list = this.GetOutputMessageStructure(comByte[index + num2], -1, "SSB");
            StringBuilder builder = new StringBuilder();
            int count = list.Count;
            if (count == 0)
            {
                string str = HelperFunctions.ByteToHex(comByte);
                return ("Can't convert: " + str);
            }
            builder.Append("\nVisible List\n");
            for (int i = 0; i < count; i++)
            {
                string str2 = "";
                OutputMsg msg = (OutputMsg) list[i];
                if (msg.endByte <= (comByte.Length - 4))
                {
                    if (msg.referenceField == "")
                    {
                        str2 = this.GetFieldValueInDecimal(comByte, msg.startByte + num2, msg.endByte + num2, msg.datatype, msg.scale);
                    }
                    else if (msg.referenceField == "LoopCount")
                    {
                        new StringBuilder();
                        int num5 = int.Parse(this.GetFieldValueInDecimal(comByte, msg.startByte + this.m_MsgChannelSize, msg.endByte + this.m_MsgChannelSize, msg.datatype, msg.scale));
                        int num6 = i++;
                        int num7 = 0;
                        int startByte = 0;
                        int endByte = 0;
                        for (int j = 0; j < num5; j++)
                        {
                            num6 = i;
                            msg = (OutputMsg) list[num6];
                            do
                            {
                                msg = (OutputMsg) list[num6++];
                                startByte = (msg.startByte + this.m_MsgChannelSize) + (num7 * j);
                                endByte = (msg.endByte + this.m_MsgChannelSize) + (num7 * j);
                                str2 = this.GetFieldValueInDecimal(comByte, startByte, endByte, msg.datatype, msg.scale);
                                if (j == 0)
                                {
                                    num7 += (msg.endByte - msg.startByte) + 1;
                                }
                                try
                                {
                                    if (msg.fieldName.Contains("SV ID"))
                                    {
                                        builder.Append(string.Format("SVID:{0:D2} ", Convert.ToByte(str2)));
                                    }
                                    else if (msg.fieldName.Contains("SV Azimuth"))
                                    {
                                        builder.Append(string.Format("Az:{0:D3} ", Convert.ToInt16(str2)));
                                    }
                                    else if (msg.fieldName.Contains("SV Elevation"))
                                    {
                                        builder.Append(string.Format("El:{0:D3}\n", Convert.ToInt16(str2)));
                                    }
                                }
                                catch
                                {
                                    builder.Append("-9999");
                                }
                            }
                            while ((num6 < count) && (msg.referenceField != "EndLoop"));
                        }
                        i += num5;
                    }
                }
            }
            list.Clear();
            return builder.ToString().TrimEnd(new char[0]);
        }

        private string DecodeSSBMessage2(byte[] comByte)
        {
            int index = 4;
            int num2 = 0;
            if ((comByte[index] == 0xee) || (comByte[index] == 0xcc))
            {
                num2 = 1;
            }
            ArrayList list = new ArrayList();
            list = this.GetOutputMessageStructure(comByte[index + num2], -1, "SSB");
            StringBuilder builder = new StringBuilder();
            int count = list.Count;
            if (count == 0)
            {
                string str = HelperFunctions.ByteToHex(comByte);
                return ("Can't convert: " + str);
            }
            for (int i = 0; i < count; i++)
            {
                string str2 = "";
                OutputMsg msg = (OutputMsg) list[i];
                if (msg.endByte <= (comByte.Length - 4))
                {
                    if (msg.referenceField == "")
                    {
                        str2 = this.GetFieldValueInDecimal(comByte, msg.startByte + num2, msg.endByte + num2, msg.datatype, msg.scale);
                    }
                    if (msg.fieldName.Contains("Vel"))
                    {
                        try
                        {
                            builder.Append(string.Format("{0:F3}", Convert.ToDouble(str2)));
                        }
                        catch
                        {
                            builder.Append("-9999");
                        }
                    }
                    else if (msg.fieldName == "HDOP")
                    {
                        try
                        {
                            builder.Append(string.Format("{0:F1}", Convert.ToDouble(str2)));
                        }
                        catch
                        {
                            builder.Append("-9999");
                        }
                    }
                    else
                    {
                        builder.Append(str2);
                    }
                    builder.Append(",");
                }
            }
            list.Clear();
            return builder.ToString().TrimEnd(new char[] { ',' });
        }

        private string DecodeSSBMessage4(byte[] comByte)
        {
            int index = 4;
            int num2 = 0;
            if ((comByte[index] == 0xee) || (comByte[index] == 0xcc))
            {
                num2 = 1;
            }
            ArrayList list = new ArrayList();
            list = this.GetOutputMessageStructure(comByte[index + num2], -1, "SSB");
            StringBuilder builder = new StringBuilder();
            int count = list.Count;
            if (count == 0)
            {
                string str = HelperFunctions.ByteToHex(comByte);
                return ("Can't convert: " + str);
            }
            for (int i = 0; i < count; i++)
            {
                string str2 = "";
                OutputMsg msg = (OutputMsg) list[i];
                if (msg.endByte <= (comByte.Length - 4))
                {
                    if (msg.referenceField == "")
                    {
                        str2 = this.GetFieldValueInDecimal(comByte, msg.startByte + num2, msg.endByte + num2, msg.datatype, msg.scale);
                    }
                    if (msg.fieldName.Contains("Azimuth") || msg.fieldName.Contains("Elev"))
                    {
                        try
                        {
                            builder.Append(Math.Round(Convert.ToDouble(str2)));
                        }
                        catch
                        {
                            builder.Append("-9999");
                        }
                    }
                    else
                    {
                        builder.Append(str2);
                    }
                    builder.Append(",");
                }
            }
            list.Clear();
            return builder.ToString().TrimEnd(new char[] { ',' });
        }

        private string DecodeSSBMessage8(byte[] comByte)
        {
            int index = 4;
            int num2 = 0;
            if ((comByte[index] == 0xee) || (comByte[index] == 0xcc))
            {
                num2 = 1;
            }
            ArrayList list = new ArrayList();
            list = this.GetOutputMessageStructure(comByte[index + num2], -1, "SSB");
            StringBuilder builder = new StringBuilder();
            int count = list.Count;
            if (count == 0)
            {
                string str = HelperFunctions.ByteToHex(comByte);
                return ("Can't convert: " + str);
            }
            builder.Append(": ");
            for (int i = 0; i < count; i++)
            {
                string s = "";
                OutputMsg msg = (OutputMsg) list[i];
                if (msg.endByte <= (comByte.Length - 4))
                {
                    if (msg.referenceField == "")
                    {
                        s = this.GetFieldValueInDecimal(comByte, msg.startByte + num2, msg.endByte + num2, msg.datatype, msg.scale);
                    }
                    if (msg.fieldName != "Message ID")
                    {
                        try
                        {
                            if (msg.fieldName.Contains("Word"))
                            {
                                builder.Append(uint.Parse(s, NumberStyles.HexNumber).ToString("X8"));
                                builder.Append(" ");
                            }
                            else
                            {
                                builder.Append(string.Format("{0:D2}", Convert.ToByte(s)));
                                builder.Append(" ");
                            }
                        }
                        catch
                        {
                            builder.Append("-9999");
                        }
                    }
                }
            }
            list.Clear();
            return builder.ToString().TrimEnd(new char[0]);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed && disposing)
            {
                this.m_msgHash.Clear();
                this.m_msgHash = null;
            }
            this.isDisposed = true;
        }

        public string FieldList_to_HexMsgStr(bool isSLCRx, ArrayList fieldList, byte channelType)
        {
            string str = "";
            StringBuilder builder = new StringBuilder();
            int num = 0;
            if (isSLCRx && (channelType != 0))
            {
                num++;
                builder.Append(channelType.ToString("X").PadLeft(2, '0'));
            }
            for (int i = 0; i < fieldList.Count; i++)
            {
                builder.Append(ConvertDecimalToHex(((InputMsg) fieldList[i]).defaultValue, ((InputMsg) fieldList[i]).datatype, ((InputMsg) fieldList[i]).scale));
                num += ((InputMsg) fieldList[i]).bytes;
            }
            str = builder.ToString();
            StringBuilder builder2 = new StringBuilder();
            builder2.Append("A0A2");
            builder2.Append(num.ToString("X").PadLeft(4, '0'));
            builder2.Append(str);
            builder2.Append(this.GetChecksum(str, isSLCRx));
            builder2.Append("B0B3");
            return builder2.ToString();
        }

        public string FieldList_to_HexMsgStr(bool isSLCRx, InputMsg[] fieldList, byte channelType)
        {
            string message = "";
            int num = 0;
            for (int i = 0; i < fieldList.Length; i++)
            {
                message = message + ConvertDecimalToHex(fieldList[i].defaultValue, fieldList[i].datatype, fieldList[i].scale);
                num += fieldList[i].bytes;
            }
            if (isSLCRx && (channelType != 0))
            {
                num++;
                message = channelType.ToString("X").PadLeft(2, '0') + message;
            }
            return ("A0A2" + num.ToString("X").PadLeft(4, '0') + message + this.GetChecksum(message, isSLCRx) + "B0B3");
        }

        ~MsgFactory()
        {
            this.Dispose(false);
        }

        public string GetChecksum(string message, bool slcRx)
        {
            int num = 0;
            string str = message.Replace(" ", "");
            try
            {
                for (int i = 0; i < str.Length; i += 2)
                {
                    num += Convert.ToByte(str.Substring(i, 2), 0x10);
                    num &= 0x7fff;
                }
                if (slcRx && (this.m_messageProtocol != "OSP"))
                {
                    num = (num & 0x7fff) | 0x8000;
                }
                return num.ToString("X2").PadLeft(4, '0');
            }
            catch
            {
                return "0000";
            }
        }

        public ArrayList GetDefaultMsgFieldList(bool isSLCRx, int mid, int sid, string messageName, string protocol)
        {
            ArrayList list = new ArrayList();
            return this.GetInputMessageStructure(mid, sid, messageName, protocol);
        }

        public string GetDefaultMsgtoSend(bool isSLCRx, int mid, int sid, string messageName, string protocol)
        {
            string str3;
            byte channelType = 0;
            string str = protocol;
            if (isSLCRx && ((str3 = protocol) != null))
            {
                if (!(str3 == "SSB"))
                {
                    if (str3 == "TTB")
                    {
                        channelType = 0xcc;
                        str = "SSB";
                    }
                    else if (str3 == "F")
                    {
                        channelType = 2;
                    }
                    else if (str3 == "AI3")
                    {
                        channelType = 1;
                    }
                }
                else
                {
                    channelType = 0xee;
                }
            }
            ArrayList fieldList = new ArrayList();
            fieldList = this.GetInputMessageStructure(mid, sid, messageName, str);
            return this.FieldList_to_HexMsgStr(isSLCRx, fieldList, channelType);
        }

        private string GetFieldValueInDecimal(byte[] comByte, int startByte, int endByte, string dataType, double scale)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = startByte - 1; i <= (endByte - 1); i++)
            {
                builder.Append(Convert.ToString(comByte[i], 0x10).PadLeft(2, '0'));
            }
            if (dataType == "HEX")
            {
                return builder.ToString();
            }
            return ConvertHexToDecimal(builder.ToString(), dataType, scale);
        }

        public List<InputMsg> GetInputMessages(string protocol)
        {
            List<InputMsg> list = new List<InputMsg>();
            InputMsg item = new InputMsg();
            XmlDocument document = new XmlDocument();
            document.Load(this.m_XmlFile);
            XmlNodeList list2 = document.SelectNodes("/protocols/protocol[@name='" + protocol + "']/input/message");
            try
            {
                foreach (XmlNode node in list2)
                {
                    item.messageID = int.Parse(node.Attributes["mid"].Value);
                    try
                    {
                        item.subID = int.Parse(node.Attributes["subid"].Value);
                    }
                    catch (Exception)
                    {
                        item.subID = -1;
                    }
                    item.messageName = node.Attributes["name"].Value;
                    list.Add(item);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            list.Sort(new Comparison<InputMsg>(MsgFactory.InputMsgCompare));
            return list;
        }

        public ArrayList GetInputMessageStructure(int mid, int sid, string messageName, string protocol)
        {
            ArrayList list = new ArrayList();
            string key = string.Empty;
            string str2 = string.Empty;
            if (sid < 0)
            {
                str2 = "NA";
            }
            else
            {
                str2 = sid.ToString();
            }
            key = string.Format(clsGlobal.MyCulture, "{0}_{1}_{2}_{3}_{4}", new object[] { protocol, "input", mid, str2, messageName });
            if (this.m_msgHash.Contains(key))
            {
                ArrayList list2 = (ArrayList) this.m_msgHash[key];
                int num = list2.Count - 1;
                for (int i = 0; i < num; i++)
                {
                    list.Add((InputMsg) list2[i]);
                }
                this.m_MsgLength = (int) list2[num];
                return list;
            }
            if (sid >= 0)
            {
                key = string.Format(clsGlobal.MyCulture, "{0}_{1}_{2}_{3}_{4}", new object[] { protocol, "input", mid, "NA", messageName });
            }
            if (this.m_msgHash.Contains(key))
            {
                ArrayList list3 = (ArrayList) this.m_msgHash[key];
                int num3 = list3.Count - 1;
                for (int j = 0; j < num3; j++)
                {
                    list.Add((InputMsg) list3[j]);
                }
                this.m_MsgLength = (int) list3[num3];
            }
            return list;
        }

        public ArrayList GetInputMsgStructureReadOnly(int mid, int sid, string protocol)
        {
            ArrayList list = new ArrayList();
            OutputMsg msg = new OutputMsg();
            int num = 0;
            XmlDocument document = new XmlDocument();
            document.Load(this.m_XmlFile);
            XmlNodeList list2 = document.SelectNodes(string.Concat(new object[] { "/protocols/protocol[@name='", protocol, "']/input/message[@mid='", mid, "'][@subid = '']/field" }));
            if (list2.Count == 0)
            {
                list2 = document.SelectNodes(string.Concat(new object[] { "/protocols/protocol[@name='", protocol, "']/input/message[@mid='", mid, "'][@subid = '", sid, "']/field" }));
            }
            try
            {
                msg.messageID = mid;
                int num2 = 0;
                foreach (XmlNode node in list2)
                {
                    msg.fieldNumber = ++num2;
                    msg.fieldName = node.Attributes["name"].Value;
                    msg.bytes = int.Parse(node.Attributes["bytes"].Value);
                    msg.datatype = node.Attributes["datatype"].Value;
                    msg.units = node.Attributes["units"].Value;
                    if (node.Attributes["scale"].Value == "")
                    {
                        msg.scale = 1.0;
                    }
                    else
                    {
                        msg.scale = double.Parse(node.Attributes["scale"].Value);
                    }
                    msg.startByte = ((4 + this.m_MsgChannelSize) + 1) + num;
                    msg.endByte = (msg.startByte + msg.bytes) - 1;
                    msg.referenceField = (node.Attributes["referenceField"] != null) ? node.Attributes["referenceField"].Value : (msg.referenceField = "");
                    num += msg.bytes;
                    list.Add(msg);
                }
                this.m_MsgLength = num;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return list;
        }

        public string GetMEMsConfigureFromFile(string fpath)
        {
            if (!File.Exists(fpath))
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            StreamReader reader = new StreamReader(fpath);
            CultureInfo provider = new CultureInfo("en-US");
            string s = string.Empty;
            string str2 = string.Empty;
            int result = 0;
            int startIndex = 0;
            builder.Append("EA01");
            while ((s = reader.ReadLine()) != null)
            {
                if ((!s.StartsWith("program_file") && !s.StartsWith("//")) && !(s == string.Empty))
                {
                    str2 = s.Replace(" ", "");
                    startIndex = str2.IndexOf("/");
                    s = str2.Remove(startIndex).Replace(",", "").Replace("0x", "");
                    if ((s != string.Empty) && int.TryParse(s, NumberStyles.HexNumber, provider, out result))
                    {
                        builder.Append(s);
                    }
                }
            }
            return builder.ToString();
        }

        private static string GetNMEAChecksum(string message)
        {
            byte num = 0;
            for (int i = 0; i < message.Length; i++)
            {
                byte num3 = Convert.ToByte(message[i]);
                num = (byte) (num ^ num3);
            }
            return num.ToString("X2").PadLeft(2, '0');
        }

        public ArrayList GetOutputMessageStructure(int mid, int sid, string protocol)
        {
            ArrayList list = new ArrayList();
            string key = string.Empty;
            key = protocol + "_output_" + mid.ToString() + "_" + sid.ToString();
            if (this.m_msgHash.Contains(key))
            {
                ArrayList list2 = (ArrayList) this.m_msgHash[key];
                int num = list2.Count - 1;
                for (int i = 0; i < num; i++)
                {
                    list.Add((OutputMsg) list2[i]);
                }
                this.m_MsgLength = (int) list2[num];
                return list;
            }
            key = protocol + "_output_" + mid.ToString() + "_NA";
            if (this.m_msgHash.Contains(key))
            {
                ArrayList list3 = (ArrayList) this.m_msgHash[key];
                int num3 = list3.Count - 1;
                for (int j = 0; j < num3; j++)
                {
                    list.Add((OutputMsg) list3[j]);
                }
                this.m_MsgLength = (int) list3[num3];
            }
            return list;
        }

        public string GetOutputMessageStructure_nmea(string mid)
        {
            StringBuilder builder = new StringBuilder();
            XmlDocument document = new XmlDocument();
            document.Load(this.m_XmlFile);
            XmlNodeList list = document.SelectNodes("/protocols/protocol[@name='NMEA']/output/message[@mid='" + mid + "'][@subid = '']/field");
            try
            {
                foreach (XmlNode node in list)
                {
                    builder.Append(node.Attributes["name"].Value + ",");
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return builder.ToString().TrimEnd(",".ToCharArray());
        }

        public ArrayList GetProtocols()
        {
            ArrayList list = new ArrayList();
            XmlDocument document = new XmlDocument();
            document.Load(this.m_XmlFile);
            XmlNodeList list2 = document.SelectNodes("/protocols/protocol");
            try
            {
                foreach (XmlNode node in list2)
                {
                    list.Add(node.Attributes["name"].Value);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return list;
        }

        private string GetRawData(string csvMessage, string messageName, string protocol)
        {
            int num = 0;
            int num2 = 0;
            int mid = 0;
            int sid = -1;
            int num5 = 0;
            bool slcRx = false;
            string[] strArray = csvMessage.Split(new char[] { ',' });
            try
            {
                if ((((protocol == "SSB") && ((int.Parse(strArray[0]) == 0xee) || (int.Parse(strArray[0]) == 0xcc))) || ((protocol == "LPL") || ((protocol == "F") && (int.Parse(strArray[0]) == 2)))) || ((protocol == "AI3") && (int.Parse(strArray[0]) == 1)))
                {
                    num2 = int.Parse(strArray[0]);
                    if ((strArray.Length > 1) && (strArray[1].ToString() != ""))
                    {
                        mid = int.Parse(strArray[1]);
                    }
                    if ((strArray.Length > 2) && (strArray[2].ToString() != ""))
                    {
                        sid = int.Parse(strArray[2]);
                    }
                }
                else
                {
                    mid = int.Parse(strArray[0]);
                    if ((strArray.Length > 1) && (strArray[1].ToString() != ""))
                    {
                        sid = int.Parse(strArray[1]);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error reading message definition", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return "";
            }
            ArrayList list = new ArrayList();
            StringBuilder builder = new StringBuilder();
            list = this.GetInputMessageStructure(mid, sid, messageName, protocol);
            switch (num2)
            {
                case 0xee:
                case 0xcc:
                case 2:
                case 1:
                    num5 = 1;
                    slcRx = true;
                    num++;
                    builder.Append(Convert.ToString(num2, 0x10).PadLeft(2, '0').PadRight(3, ' ').ToUpper());
                    break;
            }
            string str2 = string.Empty;
            InputMsg msg = new InputMsg();
            for (int i = 0; i < list.Count; i++)
            {
                if (strArray[i + num5].Length > 0)
                {
                    msg = (InputMsg) list[i];
                    try
                    {
                        str2 = ConvertDecimalToHex(strArray[i + num5], msg.datatype, msg.scale);
                    }
                    catch
                    {
                        MessageBox.Show((("Problem with input field " + msg.fieldName + "\r\n") + "Datatype must be " + msg.datatype + "\r\n") + "Value entered is " + strArray[i + num5].ToString(), "Error building input message!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return string.Empty;
                    }
                    if (msg.datatype == "TEXT")
                    {
                        int bytes = ((InputMsg) list[i]).bytes;
                        int num8 = str2.Length / 2;
                        if (num8 <= bytes)
                        {
                            builder.Append(" ");
                            builder.Append(str2);
                            num += num8;
                            for (int j = 0; j < (bytes - num8); j++)
                            {
                                builder.Append("00");
                                num++;
                            }
                        }
                        else
                        {
                            builder.Append(" ");
                            builder.Append(str2.Substring(0, bytes * 2));
                            num += strArray[i + num5].Length;
                        }
                    }
                    else if (msg.datatype == "RAW_HEX")
                    {
                        builder.Append(" ");
                        builder.Append(str2);
                        num += str2.Length / 2;
                    }
                    else
                    {
                        builder.Append(" ");
                        builder.Append(str2);
                        num += ((InputMsg) list[i]).bytes;
                    }
                }
            }
            string message = builder.ToString();
            string str5 = "A0A2 " + num.ToString("X").PadLeft(4, '0') + message + " " + this.GetChecksum(message, slcRx) + " B0B3";
            list.Clear();
            return str5;
        }

        private static int InputMsgCompare(InputMsg A, InputMsg B)
        {
            int num = 0;
            if (A.messageID == B.messageID)
            {
                if (A.subID > B.subID)
                {
                    num = 1;
                }
                else if (A.subID < B.subID)
                {
                    num = -1;
                }
            }
            if (A.messageID > B.messageID)
            {
                return 1;
            }
            if (A.messageID < B.messageID)
            {
                num = -1;
            }
            return num;
        }

        private static string LogMarketingEncryptedDebugDataMsg(byte[] comByte, int offset, int msgId, int subId)
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                builder.Append(msgId.ToString());
                if (msgId == 0x44)
                {
                    builder.Append(",");
                    builder.Append(subId.ToString());
                    builder.Append(",00");
                }
                else if (msgId == 0xe1)
                {
                    builder.Append(",00");
                }
                int num = comByte.Length - 5;
                for (int i = offset; i <= num; i++)
                {
                    builder.Append(",");
                    builder.Append(comByte[i].ToString("X2"));
                }
                return builder.ToString();
            }
            catch
            {
                return HelperFunctions.ByteToHex(comByte);
            }
        }

        public void MsgFactoryInit(string xmlFile)
        {
            this.m_msgHash.Clear();
            XmlDocument document = new XmlDocument();
            document.Load(xmlFile);
            foreach (XmlNode node in document.DocumentElement)
            {
                int num = 0;
                string str = node.Attributes["name"].Value;
                switch (str)
                {
                    case "F":
                    case "AI3":
                        num = 1;
                        break;
                }
                foreach (XmlElement element2 in node.ChildNodes)
                {
                    string name = element2.Name;
                    switch (name)
                    {
                        case "output":
                        {
                            OutputMsg msg = new OutputMsg();
                            foreach (XmlElement element3 in element2.ChildNodes)
                            {
                                string attribute = element3.GetAttribute("mid");
                                string str4 = element3.GetAttribute("subid");
                                element3.GetAttribute("name");
                                int num2 = 0;
                                int num3 = 0;
                                int num4 = 0;
                                try
                                {
                                    msg.messageID = Convert.ToInt32(attribute);
                                    int messageID = 0;
                                    if (msg.messageID == 0x45)
                                    {
                                        messageID = msg.messageID;
                                        msg.fieldNumber = messageID;
                                    }
                                    int num6 = 0;
                                    ArrayList list = new ArrayList();
                                    int count = element3.ChildNodes.Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        if (element3.ChildNodes[i].NodeType != XmlNodeType.Comment)
                                        {
                                            XmlElement element4 = (XmlElement) element3.ChildNodes[i];
                                            if (element4.GetAttribute("name").Contains("LOOP"))
                                            {
                                                num2 = int.Parse(element4.GetAttribute("numloops"));
                                                num3 = int.Parse(element4.GetAttribute("numlines"));
                                                int num9 = i + 1;
                                                for (int j = 0; j < num2; j++)
                                                {
                                                    for (int k = 0; k < num3; k++)
                                                    {
                                                        element4 = (XmlElement) element3.ChildNodes[num9 + k];
                                                        msg.fieldNumber = ++num6;
                                                        msg.fieldName = element4.GetAttribute("name") + string.Format("{0}", j + 1);
                                                        msg.bytes = int.Parse(element4.GetAttribute("bytes"));
                                                        msg.datatype = element4.GetAttribute("datatype");
                                                        msg.units = element4.GetAttribute("units");
                                                        try
                                                        {
                                                            msg.scale = (element4.GetAttribute("scale") == "") ? (msg.scale = 1.0) : (msg.scale = double.Parse(element4.GetAttribute("scale")));
                                                        }
                                                        catch
                                                        {
                                                            msg.scale = 1.0;
                                                        }
                                                        msg.startByte = ((4 + num) + 1) + num4;
                                                        msg.endByte = (msg.startByte + msg.bytes) - 1;
                                                        if (element4.Attributes["referenceField"] != null)
                                                        {
                                                            msg.referenceField = element4.GetAttribute("referenceField");
                                                        }
                                                        else
                                                        {
                                                            msg.referenceField = "";
                                                        }
                                                        num4 += msg.bytes;
                                                        list.Add(msg);
                                                    }
                                                }
                                                i += num3;
                                            }
                                            else
                                            {
                                                msg.fieldNumber = ++num6;
                                                msg.fieldName = element4.GetAttribute("name");
                                                msg.bytes = int.Parse(element4.GetAttribute("bytes"));
                                                msg.datatype = element4.GetAttribute("datatype");
                                                msg.units = element4.GetAttribute("units");
                                                try
                                                {
                                                    msg.scale = (element4.GetAttribute("scale") == "") ? (msg.scale = 1.0) : (msg.scale = double.Parse(element4.GetAttribute("scale")));
                                                }
                                                catch
                                                {
                                                    msg.scale = 1.0;
                                                }
                                                msg.startByte = ((4 + num) + 1) + num4;
                                                msg.endByte = (msg.startByte + msg.bytes) - 1;
                                                if (element4.Attributes["referenceField"] != null)
                                                {
                                                    msg.referenceField = element4.GetAttribute("referenceField");
                                                }
                                                else
                                                {
                                                    msg.referenceField = "";
                                                }
                                                num4 += msg.bytes;
                                                list.Add(msg);
                                            }
                                        }
                                    }
                                    if (str4 == string.Empty)
                                    {
                                        str4 = "NA";
                                    }
                                    list.Add(num4);
                                    string key = str + "_" + name + "_" + attribute + "_" + str4;
                                    this.m_msgHash.Add(key, list);
                                }
                                catch
                                {
                                }
                            }
                            break;
                        }
                        case "input":
                        {
                            InputMsg msg2 = new InputMsg();
                            foreach (XmlElement element5 in element2.ChildNodes)
                            {
                                string str7 = element5.GetAttribute("mid");
                                string str8 = element5.GetAttribute("subid");
                                string str9 = element5.GetAttribute("name");
                                int num12 = 0;
                                int num13 = 0;
                                int num14 = 0;
                                try
                                {
                                    msg2.messageID = Convert.ToInt32(str7);
                                    int num15 = 0;
                                    ArrayList list2 = new ArrayList();
                                    int num16 = element5.ChildNodes.Count;
                                    for (int m = 0; m < num16; m++)
                                    {
                                        if (element5.ChildNodes[m].NodeType == XmlNodeType.Comment)
                                        {
                                            continue;
                                        }
                                        XmlElement element6 = (XmlElement) element5.ChildNodes[m];
                                        if (element6.GetAttribute("name").Contains("LOOP"))
                                        {
                                            num12 = int.Parse(element6.GetAttribute("numloops"));
                                            num13 = int.Parse(element6.GetAttribute("numlines"));
                                            int num18 = m + 1;
                                            for (int n = 0; n < num12; n++)
                                            {
                                                for (int num20 = 0; num20 < num13; num20++)
                                                {
                                                    element6 = (XmlElement) element5.ChildNodes[num18 + num20];
                                                    msg2.fieldNumber = ++num15;
                                                    msg2.fieldName = element6.GetAttribute("name");
                                                    msg2.bytes = int.Parse(element6.GetAttribute("bytes"));
                                                    msg2.datatype = element6.GetAttribute("datatype");
                                                    msg2.units = element6.GetAttribute("units");
                                                    if (element6.GetAttribute("scale") == "")
                                                    {
                                                        msg2.scale = 1.0;
                                                    }
                                                    else
                                                    {
                                                        msg2.scale = double.Parse(element6.GetAttribute("scale"));
                                                    }
                                                    switch (str)
                                                    {
                                                        case "NMEA":
                                                        case "LPL":
                                                            msg2.defaultValue = element6.GetAttribute("default");
                                                            break;

                                                        default:
                                                            if (element6.GetAttribute("default") != null)
                                                            {
                                                                if (msg2.datatype != "RAW")
                                                                {
                                                                    msg2.defaultValue = ConvertHexToDecimal(element6.GetAttribute("default"), msg2.datatype, msg2.scale);
                                                                }
                                                                else
                                                                {
                                                                    msg2.defaultValue = element6.GetAttribute("default");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                msg2.defaultValue = "0";
                                                            }
                                                            break;
                                                    }
                                                    if (element6.GetAttribute("savedValue") != null)
                                                    {
                                                        msg2.savedValue = element6.GetAttribute("savedValue");
                                                    }
                                                    else
                                                    {
                                                        msg2.savedValue = "0";
                                                    }
                                                    num14 += msg2.bytes;
                                                    list2.Add(msg2);
                                                }
                                            }
                                            m += num13;
                                            continue;
                                        }
                                        msg2.fieldNumber = ++num15;
                                        msg2.fieldName = element6.GetAttribute("name");
                                        msg2.bytes = int.Parse(element6.GetAttribute("bytes"));
                                        msg2.datatype = element6.GetAttribute("datatype");
                                        msg2.units = element6.GetAttribute("units");
                                        if (element6.GetAttribute("scale") == "")
                                        {
                                            msg2.scale = 1.0;
                                        }
                                        else
                                        {
                                            msg2.scale = double.Parse(element6.GetAttribute("scale"));
                                        }
                                        switch (str)
                                        {
                                            case "NMEA":
                                            case "LPL":
                                                msg2.defaultValue = element6.GetAttribute("default");
                                                break;

                                            default:
                                                if (element6.GetAttribute("default") != null)
                                                {
                                                    if (msg2.datatype != "RAW")
                                                    {
                                                        msg2.defaultValue = ConvertHexToDecimal(element6.GetAttribute("default"), msg2.datatype, msg2.scale);
                                                    }
                                                    else
                                                    {
                                                        msg2.defaultValue = element6.GetAttribute("default");
                                                    }
                                                }
                                                else
                                                {
                                                    msg2.defaultValue = "0";
                                                }
                                                break;
                                        }
                                        if (element6.GetAttribute("savedValue") != null)
                                        {
                                            msg2.savedValue = element6.GetAttribute("savedValue");
                                        }
                                        else
                                        {
                                            msg2.savedValue = "0";
                                        }
                                        num14 += msg2.bytes;
                                        list2.Add(msg2);
                                    }
                                    if (str8 == string.Empty)
                                    {
                                        str8 = "NA";
                                    }
                                    list2.Add(num14);
                                    string str11 = string.Format(clsGlobal.MyCulture, "{0}_{1}_{2}_{3}_{4}", new object[] { str, name, str7, str8, str9 });
                                    this.m_msgHash.Add(str11, list2);
                                }
                                catch
                                {
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        public void SaveInputDataToFile(string message, string messageName, string protocol)
        {
            if ((File.GetAttributes(this.m_XmlFile) & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
            {
                string[] strArray = new string[100];
                strArray = message.Split(new char[] { ',' });
                XmlDocument document = new XmlDocument();
                document.Load(this.m_XmlFile);
                XmlNodeList list = document.SelectNodes("/protocols/protocol[@name='" + protocol + "']/input/message[@mid='" + strArray[0] + "'][@name = '" + messageName + "']/field");
                try
                {
                    int index = 0;
                    foreach (XmlNode node in list)
                    {
                        node.Attributes["savedValue"].Value = strArray[index].ToString();
                        index++;
                    }
                    document.Save(this.m_XmlFile);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("MsgFactory: SaveInputDataToFile() - " + exception.ToString(), "Error");
                }
            }
        }

        private bool VerifyChecksum(byte[] comByte, ref uint checksum, ref uint msgCheckSum)
        {
            int length = comByte.Length;
            int index = 0;
            for (int i = 4; i < (length - 4); i++)
            {
                checksum += comByte[i];
                checksum &= 0x7fff;
            }
            index = length - 4;
            msgCheckSum = (uint) ((comByte[index] << 8) + comByte[index + 1]);
            return (checksum == msgCheckSum);
        }
    }
}

