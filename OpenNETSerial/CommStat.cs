﻿namespace OpenNETCF.IO.Serial
{
    using System;
    using System.Collections.Specialized;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal class CommStat
    {
        private BitVector32 bitfield = new BitVector32(0);
        public uint cbInQue;
        public uint cbOutQue;
        public bool fCtsHold
        {
            get
            {
                return this.bitfield[1];
            }
            set
            {
                this.bitfield[1] = value;
            }
        }
        public bool fDsrHold
        {
            get
            {
                return this.bitfield[2];
            }
            set
            {
                this.bitfield[2] = value;
            }
        }
        public bool fRlsdHold
        {
            get
            {
                return this.bitfield[4];
            }
            set
            {
                this.bitfield[4] = value;
            }
        }
        public bool fXoffHold
        {
            get
            {
                return this.bitfield[8];
            }
            set
            {
                this.bitfield[8] = value;
            }
        }
        public bool fXoffSent
        {
            get
            {
                return this.bitfield[0x10];
            }
            set
            {
                this.bitfield[0x10] = value;
            }
        }
        public bool fEof
        {
            get
            {
                return this.bitfield[0x20];
            }
            set
            {
                this.bitfield[0x20] = value;
            }
        }
        public bool fTxim
        {
            get
            {
                return this.bitfield[0x40];
            }
            set
            {
                this.bitfield[0x40] = value;
            }
        }
        [Flags]
        private enum commFlags
        {
            fCtsHoldMask = 1,
            fDsrHoldMask = 2,
            fEofMask = 0x20,
            fRlsdHoldMask = 4,
            fTximMask = 0x40,
            fXoffHoldMask = 8,
            fXoffSentMask = 0x10
        }
    }
}

