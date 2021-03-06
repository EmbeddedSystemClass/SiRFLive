﻿namespace SiRFLive.MessageHandling
{
    using SiRFLive.General;
    using SiRFLive.Utilities;
    using System;
    using System.Collections;
	using System.Configuration;

    public class TrackingAnalysisData
    {
        private TrackingData[] _Curr_SV_Data = new TrackingData[0x21];
        private TrackingData[] _Prev_SV_Data = new TrackingData[0x21];
        protected MsgFactory m_Protocols = new MsgFactory(ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Protocols\Protocols.xml");
        private const int MAX_SVS = 0x21;

        public TrackingAnalysisData()
        {
            for (int i = 0; i < 0x21; i++)
            {
                this._Prev_SV_Data[i] = new TrackingData();
                this._Curr_SV_Data[i] = new TrackingData();
            }
        }

        public int NumSVSGainedBFState()
        {
            int num = 0;
            for (int i = 0; i < 0x21; i++)
            {
                if (((this._Prev_SV_Data[i] != null) && (this._Prev_SV_Data[i].state != 0xbf)) && ((this._Curr_SV_Data[i] != null) && (this._Curr_SV_Data[i].state == 0xbf)))
                {
                    num++;
                }
            }
            return num;
        }

        public int NumSVSLostBFState()
        {
            int num = 0;
            for (int i = 0; i < 0x21; i++)
            {
                if (((this._Prev_SV_Data[i] != null) && (this._Prev_SV_Data[i].state == 0xbf)) && ((this._Curr_SV_Data[i] != null) && (this._Curr_SV_Data[i].state != 0xbf)))
                {
                    num++;
                }
            }
            return num;
        }

        public int NumSVSWithBFState()
        {
            int num = 0;
            for (int i = 0; i < this._Curr_SV_Data.Length; i++)
            {
                if ((this._Curr_SV_Data[i] != null) && (this._Curr_SV_Data[i].state == 0xbf))
                {
                    num++;
                }
            }
            return num;
        }

        public void Update(string message4)
        {
            for (int i = 0; i < 0x21; i++)
            {
                this._Prev_SV_Data[i].state = this._Curr_SV_Data[i].state;
            }
            byte[] comByte = HelperFunctions.HexToByte(message4);
            Hashtable hashtable = this.m_Protocols.ConvertRawToHash(comByte, "SSB");
            for (int j = 1; j <= 12; j++)
            {
                string key = string.Format(clsGlobal.MyCulture, "SVID{0:D}", new object[] { j });
                string str2 = string.Format(clsGlobal.MyCulture, "State SVID{0:D}", new object[] { j });
                if (hashtable.ContainsKey(key) && hashtable.ContainsKey(str2))
                {
                    int index = Convert.ToInt32((string) hashtable[key]);
                    int num4 = Convert.ToInt32((string) hashtable[str2]);
                    this._Curr_SV_Data[index].state = num4;
                }
            }
        }
    }
}

