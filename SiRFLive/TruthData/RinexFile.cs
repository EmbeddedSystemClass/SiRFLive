﻿namespace SiRFLive.TruthData
{
    using System;
    using System.Collections;
    using System.IO;

    public class RinexFile
    {
        private string _filename;
        public int Day;
        public int Hour;
        private StreamReader m_streamreader;
        public int Minute;
        public int Month;
        private ArrayList rinexArray = new ArrayList();
        public int Second;
        public int UTCOffset = 15;
        public int Year;

        public int GetNumEphemerides()
        {
            return this.rinexArray.Count;
        }

        public void Read(string inFilename)
        {
            if (File.Exists(inFilename))
            {
                this.m_streamreader = File.OpenText(inFilename);
                for (bool flag = this.ReadHeader(); !flag; flag = this.ReadEphWriteToArrayList())
                {
                }
                this.m_streamreader.Close();
            }
        }

        public bool ReadEphWriteToArrayList()
        {
            try
            {
                RinexEph eph = new RinexEph();
                int startIndex = 0;
                int num2 = 0x16;
                int length = 0x13;
                int num4 = num2;
                int num5 = num4 + length;
                int num6 = num5 + length;
                string str = this.m_streamreader.ReadLine();
                if (str == null)
                {
                    return true;
                }
                eph.svid = byte.Parse(str.Substring(startIndex, 2).Trim());
                string str2 = str.Substring(2, 3).Trim();
                this.Year = Convert.ToInt32(str2);
                if (this.Year < 80)
                {
                    this.Year += 0x7d0;
                }
                else if ((this.Year >= 80) && (this.Year <= 0x63))
                {
                    this.Year += 0x76c;
                }
                else
                {
                    return false;
                }
                string str3 = str.Substring(5, 3).Trim();
                this.Month = Convert.ToInt32(str3);
                string str4 = str.Substring(8, 3).Trim();
                this.Day = Convert.ToInt32(str4);
                this.Hour = int.Parse(str.Substring(11, 3).Trim());
                this.Minute = int.Parse(str.Substring(14, 3).Trim());
                string str7 = str.Substring(0x11, 5);
                this.Second = (int) double.Parse(str7.Trim());
                eph.af0 = double.Parse(str.Substring(num2, length).ToString().Replace('D', 'e').Trim());
                eph.af1 = double.Parse(str.Substring(num5, length).ToString().Replace('D', 'e').Trim());
                eph.af2 = double.Parse(str.Substring(num6, length).ToString().Replace('D', 'e').Trim());
                string str11 = this.m_streamreader.ReadLine();
                eph.iode = double.Parse(str11.Substring(startIndex, num2).ToString().Replace('D', 'e').Trim());
                eph.Crs = double.Parse(str11.Substring(num4, length).ToString().Replace('D', 'e').Trim());
                string str14 = str11.Substring(num5, length).ToString().Replace('D', 'e');
                eph.deltan = double.Parse(str14.Trim()) / 3.1415926535897931;
                string str15 = str11.Substring(num6, length).ToString().Replace('D', 'e');
                eph.m0 = double.Parse(str15.Trim()) / 3.1415926535897931;
                string str16 = this.m_streamreader.ReadLine();
                eph.Cuc = double.Parse(str16.Substring(startIndex, num2).ToString().Replace('D', 'e').Trim());
                eph.ecc = double.Parse(str16.Substring(num4, length).ToString().Replace('D', 'e').Trim());
                eph.Cus = double.Parse(str16.Substring(num5, length).ToString().Replace('D', 'e').Trim());
                eph.sqrta = double.Parse(str16.Substring(num6, length).ToString().Replace('D', 'e').Trim());
                string str21 = this.m_streamreader.ReadLine();
                double num7 = double.Parse(str21.Substring(startIndex, num2).ToString().Replace('D', 'e').Trim());
                eph.toe = num7;
                eph.toc = eph.toe;
                eph.Cic = double.Parse(str21.Substring(num4, length).ToString().Replace('D', 'e').Trim());
                string str24 = str21.Substring(num5, length).ToString().Replace('D', 'e');
                eph.omega0 = double.Parse(str24.Trim()) / 3.1415926535897931;
                eph.Cis = double.Parse(str21.Substring(num6, length).ToString().Replace('D', 'e').Trim());
                string str26 = this.m_streamreader.ReadLine();
                string str27 = str26.Substring(startIndex, num2).ToString().Replace('D', 'e');
                eph.i0 = double.Parse(str27.Trim()) / 3.1415926535897931;
                eph.Crc = double.Parse(str26.Substring(num4, length).ToString().Replace('D', 'e').Trim());
                string str29 = str26.Substring(num5, length).ToString().Replace('D', 'e');
                eph.omega = double.Parse(str29.Trim()) / 3.1415926535897931;
                string str30 = str26.Substring(num6, length).ToString().Replace('D', 'e');
                eph.omegaDot = double.Parse(str30.Trim()) / 3.1415926535897931;
                string str31 = this.m_streamreader.ReadLine();
                string str32 = str31.Substring(startIndex, num2).ToString().Replace('D', 'e');
                eph.idot = double.Parse(str32.Trim()) / 3.1415926535897931;
                double num8 = double.Parse(str31.Substring(num5, length).ToString().Replace('D', 'e').Trim());
                eph.weekNo = num8;
                string str34 = this.m_streamreader.ReadLine();
                double num9 = double.Parse(str34.Substring(startIndex, num2).ToString().Replace('D', 'e').Trim());
                eph.accuracy = num9;
                if ((num9 >= 0.0) && (num9 <= 2.4))
                {
                    eph.ura_ind = 0;
                }
                else if ((num9 > 2.4) && (num9 <= 3.4))
                {
                    eph.ura_ind = 1;
                }
                else if ((num9 > 3.4) && (num9 <= 4.85))
                {
                    eph.ura_ind = 2;
                }
                else if ((num9 > 4.85) && (num9 <= 6.85))
                {
                    eph.ura_ind = 3;
                }
                else if ((num9 > 6.85) && (num9 <= 9.65))
                {
                    eph.ura_ind = 4;
                }
                else if ((num9 > 9.65) && (num9 <= 13.65))
                {
                    eph.ura_ind = 5;
                }
                else if ((num9 > 13.65) && (num9 <= 24.0))
                {
                    eph.ura_ind = 6;
                }
                else if ((num9 > 24.0) && (num9 <= 48.0))
                {
                    eph.accuracy = 7.0;
                }
                else if ((num9 > 48.0) && (num9 <= 96.0))
                {
                    eph.ura_ind = 8;
                }
                else if ((num9 > 96.0) && (num9 <= 192.0))
                {
                    eph.ura_ind = 9;
                }
                else if ((num9 > 192.0) && (num9 <= 384.0))
                {
                    eph.ura_ind = 10;
                }
                else if ((num9 > 384.0) && (num9 <= 768.0))
                {
                    eph.ura_ind = 11;
                }
                else if ((num9 > 768.0) && (num9 <= 1536.0))
                {
                    eph.ura_ind = 12;
                }
                else if ((num9 > 1536.0) && (num9 <= 3072.0))
                {
                    eph.ura_ind = 13;
                }
                else if ((num9 > 3072.0) && (num9 <= 6144.0))
                {
                    eph.ura_ind = 14;
                }
                else
                {
                    eph.ura_ind = 15;
                }
                if (((uint) double.Parse(str34.Substring(num4, length).ToString().Replace('D', 'e').Trim())) == 0)
                {
                    eph.status = 1;
                }
                else
                {
                    eph.status = 0;
                }
                eph.tgd = double.Parse(str34.Substring(num5, length).ToString().Replace('D', 'e').Trim());
                double num11 = double.Parse(str34.Substring(num6, length).ToString().Replace('D', 'e').Trim());
                eph.iodc = num11;
                double num12 = double.Parse(this.m_streamreader.ReadLine().Substring(num4, length).ToString().Replace('D', 'e').Trim());
                eph.fitint = (sbyte) num12;
                this.rinexArray.Add(eph);
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                return true;
            }
            return false;
        }

        public bool ReadHeader()
        {
            bool flag = false;
            this.m_streamreader.ReadLine();
            string str = null;
            while ((str = this.m_streamreader.ReadLine()) != null)
            {
                if (str.Contains("LEAP SECONDS"))
                {
                    string[] strArray = str.Trim().Split(new char[] { ' ' });
                    if (strArray.Length > 1)
                    {
                        this.UTCOffset = int.Parse(strArray[0]);
                    }
                }
                if (str.IndexOf("END OF HEADER") != -1)
                {
                    return flag;
                }
            }
            return flag;
        }

        public RinexEph SearchRinexArrayList(byte SVID, int TOW)
        {
            foreach (RinexEph eph in this.rinexArray)
            {
                if (((eph.svid == SVID) && (eph.toe <= TOW)) && (eph.toe >= (TOW - 0x1c20)))
                {
                    return eph;
                }
            }
            return null;
        }

        public string filename
        {
            get
            {
                return this._filename;
            }
            set
            {
                this._filename = value;
            }
        }
    }
}

