﻿namespace OpenNETCF.IO.Serial
{
    using System;

    [Flags]
    public enum BAUD
    {
        BAUD_075 = 1,
        BAUD_110 = 2,
        BAUD_115200 = 0x20000,
        BAUD_1200 = 0x40,
        BAUD_128K = 0x10000,
        BAUD_134_5 = 4,
        BAUD_14400 = 0x1000,
        BAUD_150 = 8,
        BAUD_1800 = 0x80,
        BAUD_19200 = 0x2000,
        BAUD_2400 = 0x100,
        BAUD_300 = 0x10,
        BAUD_38400 = 0x4000,
        BAUD_4800 = 0x200,
        BAUD_56K = 0x8000,
        BAUD_57600 = 0x40000,
        BAUD_600 = 0x20,
        BAUD_7200 = 0x400,
        BAUD_9600 = 0x800,
        BAUD_USER = 0x10000000
    }
}

