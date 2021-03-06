﻿namespace SiRFLive.Reporting
{
    using System;

    public class TestSetup
    {
        private bool _aided;
        private double _atten = -9999.0;
        private bool _init;
        private string _rxCom = "N/A";
        private string _rxSN = "N/A";
        private string _svList = "N/A";
        private string _swVersion = "N/A";
        public string DataClass = "N/A";
        public string EndTime = "N/A";
        public double HrzErrorLimit;
        public QoSSetting qosParams = new QoSSetting();
        public string RXAntType = "N/A";
        public int RXLNAType = -9999;
        public string RXManufacture = "N/A";
        public string RXPackageType = "N/A";
        public string RXPlatformType = "EVK";
        public string RXProductType = "N/A";
        public int RXPwrMode = -9999;
        public uint RXRefFreq;
        public string RXRefFreqSrc = "N/A";
        public string RXRev = "N/A";
        public string RXTempUnit = "C";
        public double RXTempVal = 25.0;
        public string RXVoltage = "1.8";
        public double SignalLevel;
        public double SignalMargin = -1.0;
        public string SignalType = "dBm";
        public string StartTime = "N/A";
        public string TestDescription = "N/A";
        public string TestGroup = "N/A";
        public string TestID = "N/A";
        public string TestName = "N/A";
        public string TestOperator = "N/A";
        public string TestRun = "N/A";
        public double TTFFLimit;
        public double TTFFTimeout = 61.0;

        public double Atten
        {
            get
            {
                return this._atten;
            }
            set
            {
                this._atten = value;
            }
        }

        public bool IsAided
        {
            get
            {
                return this._aided;
            }
            set
            {
                this._aided = value;
            }
        }

        public bool isInit
        {
            get
            {
                return this._init;
            }
            set
            {
                this._init = value;
            }
        }

        public string RXCommIntf
        {
            get
            {
                return this._rxCom;
            }
            set
            {
                this._rxCom = value;
            }
        }

        public string RxSN
        {
            get
            {
                return this._rxSN;
            }
            set
            {
                this._rxSN = value;
            }
        }

        public string SVList
        {
            get
            {
                return this._svList;
            }
            set
            {
                this._svList = value;
            }
        }

        public string SWVersion
        {
            get
            {
                return this._swVersion;
            }
            set
            {
                this._swVersion = value;
            }
        }
    }
}

