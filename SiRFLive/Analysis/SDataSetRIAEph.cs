﻿namespace SiRFLive.Analysis
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit, Pack=1)]
    public struct SDataSetRIAEph
    {
        [FieldOffset(20)]
        public uint A_SQRT;
        [FieldOffset(0x38)]
        public int AF0;
        [FieldOffset(0x36)]
        public short AF1;
        [FieldOffset(0x35)]
        public sbyte AF2;
        [FieldOffset(0x22)]
        public int AngleInclination;
        [FieldOffset(0x1a)]
        public short C_IC;
        [FieldOffset(0x20)]
        public short C_IS;
        [FieldOffset(0x26)]
        public short C_RC;
        [FieldOffset(4)]
        public short C_RS;
        [FieldOffset(12)]
        public short C_UC;
        [FieldOffset(0x12)]
        public short C_US;
        [FieldOffset(6)]
        public short DeltaN;
        [FieldOffset(14)]
        public uint Eccentricity;
        [FieldOffset(0)]
        public byte EphFlag;
        [FieldOffset(0x30)]
        public short IDOT;
        [FieldOffset(3)]
        public byte IODE;
        [FieldOffset(8)]
        public int M0;
        [FieldOffset(40)]
        public int Omega;
        [FieldOffset(0x1c)]
        public int Omega0;
        [FieldOffset(0x2c)]
        public int OmegaDOT;
        [FieldOffset(1)]
        public byte SVPRN;
        [FieldOffset(0x34)]
        public sbyte T_GD;
        [FieldOffset(50)]
        public ushort TOC;
        [FieldOffset(0x18)]
        public ushort TOE;
        [FieldOffset(2)]
        public byte URA_IND;
    }
}

