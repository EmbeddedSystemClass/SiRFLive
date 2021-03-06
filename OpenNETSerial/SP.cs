﻿namespace OpenNETCF.IO.Serial
{
    using System;

    [Flags]
    internal enum SP
    {
        SP_BAUD = 2,
        SP_DATABITS = 4,
        SP_HANDSHAKING = 0x10,
        SP_PARITY = 1,
        SP_PARITY_CHECK = 0x20,
        SP_RLSD = 0x40,
        SP_STOPBITS = 8
    }
}

