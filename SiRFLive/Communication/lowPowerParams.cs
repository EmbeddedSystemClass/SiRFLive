﻿namespace SiRFLive.Communication
{
    using System;

    public class lowPowerParams
    {
        private byte _apmDutyCycle = 10;
        private byte _apmMaxHrzError;
        private uint _apmMaxOffTime;
        private uint _apmMaxSearchTime;
        private byte _apmMaxVrtError;
        private byte _apmNumFixes;
        private byte _apmPriority;
        private byte _apmTBF = 10;
        private byte _apmTimeAccPriority;
        private byte _mode;
        private byte _mpmControl = 4;
        private byte _mpmTimeout;
        private uint _ptfMaxOffTime = 30;
        private uint _ptfMaxSearchTime;
        private uint _ptfPeriod;
        private ushort _tpDutyCycle = 0x3e8;
        private uint _tpMaxOffTime = 30;
        private uint _tpMaxSearchTime = 120;
        private uint _tpOnTime = 200;
        private sbyte _tpUpdateRate = 1;

        public byte APMDutyCycle
        {
            get
            {
                return this._apmDutyCycle;
            }
            set
            {
                this._apmDutyCycle = value;
            }
        }

        public byte APMMaxHrzError
        {
            get
            {
                return this._apmMaxHrzError;
            }
            set
            {
                this._apmMaxHrzError = value;
            }
        }

        public uint APMMaxOffTime
        {
            get
            {
                return this._apmMaxOffTime;
            }
            set
            {
                this._apmMaxOffTime = value;
            }
        }

        public uint APMMaxSearchTime
        {
            get
            {
                return this._apmMaxSearchTime;
            }
            set
            {
                this._apmMaxSearchTime = value;
            }
        }

        public byte APMMaxVrtError
        {
            get
            {
                return this._apmMaxVrtError;
            }
            set
            {
                this._apmMaxVrtError = value;
            }
        }

        public byte APMNumFixes
        {
            get
            {
                return this._apmNumFixes;
            }
            set
            {
                this._apmNumFixes = value;
            }
        }

        public byte APMPriority
        {
            get
            {
                return this._apmPriority;
            }
            set
            {
                this._apmPriority = value;
            }
        }

        public byte APMTBF
        {
            get
            {
                return this._apmTBF;
            }
            set
            {
                this._apmTBF = value;
            }
        }

        public byte APMTimeAccPriority
        {
            get
            {
                return this._apmTimeAccPriority;
            }
            set
            {
                this._apmTimeAccPriority = value;
            }
        }

        public byte Mode
        {
            get
            {
                return this._mode;
            }
            set
            {
                if (value < 0)
                {
                    this._mode = 0;
                }
                else if (value > 4)
                {
                    this._mode = 4;
                }
                else
                {
                    this._mode = value;
                }
            }
        }

        public byte MPM_Control
        {
            get
            {
                return this._mpmControl;
            }
            set
            {
                this._mpmControl = value;
            }
        }

        public byte MPM_Timeout
        {
            get
            {
                return this._mpmTimeout;
            }
            set
            {
                this._mpmTimeout = value;
            }
        }

        public uint PTFMaxOffTime
        {
            get
            {
                return this._ptfMaxOffTime;
            }
            set
            {
                this._ptfMaxOffTime = value;
            }
        }

        public uint PTFMaxSearchTime
        {
            get
            {
                return this._ptfMaxSearchTime;
            }
            set
            {
                this._ptfMaxSearchTime = value;
            }
        }

        public uint PTFPeriod
        {
            get
            {
                return this._ptfPeriod;
            }
            set
            {
                this._ptfPeriod = value;
            }
        }

        public ushort TPDutyCycle
        {
            get
            {
                return this._tpDutyCycle;
            }
            set
            {
                this._tpDutyCycle = value;
            }
        }

        public uint TPMaxOffTime
        {
            get
            {
                return this._tpMaxOffTime;
            }
            set
            {
                this._tpMaxOffTime = value;
            }
        }

        public uint TPMaxSearchTime
        {
            get
            {
                return this._tpMaxSearchTime;
            }
            set
            {
                this._tpMaxSearchTime = value;
            }
        }

        public uint TPOnTime
        {
            get
            {
                return this._tpOnTime;
            }
            set
            {
                this._tpOnTime = value;
            }
        }

        public sbyte TPUpdateRate
        {
            get
            {
                return this._tpUpdateRate;
            }
            set
            {
                this._tpUpdateRate = value;
            }
        }
    }
}

