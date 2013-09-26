﻿namespace SiRFLive.Analysis
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct strucCoarsePositionSec
    {
        public int dwDeltaLat;
        public int dwDeltaLon;
        public int dwDeltaAlt;
        public byte prErrTh;
    }
}

