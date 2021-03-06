﻿namespace OpenNETCF.IO.Serial
{
    using System;

    public enum PST
    {
        PST_FAX = 0x21,
        PST_LAT = 0x101,
        PST_MODEM = 6,
        PST_NETWORK_BRIDGE = 0x100,
        PST_PARALLELPORT = 2,
        PST_RS232 = 1,
        PST_RS422 = 3,
        PST_RS423 = 4,
        PST_RS449 = 5,
        PST_SCANNER = 0x22,
        PST_TCPIP_TELNET = 0x102,
        PST_UNSPECIFIED = 0,
        PST_X25 = 0x103
    }
}

