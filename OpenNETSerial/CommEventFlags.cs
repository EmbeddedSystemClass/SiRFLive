﻿namespace OpenNETCF.IO.Serial
{
    using System;

    [Flags]
    public enum CommEventFlags
    {
        ALLCE = 0x3fff,
        ALLCE_2 = 0x1fb,
        ALLPC = 0xf9,
        BREAK = 0x40,
        CTS = 8,
        DSR = 0x10,
        ERR = 0x80,
        EVENT1 = 0x800,
        EVENT2 = 0x1000,
        NONE = 0,
        PERR = 0x200,
        POWER = 0x2000,
        RING = 0x100,
        RLSD = 0x20,
        RX80FULL = 0x400,
        RXCHAR = 1,
        RXFLAG = 2,
        TXEMPTY = 4
    }
}

