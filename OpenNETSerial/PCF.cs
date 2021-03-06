﻿namespace OpenNETCF.IO.Serial
{
    using System;

    [Flags]
    internal enum PCF
    {
        PCF_16BITMODE = 0x200,
        PCF_DTRDSR = 1,
        PCF_INTTIMEOUTS = 0x80,
        PCF_PARITY_CHECK = 8,
        PCF_RLSD = 4,
        PCF_RTSCTS = 2,
        PCF_SETXCHAR = 0x20,
        PCF_SPECIALCHARS = 0x100,
        PCF_TOTALTIMEOUTS = 0x40,
        PCF_XONXOFF = 0x10
    }
}

