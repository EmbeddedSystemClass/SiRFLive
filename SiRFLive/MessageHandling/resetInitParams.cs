﻿namespace SiRFLive.MessageHandling
{
    using System;

    public class resetInitParams
    {
        private string _channels = "12";
        private string _clockDrift = "75000";
        private string _ECEFX = "-2686727";
        private string _ECEFY = "-4304282";
        private string _ECEFZ = "3851642";
        private bool _enable_Development_Data = true;
        private bool _enable_Navlib_Data = true;
        private bool _enableEncryptedData = true;
        private bool _enableFullSystemReset = true;
        private bool _keepFlashData;
        private string _tow = "86400";
        private string _trueHeading = "0";
        private string _weekNumber = "1311";

        public string Channels
        {
            get
            {
                return this._channels;
            }
            set
            {
                this._channels = value;
            }
        }

        public string ClockDrift
        {
            get
            {
                return this._clockDrift;
            }
            set
            {
                this._clockDrift = value;
            }
        }

        public string ECEFX
        {
            get
            {
                return this._ECEFX;
            }
            set
            {
                this._ECEFX = value;
            }
        }

        public string ECEFY
        {
            get
            {
                return this._ECEFY;
            }
            set
            {
                this._ECEFY = value;
            }
        }

        public string ECEFZ
        {
            get
            {
                return this._ECEFZ;
            }
            set
            {
                this._ECEFZ = value;
            }
        }

        public bool Enable_Development_Data
        {
            get
            {
                return this._enable_Development_Data;
            }
            set
            {
                this._enable_Development_Data = value;
            }
        }

        public bool Enable_Navlib_Data
        {
            get
            {
                return this._enable_Navlib_Data;
            }
            set
            {
                this._enable_Navlib_Data = value;
            }
        }

        public bool EnableEncryptedData
        {
            get
            {
                return this._enableEncryptedData;
            }
            set
            {
                this._enableEncryptedData = value;
            }
        }

        public bool EnableFullSystemReset
        {
            get
            {
                return this._enableFullSystemReset;
            }
            set
            {
                this._enableFullSystemReset = value;
            }
        }

        public bool KeepFlashData
        {
            get
            {
                return this._keepFlashData;
            }
            set
            {
                this._keepFlashData = value;
            }
        }

        public string TOW
        {
            get
            {
                return this._tow;
            }
            set
            {
                this._tow = value;
            }
        }

        public string TrueHeading
        {
            get
            {
                return this._trueHeading;
            }
            set
            {
                this._trueHeading = value;
            }
        }

        public string WeekNumber
        {
            get
            {
                return this._weekNumber;
            }
            set
            {
                this._weekNumber = value;
            }
        }
    }
}

