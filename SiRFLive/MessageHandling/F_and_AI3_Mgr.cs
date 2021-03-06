﻿namespace SiRFLive.MessageHandling
{
    using SiRFLive.Reporting;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.XPath;

    public class F_and_AI3_Mgr
    {
        private string _ai3ICD;
        private string _fICD;
        private int m_MsgChannelSize;
        private int m_MsgHeaderSize;
        private int m_MsgPayloadSize;
        private XPathDocument m_XPathDoc;
        private XPathNavigator m_XPathNav;

        public F_and_AI3_Mgr()
        {
            this.m_MsgHeaderSize = 2;
            this.m_MsgPayloadSize = 2;
            this.m_MsgChannelSize = 1;
            this._ai3ICD = "2.2";
            this._fICD = "2.1";
        }

        public F_and_AI3_Mgr(string xmlFile)
        {
            this.m_MsgHeaderSize = 2;
            this.m_MsgPayloadSize = 2;
            this.m_MsgChannelSize = 1;
            this._ai3ICD = "2.2";
            this._fICD = "2.1";
            try
            {
                this.m_XPathDoc = new XPathDocument(xmlFile);
                this.m_XPathNav = this.m_XPathDoc.CreateNavigator();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string AI3_ConvertInputDataToHex(string AI3_Version, int msgtype, int msgid, string EphSite, string gpsTimeStr)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(msgtype.ToString());
            builder.Append(",");
            builder.Append(msgid.ToString());
            ArrayList list = new ArrayList();
            list = this.SLC_GetMessageStructure(msgid, 0, "AI3", "2.2");
            char[] separator = new char[] { ',' };
            string[] strArray = new string[850];
            string str = this.AI3_ReadEphFile_AllEph(EphSite, gpsTimeStr);
            if (str == "")
            {
                for (int k = 0; k < 800; k++)
                {
                    strArray[k] = "0";
                }
            }
            else
            {
                strArray = str.Split(separator);
            }
            int num2 = 0;
            if (AI3_Version == "2.1")
            {
                while ((num2 < list.Count) && (((SLCMsgStructure) list[num2]).fieldName != "ICD_REV_NUM"))
                {
                    num2++;
                }
                SLCMsgStructure structure = (SLCMsgStructure) list[num2];
                structure.defaultValue = "33";
                list[num2] = structure;
                while ((num2 < list.Count) && (((SLCMsgStructure) list[num2]).fieldName != "HORI_ERROR_MAX"))
                {
                    num2++;
                }
                SLCMsgStructure structure2 = (SLCMsgStructure) list[num2];
                switch (structure2.defaultValue)
                {
                    case "1":
                        structure2.defaultValue = "0";
                        break;

                    case "5":
                        structure2.defaultValue = "1";
                        break;

                    case "10":
                        structure2.defaultValue = "2";
                        break;

                    case "20":
                        structure2.defaultValue = "3";
                        break;

                    case "40":
                        structure2.defaultValue = "4";
                        break;

                    case "80":
                        structure2.defaultValue = "5";
                        break;

                    case "160":
                        structure2.defaultValue = "6";
                        break;

                    default:
                        structure2.defaultValue = "7";
                        break;
                }
                list[num2] = structure2;
            }
            while ((num2 < list.Count) && (((SLCMsgStructure) list[num2]).fieldName != "1st EPH_FLAG"))
            {
                num2++;
            }
            int num3 = num2;
            for (int i = 0; num3 < (num2 + strArray.GetLength(0)); i++)
            {
                SLCMsgStructure structure3 = (SLCMsgStructure) list[num3];
                structure3.defaultValue = strArray[i];
                list[num3] = structure3;
                num3++;
            }
            for (int j = 0; j < list.Count; j++)
            {
                if (string.IsNullOrEmpty(((SLCMsgStructure) list[j]).defaultValue))
                {
                    builder.Append(",0");
                }
                else
                {
                    builder.Append(",");
                    builder.Append(((SLCMsgStructure) list[j]).defaultValue);
                }
            }
            string str2 = this.AI3_Request_ConvertFieldsToRaw(builder.ToString());
            list.Clear();
            return str2;
        }

        private string AI3_ReadEphFile(string site)
        {
            StreamReader reader = File.OpenText(ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Eph\" + site + @"\Eph.ai3eph");
            string str2 = null;
            string str3 = null;
            while ((str3 = reader.ReadLine()) != null)
            {
                str2 = str2 + "1, " + str3;
            }
            reader.Close();
            return str2;
        }

        private string AI3_ReadEphFile_AllEph(string site, string gpsTimeStr)
        {
            StreamReader reader = File.OpenText(ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Eph\" + site + @"\Eph.ai3eph");
            string str2 = "";
            string input = "";
            Regex regex = new Regex("[0-9]+");
            Regex regex2 = new Regex("[0-9]+,");
            int num = 0;
            int num2 = Convert.ToInt32(gpsTimeStr);
            bool flag = false;
            while ((((input = reader.ReadLine()) != null) && !input.Contains("End of File")) && !flag)
            {
                if (input.Contains("Ephemeris Data at GPS time"))
                {
                    Match match = regex.Match(input);
                    if (match.ToString() != "")
                    {
                        num = Convert.ToInt32(match.Value);
                        if ((num2 >= num) && (num2 < (num + 0x1c20)))
                        {
                            while (((input = reader.ReadLine()) != null) && (regex2.Match(input).ToString() == ""))
                            {
                            }
                            str2 = str2 + "1, " + input;
                            while (((input = reader.ReadLine()) != null) && (regex2.Match(input).ToString() != ""))
                            {
                                str2 = str2 + "1, " + input;
                            }
                            flag = true;
                        }
                    }
                }
            }
            string str4 = "";
            if (str2.Length >= 3)
            {
                str4 = str2.Substring(0, str2.Length - 2);
            }
            reader.Close();
            return str4;
        }

        private string AI3_Request_ConvertFieldsToRaw(string csvMessage)
        {
            int num2 = 0;
            string hex = "";
            char[] separator = new char[] { ',' };
            string[] strArray = new string[0x7d0];
            strArray = csvMessage.Split(separator);
            int.Parse(strArray[0]);
            int mid = int.Parse(strArray[1]);
            ArrayList list = new ArrayList();
            list = this.SLC_GetMessageStructure(mid, 0, "AI3", "2.2");
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                builder.Append(this.SLC_ConvertDecimalToHex(strArray[i + 2], ((SLCMsgStructure) list[i]).datatype, ((SLCMsgStructure) list[i]).scale));
            }
            hex = builder.ToString().Replace(" ", "");
            string str2 = "";
            byte[] buffer = this.HexStringToByteArray(hex);
            buffer[0xcab] = 8;
            byte[] buffer2 = this.compressMsg(buffer, 0xcac);
            int length = buffer2.GetLength(0);
            byte num5 = (byte) (length / 0x3f8);
            int num6 = length % 0x3f8;
            if (0 < (length % 0x3f8))
            {
                num5 = (byte) (num5 + 1);
            }
            byte[] bytes = new byte[0x3fc];
            bytes[0] = 1;
            bytes[1] = 1;
            bytes[2] = num5;
            byte[] buffer4 = new byte[num6 + 4];
            buffer4[0] = 1;
            buffer4[1] = 1;
            buffer4[2] = num5;
            for (byte j = 0; j < num5; j = (byte) (j + 1))
            {
                if (j < (num5 - 1))
                {
                    bytes[3] = (byte) (j + 1);
                    for (int k = 0; k < 0x3f8; k++)
                    {
                        bytes[k + 4] = buffer2[(j * 0x3f8) + k];
                    }
                    num2 = 0x3fc;
                    string sentence = this.ByteArrayToHexString(bytes);
                    string str4 = "A0A2" + num2.ToString("X").PadLeft(4, '0') + sentence + this.GetChecksum(sentence) + "B0B3";
                    str2 = str2 + str4 + "\r\n";
                }
                else
                {
                    buffer4[3] = (byte) (j + 1);
                    for (int m = 0; m < num6; m++)
                    {
                        buffer4[m + 4] = buffer2[(j * 0x3f8) + m];
                    }
                    num2 = num6 + 4;
                    string str5 = this.ByteArrayToHexString(buffer4);
                    string str6 = "A0A2" + num2.ToString("X").PadLeft(4, '0') + str5 + this.GetChecksum(str5) + "B0B3";
                    str2 = str2 + str6 + "\r\n";
                }
            }
            list.Clear();
            return str2;
        }

        public string ByteArrayToHexString(byte[] Bytes)
        {
            string str = "0123456789ABCDEF";
            StringBuilder builder = new StringBuilder();
            foreach (byte num in Bytes)
            {
                builder.Append(str[num >> 4]);
                builder.Append(str[num & 15]);
            }
            return builder.ToString();
        }

        private string ByteToGP2(int type, byte[] comByte)
        {
            StringBuilder builder = new StringBuilder(comByte.Length * 3);
            byte num = 0;
            byte num2 = 0;
            foreach (byte num3 in comByte)
            {
                num2 = num3;
                if (num == 160)
                {
                    if (num2 == 0xa2)
                    {
                        builder.Append("\n");
                        DateTime now = DateTime.Now;
                        builder.AppendFormat("{0:D2}/{1:D2}/{2:D4} {3:D2}:{4:D2}:{5:D2}.{6:D3} ({7})\t", new object[] { now.Month, now.Day, now.Year, now.Hour, now.Minute, now.Second, now.Millisecond, type });
                    }
                    builder.Append(Convert.ToString(num, 0x10).PadLeft(2, '0').PadRight(3, ' '));
                }
                switch (num2)
                {
                    case 160:
                    {
                        builder.Append(Convert.ToString(num3, 0x10).PadLeft(2, '0').PadRight(3, ' '));
						break;
                    }
                }
            }
            return builder.ToString().ToUpper();
        }

        private byte[] compressMsg(byte[] in_buf, int in_len)
        {
            byte[] buffer = new byte[0xe10];
            int index = 0;
            int num = 0;
            int num2 = 0;
            int num4 = 0;
            int num5 = 0;
            bool flag = false;
            bool flag2 = false;
            while (num5 < (in_len - 1))
            {
                if (in_buf[num5] == in_buf[num5 + 1])
                {
                    num++;
                    if (num < 4)
                    {
                        num2++;
                    }
                    else
                    {
                        flag2 = true;
                        if ((num2 >= num) && flag)
                        {
                            buffer[num4++] = (byte) (((num2 - num) + 1) >> 8);
                            buffer[num4++] = (byte) (((num2 - num) + 1) & 0xff);
                            int num6 = num4;
                            int num7 = index;
                            for (int j = 0; j < ((num2 - num) + 1); j++)
                            {
                                buffer[num6] = in_buf[num7];
                                num6++;
                                num7++;
                            }
                            index = ((index + num2) - num) + 1;
                            num4 = ((num4 + num2) - num) + 1;
                            flag = false;
                        }
                        num2 = 0;
                    }
                }
                else
                {
                    if (flag2)
                    {
                        num++;
                        flag2 = false;
                    }
                    else
                    {
                        num2++;
                    }
                    if (num >= 4)
                    {
                        if ((num2 >= num) && flag)
                        {
                            buffer[num4++] = (byte) (((num2 - num) + 1) >> 8);
                            buffer[num4++] = (byte) (((num2 - num) + 1) & 0xff);
                            int num9 = num4;
                            int num10 = index;
                            for (int k = 0; k < ((num2 - num) + 1); k++)
                            {
                                buffer[num9] = in_buf[num10];
                                num9++;
                                num10++;
                            }
                            index = ((index + num2) - num) + 1;
                            num4 = ((num4 + num2) - num) + 1;
                            flag = false;
                        }
                        num2 = 0;
                        buffer[num4++] = (byte) (0x80 | (num >> 8));
                        buffer[num4++] = (byte) (num & 0xff);
                        buffer[num4++] = in_buf[index];
                        index += num;
                    }
                    num = 0;
                    flag = true;
                }
                num5++;
            }
            if (num >= 4)
            {
                buffer[num4++] = (byte) (0x80 | ((num + 1) >> 8));
                buffer[num4++] = (byte) ((num + 1) & 0xff);
                buffer[num4++] = in_buf[index];
            }
            else
            {
                buffer[num4++] = (byte) ((num2 + 1) >> 8);
                buffer[num4++] = (byte) ((num2 + 1) & 0xff);
                int num12 = num4;
                int num13 = index;
                for (int m = 0; m < (num2 + 1); m++)
                {
                    buffer[num12] = in_buf[num13];
                    num12++;
                    num13++;
                }
                num4 = (num4 + num2) + 1;
            }
            byte[] buffer2 = new byte[num4];
            for (int i = 0; i < num4; i++)
            {
                buffer2[i] = buffer[i];
            }
            return buffer2;
        }

        public uint decmprss(byte[] input, uint input_length, byte[] output, uint max_out_len)
        {
            byte[] buffer = new byte[2];
            uint index = 0;
            uint num4 = 0;
            while ((index < input_length) && (num4 < max_out_len))
            {
                buffer[0] = input[index];
                buffer[1] = input[(int) ((IntPtr) (index + 1))];
                index += 2;
                ushort num = (ushort) (((buffer[0] & 0x7f) * 0x100) + buffer[1]);
                if ((buffer[0] & 0x80) == 0x80)
                {
                    for (ushort i = 0; i < num; i = (ushort) (i + 1))
                    {
                        output[num4 + i] = input[index];
                    }
                    num4 += num;
                    index++;
                }
                else
                {
                    for (ushort j = 0; j < num; j = (ushort) (j + 1))
                    {
                        output[num4 + j] = input[index + j];
                    }
                    num4 += num;
                    index += num;
                }
            }
            if ((index < input_length) && (num4 >= max_out_len))
            {
                return 0;
            }
            return num4;
        }

        private string GetChecksum(string sentence)
        {
            int num = 0;
            string str = sentence.Replace(" ", "");
            for (int i = 0; i < str.Length; i += 2)
            {
                num += Convert.ToByte(str.Substring(i, 2), 0x10);
            }
            num = (num & 0x7fff) | 0x8000;
            return num.ToString("X2").PadLeft(4, '0');
        }

        internal QoSSetting getQoSSettings(string icdStr)
        {
            QoSSetting setting = new QoSSetting();
            ArrayList list = new ArrayList();
            list = this.SLC_GetMessageStructure(1, 0, "AI3", "2.2");
            for (int i = 0; i < 11; i++)
            {
                SLCMsgStructure structure = (SLCMsgStructure) list[i];
                switch (structure.fieldName)
                {
                    case "ICD_REV_NUM":
                        setting.ICDSTR = icdStr;
                        break;

                    case "POS_REQ_FLAG":
                        setting.PosReqType = structure.defaultValue;
                        break;

                    case "NUM_FIXES":
                        setting.NumFixes = structure.defaultValue;
                        break;

                    case "TIME_BTW_FIXES":
                        setting.TBFixes = structure.defaultValue;
                        break;

                    case "HORI_ERROR_MAX":
                        setting.Position2DError = structure.defaultValue;
                        break;

                    case "VERT_ERROR_MAX":
                        setting.Position3DError = structure.defaultValue;
                        break;

                    case "RESP_TIME_MAX":
                        setting.RespTMax = structure.defaultValue;
                        break;

                    case "TIME_ACC_PRIORITY":
                        setting.TAccPriority = structure.defaultValue;
                        break;

                    case "LOCATION_METHOD":
                        setting.LocMethod = structure.defaultValue;
                        break;

                    case "ALM_REQ_FLAG":
                        setting.AlmReqFlag = structure.defaultValue;
                        break;
                }
            }
            list.Clear();
            setting.IsInit = true;
            return setting;
        }

        public byte[] HexStringToByteArray(string Hex)
        {
            string str = "\0\x0001\x0002\x0003\x0004\x0005\x0006\a\b\t|||||||\n\v\f\r\x000e\x000f";
            int num = Hex.Length / 2;
            byte[] buffer = new byte[num];
            int index = 0;
            int num3 = 0;
            while (num3 < Hex.Length)
            {
                buffer[index] = (byte) (str[char.ToUpper(Hex[num3]) - 0x30] << 4);
                buffer[index] = (byte) (buffer[index] | ((byte) str[char.ToUpper(Hex[num3 + 1]) - '0']));
                num3 += 2;
                index++;
            }
            return buffer;
        }

        private string SLC_ConvertDecimalToHex(string decimalString, string dataType, double scale)
        {
            switch (dataType)
            {
                case "UINT8":
                {
                    uint num3 = (uint) Math.Round((decimal) (uint.Parse(decimalString) * Convert.ToDecimal(scale)));
                    return num3.ToString("X").PadLeft(2, '0');
                }
                case "UINT16":
                {
                    ushort num4 = (ushort) Math.Round((decimal) (ushort.Parse(decimalString) * Convert.ToDecimal(scale)));
                    return num4.ToString("X").PadLeft(4, '0');
                }
                case "UINT24":
                    return "000000";

                case "UINT32":
                {
                    uint num5 = (uint) Math.Round((decimal) (uint.Parse(decimalString) * Convert.ToDecimal(scale)));
                    return num5.ToString("X").PadLeft(8, '0');
                }
                case "UINT64":
                {
                    ulong num6 = (ulong) Math.Round((decimal) (ulong.Parse(decimalString) * Convert.ToDecimal(scale)));
                    return num6.ToString("X").PadLeft(0x10, '0');
                }
                case "SINT8":
                {
                    int num = (int) Math.Round((decimal) (int.Parse(decimalString) * Convert.ToDecimal(scale)));
                    return Convert.ToSByte(num).ToString("X").PadLeft(2, '0');
                }
                case "SINT16":
                {
                    short num8 = (short) Math.Round((decimal) (short.Parse(decimalString) * Convert.ToDecimal(scale)));
                    return num8.ToString("X").PadLeft(4, '0');
                }
                case "SINT24":
                    return "000000";

                case "SINT32":
                {
                    int num9 = (int) Math.Round((decimal) (int.Parse(decimalString) * Convert.ToDecimal(scale)));
                    return num9.ToString("X").PadLeft(8, '0');
                }
                case "SINT64":
                {
                    long num10 = (long) Math.Round((decimal) (long.Parse(decimalString) * Convert.ToDecimal(scale)));
                    return num10.ToString("X").PadLeft(0x10, '0');
                }
            }
            uint num11 = (uint) Math.Round((decimal) (uint.Parse(decimalString) * Convert.ToDecimal(scale)));
            return num11.ToString("X").PadLeft(2, '0');
        }

        public ArrayList SLC_GetMessageStructure(int mid, int sid, string protocol, string version)
        {
            ArrayList list = new ArrayList();
            SLCMsgStructure structure = new SLCMsgStructure();
            int num = 0;
            XPathExpression expr = this.m_XPathNav.Compile(string.Concat(new object[] { "/protocols/protocol[@name='", protocol, "'][@version='", version, "']/message[@mid='", mid, "'][@subid = '']/field" }));
            XPathNodeIterator iterator = this.m_XPathNav.Select(expr);
            if (iterator.Count == 0)
            {
                expr = this.m_XPathNav.Compile(string.Concat(new object[] { "/protocols/protocol[@name='", protocol, "'][@version='", version, "']/message[@mid='", mid, "'][@subid = '", sid, "']/field" }));
                iterator = this.m_XPathNav.Select(expr);
            }
            try
            {
                while (iterator.MoveNext())
                {
                    XPathNavigator navigator = iterator.Current.Clone();
                    structure.fieldNumber = iterator.CurrentPosition;
                    structure.fieldName = navigator.GetAttribute("name", "");
                    structure.bytes = int.Parse(navigator.GetAttribute("bytes", ""));
                    structure.datatype = navigator.GetAttribute("datatype", "");
                    structure.units = navigator.GetAttribute("units", "");
                    if (navigator.GetAttribute("scale", "") == "")
                    {
                        structure.scale = 1.0;
                    }
                    else
                    {
                        structure.scale = double.Parse(navigator.GetAttribute("scale", ""));
                    }
                    structure.startByte = (((this.m_MsgHeaderSize + this.m_MsgPayloadSize) + this.m_MsgChannelSize) + 1) + num;
                    structure.endByte = (structure.startByte + structure.bytes) - 1;
                    structure.defaultValue = navigator.GetAttribute("default", "");
                    num += structure.bytes;
                    list.Add(structure);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return list;
        }

        public string AI3_ICD
        {
            get
            {
                return this._ai3ICD;
            }
            set
            {
                this._ai3ICD = value;
            }
        }

        public string F_ICD
        {
            get
            {
                return this._fICD;
            }
            set
            {
                this._fICD = value;
            }
        }
    }
}

