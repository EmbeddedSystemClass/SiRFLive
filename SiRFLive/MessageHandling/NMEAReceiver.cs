﻿namespace SiRFLive.MessageHandling
{
    using SiRFLive.Utilities;
    using System;
    using System.Collections;

    public class NMEAReceiver : Receiver
    {
        public NMEAReceiver()
        {
            base._messageProtocol = "NMEA";
        }

        public override string[] DecodeClockStatus(string message)
        {
            return new string[0];
        }

        public override string DecodeGeodeticNavigationDataToCSV(string message)
        {
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            double num4 = 0.0;
            double num5 = 0.0;
            Hashtable hashtable = base.m_Protocols.ConvertRawToHash_NMEAMsgs(message);
            if (hashtable.ContainsKey("Latitude"))
            {
                num = Convert.ToDouble((string) hashtable["Latitude"]) / 100.0;
            }
            if (hashtable.ContainsKey("Latitude N/S") && (((string) hashtable["Latitude N/S"]) == "S"))
            {
                num = -num;
            }
            if (hashtable.ContainsKey("Longitude"))
            {
                num2 = Convert.ToDouble((string) hashtable["Longitude"]) / 100.0;
            }
            if (hashtable.ContainsKey("Longitude E/W") && (((string) hashtable["Longitude E/W"]) == "W"))
            {
                num2 = -num2;
            }
            if (hashtable.ContainsKey("Altitude mean sea level(geoid)"))
            {
                num3 = Convert.ToDouble((string) hashtable["Altitude mean sea level(geoid)"]);
            }
            if (hashtable.ContainsKey("HDOP"))
            {
                num4 = Convert.ToDouble((string) hashtable["HDOP"]) / 5.0;
            }
            if (hashtable.ContainsKey("Num Svs in use"))
            {
                num5 = Convert.ToUInt32((string) hashtable["Num Svs in use"]);
            }
            return string.Format("41,{0:F6},{1:F6},{2:F6},{3:F3},{4}", new object[] { num, num2, num3, num4, num5 });
        }

        public override string DecodeNavLibMeasurementDataToCSV(string message)
        {
            return string.Empty;
        }

        public override string DecodeRawMeasuredNavigationDataToCSV(string message)
        {
            return string.Empty;
        }

        public override Hashtable DecodeRawMessageToHash(string protocol, string message)
        {
            return base.m_Protocols.ConvertRawToHash_NMEAMsgs(message);
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

        public override byte GetNavigationMode(string message)
        {
            byte num = 0;
            Hashtable hashtable = base.m_Protocols.ConvertRawToHash_NMEAMsgs(message);
            if (hashtable.ContainsKey("GPS Quality Indicator"))
            {
                num = Convert.ToByte((string) hashtable["GPS Quality Indicator"]);
            }
            return num;
        }

        public override string GetPerChanAvgCNo(string message)
        {
            string str = string.Empty;
            byte[] comByte = HelperFunctions.HexToByte(message);
            Hashtable hashtable = base.m_Protocols.ConvertRawToHash(comByte, "NMEA");
            int num = 10;
            double num2 = 0.0;
            int num3 = 0;
            for (int i = 1; i <= 12; i++)
            {
                int num5 = 0;
                for (int j = 1; j <= num; j++)
                {
                    string key = string.Format("SVID{0:D}.C/NO{1:D}", i, j);
                    if (hashtable.ContainsKey(key))
                    {
                        int num7 = Convert.ToInt32((string) hashtable[key]);
                        num5 += num7;
                    }
                }
                string str3 = string.Empty;
                double num8 = num5 / num;
                str3 = string.Format("{0:F1}", num8);
                if (num8 > 0.0)
                {
                    num3++;
                    num2 += num8;
                }
                if (str.Length > 0)
                {
                    str = str + "," + str3;
                }
                else
                {
                    str = str + str3;
                }
            }
            if (num3 <= 0)
            {
                num3 = 1;
            }
            string str4 = string.Format("{0:F1}", num2 / ((double) num3));
            base._rxNavData.AvgCNo = str + "," + str4;
            return str;
        }

        public override int GetPosition(string logTime, string message)
        {
            return 0;
        }

        public override void GetSWVersion(string message)
        {
            byte[] comByte = HelperFunctions.HexToByte(message);
            base._resetCtrl.ResetRxSwVersion = base.m_Protocols.ConvertRawToFields(comByte);
            base._commWindow.UpdateWindowTitleBar(base._commWindow.PortName + ": " + base._resetCtrl.ResetRxSwVersion);
        }

        public override string GetTTFF()
        {
            base._rxNavData.TTFFReset = -9999.0;
            base._rxNavData.TTFFAided = -9999.0;
            base._rxNavData.TTFFFirstNav = -9999.0;
            return string.Format("{0:F1},{1:F1},{2:F1},{3:F6},{4:F6},{5:F6},{6:F6}", new object[] { base._rxNavData.TTFFReset, base._rxNavData.TTFFAided, base._rxNavData.TTFFFirstNav, base._rxNavData.TimeUncer, base._rxNavData.FreqUncer, base._rxNavData.TimeErr, base._rxNavData.FreqErr });
        }

        public override byte IsNavigated(string message)
        {
            byte num = 0;
            Hashtable hashtable = base.m_Protocols.ConvertRawToHash_NMEAMsgs(message);
            if (hashtable.ContainsKey("GPS Quality Indicator"))
            {
                num = Convert.ToByte((string) hashtable["GPS Quality Indicator"]);
            }
            if (num > 0)
            {
                base._rxNavData.IsNav = true;
            }
            return num;
        }

        public static string NMEA_AddCheckSum(string inputString)
        {
            inputString.Replace(" ", "");
            char[] chArray = inputString.ToCharArray();
            byte num = 0;
            int num2 = 1;
            while (num2 < inputString.Length)
            {
                num = (byte) (num ^ Convert.ToByte(chArray[num2++]));
            }
            return (inputString + string.Format("*{0:X2}\r\n", num));
        }

        public override void PollNavigationParameters()
        {
            if (base._commWindow.FiveHzNavModePendingSet)
            {
                if (base._commWindow.FiveHzNavModeToSet)
                {
                    this.SetPosCalcMode(1);
                }
                else
                {
                    this.SetPosCalcMode(0);
                }
                base._commWindow.FiveHzNavModeToSet = false;
                base._commWindow.FiveHzNavModePendingSet = false;
            }
        }

        public override void PollSWVersion()
        {
        }

        public override void SetClockDrift(string message)
        {
            byte[] comByte = HelperFunctions.HexToByte(message);
            Hashtable hashtable = base.m_Protocols.ConvertRawToHash(comByte, "NMEA");
            if (hashtable.ContainsKey("Clock Drift"))
            {
                base._clkDrift = Convert.ToUInt32((string) hashtable["Clock Drift"]);
            }
            else
            {
                base._clkDrift = 0;
            }
        }

        public override void SetPosCalcMode(int state)
        {
            if (state == 1)
            {
                base._commWindow.WriteData("$PSRF103,00,6,00,0*23");
            }
            else
            {
                base._commWindow.WriteData("$PSRF103,00,7,00,0*22");
            }
        }
    }
}

