﻿namespace OpenNETCF.IO.Serial
{
    using System;
    using System.Collections.Specialized;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public class CommCapabilities
    {
        private ushort wPacketLength;
        private ushort wPacketVersion;
        public SEP dwServiceMask;
        private uint dwReserved1;
        [CLSCompliant(false)]
        public uint dwMaxTxQueue;
        [CLSCompliant(false)]
        public uint dwMaxRxQueue;
        public BAUD dwMaxBaud;
        public PST dwProvSubType;
        private BitVector32 dwProvCapabilities;
        private BitVector32 dwSettableParams;
        private BitVector32 dwSettableBaud;
        private BitVector32 dwSettableStopParityData;
        [CLSCompliant(false)]
        public uint dwCurrentTxQueue;
        [CLSCompliant(false)]
        public uint dwCurrentRxQueue;
        private CPS dwProvSpec1;
        private uint dwProvSpec2;
        private ushort wcProvChar;
        internal CommCapabilities()
        {
            this.wPacketLength = (ushort) Marshal.SizeOf(this);
            this.dwProvSpec1 = CPS.COMMPROP_INITIALIZED;
            this.dwProvCapabilities = new BitVector32(0);
            this.dwSettableParams = new BitVector32(0);
            this.dwSettableBaud = new BitVector32(0);
            this.dwSettableStopParityData = new BitVector32(0);
        }

        internal void _SuppressCompilerWarnings()
        {
			/*
            this.wPacketVersion = this.wPacketVersion;
            this.dwReserved1 = this.dwReserved1;
            this.dwProvSpec1 = this.dwProvSpec1;
            this.dwProvSpec2 = this.dwProvSpec2;
            this.wcProvChar = this.wcProvChar;
			*/
        }

        public bool Supports16BitMode
        {
            get
            {
                return this.dwProvCapabilities[0x200];
            }
        }
        public bool SupportsDtrDts
        {
            get
            {
                return this.dwProvCapabilities[1];
            }
        }
        public bool SupportsIntTimeouts
        {
            get
            {
                return this.dwProvCapabilities[0x80];
            }
        }
        public bool SupportsParityCheck
        {
            get
            {
                return this.dwProvCapabilities[8];
            }
        }
        public bool SupportsRlsd
        {
            get
            {
                return this.dwProvCapabilities[4];
            }
        }
        public bool SupportsRtsCts
        {
            get
            {
                return this.dwProvCapabilities[2];
            }
        }
        public bool SupportsSetXChar
        {
            get
            {
                return this.dwProvCapabilities[0x20];
            }
        }
        public bool SupportsSpecialChars
        {
            get
            {
                return this.dwProvCapabilities[0x100];
            }
        }
        public bool SupportsTotalTimeouts
        {
            get
            {
                return this.dwProvCapabilities[0x40];
            }
        }
        public bool SupportsXonXoff
        {
            get
            {
                return this.dwProvCapabilities[0x10];
            }
        }
        public bool SettableBaud
        {
            get
            {
                return this.dwSettableParams[2];
            }
        }
        public bool SettableDataBits
        {
            get
            {
                return this.dwSettableParams[4];
            }
        }
        public bool SettableHandShaking
        {
            get
            {
                return this.dwSettableParams[0x10];
            }
        }
        public bool SettableParity
        {
            get
            {
                return this.dwSettableParams[1];
            }
        }
        public bool SettableParityCheck
        {
            get
            {
                return this.dwSettableParams[0x20];
            }
        }
        public bool SettableRlsd
        {
            get
            {
                return this.dwSettableParams[0x40];
            }
        }
        public bool SettableStopBits
        {
            get
            {
                return this.dwSettableParams[8];
            }
        }
        public bool Supports5DataBits
        {
            get
            {
                return this.dwSettableStopParityData[1];
            }
        }
        public bool Supports6DataBits
        {
            get
            {
                return this.dwSettableStopParityData[2];
            }
        }
        public bool Supports7DataBits
        {
            get
            {
                return this.dwSettableStopParityData[4];
            }
        }
        public bool Supports8DataBits
        {
            get
            {
                return this.dwSettableStopParityData[8];
            }
        }
        public bool Supports16DataBits
        {
            get
            {
                return this.dwSettableStopParityData[0x10];
            }
        }
        public bool Supports16XDataBits
        {
            get
            {
                return this.dwSettableStopParityData[0x20];
            }
        }
        public bool SupportsParityEven
        {
            get
            {
                return this.dwSettableStopParityData[0x4000000];
            }
        }
        public bool SupportsParityMark
        {
            get
            {
                return this.dwSettableStopParityData[0x8000000];
            }
        }
        public bool SupportsParityNone
        {
            get
            {
                return this.dwSettableStopParityData[0x1000000];
            }
        }
        public bool SupportsParityOdd
        {
            get
            {
                return this.dwSettableStopParityData[0x2000000];
            }
        }
        public bool SupportsParitySpace
        {
            get
            {
                return this.dwSettableStopParityData[0x10000000];
            }
        }
        public bool SupportsStopBits10
        {
            get
            {
                return this.dwSettableStopParityData[0x10000];
            }
        }
        public bool SupportsStopBits15
        {
            get
            {
                return this.dwSettableStopParityData[0x20000];
            }
        }
        public bool SupportsStopBits20
        {
            get
            {
                return this.dwSettableStopParityData[0x40000];
            }
        }
        public bool HasBaud75
        {
            get
            {
                return this.dwSettableBaud[1];
            }
        }
        public bool HasBaud110
        {
            get
            {
                return this.dwSettableBaud[2];
            }
        }
        public bool HasBaud134_5
        {
            get
            {
                return this.dwSettableBaud[4];
            }
        }
        public bool HasBaud150
        {
            get
            {
                return this.dwSettableBaud[8];
            }
        }
        public bool HasBaud300
        {
            get
            {
                return this.dwSettableBaud[0x10];
            }
        }
        public bool HasBaud600
        {
            get
            {
                return this.dwSettableBaud[0x20];
            }
        }
        public bool HasBaud1200
        {
            get
            {
                return this.dwSettableBaud[0x40];
            }
        }
        public bool HasBaud2400
        {
            get
            {
                return this.dwSettableBaud[0x100];
            }
        }
        public bool HasBaud4800
        {
            get
            {
                return this.dwSettableBaud[0x200];
            }
        }
        public bool HasBaud7200
        {
            get
            {
                return this.dwSettableBaud[0x400];
            }
        }
        public bool HasBaud9600
        {
            get
            {
                return this.dwSettableBaud[0x800];
            }
        }
        public bool HasBaud14400
        {
            get
            {
                return this.dwSettableBaud[0x1000];
            }
        }
        public bool HasBaud19200
        {
            get
            {
                return this.dwSettableBaud[0x2000];
            }
        }
        public bool HasBaud38400
        {
            get
            {
                return this.dwSettableBaud[0x4000];
            }
        }
        public bool HasBaud56K
        {
            get
            {
                return this.dwSettableBaud[0x8000];
            }
        }
        public bool HasBaud128K
        {
            get
            {
                return this.dwSettableBaud[0x10000];
            }
        }
        public bool HasBaud115200
        {
            get
            {
                return this.dwSettableBaud[0x20000];
            }
        }
        public bool HasBaud57600
        {
            get
            {
                return this.dwSettableBaud[0x40000];
            }
        }
        public bool HasBaudUser
        {
            get
            {
                return this.dwSettableBaud[0x10000000];
            }
        }
    }
}

